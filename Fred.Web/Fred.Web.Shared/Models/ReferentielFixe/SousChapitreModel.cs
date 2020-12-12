using System.Collections.Generic;
using System.Linq;
using Fred.Web.Models.Referential;

namespace Fred.Web.Models.ReferentielFixe
{
    /// <summary>
    /// Représente un sous-chapitre
    /// </summary>
    public class SousChapitreModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un sous-chapitre.
        /// </summary>
        public int SousChapitreId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un sous-chapitre
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un sous-chapitre
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        /// Obtient ou définit le chapitre du sous-chapitre
        /// </summary>
        public ChapitreModel Chapitre { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des ressources associées au sous-chapitre
        /// </summary>
        public ICollection<RessourceModel> Ressources { get; set; }

        /// <summary>
        /// Obtient ou définit la concaténation du code et du libelle
        /// </summary>
        public string CodeLibelle
        {
            get
            {
                return this.Code + " - " + this.Libelle;
            }
        }
        public bool IsChecked { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du référentiel
        /// </summary>
        public string IdRef => this.SousChapitreId.ToString();

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

                if (this.Ressources != null)
                {
                    foreach (var ressource in Ressources)
                    {
                        if (ressource.ReferentielEtendus == null || !ressource.ReferentielEtendus.Any() || (ressource.ReferentielEtendus != null && ressource.ReferentielEtendus.Any()
                          && !ressource.ReferentielEtendus.ToList()[0].NatureId.HasValue && ressource.Active && !ressource.DateSuppression.HasValue))
                        {
                            count++;
                        }
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

                if (this.Ressources != null)
                {
                    //On récupère la liste des référentiels étendus
                    List<ReferentielEtendu.ReferentielEtenduModel> refEtendus = this.Ressources
                                    .SelectMany(r => r.ReferentielEtendus)
                                    .Where(c => c.Ressource != null && c.Ressource.Active && !c.Ressource.DateSuppression.HasValue)
                                    .ToList();

                    foreach (ReferentielEtendu.ReferentielEtenduModel refEtendu in refEtendus)
                    {
                        if (!refEtendu.ParametrageReferentielEtendus.Any(p => p.Montant.HasValue))
                        {
                            count++;
                        }
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

                if (this.Ressources != null)
                {
                    foreach (var ressource in Ressources)
                    {
                        if (ressource.ReferentielEtendus != null && ressource.ReferentielEtendus.Any() && ressource.ReferentielEtendus.First().Achats && !ressource.ReferentielEtendus.First().UniteReferentielEtendus.Any())
                        {
                            count++;
                        }
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
                count = Ressources?.Count(r => r.IsRessourceSpecifiqueCi) != null ? Ressources.Count(r => r.IsRessourceSpecifiqueCi) : 0;
                return count;
            }
        }
    }
}
