(function () {

    'use strict';

    angular.module('Fred').service('AvancementCalculService', AvancementCalculService);

    AvancementCalculService.$inject = ['BudgetService'];


    function AvancementCalculService(BudgetService) {

        var service = this;

        service.EcartAvancement = EcartAvancement;
        service.EcartAvancementT4 = EcartAvancementT4;
        service.EcartDAD = EcartDAD;
        service.CalculPourcentageRAD = CalculPourcentageRAD;
        service.CalculQuantiteRAD = CalculQuantiteRAD;
        service.getTotalDadMoisPrecedent = getTotalDadMoisPrecedent;
        service.getTotalDadMoisCourant = getTotalDadMoisCourant;
        service.getTotalQuantiteRad = getTotalQuantiteRad;
        service.getTotalPourcentageRad = getTotalPourcentageRad;
        service.getTotalRad = getTotalRad;
        service.getTotalEcart = getTotalEcart;
        service.CalculAvancementFromEcartPourcent = CalculAvancementFromEcartPourcent;
        
        /**
         * Calcul l'ecart entre l'avancement mois courant et mois précedent sur cette tache, si aucun avancement n'existe sur le mois précedent
         * la fonction retourne l'avancement mois courant, si aucun avancement n'existe la fonction retourne null
         * @param {any} tache une tache ni null ni undefined
         * @returns {any} null si aucun n'avancement n'existe, un montant arrondi a deux decimales sinon
         */
        function EcartAvancement(tache) {
            if (tache.AvancementPourcentPrevious) {
                return tache.AvancementPourcent - tache.AvancementPourcentPrevious;
            }
            else if (tache.AvancementPourcent) {
                return tache.AvancementPourcent;
            }
            else {
                return null;
            }
        }

        /**
         * Même chose que pour la fonction de calcul d'ecart Avancement, sauf que celle ci est spécialisée pour un T4
         * @param {any} tache4 une tache ni null ni undefined
         * @returns {any} un avancement arrondi a 2 decimales
         */
        function EcartAvancementT4(tache4) {
            if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Pourcentage) {
                return (tache4.AvancementPourcent - tache4.AvancementPourcentPrevious).toFixed(2);
            }
            else if (tache4.TypeAvancement === BudgetService.TypeAvancementEnum.Quantite) {
                return (tache4.AvancementQte - (tache4.AvancementPourcentPrevious || 0) * (tache4.Quantite || 0) / 100).toFixed(2);
            }
        }

        /**
         * Calcul l'avancement en fonction de l'avancement du mois précédent et de l'écart
         * la fonction retourne l'avancement mois courant, si aucun avancement n'existe la fonction retourne l'ecart
         * @param {number} avancementPourcentPrevious pourcentage d'avancement du mois précédent
         * @param {number} ecartAvancement ecart d'avancement
         * @returns {number} null si aucun n'avancement n'existe, un montant arrondi a deux decimales sinon
         */
        function CalculAvancementFromEcartPourcent(avancementPourcentPrevious, ecartAvancement) {

            var avancementPourcent = Number(avancementPourcentPrevious || 0) + Number(ecartAvancement || 0);
            if (avancementPourcent > 100)
                avancementPourcent = 100;

            if (avancementPourcent < 0)
                avancementPourcent = 0;

            return avancementPourcent.toFixed(2);

        }

        /**
         * Calcul l'ecart entre le DAD du mois précedent et le DAD du mois courant, si aucun DAD n'existe sur le mois précedent la fonction retourne 
         * l'avancement du mois courant
         * @param {any} tache une tache ni null ni undefined
         * @returns {any}  un montant non arrondi, jamais null
         */
        function EcartDAD(tache) {
            if (tache.DADPrevious) {
                return tache.DAD - tache.DADPrevious;
            }
            else {
                return tache.DAD;
            }
        }

        /**
         * Calcul le pourcentage du RAD 
         * @param {any} tache une tache ni null ni undefined
         * @returns {any} null si aucun montant budgété n'est connu pour cette tache, un pourcentage non arrondi sinon
         */
        function CalculPourcentageRAD(tache) {
            if (tache.Montant) {
                return tache.RAD / tache.Montant * 100;
            }
            else {
                return null;
            }
        }

        /**
         * Calcul la quantité du RAD
         * @param {any} tache une tache ni null ni undefined
         * @returns {any} null si montant budgété n'est connu puor cette tache, une quantité non arrondi sinon
         */
        function CalculQuantiteRAD(tache) {
            if (tache.Montant) {
                return tache.RAD / tache.Montant * tache.Quantite;
            }
            else {
                return null;
            }
        }

        /**
         * Calcul le montant total du DAD mois précedent, si le paramètre est null ou undefined, la fonction retourne null
         * @param {any} budget budget dont on veut connaitre le total du Dad mois précedent
         * @returns {any} null si le paramètre budget est null, un montant total non arrondi sinon
         */
        function getTotalDadMoisPrecedent(budget) {
            if (!budget) {
                return null;
            }

            let total = 0;
            budget.TachesNiveau1.forEach((t1) => {

                if (t1.DADPrevious) {
                    total += t1.DADPrevious;
                }

            });

            return total;
        }

        /**
         * Calcul le montant total du DAD mois courant, si le paramètre est null ou undefined, la fonction retourne null
         * @param {any} budget budget dont on veut connaitre le total du Dad mois courant
         * @returns {any} null si le paramètre budget est null, un montant total non arrondi sinon
         */
        function getTotalDadMoisCourant(budget) {
            if (!budget) {
                return null;
            }

            let total = 0;
            budget.TachesNiveau1.forEach((t1) => {
                if (t1.DAD) {
                    total += t1.DAD;
                }

            });

            return total;
        }

        /**
         * Calcul le montant total de l'ecart DAD, si le paramètre est null ou undefined, la fonction retourne null
         * @param {any} budget budget dont on veut connaitre le total de l'ecart DAD
         * @returns {any} null si le paramètre budget est null, un montant total non arrondi sinon
         */
        function getTotalEcart(budget) {
            if (!budget) {
                return null;
            }

            let total = 0;
            budget.TachesNiveau1.forEach((t1) => {
                let ecart = EcartDAD(t1);
                if (ecart) {
                    total += ecart;
                }

            });

            return total;
        }

        /**
         * Calcul le montant total du RAD, si le paramètre est null ou undefined, la fonction retourne null
         * @param {any} budget budget dont on veut connaitre le total du RAD
         * @returns {any} null si le paramètre budget est null, un montant total non arrondi sinon
         */
        function getTotalRad(budget) {
            if (!budget) {
                return null;
            }

            let total = 0;
            budget.TachesNiveau1.forEach((t1) => {
                if (t1.RAD) {
                    total += t1.RAD;
                }
            });

            return total;
        }

        /**
         * Calcul le montant total du pourcentage RAD, si le paramètre est null ou undefined, la fonction retourne null
         * @param {any} budget budget dont on veut connaitre le total du pourcentage RAD
         * @returns {any} null si le paramètre budget est null, un pourcentage total non arrondi sinon
         */
        function getTotalPourcentageRad(budget) {

            if (!budget) {
                return null;
            }

            let total = 0;
            budget.TachesNiveau1.forEach((t1) => {

                let pourcentage = CalculPourcentageRAD(t1);
                if (pourcentage) {
                    total += CalculPourcentageRAD(t1);
                }
            });

            let average = total / budget.TachesNiveau1.length;
            return average;
        }

        /**
         * Calcul le montant total du Rad, si le paramètre est null ou undefined, la fonction retourne null
         * @param {any} budget budget dont on veut connaitre le total du Rad
         * @returns {any} null si le paramètre budget est null, un montant total non arrondi sinon
         */
        function getTotalQuantiteRad(budget) {

            if (!budget) {
                return null;
            }

            let total = 0;
            budget.TachesNiveau1.forEach((t1) => {

                let quantite = CalculQuantiteRAD(t1);
                if (quantite) {
                    total += CalculQuantiteRAD(t1);
                }

            });

            return total;
        }
    }

})();