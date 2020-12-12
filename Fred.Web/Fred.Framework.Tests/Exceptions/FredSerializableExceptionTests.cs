using Fred.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Fred.Framework.Tests.Exceptions
{
    [TestClass]
    public class FredSerializableExceptionTests
    {
        [TestMethod]
        public void DeserializaFredSerializableException()
        {
            // Arrange
            string input = "{\r\n        \"Message\": \"An error has occurred.\",\r\n        \"ExceptionMessage\": \"[ERREUR SAP] - Response status code does not indicate success: 500 (KO - WS SAP n#0000434682:Erreur de d#termination du poste de commande # r#ceptionner: '44714'.).\",\r\n        \"ExceptionType\": \"Fred.Framework.Exceptions.FredBusinessException\",\r\n        \"StackTrace\": \"   at Fred.ImportExport.Business.ReceptionInterimaire.ReceptionInterimaireFluxManager.<ExportReceptionInterimaireJob>d__24.MoveNext() in C:\\\\dev\\\\Projet FRED\\\\dev\\\\Fred.ImportExport\\\\Fred.ImportExport.Business\\\\ReceptionInterimaire\\\\ReceptionInterimaireFluxManager.cs:line 254\"\r\n    }";

            // Act
            DeserializableFredException result = JsonConvert.DeserializeObject<DeserializableFredException>(input);

            // Assert
            Assert.IsTrue(result.Message == "An error has occurred. - [ERREUR SAP] - Response status code does not indicate success: 500 (KO - WS SAP n#0000434682:Erreur de d#termination du poste de commande # r#ceptionner: '44714'.).");
        }

        [TestMethod]
        public void DeserializaFredSerializableExceptionWithInner()
        {
            // Arrange
            string input = "{\r\n    \"Message\": \"An error has occurred.\",\r\n    \"ExceptionMessage\": \"Une erreur s'est produite lors de l'export du flux RECEPTION_INTERIMAIRE_RZB.\",\r\n    \"ExceptionType\": \"Fred.Framework.Exceptions.FredBusinessException\",\r\n    \"StackTrace\": \"   at Fred.ImportExport.Business.ReceptionInterimaire.ReceptionInterimaireFluxManager.<ExportReceptionInterimaireJob>d__24.MoveNext() in C:\\\\dev\\\\Projet FRED\\\\dev\\\\Fred.ImportExport\\\\Fred.ImportExport.Business\\\\ReceptionInterimaire\\\\ReceptionInterimaireFluxManager.cs:line 274\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.GetResult()\\r\\n   at Fred.ImportExport.Api.Controllers.JobRunnerController.<ExportReceptionInterimaireAsync>d__55.MoveNext() in C:\\\\dev\\\\Projet FRED\\\\dev\\\\Fred.ImportExport\\\\Fred.ImportExport.Api\\\\Controllers\\\\JobRunnerController.cs:line 362\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Threading.Tasks.TaskHelpersExtensions.<CastToObject>d__1`1.MoveNext()\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Web.Http.Controllers.ApiControllerActionInvoker.<InvokeActionAsyncCore>d__1.MoveNext()\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Web.Http.Filters.ActionFilterAttribute.<CallOnActionExecutedAsync>d__6.MoveNext()\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Web.Http.Filters.ActionFilterAttribute.<CallOnActionExecutedAsync>d__6.MoveNext()\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Web.Http.Filters.ActionFilterAttribute.<ExecuteActionFilterAsyncCore>d__5.MoveNext()\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Web.Http.Controllers.ActionFilterResult.<ExecuteAsync>d__5.MoveNext()\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Web.Http.Filters.AuthorizationFilterAttribute.<ExecuteAuthorizationFilterAsyncCore>d__3.MoveNext()\\r\\n--- End of stack trace from previous location where exception was thrown ---\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\\r\\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\\r\\n   at System.Web.Http.Dispatcher.HttpControllerDispatcher.<SendAsync>d__15.MoveNext()\",\r\n    \"InnerException\": {\r\n        \"Message\": \"An error has occurred.\",\r\n        \"ExceptionMessage\": \"[ERREUR SAP] - Response status code does not indicate success: 500 (KO - WS SAP n#0000434964:Erreur de d#termination du poste de commande # r#ceptionner: '44714'.).\",\r\n        \"ExceptionType\": \"Fred.Framework.Exceptions.FredBusinessException\",\r\n        \"StackTrace\": \"   at Fred.ImportExport.Business.ReceptionInterimaire.ReceptionInterimaireFluxManager.<ExportReceptionInterimaireJob>d__24.MoveNext() in C:\\\\dev\\\\Projet FRED\\\\dev\\\\Fred.ImportExport\\\\Fred.ImportExport.Business\\\\ReceptionInterimaire\\\\ReceptionInterimaireFluxManager.cs:line 254\"\r\n    }\r\n}";

            // Act
            DeserializableFredException result = JsonConvert.DeserializeObject<DeserializableFredException>(input);

            // Assert
            Assert.IsTrue(result.Message == "An error has occurred. - Une erreur s'est produite lors de l'export du flux RECEPTION_INTERIMAIRE_RZB. - An error has occurred. - [ERREUR SAP] - Response status code does not indicate success: 500 (KO - WS SAP n#0000434964:Erreur de d#termination du poste de commande # r#ceptionner: '44714'.).");
            Assert.IsTrue(result.InnerException != null);
        }
    }
}
