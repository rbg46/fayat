using System.Diagnostics;

namespace Fred.ImportExport.Business.ApplicationSap
{
    [DebuggerDisplay("IsFound={IsFound} Login={Login} Url={Url}")]
    public class ApplicationSapParameter
    {
        /// <summary>
        /// Permet de savoir si un conf pour la societe ou le groupe a été trouvée.
        /// </summary>
        public bool IsFound
        {
            get
            {
                return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Url) && !string.IsNullOrEmpty(Password);
            }
        }

        /// <summary>
        /// Login 
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Url 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Password 
        /// </summary>
        public string Password { get; set; }
    }
}
