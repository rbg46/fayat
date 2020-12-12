using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Entities.Common
{
  /// <summary>
  /// Represente un élémént facturable
  /// </summary>
  public interface IFacturable
  {
    /// <summary>
    ///   Obtient ou définit le libellé.
    /// </summary>  
    string Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit le commentaire.
    /// </summary>  
    string Commentaire { get; set; }

    /// <summary>
    ///   Obtient ou définit l'Id du ReferentielEtendu.
    /// </summary>   
    int ReferentielEtenduId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'objet ReferentielEtendu
    /// </summary>
    ReferentielEtenduEnt ReferentielEtendu { get; set; }

    /// <summary>
    ///   Obtient ou définit l'Id de la tâche.
    /// </summary>   
    int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'objet tache
    /// </summary>    
    TacheEnt Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit le prix unitaire.
    /// </summary>  
    decimal PUHT { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'unité .
    /// </summary>   
    int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'unité.
    /// </summary>    
    UniteEnt Unite { get; set; }

    /// <summary>
    ///   Obtient ou définit la quantité.
    /// </summary>   
    decimal Quantite { get; set; }

    /// <summary>
    ///   Obtient le montant HT de la dépense
    /// </summary>    
    decimal Montant { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique de l'affaire.
    /// </summary>   
    CIEnt CI { get; set; }

    /// <summary>
    ///   Obtient ou définit l'affaire.
    /// </summary>  
    int CiId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la devise.
    /// </summary>   
    int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit la devise.
    /// </summary>  
    DeviseEnt Devise { get; set; }

  }
}
