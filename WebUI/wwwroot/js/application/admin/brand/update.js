(function ($) {
    'use strict';

    var submitUpdate = function () {

        $('#btn-submit-update').on('click', function (e) {
            e.preventDefault();

            var data = getFormDataJson('frmSubmit');
            var check = validationForm('frmSubmit');
            var currentValue = $('#CategoryName').val();
            var newValue = data.Name;

            $('#Existed').text('');
            $('#Existed').hide();

            if (currentValue === newValue) {
                $('#modal-popup').modal('hide')
                return;
            }
            
            if (isExistedUpdate(0, currentValue, newValue)) {
                $('#Existed').text($('#existedText').val());
                $('#Existed').show();
                check = false;
            }

            if (check) {
                coreAjax(
                    check
                    , '/Admin/Brand/SubmitUpdate'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        toastMessage('success', res);
                        console.log(res.responseText)
                        getListBrand();
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