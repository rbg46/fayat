using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Entities.Organisation.Tree
{
    /// <summary>
    /// Represente l'entite organisation stockée dans la base mais pour une utilisation en noeud
    /// </summary>
    [DebuggerDisplay("[{OrganisationId}] Is = {GetOrganisationTypeLabel()} Code = {Code} Libelle = {Libelle} PereId = {PereId} (ID = {Id})")]
    public class OrganisationBase
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
        ///   Obtient ou définit la liaison le type d'organisation
        /// </summary>
        public int TypeOrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une organistion d'un niveau hiérarchique supérieure.
        /// </summary>
        public int? PereId { get; set; }
        /// <summary>
        /// Affectations sur l'organisation
        /// </summary>
        public List<AffectationBase> Affectations { get; set; } = new List<AffectationBase>();

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"[{OrganisationId}] Is = {GetOrganisationTypeLabel()} Code = {Code} Libelle = {Libelle} PereId = {PereId} (ID = {Id})";
        }

        /// <summary>
        /// Retourne le type d'organisation
        /// </summary>
        /// <returns>le type d'organisation</returns>
        public string GetOrganisationTypeLabel()
        {
            var typeOrganisation = (OrganisationType)TypeOrganisationId;
            switch (typeOrganisation)
            {
                case OrganisationType.Holding:
                    return "Holding";
                case OrganisationType.Pole:
                    return "Pôle";
                case OrganisationType.Groupe:
                    return "Groupe";
                case OrganisationType.Societe:
                    return "Société";
                case OrganisationType.Puo:
                    return "Périmètre UO";
                case OrganisationType.Uo:
                    return "Unité Opérationnelle";
                case OrganisationType.Etablissement:
                    return "Etablissement";
                case OrganisationType.Ci:
                    return "Centre d'imputation";
                case OrganisationType.SousCi:
                    return "Sous centre d'imputation";
                default:
                    return "Type Inconnu";

            }
        }

        /// <summary>
        /// Permet de savoir si l'OrganisationBase est une holding
        /// </summary>       
        /// <returns>Vraie si l'organisationBase est une holding</returns>
        public bool IsHolding()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Holding);
        }

        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un pole
        /// </summary>       
        /// <returns>Vraie si l'organisationBase est un pole</returns>
        public bool IsPole()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Pole);
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un groupe
        /// </summary>       
        /// <returns>Vraie si l'organisationBase est un groupe</returns>
        public bool IsGroupe()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Groupe);
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est une societe
        /// </summary>        
        /// <returns>Vraie si l'organisationBase est une societe</returns>
        public bool IsSociete()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Societe);
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un PUO
        /// </summary>        
        /// <returns>Vraie si l'organisationBase est une PUO</returns>
        public bool IsPuo()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Puo);
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un UO 
        /// </summary>       
        /// <returns>Vraie si l'organisationBase est un Uo</returns>
        public bool IsUo()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Uo);
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est une Etablissement
        /// </summary>       
        /// <returns>Vraie si l'organisationBase est un Etablissement</returns>
        public bool IsEtablissement()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Etablissement);
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un CI
        /// </summary>      
        /// <returns>Vraie si l'organisationBase est un CI</returns>
        public bool IsCi()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.Ci);
        }

        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un sous CI
        /// </summary>     
        /// <returns>Vraie si l'organisationBase est un sous CIsous CI</returns>
        public bool IsSousCi()
        {
            return this.TypeOrganisationId == ToIntValue(OrganisationType.SousCi);
        }

        private int ToIntValue(Enum value)
        {
            object item = Enum.Parse(value.GetType(), value.ToString());
            return (int)item;
        }

    }

}
