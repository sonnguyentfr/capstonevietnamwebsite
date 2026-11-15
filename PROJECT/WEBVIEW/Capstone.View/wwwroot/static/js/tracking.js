/* Tracking lead & lien he cho GA4 (+ Meta Pixel).
   - window.cvTrack(ten_su_kien, tham_so): gui event GA4 (gtag; neu khong co gtag thi dataLayer cho GTM).
     generate_lead dong thoi gui fbq('track','Lead') neu co Meta Pixel.
   - Tu dong bat click: tel: -> click_hotline, mailto: -> click_email, Zalo -> click_zalo, Messenger -> click_messenger.
   GA4 tu gan utm_* cua phien vao tung event nen bao cao Key event se chia duoc theo nguon / chien dich. */
(function (w, d) {
    // Site chi con Google tag nap qua GTM (G-9NQQM9KEWB) sau khi go ma gtag cu trong Header/Footer,
    // nen tu dinh nghia gtag() neu chua co: lenh se vao dataLayer chung cho GTM / gtag.js xu ly.
    w.dataLayer = w.dataLayer || [];
    if (typeof w.gtag !== 'function') {
        w.gtag = function () { w.dataLayer.push(arguments); };
    }

    function track(name, params) {
        params = params || {};
        try {
            w.gtag('event', name, params);
            if (name === 'generate_lead' && typeof w.fbq === 'function') {
                w.fbq('track', 'Lead', { content_name: params.lead_type || 'lead' });
            }
        } catch (e) { /* tracking khong duoc lam hong trang */ }
    }
    w.cvTrack = track;

    function where(el) {
        return el.closest('.cta-fixed') ? 'floating'
             : el.closest('footer') ? 'footer'
             : el.closest('header') ? 'header' : 'content';
    }

    d.addEventListener('click', function (e) {
        var a = e.target && e.target.closest ? e.target.closest('a[href]') : null;
        if (!a) return;
        var href = (a.getAttribute('href') || '').toLowerCase();
        var name = null;
        var p = { link_location: where(a), link_url: href.slice(0, 120) };
        if (href.indexOf('tel:') === 0) { name = 'click_hotline'; p.phone = href.slice(4); }
        else if (href.indexOf('mailto:') === 0) { name = 'click_email'; }
        else if (/zalo\.me|chat\.zalo\.me/.test(href) || a.querySelector('img[src*="zalo"]')) { name = 'click_zalo'; }
        else if (/m\.me\/|messenger\.com/.test(href) || a.querySelector('img[src*="mess"]')) { name = 'click_messenger'; }
        if (name) track(name, p);
    }, true);
})(window, document);
