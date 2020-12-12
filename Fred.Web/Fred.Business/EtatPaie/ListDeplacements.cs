using System.Collections.Generic;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Classe gérant les ListDeplacements
    /// </summary>
    public class ListDeplacements
    {
        /// <summary>
        /// Liste des Déplacements
        /// </summary>
        public List<Deplacements> ListDeplacement { get; set; } = new List<Deplacements>();

        /// <summary>
        /// Récupère le nombre de déplacements contenu dans la liste
        /// </summary>
        public int Count
        {
            get
            {
                if (ListDeplacement != null)
                {
                    return ListDeplacement.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Renvoi le déplacement recherché 
        /// </summary>
        /// <param name="code">Code du déplacement recherché</param>
        /// <returns>Retourne un Deplacement</returns>
        public Deplacements Get(string code)
        {
            foreach (var deplacement in ListDeplacement)
            {
                if (deplacement.CodeDeplacement == code)
                {
                    return deplacement;
                }
            }

            return null;
        }

        /// <summary>
        /// Vérifie la présence d'un déplacement dans la liste
        /// </summary>
        /// <param name="code">Code du déplacement recherché</param>
        /// <returns>Retourne vrai si la liste contient un Deplacement avec le code recherché</returns>
        public bool Contains(string code)
        {
            foreach (var deplacement in ListDeplacement)
            {
                if (deplacement.Contains(code))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Ajoute un Deplacement à la liste
        /// </summary>
        /// <param name="deplacements">Deplacement à ajoutée</param>
        public void Add(Deplacements deplacements)
        {
            if (ListDeplacement != null)
            {
                ListDeplacement.Add(deplacements);
            }
        }

        /// <summary>
        /// Tri la liste
        /// </summary>
        public void Sort()
        {
            if (ListDeplacement != null)
            {
                ListDeplacement.Sort();
            }
        }
    }
}
