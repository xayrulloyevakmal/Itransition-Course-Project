// ==========================================
// AssetTrack — Theme & Language Switching
// ==========================================

document.addEventListener("DOMContentLoaded", () => {

    // --- Theme Toggle ---
    const themeBtn = document.getElementById("themeToggleBtn");
    const themeIcon = document.getElementById("themeIcon");
    const htmlEl = document.documentElement;

    function getStoredTheme() {
        return localStorage.getItem("at-theme") || getCookie("theme") || "light";
    }

    function applyTheme(theme) {
        htmlEl.setAttribute("data-bs-theme", theme);
        if (themeIcon) {
            themeIcon.className = theme === "dark" ? "bi bi-sun-fill" : "bi bi-moon-fill";
        }
    }

    applyTheme(getStoredTheme());

    if (themeBtn) {
        themeBtn.addEventListener("click", () => {
            const current = htmlEl.getAttribute("data-bs-theme") || "light";
            const next = current === "dark" ? "light" : "dark";
            applyTheme(next);
            localStorage.setItem("at-theme", next);

            fetch("/AccountPreferences/SaveTheme", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ theme: next })
            }).catch(() => {});
        });
    }

    // --- Language Toggle ---
    const langBtn = document.getElementById("langToggleBtn");
    const langLabel = document.getElementById("langLabel");

    function getStoredLang() {
        const cookie = getCookie(".AspNetCore.Culture");
        if (cookie && cookie.includes("uz")) return "uz";
        return "en";
    }

    function applyLang(lang) {
        if (langLabel) langLabel.textContent = lang.toUpperCase();
    }

    applyLang(getStoredLang());

    if (langBtn) {
        langBtn.addEventListener("click", () => {
            const current = getStoredLang();
            const next = current === "en" ? "uz" : "en";
            applyLang(next);

            fetch("/AccountPreferences/SaveLanguage", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ language: next })
            }).then(() => {
                window.location.reload();
            }).catch(() => {});
        });
    }

    // --- Cookie helper ---
    function getCookie(name) {
        const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        return match ? decodeURIComponent(match[2]) : null;
    }
});
