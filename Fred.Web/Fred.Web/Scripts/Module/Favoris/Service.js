(function () {
  'use strict';

  angular.module('Fred').service('favorisService', favorisService);

  favorisService.$inject = ['$http', '$q', 'favoriModal'];

  function favorisService($http, $q, favoriModal) {

    var uriBase = "/api/Favoris/";

    return {
      /* Service ajout aux favoris */
      Add: function (favori) {
        return $q(function (resolve, reject) {
          $http.post(uriBase, favori, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },
      /* Service récupération d'une nouvelle instance de favori */
      GetNew: function (type) {
        return $q(function (resolve, reject) {
          $http.get(uriBase + "New/" + type, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },
      /* Service récupération des favoris */
      GetList: function () {
        return $q(function (resolve, reject) {
          $http.get(uriBase, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },
      /* Service récupération d'un favori */
      GetById: function (id) {
        return $q(function (resolve, reject) {
          $http.get(uriBase + id, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },
      /* Service suppression d'un favoris */
      Delete: function (idFav) {
        return $q(function (resolve, reject) {
          $http.delete(uriBase + idFav, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },
      /* Service Mise à jour d'un favoris */
      Update: function (model) {
        return $q(function (resolve, reject) {
          $http.put(uriBase, model, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },
      /*
      * Recupere le filtre avec le service des favori ou renvoie le filtre par default dans une promise.
      * sample :
      *  favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: $ctrl.filter });
      */
      getFilterByFavoriIdOrDefault: function (resquest) {
        if (!resquest.favoriId && !resquest.defaultFilter) {
          throw new Error('La fonction ne peux pas marcher sans les parametres requis.');
        }
        var promise = null;
        if (resquest.favoriId > 0 && resquest.favoriId !== "") {
          promise = this.GetById(resquest.favoriId);
        }
        else {
          var deferred = $q.defer();
          deferred.resolve(resquest.defaultFilter);
          promise = deferred.promise;
        }
        return promise;
      },

      /*
       * Récupère le favori avec le service des favoris
       * sample :
       * favoriService.getFilterByFavoriId(favoriId);
       */
      getFilterByFavoriId: function (favoriId) {
        return this.GetById(favoriId);
      },

      /**
       * Initialize et ouvre la modal d'enregistrement de la modal.
       * exemple :
       *  favorisService.initializeAndOpenModal({ path: "Personnel", filter: $ctrl.filter })
       * @param {Object} resquest - The employee who is responsible for the project.
       * @param {string} employee.path - chemin
       * @param {Object} employee.filter - le filtre courant.
       */
      initializeAndOpenModal: function (type, url, filter) {
        if (!type || !url || !filter) {
          throw new Error('La fonction ne peux pas marcher sans les parametres requis.');
        }
        var that = this;
        $q.when()
        .then(function () {
          return that.actionGetNewFavori(type, url, filter);
        })
        .then(function (favori) {
          favoriModal.open(resources, favori);
        });
      },


      /**
       * Initialize et ouvre la modal d'enregistrement de la modal.
       * exemple :
       *  favorisService.initializeAndOpenModal({ path: "Personnel", filter: $ctrl.filter })
       * @param {Object} resquest - The employee who is responsible for the project.
       * @param {string} employee.path - chemin
       * @param {Object} employee.filter - le filtre courant.
       */
      OpenModalDeleteFavori: function (favori, home) {
        favoriModal.openDeleteModal(resources, favori, home)
      },
      /*
      * @function actionGetNewFavori()
      * @description Récupère un nouvelle objet Favori
      */
      actionGetNewFavori: function (type, url, filter) {
        var favori = {
          FavoriId : 0,
          UtilisateurId : 0,
          Libelle : null,
          Couleur : null,
          TypeFavori : type,
          UrlFavori : url + "/",
          Filtre : filter,
          Search : null
        }

        return favori;
      }
    };
  }

}());