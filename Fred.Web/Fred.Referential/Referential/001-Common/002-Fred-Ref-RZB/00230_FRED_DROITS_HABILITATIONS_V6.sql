update FRED_PERMISSION_FONCTIONNALITE set PermissionId =
	(select p.PermissionId from FRED_PERMISSION p where p.PermissionKey = 'menu.show.datescloturecomptable.index')
where FonctionnaliteId = 52 and PermissionId = (select p.PermissionId from FRED_PERMISSION p where p.PermissionKey = 'menu.show.datescalendrierpaie.index')

update FRED_PERMISSION_FONCTIONNALITE set PermissionId=(select p.PermissionId from FRED_PERMISSION p where p.PermissionKey = 'menu.show.datescalendrierpaie.index')
	where FonctionnaliteId = 21 and PermissionId = (select p.PermissionId from FRED_PERMISSION p where p.PermissionKey = 'menu.show.datescloturecomptable.index')