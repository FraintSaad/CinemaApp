using CinemaApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CinemaApp.Shared.Services
{
    public class FilmService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "1aedc52c-6963-41f8-b059-0fa21cc2b42b";


        public FilmService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://kinopoiskapiunofficial.tech/api/v2.2/");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
        }

        public async Task<List<FilmModel>> GetFilmsAsync()
        {
            try
            {
                var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films?order=RATING&type=ALL&ratingFrom=9&ratingTo=10&yearFrom=1000&yearTo=3000&page=1";
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var filmResponse = JsonConvert.DeserializeObject<FilmResponse>(json);
                return filmResponse?.Items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка десериализации: {ex.Message} ");
                return null;
            }
        }
    }
}
