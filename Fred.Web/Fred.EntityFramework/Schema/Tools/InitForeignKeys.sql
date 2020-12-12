/******************************************************************************
	_____ _ __   __ _  _____      __  _____ ____  _____ ____  
 |  ___/ \\ \ / // \|_   _|    / / |  ___|  _ \| ____|  _ \ 
 | |_ / _ \\ V // _ \ | |     / /  | |_  | |_) |  _| | | | |
 |  _/ ___ \| |/ ___ \| |    / /   |  _| |  _ <| |___| |_| |
 |_|/_/   \_\_/_/   \_\_|   /_/    |_|   |_| \_\_____|____/ 
																														


APPLICATION				: FAYAT / FRED
OBJECT TYPE				: CREATION DES INDEX SECONDAIRES POUR LES CLES ETRANGERES
NAME					: 
DESCRIPTION				: Cr�ation des Index De Cl�s Etrang�res 			  
PARAMETER:
USAGE					: Automatique

========================================================================================================
DATE			AUTHOR		VERSION		OBJECT
========================================================================================================
20/01/2017		KKE			V1.1		US 1722 : Creation
*******************************************************************************/

----------------------
--	DROP INDEXES	--
----------------------

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_UTILISATEUR]') AND name = N'IX_UTILISATEUR_FayatAccessDirectoryId')
DROP INDEX [IX_UTILISATEUR_FayatAccessDirectoryId] ON [dbo].[FRED_UTILISATEUR];
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_UTILISATEUR]') AND name = N'IX_UTILISATEUR_CiId')
DROP INDEX [IX_UTILISATEUR_CiId] ON [dbo].[FRED_UTILISATEUR];
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_TACHE]') AND name = N'IX_TACHE_ParentId')
DROP INDEX [IX_TACHE_ParentId] ON [dbo].[FRED_TACHE];
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_TACHE]') AND name = N'IX_TACHE_CiId')
DROP INDEX [IX_TACHE_CiId] ON [dbo].[FRED_TACHE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_SOUS_CHAPITRE]') AND name = N'IX_SOUS_CHAPITRE_GroupeId')
DROP INDEX [IX_SOUS_CHAPITRE_GroupeId] ON [dbo].[FRED_SOUS_CHAPITRE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ROLE]') AND name = N'IX_ROLE_GroupeId')
DROP INDEX [IX_ROLE_GroupeId] ON [dbo].[FRED_ROLE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PRIME]') AND name = N'IX_PRIME_PrimeId')
DROP INDEX [IX_PRIME_PrimeId] ON [dbo].[FRED_PRIME]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_POLE]') AND name = N'IX_POLE_OrganisationId')
DROP INDEX [IX_POLE_OrganisationId] ON [dbo].[FRED_POLE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_POLE]') AND name = N'IX_POLE_HoldingId')
DROP INDEX [IX_POLE_HoldingId] ON [dbo].[FRED_POLE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_UtilisateurId')
DROP INDEX [IX_PERSONNEL_UtilisateurId] ON [dbo].[FRED_PERSONNEL]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_SocieteId')
DROP INDEX [IX_PERSONNEL_SocieteId] ON [dbo].[FRED_PERSONNEL]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_RessourceId')
DROP INDEX [IX_PERSONNEL_RessourceId] ON [dbo].[FRED_PERSONNEL]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_PaysId')
DROP INDEX [IX_PERSONNEL_PaysId] ON [dbo].[FRED_PERSONNEL]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGANISATION_GENERIQUE]') AND name = N'IX_ORGANISATION_GENERIQUE_OrganisationId')
DROP INDEX [IX_ORGANISATION_GENERIQUE_OrganisationId] ON [dbo].[FRED_ORGANISATION_GENERIQUE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGA_LIENS]') AND name = N'IX_ORGA_LIENS_SocieteId')
DROP INDEX [IX_ORGA_LIENS_SocieteId] ON [dbo].[FRED_ORGA_LIENS]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGA_LIENS]') AND name = N'IX_ORGA_LIENS_OrganisationId')
DROP INDEX [IX_ORGA_LIENS_OrganisationId] ON [dbo].[FRED_ORGA_LIENS]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGA_LIENS]') AND name = N'IX_ORGA_LIENS_EtablissementComptableId')
DROP INDEX [IX_ORGA_LIENS_EtablissementComptableId] ON [dbo].[FRED_ORGA_LIENS]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_NATURE]') AND name = N'IX_NATURE_SocieteId')
DROP INDEX [IX_NATURE_SocieteId] ON [dbo].[FRED_NATURE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_MATERIEL]') AND name = N'IX_MATERIEL_SocieteId')
DROP INDEX [IX_MATERIEL_SocieteId] ON [dbo].[FRED_MATERIEL]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_JOURNAL]') AND name = N'IX_JOURNAL_SocieteId')
DROP INDEX [IX_JOURNAL_SocieteId] ON [dbo].[FRED_JOURNAL]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_INDEMNITE_DEPLACEMENT]') AND name = N'IX_INDEMNITE_DEPLACEMENT_PersonnelId')
DROP INDEX [IX_INDEMNITE_DEPLACEMENT_PersonnelId] ON [dbo].[FRED_INDEMNITE_DEPLACEMENT]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_INDEMNITE_DEPLACEMENT]') AND name = N'IX_INDEMNITE_DEPLACEMENT_CodeZoneDeplacementId')
DROP INDEX [IX_INDEMNITE_DEPLACEMENT_CodeZoneDeplacementId] ON [dbo].[FRED_INDEMNITE_DEPLACEMENT]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_INDEMNITE_DEPLACEMENT]') AND name = N'IX_INDEMNITE_DEPLACEMENT_CodeDeplacementId')
DROP INDEX [IX_INDEMNITE_DEPLACEMENT_CodeDeplacementId] ON [dbo].[FRED_INDEMNITE_DEPLACEMENT]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_HOLDING]') AND name = N'IX_HOLDING_OrganisationId')
DROP INDEX [IX_HOLDING_OrganisationId] ON [dbo].[FRED_HOLDING]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_GROUPE]') AND name = N'IX_GROUPE_PoleId')
DROP INDEX [IX_GROUPE_PoleId] ON [dbo].[FRED_GROUPE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_GROUPE]') AND name = N'IX_GROUPE_OrganisationId')
DROP INDEX [IX_GROUPE_OrganisationId] ON [dbo].[FRED_GROUPE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_FOURNISSEUR]') AND name = N'IX_FOURNISSEUR_GroupeId')
DROP INDEX [IX_FOURNISSEUR_GroupeId] ON [dbo].[FRED_FOURNISSEUR]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_FONCTIONNALITE]') AND name = N'IX_FONCTIONNALITE_ModuleId')
DROP INDEX [IX_FONCTIONNALITE_ModuleId] ON [dbo].[FRED_FONCTIONNALITE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ETABLISSEMENT_PAIE]') AND name = N'IX_ETABLISSEMENT_PAIE_AgenceRattachementId')
DROP INDEX [IX_ETABLISSEMENT_PAIE_AgenceRattachementId] ON [dbo].[FRED_ETABLISSEMENT_PAIE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ETABLISSEMENT_COMPTABLE]') AND name = N'IX_ETABLISSEMENT_COMPTABLE_SocieteId')
DROP INDEX [IX_ETABLISSEMENT_COMPTABLE_SocieteId] ON [dbo].[FRED_ETABLISSEMENT_COMPTABLE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ETABLISSEMENT_COMPTABLE]') AND name = N'IX_ETABLISSEMENT_COMPTABLE_OrganisationId')
DROP INDEX [IX_ETABLISSEMENT_COMPTABLE_OrganisationId] ON [dbo].[FRED_ETABLISSEMENT_COMPTABLE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_DATES_CLOTURE_COMPTABLE]') AND name = N'IX_DATES_CLOTURE_COMPTABLE_CiId')
DROP INDEX [IX_DATES_CLOTURE_COMPTABLE_CiId] ON [dbo].[FRED_DATES_CLOTURE_COMPTABLE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_DATES_CALENDRIER_PAIE]') AND name = N'IX_DATES_CALENDRIER_PAIE_SocieteId')
DROP INDEX [IX_DATES_CALENDRIER_PAIE_SocieteId] ON [dbo].[FRED_DATES_CALENDRIER_PAIE]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_CODE_ZONE_DEPLACEMENT]') AND name = N'IX_CODE_ZONE_DEPLACEMENT_SocieteId')
DROP INDEX [IX_CODE_ZONE_DEPLACEMENT_SocieteId] ON [dbo].[FRED_CODE_ZONE_DEPLACEMENT]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_CODE_MAJORATION]') AND name = N'IX_CODE_MAJORATION_GroupeId')
DROP INDEX [IX_CODE_MAJORATION_GroupeId] ON [dbo].[FRED_CODE_MAJORATION]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_CODE_DEPLACEMENT]') AND name = N'IX_CODE_DEPLACEMENT_SocieteId')
DROP INDEX [IX_CODE_DEPLACEMENT_SocieteId] ON [dbo].[FRED_CODE_DEPLACEMENT]
GO

