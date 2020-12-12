using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Framework.Templating
{
    /// <summary>
    /// Liste des noms des templates
    /// </summary>
    public static class TemplateNames
    {
        /// <summary>
        /// Template de l'email envoyé lors de la validation d'une commande
        /// </summary>
        public static string EmailCommandeValidation { get { return "EmailCommandeValidation"; } }

        /// <summary>
        /// Template de l'email envoyé pour une commande Validé
        /// </summary>
        public static string EmailCommandeImpression { get { return "TemplateEmailImpression"; } }
    }
}
