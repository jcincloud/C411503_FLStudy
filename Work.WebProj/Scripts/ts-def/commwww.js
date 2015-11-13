;
$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('a.goTop').fadeIn();
        }
        else {
            $('a.goTop').fadeOut();
        }
    });
    $('a.goTop').click(function () {
        $('html, body').animate({
            scrollTop: $($.attr(this, 'href')).offset().top
        }, 1000);
        return false;
    });
    $('#menu-toggle').click(function () {
        $('.md-overlay').addClass("show");
        return false;
    });
    $('.main-nav .md-close, .md-overlay').click(function () {
        $('.md-overlay').removeClass("show");
        return false;
    });
    $("#loginMember").submit(function (event) {
        event.preventDefault();
        var act = $('#act').val();
        var pwd = $('#pwd').val();
        var data = {
            "act": act,
            "pwd": pwd
        };
        jqPost(gb_approot + 'Sys_Base/MNGLogin/ajax_MemberLogin', data)
            .done(function (data, textStatus, jqXHRdata) {
            if (!data.result) {
                alert(data.message);
                $('#pwd').val('');
            }
            else {
                document.location.href = gb_approot + 'Sys_Active/MemberData';
            }
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
            showAjaxError(errorThrown);
        });
    });
    $("#joinInfo").submit(function (event) {
        event.preventDefault();
        var m_name = $("#m_name").val();
        var m_tel = $("#m_tel").val();
        var m_addr = $("#m_addr").val();
        var m_email = $("#m_email").val();
        var m_interest = $("#m_interest").val();
        var data = {
            "name": m_name,
            "tel": m_tel,
            "addr": m_addr,
            "email": m_email,
            "interest": m_interest
        };
        jqPost(gb_approot + 'Index/ajaxSendMail', data)
            .done(function (data, textStatus, jqXHRdata) {
            if (!data.result) {
                alert(data.message);
            }
            else {
                alert('申請已送出，我們會儘快與您聯絡!');
                $("#m_name").val('');
                $("#m_tel").val('');
                $("#m_addr").val('');
                $("#m_email").val('');
                $("#m_interest").val('');
            }
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
            showAjaxError(errorThrown);
        });
    });
});
