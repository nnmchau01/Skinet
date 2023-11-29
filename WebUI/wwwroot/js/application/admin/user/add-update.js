(function ($) {
    'use strict';

    var submitAddNewUser = function () {
        $('#btn-submit-add-new').on('click', function (e) {
            e.preventDefault();
            var data = getFormDataJson('frmSubmit');
            var check = validationForm('frmSubmit');
            var roles = [];

            $('#sRole').hide()
            $("input[name='Role']").each(function () {
                if ($(this).is(":checked")) {
                    roles.push($(this).val())
                }
            });

            if (roles.length === 0) {
                $('#sRole').show()
                check = false;
            }

            if (check) {
                data.Roles = roles;
                coreAjax(
                    check
                    , '/Admin/User/SubmitAddUser'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        toastMessage('success', res);
                        console.log(res.responseText)
                        getListUser();
                        $('#modal-popup').modal('hide');
                    }
                    , function () {
                    }
                );
            }
        });
    }

    var updateRole = function () {
        $('#btn-submit-update-role').on('click', function (e) {
            var roles = [];
            var check = true;
            var data = {
                Id: $('#UserId').val()
            };

            $('#sRole').hide()
            $("input[name='Role']").each(function () {
                if ($(this).is(":checked")) {
                    roles.push($(this).val())
                }
            });

            if (roles.length === 0) {
                $('#sRole').show()
                check = false;
            }

            if (check) {
                data.Roles = roles;
                coreAjax(
                    check
                    , '/Admin/User/SubmitUpdateRole'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        toastMessage('success', res);
                        getListUser();
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
        submitAddNewUser();
        updateRole();
    });

})(jQuery);