namespace Shared.Model
{
  public class SwaggerConfigModel
  {
    public string Version { get; set; }
    public string XmlPath { get; set; }
    public SwaggerOpenApiInfoModel OpenApiInfo { get; set; }
    public SwaggerUIOptionsModel SwaggerUIOptions { get; set; }
  }

  public class SwaggerOpenApiInfoModel
  {
    public string Title { get; set; }
    public string Description { get; set; }
    //public string TermsOfService { get; set; }
  }

  public class SwaggerUIOptionsModel
  {
    public string Url { get; set; }
    public string Name { get; set; }
  }
}