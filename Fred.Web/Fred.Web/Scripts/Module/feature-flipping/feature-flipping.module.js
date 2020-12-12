(function (angular, undefined) {
    'use strict';

    angular.module('Fred').provider('FeatureFlags', function () {
        this.$get = ['$rootScope', '$q', function ($rootScope, $q) {

            //var flags = this.flags;
            var flags = featureFlippingConfig;
            /**
            * Checks if the config object is in the valid format and is not a duplicate
            * @param {object} flagObj a config object in the example format.: {'id': 'foo', 'active': false}
            * @returns {boolean} the validity of the flagObj
            */
            function isValidFlagObj(flagObj) {
                return typeof flagObj.Name === 'string' && typeof flagObj.IsActived === 'boolean' && !isDuplicate(flagObj);
            }

            /**
            * Checks if the config object is not a duplicate
            * @param {object} flagObj a config object in the example format.: {'id': 'foo', 'active': false}
            * @returns {boolean} the duplicate status of the flagObj
            */
            function isDuplicate(flagObj) {
                return typeof _.find(flags, function (flag) { return flagObj.Name === flag.Name; }) === 'object';
            }

            /**
            * Checks if the supplied argument is a string
            * @param {string} flagId an id string used to identify a flag object
            * @returns {boolean} the status of the supplied string
            */
            function isString(flagId) {
                return typeof flagId === 'string';
            }

            /**
            * Accepts a string and returns a matching flag object
            * @param {string} flagId an id string used to identify a flag object
            * @returns {object|boolean} the matched object or false if there is no match
            */
            function getFlag(flagId) {
                var targetObj;

                if (isString(flagId)) {
                    targetObj = flags.find(function (flag) {
                        return flag.Name === flagId;
                    });
                }

                return targetObj ? targetObj : false;
            }

            /**
            * Add a new flag object to the config array
            * @param {object} flagObj a config object in the example format.: {'id': 'foo', 'active': false}
            * @returns {boolean} true if the object was successfully added to the array, otherwise false
            */
            function addFlag(flagObj) {
                return isValidFlagObj(flagObj) ? flags.push(flagObj) > 0 : false;
            }

            /**
            * Adds an array of config objects to the config array
            * @param {array} configArray an array of config objects
            * @returns {boolean} true if flag/s were added, otherwise false
            */
            function addFlags(configArray) {
                var intialLength = flags.length;

                if (_.isArray(configArray)) {
                    _.forEach(configArray, function (element) {
                        addFlag(element);
                    });
                }

                return flags.length > intialLength ? true : false;
            }

            /**
            * remove a flag from the config array
            * @param {string} flagId an id string used to identify a flag object
            * @returns {boolean} true if flag was removed, otherwise false
            */
            function removeFlag(flagId) {
                var status = false, flagLength = flags.length;

                if (isString(flagId)) {
                    flags = _.without(flags, _.findWhere(flags, { id: flagId }));

                    if (flags.length < flagLength) {
                        status = true;
                    }
                }

                return status;
            }

            /**
            * returns the status of the flagId provided
            * @param {string} flagId an id string used to identify a flag object
            * @returns {boolean} the status of the requested flag, false if the flag doesn't exist
            */
            function getFlagStatus(flagId) {
                var targetObj = getFlag(flagId);
                return targetObj && targetObj.IsActived !== undefined ? targetObj.IsActived : false;
            }

            /**
            * sets the status of the flagId provided
            * @param {string} flagId an id string used to identify a flag object
            * @param {newStatus} newStatus indicate the new status
            * @returns {boolean} true if the flag exists, otherwise false
            */
            function setFlagStatus(flagId, newStatus) {
                var targetObj = getFlag(flagId);
                var status = false;

                if (targetObj && typeof newStatus === 'boolean') {
                    targetObj.IsActived = newStatus;
                    $rootScope.$broadcast('featureUpdated', flagId);
                    status = true;
                }

                return status;
            }

            /**
            * get the array of flags
            * @returns {array} an array of flag objects
            */
            function getAllFlags() {
                return flags;
            }

            /**
            * reset the flags object to an empty array
            * @returns {array} an empty array
            */
            function removeAllFlags() {
                return flags = [];
            }

            /**
            * returns a promise which is resolved if all provided feature flags are enabled,
            * or rejected if any one is disabled; intended to be used in a router resolve
            * function to protect a route
            * @param {string|array} flagIds one or many flag ids required for the route
            * @return {promise} promise 
            */
            function guardRoute(flagIds) {
                return $q(function (resolve, reject) {
                    var allActive = true;
                    flagIds = angular.isArray(flagIds) ? flagIds : [flagIds];
                    for (var i = 0; i < flagIds.length; i++) {
                        allActive = getFlagStatus(flagIds[i]);
                        if (!allActive) { break; }
                    }
                    (allActive ? resolve : reject)();
                });
            }

            return {
                addFlag: addFlag,
                addFlags: addFlags,
                removeFlag: removeFlag,
                getFlagStatus: getFlagStatus,
                setFlagStatus: setFlagStatus,
                getAllFlags: getAllFlags,
                removeAllFlags: removeAllFlags,
                guardRoute: guardRoute
            };
        }];
    })
        .directive('featureFlag', ['FeatureFlags', '$rootScope', function (FeatureFlags, $rootScope) {

            var determineVisiibility = function (flagStatus, isInverted) {
                return isInverted === undefined ? flagStatus : !flagStatus;
            };

            var setVisibility = function (element, attrs) {
                var flagStatus = determineVisiibility(FeatureFlags.getFlagStatus(attrs.featureKey), attrs.invert);
                flagStatus ? element[0].classList.remove('ng-hide') : element[0].classList.add('ng-hide');
            };

            return {
                restrict: 'AE',
                compile: function () {

                    return function ($scope, element, attrs) {

                        $rootScope.$on('featureUpdated', function () {
                            setVisibility(element, attrs);
                        });
                        setVisibility(element, attrs);
                    };
                }
            };
        }]);
})(angular);
