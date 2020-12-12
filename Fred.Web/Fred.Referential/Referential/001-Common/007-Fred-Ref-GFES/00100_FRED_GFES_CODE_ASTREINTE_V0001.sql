
DECLARE @GroupeId INT;
Set @GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFES')
insert into FRED_Code_Astreinte  Values ('ASTRS','Astreinte du lundi au vendredi',0,@GroupeId)
insert into FRED_Code_Astreinte  Values ('ASTRWE','Astreinte samedi ou dimanche',0,@GroupeId)
insert into FRED_Code_Astreinte  Values ('TASTRS','Sortie d astreinte du lundi au vendredi',1,@GroupeId)
insert into FRED_Code_Astreinte  Values ('AS200','Sortie d astreinte samedi ou dimanche',1,@GroupeId)


