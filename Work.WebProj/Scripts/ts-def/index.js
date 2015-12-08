// 頁籤切換
$(function () {
    // 預設顯示第一個 Tab
    var _showTab = 0;
    var $defaultLi = $('.tabs li').eq(_showTab).addClass('active');
    $($defaultLi.find('a').attr('href')).siblings().hide();
    // 當 li 頁籤被點擊時...
    // 若要改成滑鼠移到 li 頁籤就切換時, 把 click 改成 mouseover
    $('.tabs li').mouseover(function () {
        // 找出 li 中的超連結 href(#id)
        var $this = $(this), _clickTab = $this.find('a').attr('href');
        // 把目前點擊到的 li 頁籤加上 .active
        // 並把兄弟元素中有 .active 的都移除 class
        $this.addClass('active').siblings('.active').removeClass('active');
        // 淡入相對應的內容並隱藏兄弟元素
        $(_clickTab).stop(false, true).fadeIn().siblings().hide();
        return false;
    }).find('a').focus(function () {
        this.blur();
    });
});
//# sourceMappingURL=index.js.map