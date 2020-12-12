if not exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Notification_Periode%')
BEGIN
INSERT INTO FRED_PARAM_KEY ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId]) VALUES ('Notification_Periode','la Clé pour identifier la période pour afficher les notifications ',getDate(),getDate(),1,1);
END

DECLARE @NotificationPeriodeId INT ;
 set @NotificationPeriodeId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Notification_Periode%');


DECLARE @OrganisationId int
DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT DISTINCT OrganisationId 
FROM FRED_ORGANISATION where TypeOrganisationId = 4

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN 
   
  INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('30000',getDate(),getDate(),1,1,@OrganisationId, @NotificationPeriodeId);

  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR
