using System;
using Fred.Entities.Personnel;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    public class PersonnelComparator
	 {

			/// <summary>
			/// Permet de savoir si le personnel existe dans fred
			/// </summary>
			/// <param name="existingPersonnelForSociete">existingPersonnelForSociete</param>
			/// <returns>vrai ou faux</returns>
			public bool ExistInFred(PersonnelEnt existingPersonnelForSociete)
			{
				 return existingPersonnelForSociete != null;
			}


			/// <summary>
			/// Permet de savoier si in personnel a été modifié
			/// </summary>
			/// <param name="oldPersonnel">oldPersonnel</param>
			/// <param name="newPersonnel">newPersonnel</param>
			/// <returns>vrai ou faux</returns>
			public bool IsModified(PersonnelEnt oldPersonnel, PersonnelEnt newPersonnel)
			{
				 bool result = false;
				 if (string.Compare(oldPersonnel.Nom, newPersonnel.Nom) != 0)
				 {
						result = true;
				 }
				 if (string.Compare(oldPersonnel.Prenom, newPersonnel.Prenom) != 0)
				 {
						result = true;
				 }
				 if (oldPersonnel.EtablissementPaieId != newPersonnel.EtablissementPaieId)
				 {
						result = true;
				 }
				 if (DateTime.Compare(oldPersonnel.DateEntree ?? DateTime.MinValue, newPersonnel.DateEntree ?? DateTime.MinValue) != 0)
				 {
						result = true;
				 }
				 if (string.Compare(oldPersonnel.CategoriePerso, newPersonnel.CategoriePerso) != 0)
				 {
						result = true;
				 }
				 if (string.Compare(oldPersonnel.Statut, newPersonnel.Statut) != 0)
				 {
						result = true;
				 }
				 if (oldPersonnel.RessourceId != newPersonnel.RessourceId)
				 {
						result = true;
				 }

				 var dateSortieFredForCompare = oldPersonnel.DateSortie.HasValue ? (DateTime)oldPersonnel.DateSortie : DateTime.MinValue;
				 var dateSortieAnaelForCompare = newPersonnel.DateSortie.HasValue ? (DateTime)newPersonnel.DateSortie : DateTime.MinValue;

				 if (DateTime.Compare(dateSortieFredForCompare, dateSortieAnaelForCompare) != 0)
				 {
						result = true;
				 }

				 if (string.Compare(oldPersonnel.Email, newPersonnel.Email) != 0)
				 {
						return true;
				 }


            return result;
			}


	 }
}
