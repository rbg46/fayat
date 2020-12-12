SELECT TRIM(SUPSOC) AS Societe
      ,TRIM(SUPETB) AS Etablissement
      ,TRIM(SUPLIL) AS Libelle
      ,TRIM(SUPMAT) AS Matricule
      ,TRIM(SUPNOM) AS Nom
      ,TRIM(SUPPRE) AS Prenom
      ,TRIM(SUPCAG) AS CodeAbsenceFred
      ,TRIM(SUPCAA) AS CodeAbsenceAnael
      ,CASE 
        WHEN SUPDEB <> '' AND SUPDEB IS NOT NULL AND LENGTH(TRIM(SUPDEB)) = 8
        THEN CAST(SUBSTRING(SUPDEB, 1, 4) || '-' || SUBSTRING(SUPDEB, 5, 2) || '-' || SUBSTRING(SUPDEB, 7, 2)  As DATE)
        ELSE NULL 
        END AS DateDebut
      ,CASE 
        WHEN SUPFIN <> '' AND SUPFIN IS NOT NULL AND LENGTH(TRIM(SUPFIN)) = 8
        THEN CAST(SUBSTRING(SUPFIN, 1, 4) || '-' || SUBSTRING(SUPFIN, 5, 2) || '-' || SUBSTRING(SUPFIN, 7, 2)  As DATE)
        ELSE NULL 
        END AS DateFin
      ,TRIM(NOLOT) AS NumeroLot
FROM INTFOND.SANABSP
WHERE TRIM(SUSER) = '{0}' AND TRIM(NOLOT) = '{1}'
