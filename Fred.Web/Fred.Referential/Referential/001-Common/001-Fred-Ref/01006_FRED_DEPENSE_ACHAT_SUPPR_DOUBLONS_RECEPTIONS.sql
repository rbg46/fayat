CREATE PROCEDURE #FlagDuplicateReceptionsAsDeleted (
	@DateSuppressionDepense DATETIME,
	@CommentaireMajDepense VARCHAR(30)
)
AS
BEGIN
	-- Liste des réceptions dont le doublon à mettre à jour n'est pas le plus ancien
	-- Ces cas particuliers sont issu de la PJ "Liste V4.xlsx" du 08/06/2020 ajouté par MITAULT Marlène
	DECLARE @DepenseIdsToKeep TABLE (Id INT)
	INSERT INTO @DepenseIdsToKeep VALUES (40046), (40059), (115405);

	-- On récupère la réception la plus ancienne (sauf cas particuliers) parmi les doublons car c'est celle que l'on doit conserver (pas de modification à faire dessus)
    WITH Duplicates (
        DepenseId,
        RowNumber
    ) AS (
        SELECT
            DepenseId,
            ROW_NUMBER() OVER (
                PARTITION BY
                    CiId,
                    Libelle, 
                    Date
                ORDER BY
                    CASE WHEN DepenseId IN (SELECT Id FROM @DepenseIdsToKeep) THEN 0 ELSE 1 END,
                    DepenseId
            ) AS RowNumber
        FROM FRED_DEPENSE_ACHAT
        WHERE 
            DepenseTypeId = 1 
            AND IsReceptionInterimaire = 1
            AND DateSuppression IS NULL
            AND Quantite <> 0
    )

	-- Mise à jour des réceptions en doublons les plus récentes sauf cas particuliers (voir plus haut)
    UPDATE DA
    SET
        DA.DateSuppression = @DateSuppressionDepense, 
        DA.Commentaire = @CommentaireMajDepense
    FROM
        FRED_DEPENSE_ACHAT DA
        JOIN Duplicates D ON
            D.DepenseId = DA.DepenseId
            AND D.RowNumber > 1;
END
GO

CREATE PROCEDURE #DeleteReceptions (
	@DateSuppressionDepense DATETIME,
	@CommentaireMajDepense VARCHAR(30)
)
AS
BEGIN
	/*
		Liste des cas particuliers de dépense à mettre à jour mais qui n'apparaissent pas dans la requête précédente :
		S40 - Intérim Vincent CRETIN - 14390216 - 0009 / S28 - Intérim Messaoud MAMMERI - 14390170 - MM7 / S29 - Intérim Messaoud MAMMERI - 14390170 - MM7 /
		S4 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00049 / S7 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00064
	*/

	DECLARE @DepenseToDelete TABLE (Id INT)
	INSERT INTO @DepenseToDelete VALUES (82270), (42164), (40069), (115412), (122245)
	
	UPDATE da
	SET
			DA.DateSuppression = @DateSuppressionDepense, 
			DA.Commentaire = @CommentaireMajDepense
	FROM
			FRED_DEPENSE_ACHAT da
			INNER JOIN @DepenseToDelete depenseToDelete ON depenseToDelete.Id = da.DepenseId
END
GO

CREATE PROCEDURE #DeleteValorisations
AS
BEGIN
	/* 
		Liste des valorisations à supprimer concernant les intérimaires suivants :
		S31 - Intérim CYRIL BROUSSEAU - 14390219 - 00014 / S5 - Intérim Cengiz CETIN - 14390311 - 00058 / S5 - Intérim Hycham EL BOUFI - 14390313 - 00061 /
		S50 - Intérim MAHDI BEN MAALEM - 14390299 - 00045 / S51 - Intérim MAHDI BEN MAALEM - 14390299 - 00045 / S40 - Intérim Vincent CRETIN - 14390216 - 0009 /
		S28 - Intérim Messaoud MAMMERI - 14390170 - MM7 / S4 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00049 /
		S7 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00064
	*/
	DELETE FROM FRED_VALORISATION 
	WHERE ValorisationId IN (148643, 190748, 190750, 190773, 161991, 164354, 148645, 102618, 190781, 218124)
END
GO

