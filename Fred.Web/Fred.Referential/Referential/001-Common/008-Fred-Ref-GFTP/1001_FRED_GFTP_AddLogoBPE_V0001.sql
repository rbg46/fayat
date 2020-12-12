INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/Logo_BPE.png'
           ,null
           ,2
           ,0)
GO

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/Logo_BPE.png%') 
  where [Code] = '0208'
  Go