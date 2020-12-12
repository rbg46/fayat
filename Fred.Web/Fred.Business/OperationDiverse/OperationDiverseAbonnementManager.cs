using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.OperationDiverse;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Extensions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Enum;
using Fred.Web.Shared.Models.Enum;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.OperationDiverse
{
    public class OperationDiverseAbonnementManager : Manager<OperationDiverseEnt, IOperationDiverseRepository>, IOperationDiverseAbonnementManager
    {
        private readonly IMapper mapper;
        private readonly IDateTimeExtendManager dateTimeExtendManager;
        private readonly IUniteManager uniteManager;
        private readonly IUtilisateurManager utilisateurManager;

        public OperationDiverseAbonnementManager(
            IUnitOfWork uow,
            IOperationDiverseRepository operationDiverseRepository,
            IMapper mapper,
            IDateTimeExtendManager dateTimeExtendManager,
            IUniteManager uniteManager,
            IUtilisateurManager utilisateurManager)
            : base(uow, operationDiverseRepository)
        {
            this.mapper = mapper;
            this.dateTimeExtendManager = dateTimeExtendManager;
            this.uniteManager = uniteManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Recupère la liste des fréquences d'abonnement
        /// </summary>
        /// <returns>Liste de <see cref="EnumModel"/></returns>
        public List<EnumModel> GetFrequenceAbonnement()
        {
            return ConvertEnum(typeof(FrequenceAbonnement));
        }

        /// <summary>
        /// Converti un enum dans un model
        /// </summary>
        /// <param name="enumType">Enum a d</param>
        /// <returns>Liste de <see cref="EnumModel"/></returns>
        private List<EnumModel> ConvertEnum(Type enumType)
        {
            List<EnumModel> enumTypeList = new List<EnumModel>();
            foreach (object value in Enum.GetValues(enumType))
            {
                enumTypeList.Add(new EnumModel()
                {
                    Libelle = Enumeration.ResourceManager.GetString("Enum_" + value) ?? string.Empty,
                    Value = (int)value
                });
            }
            return enumTypeList;
        }

        /// <summary>
        /// Récupère toutes les opérations diverses d'un abonnement par ID du parent de l'abonnement
        /// </summary>
        /// <param name="parentODperationDiversesId">ID du parent de l'abonnement</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see></returns>
        public IEnumerable<OperationDiverseEnt> GetODAbonnement(int parentODperationDiversesId)
        {
            return Repository.GetAbonnementByODMere(parentODperationDiversesId);
        }

        /// <summary>
        /// Récupération de la dernière date (dernière échéance) de génération des opérations diverses abonnements
        /// </summary>
        /// <param name="datePremiereGeneration">Date de la première génération</param>
        /// <param name="frequenceAbonnement">Fréquence de l'abonnement</param>
        /// <param name="nombreOccurence">Nombre d'occurence d'opération diverse à générer</param>
        /// <returns>Date du dernier abonnement</returns>
        public DateTime GetLastDayOfODAbonnement(DateTime datePremiereGeneration, int frequenceAbonnement, int nombreOccurence)
        {
            nombreOccurence = nombreOccurence > 0 ? nombreOccurence - 1 : nombreOccurence + 1;

            int frequenceAbonnementAbsolu = Math.Abs(frequenceAbonnement);

            if (nombreOccurence == 0)
            {
                return datePremiereGeneration;
            }

            switch ((FrequenceAbonnement)frequenceAbonnementAbsolu)
            {
                case FrequenceAbonnement.Jour:
                    return CalculateLastDateFrequenceByBusinessDay(datePremiereGeneration, nombreOccurence);
                case FrequenceAbonnement.Semaine:
                    return CalculateLastDateFrequenceByDay(datePremiereGeneration, nombreOccurence);
                case FrequenceAbonnement.Mois:
                    return CalculateLastDateFrequenceByMonth(datePremiereGeneration, nombreOccurence);
                case FrequenceAbonnement.Trimestre:
                    return CalculateLastDateFrequenceBySemester(datePremiereGeneration, nombreOccurence);
                case FrequenceAbonnement.Annee:
                    return CalculateLastDateFrequenceByYear(datePremiereGeneration, nombreOccurence);
                default:
                    return datePremiereGeneration;
            }
        }

        /// <summary>
        /// Calcul de la derniere date d'une fréquence par jour travaillé
        /// </summary>
        /// <param name="dateInitial">Date de départ</param>
        /// <param name="nombreReccurence">Nombre de jour travaillé à ajouter</param>
        /// <returns>La derniere date de la fréquence</returns>
        private DateTime CalculateLastDateFrequenceByBusinessDay(DateTime dateInitial, int nombreReccurence)
        {
            int workDayLeftToCount = Math.Abs(nombreReccurence);
            DateTime lastDate = dateInitial;
            while (workDayLeftToCount >= 0)
            {
                bool isBusinessDay = dateTimeExtendManager.IsBusinessDay(lastDate);
                if (isBusinessDay)
                {
                    workDayLeftToCount--;
                }
                if (workDayLeftToCount >= 0)
                {
                    if (nombreReccurence > 0)
                    {
                        lastDate = lastDate.AddDays(1);
                    }
                    else
                    {
                        lastDate = lastDate.AddDays(-1);
                    }
                }
            }
            return lastDate;
        }

        /// <summary>
        /// Calcul de la dernière date d'une fréquence par jour
        /// </summary>
        /// <param name="dateInitial">Date de départ</param>
        /// <param name="nombreReccurence">Nombre de semaine à ajouter</param>
        /// <returns>La derniere date de la fréquence</returns>
        private DateTime CalculateLastDateFrequenceByDay(DateTime dateInitial, int nombreReccurence)
        {
            return dateInitial.AddDays(7 * nombreReccurence);
        }

        /// <summary>
        /// Calcul de la dernière date d'une fréquence par semaine
        /// </summary>
        /// <param name="dateInitial">Date de départ</param>
        /// <param name="nombreReccurence">Nombre de mois à ajouter</param>
        /// <returns>La derniere date de la fréquence</returns>
        private DateTime CalculateLastDateFrequenceByMonth(DateTime dateInitial, int nombreReccurence)
        {
            return dateInitial.AddMonths(1 * nombreReccurence);
        }

        /// <summary>
        /// Calcul de la dernière date d'une fréquence par semestre
        /// </summary>
        /// <param name="dateInitial">Date de départ</param>
        /// <param name="nombreReccurence">Nombre de semestre à ajouter</param>
        /// <returns>La derniere date de la fréquence</returns>
        private DateTime CalculateLastDateFrequenceBySemester(DateTime dateInitial, int nombreReccurence)
        {
            return dateInitial.AddMonths(3 * nombreReccurence);
        }

        /// <summary>
        /// Calcul de la dernière date d'une fréquence par année
        /// </summary>
        /// <param name="dateInitial">Date de départ</param>
        /// <param name="nombreReccurence">Nombre d'année à ajouter</param>
        /// <returns>La derniere date de la fréquence</returns>
        private DateTime CalculateLastDateFrequenceByYear(DateTime dateInitial, int nombreReccurence)
        {
            return dateInitial.AddYears(1 * nombreReccurence);
        }

        /// <summary>
        /// Chargement des données abonnements
        /// </summary>
        /// <param name="operationDiverseModel">Opération diverse model à charger</param>
        /// <returns>Opération diverse model avec les données abonnements chargées</returns>
        public OperationDiverseAbonnementModel LoadAbonnement(OperationDiverseAbonnementModel operationDiverseModel)
        {
            if (operationDiverseModel.EstUnAbonnement)
            {
                IEnumerable<OperationDiverseEnt> abonnementODList = GetODAbonnement(operationDiverseModel.OperationDiverseMereIdAbonnement.HasValue ? operationDiverseModel.OperationDiverseMereIdAbonnement.Value : operationDiverseModel.OperationDiverseId);
                operationDiverseModel.DureeAbonnement = abonnementODList.Count();
                operationDiverseModel.DatePremiereODAbonnement = abonnementODList.Min(x => x.DateComptable);
                operationDiverseModel.DateDerniereODAbonnement = abonnementODList.Max(x => x.DateComptable);
                operationDiverseModel.DateProchaineODAbonnement = operationDiverseModel.DateComptable;
                operationDiverseModel.DatePreviousODAbonnement = abonnementODList.Where(x => x.DateComptable < operationDiverseModel.DateComptable)?.Max(x => x.DateComptable);
                FrequenceAbonnement abonnement = DetectPeriodeAbonnement(operationDiverseModel.DatePremiereODAbonnement.Value, operationDiverseModel.DateDerniereODAbonnement.Value, operationDiverseModel.DureeAbonnement.Value);
                operationDiverseModel.FrequenceAbonnementModel = new EnumModel()
                {
                    Libelle = Enumeration.ResourceManager.GetString("Enum_" + abonnement.ToString()),
                    Value = (int)abonnement
                };
            }
            return operationDiverseModel;
        }

        /// <summary>
        /// Detecte la fréquence d'abonnement
        /// </summary>
        /// <param name="firstDate">Premiere date de l'abonnement</param>
        /// <param name="endDate">Dernière date de l'abonnement</param>
        /// <param name="dureeAbonnement">Nombre de date générée</param>
        /// <returns>Fréquence d'abonnement</returns>
        private FrequenceAbonnement DetectPeriodeAbonnement(DateTime firstDate, DateTime endDate, int dureeAbonnement)
        {
            // Prevent zero value
            float dureeAbonnementCalcul = dureeAbonnement == 1 ? 1 : (dureeAbonnement / 2);

            float diffInMonth = (float)(((endDate.Year - firstDate.Year) * 12) + endDate.Month - firstDate.Month) / dureeAbonnementCalcul;
            float diffInDay = (float)((endDate - firstDate).TotalDays) / dureeAbonnementCalcul;

            if (diffInMonth >= 12)
            {
                return FrequenceAbonnement.Annee;
            }
            else if (diffInMonth >= 3)
            {
                return FrequenceAbonnement.Trimestre;
            }
            else if (diffInMonth >= 1)
            {
                return FrequenceAbonnement.Mois;
            }
            else if (diffInDay >= 7)
            {
                return FrequenceAbonnement.Semaine;
            }
            else if (diffInDay >= 1)
            {
                return FrequenceAbonnement.Jour;
            }
            return FrequenceAbonnement.NONE;
        }

        /// <summary>
        /// Mise à jour d'un abonnement d'opération diverse
        /// </summary>
        /// <param name="operationDiverseAbonnementModel">Opération diverse à mettre à jour</param>
        /// <returns>Liste de <see cref="OperationDiverseAbonnementModel"></see> mises à jour </returns>
        public IEnumerable<OperationDiverseEnt> Update(OperationDiverseAbonnementModel operationDiverseAbonnementModel)
        {
            operationDiverseAbonnementModel = SetDefaultValue(operationDiverseAbonnementModel);
            List<OperationDiverseEnt> updatedODList = new List<OperationDiverseEnt>();
            bool isChangeSubscriptionConditions = IsChangeSubscriptionConditions(operationDiverseAbonnementModel);

            IEnumerable<OperationDiverseAbonnementModel> operationDiverseModelList = GenerateAbonnement(operationDiverseAbonnementModel).OrderBy(x => x.DateComptable);

            if (operationDiverseAbonnementModel.OperationDiverseMereIdAbonnement.HasValue)
            {
                operationDiverseModelList = ReuseOldAbonnement(operationDiverseModelList, operationDiverseAbonnementModel.OperationDiverseId, operationDiverseAbonnementModel.OperationDiverseMereIdAbonnement.Value, isChangeSubscriptionConditions);
            }
            else
            {
                operationDiverseModelList = ReuseOldAbonnement(operationDiverseModelList, operationDiverseAbonnementModel.OperationDiverseId, operationDiverseAbonnementModel.OperationDiverseId, isChangeSubscriptionConditions);
            }

            IEnumerable<OperationDiverseEnt> operationDiverseEntListAddOrUpdate;

            if (isChangeSubscriptionConditions)
            {
                operationDiverseEntListAddOrUpdate = SaveActionsForOperationDiverseAbonnement(operationDiverseModelList, operationDiverseAbonnementModel.OperationDiverseId);

                if (operationDiverseAbonnementModel.OperationDiverseMereIdAbonnement.HasValue)
                {
                    UpdateOdListByCancelingSubscription(operationDiverseAbonnementModel.OperationDiverseMereIdAbonnement.Value);
                }
            }
            else
            {
                operationDiverseEntListAddOrUpdate = SaveActionsForOperationDiverseAbonnement(operationDiverseModelList.Where(od => od.OperationDiverseId != 0), operationDiverseAbonnementModel.OperationDiverseId);
            }

            updatedODList.AddRange(operationDiverseEntListAddOrUpdate);

            return updatedODList;
        }

        private bool IsChangeSubscriptionConditions(OperationDiverseAbonnementModel operationDiverseAbonnementModelInProgress)
        {
            OperationDiverseAbonnementModel operationDiverseAbonnementModelInProgressTmp = new OperationDiverseAbonnementModel
            {
                EstUnAbonnement = operationDiverseAbonnementModelInProgress.EstUnAbonnement,
                OperationDiverseMereIdAbonnement = operationDiverseAbonnementModelInProgress.OperationDiverseMereIdAbonnement,
                OperationDiverseId = operationDiverseAbonnementModelInProgress.OperationDiverseId,
                DateComptable = operationDiverseAbonnementModelInProgress.DateComptable,
                DatePremiereODAbonnement = operationDiverseAbonnementModelInProgress.DatePremiereODAbonnement,
                DateDerniereODAbonnement = operationDiverseAbonnementModelInProgress.DateDerniereODAbonnement,
                DureeAbonnement = operationDiverseAbonnementModelInProgress.DureeAbonnement,
                FrequenceAbonnementModel = operationDiverseAbonnementModelInProgress.FrequenceAbonnementModel
            };

            OperationDiverseAbonnementModel operationDiverseAbonnementModelInitial = LoadAbonnement(operationDiverseAbonnementModelInProgressTmp);

            return operationDiverseAbonnementModelInProgress.FrequenceAbonnementModel.Value != operationDiverseAbonnementModelInitial.FrequenceAbonnementModel.Value ||
                operationDiverseAbonnementModelInProgress.DureeAbonnement != operationDiverseAbonnementModelInitial.DureeAbonnement ||
                operationDiverseAbonnementModelInProgress.DatePremiereODAbonnement != operationDiverseAbonnementModelInitial.DatePremiereODAbonnement ||
                operationDiverseAbonnementModelInProgress.DateDerniereODAbonnement != operationDiverseAbonnementModelInitial.DateDerniereODAbonnement;
        }

        private IEnumerable<OperationDiverseEnt> SaveActionsForOperationDiverseAbonnement(IEnumerable<OperationDiverseAbonnementModel> operationDiverseModelList, int operationDiverseId)
        {
            IEnumerable<OperationDiverseEnt> operationDiverseEntListAddOrUpdate = operationDiverseModelList.Where(x => !x.NeedToBeDeleted).Select(x => mapper.Map<OperationDiverseEnt>(x));
            IEnumerable<OperationDiverseEnt> operationDiverseEntListDelete = operationDiverseModelList.Where(x => x.NeedToBeDeleted).Select(x => mapper.Map<OperationDiverseEnt>(x));

            // Update la première opération diverse d'un abonnement (OD mère de l'abonnement)
            OperationDiverseEnt operationDiverseMere = operationDiverseEntListAddOrUpdate.FirstOrDefault(x => x.OperationDiverseId == operationDiverseId);
            if (operationDiverseMere != null)
            {
                Repository.UpdateOD(operationDiverseMere);
            }

            Repository.DeleteListOD(operationDiverseEntListDelete.ToList());
            Save();

            Repository.AddListOD(operationDiverseEntListAddOrUpdate.Where(x => x.OperationDiverseId == 0).ToList());
            Repository.UpdateListOD(operationDiverseEntListAddOrUpdate.Where(x => x.OperationDiverseId != 0 && x.OperationDiverseId != operationDiverseId).ToList());

            return operationDiverseEntListAddOrUpdate;
        }

        private void UpdateOdListByCancelingSubscription(int idMereOdAbonnement)
        {
            IEnumerable<OperationDiverseEnt> odAbonnementList = GetODAbonnement(idMereOdAbonnement);

            odAbonnementList.ForEach(od => od.EstUnAbonnement = false);

            Repository.UpdateListOD(odAbonnementList.ToList());
        }

        /// <summary>
        /// Ajout d'un abonnement d'opération diverse
        /// </summary>
        /// <param name="operationDiverseModel">Opération diverse à ajouter</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see> ajoutées </returns>
        public IEnumerable<OperationDiverseEnt> Add(OperationDiverseAbonnementModel operationDiverseModel)
        {
            List<OperationDiverseEnt> operationDiverseEntListUpdated = new List<OperationDiverseEnt>();

            operationDiverseModel = SetDefaultValue(operationDiverseModel);
            IEnumerable<OperationDiverseAbonnementModel> operationDiverseModelList = GenerateAbonnement(operationDiverseModel);
            List<OperationDiverseEnt> operationDiverseEntList = operationDiverseModelList.Select(x => mapper.Map<OperationDiverseEnt>(x)).ToList();

            OperationDiverseEnt operationDiverseToInsert = operationDiverseEntList.OrderBy(x => x.DateComptable).First();

            if (operationDiverseToInsert != null)
            {
                OperationDiverseEnt operationDiverseMere = Repository.Insert(operationDiverseToInsert);
                Save();
                operationDiverseEntListUpdated.Add(operationDiverseMere);

                IEnumerable<OperationDiverseEnt> operationDiverseEntListChild = operationDiverseEntList.Where(x => x.DateComptable != operationDiverseMere.DateComptable);
                operationDiverseEntListChild.ForEach(x => x.OperationDiverseMereIdAbonnement = operationDiverseMere.OperationDiverseId);
                operationDiverseEntListUpdated.AddRange(Repository.AddListOD(operationDiverseEntListChild));
            }

            return operationDiverseEntListUpdated;
        }

        /// <summary>
        /// Génere l'abonnement d'une opération diverse
        /// </summary>
        /// <param name="operationDiverse">Opération diverse parent à générer</param>
        /// <returns>Abonnement <see cref="OperationDiverseAbonnementModel"> généré</see></returns>
        private IEnumerable<OperationDiverseAbonnementModel> GenerateAbonnement(OperationDiverseAbonnementModel operationDiverse)
        {
            List<OperationDiverseAbonnementModel> abonnementGenerated = new List<OperationDiverseAbonnementModel>();
            if (operationDiverse.EstUnAbonnement)
            {
                abonnementGenerated.AddRange(Enumerable.Repeat(operationDiverse, Math.Abs(operationDiverse.DureeAbonnement.Value)).Select(x => CloningOperationDiverseAbonnementModel(x)));
                DateTime dateCreation = DateTime.Now;

                for (int i = 0; i < Math.Abs(operationDiverse.DureeAbonnement.Value); i++)
                {
                    if (operationDiverse.DureeAbonnement > 0)
                    {
                        abonnementGenerated[i].DateComptable = GetLastDayOfODAbonnement(operationDiverse.DateProchaineODAbonnement.Value, operationDiverse.FrequenceAbonnementModel.Value, i + 1);
                    }
                    else
                    {
                        abonnementGenerated[i].DateComptable = GetLastDayOfODAbonnement(operationDiverse.DateProchaineODAbonnement.Value, operationDiverse.FrequenceAbonnementModel.Value, (i + 1) * -1);
                    }
                    abonnementGenerated[i].DateCreation = dateCreation;
                    abonnementGenerated[i].OperationDiverseId = i == 0 ? abonnementGenerated[0].OperationDiverseId : 0;
                    abonnementGenerated[i].OperationDiverseMereIdAbonnement = i == 0 ? (int?)null : abonnementGenerated[0].OperationDiverseId;
                }
            }
            else
            {
                abonnementGenerated.Add(operationDiverse);
            }
            return abonnementGenerated;
        }

        private OperationDiverseAbonnementModel CloningOperationDiverseAbonnementModel(OperationDiverseAbonnementModel operationDiverse)
        {
            return new OperationDiverseAbonnementModel()
            {
                OperationDiverseId = operationDiverse.OperationDiverseId,
                Libelle = operationDiverse.Libelle,
                Commentaire = operationDiverse.Commentaire,
                CiId = operationDiverse.CiId,
                TacheId = operationDiverse.TacheId,
                Tache = operationDiverse.Tache,
                PUHT = operationDiverse.PUHT,
                Quantite = operationDiverse.Quantite,
                Montant = operationDiverse.Montant,
                Unite = operationDiverse.Unite,
                UniteId = operationDiverse.UniteId,
                DeviseId = operationDiverse.DeviseId,
                Cloturee = operationDiverse.Cloturee,
                OdEcart = operationDiverse.OdEcart,
                DateCloture = operationDiverse.DateCloture,
                DateComptable = operationDiverse.DateComptable,
                AuteurCreationId = operationDiverse.AuteurCreationId,
                FamilleOperationDiverseId = operationDiverse.FamilleOperationDiverseId,
                RessourceId = operationDiverse.RessourceId,
                Ressource = operationDiverse.Ressource,
                EcritureComptableId = operationDiverse.EcritureComptableId,
                DateCreation = operationDiverse.DateCreation,
                EstUnAbonnement = operationDiverse.EstUnAbonnement,
                DureeAbonnement = operationDiverse.DureeAbonnement,
                FrequenceAbonnementModel = operationDiverse.FrequenceAbonnementModel,
                DateProchaineODAbonnement = operationDiverse.DateProchaineODAbonnement,
                DatePremiereODAbonnement = operationDiverse.DatePremiereODAbonnement,
                DateDerniereODAbonnement = operationDiverse.DateDerniereODAbonnement,
                OperationDiverseMereIdAbonnement = operationDiverse.OperationDiverseMereIdAbonnement
            };
        }

        /// <summary>
        /// Réutilisation des OD en base à supprimer
        /// </summary>
        /// <param name="odAbonnementGenerated">Liste des opération diverses à générer</param>
        /// <param name="newODMereAbonnementId">ID de la première OD de l'abonnement à générer (OD Mère)</param>
        /// <param name="oldODMereAbonnementId">ID dde la première OD de l'ancien abonnement</param>
        /// <param name="isChangeSubscriptionConditions">Si c'est un changement de conditions d'abonnement (périodicité, durée,..) ou non</param>
        /// <returns>Liste de <see cref="OperationDiverseAbonnementModel"> contenant les opérations diverses à mettre à jour, à ajouter et à supprimer</see></returns>
        private IEnumerable<OperationDiverseAbonnementModel> ReuseOldAbonnement(IEnumerable<OperationDiverseAbonnementModel> odAbonnementGenerated, int newODMereAbonnementId, int oldODMereAbonnementId, bool isChangeSubscriptionConditions)
        {
            IEnumerable<OperationDiverseEnt> previousODAbonnement = GetODAbonnement(oldODMereAbonnementId);

            DateTime? dateOriginalVersion = previousODAbonnement.First(x => x.OperationDiverseId == newODMereAbonnementId).DateComptable;

            if (dateOriginalVersion.HasValue)
            {
                List<OperationDiverseEnt> oDListAfterDateComptable = previousODAbonnement.Where(x => x.DateComptable >= dateOriginalVersion)
                    .OrderBy(x => x.DateComptable)
                    .ToList();
                List<OperationDiverseAbonnementModel> oDReuseListResult = ReuseOldEnt(odAbonnementGenerated.ToList(), oDListAfterDateComptable, isChangeSubscriptionConditions).ToList();
                oDReuseListResult.AddRange(GetOldODsCannotReuse(odAbonnementGenerated.ToList(), oDListAfterDateComptable).ToList());
                return oDReuseListResult;
            }
            return odAbonnementGenerated;
        }

        /// <summary>
        /// Récupère les opérations diverses ne pouvant pas être réutilisées
        /// </summary>
        /// <param name="operationDiverseModelList">Liste de model d'opérations diverses alimentées</param>
        /// <param name="operationDiverseEntList">Liste d'entités d'opérations diverses</param>
        /// <returns>Liste de <see cref="OperationDiverseAbonnementModel"></see></returns>
        private IEnumerable<OperationDiverseAbonnementModel> GetOldODsCannotReuse(List<OperationDiverseAbonnementModel> operationDiverseModelList, List<OperationDiverseEnt> operationDiverseEntList)
        {
            List<OperationDiverseAbonnementModel> operationDiverseModelListToDelete = new List<OperationDiverseAbonnementModel>();

            if (operationDiverseModelList.Count <= operationDiverseEntList.Count)
            {
                List<OperationDiverseEnt> operationDiverseEntListToDelete = operationDiverseEntList;
                operationDiverseEntListToDelete.RemoveRange(0, operationDiverseModelList.Count);
                operationDiverseModelListToDelete = operationDiverseEntListToDelete.Select(x => mapper.Map<OperationDiverseAbonnementModel>(x)).ToList();
                operationDiverseModelListToDelete.ForEach(x => x.NeedToBeDeleted = true);
            }
            return operationDiverseModelListToDelete;

        }

        /// <summary>
        /// Transfère les ID des entités d'opérations diverses dans les model d'opérations diverses.
        /// Récupère les opérations diverses pouvant être réutilisées
        /// </summary>
        /// <param name="operationDiverseModelList">Liste de model d'opérations diverses à alimentéer</param>
        /// <param name="operationDiverseEntList">Liste d'entités d'opérations diverses</param>
        /// <param name="isChangeSubscriptionConditions">Si c'est un changement de conditions d'abonnement (périodicité, durée,..) ou non</param>
        /// <returns>Liste de <see cref="OperationDiverseAbonnementModel"></see></returns>
        private IEnumerable<OperationDiverseAbonnementModel> ReuseOldEnt(List<OperationDiverseAbonnementModel> operationDiverseModelList, List<OperationDiverseEnt> operationDiverseEntList, bool isChangeSubscriptionConditions)
        {
            int index = 0;
            foreach (OperationDiverseAbonnementModel item in operationDiverseModelList)
            {
                if (operationDiverseEntList.Count > index)
                {
                    item.OperationDiverseId = operationDiverseEntList[index].OperationDiverseId;
                    if (!isChangeSubscriptionConditions)
                    {
                        item.OperationDiverseMereIdAbonnement = operationDiverseEntList[index].OperationDiverseMereIdAbonnement;
                    }
                }
                else
                {
                    break;
                }
                index++;
            }
            return operationDiverseModelList;
        }

        /// <summary>
        /// Supprime tout ou partie d'un abonnement
        /// </summary>
        /// <param name="operationDiverseModel">Opération diverse à supprimer</param>
        public void Delete(OperationDiverseAbonnementModel operationDiverseModel)
        {
            if (operationDiverseModel.EstUnAbonnement)
            {
                IEnumerable<OperationDiverseEnt> abonnementODList = GetODAbonnement(operationDiverseModel.OperationDiverseMereIdAbonnement.HasValue ? operationDiverseModel.OperationDiverseMereIdAbonnement.Value : operationDiverseModel.OperationDiverseId);

                OperationDiverseEnt oDNeedToBeDeleted = abonnementODList.First(x => x.OperationDiverseId == operationDiverseModel.OperationDiverseId);
                IEnumerable<OperationDiverseEnt> operationDiverseEntListDelete = abonnementODList.Where(x => x.DateComptable >= oDNeedToBeDeleted.DateComptable);
                Repository.DeleteListOD(operationDiverseEntListDelete.ToList());
                Save();
            }
        }

        /// <summary>
        /// Assigned Default values
        /// </summary>
        /// <param name="operationDiverseAbonnementModel">operation diverse model targeted</param>
        /// <returns>OperationDiverseModelwith default values</returns>
        private OperationDiverseAbonnementModel SetDefaultValue(OperationDiverseAbonnementModel operationDiverseAbonnementModel)
        {
            operationDiverseAbonnementModel.Tache = null;
            operationDiverseAbonnementModel.Ressource = null;
            operationDiverseAbonnementModel.Unite = null;
            operationDiverseAbonnementModel.DateCreation = SetCurrentDatimeIfMinumum(operationDiverseAbonnementModel.DateCreation);
            operationDiverseAbonnementModel.UniteId = SetDefaultUniteIfNotSet(operationDiverseAbonnementModel.UniteId);
            operationDiverseAbonnementModel.AuteurCreationId = utilisateurManager.GetContextUtilisateurId();
            operationDiverseAbonnementModel.DeviseId = 48;
            return operationDiverseAbonnementModel;
        }

        /// <summary>
        /// Get the default UniteID if it's not set
        /// </summary>
        /// <param name="actualUniteId">current value of the UniteId</param>
        /// <returns>UniteId updated</returns>
        private int SetDefaultUniteIfNotSet(int actualUniteId)
        {
            if (actualUniteId == 0)
            {
                return uniteManager.FindById(1).UniteId;
            }
            return actualUniteId;
        }

        /// <summary>
        /// Get the current date time if the minimum value is set
        /// </summary>
        /// <param name="actualDatime">current value of the datetime</param>
        /// <returns>DateTime updated</returns>
        private DateTime SetCurrentDatimeIfMinumum(DateTime actualDatime)
        {
            if (actualDatime == DateTime.MinValue)
            {
                return DateTime.UtcNow;
            }
            return actualDatime;
        }
    }
}
