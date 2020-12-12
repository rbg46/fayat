-- --------------------------------------------------
-- Correction des anomalies pour les réceptions Automatiqueq des commandes Abonnnements 
-- --------------------------------------------------
begin 
update  DA set da.DateSuppression=da.DateCreation
from FRED_COMMANDE C
inner join FRED_STATUT_COMMANDE STC on stc.StatutCommandeId=c.StatutCommandeId
inner join FRED_COMMANDE_LIGNE L on c.CommandeId=l.CommandeId
inner join FRED_DEPENSE_ACHAT DA on da.CommandeLigneId=l.CommandeLigneId
inner join FRED_DEPENSE_TYPE DP on dp.DepenseTypeId=da.DepenseTypeId
inner join FRED_CI Ci on da.CiId=ci.CiId 
inner join FRED_SOCIETE S on ci.SocieteId=s.SocieteId
where c.IsAbonnement=1
and stc.Code='VA'
and c.DureeAbonnement>0
and da.DateVisaReception is null
and s.Code='MBTP'
and dp.Code='1'
and da.DepenseId = DepenseId
and da.DateSuppression IS NULL;
end

