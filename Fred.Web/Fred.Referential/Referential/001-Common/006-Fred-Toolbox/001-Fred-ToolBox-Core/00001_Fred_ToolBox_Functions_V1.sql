

/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	CONVERTION DES DONNEES Oui ou Non en INt
************************************************** */
BEGIN TRY
    DROP FUNCTION dbo.FredGetBoolean;
	PRINT 'Function FredGetBoolean updated.';
END TRY
BEGIN CATCH
    PRINT 'Function FredGetBoolean did not exist.';
END CATCH
GO
CREATE FUNCTION dbo.FredGetBoolean (@flag varchar(50))  
RETURNS int  
WITH EXECUTE AS CALLER  
AS  
BEGIN  

	DECLARE @RTN_VAL INT;
	SET @RTN_VAL = 0;

     IF UPPER(@flag) = 'OUI' OR UPPER(@flag) = '1' OR UPPER(@flag) = 'YES' OR UPPER(@flag) = 'OK'
		
			SET @RTN_VAL = 1; 
		
     RETURN @RTN_VAL;  
END  
GO  
SET DATEFIRST 1;  
GO




/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	CHECK SUR LE TYPE DE CHAMP
	MODOP : SELECT dbo.FredGetContrainteField('FRED_FOURNISSEUR', 'FournisseurId')
************************************************** */
BEGIN TRY
    DROP FUNCTION dbo.FredGetContrainteField;
	PRINT 'Function FredGetContrainteField updated.';
END TRY
BEGIN CATCH
    PRINT 'Function FredGetContrainteField did not exist.';
END CATCH
GO
CREATE FUNCTION dbo.FredGetContrainteField (@table_name varchar(50), @field_name varchar(50))  
RETURNS int  
WITH EXECUTE AS CALLER  
AS  
BEGIN  
	DECLARE @RTN INT;
	SET @RTN =  0;

	DECLARE @CHECK varchar(max);
	SET @CHECK = (
			SELECT CONSTRAINT_TYPE
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
			INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS ccu
				ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
		WHERE  tc.TABLE_NAME = @table_name
				AND COLUMN_NAME = @field_name
		)
		
		IF @CHECK = 'PRIMARY KEY'
		BEGIN
			SET @RTN =  1;
		END
		RETURN @RTN;
END  
GO  
SET DATEFIRST 1;  
GO



/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	CHECK SUR LE TYPE DE CHAMP
	MODOP : SELECT dbo.FredGetForeignKey('FRED_FOURNISSEUR', 'FournisseurId')
************************************************** */
BEGIN TRY
    DROP FUNCTION dbo.FredGetForeignKey;
	PRINT 'Function FredGetForeignKey updated.';
END TRY
BEGIN CATCH
    PRINT 'Function FredGetForeignKey did not exist.';
END CATCH
GO
CREATE  FUNCTION dbo.FredGetForeignKey (@table_name varchar(50), @field_name varchar(50))  
RETURNS int  
WITH EXECUTE AS CALLER  
AS  
BEGIN  
	DECLARE @RTN INT;
	SET @RTN =  0;

	DECLARE @CHECK varchar(max);
	SET @CHECK = (
			SELECT CONSTRAINT_TYPE
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
			INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS ccu
				ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
		WHERE  tc.TABLE_NAME = @table_name
				AND COLUMN_NAME = @field_name
		)
		
		IF @CHECK = 'FOREIGN KEY'
		BEGIN
			SET @RTN =  1;
		END
		RETURN @RTN;
END  
GO  
SET DATEFIRST 1;  
GO


/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	CHECK POUR SAVOIR SI DERNIER CHAMPS pour GESTION DE LA VIRGULE
	MODOP : SELECT dbo.FredGetContrainteField('FRED_FOURNISSEUR', 'FournisseurId')
************************************************** */
BEGIN TRY
    DROP FUNCTION dbo.FredGetLastField;
	
	PRINT 'Function FredGetLastField updated.';
END TRY
BEGIN CATCH
    PRINT 'Function FredGetLastField did not exist.';
END CATCH
GO
CREATE  FUNCTION dbo.FredGetLastField (@i int, @a int)  
RETURNS char(2)  
WITH EXECUTE AS CALLER  
AS  
BEGIN  
	
	DECLARE @RTN char(2);
	SET @RTN = '  ';
	IF @i <> @a
	SET @RTN = ', ';
	RETURN @RTN;
