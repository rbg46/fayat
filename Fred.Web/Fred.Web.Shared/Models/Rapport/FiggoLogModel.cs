using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport
{
    public class FiggoLogModel
    {
        /// <summary>
        /// Nombre de ligne recu
        /// </summary>
        public int NombreLigneRecu { get; set; }

        /// <summary>
        /// Nombre de ligne erreur
        /// </summary>
        public int NombreLigneErreur { get; set; }

        /// <summary>
        /// Nombre de ligne Ok
        /// </summary>
        public int nombreLigneOk => NombreLigneRecu - NombreLigneErreur;

        /// <summary>
        /// Liste des erreurs focntinnelles lors de l'import Figgo
        /// </summary>
        public List<TibcoModel> Tibco { get; set; }
    }
}
