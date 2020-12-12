using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Personnel.Models
{
    /// <summary>
    /// Represente l'entite organisation stockée dans la base mais pour une utilisation en noeud
    /// </summary>
    public class OrganisationSimple
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une organisation.
        /// </summary>
        public int OrganisationId { get; set; }


        /// <summary>
        /// L'id de la base de donne de l'organisation
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Le code de l'oranisation
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Le libelle de l'organisation
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + "-" + Libelle;


    }
}
