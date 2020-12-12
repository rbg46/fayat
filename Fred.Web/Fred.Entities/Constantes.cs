using System.Collections.Generic;
using System.Collections.Immutable;

namespace Fred.Entities
{
    /// <summary>
    ///   Fichier des Constantes
    /// </summary>
    public static class Constantes
    {
        public const string CodeGroupeDefault = "Default";
        public const string CodeGroupeFES = "GFES";
        public const string CodeGroupeFON = "GFON";
        public const string CodeGroupeFTP = "GFTP";
        public const string CodeGroupeRZB = "GRZB";

        /// <summary>
        ///   Code de la société Razel-Bec
        /// </summary>
        public const string CodeRazelBec = "RB";
        /// <summary>
        ///   Code de la société RMoulin BTP
        /// </summary>
        public const string CodeMoulinBTP = "MBTP";
        /// <summary>
        ///   Code Societe paye de la société Razel-Bec
        /// </summary>
        public const string CodeSocietePayeRazelBec = "RZB";

        /// <summary>
        ///   Code Societe comptable de la société Razel-Bec
        /// </summary>
        public const string CodeSocieteComptableRazelBec = "1000";

        /// <summary>
        /// Code societe paye FTP
        /// </summary>
        public const string CodeSocietePayeFTP = "FTP";
        /// <summary>
        ///   Code du société SOMOPA
        /// </summary>
        public const string CodeSocieteSOMOPA = "0143";
        /// <summary>
        ///   Prix par défaut du barème
        /// </summary>
        public const decimal PrixBaremeParDefaut = 0.01m;
        /// <summary>
        /// Période indéterminée
        /// </summary>
        public const string IndeterminatePeriod = "Période indéterminée";
        /// <summary>
        /// Avant un date
        /// </summary>
        public const string BeforeDate = "Avant le ";
        /// <summary>
        /// Après un date
        /// </summary>
        public const string AfterDate = "Après le ";
        /// <summary>
        /// SAP matricule externe resource
        /// </summary>
        public const string MatriculeExterneSapResource = "SAP";
        /// <summary>
        /// Max des heures de pointage pour FES
        /// </summary>
        public const int MaxHeurePoinatageFES = 35;
        /// <summary>
        ///   Classe regroupant les types d'organisations
        /// </summary>
        public static class OrganisationType
        {
            /// <summary>
            /// Type CI.
            /// </summary>
            public const string CodeCi = "CI";
            /// <summary>
            ///   Code du type d'organisation sous CI
            /// </summary>
            public const string CodeSousCi = "SCI";
            /// <summary>
            /// Type établissement.
            /// </summary>
            public const string CodeEtablissement = "ETABLISSEMENT";
            /// <summary>
            /// Type société.
            /// </summary>
            public const string CodeSociete = "SOCIETE";
            /// <summary>
            /// Type groupe.
            /// </summary>
            public const string CodeGroupe = "GROUPE";
            /// <summary>
            /// Type pôle.
            /// </summary>
            public const string CodePole = "POLE";
            /// <summary>
            /// Type holding.
            /// </summary>
            public const string CodeHolding = "HOLDING";
            /// <summary>
            /// Type UO.
            /// </summary>
            public const string CodeUo = "UO";
            /// <summary>
            /// Type PUO.
            /// </summary>
            public const string CodePuo = "PUO";
        }

