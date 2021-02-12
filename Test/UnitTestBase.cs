using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public abstract class UnitTestBase
    {
        protected IConfiguration _config;

        [TestInitialize]
        public virtual void Initialize()
        {
            _config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.Test.json")
              .Build();
        }

        protected Dictionary<string, string> GetCustomAttributes(string methodName)
        {
            return GetType().GetMethod(methodName).GetCustomAttributes(true).OfType<TestPropertyAttribute>().ToDictionary(r => r.Name, r => r.Value);
        }


        [TestCleanup]
        public virtual void Cleanup()
        {
        }
    }
}
