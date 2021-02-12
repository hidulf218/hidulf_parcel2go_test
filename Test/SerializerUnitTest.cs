using Core;
using Core.Model;
using Core.Model.Parameter;
using Core.Model.Result;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class SerializerUnitTest : UnitTestBase
    {
        [TestMethod]
        [TestProperty("Json", @"{""access_token"":""IOMuc7iy5x_3fG7-M4j243jHEAgtBuHvVhCCV3cLWXE"",""expires_in"":7200,""token_type"":""Bearer"",""scope"":""public-api""}")]
        [TestProperty("AccessToken", "IOMuc7iy5x_3fG7-M4j243jHEAgtBuHvVhCCV3cLWXE")]
        [TestProperty("ExpiryIn", "7200")]
        [TestProperty("TokenType", "Bearer")]
        [TestProperty("Scope", "public-api")]
        public void Deserialize_Token()
        {
            var properties = GetCustomAttributes("Deserialize_Token");
            var json = properties["Json"];
            var expected = new Token
            {
                AccessToken = properties["AccessToken"],
                ExpiryIn = int.Parse(properties["ExpiryIn"]),
                TokenType = properties["TokenType"],
                Scope = properties["Scope"],
            };

            var result = Serializer.Deserialize<Token>(json);

            Assert.AreEqual(result.AccessToken, expected.AccessToken, "AccessToken is correct");
            Assert.AreEqual(result.ExpiryIn, expected.ExpiryIn, "ExpiryIn is correct");
            Assert.AreEqual(result.TokenType, expected.TokenType, "TokenType is correct");
            Assert.AreEqual(result.Scope, expected.Scope, "Scope is correct");
        }

        [TestMethod]
        [TestProperty("Json", @"{""AvailableExtras"":[{""Type"":""Cover"",""Price"":0,""Vat"":0,""Total"":0,""Details"":null},{""Type"":""DeliveryGuarantee"",""Price"":1.1,""Vat"":0.22,""Total"":1.32,""Details"":null},{""Type"":""ExtendedBaseCover"",""Price"":1,""Vat"":0.2,""Total"":1.2,""Details"":{""IncludedCover"":""20.0000"",""MaxWeight"":""999.00""}}],""Discount"":0,""Extras"":[],""Service"":{""DropOffProviderCode"":"""",""CourierName"":""Yodel"",""CourierSlug"":""yodel"",""Slug"":""yodel-northern-ireland"",""Name"":""Yodel Northern Ireland"",""CollectionType"":""Collection"",""DeliveryType"":""Door"",""Links"":{""ImageSmall"":""https://sandbox.parcel2go.com/logo/service/10003.small"",""Imagelarge"":""https://sandbox.parcel2go.com/logo/service/10003.large"",""ImageSvg"":""https://sandbox.parcel2go.com/logo/service/yodel-northern-ireland/logo.normal-svg"",""Courier"":""https://sandbox.parcel2go.com/couriers/yodel"",""Service"":""https://sandbox.parcel2go.com/service/yodel-northern-ireland""},""MaxHeight"":null,""MaxWidth"":null,""MaxLength"":1.2,""MaxWeight"":25,""MaxCover"":5000,""IsPrinterRequired"":true,""ShortDescriptions"":"""",""Overview"":"""",""Features"":"""",""RequiresDimensions"":false,""Classification"":""Slow""},""TotalPrice"":13.19,""TotalPriceExVat"":10.99,""TotalVat"":2.2,""UnitPrice"":10.99,""VatRate"":20,""Collection"":""2021-02-15T00:00:00"",""CutOff"":""2021-02-12T23:00:00"",""EstimatedDeliveryDate"":""2021-02-18T00:00:00"",""IncludedCover"":0,""TariffSystem"":null,""Distance"":null,""RequiresCommercialInvoice"":false}")]
        [TestProperty("ServiceName", @"Yodel Northern Ireland")]
        [TestProperty("TotalPrice", @"13.19")]
        public void Deserialize_Quote()
        {
            var properties = GetCustomAttributes("Deserialize_Quote");
            var json = properties["Json"];
            var expected = new Quote
            {
                Service = new Service
                {
                    Name = properties["ServiceName"],
                },
                TotalPrice = decimal.Parse(properties["TotalPrice"]),
            };

            var result = Serializer.Deserialize<Quote>(json);

            Assert.AreEqual(result.Service.Name, expected.Service.Name, "Service name is correct");
            Assert.AreEqual(result.TotalPrice, expected.TotalPrice, "Total price is correct");
        }

        [TestMethod]
        [TestProperty("Weight", "1.5")]
        [TestProperty("Length", "10")]
        [TestProperty("Width", "10")]
        [TestProperty("Height", "10")]
        [TestProperty("Json", @"{""Weight"":1.5,""Length"":10,""Width"":10,""Height"":10}")]
        public void Serialize_Parcel()
        {
            var properties = GetCustomAttributes("Serialize_Parcel");

            var obj = new Parcel
            {
                Weight = decimal.Parse(properties["Weight"]),
                Length = decimal.Parse(properties["Length"]),
                Width = decimal.Parse(properties["Width"]),
                Height = decimal.Parse(properties["Height"]),
            };
            var result = Serializer.Serialize<Parcel>(obj);
            var expected = properties["Json"];

            Assert.AreEqual(result, expected, $"Serialized JSON data matched");
        }

        [TestMethod]
        [TestProperty("Country", "GBR")]
        [TestProperty("Json", @"{""Country"":""GBR""}")]
        public void Serialize_Address()
        {
            var properties = GetCustomAttributes("Serialize_Address");

            var obj = new Address
            {
                Country = properties["Country"],
            };
            var result = Serializer.Serialize<Address>(obj);
            var expected = properties["Json"];

            Assert.AreEqual(result, expected, $"Serialized JSON data matched");
        }

        [TestMethod]
        [TestProperty("Weight", "1.5")]
        [TestProperty("Length", "10")]
        [TestProperty("Width", "10")]
        [TestProperty("Height", "10")]
        [TestProperty("CollectCountry", "GBR")]
        [TestProperty("DeliveryCountry", "GBR")]
        [TestProperty("Json", @"{""CollectionAddress"":{""Country"":""GBR""},""DeliveryAddress"":{""Country"":""GBR""},""Parcels"":[{""Weight"":1.5,""Length"":10,""Width"":10,""Height"":10}]}")]
        public void Serialize_QuoteParameter()
        {
            var properties = GetCustomAttributes("Serialize_QuoteParameter");

            var obj = new QuoteParameter
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
            var result = Serializer.Serialize<QuoteParameter>(obj);
            var expected = properties["Json"];

            Assert.AreEqual(result, expected, $"Serialized JSON data matched");
        }

    }
}
