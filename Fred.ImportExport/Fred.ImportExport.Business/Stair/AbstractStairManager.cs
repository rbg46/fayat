using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.Stair
{
    public abstract class AbstractStairManager
    {
        /// <summary>
        /// Initialise une nouvelle instance.
        /// </summary>
        /// <param name="unitOfWork">L'instance de UnifOfWork</param>
        protected AbstractStairManager()
        {
            UnitOfWork = new UnitOfWorkStair();
        }

        protected UnitOfWorkStair UnitOfWork { get; set; }
    }
}
