using FluentValidation;

namespace Fred.Business
{
    /// <summary>
    /// Base class for entity validator classes.
    /// Provides access to managers.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated</typeparam>
    public abstract class AbstractValidatorWithManagersAccess<T> : AbstractValidator<T>
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
        }
    }
}
