using Fred.Business.Utilisateur;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Web.Modules.Authentification
{
    /// <summary>
    /// Redéfini la façon dont Aps.Net gére les utilisateurs.
    /// Fait le lien entre l'utilisateur connecté sur l'application et celui authorisé dans l'application. 
    /// </summary>
    /// <typeparam name="T">Type User</typeparam>
    public class FayatUserStore<T> : IUserRoleStore<T, int> where T : User
    {
        private readonly IUtilisateurManager utilisateurManager;

        public FayatUserStore(IUtilisateurManager userService)
        {
            utilisateurManager = userService;
        }

        public Task AddToRoleAsync(T user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Aucune ressource à libérer pour le moment
            }
        }

        public Task<T> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(int userId)
        {

            var fayatUser = utilisateurManager.FindById(userId);
            if (fayatUser != null)
            {
                var result = new User()
                {
                    Id = fayatUser.UtilisateurId,
                    FirstName = fayatUser.Prenom,
                    UserName = fayatUser.Nom,
                };
                return Task.FromResult((T)result);
            }
            else
            {
                return Task.FromResult((T)null);
            }

        }

        public Task<T> FindByNameAsync(string userName)
        {
            var task = Task<T>.Run(() =>
            {
                var fayatUser = utilisateurManager.GetByLogin(userName);
                if (fayatUser != null)
                {
                    var result = new User()
                    {
                        Id = fayatUser.Personnel.PersonnelId,
                        FirstName = fayatUser.Prenom,
                        UserName = fayatUser.Nom,
                    };
                    return (T)result;
                }
                else
                {
                    return (T)null;
                }
            });

            return task;
        }

        public Task<IList<string>> GetRolesAsync(T user)
        {
            var roles = new List<string>() { } as IList<string>;
            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(T user, string roleName)
        {

            return Task.FromResult(false);
        }

        public Task RemoveFromRoleAsync(T user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T user)
        {
            throw new NotImplementedException();
        }
    }
}
