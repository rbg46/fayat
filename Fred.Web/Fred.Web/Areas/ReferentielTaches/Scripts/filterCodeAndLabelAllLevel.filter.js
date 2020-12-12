angular.module('Fred').filter('filterCodeAndLibelleAllLevel', function () {
  return function (tasks, searchText, level) {
    if (searchText !== undefined && searchText !== null && searchText !== '') {
      var filtered = [];
      for (var i = 0; i < tasks.length; i++) {
        var task = tasks[i];

        if (level === 1) {
          // Je regarde au niveau 1
          if (match(task, searchText)) {
            filtered.push(task);
          } else {
            // Je regarde au niveau 2
            if (childMatch(task, searchText)) {
              filtered.push(task);
            } else {
              // Je regarde au niveau 3
              if (task.TachesEnfants) {
                for (var j = 0; j < task.TachesEnfants.length; j++) {
                  if (childMatch(task.TachesEnfants[j], searchText)) {
                    filtered.push(task);
                  }
                }
              }

            }
          }
        }

        if (level === 2) {
          // Je regarde au niveau 1
          if (match(task, searchText)) {
            filtered.push(task);
          } else {
            // Je regarde au niveau 2
            if (childMatch(task, searchText)) {
              filtered.push(task);
            }
          }
        }

        if (level === 3) {
          if (match(task, searchText)) {
            filtered.push(task);
          }
        }
      }
      return filtered;
    }

    return tasks;
  };

  // verifie si les enfant d'une tache correspondent à la recherche
  function childMatch(task, searchText) {
    if (task.TachesEnfants) {
      for (var j = 0; j < task.TachesEnfants.length; j++) {
        if (match(task.TachesEnfants[j], searchText)) {
          return true;
        }
      }
    }
    return false;
  }

  // verifie si une tache correspondent a la recherche
  function match(task, searchText) {
    if (task.Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || task.Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
      return true;
    }
    return false;
  }
});