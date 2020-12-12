using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Parameter
{
    /// <summary>
    /// Classe qui transporte toutes les informations necessaire a tous les inputs, transforms ou ouputs
    /// 
    /// </summary>
    public class EtlPointageParameter
    {
        public int RapportId { get; set; }

        /// <summary>
        /// SocieteId du rapport
        /// </summary>
        public int SocieteId { get; internal set; }

        /// <summary>
        /// Prefix des logs
        /// </summary>
        public string LogPrefix { get; internal set; }

        /// <summary>
        /// Cest le CodeSocieteStorm de la table societe, c'est aussi la clé qui permet d'identifier si la societe
        /// est consernee par la fonctionnalite d'envoie du pointage personnel
        /// </summary>
        public string CodeSocieteStorm { get; internal set; }

        /// <summary>
        /// Propiete uniquement utile pour que les logs soit plus parlants
        /// </summary>
        public string SocieteLibelle { get; internal set; }

        /// <summary>
        /// Id de l'auteur du changement
        /// </summary>
        public int AuteurId { get; internal set; }

        /// <summary>
        /// Dependences 
        /// </summary>
        public PointageEtlDependenciesWrapper EtlDependencies { get; set; } = new PointageEtlDependenciesWrapper();

        public string BackgroundJobId { get; internal set; }

        /// <summary>
        /// Liste des pointages de la base Fred
        /// </summary>
        public List<RapportLigneEnt> ListRapportLignesEnt { get; internal set; } = new List<RapportLigneEnt>();
    }
}
