using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Threading.Tasks;

namespace LookupService.Controllers
{
    [Route("api/lookup")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly ICreditDataApiProvider _serviceProvider;

        public LookupController(ICreditDataApiProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("credit-data/{ssn}")]
        public async Task<CreditData> LookupCreditData(string ssn)
        {
            var result = await _serviceProvider.GetSsnCreditDetails(ssn);

            return result;
        }
    }
}
