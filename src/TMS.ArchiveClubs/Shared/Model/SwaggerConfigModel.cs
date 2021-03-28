namespace Shared.Model
{
  /// <summary>
  /// Defines the <see cref="SwaggerConfigModel" />.
  /// </summary>
  public class SwaggerConfigModel
  {
    #region Properties

    /// <summary>
    /// Gets or sets the ApiName.
    /// </summary>
    public string ApiName { get; set; }

    /// <summary>
    /// Gets or sets the OpenApiInfo.
    /// </summary>
    public SwaggerOpenApiInfoModel OpenApiInfo { get; set; }

    /// <summary>
    /// Gets or sets the XmlPath.
    /// </summary>
    public string XmlPath { get; set; }

    #endregion
  }

  /// <summary>
  /// Defines the <see cref="SwaggerOpenApiInfoModel" />.
  /// </summary>
  public class SwaggerOpenApiInfoModel
  {
    #region Properties

    /// <summary>
    /// Gets or sets the Description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the Title.
    /// </summary>
    public string Title { get; set; }

    #endregion
  }

  /// <summary>
  /// Defines the <see cref="SwaggerUIOptionsModel" />.
  /// </summary>
  public class SwaggerUIOptionsModel
  {
    #region Properties

    /// <summary>
    /// Gets or sets the Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Url.
    /// </summary>
    public string Url { get; set; }

    #endregion
  }
}
