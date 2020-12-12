
-- EXPLICATION DU FICHIER RESET
-- IL A FALLUT REMPLACER UNE COLONNE DE TYPE GROUPE PAR UNE COLONNE DE TYPE SOCIETE DANS LA TABLE ROLE
-- DONC POUR LE LOCAL :
-- INTERVERTIR DES FICHIERS D IMPORT 
-- MODIFIER LE FICHIER D IMPORT DES ROLE

-- POUR CELA EN LOCAL JE NE FAIT RIEN 
-- PAR CONTRE SUR LES BASES AVEC DEJA DES INPORTS EFFECTUES, ON REMPLACE LE TABLE D IMPORT DES REFERENCTIELS
-- IL A FALLUT AUSSI SUPPRIMER LES DONNEES DE LA TABLE FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE
-- CAR ELLE STOCKE DES ORGANISATION QUI NE SONT PAS DES SOCIETE MAIS DES GROUPES;
-----------------------------------------------------------------------------------------------

-- SUPPRESSION DES ROLES ET DES LIENS AVEC LES SEUILS
DELETE FROM dbo.FRED_ROLE_MODULE
DELETE FROM dbo.FRED_ROLE_DEVISE
DELETE FROM dbo.FRED_ROLE_ORGANISATION_DEVISE
DELETE FROM dbo.FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE
DELETE FROM dbo.FRED_ROLE
DELETE FROM dbo.FRED_FONCTIONNALITE
DELETE FROM dbo.FRED_MODULE
-- REMISE A PLAT DES REFERENTIELS CAR LE NOMS DES FICHIERS A CHANGE
-- IL FAUT QU ISL SOIT CONSIDERES COMME DEJA EXECUTES
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[__ReferentialHistory];


GO
CREATE TABLE [dbo].[__ReferentialHistory] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ScriptName] NVARCHAR (255) NOT NULL,
    [Applied]    DATETIME       NOT NULL
);

