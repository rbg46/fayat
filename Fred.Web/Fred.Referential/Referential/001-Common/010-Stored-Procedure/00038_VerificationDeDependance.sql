IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VerificationDeDependance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[VerificationDeDependance]
GO

/******************************************************************************
  _____ _ __   __ _  _____      __  _____ ____  _____ ____  
 |  ___/ \\ \ / // \|_   _|    / / |  ___|  _ \| ____|  _ \ 
 | |_ / _ \\ V // _ \ | |     / /  | |_  | |_) |  _| | | | |
 |  _/ ___ \| |/ ___ \| |    / /   |  _| |  _ <| |___| |_| |
 |_|/_/   \_\_/_/   \_\_|   /_/    |_|   |_| \_\_____|____/ 
                                                            


APPLICATION				 : FAYAT / FRED
OBJECT TYPE				 : Procedure
NAME					     : [VerifDependance]
DESCRIPTION				 : Fonction decoupant une chaine de caractère avec separateur		 - retourne une table		
PARAMETER:
	@origTableName   : Nom de la table pour laquelle il faut verifier les dépendances
	@exclusion       : Chaine avec delimiter contenant les nom des tables à exclure de la vérification - FORMAT : 'FRED_SOCIETE_DEVISE|FRED_CODE_ABSENCE|FRED_ORGANISATION'
	@dependance      : Chaine avec delimiter contenant les nom des tables supplémentaire dont il faut vérifier les dependances - FORMAT TABLE;Identifiant: '''FRED_ORGANISATION'',74'
	@origineId       : Identifiant de l'objet dont il faut vérifier les dependances
  @delimiter 	     : Delimiter utilisé pour le parsing des exclusions et dependances - FORMAT : '|'
  @resu	           : Valeur de type entier, retourne le résultat d'execution de la procédure
USAGE					     : EXEC [dbo].[VerificationDeDependance] @origTableName, @exclusion, @dependance, @resu output;

========================================================================================================
DATE			   AUTHOR		VERSION		OBJECT
========================================================================================================
03/02/2017		JCA			V1.0		  US 1809 : Creation

*******************************************************************************/
create procedure [dbo].[VerificationDeDependance] (
	@origTableName varchar(50),
	@exclusion varchar(4000),
	@dependance varchar(4000),
	@origineId int,
  @delimiter  char(1),
	@resu int OUTPUT
)
as
begin
  IF OBJECT_ID('lstExclusion', 'U') IS NOT NULL
      DROP TABLE lstExclusion

  IF OBJECT_ID('lstAutreTable', 'U') IS NOT NULL
      DROP TABLE lstAutreTable

  IF OBJECT_ID('tablesAVerifierWithParam', 'U') IS NOT NULL
      DROP TABLE tablesAVerifierWithParam

  --Recupération de la liste d'exclusion
  select * into lstExclusion  from [dbo].[Splitstring] ( @exclusion , @delimiter )  

  DECLARE @lstTable AS tablesAVerifier;

  -- Récupération des tables à vérifier
  insert into @lstTable (TableName, CleEtrangere, TableOrigine, ClePrimaire)
  SELECT src.name TableName, srcCol.name CleEtrangere, dst.name TableOrigine, dstCol.name ClePrimaire
	  FROM sys.foreign_key_columns fk
      INNER JOIN sys.columns srcCol ON fk.parent_column_id = srcCol.[column_id] 
          AND fk.parent_object_id = srcCol.[object_id]
      INNER JOIN sys.tables src ON src.[object_id] = fk.parent_object_id
      INNER JOIN sys.tables dst ON dst.[object_id] = fk.[referenced_object_id]
      INNER JOIN sys.columns dstCol ON fk.referenced_column_id = dstCol.[column_id] 
          AND fk.[referenced_object_id] = dstCol.[object_id]
		  where dst.name = @origTableName and src.name not in (select * from lstExclusion);


  --vérification des dépendances directe
  exec VerifDependance @lstTable, @origineId, @resu output;
  if(@resu >= 0)
	  return

  --vérification des dépendances des objets dépendant passé en parametre
  --Recupération de la liste des autres tables ayant une dependance à vérifier
  select Name into lstAutreTable from [dbo].[Splitstring] ( @dependance, @delimiter )

	  CREATE table tablesAVerifierWithParam
	  (
		  TableName nvarchar(31), 
		  Id int 
	  )

	  DECLARE tables_cursor CURSOR  
	     FOR  
		  select * from [dbo].[lstAutreTable];
	  OPEN tables_cursor;  
	  DECLARE @TName varchar(100);    
	  FETCH NEXT FROM tables_cursor INTO @TName;
	  WHILE (@@FETCH_STATUS <> -1)  
	  BEGIN;  
		  exec ('insert into tablesAVerifierWithParam (TableName, Id) values ('+@TName+')');		
		  --print @TName
	     FETCH NEXT FROM tables_cursor INTO @TName;  
	  END;   
	  CLOSE tables_cursor;  
	  DEALLOCATE tables_cursor; 		
	
	  DECLARE tables_cursor CURSOR  
	     FOR  
	  select TableName, Id from tablesAVerifierWithParam
	  OPEN tables_cursor;   
	  declare @Id int
	  FETCH NEXT FROM tables_cursor INTO @TName, @Id;
	  WHILE (@@FETCH_STATUS <> -1)  
	  BEGIN;  
		  delete from @lstTable;

		  insert into @lstTable (TableName, CleEtrangere, TableOrigine, ClePrimaire)
		  SELECT src.name TableName, srcCol.name CleEtrangere, dst.name TableOrigine, dstCol.name ClePrimaire
		  FROM sys.foreign_key_columns fk
		  INNER JOIN sys.columns srcCol ON fk.parent_column_id = srcCol.[column_id] 
			  AND fk.parent_object_id = srcCol.[object_id]
		  INNER JOIN sys.tables src ON src.[object_id] = fk.parent_object_id
		  INNER JOIN sys.tables dst ON dst.[object_id] = fk.[referenced_object_id]
		  INNER JOIN sys.columns dstCol ON fk.referenced_column_id = dstCol.[column_id] 
			  AND fk.[referenced_object_id] = dstCol.[object_id]	
		  inner join tablesAVerifierWithParam t on t.TableName = dst.name and src.name not in (select * from lstExclusion);

		  exec VerifDependance @lstTable, @Id, @resu output;

		  if(@resu >= 0)
		  begin
			  CLOSE tables_cursor;  
			  DEALLOCATE tables_cursor; 
			  return
		  end
	  FETCH NEXT FROM tables_cursor INTO @TName, @Id;  
	  END;   
	  CLOSE tables_cursor;  
	  DEALLOCATE tables_cursor; 
	
	  return
END
