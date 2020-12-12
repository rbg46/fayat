'use strict';

class ReferentialBase {
    constructor(referentialFixe) {
        this.Code = referentialFixe.Code;
        this.Libelle = referentialFixe.Libelle;
    }
}

class Chapitre extends ReferentialBase {
    constructor(chapitre, index) {
        super(chapitre);
        this.SousChapitres = null;
        this.Index = index;
        this.View = new ReferentialFixeView("BIBLIOTHEQUE_PRIX_CHAPITRE_ID_" + index);
    }
}

class SousChapitre extends ReferentialBase {
    constructor(sousChapitre, chapitre, index) {
        super(sousChapitre);
        this.Ressources = null;
        this.Chapitre = chapitre;
        this.Index = index;
        this.View = new ReferentialFixeView("BIBLIOTHEQUE_PRIX_SOUS_CHAPITRE_ID_" + chapitre.Index + "_" + index);
    }
}

class Ressource extends ReferentialBase {
    constructor(ressource) {
        super(ressource);
        this.RessourceId = ressource.RessourceId;
        this.Items = [];
    }
}

class ReferentialFixeView {
    constructor(viewId) {
        this.ViewId = viewId;
        this._prix = null;
        this.Unite = null;
        this.SnapshotPrix = null;
    }

    get Prix() {
        return this._prix;
    }
    set Prix(value) {
        this._prix = value;
        this.SnapshotPrix = value;
    }
}

class Item {
    constructor(organisationId, ressource) {
        this.OrganisationId = organisationId;
        this.Ressource = ressource;

        let viewId = "BIBLIOTHEQUE_PRIX_ITEM_COMPOSANTE_" + organisationId + "_" + ressource.RessourceId;
        this.Original = new ReferentialFixeView(viewId);
        this.View = new ReferentialFixeView(viewId);
        this.DateModification = null;
        this.DateCreation = null;
    }

    HasChanged() {
        this.View.Prix = Item.GetValue(this.View.Prix);
        return this.View.Prix !== this.Original.Prix
            || this.View.Unite !== null && this.Original.Unite === null
            || this.View.Unite === null && this.Original.Unite !== null
            || this.View.Unite !== null && this.Original.Unite !== null && this.View.Unite.UniteId !== this.Original.Unite.UniteId;
    }

    Saved(date) {
        this.Original.Prix = this.View.Prix;
        this.Original.Unite = this.View.Unite;
        if (this.DateCreation === null) {
            this.DateCreation = date;
        }
        else {
            this.DateModification = date;
        }
    }

    Cancelled() {
        this.View.Prix = this.Original.Prix;
        this.View.Unite = this.Original.Unite;
    }

    // Retourne la bonne valeur des composantes décimales de la vue
    // - viewValue : la valeur dans la vue
    // - return : la bonne valeur
    static GetValue(viewValue) {
        if (viewValue === null || viewValue === '')
            return null;
        return Number(viewValue);
    }
}

class RessourceToUpdate {
    constructor(ressourceId, prix, uniteId) {
        this.RessourceId = ressourceId;
        this.Prix = prix;
        this.UniteId = uniteId;
        this.ChapitreIndex = null;
        this.SousChapitreIndex = null;
        this.RessourceIndex = null;
    }
}
