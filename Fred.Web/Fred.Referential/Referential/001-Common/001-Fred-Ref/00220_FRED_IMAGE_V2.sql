
-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

UPDATE [dbo].[FRED_IMAGE]  SET [IsDefault] = 0 WHERE [Type] = 2 AND [IsDefault] = 1;
INSERT INTO [dbo].[FRED_IMAGE] ([Path], [Credit], [Type], [IsDefault]) VALUES (N'/medias/app/societe/logotype/FAYAT.jpg', N'', 2, 1)