END  
GO  
SET DATEFIRST 1;  
GO



/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	RECUPERE UN ID en fonction de l'objet
	MODOP : SELECT dbo.FredIdFromCode('SocieteId','0001')
			dbo.FredIdFromCode('GroupeID','GRP')
************************************************** */
BEGIN TRY
    DROP FUNCTION dbo.FredIdFromCode;
	PRINT 'Function FredIdFromCode updated.';
END TRY
BEGIN CATCH
    PRINT 'Function FredIdFromCode did not exist.';
END CATCH
GO
CREATE  FUNCTION dbo.FredIdFromCode (@Type as varchar(max), @Code as varchar(max))  
RETURNS int
WITH EXECUTE AS CALLER  
AS  
BEGIN  
	
	DECLARE @RTN int;
	
	IF (@Type = 'GroupeId')
	BEGIN
		SET @RTN = (SELECT GroupeId FROM FRED_GROUPE where Code = @Code);
	END


	IF (@Type = 'SocieteId')
	BEGIN
		SET @RTN = (SELECT SocieteID FROM FRED_SOCIETE where Code = @Code);
	END


	IF (@Type = 'PaysId')
	BEGIN
		SET @RTN = (SELECT PaysId FROM FRED_PAYS where Code = @Code);
	END

	RETURN @RTN;
END  
GO  
SET DATEFIRST 1;  
GO






/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	Procédure générant le template de gestion CU 
	MODOP : EXEC Fred_ToolBox_Fw_Fields_Table @table_name='FRED_NATURE', @sp_name='Fred_ToolBox_Nature'
