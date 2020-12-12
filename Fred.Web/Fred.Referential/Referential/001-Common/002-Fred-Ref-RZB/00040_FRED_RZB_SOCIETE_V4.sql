-- --------------------------------------------------
-- FRED 2017 - R4 - SEPT 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE RZB
-- CREATION DES SOCIETES DANS LE CADRE DE l'UTILSATION DE FRED POUR STAIR
-- Correction du script 00040_FRED_RZB_SOCIETE_V3.sql
-- --------------------------------------------------

-- Champs génériques pour la gestion des organisations
DECLARE @ORGA_GENE_PERE int;
SELECT @ORGA_GENE_PERE=(SELECT ORGANISATIONID FROM FRED_GROUPE WHERE CODE='GRZB')

update FRED_ORGANISATION
set PereId=@ORGA_GENE_PERE
from FRED_ORGANISATION
inner join FRED_SOCIETE on FRED_SOCIETE.OrganisationId=FRED_ORGANISATION.OrganisationId and FRED_SOCIETE.Code in ('LHTP','COTG','LACH','GEO','BIAN')
