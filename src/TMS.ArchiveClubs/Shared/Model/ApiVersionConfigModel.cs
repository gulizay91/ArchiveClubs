using System.Collections.Generic;

namespace Shared.Model
{
  public class ApiVersionConfigModel
  {
    public int DefaultApiVersionMajor { get; set; }
    public int DefaultApiVersionMinor { get; set; } 
    public bool GetApiVersionFromHeader { get; set; }
    public bool ReportApiVersion { get; set; }
    public List<(int, int)> ApiVersionList { get; set; }
  }
}
