(function ($) {
    'use strict';
    
    var submitAddNew = function () {
        
        $('#btn-submit-add-new').on('click', function (e) {
            e.preventDefault();
            
            var data = getFormDataJson('frmSubmit');
            var check = validationForm('frmSubmit');
            var val = data.Name;

            $('#Existed').text('');
            $('#Existed').hide();
            
            if (isExistedAddNew(0, val)){
                $('#Existed').text($('#existedText').val());
                $('#Existed').show();
                check = false;
            }
            
            if (check){
                coreAjax(
                    check
                    , '/Admin/Category/SubmitAddNew'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        toastMessage('success', res);
                        console.log(res.responseText)
                        getListCate();
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