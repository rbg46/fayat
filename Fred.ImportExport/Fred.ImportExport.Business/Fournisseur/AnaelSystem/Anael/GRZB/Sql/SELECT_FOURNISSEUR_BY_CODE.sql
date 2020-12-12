SElECT DISTINCT
       TRIM(Tiers.RAUX) As Code
       ,TRIM(Tiers.RSEQ) As TypeSequence
       ,TRIM(Tiers.RSTE) As CodeSociete       
       ,TRIM(Tiers.TNOM) As Libelle
       ,TRIM(Tiers.TAD1) || ' ' || TRIM(Tiers.TAD2) || ' ' || TRIM(Tiers.TAD3) As Adresse
       ,TRIM(Tiers.TPOST) As CodePostal
       ,TRIM(Tiers.TVILLE) As Ville
       ,TRIM(Tiers.TTEL) As Telephone
       ,TRIM(Tiers.TFAX) As Fax			 
       ,SUBSTRING(TRIM(Tiers.TPAYS), 1, 2) As CodePays
       ,TRIM(Tiers.TEMAIL) As Email       
       ,TRIM(Tiers.TSIRET) As SIRET
       ,TRIM(Tiers.TSIREN) As SIREN
       ,TRIM(B.FCAD3) As ComplementAdresse
       ,TRIM(C.TKLEPS) AS RegleGestion
       ,TRIM(C.TREGLT) As ModeReglement
       ,TRIM(Tiers.TMOT) As CritereRecherche
       ,TRIM(D.TISO) As IsoTVA
       ,TRIM(D.TIDENT) As NumeroTVA
       ,C.TCC1 AS CC1
       ,C.TCC2 AS CC2
       ,C.TCC3 AS CC3
       ,C.TCC4 AS CC4
       ,C.TCC5 AS CC5
       ,C.TCC6 AS CC6
       ,C.TCC7 AS CC7
       ,C.TCC8 AS CC8
       ,CASE 
          WHEN  C.TOUVA >0 AND C.TOUVM >0 AND C.TOUVJ >0
          THEN   CAST(C.TOUVA || '-' || C.TOUVM || '-' || C.TOUVJ  As DATE)
       ELSE NULL 
       END  As DateOuverture
       ,CASE 
          WHEN  C.TFERA >0 AND  C.TFERM >0 AND C.TFERJ >0
          THEN   CAST(C.TFERA || '-' || C.TFERM || '-' || C.TFERJ  As DATE)
          ELSE NULL 
       END As DateFermeture       
       ,TTELEX  as IsProfessionLiberale
FROM AXFILE.FAN050P1 Tiers -- Source de données Compte Tiers
LEFT JOIN AXFILE.FEG050P1 AS B ON Tiers.RSTE = B.RSTE and Tiers.RSEQ = B.RSEQ and Tiers.RAUX = B.RAUX
LEFT JOIN AXFILE.FAN052P1 AS C ON Tiers.RSTE = C.RSTE AND Tiers.RSEQ = C.RSEQ AND Tiers.RAUX = C.RAUX -- source données Règle de gestion                                                                                                                             
LEFT JOIN AXFILE.FTV050P1 AS D ON Tiers.RSTE = D.RSTE AND Tiers.RSEQ = D.RGEN AND TRIM(Tiers.RAUX) = TRIM(D.RAUX)
WHERE 	1=1
        AND TRIM(Tiers.RSTE) IN ({0})
        AND TRIM(Tiers.RSEQ) IN ({1})
        AND TRIM(C.TKLEPS) IN ({2})  
        AND TRIM(Tiers.RAUX) IN ({3})
ORDER BY Code, TypeSequence