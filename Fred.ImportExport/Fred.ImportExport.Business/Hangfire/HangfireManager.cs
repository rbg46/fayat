using Hangfire;
using System;

namespace Fred.ImportExport.Business.Hangfire
{
  public class HangfireManager
  {
    public HangfireManager()
    {

    }

    public string Enqueue(Action action)
    {
      return BackgroundJob.Enqueue(() => action());
    }

  }
}
