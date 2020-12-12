(function () {
    'use strict';

    angular.module('Fred').service('CommandeService', CommandeService);

    CommandeService.$inject = ['$http', '$q'];

    function CommandeService($http, $q) {

        return {

            /* Service de création d'une nouvelle instance de commande */
            Detail: function (id) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Commande/Detail/' + id, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            Duplicate: function (id) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Commande/Detail/Duplicate/' + id, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Get objet de recherche */
            GetFilter: function () {
                return $q(function (resolve, reject) {
                    $http.get('/api/Commande/Filter/', { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service GetCommandesGroupByCI */
            GetGroupByCI: function (filter, page, pageSize) {
                return $http.post('/api/Commande/ListByCI/' + page + '/' + pageSize, filter, { cache: false });
            },

            /* Service GetCommandesGroupByCI */
            Export: function (filter) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Commande/GenerateExcel/', filter, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Update */
            Update: function (model) {
                return $q(function (resolve, reject) {
                    $http.put("/api/Commande/", model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Create */
            Save: function (model, setTacheParDefaut) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/" + '?setTacheParDefaut=' + setTacheParDefaut, model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            RequestValidation: function (validationRequestModel) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/RequestValidation/", validationRequestModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            ValidateHeader: function (model) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/ValidateHeader", model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Delete */
            Delete: function (id) {
                return $q(function (resolve, reject) {
                    $http.delete("/api/Commande/" + id, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de récupération des types de commandes */
            CommandeTypeList: function () {
                return $q(function (resolve, reject) {
                    $http.get("/api/Commande/CommandeTypeList", { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de récupération des types de devises */
            DeviseRef: function (id) {
                return $q(function (resolve, reject) {
                    $http.get("/api/CI/DeviseRef/" + id + "/", { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de récupération de la valeur IsCiHaveManyDevises */
            IsCiHaveManyDevises: function (id) {
                return $q(function (resolve, reject) {
                    $http.get("/api/CI/IsCiHaveManyDevises/" + id + "/", { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de récupération de la societe gérante */
            SocieteGerante: function (ci) {
                return $q(function (resolve, reject) {
                    $http.get(`/api/CI/GetSocieteByCIId/${ci.CiId}`, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            ExtractExcel: function (filter) {
                return $http.post("/api/Commande/GenerateExcel", filter, { cache: false });
            },

            ExtractPDFBonDeCommande: function (commande) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/GeneratePdfBonDeCommande", JSON.stringify(commande),
                        {
                            cache: false,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json'
                        })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            ExtractPDFBonDeCommandeDeCommandeBrouillon: function (commande) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/GenerateBonDeCommandeDeCommandeBrouillon", JSON.stringify(commande),
                        {
                            cache: false,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json'
                        })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            ExtractPDFBrouillonDeBonDeCommande: function (commande) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/GenerateBrouillonDeBonDeCommande", JSON.stringify(commande),
                        {
                            cache: false,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json'
                        })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Envoi de la commande par mail */
            SendByMail: function (commande) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/SendByMail", commande, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Clôture de la commande */
            Cloturer: function (commandeId) {
                return $q(function (resolve, reject) {
                    $http.get("/api/Commande/Cloturer/" + commandeId)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /*Service pour Valider les commande */
            Valider: function (commandeId, StatutModel) {
                return $q(function (resolve, reject) {
                    $http.put("/api/Commande/Valider?commandeId=" + commandeId, StatutModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Déclôture de la commande */
            Decloturer: function (commandeId) {
                return $q(function (resolve, reject) {
                    $http.get("/api/Commande/Decloturer/" + commandeId)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Export d'une commande validée vers SAP */
            ReturnCommandeToSap: function (commandeId) {
                return $q(function (resolve, reject) {
                    $http.get("/api/Commande/ReturnCommandeToSap/" + commandeId)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Récupération de la dernière date de génération de réception */
            GetLastDayOfReceptionGeneration: function (nextReceptionDate, frequenceAbo, dureeAbo) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Commande/GetLastDateOfReceptionGeneration/' + nextReceptionDate + '/' + frequenceAbo + '/' + dureeAbo)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Récupération de la dernière date de génération de réception */
            GetListUniteByRessourceId: function (ciId, ressourceId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Unite/GetListUniteByRessourceId/' + ciId + '/' + ressourceId)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Récupération de la dernière date de génération de réception */
            IsLimitationUnitesRessource: function (ciId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Societe/IsLimitationUnitesRessource/' + ciId)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Enregistre les lignes d'avenant et le valide au besoin */
            SaveAvenant: function (commande, toValidate) {
                var model = {
                    CommandeId: commande.CommandeId,
                    CreatedLignes: [],
                    UpdatedLignes: [],
                    DeletedLigneIds: [],
                    CommentaireFournisseur: commande.CommentaireFournisseur,
                    CommentaireInterne: commande.CommentaireInterne,
                    Abonnement: {
                        IsAbonnement: commande.IsAbonnement,
                        Frequence: commande.FrequenceAbonnement,
                        Duree: commande.DureeAbonnement,
                        DateProchaineReception: commande.DateProchaineReception,
                        DatePremiereReception: commande.DatePremiereReception
                    },
                    DelaiLivraison: commande.DelaiLivraison,
                    Fournisseur: {
                        Adresse: commande.FournisseurAdresse,
                        CodePostal: commande.FournisseurCPostal,
                        Ville: commande.FournisseurVille,
                        PaysId: commande.FournisseurPaysId
                    },
                    DateModification: commande.DateModification
                };

                for (var i = 0; i < commande.Lignes.length; i++) {
                    var ligne = commande.Lignes[i];
                    if (!ligne.IsAvenantNonValide) {
                        continue;
                    }
                    if (ligne.IsDeleted) {
                        model.DeletedLigneIds.push(ligne.CommandeLigneId);
                    }
                    else {
                        var ligneModel = {
                            CommandeLigneId: ligne.CommandeLigneId,
                            NumeroLigne: ligne.NumeroLigne,
                            TacheId: ligne.TacheId,
                            RessourceId: ligne.RessourceId,
                            UniteId: ligne.UniteId,
                            Libelle: ligne.Libelle,
                            Quantite: ligne.Quantite,
                            PUHT: ligne.PUHT,
                            IsDiminution: ligne.AvenantLigne.IsDiminution,
                            ViewId: ligne.ViewId
                        };
                        if (ligne.IsCreated) {
                            model.CreatedLignes.push(ligneModel);
                        }
                        else if (ligne.IsUpdated) {
                            model.UpdatedLignes.push(ligneModel);
                        }
                    }
                }

                var funcName = toValidate ? "SaveAvenantAndValidate" : "SaveAvenant";
                return $q(function (resolve, reject) {
                    $http.put("/api/Commande/" + funcName + "/", model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            }
        };
    }
})();
