(function (angular) {
  'use strict';

  angular.module('Fred').directive('fredStepper', stepperDirective);

  stepperController.$inject = ['$scope'];

  function stepperController($scope) {
    $scope.forms = {
      "steps": [{
        step: 1,
        name: "First Step",
        // template: 'This is the first step',
        // template: "template.step1.html", // Load an html file. Ideally with Angular template cache. If you do so you have to replace {{step.template}} with ng-include="step.template" in directive -> template: ....
        expanded: true
      }, {
        step: 2,
        name: "Second Step",
        // template: "This is the second step",
        // template: "template.step2.html", // Load an html file. Ideally with Angular template cache. If you do so you have to replace {{step.template}} with ng-include="step.template" in directive -> template: ....
        expanded: false
      }, {
        step: 3,
        name: "Third Step",
        // template: "This is the third step",
        // template: "template.step3.html", // Load an html file. Ideally with Angular template cache. If you do so you have to replace {{step.template}} with ng-include="step.template" in directive -> template: ....
        expanded: false
      }, {
        step: 4,
        name: "Fourth Step",
        // template: "This is the fourth step",
        // template: "template.step4.html", // Load an html file. Ideally with Angular template cache. If you do so you have to replace {{step.template}} with ng-include="step.template" in directive -> template: ....
        expanded: false
      }]
    };
  }

  function stepperDirective() {

    function link($scope, $element, $attrs) {
      $scope.toggleListItems = function (index) {

        $scope.forms.steps[index].expanded = !$scope.forms.steps[index].expanded;

        for (var i = 0; i < $scope.forms.steps.length; i++) {

          if ($scope.forms.steps[i].expanded === true && i !== index) {
            $scope.forms.steps[i].expanded = false;
          }
        }
      };
    }

    var directive = {
      restrict: 'E',
      scope: {},
      controller: stepperController,
      templateUrl: '/Areas/Pointage/ListePointagePersonnel/Scripts/Directives/stepper.directive.html',
      link: link
    };
    return directive;
  }
}(angular));