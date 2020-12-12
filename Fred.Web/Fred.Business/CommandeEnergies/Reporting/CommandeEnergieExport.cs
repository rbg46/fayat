using Fred.Framework.Exceptions;
using Fred.Framework.Reporting;
using Fred.Web.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.CommandeEnergies.Reporting
{
    /// <summary>
    ///   Classe de gestion de l'export pdf
    /// </summary>
    public static class CommandeEnergieExport
    {
        private const string TemplatePath = "Templates/Commande/TemplateCommandeEnergie.xlsx";

        /// <summary>
        ///   Converti une commande au format Excel selon le template
        /// </summary>
        /// <param name="commandeEnergie">Commande</param>
        /// <returns>tableau de byte</returns>
        public static byte[] ToExcel(CommandeEnergieExportModel commandeEnergie)
        {
            byte[] excel;

            try
            {
                List<CommandeEnergieExportModel> list = new List<CommandeEnergieExportModel>() { commandeEnergie };
                var excelFormat = new ExcelFormat();
                excel = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + TemplatePath, list);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return excel;
        }

        /// <summary>
        /// Construit un modèle d'export pour l'édition de la commande énergie
        /// </summary>
        /// <param name="commandeEnergie">La commande à éditer</param>
        /// <returns>Le modèle d'export</returns>
        public static CommandeEnergieExportModel BuildCommandeEnergieExportModel(CommandeEnergie commandeEnergie)
        {
            var lignesSupplementaires = commandeEnergie.Lignes.Where(l => !l.MaterielId.HasValue && !l.PersonnelId.HasValue)
                                                              .Select(x => new LigneSupplementaireExportModel
                                                              {
                                                                  Libelle = x.Libelle,
                                                                  Ressource = x.Ressource.Code + " - " + x.Ressource.Libelle,
                                                                  Tache = x.Tache.Code + " - " + x.Tache.Libelle,
                                                                  Unite = x.Unite.Code,
                                                                  PUHT = x.PUHT,
                                                                  Quantite = x.Quantite
                                                              })
                                                              .ToList();

            var pointageEtAjustement = commandeEnergie.Lignes.Where(l => l.MaterielId.HasValue || l.PersonnelId.HasValue)
                                                              .Select(x => new PointageEtAjustementExportModel
                                                              {
                                                                  Libelle = x.Libelle,
                                                                  Ressource = x.Ressource?.Code + " - " + x.Ressource?.Libelle,
                                                                  Unite = x.Unite.Code,
                                                                  PUHT = x.PUHT,
                                                                  QuantitePointee = x.QuantitePointee,
                                                                  QuantiteConvertie = x.QuantiteConvertie,
                                                                  Quantite = x.Quantite,
                                                                  UniteBareme = x.UniteBareme?.CodeLibelle,
                                                                  Bareme = x.Bareme ?? 0,
                                                                  Commentaire = x.Commentaire
                                                              })
                                                              .ToList();

            var month = string.Concat("0", commandeEnergie.Date.Month);
            var periode = string.Concat(month.Length > 2 ? month.Substring(month.Length - 2) : month, "/", commandeEnergie.Date.Year);

            decimal totalMontantHt = (lignesSupplementaires.Sum(x => x.PUHT * x.Quantite) ?? 0) + (pointageEtAjustement.Sum(x => x.PUHT * x.Quantite) ?? 0);
            return new CommandeEnergieExportModel
            {
                Numero = commandeEnergie.NumeroCommandeExterne,
                TypeEnergie = commandeEnergie.TypeEnergie.Libelle,
                Periode = periode,
                CiCode = commandeEnergie.CI.CodeLibelle,
                FournisseurCode = commandeEnergie.Fournisseur.Code + " - " + commandeEnergie.Fournisseur.Libelle,
                //Devise = commandeEnergie.Devise.Symbole,
                LignesSupplementaires = lignesSupplementaires,
                PointagesEtAjustements = pointageEtAjustement,
                TotalMontantHT = totalMontantHt
            };
        }
    }
}
