using System;

namespace Fred.Entities.Organisation
{
    /// <summary>
    ///   Une représentation légère de l'entité Organisation
    /// </summary>
    [Serializable]
    public class OrganisationLightEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant de l'organisation
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'organisation parente
        /// </summary>
        public int? PereId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du type de l'organisation
        /// </summary>
        public int TypeOrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé type organisation.
        /// </summary>
        public string TypeOrganisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de l'organisation
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit les codes parent
        /// </summary>
        public string CodeParents { get; set; }

        /// <summary>
        ///   Obtient ou définit le code parent direct
        /// </summary>
        public string CodeParent { get; set; }

        /// <summary>
        ///   Obtient ou définit la concaténation des Identifiants des organisations parents
        /// </summary>
        public string IdParents { get; set; }

        /// <summary>
        ///   Obtient ou définit les inetiales de la société
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// override
        /// </summary>
        /// <returns>OrganisationLightEnt en chaine de caractere</returns>
        public override string ToString()
        {
            return $"OrganisationId = {OrganisationId} -" +
                    $" PereId = {PereId} -" +
                    $" TypeOrganisationId = {TypeOrganisationId} -" +
                    $" TypeOrganisation = {TypeOrganisation} -" +
                    $" Code = {Code} -" +
                    $" Libelle = {Libelle} -" +
                    $" CodeParents = {CodeParents} -" +
                    $" CodeParent = {CodeParent} -" +
                    $" IdParents = {IdParents} " +
                    $" CodeSociete = {CodeSociete} ";
        }
    }
}
