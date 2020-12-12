-- =======================================================================================================================================
-- Author:		Yoann Collet    20/03/2019
--
-- Description:
--      - Ajout de la fonctionnalité 'Préparer les Energies' à la table FRED_FONCTIONNALITE lié au module Achat
--      - Ajout de la permission 'Affichage du menu / Accès à la page 'Préparer les Energies''  à la table FRED_PERMISSION
--      - Ajout de la relation de la permission à la fonctionnalité dans la table FRED_PERMISSION_FONCTIONNALITE
--
-- =======================================================================================================================================

DECLARE @module_id int = (SELECT ModuleId FROM FRED_MODULE where Code = '8');

INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, DateSuppression, Description) VALUES (@module_id, '806', 'Préparer les Energies', 0, NULL, NULL); 

DECLARE @fonctionnalite_id int = (SELECT @@IDENTITY);

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle) VALUES ('menu.show.preparer.energies', 1 , '1514', 'Affichage du menu / Accès à la page ''Préparer les Energies'' ', 0);

DECLARE @permission_id int = ( SELECT @@IDENTITY);

INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionId, FonctionnaliteId) VALUES (@permission_id, @fonctionnalite_id);


