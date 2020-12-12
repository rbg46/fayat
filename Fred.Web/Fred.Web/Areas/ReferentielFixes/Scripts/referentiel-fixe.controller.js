(function (angular) {
  'use strict';

  angular.module('Fred').controller('ReferentielsFixesCtrl', referentielsFixesCtrl);

  referentielsFixesCtrl.$inject = ['$scope', '$http', '$uibModal', '$filter', 'truncateTextFilter', 'Notification', 'ReferentielFixeService', 'confirmDialog'];

  function referentielsFixesCtrl($scope, $http, $uibModal, $filter, truncateTextFilter, Notification, ReferentielFixeService, confirmDialog) {

    //////////////////////////////////////////////////////////////////
    //                         INIT                                 //
    //////////////////////////////////////////////////////////////////

    // Instanciation Objet Resources
    $scope.resources = resources;

    $scope.letterLimit = 50;

    $scope.searchChapitre = '';
    $scope.chapterList = [];

    $scope.searchSouschapitre = '';
    $scope.subChapterList = [];

    $scope.searchRessource = '';
    $scope.resourceList = [];

    $scope.idSelectedChapitre = null;
    $scope.idSelectedSousChapitre = null;
    $scope.idSelectedRessource = null;
    $scope.selectedRessource = null;

      $scope.init = function () {
          
          actionLoadData();

          // Chargement de la liste des types de ressources
          ReferentielFixeService.getListTypeRessource().then(function (response) {
              $scope.resourceTypeList = response.data;
          }).catch(function (error) { console.log(error); });

          // Chargement de la liste des carburants
          ReferentielFixeService.getListCarburants().then(function (response) {
              $scope.carburantList = response.data;
          }).catch(function (error) { console.log(error); });
      };

    //////////////////////////////////////////////////////////////////
    //                         MODALS                               //
    //////////////////////////////////////////////////////////////////

    $scope.handleAddChapitre = actionCreateChapterModal;
    $scope.handleEditChapitre = actionEditChapterModal;
    $scope.handleAddSousChapitre = actionCreateSubChapterModal;
    $scope.handleEditSousChapitre = actionEditSubChapterModal;
    $scope.handleAddRessource = actionCreateResourceModal;
    $scope.handleEditRessource = actionEditResourceModal;


    //////////////////////////////////////////////////////////////////
    //                         HANDLERS                             //
    //////////////////////////////////////////////////////////////////

    // Handler Supprimer un chapitre
    $scope.handleDeleteChapter = function (chapter) {
      actionDeleteChapter(chapter);
    };

    // Handler Supprimer un sous-chapitre
    $scope.handleDeleteSubChapter = function (subChapter) {
      actionDeleteSubChapter(subChapter);
    };

    // Handler Supprimer une ressource
    $scope.handleDeleteResource = function (resource) {
      actionDeleteResource(resource);
    };

    // Handler Sélection d'un chapitre
    $scope.handleSelectChapter = function (chapterId) {
      actionSelectChapter(chapterId);
    };

    // Handler Sélection d'un sous-chapitre
    $scope.handleSelectSubChapter = function (subChapterId) {
      actionSelectSubChapter(subChapterId);
    };

    // Handler Sélection d'une ressource
    $scope.handleSelectResource = function (resource) {
      actionSelectResource(resource);
    };

    // Handler Fonction d'extraction des commandes au format excel
    $scope.handleExtractExcel = function () {
      ReferentielFixeService.generateExcel().then(function (response) {
        window.location.href = '/api/ReferentielFixes/ExtractExcel/' + response.data.id;
      }).catch(function (error) { console.log(error); });
    };


    //////////////////////////////////////////////////////////////////
    //                          ACTIONS                             //
    //////////////////////////////////////////////////////////////////

    // Action chargement des données de la page
    function actionLoadData() {
      ReferentielFixeService.getChapitres().then(function (t) {
        $scope.chapterList = t.data;
        if ($scope.chapterList.length > 0) {
          $scope.idSelectedChapitre = t.data[0].ChapitreId;
          ReferentielFixeService.getSousChapitres($scope.idSelectedChapitre).then(function (t) {
            $scope.subChapterList = t.data;
            if ($scope.subChapterList.length > 0) {
              $scope.idSelectedSousChapitre = $scope.subChapterList[0].SousChapitreId;
              ReferentielFixeService.getResources($scope.idSelectedSousChapitre).then(function (t) {
                $scope.resourceList = t.data;
                if ($scope.resourceList.length > 0) {
                  $scope.idSelectedRessource = $scope.resourceList[0].RessourceId;
                  $scope.selectedRessource = $scope.resourceList[0];
                }
              });

            }
          });
        }
      });
    }

    // Action supprimer un chapitre
    function actionDeleteChapter(chapter) {
      confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {

        ReferentielFixeService.deleteChapitre(chapter).then(function (response) {
          $scope.chapterList.splice($scope.chapterList.indexOf(chapter), 1);
          $scope.idSelectedChapitre = null;
          $scope.idSelectedSousChapitre = null;
          $scope.idSelectedRessource = null;
          $scope.selectedRessource = null;
          $scope.resourceList = [];
          $scope.subChapterList = [];
          Notification({ message: resources.Global_Notification_Suppression_Success, title: resources.Global_Notification_Titre });
        }).catch(function (error) {
          Notification.error({ message: error.data.Message, positionY: 'bottom', positionX: 'right' });
        });
      });
    }

    // Action supprimer un sous-chapitre
    function actionDeleteSubChapter(subChapter) {
      confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {

        ReferentielFixeService.deleteSousChapitre(subChapter).then(function (response) {
          $scope.subChapterList.splice($scope.subChapterList.indexOf(subChapter), 1);
          $scope.idSelectedRessource = null;
          $scope.selectedRessource = null;
          $scope.idSelectedSousChapitre = null;
          $scope.resourceList = [];
          Notification({ message: resources.Global_Notification_Suppression_Success, title: resources.Global_Notification_Titre });
        }).catch(function (error) {
          Notification.error({ message: error.data.Message, positionY: 'bottom', positionX: 'right' });
        });
      });
    }

    // Action supprimer une ressource
    function actionDeleteResource(resource) {
      confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {

        ReferentielFixeService.deleteRessource(resource).then(function (response) {
          $scope.resourceList.splice($scope.resourceList.indexOf(resource), 1);
          $scope.idSelectedRessource = null;
          $scope.selectedRessource = null;
          Notification({ message: resources.Global_Notification_Suppression_Success, title: resources.Global_Notification_Titre });
        }).catch(function (error) {
          Notification.error({ message: error.data.Message, positionY: 'bottom', positionX: 'right' });
        });
      });
    }

    // Action Sélection d'un chapitre
    function actionSelectChapter(chapterId) {
      $scope.resourceList = [];
      $scope.idSelectedRessource = null;
      $scope.selectedRessource = null;
      $scope.idSelectedSousChapitre = null;
      $scope.idSelectedChapitre = chapterId;

      ReferentielFixeService.getSousChapitres($scope.idSelectedChapitre).then(function (response) {
        $scope.subChapterList = response.data;
      }).catch(function (error) {
        Notification.error({ message: error.data.Message, positionY: 'bottom', positionX: 'right' });
      });
    }

    // Action Sélection d'un sous-chapitre
    function actionSelectSubChapter(subChapterId) {
      $scope.idSelectedRessource = null;
      $scope.selectedRessource = null;
      $scope.idSelectedSousChapitre = subChapterId;

      ReferentielFixeService.getResources($scope.idSelectedSousChapitre).then(function (response) {
        $scope.resourceList = response.data;
      }).catch(function (error) {
        Notification.error({ message: error.data.Message, positionY: 'bottom', positionX: 'right' });
      });
    }

    // Action Sélection d'une ressource
    function actionSelectResource(resourceId) {
      ReferentielFixeService.getRessourceById(resourceId).then(function (response) {
        $scope.idSelectedRessource = response.data.RessourceId;
        $scope.selectedRessource = response.data;
      }).catch(function (error) { console.log(error); });
    }

    // Action Création d'un chapitre
    function actionCreateChapterModal(items) {
      $uibModal.open({
        templateUrl: '/Areas/ReferentielFixes/Scripts/modals/chapitre-modal.html',
        backdrop: 'static',
        controller: 'ActionCreateChapterModalController',
        size: 'md',
        resolve: {
          items: function () {
            return items;
          }
        }
      });
    }

    // Action Modification d'un chapitre
    function actionEditChapterModal(item) {
      $uibModal.open({
        templateUrl: '/Areas/ReferentielFixes/Scripts/modals/chapitre-modal.html',
        backdrop: 'static',
        controller: 'ActionEditChapterModalController',
        size: 'md',
        resolve: {
          item: function () {
            return item;
          }
        }
      });
    }

    // Action Création d'un sous-chapitre
    function actionCreateSubChapterModal(items, chapitreId) {
      var chapterList = $scope.chapterList;

      var modalInstance = $uibModal.open({
        templateUrl: '/Areas/ReferentielFixes/Scripts/modals/sous-chapitre-modal.html',
        backdrop: 'static',
        controller: 'ActionCreateSubChapterModalController',
        size: 'md',
        resolve: {
          chapterList: function () {
            return chapterList;
          },
          chapitreId: function () {
            return chapitreId;
          }
        }
      });

      modalInstance.result.then(function (createdSubChapter) {
        $scope.handleSelectChapter(createdSubChapter.ChapitreId);
        $scope.handleSelectSubChapter(createdSubChapter.SousChapitreId);
      });
    }

    // Action Modification d'un sous-chapitre
    function actionEditSubChapterModal(item) {     

      var modalInstance = $uibModal.open({
        templateUrl: '/Areas/ReferentielFixes/Scripts/modals/sous-chapitre-modal.html',
        backdrop: 'static',
        controller: 'ActionEditSubChapterModalController',
        size: 'md',
        resolve: {
          item: function () {
            return item;
          },
          chapterList: function () {
            return $scope.chapterList;
          }
        }
      });

      modalInstance.result.then(function (updatedSubChapter) {
        $scope.handleSelectChapter(updatedSubChapter.ChapitreId);
        $scope.handleSelectSubChapter(updatedSubChapter.SousChapitreId);
      });
    }

    // Action Création d'une ressource
    function actionCreateResourceModal(resourceList, chapitreId, sousChapitreId) {
      var chapterList = $scope.chapterList;
      var subChapterList = $scope.subChapterList;
      var resourceTypeList = $scope.resourceTypeList;
      var carburantList = $scope.carburantList;

      var modalInstance = $uibModal.open({
        templateUrl: '/Areas/ReferentielFixes/Scripts/modals/ressource-modal.html',
        backdrop: 'static',
        controller: 'ActionCreateResourceModalController',
        size: 'md',
        resolve: {
          chapterList: function () {
            return chapterList;
          },
          subChapterList: function () {
            return subChapterList;
          },
          resourceTypeList: function () {
            return resourceTypeList;
          },
          carburantList: function () {
            return carburantList;
          },
          chapitreId: function () {
            return chapitreId;
          },
          sousChapitreId: function () {
            return sousChapitreId;
          }
        }
      });

      modalInstance.result.then(function (createdResource) {
        $scope.handleSelectChapter(createdResource.SousChapitre.ChapitreId);
        $scope.handleSelectSubChapter(createdResource.SousChapitreId);
        $scope.handleSelectResource(createdResource.RessourceId);
      });
    }

    // Action Modification d'une ressource
    function actionEditResourceModal(item, chapitreId, sousChapitreId, resourceList) {
      var chapterList = $scope.chapterList;
      var subChapterList = $scope.subChapterList;
      var resourceTypeList = $scope.resourceTypeList;
      var carburantList = $scope.carburantList;

      var modalInstance = $uibModal.open({
        templateUrl: '/Areas/ReferentielFixes/Scripts/modals/ressource-modal.html',
        backdrop: 'static',
        controller: 'ActionEditResourceModalController',
        size: 'md',
        resolve: {
          chapterList: function () {
            return chapterList;
          },
          subChapterList: function () {
            return subChapterList;
          },
          resourceTypeList: function () {
            return resourceTypeList;
          },
          carburantList: function () {
            return carburantList;
          },
          item: function () {
            return item;
          },
          chapitreId: function () {
            return chapitreId;
          },
          sousChapitreId: function () {
            return sousChapitreId;
          }
        }
      });

      modalInstance.result.then(function (updatedResource) {
        $scope.handleSelectSubChapter(updatedResource.SousChapitreId);
        $scope.handleSelectResource(updatedResource.RessourceId);
      });
    }
  }




  angular.module('Fred').controller('ActionCreateChapterModalController', ActionCreateChapterModalController);

  ActionCreateChapterModalController.$inject = ['$scope', '$uibModalInstance', 'ReferentielFixeService', 'Notification'];

  function ActionCreateChapterModalController($scope, $uibModalInstance, ReferentielFixeService, Notification) {
    $scope.resources = resources;
    $scope.modalTitle = resources.ReferentielFixe_Modal_Chapitre_Creation;
    var clone = { ChapitreId: 0, Code: '', Libelle: '' };
    $scope.clone = clone;

    $scope.close = function () {
      $uibModalInstance.dismiss('cancel');
    };

    $scope.save = function () {
      $scope.ErrorMessage;
      var message = '';
      if ($scope.clone.Code.length === 0) {
        message += resources.ReferentielFixe_Modal_Code_Erreur + '. ';
      }
      if ($scope.clone.Libelle.length === 0) {
        message += resources.ReferentielFixe_Modal_Libelle_Erreur + '. ';
      }
      if (message.length === 0) {
        ReferentielFixeService.addChapitre($scope.clone).then(
          function (response) {
            $scope.$resolve.items.push(response.data);
            $uibModalInstance.close();
            Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
          },
          function (error) {
            $scope.ErrorMessage = error.data.Message;
          });
      }
      else {
        $scope.ErrorMessage = message;
      }
    };
  }



  angular.module('Fred').controller('ActionEditChapterModalController', ActionEditChapterModalController);

  ActionEditChapterModalController.$inject = ['$scope', '$uibModalInstance', 'ReferentielFixeService', 'Notification'];

  function ActionEditChapterModalController($scope, $uibModalInstance, ReferentielFixeService, Notification) {
    $scope.modalTitle = resources.ReferentielFixe_Modal_Chapitre_Modification;
    $scope.resources = resources;

    var clone = {};
    angular.copy($scope.$resolve.item, clone);
    $scope.clone = clone;
    $scope.close = function () {
      $uibModalInstance.dismiss('cancel');
    };
    $scope.save = function () {
      $scope.ErrorMessage;
      var message = '';
      if ($scope.clone.Code.length === 0) {
        message += resources.ReferentielFixe_Modal_Code_Erreur;
      }
      if ($scope.clone.Libelle.length === 0) {
        message += ' ' + resources.ReferentielFixe_Modal_Libelle_Erreur;
      }
      if (message.length === 0) {
        ReferentielFixeService.updateChapitre($scope.clone).then(
          function (response) {
            angular.extend($scope.$resolve.item, clone);
            $uibModalInstance.close();
            Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
          }).catch(function (error) {
            $scope.ErrorMessage = error.data.Message;
          });
      }
      else {
        $scope.ErrorMessage = message;
      }
    };
  }



  angular.module('Fred').controller('ActionCreateSubChapterModalController', ActionCreateSubChapterModalController);

  ActionCreateSubChapterModalController.$inject = ['$scope', '$uibModalInstance', 'ReferentielFixeService', 'Notification', '$filter', 'ReferentielFixeHelperService'];

  function ActionCreateSubChapterModalController($scope, $uibModalInstance, ReferentielFixeService, Notification, $filter, ReferentielFixeHelperService) {
    $scope.resources = resources;
    $scope.modalTitle = resources.ReferentielFixe_Modal_SousChapitre_Creation;
    $scope.modalSelectedChapitre = null;
    $scope.chapterList = $scope.$resolve.chapterList;
    $scope.modalSelectedChapitre = $filter('filter')($scope.chapterList, { ChapitreId: $scope.$resolve.chapitreId }, true)[0];
    $scope.clone = { SousChapitreId: 0, ChapitreId: $scope.$resolve.chapitreId, Code: '', Libelle: '' };

    $scope.close = function () {
      $uibModalInstance.dismiss('cancel');
    };
      $scope.save = function () {
          $scope.ErrorMessage;
          var message = ReferentielFixeHelperService.validateReferentielFixe($scope.clone);

          if (message.length === 0) {
              ReferentielFixeService.addSousChapitre($scope.clone).then(function (response) {
                  $uibModalInstance.close(response.data);
                  Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
              }).catch(function (error) {
                  $scope.ErrorMessage = error.data.Message;
              });
          }
          else {
              $scope.ErrorMessage = message;
          }
      };

      $scope.getSelectedChapitre = function () {
          $scope.clone.ChapitreId = $scope.modalSelectedChapitre.ChapitreId;
      };
  }




  angular.module('Fred').controller('ActionEditSubChapterModalController', ActionEditSubChapterModalController);

  ActionEditSubChapterModalController.$inject = ['$scope', '$uibModalInstance', 'ReferentielFixeService', 'Notification', '$filter', 'ReferentielFixeHelperService'];

  function ActionEditSubChapterModalController($scope, $uibModalInstance, ReferentielFixeService, Notification, $filter, ReferentielFixeHelperService) {
    $scope.resources = resources;
    $scope.modalTitle = resources.ReferentielFixe_Modal_SousChapitre_Modification;
    $scope.chapterList = $scope.$resolve.chapterList;
    $scope.modalSelectedChapitre = $filter('filter')($scope.$resolve.chapterList, { ChapitreId: $scope.$resolve.item.ChapitreId }, true)[0];

    var clone = {};
    angular.copy($scope.$resolve.item, clone);
    $scope.clone = clone;

    $scope.close = function () {
      $uibModalInstance.dismiss('cancel');
    };

    $scope.save = function () {
      $scope.ErrorMessage;
      var message = ReferentielFixeHelperService.validateReferentielFixe();

      if (message.length === 0) {
        ReferentielFixeService.updateSousChapitre($scope.clone).then(function (response) {
          $uibModalInstance.close(response.data);
          Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
        }).catch(function (error) {
          $scope.ErrorMessage = error.data.Message;
        });
      }
      else {
        $scope.ErrorMessage = message;
      }
    };
  }




  angular.module('Fred').controller('ActionCreateResourceModalController', ActionCreateResourceModalController);

  ActionCreateResourceModalController.$inject = ['$scope', '$uibModalInstance', 'ReferentielFixeService', 'Notification', '$filter', 'ReferentielFixeHelperService'];

  function ActionCreateResourceModalController($scope, $uibModalInstance, ReferentielFixeService, Notification, $filter, ReferentielFixeHelperService) {
    $scope.resources = resources;
    $scope.modalTitle = resources.ReferentielFixe_Modal_Ressource_Creation;
    $scope.clone = {
      RessourceId: 0,
      Code: null,
      Libelle: '',
      Active: true,
      ChapitreId: $scope.$resolve.chapitreId,
      SousChapitreId: $scope.$resolve.sousChapitreId
    };

    // Init des dropdownlist
    $scope.chapterList = $scope.$resolve.chapterList;
    $scope.subChapterList = $scope.$resolve.subChapterList;
    $scope.typeRessources = $scope.$resolve.resourceTypeList;
    $scope.listCarburants = $scope.$resolve.carburantList;

    // Init des éléments sélectionnés dans les dropdownlist
    $scope.modalSelectedChapitre = $filter('filter')($scope.$resolve.chapterList, { ChapitreId: $scope.$resolve.chapitreId }, true)[0];
    $scope.modalSelectedSousChapitre = $filter('filter')($scope.$resolve.subChapterList, { SousChapitreId: $scope.$resolve.sousChapitreId }, true)[0];

    // Init Proposition d'un nouveau code ressource
    ReferentielFixeService.getNextRessourceCode($scope.$resolve.sousChapitreId).then(function (response) {
      $scope.clone.Code = response.data;
    }).catch(function (error) { console.log(error); });

    $scope.close = function () {
      $uibModalInstance.dismiss('cancel');
    };

    $scope.save = function () {
      $scope.ErrorMessage;
      var message = ReferentielFixeHelperService.validateReferentielFixe($scope.clone);

      if (message.length === 0) {
        ReferentielFixeService.addRessource($scope.clone).then(function (response) {
          $uibModalInstance.close(response.data);
          Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
        }).catch(function (error) {
          $scope.ErrorMessage = error.data.Message;
        });
      }
      else {
        $scope.ErrorMessage = message;
      }
    };

    // Action sur Chapitre dropdownlist 
      $scope.getSelectedChapitre = function () {
          $scope.clone.ChapitreId = $scope.modalSelectedChapitre.ChapitreId;

          if ($scope.clone.ChapitreId !== null && $scope.clone.ChapitreId !== undefined) {
              ReferentielFixeService.getSousChapitres($scope.clone.ChapitreId).then(function (response) {
                  $scope.subChapterList = response.data;
                  if ($scope.subChapterList.length > 0) {
                      $scope.modalSelectedSousChapitre = $scope.subChapterList[0];
                      $scope.clone.SousChapitreId = $scope.modalSelectedSousChapitre.SousChapitreId;
                      ReferentielFixeService.getNextRessourceCode($scope.clone.SousChapitreId).then(function (response) {
                          $scope.clone.Code = response.data;
                      }).catch(function (error) { console.log(error); });
                  }
                  else {
                      $scope.modalSelectedSousChapitre = null;
                      $scope.clone.SousChapitreId = null;
                  }
              }).catch(function (error) { console.log(error); });
          }
      };

    // Action sur Sous Chapitre dropdownlist
      $scope.getSelectedSousChapitre = function () {
          if ($scope.modalSelectedSousChapitre !== null && $scope.modalSelectedSousChapitre !== undefined) {
              $scope.clone.SousChapitreId = $scope.modalSelectedSousChapitre.SousChapitreId;
              ReferentielFixeService.getNextRessourceCode($scope.clone.SousChapitreId).then(function (response) {
                  $scope.clone.Code = response.data;
              }).catch(function (error) { console.log(error); });
          }
      };

    // Action sur Type Ressource dropdownlist
      $scope.getSelectedRessourceType = function () {
          if ($scope.modalSelectedRessourceType !== undefined && $scope.modalSelectedRessourceType !== null) {
              $scope.clone.TypeRessourceId = $scope.modalSelectedRessourceType.TypeRessourceId;
              $scope.clone.TypeRessource = $scope.modalSelectedRessourceType;
              $scope.clone.TypeCode = $scope.modalSelectedRessourceType.Code;
          }
          else {
              $scope.clone.TypeRessource = null;
              $scope.clone.TypeRessourceId = null;
              $scope.clone.TypeCode = null;
          }
      };

    // Action sur Carburant dropdownlist
      $scope.getSelectedCarburant = function () {
          $scope.clone.CarburantId = $scope.modalSelectedCarburant.CarburantId;
          $scope.clone.Carburant = $scope.modalSelectedCarburant.Carburant;
      };
  }


  angular.module('Fred').controller('ActionEditResourceModalController', ActionEditResourceModalController);

  ActionEditResourceModalController.$inject = ['$scope', '$uibModalInstance', 'ReferentielFixeService', 'Notification', '$filter', 'ReferentielFixeHelperService'];

  function ActionEditResourceModalController($scope, $uibModalInstance, ReferentielFixeService, Notification, $filter, ReferentielFixeHelperService) {
    $scope.resources = resources;
    $scope.modalTitle = resources.ReferentielFixe_Modal_Ressource_Modification;

    // Init des dropdownlist
    $scope.chapterList = $scope.$resolve.chapterList;
    $scope.subChapterList = $scope.$resolve.subChapterList;
    $scope.typeRessources = $scope.$resolve.resourceTypeList;
    $scope.listCarburants = $scope.$resolve.carburantList;
    $scope.clone = $scope.$resolve.item;

    // Init des éléments sélectionnés dans les dropdownlist
    $scope.modalSelectedChapitre = $filter('filter')($scope.$resolve.chapterList, { ChapitreId: $scope.$resolve.chapitreId }, true)[0];
    $scope.modalSelectedSousChapitre = $filter('filter')($scope.$resolve.subChapterList, { SousChapitreId: $scope.$resolve.sousChapitreId }, true)[0];
    $scope.modalSelectedRessourceType = $filter('filter')($scope.$resolve.resourceTypeList, { TypeRessourceId: $scope.$resolve.item.TypeRessourceId }, true)[0];
    $scope.modalSelectedCarburant = $filter('filter')($scope.$resolve.carburantList, { CarburantId: $scope.$resolve.item.CarburantId }, true)[0];

    $scope.close = function () {
      $uibModalInstance.dismiss('cancel');
    };

    $scope.save = function () {
      $scope.savingRessourceInProgress = true;
      $scope.ErrorMessage;
      var message = ReferentielFixeHelperService.validateReferentielFixe($scope.clone);

      if (message.length === 0) {

        ReferentielFixeService.updateRessource($scope.clone).then(function (response) {
          $uibModalInstance.close(response.data);
          Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
        }).catch(function (error) {
          $scope.ErrorMessage = error.data.Message;
        });
      }
      else {
        $scope.ErrorMessage = message;
      }
    };

      $scope.getSelectedRessourceType = function () {
          if ($scope.modalSelectedRessourceType) {
              $scope.clone.TypeCode = $scope.modalSelectedRessourceType.Code;
              $scope.clone.TypeRessourceId = $scope.modalSelectedRessourceType.TypeRessourceId;
              $scope.clone.TypeRessource = $scope.modalSelectedRessourceType;
          }
          else {
              $scope.clone.TypeCode = null;
              $scope.clone.TypeRessourceId = null;
              $scope.clone.TypeRessource = null;
              $scope.clone.CarburantId = null;
              $scope.clone.Carburant = null;
              $scope.clone.Consommation = null;
              $scope.modalSelectedRessourceType = null;
              $scope.modalSelectedCarburant = null;
          }
      };

      $scope.getSelectedCarburant = function () {
          if ($scope.modalSelectedCarburant !== null && $scope.modalSelectedCarburant !== undefined) {
              $scope.clone.CarburantId = $scope.modalSelectedCarburant.CarburantId;
              $scope.clone.Carburant = $scope.modalSelectedCarburant.Carburant;
          }
      };
  }

  // Action Validation des formulaires de création

  angular.module('Fred').service('ReferentielFixeHelperService', ReferentielFixeHelperService);

  function ReferentielFixeHelperService() {

    var service = {
      validateReferentielFixe: validateReferentielFixe
    };

    return service;

    function validateReferentielFixe(object) {
      var message = '';
      if (object !== null && object !== undefined) {
        if (object.Code.length === 0 && object.Code !== undefined) {
          message += resources.ReferentielFixe_Modal_Code_Erreur + '. ';
        }
        if (object.Libelle.length === 0 && object.Libelle !== undefined) {
          message += resources.ReferentielFixe_Modal_Libelle_Erreur + '. ';
        }
        if ((object.ChapitreId === null || object.ChapitreId === '') && object.ChapitreId !== undefined) {
          message += resources.ReferentielFixe_Modal_Chapitre_Erreur + '. ';
        }
        if ((object.SousChapitreId === null || object.SousChapitreId === '') && object.SousChapitreId !== undefined) {
          message += resources.ReferentielFixe_Modal_SousChapitre_Erreur + '. ';
        }
      }
      return message;
    }
  }


}(angular));