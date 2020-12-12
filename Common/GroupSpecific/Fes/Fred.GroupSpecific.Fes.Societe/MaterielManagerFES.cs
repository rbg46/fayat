using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;

namespace Fred.Business.Referential.Materiel
{
    public class MaterielManagerFES : MaterielManager
    {
        public MaterielManagerFES(
            IUnitOfWork uow,
            IMaterielRepository materielRepository,
            IUtilisateurManager utilisateurMgr,
            ICIManager ciMgr,
            IValorisationManager valorisationMgr,
            ISepService sepService)
            : base(uow, materielRepository, utilisateurMgr, ciMgr, valorisationMgr, sepService)
        {
        }

        public override void InsertOrUpdate(IEnumerable<MaterielEnt> materiels)
        {
            try
            {
                List<MaterielEnt> materielToUpdate = new List<MaterielEnt>();
                List<MaterielEnt> materielToAdd = new List<MaterielEnt>();
                List<MaterielEnt> existingMateriels = Repository.GetMaterielListAll().ToList();

                materiels.ToList().ForEach(mat =>
                {
                    if (existingMateriels.Exists(y => y.Code == mat.Code && y.EtablissementComptableId == mat.EtablissementComptableId && y.SocieteId == mat.SocieteId))
                    {
                        mat.MaterielId = existingMateriels.Where(e => e.Code == mat.Code && e.SocieteId == mat.SocieteId && e.EtablissementComptableId == mat.EtablissementComptableId).Select(e => e.MaterielId).FirstOrDefault();
                        materielToUpdate.Add(mat);
                    }
                    else
                    {
                        materielToAdd.Add(mat);
                    }
                });

                Repository.InsertRange(materielToAdd);
                Repository.UpdateRange(materielToUpdate);

                Save();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }
    }
}
