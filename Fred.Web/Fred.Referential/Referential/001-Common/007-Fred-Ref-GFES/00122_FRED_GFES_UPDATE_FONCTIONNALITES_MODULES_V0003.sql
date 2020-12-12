---- Mise à jour de libellés et code
--DECLARE @OldFoncId INT
--DECLARE @NewFoncId INT
--SET @OldFoncId= (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code='1202')
--SET @NewFoncId= (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code='12002')
--IF (@OldFoncId is not null AND @NewFoncId is not null)
--BEGIN
--	INSERT INTO FRED_PERMISSION_FONCTIONNALITE SELECT PermissionId, @NewFoncId FROM FRED_PERMISSION_FONCTIONNALITE WHERE FonctionnaliteId=@OldFoncId

--	DELETE FROM FRED_PERMISSION_FONCTIONNALITE WHERE FonctionnaliteId=@OldFoncId
--	DELETE FROM FRED_FONCTIONNALITE WHERE FonctionnaliteId=@OldFoncId

--	UPDATE FRED_FONCTIONNALITE SET Libelle='Budget' WHERE FonctionnaliteId=@NewFoncId
--END

--UPDATE FRED_FONCTIONNALITE set Code='12003' WHERE Code='1203'
--UPDATE FRED_FONCTIONNALITE set Code='12004' WHERE Code='1204'

---- Suppression du Mode développeur
--DELETE FROM FRED_PERMISSION_FONCTIONNALITE
--WHERE PermissionFonctionnaliteId in (
--	select pf.PermissionFonctionnaliteId from FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId=f.FonctionnaliteId where f.Code='1300')

--DELETE FROM FRED_ROLE_FONCTIONNALITE
--WHERE FonctionnaliteId in (select FonctionnaliteId from FRED_FONCTIONNALITE where Code='1300')

--DELETE FROM FRED_FONCTIONNALITE_DESACTIVE
--WHERE FonctionnaliteId in (select FonctionnaliteId from FRED_FONCTIONNALITE where Code='1300')

--DELETE FROM FRED_FONCTIONNALITE WHERE Code='1300'

--DELETE FROM FRED_MODULE WHERE Code='13'

---- Mise à niveau de l'Admin SI
--SELECT pf.PermissionFonctionnaliteId, p.Code as PermCode INTO #tmpPermFonc
--FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId=p.PermissionId
--	inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId=f.FonctionnaliteId
--WHERE f.Code = '1401'

--IF NOT EXISTS (select PermissionFonctionnaliteId from #tmpPermFonc where PermCode='0000')
--	INSERT INTO FRED_PERMISSION_FONCTIONNALITE
--		SELECT p.PermissionId, f.FonctionnaliteId FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f WHERE p.Code='0000' and f.Code='1401'

--IF NOT EXISTS (select PermissionFonctionnaliteId from #tmpPermFonc where PermCode='0052')
--	INSERT INTO FRED_PERMISSION_FONCTIONNALITE
--		SELECT p.PermissionId, f.FonctionnaliteId FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f WHERE p.Code='0052' and f.Code='1401'

--IF NOT EXISTS (select PermissionFonctionnaliteId from #tmpPermFonc where PermCode='0018')
--	INSERT INTO FRED_PERMISSION_FONCTIONNALITE
--		SELECT p.PermissionId, f.FonctionnaliteId FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f WHERE p.Code='0018' and f.Code='1401'

--DROP TABLE #tmpPermFonc

---- Gestion des dépenses
--DELETE FROM FRED_MODULE_DESACTIVE
--WHERE ModuleId in (select ModuleId from FRED_MODULE where Code='15')

--INSERT INTO FRED_MODULE_DESACTIVE
--SELECT s.SocieteId, m.ModuleId FROM FRED_MODULE m, FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
--WHERE m.Code='15' and g.Code!='FIT'

---- Pointages
--DELETE FROM FRED_FONCTIONNALITE_DESACTIVE
--WHERE FonctionnaliteDesactiveId in (
--	select fd.FonctionnaliteDesactiveId
--	from FRED_FONCTIONNALITE_DESACTIVE fd inner join FRED_FONCTIONNALITE f on fd.FonctionnaliteId=f.FonctionnaliteId
--		inner join FRED_MODULE m on f.ModuleId=m.ModuleId
--	where m.code='4')

--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='401' and g.Code='GFES'
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='403' and g.Code in ('GFON', 'GFTP')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='405' and g.Code in ('GFON', 'GFTP')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='409' and g.Code in ('GFON', 'GFTP', 'GFES')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='410' and g.Code in ('GFON', 'GFTP', 'GRZB')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='411' and g.Code in ('GFON', 'GFTP', 'GRZB')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='412' and g.Code in ('GFON', 'GFTP', 'GRZB')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='413' and g.Code in ('GFON', 'GFTP', 'GRZB')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='416' and g.Code in ('GFON', 'GFTP', 'GRZB')

---- Gestion des moyens
--DELETE FROM FRED_MODULE_DESACTIVE
--WHERE ModuleId in (select ModuleId from FRED_MODULE where Code='51')

--INSERT INTO FRED_MODULE_DESACTIVE select s.SocieteId, m.ModuleId FROM FRED_MODULE m, FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId WHERE m.Code='51' and g.Code not in ('FIT', 'GFES')

---- Exploitation
--DELETE FROM FRED_MODULE_DESACTIVE
--WHERE ModuleId in (select ModuleId from FRED_MODULE where Code='9')

--DELETE FROM FRED_FONCTIONNALITE_DESACTIVE
--WHERE FonctionnaliteId in (
--	select fd.FonctionnaliteDesactiveId
--	from FRED_FONCTIONNALITE_DESACTIVE fd inner join FRED_FONCTIONNALITE f on fd.FonctionnaliteId=f.FonctionnaliteId
--		inner join FRED_MODULE m on f.ModuleId=m.ModuleId
--	where m.code='9')

--DELETE FROM FRED_PERMISSION_FONCTIONNALITE
--WHERE FonctionnaliteId in (select FonctionnaliteId from FRED_FONCTIONNALITE where Code in ('906', '907', '908'))

--DELETE FROM FRED_ROLE_FONCTIONNALITE
--WHERE FonctionnaliteId in (select FonctionnaliteId from FRED_FONCTIONNALITE where Code in ('906', '907', '908'))

--DELETE FROM FRED_FONCTIONNALITE WHERE Code in ('906', '907', '908')

--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='900' and g.Code in ('GFON', 'GFES')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='901' and g.Code in ('GFON', 'GFES')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='902' and g.Code in ('GFON', 'GFES', 'GFTP')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='903' and g.Code in ('GFON', 'GFES', 'GFTP', 'GRZB')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='904' and g.Code in ('GFON', 'GFES')
--INSERT INTO FRED_FONCTIONNALITE_DESACTIVE select s.SocieteId, f.FonctionnaliteId from FRED_SOCIETE s inner join FRED_GROUPE g on s.GroupeId=g.GroupeId, FRED_FONCTIONNALITE f where f.Code='905' and g.Code in ('GFON', 'GFES')
