using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Societe;
using static Fred.Entities.Constantes;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Sap
{
    public class SapFilter
    {

        /// <summary>
        /// Permet de savoir si on envoie ou non a sap les information du CI
        /// </summary>
        /// <param name="societe">la societe</param>
        /// <param name="typeSocietes">Les  type de Societes</param>
        /// <returns>true si on peux envoyé a sap</returns>
        public bool CanSendToSap(SocieteEnt societe, List<TypeSocieteEnt> typeSocietes)
        {
            if (societe?.CodeSocieteComptable == "1000")
            {
                return true;
            }

            var typeSocieteSep = typeSocietes.FirstOrDefault(x => x.Code == TypeSociete.Sep);
            if (societe != null && societe.TypeSocieteId != null && typeSocieteSep != null && typeSocieteSep.TypeSocieteId == societe.TypeSocieteId)
            {
                return true;
            }
            return false;
        }

    }
}
