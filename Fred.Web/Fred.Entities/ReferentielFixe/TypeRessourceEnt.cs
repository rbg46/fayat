using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.ReferentielFixe
{
  /// <summary>
  ///   Représente une association d'une organisation et d'un groupe
  /// </summary>
  public class TypeRessourceEnt
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un Groupe.
    /// </summary>
    public int TypeRessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit le code d'une OrgaGroupe.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le libellé d'un Groupe.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    ///   Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle => Code + " - " + Libelle;


    ///////////////////////////////////////////////////////////////////////////
    // AJOUT LORS DE LE MIGRATION CODE FIRST 
    ///////////////////////////////////////////////////////////////////////////


    // Reverse navigation

    /// <summary>
    /// Child Ressources where [FRED_RESSOURCE].[TypeRessourceId] point to this entity (FK_FRED_RESSOURCE_TYPE_RESSOURCE)
    /// </summary>
    public virtual System.Collections.Generic.ICollection<RessourceEnt> Ressources { get; set; } // FRED_RESSOURCE.FK_FRED_RESSOURCE_TYPE_RESSOURCE

  }
}