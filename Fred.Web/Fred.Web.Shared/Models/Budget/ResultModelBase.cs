namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Représente la base des models transmis du back au front.
    /// </summary>
    public abstract class ResultModelBase
    {
#if DEBUG

#pragma warning disable CA1707

        // Type doit être préfixé par un underscore ici pour ne pas empêcher
        //   un model de créer une propriété "Type" sans underscore.

        /// <summary>
        /// Permet coté front de savoir de quel type C# est le model retourné.
        /// </summary>
        public string _Type { get; private set; }

#pragma warning restore CA1707

        protected ResultModelBase()
        {
            _Type = GetType().ToString();
        }

#endif
    }
}
