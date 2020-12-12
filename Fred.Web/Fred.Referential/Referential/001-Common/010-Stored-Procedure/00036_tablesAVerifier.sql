/******************************************************************************
  _____ _ __   __ _  _____      __  _____ ____  _____ ____  
 |  ___/ \\ \ / // \|_   _|    / / |  ___|  _ \| ____|  _ \ 
 | |_ / _ \\ V // _ \ | |     / /  | |_  | |_) |  _| | | | |
 |  _/ ___ \| |/ ___ \| |    / /   |  _| |  _ <| |___| |_| |
 |_|/_/   \_\_/_/   \_\_|   /_/    |_|   |_| \_\_____|____/ 
                                                            


APPLICATION				: FAYAT / FRED
OBJECT TYPE				: TYPE
NAME					    : tablesAVerifier
DESCRIPTION				: Type de table utilisé dans le cadre de la vérification des dépendances						 
USAGE					    : 

========================================================================================================
DATE			   AUTHOR		VERSION		OBJECT
========================================================================================================
03/02/2017		JCA			V1.0		  US 1809 : Creation

*******************************************************************************/

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'tablesAVerifier' AND ss.name = N'dbo')
CREATE TYPE tablesAVerifier
AS TABLE
(
  TableName nvarchar(100), 
  CleEtrangere nvarchar(100), 
	TableOrigine nvarchar(100), 
	ClePrimaire nvarchar(100) 
)
GO
