using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Sample.CoffeeBot
{
    public class GifService
    {
        private readonly string serviceUrl = 
            $"https://api.giphy.com/v1/gifs/random?api_key={Keys.GiphyApiKey}&tag=coffee&rating=G";

        public GifService()
        {

        }

        public async Task<CoffeeGif> GetRandomCoffeeGif()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(serviceUrl);
                var content = await result.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(content);

                CoffeeGif reply = new CoffeeGif
                {
                    GiphyWebUrl = obj["data"]["bitly_gif_url"].ToString(),
                    GifUrl = obj["data"]["images"]["fixed_width"]["url"].ToString()
                };

                return reply;
            }
        }
    }
}