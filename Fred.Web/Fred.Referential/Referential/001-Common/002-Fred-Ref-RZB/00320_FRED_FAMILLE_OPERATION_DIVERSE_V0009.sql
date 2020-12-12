select libelle 
into #Libelle
from FRED_FAMILLE_OPERATION_DIVERSE 
where code in('RCT','FG','OTHD')

select top 4 tacheId, t.Libelle 
into #Update
from FRED_TACHE t
inner join #Libelle l on t.Libelle = 'ECART ' +l.Libelle
ORDER BY ROW_NUMBER() OVER(PARTITION BY t.libelle ORDER BY tacheid );

Update f set f.TacheId = u.TacheId
from FRED_FAMILLE_OPERATION_DIVERSE f
inner join #Update u on u.Libelle =  'ECART ' +f.Libelle

DROP TABLE #Libelle, #Update