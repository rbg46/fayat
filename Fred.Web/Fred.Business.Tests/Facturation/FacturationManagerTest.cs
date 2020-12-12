using System;
using FluentAssertions;
using Fred.Business.Facturation;
using Fred.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Facturation
{
    [TestClass]
    public class FacturationManagerTest : BaseTu<FacturationManager>
    {
        [TestMethod]
        [TestCategory("FacturationManager")]
        public void MethodBulkInsert_WithNullList_ThrowException()
        {
            Invoking(() => Actual.BulkInsert(null)).Should().Throw<ArgumentNullException>();
        }
    }
}
