using Core;
using Core.Model;
using Core.Model.Parameter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class QuoteServiceUnitTest : UnitTestBase
    {
        private QuoteService _service;
        private Token _token;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            _service = new QuoteService(_config);
        }

        [TestMethod]
        public void IsTokenValid_ReturnFalse()
        {
            bool result = _service.IsTokenValid(null);

            Assert.IsFalse(result, "Token must not valid when service created");
        }

        [TestMethod]
        public async Task GetAuthorizeToken_ReturnToken()
        {
            _token = await _service.GetAuthorizeToken();

            Assert.IsNotNull(_token, "Get token successfully");

            bool isValid = _service.IsTokenValid(_token);
            Assert.IsTrue(isValid, "Token should be valid after access token granted");
        }

        [TestMethod]
        [TestProperty("Weight", "1.5")]
        [TestProperty("Length", "10")]
        [TestProperty("Width", "10")]
        [TestProperty("Height", "10")]
        [TestProperty("CollectCountry", "GBR")]
        [TestProperty("DeliveryCountry", "GBR")]
        public async Task GetQuote_ReturnGuoteData()
        {
            var properties = GetCustomAttributes("GetQuote_ReturnGuoteData");

            var parameters = new QuoteParameter
            {
                CollectionAddress = new Address { Country = properties["CollectCountry"] },
                DeliveryAddress = new Address { Country = properties["DeliveryCountry"] },
                Parcels = new Parcel[]
                {
                    new Parcel
                    {
                        Weight = decimal.Parse(properties["Weight"]),
                        Length = decimal.Parse(properties["Length"]),
                        Width = decimal.Parse(properties["Width"]),
                        Height = decimal.Parse(properties["Height"]),
                    }
                },
            };

            // request for access token if existing is not valid
            if (!_service.IsTokenValid(_token))
                _token = await _service.GetAuthorizeToken();

            var result = await _service.GetQuote(_token, parameters);
            Assert.IsNotNull(result, "Result must be a object converted from json data");
        }

        public override void Cleanup()
        {
            base.Cleanup();

            if (_service != null)
            {
                _service.Dispose();
                _service = null;
            }

        }
    }
}
