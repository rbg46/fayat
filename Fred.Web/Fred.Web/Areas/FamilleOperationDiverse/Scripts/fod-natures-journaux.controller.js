(function (angular) {
	'use strict';

	angular.module('Fred').controller('FodNaturesJournauxController', FodNaturesJournauxController);

	FodNaturesJournauxController.$inject = ['$scope', 'Notify', '$uibModal', 'FamilleOperationDiverseService', 'ProgressBar', 'confirmDialog', 'TypeSocieteService', 'JournalComptableService', 'fredDialog', 'UserService', 'NatureService'];

	function FodNaturesJournauxController($scope, Notify, $uibModal, FamilleOperationDiverseService, ProgressBar, confirmDialog, TypeSocieteService, JournalComptableService, fredDialog, UserService, NatureService) {

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
			handleCheckbox: handleCheckbox,
			handleCancel: handleCancel,
			handleCheckboxJournal: handleCheckboxJournal,
			handleCheckboxNature: handleCheckboxNature,
			handleToggleSelectAllJournaux: handleToggleSelectAllJournaux,
			handleToggleSelectAllNatures: handleToggleSelectAllNatures,
			getListParametrageFodNaturesJournaux: getListParametrageFodNaturesJournaux,
			arrayObjectFindIndexOf: arrayObjectFindIndexOf,
			showDuplicateParametrage: showDuplicateParametrage,
			initializeEditFamille: initializeEditFamille
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
				initialFamilleOperationDiverseList: [],
				allParametragesFodNaturesJournaux: [],
				naturesAssociedItem: [],
				journauxAssociedItem: [],
				btnEnregistrerDisabled: true,
				duplicateParams: []
			});

			// Récupération et formattage des type société Interne pour paramétrer l'url de la lookup société            
			$ctrl.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE]);

			// Filtre de recherche pour les familles et journaux/natures

			$scope.searchFamilleOD = function (row) {
				return filteredSearch(row, $scope.queryFamilleOD);
			};

			$scope.searchNatureAssociatedItem = function (row) {
				return filteredSearch(row, $scope.queryNatureAssociatedItem);
			};

			$scope.searchJournalAssociatedItem = function (row) {
				return filteredSearch(row, $scope.queryJournalAssociatedItem);
			};

			ProgressBar.complete();
		}

		/* -------------------------------------------------------------------------------------------------------------
		 *                                            HANDLERS
		 * -------------------------------------------------------------------------------------------------------------
		 */

		function handleCheckbox(familleOperationDiverse) {
			ProgressBar.start();
			$ctrl.btnEnregistrerDisabled = true;

			$ctrl.familleSelectedCheckbox = familleOperationDiverse.selectedCheckbox;
			$ctrl.journauxAssociedItem = $ctrl.sauvegardeJournauxAssociedItem;
			$ctrl.naturesAssociedItem = $ctrl.sauvegardeNaturesAssociedItem;

			resetCheckbox();
			initializeEditFamille(familleOperationDiverse);
			getListParametrageFodNaturesJournaux();
			familleOperationDiverse.selectedCheckbox = $ctrl.familleSelectedCheckbox;

			if (familleOperationDiverse.selectedCheckbox === true) {
				//Cette ligne permet surligner l'élement 
				$ctrl.selectedFamilly = familleOperationDiverse.FamilleOperationDiverseId;

				$ctrl.editFamille = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === familleOperationDiverse.FamilleOperationDiverseId);
				$ctrl.editFamille.AssociatedJournaux = [];
				$ctrl.editFamille.AssociatedNatures = [];
			}
			else {
				handleLoadData();
				// Cette ligne permet dé-surligner l'élement (famille)
				$ctrl.selectedFamilly = null;
				$ctrl.editFamille = null;
			}

			if (familleOperationDiverse.MustHaveOrder === true) {
				GetItemWithOrder(familleOperationDiverse.FamilleOperationDiverseId);
			}
			else {
				GetItemWithoutOrder(familleOperationDiverse.FamilleOperationDiverseId);
			}

			ProgressBar.complete();
		}

		function handleCheckboxJournal(journal) {
			ProgressBar.start();

			//récupération de la famille selectionner 
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {
				//si la case est coché sur la famille alors le journal va dans la famille avec commande
				if (selectedFamilly.MustHaveOrder) {
					journal.ParentFamilyODWithOrder = selectedFamilly.FamilleOperationDiverseId;
					$ctrl.naturesAssociedItem = $ctrl.sauvegardeNaturesAssociedItem.filter(nature => $ctrl.allParametragesFodNaturesJournaux.filter(param => param.NatureCode === nature.Code && param.JournalCode === journal.Code && nature.ParentFamilyODWithOrder !== selectedFamilly.FamilleOperationDiverseId && journal.Checked).length === 0);
				}
				else {
					journal.ParentFamilyODWithoutOrder = selectedFamilly.FamilleOperationDiverseId;
					$ctrl.naturesAssociedItem = $ctrl.sauvegardeNaturesAssociedItem.filter(nature => $ctrl.allParametragesFodNaturesJournaux.filter(param => param.NatureCode === nature.Code && param.JournalCode === journal.Code && nature.ParentFamilyODWithoutOrder !== selectedFamilly.FamilleOperationDiverseId).length === 0);
				}

				if (!journal.Checked && selectedFamilly.MustHaveOrder) {
					journal.ParentFamilyODWithOrder = 0;
				}
				else if (!journal.Checked) {
					journal.ParentFamilyODWithoutOrder = 0;
				}

				journal.Selected = !journal.Selected;

				const index = arrayObjectFindIndexOf($ctrl.editFamille.AssociatedJournaux, journal.Code, "Code");
				if (index >= 0) {
					$ctrl.editFamille.AssociatedJournaux[index] = journal;
				}
				else {
					$ctrl.editFamille.AssociatedJournaux.push(journal);
				}

				if (selectedFamilly.MustHaveOrder) {
					GetItemWithOrder(selectedFamilly.FamilleOperationDiverseId);
				}
				else {
					GetItemWithoutOrder(selectedFamilly.FamilleOperationDiverseId);
				}

				$ctrl.btnEnregistrerDisabled = !($ctrl.editFamille !== null && $ctrl.editFamille !== undefined && $ctrl.editFamille.AssociatedJournaux.length > 0 && $ctrl.editFamille.AssociatedNatures.length > 0);
			}
			else {
				journal.Checked = false;
				Notify.error($ctrl.resources.FamilleOperationDiverse_Index_SelectionnerFamille);
			}

			ProgressBar.complete();
		}

		function handleCheckboxNature(nature) {
			ProgressBar.start();

			//récupération de la famille selectionner 
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {
				//si la case est coché sur la famille alors le journal va dans la famille avec commande
				if (selectedFamilly.MustHaveOrder) {
					nature.ParentFamilyODWithOrder = selectedFamilly.FamilleOperationDiverseId;
					$ctrl.journauxAssociedItem = $ctrl.sauvegardeJournauxAssociedItem.filter(journal => $ctrl.allParametragesFodNaturesJournaux.filter(param => param.JournalCode === journal.Code && param.NatureCode == nature.Code && journal.ParentFamilyODWithOrder !== selectedFamilly.FamilleOperationDiverseId && nature.Checked).length === 0);
				}
				else {
					nature.ParentFamilyODWithoutOrder = selectedFamilly.FamilleOperationDiverseId;
					$ctrl.journauxAssociedItem = $ctrl.sauvegardeJournauxAssociedItem.filter(journal => $ctrl.allParametragesFodNaturesJournaux.filter(param => param.JournalCode === journal.Code && param.NatureCode == nature.Code && journal.ParentFamilyODWithoutOrder !== selectedFamilly.FamilleOperationDiverseId && nature.Checked).length === 0);
				}

				if (!nature.Checked && selectedFamilly.MustHaveOrder) {
					nature.ParentFamilyODWithOrder = 0;
				}
				else if (!nature.Checked) {
					nature.ParentFamilyODWithoutOrder = 0;
				}

				nature.Selected = !nature.Selected;

				const index = arrayObjectFindIndexOf($ctrl.editFamille.AssociatedNatures, nature.Code, "Code");
				if (index >= 0) {
					$ctrl.editFamille.AssociatedNatures[index] = nature;
				}
				else {
					$ctrl.editFamille.AssociatedNatures.push(nature);
				}

				if (selectedFamilly.MustHaveOrder) {
					GetItemWithOrder(selectedFamilly.FamilleOperationDiverseId);
				}
				else {
					GetItemWithoutOrder(selectedFamilly.FamilleOperationDiverseId);
				}

				$ctrl.btnEnregistrerDisabled = !($ctrl.editFamille !== null && $ctrl.editFamille !== undefined && $ctrl.editFamille.AssociatedJournaux.length > 0 && $ctrl.editFamille.AssociatedNatures.length > 0);
			}
			else {
				nature.Checked = false;
				Notify.error($ctrl.resources.FamilleOperationDiverse_Index_SelectionnerFamille);
			}

			ProgressBar.complete();
		}

		function arrayObjectFindIndexOf(myArray, searchTerm, property) {
			for (var i = 0, len = myArray.length; i < len; i++) {
				if (myArray[i][property] === searchTerm) return i;
			}
			return -1;
		}

		function resetCheckbox() {
			angular.forEach($ctrl.initialFamilleOperationDiverseList, function (item) {
				item.selectedCheckbox = false;
			});
		}

		/*
		 * @function handleLoadData()
		 * @description Chargement des données en fonction de la Société sélectionnée
		 */
		function handleLoadData() {
			ProgressBar.start();
			getFamilleOperationDiverse();
			getJournaux();
			getNatures();

			ProgressBar.complete();
		}

		/*
		  * @function handleSave()
		  * @description Gestion de la sauvegarde des modifications des familles OD
		  */
		function handleSave() {
			ProgressBar.start();
			FamilleOperationDiverseService.SetParametrageNaturesJournaux($ctrl.editFamille)
				.then(function (response) {
					if (response && response.data && response.data.Result && response.data.Result.length > 0) {
						showDuplicateParametrage(response.data.Result);
					}
					else {
						$ctrl.editFamille = null;
						Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
					}
				})
				.finally(() => ProgressBar.complete());
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

		function getFamilleOperationDiverse() {
			$ctrl.editFamille = null;
			$ctrl.selectedFamilly = null;
			FamilleOperationDiverseService.SearchFamilleOperationDiverseBySociete($ctrl.selectedSociete.SocieteId)
				.then(function (response) {
					$ctrl.initialFamilleOperationDiverseList = response.data;
					if (response && response.data && response.length === 0) {
						Notify.error(resources.Global_Notification_AucuneDonnees);
					}
				});
		}

		function getListParametrageFodNaturesJournaux() {
			FamilleOperationDiverseService.GetAllParametrages($ctrl.selectedSociete.SocieteId)
				.then(function (response) {
					$ctrl.allParametragesFodNaturesJournaux = Array.from(response.data);
				});
		}


		function getJournaux() {
			JournalComptableService.GetJournauxActifs($ctrl.selectedSociete.SocieteId)
				.then(function (response) {
					$ctrl.journauxAssociedItem = response.data;
					$ctrl.sauvegardeJournauxAssociedItem = response.data;
				});
		}

		function getNatures() {
			NatureService.GetNatureFamilleOdBySocieteId($ctrl.selectedSociete.SocieteId)
				.then(function (response) {
					$ctrl.naturesAssociedItem = response.data;
					$ctrl.sauvegardeNaturesAssociedItem = response.data;
				});
		}

		function initializeEditFamille(famille) {
			$ctrl.editFamille = famille;
			$ctrl.editFamille.AssociatedNatures = [];
			$ctrl.editFamille.AssociatedJournaux = [];
		}

		// Handle toggle select all journaux
		function handleToggleSelectAllJournaux(check) {
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {

				if (!$ctrl.filteredJournaux || $ctrl.filteredJournaux.length === 0) {
					$ctrl.SelectAllJournaux = false;
					return;
				}

				angular.forEach($ctrl.filteredJournaux, function (el) {
					el.Selected = check;
					el.Checked = check;
					if (check) {
						changeCheckboxForAllJournaux(el);
					}
					else {
						changeCheckboxForAllJournaux(el);
						$ctrl.editFamille.AssociatedJournaux = [];
					}
				});
			}
			else {
				$ctrl.SelectAllJournaux = false;
				Notify.error($ctrl.resources.FamilleOperationDiverse_Index_SelectionnerFamille);
			}
		}

		// Handle toggle select all natures
		function handleToggleSelectAllNatures(check) {
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {
				if (!$ctrl.filteredNatures || $ctrl.filteredNatures.length === 0) {
					$ctrl.SelectAllNatures = false;
					return;
				}

				angular.forEach($ctrl.filteredNatures, function (el) {
					el.Selected = check;
					el.Checked = check;
					if (check) {
						changeCheckboxForAllNatures(el);
					}
					else {
						changeCheckboxForAllNatures(el);
						$ctrl.editFamille.AssociatedNatures = [];
					}
				});
			}
			else {
				$ctrl.SelectAllNatures = false;
				Notify.error($ctrl.resources.FamilleOperationDiverse_Index_SelectionnerFamille);
			}
		}

		function GetItemWithoutOrder(familleOperationDiverseId) {
			angular.forEach($ctrl.naturesAssociedItem, function (item) {
				item.Selected = false;
				item.Checked = false;
			});
			angular.forEach($ctrl.naturesAssociedItem.filter(x => x.ParentFamilyODWithoutOrder === familleOperationDiverseId), function (item) {
				item.Selected = true;
				item.Checked = true;
				const index = arrayObjectFindIndexOf($ctrl.editFamille.AssociatedNatures, item.Code, "Code");
				if (index === -1) {
					$ctrl.editFamille.AssociatedNatures.push(item);
				}
			});

			angular.forEach($ctrl.journauxAssociedItem, function (item) {
				item.Selected = false;
				item.Checked = false;
			});
			angular.forEach($ctrl.journauxAssociedItem.filter(x => x.ParentFamilyODWithoutOrder === familleOperationDiverseId), function (item) {
				item.Selected = true;
				item.Checked = true;
				const index = arrayObjectFindIndexOf($ctrl.editFamille.AssociatedJournaux, item.Code, "Code");
				if (index === -1) {
					$ctrl.editFamille.AssociatedJournaux.push(item);
				}
			});
		}

		function GetItemWithOrder(familleOperationDiverseId) {
			angular.forEach($ctrl.naturesAssociedItem, function (item) {
				item.Selected = false;
				item.Checked = false;
			});

			angular.forEach($ctrl.naturesAssociedItem.filter(x => x.ParentFamilyODWithOrder === familleOperationDiverseId), function (item) {
				item.Checked = true;
				item.Selected = true;
				const index = arrayObjectFindIndexOf($ctrl.editFamille.AssociatedNatures, item.Code, "Code");
				if (index === -1) {
					$ctrl.editFamille.AssociatedNatures.push(item);
				}
			});

			angular.forEach($ctrl.journauxAssociedItem, function (item) {
				item.Selected = false;
				item.Checked = false;
			});

			angular.forEach($ctrl.journauxAssociedItem.filter(x => x.ParentFamilyODWithOrder === familleOperationDiverseId), function (item) {
				item.Checked = true;
				item.Selected = true;
				const index = arrayObjectFindIndexOf($ctrl.editFamille.AssociatedJournaux, item.Code, "Code");
				if (index === -1) {
					$ctrl.editFamille.AssociatedJournaux.push(item);
				}
			});
		}

		function changeCheckboxForAllJournaux(journal) {
			ProgressBar.start();
			//récupération de la famille selectionner 
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {
				//si la case est coché sur la famille alors le journal va dans la famille avec commande
				if (selectedFamilly.MustHaveOrder === true) {
					journal.ParentFamilyODWithOrder = selectedFamilly.FamilleOperationDiverseId;
				}
				else {
					journal.ParentFamilyODWithoutOrder = selectedFamilly.FamilleOperationDiverseId;
				}

				if (!journal.Checked && selectedFamilly.MustHaveOrder === true) {
					journal.ParentFamilyODWithOrder = 0;
				}
				else if (!journal.Checked) {
					journal.ParentFamilyODWithoutOrder = 0;
				}
				if ($ctrl.editFamille.AssociatedJournaux === null || $ctrl.editFamille.AssociatedJournaux === undefined) {
					$ctrl.editFamille.AssociatedJournaux = [];
				}
				$ctrl.editFamille.AssociatedJournaux.push(journal);
			}

			ProgressBar.complete();
		}

		function changeCheckboxForAllNatures(nature) {
			ProgressBar.start();
			//récupération de la famille selectionner 
			var selectedFamilly = $ctrl.initialFamilleOperationDiverseList.find(x => x.FamilleOperationDiverseId === $ctrl.selectedFamilly);

			if (selectedFamilly) {
				//si la case est coché sur la famille alors le journal va dans la famille avec commande
				if (selectedFamilly.MustHaveOrder === true) {
					nature.ParentFamilyODWithOrder = selectedFamilly.FamilleOperationDiverseId;
				}
				else {
					nature.ParentFamilyODWithoutOrder = selectedFamilly.FamilleOperationDiverseId;
				}

				if (!nature.Checked && selectedFamilly.MustHaveOrder === true) {
					nature.ParentFamilyODWithOrder = 0;
				}
				else if (!nature.Checked) {
					nature.ParentFamilyODWithoutOrder = 0;
				}

				if ($ctrl.editFamille.AssociatedNatures === null || $ctrl.editFamille.AssociatedNatures === undefined) {
					$ctrl.editFamille.AssociatedNatures = [];
				}
				$ctrl.editFamille.AssociatedNatures.push(nature);
			}

			ProgressBar.complete();
		}

		function filteredSearch(row, criterion) {
			if ((angular.lowercase(criterion) || '').trim().indexOf('*') >= 0) {
				criterion = criterion.split('*')[0];
				return (angular.lowercase(row.Code).indexOf(angular.lowercase(criterion)) === 0);
			}

			return (angular.lowercase(row.Libelle).indexOf(angular.lowercase(criterion) || '') !== -1 ||
				angular.lowercase(row.Code).indexOf(angular.lowercase(criterion) || '') !== -1);
		}

		function showDuplicateParametrage(data) {
			$uibModal.open({
				animation: true,
				component: 'duplicateParametrageComponent',
				windowClass: 'modal-duplicateparametrage',
				controller: 'DuplicateParametrageController',
				backdrop: 'static',
				size: 'lg',
				resolve: {
					resources: function () { return $ctrl.resources; },
					editFamille: function () { return $ctrl.editFamille; },
					parametrageList: function () { return data; },
					journalCodeList: function () { return Array.from(new Set(data.map(s => s.Journal.Code))); },
					natureCodeList: function () { return Array.from(new Set(data.map(s => s.Nature.Code))); }
				}
			});
		}
	}
})(angular);