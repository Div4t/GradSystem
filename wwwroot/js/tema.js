// ── Modo Nocturno ─────────────────────────────────────────────────────────
(function () {
    var KEY   = 'grad-tema';
    var DARK  = 'dark';
    var LIGHT = 'light';

    function temaActual() {
        return localStorage.getItem(KEY) || LIGHT;
    }

    function aplicarTema(tema) {
        // Aplicar sobre <html> para que los selectores [data-theme="dark"] funcionen
        document.documentElement.setAttribute('data-theme', tema);

        var btn = document.getElementById('btn-tema');
        if (btn) {
            btn.textContent = tema === DARK ? '☀️' : '🌙';
            btn.title = tema === DARK ? 'Cambiar a modo claro' : 'Cambiar a modo oscuro';
        }
    }

    // Aplicar ANTES de que el navegador pinte el body (evita flash blanco)
    aplicarTema(temaActual());

    // Cuando el DOM esté listo, conectar el botón
    document.addEventListener('DOMContentLoaded', function () {
        aplicarTema(temaActual()); // re-aplicar para actualizar el ícono del botón

        var btn = document.getElementById('btn-tema');
        if (btn) {
            btn.addEventListener('click', function () {
                var nuevo = temaActual() === DARK ? LIGHT : DARK;
                localStorage.setItem(KEY, nuevo);
                aplicarTema(nuevo);
            });
        }
    });
})();
