using ComprobantePago.Application.DTOs.Seguridad;
using ComprobantePago.Application.Exceptions;
using ComprobantePago.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ComprobantePago.Infrastructure.Services
{
    public class SeguridadService : ISeguridadService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly TokenService _token;

        public SeguridadService(
            IConfiguration configuration,
            HttpClient httpClient,
            TokenService token)
        {
            _configuration = configuration;
            _httpClient    = httpClient;
            _token         = token;
        }

        public async Task<IEnumerable<SeguridadRolResponse>> ObtenerPermisos(
            string codTenant, string codUsuario, string codApp)
        {
            var token = await _token.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var url = _configuration["ApiSettings:Seguridad:ObtenerPermisos"]
                    + $"?codUsuario={codUsuario}&codTenant={codTenant}&codApp={codApp}";

            var response = await _httpClient.GetAsync(url);
            var content  = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<SeguridadResponse<IEnumerable<SeguridadRolResponse>>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null)
                throw new AppException("No se pudo obtener los permisos del usuario.", "NOT_FOUND", 404);

            if (!apiResponse.Success)
                throw new AppException(
                    apiResponse.Error?.UserMessage ?? "Error al obtener permisos.",
                    "SEGURIDAD_ERROR",
                    StatusCodes.Status400BadRequest);

            return apiResponse.Data;
        }
    }
}
