using Fred.Entities;

namespace Fred.Business.Action.Models
{
    public class ActionInputModel
    {
        public string JobName { get; set; }

        public ActionStatus? ActionStatus { get; set; }

        public JobActionInputModel JobActionInput { get; set; }

        public ActionType? ActionType { get; set; }

        public string Message { get; set; }

        public int? AuteurId { get; set; }

        public ActionInputModelOptions Options { get; set; } = ActionInputModelOptions.Default;
    }
}
