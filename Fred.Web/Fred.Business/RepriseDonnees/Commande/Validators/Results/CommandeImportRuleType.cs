namespace Fred.Business.RepriseDonnees.Commande.Validators.Results
{
    /// <summary>
    /// type de regle à verifiée
    /// </summary>
    public enum CommandeImportRuleType
    {

        /// <summary>
        /// Vérification de la cohérence des informations d’en-tête de chaque commande 
        /// </summary>
        CoherenceInformationEntete = 0,
        /// <summary>
        /// Vérification de l’unicité des numéros externes des commandes 
        /// </summary>
        UniciteNumeroExterne = 1,
        /// <summary>
        /// Vérification des champs obligatoires 
        /// </summary>
        RequiredField = 2,
        /// <summary>
        /// Transco à appliquer sur la valeur "Type Commande" :
        ///  - "Fourniture" => 1
        ///  - "Location" => 2
        ///  - "Prestation" => 3
        ///  - "Intérimaire" => 4
        /// Non reconnu => rejet de la ligne
        /// </summary>
        TypeCommandeInvalide = 3,

        /// <summary>
        /// Rechercher la valeur "Code Fournisseur" dans la table FRED_FOURNISSEUR
        /// Non reconnu => rejet de la ligne
        /// </summary>
        CodeFournisseurInvalide = 4,
        /// <summary>
        /// le ci n'est pas dans la societe
        /// </summary>
        CiNotInSociete = 5,
        /// <summary>
        /// A positionner avec la date du champ "Date Commande"
        /// Format non valide = rejet de la ligne
        /// </summary>
        DateCommandeInvalid = 6,
        /// <summary>
        /// Rechercher la valeur "Code Devise" dans la table FRED_DEVISE
        /// Non reconnu = rejet de la ligne
        /// </summary>
        DeviseInvalide = 7,
        /// <summary>
        /// Si la valeur "Code Tâche" est non vide : 
        /// rechercher cette valeur dans la table FRED_TACHE, parmi les tâches de niveau 3 associées au CI identifié pour la commande.
        /// Si non reconnu => rejet de la ligne.
        /// Si la valeur "Code Tâche" est vide, rechercher la tâche par défaut du CI identifié pour la commande.
        /// Si non trouvé => rejet de la ligne.
        /// </summary>
        CodeTacheForCommandeLigneInvalid = 8,
        /// <summary>
        /// Rechercher la valeur "Code Ressource" dans la table FRED_RESSOURCE, 
        /// parmi les ressources associées au Groupe choisi par l'utilisateur.
        /// Si non reconnu => rejet de la ligne.
        /// </summary>
        RessourceForCommandeLigneInvalid = 9,
        /// <summary>
        /// QuantiteReceptionnee format invalide       
        /// </summary>
        QuantiteReceptionneeForCommandeLigneInvalid = 10,
        /// <summary>
        /// QuantiteCommandee format invalide       
        /// </summary>
        QuantiteCommandeeForCommandeLigneInvalid = 11,
        /// <summary>
        /// Positionner à la valeur "Qté commandée" – "Qté facturée rapprochée" si "Qté commandée" – "Qté facturée rapprochée" > 0
        /// Sinon, positionner à la valeur "Qté réceptionnée" – "Qté facturée rapprochée" si "Qté réceptionnée" – "Qté facturée rapprochée" > 0
        /// Sinon rejet de la ligne.
        /// </summary>
        QuantiteForCommandeLigneInvalid = 12,
        /// <summary>
        /// QuantiteFacturee format invalide  
        /// </summary>
        QuantiteFactureeRapprocheeForCommandeLigneInvalid = 13,
        /// <summary>
        /// Positionner à la valeur du champ "PU HT"
        /// Si format non valide = rejet de la ligne.
        /// </summary>
        PuhtForCommandeLigneInvalid = 14,
        /// <summary>
        /// Rechercher la valeur "Code Unité" dans la table FRED_UNITE.
        /// Si non reconnu = rejet de la ligne.
        /// </summary>
        UniteForCommandeLigneInvalid = 15,
        /// <summary>
        /// A positionner avec la date du champ "Date Réception"
        /// Format non valide => rejet de la ligne
        /// </summary>
        DateReceptionForReceptionInvalid = 16,
        /// <summary>
        /// Pattern du NumeroCommandeExterne est invalide
        /// </summary>
        PatternNumeroCommandeExterne = 17,

        /// <summary>
        /// Le code de la societe n'est pas dans connu pour le groupe
        /// </summary>
        SocieteNotInGroupe = 18,


    }
}
