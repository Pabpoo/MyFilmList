using BlazorPeliculas.Client.Repositories;
using BlazorPeliculas.Client.Utilities;
using BlazorPeliculas.Shared.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace BlazorPeliculas.Client.Auth
{
	public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginService
	{
		private readonly IJSRuntime js;
		private readonly HttpClient httpClient;
        private readonly IRepository repositorio;

        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient, IRepository repositorio)
		{
			this.js = js;
			this.httpClient = httpClient;
            this.repositorio = repositorio;
        }

		public static readonly string TokenKey = "TokenKey";
		public static readonly string ExpirationTokenKey = "ExpirationTokenKey";


		private AuthenticationState Anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

		//Vamos a guardar el token en el ordenador del usuario para que si se sale sin querer no tanga que estar autenticandose todo el rato
		public async override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			 var token = await js.ObtenerDeLocalStorage(TokenKey);

			if (token is null)
			{
				return Anonimo;
			}

			var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(ExpirationTokenKey);
			DateTime tiempoExpiracion;

			if (tiempoExpiracionObject is null)
			{
				await Limpiar();
				return Anonimo;
			}

			if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
			{
				if (TokenExpirado(tiempoExpiracion))
				{
					await Limpiar();
					return Anonimo;
				}

				if (DebeRenovarToken(tiempoExpiracion))
				{
					token = await RenovarToken(token.ToString());
				}
			}

			return ConstruirAuthenticationState(token.ToString());
		}

		private bool TokenExpirado(DateTime tiempoExpiracion)
		{
			return tiempoExpiracion <= DateTime.UtcNow;
		}

		private bool DebeRenovarToken(DateTime tiempoExpiracion)
		{
			return tiempoExpiracion.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(5);
		}

		public async Task ManejarRenovacionToken()
		{
            var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(ExpirationTokenKey);
			DateTime tiempoExpiracion;
			if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
			{
				if (TokenExpirado(tiempoExpiracion))
				{
					await LogOut();
				}

				if (DebeRenovarToken(tiempoExpiracion))
				{
					var token = await js.ObtenerDeLocalStorage(TokenKey);
					var nuevoToken = await RenovarToken(token.ToString());
					var authState = ConstruirAuthenticationState(nuevoToken);
					//al obtener un nuevo token este puede tener nuevos claims y de esta manera si ha cambiado algo en sus permisos
					//por ejemplo ya no hay que desloguearse, sino que el token llega con los nuevos permisos
					NotifyAuthenticationStateChanged(Task.FromResult(authState));
				}
			}
        }

		private async Task<string> RenovarToken(string token)
		{
			Console.WriteLine("Renovando el token...");
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			var nuevoTokenResponse = await repositorio.Get<UserTokenDTO>("api/cuentas/renovarToken");
			var nuevoToken = nuevoTokenResponse.Response;

            await js.GuardarEnLocalStorage(TokenKey, nuevoToken.Token);
            await js.GuardarEnLocalStorage(ExpirationTokenKey, nuevoToken.Expiration.ToString());

			return nuevoToken.Token;
        }

		private AuthenticationState ConstruirAuthenticationState(string token)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
			var claims = PasearClaimsJWT(token);
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
		}

		private IEnumerable<Claim> PasearClaimsJWT (string token)
		{
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);
			return tokenDeserializado.Claims;
		}

		public async Task Login(UserTokenDTO tokenDTO)
		{
			await js.GuardarEnLocalStorage(TokenKey, tokenDTO.Token);
			await js.GuardarEnLocalStorage(ExpirationTokenKey, tokenDTO.Expiration.ToString());
			var authState = ConstruirAuthenticationState(tokenDTO.Token);
			//Método de blazor para decirle que ha cambiado el status de autenticación, se van a tomar en cuenta sus claims
			NotifyAuthenticationStateChanged(Task.FromResult(authState));
		}

		public async Task LogOut()
		{
			await Limpiar();
			NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
		}

		private async Task Limpiar()
		{
            await js.RemoverDeLocalStorage(TokenKey);
            await js.RemoverDeLocalStorage(ExpirationTokenKey);
            // Removemos el token del Http Client
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
	}
}
