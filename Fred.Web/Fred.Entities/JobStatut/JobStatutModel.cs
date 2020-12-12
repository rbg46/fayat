namespace Fred.Entities.JobStatut
{
  /// <summary>
  /// classe qu encapsule le status d'un job
  /// </summary>
  public class JobStatutModel
  {
    /// <summary>
    /// Determine si le job est en queue
    /// </summary>
    public bool IsEnqueued { get; set; }

    /// <summary>
    /// Determine si le job est en cours
    /// </summary>
    public bool IsRunning { get; set; }
 
  }
}
