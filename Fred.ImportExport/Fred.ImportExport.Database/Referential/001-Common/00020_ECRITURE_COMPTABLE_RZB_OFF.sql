-- Désactivation du flux ECRITURE_COMPTABLE_RZB qui n'est plus actif
UPDATE [IMPORTEXPORT].[FLUX]
SET ISACTIF=0,Libelle='Transfert écritures comptables (obsolete)'
WHERE CODE='ECRITURE_COMPTABLE_RZB'