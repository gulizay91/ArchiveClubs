using Shared.Model;
using System;
using System.IO;
using System.Reflection;

namespace BookArchive.API.Configuration
{
  public class ApiConfiguration
  {
    private static readonly Lazy<ApiConfiguration> Lazy = new Lazy<ApiConfiguration>(() => new ApiConfiguration());
    public static ApiConfiguration Instance => Lazy.Value;

    private readonly SwaggerConfigModel _swaggerConfigModelInstance;
    public SwaggerConfigModel SwaggerConfigModelInstance => _swaggerConfigModelInstance;

    private ApiConfiguration()
    {
      // get configuration for this api
      _swaggerConfigModelInstance = GetSwaggerConfigModel();
    }

    private SwaggerConfigModel GetSwaggerConfigModel()
    {
      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
      return new SwaggerConfigModel()
      {
        Version = "v1",
        XmlPath = xmlPath,
        OpenApiInfo = new SwaggerOpenApiInfoModel()
        {
          Title = "BookArchive API",
          Description = "Book Archive API for ArchiveClubs"
        },
        SwaggerUIOptions = new SwaggerUIOptionsModel()
        {
          Url = "/swagger/v1/swagger.json",
          Name = "ArchiveClubs BookArchive API V1.0"
        }
      };
    }
  }
}