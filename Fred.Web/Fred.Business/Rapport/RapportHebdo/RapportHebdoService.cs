using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Rapport.Common.RapportParStatut;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;

namespace Fred.Business.Rapport.RapportHebdo
{
    public class RapportHebdoService : IRapportHebdoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICodeAstreinteRepository codeAstreinteRepository;
        private readonly IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository;
        private readonly IRapportRepository rapportRepository;

        public RapportHebdoService(
            IUnitOfWork unitOfWork,
            ICodeAstreinteRepository codeAstreinteRepository,
            IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository,
            IRapportRepository rapportRepository)
        {
            this.unitOfWork = unitOfWork;
            this.codeAstreinteRepository = codeAstreinteRepository;
            this.rapportLigneCodeAstreinteRepository = rapportLigneCodeAstreinteRepository;
            this.rapportRepository = rapportRepository;
        }

        public void AddPersonnelPointageToAllRapports(List<RapportEnt> rapportList, int personnelId)
        {
            if (rapportList != null && rapportList.Any())
            {
                foreach (RapportEnt rapport in rapportList)
                {
                    if (rapport.ListLignes == null)
                    {
                        rapport.ListLignes = new List<RapportLigneEnt>();
                    }

                    if (!rapport.ListLignes.Any(l => l.PersonnelId == personnelId && l.DateSuppression == null))
                    {
                        rapport.ListLignes.Add(new RapportLigneEnt
                        {
                            PersonnelId = personnelId,
                            CiId = rapport.CiId,
                            DatePointage = rapport.DateChantier,
                            RapportId = rapport.RapportId,
                            ListRapportLigneAstreintes = new List<RapportLigneAstreinteEnt>(),
                            ListRapportLigneMajorations = new List<RapportLigneMajorationEnt>(),
                            ListRapportLignePrimes = new List<RapportLignePrimeEnt>(),
                            ListRapportLigneTaches = new List<RapportLigneTacheEnt>()
                        });
                    }
                }
            }
        }

        public void CreateOrUpdatePrimeAstreinte(RapportEnt rapport)
        {
            if (rapport != null && rapport.ListLignes != null && rapport.ListLignes.Any())
            {
                foreach (var rapportligne in rapport.ListLignes)
                {
                    if (rapportligne.DateSuppression == null)
                    {
                        AddOrUpdateAstreintePrime(rapportligne);
                    }
                }
            }
        }

        public void AddOrUpdateAstreintePrime(RapportLigneEnt rapportLigne)
        {
            if (rapportLigne?.ListRapportLigneAstreintes?.Any() == true)
            {
                CodeAstreinteEnt codeSortieAstreinteDimancheNuit = codeAstreinteRepository.GetCodeAstreintes("AS200");
                CodeAstreinteEnt codeSortieAstreinteLundi_Samedi = codeAstreinteRepository.GetCodeAstreintes("TASTRS");
                bool isPrimeNuit = false;

                if (rapportLigne.DatePointage.DayOfWeek == DayOfWeek.Sunday)
                {
                    AddOrUpdateAstreintePrimeTASTRS(rapportLigne, codeSortieAstreinteDimancheNuit);
                }
                else
                {
                    foreach (RapportLigneAstreinteEnt ligneAstreinte in rapportLigne.ListRapportLigneAstreintes)
                    {
                        if (CalculNombreHeuresAstreinte(ligneAstreinte) > 0)
                        {
                            AddOrUpdateAstreintePrimeSemaine(ligneAstreinte, codeSortieAstreinteDimancheNuit, codeSortieAstreinteLundi_Samedi);
                        }
                        else
                        {
                            DeleteRapportLigneCodeAstreinte(ligneAstreinte, isPrimeNuit);
                            DeleteRapportLigneCodeAstreinte(ligneAstreinte, !isPrimeNuit);
                        }
                    }
                }
            }
        }

        private void AddOrUpdateAstreintePrimeTASTRS(RapportLigneEnt rapportLigne, CodeAstreinteEnt codeSortieAstreinteDimancheNuit)
        {
            foreach (RapportLigneAstreinteEnt ligneAstreinte in rapportLigne.ListRapportLigneAstreintes)
            {
                if (CalculNombreHeuresAstreinte(ligneAstreinte) > 0)
                {
                    AddRapportLigneCodeAstreinte(ligneAstreinte, codeSortieAstreinteDimancheNuit.CodeAstreinteId, true);
                }
                else
                {
                    DeleteRapportLigneCodeAstreinte(ligneAstreinte, false);
                }
            }
        }

        private double CalculNombreHeuresAstreinte(RapportLigneAstreinteEnt listAstreinte)
        {
            TimeSpan total = listAstreinte.DateFinAstreinte - listAstreinte.DateDebutAstreinte;
            return total.TotalHours;
        }

