

-- MEP FES / 16-01-2019


EXEC Fred_ToolBox_Code_Majoration @Code='THRA', @Libelle='Travail heures de route aller', @EtatPublic='Oui', @IsActif='Oui', @GroupeCode='GFES', @IsHeureNuit='Non';
EXEC Fred_ToolBox_Code_Majoration @Code='THRR', @Libelle='Travail heures de route retour', @EtatPublic='Oui', @IsActif='Oui', @GroupeCode='GFES', @IsHeureNuit='Non';
EXEC Fred_ToolBox_Code_Majoration @Code='TNH1', @Libelle='Travail de nuit (Horaire)', @EtatPublic='Oui', @IsActif='Oui', @GroupeCode='GFES', @IsHeureNuit='Oui';
EXEC Fred_ToolBox_Code_Majoration @Code='TNH2', @Libelle='Travail de nuit (Horaire)', @EtatPublic='Oui', @IsActif='Oui', @GroupeCode='GFES', @IsHeureNuit='Oui';