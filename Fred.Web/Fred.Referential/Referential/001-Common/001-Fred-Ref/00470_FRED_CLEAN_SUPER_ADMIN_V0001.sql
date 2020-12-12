
-- Tous les utilisateurs SuperAdmin, hors FAYAT IT, prennent le rôle "Admin Appli" sur leur Société
CREATE TABLE #TMP (UtilisateurId INT NOT NULL, 
   SocieteId INT NOT NULL, 
   SocieteOrganisationId INT NOT NULL,
   RoleId INT)
   


INSERT INTO #TMP(UtilisateurId, SocieteId, SocieteOrganisationId) 
SELECT u.UtilisateurId, p.SocieteId, s.OrganisationId
FROM FRED_UTILISATEUR u
INNER JOIN FRED_PERSONNEL p ON u.UtilisateurId = p.PersonnelId
LEFT OUTER JOIN FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE urod ON urod.UtilisateurId = u.UtilisateurId 
LEFT OUTER JOIN FRED_ROLE r ON urod.RoleId = r.RoleId
LEFT OUTER JOIN FRED_SOCIETE s ON s.SocieteId = p.SocieteId
WHERE  s.Code<>'FIT'-- Société différente de FAYAT IT
  AND u.SuperAdmin = 1 
-- On exclut les utlisateurs qui sont déjà "Admin Appli" sur leur Société
EXCEPT
SELECT u.UtilisateurId, p.SocieteId, s.OrganisationId
FROM FRED_UTILISATEUR u
INNER JOIN FRED_PERSONNEL p ON u.UtilisateurId = p.PersonnelId
LEFT OUTER JOIN FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE urod ON urod.UtilisateurId = u.UtilisateurId 
LEFT OUTER JOIN FRED_ROLE r ON urod.RoleId = r.RoleId
LEFT OUTER JOIN FRED_SOCIETE s ON s.SocieteId = p.SocieteId
WHERE  s.Code<>'FIT'-- Société différente de FAYAT IT
  AND u.SuperAdmin = 1 
  AND r.Libelle like '%Admin Appli%'; -- Code et CodeNomFamilier ne sont pas fiable pour retrouver le rôle Admin Appli



UPDATE tmp 
SET RoleId = r.RoleId
FROM #TMP tmp
INNER JOIN FRED_ROLE r ON r.SocieteId = tmp.SocieteId
WHERE r.Libelle like '%Admin Appli%';



INSERT INTO FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE(UtilisateurId, OrganisationId, RoleId) 
SELECT UtilisateurId, SocieteOrganisationId, RoleId FROM #TMP where RoleId IS NOT NULL;


DROP TABLE #TMP


 --Tous les utilisateurs, hors FAYAT IT, perdent leur statut SuperAdmin
UPDATE u SET u.SuperAdmin = 0 
FROM FRED_UTILISATEUR u
INNER JOIN FRED_PERSONNEL p ON u.UtilisateurId = p.PersonnelId
INNER JOIN FRED_SOCIETE s ON s.SocieteId = p.SocieteId
WHERE s.Code<>'FIT'-- Société différente de FAYAT IT
  AND u.SuperAdmin = 1
