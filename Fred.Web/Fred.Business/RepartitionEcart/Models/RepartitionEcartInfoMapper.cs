using System.Collections.Generic;
using System.Linq;
using Fred.Entities;

namespace Fred.Business.RepartitionEcart.Models
{
    /// <summary>
    /// RepartitionEcartInfoMapper
    /// </summary>
    public static class RepartitionEcartInfoMapper
    {
        #region Constantes et propriété

        /// <summary>
        /// MoEncadrementKey
        /// </summary>
        public const string MoKey = "Mo";
        /// <summary>
        /// MoProductionKey
        /// </summary>
        public const string MaterielImmobiliseKey = "MaterielImmobilise";
        /// <summary>
        /// MaterielKey
        /// </summary>
        public const string MaterielKey = "Materiel";
        /// <summary>
        /// AchatKey
        /// </summary>
        public const string AchatKey = "Achat";
        /// <summary>
        /// AutreFraisKey
        /// </summary>
        public const string AutreFraisKey = "AutreFrais";


        private static List<RepartitionEcartInfo> infos = new List<RepartitionEcartInfo>();

        #endregion

        static RepartitionEcartInfoMapper()
        {
            var chapitreCodesMo = new List<string>() { Constantes.ChapitreCode.MoEncadrement, Constantes.ChapitreCode.MoProduction };
            var chapitreCodesMaterielImmo = new List<string>() { Constantes.ChapitreCode.MoProduction };
            var chapitreCodesMateriel = new List<string>() { Constantes.ChapitreCode.Materiel20 };
            var chapitreCodesAutreFrais = new List<string>() { Constantes.ChapitreCode.AutreFrais60, Constantes.ChapitreCode.AutreFrais13 };
            var chapitreCodesAchat = new List<string>() { Constantes.ChapitreCode.Achats30, Constantes.ChapitreCode.Achats40, Constantes.ChapitreCode.Achats50 };

            var moEncadrement = new RepartitionEcartInfo()
            {
                Key = MoKey,
                RowIndex = 1,
                ChapitresCodes = chapitreCodesMo,
                CodeRessourceEcart = "85002-20",
                RepartitionLibelle = "Mo Pointee (Hors Interim)",
                OdLibelle = "OD d’écart MO",
                CodeTacheParDefault = "999991",
                CodeOdFamilly = "MO"
            };
            infos.Add(moEncadrement);

            var moProduction = new RepartitionEcartInfo()
            {
                Key = MaterielImmobiliseKey,
                RowIndex = 2,
                ChapitresCodes = chapitreCodesMaterielImmo,
                CodeRessourceEcart = "85002-22",
                RepartitionLibelle = "Materiel Immobilise",
                OdLibelle = "OD d’écart Materiel Immobilise",
                CodeTacheParDefault = "999992",
                CodeOdFamilly = "MI"
            };
            infos.Add(moProduction);

            var materiel = new RepartitionEcartInfo()
            {
                Key = MaterielKey,
                RowIndex = 3,
                ChapitresCodes = chapitreCodesMateriel,
                CodeRessourceEcart = "210005-01",
                RepartitionLibelle = "Matériel",
                OdLibelle = "OD d’écart Matériel",
                CodeTacheParDefault = "999993",
                CodeOdFamilly = "MIP"
            };
            infos.Add(materiel);


            var achat = new RepartitionEcartInfo()
            {
                Key = AchatKey,
                RowIndex = 5,
                ChapitresCodes = chapitreCodesAchat,
                CodeRessourceEcart = "500000-002",
                RepartitionLibelle = "Achats",
                OdLibelle = "OD d’écart Achats",
                CodeTacheParDefault = "999994",
                CodeOdFamilly = "ACH"
            };
            infos.Add(achat);

            var autreFrais = new RepartitionEcartInfo()
            {
                Key = AutreFraisKey,
                RowIndex = 4,
                ChapitresCodes = chapitreCodesAutreFrais,
                CodeRessourceEcart = "800000-01",
                RepartitionLibelle = "Autre frais",
                OdLibelle = string.Empty,
                CodeTacheParDefault = "999995",
                CodeOdFamilly = "OTH"
            };
            infos.Add(autreFrais);
        }

        /// <summary>
        /// Recuperation les infos pour une clé donnée
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>RepartitionEcartInfo</returns>
        public static RepartitionEcartInfo GetInfoFor(string key)
        {
            return infos.FirstOrDefault(rei => rei.Key == key);
        }


        /// <summary>
        /// Recuperation les infos pour un index donné
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>RepartitionEcartInfo</returns>
        public static RepartitionEcartInfo GetInfoForIndex(int index)
        {
            return infos.FirstOrDefault(rei => rei.RowIndex == index);
        }

        /// <summary>
        /// Recuperation toutes les infos 
        /// </summary> 
        /// <returns>Liste de RepartitionEcartInfo</returns>
        public static IEnumerable<RepartitionEcartInfo> GetAll()
        {
            return infos.ToList();
        }
    }
}
