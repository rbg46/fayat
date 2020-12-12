-- =======================================================================================================================================
-- Author:		Yannick DEFAY 06/03/2019
--
-- Description:
--      - Ajoute les chemins vers le CGA de RZB
--
-- =======================================================================================================================================

BEGIN TRAN
    -- Ajout des CGA à la table FRED_IMAGE
    IF NOT EXISTS ( SELECT * FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_FOURNITURES_RZB.docx' AND Type = 3)
    BEGIN
        INSERT INTO FRED_IMAGE (Path,Credit,Type,IsDefault)
        VALUES ('\medias\app\societe\CGA\RZB\CGA_FOURNITURES_RZB.docx',null,3,0)
    END
    IF NOT EXISTS ( SELECT * FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_LOCATION_RZB.docx' AND Type = 3)
    BEGIN
        INSERT INTO FRED_IMAGE (Path,Credit,Type,IsDefault)
        VALUES ('\medias\app\societe\CGA\RZB\CGA_LOCATION_RZB.docx',null,3,0)
    END
    IF NOT EXISTS ( SELECT * FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_PRESTATION_RZB.docx' AND Type = 3)
    BEGIN
        INSERT INTO FRED_IMAGE (Path,Credit,Type,IsDefault)
        VALUES ('\medias\app\societe\CGA\RZB\CGA_PRESTATION_RZB.docx',null,3,0)
    END

	-- Sociétés RZB (Hors MOULIN BTP)
    UPDATE S
    SET CGAFournitureId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_FOURNITURES_RZB.docx'),
	    CGALocationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_LOCATION_RZB.docx'),
	    CGAPrestationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_PRESTATION_RZB.docx'),
	    PiedDePage = 'RAZEL-BEC S.A.S., Société par Actions Simplifiées, au capital de 20 000 000€ - 562 136 036 R.C.S. Evry - TVA FR 70 562 136 036\nSiège Social: 3, rue René Razel - Christ de Saclay - 91892 Orsay Cedex - Tel: 01 69 85 69 85 - Fax: 01 60 19 06 45 - razel-bec.com'
    FROM FRED_SOCIETE S, FRED_GROUPE G
    WHERE S.GroupeId = G.GroupeId AND G.Code = 'GRZB' AND S.CodeSocieteComptable != '550'

	-- Société MOULIN BTP
	UPDATE S
    SET CGAFournitureId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_FOURNITURES_RZB.docx'),
	    CGALocationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_LOCATION_RZB.docx'),
	    CGAPrestationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_PRESTATION_RZB.docx'),
	    PiedDePage = 'MOULIN BTP S.A.S, Société par Actions Simplifiées, au Capital de 5 052 400 € - APE 4312 B – RCS Vienne 413 838 830 –TVA FR 19 413 838 830\nSiège Social : 38 Petite rue de la plaine – CS 13004 – 38307 BOURGOIN-JALLIEU CEDEX – Tel : 04.74.43.69.10 – Fax : 04.74.43.84.28'
    FROM FRED_SOCIETE S, FRED_GROUPE G
    WHERE S.GroupeId = G.GroupeId AND G.Code = 'GRZB' AND S.CodeSocieteComptable = '550'
COMMIT TRAN