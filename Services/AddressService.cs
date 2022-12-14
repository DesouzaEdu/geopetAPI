using System.Globalization;
using GeoPetWebApi.Controllers.inputs;
using GeoPetWebApi.Services.output;

namespace projetoFinal.Services
{
    public class AddressService {
        private readonly HttpClient _client;
        public AddressService(HttpClient client) {
            _client = client;
        }

        public async Task<AddressOutput> GetAddressByLat(AddressInput addressInput){
            _client.BaseAddress = new Uri("https://nominatim.openstreetmap.org");
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(CultureInfo.InvariantCulture, $"/reverse?format=jsonv2&lat={addressInput.lat}&lon={addressInput.lon}" ));
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "WHATEVER VALUE");
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<AddressOutput>();
            return result;
        }

        

    }
};
