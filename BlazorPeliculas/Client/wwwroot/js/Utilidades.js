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