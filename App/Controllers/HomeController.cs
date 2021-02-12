using App.Helpers;
using App.Models;
using Core;
using Core.Model.Parameter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly QuoteService _service;

        public const string COLSERVICENAMECODE = "ServiceName";
        public const string COLTOTALPRICECODE = "TotalPrice";

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new QuoteService(_config);
        }

        [HttpGet]
        public IActionResult Index()
        {
            // prepare the view model with default values
            GetQuoteViewModel model = new GetQuoteViewModel
            {
                InputCollectionCountry = "GBR",
                InputDeliveryCountry = "GBR",
                InputCollectionType = "Collection",
                InputDeliveryType = "Door",
                InputHeght = 10,
                InputLength = 10,
                InputWidth = 10,
            };

            return View(model);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetQuote(GetQuoteViewModel model)
        {
            if (ModelState.IsValid) // Process if validation success
            {
                try
                {
                    // validate current token, get authorize token if not exists 
                    if (!_service.IsTokenValid(model.Token))
                        model.Token = await _service.GetAuthorizeToken();

                    // prepare the parameter for calling the quote service
                    var parameter = new QuoteParameter
                    {
                        CollectionAddress = new Address { Country = model.InputCollectionCountry },
                        DeliveryAddress = new Address { Country = model.InputDeliveryCountry },
                        Parcels = new Parcel[]
                        {
                    new Parcel
                    {
                        Weight = model.InputWeight.Value,
                        Length = model.InputLength,
                        Height = model.InputHeght,
                        Width = model.InputWidth,
                    }
                        }
                    };
                    var result = await _service.GetQuote(model.Token, parameter);

                    // perform the filter on collection type and delivery type, then sort the result by total price in ascending order
                    model.Quotes = result.Quotes.Where(quote =>
                           (string.IsNullOrWhiteSpace(model.InputCollectionType) || quote.Service.CollectionType == model.InputCollectionType) // filter by collection type if given
                        && (string.IsNullOrWhiteSpace(model.InputDeliveryType) || quote.Service.DeliveryType == model.InputDeliveryType) // filter by delivery type if given
                        )
                        .OrderBy(quote => quote.TotalPrice)
                        .ToArray();
                    model.SortCol = COLTOTALPRICECODE;

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "CurrentModel", model); // Store model data in session for further sorting purpose
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex.Message, ex.InnerException); // log the error in the log file
                    model.ErrorMessage = "Something goes wrong, please try again"; // return a general error message to the frontend
                }
            }
            return View("Index", model);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult SortQuotes(string col)
        {
            var model = SessionHelper.GetObjectFromJson<GetQuoteViewModel>(HttpContext.Session, "CurrentModel"); // Restore current model data from session

            // perform sorting based on the given column code
            bool sorted = false;
            switch (col)
            {
                case COLSERVICENAMECODE:
                    model.Quotes = model.Quotes.OrderBy(quote => quote.Service.Name).ToArray();
                    sorted = true;
                    break;
                case COLTOTALPRICECODE:
                    model.Quotes = model.Quotes.OrderBy(quote => quote.TotalPrice).ToArray();
                    sorted = true;
                    break;
            }
            if (sorted)
                model.SortCol = col;

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_service != null)
                    _service.Dispose();
            }
        }
    }
}
