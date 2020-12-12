using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Fred.Entities.CI;
using Fred.Entities.Personnel;

namespace Fred.Entities.Affectation
{
	 /// <summary>
	 /// Represente une affecattion de <see cref="CIEnt"/>
	 /// </summary>
	 [DebuggerDisplay("AffectationId = {AffectationId} CiId = {CiId} PersonnelId = {PersonnelId} IsDefault = {IsDefault} ")]
	 public class AffectationEnt
	 {
			/// <summary>
			/// Obtient ou definit l'identifiant unique d'une affectation 
			/// </summary>
			public int AffectationId { get; set; }

			/// <summary>
			/// Obtient ou definit le role delegue de l'affectation 
			/// </summary>
			public bool IsDelegue { get; set; }

			/// <summary>
			/// Obtient ou definit l'identifiant unique du CI d'une affectation
			/// </summary>
			public int CiId { get; set; }

			/// <summary>
			/// Obtient ou definit le CI de l'affectation
			/// </summary>
			public CIEnt CI { get; set; }

			/// <summary>
			/// Obtient ou definit l'identifiant unique du personnel
			/// </summary>
			public int PersonnelId { get; set; }

			/// <summary>
			/// Obtient ou definit le personnel affecté
			/// </summary>
			public PersonnelEnt Personnel { get; set; }

			/// <summary>
			/// Child Astreintes where [FRED_ASTREINTE].AffectationId point to yhis Entry (FK_AffectationId)
			/// </summary>
			public ICollection<AstreinteEnt> Astreintes { get; set; } //FRED_ASTREINTE.FK_AffectationId

			/// <summary>
			/// Check if this affectation th default 
			/// </summary>
			public bool IsDefault { get; set; }

            /// <summary>
            /// Check if this affectation is deleted
            /// </summary>
            public bool IsDelete { get; set; }

            /// <summary>
            /// verifier si l'affectation est nouvelle ou modifié
            /// </summary>
            public bool AffectationIsNewOrModified { get; set; }
  }
}
