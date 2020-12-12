/****** Object:  StoredProcedure [dbo].[FRED_saveCommande_fromBUYER]    Script Date: 16/11/2018 17:49:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Emmanuel Rouyer
-- Create date: 16/11/2018
-- Description:	SP de vérification des commmandes non recues par SAP pour 
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[FRES_SP_CHECK_COMMANDE_NON_RECUES]
AS
BEGIN

declare @nb_erreur int
declare @serveur varchar(100)
declare @base	 varchar(100)
declare @subject varchar(8000)
declare @body	 varchar(8000)



SELECT @serveur=@@SERVERNAME
SELECT @base=DB_NAME()

SELECT @nb_erreur=count(1) 
  FROM [nlog].[NLogs]
  where [Message] like '%FLUX_ME21%Réponse de Sap détaillée : StatusCode: 200, ReasonPhrase:%'
  and [Message] not like '%ref 00%'
  and [nlog].[NLogs].Logged > DATEADD(DAY,-1,CONVERT (date, GETDATE()))

IF @nb_erreur>0
BEGIN
	SELECT @subject='SERVEUR: ' +  @serveur + ' - BASE : '+@serveur + 'Commandes non reçues, nombre:'+convert(varchar(100),@nb_erreur)
	select @body= 
      [Application]
      +convert(varchar(10),[Logged])
      +[Level]
      +[Message]
      +[UserName]
      +[Logger]
      +[Callsite]
      +[Exception]
	FROM [nlog].[NLogs] 
		  where [Message] like '%FLUX_ME21%Réponse de Sap détaillée : StatusCode: 200, ReasonPhrase:%'
		  and [Message] not like '%ref 00%'
		  and [nlog].[NLogs].Logged > DATEADD(DAY,-1,CONVERT (date, GETDATE()))

	EXEC msdb.dbo.sp_send_dbmail @profile_name='FAYATIT',
	@recipients='support.FRED@fayatit.fayat.com',
	@subject=@subject,
	@body=@body

END
END

GO


