(function ($) {
    'use strict';

    var getHeader = function () {
        coreAjaxGetPartialView(true, '/Common/GetHeader', null, function (response) {
            $("#div-header").html(response);
        }, function () {
        });
    }

    var getMenu = function () {
        coreAjaxGetPartialView(true, '/Common/GetMenu', null, function (response) {
            $("#div-menu").html(response);
        }, function () {
        });
    }

    var initSearch = function () {
        var text = getParamFromUrl('searchText', window.location.href) ? getParamFromUrl('searchText', window.location.href) : "";
        if (text) {
            $('#searchText').val(text);
        }

        $('#btn-search').on('click', function (e) {
            e.preventDefault();
            var url = window.location.href;
            var page = getParamFromUrl('page', url) ? getParamFromUrl('page', url) : 1;
            var searchText = $('#searchText').val();

            window.location.href = window.location.origin + '/Product/Search?searchText=' + searchText + '&page=' + page
        });

        $('#btn-load-more').on('click', function () {
            window.location.href = '/Product/Search'
        })
    }

    var initSearchRangePrice = function () {
        $('#btn-search-by-price').on('click', function () {
            var my_range = $(".js-range-slider-custom").data("ionRangeSlider");
            var min = my_range.result.from;
            var max = my_range.result.to;

            window.location.href = '/Product/SearchRangePrice?min=' + min + '&max=' + max;
        })
    }

    var rangeSlider = function () {
        // init slider range
        var min = parseInt($('#MinPrice').val())
        var max = parseInt($('#MaxPrice').val())
        $(".js-range-slider-custom").ionRangeSlider({
            type: "double",
            // grid: true,
            min: isNaN(min) ? 0 : min,
            max: isNaN(max) ? 0 : max,
            from: min,
            to: max,
            prefix: "$",
            //postfix: " $",
            // values: custom_values,
            step: 1
        });
    }

    var handleOrder = function () {
        var textSuccess = $('#IsEnglishVersion').val() === 'true' ? 'Order successfully. \n Your order code is: \n' : 'Đặt hàng thành công. \n Mã đơn hàng của bạn là: \n'
        var textFail = $('#IsEnglishVersion').val() === 'true' ? 'Order failure' : 'Đặt hàng thất bại'
        var success = getParamFromUrl('success', window.location.href);

        if (success === 'true') {
            coreAjax(
                true
                , '/Payment/CompleteOrder'
                , localStorage.getItem('orderInfo')
                , 'POST'
                , function (res) {
                    localStorage.removeItem('cart');
                    localStorage.removeItem('orderInfo');
                    $('#total-item-cart').text(0);
                    sendMessage('order', 'new order');
                    window.history.pushState("data", "Title", window.location.origin);

                    Swal.fire(
                        textSuccess + res,
                        '',
                        'success'
                    )
                }
                , function () {
                }
            );
        } else if (success === 'false') {
            Swal.fire(
                textFail,
                '',
                'error'
            )
        }
    }

    var onViewOrderDetail = function () {
        $('#btn-check-order').on('click', function (e) {
            e.preventDefault();
            $('#sOrderCode').hide();
            var code = $('#OrderCode').val();
            if (code === '') {
                $('#sOrderCode').show();
            } else {
                coreAjaxGetPartialView(true, '/Common/GetOrderDetailPartial/?code=' + code, null, function (response) {
                    $('#modal-popup-order-detail-check').modal('hide');
                    $("#modal-body-order-detail").html(response);
                    $('#modal-popup-order-detail').modal('show');
                }, function () {

                });
            }
        });
    }
    
    //Load functions
    $(document).ready(function () {
        onViewOrderDetail();
        handleOrder();
        $('#total-item-cart').text(getDataCart().length);
        getHeader();
        getMenu();
        initSearch();
        initSearchRangePrice();
        rangeSlider();
        $('.modal').modal({
            backdrop: 'static',
            keyboard: false,
            show: false
        });
        setTimeout(function () {
            if (window.location.pathname === '/' && window.location.hash === '#login'){
                $('#modal-sign-in').modal('show');
            }
        }, 1000)
    });

})(jQuery);