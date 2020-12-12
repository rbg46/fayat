---------------------------------------------
-- Script d'ajout d'un serveur SQL distant --
---------------------------------------------
EXECUTE master.dbo.sp_addlinkedserver @server = N'10.0.2.23', @srvproduct=N'SQL Server'
GO
EXECUTE master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'10.0.2.23',@useself=N'False',@locallogin=NULL,@rmtuser=N'pfred_buyer',@rmtpassword='fci$2015'