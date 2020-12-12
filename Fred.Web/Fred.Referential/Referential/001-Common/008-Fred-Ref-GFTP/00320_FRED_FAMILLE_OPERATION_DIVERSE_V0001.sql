BEGIN
	
	DECLARE @TacheMO			int, @TacheAchat		int, @TacheMateriel			int,  @TacheImmo			int, @TacheAutre int, @TacheRecette int, @SocieteId int, @GroupeId int, @Ciid int
	DECLARE @RessourceMo		int, @RessourceAchat	int, @RessourceMateriel		int,  @RessourceAutre		int
	DECLARE @ChapitreMo			int, @ChapitreAchat		int, @ChapitreMateriel		int,  @ChapitreAutre		int
	DECLARE @CodeChapitreMo		int, @CodeChapitreAchat int, @CodeChapitreMateriel	int,  @CodeChapitreAutre	int
	DECLARE @SousChapitreMo		int, @SousChapitreAchat int, @SousChapitreMateriel	int,  @SousChapitreAutre	int

	select top 1 @SocieteId			= SocieteId from FRED_SOCIETE	where CODE ='0001'
	select top 1 @GroupeId			= GroupeId  from fred_groupe	where Code = 'GFTP'
	select top 1 @Ciid				= Ciid		from FRED_CI		where 1=1 AND SocieteId = @SocieteId

	INSERT INTO FRED_TACHE (ParentId,CiId,TacheType,DateCreation,Active,Niveau,TacheParDefaut,Libelle,Code)
	VALUES (4,@Ciid,3,GETDATE(),1,3,0,'ECART MO ENCADREMENT','999991')

	INSERT INTO FRED_TACHE (ParentId,CiId,TacheType,DateCreation,Active,Niveau,TacheParDefaut,Libelle,Code)
	VALUES (4,@Ciid,3,GETDATE(),1,3,0,'ECART ACHAT','999994')

	INSERT INTO FRED_TACHE (ParentId,CiId,TacheType,DateCreation,Active,Niveau,TacheParDefaut,Libelle,Code)
	VALUES (4,@Ciid,3,GETDATE(),1,3,0,'ECART MATERIEL','999993')

	INSERT INTO FRED_TACHE (ParentId,CiId,TacheType,DateCreation,Active,Niveau,TacheParDefaut,Libelle,Code)
	VALUES (4,@Ciid,3,GETDATE(),1,3,0,'ECART MATERIEL IMMOBILISE','999998')

	INSERT INTO FRED_TACHE (ParentId,CiId,TacheType,DateCreation,Active,Niveau,TacheParDefaut,Libelle,Code)
	VALUES (4,@Ciid,3,GETDATE(),1,3,0,'ECART AUTRE FRAIS','999995')
		
	INSERT INTO FRED_TACHE (ParentId,CiId,TacheType,DateCreation,Active,Niveau,TacheParDefaut,Libelle,Code)
	VALUES (4,@Ciid,3,GETDATE(),1,3,0,'ECART RECETTES','999999')

	
	select top 1 @TacheMO			= TacheId	from FRED_TACHE		where Libelle = 'ECART MO ENCADREMENT' AND Ciid = @Ciid
	select top 1 @TacheAchat		= TacheId	from FRED_TACHE		where Libelle = 'ECART ACHAT' AND Ciid = @Ciid
	select top 1 @TacheMateriel		= TacheId	from FRED_TACHE		where Libelle = 'ECART MATERIEL' AND Ciid = @Ciid
	select top 1 @TacheImmo			= TacheId	from FRED_TACHE		where Libelle = 'Ecart Materiel Immobilisé' AND Ciid = @Ciid
	select top 1 @TacheAutre		= TacheId	from FRED_TACHE		where Libelle = 'ECART AUTRE FRAIS' AND Ciid = @Ciid
	select top 1 @TacheRecette		= TacheId	from FRED_TACHE		where Libelle = 'ECART RECETTES' AND Ciid = @Ciid


	IF NOT EXISTS(SELECT * FROM FRED_RESSOURCE WHERE CODE = 'OD-PERSO-99')
	BEGIN 
		INSERT INTO FRED_RESSOURCE (Code,Libelle,SousChapitreId,TypeRessourceId,Active,DateCreation,AuteurCreationId)
		VALUES ('OD-PERSO-99','OD MAIN D''OEUVRE DIVERSES',@SousChapitreMo,2,1,Getdate(),1)
	END

	IF NOT EXISTS(SELECT * FROM FRED_RESSOURCE WHERE CODE = 'OD-MAT-99')
	BEGIN 
		INSERT INTO FRED_RESSOURCE (Code,Libelle,SousChapitreId,TypeRessourceId,Active,DateCreation,AuteurCreationId)
		VALUES ('OD-MAT-99','OD MATERIEL',@SousChapitreMateriel,2,1,Getdate(),1)
	END

	IF NOT EXISTS(SELECT * FROM FRED_RESSOURCE WHERE CODE = 'OD-ACH-99')
	BEGIN 
		INSERT INTO FRED_RESSOURCE (Code,Libelle,SousChapitreId,TypeRessourceId,Active,DateCreation,AuteurCreationId)
		VALUES ('OD-ACH-99','OD ACHATS FOURNITURES',@SousChapitreAchat,2,1,Getdate(),1)
	END

	IF NOT EXISTS(SELECT * FROM FRED_RESSOURCE WHERE CODE = 'OD-DIVERS-99')
	BEGIN 
		INSERT INTO FRED_RESSOURCE (Code,Libelle,SousChapitreId,TypeRessourceId,Active,DateCreation,AuteurCreationId)
		VALUES ('OD-DIVERS-99','OD AUTRE FRAIS',@SousChapitreAutre,2,1,Getdate(),1)
	END

	select top 1 @RessourceMo		= RessourceId	from FRED_RESSOURCE WHERE 1=1 AND Code = 'OD-PERSO-99'
	select top 1 @RessourceAchat	= RessourceId	from FRED_RESSOURCE WHERE 1=1 AND Code = 'OD-ACH-99'
	select top 1 @RessourceMateriel = RessourceId	from FRED_RESSOURCE WHERE 1=1 AND Code = 'OD-MAT-99'
	select top 1 @RessourceAutre	= RessourceId	from FRED_RESSOURCE WHERE 1=1 AND Code = 'OD-DIVERS-99'
	
	
	IF(@TacheMO IS NOT NULL AND @RessourceMo IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MO POINTEE (Hors Interim)' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values   (N'MO',N'MO POINTEE (Hors Interim)',@SocieteId,Getdate(),1,NULL,NULL,1,0,1,@TacheMO,@RessourceMo,0)
		END
	END

	IF(@TacheAchat IS NOT NULL AND @RessourceAchat IS NOT NULL) 
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'ACHAT AVEC COMMANDE (Y compris interim)' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values   (N'ACH',N'ACHAT AVEC COMMANDE (Y compris interim)',@SocieteId,Getdate(),1,NULL,NULL,1,1,0,@TacheAchat,@RessourceAchat,NULL)
		END
	END

	IF(@TacheMateriel IS NOT NULL AND @RessourceMateriel IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MATERIEL INTERNE POINTE' AND SocieteId = @SocieteId )
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])	
			Values   (N'MIT',N'MATERIEL INTERNE POINTE',@SocieteId,Getdate(),1,NULL,NULL,1,0,1,@TacheMateriel,@RessourceMateriel,1)
		END
	END

	IF(@TacheImmo IS NOT NULL AND @RessourceAchat IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MATERIEL IMMOBILISE' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values  (N'MI',N'MATERIEL IMMOBILISE',@SocieteId,Getdate(),1,NULL,NULL,1,0,1,@TacheImmo,@RessourceAchat,NULL)
		END
	END

	IF(@TacheAutre IS NOT NULL AND @RessourceAutre IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'AUTRE DEPENSES SANS COMMANDE' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values (N'OTH',N'AUTRE DEPENSES SANS COMMANDE',@SocieteId,Getdate(),1,NULL,NULL,1,0,0,@TacheAutre,@RessourceAutre,NULL)
		END
	END

	IF(@TacheRecette IS NOT NULL AND @RessourceAutre IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'RECETTES' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values (N'RCT',N'RECETTES',@SocieteId,Getdate(),1,NULL,NULL,1,0,0,@TacheRecette,@RessourceAutre,NULL)
		END
	END
END