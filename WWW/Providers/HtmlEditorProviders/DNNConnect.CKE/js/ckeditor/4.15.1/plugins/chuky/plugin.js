CKEDITOR.plugins.add('chuky', {
    icons: 'chuky',
    init: function (editor) {
        editor.addCommand('insertchuky', {
            exec: function (editor) {
                var now = new Date();
                editor.insertHtml('<div class="col-lg-12 signartilce" style=""> <div class="col-lg-4 col-xs-12 signartilcetd1"> <a href="https://capstonevietnam.com" target="_blank"> <img src="https://capstonevietnam.com/static/CapstoneVN/img/logo.png" border="0" alt="Capstone Vietnam" width="150" height="70" style="height: auto; border: 0px;"> </a> </div><div class="col-lg-4 col-xs-6 signartilcetd2"> <p dir="ltr"><strong>Văn phòng Hà Nội</strong></p><p dir="ltr">2 Lê Quý Đôn, P. Bạch Đằng, Q. Hai Bà Trưng</p><p dir="ltr">ĐT: 024.3938 8455</p><p dir="ltr">Hotline: 0936 701 696</p></div><div class="col-lg-4 col-xs-6 signartilcetd3"> <p dir="ltr"><strong>Văn phòng Hồ Chí Minh</strong></p><p dir="ltr">22 Trần Quý Khoách, P .Tân Định, Q.1</p><p dir="ltr">ĐT: 028.3848 2628</p><p dir="ltr">Hotline: 0966 703 162</p></div><div class="clear"></div></div><div class="col-lg-12 signartilces" style=""> <div class="col-lg-4 col-xs-12" style=""> <ul class="topleft-info"><li><a target="_blank" href="https://www.facebook.com/CapstoneVN"><i class="fa fa-facebook" aria-hidden="true"></i></a></li><li><a target="_blank" href="https://twitter.com/capstonevietnam"><i class="fa fa-twitter" aria-hidden="true"></i></a></li><li><a target="_blank" href="https://www.linkedin.com/company/capstonevietnam"><i class="fa fa-linkedin" aria-hidden="true"></i></a></li><li><a target="_blank" href="https://www.youtube.com/channel/UCdg56kflbBVIF7CcU4f0NHw"><i class="fa fa-youtube" aria-hidden="true"></i></a></li></ul> </div><div class="col-lg-8 col-xs-12" style=""> <iframe name="f2c59cc055b645c" width="500" height="80" frameborder="0" allowtransparency="true" allowfullscreen="true" scrolling="no" title="fb:like_box Facebook Social Plugin" src="https://www.facebook.com/plugins/like_box.php?app_id=575447435965848&amp;channel=https%3A%2F%2Fstaticxx.facebook.com%2Fconnect%2Fxd_arbiter%2Fr%2FSh-3BhStODe.js%3Fversion%3D42%23cb%3Df641cbab30d5a8%26domain%3Dcapstonevietnam.com%26origin%3Dhttps%253A%252F%252Fcapstonevietnam.com%252Ff2c58db6113d6f4%26relation%3Dparent.parent&amp;color_scheme=light&amp;container_width=500&amp;header=false&amp;height=80&amp;href=http%3A%2F%2Fwww.facebook.com%2FCapstoneVN&amp;locale=en_US&amp;sdk=joey&amp;show_faces=false&amp;hide_cover=true&amp;stream=false&amp;width=500" style="border: none; visibility: visible; width: 500px; height: 80px;" class=""></iframe></div><div class="clear" style=""></div></div>');
            }
        });
        editor.ui.addButton('chuky', {
            label: 'Chèn chữ ký',
            command: 'insertchuky',
            toolbar: 'insert'
        });
    }
});