using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using EFCore.Sample.Business.Models;
using EFCore.Sample.ORM.Context;
using EFCore.Sample.ORM.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Sample.NUnitTest
{
    public class Tests
    {
        [TestFixture]
        public class UsingSqliteInMemoryProvider
        {

            [SetUp]
            public void SetUp()
            {
            }

            [Test]
            public async Task TestMethod_UsingSqliteInMemoryProvider_Success()
            {
                var options = new DbContextOptionsBuilder<Sped_SafewebContextMemory>()
                    .UseInMemoryDatabase(databaseName: "SafeWebInMemory")
                    .Options;

                using (var context = new Sped_SafewebContextMemory(options))
                {
                    context.TransactionReference.Add(new TransactionReference { Document = "34460140000164", ReferenceType = Business.Enum.ReferenceType.ENOTA, YearMonth = "202010" });
                    context.TransactionReference.Add(new TransactionReference { Document = "89837500000196", ReferenceType = Business.Enum.ReferenceType.ENOTAPARTNER, YearMonth = "202010" });
                    context.TransactionReference.Add(new TransactionReference { Document = "99837500000196", ReferenceType = Business.Enum.ReferenceType.ENOTAPARTNER, YearMonth = "202010" });
                    context.SaveChanges();
                }

                using (var context = new Sped_SafewebContextMemory(options))
                {
                    var sut = new TransactionReferenceRepository(context);

                    var references = sut.GetAll();

                    NUnit.Framework.Assert.AreEqual(3, references.Result.Count());
                }
            }
        }
    }
}