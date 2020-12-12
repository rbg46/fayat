using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Personnel;

namespace Fred.Business.Moyen.Common
{
    /// <summary>
    /// Cette contient le résultat du process d'une affectation de moyen à un personnel
    /// </summary>
    public class ProcessAffectationPersonnelResponse : ProcessAffectationResponse
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ProcessAffectationPersonnelResponse() { }

        /// <summary>
        /// Les personnel n'ayant pas de pointage 
        /// </summary>
        public PersonnelPointageError PersonnelPointageError { get; set; }

        /// <summary>
        /// Ajour d'erreur d'absence de pointage à une date
        /// </summary>
        /// <param name="personnel">Personnel entité</param>
        /// <param name="date">Date de pointage</param>
        public void AddPersonnelErrorDate(PersonnelEnt personnel, DateTime date)
        {
            if (PersonnelPointageError == null)
            {
                PersonnelPointageError = new PersonnelPointageError
                {
                    Dates = new List<DateTime> { date },
                    Personnel = new PersonnelMoyenErrorModel
                    {
                        PersonnelId = personnel.PersonnelId,
                        PersonnelRef = personnel.NomPrenom
                    }
                };
            }
            else
            {
                bool isDateExists = PersonnelPointageError.Dates.Any(d => d.Date == date.Date);
                if (!isDateExists)
                {
                    PersonnelPointageError.Dates.Add(date);
                }
            }
            
        }
    }
}
