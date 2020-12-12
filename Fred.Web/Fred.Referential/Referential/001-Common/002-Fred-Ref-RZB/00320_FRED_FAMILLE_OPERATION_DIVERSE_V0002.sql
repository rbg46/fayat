BEGIN
	DECLARE @TacheMO									int, @TacheAchat						int, @TacheMateriel											int,  @TacheImmo								 int, @TacheAutre int, @SocieteId int = 1
	DECLARE @RessourceMo					int, @RessourceAchat				int, @RessourceMateriel					int,  @RessourceAutre				int
	DECLARE @ChapitreMo						int, @ChapitreAchat					int, @ChapitreMateriel						int,  @ChapitreAutre					int
	DECLARE @CodeChapitreMo		int, @CodeChapitreAchat int, @CodeChapitreMateriel		int,  @CodeChapitreAutre int
	DECLARE @SousChapitreMo		int, @SousChapitreAchat int, @SousChapitreMateriel		int,  @SousChapitreAutre int

	/*Récupération des tâches par défaut des Famille d'OD*/
	select top 1 @TacheMO								= TacheId from FRED_TACHE where Libelle = 'ECART MO ENCADREMENT'
	select top 1 @TacheAchat					= TacheId from FRED_TACHE where Libelle = 'ECART ACHAT'
	select top 1 @TacheMateriel		= TacheId from FRED_TACHE where Libelle = 'ECART MATERIEL'
	select top 1 @TacheImmo						= TacheId from FRED_TACHE where Libelle = 'Ecart Matériel Immobilisé'
	select top 1 @TacheAutre					= TacheId from FRED_TACHE where Libelle = 'ECART AUTRE FRAIS'

	/*Récupération des Chapitre*/
	select @ChapitreMo							= ChapitreId, @CodeChapitreMo							= Code	from FRED_CHAPITRE Where  Code = '12' And GroupeId = 1
	select @ChapitreAchat				= ChapitreId, @CodeChapitreAchat				= Code	from FRED_CHAPITRE Where  Code = '30' And GroupeId = 1
	select @ChapitreMateriel	= ChapitreId, @CodeChapitreMateriel = Code	from FRED_CHAPITRE Where  Code = '20' And GroupeId = 1
	select @ChapitreAutre				= ChapitreId, @CodeChapitreAutre				= Code	from FRED_CHAPITRE Where  Code = '60' And GroupeId = 1
	
	/*Récupération des Sous-Chapitre*/
	select @SousChapitreMo							= SousChapitreId from FRED_SOUS_CHAPITRE Where  Code = CAST(@CodeChapitreMo  AS VARCHAR)+ '-999'
	select @SousChapitreAchat				= SousChapitreId from FRED_SOUS_CHAPITRE Where  Code = CAST(@CodeChapitreAchat  AS VARCHAR)+ '-999'
	select @SousChapitreMateriel	= SousChapitreId from FRED_SOUS_CHAPITRE Where  Code = CAST(@CodeChapitreMateriel  AS VARCHAR)+ '-999'
	select @SousChapitreAutre				= SousChapitreId from FRED_SOUS_CHAPITRE Where  Code = CAST(@CodeChapitreAutre  AS VARCHAR)+ '-999'
	
	/*Création des Sous-Chapitre si inexistant*/
	IF(@SousChapitreMo IS NULL)
	BEGIN 
		Insert Into FRED_SOUS_CHAPITRE(Code,Libelle,ChapitreId, DateCreation, AuteurCreationId)
		Values (CAST(@CodeChapitreMo  AS VARCHAR)+ '-999', 'OD MAIN D''OEUVRE DIVERSES',@ChapitreMo,Getdate(),1)
		SET @SousChapitreMo  = SCOPE_IDENTITY()
	END

	IF(@SousChapitreAchat IS NULL)
	BEGIN 
		Insert Into FRED_SOUS_CHAPITRE(Code,Libelle,ChapitreId, DateCreation, AuteurCreationId)
		Values (CAST(@CodeChapitreAchat  AS VARCHAR)+ '-999', 'OD ACHATS FOURNITURES',@ChapitreAchat,Getdate(),1)
		SET @SousChapitreAchat  = SCOPE_IDENTITY()
	END

	IF(@SousChapitreMateriel IS NULL)
	BEGIN 
		Insert Into FRED_SOUS_CHAPITRE(Code,Libelle,ChapitreId, DateCreation, AuteurCreationId)
		Values (CAST(@CodeChapitreMateriel  AS VARCHAR)+ '-999', 'OD MATERIEL',@ChapitreMateriel,Getdate(),1)
		SET @SousChapitreMateriel = SCOPE_IDENTITY()
	END

	IF(@SousChapitreAutre IS NULL)
	BEGIN 
		Insert Into FRED_SOUS_CHAPITRE(Code,Libelle,ChapitreId, DateCreation, AuteurCreationId)
		Values (CAST(@CodeChapitreAutre  AS VARCHAR)+ '-999', 'OD AUTRE FRAIS',@ChapitreAutre,Getdate(),1)
		SET @SousChapitreAutre  = SCOPE_IDENTITY()
	END

	/*Récupération de la ressource par défaut*/
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
	
	
	/*Création des familles d'OD*/
	IF(@TacheMO IS NOT NULL AND @RessourceMo IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MO POINTEE (Hors Interim)' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values   (N'MO',N'MO POINTEE (Hors Interim)',@SocieteId,Getdate(),1,NULL,NULL,0,0,1,@TacheMO,@RessourceMo,0)
		END
	 ELSE
		BEGIN
				Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 1 WHERE [Libelle] = 'MO POINTEE (Hors Interim)' AND SocieteId = @SocieteId
		END 
	END

	IF(@TacheAchat IS NOT NULL AND @RessourceAchat IS NOT NULL) 
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'ACHAT AVEC COMMANDE (Y compris interim)' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values   (N'ACH',N'ACHAT AVEC COMMANDE (Y compris interim)',@SocieteId,Getdate(),1,NULL,NULL,0,1,0,@TacheAchat,@RessourceAchat,NULL)
		END
		ELSE
		BEGIN
				Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 1 WHERE [Libelle] = 'ACHAT AVEC COMMANDE (Y compris interim)' AND SocieteId = @SocieteId
		END 
	END

	IF(@TacheMateriel IS NOT NULL AND @RessourceMateriel IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MATERIEL INTERNE POINTE' AND SocieteId = @SocieteId )
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])	
			Values   (N'MIT',N'MATERIEL INTERNE POINTE',@SocieteId,Getdate(),1,NULL,NULL,0,0,1,@TacheMateriel,@RessourceMateriel,1)
		END
		ELSE
		BEGIN
				Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 1  WHERE [Libelle] = 'MATERIEL INTERNE POINTE' AND SocieteId = @SocieteId
		END
	END

	IF(@TacheImmo IS NOT NULL AND @RessourceAchat IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MATERIEL IMMOBILISE' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values   (N'MI',N'MATERIEL IMMOBILISE',@SocieteId,Getdate(),1,NULL,NULL,1,0,1,@TacheImmo,@RessourceAchat,NULL)
		END
		ELSE
		BEGIN
				Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 0 WHERE [Libelle] = 'MATERIEL IMMOBILISE' AND SocieteId = @SocieteId
		END
	END

	IF(@TacheAutre IS NOT NULL AND @RessourceAutre IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'AUTRE DEPENSES SANS COMMANDE' AND SocieteId = @SocieteId)
		BEGIN 
			Insert Into FRED_FAMILLE_OPERATION_DIVERSE ([Code],[Libelle],[SocieteId],[DateCreation],[AuteurCreationId],[DateModification],[AuteurModificationId],[IsAccrued],[MustHaveOrder],[IsValued],[TacheId],[RessourceId],[CategoryValorisationId])
			Values			(N'OTH',N'AUTRE DEPENSES SANS COMMANDE',@SocieteId,Getdate(),1,NULL,NULL,1,0,0,@TacheAutre,@RessourceAutre,NULL)
		END
		ELSE
		BEGIN
				Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 0 WHERE [Libelle] = 'AUTRE DEPENSES SANS COMMANDE' AND SocieteId = @SocieteId
		END
	END
END