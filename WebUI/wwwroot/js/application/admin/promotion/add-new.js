(function ($) {
    'use strict';
    
    var submitAddNew = function () {
        $('#DiscountPercent').on('keyup', function () {
            var p = parseInt($('#DiscountPercent').val())

            if ($('#DiscountPercent').val()){
                if (isNaN(p) || p <= 0) {
                    $('#sDiscountPercent').show()
                    $('#DiscountPercent').val('')
                }else{
                    $('#sDiscountPercent').hide()
                }
            }
        })
        
        $('#btn-submit-add-new').on('click', function (e) {
            e.preventDefault();
            $('#sDiscountPercent').hide();
            
            var data = getFormDataJson('frmSubmit');
            var check = validationForm('frmSubmit');
            
            if (check){
                coreAjax(
                    check
                    , '/Admin/Promotion/SubmitAddNew'
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
        submitAddNew();
    });

})(jQuery);