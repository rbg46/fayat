-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE FAYAT TP
-- --------------------------------------------------


-- SUPPRESSION DES SOCIETES CREES AUPARAVENT
--DELETE FROM FRED_SOCIETE WHERE Code = 'FTP' AND GroupeId = 3;

--DELETE FROM FRED_SOCIETE_DEVISE WHERE SocieteId IN (SELECT SocieteId FROM FRED_SOCIETE where GroupeId = 3);


--DELETE FROM FRED_SOCIETE WHERE Code = 'SOMOPA' AND GroupeId = 3;
--DELETE FROM FRED_GROUPE WHERE GroupeId = 3 AND Code = 'GFTP';

PRINT ' --------------------------------'
PRINT 'INFO : SUPPRESSION GROUPE 3 et SOCIETE SOMOPA et FTP LIEE AU GROUPE 3'
PRINT ' --------------------------------'