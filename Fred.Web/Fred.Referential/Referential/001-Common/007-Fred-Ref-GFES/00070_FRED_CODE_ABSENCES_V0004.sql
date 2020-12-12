DECLARE @SocieteId INT;
DECLARE @GroupeId  INT;
DECLARE @CodeAbsenceId INT;
Set @GroupeId = (SELECT GroupeId from FRED_GROUPE WHERE Code = 'GFES');
--------------------------------------------------------
--Update des codes d'absence 
DECLARE MyCURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
select DISTINCT l.CodeAbsenceId, c.SocieteId from FRED_RAPPORT_LIGNE l , FRED_CODE_ABSENCE ab , FRED_CI c where l.CodeAbsenceId = ab.CodeAbsenceId and ab.SocieteId is null and c.CiId = l.CiId

OPEN MyCURSOR
FETCH NEXT FROM MyCURSOR INTO @CodeAbsenceId, @SocieteId
WHILE @@FETCH_STATUS = 0
BEGIN 
 update FRED_CODE_ABSENCE set SocieteId = @SocieteId where CodeAbsenceId = @CodeAbsenceId
 FETCH NEXT FROM MyCURSOR INTO @CodeAbsenceId, @SocieteId
END
CLOSE MyCURSOR
DEALLOCATE MyCURSOR

-----------------------------------------------------
--Suppression des codes absences avec societeId null 
DELETE FROM FRED_CODE_ABSENCE where SocieteId is null and GroupeId = @GroupeId

-----------------------------------------------------
--Ajout des codes d'absence avec societeId
DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT DISTINCT SocieteId 
FROM FRED_SOCIETE where GroupeId=@GroupeId

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @SocieteId
WHILE @@FETCH_STATUS = 0
BEGIN 

IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='ANA'  AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'ANA','Absence non autorisée',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='ANP' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'ANP','Absence autorisée non payée',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='AP' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'AP','Absence payée',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='I' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'I','Absence intempérie',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='DEL' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'DEL','Absence délégation',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='PREAV' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'PREAV','Préavis effectué payé',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='PNE' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'PNE','Préavis non effectué payé',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='VM' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'VM','Visite médicale',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='CP' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'CP','Conge paye',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='RC' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'RC','Repos compensateur',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0); 
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='RT' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'RT','RTT',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null,0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='AJ' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'AJ','Accident de trajet',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='AT' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'AT','Accident du travail',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='ML' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'ML','Maladie non professionnelle',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='MP' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'MP','Maladie professionnelle',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='MTHN' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'MTHN','Mi-temps thérapeutique',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='CONV' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'CONV','Congé conventionnel',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='CPAR' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'CPAR','Congé parental',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='CSAB' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'CSAB','Congé sabbatique',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='CSS' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'CSS','Congé sans solde',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='MT' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'MT','Maternité',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId,null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='PT' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'PT','Paternité',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId, null, 0);
END
IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='RA' AND a.SocieteId = @SocieteId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,@SocieteId,'RA','Repos autorisé',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId, null, 0);
END
FETCH NEXT FROM MY_CURSOR INTO @SocieteId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR


