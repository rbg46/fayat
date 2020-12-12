--Correction du bug [5153]
DECLARE @groupeId INT ;
DECLARE @rapportLigneId INT ;

set @groupeId = (SELECT GroupeId from FRED_GROUPE WHERE Code = 'GFES')
set @rapportLigneId = (SELECT RapportLigneId from FRED_RAPPORT_LIGNE where codeMajorationId = (select CodeMajorationId from FRED_CODE_MAJORATION where Code='TNF'));
DELETE FROM FRED_RAPPORT_LIGNE_MAJORATION WHERE RapportLigneId = @rapportLigneId;
DELETE FROM FRED_RAPPORT_LIGNE_TACHE WHERE RapportLigneId = @rapportLigneId;
DELETE FROM FRED_RAPPORT_LIGNE_PRIME WHERE RapportLigneId = @rapportLigneId;
DELETE FROM FRED_RAPPORT_LIGNE WHERE RapportLigneId = @rapportLigneId;
DELETE from FRED_CODE_MAJORATION  where GroupeId = @groupeId and Code = 'TNF'