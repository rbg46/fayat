--Suppression des code primes astreinte 

Declare @AstrsID INT;
Declare @AstrweID INT;

SET @AstrsID = (SELECT CodeAstreinteId from FRED_CODE_ASTREINTE WHERE Code ='ASTRS');
SET @AstrweID = (SELECT CodeAstreinteId from FRED_CODE_ASTREINTE WHERE Code ='ASTRWE');


DELETE FROM FRED_RAPPORT_LIGNE_CODE_ASTREINTE WHERE CodeAstreinteId IN (@AstrsID, @AstrweID);
DELETE FROM FRED_CODE_ASTREINTE where CodeAstreinteId IN (@AstrsID, @AstrweID);
-----------------------------------------

 --Ajout des primes Astreinte dans la table FRED_PRIME
 DECLARE @GroupFESId INT;
 DECLARE @SocieteId INT;
 SET @GroupFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFES');


 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT SocieteId FROM FRED_SOCIETE  WHERE GroupeId = @GroupFESId

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @SocieteId
WHILE @@FETCH_STATUS = 0
BEGIN 
  
  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='ASTRS' AND p.SocieteId=@SocieteId))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId,IsPrimeAstreinte)
					VALUES ('ASTRS','Astreinte du lundi au vendredi',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,0,@GroupFESId,1)
  END
	IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='ASTRWE' AND p.SocieteId=@SocieteId))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId,IsPrimeAstreinte)
					VALUES ('ASTRWE','Astreinte samedi ou dimanche',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,0,@GroupFESId,1)
  END

	FETCH NEXT FROM MY_CURSOR INTO @SocieteId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

