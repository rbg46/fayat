using AutoMapper;
using Fred.Business.Notification;
using Fred.Entities.Notification;
using Fred.Web.Models.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des Notifications
    /// </summary>
    public class NotificationController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly INotificationManager notificationManager;

        public NotificationController(INotificationManager notificationManager, IMapper mapper)
        {
            this.notificationManager = notificationManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get notifications for the current user
        /// </summary>
        /// <returns>Notification list</returns>
        [HttpGet]
        [Route("api/Notification/")]
        public async Task<IHttpActionResult> Get()
        {
            IEnumerable<NotificationEnt> notifications = await notificationManager.GetUserUnreadNotificationsAsync();

            return Ok(mapper.Map<IEnumerable<NotificationModel>>(notifications));
        }

        /// <summary>
        /// Mark the specified notified as read
        /// </summary>
        /// <param name="utilisateurId">Utilisateur identifiant</param>
        /// <returns>True if the method has been executed</returns>
        [HttpPost]
        [Route("api/Notification/MarkAsRead/{utilisateurId}")]
        public async Task<IHttpActionResult> MarkAsRead(int utilisateurId)
        {
            await notificationManager.MarkUserNotificationsAsReadAsync(utilisateurId);

            return Created(string.Empty, true);
        }

        /// <summary>
        /// Récuperer le nombre des notifications non lues
        /// </summary>
        /// <param name="utilisateurId">Utilisateur identifiant</param>
        /// <returns>Nombre des notifications non lues</returns>
        [HttpGet]
        [Route("api/Notification/GetUnreadableNotificationsNumberByUserId/{utilisateurId}")]
        public async Task<IHttpActionResult> GetUnreadableNotificationsNumberByUserId(int utilisateurId)
        {
            int notificationCount = await notificationManager.GetUserUnreadNotificationCountAsync(utilisateurId);

            return Ok(notificationCount);
        }

        /// <summary>
        /// Supprimer tous les notifications d'un utilisateur
        /// </summary>
        /// <param name="utilisateurId">Utilisateur identifiant</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Notification/DeleteAllNotificationsByUtilisateurId/{utilisateurId}")]
        public async Task<IHttpActionResult> DeleteAllNotificationsByUtilisateurId(int utilisateurId)
        {
            await notificationManager.DeleteUserNotificationsAsync(utilisateurId);

            return Created(string.Empty, true);
        }

        /// <summary>
        /// Supprimer une list des notifications 
        /// </summary>
        /// <param name="notifications">Utilisateur identifiant</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Notification/DeleteNotifications/")]
        public async Task<IHttpActionResult> DeleteNotifications(IEnumerable<NotificationModel> notifications)
        {
            var notificationEntities = mapper.Map<IEnumerable<NotificationEnt>>(notifications);
            await notificationManager.DeleteNotificationsAsync(notificationEntities);

            return Created(string.Empty, true);
        }

        /// <summary>
        /// Rechercher les notifications
        /// </summary>
        /// <param name="utilisateurId">Utilisateur identifiant</param>
        /// <param name="searchText">text a chercher</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Notification/SearchLight/{utilisateurId}/{searchText?}")]
        public async Task<IHttpActionResult> SearchLight(int utilisateurId, string searchText = "")
        {
            IEnumerable<NotificationEnt> notifications = await notificationManager.FindUserNotificationsAsync(utilisateurId, searchText);

            return Ok(mapper.Map<IEnumerable<NotificationModel>>(notifications));
        }
    }
}