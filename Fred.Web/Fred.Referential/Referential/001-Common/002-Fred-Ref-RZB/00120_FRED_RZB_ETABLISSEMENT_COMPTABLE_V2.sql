



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- Correction d'un super Gremlins pour corriger l'affectation du chantier de Boulogne lié à l'activité nucléaire
-- --------------------------------------------------
UPDATE FRED_ETABLISSEMENT_COMPTABLE SET OrganisationId = 100 where EtablissementComptableId = 1
UPDATE FRED_ETABLISSEMENT_COMPTABLE SET OrganisationId = 101 where EtablissementComptableId = 2
UPDATE FRED_ETABLISSEMENT_COMPTABLE SET OrganisationId = 102 where EtablissementComptableId = 3
