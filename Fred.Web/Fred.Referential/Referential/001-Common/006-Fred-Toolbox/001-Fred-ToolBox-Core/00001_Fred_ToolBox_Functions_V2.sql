

/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	CONVERTION DES DONNEES Oui ou Non en INt
************************************************** */

BEGIN TRY
    DROP FUNCTION dbo.FredCodeFromId;
	PRINT 'Function FredCodeFromId updated.';
END TRY
BEGIN CATCH
    PRINT 'Function FredCodeFromId did not exist.';
END CATCH
GO
CREATE FUNCTION dbo.FredCodeFromId (@Type as varchar(max), @id  as varchar(max))  
RETURNS nvarchar(max)
WITH EXECUTE AS CALLER  
AS  
BEGIN  
	
	DECLARE @RTN nvarchar(max);
	
	IF (@Type = 'GroupeId')
	BEGIN
		SET @RTN = (SELECT Code FROM FRED_GROUPE where GroupeId = @id);
	END


	IF (@Type = 'SocieteId')
	BEGIN
		SET @RTN = (SELECT Code FROM FRED_SOCIETE where SocieteId = @id);
	END


	IF (@Type = 'PaysId')
	BEGIN
		SET @RTN = (SELECT Code FROM FRED_PAYS where PaysId = @id);
	END

	IF (@Type = 'RessourceId')
	BEGIN
		SET @RTN = (SELECT Code FROM FRED_RESSOURCE where RessourceId = @id );
	END

	IF (@Type = 'SousChapitreId')
	BEGIN
		SET @RTN = (SELECT Code FROM FRED_SOUS_CHAPITRE where SousChapitreId = @id);
	END

	IF (@Type = 'ChapitreId')
	BEGIN
		SET @RTN = (SELECT Code FROM FRED_CHAPITRE where ChapitreId = @id);
	END


	IF (@Type = 'NatureId')
	BEGIN
		SET @RTN = (SELECT Code FROM FRED_NATURE where NatureId = @id);
	END

	RETURN REPLACE(@RTN, '  ', '');
END

GO





