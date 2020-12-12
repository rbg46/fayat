(function(angular) {
    'use strict';

    angular.module('Fred').directive("fredDisplayHandler", fredDisplayHandler);

    fredDisplayHandler.$inject = ['ParamsHandlerService', 'Notify', '$localStorage'];
    function fredDisplayHandler($ParamsHandlerService, Notify, $localStorage) {
        const showElementByOrganizationId = function(organizationId, element) {
            element.style.display = 'none';
            toggleElementByParamValues(organizationId, element, 'showenInputs', 'flex');
            element.style.justifyContent = "center";
        };

        const hideElementByOrganizationId = function(organizationId, element) {
            toggleElementByParamValues(organizationId, element, 'hiddenInputs', 'none');
        };

        const toggleElementByParamValues = function(organizationId, element, key, displayMode) {
            var elementId = element.id;

            const isElementsInLocalStorage = function() {
                return $localStorage[key] !== undefined && $localStorage[key][organizationId.toString()] !== undefined;
            };

            const isCurrentElementTargeted = function(elements) {
                return elements.indexOf(elementId) !== -1;
            };

            const saveElementsInLocalStorage = function(data) {
                if ($localStorage[key] === undefined)
                    $localStorage[key] = {};

                $localStorage[key][organizationId.toString()] = data;
            };

            const setCurrentElementDisplayMode = function() {
                element.style.display = displayMode;
            };

            const handleCurrentElementDisplayMode = function(elements) {
                if (isCurrentElementTargeted(elements)) {
                    setCurrentElementDisplayMode();
                }
            };

            if (isElementsInLocalStorage()) {
                var localStorageValue = $localStorage[key][organizationId.toString()];

                handleCurrentElementDisplayMode(localStorageValue);
            } else {
                $ParamsHandlerService.GetParamValues(organizationId, key).then(function(response) {
                    var data = response.data;

                    saveElementsInLocalStorage(data);
                    handleCurrentElementDisplayMode(data);
                }).catch(function() {
                    Notify.error(resources.Global_Notification_Error);
                });
            }
        };

        return {
            link: function(scope, element, attrs) {
                scope.$watch(attrs['fredDisplayHandler'], function(organizationId) {
                    if (organizationId === undefined) {
                        return;
                    }

                    var displayAttribute = attrs['display'];

                    if (displayAttribute === 'visible') {
                        showElementByOrganizationId(organizationId, element[0]);
                    } else if (displayAttribute === 'hidden') {
                        hideElementByOrganizationId(organizationId, element[0]);
                    }
                });
            }
        };
    }
}(angular));