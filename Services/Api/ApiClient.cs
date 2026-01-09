using Project.DesktopClient.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Project.DesktopClient.Services.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

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
        public async Task<UserDetailsDto> PostUserAsync(UserCreateDto dto)
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
    }
}