        /// <summary>
        ///  Add or update astreinte prime sortie astreinte
        ///  Soit il y a une sortie la journée entre lundi et samedi (si heure de sortie supérieure à 6h00 et inférieure à 21h00) et donc c'est le code TASTRS
        ///  Soit il y a une sortie la nuit entre lundi et samedi(si heure de sortie supérieure ou égale à 21h00 et inférieure ou égale à 6h00) et donc c'est le code AS200
        ///  Pour dimanche et jour ferié(le jour où ça sera géré) c'est AS200
        ///  Dans le cas d’un chevauchement des heures de sortie jour/nuit, il faudra affecter le code AS200
        /// </summary>
        /// <param name="ligneAstreinte">Ligne Astreinte</param>
        /// <param name="codeSortieAstreinteDimancheNuit">Code sortie astreinte TASTRS</param>
        /// <param name="codeSortieAstreinteLundi_Samedi">Code sortie astreinte AS200</param>
        private void AddOrUpdateAstreintePrimeSemaine(RapportLigneAstreinteEnt ligneAstreinte, CodeAstreinteEnt codeSortieAstreinteDimancheNuit,
            CodeAstreinteEnt codeSortieAstreinteLundi_Samedi)
        {
            if (ligneAstreinte.DateDebutAstreinte.Hour >= Constantes.PrimeAstreinteSeuil.SeuilJour && ligneAstreinte.DateDebutAstreinte.Hour <
                                                                                                   Constantes.PrimeAstreinteSeuil.SeuilNuit
                                                                                                   && ligneAstreinte.DateFinAstreinte.Hour >
                                                                                                   Constantes.PrimeAstreinteSeuil.SeuilJour &&
                                                                                                   ligneAstreinte.DateFinAstreinte.Hour <=
                                                                                                   Constantes.PrimeAstreinteSeuil.SeuilNuit)
            {
                AddRapportLigneCodeAstreinte(ligneAstreinte, codeSortieAstreinteLundi_Samedi.CodeAstreinteId, false);
                DeleteRapportLigneCodeAstreinte(ligneAstreinte, true);
            }
            else
            {
                AddRapportLigneCodeAstreinte(ligneAstreinte, codeSortieAstreinteDimancheNuit.CodeAstreinteId, true);
                DeleteRapportLigneCodeAstreinte(ligneAstreinte, false);
            }
        }

        /// <summary>
        /// Add Rapport ligne code astreinte
        /// </summary>
        /// <param name="ligneAstreinte">Ligne astreinte</param>
        /// <param name="codeAstreinteId">Code astreinte id</param>
        /// <param name="isPrimeNuit">is prime nuit</param>
        private void AddRapportLigneCodeAstreinte(RapportLigneAstreinteEnt ligneAstreinte, int codeAstreinteId, bool isPrimeNuit)
        {
            RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte =
                rapportLigneCodeAstreinteRepository.GetRapportLigneCodeAstreinteEnt(ligneAstreinte.RapportLigneAstreinteId, isPrimeNuit);
            if (rapportLigneCodeAstreinte == null)
            {
                rapportLigneCodeAstreinteRepository.AddRapportLigneAstreintes(ligneAstreinte.RapportLigneId, codeAstreinteId,
                    ligneAstreinte.RapportLigneAstreinteId, isPrimeNuit);
            }
        }

        /// <summary>
        /// Delete rapport ligne code astreinte
        /// </summary>
        /// <param name="ligneAstreinte">Ligne astreinte</param>
        /// <param name="isPrimeNuit">is prime nuit</param>
        private void DeleteRapportLigneCodeAstreinte(RapportLigneAstreinteEnt ligneAstreinte, bool isPrimeNuit)
        {
            RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte =
                rapportLigneCodeAstreinteRepository.GetRapportLigneCodeAstreinteEnt(ligneAstreinte.RapportLigneAstreinteId, isPrimeNuit);
            if (rapportLigneCodeAstreinte != null)
            {
                DeleteRapportLigneCodeAstreinte(rapportLigneCodeAstreinte);
            }
        }

        /// <summary>
        /// Delete rapport ligne code astreinte
        /// </summary>
        /// <param name="rapportLigneCodeAstreinte">Rapport ligne code astreinte</param>
        private void DeleteRapportLigneCodeAstreinte(RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte)
        {
            rapportLigneCodeAstreinteRepository.Delete(rapportLigneCodeAstreinte);
            unitOfWork.Save();
        }

