using System;
using Fred.Common.Tests.EntityFramework;
using Fred.ImportExport.Models.EcritureComptable;

namespace Fred.Common.Tests.Data.EcritureComptable.Builder
{
    public class EcritureComptableFayatTpModelBuilder : ModelDataTestBuilder<EcritureComptableFayatTpModel>
    {
        public EcritureComptableFayatTpModelBuilder Default()
        {
            base.New();
            Model.DateCreation = new DateTime(2019, 11, 12);
            Model.DateComptable = new DateTime(2019, 11, 12);
            Model.GroupeCode = "GFTP";
            Model.SocieteCode = "0001";
            Model.Libelle = "Import ecriture comptable test";
            Model.NatureAnalytique = "60500405";
            Model.MontantDeviseInterne = 400;
            Model.DeviseInterne = "EUR";
            Model.MontantDeviseTransaction = 400;
            Model.DeviseTransaction = "EUR";
            Model.CiCode = "036704";
            Model.NumeroPiece = "0000000005";
            Model.NumeroCommande = "F000403940";
            Model.Ressource = "PERS-0020";
            Model.Quantite = 8;
            Model.Unite = "UO";
            Model.RapportLigneId = "40120";
            Model.Personne = "";
            Model.MaterielSocieteCode = null;
            Model.MaterielCode = null;
            Model.CodeRef = "";
            return this;
        }

        public EcritureComptableFayatTpModelBuilder RapportLigneId(string id)
        {
            Model.RapportLigneId = id;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder DateCreation(DateTime? dateCreation)
        {
            Model.DateCreation = dateCreation;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder DateComptable(DateTime? dateComptable)
        {
            Model.DateComptable = dateComptable;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder NatureAnalytique(string natureAnalytique)
        {
            Model.NatureAnalytique = natureAnalytique;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder MontantDeviseInterne(decimal? montantDeviseInterne)
        {
            Model.MontantDeviseInterne = montantDeviseInterne;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder DeviseInterne(string deviseInterne)
        {
            Model.DeviseInterne = deviseInterne;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder MontantDeviseTransaction(decimal? montantDeviseTransaction)
        {
            Model.MontantDeviseTransaction = montantDeviseTransaction;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder DeviseTransaction(string deviseTransaction)
        {
            Model.DeviseTransaction = deviseTransaction;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder Quantite(decimal? quantite)
        {
            Model.Quantite = quantite;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder Unite(string unite)
        {
            Model.Unite = unite;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder Ci(string ci)
        {
            Model.CiCode = ci;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder NumeroPiece(string numeroPiece)
        {
            Model.NumeroPiece = numeroPiece;
            return this;
        }

        public EcritureComptableFayatTpModelBuilder NumeroCommande(string numeroCommande)
        {
            Model.NumeroCommande = numeroCommande;
            return this;
        }
    }
}
