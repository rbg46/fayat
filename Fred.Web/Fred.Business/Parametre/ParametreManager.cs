using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Newtonsoft.Json;

namespace Fred.Business.Parametre
{
    /// <summary>
    ///   Gestionnaire des Paramètres
    /// </summary>
    public class ParametreManager : Manager<ParametreEnt, IParametreRepository>, IParametreManager
    {
        private const double BaremeExploitationPrixParDefautSiIndefiniEnBase = 0.01;

        public ParametreManager(IUnitOfWork uow, IParametreRepository parametreRepository)
          : base(uow, parametreRepository)
        {
        }

        /// <inheritdoc />
        public GoogleApiParam GetGoogleApiParams()
        {
            var parametres = Repository.Get(ParametreId.GoogleApiParams);
            if (parametres != null)
            {
                return JsonConvert.DeserializeObject<GoogleApiParam>(parametres.Valeur);
            }
            return null;
        }

        /// <inheritdoc />
        public void UpdateGoogleApiParams(GoogleApiParam param)
        {
            if (param != null)
            {
                var parametres = Repository.Get(ParametreId.GoogleApiParams);
                parametres.Valeur = JsonConvert.SerializeObject(param);
                Repository.Update(parametres);
                Save();
            }
        }

        /// <inheritdoc />
        public string GetScanShareUrl(int groupeId)
        {
            var parametre = Repository.Get(ParametreId.URLScanFacture, groupeId);
            return parametre?.Valeur;
        }

        /// <inheritdoc />
        public double GetBaremeExploitationPrixDefaut()
        {
            return GetBaremeExploitationPrix(ParametreId.BaremeExploitationPrixParDefaut);
        }

        /// <inheritdoc />
        public double GetBaremeExploitationPrixChauffeurDefaut()
        {
            return GetBaremeExploitationPrix(ParametreId.BaremeExploitationPrixChauffeurParDefaut);
        }

        /// <summary>
        /// Retourne le prix / prix chauffeur par défaut pour les barèmes exploitation.
        /// </summary>
        /// <param name="parametreId">L'identifiant du prix concerné.</param>
        /// <returns>Le prix / prix chauffeur par défaut pour les barèmes exploitation.</returns>
        private double GetBaremeExploitationPrix(ParametreId parametreId)
        {
            var ret = BaremeExploitationPrixParDefautSiIndefiniEnBase;
            var parametre = Repository.Get(parametreId);
            double value;
            if (parametre != null && double.TryParse(parametre.Valeur, out value))
            {
                ret = value;
            }
            return ret;
        }
    }
}
