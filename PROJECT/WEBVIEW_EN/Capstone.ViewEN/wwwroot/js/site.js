// Capstone Vietnam EN — small progressive enhancements. The site works without this file.
// Uses event delegation so it keeps working after Blazor enhanced navigation swaps the page.
(function () {
  "use strict";

  function closeNav() {
    var nav = document.getElementById("site-nav");
    var toggle = document.querySelector("[data-nav-toggle]");
    if (!nav || !toggle) return;
    nav.classList.remove("is-open");
    toggle.setAttribute("aria-expanded", "false");
    toggle.setAttribute("aria-label", "Open menu");
    document.body.classList.remove("nav-open");
  }

  document.addEventListener("click", function (e) {
    var toggle = e.target.closest("[data-nav-toggle]");
    if (toggle) {
      var nav = document.getElementById("site-nav");
      var open = !nav.classList.contains("is-open");
      nav.classList.toggle("is-open", open);
      toggle.setAttribute("aria-expanded", String(open));
      toggle.setAttribute("aria-label", open ? "Close menu" : "Open menu");
      document.body.classList.toggle("nav-open", open);
      return;
    }

    var expand = e.target.closest("[data-nav-expand]");
    if (expand) {
      var item = expand.closest(".nav__item");
      var expanded = !item.classList.contains("is-expanded");
      item.classList.toggle("is-expanded", expanded);
      expand.setAttribute("aria-expanded", String(expanded));
      return;
    }

    // Following a link inside the mobile menu closes it.
    if (e.target.closest("#site-nav a")) closeNav();
  });

  document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") closeNav();
  });

  // Header gets a hairline + shadow once the page scrolls.
  var ticking = false;
  window.addEventListener("scroll", function () {
    if (ticking) return;
    ticking = true;
    requestAnimationFrame(function () {
      var header = document.querySelector("[data-header]");
      if (header) header.classList.toggle("is-scrolled", window.scrollY > 8);
      ticking = false;
    });
  }, { passive: true });

  // After an enhanced navigation the new page starts with the menu closed.
  if (window.Blazor && typeof window.Blazor.addEventListener === "function") {
    window.Blazor.addEventListener("enhancedload", closeNav);
  }
})();
