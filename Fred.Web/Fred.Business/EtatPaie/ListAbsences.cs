using System.Collections.Generic;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Classe gérant les ListAbsences
    /// </summary>
    public class ListAbsences
    {
        /// <summary>
        /// Liste des Absences
        /// </summary>
        public List<Absence> ListAbsence { get; set; } = new List<Absence>();

        /// <summary>
        /// Récupère le nombre d'Absences contenu dans la liste
        /// </summary>
        public int Count
        {
            get
            {
                if (ListAbsence != null)
                {
                    return ListAbsence.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Renvoi l'absence recherché 
        /// </summary>
        /// <param name="code">Code de l'absence recherché</param>
        /// <returns>Retourne une Absence</returns>
        public Absence Get(string code)
        {
            foreach (var absence in ListAbsence)
            {
                if (absence.CodeAbsence == code)
                {
                    return absence;
                }
            }

            return null;
        }

        /// <summary>
        /// Renvoi l'absence recherché 
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="ciLibelle">libelle de ci</param>
        /// <returns>Retourne une Absence</returns>
        public Absence Get(string code, string ciLibelle)
        {
            foreach (var absence in ListAbsence)
            {
                if (absence.CodeAbsence == code && absence.CILibelle == ciLibelle)
                {
                    return absence;
                }
            }

            return null;
        }

        /// <summary>
        /// Vérifie la présence d'une absence dans la liste
        /// </summary>
        /// <param name="code">Code de l'absence recherché</param>
        /// <returns>Retourne vrai si la liste contient une Absence avec le code recherché</returns>
        public bool Contains(string code)
        {
            foreach (var deplacement in ListAbsence)
            {
                if (deplacement.Contains(code))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///  Vérifie la présence d'une absence dans la liste
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="ciLibelle">libelle de ci</param>
        /// <returns>Retourne vrai si la liste contient une Absence avec le code et CI recherché</returns>
        public bool Contains(string code, string ciLibelle)
        {
            foreach (var deplacement in ListAbsence)
            {
                if (deplacement.Contains(code, ciLibelle))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Ajoute une Absence à la liste
        /// </summary>
        /// <param name="absence">Absence à ajoutée</param>
        public void Add(Absence absence)
        {
            if (ListAbsence != null)
            {
                ListAbsence.Add(absence);
            }
        }

        /// <summary>
        /// Tri la liste
        /// </summary>
        public void Sort()
        {
            if (ListAbsence != null)
            {
                ListAbsence.Sort();
            }
        }
    }
}
