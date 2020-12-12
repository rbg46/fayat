SELECT
   TRIM(FSTE) As CodeSocietePaye --OK
  --,TRIM(A.P72SCA) AS CodeSocieteCompta
  ,TRIM(FETA) As CodeEtablissement --OK
  ,TRIM(FMAT) As Matricule --OK
  ,TRIM(FNOM) As Nom --OK
  ,TRIM(FPRE) As Prenom --OK
  ,TRIM(FTYPAI) As CategoriePerso -- erreur FTYPAI1 => FSAIP1
  ,TRIM(FSTATU) As Statut  --OK
  ,TRIM(FEMPLO) As CodeEmploi  --OK
  ,CASE 
    WHEN  FEMBJJ >0 AND FEMBMM >0 AND FEMBAA >0
    THEN   CAST(FEMBAA || '-' || FEMBMM || '-' || FEMBJJ  As DATE)
    ELSE NULL 
  END As DateEntree --OK
  ,CASE 
    WHEN  FSORJJ >0 AND FSORMM >0 AND FSORAA >0
    THEN   CAST(FSORAA || '-' || FSORMM || '-' || FSORJJ  As DATE)
    ELSE NULL 
  END As DateSortie   --OK
  --,TRIM(FNORUE) As NumeroRue
  --,TRIM(FNOBIS) As NumeroRueDetail -- delta en -
  --,TRIM(FTYRUE) As TypeRue -- delta en -
  --,TRIM(FRUE) As NomRue -- delta en -
  --,TRIM(FLIEU) As NomLieuDit -- delta en -
  --,FPOST1 ||  FPOST2 As CodePostal -- delta en -
  --,TRIM(FVILLE) As Ville  -- delta en -
  ,CASE 
    WHEN  FMDAAA >0 AND FMDAMM >0 AND FMDAJJ >0
    THEN   CAST(FMDAAA || '-' || FMDAMM || '-' || FMDAJJ  As DATE)
    ELSE NULL 
  END As DateModification
  ,TRIM(EMAIL_COLLAB) As Email  
  ,TRIM(FPECI) As Section  
  ,TRIM(FPECI2) As Entreprise  
  ,TRIM(STE_MANAGER) As SocieteManager 
  ,TRIM(MAT_MANAGER) As MatriculeManager 
FROM INTFOND.SPPE1L6 

-- EXEMPLE FES
--WHERE FSTE IN ('001','002') AND 
--      (1 = '0' OR ((FMDAAA* 100000000) + (FMDAMM * 1000000) + (FMDAJJ * 10000) + SUBSTRING(RIGHT('0' || CAST(FMHEUR AS VARCHAR(6)), 6), 1, 4))  >= '201801010101')
WHERE FSTE IN ( {0}) AND 
(1 = '{1}' OR ((FMDAAA* 100000000) + (FMDAMM * 1000000) + (FMDAJJ * 10000) + SUBSTRING(RIGHT('0' || CAST(FMHEUR AS VARCHAR(6)), 6), 1, 4))  >= '{2}')
ORDER BY FSTE, FMAT
