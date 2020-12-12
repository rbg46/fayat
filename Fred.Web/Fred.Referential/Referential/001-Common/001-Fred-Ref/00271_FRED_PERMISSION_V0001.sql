--Insertion des permissions pour : gestion d'équipe et délégation
IF NOT EXISTS (SELECT PermissionId FROM FRED_PERMISSION WHERE Code = '0049')
BEGIN
  INSERT INTO[dbo].[FRED_PERMISSION] 
  ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
   VALUES('menu.ci.detail.gestion.equipe',1,'0049','Gestion du calendrier hebdomadaire des affectations',1)
END
 
IF NOT EXISTS (SELECT PermissionId FROM FRED_PERMISSION WHERE Code = '0050')
BEGIN
  INSERT INTO[dbo].[FRED_PERMISSION] 
  ([PermissionKey], [PermissionType], [Code], [Libelle], [PermissionContextuelle]) 
  VALUES('menu.ci.detail.gestion.equipe.delegation',1,'0050','Possibilité de déléguer au niveau du calendrier hebdomadaire des affectations',1)
END