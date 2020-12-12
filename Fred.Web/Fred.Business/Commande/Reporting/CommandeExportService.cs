using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Entities.Commande;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Framework.Export.Order;
using Fred.Framework.Export.Order.Models;
using static Fred.Entities.Constantes;
using Fred.Business.Utilisateur;
using System.Configuration;

namespace Fred.Business.Commande.Reporting
{
    /// <summary>
    ///   Classe de gestion de l'export pdf
    /// </summary>
    public class CommandeExportService : ICommandeExportService
    {
        private const string MontantHtFormat = "#,#0.00";
        private const string PuHtQuantiteFormat = "#,#0.00#";
        private readonly string cgaFolder = ConfigurationManager.AppSettings["cga:folder"];

        private readonly Dictionary<string, string> templates = new Dictionary<string, string>
        {
          {CommandeType.Fourniture, "Templates/Commande/TemplateCommandeFourniture.docx" },
          {CommandeType.Location, "Templates/Commande/TemplateCommandeLocation.docx" },
          {CommandeType.Prestation, "Templates/Commande/TemplateCommandePrestation.docx" },
          {CommandeType.PrestationAvenantCommande, "Templates/Commande/TemplateCommandeAvenant_Commande.docx" },
          {CommandeType.PrestationAvenant, "Templates/Commande/TemplateCommandeAvenant_Avenant.docx" },
          {CommandeType.PrestationDernierAvenant, "Templates/Commande/TemplateCommandeAvenant_DernierAvenant.docx" }
        };

