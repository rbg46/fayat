------------------------------
-- Recatégorisation de la création des modules et des premières fonctionnalités
------------------------------

IF NOT EXISTS (SELECT ModuleId FROM FRED_MODULE WHERE ModuleId in (10, 11, 12, 13, 14, 15, 18, 19, 20, 21, 22))
BEGIN
  SET IDENTITY_INSERT [dbo].[FRED_MODULE] ON;
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (10,3,'Paramétrage Ressources Humaines',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (11,4,'Pointage',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (12,5,'Paramétrage des Habilitations',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (13,6,'Paramétrage Organisation',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (14,7,'Paramétrage référentiel',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (15,8,'Achats',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (18,11,'Gestion des affaires',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (19,12,'Gestion budgetaire',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (20,13,'Module En cours de developpement',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (21,9,'Exploitation',NULL);
  INSERT INTO FRED_MODULE (ModuleId,Code,Libelle,Description) VALUES (22,10,'Edition',NULL);
  SET IDENTITY_INSERT [dbo].[FRED_MODULE] OFF;
END

IF NOT EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE FonctionnaliteId in (17, 18, 19, 20, 21, 22, 28, 29, 30, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53))
BEGIN
  SET IDENTITY_INSERT [dbo].[FRED_FONCTIONNALITE] ON;
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (17,10,300,'Affichage des menus Administration RH',0,NULL,'Affiche les menus des différentes fenêtre de code lié à la gestion des ressources humaines');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (18,11,400,'Affichage du menu ''Liste des rapports''',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (19,12,500,'Affichage des menus ''Habilitation''',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (20,14,700,'Gestion des devises',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (21,13,600,'Gestion des établissements comptables',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (22,15,800,'Gestion des commandes',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (28,20,1300,'Affichage des menus ''DEVELOPPEUR ONLY''',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (29,10,301,'Affichage de la liste des personnels',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (30,10,302,'Gestion du personnel',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (32,11,401,'Affichage de la page ''Nouveau rapport''',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (33,11,402,'Affichage de la page ''Liste des  pointages personnel''',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (34,11,403,'Affichage de la page ''Validation lots de pointage''',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (35,11,404,'Affichage de la page ''Détail d''un rapport''',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (36,12,501,'Affichage du journal de connexion',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (37,13,601,'Gestion des sociétés',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (38,13,602,'Gestion des établissements de paie',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (39,13,603,'Gestion des fournisseurs',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (40,14,701,'Gestion des codes de natures analytiques',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (41,14,702,'Gestion du plan de ressources',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (42,14,703,'Gestion des natures analytiques / ressources',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (43,14,704,'Gestion du matériel',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (44,15,801,'Nouvelle commande',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (45,15,802,'Gestion des réceptions',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (46,15,803,'Accès au tableau des réceptions',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (47,21,900,'Barème CI',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (48,21,901,'Barème organisation',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (49,21,902,'OD',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (50,21,903,'Bilan flash',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (51,18,1100,'Centre d''imputation',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (52,18,1101,'Clôtures',0,NULL,'NULL');
  INSERT INTO FRED_FONCTIONNALITE (FonctionnaliteId,ModuleId,Code,Libelle,HorsOrga,DateSuppression,Description) VALUES (53,19,1200,'Plan de tâches',0,NULL,'NULL');
  SET IDENTITY_INSERT [dbo].[FRED_FONCTIONNALITE] OFF;
END

IF NOT EXISTS (SELECT PermissionFonctionnaliteId FROM [FRED_PERMISSION_FONCTIONNALITE] WHERE PermissionFonctionnaliteId BETWEEN 1 AND 40)
BEGIN
  SET IDENTITY_INSERT [dbo].[FRED_PERMISSION_FONCTIONNALITE] ON;
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (1,3,17);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (2,4,17);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (3,6,17);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (4,5,17);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (5,26,17);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (6,22,29);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (7,39,30);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (9,24,18);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (10,40,32);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (11,25,33);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (12,34,34);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (13,23,35);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (14,32,19);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (15,20,19);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (16,18,19);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (17,0,36);

  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (18,9,21);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (19,13,21);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (20,33,37);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (21,14,38);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (22,16,39);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (23,28,20);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (24,19,40);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (25,30,41);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (26,29,42);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (27,17,43);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (28,35,22);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (29,7,44);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (30,27,45);

  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (36,2,51);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (37,36,51);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (38,8,52);
  INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionFonctionnaliteId,PermissionId,FonctionnaliteId) VALUES (40,31,53);
  SET IDENTITY_INSERT [dbo].[FRED_PERMISSION_FONCTIONNALITE] OFF;
END