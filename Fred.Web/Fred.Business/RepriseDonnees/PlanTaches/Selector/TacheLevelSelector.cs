namespace Fred.Business.RepriseDonnees.PlanTaches.Selector
{
    /// <summary>
    /// Selection du niveau
    /// </summary>
    public class TacheLevelSelector
    {
        /// <summary>
        /// Convertie une chaine en Niveau
        /// </summary>
        /// <param name="niveau">T1/T2/T3</param>
        /// <returns>null si non convertie sinon un entier correspondant au niveau</returns>
        public int? GetTacheNiveau(string niveau)
        {
            int? result = null;
            var niveauUpper = niveau.ToUpper();
            switch (niveauUpper)
            {
                case "T1":
                    result = 1;
                    break;
                case "T2":
                    result = 2;
                    break;
                case "T3":
                    result = 3;
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}
