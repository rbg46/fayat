using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.Budget.Avancement;
using static Fred.Entities.Constantes;

namespace Fred.Business.Tests.ModelBuilder
{
    internal static class AvancementEtat
    {
        public static AvancementEtatEnt GetFakeAvancementEtatEnregistre()
        {
            return new AvancementEtatEnt
            {
                Code = "ER",
                Libelle = "Enregistre",
                AvancementEtatId = 1
            };
        }

        public static AvancementEtatEnt GetFakeAvancementAValider()
        {
            return new AvancementEtatEnt
            {
                Code = "AV",
                Libelle = "AValider",
                AvancementEtatId = 2
            };
        }

        public static AvancementEtatEnt GetFakeAvancementEtatValide()
        {

            return new AvancementEtatEnt
            {
                Code = "VA",
                Libelle = "Valide",
                AvancementEtatId = 3
            };

        }

        public static AvancementEtatEnt GetFakeAvancementEtatByCode(string code)
        {
            switch (code)
            {
                case EtatAvancement.Enregistre:
                    return GetFakeAvancementEtatEnregistre();
                case EtatAvancement.AValider:
                    return GetFakeAvancementAValider();
                case EtatAvancement.Valide:
                    return GetFakeAvancementEtatValide();
                default:
                    throw new ArgumentException("code inconnu");
            }
        }
    }
}
