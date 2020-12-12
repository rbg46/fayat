
DECLARE @Code INT = 23;
IF(NOT EXISTS(SELECT 1 FROM [dbo].[FRED_FEATURE_FLIPPING] WHERE [Name] = N'ActivationUS13085_6000'))
BEGIN
INSERT INTO [dbo].[FRED_FEATURE_FLIPPING]
           ([Code]
           ,[Name]
           ,[IsActived]
           ,[DateActivation]
           ,[UserActivation])
     VALUES
           (@Code
           ,N'ActivationUS13085_6000'
           ,0
           ,GETDATE()
           ,N'super_fred')
END
