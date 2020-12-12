using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    /// Helper pour nettoyer le personnel et le materiel sur un pointage
    /// </summary>
    public static class PointageClearPersonnelAnMaterielHelper
    {
        /// <summary>
        /// Nettoie le personnel d'un pointage
        /// </summary>
        /// <param name="pointage">Le pointage a nettoyer</param>
        public static void ClearPersonnel(RapportLigneEnt pointage)
        {
            if (pointage.Personnel != null && pointage.PersonnelId != null)
            {
                if (pointage.Personnel.LibelleRef == pointage.PrenomNomTemporaire)
                {
                    pointage.PrenomNomTemporaire = null;
                }
                else
                {
                    pointage.Personnel = null;
                    pointage.PersonnelId = null;
                }
            }
        }

        /// <summary>
        /// Nettoie le materiel du pointage
        /// </summary>
        /// <param name="pointage">Le pointage a nettoyer</param>
        public static void ClearMateriel(RapportLigneEnt pointage)
        {
            if (pointage.Materiel != null && pointage.MaterielId != null)
            {
                if (pointage.Materiel.Libelle == pointage.MaterielNomTemporaire || pointage.Materiel.LibelleLong == pointage.MaterielNomTemporaire)
                {
                    pointage.MaterielNomTemporaire = null;
                }
                else
                {
                    pointage.Materiel = null;
                    pointage.MaterielId = null;
                }
            }
        }
    }
}
