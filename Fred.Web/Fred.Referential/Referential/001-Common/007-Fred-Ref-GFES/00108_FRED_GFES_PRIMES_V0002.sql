/****** Script d'alimentation du Référentiel des primes Groupe FES  ******/

-- Supprimer les doublons
 DELETE FROM [dbo].[FRED_PRIME] where GroupeId IS NOT NULL
--

DECLARE @GroupFESId int;
DECLARE @SocieteId int;
DECLARE @All  int =0;
DECLARE @Ouvrier int =1;
DECLARE @EtamIac int =2;
SET @GroupFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFES')

--Insert code absence with societeId null

INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('GDI','IGD IDF',7,1,0,1,null,GETDATE(),GETDATE(),0,@All,@GroupFESId)
INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('GDP','IGD Province',7,1,0,1,null,GETDATE(),GETDATE(),0,@All,@GroupFESId)
INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('IR','Indemnité Repas',7,1,0,1,null,GETDATE(),GETDATE(),0,@EtamIac,@GroupFESId)
INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('TR','Titres restaurant',7,1,0,1,null,GETDATE(),GETDATE(),0,@EtamIac,@GroupFESId)
INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('AE','Prime égouts (journalière)',7,1,0,1,null,GETDATE(),GETDATE(),0,@All,@GroupFESId)
INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('INS','Prime insalubrité (journalière)',7,1,0,1,null,GETDATE(),GETDATE(),0,@All,@GroupFESId)
INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('ES','Prime salissure (journalière)',7,1,0,1,null,GETDATE(),GETDATE(),0,@All,@GroupFESId)

--Update Data  that exist


DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT soc.SocieteId FROM FRED_SOCIETE soc WHERE soc.GroupeId = @GroupFESId

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @SocieteId
WHILE @@FETCH_STATUS = 0
BEGIN 
  
  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='GDI' AND p.SocieteId=@SocieteId))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('GDI','IGD IDF',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,@All,@GroupFESId)
  END
	ELSE 
  BEGIN
		UPDATE FRED_PRIME  SET TargetPersonnel = @All, GroupeId = @GroupFESId, PrimeType = 0  where Code='GDI' AND SocieteId=@SocieteId
  END

	---------

  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='GDP' AND p.SocieteId=@SocieteId  ))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('GDP','IGD Province',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,@All,@GroupFESId)
  END
	ELSE 
  BEGIN
		UPDATE FRED_PRIME  SET TargetPersonnel = @All, GroupeId = @GroupFESId, PrimeType = 0  where Code='GDP' AND SocieteId=@SocieteId
  END

	---------

  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='IR' AND p.SocieteId=@SocieteId  ))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('IR','Indemnité Repas',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,@EtamIac,@GroupFESId)
  END
	ELSE 
  BEGIN
		UPDATE FRED_PRIME  SET TargetPersonnel = @EtamIac, GroupeId = @GroupFESId, PrimeType = 0  where Code='IR' AND SocieteId=@SocieteId
  END

	---------

  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='TR' AND p.SocieteId=@SocieteId  ))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('TR','Titres restaurant',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,@EtamIac,@GroupFESId)
  END
	ELSE 
  BEGIN
		UPDATE FRED_PRIME  SET TargetPersonnel = @EtamIac, GroupeId = @GroupFESId, PrimeType = 0  where Code='TR' AND SocieteId=@SocieteId
  END

	---------

  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='AE' AND p.SocieteId=@SocieteId  ))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('AE','Prime égouts (journalière)',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,@All,@GroupFESId)
  END
	ELSE 
  BEGIN
		UPDATE FRED_PRIME  SET TargetPersonnel = @All, GroupeId = @GroupFESId, PrimeType = 0  where Code='AE' AND SocieteId=@SocieteId
  END

	---------

  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='INS' AND p.SocieteId=@SocieteId  ))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('INS','Prime insalubrité (journalière)',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,@All,@GroupFESId)
  END
	ELSE 
  BEGIN
		UPDATE FRED_PRIME  SET TargetPersonnel = @All, GroupeId = @GroupFESId, PrimeType = 0  where Code='INS' AND SocieteId=@SocieteId
  END

	---------
  
  IF(NOT EXISTS (SELECT p.PrimeId FROM FRED_PRIME p WHERE p.Code='ES' AND p.SocieteId=@SocieteId  ))
  BEGIN
	INSERT INTO FRED_PRIME (Code,Libelle,NombreHeuresMax,Actif,PrimePartenaire,Publique,SocieteId,DateCreation,DateModification,PrimeType,TargetPersonnel,GroupeId)
					VALUES ('ES','Prime salissure (journalière)',7,1,0,1,@SocieteId,GETDATE(),GETDATE(),0,@All,@GroupFESId)
  END
	ELSE 
  BEGIN
		UPDATE FRED_PRIME  SET TargetPersonnel = @All, GroupeId = @GroupFESId, PrimeType = 0  where Code='ES' AND SocieteId=@SocieteId
  END

  FETCH NEXT FROM MY_CURSOR INTO @SocieteId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR
 