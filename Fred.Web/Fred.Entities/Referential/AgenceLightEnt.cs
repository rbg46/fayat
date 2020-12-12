using System;

namespace Fred.Entities.Referential
{    
    /// <summary>
    ///   Représente une recherche d'Agence
    /// </summary>
    [Serializable]
    public class AgenceLightEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une agence
        /// </summary>
        public int AgenceId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une agence
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une agence
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;
    }
}
