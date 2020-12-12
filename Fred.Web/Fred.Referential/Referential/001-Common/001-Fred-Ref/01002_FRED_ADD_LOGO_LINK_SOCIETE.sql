-- --------------------------------------------------
-- Bug 12637 : Affichage des logos
-- --------------------------------------------------

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Satelec.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Semeru.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Fareco.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo FES.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Ers.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Ers maine.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Fesi.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Gabrielle.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Fayat Power.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Satelec Cenergi.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Satcomptages.png'
           ,null
           ,2
           ,0)
GO

INSERT INTO [FRED_IMAGE]
           ([Path]
           ,[Credit]
           ,[Type]
           ,[IsDefault])
     VALUES
           ('/medias/app/societe/logotype/logo Valiance.png'
           ,null
           ,2
           ,0)
GO

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Fareco.png%') 
  where [Code] = 'E003'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/CITEPARK.png%') 
  where [Code] = 'E006'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'E007'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Ers.png%') 
  where [Code] = 'E010'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Ers maine.png%') 
  where [Code] = 'E011'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/DHENNIN.png%') 
  where [Code] = 'E012'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/SODECLIM.png%') 
  where [Code] = 'E013'

    Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Fesi.png%') 
  where [Code] = 'E014'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Gabrielle.png%') 
  where [Code] = 'E015'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Fayat Power.png%') 
  where [Code] = 'E016'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Satelec Cenergi.png%') 
  where [Code] = 'E017'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Satcomptages.png%') 
  where [Code] = 'E018'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Valiance.png%') 
  where [Code] = 'E019'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'EG01'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'EG02'

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES01'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES02'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES04'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES05'
  Go

 Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES06'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES07'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES08'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES09'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES10'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES11'

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES12'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES13'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES14'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES15'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES16'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES17'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES18'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES19'

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES20'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES21'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES22'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES23'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES24'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES25'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'ES25'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo FES.png%') 
  where [Code] = 'E004'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Fesi.png%') 
  where [Code] = 'EX05'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Fesi.png%') 
  where [Code] = 'EX06'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Fesi.png%') 
  where [Code] = 'EX07'
  Go

  Update [FRED_SOCIETE] 
  Set [ImageLogoId] = (select imageid from FRED_IMAGE where [path] like '%/logo Fesi.png%') 
  where [Code] = 'EX08'
  Go