        /// <summary>
        ///   Constante de représentation des entités
        /// </summary>
        public static class EntityType
        {
            /// <summary>
            /// CodeAbsenceEnt
            /// </summary>
            public const string Absence = "CodeAbsence";
            /// <summary>
            /// CodeDeplacementEnt
            /// </summary>
            public const string Deplacement = "CodeDeplacement";
            /// <summary>
            /// PersonnelEnt
            /// </summary>
            public const string Personnel = "Personnel";
            /// <summary>
            /// CodeMajorationEnt
            /// </summary>
            public const string Majoration = "CodeMajoration";
            /// <summary>
            /// PrimeEnt
            /// </summary>
            public const string Prime = "Prime";
            /// <summary>
            /// TacheEnt
            /// </summary>
            public const string Tache = "Tache";
            /// <summary>
            /// CIEnt
            /// </summary>
            public const string CI = "CI";
            /// <summary>
            /// CodeZoneDeplacementEnt
            /// </summary>
            public const string Zone = "CodeZoneDeplacement";
            /// <summary>
            /// SocieteEnt
            /// </summary>
            public const string Societe = "Societe";
            /// <summary>
            /// RoleEnt
            /// </summary>
            public const string Role = "Role";
            /// <summary>
            /// DevideEnt
            /// </summary>
            public const string Devise = "Devise";
            /// <summary>
            /// OrganisationEnt
            /// </summary>
            public const string Organisation = "Organisation";
            /// <summary>
            /// MaterielEnt
            /// </summary>
            public const string Materiel = "Materiel";
            /// <summary>
            /// CommandeEnt
            /// </summary>
            public const string Commande = "Commande";
            /// <summary>
            /// GroupeEnt
            /// </summary>
            public const string Groupe = "Groupe";
            /// <summary>
            /// RessourceEnt
            /// </summary>
            public const string Ressource = "Ressource";
            /// <summary>
            /// FournisseurEnt
            /// </summary>
            public const string Fournisseur = "Fournisseur";
            /// <summary>
            /// EtablissementPaieEnt
            /// </summary>
            public const string EtablissementPaie = "EtablissementPaie";
            /// <summary>
            /// EtablissementRattachementEnt
            /// </summary>
            public const string EtablissementRattachement = "EtablissementRattachement";
            /// <summary>
            /// TypeRattachementEnt
            /// </summary>
            public const string TypeRattachement = "TypeRattachement";
            /// <summary>
            /// PaysEnt
            /// </summary>
            public const string Pays = "Pays";
            /// <summary>
            /// RapportEnt
            /// </summary>
            public const string Rapport = "Rapport";
            /// <summary>
            /// DepenseEnt
            /// </summary>
            public const string Depense = "Depense";
            /// <summary>
            /// FactureEnt
            /// </summary>
            public const string Facture = "Facture";
            /// <summary>
            /// AvancementEnt
            /// </summary>
            public const string Avancement = "Avancement";
            /// <summary>
            /// ControleBudgetaireEnt
            /// </summary>
            public const string ControleBudgetaire = "ControleBudgetaire";
            /// <summary>
            /// ListeBudgetEnt
            /// </summary>
            public const string ListeBudget = "ListeBudget";
            /// <summary>
            /// DetailBudgetEnt
            /// </summary>
            public const string DetailBudget = "DetailBudget";
            /// <summary>
            /// BudgetBibliothequePrixEnt
            /// </summary>
            public const string BudgetBibliothequePrix = "BudgetBibliothequePrix";
            /// <summary>
            /// ListePointagePersonnelEnt
            /// </summary>
            public const string ListePointagePersonnel = "ListePointagePersonnel";
            /// <summary>
            /// CommandeReceptionEnt
            /// </summary>
            public const string CommandeReception = "CommandeReception";
            /// <summary>
            /// TableauReceptionEnt
            /// </summary>
            public const string TableauReception = "TableauReception";
            /// <summary>
            /// BaremeExploitationOrganisationEnt
            /// </summary>
            public const string BaremeExploitationOrganisation = "BaremeExploitationOrganisation";
            /// <summary>
            /// BaremeExploitationCIEnt
            /// </summary>
            public const string BaremeExploitationCI = "BaremeExploitationCI";
            /// <summary>
            /// ExplorateurDepenseEnt
            /// </summary>
            public const string ExplorateurDepense = "ExplorateurDepense";
            /// <summary>
            /// CompteExploitationEditionEnt
            /// </summary>
            public const string CompteExploitationEdition = "CompteExploitationEdition";
            /// <summary>
            /// OperationDiverseEnt
            /// </summary>
            public const string OperationDiverse = "OperationDiverse";
        }

