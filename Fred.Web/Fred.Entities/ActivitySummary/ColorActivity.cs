namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Couleur de la cellule
    /// </summary>
    public enum ColorActivity
    {
        /// <summary>
        /// Si la valeur est >0 et  inferieur à 5, afficher la valeur et laisser le fond de la case en blanc.
        /// </summary>
        ColorWhite = 0,
        /// <summary>
        /// Si la valeur est >= 5 et inferieur à 10, afficher la valeur et mettre le fond de la case en orange.
        /// </summary>
        ColorOrange = 1,
        /// <summary>
        /// Si la valeur est >=10, afficher la valeur et mettre le fond de la case en rouge.
        /// </summary>
        ColorRed = 2,

        /// <summary>
        /// Couleur pour le reste
        /// </summary>
        ColorBlue = 3
    }
}
