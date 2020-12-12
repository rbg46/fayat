angular.module('Fred', ["kendo.directives"])
    .controller('countryController', function ($scope, $http) {

      // Instanciation Objet Ressources
      $scope.resources = resources;

      dataSource = new kendo.data.DataSource({
        transport: {
          read: function (e) {
            $http.get("/api/countries").success(function (data, status, headers, config) {
              e.success(data);
            })
            .error(function () {
              e.error(data);
            });
          },
          create: function (e) {
            $http.post('/api/countries/', e.data).success(function (data) {
              e.success(data);
            }).error(function (data) {
              e.error(data);
            });
          },
          update: function (e) {
            $http.put('/api/countries/' + e.data.CountryId, e.data).success(function (data) {
              e.success();
            }).error(function (data) {
              e.error(data);
            });

          },
          destroy: function (e) {
            $http.delete('/api/countries/' + e.data.CountryId).success(function (data) {
              e.success();
            }).error(function (data) {
              e.error(data);
            });
          }
        },
        schema: {
          model: {
            id: "CountryId",
            fields: {
              CountryId: { editable: false, type: "number" },
              NameFr: { validation: { required: true } },
              NameGb: { validation: { required: true } },
              CodeIso: { validation: { required: true } }
            }
          }
        },
      });

      $scope.countryGrid = {
        sortable: true,
        selectable: true,
        dataSource: dataSource,
        toolbar: ["create"],
        columns: [
          { field: "CountryId", title: "#" },
          { field: "NameFr", title: resources.NameFR_lb },
          { field: "NameGb", title: resources.NameGb_lb },
          { field: "CodeIso", title: resources.CodeISO_lb },
          { command: ["edit", "destroy"], title: "&nbsp;" }],
        editable: "inline"
      };
    });