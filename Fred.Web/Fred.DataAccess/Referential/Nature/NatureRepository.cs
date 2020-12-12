using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.Nature
{
    public class NatureRepository : FredRepository<NatureEnt>, INatureRepository
    {
        public NatureRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Appel à une procédure stockée
        /// </summary>
        /// <param name="natureId">Id de la prime a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(int natureId)
        {
            int? factureLigneID;
            factureLigneID = GetFactureLigneIdByPrimeId(natureId);

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_NATURE"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@dependance", factureLigneID.HasValue ? $"'FRED_FACTURE_LIGNE',{factureLigneID}" : string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", natureId));
            sqlCommand.Parameters.Add(new SqlParameter("@delimiter", '|'));
            sqlCommand.Parameters.Add(new SqlParameter("@resu", SqlDbType.Int) { Direction = ParameterDirection.Output });

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();

            int nbcmd = (int)sqlCommand.Parameters["@resu"].Value;

            if (nbcmd == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Permet de récupérer l'id d'un pointage anticipé lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetFactureLigneIdByPrimeId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.FactureLigneARs.Where(s => s.NatureId == id).Select(s => s.LigneFactureId).FirstOrDefault();
        }

        /// <summary>
        /// Méthode d'ajout d'une nature
        /// </summary>
        /// <param name="nature">objet Nature à ajouter</param>
        /// <returns>Identifiant de la nature ajoutée</returns>
        public void AddNature(NatureEnt nature)
        {
            if (nature.Societe != null)
            {
                Context.Entry(nature.Societe).State = EntityState.Detached;
            }

            nature.Societe = null;
            Context.Natures.Add(nature);
        }

        /// <summary>
        /// Vérifie qu'une nature peut être supprimée
        /// </summary>
        /// <param name="natureEnt">La nature à vérifier avant suppression</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(NatureEnt natureEnt)
        {
            return AppelTraitementSqlVerificationDesDependances(natureEnt.NatureId);
        }

        /// <summary>
        /// Retourne une nature précise via son identifiant
        /// </summary>
        /// <param name="natureId">Identifiant d'une nature</param>
        /// <returns>Objet Nature correspondant</returns>
        public NatureEnt GetNatureById(int natureId)
        {
            return Context.Natures.Find(natureId);
        }

        /// <summary>
        /// Retourne l'identifiant de la nature portant le code devise indiqué.
        /// </summary>
        /// <param name="natureCode">Code de la nature à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        public int? GetNatureIdByCode(string natureCode)
        {
            int? natureId = null;
            NatureEnt nature = Context.Natures.FirstOrDefault(n => n.Code == natureCode);

            if (nature != null)
            {
                natureId = nature.NatureId;
            }

            return natureId;
        }

        /// <summary>
        /// Retourne la liste de toutes les natures à l'exception des supprimées
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureList()
        {
            foreach (NatureEnt nature in Context.Natures.Include(n => n.Societe))
            {
                yield return nature;
            }
        }

        /// <summary>
        /// Retourne la liste de toutes les natures avec les supprimées.
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureListAll()
        {
            foreach (NatureEnt nature in Context.Natures.Include(n => n.Societe))
            {
                yield return nature;
            }
        }

        /// <summary>
        /// Retourne la liste de toutes les natures d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureBySocieteId(int societeId)
        {
            return Context.Natures.Where(x => x.SocieteId == societeId).OrderBy(n => n.Code);
        }

        /// <summary>
        /// Méthode vérifiant l'existence d'une nature via son code pour une société donnée.
        /// </summary>
        /// <param name="natureId">Identifiant courant</param>
        /// <param name="natureCode">Code Nature</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        public bool IsNatureExistsByCode(int natureId, string natureCode, int societeId)
        {
            if (societeId > 0)
            {
                if (natureId != 0)
                {
                    return Context.Natures
                                  .Where(x => x.SocieteId == societeId)
                                  .Any(n => n.Code == natureCode && n.NatureId != natureId);
                }

                return Context.Natures
                              .Where(x => x.SocieteId == societeId)
                              .Any(n => n.Code == natureCode);
            }

            return false;
        }

        /// <summary>
        /// Retourne une liste de natures filtrées selon des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les natures</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> SearchNatureWithFilters(Expression<Func<NatureEnt, bool>> predicate)
        {
            return Context.Natures
                          .Include(n => n.Societe)
                          .Where(predicate)
                          .OrderBy(n => n.Code);
        }

        /// <summary>
        /// Retourne une liste de natures dont le code ou le libellé contient un texte donné
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> SearchLight(string text)
        {
            return Context.Natures
                          .Where(c => c.Code.Contains(text) || c.Libelle.Contains(text));
        }

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return !AppelTraitementSqlVerificationDesDependances(id);
        }

        /// <summary>
        /// Retourne la liste des natures qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des natures qui ne possèdent pas de famille</returns>
        public IEnumerable<NatureEnt> GetListNaturesWithoutFamille(int societeId)
        {
            return Context.Natures
                .Where(n => n.ParentFamilyODWithOrder == 0
                && n.ParentFamilyODWithoutOrder == 0
                && n.IsActif
                && n.SocieteId == societeId);
        }

        /// <summary>
        /// Retourne la liste des natures pour un code societe et une liste de code nature
        /// </summary>
        /// <param name="codes">Liste de code nature</param>
        /// <param name="societeIds">Liste d'identifiant de la société</param>
        /// <returns>Liste de <see cref="NatureEnt" /></returns>
        public IEnumerable<NatureEnt> GetNatureList(List<string> codes, List<int> societeIds)
        {
            return Context.Natures.Include(x => x.Societe).Where(q => codes.Contains(q.Code) && societeIds.Contains(q.Societe.SocieteId));
        }

        /// <summary>
        /// Retourne la liste de toutes les natures pour une ressource donnée
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureListByRessourceId(int ressourceId)
        {
            return Context.Natures.Where(n => n.RessourceId == ressourceId);
        }

        /// <summary>
        /// Retourne une nature en fonction de son code de sa société
        /// </summary>
        /// <param name="codeNatures">Code de la nature</param>
        /// <param name="societeId">Identifiant du CI</param>
        /// <returns>Liste de <see cref="NatureEnt" /></returns>
        public IEnumerable<NatureEnt> GetNatures(List<string> codeNatures, List<int> societeIds)
        {
            return Context.Natures.Include(x => x.ReferentielEtendus).Where(x => codeNatures.Contains(x.Code) && societeIds.Contains(x.Societe.SocieteId) && x.IsActif);
        }
    }
}
