
/*
 * Cette fonction permet de creer un fichier à partir d'une liste des données.
 */

function exportFile(filename, extention, rows) {
    var types; //fichier texte
    var breakligne = ',';

    switch (extention) {

        case "csv": {
            types = 'text/csv';
            break;
        }
        case "xls": {
            types = 'application/vnd.ms-excel';
            break;

        }
        case "xlsx": {
            types = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
            break;
        }

        default:
            {
                breakligne = '\r\n';
                types = "text/plain";
                break;
            }
    }

    ///function Process read row
    var processRow = function (row) {
        var finalVal = '';
        for (var j = 0; j < row.length; j++) {
            var innerValue = row[j] === null ? '' : row[j].toString();
            if (row[j] instanceof Date) {
                innerValue = row[j].toLocaleString();
            }

            var result = innerValue.replace(/"/g, '""');
            if (result.search(/("|,|\n)/g) >= 0)
                result = '"' + result + '"';
            if (j > 0)
                finalVal += breakligne;
            finalVal += result;
        }
        return finalVal + '\r\n';
    };

    var fileErreur = '';
    for (var i = 0; i < rows.length; i++) {
        fileErreur += processRow(rows[i]);
    }
    var data = fileErreur;
    if (extention.indexOf()!==-1) {
         data = rows;
    } 
    var blob = new Blob([data], { type: types });

    if (navigator.msSaveBlob) { // IE 10+
        navigator.msSaveBlob(blob, filename + "." + extention);
    } else {
        var link = document.createElement("a");
        if (link.download !== undefined) { // feature detection
            // Browsers that support HTML5 download attribute
            var url = URL.createObjectURL(blob);
            link.setAttribute("href", url);
            link.setAttribute("download", filename + "." + extention);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }


}



