using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ReferentielTachesIndex
  /// </summary>
  public class ReferentielTachesIndexBundleFactory : BundleFactory
  {
    public ReferentielTachesIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ReferentielTachesIndexBundle.js");
      Js("~/Scripts/Module/Notification/module.js",
           "~/Areas/ReferentielTaches/Scripts/referentieltaches.service.js",
           "~/Areas/ReferentielTaches/Scripts/ReferentielTachesComponent.js",
           "~/Areas/ReferentielTaches/Scripts/filterCodeAndLibelle.filter.js",
           "~/Areas/ReferentielTaches/Scripts/filterCodeAndLabelAllLevel.filter.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-one-create-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-one-edit-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-two-create-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-two-edit-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-three-create-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-three-edit-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-four-create-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/Dialogs/task-four-edit-modal-controller.js",
           "~/Areas/ReferentielTaches/Scripts/referentiel-tache-view-selector.component.js",
           "~/Areas/ReferentielTaches/Scripts/TaskManagerService.js",
           "~/Areas/ReferentielTaches/Scripts/filterCodeAndLibelle.filter.js",
           "~/Areas/ReferentielTaches/Scripts/filterCodeAndLabelAllLevel.filter.js");

      CssBundleName("~/ReferentielTachesIndexBundle.css");
      Css("~/Content/module/Notification/style.css",
           "~/Areas/ReferentielTaches/Content/style.css",
           "~/Content/ReferentialPickList/ReferentialPickList.css");
    }
  }
}

