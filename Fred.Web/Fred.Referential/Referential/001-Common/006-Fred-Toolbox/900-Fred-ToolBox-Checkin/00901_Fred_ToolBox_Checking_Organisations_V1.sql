
-- --------------------------------------------------
-- FRED 2017 - R4 - SEPTEMBRE 2018 
-- TOOLBOX Controle des organisations
-- MODOP : 
--    EXEC Fred_ToolBox_Check_Organisations
-- --------------------------------------------------


/*PROCEDURE STOCKEES*/
IF OBJECT_ID ( 'Fred_ToolBox_Check_Organisations', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Check_Organisations;  
GO  
CREATE PROCEDURE Fred_ToolBox_Check_Organisations   

AS   




PRINT '----------------------------------'
PRINt ' CONTROLE DES DONNEES'
PRINT '----------------------------------'


DECLARE @OrganisationId_A INT;
Declare A Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 1
Open A
Fetch next From A into @OrganisationId_A
While @@Fetch_Status=0 Begin
   
   -- HOLDING
   DECLARE @HOLDING VARCHAR (20)
   SET @HOLDING = (SELECT Libelle FROM FRED_HOLDING where HoldingId = @OrganisationId_A)
   PRINT CAST(@OrganisationId_A AS VARCHAR(20)) + '-' + @HOLDING

   -- RECHERCHE POLE

		DECLARE @OrganisationId_B INT;
		Declare B Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 2 AND PereId = @OrganisationId_A
		Open B
		Fetch next From B into @OrganisationId_B
		While @@Fetch_Status=0 Begin
   
		   -- HOLDING
		   DECLARE @POLE VARCHAR (20)
		   SET @POLE = (SELECT Libelle FROM FRED_POLE where OrganisationId = @OrganisationId_B)
		   
		   PRINT '     --->' + CAST(@OrganisationId_B AS VARCHAR(20)) + '-' + @POLE

					 -- RECHERCHE GROUPE

						DECLARE @OrganisationId_C INT;
						Declare C Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 3 AND PereId = @OrganisationId_B
						Open C
						Fetch next From C into @OrganisationId_C
						While @@Fetch_Status=0 Begin
							-- HOLDING
							DECLARE @GROUPE VARCHAR (20)
							SET @GROUPE = (SELECT Libelle FROM FRED_GROUPE where OrganisationId = @OrganisationId_C)
		   
							PRINT '             ---> ' + CAST(@OrganisationId_C AS VARCHAR(20)) + '-' + @GROUPE


									

									-- RECHERCHE SOCIETE

									DECLARE @OrganisationId_D INT;
									Declare D Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 4 AND PereId = @OrganisationId_C
									Open D
									Fetch next From D into @OrganisationId_D
									While @@Fetch_Status=0 Begin
										-- HOLDING
										DECLARE @SOCIETE VARCHAR (20)
										SET @SOCIETE = (SELECT Libelle FROM FRED_SOCIETE where OrganisationId = @OrganisationId_D)
		   
										PRINT '                   SOC  ---> ' + CAST(@OrganisationId_D AS VARCHAR(20)) + '-' + @SOCIETE

												-- RECHERCHE PUO
												--SELECT * from FRED_TYPE_ORGANISATION
												--SELECT * FROM FRED_ORGANISATION_GENERIQUE
												DECLARE @OrganisationId_E INT;
												Declare E Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 5 AND PereId =@OrganisationId_D
												Open E
												Fetch next From E into @OrganisationId_E
												While @@Fetch_Status=0 Begin
												
													DECLARE @PUO VARCHAR (20)
													SET @PUO = (SELECT Libelle FROM FRED_ORGANISATION_GENERIQUE where OrganisationId = @OrganisationId_E)
		   
													PRINT '                              PUO   ---> ' + CAST(@OrganisationId_E AS VARCHAR(20)) + '-' + @PUO

															-- RECHERCHE UO
															--SELECT * from FRED_TYPE_ORGANISATION
															--SELECT * FROM FRED_ORGANISATION_GENERIQUE
															DECLARE @OrganisationId_F INT;
															Declare F Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 6 AND PereId =@OrganisationId_E
															Open F
															Fetch next From F into @OrganisationId_F
															While @@Fetch_Status=0 Begin
												
																DECLARE @UO VARCHAR (20)
																SET @UO = (SELECT Libelle FROM FRED_ORGANISATION_GENERIQUE where OrganisationId = @OrganisationId_F)
		   
																PRINT '                                        UO     ---> ' + CAST(@OrganisationId_F AS VARCHAR(20)) + '-' + @UO

																		-- RECHERCHE ETABLISSEMENT
																		DECLARE @OrganisationId_G INT;
																		Declare G Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 7 AND PereId = @OrganisationId_F
																		Open G
																		Fetch next From G into @OrganisationId_G
																		While @@Fetch_Status=0 Begin
																		
																			DECLARE @ETABLISSEMENT VARCHAR (20)
																			SET @ETABLISSEMENT = (SELECT Libelle FROM FRED_ETABLISSEMENT_COMPTABLE where OrganisationId = @OrganisationId_G)
		   
																			PRINT '                                          ETB   ---> ' + CAST(@OrganisationId_F AS VARCHAR(20)) + '-' + @ETABLISSEMENT

																						-- RECHERCHE CI
																						DECLARE @OrganisationId_H INT;
																						Declare H Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 8 AND PereId = @OrganisationId_G
																						Open H
																						Fetch next From H into @OrganisationId_H
																						While @@Fetch_Status=0 Begin
																		
																							DECLARE @CI VARCHAR (50)
																							SET @CI  = (SELECT Libelle FROM FRED_CI where OrganisationId = @OrganisationId_H)
		   
																							PRINT '                                         CI    ---> ' + CAST(@OrganisationId_H AS VARCHAR(20)) + '-' + @CI

																							Fetch next From H into @OrganisationId_H
																						End
																						Close H
																						Deallocate H
																						-- RECHERCHE ETABLISSEMENT


																			Fetch next From G into @OrganisationId_G
																		End
																		Close G
																		Deallocate G
																		-- RECHERCHE ETABLISSEMENT




																Fetch next From F into @OrganisationId_F
															End
															Close F
															Deallocate F

															-- RECHERCHE PUO



													Fetch next From E into @OrganisationId_E
												End
												Close E
												Deallocate E

												-- RECHERCHE PUO


												-- RECHERCHE ETB DIRECEMENT SUR SOC
												-- RECHERCHE ETABLISSEMENT
												DECLARE @OrganisationId_G2 INT;
												Declare G2 Cursor For SELECT OrganisationId FROM FRED_ORGANISATION WHERE TypeOrganisationId = 7 AND PereId = @OrganisationId_D
												Open G2
												Fetch next From G2 into @OrganisationId_G2
												While @@Fetch_Status=0 Begin
													
													DECLARE @ETABLISSEMENT2 VARCHAR (20)
													SET @ETABLISSEMENT2 = (SELECT Libelle FROM FRED_ETABLISSEMENT_COMPTABLE where OrganisationId = @OrganisationId_G2)
		   
													PRINT '                                            ETB ---> ' + CAST(@OrganisationId_G2 AS VARCHAR(20)) + '-' + @ETABLISSEMENT2

																


													Fetch next From G2 into @OrganisationId_G2
												End
												Close G2
												Deallocate G2
												-- RECHERCHE ETABLISSEMENT

										Fetch next From D into @OrganisationId_D
									End
									Close D
									Deallocate D

								-- RECHERCHE SOCIETE




							Fetch next From C into @OrganisationId_C
						End
						Close C
						Deallocate C

					-- RECHERCHE GROUPE


		   Fetch next From B into @OrganisationId_B
		End
		Close B
		Deallocate B

   -- RCEHERCHE POLE

   Fetch next From A into @OrganisationId_A
End
Close A
Deallocate A



