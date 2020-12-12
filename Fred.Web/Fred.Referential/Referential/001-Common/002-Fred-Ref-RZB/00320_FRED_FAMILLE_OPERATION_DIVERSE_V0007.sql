-- création d'une table provisoire qui contient les nouvelles familles
CREATE TABLE #FAMILLES_OD
(
	[Code] [nvarchar](6) NULL,
	[Libelle] [nvarchar](250) NULL,
	[SocieteId] [int] NOT NULL,
	[DateCreation] [datetime] NOT NULL,
	[AuteurCreationId] [int] NULL,
	[DateModification] [datetime] NULL,
	[AuteurModificationId] [int] NULL,
	[IsAccrued] [bit] NOT NULL,
	[MustHaveOrder] [bit] NOT NULL,
	[IsValued] [bit] NOT NULL,
	[TacheId] [int] NOT NULL,
	[RessourceId] [int] NOT NULL,
	[CategoryValorisationId] [int] NULL
)

INSERT INTO #FAMILLES_OD VALUES
('RCT', 'RECETTES (Hors explo et CB)', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0),
('MO', 'DEBOURSE MAIN D''OEUVRE (Hors Intérim)', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0),
('ACH', 'DEBOURSE ACHATS AVEC COMMANDE (y compris Intérim)', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0),
('MIT', 'DEBOURSE MATERIEL INTERNE', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0),
('MI', 'AMMORTISSEMENT', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0),
('OTH', 'AUTRES DEPENSES SANS COMMANDE', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0),
('FG', 'FRAIS GENERAUX (Hors CB)', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0),
('OTHD', 'AUTRES HORS DEBOURS', 1, GETDATE(), 1, NULL, NULL, 0, 0, 1, 3470, 975, 0)

-- Mise à jour des familles existantes
UPDATE OD SET OD.Libelle = FT.Libelle, OD.DateModification = GETDATE()
FROM FRED_FAMILLE_OPERATION_DIVERSE OD 
INNER JOIN #FAMILLES_OD FT ON OD.Code  = FT.Code AND OD.SocieteId = FT.SocieteId

-- Insertion des nouvelles familles
INSERT INTO FRED_FAMILLE_OPERATION_DIVERSE (Code, Libelle, SocieteId, DateCreation, AuteurCreationId, DateModification, AuteurModificationId, IsAccrued, MustHaveOrder, IsValued, TacheId, RessourceId, CategoryValorisationId)
SELECT FT.Code, FT.Libelle, FT.SocieteId, FT.DateCreation, FT.AuteurCreationId, FT.DateModification, FT.AuteurModificationId, FT.IsAccrued, FT.MustHaveOrder, FT.IsValued, FT.TacheId, FT.RessourceId, FT.CategoryValorisationId 
FROM FRED_FAMILLE_OPERATION_DIVERSE OD 
RIGHT JOIN #FAMILLES_OD FT ON OD.Code  = FT.Code AND OD.SocieteId = FT.SocieteId
WHERE OD.Code IS NULL