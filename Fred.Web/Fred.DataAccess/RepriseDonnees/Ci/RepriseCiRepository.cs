using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RepriseDonnees.Ci
{
    /// <summary>
    /// Repository des ci pour la Reprises des données
    /// </summary>
    public class RepriseCiRepository : IRepriseCiRepository
    {
        private readonly FredDbContext context;

        public RepriseCiRepository(FredDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retourne les pays en fonction de leur codes
        /// </summary>
        /// <param name="allCodesPays">codes des pays</param>
        /// <returns>Liste des pays</returns>
        public List<PaysEnt> GetPaysByCodes(List<string> allCodesPays)
        {
            return context.Pays
                    .Where(x => allCodesPays.Contains(x.Code))
                    .AsNoTracking()
                    .ToList();
        }


        /// <summary>
        ///  Recupere les personnels dont le matricule est contenu dans la liste matricules et pour plusieurs societes
        ///  ATTENTION !!!! : il se peut que 2 personnels aient le meme matricule pour 2 societes differentes.
        /// </summary>
        /// <param name="societeIds">liste des societes dans lequel les personnels existent</param>
        /// <param name="matricules">liste des matricules recherchés</param>
        /// <returns>les personnels pour plusieurs societes</returns>
        public List<PersonnelEnt> GetPersonnelListBySocieteIdsAndMatricules(List<int> societeIds, List<string> matricules)
        {
            return context.Personnels
                    .Where(x => x.SocieteId.HasValue && societeIds.Contains(x.SocieteId.Value) && matricules.Contains(x.Matricule))
                    .AsNoTracking()
                    .ToList();
        }


        /// <summary>
        /// Mise a jour des cis, seules quelque proprietes sont mise à jours
        /// </summary>
        /// <param name="cis">cis a mettre a jours</param>
        public void UpdateCis(List<CIEnt> cis)
        {
            foreach (var ci in cis)
            {
                this.context.CIs.Attach(ci);
                this.context.Entry(ci).Property(x => x.Adresse).IsModified = true;
                this.context.Entry(ci).Property(x => x.Adresse2).IsModified = true;
                this.context.Entry(ci).Property(x => x.Adresse3).IsModified = true;
                this.context.Entry(ci).Property(x => x.Ville).IsModified = true;
                this.context.Entry(ci).Property(x => x.CodePostal).IsModified = true;
                this.context.Entry(ci).Property(x => x.PaysId).IsModified = true;
                this.context.Entry(ci).Property(x => x.EnteteLivraison).IsModified = true;
                this.context.Entry(ci).Property(x => x.AdresseLivraison).IsModified = true;
                this.context.Entry(ci).Property(x => x.CodePostalLivraison).IsModified = true;
                this.context.Entry(ci).Property(x => x.VilleLivraison).IsModified = true;
                this.context.Entry(ci).Property(x => x.PaysLivraisonId).IsModified = true;
                this.context.Entry(ci).Property(x => x.AdresseFacturation).IsModified = true;
                this.context.Entry(ci).Property(x => x.CodePostalFacturation).IsModified = true;
                this.context.Entry(ci).Property(x => x.VilleFacturation).IsModified = true;
                this.context.Entry(ci).Property(x => x.PaysFacturationId).IsModified = true;
                this.context.Entry(ci).Property(x => x.ResponsableChantier).IsModified = true;
                this.context.Entry(ci).Property(x => x.ResponsableAdministratifId).IsModified = true;
                this.context.Entry(ci).Property(x => x.ZoneModifiable).IsModified = true;
                this.context.Entry(ci).Property(x => x.DateUpdate).IsModified = true;
                this.context.Entry(ci).Property(x => x.FacturationEtablissement).IsModified = true;
                this.context.Entry(ci).Property(x => x.DateOuverture).IsModified = true;
                this.context.Entry(ci).Property(x => x.LatitudeLocalisation).IsModified = true;
                this.context.Entry(ci).Property(x => x.LongitudeLocalisation).IsModified = true;
            }

        }

    }
}
