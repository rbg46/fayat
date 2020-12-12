/******************************************************************************
  _____ _ __   __ _  _____      __  _____ ____  _____ ____  
 |  ___/ \\ \ / // \|_   _|    / / |  ___|  _ \| ____|  _ \ 
 | |_ / _ \\ V // _ \ | |     / /  | |_  | |_) |  _| | | | |
 |  _/ ___ \| |/ ___ \| |    / /   |  _| |  _ <| |___| |_| |
 |_|/_/   \_\_/_/   \_\_|   /_/    |_|   |_| \_\_____|____/ 
															


APPLICATION				 : FAYAT / FRED
OBJECT TYPE				 : Procedure
NAME					     : [DeleteSociete]
DESCRIPTION				 : Suppression d'une societe et de ses dépendances	
PARAMETER:
	@societeId       : Id de la societe a supprimer
USAGE					     : EXEC [dbo].[DeleteSociete] @societeId

========================================================================================================
DATE			   AUTHOR		VERSION		OBJECT
========================================================================================================
10/02/2017		JCA			V1.0		  US 1809 : Creation
21/02/2018		JSM			V2.0		  US 5830 : Suppression des Paramvalues pour une organisation

*******************************************************************************/
IF OBJECT_ID ( 'dbo.DeleteSociete', 'P' ) IS NOT NULL  
	DROP PROCEDURE [dbo].[DeleteSociete]		
		GO

create procedure [dbo].[DeleteSociete]
	@societeId int
as
begin
	declare @organisationId int;

	select @organisationId = OrganisationId from [dbo].[FRED_SOCIETE] where SocieteId = @societeId;

	--Suppression des devises
	delete from [dbo].[FRED_SOCIETE_DEVISE] where SocieteId = @societeId;
	--Suppression des codes absences
	delete from [dbo].[FRED_CODE_ABSENCE] where SocieteId = @societeId;
	--Suppression de la societe
	delete from [dbo].[FRED_SOCIETE] where SocieteId = @societeId;
	--Suppression de param values
	delete from [dbo].[FRED_PARAM_VALUE] where OrganisationId = @organisationId;
	--Suppression de l'organisation
	delete from [dbo].[FRED_ORGANISATION] where OrganisationId = @organisationId;
end
