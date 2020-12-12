using System;
using Fred.Web.Models;
using Fred.Web.Models.Commande;

namespace Fred.Web.Shared.Models
{
    public class CommandeEnergieItemModel
    {
        public int CommandeId { get; set; }

        public StatutCommandeModel StatutCommande { get; set; }

        public FournisseurLightModel Fournisseur { get; set; }

        public TypeEnergieModel TypeEnergie { get; set; }

        public DateTime Periode { get; set; }

        public string Numero { get; set; }

        public string NumeroCommandeExterne { get; set; }

        public decimal MontantHT { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime Date { get; set; }

        public CILightModel CI { get; set; }
    }
}
