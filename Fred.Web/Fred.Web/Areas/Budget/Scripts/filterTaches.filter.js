angular.module('Fred').filter('filterTaches', function () {
    return function (tasks, searchText, level) {
        if (searchText !== undefined && searchText !== null && searchText !== '' && tasks) {
            var filtered = [];
            for (var i = 0; i < tasks.length; i++) {
                var task = tasks[i];

                if (level === 1) {
                    ManageFilterNiveau1(task, searchText, filtered);
                }

                if (level === 2) {
                    ManageFilterNiveau2(task, searchText, filtered);
                }

                if (level === 3) {
                    ManageFilterNiveau3(task, searchText, filtered);
                }

                if (level === 4) {
                    ManageFilterNiveau4(task, searchText, filtered);
                }
            }
            return filtered;
        }

        return tasks;
    };

    function ManageFilterNiveau1(tache1, searchText, filtered) {
        // Je regarde au niveau 1
        if (match(tache1, searchText)) {
            filtered.push(tache1);
        } else {
            // Je regarde au niveau 2
            if (childMatch(tache1.TachesNiveau2, searchText)) {
                filtered.push(tache1);
            }
            else {
                var niveau3Match = false;
                if (tache1.TachesNiveau2) {
                    for (var j = 0; j < tache1.TachesNiveau2.length; j++) {
                        var tache2 = tache1.TachesNiveau2[j];
                        if (childMatch(tache2.TachesNiveau3, searchText)) {
                            niveau3Match = true;
                        }
                    }
                }
                if (niveau3Match) {
                    filtered.push(tache1);
                }
                else {
                    var niveau4Match = false;
                    if (tache1.TachesNiveau2) {
                        for (var k = 0; k < tache1.TachesNiveau2.length; k++) {
                            var tacheN2 = tache1.TachesNiveau2[k];
                            if (tacheN2.TachesNiveau3) {
                                for (var l = 0; l < tacheN2.TachesNiveau3.length; l++) {
                                    var tache3 = tacheN2.TachesNiveau3[l];
                                    if (childMatch(tache3.TachesNiveau4, searchText)) {
                                        niveau4Match = true;
                                    }
                                }
                            }
                        }
                    }
                    if (niveau4Match) {
                        filtered.push(tache1);
                    }
                }
            }
        }
    }

    function ManageFilterNiveau2(tache2, searchText, filtered) {
        // Je regarde au niveau 2
        if (match(tache2, searchText)) {
            filtered.push(tache2);
        } else {
            // Je regarde au niveau 3
            if (childMatch(tache2.TachesNiveau3, searchText)) {
                filtered.push(tache2);
            }
            else {
                // Je regarde au niveau 4
                var niveau3Match = false;
                if (tache2.TachesNiveau3) {
                    for (var j = 0; j < tache2.TachesNiveau3.length; j++) {
                        var tache3 = tache2.TachesNiveau3[j];
                        if (childMatch(tache3.TachesNiveau4, searchText)) {
                            niveau3Match = true;
                        }
                    }
                }
                if (niveau3Match) {
                    filtered.push(tache2);
                }
            }
        }
    }

    function ManageFilterNiveau3(tache3, searchText, filtered) {
        // Je regarde au niveau 2
        if (match(tache3, searchText)) {
            filtered.push(tache3);
        } else {
            // Je regarde au niveau 3
            if (childMatch(tache3.TachesNiveau4, searchText)) {
                filtered.push(tache3);
            }
        }
    }

    function ManageFilterNiveau4(tache4, searchText, filtered) {
        if (match(tache4, searchText)) {
            filtered.push(tache4);
        }
    }

    // verifie si une tache correspondent a la recherche dans les taches enfants
    function childMatch(listTaches, searchText) {
        if (listTaches && listTaches.length) {
            for (var i = 0; i < listTaches.length; i++) {
                var tacheEnfant = listTaches[i];
                if (match(tacheEnfant, searchText)) {
                    return true;
                }
            }
            return false;
        }
        else {
            return false;
        }
    }

    // verifie si une tache correspondent a la recherche
    function match(task, searchText) {
        if (task.Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || task.Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
            return true;
        }
        return false;
    }

});