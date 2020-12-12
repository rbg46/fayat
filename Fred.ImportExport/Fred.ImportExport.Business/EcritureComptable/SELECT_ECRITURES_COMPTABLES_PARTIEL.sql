SELECT
    TRIM(DTCPTA)  AS DateComptable,
    TRIM(DLIB) AS Libelle, 
    TRIM(RNAT) AS AnaelCodeNature,
    TRIM(MONTANT) AS Montant,
    TRIM(DCDEV)   AS Devise,
    TRIM(MONTDEV) AS MontantDevise, 
    TRIM(RSECT)   AS AnaelCodeCi,
    TRIM(DWS)  AS Dws,
    TRIM(DINT) AS Dint,
    TRIM(DNOLIG)  AS Dnolig,
    TRIM(LIGANA)  AS Ligana,
    TRIM(DJAL) AS AnaelCodeJournal,
    TRIM(COMMSAP) AS AnaelCodeCommande
FROM AXSPE.SECAFRW
WHERE
    RSTE='{0}'
    AND (RANP * 10000 + RMOISP * 100 + RJOURP) >= '{1}'
    AND (RANP * 10000 + RMOISP * 100 + RJOURP) <= '{2}' 
ORDER BY
    RANP DESC,
    RMOISP DESC,
    RJOURP DESC