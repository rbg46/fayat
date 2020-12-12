-- --------------------------------------------------
-- Prix et prix chauffeur par défaut pour les barèmes exploitation
-- ------------------------------------------------

SET IDENTITY_INSERT [dbo].[FRED_PARAMETRE] ON;
INSERT INTO FRED_PARAMETRE (ParametreId,Code,Libelle,Valeur,GroupeId) VALUES (3,3,'Barème exploitation - prix par défaut','0.01',NULL)
INSERT INTO FRED_PARAMETRE (ParametreId,Code,Libelle,Valeur,GroupeId) VALUES (4,4,'Barème exploitation - prix chauffeur par défaut','0.01',NULL)
SET IDENTITY_INSERT [dbo].[FRED_PARAMETRE] OFF;