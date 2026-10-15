// Light / dark theme toggle. The initial theme is set by an inline script in
// _Layout.cshtml so the page doesn't flash the wrong colours on load.
document.querySelectorAll("[data-theme-toggle]").forEach(button => {
    button.addEventListener("click", () => {
        const next = document.documentElement.getAttribute("data-bs-theme") === "dark" ? "light" : "dark";
        document.documentElement.setAttribute("data-bs-theme", next);
        try { localStorage.setItem("theme", next); } catch { }
    });
});

// Filter forms submit as soon as a dropdown or checkbox changes.
document.querySelectorAll("form[data-auto-submit]").forEach(form => {
    form.querySelectorAll("select, input[type=checkbox]").forEach(input => {
        input.addEventListener("change", () => form.requestSubmit());
    });
});
