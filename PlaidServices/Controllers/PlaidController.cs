// Controllers/PlaidController.cs
using Microsoft.AspNetCore.Mvc;
using PlaidIntegration.Services;
using System.Threading.Tasks;

namespace PlaidIntegration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaidController : ControllerBase
    {
        private readonly PlaidService _plaidService;

        public PlaidController(PlaidService plaidService)
        {
            _plaidService = plaidService;
        }

        // Step 1: Create a link token
        [HttpGet("create_link_token")]
        public async Task<IActionResult> CreateLinkToken()
        {
            var linkToken = await _plaidService.CreateLinkTokenAsync("user_good");
            return Ok(new { link_token = linkToken });
        }

        // Step 2: Exchange public token for access token
        [HttpPost("exchange_public_token")]
        public async Task<IActionResult> ExchangePublicToken([FromBody] ExchangePublicTokenRequest request)
        {
            var accessToken = await _plaidService.ExchangePublicTokenAsync(request.PublicToken);
            return Ok(new { access_token = accessToken });
        }

        // Step 3: Get account info (routing and account number)
        [HttpPost("get_account_info")]
        public async Task<IActionResult> GetAccountInfo([FromBody] GetAccountInfoRequest request)
        {
            var accountInfo = await _plaidService.GetAccountInfoAsync(request.AccessToken);
            return Ok(new
            {
                account_number = accountInfo.Name,
                routing_number = accountInfo.Balances
            });
        }
    }

    public class ExchangePublicTokenRequest
    {
        public string PublicToken { get; set; }
    }

    public class GetAccountInfoRequest
    {
        public string AccessToken { get; set; }
    }
}
