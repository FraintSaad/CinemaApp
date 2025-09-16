using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Data.Models;
using CinemaApp.Models;

public class TmdbService
{
    private readonly HttpClient _httpClient;

    private const string BaseUrl = "https://api.themoviedb.org/3/";
    private const string BearerToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhMzJmMGQ5ZmNhZDFjMmY3NjRjZDM0Yzc3MzdiMjQxZCIsIm5iZiI6MTc1ODAxNjk2MC41NTQsInN1YiI6IjY4YzkzNWMwMjNiN2M0NjBkNjc4YmVjMiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.6phDz58x6XVOp_Iy2uyGMM70N5O6JzsgJhr3wM_10L0";

    public TmdbService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BearerToken}");

    }

    public async Task<string> SendRequestAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ошибка запроса {response.StatusCode}: {error}");
        }
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<TmdbFilmModel>> GetPopularFilmsAsync(int page = 1, string language = "en-US")
    {
        string endpoint = $"movie/popular?language={language}&page={page}";
        string json = await SendRequestAsync(endpoint);

        var response = JsonConvert.DeserializeObject<PopularMoviesResponse>(json);

        var films = new List<TmdbFilmModel>();
        if (response?.Results != null)
        {
            foreach (var item in response.Results)
            {
                films.Add(new TmdbFilmModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    OriginalTitle = item.OriginalTitle,
                    OriginalLanguage = item.OriginalLanguage,
                    Overview = item.Overview,
                    ReleaseDate = item.ReleaseDate,
                    VoteAverage = item.VoteAverage,
                    VoteCount = item.VoteCount,
                    Popularity = item.Popularity,
                    Adult = item.Adult,
                    Video = item.Video,
                    PosterPath = item.PosterPath,
                    BackdropPath = item.BackdropPath,
                    GenreIds = item.GenreIds
                });
            }
        }

        return films;
    }
}
