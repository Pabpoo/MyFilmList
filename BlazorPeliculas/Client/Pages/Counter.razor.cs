using BlazorPeliculas.Client.Utilities;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        //[Inject] ServicesSingleton singleton { get; set; } = null!;
        //[Inject] ServicesTransient transient {  get; set; } = null!;
        //[Inject] IJSRuntime js { get; set; } = null!;
        //[CascadingParameter (Name="Color")] protected string Color { get; set; } = null!;
        //[CascadingParameter (Name="Size")] protected string Size { get; set; } = null!;
        //[CascadingParameter] protected AppState appState { get; set; } = null!;

        //IJSObjectReference? modulo;

        private int currentCount = 0;
        [Inject] public IJSRuntime js { get; set; } = null!;

        //Como AuthorizeRouteView devuelve un parámetro en cascada de tipo Task podemos aprovecharlo para dar acceso a funcionalidades dependiendo de si el usuario está autenticado
        [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }

        //private static int currentCountStatic = 0;

        //[JSInvokable]
        //public async Task IncrementCount()
        public async Task IncrementCount()
        {
            var arreglo = new double[] { 1, 2, 3, 4, 5 };
            var max = arreglo.Maximum();
            var min = arreglo.Minimum();

            //await js.InvokeVoidAsync("alert", $"El max es {max} y el min es {min}");

            // Definimos si el usuario está autenticado
            var authenticationState = await authenticationStateTask;
            var usuarioEstaAutenticado = authenticationState.User.Identity.IsAuthenticated;
            

            if (usuarioEstaAutenticado)
            {
                currentCount++;
            }

            else
            {
                currentCount--;
            }

            //currentCount++;

            // Asignar modulos de js a una variable hace que no se carguen hasta que se les invoca,
            // lo que hace más eficiente las cosas
            //modulo = await js.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");
            //await modulo.InvokeVoidAsync("mostrarAlerta", "Hola Mundo!");

            //currentCountStatic = currentCount;
            //singleton.Valor = currentCount;
            //transient.Valor = currentCount;
            //await js.InvokeVoidAsync("pruebaPuntoNetStatic");
        }

        //private async Task IncrementCountJavascript()
        //{
        //	await js.InvokeVoidAsync("pruebaPuntoNetInstancia",
        //		DotNetObjectReference.Create(this));
        //}
        ////No mpuedo acceder a currentCount desde un estatico, asi que tenemos que
        ////crear una variable intermedia que sea estática
        //[JSInvokable]
        //public static Task<int> ObtenerCurrentCount()
        //{
        //	return Task.FromResult(currentCountStatic);
        //}
    }
}
