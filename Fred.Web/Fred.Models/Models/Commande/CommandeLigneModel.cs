using Fred.Web.Models.Depense;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Commande
{
  /// <summary>
  /// Représente une ligne de commande
  /// </summary>
  public class CommandeLigneModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ligne de commande.
    /// </summary>
    public int CommandeLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant temporaire unique d'une commande.
    /// Cette propriété est utilisé pour ajouter des lignes de commande dans l'interface.
    /// </summary>
    public int CommandeLigneIdTemp { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une ligne de commande.
    /// </summary>
    public int CommandeId { get; set; }

    /// <summary>
    /// Obtient ou définit la commande dont dépend une ligne de commande.
    /// </summary>
    public CommandeModel Commande { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la tâche d'une ligne de commande.
    /// </summary>
    public int? TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'objet tâche d'une ligne de commande.
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la ressource d'une ligne de commande.
    /// </summary>
    public int? RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'objet ressource d'une ligne de commande.
    /// </summary>
    public RessourceModel Ressource { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des réceptions d'une ligne de commande
    /// </summary>
    public DepenseModel[] Receptions { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une ligne de commande.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le montant d'une ligne de commande.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le prix unitaire d'une ligne de commande.
    /// Formatage à 2 décimales fait dans le template :
    /// Basculer les codes de champs --> \m \# "# ##0,00"
    /// </summary>
    public decimal PUHT { get; set; }

    /// <summary>
    /// Obtient ou définit l'unité d'une ligne de commande.
    /// </summary>
    public string Unite { get; set; }

    /// <summary>
    /// Obtient ou définit le montant d'une ligne de commande.
    /// Formatage à 2 décimales fait dans le template :
    /// Basculer les codes de champs --> \m \# "# ##0,00"
    /// </summary>
    public decimal MontantHT { get; set; }

    /// <summary>
    /// Obtient le montant total réceptionné d'une ligne de commande
    /// </summary>
    public decimal MontantHTReceptionne { get; set; }

    /// <summary>
    /// Obtient la quantité réceptionnée de la ligne de commande
    /// </summary>
    public decimal QuantiteReceptionnee { get; set; }

    /// <summary>
    /// Obtient le solde de la commande
    /// </summary>
    public decimal MontantHTSolde { get; set; }

    /// <summary>
    /// Obtient la devise de la ligne de commande (renvoie la devise de l'en-tête de la commande)
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une ligne de commande a été créée.
    /// </summary>
    /// <value>
    /// <c>true</c> si une ligne de commande a été créé; sinon, <c>false</c>.
    /// </value>
    public bool IsCreated { get; set; }
        
    /// <summary>
    /// Obtient ou définit une valeur indiquant si une ligne de commande a été supprimée.
    /// </summary>
    /// <value>
    /// <c>true</c> si une ligne de commande a été supprimée; sinon, <c>false</c>.
    /// </value>
    public bool IsDeleted { get; set; }
  }
}