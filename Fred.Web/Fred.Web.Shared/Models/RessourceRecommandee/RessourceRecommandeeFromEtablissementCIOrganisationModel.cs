using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.RessourceRecommandee
{
    /// <summary>
    /// Modèle regroupant les informations nécessaires d'une ressource recommandée retrouvée à partir de l'identifiant d'un référentiel étendu
    /// </summary>
    public class RessourceRecommandeeFromEtablissementCIOrganisationModel
    {
        /// <summary>
        /// Identifiant du référentiel étendu dont la ressource est notée comme recommandée
        /// </summary>
        public int ReferentielEtenduId { get; set; }
    }
}
