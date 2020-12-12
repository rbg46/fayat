namespace Fred.Business.Managers
{
    public abstract class SampleManager : Manager<SampleEnt, ISampleRepository>, ISampleManager
    {
        public virtual string GroupCode { get { return Constantes.CodeGroupeDefault; } }

        public virtual void Contract()
        {
            // Do something non custom
        }
    }
}