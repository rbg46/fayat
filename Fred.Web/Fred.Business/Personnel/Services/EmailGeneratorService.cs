using System.Collections.Generic;
using System.Text;

namespace Fred.Business.Personnel
{
    public class EmailGeneratorService : IEmailGeneratorService
    {
        private static Dictionary<int, string> _societeIdDomaine;

        private static Dictionary<int, string> SocieteIdDomaine => _societeIdDomaine ?? (_societeIdDomaine = InitSocieteIdDomaine());

        private static Dictionary<int, string> InitSocieteIdDomaine()
        {
            // SocieteId -- Domaine
            return new Dictionary<int, string>()
            {
                {1, "@razel-bec.fayat.com"},
                {72, "@moulin-btp.fr"}
            };
        }

        public string GenerateEmail(string nom, string prenom, string codeSocietePaye, int societeId)
        {
            if (SocieteIdDomaine.ContainsKey(societeId))
            {
                var domaine = SocieteIdDomaine[societeId];
                if (codeSocietePaye == "MTP")
                {
                    var newPrenom = prenom.Replace(" ", string.Empty);
                    var newNom = nom.Replace(" ", string.Empty);
                    return newPrenom.ToLower() + "." + newNom.ToLower() + domaine;
                }
                else if (codeSocietePaye == "RZB")
                {
                    string initialPrenom = GestionPrenom(prenom);
                    string newNom = GestionNom(nom);
                    return initialPrenom + "." + newNom + domaine;
                }
            }

            return null;
        }


        private string GestionPrenom(string prenom)
        {
            string[] prenoms = prenom.Split(new char[] { '-', ' ' });

            var initialPrenom = new StringBuilder();
            if (prenoms.Length > 1)
            {
                foreach (string s in prenoms)
                {
                    initialPrenom.Append(s, 0, 1);
                }
            }
            else
            {
                initialPrenom.Append(prenom, 0, 1);
            }

            return initialPrenom.ToString().ToLower();
        }

        private string GestionNom(string nom)
        {
            string[] noms = nom.Split(new char[] { '-', ' ', '\'' });

            var concatNoms = new StringBuilder();
            if (noms.Length > 1)
            {
                foreach (string s in noms)
                {
                    concatNoms.Append(s);
                }
            }
            else
            {
                concatNoms.Append(nom);
            }

            return concatNoms.ToString().ToLower();
        }

    }

}
