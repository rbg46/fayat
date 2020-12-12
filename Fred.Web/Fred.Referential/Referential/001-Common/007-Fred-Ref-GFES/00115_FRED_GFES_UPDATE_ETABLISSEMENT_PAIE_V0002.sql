-- Activation des établissements de paie (bug toolbox ?)
update FRED_ETABLISSEMENT_PAIE set Actif=1
where EtablissementPaieId in (
	select ep.EtablissementPaieId
	from FRED_ETABLISSEMENT_PAIE ep inner join FRED_SOCIETE s on ep.SocieteId=s.SocieteId
		inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
	where g.Code='GFES')