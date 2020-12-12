
-- PAS D'IMPORT DE DONNEES DE TESTS !!!

/*
-- MAJ des responsables administratifs

DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1016')
    WHERE Code='A00000001' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1016')
    WHERE Code='A00000002' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1025')
    WHERE Code='A00000003' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1025')
    WHERE Code='A00000004' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1008')
    WHERE Code='S8925' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1015')
    WHERE Code='S5000' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1010')
    WHERE Code='E5555' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)

UPDATE FRED_CI SET ResponsableAdministratifId=
    (SELECT p.PersonnelId
        FROM FRED_PERSONNEL p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
        WHERE s.GroupeId=@GroupeFESId and p.Matricule='1012')
    WHERE Code='E3333' and SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)
    */