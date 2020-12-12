using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Commande
{
    /// <summary>
    /// Modele de commande utilisé pour la génération des OD lors de l'import des écritures comptable pour Fayat TP
    /// </summary>
    public class CommandeEcritureComptableOdModel
    {
        public int CommandeId { get; set; }

        public string NumeroCommande { get; set; }

        public decimal Montant { get; set; }

        public List<int> CommandeLigneId { get; set; }

        public DateTime DateComptable { get; set; }
    }
}