        /// <summary>
        ///   Constante pour les tâches système.
        /// </summary>
        public static class TacheSysteme
        {
            /// <summary>
            /// Code de tache par défaut de niveau 1
            /// </summary>
            public const string CodeTacheDefautNiveau1 = "00";
            /// <summary>
            /// Code de tache par défaut de niveau 2
            /// </summary>
            public const string CodeTacheDefautNiveau2 = "0000";
            /// <summary>
            /// Code de tache par défaut de niveau 3
            /// </summary>
            public const string CodeTacheDefautNiveau3 = "000000";
            /// <summary>
            /// Libellé tâche par défaut tout niveau.
            /// </summary>
            public const string LibelleTacheDefaut = "TACHE PAR DEFAUT";
            /// <summary>
            /// Code tâche écart niveau 1 par défaut
            /// </summary>
            public const string CodeTacheEcartNiveau1 = "99";
            /// <summary>
            /// Code tâche écart niveau 2 par défaut
            /// </summary>
            public const string CodeTacheEcartNiveau2 = "9999";
            /// <summary>
            /// Code tâche écart MO encadrement
            /// </summary>
            public const string CodeTacheEcartMOEncadrement = "999991";
            /// <summary>
            /// Code tâche écart MO Production
            /// </summary>
            public const string CodeTacheEcartMOProduction = "999992";
            /// <summary>
            /// Code tâche écart Materiel
            /// </summary>
            public const string CodeTacheEcartMateriel = "999993";
            /// <summary>
            /// Code tâche écart Materiel Immobilise
            /// </summary>
            public const string CodeTacheEcartMaterielImmobilise = "999998";
            /// <summary>
            /// Code tâche écart Achat
            /// </summary>
            public const string CodeTacheEcartAchat = "999994";
            /// <summary>
            /// Code tâche écart Autre Frais
            /// </summary>
            public const string CodeTacheEcartAutreFrais = "999995";
            /// <summary>
            /// Code tâche écart Interim
            /// </summary>
            public const string CodeTacheEcartInterim = "999996";
            /// <summary>
            /// Code tâche de litige - article non commandé
            /// </summary>
            public const string CodeTacheLitige = "999981";
            /// <summary>
            /// Code tâche écart Materiel Externe
            /// </summary>
            public const string CodeTacheEcartMaterielExterne = "999997";
            /// <summary>
            /// Code tâche écart Recette
            /// </summary>
            public const string CodeTacheEcartRecette = "999999";

            /// <summary>
            /// Tâche d'écart frais generaux.
            /// </summary>
            public const string CodeTacheEcartFraisGeneraux = "999984";

            /// <summary>
            /// Tâche d'écart autres dépenses hors debours.
            /// </summary>
            public const string CodeTacheEcartAutresDepensesHorsDebours = "999985";

            /// <summary>
            /// Libelle tâche écart Niveau 1 et 2
            /// </summary>
            public const string LibelleTacheEcartNiveau1Et2 = "ECART";
            /// <summary>
            /// Libelle tâche écart MO Encadrement
            /// </summary>
            public const string LibelleTacheEcartMOEncadrement = "ECART MO ENCADREMENT";
            /// <summary>
            /// Libelle tâche écart MO Production
            /// </summary>
            public const string LibelleTacheEcartMOProduction = "ECART MO PRODUCTION";
            /// <summary>
            /// Libelle tâche écart Materiel
            /// </summary>
            public const string LibelleTacheEcartMateriel = "ECART MATERIEL";
            /// <summary>
            /// Libelle tâche écart Materiel Immobilise
            /// </summary>
            public const string LibelleTacheEcartMaterielImmobilise = "ECART MATERIEL IMMOBILISE";
            /// <summary>
            /// Libelle tâche écart Achat
            /// </summary>
            public const string LibelleTacheEcartAchat = "ECART ACHAT";
            /// <summary>
            /// Libelle tâche écart Autre Frais
            /// </summary>
            public const string LibelleTacheEcartAutreFrais = "ECART AUTRE FRAIS";
            /// <summary>
            /// Libelle tâche écart Interim
            /// </summary>
            public const string LibelleTacheEcartInterim = "ECART INTERIM";
            /// <summary>
            /// Libellé tâche de litige - article non commandé
            /// </summary>
            public const string LibelleTacheLitige = "LITIGE - ARTICLE NON COMMANDE";
            /// <summary>
            /// Libelle tâche écart Materiel Externe
            /// </summary>
            public const string LibelleTacheEcartMaterielExterne = "ECART MATERIEL EXTERNE";

            /// <summary>
            /// Libelle tâche écart recette
            /// </summary>
            public const string LibelleTacheEcartRecette = "ECART RECETTES";

            /// <summary>
            /// Libelle tâche d'écart frais generaux.
            /// </summary>
            public const string LibelleTacheEcartFraisGeneraux = "ECART FRAIS GENERAUX (HORS CB)";

