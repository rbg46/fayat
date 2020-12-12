-- =============================================
-- Author:		Marlène MITAULT
-- Create date: 01/10/2019
-- Description:	Create the TYPE_ORGANISATION_LIST
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'TYPE_ORGANISATION_LIST')
CREATE TYPE [dbo].[TYPE_ORGANISATION_LIST]  AS TABLE 
(
	ORGANISATION_TYPE_ID INT
)
GO