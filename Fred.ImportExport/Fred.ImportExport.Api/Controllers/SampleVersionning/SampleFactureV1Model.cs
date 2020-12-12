#pragma warning disable SA1600 // Elements must be documented : SAMPLE CODE
#pragma warning disable CS1591 // Elements must be documented : SAMPLE CODE
#pragma warning disable S125 // Sections of code should not be "commented out" : SAMPLE CODE


using System.ComponentModel.DataAnnotations;

namespace Fred.ImportExport.Api.Controllers.SampleVersionning
{
  /// <summary>
  /// Represents a sample facture.
  /// </summary>
  public class SampleFactureV1Model
  {


    public int Id { get; set; }


    [Required]
    [StringLength(25)]
    public string FactureCode { get; set; }


    [Required]
    [StringLength(25)]
    public string FactureData { get; set; }
  }
}



#pragma warning restore SA1600 // Elements must be documented : SAMPLE CODE
#pragma warning restore CS1591 // Elements must be documented : SAMPLE CODE
#pragma warning restore S125 // Sections of code should not be "commented out" : SAMPLE CODE