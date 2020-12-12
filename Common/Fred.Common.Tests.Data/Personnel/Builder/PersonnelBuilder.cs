using Fred.Business.Referential.TypeRattachement;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;

namespace Fred.Common.Tests.Data.Personnel.Builder
{
    public class PersonnelBuilder : ModelDataTestBuilder<PersonnelEnt>
    {
        /// <summary>
        /// Obtient un personnel par défaut
        /// </summary>
        /// <returns>un personnel</returns>
        public PersonnelEnt Prototype()
        {
            Model.PersonnelId = 1;
            var societe = new SocieteBuilder();
            Model.Societe = societe.Prototype();
            return Model;
        }

        /// <summary>
        /// Obtient un personnel par défaut
        /// </summary>
        /// <returns>un personnel</returns>
        public PersonnelBuilder Prerequi()
        {
            New();
            Model.PersonnelId = 1;
            Model.Nom = "ABBES";
            Model.Prenom = "HAMID";
            Model.Matricule = "254879";
            var societe = new SocieteBuilder();
            Model.Societe = societe.Prototype();
            return this;
        }

        /// <summary>
        /// Fluent Champ PersonalId
        /// </summary>
        /// <param name="groupe">Personnel id à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder PersonnelId(int personnelId)
        {
            Model.PersonnelId = personnelId;
            return this;
        }

        /// <summary>
        /// Fluent Champ Utilisateur
        /// </summary>
        /// <param name="groupe">utilisateur à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder Utilisateur(UtilisateurEnt utilisateur)
        {
            Model.Utilisateur = utilisateur;
            return this;
        }

        /// <summary>
        /// Fluent Champ Societe
        /// </summary>
        /// <param name="groupe">societe à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder Societe(SocieteEnt societe)
        {
            Model.Societe = societe;
            return this;
        }

        /// <summary>
        /// Fluent Champ Statut
        /// </summary>
        /// <param name="groupe">societe à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder Statut(string status)
        {
            Model.Statut = status;
            return this;
        }

        /// <summary>
        /// Fluent Champ Statut
        /// </summary>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder IsInterimaire()
        {
            Model.IsInterimaire = true;
            return this;
        }

        /// <summary>
        /// Fluent foreignKey EtablissementRattachement
        /// </summary>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder EtablissementRattachement(EtablissementPaieEnt etab)
        {
            Model.EtablissementRattachement = etab;
            Model.EtablissementRattachementId = etab.EtablissementPaieId;
            return this;
        }

        /// <summary>
        /// Fluent foreignKey EtablissementPaie
        /// </summary>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder EtablissementPaie(EtablissementPaieEnt etab)
        {
            Model.EtablissementPaie = etab;
            Model.EtablissementPaieId = etab.EtablissementPaieId;
            return this;
        }

        /// <summary>
        /// Fluent foreignKey EtablissementPaie
        /// </summary>
        /// <returns>Builder pour fluent</returns>
        public PersonnelBuilder EtablissementPaieWithComptaDefault()
        {
            var etab = new EtablissementPaieBuilder().Prototype();
            Model.EtablissementPaie = etab;
            Model.EtablissementPaieId = etab.EtablissementPaieId;
            return this;
        }

        /// <summary>
        /// Permet de definir un personnel avec un rattachement domicile
        /// </summary>
        /// <returns>Builder pour fluent</returns>        
        public PersonnelBuilder TypeRattachementDomicile()
        {
            Model.TypeRattachement = TypeRattachement.Domicile;
            return this;
        }

        /// <summary>
        /// Permet de definir un personnel avec un rattachement établissement de rattachement
        /// </summary>
        /// <returns>Builder pour fluent</returns>        
        public PersonnelBuilder TypeRattachementSecteur()
        {
            Model.TypeRattachement = TypeRattachement.Secteur;
            return this;
        }

        /// <summary>
        /// Permet de definir un personnel avec un rattachement domicile
        /// </summary>
        /// <returns>Builder pour fluent</returns>        
        public PersonnelBuilder TypeRattachementAgence()
        {
            Model.TypeRattachement = TypeRattachement.Agence;
            return this;
        }

        /// <summary>
        /// Permet de géolocaliser à pessac
        /// </summary>
        /// <returns>fluent builder</returns>
        public PersonnelBuilder LocalisationPessac()
        {
            Model.LatitudeDomicile = 44.7516;
            Model.LongitudeDomicile = -0.6317;
            return this;
        }

        /// <summary>
        /// Permet de géolocaliser à Montpellier
        /// </summary>
        /// <returns>fluent builder</returns>
        public PersonnelBuilder LocalisationMontpellier()
        {
            Model.LatitudeDomicile = 43.5555;
            Model.LongitudeDomicile = 3.8782;
            return this;
        }
    }
}
