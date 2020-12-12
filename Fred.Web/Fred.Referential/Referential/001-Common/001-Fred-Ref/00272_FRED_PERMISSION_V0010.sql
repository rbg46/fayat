-- Ajout des permissions pour la gestion des moyens : RATTRAPAGE D'UN PROBLEME DE MERGE
IF NOT EXISTS (SELECT 1 FROM FRED_PERMISSION WHERE Code='0100')
    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
    VALUES ('menu.show.gestion.moyen', 1, '0100', 'Gestion des moyens', 0)