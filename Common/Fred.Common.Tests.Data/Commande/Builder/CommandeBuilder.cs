using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeBuilder : ModelDataTestBuilder<CommandeEnt>
    {
        public CommandeStatutBuilder Statut => new CommandeStatutBuilder(Model);
        public CommandeTypeBuilder Type => new CommandeTypeBuilder(Model);

        public override CommandeEnt New()
        {
            Model = new CommandeEnt
            {
                CiId = 1,
                TypeId = 1,
                Date = new DateTime(2016, 1, 1),
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "2200001",
                AuteurCreationId = 1,
                FraisAmortissement = false,
                FraisAssurance = false,
                Carburant = false,
                Lubrifiant = false,
                EntretienJournalier = false,
                EntretienMecanique = false,
                MOConduite = false,
                CommandeManuelle = false,
                ContactId = 1,
                SuiviId = 1,
                IsAbonnement = false,
                LivraisonAdresse = "blabla",
                LivraisonVille = "blabla",
                LivraisonCPostale = "34000",
                Contact = new Entities.Personnel.PersonnelEnt() { },
                FacturationAdresse = "blabla",
                FacturationVille = "blabla",
                FacturationCPostale = "34000",
                Lignes = new List<CommandeLigneEnt>(),
                AccordCadre = false,
                DeviseId = 48 // €
            };
            return Model;
        }

        /// <summary>
        /// A ne pas modifier !
        /// </summary>
        /// <returns></returns>
        public CommandeBuilder ParDefaut()
        {
            Model.CommandeId = 1;
            Model.CommentaireFournisseur = "commentaire";
            Model.CommentaireInterne = "commentaire";
            Model.DelaiLivraison = "delai";
            Model.IsAbonnement = true;
            Model.DureeAbonnement = 0;
            Model.DateProchaineReception = new DateTime(2019, 12, 02);
            Model.DatePremiereReception = new DateTime(2019, 12, 02);
            Model.FournisseurAdresse = "adresse";
            Model.FournisseurCPostal = "cp";
            Model.FournisseurVille = "ville";
            Model.FournisseurPaysId = 0;
            return this;
        }

        public CommandeBuilder ParDefautWithLignes()
        {
            Model = ParDefaut().Build();
            return AddLigne(new CommandeLigneBuilder().ParDefaut().Build());
        }

        public CommandeBuilder MinimumInfoWithLignes()
        {
            Model = ParDefaut().Build();
            return AddLigne(new CommandeLigneBuilder().MinimumInfo().Build());
        }

        public CommandeBuilder AddLigne(CommandeLigneEnt ligne)
        {
            Model.Lignes.Add(ligne);
            return this;
        }

        public CommandeBuilder AddLignes(int nbLignes)
        {
            AddLignes(new CommandeLigneGenerator().Generate(nbLignes));
            return this;
        }

        public CommandeBuilder AddLignes(IEnumerable<CommandeLigneEnt> lignes)
        {
            foreach (var item in lignes)
            {
                Model.Lignes.Add(item);
            }

            return this;
        }

        public CommandeBuilder Numero(string numero)
        {
            Model.Numero = numero;
            return this;
        }

        public CommandeBuilder NumeroExterne(string numero)
        {
            Model.NumeroCommandeExterne = numero;
            return this;
        }

        public CommandeBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public CommandeBuilder FacturationCodePostal(string cp)
        {
            Model.FacturationCPostale = cp;
            return this;
        }

        public CommandeBuilder FacturationVille(string ville)
        {
            Model.FacturationVille = ville;
            return this;
        }

        public CommandeBuilder FacturationAdresse(string adresse)
        {
            Model.FacturationAdresse = adresse;
            return this;
        }

        public CommandeBuilder LivraisonCodePostal(string cp)
        {
            Model.LivraisonCPostale = cp;
            return this;
        }

        public CommandeBuilder LivraisonVille(string ville)
        {
            Model.LivraisonVille = ville;
            return this;
        }

        public CommandeBuilder LivraisonAdresse(string adresse)
        {
            Model.LivraisonAdresse = adresse;
            return this;
        }

        public CommandeBuilder Date(DateTime datetime)
        {
            Model.Date = datetime;
            return this;
        }

        public CommandeBuilder DateMiseADispo(DateTime? datetime)
        {
            Model.DateMiseADispo = datetime;
            return this;
        }

        public CommandeBuilder DateProchaineReception(DateTime? datetime)
        {
            Model.DateProchaineReception = datetime;
            return this;
        }

        public CommandeBuilder DatePremiereReception(DateTime? datetime)
        {
            Model.DatePremiereReception = datetime;
            return this;
        }

        public CommandeBuilder Abonnement()
        {
            Model.IsAbonnement = true;
            return this;
        }

        public CommandeBuilder FrequenceAbonnement(int? frequence)
        {
            Model.FrequenceAbonnement = frequence;
            return this;
        }

        public CommandeBuilder DureeAbonnement(int? duree)
        {
            Model.DureeAbonnement = duree;
            return this;
        }

        public CommandeBuilder Manuelle()
        {
            Model.CommandeManuelle = true;
            return this;
        }

        public CommandeBuilder AccordCadre()
        {
            Model.AccordCadre = true;
            return this;
        }

        public CommandeBuilder Justificatif(string justificatif)
        {
            Model.Justificatif = justificatif;
            return this;
        }

        public CommandeBuilder Contact(PersonnelEnt personnel)
        {
            Model.Contact = personnel;
            Model.ContactId = personnel.PersonnelId;
            return this;
        }

        public CommandeBuilder Suivi(PersonnelEnt personnel)
        {
            Model.Suivi = personnel;
            Model.SuiviId = personnel.PersonnelId;
            return this;
        }

        public CommandeBuilder Fournisseur(FournisseurEnt fournisseur)
        {
            Model.Fournisseur = fournisseur;
            Model.FournisseurId = fournisseur.FournisseurId;
            return this;
        }

        public CommandeBuilder Devise(DeviseEnt devise)
        {
            Model.Devise = devise;
            Model.DeviseId = devise.DeviseId;
            return this;
        }

        public CommandeBuilder DateSuppression(DateTime? date)
        {
            Model.DateSuppression = date;
            return this;
        }

        public CommandeBuilder IsEnergie(bool isEnergie)
        {
            Model.IsEnergie = isEnergie;
            return this;
        }

        public CommandeBuilder Agence(AgenceEnt agence)
        {
            Model.Agence = agence;
            Model.AgenceId = agence.AgenceId;
            return this;
        }

        public CommandeBuilder FournisseurAdresse(string fournisseurAdresse)
        {
            Model.FournisseurAdresse = fournisseurAdresse;
            return this;
        }

        public CommandeBuilder FournisseurCPostal(string fournisseurCPostal)
        {
            Model.FournisseurCPostal = fournisseurCPostal;
            return this;
        }

        public CommandeBuilder FournisseurVille(string fournisseurVille)
        {
            Model.FournisseurVille = fournisseurVille;
            return this;
        }

        public CommandeBuilder FournisseurPaysId(int? id)
        {
            Model.FournisseurPaysId = id;
            return this;
        }

        public CommandeBuilder DelaiLivraison(string delai)
        {
            Model.DelaiLivraison = delai;
            return this;
        }

        public CommandeBuilder CI(CIEnt ci)
        {
            Model.CI = ci;
            Model.CiId = ci.CiId;
            return this;
        }

        public CommandeBuilder TypeId(int? id)
        {
            Model.TypeId = id;
            return this;
        }

        public CommandeBuilder CiId(int? id)
        {
            Model.CiId = id;
            return this;
        }

        public CommandeBuilder DeviseId(int? id)
        {
            Model.DeviseId = id;
            return this;
        }

        public CommandeBuilder FournisseurId(int? id)
        {
            Model.FournisseurId = id;
            return this;
        }

        public CommandeBuilder CommandeId(int id)
        {
            Model.CommandeId = id;
            return this;
        }

        public CommandeBuilder CommentaireFournisseur(string msg)
        {
            Model.CommentaireFournisseur = msg;
            return this;
        }

        public CommandeBuilder CommentaireInterne(string msg)
        {
            Model.CommentaireInterne = msg;
            return this;
        }
    }
}
