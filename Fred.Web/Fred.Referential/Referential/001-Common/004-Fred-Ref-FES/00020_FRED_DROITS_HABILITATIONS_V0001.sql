--Insertion des roles ResponsableCI et Délégué
INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier, Specification) 
  SELECT 'RCI','Responsable CI',NULL,0,1,1,1,'Responsable CI',s.SocieteId,'RCI', 1
  FROM FRED_SOCIETE s inner join FRED_GROUPE g on g.GroupeId = s.GroupeId
    inner join FRED_POLE p on g.PoleId = p.PoleId
  WHERE p.Code = 'PFES'

INSERT INTO FRED_ROLE (Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta,Description,SocieteId,CodeNomFamilier, Specification) 
  SELECT 'DCI','Délégué CI',NULL,0,1,1,1,'Délégué CI',s.SocieteId,'DCI', 2
  FROM FRED_SOCIETE s inner join FRED_GROUPE g on g.GroupeId = s.GroupeId
    inner join FRED_POLE p on g.PoleId = p.PoleId
  WHERE p.Code = 'PFES'

--Association des roles : ResponsableCI et Délégué aux fonctionnalités gestion équipe et délégation
INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
  FROM FRED_ROLE r, FRED_FONCTIONNALITE f
  where r.Specification=1	 and f.code='1102'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 2
  FROM FRED_ROLE r, FRED_FONCTIONNALITE f
  where r.Specification=1	 and f.code='1103'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
  FROM FRED_ROLE r, FRED_FONCTIONNALITE f
  where r.Specification=2	 and f.code='1102'

INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
  SELECT r.RoleId, f.FonctionnaliteId, 1
  FROM FRED_ROLE r, FRED_FONCTIONNALITE f
  where r.Specification=2	 and f.code='1103'