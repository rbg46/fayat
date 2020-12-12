IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Splitstring]') AND type in (N'TF'))
DROP FUNCTION [dbo].[Splitstring]
GO

/******************************************************************************
  _____ _ __   __ _  _____      __  _____ ____  _____ ____  
 |  ___/ \\ \ / // \|_   _|    / / |  ___|  _ \| ____|  _ \ 
 | |_ / _ \\ V // _ \ | |     / /  | |_  | |_) |  _| | | | |
 |  _/ ___ \| |/ ___ \| |    / /   |  _| |  _ <| |___| |_| |
 |_|/_/   \_\_/_/   \_\_|   /_/    |_|   |_| \_\_____|____/ 
                                                            


APPLICATION				: FAYAT / FRED
OBJECT TYPE				: Function
NAME					    : Splitstring
DESCRIPTION				: Fonction decoupant une chaine de caractère avec separateur		 - retourne une table		
PARAMETER:
	@stringToSplit	: Chaine de caractere à découper
	@separator 	    : Séparateur	 
USAGE					    : select Name from Splitstring ('FRED_SOCIETE|FRED_ORGANISATION', '|')

========================================================================================================
DATE			   AUTHOR		VERSION		OBJECT
========================================================================================================
03/02/2017		JCA			V1.0		  US 1809 : Creation

*******************************************************************************/

CREATE FUNCTION [dbo].[Splitstring] ( @stringToSplit VARCHAR(MAX), @separator char(1))
RETURNS
 @returnList TABLE ([Name] [nvarchar] (500))
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(@separator, @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(@separator, @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT @name

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @stringToSplit

 RETURN
END

