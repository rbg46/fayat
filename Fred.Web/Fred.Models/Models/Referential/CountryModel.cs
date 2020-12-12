using Fred.Web.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fred.Web.Models
{
  public class CountryModel
  {
    public int CountryId { get; set; }

    [Required]
    [Display(Name = "LabelNameFr", ResourceType =typeof(FredResource))]
    public string NameFr { get; set; }

    [Required]
    [Display(Name = "LabelNameGb", ResourceType = typeof(FredResource))]
    public string NameGb { get; set; }

    [Required]
    [Display(Name = "LabelCodeIso", ResourceType = typeof(FredResource))]
    public string CodeIso { get; set; }
  }
}