            /// <summary>
            /// Libelle tâche d'écart autres dépenses hors debours.
            /// </summary>
            public const string LibelleTacheEcartAutresDepensesHorsDebours = "ECART AUTRES DÉPENSES HORS DÉBOURS";
        }

        /// <summary>
        ///   Constantes Type d'un fournisseur
        /// </summary>
        public static class TypeFournisseur
        {
            /// <summary>
            /// Code ETT (entreprise travail temporaire)
            /// </summary>
            public const string ETT = "I";
            /// <summary>
            /// Code pour Fournisseur de type Locatier
            /// </summary>
            public const string Locatier = "L";
        }

        /// <summary>
        ///   Constantes Type de Ressource
        /// </summary>
        public static class TypeRessource
        {
            /// <summary>
            /// code pour Ressource de type Materiel
            /// </summary>
            public const string CodeTypeMateriel = "MAT";
            /// <summary>
            /// Code pour Ressource de type Personnel
            /// </summary>
            public const string CodeTypePersonnel = "PERS";
        }

        /// <summary>
        /// Etat d'un budget
        /// </summary>
        public static class EtatBudget
        {
            /// <summary>
            /// Code pour un Budget en etat Brouillon 
            /// </summary>
            public const string Brouillon = "BR";
            /// <summary>
            /// Code pour un Budget en etat A Valider
            /// </summary>
            public const string AValider = "AV";
            /// <summary>
            /// Code pour un Budget en etat en Application
            /// </summary>
            public const string EnApplication = "EA";
            /// <summary>
            /// Code pour un Budget en etat archivé
            /// </summary>
            public const string Archive = "AR";
        }

        /// <summary>
        /// Etat d'un avancement
        /// </summary>
        public static class EtatAvancement
        {
            /// <summary>
            /// Code pour un avencement en etat Enregistré 
            /// </summary>
            public const string Enregistre = "ER";
            /// <summary>
            /// Code pour un avencement en etat à valider
            /// </summary>
            public const string AValider = "AV";
            /// <summary>
            /// Code pour un avencement en etat validé
            /// </summary>
            public const string Valide = "VA";
        }

        /// <summary>
        ///   Constantes Type de Journal
        /// </summary>
        public static class TypeJournal
        {
            /// <summary>
            /// Code pour un Journal de type achat
            /// </summary>
            public const string Achat = "ACHAT";
            /// <summary>
            /// Code pour un Journal de type FAR
            /// </summary>
            public const string Far = "FAR";
        }

        /// <summary>
        ///   Classe regroupant les types d'organisations
        /// </summary>
        public static class NiveauPaie
        {
            /// <summary>
            ///  Niveau de paie CDC
            /// </summary>
            public const int LevelCDC = 1;
            /// <summary>
            ///  Niveau de paie CDT
            /// </summary>
            public const int LevelCDT = 2;
            /// <summary>
            ///  Niveau de paie DRC
            /// </summary>
            public const int LevelDRC = 3;
            /// <summary>
            ///  Niveau de paie CSP
            /// </summary>
            public const int LevelCSP = 4;
            /// <summary>
            ///  Niveau de paie GSP
            /// </summary>
            public const int LevelGSP = 5;
        }

        /// <summary>
        /// Constantes de paramétrage des notifications
        /// </summary>
        public static class Notification
        {
            /// <summary>
            /// Nombre de notifications lues avant purge
            /// </summary>
            public const int NombreNotificationsLuesPurge = 10;
        }

        /// <summary>
        /// Constantes de type Statut Rapport
        /// </summary>
        public static class TypeRapportStatut
        {
            /// <summary>
            /// Code pour un personnel qui n'as pas de statut
            /// </summary>
            public const int None = 0;
            /// <summary>
            /// Code pour un personnel de type Ouvrier
            /// </summary>
            public const int Ouvrier = 1;
            /// <summary>
            /// Statut ETAM (créé pour désigner les 3 statuts ETAM)
            /// </summary>
            public const int ETAM = 2;
            ///<summary>
            /// Code pour un personnel de type Cadre
            /// </summary>
            public const int Cadre = 3;
            /// <summary>
            /// Code pour un les statuts rapport de type Material
            /// </summary>
            public const int Material = 99;
        }

