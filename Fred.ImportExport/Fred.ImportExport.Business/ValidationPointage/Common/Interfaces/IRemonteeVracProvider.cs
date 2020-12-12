using System;
using Fred.Entities.ValidationPointage;

namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    /// <summary>
    /// Provider qui fournie les données relative a la remontée vrac
    /// </summary>
    public interface IRemonteeVracProvider
    {
        /// <summary>
        ///   Création d'une RemonteeVracEnt
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur exécutant le contrôle</param>
        /// <param name="periode">Période choisie</param>    
        /// <returns>ControlePointageEnt</returns>
        RemonteeVracEnt CreateRemonteeVrac(int utilisateurId, DateTime periode);
    }
}
