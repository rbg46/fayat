namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Informations nécessaires au traitement des Dépenses de Type OD qui n'ont pas la bonne Tache (car Tache liée à un mauvais CI)
    /// </summary>
    public class CiIdAndTacheCodeModel
    {
        public int CiId { get; set; }

        public string Code { get; set; }
    }
}
