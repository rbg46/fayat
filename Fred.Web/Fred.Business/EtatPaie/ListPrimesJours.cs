using System.Collections.Generic;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Classe gérant les ListPrimesJours
    /// </summary>
    public class ListPrimesJours
    {
        /// <summary>
        /// Liste des PrimesJours
        /// </summary>
        public List<PrimeJours> ListPrimeJours { get; set; } = new List<PrimeJours>();

        /// <summary>
        /// Récupère le nombre de PrimesJours contenu dans la liste
        /// </summary>
        public int Count
        {
            get
            {
                if (ListPrimeJours != null)
                {
                    return ListPrimeJours.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Renvoi la prime recherché 
        /// </summary>
        /// <param name="code">Code de la prime recherché</param>
        /// <returns>Retourne une PrimeJours</returns>
        public PrimeJours Get(string code)
        {
            foreach (var primeJour in ListPrimeJours)
            {
                if (primeJour.CodePrime == code)
                {
                    return primeJour;
                }
            }

            return null;
        }

        /// <summary>
        /// Vérifie la présence d'une prime dans la liste
        /// </summary>
        /// <param name="code">Code de la prime recherché</param>
        /// <returns>Retourne vrai si la liste contient une PrimeJours avec le code recherché</returns>
        public bool Contains(string code)
        {
            foreach (var primeJour in ListPrimeJours)
            {
                if (primeJour != null && primeJour.Contains(code))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Ajoute une PrimeJours à la liste
        /// </summary>
        /// <param name="primeJours">PrimeJours à ajoutée</param>
        public void Add(PrimeJours primeJours)
        {
            if (ListPrimeJours != null)
            {
                ListPrimeJours.Add(primeJours);
            }
        }

        /// <summary>
        /// Tri la liste
        /// </summary>
        public void Sort()
        {
            if (ListPrimeJours != null)
            {
                ListPrimeJours.Sort();
            }
        }
    }
}
