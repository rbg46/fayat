/*IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='ExplorateurDepensesFiltresMOMateriels')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'ExplorateurDepensesFiltresMOMateriels', 0, GetDate(), N'super_fred', 13)
END */


/*Déplacement du code car ce nom de fichier a déjà été utilisé sur une autre branche 
donc pour éviter ce problème on déplace ce code (qui n'a pas pu être exécuté sur tous environnement)
dans un autre fichier dont le nom n'est utilisé sur aucune autre branche*/
