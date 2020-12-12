using System.Diagnostics;

namespace Fred.ImportExport.Business.ApplicationSap
{
  [DebuggerDisplay("SocieteId={SocieteId} Url={ApplicationSapParameter.Url}")]
  public class ApplicationSapRequestParameter<T>
  {
    public ApplicationSapRequestParameter(int id, ApplicationSapParameter applicationSapParameter, T data)
    {
      SocieteId = id;
      ApplicationSapParameter = applicationSapParameter;
      Data = data;
    }

    public int SocieteId { get; set; }

    public ApplicationSapParameter ApplicationSapParameter { get; set; }

    public T Data { get; set; }
  }
}
