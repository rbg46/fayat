-- chantier en erreur qui viennent des API
 -- les chantiers avec chantierfred = 0 alors qu'il devrait etre a 1
 select g.Code, ci.CiId,ci.Code,ci.Libelle, ci.SocieteId,s.Code as Societe,	ci.ChantierFRED	 from fred_ci ci
  left outer join fred_societe s on  ci.SocieteId = s.SocieteId 
  left outer join fred_groupe g on  s.groupeid = g.groupeid 
 where  g.Code IN ('GFES','GFTP','GFON') and ci.chantierfred = 0
 
UPDATE
    fred_ci
SET
    fred_ci.ChantierFRED = 1
WHERE
    societeid in (select s.societeid from fred_societe s left outer join fred_groupe g on  s.groupeid = g.groupeid where g.Code IN ('GFES','GFTP','GFON') ) and chantierfred = 0 ;
