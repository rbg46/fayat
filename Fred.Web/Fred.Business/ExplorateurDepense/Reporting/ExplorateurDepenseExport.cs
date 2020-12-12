using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Reporting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    ///   Exportation des dépenses
    /// </summary>
    public static class ExplorateurDepenseExport
    {
        private static readonly string ExcelTemplate = "Templates/Depense/TemplateExplorateurDepense.xlsx";

        /// <summary>
        ///   Crée un fichier excel comprenant les dépenses
        /// </summary>
        /// <param name="depenses">Liste de dépenses</param>
        /// <returns>Fichier excel au format Byte</returns>
        public static byte[] ToExcel(IEnumerable<ExplorateurDepenseGeneriqueModel> depenses)
        {
            MemoryStream memoryStream;

            try
            {
                var excelFormat = new ExcelFormat();
                List<ExplorateurDepenseExportModel> depenseList = new List<ExplorateurDepenseExportModel>();

                depenses.ForEach(d =>
                {
                    var devise = " " + d.Devise?.Symbole;

                    var dep = new ExplorateurDepenseExportModel
                    {
                        CodeCiLibelle = d.InfoCI,
                        CodeLibelleChapitre = d.Ressource.SousChapitre.Chapitre.Code + " - " + d.Ressource.SousChapitre.Chapitre.Libelle,
                        CodeLibelleSousChapitre = d.Ressource.SousChapitre.Code + " - " + d.Ressource.SousChapitre.Libelle,
                        CodeLibelleRessource = d.Ressource.Code + " - " + d.Ressource.Libelle,
                        CodeLibelleTacheT1 = d.Tache.Parent.Parent.Code + " - " + d.Tache.Parent.Parent.Libelle,
                        CodeLibelleTacheT2 = d.Tache.Parent.Code + " - " + d.Tache.Parent.Libelle,
                        CodeLibelleTacheT3 = d.Tache.Code + " - " + d.Tache.Libelle,
                        Libelle1 = d.Libelle1,
                        CodeUnite = d.Unite.Code,
                        Quantite = d.Quantite.ToString(),
                        PUHT = d.PUHT.ToString() + devise,
                        MontantHT = d.MontantHT.ToString() + devise,
                        SymboleDevise = d.Devise?.Symbole,
                        Code = d.Code,
                        Libelle2 = d.Libelle2,
                        Commentaire = d.Commentaire,
                        DateFacture = d.DateFacture.HasValue ? string.Format("{0:dd/MM/yyyy}", d.DateFacture.Value.ToLocalTime()) : null,
                        DateDepense = string.Format("{0:dd/MM/yyyy}", d.DateDepense.ToLocalTime()),
                        Periode = string.Format("{0:MM/yyyy}", d.Periode.ToLocalTime()),
                        CodeLibelleNature = d.Nature != null ? d.Nature.Code + " - " + d.Nature.Libelle : string.Empty,
                        TypeDepense = d.TypeDepense,
                        SousTypeDepense = d.SousTypeDepense,
                        DateRapprochement = d.DateRapprochement.HasValue ? string.Format("{0:MM/yyyy}", d.DateRapprochement.Value.ToLocalTime()) : null,
                        NumeroFacture = d.NumeroFacture,
                        MontantFacture = d.MontantFacture.HasValue ? d.MontantFacture.Value.ToString() + devise : null
                    };
                    depenseList.Add(dep);
                });

                var excelByte = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate, depenseList);
                memoryStream = new MemoryStream(excelByte);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return memoryStream.ToArray();
        }
    }
}