        /// <summary>
        /// Récupérer la liste des rapports d'un CI dans une semaine donnée
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <param name="personnelsIds">List des personnels Id</param>
        /// <returns>List des rapports</returns>
        public List<RapportEnt> GetCiRapportsByWeek(int ciId, DateTime dateDebut, DateTime dateFin, IEnumerable<int> personnelsIds = null)
        {
            List<RapportEnt> rapportLists;
            rapportLists = personnelsIds.IsNullOrEmpty()
                ? rapportRepository.GetCiRapportHebdomadaire(ciId, dateDebut, dateFin)
                : rapportRepository.GetCiRapportHebdomadaire(ciId, personnelsIds.Distinct().ToList(), dateDebut, dateFin);

            if (rapportLists.Count < 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    RapportEnt rapport = rapportLists.FirstOrDefault(r => (int)r.DateChantier.DayOfWeek == i);
                    if (rapport == null)
                    {
                        RapportEnt rapportToAdd = new RapportEnt
                        {
                            RapportId = 0,
                            CiId = ciId,
                            HoraireDebutM = DateTime.UtcNow.Date.AddHours(8),
                            HoraireFinM = DateTime.UtcNow.Date.AddHours(12)
                        };
                        if (i == 0)
                        {
                            rapportToAdd.DateChantier = dateFin;
                        }
                        else
                        {
                            rapportToAdd.DateChantier = dateDebut.AddDays(i - 1);
                        }

                        rapportLists.Add(rapportToAdd);
                    }
                }
            }
            return rapportLists.OrderBy(r => r.DateChantier).ToList();
        }

        /// <summary>
        /// Check Statut personnel
        /// </summary>
        /// <param name="rapportHebdoNodes"> Pointage view model</param>
        /// <returns>statut</returns>
        public string GetStatutPersonnelRapportHebdo(IEnumerable<RapportHebdoNode<PointageCell>> rapportHebdoNodes)
        {
            if (rapportHebdoNodes.FirstOrDefault().Statut == RapportStatutEnt.RapportStatutEnCours.Value || rapportHebdoNodes.FirstOrDefault().Statut == RapportStatutEnt.RapportStatutVerrouille.Value || rapportHebdoNodes.FirstOrDefault().Statut == RapportStatutEnt.RapportStatutValide2.Value)
            {
                if (rapportHebdoNodes.FirstOrDefault().SubNodeList != null && rapportHebdoNodes.FirstOrDefault().SubNodeList.Any())
                {
                    return rapportHebdoNodes.FirstOrDefault().SubNodeList.FirstOrDefault().Statut;
                }
            }
            return rapportHebdoNodes.FirstOrDefault().Statut;
        }

        /// <summary>
        /// Récupérer la liste des rapports d'un ensemble de CI dans une semaine donnée
        /// </summary>
        /// <param name="ciIds">CI concernés</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <returns>List des rapports</returns>
        public Dictionary<int, List<RapportEnt>> GetCiRapportsByWeek(IEnumerable<int> ciIds, DateTime dateDebut, string statut = null)
        {
            DateTime dateFin = dateDebut.AddDays(6);
            int typePersonnel = RapportStatutHelper.GetTypePersonnel(statut);
            Dictionary<int, List<RapportEnt>> groupes = rapportRepository.GetCiRapportHebdomadaire(ciIds, dateDebut, dateFin, typePersonnel);
            return HandleHebdoRapportsForEmployee(groupes, ciIds, dateDebut, dateFin, statut);
        }

        /// <summary>
        /// Handle Hebdo Rapports for employee
        /// </summary>
        /// <param name="groupes">Dictionnaire des Cis - Rapports</param>
        /// <param name="ciIds">List des CIs</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Dictionnaire</returns>
        public Dictionary<int, List<RapportEnt>> HandleHebdoRapportsForEmployee(Dictionary<int, List<RapportEnt>> groupes, IEnumerable<int> ciIds, DateTime dateDebut, DateTime dateFin, string statut = null)
        {
            var ret = new Dictionary<int, List<RapportEnt>>();
            foreach (var ciId in ciIds)
            {
                var groupe = groupes.FirstOrDefault(g => g.Key == ciId);
                var rapports = groupe.Value;
                if (statut == null)
                {
                    statut = Constantes.PersonnelStatutValue.Ouvrier;
                }
                if (rapports == null)
                {
                    rapports = new List<RapportEnt>(7);
                    for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                    {
                        rapports.Add(GetNewRapportFoRapportsByWeek(ciId, dayOfWeek, dateDebut, dateFin, statut));
                    }
                }
                else if (rapports.Count < 7)
                {
                    for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                    {
                        var rapport = rapports.FirstOrDefault(r => (int)r.DateChantier.DayOfWeek == dayOfWeek);
                        if (rapport == null)
                        {
                            rapports.Add(GetNewRapportFoRapportsByWeek(ciId, dayOfWeek, dateDebut, dateFin, statut));
                        }
                    }
                }

                rapports = rapports.OrderBy(r => r.DateChantier).ToList();
                ret.Add(ciId, rapports);
            }
            return ret;
        }

        private RapportEnt GetNewRapportFoRapportsByWeek(int ciId, int dayOfWeek, DateTime dateDebut, DateTime dateFin, string statut)
        {
            RapportEnt rapport = new RapportEnt();
            rapport.RapportId = 0;
            rapport.CiId = ciId;
            rapport.HoraireDebutM = DateTime.UtcNow.Date.AddHours(8);
            rapport.HoraireFinM = DateTime.UtcNow.Date.AddHours(12);
            rapport.DateChantier = dayOfWeek == 0 ? dateFin : dateDebut.AddDays(dayOfWeek - 1);
            rapport = RapportStatutHelper.CheckPersonnelStatut(rapport, statut);
            return rapport;
        }
    }
}
