(function ($) {
    'use strict';

    var submitUpdate = function () {

        $('#btn-submit-update').on('click', function (e) {
            e.preventDefault();

            var data = getFormDataJson('frmSubmit');
            var check = validationForm('frmSubmit');
            var currentValue = $('#ServiceName').val();
            var newValue = data.ServiceName;

            $('#Existed').text('');
            $('#Existed').hide();
            
            if (isExistedUpdate(0, currentValue, newValue)) {
                $('#Existed').text($('#existedText').val());
                $('#Existed').show();
                check = false;
            }

            if (check) {
                data.Price = data.Price.replaceAll('.', '');
                coreAjax(
                    check
                    , '/Admin/Service/SubmitUpdate'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        toastMessage('success', res);
                        console.log(res.responseText)
                        getListService();
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
        onChangePrice();
    });

})(jQuery);