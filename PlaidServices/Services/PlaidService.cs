// Services/PlaidService.cs
using Microsoft.Extensions.Configuration;
using PlaidIntegration.Controllers;
using Going.Plaid;
using System.Threading.Tasks;
using Going.Plaid.Link;
using Going.Plaid.Entity;

namespace PlaidIntegration.Services
{
    public class PlaidService
    {
        private readonly PlaidClient _plaidClient;
        private readonly string _clientId;
        private readonly string _secret;

        public PlaidService(IConfiguration configuration)
        {
            _plaidClient = new PlaidClient(
                Going.Plaid.Environment.Sandbox,
                secret: "80f488dc08e104e63cd156c8d7f55a",
                clientId: "67acd56f70dce6002146c70a");

        }

        public async Task<string> CreateLinkTokenAsync(string clientUserId)
        {

            var linkTokenRequest = new LinkTokenCreateRequest
            {
                User = new LinkTokenCreateRequestUser {ClientUserId = "user_good"},
                ClientName = "Sandbox",
                Products = new List<Products> { Products.Auth },
                CountryCodes = new List<CountryCode> { CountryCode.Us },
                Language = Language.English
            };


            var linkTokenResponse = await _plaidClient.LinkTokenCreateAsync(linkTokenRequest);
            return linkTokenResponse.LinkToken;
        }

        public async Task<string> ExchangePublicTokenAsync(string publicToken)
        {
            var exchangeRequest = new Going.Plaid.Item.ItemPublicTokenExchangeRequest
            {
                PublicToken = publicToken
            };

            var exchangeResponse = await _plaidClient.ItemPublicTokenExchangeAsync(exchangeRequest);
            return exchangeResponse.AccessToken;
        }

        public async Task<Going.Plaid.Entity.Account> GetAccountInfoAsync(string accessToken)
        {
            var authResponse = await _plaidClient.AccountsGetAsync(new Going.Plaid.Accounts.AccountsGetRequest
            {
                AccessToken = accessToken
            });

            // Returning the first account's routing number and account number
            return authResponse.Accounts[0];
        }
    }
}