-- Alimentation de la table FRED_SITE

CREATE PROCEDURE #SET_FRED_SITE
	@Code NVARCHAR(MAX), 
	@Libelle NVARCHAR(MAX), 
	@DateCreation datetime

AS
BEGIN
	IF NOT EXISTS ( SELECT 1 FROM FRED_SITE WHERE Code = @Code)
	BEGIN
		INSERT INTO FRED_SITE (Code, Libelle, DateCreation)
		VALUES (@Code, @Libelle, @DateCreation)
	END
END
GO

-- Insertion des données

EXEC #SET_FRED_SITE @Code='1', @Libelle='MATERIEL GRIGNY', @DateCreation='2012-11-29';
EXEC #SET_FRED_SITE @Code='2', @Libelle='MATERIEL CHAPONOST', @DateCreation='2012-11-29';
EXEC #SET_FRED_SITE @Code='ANTONY', @Libelle='Antony', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='BAGNOLET', @Libelle='BAGNOLET', @DateCreation='2008-08-08';
EXEC #SET_FRED_SITE @Code='BLANQUEFOR', @Libelle='Blanquefort', @DateCreation='2006-11-29';
EXEC #SET_FRED_SITE @Code='BONNEUIL', @Libelle='BONNEUIL SUR MARNE', @DateCreation='2013-04-03';
EXEC #SET_FRED_SITE @Code='BORDEAUX', @Libelle='Bordeaux', @DateCreation='2010-09-29';
EXEC #SET_FRED_SITE @Code='CANNES', @Libelle='Cannes ', @DateCreation='2010-04-21';
EXEC #SET_FRED_SITE @Code='CUINCY', @Libelle='Cuincy', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='DOMONT', @Libelle='Domont (95)', @DateCreation='2010-03-31';
EXEC #SET_FRED_SITE @Code='GDE SYNTHE', @Libelle='GRANDE SYNTHE', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='HENIN', @Libelle='Hénin Beaumont', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='LYON', @Libelle='Lyon ', @DateCreation='2010-08-09';
EXEC #SET_FRED_SITE @Code='METZ', @Libelle='Metz', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='MEYREUIL', @Libelle='Meyreuil', @DateCreation='2012-01-18';
EXEC #SET_FRED_SITE @Code='MONACO', @Libelle='MONACO', @DateCreation='2009-12-16';
EXEC #SET_FRED_SITE @Code='MONTROUGE', @Libelle='Montrouge', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='MOUANS', @Libelle='Mouans Sartoux', @DateCreation='2006-11-29';
EXEC #SET_FRED_SITE @Code='NANTERRE', @Libelle='Nanterre', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='NOISY', @Libelle='NOISY LE SEC', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='PACA', @Libelle='P.A.C.A', @DateCreation='2010-09-29';
EXEC #SET_FRED_SITE @Code='ST GREG', @Libelle='Saint Gregoire', @DateCreation='2013-07-18';
EXEC #SET_FRED_SITE @Code='TOURCOING', @Libelle='Tourcoing', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='TRITH', @Libelle='Trith Saint Léger', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='VIRY', @Libelle='Viry Chatillon', @DateCreation='2007-04-11';
EXEC #SET_FRED_SITE @Code='POULLAINVI', @Libelle='Poullainville (Amiens)', @DateCreation='2014-04-02';
EXEC #SET_FRED_SITE @Code='TROYES', @Libelle='Troyes', @DateCreation='2014-04-02';
EXEC #SET_FRED_SITE @Code='Orgeres', @Libelle='Orgères en beauce', @DateCreation='2014-12-11';
EXEC #SET_FRED_SITE @Code='Changé', @Libelle='ERS MAINE  Changé', @DateCreation='2014-12-11';
EXEC #SET_FRED_SITE @Code='TOULOUSE', @Libelle='TOULOUSE', @DateCreation='2016-02-12';
EXEC #SET_FRED_SITE @Code='EI REUNION', @Libelle='FES REUNION', @DateCreation='2017-07-07';


-- Fin insertion

DROP PROCEDURE #SET_FRED_SITE