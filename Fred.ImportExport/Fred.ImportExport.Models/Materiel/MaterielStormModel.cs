using System;

namespace Fred.ImportExport.Models.Materiel
{
  /// <summary>
  /// Représente un model pour le materiel STORM
  /// </summary>
  public class MaterielStormModel
  {
    /// <summary>
    /// Obtient ou définit le code du matériel
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si Actif
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création
    /// </summary>
    public DateTime DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit la date de debut de location 
    /// </summary>
    public DateTime? DateDebutLocation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de fin de location
    /// </summary>
    public DateTime? DateFinLocation { get; set; }

    /// <summary>
    /// Obtient ou définit si le materiel est de type location.
    /// </summary>
    public bool MaterielLocation { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la classe Equipement STORM
    /// </summary>
    public string ClasseFamilleCode { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé de la classe Equipement STORM
    /// </summary>
    public string ClasseFamilleLibelle { get; set; }

    /// <summary>
    /// Obtient ou définit si le materiel est STORM.
    /// </summary>
    public bool IsStorm { get; set; }

    /// <summary>
    /// Obtient ou définit le partenaire fabriquant machine 
    /// </summary>
    public string Fabriquant { get; set; }

    /// <summary>
    /// Obtient ou définit l'identification véhicule 
    /// </summary>
    public string VIN { get; set; }

    /// <summary>
    /// Obtient ou définit la date mise en service  
    /// </summary>
    public string DateMiseEnService { get; set; }

    /// <summary>
    /// Obtient ou définit l'immatriculation machine 
    /// </summary>
    public string Immatriculation { get; set; }

    /// <summary>
    /// Obtient ou définit la hauteur machine 
    /// </summary>
    public decimal DimensionH { get; set; }

    /// <summary>
    /// Obtient ou définit largeur machine 
    /// </summary>
    public decimal DimensionL { get; set; }

    /// <summary>
    /// Obtient ou définit longueur machine  
    /// </summary>
    public decimal Dimensiionl { get; set; }

    /// <summary>
    /// Obtient ou définit la puissance machine 
    /// </summary>
    public decimal Puissance { get; set; }

    /// <summary>
    /// Obtient ou définit l'unité de la puissance
    /// </summary>
    public string UnitePuissance { get; set; }

    /// <summary>
    /// Obtient ou définit l'unité de la dimention
    /// </summary>
    public string UniteDimension { get; set; }

    #region ForeignKey

    /// <summary>
    /// Obtient ou définit le code de la société.
    /// CommandeEnt.SocieteDataId => SocieteEnt.Code
    /// </summary>
    public string SocieteCode { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une ressource.
    /// CommandeLigneEnt.RessourceId => RessourceEnt.Code
    /// </summary>
    public string RessourceCode { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un fournisseur.
    /// CommandeEnt.FournisseurId => FournisseurEnt.Code
    /// </summary>
    public string FournisseurCode { get; set; }

    #endregion ForeignKey
  }
}
