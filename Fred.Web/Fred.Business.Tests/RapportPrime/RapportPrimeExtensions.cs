using System;
using System.Collections.Generic;
using System.Text;
using Fred.Entities.RapportPrime;

namespace Fred.Business.Tests.RapportPrime
{
    public static class RapportPrimeExtensions
    {
        public static void TruncateDates(this RapportPrimeEnt rapportPrime)
        {
            rapportPrime.DateCreation = rapportPrime.DateCreation.Value.Date;
            rapportPrime.DateRapportPrime = rapportPrime.DateRapportPrime.Date;
        }
    }
}
