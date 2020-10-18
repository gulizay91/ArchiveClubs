using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Globalization;

namespace Shared.Configuration
{
    public sealed class ApplicationConfiguration
    {
        private static readonly Lazy<ApplicationConfiguration> Lazy = new Lazy<ApplicationConfiguration>(() => new ApplicationConfiguration());
        public static ApplicationConfiguration Instance => Lazy.Value;
        private IConfiguration Configuration { get; }

        private const string projectName = "archiveclubs";
        private const string configFileName = projectName + "_configuration.json";
        private const string configFilePathEnvironmentVariable = "tms_configuration_path";

        private readonly string _configPath;
        public string ConfigurationPath => Path.Combine(_configPath, configFileName);

        private ApplicationConfiguration()
        {
            _configPath = GetConfigurationPath();

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(_configPath, configFileName), optional: false, reloadOnChange: false)
                .AddJsonFile(string.Format("{0}\\archiveclubs_configuration.json", Environment.CurrentDirectory), true, false);

            Configuration = builder.Build();
            Console.WriteLine("builded config.json");
        }

        public T GetValue<T>(string configurationKey) where T : IConvertible
        {
            T result;
            try
            {
                if (Configuration[configurationKey] == null)
                    throw new ArgumentNullException($"Configuration Value couldn't find for the given key: {configurationKey} . Configuration Location: {_configPath}");

                if (typeof(T).IsEnum)
                {
                    result = (T)Enum.Parse(typeof(T), Configuration[configurationKey]);
                }
                else
                {
                    result = (T)Convert.ChangeType(Configuration[configurationKey], typeof(T));
                }

                if (string.IsNullOrEmpty(Convert.ToString(result, CultureInfo.InvariantCulture)))
                    throw new ArgumentNullException(nameof(configurationKey));
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException("Couldn't convert");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"GetValue error: {ex.Message}", ex);
            }
            return result;
        }

        public T GetValue<T>(string configurationKey, object defaultValue) where T : IConvertible
        {
            if (Configuration[configurationKey] == null)
            {
                return (T)Convert.ChangeType(defaultValue, typeof(T));
            }
            else
            {
                return GetValue<T>(configurationKey);
            }
        }

        public IConfigurationSection GetSection(string configurationKey)
        {
            return Configuration.GetSection(configurationKey);
        }

        private string GetConfigurationPath()
        {
            // expecting the path of the configuration file in the Machine level environment variable 
            string path = Environment.GetEnvironmentVariable(configFilePathEnvironmentVariable, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrEmpty(path))
            {
                // trying the_moon_studio folder on the root for each logical drive on the computer and take the first path if config file has been found within 
                foreach (var drive in Environment.GetLogicalDrives())
                {
                    var _tmpPath = Path.Combine(drive, "the_moon_studio");

                    if (File.Exists(Path.Combine(_tmpPath, configFileName)))
                    {
                        path = _tmpPath;
                        break;
                    }
                }
            }

            Console.WriteLine("your config path: " + path);

            return path;
        }
    }
}
