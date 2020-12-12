(function () {
    'use strict';

    angular
        .module('Fred')
        .component('referentielTachesComponent', {
            templateUrl: '/Areas/ReferentielTaches/Scripts/ReferentielTachesTemplate.html',
            bindings: {
                resources: '<',
                favoriId: '<',
                showTaskLevelFour: '<',
                tacksChanged: '&?'
            },
            controller: 'referentielTachesComponentController'
        });


    angular.module('Fred').controller('referentielTachesComponentController', referentielTachesComponentController);

    referentielTachesComponentController.$inject = ['$scope', '$uibModal', 'TaskManagerService', 'Notification', 'TachesService', 'confirmDialog', 'ngProgressFactory', 'fredSubscribeService', '$q', 'favorisService', '$window', 'Notify'];

    function referentielTachesComponentController($scope, $uibModal, TaskManagerService, Notification, TachesService, confirmDialog, ngProgressFactory, fredSubscribeService, $q, favorisService, $window, Notify) {


        var $ctrl = this;
        $scope.selectedTaskOnMainList = null;
        $scope.letterLimit = 50;
        $scope.filters = {
            CI: null,
            searchTache: null
        };

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////

        this.$onInit = function () {
            var winHeight = $(window).height() - 160;
            //$('#accordion').height(winHeight);

            $('.niveau-container-vertical').css('max-height', winHeight);
            // Affichage par défaut du mode Arbo
            $ctrl.viewArbo = true;
            $ctrl.viewSplitted = false;
            $ctrl.btnContinuerToStep2 = false;
            $ctrl.Step2 = false;
            $ctrl.Step1 = true;
            $ctrl.Step3 = false;
            $ctrl.btnAnnuler = true;
            $ctrl.btnReturnToStep1 = false;
            $ctrl.btnReturnToStep2 = false;
            $ctrl.btnValider = false;
            $ctrl.btnContinuerToStep3 = false;
            $scope.listIdTache = [];

            $scope.$on('budgetCrtl.ciSelectedChanged', function (event, ciSelected) {
                $scope.filters.CI = ciSelected;
                $scope.loadData(ciSelected);
            });
            if (!$ctrl.resources) {
                $scope.resources = resources;
                $ctrl.resources = resources;
            } else {
                $scope.resources = $ctrl.resources;
            }

            $scope.progressBar = ngProgressFactory.createInstance();
            $scope.progressBar.setHeight("7px");
            $scope.progressBar.setColor("#FDD835");
            /*
             * Action executée lorsque le selecteur de vue change.
             */
            fredSubscribeService.subscribe({
                eventName: 'taskPlan.viewChanged',
                callback: function (views) {
                    $ctrl.viewArbo = views.viewArbo;
                    $ctrl.viewSplitted = views.viewSplitted;
                }
            });

            $scope.getFilterOrFavoris($ctrl.favoriId);
        };

        $ctrl.copyCI = function () {
            $scope.progressBar.start();

            $q.when()
                .then(function () { return { source: $scope.filters.CiSource.CiId, destination: $scope.ciId }; })
                .then(TachesService.copyCI)
                .then(function () { return $scope.ciId; })
                .then($scope.loadData)
                .then(function () { $('#copy-tasks').modal('hide'); })
                .catch($scope.progressBar.complete)
                .finally($scope.progressBar.complete);
        };


        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// CHARGEMENT        ///////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////
        $scope.loadData = function (ciSelected) {
            sessionStorage.setItem('planDeTacheFilters', JSON.stringify($scope.filters));
            // Set selected referential ID
            if ($ctrl.showTaskLevelFour) {
                $scope.ciId = ciSelected.CiId;
            } else {
                $scope.ciId = $scope.filters.CI.CiId;
            }

            $scope.selectedTask1 = null;
            $scope.selectedTask2 = null;
            $scope.selectedTask3 = null;
            $scope.selectedTask4 = null;

            $scope.mainList = [];

            $scope.list1 = [];
            $scope.list2 = [];
            $scope.list3 = [];
            $scope.list4 = [];


            $scope.progressBar.start();

            TachesService.getTachesByCIId($scope.ciId).then(function (response) {
                var tasks = response.data;
                TaskManagerService.init(tasks);
                $scope.list1 = TaskManagerService.getList1();
                $scope.mainList = TaskManagerService.getList1();

                $scope.progressBar.complete();
            });
        };


        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// SELECTION         ///////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////
        $scope.select = function (task) {
            select(task);
        };

        function select(task) {
            $scope.selectedTaskOnMainList = task;
            var result = TaskManagerService.selectTask(task);
            $scope.list1 = result.TasksLevelOne;
            $scope.list2 = result.TasksLevelTwo;
            $scope.list3 = result.TasksLevelTwree;
            $scope.list4 = result.TasksLevelFoor;

            $scope.selectedTask1 = result.TaskLevelOne;
            $scope.selectedTask2 = result.TaskLevelTwo;
            $scope.selectedTask3 = result.TaskLevelTwree;
            $scope.selectedTask4 = result.TaskLevelFoor;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// TASKS LEVEL ONE   ///////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////
        // Gestion des tâches de niveau 1
        $scope.addTask1 = function () {
            var ciId = $scope.ciId;

            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-one-modal.html',
                backdrop: 'static',
                controller: 'TaskOneCreateModalController',
                size: 'md',
                resolve: {
                    resources: function () {
                        return $ctrl.resources;
                    },
                    ciId: function () {
                        return ciId;
                    },
                    validateTask: function () {
                        return validateTask;
                    },
                    mainList: function () {
                        return $scope.mainList;
                    }
                }
            });

            modalInstance.result.then(function (newTask) {
                TaskManagerService.addTask(newTask);
                select(newTask);
                raisedEvent('add', newTask);
            });
        };

        $scope.editTask1 = function (editedTask1) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-one-modal.html',
                backdrop: 'static',
                controller: 'TaskOneEditModalController',
                size: 'md',
                resolve: {
                    resources: function () {
                        return $ctrl.resources;
                    },
                    validateTask: function () {
                        return validateTask;
                    },
                    editedTask1: function () {
                        return editedTask1;
                    }
                }
            });

            modalInstance.result.then(function (modifiedTask) {
                angular.extend(editedTask1, modifiedTask);
                select(editedTask1);
                raisedEvent('edit', editedTask1);
            });
        };

        $scope.deleteTask1 = function (deletedTask1) {
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                TachesService.deleteTache(deletedTask1).then(function (t) {
                    TaskManagerService.removeTask(deletedTask1);
                    $scope.selectedTask1 = null;
                    $scope.selectedTask2 = null;
                    $scope.selectedTask3 = null;
                    $scope.selectedTask4 = null;

                    $scope.list2 = [];
                    $scope.list3 = [];
                    $scope.list4 = [];
                    showSuppressionSuccessMessage();
                    raisedEvent('delete', deletedTask1);
                }).catch(function (error) {
                    showSuppressionSuccesError(error);
                });
            });

        };
        // Fin gestion des tâches de niveau 1


        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// TASKS LEVEL 2     ///////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////
        // Gestion des tâches de niveau 2
        $scope.addTask2 = function (selectedTask1) {

            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-two-modal.html',
                backdrop: 'static',
                controller: 'taskTwoCreateModalController',
                size: 'md',
                resolve: {
                    resources: function () {
                        return resources;
                    },
                    ciId: function () {
                        return $scope.ciId;
                    },
                    selectedTask1: function () {
                        return selectedTask1;
                    },
                    validateTask: function () {
                        return validateTask;
                    }
                }
            });

            modalInstance.result.then(function (newTask) {
                TaskManagerService.addTask(newTask);
                select(newTask);
                raisedEvent('add', newTask);
            });
        };

        $scope.editTask2 = function (editedTask2) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-two-modal.html',
                backdrop: 'static',
                controller: 'taskTwoEditModalController',
                size: 'md',
                resolve: {
                    resources: function () {
                        return resources;
                    },
                    editedTask2: function () {
                        return editedTask2;
                    },
                    validateTask: function () {
                        return validateTask;
                    }
                }
            });

            modalInstance.result.then(function (modifiedTask) {
                angular.extend(editedTask2, modifiedTask);
                select(editedTask2);
                raisedEvent('edit', editedTask2);
            });
        };

        $scope.deleteTask2 = function (deletedTask2) {
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                TachesService.deleteTache(deletedTask2).then(function () {
                    TaskManagerService.removeTask(deletedTask2);
                    $scope.selectedTask2 = null;
                    $scope.selectedTask3 = null;
                    $scope.selectedTask4 = null;
                    $scope.list3 = [];
                    $scope.list4 = [];

                    showSuppressionSuccessMessage();
                    raisedEvent('delete', deletedTask2);
                }).catch(function (error) {
                    showSuppressionSuccesError(error);
                });
            });
        };
        // Fin gestion des tâches de niveau 2


        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// TASKS LEVEL 3     ///////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////
        // Gestion des tâches de niveau 3
        $scope.addTask3 = function (selectedTache1, selectedTache2) {

            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-three-modal.html',
                backdrop: 'static',
                controller: 'TaskThreeCreateModalController',

                size: 'md',
                resolve: {
                    resources: function () {
                        return $ctrl.resources;
                    },
                    selectedTache2: function () {
                        return selectedTache2;
                    },
                    ciId: function () {
                        return $scope.ciId;
                    },

                    validateTask: function () {
                        return validateTask;
                    }
                }
            });
            modalInstance.result.then(function (newTask) {
                // Invalide la tache par défaut actuelle au besoin
                if (newTask.TacheParDefaut) {
                    invalidCurrentDefaultTask();
                }
                TaskManagerService.addTask(newTask);
                select(newTask);
                raisedEvent('add', newTask);

            });
        };
        $scope.editTask3 = function (editedTask3) {

            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-three-modal.html',
                backdrop: 'static',
                controller: 'TaskThreeEditModalController',
                size: 'md',
                resolve: {
                    resources: function () {
                        return $ctrl.resources;
                    },
                    editedTask3: function () {
                        return editedTask3;
                    },
                    validateTask: function () {
                        return validateTask;
                    }
                }
            });

            modalInstance.result.then(function (modifiedTask) {
                // Invalide la tache par défaut actuelle au besoin
                if (modifiedTask.TacheParDefaut && !editedTask3.TacheParDefaut) {
                    invalidCurrentDefaultTask();
                }
                angular.extend(editedTask3, modifiedTask);
                select(editedTask3);
                raisedEvent('edit', editedTask3);
            });
        };

        $scope.deleteTask3 = function (deletedTask3) {
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                TachesService.deleteTache(deletedTask3).then(function (response) {
                    TaskManagerService.removeTask(deletedTask3);
                    $scope.selectedTask3 = null;
                    $scope.selectedTask4 = null;
                    $scope.list4 = [];
                    showSuppressionSuccessMessage();
                    raisedEvent('delete', deletedTask3);
                }).catch(function (error) {
                    showSuppressionSuccesError(error);
                });
            });
        };
        // Fin gestion des tâches de niveau 3


        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// TASKS LEVEL 4     ///////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////
        // Gestion des tâches de niveau 4
        $scope.addTask4 = function (selectedTache1, selectedTache2, selectedTache3) {
            if (!$ctrl.showTaskLevelFour) {
                return;
            }

            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-four-modal.html',
                backdrop: 'static',
                controller: 'TaskFourCreateModalController',
                size: 'md',
                resolve: {
                    resources: function () {
                        return $ctrl.resources;
                    },
                    selectedTache3: function () {
                        return selectedTache3;
                    },

                    ciId: function () {
                        return $scope.ciId;
                    },

                    validateTask: function () {
                        return validateTask;
                    }
                }
            });
            modalInstance.result.then(function (newTask) {
                TaskManagerService.addTask(newTask);
                select(newTask);
                raisedEvent('add', newTask);
            });
        };

        $scope.editTask4 = function (editedTask4) {
            if (!$ctrl.showTaskLevelFour) {
                return;
            }
            var modalInstance = $uibModal.open({
                templateUrl: '/Areas/ReferentielTaches/Scripts/Dialogs/task-four-modal.html',
                backdrop: 'static',
                controller: 'TaskFourEditModalController',
                size: 'md',
                resolve: {
                    resources: function () {
                        return $ctrl.resources;
                    },
                    editedTask4: function () {
                        return editedTask4;
                    },
                    validateTask: function () {
                        return validateTask;
                    }
                }
            });

            modalInstance.result.then(function (modifiedTask) {
                angular.extend(editedTask4, modifiedTask);
                select(editedTask4);
                raisedEvent('edit', editedTask4);
            });
        };

        $scope.deleteTask4 = function (deletedTask4) {
            if (!$ctrl.showTaskLevelFour) {
                return;
            }
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                TachesService.deleteTache(deletedTask4).then(function (response) {
                    TaskManagerService.removeTask(deletedTask4);
                    $scope.selectedTask4 = null;
                    showSuppressionSuccessMessage();
                    raisedEvent('delete', deletedTask4);
                }).catch(function (error) {
                    showSuppressionSuccesError(error);
                });
            });
        };
        // Fin gestion des tâches de niveau 4


        /////////////////////////////////////////////////////////////////////////////////////////////
        // Tâche par défaut                                                                        //
        /////////////////////////////////////////////////////////////////////////////////////////////

        // Indique si une tâche est une tâche par défaut ou si elle est parente d'une tâche par défaut
        // - task : la tâche concernée
        $scope.isDefaultTaskOrParentOf = function (task) {
            if (task.TacheParDefaut) {
                return true;
            }

            for (var i = 0; i < task.TachesEnfants.length; i++) {
                var child = task.TachesEnfants[i];

                if (child.TacheParDefaut) {
                    return true;
                }

                if ($scope.isDefaultTaskOrParentOf(child)) {
                    return true;
                }
            }
        };


        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// PRIVATES METHODES ///////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        function raisedEvent(eventName, task) {
            if ($ctrl.tacksChanged) {
                var tasksChangedInfo = { event: eventName, task: task };
                $ctrl.tacksChanged({ tasksChangedInfo: tasksChangedInfo });
            }
        }

        function showSuppressionSuccessMessage() {
            Notification({
                message: resources.Global_Notification_Suppression_Success,
                title: resources.Global_Notification_Titre
            });
        }

        function showSuppressionSuccesError(error) {
            Notification.error({
                message: error.data.Message,
                positionY: 'bottom', positionX: 'right'
            });
        }

        function validateTask(task, ddl1, ddl2, ddl3) {
            var message = '';
            if (task !== undefined) {
                if (task.Code === '') { message += resources.ReferencielTache_Component_SaisieCode + '. '; }
                if (task.Libelle === '') { message += resources.ReferencielTache_Component_SaisieLibelle + '. '; }
            }
            if (ddl1 !== undefined && ddl1.selected === null) { message += resources.ReferencielTache_Component_SelectionnerTache1 + ' '; }
            if (ddl2 !== undefined && ddl2.selected === null) { message += resources.ReferencielTache_Component_SelectionnerTache2 + ' '; }
            if (ddl3 !== undefined && ddl3.selected === null) { message += resources.ReferencielTache_Component_SelectionnerTache3 + ' '; }
            return message;
        }

        $scope.handleCloseAll = function (level) {
            if (level) {
                angular.element(document.querySelectorAll('[id^=' + level + ']')).collapse('hide');
            }
            else {
                angular.element(document.querySelectorAll('.panel-collapse')).collapse('hide');
            }
        };

        $scope.handleOpenAll = function (level) {
            switch (level) {
                case 't1':
                    // Ouverture des panneaux T1
                    angular.element(document.querySelectorAll('[id^=t1]')).collapse('show');
                    break;
                case 't2':
                    // Ouverture des panneaux T1 et T2
                    angular.element(document.querySelectorAll('[id^=t1]')).collapse('show');
                    angular.element(document.querySelectorAll('[id^=t2]')).collapse('show');
                    break;
                case 't3':
                    // Ouverture des panneaux T1, T2 et T3
                    angular.element(document.querySelectorAll('[id^=t1]')).collapse('show');
                    angular.element(document.querySelectorAll('[id^=t2]')).collapse('show');
                    angular.element(document.querySelectorAll('[id^=t3]')).collapse('show');
                    break;
                default:
                    angular.element(document.querySelectorAll('.panel-collapse:not(.in)')).collapse('show');
                    break;
            }
        };

        // Invalide la tache par défaut
        function invalidCurrentDefaultTask() {
            var currentDefaultTask = TaskManagerService.getDefaultTask(null);
            if (currentDefaultTask)
                currentDefaultTask.TacheParDefaut = false;
        }

        $scope.updateFilters = function () {
            sessionStorage.setItem('planDeTacheFilters', JSON.stringify($scope.filters));
        };

        /*
        * @function addFilter2Favoris()
        * @description Crée un nouveau favori
        */
        $scope.addFilter2Favoris = function () {
            var filter = {
                ValueText: $scope.filters.searchTache,
                CI: $scope.filters.CI
            };
            var url = $window.location.pathname;
            if ($scope.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("Tache", url, filter);
        };

        $scope.getFilterOrFavoris = function (favoriId) {
            $scope.favoriId = parseInt(favoriId);
            if ($scope.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $scope.favoriId, defaultFilter: $scope.filter }).then(function (response) {
                    $scope.filters.searchTache = response.ValueText;
                    $scope.filters.CI = response.CI;
                    $scope.loadData($scope.filters.CI);
                }).catch(function (error) {
                    if (error && error.ExceptionMessage) {
                        Notify.error(error.ExceptionMessage);
                    }
                    console.log(error);
                })
            } else if (sessionStorage.getItem('planDeTacheFilters') !== null) {
                $scope.filters = JSON.parse(sessionStorage.getItem('planDeTacheFilters'));
                $scope.loadData($scope.filters.CI);
            }
        };

        $scope.selectCISource = function () {
            $ctrl.btnContinuerToStep2 = true;

        };
        $ctrl.ToStep2 = function () {
            $ctrl.Step2 = true;
            $ctrl.Step1 = false;
            $ctrl.btnAnnuler = false;
            $ctrl.btnReturnToStep1 = true;
            $ctrl.btnContinuerToStep2 = false;
            $('#stepper1').removeClass("selected");
            $('#stepper2').addClass("selected");
            if ($('#copieTotal').hasClass("selected"))
                $ctrl.btnValider = true;
            else {
                $ctrl.btnValider = false;
                $ctrl.btnContinuerToStep3 = true;
            }
        };
        $ctrl.returnToStep2 = function () {
            $ctrl.Step2 = true;
            $ctrl.Step3 = false;
            $ctrl.btnAnnuler = false;
            $ctrl.btnValider = false;
            $ctrl.btnValider2 = false;
            $ctrl.btnReturnToStep1 = true;
            $ctrl.btnReturnToStep2 = false;
            $ctrl.btnContinuerToStep2 = false;
            $ctrl.btnContinuerToStep3 = true;
            $('#stepper3').removeClass("selected");
            $('#stepper2').addClass("selected");
        };
        $ctrl.returnToStep1 = function () {
            $ctrl.Step2 = false;
            $ctrl.Step1 = true;
            $ctrl.btnAnnuler = true;
            $ctrl.btnValider = false;
            $ctrl.btnReturnToStep1 = false;
            $ctrl.btnContinuerToStep2 = true;
            $ctrl.btnContinuerToStep3 = false;
            $('#stepper2').removeClass("selected");
            $('#stepper1').addClass("selected");
        };
        $ctrl.clickToPartialCopy = function () {
            if (!$('#copiePartial').hasClass("selected")) {
                $ctrl.btnValider = false;
                $ctrl.btnContinuerToStep3 = true;
                $('#copieTotal').removeClass("selected");
                $('#copiePartial').addClass("selected");
                angular.element(document.querySelector('#copiePartial')).append('<div class="inner-triangle"></div>');
                angular.element(document.querySelector('#copieTotal .inner-triangle')).remove();
            }
        };
        $ctrl.clickToTotalCopy = function () {
            if (!$('#copieTotal').hasClass("selected")) {
                $ctrl.btnValider = true;
                $ctrl.btnContinuerToStep3 = false;
                $('#copiePartial').removeClass("selected");
                $('#copieTotal').addClass("selected");
                angular.element(document.querySelector('#copieTotal')).append('<div class="inner-triangle"></div>');
                angular.element(document.querySelector('#copiePartial .inner-triangle')).remove();
            }
        };
        $ctrl.ToStep3 = function () {
            $ctrl.listTacheLevel1 = null;
            $ctrl.Step2 = false;
            $ctrl.btnValider = false;
            $ctrl.btnValider2 = true;
            $ctrl.btnReturnToStep2 = true;
            $ctrl.btnReturnToStep1 = false;
            $ctrl.btnContinuerToStep3 = false;
            $('#stepper2').removeClass("selected");
            $('#stepper3').addClass("selected");
            TachesService.GetTacheLevel1({ ciId: $scope.filters.CiSource.CiId, level: 1 }).then(function (response) {
                $ctrl.listTacheLevel1 = response.data;
                $ctrl.listTacheLevel1 = $ctrl.listTacheLevel1.filter(tache => tache.Code != '00');
            }).catch(function (error) {
                showSuppressionSuccesError(error);
            });
            $ctrl.Step3 = true;
        };
        $ctrl.cancelCopy = function () {
            initmodal();
        };

        $ctrl.onCheckTache = function (item) {
            var id = "#ch" + item;
            var isCheked = angular.element(document.querySelector(id)).context.checked;
            if (isCheked) {
                $scope.listIdTache.push(item);
            }
            else {
                $scope.listIdTache = $scope.listIdTache.filter(x => x !== item);
            }
        };

        $ctrl.copyPartialTache = function () {
            $scope.progressBar.start();
            $q.when()
                .then(function () { return { source: $scope.filters.CiSource.CiId, destination: $scope.ciId, listIdTache: $scope.listIdTache }; })
                .then(TachesService.copyPartialTache)
                .then(function () { return $scope.ciId; })
                .then($scope.loadData)
                .then(function () { $('#copy-tasks').modal('hide'); })
                .catch(function (error) {
                    showSuppressionSuccesError(error);
                    $scope.progressBar.complete();
                })
                .finally($scope.progressBar.complete);
        };

        function initmodal() {
            $('#stepper2').removeClass("selected");
            $('#stepper1').addClass("selected");
            $('#stepper3').removeClass("selected");
            $('#copiePartial').removeClass("selected");
            $('#copieTotal').addClass("selected");
            angular.element(document.querySelector('#copiePartial .inner-triangle')).remove();
            angular.element(document.querySelector('#copieTotal .inner-triangle')).remove();
            angular.element(document.querySelector('#copieTotal')).append('<div class="inner-triangle"></div>');
            $scope.listIdTache = [];
            $scope.filters.CiSource = null;
            $ctrl.Step2 = false;
            $ctrl.Step3 = false;
            $ctrl.Step1 = true;
            $ctrl.btnAnnuler = true;
            $ctrl.btnValider = false;
            $ctrl.btnValider2 = false;
            $ctrl.btnReturnToStep1 = false;
            $ctrl.btnReturnToStep2 = false;
            $ctrl.btnContinuerToStep2 = false;
            $ctrl.btnContinuerToStep3 = false;

        }
        $ctrl.showmodal = function () {
            initmodal();
        };
    }
})();
