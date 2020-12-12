DECLARE @Code as nvarchar(8)

SELECT @Code ='00'+CAST(MAX(CAST(CODE AS INT)) +10 as nvarchar ) FROM FRED_PERMISSION
INSERT INTO FRED_PERMISSION   
SELECT 'menu.show.datecloturecomptaleenmasse.index',1, @Code, 'Affichage du menu / Accès à la page  ''Gérer les clôtures en masse''.',0