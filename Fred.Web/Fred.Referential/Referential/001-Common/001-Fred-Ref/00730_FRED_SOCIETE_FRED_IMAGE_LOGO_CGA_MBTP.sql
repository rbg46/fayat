-- =======================================================================================================================================
-- Author:		Yannick DEFAY 11/03/2019
--
-- Description:
--      - Ajoute les chemins vers le CGA, le logo et le pied de page de MBTP
--
-- =======================================================================================================================================

BEGIN TRAN
    -- Ajout des CGA à la table FRED_IMAGE
    IF NOT EXISTS ( SELECT * FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\MBTP\CGA_FOURNITURES_MBTP.docx' AND Type = 3)
    BEGIN
        INSERT INTO FRED_IMAGE (Path,Credit,Type,IsDefault)
        VALUES ('\medias\app\societe\CGA\MBTP\CGA_FOURNITURES_MBTP.docx',null,3,0)
    END
    IF NOT EXISTS ( SELECT * FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\MBTP\CGA_LOCATION_MBTP.docx' AND Type = 3)
    BEGIN
        INSERT INTO FRED_IMAGE (Path,Credit,Type,IsDefault)
        VALUES ('\medias\app\societe\CGA\MBTP\CGA_LOCATION_MBTP.docx',null,3,0)
    END
    IF NOT EXISTS ( SELECT * FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\MBTP\CGA_PRESTATION_MBTP.docx' AND Type = 3)
    BEGIN
        INSERT INTO FRED_IMAGE (Path,Credit,Type,IsDefault)
        VALUES ('\medias\app\societe\CGA\MBTP\CGA_PRESTATION_MBTP.docx',null,3,0)
    END

	-- Société MOULIN BTP
	UPDATE S
    SET ImageLogoId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '/medias/app/societe/logotype/MOULIN_BTP.png' AND Type = 2),
        CGAFournitureId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\MBTP\CGA_FOURNITURES_MBTP.docx' AND Type = 3),
	    CGALocationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\MBTP\CGA_LOCATION_MBTP.docx' AND Type = 3),
	    CGAPrestationId = (SELECT TOP(1) ImageId FROM FRED_IMAGE WHERE Path = '\medias\app\societe\CGA\MBTP\CGA_PRESTATION_MBTP.docx' AND Type = 3),
	    PiedDePage = 'MOULIN BTP S.A.S, Société par Actions Simplifiées, au Capital de 5 052 400 € - APE 4312 B – RCS Vienne 413 838 830 –TVA FR 19 413 838 830\nSiège Social : 38 Petite rue de la plaine – CS 13004 – 38307 BOURGOIN-JALLIEU CEDEX – Tel : 04.74.43.69.10 – Fax : 04.74.43.84.28'
    FROM FRED_SOCIETE S, FRED_GROUPE G
    WHERE S.GroupeId = G.GroupeId AND G.Code = 'GRZB' AND S.Code = 'MBTP'
COMMIT TRAN