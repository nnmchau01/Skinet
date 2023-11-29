(function ($) {
    'use strict';
    
    var submitAddNew = function () {
        
        $('#btn-submit-add-new').on('click', function (e) {
            e.preventDefault();
            
            var data = getFormDataJson('frmSubmit');
            var check = validationForm('frmSubmit');
            var val = data.ServiceName;

            $('#Existed').text('');
            $('#Existed').hide();
            
            if (isExistedAddNew(0, val)){
                $('#Existed').text($('#existedText').val());
                $('#Existed').show();
                check = false;
            }
            
            if (check){
                data.Price = data.Price.replaceAll('.', '');
                coreAjax(
                    check
                    , '/Admin/Service/SubmitAddNew'
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
        submitAddNew();
        onChangePrice();
    });

})(jQuery);