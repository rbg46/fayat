using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Extentions;
using System.Linq;

namespace Fred.Business.Moyen.Common
{
    /// <summary>
    /// Cette contient les différents erreurs qu'on peut avoir lors de la génération du pointage des moyens
    /// </summary>
    public class PointageMoyenResponse
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public PointageMoyenResponse()
        {
            PersonnelListNotRegistred = new List<PersonnelPointageError>();
        }

        /// <summary>
        /// Errreur générale qui peut parvenir lors de la génération
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// La liste des personnels qui n'ont pas de pointage dans l'intervalle de génération
        /// </summary>
        public List<PersonnelPointageError> PersonnelListNotRegistred { get; set; }

        /// <summary>
        /// Le cumul des erreurs du personnel
        /// </summary>
        /// <param name="personnelError">Personnel pointage error</param>
        public void AddPersonnelErrors(PersonnelPointageError personnelError)
        {
            if (personnelError == null)
            {
                return;
            }

            PersonnelPointageError personnel = PersonnelListNotRegistred.FirstOrDefault(v => v.Personnel.PersonnelId == personnelError.Personnel.PersonnelId);
            if (personnel != null)
            {
                IEnumerable<DateTime> newDates = personnelError.Dates.Where(a => !personnel.Dates.Any(j => j.Date.Date == a.Date.Date));
                if (!newDates.IsNullOrEmpty())
                {
                    personnel.Dates.AddRange(newDates);
                }
            }
            else
            {
                PersonnelListNotRegistred.Add(new PersonnelPointageError
                {
                    Personnel = personnelError.Personnel,
                    Dates = personnelError.Dates
                });
            }
        }
    }
}
