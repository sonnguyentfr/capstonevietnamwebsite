/* ============================================================
   Form tư vấn nhanh – sidebar trang chi tiết trường / tin tức
   (Views/Shared/Components/TuVanNhanh/Default.cshtml)
   ============================================================ */
(function () {
    var root = document.getElementById('tuvan-nhanh');
    if (!root || root.dataset.ready) return;
    root.dataset.ready = '1';

    var form      = document.getElementById('tvn-form');
    var alertBox  = document.getElementById('tvn-alert');
    var submitBtn = document.getElementById('tvn-submit');
    var chipsBox  = document.getElementById('tvn-countries');
    var moreSel   = document.getElementById('tvn-country-more');
    var hiddenBox = document.getElementById('tvn-country-inputs');
    var siteKey   = root.dataset.recaptcha || '';
    var maxCountry = parseInt(root.dataset.maxCountry || '10', 10);

    // ── reCAPTCHA v3: chỉ tải khi khách bắt đầu tương tác ──────────────
    var recaptchaLoading = false;
    function loadRecaptcha() {
        if (!siteKey || recaptchaLoading || window.grecaptcha) return;
        recaptchaLoading = true;
        var s = document.createElement('script');
        s.src = 'https://www.google.com/recaptcha/api.js?render=' + encodeURIComponent(siteKey);
        s.async = true;
        document.head.appendChild(s);
    }
    form.addEventListener('focusin', loadRecaptcha, { once: true });
    form.addEventListener('pointerdown', loadRecaptcha, { once: true });

    function getToken() {
        return new Promise(function (resolve, reject) {
            if (!siteKey) return resolve('');
            loadRecaptcha();
            var tries = 0;
            (function wait() {
                if (window.grecaptcha && window.grecaptcha.ready) {
                    window.grecaptcha.ready(function () {
                        window.grecaptcha.execute(siteKey, { action: 'tu_van_nhanh' }).then(resolve, reject);
                    });
                } else if (++tries > 50) {
                    reject(new Error('recaptcha timeout'));
                } else {
                    setTimeout(wait, 200);
                }
            })();
        });
    }

    // ── Quốc gia: chip phổ biến + chọn thêm từ danh sách ────────────────
    var selected = []; // [{id, title}]

    function renderCountries() {
        hiddenBox.innerHTML = '';
        selected.forEach(function (c) {
            var inp = document.createElement('input');
            inp.type = 'hidden';
            inp.name = 'QuocGiaIds';
            inp.value = c.id;
            hiddenBox.appendChild(inp);
        });
        chipsBox.querySelectorAll('.tvn__chip').forEach(function (chip) {
            var on = selected.some(function (c) { return c.id === chip.dataset.id; });
            chip.classList.toggle('is-on', on);
            chip.setAttribute('aria-pressed', on ? 'true' : 'false');
        });
    }
    function toggleCountry(id, title, chipEl) {
        var idx = selected.findIndex(function (c) { return c.id === id; });
        if (idx >= 0) {
            selected.splice(idx, 1);
            if (chipEl && chipEl.dataset.extra) chipEl.remove();
        } else {
            if (selected.length >= maxCountry) {
                showAlert('Chỉ chọn tối đa ' + maxCountry + ' quốc gia.', 'error');
                return;
            }
            selected.push({ id: id, title: title });
        }
        hideAlert();
        renderCountries();
    }

    chipsBox.addEventListener('click', function (e) {
        var chip = e.target.closest('.tvn__chip');
        if (!chip) return;
        toggleCountry(chip.dataset.id, chip.textContent.trim(), chip);
    });

    moreSel.addEventListener('change', function () {
        var opt = moreSel.options[moreSel.selectedIndex];
        var id = moreSel.value;
        moreSel.value = '';
        if (!id || selected.some(function (c) { return c.id === id; })) return;
        if (selected.length >= maxCountry) {
            showAlert('Chỉ chọn tối đa ' + maxCountry + ' quốc gia.', 'error');
            return;
        }
        var chip = document.createElement('button');
        chip.type = 'button';
        chip.className = 'tvn__chip';
        chip.dataset.id = id;
        chip.dataset.extra = '1';
        chip.textContent = opt.text;
        chipsBox.appendChild(chip);
        toggleCountry(id, opt.text, chip);
    });

    // ── Validate ──────────────────────────────────────────────────────────
    function val(name) {
        var el = form.elements[name];
        return el ? String(el.value || '').trim() : '';
    }
    function mark(name, bad) {
        var el = form.elements[name];
        if (el && el.classList) el.classList.toggle('is-invalid', !!bad);
    }
    function isEmail(v) { return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v); }
    function isPhone(v) { return /^(0|\+?84)(2|3|5|7|8|9)[0-9]{8}$/.test(v.replace(/[\s\-\.\(\)]/g, '')); }

    function validate() {
        var errors = [];
        function check(name, ok, msg) { mark(name, !ok); if (!ok) errors.push(msg); }

        check('HoVaTen', val('HoVaTen').length > 0, 'Vui lòng nhập họ và tên.');

        var d = +val('NgaySinhNgay'), m = +val('NgaySinhThang'), y = +val('NgaySinhNam');
        var dt = new Date(y, m - 1, d);
        var dobOk = d && m && y && dt.getDate() === d && dt.getMonth() === m - 1;
        ['NgaySinhNgay', 'NgaySinhThang', 'NgaySinhNam'].forEach(function (n) { mark(n, !dobOk); });
        if (!dobOk) errors.push('Vui lòng chọn ngày sinh hợp lệ.');

        if (!form.querySelector('input[name="GioiTinh"]:checked')) errors.push('Vui lòng chọn giới tính.');

        var email = val('Email'), phone = val('SoDienThoai');
        var noEmail = !email || email.toUpperCase() === 'NA';
        var noPhone = !phone || phone === '0';
        mark('Email', false); mark('SoDienThoai', false);
        if (noEmail && noPhone) {
            mark('Email', true); mark('SoDienThoai', true);
            errors.push('Vui lòng nhập Email hoặc Số điện thoại.');
        } else {
            if (!email) { mark('Email', true); errors.push('Không có email thì nhập NA.'); }
            else if (!noEmail && !isEmail(email)) { mark('Email', true); errors.push('Email không đúng định dạng.'); }
            if (!phone) { mark('SoDienThoai', true); errors.push('Không có số điện thoại thì nhập 0.'); }
            else if (!noPhone && !isPhone(phone)) { mark('SoDienThoai', true); errors.push('Số điện thoại không hợp lệ.'); }
        }

        check('TinhId', !!val('TinhId'), 'Vui lòng chọn tỉnh/thành.');

        moreSel.classList.toggle('is-invalid', selected.length === 0);
        if (selected.length === 0) errors.push('Vui lòng chọn ít nhất một quốc gia.');

        if (!form.elements['DongYDieuKhoan'].checked) errors.push('Bạn phải đồng ý điều khoản bảo mật.');

        return errors;
    }

    form.addEventListener('input', function (e) { if (e.target.classList) e.target.classList.remove('is-invalid'); });
    form.addEventListener('change', function (e) { if (e.target.classList) e.target.classList.remove('is-invalid'); });

    // ── Alert ─────────────────────────────────────────────────────────────
    function showAlert(msg, type) {
        alertBox.hidden = false;
        alertBox.className = 'tvn__alert tvn__alert--' + (type || 'error');
        if (Array.isArray(msg)) {
            alertBox.innerHTML = '';
            var ul = document.createElement('ul');
            msg.forEach(function (t) { var li = document.createElement('li'); li.textContent = t; ul.appendChild(li); });
            alertBox.appendChild(ul);
        } else {
            alertBox.textContent = msg;
        }
    }
    function hideAlert() { alertBox.hidden = true; }

    function setLoading(on) {
        submitBtn.disabled = on;
        submitBtn.classList.toggle('is-loading', on);
    }

    // ── Submit (AJAX) ─────────────────────────────────────────────────────
    form.addEventListener('submit', function (e) {
        e.preventDefault();
        var errors = validate();
        if (errors.length) {
            showAlert(errors, 'error');
            var bad = form.querySelector('.is-invalid');
            if (bad) bad.focus();
            return;
        }
        hideAlert();
        setLoading(true);

        getToken().then(function (token) {
            var fd = new FormData(form);
            if (token) fd.append('g-recaptcha-response', token);
            return fetch(form.action, { method: 'POST', body: fd, headers: { 'X-Requested-With': 'XMLHttpRequest' } });
        }).then(function (res) {
            if (!res.ok) throw new Error('HTTP ' + res.status);
            return res.json();
        }).then(function (data) {
            if (data && data.success) {
                form.hidden = true;
                document.getElementById('tvn-done-msg').textContent = data.message;
                document.getElementById('tvn-done').hidden = false;
            } else {
                showAlert((data && (data.errors && data.errors.length ? data.errors : data.message)) || 'Có lỗi xảy ra. Vui lòng thử lại.', 'error');
            }
        }).catch(function () {
            showAlert('Có lỗi xảy ra. Vui lòng thử lại sau.', 'error');
        }).then(function () {
            setLoading(false);
        });
    });
})();
