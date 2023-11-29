(function ($) {
    'use strict';
    
    var getYourCart = function () {
        coreAjaxPostPartialView(true, '/Cart/GetCartFromCheckout', localStorage.getItem('cart'), function (response) {
            $("#checkout-cart").html(response);
        }, function () {

        });
    }

    var processCheckout = function () {
        $('#btn-submit-order').on('click', function (e) {
            e.preventDefault();
            var data = getFormDataJson('frmSubmitCheckout');
            var check = validationForm('frmSubmitCheckout');
            data.Carts = JSON.parse(localStorage.getItem('cart'))
            data.PaymentMethod = $('input[name=Payment]:checked').val()
            
            if (check){
                coreAjax(
                    check
                    , '/Payment/ProcessCheckout'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        localStorage.setItem('orderInfo', JSON.stringify(data))
                        setTimeout(function () {
                            window.location.href = res;
                        }, 500)
                    }
                    , function () { }
                );
            }
        });
    }
    
    //Load functions
    $(document).ready(function () {
        getYourCart();
        processCheckout();
    });

})(jQuery);