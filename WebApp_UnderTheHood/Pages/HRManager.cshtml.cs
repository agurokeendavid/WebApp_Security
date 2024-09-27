using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp_UnderTheHood.Authorization;
using WebApp_UnderTheHood.DTO;
using WebApp_UnderTheHood.Pages.Account;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy = "HRManagerOnly")]
    public class HRManagerModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public List<WeatherForecastDTO> weatherForecastItems { get; set; } = new List<WeatherForecastDTO>();

        public HRManagerModel(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            // get token from session
            JwtToken token = new JwtToken();
            var strToken = HttpContext.Session.GetString("access_token");

            if (string.IsNullOrEmpty(strToken))
            {
                token = await AuthenticateAsync();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strToken) ?? new JwtToken();
            }

            if (token is null || string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpiresAt <= DateTime.UtcNow)
            {
                token = await AuthenticateAsync();
            }
            
            var httpClient = this._httpClientFactory.CreateClient("OurWebAPI");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
            weatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast")?? new List<WeatherForecastDTO>();
        }

        private async Task<JwtToken> AuthenticateAsync()
        {
            // authentication and getting the token
            var httpClient = this._httpClientFactory.CreateClient("OurWebAPI");
            var response = await httpClient.PostAsJsonAsync("auth", new Credential() { UserName = "admin", Password = "admin" });
            response.EnsureSuccessStatusCode();
            string strJwt = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", strJwt);
            return JsonConvert.DeserializeObject<JwtToken>(strJwt) ?? new JwtToken();
        }
    }
    
}