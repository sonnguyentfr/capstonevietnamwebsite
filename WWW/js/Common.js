/*
    url: vi du  'http://thanhtra.com.vn'
    <a href="#" class="datlamtrangchu" onclick="SetHomepage();">Đặt làm trang chủ</a>
*/
function SetHomepage(url) {
    if (document.all) {
        document.body.style.behavior = 'url(#default#homepage)';
        document.body.setHomePage(url);
    }
    else if (window.sidebar) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            }
            catch (e) {
                alert("Vì lý do bảo mật, mặc định firefox không cho phép thực hiện hành động này，nếu bạn muốn cho phép，hãy gõ about:config trên ô địa chỉ và thay đổi giá trị của signed.applets.codebase_principal_support thành true");
            }
        }
        var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
        prefs.setCharPref('browser.startup.homepage', url);
    }
}
/* Load Css */
function LoadCss(href)
{
    var tags = document.getElementsByTagName("LINK");     
    var done = false;            
    for(var i=0; i<tags.length; i++)
    {   
        if(tags[i].getAttribute("href")== href || tags[i].getAttribute("id")== href)
        {   
            done = true;
            break;
        }       
    }  
    if(!done)
    {      
        var newsCssNode = document.createElement("LINK");
        newsCssNode.href = href;
        newsCssNode.rel = "stylesheet";
        newsCssNode.type = "text/css";
        document.getElementsByTagName("HEAD")[0].appendChild(newsCssNode);
    }
}
/* End Load Css */
function LoadJs(src)
{
    var tags = document.getElementsByTagName("SCRIPT");     
    var done = false;    
    for(var i=0; i<tags.length; i++)
    {   
        if(tags[i].getAttribute("src")== src || tags[i].getAttribute("id")== src)
        {           
            done = true;
            break;
        }       
    }  
    if(!done)
    {      
        var newsJsNode = document.createElement("SCRIPT");
        newsJsNode.src = src;        
        newsJsNode.type = "text/javascript";
        document.getElementsByTagName("HEAD")[0].appendChild(newsJsNode);
    }
}
function showSelects(){
   var elements = document.getElementsByTagName("select");
   for (i=0;i< elements.length;i++){
      elements[i].style.visibility='visible';
   }
}
function hideSelects(){
   var elements = document.getElementsByTagName("select");
   for (i=0;i< elements.length;i++){
   elements[i].style.visibility='hidden';
   }
}
function getDecodeString(val) {
    try {
        return decodeURIComponent(val);
    }
    catch (e) {
        return unescape(val);
    }
}
function getEncodedString(val) {
    try {
        return encodeURI(unescape(val));
    }
    catch (e) {
        return val;
    }
}
function ViewMedia(mfile) {
    var extension = mfile.substr((mfile.lastIndexOf('.') + 1)).toLowerCase();
    switch (extension) {
        case 'flv':
            var soptions = "width=300,height=225,status=no,location=no,menubar=no,resizable=no,scrollbars=no,toolbar=no,titlebar=no,top=50,left=100";
            window.open("/DesktopModules/AIMedia/ListByFilter/ViewMediaflv.htm?mfile=" + Base64.encode(mfile), "", soptions);
            break;
        case 'mp3':
        case 'wav':
            var soptions = "width=420,height=80,status=no,location=no,menubar=no,resizable=no,scrollbars=no,toolbar=no,titlebar=no,top=50,left=100";
            window.open("/DesktopModules/AIMedia/ListByFilter/ViewMedia.htm?mfile=" + Base64.encode(mfile), "", soptions);
            break;
        case 'mp4':
        case 'mpeg':
            var soptions = "width=670,height=400,status=no,location=no,menubar=no,resizable=no,scrollbars=no,toolbar=no,titlebar=no,top=50,left=100";
            window.open("/DesktopModules/AIMedia/ListByFilter/ViewVideo.htm?mfile=" + Base64.encode(mfile), "", soptions);
            break;

    }
}