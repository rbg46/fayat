-----------------------------------
-- Mise à jour des primes FES => Journalière
-- --------------------------------------------------


DECLARE @SocieteId INT; 
DECLARE @Code VARCHAR(MAX); 
DECLARE @Libelle VARCHAR(MAX); 
Declare A Cursor For SELECT  SocieteId, Code, Libelle FROM FRED_SOCIETE WHERE GroupeId = (SELECT  GroupeId FROM FRED_GROUPE WHERE Code = 'GFES')

	Open A
			Fetch next From A into @SocieteId, @Code, @Libelle
				While @@Fetch_Status=0 Begin

			-- --------------------------------------------------
			-- Mise à jour des primes des sociétés FES (BOUCLE PAR SOCIETE)
			-- --------------------------------------------------
			-- PrimeType = 0 : journalier
			update FRED_PRIME set PrimeType=0  where Code='GDI' and SocieteId = @SocieteId
			update FRED_PRIME set PrimeType=0  where Code='GDP' and SocieteId = @SocieteId
			update FRED_PRIME set PrimeType=0  where Code='IR' and SocieteId = @SocieteId
			update FRED_PRIME set PrimeType=0  where Code='TR' and SocieteId = @SocieteId
			update FRED_PRIME set PrimeType=0  where Code='AE' and SocieteId = @SocieteId
			update FRED_PRIME set PrimeType=0  where Code='INS' and SocieteId = @SocieteId
			update FRED_PRIME set PrimeType=0  where Code='ES' and SocieteId = @SocieteId

			Fetch next From A into  @SocieteId, @Code, @Libelle
	End
	Close A

Deallocate A
	

