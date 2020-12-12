-- =======================================================================================================================================
-- Author:		Yannick DEFAY 12/03/2019
--
-- Description:
--      - Ajoute les chemins vers le CGA, le logo et le pied de page de RZB
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

	-- Société MOULIN BTP
	UPDATE S
    SET ImageLogoId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '/medias/app/societe/logotype/RAZEL-BEC.png' AND Type = 2),
        CGAFournitureId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_FOURNITURES_RZB.docx' AND Type = 3),
	    CGALocationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_LOCATION_RZB.docx' AND Type = 3),
	    CGAPrestationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\RZB\CGA_PRESTATION_RZB.docx' AND Type = 3),
	    PiedDePage = 'RAZEL-BEC S.A.S., Société par Actions Simplifiées, au capital de 20 000 000€ - 562 136 036 R.C.S. Evry - TVA FR 70 562 136 036\nSiège Social: 3, rue René Razel - Christ de Saclay - 91892 Orsay Cedex - Tel: 01 69 85 69 85 - Fax: 01 60 19 06 45 - razel-bec.com'
    FROM FRED_SOCIETE S, FRED_GROUPE G
    WHERE S.GroupeId = G.GroupeId AND G.Code = 'GRZB' AND S.Code != 'MBTP'
COMMIT TRAN