CREATE PROCEDURE #UpdateValorisationQuantities
AS
BEGIN
	/* 
		Liste des valorisations à mettre à jour (Quantité) concernant les intérimaires suivants :
		S4 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00049
		S7 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00064
	*/

	DECLARE @QuantiteValorisationToUpdate TABLE (Id INT, Quantite DECIMAL(18,3))
	INSERT INTO @QuantiteValorisationToUpdate VALUES (180503, -37.50), (202163, -39.50)

	UPDATE v
	SET v.Quantite = qteValo.Quantite
	FROM 
		FRED_VALORISATION v 
		INNER JOIN @QuantiteValorisationToUpdate qteValo ON qteValo.Id = v.ValorisationId
END
GO

CREATE PROCEDURE #UpdateValorisationAmounts
AS
BEGIN
	/* 
		Liste des valorisations à mettre à jour (Montant) concernant les intérimaires suivants :
		S27 - Intérim Ahmed ABDELKRIM - 14390146 - AA10 / S27 - Intérim HAMADI HENIA - 14390153 - HH10 / S28 - Intérim Ahmed ABDELKRIM - 14390146 - AA10 /
		S28 - Intérim HAMADI HENIA - 14390153 - HH10 / S29 - Intérim Ahmed ABDELKRIM - 14390146 - AA10 / S27 - Intérim ALESSANDRO PERPIGNANO - 14390175 - AP2 /
		S28 - Intérim ALESSANDRO PERPIGNANO - 14390175 - AP2 / S29 - Intérim ALESSANDRO PERPIGNANO - 14390175 - AP2 / S27 - Intérim Messaoud MAMMERI - 14390170 - MM7 /
		S30 - Intérim Messaoud MAMMERI - 14390170 - MM7 / S27 - Intérim Messaoud MAMMERI - 14390170 - MM6 / S27 - Intérim MOSTEFA DIFFALHA - 14390151 - DM6 /
		S4 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00049 / S7 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00064 / S29 - Intérim Messaoud MAMMERI - 14390170 - MM7
	*/
	UPDATE v
	SET Montant = ROUND(PUHT * Quantite, 2)
	FROM FRED_VALORISATION v 
	WHERE ValorisationId IN (96685, 96688, 96686, 96689, 96687, 96690, 96691, 96692, 102617, 96680, 96682, 96681, 180503, 202163, 102619)
END
GO

CREATE PROCEDURE #UpdateExpenseQuantities(
	@CommentaireMajDepense VARCHAR(30)
)
AS
BEGIN
	/* 
		Liste des dépenses à mettre à jour (Quantité) concernant les intérimaires suivants :
		S5 - Intérim Vincent CRETIN - 14390216 - 0009 / S4 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00049 / S7 - Intérim Jean-Baptiste SIVIGNON NORGEUX - 14390257 - 00064
	*/
	DECLARE @QuantiteDepenseToUpdate TABLE (Id INT, Quantite NUMERIC(12,3))
	INSERT INTO @QuantiteDepenseToUpdate VALUES (107734, 38.50), (107778, 37.50), (118688, 39.50)

	UPDATE da
	SET 
		da.Quantite = qteDepense.Quantite,
		da.Commentaire = @CommentaireMajDepense
	FROM 
		FRED_DEPENSE_ACHAT da 
		INNER JOIN @QuantiteDepenseToUpdate qteDepense ON qteDepense.Id = da.DepenseId
END
GO

DECLARE @DateSuppressionDepense DATETIME
SET @DateSuppressionDepense = SYSDATETIME();

DECLARE @CommentaireMajDepense VARCHAR(30)
SET @CommentaireMajDepense = 'Reprise Ticket 13489';

EXEC #FlagDuplicateReceptionsAsDeleted @DateSuppressionDepense, @CommentaireMajDepense;
EXEC #DeleteReceptions @DateSuppressionDepense, @CommentaireMajDepense;
EXEC #DeleteValorisations;
EXEC #UpdateValorisationQuantities;
EXEC #UpdateValorisationAmounts;
EXEC #UpdateExpenseQuantities @CommentaireMajDepense;

DROP PROCEDURE #FlagDuplicateReceptionsAsDeleted;
DROP PROCEDURE #DeleteReceptions;
DROP PROCEDURE #DeleteValorisations;
DROP PROCEDURE #UpdateValorisationQuantities;
DROP PROCEDURE #UpdateValorisationAmounts;
DROP PROCEDURE #UpdateExpenseQuantities;