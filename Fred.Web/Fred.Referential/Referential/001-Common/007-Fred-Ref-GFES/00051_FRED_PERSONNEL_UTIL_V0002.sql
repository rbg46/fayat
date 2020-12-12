-- SUPPRESSION des données de tests du script 00051_FRED_PERSONNEL_UTIL_V0001.sql
-- Exéctué à la main sur l'environnement de qualif
-- Ne doit pas être éxécuté en dev/integ et sert à rien sur la branche de prod.
/*

CREATE PROCEDURE #DROP_USER
    @Login VARCHAR(100)
AS
BEGIN
    print @Login
    declare @fad int;
    declare @uid int;
    set @fad = (select FayatAccessDirectoryId from FRED_UTILISATEUR where Login =@Login)
    set @uid = (select UtilisateurId from FRED_UTILISATEUR where Login =@Login)
    print @fad
    print @uid
    delete from FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE where @uid is not null and UtilisateurId = @uid
    delete from FRED_UTILISATEUR where @fad is not null and FayatAccessDirectoryId = @fad 
    delete from FRED_EXTERNALDIRECTORY where @fad is not null and FayatAccessDirectoryId = @fad 
END
GO


EXEC #DROP_USER 'cNGuyen';
EXEC #DROP_USER 'mVErkelens';
EXEC #DROP_USER 'lRobert';
EXEC #DROP_USER 'cBattisti';
EXEC #DROP_USER 'sLabousse';
EXEC #DROP_USER 'lCheriot';
EXEC #DROP_USER 'aVerlaguet';
EXEC #DROP_USER 'cGolenko';
EXEC #DROP_USER 'bDeschamps';
EXEC #DROP_USER 'dCocteau';
EXEC #DROP_USER 'cLelouche';
EXEC #DROP_USER 'rDeNiraux';
EXEC #DROP_USER 'aRoussiere';
EXEC #DROP_USER 'ePlatane';
EXEC #DROP_USER 'mRodriguez';
EXEC #DROP_USER 'yTriki';
EXEC #DROP_USER 'nPoulizac';
EXEC #DROP_USER 'jChatillon';
EXEC #DROP_USER 'jPetit';
EXEC #DROP_USER 'rGaillard';
EXEC #DROP_USER 'oSamson';
EXEC #DROP_USER 'sLefebvre';
EXEC #DROP_USER 'aPereira';
EXEC #DROP_USER 'mRoux';
EXEC #DROP_USER 'cLemoine';
EXEC #DROP_USER 'eLafitte';
EXEC #DROP_USER 'kKhobzi';
EXEC #DROP_USER 'tTrissac';

DROP PROCEDURE #DROP_USER
*/