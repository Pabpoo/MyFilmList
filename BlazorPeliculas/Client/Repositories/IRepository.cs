using BlazorPeliculas.Shared.Entity;

namespace BlazorPeliculas.Client.Repositories
{
	public interface IRepository
	{
		Task<HttpResponseWraper<object>> Delete(string url);
		Task<HttpResponseWraper<T>> Get<T>(string url);
		Task<HttpResponseWraper<object>> Post<T>(string url, T enviar);
        Task<HttpResponseWraper<TResponse>> Post<T, TResponse>(string url, T enviar);
        Task<HttpResponseWraper<object>> Put<T>(string url, T enviar);
    }
}
