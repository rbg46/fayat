using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Fred.Business.Referential;
using Fred.Business.Referential.Materiel;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.CI.ImportMaterielEtl.Result;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Models.Materiel;

namespace Fred.ImportExport.Business.CI.ImportMaterielEtl.Transform
{
    /// <summary>
    /// Processus etl : Transformation du resultat de la requête Anael
    /// </summary>
    public class ImportMaterielTransform : IEtlTransform<ImportMaterielModel, MaterielEnt>
    {
        private readonly string logLocation = "[FLUX MATERIEL][IMPORT DANS FRED][TRANSFORM]";

        private readonly Lazy<IEtablissementComptableManager> etablissementComptableManager = new Lazy<IEtablissementComptableManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IEtablissementComptableManager>();
        });

        private readonly Lazy<ISocieteManager> societeManager = new Lazy<ISocieteManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<ISocieteManager>();
        });
        private readonly Lazy<IReferentielFixeManager> referentielFixeManager = new Lazy<IReferentielFixeManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IReferentielFixeManager>();
        });
        private readonly Lazy<IFournisseurManager> fournisseurManager = new Lazy<IFournisseurManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IFournisseurManager>();
        });
        private readonly Lazy<IUtilisateurManager> utilisateurManager = new Lazy<IUtilisateurManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IUtilisateurManager>();
        });

        private readonly Lazy<IMaterielManager> materielManager = new Lazy<IMaterielManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IMaterielManager>();
        });
        private readonly EtlExecutionLogger etlExecutionLogger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="etlExecutionLogger">etlExecutionLogger</param>  
        public ImportMaterielTransform(EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        public void Execute(IEtlInput<ImportMaterielModel> input, ref IEtlResult<MaterielEnt> result)
        {
            if (result == null)
            {
                result = new ImportMaterielResult();
            }

            List<EtablissementComptableEnt> etablissementComptables = etablissementComptableManager.Value.GetEtablissementComptableList().ToList();// FILTRE SUR LA SOCIETE OK, voir plus bas
            List<SocieteEnt> societes = societeManager.Value.GetSocieteListAll().ToList();

            foreach (var item in input.Items)
            {
                var model = GetModelForAddOrUpdate(item, etablissementComptables, societes);
                result.Items.Add(model);
            }
        }

        private MaterielEnt GetModelForAddOrUpdate(ImportMaterielModel materielModel, List<EtablissementComptableEnt> etablissementComptables, List<SocieteEnt> societes)
        {
            //On récupère la société.
            SocieteEnt societe = GetSocieteOrThrowErrorIfNull(materielModel, societes);

            //On récupère l'établissement comptable.
            EtablissementComptableEnt etablissementComptable = GetEtablissementComptableOrThrowErrorIfNull(materielModel, etablissementComptables, societe);

            //On récupère le founisseur.
            FournisseurEnt fournisseur = GetFournisseurOrThrowErrorIfNull(materielModel, societe);

            //On récupère la ressource.
            RessourceEnt ressource = GetRessourceOrThrowErrorIfNull(materielModel, societe);

            // Récuperation de l'utilisateur fred IE
            var fredIeUtilisateur = GetFredIeUtilisateur();

            // Je recupere la materiel de la base de donnée car sinon, je peux ecrasser des données qui n'ont pas étées envoyé dans le flux !!!
            // cad : champ dans le model mais pas dans le flux.
            IEnumerable<MaterielEnt> materielsDb = GetMaterielFromDatabase(materielModel, societe);


            var materielDb = materielsDb.FirstOrDefault();

            MaterielEnt materiel = null;

            if (materielDb != null)
            {
                //Mise a jour
                materiel = materielDb;
            }
            else
            {
                //Création
                materiel = new MaterielEnt();
            }

            materiel.Code = materielModel.Code;
            materiel.Libelle = materielModel.Libelle;
            materiel.Actif = materielModel.Actif;
            materiel.DateCreation = materielModel.DateCreation;
            materiel.DateModification = materielModel.DateModification;
            materiel.DateSuppression = materielModel.DateSuppression;
            materiel.MaterielLocation = materielModel.MaterielLocation;
            materiel.DateDebutLocation = materielModel.DateDebutLocation;
            materiel.DateFinLocation = materielModel.DateFinLocation;
            materiel.ClasseFamilleCode = materielModel.ClasseFamilleCode;
            materiel.ClasseFamilleLibelle = materielModel.ClasseFamilleLibelle;
            materiel.IsStorm = materielModel.IsStorm;
            materiel.Fabriquant = materielModel.Fabriquant;
            materiel.VIN = materielModel.VIN;
            materiel.DateMiseEnService = materielModel.DateMiseEnService;
            materiel.Immatriculation = materielModel.Immatriculation;
            materiel.DimensionH = materielModel.DimensionH;
            materiel.DimensionL = materielModel.DimensionLa;
            materiel.Dimensiionl = materielModel.DimensiionLo;
            materiel.UniteDimension = materielModel.UniteDimension;
            materiel.UnitePuissance = materielModel.UnitePuissance;

            materiel.AuteurCreationId = fredIeUtilisateur.UtilisateurId;
            materiel.AuteurModificationId = fredIeUtilisateur.UtilisateurId;
            materiel.SocieteId = societe.SocieteId;
            materiel.FournisseurId = fournisseur.FournisseurId;
            materiel.RessourceId = ressource.RessourceId;
            materiel.EtablissementComptableId = etablissementComptable?.EtablissementComptableId;

            return materiel;

        }

        private SocieteEnt GetSocieteOrThrowErrorIfNull(ImportMaterielModel materielModel, List<SocieteEnt> societes)
        {
            SocieteEnt societe = societes.FirstOrDefault(x => x.CodeSocieteComptable == materielModel.SocieteCode);

            if (societe == null)
            {
                throw new FredBusinessException($"[MATERIEL] Import Materiel {materielModel.Code} : Aucune société pour le CodeSocieteComptable ({materielModel.SocieteCode})");
            }

            return societe;
        }


        private EtablissementComptableEnt GetEtablissementComptableOrThrowErrorIfNull(ImportMaterielModel materielModel, List<EtablissementComptableEnt> etablissementComptables, SocieteEnt societe)
        {
            EtablissementComptableEnt etablissementComptable = etablissementComptables.FirstOrDefault(x => x.Code == materielModel.EtablissementCode && x.SocieteId == societe.SocieteId);
            if (etablissementComptable == null)
            {
                throw new FredBusinessException($"[MATERIEL] Import Materiel {materielModel.Code} : Aucun etablissementComptable ({materielModel.EtablissementCode}) pour la société ({materielModel.SocieteCode})");
            }

            return etablissementComptable;
        }

        private IEnumerable<MaterielEnt> GetMaterielFromDatabase(ImportMaterielModel materielModel, SocieteEnt societe)
        {
            var materielsDb = materielManager.Value.GetMateriels(societe.SocieteId, x => x.Code == materielModel.Code);
            etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Materiel de la base de donnée avec le code ({materielModel.Code}) et la societe a(CodeSocieteComptable={societe.CodeSocieteComptable})", materielsDb);
            if (materielsDb.Count() > 1)
            {
                throw new FredBusinessException($"[MATERIEL] Import Materiel : Plusieurs Materiels ont été trouvés pour le code ({materielModel.Code}) et pour le SocieteCode ({materielModel.SocieteCode}).");
            }

            return materielsDb;
        }

        private RessourceEnt GetRessourceOrThrowErrorIfNull(ImportMaterielModel materielModel, SocieteEnt societe)
        {
            var ressources = referentielFixeManager.Value.GetRessourceListByGroupeId(societe.GroupeId);

            RessourceEnt ressource = ressources.FirstOrDefault(x => x.Code == materielModel.RessourceCode);

            if (ressource == null)
            {
                throw new FredBusinessException($"[MATERIEL] Import Materiel : Aucune ressource pour le code ({materielModel.RessourceCode}) et pour la société ({societe.Code})({societe.CodeSocieteComptable}) ");
            }

            return ressource;
        }

        private FournisseurEnt GetFournisseurOrThrowErrorIfNull(ImportMaterielModel materielModel, SocieteEnt societe)
        {
            var fournisseursInGroupe = fournisseurManager.Value.GetFournisseurList(societe.GroupeId).ToList();

            var fournisseur = fournisseursInGroupe.FirstOrDefault(f => f.Code == materielModel.FournisseurCode);

            if (fournisseur == null)
            {
                throw new FredBusinessException($"[MATERIEL] Import Materiel : Aucun fournisseur pour le code ({materielModel.FournisseurCode}) pour la société ({societe.Code})({societe.CodeSocieteComptable})");
            }

            return fournisseur;
        }



        /// <summary>
        /// Retourne l'utilisateur FRED IE.
        /// </summary>
        /// <returns>L'utilisateur FRED IE.</returns>
        private UtilisateurEnt GetFredIeUtilisateur()
        {
            return utilisateurManager.Value.GetByLogin("fred_ie");
        }
    }
}
