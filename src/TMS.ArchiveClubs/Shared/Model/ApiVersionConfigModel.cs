namespace Shared.Model
{
  using System.Collections.Generic;

  /// <summary>
  /// Defines the <see cref="ApiVersionConfigModel" />.
  /// </summary>
  public class ApiVersionConfigModel
  {
    #region Properties

    /// <summary>
    /// Gets or sets the ApiVersionList.
    /// </summary>
    public List<(int, int)> ApiVersionList { get; set; }

    /// <summary>
    /// Gets or sets the DefaultApiVersionMajor.
    /// </summary>
    public int DefaultApiVersionMajor { get; set; }

    /// <summary>
    /// Gets or sets the DefaultApiVersionMinor.
    /// </summary>
    public int DefaultApiVersionMinor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether GetApiVersionFromHeader.
    /// </summary>
    public bool GetApiVersionFromHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether ReportApiVersion.
    /// </summary>
    public bool ReportApiVersion { get; set; }

    #endregion
  }
}
