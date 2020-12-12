-- RG_5400_001 Renommer Compte Exploitation en Editions d'Exploitation... 
UPDATE FRED_PERMISSION
SET LIBELLE = 'Affichage du menu / Accès à la page ''Editions d’Exploitation''.'
WHERE PERMISSIONKEY ='menu.show.compte.exploitation.index'

UPDATE FRED_FONCTIONNALITE
SET LIBELLE = 'Editions d’Exploitation'
WHERE Code='905'
