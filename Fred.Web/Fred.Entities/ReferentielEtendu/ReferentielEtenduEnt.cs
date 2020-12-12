using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.RessourcesRecommandees;
using Fred.Entities.Societe;

namespace Fred.Entities.ReferentielEtendu
{
    /// <summary>
    ///   Représente une association entre une société, une ressource et une nature
    /// </summary>
    [DebuggerDisplay("ReferentielEtenduId = {ReferentielEtenduId} SocieteId = {SocieteId} RessourceId = {RessourceId} NatureId = {NatureId}")]

    public class ReferentielEtenduEnt : ICloneable
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'entité.
        /// </summary>
        public int ReferentielEtenduId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Société.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Nature.
        /// </summary>
        public int? NatureId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société
        /// </summary>
        public virtual SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit le ressource
        /// </summary>
        public virtual RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit le nature
        /// </summary>
        public virtual NatureEnt Nature { get; set; }

        /// <summary>
        ///   Obtient ou définit si la ressource concerne les achats
        /// </summary>
        public virtual bool Achats { get; set; } = false;

        /// <summary>
        /// Obtient ou défini la liste abrégée des unités
        /// </summary>
        public virtual string ListeUnitesAbregees { get; set; }

        /// <summary>
        /// Obtient ou définit le paramétrage associé au référentiel étendu
        /// </summary>
        public ICollection<ParametrageReferentielEtenduEnt> ParametrageReferentielEtendus { get; set; }

        /// <summary>
        /// Obtient ou définit les unités associé au référentiel étendu
        /// </summary>
        public ICollection<UniteReferentielEtenduEnt> UniteReferentielEtendus { get; set; }

        /// <summary>
        /// Gets or sets the ressources recommandees.
        /// </summary>
        /// <value>
        /// The ressources recommandees.
        /// </value>
        public ICollection<RessourceRecommandeeEnt> RessourcesRecommandees { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            ReferentielEtenduEnt newRefEtendu = (ReferentielEtenduEnt)this.MemberwiseClone();

            if (this.ParametrageReferentielEtendus.Any())
            {
                newRefEtendu.ParametrageReferentielEtendus = new List<ParametrageReferentielEtenduEnt>();

                foreach (var paramRefEtendu in this.ParametrageReferentielEtendus)
                {
                    newRefEtendu.ParametrageReferentielEtendus.Add((ParametrageReferentielEtenduEnt)paramRefEtendu.Clone());
                }
            }

            return newRefEtendu;
        }

        /// <summary>
        /// Clean - retire toutes les dépendances pour insertion en base
        /// </summary>
        public void Clean()
        {
            this.Ressource = null;
            this.Societe = null;
            this.Nature = null;
            this.RessourceId = 0;
            this.ReferentielEtenduId = 0;

            if (this.ParametrageReferentielEtendus.Any())
            {
                foreach (var paramRefEtendu in this.ParametrageReferentielEtendus)
                {
                    paramRefEtendu.Clean();
                }
            }
        }

        /// <summary>
        /// Met à jour le libellé abrégé du référentiel étendu
        /// </summary>
        public void AffectListeUnitesAbregees()
        {
            var libelle = string.Empty;
            if (this.UniteReferentielEtendus != null && this.UniteReferentielEtendus.Count > 0)
            {
                if (this.UniteReferentielEtendus.Count == 1)
                {
                    libelle = this.UniteReferentielEtendus.ToList()[0].Unite.Code;
                }
                else if (this.UniteReferentielEtendus.Count == 2)
                {
                    libelle = string.Concat(this.UniteReferentielEtendus.ToList()[0].Unite.Code, ", ", this.UniteReferentielEtendus.ToList()[1].Unite.Code);
                }
                else if (this.UniteReferentielEtendus.Count == 3)
                {
                    libelle = string.Concat(this.UniteReferentielEtendus.ToList()[0].Unite.Code, ", ", this.UniteReferentielEtendus.ToList()[1].Unite.Code, ", ", this.UniteReferentielEtendus.ToList()[2].Unite.Code);
                }
                else if (this.UniteReferentielEtendus.Count > 3)
                {
                    libelle = string.Concat(this.UniteReferentielEtendus.ToList()[0].Unite.Code, ", ", this.UniteReferentielEtendus.ToList()[1].Unite.Code, ", ", this.UniteReferentielEtendus.ToList()[2].Unite.Code, ", ...");
                }
            }
            ListeUnitesAbregees = libelle;
        }
    }
}