----------------------
--	CREATE INDEXES	--
----------------------

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_CODE_DEPLACEMENT]') AND name = N'IX_CODE_DEPLACEMENT_SocieteId')
CREATE NONCLUSTERED INDEX [IX_CODE_DEPLACEMENT_SocieteId] ON [dbo].[FRED_CODE_DEPLACEMENT]
(
	[SocieteId] ASC
)
INCLUDE ( 	[CodeDeplacementId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_CODE_MAJORATION]') AND name = N'IX_CODE_MAJORATION_GroupeId')
CREATE NONCLUSTERED INDEX [IX_CODE_MAJORATION_GroupeId] ON [dbo].[FRED_CODE_MAJORATION]
(
	[GroupeId] ASC
)
INCLUDE ( 	[CodeMajorationId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_CODE_ZONE_DEPLACEMENT]') AND name = N'IX_CODE_ZONE_DEPLACEMENT_SocieteId')
CREATE NONCLUSTERED INDEX [IX_CODE_ZONE_DEPLACEMENT_SocieteId] ON [dbo].[FRED_CODE_ZONE_DEPLACEMENT]
(
	[SocieteId] ASC
)
INCLUDE ( 	[CodeZoneDeplacementId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_DATES_CALENDRIER_PAIE]') AND name = N'IX_DATES_CALENDRIER_PAIE_SocieteId')
CREATE NONCLUSTERED INDEX [IX_DATES_CALENDRIER_PAIE_SocieteId] ON [dbo].[FRED_DATES_CALENDRIER_PAIE]
(
	[SocieteId] ASC
)
INCLUDE ( 	[DatesCalendrierPaieId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_DATES_CLOTURE_COMPTABLE]') AND name = N'IX_DATES_CLOTURE_COMPTABLE_CiId')
CREATE NONCLUSTERED INDEX [IX_DATES_CLOTURE_COMPTABLE_CiId] ON [dbo].[FRED_DATES_CLOTURE_COMPTABLE]
(
	[CiId] ASC
)
INCLUDE ( 	[DatesClotureComptableId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ETABLISSEMENT_COMPTABLE]') AND name = N'IX_ETABLISSEMENT_COMPTABLE_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_ETABLISSEMENT_COMPTABLE_OrganisationId] ON [dbo].[FRED_ETABLISSEMENT_COMPTABLE]
(
	[OrganisationId] ASC
)
INCLUDE ( 	[EtablissementComptableId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ETABLISSEMENT_COMPTABLE]') AND name = N'IX_ETABLISSEMENT_COMPTABLE_SocieteId')
CREATE NONCLUSTERED INDEX [IX_ETABLISSEMENT_COMPTABLE_SocieteId] ON [dbo].[FRED_ETABLISSEMENT_COMPTABLE]
(
	[SocieteId] ASC
)
INCLUDE ( 	[EtablissementComptableId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ETABLISSEMENT_PAIE]') AND name = N'IX_ETABLISSEMENT_PAIE_AgenceRattachementId')
CREATE NONCLUSTERED INDEX [IX_ETABLISSEMENT_PAIE_AgenceRattachementId] ON [dbo].[FRED_ETABLISSEMENT_PAIE]
(
	[AgenceRattachementId] ASC
)
INCLUDE ( 	[EtablissementPaieId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_FONCTIONNALITE]') AND name = N'IX_FONCTIONNALITE_ModuleId')
CREATE NONCLUSTERED INDEX [IX_FONCTIONNALITE_ModuleId] ON [dbo].[FRED_FONCTIONNALITE]
(
	[ModuleId] ASC
)
INCLUDE ( 	[FonctionnaliteId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_FOURNISSEUR]') AND name = N'IX_FOURNISSEUR_GroupeId')
CREATE NONCLUSTERED INDEX [IX_FOURNISSEUR_GroupeId] ON [dbo].[FRED_FOURNISSEUR]
(
	[GroupeId] ASC
)
INCLUDE ( 	[FournisseurId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_GROUPE]') AND name = N'IX_GROUPE_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_GROUPE_OrganisationId] ON [dbo].[FRED_GROUPE]
(
	[OrganisationId] ASC
)
INCLUDE ( 	[GroupeId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_GROUPE]') AND name = N'IX_GROUPE_PoleId')
CREATE NONCLUSTERED INDEX [IX_GROUPE_PoleId] ON [dbo].[FRED_GROUPE]
(
	[PoleId] ASC
)
INCLUDE ( 	[GroupeId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_HOLDING]') AND name = N'IX_HOLDING_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_HOLDING_OrganisationId] ON [dbo].[FRED_HOLDING]
(
	[OrganisationId] ASC
)
INCLUDE ( 	[HoldingId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_INDEMNITE_DEPLACEMENT]') AND name = N'IX_INDEMNITE_DEPLACEMENT_CodeDeplacementId')
CREATE NONCLUSTERED INDEX [IX_INDEMNITE_DEPLACEMENT_CodeDeplacementId] ON [dbo].[FRED_INDEMNITE_DEPLACEMENT]
(
	[CodeDeplacementId] ASC
)
INCLUDE ( 	[IndemniteDeplacementId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_INDEMNITE_DEPLACEMENT]') AND name = N'IX_INDEMNITE_DEPLACEMENT_CodeZoneDeplacementId')
CREATE NONCLUSTERED INDEX [IX_INDEMNITE_DEPLACEMENT_CodeZoneDeplacementId] ON [dbo].[FRED_INDEMNITE_DEPLACEMENT]
(
	[CodeZoneDeplacementId] ASC
)
INCLUDE ( 	[IndemniteDeplacementId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_INDEMNITE_DEPLACEMENT]') AND name = N'IX_INDEMNITE_DEPLACEMENT_PersonnelId')
CREATE NONCLUSTERED INDEX [IX_INDEMNITE_DEPLACEMENT_PersonnelId] ON [dbo].[FRED_INDEMNITE_DEPLACEMENT]
(
	[PersonnelId] ASC
)
INCLUDE ( 	[IndemniteDeplacementId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_JOURNAL]') AND name = N'IX_JOURNAL_SocieteId')
CREATE NONCLUSTERED INDEX [IX_JOURNAL_SocieteId] ON [dbo].[FRED_JOURNAL]
(
	[SocieteId] ASC
)
INCLUDE ( 	[JournalId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_MATERIEL]') AND name = N'IX_MATERIEL_SocieteId')
CREATE NONCLUSTERED INDEX [IX_MATERIEL_SocieteId] ON [dbo].[FRED_MATERIEL]
(
	[SocieteId] ASC
)
INCLUDE ( 	[MaterielId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_NATURE]') AND name = N'IX_NATURE_SocieteId')
CREATE NONCLUSTERED INDEX [IX_NATURE_SocieteId] ON [dbo].[FRED_NATURE]
(
	[SocieteId] ASC
)
INCLUDE ( 	[NatureId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGA_LIENS]') AND name = N'IX_ORGA_LIENS_EtablissementComptableId')
CREATE NONCLUSTERED INDEX [IX_ORGA_LIENS_EtablissementComptableId] ON [dbo].[FRED_ORGA_LIENS]
(
	[EtablissementComptableId] ASC
)
INCLUDE ( 	[OrgaLiensId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGA_LIENS]') AND name = N'IX_ORGA_LIENS_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_ORGA_LIENS_OrganisationId] ON [dbo].[FRED_ORGA_LIENS]
(
	[OrganisationId] ASC
)
INCLUDE ( 	[OrgaLiensId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGA_LIENS]') AND name = N'IX_ORGA_LIENS_SocieteId')
CREATE NONCLUSTERED INDEX [IX_ORGA_LIENS_SocieteId] ON [dbo].[FRED_ORGA_LIENS]
(
	[SocieteId] ASC
)
INCLUDE ( 	[OrgaLiensId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ORGANISATION_GENERIQUE]') AND name = N'IX_ORGANISATION_GENERIQUE_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_ORGANISATION_GENERIQUE_OrganisationId] ON [dbo].[FRED_ORGANISATION_GENERIQUE]
(
	[OrganisationId] ASC
)
INCLUDE ( 	[OrganisationGeneriqueId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_PaysId')
CREATE NONCLUSTERED INDEX [IX_PERSONNEL_PaysId] ON [dbo].[FRED_PERSONNEL]
(
	[PaysId] ASC
)
INCLUDE ( 	[PersonnelId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_RessourceId')
CREATE NONCLUSTERED INDEX [IX_PERSONNEL_RessourceId] ON [dbo].[FRED_PERSONNEL]
(
	[RessourceId] ASC
)
INCLUDE ( 	[PersonnelId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_SocieteId')
CREATE NONCLUSTERED INDEX [IX_PERSONNEL_SocieteId] ON [dbo].[FRED_PERSONNEL]
(
	[SocieteId] ASC
)
INCLUDE ( 	[PersonnelId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PERSONNEL]') AND name = N'IX_PERSONNEL_UtilisateurId')
CREATE NONCLUSTERED INDEX [IX_PERSONNEL_UtilisateurId] ON [dbo].[FRED_PERSONNEL]
(
	[UtilisateurId] ASC
)
INCLUDE ( 	[PersonnelId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_POLE]') AND name = N'IX_POLE_HoldingId')
CREATE NONCLUSTERED INDEX [IX_POLE_HoldingId] ON [dbo].[FRED_POLE]
(
	[HoldingId] ASC
)
INCLUDE ( 	[PoleId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_POLE]') AND name = N'IX_POLE_OrganisationId')
CREATE NONCLUSTERED INDEX [IX_POLE_OrganisationId] ON [dbo].[FRED_POLE]
(
	[OrganisationId] ASC
)
INCLUDE ( 	[PoleId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_PRIME]') AND name = N'IX_PRIME_PrimeId')
CREATE NONCLUSTERED INDEX [IX_PRIME_PrimeId] ON [dbo].[FRED_PRIME]
(
	[SocieteId] ASC
)
INCLUDE ( 	[PrimeId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_ROLE]') AND name = N'IX_ROLE_GroupeId')
CREATE NONCLUSTERED INDEX [IX_ROLE_GroupeId] ON [dbo].[FRED_ROLE]
(
	[GroupeId] ASC
)
INCLUDE ( 	[RoleId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_SOUS_CHAPITRE]') AND name = N'IX_SOUS_CHAPITRE_GroupeId')
CREATE NONCLUSTERED INDEX [IX_SOUS_CHAPITRE_GroupeId] ON [dbo].[FRED_SOUS_CHAPITRE]
(
	[ChapitreId] ASC
)
INCLUDE ( 	[SousChapitreId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_TACHE]') AND name = N'IX_TACHE_CiId')
CREATE NONCLUSTERED INDEX [IX_TACHE_CiId] ON [dbo].[FRED_TACHE]
(
	[CiId] ASC
)
INCLUDE ( 	[TacheId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_TACHE]') AND name = N'IX_TACHE_ParentId')
CREATE NONCLUSTERED INDEX [IX_TACHE_ParentId] ON [dbo].[FRED_TACHE]
(
	[ParentId] ASC
)
INCLUDE ( 	[TacheId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_UTILISATEUR]') AND name = N'IX_UTILISATEUR_CiId')
CREATE NONCLUSTERED INDEX [IX_UTILISATEUR_CiId] ON [dbo].[FRED_UTILISATEUR]
(
	[PersonnelId] ASC
)
INCLUDE ( 	[UtilisateurId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FRED_UTILISATEUR]') AND name = N'IX_UTILISATEUR_FayatAccessDirectoryId')
CREATE NONCLUSTERED INDEX [IX_UTILISATEUR_FayatAccessDirectoryId] ON [dbo].[FRED_UTILISATEUR]
(
	[FayatAccessDirectoryId] ASC
)
INCLUDE ( 	[UtilisateurId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO