(function ($) {
    'use strict';
    
    var submitSignInOrSignUp = function () {
        $('#btn-sign-in').on('click', function () {
            var data = getFormDataJson('frmLogin');
            var check = validationForm('frmLogin');
            
            if (check){
                coreAjax(
                    check
                    , '/Authenticate/SignIn'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) { window.location.reload(); }
                    , function () { }
                );
            }
        });

        $('#btn-sign-up').on('click', function (e) {
            e.preventDefault();
            var data = getFormDataJson('frmSubmitSignUp');
            var check = validationForm('frmSubmitSignUp');

            if (check){
                coreAjax(
                    check
                    , '/Authenticate/SignUp'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) { window.location.reload(); }
                    , function () { }
                );
            }
        });
    }

    //Load functions
    $(document).ready(function () {
        submitSignInOrSignUp();
    });

})(jQuery);