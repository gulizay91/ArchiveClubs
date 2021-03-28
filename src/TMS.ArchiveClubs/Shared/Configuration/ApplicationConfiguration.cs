namespace Shared.Configuration
{
  using Microsoft.Extensions.Configuration;
  using System;
  using System.Globalization;
  using System.IO;

  /// <summary>
  /// Defines the <see cref="ApplicationConfiguration" />.
  /// </summary>
  public sealed class ApplicationConfiguration
  {
    #region Constants

    /// <summary>
    /// Defines the configFileName.
    /// </summary>
    private const string configFileName = projectName + "_configuration.json";

    /// <summary>
    /// Defines the configFilePathEnvironmentVariable.
    /// </summary>
    private const string configFilePathEnvironmentVariable = "tms_configuration_path";

    /// <summary>
    /// Defines the projectName.
    /// </summary>
    private const string projectName = "archiveclubs";

    #endregion

    #region Fields

    /// <summary>
    /// Defines the Lazy.
    /// </summary>
    private static readonly Lazy<ApplicationConfiguration> Lazy = new Lazy<ApplicationConfiguration>(() => new ApplicationConfiguration());

    /// <summary>
    /// Defines the _configPath.
    /// </summary>
    private readonly string _configPath;

    /// <summary>
    /// Defines the secretKey.
    /// </summary>
    public static string secretKey;

    #endregion

    #region Constructors

    /// <summary>
    /// Prevents a default instance of the <see cref="ApplicationConfiguration"/> class from being created.
    /// </summary>
    private ApplicationConfiguration()
    {
      _configPath = GetConfigurationPath();

      var builder = new ConfigurationBuilder()
          .AddJsonFile(Path.Combine(_configPath, configFileName), optional: false, reloadOnChange: false)
          .AddJsonFile(string.Format("{0}\\{1}", Environment.CurrentDirectory, configFileName), true, false);

      Configuration = builder.Build();
      secretKey = GetValue<string>("IdentityServer:JwtOptions:Security", "secret");
      Console.WriteLine("builded config.json");
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the Instance.
    /// </summary>
    public static ApplicationConfiguration Instance => Lazy.Value;

    /// <summary>
    /// Gets the ConfigurationPath.
    /// </summary>
    public string ConfigurationPath => Path.Combine(_configPath, configFileName);

    /// <summary>
    /// Gets the Configuration.
    /// </summary>
    private IConfiguration Configuration { get; }

    #endregion

    #region Methods

    /// <summary>
    /// The GetSection.
    /// </summary>
    /// <param name="configurationKey">The configurationKey<see cref="string"/>.</param>
    /// <returns>The <see cref="IConfigurationSection"/>.</returns>
    public IConfigurationSection GetSection(string configurationKey)
    {
      return Configuration.GetSection(configurationKey);
    }

    /// <summary>
    /// The GetValue.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="configurationKey">The configurationKey<see cref="string"/>.</param>
    /// <returns>The <see cref="T"/>.</returns>
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

    /// <summary>
    /// The GetValue.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="configurationKey">The configurationKey<see cref="string"/>.</param>
    /// <param name="defaultValue">The defaultValue<see cref="object"/>.</param>
    /// <returns>The <see cref="T"/>.</returns>
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

    /// <summary>
    /// The GetConfigurationPath.
    /// </summary>
    /// <returns>The <see cref="string"/>.</returns>
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

    #endregion
  }
}
