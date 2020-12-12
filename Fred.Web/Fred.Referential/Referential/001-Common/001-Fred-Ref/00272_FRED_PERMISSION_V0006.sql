-- Ajout des permissions pour les menus de Rapport Hebdomadaires, Rapports ETAM/IAC et Synthèse mensuelle
INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('menu.show.areas.pointage.views.rapport.hebdo', 1, '0057', 'Rapport hebdomadaire ouvrier', 0)

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('menu.show.areas.pointage.views.rapport.etamiac', 1, '0058', 'Rapport hebdomadaire ETAM IAC', 0)

INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
VALUES ('menu.show.areas.pointage.views.rapport.synthese', 1, '0059', 'ETAM IAC synthèse mensuelle', 0)