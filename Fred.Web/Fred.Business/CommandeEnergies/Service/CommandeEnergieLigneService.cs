using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Service Commande Energie
    /// </summary>
    public class CommandeEnergieLigneService : ICommandeEnergieLigneService
    {
        private readonly ICommandeEnergieMapperService commandeEnergieMapperService;
        private readonly ICommandeEnergieRepository commandeEnergieRepository;
        private readonly IReferentielEtenduRepository referentielEtenduRepository;
        private readonly ISepService sepService;

        /// <summary>
        /// Constructeur CommandeEnergieLigneService
        /// </summary>
        /// <param name="commandeEnergieMapperService">CommandeMannagerMapper</param>
        /// <param name="commandeEnergieRepository">Repository de commande Energie</param>
        /// <param name="referentielEtenduRepository">Repository référentiel Etendue</param>
        /// <param name="sepService">Service SEP</param>
        public CommandeEnergieLigneService(ICommandeEnergieMapperService commandeEnergieMapperService, ICommandeEnergieRepository commandeEnergieRepository,
            IReferentielEtenduRepository referentielEtenduRepository,
            ISepService sepService)
        {
            this.commandeEnergieMapperService = commandeEnergieMapperService;
            this.commandeEnergieRepository = commandeEnergieRepository;
            this.referentielEtenduRepository = referentielEtenduRepository;
            this.sepService = sepService;
        }

        /// <summary>
        /// Génération des lignes de commande énergie pour un commande énergie en fonction du type d'énergie
        /// </summary>
        /// <param name="typeEnergie">Type d'énergie</param>
        /// <param name="ciId">Identifiant du CI SEP</param>
        /// <param name="periode">Période sélectionnée</param>        
        /// <param name="tache">Tache par défaut : 999998</param>
        /// <param name="unite">Unite par défaut : H</param>
        /// <param name="societeParticipanteIds">Liste d'identifiants de sociétés participantes à la SEP</param>
        /// <returns>Liste de lignes de commande</returns>
        public List<CommandeEnergieLigne> GetGeneratedCommandeEnergieLignes(TypeEnergieEnt typeEnergie, int ciId, DateTime periode, TacheEnt tache, UniteEnt unite, List<int> societeParticipanteIds)
        {
            List<CommandeEnergieLigne> listcommandeEnergies = new List<CommandeEnergieLigne>();
            switch (typeEnergie.Code)
            {
                case Constantes.TypeEnergie.Personnel: listcommandeEnergies = GenerateCommandeEnergieLignePersonnel(ciId, periode, tache, unite, societeParticipanteIds); break;
                case Constantes.TypeEnergie.Materiel: listcommandeEnergies = GenerateCommandeEnergieLigneMateriel(ciId, periode, tache, unite, societeParticipanteIds); break;
                case Constantes.TypeEnergie.Interimaire: listcommandeEnergies = GenerateCommandeEnergieLigneInterimaire(ciId, periode, tache, unite, societeParticipanteIds); break;
                default: return listcommandeEnergies;
            }
            return listcommandeEnergies;
        }

        /// <summary>
        /// Champs calculés
        /// </summary>
        /// <param name="typeEnergie">Type d'énergie</param>
        /// <param name="commandeEnergieLignes">Ligne Commande énergie en BD</param>
        /// <param name="generatedCommandeEnergieLignes">Liste des lignes énergie générés en fonction des pointages</param>
        public void ComputeCalculatedFields(TypeEnergieEnt typeEnergie, List<CommandeEnergieLigne> commandeEnergieLignes, List<CommandeEnergieLigne> generatedCommandeEnergieLignes)
        {
            // Renseignement des champs calculés non enregistrés des lignes commandes énergie
            foreach (CommandeEnergieLigne ligneEnergie in commandeEnergieLignes.Where(x => x.PersonnelId.HasValue || x.MaterielId.HasValue))
            {
                ligneEnergie.MontantHT = ligneEnergie.PUHT * ligneEnergie.Quantite.Value;

                if (generatedCommandeEnergieLignes.Count > 0)
                {
                    CommandeEnergieLigne generatedCommandeEnergieLigne = generatedCommandeEnergieLignes.First(GetPredicat(typeEnergie, ligneEnergie));
                    ligneEnergie.Bareme = generatedCommandeEnergieLigne?.Bareme ?? 0;
                    ligneEnergie.UniteBareme = generatedCommandeEnergieLigne?.UniteBareme;
                    ligneEnergie.UniteBaremeId = generatedCommandeEnergieLigne?.UniteBaremeId ?? 0;
                    ligneEnergie.QuantitePointee = generatedCommandeEnergieLigne?.QuantitePointee ?? 0;
                    ligneEnergie.QuantiteConvertie = generatedCommandeEnergieLigne?.QuantiteConvertie ?? 0;
                    ligneEnergie.MontantValorise = generatedCommandeEnergieLigne?.MontantValorise ?? 0;

                    // Champs calculés
                    ligneEnergie.EcartPu = ligneEnergie.PUHT - ligneEnergie.Bareme.Value;
                    ligneEnergie.EcartQuantite = ligneEnergie.Quantite.Value - ligneEnergie.QuantiteConvertie.Value;
                    ligneEnergie.EcartMontant = ligneEnergie.MontantHT - ligneEnergie.MontantValorise;
                }
            }
        }

        private List<CommandeEnergieLigne> GenerateCommandeEnergieLigneInterimaire(int ciId, DateTime periode, TacheEnt tache, UniteEnt unite, List<int> societeParticipanteIds)
        {
            List<CommandeEnergieLigne> commandeEnergieLigneList = new List<CommandeEnergieLigne>();
            List<RapportLigneEnt> rapportLigneList = commandeEnergieRepository.GetPointagesInterimaires(x => x.DatePointage.Year == periode.Year
                                                                      && x.DatePointage.Month == periode.Month
                                                                      && x.CiId == ciId
                                                                      && !x.DateSuppression.HasValue
                                                                      && x.ContratId.HasValue
                                                                      && x.Contrat.ContratInterimaireId == x.ContratId && societeParticipanteIds.Contains(x.Contrat.SocieteId.Value)).ToList();
            var pointages = rapportLigneList?.GroupBy(x => x.Contrat).Select(x => new
            {
                Contrat = x.Key,
                x.Key.Ressource,
                Personnel = x.Key.Interimaire,
                x.Key.Unite,
                Quantite = x.Sum(s => s.HeureNormale)

            }).ToList();
            if (pointages != null && pointages.Any())
            {
                commandeEnergieLigneList = pointages.Select(x => new CommandeEnergieLigne
                {
                    Libelle = $"{x.Contrat.Societe.Code} - {x.Personnel.Matricule} - {x.Personnel.Nom}  {x.Personnel.Prenom} ({x.Contrat.NumContrat})",
                    TacheId = tache.TacheId,
                    RessourceId = x.Ressource.RessourceId,
                    Ressource = x.Ressource,
                    UniteId = unite.UniteId,
                    Unite = unite,
                    PersonnelId = x.Contrat.Interimaire.PersonnelId,
                    //bareme
                    UniteBaremeId = x.Contrat.Unite.UniteId,
                    UniteBareme = x.Contrat.Unite,
                    Bareme = x.Contrat.Valorisation,

                    QuantitePointee = (decimal)x.Quantite,
                    QuantiteConvertie = (decimal)x.Quantite,

                    PUHT = x.Contrat.Valorisation,

                    // Ou Quantité Ajustée
                    Quantite = (decimal)x.Quantite,

                    MontantHT = (decimal)x.Quantite * x.Contrat.Valorisation,

                    MontantValorise = (decimal)x.Quantite * x.Contrat.Valorisation,
                }).ToList();
            }

            return commandeEnergieLigneList;
        }

        /// <summary>
        /// Génération des lignes de commande énergie pour un commande énergie de type Matériel
        /// </summary>
        /// <param name="ciId">Identifiant du CI SEP</param>
        /// <param name="periode">Période sélectionnée</param>        
        /// <param name="tache">Tache par défaut : 999998</param>
        /// <param name="unite">Unite par défaut : H</param>       
        /// <param name="societeParticipanteIds">Liste des sociétés participantes</param>
        /// <returns>Liste de lignes de commande</returns>
        private List<CommandeEnergieLigne> GenerateCommandeEnergieLigneMateriel(int ciId, DateTime periode, TacheEnt tache, UniteEnt unite, List<int> societeParticipanteIds)
        {
            List<CommandeEnergieLigne> commandeEnergieLigneList = new List<CommandeEnergieLigne>();
            int societeId = sepService.GetSocieteGeranteForSep(ciId).SocieteId;
            List<RapportLigneEnt> rapportLigneList = commandeEnergieRepository.GetPointagesMateriels(x => x.DatePointage.Year == periode.Year
                                                                       && x.DatePointage.Month == periode.Month
                                                                       && x.CiId == ciId
                                                                       && !x.Rapport.DateSuppression.HasValue
                                                                       && x.MaterielId.HasValue
                                                                       && x.MaterielMarche > 0
                                                                       && !x.Materiel.IsLocation // Matériel interne
                                                                       && (societeParticipanteIds.Count == 0 || societeParticipanteIds.Contains(x.Materiel.SocieteId))
                                                                       && !x.DateSuppression.HasValue).ToList();
            List<PointageMaterielCommandeEnergie> pointages = rapportLigneList?.GroupBy(x => x.Materiel)
                                          .Select(x => new PointageMaterielCommandeEnergie
                                          {
                                              Materiel = x.Key,
                                              Ressource = x.Key.Ressource,
                                              SocieteMatriel = x.Key.Societe,
                                              ReferentielEtendu = x.Key.Ressource.ReferentielEtendus.FirstOrDefault(y => y.SocieteId == societeId && y.RessourceId == x.Key.RessourceId),
                                              QuantitePointee = x.Sum(y => y.MaterielMarche)
                                          })
                                          .ToList();
            if (pointages != null && pointages.Any())
            {
                List<int> materielIds = pointages.Select(x => x.Materiel.MaterielId).Distinct().ToList();
                List<int> ressourceIds = pointages.Select(x => x.Materiel.RessourceId).Distinct().ToList();
                List<int> refEtendus = referentielEtenduRepository.Query()
                                                                  .Filter(x => ressourceIds.Contains(x.RessourceId) && societeId == x.SocieteId)
                                                                  .Get()
                                                                  .Select(l => l.ReferentielEtenduId)
                                                                  .ToList();
                CIEnt ci = commandeEnergieRepository.GetCi(ciId, periode, personnelIds: null, materielIds: materielIds, referentielEtenduIds: refEtendus);
                commandeEnergieLigneList = commandeEnergieMapperService.RapportLigneMaterielToCommandeEnergieLigne(pointages, ci, tache, unite);
            }

            return commandeEnergieLigneList;
        }

        /// <summary>
        /// Génération des lignes de commande énergie pour un commande énergie de type Personnel
        /// </summary>
        /// <param name="ciId">Identifiant du CI SEP</param>
        /// <param name="periode">Période sélectionnée</param>        
        /// <param name="tache">Tache par défaut : 999998</param>
        /// <param name="unite">Unite par défaut : H</param>        
        /// <param name="societeParticipanteIds">Liste des sociétés participantes</param>
        /// <returns>Liste de lignes de commande</returns>
        private List<CommandeEnergieLigne> GenerateCommandeEnergieLignePersonnel(int ciId, DateTime periode, TacheEnt tache, UniteEnt unite, List<int> societeParticipanteIds)
        {
            List<CommandeEnergieLigne> commandeEnergieLigneList = new List<CommandeEnergieLigne>();
            int societeId = sepService.GetSocieteGeranteForSep(ciId).SocieteId;
            List<RapportLigneEnt> rapportLigneList = commandeEnergieRepository.GetPointagesPersonnels(x => x.DatePointage.Year == periode.Year
                                                                      && x.DatePointage.Month == periode.Month
                                                                      && x.CiId == ciId
                                                                      && !x.Rapport.DateSuppression.HasValue
                                                                      && x.PersonnelId.HasValue
                                                                      && (societeParticipanteIds.Count == 0 || societeParticipanteIds.Contains(x.Personnel.SocieteId.Value))
                                                                      && !x.Personnel.IsInterimaire
                                                                      && !x.DateSuppression.HasValue).ToList();
            List<PointagePersonnelCommandeEnergie> pointages = rapportLigneList?.GroupBy(x => x.Personnel)
                                                    .Select(x => new PointagePersonnelCommandeEnergie
                                                    {
                                                        Personnel = x.Key,
                                                        Ressource = x.Key.Ressource,
                                                        SocietePerso = x.Key.Societe,
                                                        ReferentielEtendu = x.Key.Ressource.ReferentielEtendus.FirstOrDefault(y => y.SocieteId == societeId && y.RessourceId == x.Key.RessourceId),
                                                        QuantitePointee = x.Sum(y => y.HeureNormale)
                                                    })
                                                    .ToList();
            if (pointages != null && pointages.Any())
            {
                List<int> persoIds = pointages.Select(x => x.Personnel.PersonnelId).Distinct().ToList();
                List<int> ressourceIds = pointages.Where(x => x.Personnel.RessourceId.HasValue).Select(x => x.Personnel.RessourceId.Value).Distinct().ToList();
                List<int> refEtendus = referentielEtenduRepository.Query()
                                                                  .Filter(x => ressourceIds.Contains(x.RessourceId) && societeId == x.SocieteId)
                                                                  .Get()
                                                                  .Select(l => l.ReferentielEtenduId)
                                                                  .ToList();

                CIEnt ci = commandeEnergieRepository.GetCi(ciId, periode, personnelIds: persoIds, materielIds: null, referentielEtenduIds: refEtendus);
                commandeEnergieLigneList = commandeEnergieMapperService.RapportLignePersonnelToCommandeEnergieLigne(pointages, ci, tache, unite);
            }

            return commandeEnergieLigneList;
        }

        /// <summary>
        /// Récupération du prédicat de recherche d'une ligne de commande énergie générée
        /// </summary>
        /// <param name="typeEnergie">Type energie</param>
        /// <param name="ligneEnergie">Commande Energie Ligne</param>
        /// <returns>Predicat</returns>
        private Func<CommandeEnergieLigne, bool> GetPredicat(TypeEnergieEnt typeEnergie, CommandeEnergieLigne ligneEnergie)
        {
            if (typeEnergie.Code == Constantes.TypeEnergie.Personnel)
            {
                return x => x.PersonnelId.HasValue && x.PersonnelId == ligneEnergie.PersonnelId;
            }
            else if (typeEnergie.Code == Constantes.TypeEnergie.Materiel)
            {
                return x => x.MaterielId.HasValue && x.MaterielId == ligneEnergie.MaterielId;
            }
            else
            {
                throw new FredBusinessException(string.Format(FeatureCommandeEnergie.Notification_Type_Energie_Non_Gere, typeEnergie.Code));
            }
        }
    }
}
