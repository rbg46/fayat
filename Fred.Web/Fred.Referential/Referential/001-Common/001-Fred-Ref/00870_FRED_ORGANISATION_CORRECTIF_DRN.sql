-- =======================================================================================================================================
-- Author:		Yannick DEFAY 24/06/2019
--
-- Description:
--      - Corrige le chaînage des organisations pour  que les établissements comptable DRN soit rattaché directement au PUO DRN + Suppression de l'UO DRN
--
-- =======================================================================================================================================

BEGIN TRAN
    -- Réaffectation de l'organisation Père des établissement de la DRN vers PUO DRN
    DECLARE @PUO_DRN_OrgaId int = 0;
    SET @PUO_DRN_OrgaId = (SELECT OrganisationId FROM FRED_ORGANISATION_GENERIQUE WHERE Code = 'PUO_DRN');

    IF (@PUO_DRN_OrgaId <> 0)
    BEGIN
        UPDATE FRED_ORGANISATION SET PereId = @PUO_DRN_OrgaId WHERE OrganisationId IN (
        SELECT OrganisationId FROM FRED_ETABLISSEMENT_COMPTABLE WHERE Libelle LIKE 'DRN%')
    END

    DECLARE @UO_DRN_OrgaId int = 0;
    SET @UO_DRN_OrgaId = (SELECT OrganisationId FROM FRED_ORGANISATION_GENERIQUE WHERE CODE = 'UO_IDF_OUEST');

    -- Suppression de l'organisation UO_IDF_OUEST et de ses dépendances
    IF (@UO_DRN_OrgaId <> 0)
    BEGIN
        DELETE FRED_BAREME_EXPLOITATION_ORGANISATION WHERE OrganisationId IN (
        SELECT OrganisationId FROM FRED_ORGANISATION_GENERIQUE WHERE OrganisationId = @UO_DRN_OrgaId)

        DELETE FROM FRED_ORGANISATION_GENERIQUE WHERE OrganisationId = @UO_DRN_OrgaId
    
        DELETE FRED_ORGANISATION WHERE OrganisationId IN (
        SELECT OrganisationId FROM FRED_ORGANISATION_GENERIQUE WHERE OrganisationId = @UO_DRN_OrgaId)
    END
COMMIT TRAN