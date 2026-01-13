using Project.Api.DTO.Reviews;
using Project.DesktopClient.DTO.Review;
using Project.DesktopClient.DTO.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.Services.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public Uri BaseAddress => _httpClient.BaseAddress!;

        public ApiClient(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        #region User
        // GET api/User
        public async Task<List<UserDetailsDto>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/User");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar usuários: {response.StatusCode}");

            var users = await response.Content.ReadFromJsonAsync<List<UserDetailsDto>>();

            return users ?? new List<UserDetailsDto>();

        }

        // GET api/User/{id}
        public async Task<UserDetailsDto> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/User/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            var user = await response.Content.ReadFromJsonAsync<UserDetailsDto>();

            if (user is null)
                throw new Exception($"Erro ao buscar usuário: {response.StatusCode}");

            return user ?? new UserDetailsDto();
        }

        // POST api/User/
        public async Task<UserDetailsDto?> PostUserAsync(UserCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/User/", dto);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserDetailsDto>();
        }

        // PATCH api/User/{id}
        public async Task<UserDetailsDto?> PatchUserAsync(int id, UserUpdateDto dto)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/User/{id}", dto);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserDetailsDto>();
        }

        // DELETE api/User/{id}
        public async Task DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/User/{id}");

            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Review
        // GET api/Review
        public async Task<List<ReviewDetailsDto>> GetReviewsAsync()
        {
            var response = await _httpClient.GetAsync("api/Review");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar reviews: {response.StatusCode}");

            var reviews = await response.Content.ReadFromJsonAsync<List<ReviewDetailsDto>>();

            return reviews ?? new List<ReviewDetailsDto>();
        }

        // GET api/Review
        public async Task<ReviewDetailsDto> GetReviewById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Review/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            var review = await response.Content.ReadFromJsonAsync<ReviewDetailsDto>();

            if(review == null)
                throw new Exception($"Erro ao buscar review: {response.StatusCode}");

            return review ?? new ReviewDetailsDto();
        }

        // GET api/Review/{id}
        public async Task<ReviewDetailsDto?> PostReviewAsync(ReviewCreateDto dto)
        {
            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(dto.Title), "Title");
            form.Add(new StringContent(dto.Category), "Category");
            form.Add(new StringContent(dto.Description), "Description");
            form.Add(new StringContent(dto.UserId.ToString()), "UserId");

            if (!string.IsNullOrWhiteSpace(dto.ImageFilePath) && File.Exists(dto.ImageFilePath))
            {
                var stream = File.OpenRead(dto.ImageFilePath);
                var fileContent = new StreamContent(stream);

                var extension = Path.GetExtension(dto.ImageFilePath).ToLowerInvariant();
                var mime = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".bmp" => "image/bmp",
                    _ => "application/octet-stream"
                };
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mime);

                // "Image" = nome da propriedade IFormFile em ReviewCreateDto (lado da API)
                form.Add(fileContent, "Image", Path.GetFileName(dto.ImageFilePath));
            }

            var response = await _httpClient.PostAsync("api/Review", form);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ReviewDetailsDto>();
        }

        #endregion
    }
}
