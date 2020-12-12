(function (angular) {
    'use strict';

    angular.module('Fred').controller('ClassificationSocietesController', ClassificationSocietesController);

    ClassificationSocietesController.$inject = ['$q', 'Notify', 'ProgressBar', 'ClassificationSocietesService', 'ModelStateErrorManager'];

    function ClassificationSocietesController($q, Notify, ProgressBar, ClassificationSocietesService, ModelStateErrorManager) {
        var $ctrl = this;
        $ctrl.IsOpen = false;
        $ctrl.IsGroupSelected = false;
        $ctrl.ModeCreation = false;
        $ctrl.class = {};
        angular.extend($ctrl, {
            handleSelect: handleSelect
        });

        // Instanciation Objet Ressources    
        $ctrl.resources = resources;
        function init() {
            angular.extend($ctrl, {
                classSocieteList: [],
                DeleteList: [],
                formClassificationSociete: {}
            });
        }

        init();

        function initFormParam() {
            $ctrl.class = {};
            $ctrl.class.Statut = true;
            $ctrl.class.GroupeId = $ctrl.GrpSelected.GroupeId;
        }

        /**
         * Selected Group changed
         */
        $ctrl.GrpSelectedChanged = function () {
            ProgressBar.start();
            ClassificationSocietesService.GetClasssificationSocietyByIdGrp($ctrl.GrpSelected.GroupeId)
                .then(r => {
                    $ctrl.IsGroupSelected = true;
                    $ctrl.classSocieteList = r.data;
                })
                .catch(e => Notify.error(e))
                .finally(ProgressBar.complete());
        };

        /*
            Gestion de la sélection d'une classification
        */
        function handleSelect(item) {
            $ctrl.class = angular.copy(item);
            $ctrl.ModeCreation = false;
            $ctrl.IsOpen = true;
        }

        /*
            Handler de click sur le bouton Cancel
        */
        $ctrl.handleClickCancel = function () {
            $ctrl.IsOpen = false;
        };

        /*
            Handler de click sur le bouton Add
        */
        $ctrl.handleClickOpen = function () {
            initFormParam();
            $ctrl.ModeCreation = true;
            $ctrl.IsOpen = true;
        };

        /*
         * Appel au service pour creer ou mettre à jour la liste 
         */
        function Create() {
            let updateList = angular.copy($ctrl.classSocieteList);
            if ($ctrl.ModeCreation) {
                updateList.push($ctrl.class);
            }
            else {
                let updateIndex = updateList.findIndex(x => x.SocieteClassificationId === $ctrl.class.SocieteClassificationId);
                updateList[updateIndex] = $ctrl.class;
            }
            return ClassificationSocietesService.CreateOrUpdateListOfClass(updateList)
                .then(r => {
                    if (r.status === 203) {
                        for (var key in r.data.ModelState) {
                            for (var i = 0; i < r.data.ModelState[key].length; i++) {
                                Notify.error(r.data.ModelState[key][i]);
                            }
                        }
                        if (!$ctrl.ModeCreation) {
                            $ctrl.class.Statut = true;
                        }
                    }
                    else {
                        $ctrl.classSocieteList = r.data;
                        Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                    }                  
                })
                .catch(e => {
                    var validationError = ModelStateErrorManager.getErrors(e);
                    if (validationError) {
                        Notify.error(validationError.replace(/\n/g, "<br />"));
                        if (!$ctrl.ModeCreation) {
                            $ctrl.class.Statut = true;
                        }
                    }                
                });
        }

        function Delete() {
            let deleteList = [];
            deleteList.push($ctrl.class);
            return ClassificationSocietesService.DeleteListOfClass(deleteList)
                .then(r => {
                    if (r.status === 203) {
                        for (var key in r.data.ModelState) {
                            for (var i = 0; i < r.data.ModelState[key].length; i++) {
                                Notify.error(r.data.ModelState[key][i]);
                            }
                        }
                    }
                    else {
                        $ctrl.classSocieteList.splice($ctrl.classSocieteList.indexOf($ctrl.class), 1);
                        $ctrl.IsOpen = false;
                    }
                })
                .catch(e => {
                    var validationError = ModelStateErrorManager.getErrors(e);
                    if (validationError) {
                        Notify.error(validationError.replace(/\n/g, "<br />"));
                    } 
                });
        }

        /*
            Handler de click sur le bouton Save
        */
        $ctrl.handleClickSave = function () {
            if (!actionValidateClassificationSociete()) {
                return;
            }
            $q.when()
                .then(Create)
                .then(function () { $ctrl.IsOpen = true; });
        };

        /*
            Handler de click sur le bouton Save And Creat
        */
        $ctrl.handleClickSaveCreat = function () {
            if (!actionValidateClassificationSociete()) {
                return;
            }
            
            $q.when()
                .then(Create)
                .then(resetForm)
                .then(initFormParam)
                .then(function () { $ctrl.ModeCreation = true; });
        };

        /*
            Handler de click sur le bouton Delete
        */
        $ctrl.handleClickDelete = function () {
            $q.when()
                .then(Delete);
        };

        /*
            Validation de la classification de société
        */
        function actionValidateClassificationSociete() {
            if (!$ctrl.class.Code || !$ctrl.class.Libelle) {
                return false;
            }
            return true;
        }

        /*
            Reset du form 
        */
        function resetForm() {
            $ctrl.formClassificationSociete.$setPristine();
            $ctrl.formClassificationSociete.$setUntouched();
        }

        return $ctrl;
    }
})(angular);