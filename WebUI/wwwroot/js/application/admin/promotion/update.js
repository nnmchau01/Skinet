(function ($) {
    'use strict';

    var submitUpdate = function () {
        $('#DiscountPercent').on('keyup', function () {
            var p = parseInt($('#DiscountPercent').val())

            if (isNaN(p) || p <= 0) {
                $('#sDiscountPercent').show()
                $('#DiscountPercent').val('')
            }else{
                $('#sDiscountPercent').hide()
            }
        })
        
        $('#btn-submit-update').on('click', function (e) {
            e.preventDefault();

            var data = getFormDataJson('frmSubmit');
            var check = validationForm('frmSubmit');

            if (check) {
                coreAjax(
                    check
                    , '/Admin/Promotion/SubmitUpdate'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        toastMessage('success', res);
                        console.log(res.responseText)
                        getListPromotion();
                        $('#modal-popup').modal('hide');
                    }
                    , function () {
                    }
                );
            }
        })
    }

    //Load functions
    $(document).ready(function () {
        submitUpdate();
    });

})(jQuery);