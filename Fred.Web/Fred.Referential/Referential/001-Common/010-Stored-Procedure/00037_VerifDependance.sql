IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VerifDependance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[VerifDependance]
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
	@tablesAVerifier : Table données sources.
	@origineId 	     : Identifiant unique de l'objet dont il faut verifier les dépendances
  @resu	           : Valeur de type entier, retourne le résultat d'execution de la procédure
USAGE					     : EXEC [dbo].[VerifDependance] @tablesAVerifier, @origineId, @resu OUTPUT

========================================================================================================
DATE			   AUTHOR		VERSION		OBJECT
========================================================================================================
03/02/2017		JCA			V1.0		  US 1809 : Creation

*******************************************************************************/

create procedure [dbo].[VerifDependance] 
(
	@tablesAVerifier tablesAVerifier readonly,
	@origineId int,
	@resu int OUTPUT 
)
as 
	begin

	create table tmp(nbEnregistrement int);

	DECLARE tables_cursor CURSOR  
	   FOR  
		select * from @tablesAVerifier;
	OPEN tables_cursor;  
	DECLARE @TableName varchar(100);  
	DECLARE @CleEtrangere varchar(100);  
	DECLARE @TableOrigine varchar(100);  
	DECLARE @ClePrimaire varchar(100);  
	FETCH NEXT FROM tables_cursor INTO @TableName, @CleEtrangere, @TableOrigine, @ClePrimaire;  
	WHILE (@@FETCH_STATUS <> -1)  
	BEGIN;  
	   insert INTO tmp Exec ('Select count(1) from ' + @TableName + ' where ' + @CleEtrangere + ' =  ' + @origineId);   
    
	   FETCH NEXT FROM tables_cursor INTO @TableName, @CleEtrangere, @TableOrigine, @ClePrimaire;  
	END;   
	CLOSE tables_cursor;  
	DEALLOCATE tables_cursor;  
	  

	select @resu = sum(nbEnregistrement) from tmp;
	drop table tmp;

	return
end
