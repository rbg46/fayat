﻿using System;

namespace Fred.ImportExport.Models.Materiel
{
  /// <summary>
  /// Représente un model pour le materiel FRED
  /// </summary>
  public class MaterielFredModel
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

    /// <summary>
    /// Obtient ou définit le site propriétaire du matériel.
    /// </summary>
    public string SiteAppartenance { get; set; }

    /// <summary>
    /// Obtient ou définit le le site où a été restitué le matériel.
    /// </summary>
    public string SiteRestitution { get; set; }

    #region ForeignKey

    /// <summary>
    /// Obtient ou définit l'Id de la société
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la RessourceId
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du Fournisseur.
    /// </summary>
    public int? FournisseurId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'établissement comptable.
    /// </summary>
    public int? EtablissementComptableId { get; set; }

    #endregion ForeignKey
  }
}