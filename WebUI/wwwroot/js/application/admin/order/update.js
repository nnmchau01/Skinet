(function ($) {
    'use strict';

    var submitUpdate = function () {
        $('#btn-submit-update-order').on('click', function (e) {
            e.preventDefault();

            var data = {
                OrderId: $('#OrderId').val(),
                StatusOrderId: $('input[name="OrderStatus"]:checked').val(),
                StatusPaymentId: $('input[name="PaymentStatus"]:checked').val()
            }

            coreAjax(
                true
                , '/Admin/Order/SubmitUpdate'
                , JSON.stringify(data)
                , 'POST'
                , function (res) {
                    toastMessage('success', res);
                    console.log(res.responseText)
                    getListOrder();
                    $('#modal-popup-update').modal('hide');
                }
                , function () {
                }
            );
        })
    }

    //Load functions
    $(document).ready(function () {
        submitUpdate();
    });

})(jQuery);