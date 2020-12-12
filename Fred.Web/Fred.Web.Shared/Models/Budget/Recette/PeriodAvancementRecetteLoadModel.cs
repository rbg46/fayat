namespace Fred.Web.Shared.Models.Budget.Recette
{
    public class PeriodAvancementRecetteLoadModel
    {
        public int PeriodRequested { get; }
        public AvancementRecetteLoadModel AvancementRecetteLoadModel { get; }
        public PeriodAvancementRecetteLoadModel(int periode, AvancementRecetteLoadModel avancementRecetteLoadModel)
        {
            this.PeriodRequested = periode;
            this.AvancementRecetteLoadModel = avancementRecetteLoadModel;
        }

    }
}