        /// <summary>
        ///   Constantes Type Personnel
        /// </summary>
        public static class TypePersonnel
        {
            /// <summary>
            /// Code pour un personnel de type Ouvrier
            /// </summary>
            public const string Ouvrier = "1";
            /// <summary>
            /// Statut ETAM (créé pour désigner les 3 statuts ETAM)
            /// </summary>
            public const string ETAM = "2";
            /// <summary>
            /// Code pour un personnel de type ETAM Chantier
            /// </summary>
            public const string ETAMChantier = "2";
            /// <summary>
            /// Code pour un personnel de type ETAM Bureau
            /// </summary>
            public const string ETAMBureau = "4";
            /// <summary>
            /// Code pour un personnel de type ETAM de l'Article 36 de la convention collective nationnale
            /// </summary>
            public const string ETAMArticle36 = "5";
            /// <summary>
            /// Code pour un personnel de type Cadre
            /// </summary>
            public const string Cadre = "3";
            /// <summary>
            /// Code pour un personnel de type Horaire
            /// </summary>
            public const string Horaire = "H";
            /// <summary>
            /// Code pour un personnel de type Mensuel
            /// </summary>
            public const string Mensuel = "M";
            /// <summary>
            ///   Dico d'association des statuts/catégories
            /// </summary>
            public static readonly ImmutableDictionary<string, string> StatutCategorie = new Dictionary<string, string>() { { "1", "H" }, { "2", "H" }, { "3", "M" }, { "4", "H" }, { "5", "M" } }.ToImmutableDictionary();
        }

        /// <summary>
        ///   Constante pour les code unité
        /// </summary>
        public static class CodeUnite
        {
            /// <summary>
            /// Code unite en Heure
            /// </summary>
            public const string Heure = "H";
            /// <summary>
            /// Code unite en Jour
            /// </summary>
            public const string Jour = "JR";
            /// <summary>
            /// Code unite en Semaine
            /// </summary>
            public const string Semaine = "SM";
            /// <summary>
            /// Code unite en Mois
            /// </summary>
            public const string Mois = "MOS";
            /// <summary>
            /// Code unite en Forfait
            /// </summary>
            public const string Forfait = "FRT";
        }

        /// <summary>
        ///   Constante pour les code devise
        /// </summary>
        public static class CodeDevise
        {
            /// <summary>
            /// Code devise en Euro
            /// </summary>
            public const string Euro = "EUR";
        }

        /// <summary>
        /// Statuts des personnels
        /// </summary>
        public static class PersonnelStatutValue
        {
            /// <summary>
            /// Code Statut du personnel pour un ouvrier
            /// </summary>
            public const string Ouvrier = "Ouvrier";
            /// <summary>
            /// Code Statut ETAM (créé pour désigner les 3 statuts ETAM)
            /// </summary>
            public const string ETAM = "ETAM";
            /// <summary>
            /// Code Statut du personnel pour un cadre
            /// </summary>
            public const string Cadre = "IAC";
        }

        /// <summary>
        ///   Depense                                                    
        /// </summary>
        public static class DepenseType
        {
            /// <summary>
            /// Code pour une depense de type réception
            /// </summary>
            public const string Reception = "Réception";
            /// <summary>
            /// Code pour une depense de type valorisation
            /// </summary>
            public const string Valorisation = "Valorisation";
            /// <summary>
            /// Code pour une depense de type ajustement FAR
            /// </summary>
            public const string AjustementFar = "Ajustement FAR";
            /// <summary>
            /// Type dépense Opération diverse
            /// </summary>
            public const string OD = "OD";
            /// <summary>
            /// Code pour une depense de type facturation
            /// </summary>
            public const string Facturation = "Facturation";
            /// <summary>
            /// Code pour une depense de type extourne Far
            /// </summary>
            public const string ExtourneFar = "Extourne FAR";
            /// <summary>
            /// Code pour une depense de type transfert Tâche
            /// </summary>
            public const string TransfertTache = "Transfert Tâche";
        }

        public static class DepenseTypeCode
        {
            /// <summary>
            /// Réception
            /// </summary>
            public const int Reception = 1;

            /// <summary>
            /// Facture
            /// </summary>
            public const int Facture = 2;

            /// <summary>
            /// Facture écart
            /// </summary>
            public const int FactureEcart = 3;

            /// <summary>
            /// Facture non commandé
            /// </summary>
            public const int FactureNonCommande = 4;

