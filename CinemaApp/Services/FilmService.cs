using CinemaApp.Models;
using Data.Context;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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

namespace CinemaApp.Services
{
    public class FilmService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "1aedc52c-6963-41f8-b059-0fa21cc2b42b";


        private readonly FilmsDbContext _dbContext;


        public FilmService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://kinopoiskapiunofficial.tech/api/v2.2/");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _dbContext = new FilmsDbContext();
        }

        public async Task<List<FilmModel>> GetFilmsAsync(int offset)
        {
            try
            {
                var url = $"films?order=RATING&type=ALL&ratingFrom=9&ratingTo=10&yearFrom=1000&yearTo=3000&page={offset + 1}";
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something went drastically wrong");
                }

                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);

                var filmResponse = JsonConvert.DeserializeObject<FilmResponse>(json);
                foreach (var film in filmResponse.Items)
                {
                    foreach (var item in _dbContext.FavoriteFilms)
                    {
                        var local = _dbContext.Set<FilmEntity>().FirstOrDefault(item => item.KinopoiskId.Equals(film.KinopoiskId));

                        if (local != null)
                        {
                            film.IsInFavorites = true;
                        }
                    }
                }
                return filmResponse?.Items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка десериализации: {ex.Message} ");
                throw;
            }
        }
    }
}
