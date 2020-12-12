using System.Collections.Generic;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.EcritureComptable
{
    /// <summary>
    /// Model permettant de determiner si une écriture comptable est valide ou non
    /// </summary>
    public class EcritureComptableMappingModel
    {
        public string NumeroPiece { get; set; }

        public int DeviseId { get; set; }

        public DeviseEnt Devise { get; set; }

        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }

        public int NatureId { get; set; }

        public NatureEnt Nature { get; set; }

        public int CiId { get; set; }

        public List<string> Erreurs { get; set; }

        public string DeviseCode { get; set; }

        public bool Success { get; set; }

        public object NatureCode { get; set; }

        public int ParentFamilyODWithOrder { get; set; }

        public int ParentFamilyODWithoutOrder { get; set; }

        public string NumeroCommande { get; set; }

        public int FamilleOperationDiverseId { get; set; }

        public string CodeCi { get; set; }

        public int RessourceId { get; set; }

        public int? CommandeId { get; set; }

        public int SocieteId { get; set; }

        public int UniteId { get; set; }

        public string UniteCode { get; set; }

        public string FamilleOperationDiverseCode { get; set; }

        public string NumeroFactureSAP { get; set; }

        public string RessourceCode { get; set; }
    }
}
