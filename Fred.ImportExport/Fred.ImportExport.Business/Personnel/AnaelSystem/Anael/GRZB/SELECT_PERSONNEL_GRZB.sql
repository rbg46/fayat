﻿SELECT
   TRIM(FSTE) As CodeSocietePaye
  ,TRIM(A.P72SCA) AS CodeSocieteCompta
  ,TRIM(FETA) As CodeEtablissement
  ,TRIM(FMAT) As Matricule
  ,TRIM(FNOM) As Nom
  ,TRIM(FPRE) As Prenom
  ,TRIM(FTYPAI) As CategoriePerso
  ,TRIM(FSTATU) As Statut
  ,TRIM(P01BTP) AS CodePays
  ,TRIM(FEMPLO) As CodeEmploi
  ,CASE 
    WHEN  FEMBJJ >0 AND FEMBMM >0 AND FEMBAA >0
    THEN   CAST(FEMBAA || '-' || FEMBMM || '-' || FEMBJJ  As DATE)
    ELSE NULL 
  END As DateEntree
  ,CASE 
    WHEN  FSORJJ >0 AND FSORMM >0 AND FSORAA >0
    THEN   CAST(FSORAA || '-' || FSORMM || '-' || FSORJJ  As DATE)
    ELSE NULL 
  END As DateSortie  
  ,TRIM(FNORUE) As NumeroRue
  ,TRIM(FNOBIS) As NumeroRueDetail
  ,TRIM(FTYRUE) As TypeRue
  ,TRIM(FRUE) As NomRue
  ,TRIM(FLIEU) As NomLieuDit
  ,FPOST1 ||  FPOST2 As CodePostal
  ,TRIM(FVILLE) As Ville  
  ,CASE 
    WHEN  FMDAAA >0 AND FMDAMM >0 AND FMDAJJ >0
    THEN   CAST(FMDAAA || '-' || FMDAMM || '-' || FMDAJJ  As DATE)
    ELSE NULL 
  END As DateModification
FROM INTRB.SPPE1L6
LEFT JOIN PXBD.PR72P A ON A.P72STE= FSTE AND A.P72ETA= FETA
WHERE FSTE = '{0}' AND 
      (1 = '{1}' OR ((FMDAAA* 100000000) + (FMDAMM * 1000000) + (FMDAJJ * 10000) + SUBSTRING(RIGHT('0' || CAST(FMHEUR AS VARCHAR(6)), 6), 1, 4))  >= '{2}')
ORDER BY FSTE, FMAT