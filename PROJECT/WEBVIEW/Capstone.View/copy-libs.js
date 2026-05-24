const fs   = require("fs");
const path = require("path");

function ensureDir(dir) {
    if (!fs.existsSync(dir)) fs.mkdirSync(dir, { recursive: true });
}
function copy(src, dest) {
    ensureDir(path.dirname(dest));
    fs.copyFileSync(src, dest);
    console.log("  OK  " + dest.replace(__dirname, ""));
}
function copyDir(src, dest) {
    ensureDir(dest);
    for (const entry of fs.readdirSync(src, { withFileTypes: true })) {
        const s = path.join(src, entry.name);
        const d = path.join(dest, entry.name);
        entry.isDirectory() ? copyDir(s, d) : copy(s, d);
    }
}

const nm  = path.join(__dirname, "node_modules");
const lib = path.join(__dirname, "wwwroot", "lib");
const sta = path.join(__dirname, "wwwroot", "static");

console.log("\n=== Copy node_modules -> wwwroot ===\n");

const libMap = [
    [nm+"/jquery/dist/jquery.js",                       lib+"/jquery/dist/jquery.js"],
    [nm+"/jquery/dist/jquery.min.js",                   lib+"/jquery/dist/jquery.min.js"],
    [nm+"/bootstrap/dist/css/bootstrap.min.css",        lib+"/bootstrap/dist/css/bootstrap.min.css"],
    [nm+"/bootstrap/dist/js/bootstrap.bundle.min.js",   lib+"/bootstrap/dist/js/bootstrap.bundle.min.js"],
    [nm+"/jquery-validation/dist/jquery.validate.js",              lib+"/jquery-validation/dist/jquery.validate.js"],
    [nm+"/jquery-validation/dist/jquery.validate.min.js",          lib+"/jquery-validation/dist/jquery.validate.min.js"],
    [nm+"/jquery-validation/dist/additional-methods.js",           lib+"/jquery-validation/dist/additional-methods.js"],
    [nm+"/jquery-validation/dist/additional-methods.min.js",       lib+"/jquery-validation/dist/additional-methods.min.js"],
    [nm+"/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js",     lib+"/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js"],
    [nm+"/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js", lib+"/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js"],
];

for (const [src, dest] of libMap) {
    if (fs.existsSync(src)) copy(src, dest);
    else console.warn("  SKIP: " + src);
}

// Swiper
copy(nm+"/swiper/swiper-bundle.min.js",  sta+"/js/swiper.min.js");
copy(nm+"/swiper/swiper-bundle.min.css", sta+"/css/swiper.min.css");

// AOS
copy(nm+"/aos/dist/aos.js",  sta+"/js/aos.js");
copy(nm+"/aos/dist/aos.css", sta+"/css/aos.css");

// Lazysizes
copy(nm+"/lazysizes/lazysizes.min.js", sta+"/js/lazysizes/lazysizes.min.js");

// Font Awesome
copy(nm+"/@fortawesome/fontawesome-free/css/all.min.css", sta+"/fonts/fontawesome/css/all.min.css");
copyDir(nm+"/@fortawesome/fontawesome-free/webfonts",     sta+"/fonts/fontawesome/webfonts");

// Boxicons
copy(nm+"/boxicons/css/boxicons.min.css", sta+"/fonts/boxicons.min.css");
copyDir(nm+"/boxicons/fonts",             sta+"/fonts/boxicons-fonts");

console.log("\n=== Done! ===\n");