            /// <summary>
            /// Facture Avoir
            /// </summary>
            public const int Avoir = 5;

            /// <summary>
            /// Avoir Ecart
            /// </summary>
            public const int AvoirEcart = 6;

            /// <summary>
            /// Ajustement FAR SAP
            /// </summary>
            public const int AjustementFar = 7;

            /// <summary>
            /// Extourne FAR
            /// </summary>
            public const int ExtourneFar = 8;
        }

        /// <summary>
        ///   Sous type Depense
        /// </summary>
        public static class DepenseSousType
        {
            /// <summary>
            /// Code pour une depense de sous type reconduction
            /// </summary>
            public const string Reconduction = "Reconduction";
            /// <summary>
            /// Code pour une depense de sous type non commmandé
            /// </summary>
            public const string NonCommandee = "Non Commandée";
            /// <summary>
            /// Code pour une depense de sous type avoir
            /// </summary>
            public const string Avoir = "Avoir";
            /// <summary>
            /// Code pour une depense de sous type écart
            /// </summary>
            public const string Ecart = "Ecart";
        }

        /// <summary>
        ///   Mouvement comptable débit ou crédit (SAP)
        /// </summary>
        public static class MouvementComptable
        {
            /// <summary>
            /// Code pour un mouvement comptable Crédit
            /// </summary>
            public const string Credit = "H";
            /// <summary>
            /// Code pour un mouvement comptable Débit
            /// </summary>
            public const string Debit = "S";
        }

        /// <summary>
        /// Chapitre moyen fes
        /// </summary>
        public static class ChapitreMoyenFes
        {
            /// <summary>
            /// Code pour une chapitre Moyen Fes materiel
            /// </summary>
            public const string EIMATERIEL = "EIMATERIEL";
            /// <summary>
            /// Code pour une chapitre Moyen Fes roulant
            /// </summary>
            public const string EIROULANT = "EIROULANT";
            /// <summary>
            /// Code pour une chapitre Moyen Fes outillage
            /// </summary>
            public const string EIOUTILLAGE = "EIOUTILLAGE";
            /// <summary>
            /// Chapitre moyen fes code list
            /// </summary>
            public static IEnumerable<string> ChapitreMoyenFesCodeList
            {
                get
                {
                    return new[]
                    {
                        EIMATERIEL,
                        EIROULANT,
                        EIOUTILLAGE
                    };
                }
            }
        }

        /// <summary>
        ///     Axe d'analyse 
        /// </summary>
        public static class AnalysisAxis
        {
            /// <summary>
            /// Code pour une axe analytique chapitre 
            /// </summary>
            public const string Chapitre = "Chapitre";
            /// <summary>
            /// Code pour une axe analytique sous chapitre
            /// </summary>
            public const string SousChapitre = "SousChapitre";
            /// <summary>
            /// Code pour une axe analytique ressource
            /// </summary>
            public const string Ressource = "Ressource";
            /// <summary>
            /// Code pour une axe analytique T1
            /// </summary>
            public const string T1 = "T1";
            /// <summary>
            /// Code pour une axe analytique T2
            /// </summary>
            public const string T2 = "T2";
            /// <summary>
            /// Code pour une axe analytique T3
            /// </summary>
            public const string T3 = "T3";
            /// <summary>
            /// Code pour une axe analytique Complet
            /// </summary>
            public const string Complet = "Complet";
        }

        /// <summary>
        /// Class des constantes des code primes
        /// </summary>
        public static class CodePrime
        {
            /// <summary>
            /// Prime Astreinte weekend et jour férié
            /// </summary>
            public const string ASTRWE = "ASTRWE";
            /// <summary>
            /// Prime Astreinte du lundi au vendredi
            /// </summary>
            public const string ASTRS = "ASTRS";
            /// <summary>
            /// Code prime GDI.
            /// </summary>
            public const string GDI = "GDI";
            /// <summary>
            /// Code prime GDP.
            /// </summary>
            public const string GDP = "GDP";
        }

        /// <summary>
        /// Seuils pour les codes de prime d'astreinte
        /// </summary>
        public static class PrimeAstreinteSeuil
        {
            /// <summary>
            /// Seuil diurne pour une prime astreinte
            /// </summary>
            public const int SeuilJour = 6;
            /// <summary>
            /// Seuil nocturne pour une prime astreinte
            /// </summary>
            public const int SeuilNuit = 21;
        }

