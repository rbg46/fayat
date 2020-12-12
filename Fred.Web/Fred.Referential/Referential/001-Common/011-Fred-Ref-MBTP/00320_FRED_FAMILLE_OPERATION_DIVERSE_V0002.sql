﻿BEGIN 
	DECLARE @SocieteId int

	/*Récupération de la sociétéID*/
	select top 1 @SocieteId			= SOCIETEID FROM FRED_SOCIETE where Code = 'MBTP'

	IF EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MO POINTEE (Hors Interim)' AND SocieteId = @SocieteId)
	BEGIN 
		Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 1 WHERE [Libelle] = 'MO POINTEE (Hors Interim)' AND SocieteId = @SocieteId
	END

	IF EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'ACHAT AVEC COMMANDE (Y compris interim)' AND SocieteId = @SocieteId)
	BEGIN 
		Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 1 WHERE [Libelle] = 'ACHAT AVEC COMMANDE (Y compris interim)' AND SocieteId = @SocieteId
	END

	IF EXISTS(SELECT * FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE FRED_FAMILLE_OPERATION_DIVERSE.[Libelle] = 'MATERIEL INTERNE POINTE' AND SocieteId = @SocieteId)
	BEGIN 
		Update FRED_FAMILLE_OPERATION_DIVERSE SET [IsAccrued] = 1 WHERE [Libelle] = 'MATERIEL INTERNE POINTE' AND SocieteId = @SocieteId
	END	
END