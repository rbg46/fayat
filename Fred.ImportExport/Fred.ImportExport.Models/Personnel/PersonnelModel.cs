﻿using System;

namespace Fred.ImportExport.Models.Personnel
{
  /// <summary>
  /// Représente un model pour le Personnel
  /// </summary>
  public class PersonnelModel
  {
    /// <summary>
    /// Obtient ou définit le code de la société Paye
    /// </summary>
    public string CodeSocietePaye { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la société Compta
    /// </summary>
    public string CodeSocieteCompta { get; set; }

    /// <summary>
    /// Obtient ou définit le code de l'établissement.
    /// </summary>
    public string CodeEtablissement { get; set; }

    /// <summary>
    /// Obtient ou définit le matricule.
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    /// Obtient ou définit le nom.
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Obtient ou définit le prénom.
    /// </summary>
    public string Prenom { get; set; }

    /// <summary>
    /// Obtient ou définit la catégorie interne.
    /// </summary>    
    public string CategoriePerso { get; set; }

    /// <summary>
    /// Obtient ou définit le statut.
    /// </summary>    
    public string Statut { get; set; }

    /// <summary>
    /// Obtient ou définit le code emploi.
    /// </summary>    
    public string CodeEmploi { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'entrée du personnel.
    /// </summary>
    public DateTime? DateEntree { get; set; }

    /// <summary>
    /// Obtient ou définit la date de sortie du personnel.
    /// </summary>
    public DateTime? DateSortie { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de rue.
    /// </summary>
    public string NumeroRue { get; set; }

    /// <summary>
    /// Obtient ou définit le détail du numéro de rue.
    /// </summary>
    public string NumeroRueDetail { get; set; }

    /// <summary>
    /// Obtient ou définit le type de rue.
    /// </summary>
    public string TypeRue { get; set; }

    /// <summary>
    /// Obtient ou définit le nom de rue.
    /// </summary>
    public string NomRue { get; set; }

    /// <summary>
    /// Obtient ou définit le lieu-dit.
    /// </summary>
    public string NomLieuDit { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal du personnel.
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit la ville du personnel.
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modification.
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit le code établissement de paye du personnel.
    /// </summary>
    public int? EtablissementPaieId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id ressource du personnel.
    /// </summary>
    public int? RessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'email
    /// </summary>
    public string Email { get; set; }
  }
}
