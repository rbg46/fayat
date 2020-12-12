using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Framework.Extensions;
using Fred.Web.Shared.Models.Depense;

namespace Fred.Business.Depense.Converter
{
    /// <summary>
    /// Permet de convertir une DepenseAchatEnt en DepenseExhibition
    /// </summary>
    public class DepenseAchatConverter
    {
        /// <summary>
        /// Converti une liste de DepenseAchat en DepenseExhibition
        /// </summary>
        /// <param name="depenseAchats">List de DepenseAchatEnt</param>
        /// <returns>Liste de DepenseExhibition</returns>
        public IEnumerable<DepenseExhibition> Convert(List<DepenseAchatEnt> depenseAchats)
        {
            List<DepenseExhibition> result = new List<DepenseExhibition>();
            foreach (DepenseAchatEnt depAchat in depenseAchats)
            {
                DepenseExhibition depenseExhibition = Convert(depAchat, depenseAchats);
                if (depenseExhibition != null)
                {
                    result.Add(depenseExhibition);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une liste de dépense achat en depense Exihibition en récupérant les Extournes FAR pour les dépenses de type reception
        /// </summary>
        /// <param name="depAchat">Depense Achat a convertir</param>
        /// <param name="depenseAchats">Liste de dépense achat nécessaire au calcul des extourne FAR</param>
        /// <returns><see cref="DepenseExhibition"/> </returns>
        private DepenseExhibition Convert(DepenseAchatEnt depAchat, List<DepenseAchatEnt> depenseAchats)
        {
            string[] commentaire = new string[] { depAchat.NumeroBL, depAchat.Commentaire };
            Tuple<decimal?, decimal> tuple = null;

            DepenseExhibition depenseExhibition = Initialize(depAchat, commentaire);

            if (depenseExhibition.TypeDepense == Constantes.DepenseType.Reception)
            {
                tuple = GetExtourneFarData(depAchat, depenseAchats);
            }

            depenseExhibition = ProcessExtourneFar(depenseExhibition, depAchat, tuple);

            if (depAchat.DepenseType != null && depAchat.DepenseType.Code == DepenseType.ExtourneFar.ToIntValue())
            {
                return null;
            }
            return depenseExhibition;
        }

        /// <summary>
        /// Converti une liste de DepenseAchat en DepenseExhibition pour l'export
        /// </summary>
        /// <param name="depenseAchats">List de DepenseAchatEnt</param>
        /// <returns>Liste de DepenseExhibition</returns>
        public IEnumerable<DepenseExhibition> ConvertForExport(List<DepenseAchatEnt> depenseAchats)
        {
            List<DepenseExhibition> result = new List<DepenseExhibition>();
            foreach (DepenseAchatEnt depAchat in depenseAchats)
            {
                DepenseExhibition depenseExhibition = ConvertForExport(depAchat);
                if (depenseExhibition != null)
                {
                    result.Add(depenseExhibition);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une dépense Achat en DepenseExhibition pour un export de données.
        /// Dans le cas d'un export de données, le remplacement de la depense exhibition est remplacer par celui de la dépense Achat
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <returns>><see cref="DepenseExhibition"/></returns>
        private DepenseExhibition ConvertForExport(DepenseAchatEnt depAchat)
        {
            string[] commentaire = new string[] { depAchat.NumeroBL, depAchat.Commentaire };
            DepenseExhibition depenseExhibition = Initialize(depAchat, commentaire);
            depenseExhibition.RemplacementTaches = depAchat.RemplacementTaches;
            return depenseExhibition;
        }

        /// <summary>
        /// Initialise une DepenseExhibition en fonction d'un depenseAchatEnt
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns><see cref="DepenseExhibition"/></returns>
        private DepenseExhibition Initialize(DepenseAchatEnt depAchat, string[] commentaire)
        {
            DepenseExhibition depenseExhibition = new DepenseExhibition();
            AssociateDepenseExhibitionToDepenseAchat(depenseExhibition, depAchat, commentaire);
            depenseExhibition.TypeDepense = SetDepenseType(depAchat);
            depenseExhibition.SousTypeDepense = SetSousTypeDepense(depAchat);
            ProcessFacturation(depenseExhibition, depAchat);

            if (depenseExhibition.TypeDepense == Constantes.DepenseType.Reception)
            {
                depenseExhibition.TacheRemplacable = depenseExhibition.DepenseVisee;
            }
            return depenseExhibition;
        }

        /// <summary>
        /// Asssocie un DepenseExhibition à une DepenseAchatEnt
        /// </summary>
        /// <param name="depenseExhibition"><see cref="DepenseExhibition"/></param>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <param name="commentaire">Tableau de commentaire</param>
        private void AssociateDepenseExhibitionToDepenseAchat(DepenseExhibition depenseExhibition, DepenseAchatEnt depAchat, string[] commentaire)
        {
            depenseExhibition.Ci = depAchat.CI;
            depenseExhibition.Code = depAchat.CommandeLigne?.Commande?.Numero;
            depenseExhibition.CommandeId = depAchat.CommandeLigne?.CommandeId;
            depenseExhibition.Commentaire = string.Join(" - ", commentaire.Where(x => !string.IsNullOrEmpty(x)));
            depenseExhibition.RessourceId = depAchat.RessourceId ?? 0;
            depenseExhibition.Ressource = depAchat.Ressource;
            depenseExhibition.TacheId = depAchat.TacheId ?? 0;
            depenseExhibition.Tache = depAchat.Tache;
            depenseExhibition.NatureId = depAchat.Nature?.NatureId ?? 0;
            depenseExhibition.Nature = depAchat.Nature;
            depenseExhibition.UniteId = depAchat.UniteId ?? 0;
            depenseExhibition.Unite = depAchat.Unite;
            depenseExhibition.DeviseId = depAchat.DeviseId ?? 0;
            depenseExhibition.Devise = depAchat.Devise;
            depenseExhibition.PUHT = depAchat.PUHT;
            depenseExhibition.MontantHT = depAchat.PUHT * depAchat.Quantite;
            depenseExhibition.Libelle1 = depAchat.Fournisseur?.Libelle;
            depenseExhibition.Libelle2 = depAchat.Libelle;
            depenseExhibition.DateDepense = depAchat.Date ?? DateTime.MinValue;
            depenseExhibition.Periode = depAchat.DateComptable ?? DateTime.UtcNow;
            depenseExhibition.FournisseurId = depAchat.FournisseurId;
            depenseExhibition.Id = string.Concat(depenseExhibition.TypeDepense, depAchat.DepenseId.ToString());
            depenseExhibition.DateRapprochement = depAchat.DateOperation;
            depenseExhibition.Quantite = depAchat.AfficherQuantite ? depAchat.Quantite : default(decimal?);
            depenseExhibition.DepenseVisee = depAchat.DateVisaReception.HasValue;
            depenseExhibition.DepenseId = depAchat.DepenseId;
            depenseExhibition.GroupeRemplacementTacheId = depAchat.GroupeRemplacementTacheId ?? 0;
            depenseExhibition.SoldeFar = depAchat.SoldeFar;
            depenseExhibition.TacheRemplacable = true;
            depenseExhibition.AgenceId = depAchat.CommandeLigne?.Commande.AgenceId;
            depenseExhibition.MontantHtInitial = depAchat.MontantHtInitial;
            depenseExhibition.IsEnergie = depAchat.CommandeLigne?.Commande.IsEnergie ?? false;
        }

        /// <summary>
        /// Applique les informations des extournes FAR à une dépenseExhibition
        /// </summary>
        /// <param name="depenseExhibition"><see cref="DepenseExhibition"/></param>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <param name="tuple">Le solde (Qte * PUHT) de la dépense + Somme(Qte  * PUHT) des extournes et la somme des quantités des FAR des dépenses achat </param>
        /// <returns>DepenseExhibition avec les informations des extrournes FAR</returns>
        private DepenseExhibition ProcessExtourneFar(DepenseExhibition depenseExhibition, DepenseAchatEnt depAchat, Tuple<decimal?, decimal> tuple)
        {
            if (tuple != null && tuple.Item1 != null)
            {
                if (tuple.Item1 != 0)
                {
                    depenseExhibition.Quantite = depAchat.Quantite + tuple.Item2;

                    if (depenseExhibition.Quantite.Value == 0)
                    {
                        // Correctif division par 0
                        return null;
                    }

                    depenseExhibition.PUHT = tuple.Item1 / depenseExhibition.Quantite.Value;
                    depenseExhibition.MontantHT = depenseExhibition.PUHT.Value * depenseExhibition.Quantite.Value;
                    // Réceptions reconductions jamais remplaçables
                    depenseExhibition.TacheRemplacable = false;
                }
                else
                {
                    return null;
                }
            }

            return depenseExhibition;
        }

        /// <summary>
        /// Récupere les information d'extourne d'une FAR
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <param name="depenseAchats">Liste de <see cref="DepenseAchatEnt"/></param>
        /// <returns>Le solde (Qte * PUHT) de la dépense + Somme(Qte  * PUHT) des extournes et la somme des quantités des FAR des dépenses achat </returns>
        private Tuple<decimal?, decimal> GetExtourneFarData(DepenseAchatEnt depAchat, List<DepenseAchatEnt> depenseAchats)
        {
            decimal? soldeExtourne = default(decimal?);
            decimal extFarQuantite = default(decimal);

            IEnumerable<DepenseAchatEnt> extourneFars = depenseAchats.Where(x => x.DepenseParentId == depAchat.DepenseId && x.DepenseType.Code == DepenseType.ExtourneFar.ToIntValue());

            if (extourneFars.Any())
            {
                soldeExtourne = (depAchat.Quantite * depAchat.PUHT) + extourneFars.Sum(x => x.Quantite * x.PUHT);
                extFarQuantite = extourneFars.Sum(x => x.Quantite);
            }
            return new Tuple<decimal?, decimal>(soldeExtourne, extFarQuantite);
        }

        /// <summary>
        /// Applique les informations de la faction pour une depenseExhibition pour une DepenseAchatEnt
        /// </summary>
        /// <param name="depenseExhibition"><see cref="DepenseExhibition"/></param>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        private void ProcessFacturation(DepenseExhibition depenseExhibition, DepenseAchatEnt depAchat)
        {
            FacturationEnt fact = null;

            if (depAchat.DepenseType != null)
            {
                if (depAchat.DepenseType.Code == DepenseType.Facture.ToIntValue() ||
                    depAchat.DepenseType.Code == DepenseType.Avoir.ToIntValue())
                {
                    fact = depAchat.FacturationsFacture.FirstOrDefault();
                }
                else if (depAchat.DepenseType.Code == DepenseType.FactureEcart.ToIntValue() ||
                         depAchat.DepenseType.Code == DepenseType.FactureNonCommande.ToIntValue() ||
                         depAchat.DepenseType.Code == DepenseType.AvoirEcart.ToIntValue())
                {
                    fact = depAchat.FacturationsFactureEcart.FirstOrDefault();

                    if (!depAchat.AfficherPuHt && fact?.EcartPu.HasValue == true && fact?.EcartPu.Value != 0)
                    {
                        depenseExhibition.PUHT = fact?.EcartPu ?? default(decimal?);
                    }
                }
            }

            string[] numeroFacture = new string[] { fact?.NumeroFactureFMFI, fact?.NumeroFactureFournisseur };

            depenseExhibition.DateFacture = fact?.DatePieceSap;
            depenseExhibition.NumeroFacture = string.Join(" - ", numeroFacture.Where(x => !string.IsNullOrEmpty(x)));
            depenseExhibition.MontantFacture = fact?.MontantTotalHT;
        }

        /// <summary>
        /// Determine le sous type de dépense de la DepenseExhibition
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <returns>le sous type de dépenses</returns>
        private string SetSousTypeDepense(DepenseAchatEnt depAchat)
        {
            if (depAchat.DepenseType != null)
            {
                if (depAchat.DepenseType.Code == DepenseType.FactureEcart.ToIntValue())
                {
                    return Constantes.DepenseSousType.Ecart;
                }
                else if (depAchat.DepenseType.Code == DepenseType.FactureNonCommande.ToIntValue())
                {
                    return Constantes.DepenseSousType.NonCommandee;
                }
                else if (depAchat.DepenseType.Code == DepenseType.Avoir.ToIntValue() ||
                         depAchat.DepenseType.Code == DepenseType.AvoirEcart.ToIntValue())
                {
                    return Constantes.DepenseSousType.Avoir;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Determine le type de dépense de la DepenseExhibition
        /// </summary>
        /// <param name="depAchat"><see cref="DepenseAchatEnt"/></param>
        /// <returns>le type de dépenses</returns>
        private string SetDepenseType(DepenseAchatEnt depAchat)
        {
            if(depAchat.DepenseType != null)
            {
                if (depAchat.DepenseType.Code == DepenseType.Reception.ToIntValue())
                {
                    return Constantes.DepenseType.Reception;
                }
                else if (depAchat.DepenseType.Code == DepenseType.Facture.ToIntValue() || depAchat.DepenseType.Code == DepenseType.FactureEcart.ToIntValue() || depAchat.DepenseType.Code == DepenseType.FactureNonCommande.ToIntValue() || depAchat.DepenseType.Code == DepenseType.Avoir.ToIntValue() || depAchat.DepenseType.Code == DepenseType.AvoirEcart.ToIntValue())
                {
                    return Constantes.DepenseType.Facturation;
                }
                else if (depAchat.DepenseType.Code == DepenseType.AjustementFar.ToIntValue())
                {
                    return Constantes.DepenseType.AjustementFar;
                }
                else if (depAchat.DepenseType.Code == DepenseType.ExtourneFar.ToIntValue())
                {
                    return Constantes.DepenseType.ExtourneFar;
                }
            }

            return string.Empty;
        }
    }
}
