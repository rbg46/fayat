IF NOT EXISTS( SELECT * FROM FRED_CI WHERE CODE ='064701')
BEGIN
	DECLARE @SocieteId int, @PersonnelId int , @EtablissementComptableId int, @OrganisationId int
	
	SELECT TOP 1  @SocieteId = SOCIETEID FROM FRED_SOCIETE
	WHERE CODE ='0001'
	
	SELECT TOP 1  @PersonnelId = PERSONNELID FROM FRED_PERSONNEL
	WHERE SOCIETEID = @SocieteId
	
	SELECT TOP 1 @EtablissementComptableId = ETABLISSEMENTCOMPTABLEID, @OrganisationId = OrganisationId FROM FRED_ETABLISSEMENT_COMPTABLE
	WHERE SOCIETEID = @SocieteId
	
	
	INSERT INTO FRED_CI (
	EtablissementComptableId,
	Code,
	Libelle,
	Sep,
	DateOuverture,
	DateFermeture,
	Adresse,
	Ville,
	CodePostal,
	EnteteLivraison,
	AdresseLivraison,
	CodePostalLivraison,
	VilleLivraison,
	AdresseFacturation,
	CodePostalFacturation,
	VilleFacturation,
	LongitudeLocalisation,
	LatitudeLocalisation,
	FacturationEtablissement,
	ResponsableChantier,
	ResponsableAdministratifId,
	FraisGeneraux,
	TauxHoraire,
	HoraireDebutM,
	HoraireFinM,
	TypeCI,
	ZoneModifiable,
	CarburantActif,
	MontantHT,
	DureeChantier,
	MontantDeviseId,
	HoraireDebutS,
	HoraireFinS,
	SocieteId,
	OrganisationId,
	Adresse2,
	Adresse3,
	PaysId,
	PaysLivraisonId,
	PaysFacturationId,
	ChantierFRED,
	DateImport,
	DateUpdate,
	CITypeId,
	IsAstreinteActive,
	Description,
	CodeExterne,
	ResponsableChantierId,
	CompteInterneSepId,
	ObjectifHeuresInsertion,
	IsDisableForPointage)
	VALUES(@EtablissementComptableId,'064701','CREATION BASSIN CARES Assainissement',0,'2019-10-30 14:42:00.000','2030-12-31 14:42:00.000','Rue de Cares','Eysines',33320,NULL, 'CARES   ',33320,'EYSINES','197 Avenue Clément Fayat',33502,'LIBOURNE',0,0,1,'GEOFFREY GARCIA',@PersonnelId,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL,NULL,NULL,@SocieteId,@OrganisationId,NULL,NULL,1,1,1,1,NULL,NULL,NULL,0,NULL,NULL,@PersonnelId,NULL,NULL,0)

END