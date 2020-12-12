namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Représente le "sens" de l'arbre 
    /// </summary>
    public enum AxePrincipal
    {
        /// <summary>
        /// TacheRessource définit l'axe principal comme étant la tache et le secondaire étant la ressource
        /// </summary>
        TacheRessource,

        /// <summary>
        /// RessourceTache définit l'axe principal comme étant la ressource et le secondaire étant la tache
        /// </summary>
        RessourceTache,

        /// <summary>
        /// TachesOnly indique que le controle budgétaire ne contiendra que des taches  
        /// </summary>
        TacheOnly,

        /// <summary>
        /// RessourceOnly indique que le controle budgétaire ne contiendra que des ressources (et chapitre et sous chapitre)
        /// </summary>
        RessourceOnly
    }

    /// <summary>
    /// Décrit les différents niveau des branches de l'arbre
    /// </summary>
    public enum AxeTypes
    {
        /// <summary>
        /// Niveau T1
        /// </summary>
        T1,

        /// <summary>
        /// Niveau T2
        /// </summary>
        T2,

        /// <summary>
        /// Niveau T3, peut être le dernier niveau si l'axe principal choisi est l'axe Ressource
        /// </summary>
        T3,

        /// <summary>
        /// Niveau Chapitre
        /// </summary>
        Chapitre,

        /// <summary>
        /// Niveau Sous Chapitre
        /// </summary>
        SousChapitre,

        /// <summary>
        /// Niveau ressource, peut être le dernier niveau si l'axe principal choisi est l'axe Tache
        /// </summary>
        Ressource
    }
}
