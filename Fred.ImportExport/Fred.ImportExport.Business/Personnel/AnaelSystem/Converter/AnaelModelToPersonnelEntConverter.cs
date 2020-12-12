using System.Collections.Generic;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Entities.Transposition;
using Fred.ImportExport.Models.Personnel;
using Fred.Web.Shared.Extentions;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Converter
{
    public class AnaelModelToPersonnelEntConverter
    {
        private readonly IEmailGeneratorService emailGeneratorService;

        public AnaelModelToPersonnelEntConverter(IEmailGeneratorService emailGeneratorService)
        {
            this.emailGeneratorService = emailGeneratorService;
        }

        /// <summary>
        /// Convertie une liste de model Anael en entité fred
        /// </summary>
        /// <param name="allRessources">liste des ressources</param>
        /// <param name="societeContext">context de la societes</param>
        /// <param name="pays">List des pays</param>
        /// <returns>Liste d'entites fred</returns>
        public List<PersonnelEnt> ConvertToEnts(List<RessourceEnt> allRessources, ImportPersonnelSocieteContext societeContext, IEnumerable<PaysEnt> pays)
        {
            var result = new List<PersonnelEnt>();

            var personnelAnaelModels = societeContext.AnaelPersonnels;
            var transpoCodeEmploiRessourceList = societeContext.TranspoCodeEmploiRessourceList;
            var etablissementsPayes = societeContext.EtablissementsPaies;

            foreach (var personnelAnaelModel in personnelAnaelModels)
            {
                result.Add(ConvertToEnt(personnelAnaelModel, transpoCodeEmploiRessourceList, allRessources, etablissementsPayes, societeContext.Societe, pays));
            }

            return result;
        }


        private PersonnelEnt ConvertToEnt(PersonnelAnaelModel personnelAnaelModel,
          List<TranspoCodeEmploiToRessourceEnt> transpoCodeEmploiRessourceList,
          List<RessourceEnt> allRessources,
          List<EtablissementPaieEnt> etablissementsPayes,
          SocieteEnt societe,
          IEnumerable<PaysEnt> pays)
        {
            TranspoCodeEmploiToRessourceEnt transpo = transpoCodeEmploiRessourceList.Find(x => x.CodeEmploi?.Trim() == personnelAnaelModel?.CodeEmploi?.Trim());
            RessourceEnt ressource = allRessources.FirstOrDefault(x => x.Code == transpo?.CodeRessource?.Trim());
            int? ressourceId = ressource?.RessourceId;
            var etablissement = etablissementsPayes.FirstOrDefault(x => x.Code.Trim() == personnelAnaelModel.CodeEtablissement?.Trim());
            int? etablissementPaieId = etablissement?.EtablissementPaieId;

            var personnelEnt = new PersonnelEnt
            {
                EtablissementPaieId = etablissementPaieId,
                RessourceId = ressourceId,
                Matricule = personnelAnaelModel.Matricule,
                Nom = personnelAnaelModel.Nom,
                Prenom = personnelAnaelModel.Prenom,
                CategoriePerso = personnelAnaelModel.CategoriePerso,
                Statut = personnelAnaelModel.Statut,
                DateEntree = personnelAnaelModel.DateEntree.Value,
                DateSortie = personnelAnaelModel.DateSortie,
                Adresse1 = personnelAnaelModel.NumeroRue + " " + personnelAnaelModel.TypeRue + " " + personnelAnaelModel.NomRue,
                CodePostal = personnelAnaelModel.CodePostal,
                Ville = personnelAnaelModel.Ville,
                DateModification = personnelAnaelModel.DateModification,
                Email = emailGeneratorService.GenerateEmail(personnelAnaelModel.Nom, personnelAnaelModel.Prenom, societe.CodeSocietePaye, societe.SocieteId),
                IsInterimaire = false, // False par défaut
                CodeEmploi = personnelAnaelModel.CodeEmploi
            };

            ManageStatutAndCategoriePersonnel(personnelEnt, societe.CodeSocietePaye);
            ManagePersonnelPays(personnelAnaelModel, ref personnelEnt, pays);

            return personnelEnt;
        }

        // ATTENTION PROPRE A RZB
        private void ManageStatutAndCategoriePersonnel(PersonnelEnt personnel, string codeSocietePaye)
        {
            const string etam = "2";

            //RB_1410_2002
            // Transposition du Statut du personnel
            if (codeSocietePaye == "RZB")
            {
                if (personnel.Statut == Constantes.TypePersonnel.Ouvrier || personnel.Statut == etam || personnel.Statut == Constantes.TypePersonnel.Cadre)
                {
                    AdaptPersonnelStatus(personnel, etam);
                }
                else
                {
                    personnel.Statut = string.Empty;
                    personnel.CategoriePerso = string.Empty;
                }
            }
            else if (codeSocietePaye == "MTP" &&
                    (personnel.Statut == Constantes.TypePersonnel.Ouvrier || personnel.Statut == etam || personnel.Statut == Constantes.TypePersonnel.Cadre))
            {
                AdaptPersonnelStatus(personnel, etam);
            }
        }

        private void AdaptPersonnelStatus(PersonnelEnt personnel, string etam)
        {
            if (personnel.Statut == etam)
            {
                if (personnel.CategoriePerso == "B")
                {
                    personnel.Statut = Constantes.TypePersonnel.ETAMBureau;
                    personnel.CategoriePerso = Constantes.TypePersonnel.Horaire;
                }
                else if (personnel.CategoriePerso == "F")
                {
                    personnel.Statut = Constantes.TypePersonnel.ETAMArticle36;
                    personnel.CategoriePerso = Constantes.TypePersonnel.Mensuel;
                }
                else
                {
                    personnel.Statut = Constantes.TypePersonnel.ETAMChantier;
                    personnel.CategoriePerso = Constantes.TypePersonnel.Horaire;
                }
            }
            else if (personnel.Statut == Constantes.TypePersonnel.Ouvrier)
            {
                personnel.CategoriePerso = Constantes.TypePersonnel.Horaire;
            }
            else if (personnel.Statut == Constantes.TypePersonnel.Cadre)
            {
                personnel.CategoriePerso = Constantes.TypePersonnel.Mensuel;
            }
        }

        private void ManagePersonnelPays(PersonnelAnaelModel personnelAnaelModel, ref PersonnelEnt personnel, IEnumerable<PaysEnt> paysList)
        {
            if (!paysList.IsNullOrEmpty())
            {
                PaysEnt pays = paysList.FirstOrDefault(x => x.Code == personnelAnaelModel.CodePays);
                if (pays != null)
                {
                    personnel.PaysId = pays.PaysId;
                    personnel.PaysLabel = pays.Libelle;
                }
            }
        }
    }
}
