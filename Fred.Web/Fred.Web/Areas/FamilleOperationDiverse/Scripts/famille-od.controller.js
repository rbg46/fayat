(function (angular) {
  'use strict';

	angular.module('Fred').controller('FamilleOperationDiverseController', FamilleOperationDiverseController);

	FamilleOperationDiverseController.$inject = ['$scope','$q', 'Notify', 'FamilleOperationDiverseService', 'ProgressBar', 'confirmDialog', 'TypeSocieteService', 'JournalComptableService', 'fredDialog', 'UserService', 'NatureService'];

	function FamilleOperationDiverseController($scope, $q, Notify, FamilleOperationDiverseService, ProgressBar, confirmDialog, TypeSocieteService, JournalComptableService, fredDialog, UserService, NatureService) {

		/* -------------------------------------------------------------------------------------------------------------
		 *                                            INIT
		 * -------------------------------------------------------------------------------------------------------------
		 */

		// assignation de la valeur du scope au controller pour les templates non mis à jour
		var $ctrl = this;

		// méthodes exposées
		angular.extend($ctrl, {
			handleLoadData: handleLoadData,
			handleSave: handleSave,
			handleCancel: handleCancel,
			handleExportExcelErreursParametrage: handleExportExcelErreursParametrage,
			handleLaunchControleParametrage: handleLaunchControleParametrage,      
			handleCheckbox: handleCheckbox,
			handleLineSelected: handleLineSelected,
			handleCheckboxJournal: handleCheckboxJournal,
			handleEdit: handleEdit,
			handleEditFamille: handleEditFamille,
			handleClickCancel: handleClickCancel,
			handlePrepareDataForExport: handlePrepareDataForExport,
			handleGetDataForExport: handleGetDataForExport,
			handleToggleSelectAll: handleToggleSelectAll,
			handleCancelEdit: handleCancelEdit
		});

		init();

		return $ctrl;

		/**
		 * Initialisation du controller.     
		 */
		function init() {
			ProgressBar.start();

			$("div[id^=actionChoix]").removeClass("selected");
			$(".inner-triangle").addClass("ng-hide");

			angular.extend($ctrl, {
				// Instanciation Objet Ressources
				resources: resources,

				selectedSociete: null,
				popVisible: false,
				initialFamilleOperationDiverseList: [],
				initialAssociedItem: [],
				modifiedFamilleOperationDiverseList: [],
				modifiedAssociatedItemList: [],
				resultControleParametrageList: [],
				btnValiderVisible: false,
				selectedTypeData: null,
				displayEdit: false,
				displayJournal: true,
				disableSaveWhenLoad: false
			});

            UserService.getCurrentUser().then(function(user) {
                if (user.Personnel.Societe.Groupe.Code === 'GFTP') {
                    $ctrl.displayJournal = false;
                }
            });

			// Récupération et formattage des type société Interne pour paramétrer l'url de la lookup société            
			$ctrl.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE]);

			// Filtre de recherche pour les familles et journaux/natures

			$scope.searchFamilleOD = function (row) {
				return filteredSearch(row, $scope.queryFamilleOD);
			}; 

			$scope.searchAssociatedItem = function (row) {
				return filteredSearch(row, $scope.queryAssociatedItem);
			}; 

			ProgressBar.complete();
		}

		/* -------------------------------------------------------------------------------------------------------------
		 *                                            HANDLERS
		 * -------------------------------------------------------------------------------------------------------------
		 */

		function handleCheckbox(familleOperationDiverse) {
			ProgressBar.start();

			$ctrl.familleSelectedCheckbox = familleOperationDiverse.selectedCheckbox;

			resetCheckbox();

			familleOperationDiverse.selectedCheckbox = $ctrl.familleSelectedCheckbox;

			if (familleOperationDiverse.selectedCheckbox === true) {
				//Cette ligne permet surligner l'élement 
				$ctrl.selectedFamilly = familleOperationDiverse.FamilleOperationDiverseId;
			}
			else {
				handleLoadData();
				// Cette ligne permet dé-surligner l'élement (famille)
				$ctrl.selectedFamilly = null;
			}

			GetItemWithOrder(familleOperationDiverse.FamilleOperationDiverseId);
			ProgressBar.complete();
		}

		function handleLineSelected(familleOperationDiverse) {
			ProgressBar.start();

			$ctrl.selectedFamilly = familleOperationDiverse.FamilleOperationDiverseId;

			GetItemWithoutOrder(familleOperationDiverse.FamilleOperationDiverseId);
			resetCheckbox();

			ProgressBar.complete();
		}

		function handleCheckboxJournal(journal) {
			ProgressBar.start();

			//récupération de la famille selectionner 
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {
				//si la case est coché sur la famille alors le journal va dans la famille avec commande
				if (selectedFamilly.selectedCheckbox === true) {
					journal.ParentFamilyODWithOrder = selectedFamilly.FamilleOperationDiverseId;
				}
				else {
					journal.ParentFamilyODWithoutOrder = selectedFamilly.FamilleOperationDiverseId;
				}

				if (!journal.Checked && selectedFamilly.selectedCheckbox === true) {
					journal.ParentFamilyODWithOrder = 0;
				}
				else if (!journal.Checked) {
					journal.ParentFamilyODWithoutOrder = 0;
				}

				journal.Selected = !journal.Selected;
				$ctrl.modifiedAssociatedItemList.push(journal);
			}

			ProgressBar.complete();
		}

		function resetCheckbox() {
			angular.forEach($ctrl.initialFamilleOperationDiverseList, function (item) {
				item.selectedCheckbox = false;
			});
		}

		function handleClickCancel() {
			$ctrl.displayEdit = false;
			$ctrl.editFamille.Libelle = $ctrl.sauvegardeLibelleFamille;
			$ctrl.editFamille.LibelleCourt = $ctrl.sauvegardeLibelleFamille;
		}
		/*
		 * @function handleLoadData()
		 * @description Chargement des données en fonction de la Société sélectionnée
		 */
		function handleLoadData() {
			ProgressBar.start();
			getFamilleOperationDiverse();
			if ($ctrl.displayJournal) {
				getJournaux();
			}
			else {
				getNatures();
			}
			
			ProgressBar.complete();
		}

		function handleEditFamille() {
			actionSaveLoadStart();
			FamilleOperationDiverseService.UpdateFamilleOperationDiverse($ctrl.editFamille)
				.success(function ()
				{
					$ctrl.sauvegardeLibelleFamille = $ctrl.editFamille.Libelle;
					$ctrl.sauvegardeLibelleCourtFamille = $ctrl.editFamille.LibelleCourt;
					$ctrl.sauvegardeOrderFamille = $ctrl.editFamille.Order;
					$ctrl.sauvegardeMustHaveOrderFamille = $ctrl.editFamille.MustHaveOrder;
					Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
					getFamilleOperationDiverse();
					$ctrl.displayEdit = false;
					$ctrl.selectedFamilly = null;
				})
				.error(function (error)
				{
					Notify.error(error.Message);
				})
				.finally(function ()
				{
					actionSaveLoadEnd();
				});
		}
		/*
		  * @function handleSave()
		  * @description Gestion de la sauvegarde des modifications des familles OD
		  */
		function handleSave() {
			if ($ctrl.modifiedAssociatedItemList.length > 0) {
				ProgressBar.start();
				if ($ctrl.displayJournal) {
					JournalComptableService.UpdateFamilleOperationDiverseJournal($ctrl.modifiedAssociatedItemList)
						.success(function () {
							$ctrl.modifiedAssociatedItemList = [];
							Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
						})
						.error(function (error) { Notify.error(error); })
						.finally(() => ProgressBar.complete());
				}
				else
				{
					NatureService.UpdateNatureFamilleOd($ctrl.modifiedAssociatedItemList)
						.success(function () {
							$ctrl.modifiedAssociatedItemList = [];
							Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
						})
						.error(function (error) { Notify.error(error); })
						.finally(() => ProgressBar.complete());
				}
			}
		}

		/*
		 * @function handleCancel()
		 * @description Gestion de l'annulation des modifications du référentiel étendu
		 */
		function handleCancel() {
			confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationAnnulation).then(function () {
				handleLoadData();
				// Cette ligne permet dé-surligner l'élement (famille)
				$ctrl.selectedFamilly = null;
			});
		}

		function handleCancelEdit() {
			$ctrl.displayEdit = true;
			$ctrl.editFamille.Libelle = $ctrl.sauvegardeLibelleFamille;
			$ctrl.editFamille.LibelleCourt = $ctrl.sauvegardeLibelleCourtFamille;
			$ctrl.editFamille.Order = $ctrl.sauvegardeOrderFamille;
			$ctrl.editFamille.MustHaveOrder = $ctrl.sauvegardeMustHaveOrderFamille;
		}

		function handleEdit(famille) {
			$ctrl.displayEdit = true;
			$ctrl.editFamille = famille;
			$ctrl.selectedFamilly = famille.FamilleOperationDiverseId;
			$ctrl.sauvegardeLibelleFamille = famille.Libelle;
			$ctrl.sauvegardeLibelleCourtFamille = famille.LibelleCourt;
			$ctrl.sauvegardeOrderFamille = famille.Order;
			$ctrl.sauvegardeMustHaveOrderFamille = famille.MustHaveOrder;
		}

		/*
		 * @function handleExport()
		 * @description Gestion de l'export des erreurs du contrôle du paramétrage
		 */
		function handleExportExcelErreursParametrage() {
			ProgressBar.start();
			
			FamilleOperationDiverseService.GenerateExportExcelErreursParametrage($ctrl.erreurParametrageList)
				.then(response => {
					FamilleOperationDiverseService.DownloadExportExcelErreursParametrage(response.data.id, $ctrl.selectedSociete.SocieteId);
				})
				.catch(() => {
					Notify.error($ctrl.resources.FamilleOperationDiverse_Export_Erreur);
				})
				.finally(() => ProgressBar.complete());
		}

		/*
		 * @function handleLaunchControleParametrage()
		 * @description Gestion du lancement du contrôle du paramétrage
		 */
		function handlePrepareDataForExport(el, typeDonnee) {
			if ($ctrl.selectedSociete) {
				changeSelectedButton(el, typeDonnee);

				$ctrl.selectedTypeData = typeDonnee;
		}
			else {
				Notify.error($ctrl.resources.FamilleOperationDiverse_Export_SocieteVide);
			}
		}


		/*
		 * @function handleLaunchControleParametrage()
		 * @description Gestion du lancement de la popin du contrôle du paramétrage
		 */
		function handleLaunchControleParametrage() {
			if ($ctrl.selectedSociete) {
				fredDialog.confirmation(
					$ctrl.resources.FamilleOperationDiverse_ControleParametrage_Question_Message,
					$ctrl.resources.FamilleOperationDiverse_ControleParametrage_Question_Titre, "flaticon flaticon - shuffle - 1", '', '', ValidateAction);
			}
			else {
				Notify.error($ctrl.resources.FamilleOperationDiverse_ControleParametrage_SocieteVide);
			}
		}

		function getFamilleOperationDiverse() {
			FamilleOperationDiverseService.SearchFamilleOperationDiverseBySociete($ctrl.selectedSociete.SocieteId)
				.then(function (response) {
					$ctrl.initialFamilleOperationDiverseList = response.data;
					if (response && response.data && response.length === 0) {
						Notify.error(resources.Global_Notification_AucuneDonnees);
					}
				});
		}

		function getJournaux() {
			JournalComptableService.GetJournauxActifs($ctrl.selectedSociete.SocieteId)
				.then(function (response) {
					$ctrl.initialAssociedItem = response.data;
				});
		}

		function getNatures() {
			NatureService.GetNatureFamilleOdBySocieteId($ctrl.selectedSociete.SocieteId)
				.then(function (response) {
					$ctrl.initialAssociedItem = response.data;
				});
		}

		function getLaunchControleParametrageSuccess(response) {
			$ctrl.resultControleParametrageList = response.data;

			if ($ctrl.resultControleParametrageList.length > 0) {
			var titre = 'Erreurs de paramétrage';
				var message = '<table class="mod-error-table"><thead>';
				message += '<tr ><th class="col1">Type</th><th class="col2">Code</th><th class="col3">Libellé</th><th class="col4">Erreur</th></tr></thead > <tbody class="fred-scroll">';

				var styleBorder = '';

			angular.forEach($ctrl.resultControleParametrageList, function (controle) {
				message += '<tr><td ' + styleBorder + ' class="col1">' + controle.TypeFamilleOperationDiverse + '</td>';
				message += '<td ' + styleBorder + ' class="col2">' + controle.Code + '</td>';
				message += '<td ' + styleBorder + ' class="col3">' + controle.Libelle + '</td>';
				message += '<td ' + styleBorder + ' class="col4">' + controle.Erreur + '</td></tr>';
			});

				message += '</tbody></table>';

				fredDialog.generic(message, titre, "flaticon flaticon-shuffle-1", '.', '', ExportParametersAction, '', '', '', 'modal-err-parametrage');
			}
			else {
				fredDialog.message(
					$ctrl.resources.FamilleOperationDiverse_ControleParametrage_Question_Message_AucuneDonnees,
					$ctrl.resources.FamilleOperationDiverse_ControleParametrage_Question_Titre);
			}
		}

		function getLaunchControleParametrageError(error) {
			Notify.error($ctrl.resources.Global_Notification_Error);
		}

		/*
		 * @function handleGetDataForExport()
		 * @description Gestion de la récupération des données pour l'export des familles OD/journaux
		 */
		function handleGetDataForExport() {
			ProgressBar.start();

			if ($ctrl.displayJournal) {
				FamilleOperationDiverseService.GenerateExportExcelForJournal($ctrl.selectedSociete.SocieteId, $ctrl.selectedTypeData)
					.then(function (response) {
						FamilleOperationDiverseService.DownloadExportExcel(response.data.id, $ctrl.selectedSociete.SocieteId, $ctrl.selectedTypeData);
					})
					.catch(() => {
						Notify.error($ctrl.resources.FamilleOperationDiverse_Export_Erreur);
					})
					.finally(() => ProgressBar.complete());
			}
			else {
				FamilleOperationDiverseService.GenerateExportExcelForNature($ctrl.selectedSociete.SocieteId, $ctrl.selectedTypeData)
					.then(function (response) {
						FamilleOperationDiverseService.DownloadExportExcel(response.data.id, $ctrl.selectedSociete.SocieteId, $ctrl.selectedTypeData);
					})
					.catch(() => {
						Notify.error($ctrl.resources.FamilleOperationDiverse_Export_Erreur);
					})
					.finally(() => ProgressBar.complete());
			}
		}

		// Handle toggle select all
		function handleToggleSelectAll(check) {
			if (!$ctrl.filteredItems || $ctrl.filteredItems.length === 0) {
				$ctrl.SelectedAll = false;
				return;
			}

			angular.forEach($ctrl.filteredItems, function (el) {
				el.Selected = check;
				el.Checked = check;
				if (check) {
					changeCheckboxForAllJournaux(el);
				}
				else {
					$ctrl.modifiedAssociatedItemList = [];
				}
			});
		}

		/* -------------------------------------------------------------------------------------------------------------
		 *                                            ACTIONS
		 * -------------------------------------------------------------------------------------------------------------
		 */

		function GetItemWithoutOrder(familleOperationDiverseId) {

			angular.forEach($ctrl.initialAssociedItem, function (item) {
				item.Selected = false;
				item.Checked = false;
			});

			angular.forEach($ctrl.initialAssociedItem.filter(x => x.ParentFamilyODWithoutOrder === familleOperationDiverseId), function (item) {
				item.Selected = true;
				item.Checked = true;
			});
		}

	/*
		 * @function ValidateAction 
		 * @description Lance le service de contrôle du paramétrage après validation de la popin
		 */
		function ValidateAction() {
			if ($ctrl.displayJournal) {
				FamilleOperationDiverseService.LaunchControleParametrageForJournal($ctrl.selectedSociete.SocieteId)
					.then(getLaunchControleParametrageSuccess)
					.catch(getLaunchControleParametrageError);
			}
			else {
				FamilleOperationDiverseService.LaunchControleParametrageForNature($ctrl.selectedSociete.SocieteId)
					.then(getLaunchControleParametrageSuccess)
					.catch(getLaunchControleParametrageError);
			}
		}

		/*
		 * @function getLaunchControleParametrageSuccess
		 * @description Lance le service d'export des résultats du contrôle du paramétrage
		 */
		function ExportParametersAction() {  
			FamilleOperationDiverseService.GenerateExportExcelErreursParametrage($ctrl.resultControleParametrageList)
				.then(function (data) {
					FamilleOperationDiverseService.DownloadExportExcelErreursParametrage(data.data.id, $ctrl.selectedSociete.SocieteId);
				});
		}

		function GetItemWithOrder(familleOperationDiverseId) {

			angular.forEach($ctrl.initialAssociedItem, function (item) {
				item.Selected = false;
				item.Checked = false;
			});

			angular.forEach($ctrl.initialAssociedItem.filter(x => x.ParentFamilyODWithOrder === familleOperationDiverseId), function (item) {
				item.Checked = true;
				item.Selected = true;
			});
		}

		/*
		 * @function changeSelectedButton
		 * @description Change l'apparence des boutons dans la popup d'export du paramétrage
		 */
		function changeSelectedButton(el, actionType) {
			var currentEl = el.currentTarget;

			$("div[id^=actionChoix]").removeClass("selected");
			$(".inner-triangle").addClass("ng-hide");
			
			$(currentEl).closest("div").find(".inner-triangle").removeClass("ng-hide");

			$ctrl.action = actionType;
			// Affichage bouton validation
			$ctrl.btnValiderVisible = true;
		}

		function changeCheckboxForAllJournaux(journal) {
			ProgressBar.start();

			//récupération de la famille selectionner 
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {
				//si la case est coché sur la famille alors le journal va dans la famille avec commande
				if (selectedFamilly.selectedCheckbox === true) {
					journal.ParentFamilyODWithOrder = selectedFamilly.FamilleOperationDiverseId;
				}
				else {
					journal.ParentFamilyODWithoutOrder = selectedFamilly.FamilleOperationDiverseId;
				}

				if (!journal.Checked && selectedFamilly.selectedCheckbox === true) {
					journal.ParentFamilyODWithOrder = 0;
				}
				else if (!journal.Checked) {
					journal.ParentFamilyODWithoutOrder = 0;
				}

				$ctrl.modifiedAssociatedItemList.push(journal);
			}
			
			ProgressBar.complete();
		}

		/*
		 *@description Gestion du progress bar lors du fin d'enregistrement et activation des actions 
		 */
		function actionSaveLoadEnd() {
			$q.when()
				.then(ProgressBar.complete)
				.then(disableSaveWhenLoad(false));
		}

		/*
		 *@description Gestion du progress bar lors du début d'enregistrement et désactivation des actions 
		 */
		function actionSaveLoadStart() {
			$q.when()
				.then(ProgressBar.start)
				.then(disableSaveWhenLoad(true));
		}

		/*
		 *@description Activation ou désactivation des actions
		 */
		function disableSaveWhenLoad(disable) {
			$ctrl.disableSaveWhenLoad = disable;
		}

		function filteredSearch(row, criterion) {
			return (angular.lowercase(row.Libelle).indexOf(angular.lowercase(criterion) || '') !== -1 ||
				angular.lowercase(row.Code).indexOf(angular.lowercase(criterion) || '') !== -1);
		}
	}
})(angular);