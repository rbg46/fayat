(function () {
    'use strict';

    angular.module('Fred').service('BudgetComparaisonService', BudgetComparaisonService);
    BudgetComparaisonService.$inject = ['$q', '$http', '$filter'];

    function BudgetComparaisonService($q, $http, $filter) {

        const AxeAnalytiqueType = {
            TacheRessource: 0,
            RessourceTache: 1
        };

        const AxeAnalytiqueNodeType = {
            Tache1: 0,
            Tache2: 1,
            Tache3: 2,
            Tache4: 3,
            Chapitre: 4,
            SousChapitre: 5,
            Ressource: 6
        };

        const ColonneType = {
            Quantite: 1,
            Unite: 2,
            PrixUnitaire: 3,
            Montant: 4
        };

        const AxeAnalytiqueTacheRessourceNodes = [
            AxeAnalytiqueNodeType.Tache1,
            AxeAnalytiqueNodeType.Tache2,
            AxeAnalytiqueNodeType.Tache3,
            AxeAnalytiqueNodeType.Tache4,
            AxeAnalytiqueNodeType.Chapitre,
            AxeAnalytiqueNodeType.SousChapitre,
            AxeAnalytiqueNodeType.Ressource
        ];

        const AxeAnalytiqueRessourceTacheNodes = [
            AxeAnalytiqueNodeType.Chapitre,
            AxeAnalytiqueNodeType.SousChapitre,
            AxeAnalytiqueNodeType.Ressource,
            AxeAnalytiqueNodeType.Tache1,
            AxeAnalytiqueNodeType.Tache2,
            AxeAnalytiqueNodeType.Tache3,
            AxeAnalytiqueNodeType.Tache4
        ];

        const SansUnite = {
            UniteId: 0,
            Code: resources.BudgetComparaison_SansUnite_Code,
            Libelle: resources.BudgetComparaison_SansUnite_Libelle
        };


        //////////////////////////////////////////////////////////////////
        // Service                                                      //
        //////////////////////////////////////////////////////////////////
        var service = {
            Events: {
                DisplayFilterPanel: "BudgetComparaison.DisplayFilterPanel"
            },

            SansUnite: SansUnite,
            ColonneType: ColonneType,

            AxeAnalytique: {
                Type: AxeAnalytiqueType,
                NodeType: AxeAnalytiqueNodeType,
                TacheRessourceNodes: AxeAnalytiqueTacheRessourceNodes,
                RessourceTacheNodes: AxeAnalytiqueRessourceTacheNodes
            },

            Filter: {
                AxeAnalytique: {
                    Type: AxeAnalytiqueType.TacheRessource,
                    NodeTypes: angular.copy(AxeAnalytiqueTacheRessourceNodes),
                    Update: function () {
                        let axeNodeTypes = service.GetNodesFromType(this.Type);
                        let newNodeTypes = [];
                        for (let index = 0; index < axeNodeTypes.length; index++) {
                            let axeType = axeNodeTypes[index];
                            if (this.NodeTypes.findIndex((n) => n === axeType) > -1) {
                                newNodeTypes.push(axeType);
                            }
                        }
                        this.NodeTypes = newNodeTypes;
                    }
                },
                Colonnes: {
                    Budget1: {
                        HasQuantite: true,
                        HasUnite: true,
                        HasPrixUnitaire: true
                    },
                    Budget2: {
                        HasQuantite: true,
                        HasUnite: true,
                        HasPrixUnitaire: true
                    },
                    Ecart: {
                        HasQuantite: true,
                        HasUnite: true,
                        HasPrixUnitaire: true
                    }
                },
                IsPersonnalise: function () {
                    var nodes = GetNodesFromType(this.AxeAnalytique.Type);
                    if (!angular.equals(nodes, this.AxeAnalytique.NodeTypes)) {
                        return true;
                    }
                    if (ColonneIsPersonnalise(this.Colonnes.Budget1)
                        || ColonneIsPersonnalise(this.Colonnes.Budget2)
                        || ColonneIsPersonnalise(this.Colonnes.Ecart)) {
                        return true;
                    }
                    return false;
                }
            },

            GetNodesFromType: GetNodesFromType,
            GetUnitesTooltip: GetUnitesTooltip,
            GetUniteToDisplay: GetUniteToDisplay,

            // Http
            Compare: Compare,
            ExcelExport: ExcelExport
        };
        return service;


        //////////////////////////////////////////////////////////////////
        // Fonctions                                                    //
        //////////////////////////////////////////////////////////////////

        // Retourne les types de noeuds d'un type axe
        // - axeAnalytiqueType : le type d'axe
        function GetNodesFromType(axeAnalytiqueType) {
            switch (axeAnalytiqueType) {
                case AxeAnalytiqueType.TacheRessource:
                    return AxeAnalytiqueTacheRessourceNodes;
                case AxeAnalytiqueType.RessourceTache:
                    return AxeAnalytiqueRessourceTacheNodes;
            }
            return null;
        }

        // Retourne le tooltip des unités
        // - unites : les unités
        function GetUnitesTooltip(unites) {
            let hasSansUnite = false;
            let ret = '';
            let unitesOrdered = $filter('orderBy')(unites, 'Code');
            for (let i = 0; i < unitesOrdered.length; i++) {
                let unite = unitesOrdered[i];
                if (unite.UniteId === SansUnite.UniteId) {
                    hasSansUnite = true;
                }
                else {
                    ret += (i > 0 ? '\r\n' : '') + unite.Code + ' (' + unite.Libelle + ')';
                }
            }
            if (hasSansUnite) {
                ret += (unitesOrdered.length > 0 ? '\r\n' : '') + SansUnite.Libelle;
            }

            return ret;
        }

        // Retourne le texte des unités
        // - unites : les unités
        function GetUniteToDisplay(unites) {
            if (unites.length === 1) {
                return unites[0].Code;
            }
            else if (unites.length > 1) {
                return '#';
            }
            return '';
        }

        // Indique si les colonnes d'un groupe sont personnalisées
        // - group : le groups concerné
        function ColonneIsPersonnalise(group) {
            return !group.HasQuantite || !group.HasUnite || !group.HasPrixUnitaire;
        }


        //////////////////////////////////////////////////////////////////
        // HTTP                                                         //
        //////////////////////////////////////////////////////////////////

        // Compare des budgets
        // - request : la requête de comparaison
        // - return : le résultat de la comparaison
        function Compare(request) {
            return $http.post("/api/BudgetComparaison/Compare", request);
        }

        // Exporte la comparaison au format Excel
        // - request : la requête de l'export Excel
        // - return : le résultat de l'export
        function ExcelExport(request) {
            return $q(function (resolve, reject) {
                return $http.post("/api/BudgetComparaison/ExcelExport", request)
                    .success(function (data) {
                        if (!data.Erreur) {
                            window.location.href = '/api/BudgetComparaison/ExtractDocument/' + data.CacheId + '/' + 'BudgetComparaison';
                        }
                        resolve(data);
                    })
                    .error(function (data) { reject(data); });
            });
        }
    }
})();