SET IDENTITY_INSERT [dbo].[__ReferentialHistory] ON
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (1, N'001-Common\001-Fred-Ref\00010_FRED_UNITE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (2, N'001-Common\001-Fred-Ref\00020_FRED_CARBURANT_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (3, N'001-Common\001-Fred-Ref\00030_FRED_CI_SOCIETESPARTENAIRES_TYPES_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (4, N'001-Common\001-Fred-Ref\00040_FRED_COMMANDE_TYPE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (5, N'001-Common\001-Fred-Ref\00050_FRED_DEPENSE_STATUT_RAPPROCHEMENT_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (6, N'001-Common\001-Fred-Ref\00060_FRED_DEVISE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (7, N'001-Common\001-Fred-Ref\00070_FRED_EXTERNALDIRECTORY_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (8, N'001-Common\001-Fred-Ref\00080_FRED_TYPE_ORGANISATION_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (9, N'001-Common\001-Fred-Ref\00090_FRED_ORGANISATION_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (10, N'001-Common\001-Fred-Ref\00100_FRED_HOLDING_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (11, N'001-Common\001-Fred-Ref\00110_FRED_POLE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (12, N'001-Common\001-Fred-Ref\00120_FRED_GROUPE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (13, N'001-Common\001-Fred-Ref\00130_FRED_PAYS_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (14, N'001-Common\001-Fred-Ref\00140_FRED_SOCIETE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (15, N'001-Common\001-Fred-Ref\00150_FRED_RAPPORT_STATUT_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (16, N'001-Common\001-Fred-Ref\00160_FRED_ROLE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (17, N'001-Common\001-Fred-Ref\00170_FRED_STATUT_COMMANDE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (18, N'001-Common\001-Fred-Ref\00180_FRED_TYPE_RESSOURCE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (19, N'001-Common\001-Fred-Ref\00190_FRED_PARAMETRE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (20, N'001-Common\001-Fred-Ref\00200_FRED_PERSONNEL_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (21, N'001-Common\001-Fred-Ref\00210_FRED_UTILISATEUR_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (22, N'001-Common\001-Fred-Ref\00220_FRED_IMAGE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (24, N'001-Common\002-Fred-Ref-RZB\00010_FRED_RZB_ORGANISATION_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (25, N'001-Common\002-Fred-Ref-RZB\00020_FRED_RZB_ORGANISATION_GENERIQUE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (26, N'001-Common\002-Fred-Ref-RZB\00030_FRED_RZB_GROUPE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (27, N'001-Common\002-Fred-Ref-RZB\00040_FRED_RZB_SOCIETE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (28, N'001-Common\002-Fred-Ref-RZB\00050_FRED_RZB_ROLE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (29, N'001-Common\002-Fred-Ref-RZB\00060_FRED_RZB_CHAPITRE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (30, N'001-Common\002-Fred-Ref-RZB\00070_FRED_RZB_CODE_ABSENCE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (31, N'001-Common\002-Fred-Ref-RZB\00080_FRED_RZB_CODE_DEPLACEMENT_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (32, N'001-Common\002-Fred-Ref-RZB\00090_FRED_RZB_CODE_MAJORATION_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (33, N'001-Common\002-Fred-Ref-RZB\00100_FRED_RZB_CODE_ZONE_DEPLACEMENT_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (34, N'001-Common\002-Fred-Ref-RZB\00110_FRED_RZB_ETABLISSEMENT_PAIE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (35, N'001-Common\002-Fred-Ref-RZB\00120_FRED_RZB_ETABLISSEMENT_COMPTABLE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (36, N'001-Common\002-Fred-Ref-RZB\00130_FRED_RZB_JOURNAL_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (37, N'001-Common\002-Fred-Ref-RZB\00140_FRED_RZB_NATURE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (38, N'001-Common\002-Fred-Ref-RZB\00150_FRED_RZB_PRIME_V1.sql', N'2018-01-11 16:52:51')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (39, N'001-Common\002-Fred-Ref-RZB\00160_FRED_RZB_SOCIETE_DEVISE_V1.sql', N'2018-01-11 16:52:51')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (40, N'001-Common\002-Fred-Ref-RZB\00170_FRED_RZB_SOUS_CHAPITRE_V1.sql', N'2018-01-11 16:52:51')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (41, N'001-Common\002-Fred-Ref-RZB\00180_FRED_RZB_RESSOURCE_V1.sql', N'2018-01-11 16:52:51')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (42, N'001-Common\002-Fred-Ref-RZB\00190_FRED_RZB_PARAMETRE_V1.sql', N'2018-01-11 16:52:51')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (43, N'001-Common\002-Fred-Ref-RZB\00200_FRED_RZB_ETABLISSEMENT_PAIE_V2.sql', N'2018-01-11 16:52:51')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (43, N'002-Dev\001-Fred-FakeData\0001_FRED_FakeData_EXTERNALDIRECTORY_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (44, N'002-Dev\001-Fred-FakeData\0002_FRED_FakeData_ORGANISATION_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (45, N'002-Dev\001-Fred-FakeData\0003_FRED_FakeData_FOURNISSEUR_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (46, N'002-Dev\001-Fred-FakeData\0004_FRED_FakeData_SOCIETE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (47, N'002-Dev\001-Fred-FakeData\0005_FRED_FakeData_CI_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (48, N'002-Dev\001-Fred-FakeData\0006_FRED_FakeData_PERSONNEL_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (49, N'002-Dev\001-Fred-FakeData\0007_FRED_FakeData_CI_SOCIETE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (50, N'002-Dev\001-Fred-FakeData\0008_FRED_FakeData_UTILISATEUR_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (51, N'002-Dev\001-Fred-FakeData\0009_FRED_FakeData_BUDGET_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (52, N'002-Dev\001-Fred-FakeData\0010_FRED_FakeData_BUDGET_REVISION_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (53, N'002-Dev\001-Fred-FakeData\0012_FRED_FakeData_CI_DEVISE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (54, N'002-Dev\001-Fred-FakeData\0013_FRED_FakeData_CARBURANT_ORGANISATION_DEVISE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (55, N'002-Dev\001-Fred-FakeData\0014_FRED_FakeData_CI_PRIME_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (56, N'002-Dev\001-Fred-FakeData\0015_FRED_FakeData_SOCIETE_DEVISE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (57, N'002-Dev\001-Fred-FakeData\0016_FRED_FakeData_LES_ERO_TEMPO_TODELETE_AffectationPersonnelUtilisateur_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (58, N'002-Dev\001-Fred-FakeData\0017_FRED_FakeData_MATERIEL_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (59, N'002-Dev\001-Fred-FakeData\0018_FRED_FakeData_TACHE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (60, N'002-Dev\001-Fred-FakeData\0019_FRED_FakeData_COMMANDE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (61, N'002-Dev\001-Fred-FakeData\0020_FRED_FakeData_COMMANDE_LIGNE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (62, N'002-Dev\001-Fred-FakeData\0021_FRED_FakeData_DEPENSE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (63, N'002-Dev\001-Fred-FakeData\0022_FRED_FakeData_PERSONNEL_FOURNISSEUR_SOCIETE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (64, N'002-Dev\001-Fred-FakeData\0023_FRED_FakeData_RESSOURCE_TACHE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (65, N'002-Dev\001-Fred-FakeData\0024_FRED_FakeData_SOCIETE_RESSOURCE_NATURE_V1.sql', N'2018-01-11 16:52:50')
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (66, N'002-Dev\001-Fred-FakeData\0025_FRED_FakeData_UTILISATEUR_ROLE_ORGANISATION_DEVISE_V1.sql', N'2018-01-11 16:52:50')
SET IDENTITY_INSERT [dbo].[__ReferentialHistory] OFF

-- INSERTION DES ROLES
-- APRES LORS DE LA MIGRATION LE SCRIPT 00050_FRED_RZB_ROLE_V2.sql VA ETRE JOUEE ET ON REMETTRA LES ROLES



