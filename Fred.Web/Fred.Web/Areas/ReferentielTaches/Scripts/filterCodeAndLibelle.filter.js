angular.module('Fred').filter('filterCodeAndLibelle', function () {
  return function (items, searchText) {
    if (searchText !== undefined && searchText !== null) {
      var filtered = [];
      for (var i = 0; i < items.length; i++) {
        var item = items[i];
        if (item.Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
          filtered.push(item);
        }
      }
      return filtered;
    }

    return items;
  };
});