        /// <summary>
        /// ChapitreCode
        /// </summary>
        public static class ChapitreCode
        {
            /// <summary>
            /// Code pour un chapitre Mo encadrement
            /// </summary>
            public const string MoEncadrement = "10";
            /// <summary>
            /// Code pour un chapitre Mo production
            /// </summary>
            public const string MoProduction = "11";
            /// <summary>
            /// Code pour un chapitre Matériel 20
            /// </summary>
            public const string Materiel20 = "20";
            /// <summary>
            /// Code pour un chapitre Autre Frais 60
            /// </summary>
            public const string AutreFrais60 = "60";
            /// <summary>
            /// Code pour un chapitre Autre Frais 13
            /// </summary>
            public const string AutreFrais13 = "13";
            /// <summary>
            /// Code pour un chapitre Achats 30
            /// </summary>
            public const string Achats30 = "30";
            /// <summary>
            /// Code pour un chapitre Achats 40
            /// </summary>
            public const string Achats40 = "40";
            /// <summary>
            /// Code pour un chapitre Achats 50
            /// </summary>
            public const string Achats50 = "50";
        }

        /// <summary>
        /// Etat paie
        /// </summary>
        public static class EtatPaie
        {
            /// <summary>
            /// Code pour un etat paie en jour
            /// </summary>
            public const string Jour = "Jour";
            /// <summary>
            /// Seuil des heures pour dire qu'un jour est travaillé ou d'absence .
            /// </summary>
            public const int WorkingHoursThreshold = 4;
        }

        /// <summary>
        /// Type des CIs
        /// </summary>
        public static class CiType
        {
            /// <summary>
            /// The affaire
            /// </summary>
            public const string Affaire = "A";
            /// <summary>
            /// The etude
            /// </summary>
            public const string Etude = "E";
            /// <summary>
            /// The section
            /// </summary>
            public const string Section = "S";
        }

        /// <summary>
        /// Type de société
        /// </summary>
        public static class TypeSociete
        {
            /// <summary>
            /// The interne
            /// </summary>
            public const string Interne = "INT";
            /// <summary>
            /// The partenaire
            /// </summary>
            public const string Partenaire = "PAR";
            /// <summary>
            /// The sep
            /// </summary>
            public const string Sep = "SEP";
        }

        /// <summary>
        /// Type de participation SEP
        /// </summary>
        public static class TypeParticipationSep
        {
            /// <summary>
            /// The gerant
            /// </summary>
            public const string Gerant = "G";
            /// <summary>
            /// The mandataire
            /// </summary>
            public const string Mandataire = "M";
            /// <summary>
            /// The associe
            /// </summary>
            public const string Associe = "A";
        }

        /// <summary>
        /// Type de Commande
        /// </summary>
        public static class CommandeType
        {
            /// <summary>
            /// The fourniture
            /// </summary>
            public const string Fourniture = "F";
            /// <summary>
            /// The location
            /// </summary>
            public const string Location = "L";
            /// <summary>
            /// The prestation
            /// </summary>
            public const string Prestation = "P";
            /// <summary>
            /// The prestation avenant commande
            /// </summary>
            public const string PrestationAvenantCommande = "PA_C";
            /// <summary>
            /// The prestation avenant
            /// </summary>
            public const string PrestationAvenant = "PA_A";
            /// <summary>
            /// The prestation dernier avenant
            /// </summary>
            public const string PrestationDernierAvenant = "PA_DA";
        }

        /// <summary>
        /// Bon de Commande Constantes
        /// </summary>
        public static class BondeCommande
        {
            /// <summary>
            /// CGA Fourniture Suffixe
            /// </summary>
            public const string CGAFournitureSuffixe = "_CGAFourniture";
            /// <summary>
            /// CGA Location Suffixe
            /// </summary>
            public const string CGALocationSuffixe = "_CGALocation";
            /// <summary>
            /// CGA Prestation Suffixe
            /// </summary>
            public const string CGAPrestationSuffixe = "_CGAPrestation";
            /// <summary>
            /// CGA File Extension
            /// </summary>
            public const string CGAExtension = ".doc";
        }

