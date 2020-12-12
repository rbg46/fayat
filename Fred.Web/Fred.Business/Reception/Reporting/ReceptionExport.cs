using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Reporting;
using Fred.Web.Models;

namespace Fred.Business.Reception
{
    /// <summary>
    ///   Gestion de l'export des réceptions
    /// </summary>
    public static class ReceptionExport
    {
        private static readonly string ExcelTemplate = "Templates/Reception/TemplateReceptions.xls";

        /// <summary>
        ///   Crée un fichier excel comprenant les réceptions
        /// </summary>
        /// <param name="receptions">Liste de réceptions</param>
        /// <returns>Fichier excel au format Byte</returns>
        public static byte[] ToExcel(IEnumerable<DepenseAchatEnt> receptions)
        {
            MemoryStream memoryStream;

            try
            {
                ExcelFormat excelFormat = new ExcelFormat();
                List<ReceptionExportModel> receptionList = new List<ReceptionExportModel>();

                receptions.ForEach(r =>
                {
                    var devise = " " + r.Devise?.Symbole;
                    List<string> datesFactures = new List<string>();
                    List<string> datesRappro = new List<string>();
                    List<string> montantHTFactures = new List<string>();

                    r.FacturationsReception.ForEach(f =>
                    {
                        datesFactures.Add(string.Format("{0:dd/MM/yyyy}", f.DatePieceSap.ToLocalTime()));
                        datesRappro.Add(string.Format("{0:dd/MM/yyyy}", f.DateComptable.ToLocalTime()));
                        montantHTFactures.Add(f.MontantTotalHT.ToString() + devise);
                    });

                    ReceptionExportModel rcpt = new ReceptionExportModel
                    {
                        NumeroBC = GetNumeroBC(r.CommandeLigne),
                        FournisseurLibelle = r.CommandeLigne?.Commande?.Fournisseur?.Libelle,
                        CICode = r.Tache != null && r.Tache.Code == Constantes.TacheSysteme.CodeTacheEcartInterim ? r.CI?.Code : r.CommandeLigne?.Commande?.CI?.Code,
                        CommandeLigneLibelle = r.CommandeLigne?.Libelle,
                        CommandeDate = string.Format("{0:dd/MM/yyyy}", r.CommandeLigne?.Commande?.Date.ToLocalTime()),
                        CommandeMontantHT = r.CommandeLigne?.Commande?.MontantHT.ToString() + devise,
                        CommandeMontantHTReceptionne = r.CommandeLigne?.Commande?.MontantHTReceptionne.ToString() + devise,
                        Date = r.Date.HasValue ? string.Format("{0:dd/MM/yyyy}", r.Date.Value.ToLocalTime()) : string.Empty,
                        Periode = r.DateComptable.HasValue ? string.Format("{0:MM/yyyy}", r.DateComptable.Value.ToLocalTime()) : string.Empty,
                        Libelle = r.Libelle,
                        RessourceCodeLibelle = r.Ressource?.Code + " - " + r.Ressource?.Libelle,
                        NatureCodeLibelle = r.Nature?.Code + " - " + r.Nature?.Libelle,
                        TacheCodeLibelle = r.Tache?.Code + " - " + r.Tache?.Libelle,
                        UniteCode = r.Unite?.Code,
                        Quantite = r.Quantite.ToString(),
                        PUHT = r.PUHT.ToString() + devise,
                        MontantHT = r.MontantHT.ToString() + devise,
                        NumeroBL = r.NumeroBL,
                        DateVisaReception = r.DateVisaReception.HasValue ? string.Format("{0:dd/MM/yyyy}", r.DateVisaReception.Value.ToLocalTime()) : string.Empty,
                        Commentaire = r.Commentaire,
                        DateAuteurCreation = (r.DateCreation.HasValue ? string.Format("{0:dd/MM/yyyy}", r.DateCreation.Value.ToLocalTime()) : string.Empty) + " - " + r.AuteurCreation?.PrenomNom,
                        DateAuteurModification = (r.DateModification.HasValue ? string.Format("{0:dd/MM/yyyy}", r.DateModification.Value.ToLocalTime()) : string.Empty) + " - " + r.AuteurModification?.PrenomNom,
                        MontantHTFacture = r.MontantFacture.ToString() + devise,
                        SoldeFar = r.SoldeFar.ToString() + devise,
                        DatesFactures = string.Join(" ; ", datesFactures),
                        DatesRapprochements = string.Join(" ; ", datesRappro),
                        MontantHTFactures = string.Join(" ; ", montantHTFactures),
                        DateTransfertFAR = r.DateTransfertFar

                    };
                    receptionList.Add(rcpt);
                });

                var excelByte = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate, receptionList);
                memoryStream = new MemoryStream(excelByte);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        ///   Convertit une liste de depenseachatent en receptionExportModel
        /// </summary>
        /// <param name="deps">Liste des dépenses</param>
        /// <returns>Liste des réceptions export model</returns>
        public static IEnumerable<ReceptionExportModel> ToReceptionExportModel(IEnumerable<DepenseAchatEnt> deps)
        {
            List<ReceptionExportModel> result = new List<ReceptionExportModel>();

            foreach (var r in deps.ToList())
            {
                var ci = r.CommandeLigne?.Commande?.CI;
                var devise = " " + r.Devise?.Symbole;

                var rcpt = new ReceptionExportModel
                {
                    NumeroBC = GetNumeroBC(r.CommandeLigne),
                    FournisseurLibelle = r.CommandeLigne?.Commande?.Fournisseur?.Libelle,
                    CICode = ci?.Code + " - " + ci?.Libelle,
                    CommandeLigneLibelle = r.CommandeLigne?.Libelle,
                    CommandeDate = string.Format("{0:dd/MM/yyyy}", r.CommandeLigne?.Commande?.Date.ToLocalTime()),
                    CommandeLigneQuantite = r.CommandeLigne.Quantite.ToString(),
                    Date = r.Date.HasValue ? string.Format("{0:dd/MM/yyyy}", r.Date.Value.ToLocalTime()) : string.Empty,
                    Periode = r.DateComptable.HasValue ? string.Format("{0:MM/yyyy}", r.DateComptable.Value.ToLocalTime()) : string.Empty,
                    Libelle = r.Libelle,
                    RessourceCodeLibelle = r.Ressource?.Code + " - " + r.Ressource?.Libelle,
                    NatureCodeLibelle = r.Nature?.Code + " - " + r.Nature?.Libelle,
                    TacheCodeLibelle = r.Tache?.Code + " - " + r.Tache?.Libelle,
                    UniteCode = r.Unite?.Code,
                    Quantite = r.Quantite.ToString(),
                    PUHT = r.PUHT.ToString() + devise,
                    MontantHT = r.MontantHT.ToString() + devise,
                    NumeroBL = r.NumeroBL,
                    Commentaire = r.Commentaire,
                    DeviseCode = r.Devise?.IsoCode
                };
                result.Add(rcpt);
            }
            return result;
        }

        private static string GetNumeroBC(CommandeLigneEnt commandeLigne)
        {
            // RG_4005_019
            string numeroBC = null;
            if (commandeLigne != null)
            {
                numeroBC = string.IsNullOrEmpty(commandeLigne.Commande.NumeroCommandeExterne) ? commandeLigne.Commande.Numero : commandeLigne.Commande.NumeroCommandeExterne;
            }
            return numeroBC;
        }
    }
}
