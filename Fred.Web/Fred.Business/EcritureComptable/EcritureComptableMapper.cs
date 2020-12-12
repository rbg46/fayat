using Fred.Entities.EcritureComptable;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.EcritureComptable;
using Fred.Web.Shared.Models.OperationDiverse;
using Fred.Web.Shared.Models.Valorisation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.EcritureComptable
{
    /// <summary>
    /// Classe permettant de mapper un type d'écriture comptable en un autre type
    /// </summary>
    public class EcritureComptableMapper
    {
        /// <summary>
        /// Map une liste d'écriture comptable Fayat TP avec une liste de ressource
        /// </summary>
        /// <param name="ecritureComptableValorisation">Liste de <see cref="EcritureComptableFtpDto"/></param>
        /// <param name="valorisationFRED">Liste de valorisation</param>
        /// <returns>Une liste de OperationDiverseCommandeFtpModel associé avec la ressource correspondante</returns>
        public List<OperationDiverseCommandeFtpModel> MapEcritureComptableWithValorisation(List<EcritureComptableFtpDto> ecritureComptableValorisation, IReadOnlyList<ValorisationEcritureComptableODModel> valorisationFRED)
        {
            List<EcritureComptableValorisationModel> listPointagePersonnel = GetEcritureComptableValorisationPersonnel(ecritureComptableValorisation, valorisationFRED);
            List<EcritureComptableValorisationModel> listPointageMateriel = GetEcritureComptableValorisationMateriel(ecritureComptableValorisation, valorisationFRED);

            List<EcritureComptableFtpDto> listeOd = AssignCorrectAmount(listPointagePersonnel.Concat(listPointageMateriel).ToList());

            List<OperationDiverseCommandeFtpModel> result = new List<OperationDiverseCommandeFtpModel>();

            foreach (EcritureComptableFtpDto od in listeOd)
            {
                result.Add(new OperationDiverseCommandeFtpModel
                {
                    CodeCi = od.CodeCi,
                    CodeDevise = od.CodeDevise,
                    CodeMateriel = od.CodeMateriel,
                    CodeRef = od.CodeRef,
                    CommandeId = -1,
                    DateComptable = od.DateComptable,
                    FamilleOperationDiverseId = od.FamilleOperationDiverseId,
                    FamilleOperationDiverseCode = od.FamilleOperationDiversesCode,
                    Libelle = od.Libelle,
                    Montant = od.Montant,
                    NumeroPiece = od.NumeroPiece,
                    Quantite = od.Quantite,
                    RapportLigneId = od.RapportLigneId,
                    RessourceCode = od.RessourceCode,
                    SocieteCode = od.SocieteCode,
                    SapCodeNature = od.NatureAnalytique,
                    PersonnelCode = od.PersonnelCode,
                    SocieteMaterielCode = od.SocieteMaterielCode,
                    NumeroCommande = od.NumeroCommande,
                    NatureId = od.NatureId,
                    CiId = od.CiId,
                    DeviseId = od.DeviseId,
                    TacheId = od.TacheId,
                    EcritureComptableId = od.EcritureComptableId,
                    RessourceId = od.RessourceId,
                    MontantCommande = od.Montant,
                    MontantDevise = od.MontantDevise,
                    QuantiteCommande = od.Quantite,
                    Unite = od.Unite
                });
            }

            return result;
        }

        private static List<EcritureComptableFtpDto> AssignCorrectAmount(List<EcritureComptableValorisationModel> listPointage)
        {
            List<EcritureComptableFtpDto> result = new List<EcritureComptableFtpDto>();
            listPointage.ForEach(item => item.Ecriture.Montant = item.Montant);
            result.AddRange(listPointage.Select(x => x.Ecriture).ToList());
            return result;
        }

        private static List<EcritureComptableValorisationModel> GetEcritureComptableValorisationMateriel(List<EcritureComptableFtpDto> ecritureComptableValorisation, IReadOnlyList<ValorisationEcritureComptableODModel> valorisationFRED)
        {
            return (from ecriture in ecritureComptableValorisation
                    join valorisation in valorisationFRED on ecriture.RapportLigneId equals valorisation.RapportLigneId
                    where valorisation.Montant != ecriture.Montant && ecriture.IsPointageMateriel && valorisation.MaterielId.HasValue
                    select new EcritureComptableValorisationModel
                    {
                        Ecriture = ecriture,
                        Valorisation = valorisation,
                        Montant = ecriture.Montant - valorisation.Montant

                    }).ToList();
        }

        private static List<EcritureComptableValorisationModel> GetEcritureComptableValorisationPersonnel(List<EcritureComptableFtpDto> ecritureComptableValorisation, IReadOnlyList<ValorisationEcritureComptableODModel> valorisationFRED)
        {
            return (from ecriture in ecritureComptableValorisation
                    join valorisation in valorisationFRED on ecriture.RapportLigneId equals valorisation.RapportLigneId
                    where valorisation.Montant != ecriture.Montant && ecriture.IsPointagePersonnel && valorisation.PersonnelId.HasValue
                    select new EcritureComptableValorisationModel
                    {
                        Ecriture = ecriture,
                        Valorisation = valorisation,
                        Montant = ecriture.Montant - valorisation.Montant

                    }).ToList();
        }

        /// <summary>
        /// Map une liste d'écriture comptable Fayat TP avec une liste de commande
        /// </summary>
        /// <param name="ecritureComptablecommandes">Liste de <see cref="EcritureComptableFtpDto"/></param>
        /// <param name="commandeFRED">Liste de commande</param>
        /// <returns>Une liste de OperationDiverseCommandeFtpModel associé avec la ressource correspondante</returns>
        public List<OperationDiverseCommandeFtpModel> MapEcritureComptableWithCommande(IEnumerable<EcritureComptableFtpDto> ecritureComptablecommandes, IReadOnlyList<CommandeEcritureComptableOdModel> commandeFRED)
        {
            return (from ecriture in ecritureComptablecommandes
                    join commande in commandeFRED on ecriture.NumeroCommande equals commande.NumeroCommande
                    where commande.Montant != ecriture.Montant
                    select new OperationDiverseCommandeFtpModel
                    {
                        CodeCi = ecriture.CodeCi,
                        CodeDevise = ecriture.CodeDevise,
                        CodeMateriel = ecriture.CodeMateriel,
                        CodeRef = ecriture.CodeRef,
                        CommandeId = commande.CommandeId,
                        DateComptable = ecriture.DateComptable,
                        FamilleOperationDiverseId = ecriture.FamilleOperationDiverseId,
                        FamilleOperationDiverseCode = ecriture.FamilleOperationDiversesCode,
                        Libelle = ecriture.Libelle,
                        Montant = ecriture.Montant - commande.Montant,
                        MontantODCommande = ecriture.Montant - commande.Montant,
                        NumeroPiece = ecriture.NumeroPiece,
                        Quantite = ecriture.Quantite,
                        RapportLigneId = ecriture.RapportLigneId,
                        RessourceCode = ecriture.RessourceCode,
                        SocieteCode = ecriture.SocieteCode,
                        SapCodeNature = ecriture.NatureAnalytique,
                        PersonnelCode = ecriture.PersonnelCode,
                        SocieteMaterielCode = ecriture.SocieteMaterielCode,
                        NumeroCommande = ecriture.NumeroCommande,
                        TacheId = ecriture.TacheId,
                        Unite = ecriture.Unite,
                        CiId = ecriture.CiId,
                        DeviseId = ecriture.DeviseId,
                        EcritureComptableId = ecriture.EcritureComptableId,
                        NatureId = ecriture.NatureId
                    }).ToList();
        }

        /// <summary>
        /// Map une <see cref="EcritureComptableDto"/> en <see cref="EcritureComptableEnt"/>
        /// </summary>
        /// <param name="ecritureComptablesDto">Ecriture comptable</param>
        /// <returns><see cref="EcritureComptableEnt"/></returns>
        private EcritureComptableEnt MapEcritureComptableDtoWithNewEntity(EcritureComptableDto ecritureComptablesDto)
        {
            return new EcritureComptableEnt
            {
                NumeroPiece = ecritureComptablesDto.NumeroPiece,
                DateCreation = DateTime.UtcNow,
                DateComptable = ecritureComptablesDto.DateComptable,
                Montant = ecritureComptablesDto.Montant,
                Libelle = ecritureComptablesDto.Libelle
            };
        }

        public List<EcritureComptableEnt> MapEcritureComptableDtoWithNewEntity(IEnumerable<EcritureComptableDto> ecritureComptablesDtos)
        {
            List<EcritureComptableEnt> ecritureComptables = new List<EcritureComptableEnt>();
            foreach (EcritureComptableDto ecritureComptablesDto in ecritureComptablesDtos)
            {
                ecritureComptables.Add(MapEcritureComptableDtoWithNewEntity(ecritureComptablesDto));
            }
            return ecritureComptables;
        }

        /// <summary>
        /// Map une <see cref="EcritureComptableFtpDto"/> en <see cref="EcritureComptableEnt"/>
        /// </summary>
        /// <param name="ecritureComptablesFtpDto">Ecriture Comptable Fayat TP</param>
        /// <returns><see cref="EcritureComptableEnt"/></returns>
        public EcritureComptableEnt MapEcritureComptableDtoWithNewEntity(EcritureComptableFtpDto ecritureComptablesFtpDto)
        {
            return new EcritureComptableEnt
            {
                NumeroPiece = ecritureComptablesFtpDto.NumeroPiece,
                DateCreation = DateTime.UtcNow,
                DateComptable = ecritureComptablesFtpDto.DateComptable,
                Montant = ecritureComptablesFtpDto.Montant,
                Libelle = ecritureComptablesFtpDto.Libelle,
                Quantite = ecritureComptablesFtpDto.Quantite,
                CodeRef = ecritureComptablesFtpDto.CodeRef,
                NumeroFactureSAP = ecritureComptablesFtpDto.NumFactureSAP
            };
        }
    }
}
