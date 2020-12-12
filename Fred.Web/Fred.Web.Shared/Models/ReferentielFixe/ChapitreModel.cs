using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Web.Models.ReferentielFixe
{
    /// <summary>
    /// Représente un chapitre
    /// </summary>
    public class ChapitreModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un chapitre
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un chapitre
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des sous-chapitres associés au chapitre
        /// </summary>
        public ICollection<SousChapitreModel> SousChapitres { get; set; }

        /// <summary>
        /// Obtient ou définit la concaténation du code et du libelle
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsChecked { get; set; }
        
        /// <summary>
        /// Obtient ou définit l'identifiant du référentiel
        /// </summary>
        public string IdRef => this.ChapitreId.ToString();

        /// <summary>
        /// Obtient ou définit le libelle du référentiel
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// Obtient ou définit le code du référentiel
        /// </summary>
        public string CodeRef => this.Code;

        public int CountRessourcesToBeTreated
        {
            get
            {
                int count = 0;

                if (SousChapitres != null && SousChapitres.Any())
                {
                    foreach (var sousChapitre in SousChapitres)
                    {
                        count = count + sousChapitre.CountRessourcesToBeTreated;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Gets nombre des ressouces sans parametrage
        /// </summary>
        public int CountParamRefEtenduToBeTreated
        {
            get
            {
                int count = 0;

                if (SousChapitres != null && SousChapitres.Any())
                {
                    foreach (var sousChapitre in SousChapitres)
                    {
                        count = count + sousChapitre.CountParamRefEtenduToBeTreated;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// Le nombre d'unités à traîter
        /// </summary>
        public int CountUnitesRessourceToBeTreated
        {
            get
            {
                int count = 0;

                if (SousChapitres != null && SousChapitres.Any())
                {
                    foreach (var sousChapitre in SousChapitres)
                    {
                        count = count + sousChapitre.CountUnitesRessourceToBeTreated;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// Le nombre de ressources spécifiques ci attaché
        /// </summary>
        public int CountRessourceSpecifiquesCI
        {
            get
            {
                int count = 0;
                SousChapitres?.ToList().ForEach(ss => count = count + ss.Ressources.Count(r => r.IsRessourceSpecifiqueCi));
                return count;
            }
        }

    }
}
