using BlazorPeliculas.Shared.DTO;

namespace BlazorPeliculas.Client.Auth
{
	public interface ILoginService
	{
		Task Login(UserTokenDTO tokenDTO);
		Task LogOut();
        Task ManejarRenovacionToken();
    }
}
