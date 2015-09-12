var browserInfo = (function () {
    var ua = navigator.userAgent, tem,
    M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*([\d\.]+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+(\.\d+)?)/g.exec(ua) || [];

        var r = { browser: 'IE', version: (tem[1] || '') }
        return r;
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];

    return { browser: M[0], version: M[1] };
})();
if (browserInfo.browser == 'IE' || browserInfo.browser == 'MSIE') {
    if (
        browserInfo.version == '9.0' ||
        browserInfo.version == '8.0' ||
        browserInfo.version == '7.0' ||
        browserInfo.version == '6.0'
        ) {
        alert(browserInfo.browser + browserInfo.version + '本網站不支援此瀏覽器版本，請使用IE 10.0以上之版本、Google Chrome或FireFox等瀏覽器!');
        //window.history.back();
        window.location.href = "/Content/images/noIE/noIE.html";
    }
}