************************************************** */
IF OBJECT_ID ( 'Fred_ToolBox_Fw_Fields_Table', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Fw_Fields_Table;  
GO  
CREATE PROCEDURE Fred_ToolBox_Fw_Fields_Table   
	@table_name nvarchar(50),
	@sp_name  nvarchar(50)
AS   

	-- CREATION DU CURSOR
	Declare A Cursor For 
		SELECT  ORDINAL_POSITION as pos, COLUMN_NAME as colonne, DATA_TYPE as type, CHARACTER_MAXIMUM_LENGTH as longueur
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = @table_name
	
	DECLARE @pos int;
	DECLARE @longueur varchar(max);
	DECLARE @colonne varchar(max);
	DECLARE @type varchar(max);

	DECLARE @END char(2);

	DECLARE @MAX INT;
	SET @MAX = (SELECT MAX(ORDINAL_POSITION) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table_name);

	PRINT '-- -------------------------------------------------- '
	PRINT '-- FRED 2017 - R4 - SEPTEMBRE 2018  '
	PRINT '-- TOOLBOX MANAGEMENT TABLE  '+@table_name+' '
	PRINT '-- MODOP : '
	

	DECLARE @Modep_Ordre nvarchar(max);
	SET @Modep_Ordre = '--    EXEC '+ @sp_name;

	Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
			
			-- CHECK SI COLONNE EST UNE CONTRAINTE
			IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) = 0
			BEGIN
			

				
				SET @END = (SELECT dbo.FredGetLastField (@max, @pos)); 
				-- Remplacement des ID par des Code

				DECLARE @_colonne nvarchar(max);
				IF (SELECT dbo.FredGetForeignKey(@table_name, @colonne)) = 1
				BEGIN
					SET @_colonne  = REPLACE(@colonne, 'Id', 'Code');
				END
				ELSE
				BEGIN
					SET @_colonne = @colonne;
				END

				SET @Modep_Ordre = @Modep_Ordre + ' @' + @_colonne  + '='' '''+ @END
			END

		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A

	PRINT  @Modep_Ordre


	PRINT '-- -------------------------------------------------- '

	PRINT ''
	PRINT ''

	PRINT 'IF OBJECT_ID ( '''+@sp_name+''', ''P'' ) IS NOT NULL   '
	PRINT 'DROP PROCEDURE '+@sp_name+'; ' 
	PRINT 'GO ' 
	PRINT 'CREATE PROCEDURE '+@sp_name+''

	PRINT ''
	PRINT ''


	PRINT '		@verbose INT =NULL,'
	PRINT '		@version INT =NULL,'




	-- BEGIN : LOOP DECLARATION VARIABLE
		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
			
			-- CHECK SI COLONNE EST UNE CONTRAINTE
			IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) = 0
			BEGIN

			DECLARE @Check_ForeignKey INT;
			SET @Check_ForeignKey = dbo.FredGetForeignKey (@table_name, @colonne);
			
			
			SET @END = (SELECT dbo.FredGetLastField (@max, @pos)); 

				IF (@type = 'nvarchar' OR @type='bit')
					BEGIN
						IF @type = 'nvarchar'

						PRINT '		@' + REPLACE(@colonne, 'Id', 'Code') +'  ' + @type + '(' + @longueur + '),'



						IF @type = 'bit'
						PRINT '		@' + REPLACE(@colonne, 'Id', 'Code') +  ' varchar(max)'
					END
				ELSE
					BEGIN
						
						IF @Check_ForeignKey = 1
						BEGIN
							PRINT '		@' + REPLACE(@colonne, 'Id', 'Code') +'  varchar(max)' + @END
						END
						ELSE
						BEGIN
							PRINT '		@' + REPLACE(@colonne, 'Id', 'Code') +'  ' + @type + @END
						END
						
					END
			END
			



		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
	-- END : LOOP DECLARATION VARIABLE










	PRINT ' AS'
	
	
	PRINT '-- DECLARATION DES VARIABLES'
	PRINT 'DECLARE @ERROR INT;';
	PRINT 'SET @ERROR =0;';

	PRINT 'DECLARE @PrimaryKeyField varchar(max);';
	PRINT 'DECLARE @PrimaryKeyValue INT;';
	DECLARE @PrimaryKeyField nvarchar(max);
	DECLARE @PrimaryKeyValue nvarchar(max);


	PRINT '		IF @verbose = 1 '
	PRINT '		BEGIN'
	PRINT '				PRINT ''------------------------------'''
	PRINT '				PRINT ''FAYAT IT - 2018 '''
	PRINT '				PRINT ''INJECTION DES '+@table_name+' (PS '+@sp_name+')'''
	PRINT '				PRINT ''------------------------------'''
	PRINT '		END'
	PRINT '		IF @version = 1 '
	PRINT '			BEGIN'
	PRINT '				PRINT ''Version 0.1'''
	PRINT '		END'

	PRINT ''
	
	
		-- BEGIN : LOOPGESTION DES BITS POUR CONVERSION OUI / NON
	PRINT '		-- ----------------------------'
	PRINT '		-- BEGIN OUI/NON'
		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
			

				IF @TYPE = 'bit'
				BEGIN
					PRINT '				DECLARE @_'+@colonne+' bit;'
					PRINT '			SET @_'+@colonne+' =(SELECT dbo.FredGetBoolean (@'+@colonne+'));'
				END
			
			
			
		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
	PRINT '		-- END OUI/NON'
	PRINT '		-- ----------------------------'
	-- END : LOOP GETSION DES BITS POUR CONVERSION OUI / NON
	
	
	PRINT ''
	PRINT ''
	PRINT '		-- ----------------------------'
	PRINT '		-- END FOREIGN KEYS'
	-- BEGIN : LOOP FOREIGN KEYS
		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
			
			-- CHECK SI COLONNE EST UNE CONTRAINTE
			IF (SELECT dbo.FredGetForeignKey(@table_name, @colonne)) = 1
			BEGIN
				PRINT '			DECLARE @' + @colonne + ' INT;'
				DECLARE @ConvertCode varchar(max);
				SET @ConvertCode = REPLACE(@colonne, 'Id', 'Code')
				
				DECLARE @_ConvertCode varchar(max);
				SET @_ConvertCode = '@'+REPLACE(@colonne, 'Id', 'Code')

				PRINT '			SET @' +  @colonne + '= dbo.FredIdFromCode('''+@colonne+''',  @'+@ConvertCode+' )'
				PRINT ''
			END
			
			
		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
	-- END : LOOP FOREIGN KEYS
	PRINT '		-- END FOREIGN KEYS'
	PRINT '		-- ----------------------------'
	PRINT ''



	-- GESTION DE LA PRIMARY KEY
	Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
			While @@Fetch_Status=0 Begin
				-- Vérification si la clé est la clé primaire
				IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) = 1
				BEGIN
					--Récupération du nom de la primary Key 
					SET @PrimaryKeyField = @colonne;
					
				END
			Fetch next From A into @pos, @colonne, @type, @longueur
		End
	Close A

	PRINT 'DECLARE @'+@PrimaryKeyField+' INT';
	PRINT 'SET @'+@PrimaryKeyField+'= '
	PRINT '		('
	PRINT '		SELECT '+@PrimaryKeyField+' FROM '+@table_name+' WHERE code = @code' 
	PRINT '		)'
	

	PRINT 'IF @ERROR = 0'
	PRINT 'BEGIN'

	-- CONTROLE SI LA VALEUR DE PM EST OK
	PRINT 'IF @'+@PrimaryKeyField+' IS NULL' 
	PRINT '		BEGIN' 

	-- BEGIN : DECLARATION ORDRE INSERT DECLARATION DES CHAMPS
		PRINT '			-- ---------------------------- '
		PRINT '			-- ORDRE AJOUT '
		PRINT '			-- ---------------------------- '
		PRINT '			INSERT INTO '+@table_name+' ('
		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
					
					
					-- CHECK SI COLONNE EST UNE CONTRAINTE
					
					IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) = 0
					BEGIN
					SET @END = (SELECT dbo.FredGetLastField (@max, @pos));
					PRINT '				'+@colonne +@END
					END
		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
		
	-- END : DECLARATION ORDRE INSERT DECLARATION DES CHAMPS
	
	PRINT '		)'
	PRINT '		VALUES '
	PRINT '		('

	-- BEGIN : DECLARATION ORDRE INSERT VALUES

		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
				IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) = 0
				BEGIN
					SET @END = (SELECT dbo.FredGetLastField (@max, @pos));
					PRINT '				@' + @colonne +@END
				END
		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
	
	-- END : DECLARATION ORDRE INSERT VALUES

	PRINT '		);'

	PRINT '		IF @verbose = 1 '
	PRINT '			BEGIN'
	PRINT '				PRINT ''INFO : Ajout réalisée'''
	PRINT '			END'


	PRINT '		END'
	PRINT '	ELSE'
	PRINT '		BEGIN'




		-- BEGIN : REPRISE DES ANCIENNES VALEURS
		PRINT ' -- REPRISE DES ANCIENNES VALEURS'
		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
					
			IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) = 0
				BEGIN
					PRINT '		IF(@'+@colonne+' IS NULL)'
					PRINT '		BEGIN'
					PRINT '			SET @'+@colonne+' = (SELECT '+@colonne+' FROM '+@table_name+' WHERE ' + @PrimaryKeyField + ' = @'+@PrimaryKeyField+')'
					PRINT '		END'
				END
			
		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
		-- END : REPRISE DES ANCIENNES VALEURS
		PRINT ' -- REPRISE DES ANCIENNES VALEURS'


		-- BEGIN : DECLARATION ORDRE UPDATE
		PRINT '			-- ---------------------------- '
		PRINT '			-- MISE A JOUR '
		PRINT '			-- ---------------------------- '

		PRINT '				UPDATE  ' + @table_name
		PRINT '				SET'
		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
					
					
					-- CHECK SI COLONNE EST UNE CONTRAINTE
					
					IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) = 0
					BEGIN
						SET @END = (SELECT dbo.FredGetLastField (@max, @pos));
						PRINT '					' + @colonne +'= @' + @colonne + @END
					END
		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
		-- END : DECLARATION ORDRE UPDATE

		-- BEGIN : RECHERCHE CLE PRIMARY
		PRINT  ' 		WHERE '
		Open A
		Fetch next From A into @pos, @colonne, @type, @longueur
		While @@Fetch_Status=0 Begin
					
					
					-- CHECK SI COLONNE EST UNE CONTRAINTE
					
					IF (SELECT dbo.FredGetContrainteField(@table_name, @colonne)) =1
					BEGIN

						PRINT  '			' + @colonne +'= @' + @colonne
					END
		   Fetch next From A into @pos, @colonne, @type, @longueur
		End
		Close A
		-- END : DECLARATION ORDRE UPDATE


		PRINT '		IF @verbose = 1 '
		PRINT '			BEGIN'
		PRINT '				PRINT ''INFO : Mise à jour réalisée'''
		PRINT '			END'


	PRINT '		END'

	PRINT 'END'
	PRINT 'GO'
	PRINT ' -- ----------------------------------------------------------'
	PRINT ' -- FIN PROCEDURE STOCKEE '+ @sp_name +'  POUR TABLE  '+ @table_name
	PRINT ' -- ----------------------------------------------------------'

	Deallocate A
	

GO 



