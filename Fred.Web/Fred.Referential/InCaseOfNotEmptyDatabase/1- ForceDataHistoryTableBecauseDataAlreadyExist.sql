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
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (1, N'001-Common\001-Fred-Ref\0001_FRED_UNITE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (2, N'001-Common\001-Fred-Ref\0002_FRED_CARBURANT_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (3, N'001-Common\001-Fred-Ref\0003_FRED_CI_SOCIETESPARTENAIRES_TYPES_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (4, N'001-Common\001-Fred-Ref\0004_FRED_COMMANDE_TYPE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (5, N'001-Common\001-Fred-Ref\0005_FRED_DEPENSE_STATUT_RAPPROCHEMENT_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (6, N'001-Common\001-Fred-Ref\0006_FRED_DEVISE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (7, N'001-Common\001-Fred-Ref\0007_FRED_EXTERNALDIRECTORY_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (8, N'001-Common\001-Fred-Ref\0008_FRED_TYPE_ORGANISATION_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (9, N'001-Common\001-Fred-Ref\0009_FRED_ORGANISATION_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (10, N'001-Common\001-Fred-Ref\0010_FRED_HOLDING_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (11, N'001-Common\001-Fred-Ref\0011_FRED_POLE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (12, N'001-Common\001-Fred-Ref\0012_FRED_GROUPE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (13, N'001-Common\001-Fred-Ref\0013_FRED_PAYS_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (14, N'001-Common\001-Fred-Ref\0014_FRED_RAPPORT_STATUT_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (15, N'001-Common\001-Fred-Ref\0015_FRED_ROLE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (16, N'001-Common\001-Fred-Ref\0016_FRED_STATUT_COMMANDE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (17, N'001-Common\001-Fred-Ref\0017_FRED_TYPE_RESSOURCE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (18, N'001-Common\001-Fred-Ref\0018_FRED_SOCIETE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (19, N'001-Common\001-Fred-Ref\0019_FRED_PARAMETRE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (20, N'001-Common\001-Fred-Ref\0020_FRED_PERSONNEL_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (21, N'001-Common\001-Fred-Ref\0021_FRED_UTILISATEUR_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (22, N'001-Common\001-Fred-Ref\0022_FRED_IMAGE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (23, N'001-Common\002-Fred-Ref-RZB\0001_FRED_RZB_ORGANISATION_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (24, N'001-Common\002-Fred-Ref-RZB\0002_FRED_RZB_ORGANISATION_GENERIQUE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (25, N'001-Common\002-Fred-Ref-RZB\0003_FRED_RZB_GROUPE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (26, N'001-Common\002-Fred-Ref-RZB\0004_FRED_RZB_ROLE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (27, N'001-Common\002-Fred-Ref-RZB\0005_FRED_RZB_CHAPITRE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (28, N'001-Common\002-Fred-Ref-RZB\0006_FRED_RZB_SOCIETE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (29, N'001-Common\002-Fred-Ref-RZB\0007_FRED_RZB_CODE_ABSENCE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (30, N'001-Common\002-Fred-Ref-RZB\0008_FRED_RZB_CODE_DEPLACEMENT_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (31, N'001-Common\002-Fred-Ref-RZB\0009_FRED_RZB_CODE_MAJORATION_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (32, N'001-Common\002-Fred-Ref-RZB\0010_FRED_RZB_CODE_ZONE_DEPLACEMENT_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (33, N'001-Common\002-Fred-Ref-RZB\0011_FRED_RZB_ETABLISSEMENT_PAIE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (34, N'001-Common\002-Fred-Ref-RZB\0012_FRED_RZB_ETABLISSEMENT_COMPTABLE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (35, N'001-Common\002-Fred-Ref-RZB\0013_FRED_RZB_JOURNAL_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (36, N'001-Common\002-Fred-Ref-RZB\0014_FRED_RZB_NATURE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (37, N'001-Common\002-Fred-Ref-RZB\0015_FRED_RZB_PRIME_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (38, N'001-Common\002-Fred-Ref-RZB\0016_FRED_RZB_SOCIETE_DEVISE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (39, N'001-Common\002-Fred-Ref-RZB\0017_FRED_RZB_SOUS_CHAPITRE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (40, N'001-Common\002-Fred-Ref-RZB\0018_FRED_RZB_RESSOURCE_V1.sql', convert(datetime, '20171023 23:50:59'))
INSERT INTO [dbo].[__ReferentialHistory] ([Id], [ScriptName], [Applied]) VALUES (41, N'001-Common\002-Fred-Ref-RZB\0019_FRED_RZB_PARAMETRE_V1.sql', convert(datetime, '20171023 23:50:59'))
SET IDENTITY_INSERT [dbo].[__ReferentialHistory] OFF
