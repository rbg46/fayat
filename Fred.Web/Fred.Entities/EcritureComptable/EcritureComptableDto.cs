using System;

namespace Fred.Entities.EcritureComptable
{
    /// <summary>
    /// dto pour EcritureComptable
    /// </summary>
    public class EcritureComptableDto
    {
        /// <summary>
        /// Obtient ou définit la date de création de la écriture comptable.
        /// </summary>    
        public DateTime DateComptable { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une écriture comptable.
        /// </summary>   
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le	Numéro de pièce .
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet ressource
        /// </summary>
        public string AnaelCodeNature { get; set; }

        /// <summary>
        /// Obtient ou définit le code du journal comptable
        /// </summary>
        public string AnaelCodeJournal { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la commande de l'écriture comptable
        /// </summary>
        public string AnaelCodeCommande { get; set; }

        /// <summary>
        /// Obtient le montant HT de la écriture comptable
        /// </summary>   
        public decimal Montant { get; set; }

        /// <summary>
        /// Code de la devise
        /// </summary>
        public string CodeDevise { get; set; }

        /// <summary>
        /// Obtient le montant HT de la écriture comptable dans la devise utilisée
        /// </summary>   
        public decimal MontantDevise { get; set; }

        /// <summary>
        /// Obtient ou définit l'affaire d'une écriture comptable.
        /// </summary>  
        public string AnaelCodeCi { get; set; }
    }
}