/* *************************************************
	FRED TOOLBOX - FAYAT IT - 2018
	Procédure générant un SELECT sur une table pour un injection via la toolbox
	MODOP : EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_SOCIETE', @sp_name='Fred_ToolBox_Materiel'
************************************************** */
IF OBJECT_ID ( 'Fred_ToolBox_Fw_Extract_Datas_To_Transport', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Fw_Extract_Datas_To_Transport;  
GO  
CREATE PROCEDURE Fred_ToolBox_Fw_Extract_Datas_To_Transport   
	@table_name nvarchar(50) =NULL,
	@sp_name  nvarchar(50)=NULL, 
	@where nvarchar(max)= ' ',
	@GroupeCode  nvarchar(max)=NULL
AS   

	PRINT @table_name;



DECLARE @columnNames table(name varchar(30), type varchar(30));



insert into @columnNames  
	SELECT  c.COLUMN_NAME as name,c.DATA_TYPE as type
	FROM INFORMATION_SCHEMA.COLUMNS c
	LEFT JOIN (
				SELECT ku.TABLE_CATALOG,ku.TABLE_SCHEMA,ku.TABLE_NAME,ku.COLUMN_NAME
				FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
				INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS ku
					ON tc.CONSTRAINT_TYPE = 'PRIMARY KEY' 
					AND tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
			 )   pk 
	ON  c.TABLE_CATALOG = pk.TABLE_CATALOG
				AND c.TABLE_SCHEMA = pk.TABLE_SCHEMA
				AND c.TABLE_NAME = pk.TABLE_NAME
				AND c.COLUMN_NAME = pk.COLUMN_NAME
	WHERE c.TABLE_NAME =  @table_name
		AND (
			c.COLUMN_NAME <> 'AuteurClotureId' 
			AND c.COLUMN_NAME <> 'AuteurCreation'
			AND c.COLUMN_NAME <> 'AuteurCreationId'
			AND c.COLUMN_NAME <> 'AuteurId'
			AND c.COLUMN_NAME <> 'AuteurModification'
			AND c.COLUMN_NAME <> 'AuteurModificationId'
			AND c.COLUMN_NAME <> 'AuteurSuppression'
			AND c.COLUMN_NAME <> 'AuteurSuppressionId'
			AND c.COLUMN_NAME <> 'DateModification'
			AND c.COLUMN_NAME <> 'DateCreation'
			AND c.COLUMN_NAME <> 'DateSuppression'
			)
	AND c.ORDINAL_POSITION <> 1
	ORDER BY c.TABLE_SCHEMA,c.TABLE_NAME, c.ORDINAL_POSITION 

-- Compte le nombre d'occurence pour gestion des , dans les ordres SQL
DECLARE @CountMax  INT;
SET @CountMax = (
				SELECT  count(*) from @columnNames
				


				)
PRINT @CountMax;


DECLARE @CountRs INT; 
SET @CountRs=1;

DECLARE @dynamicCommand varchar(max) = 'select  ';
DECLARE @columnName varchar(30)
DECLARE @type varchar(30)


-- Gestion si le groupeCode est renseigné
IF(@GroupeCode IS NOT NULL)
BEGIN
	SET @dynamicCommand = @dynamicCommand + ' '' EXEC ' + @sp_name +  '  @GroupeCode='''''+ @GroupeCode +''''' ,'' +'
END
ELSE
BEGIN
	SET @dynamicCommand = @dynamicCommand + ' '' EXEC ' + @sp_name +  '  '' +'
END


DECLARE  columnNames_cursor CURSOR
FOR
select name, type from @columnNames

Open columnNames_cursor
FETCH NEXT FROM columnNames_cursor into @columnName, @type

while @@fetch_status = 0
Begin


		


		IF (@columnName='SocieteId' OR @columnName='GroupeId' OR @columnName='RessourcesId' OR @columnName='SousChapitreID' OR @columnName='ChapitreId'OR @columnName='RessourceId'OR @columnName='NatureId')
		BEGIN
			DECLARE @End varchar(max);
			SET @End = '+';

			IF (@CountRs < @CountMax )
			BEGIN
				SET @End = '+';
			END
			ELSE
			BEGIN
				SET @End = ' ';
			END


			/*Remplacement de Id par Code*/
			DECLARE @_columnName nvarchar(max);
			SET @_columnName = REPLACE(@columnName, 'Id', 'Code')
			/* Construction en appelant  la function dbo.FredCodeFromId */
			SET @dynamicCommand = @dynamicCommand + ''' @'+ @_columnName+'= ''''''+ dbo.FredCodeFromId('''++@columnName+''','+@columnName+') +'''''','''+@End+' '

		END
		ELSE
		BEGIN
			



			DECLARE @_End varchar(max);
			SET @_End = '+';

			IF (@CountRs < @CountMax )
			BEGIN
				SET @_End = '+'''''',''+';
			END
			ELSE
			BEGIN
				SET @_End = '+''''''''';
			END
			

			--REPLACE(IsNull(convert(nvarchar(max),Consommation)
			--SET @dynamicCommand = @dynamicCommand + ''' '+@columnName+'='''''' + convert(nvarchar(max),IsNull(REPLACE('+@columnName+','''''',''''''''),''''))'+@_End
					PRINT @columnName + '--' + @type
			IF (@type = 'nvarchar' OR @type = 'char' OR @type = 'varchar')
				BEGIN
					--SET @dynamicCommand = @dynamicCommand + ''' @'+@columnName+'='''''' + convert(nvarchar(max),REPLACE(IsNull('+@columnName+'  COLLATE French_CS_AS,''''),'''''''',''''''''''''))'+@_End
					SET @dynamicCommand = @dynamicCommand + ''' @'+@columnName+'='''''' + REPLACE(IsNull(convert(nvarchar(max),'+@columnName+')  COLLATE French_CS_AS,''''),'''''''','''''''''''')'+@_End
				END	
				ELSE
				BEGIN
					--SET @dynamicCommand = @dynamicCommand + ''' @'+@columnName+'='''''' + convert(nvarchar(max),REPLACE(IsNull('+@columnName+'  ,''''),'''''''',''''''''''''))'+@_End
					SET @dynamicCommand = @dynamicCommand + ''' @'+@columnName+'='''''' + REPLACE(IsNull(convert(nvarchar(max),'+@columnName+') ,''''),'''''''','''''''''''')'+@_End
				END
		
		
		END

		/*Incrementation du Compteur*/
		SET @CountRs = @CountRs + 1;


	
	FETCH NEXT FROM columnNames_cursor into @columnName, @type
End

 SET @dynamicCommand = @dynamicCommand + ' FROM ' + @table_name + ' WHERE '  + @where
 

Close columnNames_cursor
Deallocate columnNames_cursor


EXEC ( @dynamicCommand)
PRINT @dynamicCommand

GO







/*
EXEMPLE MODOP POUR EXTRAIRE LES DONNEES
*/
--DECLARE @groupeCode nvarchar(max) = 'GFTP';

--DECLARE @SocieteID varchar(max) = (SELECT SocieteID FROM FRED_SOCIETE WHERE Code = '0143');
--PRINT @SocieteID;

--DECLARE @WhereSocieteID  varchar(max)  = ' SocieteID = ' + @SocieteID;


--DECLARE @GroupeID varchar(max)  = (SELECT groupeID FROM FREd_GROUPE where code = 'GFTP')
--DECLARE @Where@GroupeID  varchar(max)  = ' GroupeId = ' + @GroupeID;


-- EXTRACTION DES PRIME
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_PRIME', @sp_name='Fred_ToolBox_Code_Prime', @where=@WhereSocieteID , @GroupeCode=@groupeCode


-- EXTRACTION DES CODES ZONES DEPLACEMENT
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_CODE_ZONE_DEPLACEMENT', @sp_name='Fred_ToolBox_Code_Zone_Deplacement', @where=@WhereSocieteID, @GroupeCode=@groupeCode

-- EXTRACTION DES CODES ABSENCES
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_CODE_ABSENCE', @sp_name='Fred_ToolBox_Code_Absence', @where=@WhereSocieteID, @GroupeCode=@groupeCode

-- EXTRACTION DES PRIMES
-- Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_PRIME', @sp_name='Fred_ToolBox_Code_Prime', @where=@WhereSocieteID, @GroupeCode=@groupeCode

-- EXTRACTION DES MAJORATIONS
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_CODE_MAJORATION', @sp_name='Fred_ToolBox_Code_Majoration', @where=@Where@GroupeID

--EXTRACTION DES CODES DEPLACEMENTS
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_CODE_DEPLACEMENT', @sp_name='Fred_ToolBox_Code_Deplacement', @where=@WhereSocieteID, @GroupeCode=@groupeCode

-- EXTRACTION DES CHAPITRE
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_CHAPITRE', @sp_name='Fred_ToolBox_Chapitre', @where=@Where@GroupeID

-- EXTRACTION DES SOUS-CHAPITRE
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_Get_Sous_Chapitre', @sp_name='Fred_ToolBox_SousChapitre', @where=@Where@GroupeID

-- EXTRACTION DES RESSOURCES
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_Get_Ressources', @sp_name='Fred_ToolBox_Ressources', @where=@Where@GroupeID


-- EXTRACTION DES NATURES ANALYTIQUES
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_NATURE', @sp_name='Fred_ToolBox_Nature', @where=@WhereSocieteID, @GroupeCode=@groupeCode


-- EXTRACTION DES CI
--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_CI', @sp_name='Fred_ToolBox_Nature', @where=@WhereSocieteID, @GroupeCode=@groupeCode



--EXEC Fred_ToolBox_Fw_Extract_Datas_To_Transport @table_name='FRED_SOCIETE', @sp_name='Fred_ToolBox_Societe', @where='GroupeId=3'