﻿-- Ajout de trois établissements de paie manquants
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@AgenceRattachementCode=null,@Code='12',@Libelle='SATELEC GP',@Adresse='24 avenue du Général de Gaulle ',@CodePostal='91170',@Ville='VIRY-CHATILLON',@PaysCode='FR',@Actif=1,@Latitude=48.6727919,@Longitude=2.3893148
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@AgenceRattachementCode=null,@Code='26',@Libelle='SATELEC FRETHUN',@Adresse='17 rue de l''Abbé Grégoire ',@CodePostal='59760',@Ville='GRANDE-SYNTHE',@PaysCode='FR',@Actif=1,@Latitude=50.9961,@Longitude=2.3017
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E015',@IsAgenceRattachement=1,@AgenceRattachementCode=null,@Code='40',@Libelle='GABRIELLE CADOURS',@Adresse='0 Route de Toulouse Lassoulan',@CodePostal='31480',@Ville='CADOURS',@PaysCode='FR',@Actif=1,@Latitude=43.729885,@Longitude= 1.06118