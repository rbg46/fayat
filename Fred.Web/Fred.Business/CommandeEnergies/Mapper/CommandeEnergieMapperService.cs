using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Bareme;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Fred.Entities.Valorisation;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Mapper Commandes Energies
    /// </summary>
    public class CommandeEnergieMapperService : ICommandeEnergieMapperService
    {
        /// <summary>
        /// Transforme une liste de pointages matériel en liste de ligne de commandes
        /// </summary>
        /// <param name="pointages">Liste de pointages matériel</param>
        /// <param name="ci">CI</param>
        /// <param name="tache">Tache</param>
        /// <param name="unite">Unite</param>
        /// <returns>Liste de Commande Energlie Ligne</returns>
        public List<CommandeEnergieLigne> RapportLigneMaterielToCommandeEnergieLigne(List<PointageMaterielCommandeEnergie> pointages, CIEnt ci, TacheEnt tache, UniteEnt unite)
        {
            CommandeEnergieLigne ligneEnergie;
            List<CommandeEnergieLigne> ligneEnergies = new List<CommandeEnergieLigne>();

            foreach (var p in pointages)
            {
                ligneEnergie = new CommandeEnergieLigne
                {
                    Libelle = $"{p.SocieteMatriel.Code} - {p.Materiel.Code} - {p.Materiel.Libelle}",
                    TacheId = tache?.TacheId,
                    RessourceId = p.Ressource?.RessourceId,
                    Ressource = p.Ressource,
                    UniteId = unite?.UniteId,
                    Unite = unite,
                    QuantitePointee = (decimal)p.QuantitePointee,
                    MaterielId = p.Materiel.MaterielId
                };

                SetBaremeExploitationMateriel(p, ligneEnergie, ci);

                // Ou Prix Ajusté
                ligneEnergie.PUHT = ligneEnergie.Bareme ?? 0;

                ligneEnergie.QuantiteConvertie = ligneEnergie.QuantitePointee;
                // Ou Quantité Ajustée
                ligneEnergie.Quantite = ligneEnergie.QuantiteConvertie;
                ligneEnergie.MontantValorise = ligneEnergie.Bareme.HasValue ? (decimal)(ligneEnergie.QuantiteConvertie * ligneEnergie.Bareme) : 0;
                ligneEnergie.MontantHT = (decimal)(ligneEnergie.Quantite * ligneEnergie.PUHT);

                ligneEnergies.Add(ligneEnergie);
            }

            return ligneEnergies;
        }

        /// <summary>
        /// Rempli les champs Bareme, UniteBareme, UniteBaremeId pour une ligne de commande énergie Matériel
        /// </summary>
        /// <param name="p">Pointage matériel</param>
        /// <param name="ligneEnergie">Commande Ligne énergie</param>
        /// <param name="ci">CI SEP en cours</param>
        public void SetBaremeExploitationMateriel(PointageMaterielCommandeEnergie p, CommandeEnergieLigne ligneEnergie, CIEnt ci)
        {
            SurchargeBaremeExploitationCIEnt baremeCiSurcharge = ci.SurchargeBaremeExploitationCIs.FirstOrDefault(s => s.MaterielId == p.Materiel.MaterielId);
            BaremeExploitationCIEnt baremeCi = ci.BaremeExploitationCIs.FirstOrDefault(b => p.ReferentielEtendu != null && b.ReferentielEtenduId == p.ReferentielEtendu.ReferentielEtenduId);

            if (baremeCiSurcharge?.Prix.HasValue == true)
            {
                ligneEnergie.Bareme = baremeCiSurcharge.Prix.Value;
                ligneEnergie.UniteBaremeId = baremeCiSurcharge.Unite?.UniteId;
                ligneEnergie.UniteBareme = baremeCiSurcharge.Unite;
            }
            else if (baremeCi != null)
            {
                ligneEnergie.Bareme = baremeCi.Prix.Value;
                ligneEnergie.UniteBaremeId = baremeCi.Unite?.UniteId;
                ligneEnergie.UniteBareme = baremeCi.Unite;
            }
        }

        /// <summary>
        /// Transforme une liste de pointages en liste de ligne de commandes
        /// </summary>
        /// <param name="pointages">Liste de pointages</param>
        /// <param name="ci">CI</param>
        /// <param name="tache">Tache</param>
        /// <param name="unite">Unite</param>
        /// <returns>Liste de Commande Energlie Ligne</returns>
        public List<CommandeEnergieLigne> RapportLignePersonnelToCommandeEnergieLigne(List<PointagePersonnelCommandeEnergie> pointages, CIEnt ci, TacheEnt tache, UniteEnt unite)
        {
            CommandeEnergieLigne ligneEnergie;
            List<CommandeEnergieLigne> ligneEnergies = new List<CommandeEnergieLigne>();

            foreach (var p in pointages)
            {
                ligneEnergie = new CommandeEnergieLigne
                {
                    Libelle = $"{p.SocietePerso.Code} - {p.Personnel.Matricule} - {p.Personnel.Nom} - {p.Personnel.Prenom}",
                    TacheId = tache?.TacheId,
                    RessourceId = p.Ressource?.RessourceId,
                    Ressource = p.Ressource,
                    UniteId = unite?.UniteId,
                    Unite = unite,
                    PersonnelId = p.Personnel.PersonnelId
                };

                ligneEnergie.QuantitePointee = (decimal)p.QuantitePointee;
                ligneEnergie.QuantiteConvertie = ligneEnergie.QuantitePointee;

                SetBaremeExploitationPersonnel(p, ligneEnergie, ci);

                // Ou Prix Ajusté
                ligneEnergie.PUHT = ligneEnergie.Bareme ?? 0;

                // Ou Quantité Ajustée
                ligneEnergie.Quantite = ligneEnergie.QuantiteConvertie;
                ligneEnergie.MontantHT = ligneEnergie.Quantite.HasValue ? (decimal)(ligneEnergie.Quantite * ligneEnergie.PUHT) : 0;

                ligneEnergie.MontantValorise = ligneEnergie.Bareme.HasValue ? (decimal)(ligneEnergie.QuantiteConvertie * ligneEnergie.Bareme) : 0;

                ligneEnergies.Add(ligneEnergie);
            }

            return ligneEnergies;
        }

        /// <summary>
        /// Rempli les champs Bareme, UniteBareme, UniteBaremeId pour une ligne de commande énergie Personnel
        /// </summary>
        /// <param name="p">Pointage personnel</param>
        /// <param name="ligneEnergie">Commande Ligne énergie</param>
        /// <param name="ci">CI SEP en cours</param>
        public void SetBaremeExploitationPersonnel(PointagePersonnelCommandeEnergie p, CommandeEnergieLigne ligneEnergie, CIEnt ci)
        {
            SurchargeBaremeExploitationCIEnt baremeCiSurcharge = ci.SurchargeBaremeExploitationCIs.FirstOrDefault(s => s.PersonnelId == p.Personnel.PersonnelId);
            BaremeExploitationCIEnt baremeCi = ci.BaremeExploitationCIs.FirstOrDefault(b => p.ReferentielEtendu != null && b.ReferentielEtenduId == p.ReferentielEtendu.ReferentielEtenduId);

            if (baremeCiSurcharge?.Prix.HasValue == true)
            {
                ligneEnergie.Bareme = baremeCiSurcharge.Prix.Value;
                ligneEnergie.UniteBaremeId = baremeCiSurcharge.Unite?.UniteId;
                ligneEnergie.UniteBareme = baremeCiSurcharge.Unite;
            }
            else if (baremeCi != null)
            {
                ligneEnergie.Bareme = baremeCi.Prix.Value;
                ligneEnergie.UniteBaremeId = baremeCi.Unite?.UniteId;
                ligneEnergie.UniteBareme = baremeCi.Unite;
            }
        }

        /// <summary>
        /// Création des réceptions à partir des lignes de commandes
        /// </summary>
        /// <param name="lignes">Liste de lignes de commandes énergie</param>
        /// <param name="utilisateurId">Identifiant utilisateur</param>
        /// <param name="depenseTypeReceptionId">Identifiant type réception</param>
        /// <returns>Liste de dépenses achats</returns>
        public List<DepenseAchatEnt> CommandeLigneEntToDepenseAchatEnt(List<CommandeLigneEnt> lignes, int utilisateurId, int depenseTypeReceptionId)
        {
            DateTime now = DateTime.UtcNow;
            string periode = now.ToString("yyyyMM");

            return lignes.Select(c => new DepenseAchatEnt
            {
                CommandeLigneId = c.CommandeLigneId,
                CiId = c.Commande.CiId,
                FournisseurId = c.Commande.FournisseurId,
                Libelle = c.Libelle,
                TacheId = c.TacheId,
                RessourceId = c.RessourceId,
                Quantite = c.Quantite,
                PUHT = c.PUHT,
                Date = c.Commande.Date,
                AuteurCreationId = utilisateurId,
                DateCreation = now,
                Commentaire = c.Commande.CommentaireInterne,
                DeviseId = c.Commande.DeviseId,
                NumeroBL = c.PersonnelId.HasValue ? $"M{periode}-{c.Personnel.Societe.Code}-{c.Personnel.Matricule}" :
                           c.MaterielId.HasValue ? $"M{periode}-{c.Materiel.Societe.Code}-{c.Materiel.Code}" :
                           $"M{periode}-DIVERS",
                UniteId = c.UniteId,
                DepenseTypeId = depenseTypeReceptionId,
                DateComptable = c.Commande.Date,
                QuantiteDepense = 0,
                AfficherPuHt = true,
                AfficherQuantite = true
            })
            .ToList();
        }

        /// <summary>
        /// Mapping CommandeLigneEnt vers ValorisationEnt
        /// </summary>
        /// <param name="lignes">Lignes de commande</param>
        /// <param name="tache">Tache système</param>
        /// <returns>Liste de valorisations</returns>
        public List<ValorisationEnt> CommandeLigneEntToValorisationEnt(List<CommandeLigneEnt> lignes, TacheEnt tache)
        {
            List<ValorisationEnt> valorisations = new List<ValorisationEnt>();
            ValorisationEnt mostRecentValo = null;
            decimal qteAnnulee = 0;
            // Si la somme des quantités est égale à 0, on ne créé pas d'annulation de valorisation
            bool ignoreValo = true;
            decimal sumQteValo = 0;
            DateTime now = DateTime.UtcNow;

            // Une annulation de valorisation créée par perso ou matériel
            foreach (CommandeLigneEnt x in lignes)
            {
                if (x.Commande.TypeEnergie.Code == Constantes.TypeEnergie.Personnel && x.Personnel.Valorisations.Count > 0)
                {
                    sumQteValo = x.Personnel.Valorisations.Sum(y => y.Quantite);
                    mostRecentValo = x.Personnel.Valorisations.First();
                }
                else if (x.Commande.TypeEnergie.Code == Constantes.TypeEnergie.Materiel && x.Materiel.Valorisations.Count > 0)
                {
                    sumQteValo = x.Materiel.Valorisations.Sum(y => y.Quantite);
                    mostRecentValo = x.Materiel.Valorisations.First();
                }

                qteAnnulee = sumQteValo * -1;
                ignoreValo = sumQteValo == 0;

                if (mostRecentValo != null && !ignoreValo)
                {
                    ValorisationEnt valo = new ValorisationEnt
                    {
                        CiId = x.Commande.CiId.Value,
                        RapportId = mostRecentValo.RapportId,
                        TacheId = tache.TacheId,
                        ChapitreId = mostRecentValo.ChapitreId,
                        SousChapitreId = mostRecentValo.SousChapitreId,
                        ReferentielEtenduId = mostRecentValo.ReferentielEtenduId,
                        BaremeId = mostRecentValo.BaremeId,
                        UniteId = mostRecentValo.UniteId,
                        DeviseId = mostRecentValo.DeviseId,
                        PersonnelId = x.PersonnelId,
                        MaterielId = x.MaterielId,
                        Date = x.Commande.Date,
                        DateCreation = now,
                        Source = "Annulation Energies",
                        PUHT = mostRecentValo.PUHT,
                        Quantite = qteAnnulee,
                        Montant = qteAnnulee * mostRecentValo.PUHT,
                        RapportLigneId = mostRecentValo.RapportLigneId
                    };

                    valorisations.Add(valo);
                }
            }

            return valorisations;
        }
    }
}
