using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel;

namespace Fred.Business.Rapport.Datas
{
    /// <summary>
    /// Représente les données utilisées pour la récupération d'un rapport hebdo.
    /// </summary>
    public class RapportHebdoByEmployee
    {
        #region Personnel

        /// <summary>
        /// Représente un personnel.
        /// </summary>
        public class Personnel
        {
            /// <summary>
            /// Selector permettant de constuire cet objet à partir d'une entité.
            /// </summary>
            public static Expression<Func<PersonnelEnt, Personnel>> Selector
            {
                get
                {
                    return personelEnt => new Personnel
                    {
                        SocieteCode = personelEnt.Societe != null ? personelEnt.Societe.Code : null,
                        Matricule = personelEnt.Matricule,
                        Nom = personelEnt.Nom,
                        Prenom = personelEnt.Prenom,
                        Statut = personelEnt.Statut,
                        SocieteId = personelEnt.SocieteId
                    };
                }
            }

            /// <summary>
            /// Le code de la société.
            /// </summary>
            public string SocieteCode { get; set; }

            /// <summary>
            /// Le matricule.
            /// </summary>
            public string Matricule { get; set; }

            /// <summary>
            /// Le nom.
            /// </summary>
            public string Nom { get; set; }

            /// <summary>
            /// Le prénom.
            /// </summary>
            public string Prenom { get; set; }

            /// <summary>
            /// Le statut.
            /// </summary>
            public string Statut { get; set; }

            /// <summary>
            /// L'identifiant de la société.
            /// </summary>
            public int? SocieteId { get; set; }
        }

        #endregion
        #region CI

        /// <summary>
        /// Représente un CI.
        /// </summary>
        [DebuggerDisplay("{Code} (Id: {CiId})")]
        public class CI
        {
            /// <summary>
            /// Selector permettant de constuire cet objet à partir d'une entité.
            /// </summary>
            public static Expression<Func<CIEnt, CI>> Selector
            {
                get
                {
                    return ciEnt => new CI
                    {
                        CiId = ciEnt.CiId,
                        SocieteId = ciEnt.SocieteId,
                        TypeDesignation = ciEnt.CIType != null ? ciEnt.CIType.Designation : null,
                        Code = ciEnt.Code,
                        Libelle = ciEnt.Libelle,
                        Organisation = ciEnt.Organisation,
                        IsAbsence = ciEnt.IsAbsence
                    };
                }
            }

            /// <summary>
            /// L'identifiant du CI.
            /// </summary>
            public int CiId { get; set; }

            /// <summary>
            /// L'identifiant de la société.
            /// </summary>
            public int? SocieteId { get; set; }

            /// <summary>
            /// La désignation du type du CI.
            /// </summary>
            public string TypeDesignation { get; set; }

            /// <summary>
            /// Le code.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Le libellé.
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            /// Organisation identifier
            /// </summary>
            public OrganisationEnt Organisation { get; set; }

            /// <summary>
            /// flag pour ci absence generique
            /// </summary>
            public bool IsAbsence { get; set; }

            /// <summary>
            /// Retourne un CI en fonction de l'entité correspondante.
            /// </summary>
            /// <param name="ciEnt">L'entité concernée.</param>
            /// <returns>Le CI correspondant.</returns>
            public static CI From(CIEnt ciEnt)
            {
                return new CI
                {
                    CiId = ciEnt.CiId,
                    SocieteId = ciEnt.SocieteId,
                    TypeDesignation = ciEnt.CIType?.Designation,
                    Code = ciEnt.Code,
                    Libelle = ciEnt.Libelle,
                    Organisation = ciEnt.Organisation,
                    IsAbsence = ciEnt.IsAbsence
                };
            }
        }

        #endregion
    }
}