        /// <summary>
        /// Type Energies
        /// </summary>
        public static class TypeEnergie
        {
            /// <summary>
            /// Type Personnel
            /// </summary>
            public const string Personnel = "P";
            /// <summary>
            /// Type Matériel
            /// </summary>
            public const string Materiel = "M";
            /// <summary>
            /// Type Intérimaire
            /// </summary>
            public const string Interimaire = "I";
            /// <summary>
            /// Type Divers
            /// </summary>
            public const string Divers = "D";
        }

        /// <summary>
        /// Codes Majoration
        /// </summary>
        public static class CodesMajoration
        {
            /// <summary>
            /// Travail heures de route aller
            /// </summary>
            public const string THRA = "THRA";
            /// <summary>
            /// Travail heures de route retour
            /// </summary>
            public const string THRR = "THRR";
            /// <summary>
            /// Travail de nuit 1 (Horaire)
            /// </summary>
            public const string TNH1 = "TNH1";
            /// <summary>
            /// Travail de nuit 2 (Horaire)
            /// </summary>
            public const string TNH2 = "TNH2";
        }
        /// <summary>
        /// Codes Roles
        /// </summary>
        public static class CodeRole
        {
            /// <summary>
            /// Role Responsbale CI
            /// </summary>
            public const string CodeRoleRespCI = "RCI";
            /// <summary>
            /// Role Delegue CI
            /// </summary>
            public const string CodeRoleDelegueCI = "DCI";

            /// <summary>
            /// Code du role GSP
            /// </summary>
            public const string CodeRoleGSP = "GSP";

            /// <summary>
            /// Code du role CSP
            /// </summary>
            public const string CodeRoleCSP = "CSP";

            /// <summary>
            /// Code du role : CDC
            /// </summary>
            public const string CodeRoleCDC = "CDC";

            /// <summary>
            /// Code du role : CDT
            /// </summary>
            public const string CodeRoleCDT = "CDT";

            /// <summary>
            /// Code du role : DRC
            /// </summary>
            public const string CodeRoleDRC = "DRC";
        }

        /// <summary>
        /// Etats contrat intérimaire
        /// </summary>
        public static class EtatContratInterimaire
        {
            /// <summary>
            /// Etat validé
            /// </summary>
            public const string Valid = "VAL";
            /// <summary>
            /// Etat bloqué
            /// </summary>
            public const string Blocked = "BLQ";
        }

        /// <summary>
        /// Les Libelles des fonctionnalites 
        /// </summary>
        public static class FonctionnaliteLibelle
        {
            /// <summary>
            /// Export analytique boutons fonctionnalite libelle
            /// </summary>
            public const string ExportAnalytiqueBoutons = "Affichage des boutons pour l'export analytique";

            /// <summary>
            /// Fonctionnalite pour afficher l'écran validation lots de pointage
            /// </summary>
            public const string ValidationLotsPointage = "Affichage de la page 'Validation lots de pointage'";
        }

        /// <summary>
        /// FamilleOperationDiverseType                                                   
        /// </summary>
        public static class FamilleOperationDiverseType
        {
            /// <summary>
            /// Code pour la famille RECETTES (Hors explo et CB)
            /// </summary>
            public const string Recettes = "RCT";
            /// <summary>
            /// Code pour la famille DEBOURSE MAIN D’ŒUVRE (Hors Intérim)
            /// </summary>
            public const string DebourseMainOeuvre = "MO";
            /// <summary>
            /// Code pour la famille DEBOURSE ACHATS AVEC COMMANDE (y compris Intérim)
            /// </summary>
            public const string DebourseAchat = "ACH";
            /// <summary>
            /// Code pour la famille DEBOURSE MATERIEL INTERNE
            /// </summary>
            public const string DebourseMatInt = "MIT";
            /// <summary>
            /// Code pour la famille AMMORTISSEMENT
            /// </summary>
            public const string Amortissement = "MI";
            /// <summary>
            /// Code pour la famille AUTRES DEPENSES SANS COMMANDE
            /// </summary>
            public const string AutresDepenseSansCommande = "OTH";
            /// <summary>
            /// Code pour la famille FRAIS GENERAUX (Hors CB)
            /// </summary>
            public const string FraisGeneraux = "FG";
            /// <summary>
            /// Code pour la famille AUTRES HORS DEBOURS
            /// </summary>
            public const string AutresHorsDebours = "OTHD";
        }
    }
}
