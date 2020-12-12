namespace Fred.Business
{
    /// <summary>
    /// Donne l'accès aux managers.
    /// </summary>
    public class ManagersAccess
    {
        private Managers managers;

        /// <summary>
        /// Obtient tous les managers.
        /// </summary>
        protected Managers Managers
        {
            get
            {
                if (managers == null)
                {
                    managers = new Managers();
                }
                return managers;
            }
            set
            {
                managers = value;
            }
        }
    }
}
