using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Tests.ModelBuilder
{
    public static class Utilisateur
    {
        public static UtilisateurEnt GetUtilisateurAvecNomEtPrenom(string nom, string prenom)
        {
            return new UtilisateurEnt()
            {

                Personnel = new Entities.Personnel.PersonnelEnt()
                {
                    Prenom = prenom,
                    Nom = nom
                }
            };
        }
    }
}
