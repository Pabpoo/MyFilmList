using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorPeliculas.Client.Auth
{
	public class ProveedorAutenticacionPrueba : AuthenticationStateProvider
	{
		//Método que devuelve el estado de autenticación del usuario
		//Este método se ejecuta en cuanto el usuario entra en la aplicación, no hay que ejecutarlo de ninguna manera
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			//Claim es una dato del usuario, nombre, fecha nacimiento, email...
			var anonimo = new ClaimsIdentity();
			var usuario = new ClaimsIdentity(
				new List<Claim>
				{
					//un claim no es más que tipo y valor
					new Claim("clave1", "valor1"),
					new Claim("edad", "999"),
					//ClaimTypes es un tipo de dato definido por el Framwork y por lo tanto podemos acceder a él desde otras partes de la palicación, como en el index
					new Claim(ClaimTypes.Name, "Pablo"),
					//new Claim(ClaimTypes.Role, "admin")
				},
				authenticationType: "prueba");
			return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));
		}
	}
}
