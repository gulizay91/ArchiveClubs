using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BookArchive.API.Configuration
{
  public class ApiConfiguration
  {
    private static readonly Lazy<ApiConfiguration> Lazy = new Lazy<ApiConfiguration>(() => new ApiConfiguration());
    public static ApiConfiguration Instance => Lazy.Value;

    private readonly SwaggerConfigModel _swaggerConfigModelInstance;
    private readonly ApiVersionConfigModel _apiVersionConfigModelInstance;
    public SwaggerConfigModel SwaggerConfigModelInstance => _swaggerConfigModelInstance;
    public ApiVersionConfigModel ApiVersionConfigModelInstance => _apiVersionConfigModelInstance;

    private ApiConfiguration()
    {
      // get configuration for this api
      _apiVersionConfigModelInstance = GetApiVersionConfigModel();
      _swaggerConfigModelInstance = GetSwaggerConfigModel();
    }

    private SwaggerConfigModel GetSwaggerConfigModel()
    {
      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
      //List<SwaggerOpenApiInfoModel> apiInfos = new List<SwaggerOpenApiInfoModel>();
      //foreach (var version in _apiVersionConfigModelInstance.ApiVersionList)
      //{
      //  apiInfos.Add(new SwaggerOpenApiInfoModel()
      //  {
      //    Title = "BookArchive API",
      //    Description = "Book Archive API for ArchiveClubs",
      //    Version = version.Item1.ToString() + "." + version.Item2.ToString()
      //  });
      //}
      return new SwaggerConfigModel()
      {
        ApiName = "BookAPI",
        XmlPath = xmlPath,
        OpenApiInfo = new SwaggerOpenApiInfoModel()
        {
          Title = "BookArchive API",
          Description = "Book Archive API for ArchiveClubs"
        },
        //SwaggerUIOptions = new SwaggerUIOptionsModel()
        //{
        //  Url = "/swagger/v1/swagger.json",
        //  Name = "ArchiveClubs BookArchive API V1.0"
        //}
      };
    }

    private ApiVersionConfigModel GetApiVersionConfigModel()
    {
      return new ApiVersionConfigModel()
      {
        DefaultApiVersionMajor = 1,
        DefaultApiVersionMinor = 0,
        GetApiVersionFromHeader = false,
        ReportApiVersion = true,
        ApiVersionList = new List<(int, int)>() { (1, 0), (2, 0) }
      };
    }
  }
}