function timerInactivo(dotnetHelper) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 1000 * 60 * 30) // 30minutos
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}

// Esta funcion javascript nso permite updatear los tooltips de una pagina, ya que estos se crean al inicializar la pagina, por lo que para que se actualicen se deben destruir y volver a crear
function updateTooltips() {
    $('[data-bs-toggle="tooltip"]').tooltip('dispose');
    $('[data-bs-toggle="tooltip"]').tooltip();
}