UPDATE [dbo].[FRED_PERMISSION] set [PermissionContextuelle] = 1 WHERE [Code] IN ('0040', '0047')
UPDATE [dbo].[FRED_PERMISSION] set [Libelle] = 'Affichage du menu / Accès à la page ''Gérer les clotûres''.' WHERE [PermissionKey] = 'menu.show.datescloturecomptable.index'
