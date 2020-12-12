using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using System.Linq;

namespace Fred.Business.Budget.Extensions
{
    /// <summary>
    /// Classe d'extension pour les ressources
    /// </summary>
    public static class RessourceExtension
    {

        /// <summary>
        /// Retourne l'unité associée à cette ressource dans le barème budget pour cette société
        /// La propriété ReferentielEtendus de la ressource ne doit pas être null
        /// La propriété ParametrageReferentielEtendus des ReferentielEtendus de la ressource ne doivent pas être null
        /// la propriété Organisation de la société ne doit pas être null
        /// </summary>
        /// <param name="ressource">ressource dont on veut l'unité</param>
        /// <param name="societe">la société</param>
        /// <returns>le code de l'unité si elle existe, null sinon</returns>
        public static string GetUniteAssocieeRessourceDansBaremeBudget(this RessourceEnt ressource, SocieteEnt societe)
        {
            return ressource.ReferentielEtendus.FirstOrDefault(r => r.SocieteId == societe.SocieteId)?
              .ParametrageReferentielEtendus?.FirstOrDefault(p => p.OrganisationId == societe.Organisation.OrganisationId)
              ?.Unite.Code;
        }
    }
}