        /// <summary>
        ///   Converti une commande au format PDF selon le template
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <param name="exportOptions">Options d'export</param>
        /// <param name="imageManager">Manager des images</param>
        /// <returns>tableau de byte</returns>
        public byte[] ToPdf(CommandeExportModel commande, CommandeExportOptions exportOptions, IImageManager imageManager, IUtilisateurManager userManager)
        {
            MemoryStream memoryStream;

            try
            {
                string pathCGA = string.Empty;
                commande.Lignes.Where(w => w.AvenantIsDiminution).ToList().ForEach(u => u.Quantite = "-" + u.Quantite);
                commande.LibelleCommande = GetNameCommandeByType(commande.CommandeType);

                if (commande.SocieteGeranteId.HasValue)
                {
                    string codeGroupe = userManager.GetContextUtilisateur().Personnel.Societe.Groupe.Code;
                    if (codeGroupe == CodeGroupeFTP)
                    {
                        if (File.Exists(commande.PathCGA))
                        {
                            pathCGA = commande.PathCGA;
                        }
                    }
                    else
                    {
                        var partielPath = imageManager.GetCGAPath(commande.SocieteGeranteId.Value, commande.CommandeType);
                        pathCGA = !string.IsNullOrEmpty(partielPath) ? AppDomain.CurrentDomain.BaseDirectory + imageManager.GetCGAPath(commande.SocieteGeranteId.Value, commande.CommandeType) : string.Empty;
                    }
                }

                if (!commande.Lignes.Any(l => l.AvenantNumero != null))
                {
                    OrderExportPdfModel orderExportPdfModel = new OrderExportPdfModel
                    {
                        Order = commande,
                        ExportOptions = exportOptions,
                        PathCGA = pathCGA,
                        FilePath = AppDomain.CurrentDomain.BaseDirectory + templates[commande.CommandeType]
                    };

                    memoryStream = OrderExportUtils.GeneratePdfForOrderWithoutAmendment(orderExportPdfModel);
                }
                else
                {
                    OrderWithAmendmentExportPdfModel orderWithAmendmentExportPdfModel = new OrderWithAmendmentExportPdfModel
                    {
                        Order = commande,
                        ExportOptions = exportOptions,
                        PathCGA = pathCGA,
                        FilePath = AppDomain.CurrentDomain.BaseDirectory + templates[CommandeType.PrestationAvenantCommande],
                        HtFormatAmount = MontantHtFormat,
                        AmendmentPrestationPath = templates[CommandeType.PrestationAvenant],
                        LastAmendmentPrestationPath = templates[CommandeType.PrestationDernierAvenant],
                        DecreasedAmendmentHeader = FeatureCommande.Commande_Export_EntêteAvenant_Diminution,
                        AmendmentHeader = FeatureCommande.Commande_Export_EntêteAvenant
                    };

                    memoryStream = OrderExportUtils.GeneratePdfForOrderWithAmendment(orderWithAmendmentExportPdfModel);
                }
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return memoryStream.ToArray();
        }

        private string GetCGAFilePath(CommandeEnt commande)
        {
            string cgaFilePath = $"{cgaFolder}{commande.CI?.EtablissementComptable?.SocieteId}_{commande.CI?.EtablissementComptable?.Code}";

            if (commande.Type?.Code == CommandeType.Fourniture)
            {
                cgaFilePath += $"{BondeCommande.CGAFournitureSuffixe}{BondeCommande.CGAExtension}";
            }
            else if (commande.Type?.Code == CommandeType.Location)
            {
                cgaFilePath += $"{BondeCommande.CGALocationSuffixe}{BondeCommande.CGAExtension}";
            }
            else if (commande.Type?.Code == CommandeType.Prestation)
            {
                cgaFilePath += $"{BondeCommande.CGAPrestationSuffixe}{BondeCommande.CGAExtension}";
            }
            else
            {
                cgaFilePath = string.Empty;
            }
            return cgaFilePath;
        }

        /// <summary>
        ///   Convertit un objet CommandeEnt en CommandeExportModel
        /// </summary>
        /// <param name="commande">CommandeEnt</param>
        /// <param name="societe">Societe du CI</param>
        /// <param name="personnelImageManager">Gestionnaire des personnels</param>
        /// <param name="imageManager">Manager des images</param>
        /// <returns>CommandeExportModel</returns>
        public CommandeExportModel Convert(CommandeEnt commande, SocieteEnt societe, IPersonnelImageManager personnelImageManager, IImageManager imageManager, IUtilisateurManager userManager)
        {
            byte[] signature = null;
            string logoPath = GetLogoPath(societe, imageManager);
            string cgaFilePath = GetCGAFilePath(commande);

            // Récupération de l'image de la signature.
            if (commande.Valideur?.PersonnelId != null)
            {
                // 200*70 est la taille du rectangle du PDF des commandes...
                signature = personnelImageManager.GetSignature(commande.Valideur.PersonnelId.Value, 200, 70);
            }

            //Mapping des lignes de commandes
            List<CommandeLigneExportModel> lignes = BuildCommandeLignesModels(commande.Lignes);

            string defaultTextFacturation = FeatureEtablissementComptable.EtablissementComptable_Zones_FacturationPlaceholder;

            string defaultTextPaiement = FeatureEtablissementComptable.EtablissementComptable_Zones_PaiementPlaceholder;

            string textFacturation = FeatureEtablissementComptable.EtablissementComptable_Text_Facturation_Standard;

            string textPaiement = FeatureEtablissementComptable.EtablissementComptable_Text_Paiement_Standard;

            string codeGroupe = userManager.GetContextUtilisateur().Personnel.Societe.Groupe.Code;

            if (codeGroupe == CodeGroupeFTP)
            {
                textFacturation = commande.CI?.EtablissementComptable?.Facturation ?? defaultTextFacturation;
                textPaiement = commande.CI?.EtablissementComptable?.Paiement ?? defaultTextPaiement;
            }

            return new CommandeExportModel
            {
                Numero = GetNumeroCommande(commande),// Gestion du numéro de commande selon si externe(manuelle) ou pas
                SocieteGeranteId = societe?.SocieteId,
                SocieteGerante = societe?.Libelle,
                FacturationText = textFacturation,
                PaiementText = textPaiement,
                PathCGA = cgaFilePath,
                InfosSociete = societe?.PiedDePage?.Replace("\\n", "\n"),
                FacturationAdresse = commande.FacturationAdresse,
                FacturationCPostal = commande.FacturationCPostale,
                FacturationVille = commande.FacturationVille,
                FacturationPays = commande.FacturationPays?.Libelle,
                Suivi = commande.Suivi?.PrenomNom,
                FournisseurLibelle = commande.Fournisseur?.Libelle,
                FournisseurCode = commande.Fournisseur?.Code,
                FournisseurId = commande.Fournisseur?.FournisseurId,
                FournisseurAdresse = commande.FournisseurAdresse,
                FournisseurCPostal = commande.FournisseurCPostal,
                FournisseurVille = commande.FournisseurVille,
                FournisseurPays = commande.FournisseurPays?.Libelle,
                CiCode = commande.CI?.Code,
                CiLibelle = commande.CI?.Libelle,
                Date = string.Format("{0:dd/MM/yyyy}", commande.Date.ToLocalTime()),
                Libelle = commande.Libelle,
                DeviseSymbole = commande.Devise?.Symbole,
                CommentaireFournisseur = commande.CommentaireFournisseur,
                DelaiLivraison = commande.DelaiLivraison,
                LivraisonEntete = commande.LivraisonEntete,
                LivraisonAdresse = commande.LivraisonAdresse,
                LivraisonCPostal = commande.LivraisonCPostale,
                LivraisonVille = commande.LivraisonVille,
                LivraisonPays = commande.LivraisonPays?.Libelle,
                Contact = commande.Contact?.PrenomNom,
                ContactTel = commande.ContactTel,
                Valideur = commande.Valideur?.PrenomNom,
                MontantHT = commande.MontantHT.ToString(MontantHtFormat),
                SignatureByteArray = signature,
                LogoPath = logoPath,
                Lignes = lignes.ToList(),

                // Spécifique Location
                MOConduiteOui = (commande.MOConduite) ? "X" : string.Empty,
                MOConduiteNon = (commande.MOConduite) ? string.Empty : "X",
                EntretienMecaniqueOui = (commande.EntretienMecanique) ? "X" : string.Empty,
                EntretienMecaniqueNon = (commande.EntretienMecanique) ? string.Empty : "X",
                EntretienJournalierOui = (commande.EntretienJournalier) ? "X" : string.Empty,
                EntretienJournalierNon = (commande.EntretienJournalier) ? string.Empty : "X",
                CarburantOui = (commande.Carburant) ? "X" : string.Empty,
                CarburantNon = (commande.Carburant) ? string.Empty : "X",
                LubrifiantOui = (commande.Lubrifiant) ? "X" : string.Empty,
                LubrifiantNon = (commande.Lubrifiant) ? string.Empty : "X",
                FraisAmortissementOui = (commande.FraisAmortissement) ? "X" : string.Empty,
                FraisAmortissementNon = (commande.FraisAmortissement) ? string.Empty : "X",
                FraisAssuranceOui = (commande.FraisAssurance) ? "X" : string.Empty,
                FraisAssuranceNon = (commande.FraisAssurance) ? string.Empty : "X",

                // Spécifique prestation
                ConditionSociete = commande.ConditionSociete,
                ConditionPrestation = commande.ConditionPrestation,

                CommandeType = commande.Type?.Code
            };
        }

        private string GetLogoPath(SocieteEnt societeGerante, IImageManager imageManager)
        {
            if (societeGerante?.ImageLogoId.HasValue == true)
            {
                return AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(societeGerante.SocieteId).Path;
            }

            return null;
        }

        /// <summary>
        /// Définit le numero de commande selon le type de commande (manuelle ou pas)
        /// Si manuelle alors numero externe
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <returns>Numéro de commande</returns>
        private string GetNumeroCommande(CommandeEnt commande)
        {
            return (!string.IsNullOrEmpty(commande.NumeroCommandeExterne)) ? commande.NumeroCommandeExterne : commande.Numero;
        }

        /// <summary>
        /// Mapping des lignes de commandes
        /// </summary>
        /// <param name="commandeLignes">Entités lignes de commande</param>
        /// <returns>Modèles lignes de commande</returns>
        private List<CommandeLigneExportModel> BuildCommandeLignesModels(ICollection<CommandeLigneEnt> commandeLignes)
        {
            List<CommandeLigneExportModel> lignes = new List<CommandeLigneExportModel>();

            foreach (CommandeLigneEnt commandeLigne in commandeLignes)
            {
                lignes.Add(new CommandeLigneExportModel
                {
                    Libelle = commandeLigne.Libelle,
                    MontantHTValue = commandeLigne.MontantHT,
                    MontantHT = commandeLigne.MontantHT.ToString(MontantHtFormat),
                    PUHT = commandeLigne.PUHT.ToString(PuHtQuantiteFormat),
                    Quantite = commandeLigne.Quantite.ToString(PuHtQuantiteFormat),
                    UniteCode = commandeLigne.Unite?.Code,
                    UniteLibelle = commandeLigne.Unite?.Libelle,
                    AvenantNumero = commandeLigne.AvenantLigne?.Avenant?.NumeroAvenant,
                    AvenantIsDiminution = commandeLigne.AvenantLigne?.IsDiminution ?? false
                });
            }
            return lignes;
        }

        private string GetNameCommandeByType(string type)
        {
            switch (type.ToUpper())
            {
                case "F": return FeatureCommande.Comande_Imprime_TypeCommande_Fourniture;
                case "L": return FeatureCommande.Comande_Imprime_TypeCommande_Location;
                case "P": return FeatureCommande.Comande_Imprime_TypeCommande_Prestation;
                default: return string.Empty;
            }
        }
    }
}
