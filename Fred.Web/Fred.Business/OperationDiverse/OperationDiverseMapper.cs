using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Shared.Models.OperationDiverse;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Classe permettant de mapper un OD
    /// </summary>
    public class OperationDiverseMapper
    {
        /// <summary>
        /// Map une liste d'écriture comptable Fayat TP avec une liste de nature
        /// </summary>
        /// <param name="operationDiverses">Liste de <see cref="OperationDiverseCommandeFtpModel"/></param>
        /// <param name="natures">Liste de  natures</param>
        /// <returns>Une liste de OperationDiverseCommandeFtpModel associé avec la nature correspondante</returns>
        public List<OperationDiverseCommandeFtpModel> MapOperationDiverseWithNature(List<OperationDiverseCommandeFtpModel> operationDiverses, List<NatureEnt> natures)
        {
            return (from operationDiverse in operationDiverses
                    join nature in natures on operationDiverse.NatureId equals nature.NatureId
                    into operationRessources
                    from operationRessource in operationRessources
                    select new OperationDiverseCommandeFtpModel
                    {
                        RessourceId = operationRessource.RessourceId ?? 0,
                        CiId = operationDiverse.CiId,
                        CodeCi = operationDiverse.CodeCi,
                        CodeDevise = operationDiverse.CodeDevise,
                        CodeMateriel = operationDiverse.CodeMateriel,
                        CodeRef = operationDiverse.CodeRef,
                        CommandeId = operationDiverse.CommandeId,
                        DateComptable = operationDiverse.DateComptable,
                        DeviseId = operationDiverse.DeviseId,
                        EcritureComptableId = operationDiverse.EcritureComptableId,
                        FamilleOperationDiverseCode = operationDiverse.FamilleOperationDiverseCode,
                        FamilleOperationDiverseId = operationDiverse.FamilleOperationDiverseId,
                        Libelle = operationDiverse.Libelle,
                        Montant = operationDiverse.Montant,
                        MontantCommande = operationDiverse.MontantCommande,
                        MontantODCommande = operationDiverse.MontantODCommande,
                        MontantDevise = operationDiverse.MontantDevise,
                        NatureId = operationDiverse.NatureId,
                        NumeroCommande = operationDiverse.NumeroCommande,
                        NumeroPiece = operationDiverse.NumeroPiece,
                        PersonnelCode = operationDiverse.PersonnelCode,
                        Quantite = operationDiverse.Quantite,
                        QuantiteCommande = operationDiverse.QuantiteCommande,
                        RapportLigneId = operationDiverse.RapportLigneId,
                        RessourceCode = operationRessource.Code,
                        SapCodeNature = operationDiverse.SapCodeNature,
                        SocieteCode = operationDiverse.SocieteCode,
                        SocieteMaterielCode = operationDiverse.SocieteMaterielCode,
                        TacheId = operationDiverse.TacheId,
                        Unite = operationDiverse.Unite
                    }).ToList();
        }

        /// <summary>
        /// Map une liste d'écriture comptable Fayat TP avec une liste de ressource
        /// </summary>
        /// <param name="operationDiverses">Liste de <see cref="OperationDiverseCommandeFtpModel"/></param>
        /// <param name="ressources">Liste de ressources</param>
        /// <returns>Une liste de OperationDiverseCommandeFtpModel associé avec la ressource correspondante</returns>
        public List<OperationDiverseCommandeFtpModel> MapOperationDiverseWithRessource(List<OperationDiverseCommandeFtpModel> operationDiverses, List<RessourceEnt> ressources)
        {
            return (from operationDiverse in operationDiverses
                    join ressource in ressources on operationDiverse.RessourceCode equals ressource.Code
                    into operationRessources
                    from operationRessource in operationRessources
                    select new OperationDiverseCommandeFtpModel
                    {
                        RessourceId = operationRessource.RessourceId,
                        CiId = operationDiverse.CiId,
                        CodeCi = operationDiverse.CodeCi,
                        CodeDevise = operationDiverse.CodeDevise,
                        CodeMateriel = operationDiverse.CodeMateriel,
                        CodeRef = operationDiverse.CodeRef,
                        CommandeId = operationDiverse.CommandeId,
                        DateComptable = operationDiverse.DateComptable,
                        DeviseId = operationDiverse.DeviseId,
                        EcritureComptableId = operationDiverse.EcritureComptableId,
                        FamilleOperationDiverseCode = operationDiverse.FamilleOperationDiverseCode,
                        FamilleOperationDiverseId = operationDiverse.FamilleOperationDiverseId,
                        Libelle = operationDiverse.Libelle,
                        Montant = operationDiverse.Montant,
                        MontantCommande = operationDiverse.MontantCommande,
                        MontantODCommande = operationDiverse.MontantODCommande,
                        MontantDevise = operationDiverse.MontantDevise,
                        NatureId = operationDiverse.NatureId,
                        NumeroCommande = operationDiverse.NumeroCommande,
                        NumeroPiece = operationDiverse.NumeroPiece,
                        PersonnelCode = operationDiverse.PersonnelCode,
                        Quantite = operationDiverse.Quantite,
                        QuantiteCommande = operationDiverse.QuantiteCommande,
                        RapportLigneId = operationDiverse.RapportLigneId,
                        RessourceCode = operationRessource.Code,
                        SapCodeNature = operationDiverse.SapCodeNature,
                        SocieteCode = operationDiverse.SocieteCode,
                        SocieteMaterielCode = operationDiverse.SocieteMaterielCode,
                        TacheId = operationDiverse.TacheId,
                        Unite = operationDiverse.Unite
                    }).ToList();
        }
    }
}
