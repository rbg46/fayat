using System.Collections.Generic;
using Fred.Entities.EcritureComptable;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.EcritureComptable.Import
{
    /// <summary>
    /// Permet de convertir un liste de model de type ecriture comptable en model d'OD
    /// </summary>
    public class EcritureComptableConverter
    {
        /// <summary>
        /// Permet de convertir une liste de <see cref="OperationDiverseCommandeFtpModel"/> en <see cref="EcritureComptableFtpDto"/>
        /// </summary>
        /// <param name="operationDiverses">Liste de <see cref="OperationDiverseCommandeFtpModel"/></param>
        /// <returns>Liste de <see cref="EcritureComptableFtpDto"/></returns>
        public List<EcritureComptableFtpDto> Convert(List<OperationDiverseCommandeFtpModel> operationDiverses)
        {
            List<EcritureComptableFtpDto> ecritureComptables = new List<EcritureComptableFtpDto>();
            foreach (OperationDiverseCommandeFtpModel operationDiverse in operationDiverses)
            {
                ecritureComptables.Add(new EcritureComptableFtpDto
                {
                    CiId = operationDiverse.CiId,
                    CodeCi = operationDiverse.CodeCi,
                    CodeDevise = operationDiverse.CodeDevise,
                    CodeMateriel = operationDiverse.CodeMateriel,
                    CodeRef = operationDiverse.CodeRef,
                    DeviseId = operationDiverse.DeviseId,
                    DateComptable = operationDiverse.DateComptable,
                    EcritureComptableId = operationDiverse.EcritureComptableId,
                    FamilleOperationDiverseId = operationDiverse.FamilleOperationDiverseId,
                    Libelle = operationDiverse.Libelle,
                    Montant = operationDiverse.Montant,
                    MontantDevise = operationDiverse.MontantDevise,
                    NatureId = operationDiverse.NatureId,
                    NumeroCommande = operationDiverse.NumeroCommande,
                    NumeroPiece = operationDiverse.NumeroPiece,
                    PersonnelCode = operationDiverse.PersonnelCode,
                    Quantite = operationDiverse.Quantite,
                    RapportLigneId = operationDiverse.RapportLigneId,
                    RessourceCode = operationDiverse.RessourceCode,
                    RessourceId = operationDiverse.RessourceId,
                    NatureAnalytique = operationDiverse.SapCodeNature,
                    SocieteCode = operationDiverse.SocieteCode,
                    SocieteMaterielCode = operationDiverse.SocieteMaterielCode,
                    TacheId = operationDiverse.TacheId,
                    Unite = operationDiverse.Unite
                });
            }
            return ecritureComptables;
        }

        /// <summary>
        /// Permet de convertir une liste de <see cref="EcritureComptableFtpDto"/> en liste de <see cref="OperationDiverseCommandeFtpModel"/>
        /// </summary>
        /// <param name="ecritureComptableFtps">liste de <see cref="EcritureComptableFtpDto"/></param>
        /// <returns>liste de <see cref="OperationDiverseCommandeFtpModel"/></returns>
        public List<OperationDiverseCommandeFtpModel> Convert(List<EcritureComptableFtpDto> ecritureComptableFtps)
        {
            List<OperationDiverseCommandeFtpModel> operationDiverseCommandeFtps = new List<OperationDiverseCommandeFtpModel>();
            foreach (EcritureComptableFtpDto ecritureComptable in ecritureComptableFtps)
            {
                operationDiverseCommandeFtps.Add(new OperationDiverseCommandeFtpModel
                {
                    CiId = ecritureComptable.CiId,
                    CodeCi = ecritureComptable.CodeCi,
                    CodeDevise = ecritureComptable.CodeDevise,
                    CodeMateriel = ecritureComptable.CodeMateriel,
                    CodeRef = ecritureComptable.CodeRef,
                    CommandeId = null,
                    DeviseId = ecritureComptable.DeviseId,
                    DateComptable = ecritureComptable.DateComptable,
                    EcritureComptableId = ecritureComptable.EcritureComptableId,
                    FamilleOperationDiverseCode = ecritureComptable.FamilleOperationDiversesCode,
                    FamilleOperationDiverseId = ecritureComptable.FamilleOperationDiverseId,
                    Libelle = ecritureComptable.Libelle,
                    Montant = ecritureComptable.Montant,
                    MontantCommande = 0,
                    MontantDevise = ecritureComptable.MontantDevise,
                    NatureId = ecritureComptable.NatureId,
                    NumeroCommande = ecritureComptable.NumeroCommande,
                    NumeroPiece = ecritureComptable.NumeroPiece,
                    PersonnelCode = ecritureComptable.PersonnelCode,
                    Quantite = ecritureComptable.Quantite,
                    QuantiteCommande = 0,
                    RapportLigneId = null,
                    RessourceCode = ecritureComptable.RessourceCode,
                    RessourceId = ecritureComptable.RessourceId,
                    SapCodeNature = ecritureComptable.NatureAnalytique,
                    SocieteCode = ecritureComptable.SocieteCode,
                    SocieteMaterielCode = ecritureComptable.SocieteMaterielCode,
                    TacheId = ecritureComptable.TacheId,
                    Unite = ecritureComptable.Unite
                });
            }
            return operationDiverseCommandeFtps;
        }

    }
}
