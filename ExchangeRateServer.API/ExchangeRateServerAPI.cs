using ExchangeRateServer.Helpers;
using ExchangeRateServer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;

namespace ExchangeRateServer.ExchangeRateServer.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateServerAPI : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;
        public ExchangeRateServerAPI(ICacheService cacheService)
        {
            _exchangeRateService = new ExchangeRateService(cacheService);
        }

        // GET: api/<ExchangeRateServerAPI>
        [HttpGet]
        public async Task<ActionResult> Get(string startDate, string endDate, string currency)
        {
            var resultString = string.Empty;

            try
            {
                resultString = await _exchangeRateService.GetRatesAsync(startDate, endDate, currency);
            } 
            catch (PageNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return Ok(resultString);
            
        }
    }
}
