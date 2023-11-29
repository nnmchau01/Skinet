(function ($) {
    'use strict';
    var current = $('#PValid').text();
    
    var firstLoadOrRefresh = function () {
        var dataCartFirst = JSON.parse(localStorage.getItem('cart'))

        for(var item of dataCartFirst){
            item.PromotionCode = ''
        }

        localStorage.removeItem('cart');
        localStorage.setItem('cart', JSON.stringify(dataCartFirst))
    }
    
    var getYourCart = function () {
        coreAjaxPostPartialView(true, '/Cart/GetCart', localStorage.getItem('cart'), function (response) {
            $("#your-cart").html(response);
        }, function () {

        });
    }

    var getPromotion = function () {
        $('#btn-get-promotion').on('click', function (e) {
            e.preventDefault();
            $('#PRequired').hide();
            $('#PValid').hide();
            $('#PInvalid').hide();
            $('#PValid').text(current);
            
            if ($('#PromotionCode').val()===''){
                $('#PRequired').show();
                return;
            }

            coreAjax(
                true
                , '/Common/GetPromotion?code=' + $('#PromotionCode').val()
                , null
                , 'GET'
                , function (res) {
                    if (res.code){
                        var text = $('#PValid').text();
                        var newText = text + ' ' + res.discountPercent + '%';
                        $('#PValid').text(newText);
                        $('#PValid').show();
                        
                        var dataCart = JSON.parse(localStorage.getItem('cart'))
                        
                        for(var item of dataCart){
                            item.PromotionCode = res.code
                        }
                        
                        localStorage.removeItem('cart');
                        localStorage.setItem('cart', JSON.stringify(dataCart))
                        
                        getYourCart();
                        
                    }else{
                        $('#PInvalid').show();
                    }
                }
                , function () { }
            );
        })
    }
    
    //Load functions
    $(document).ready(function () {
        firstLoadOrRefresh();
        getYourCart();
        getPromotion();
    });

})(jQuery);