(function ($) {
    'use strict';
    
    var getListServiceForDropdown = function () {
        coreAjax(true, '/Service/GetListFilter', null, 'GET', function (res) {
            setTimeout(function () {
                $("#ServiceId").select2({
                    placeholder: '',
                    data: res,
                    closeOnSelect: true,
                    allowClear: true
                });
            }, 200);
        })
    }

    var getListAvailableTimeForDropdown = function () {
        coreAjax(true, '/Service/GetListAvailableTime', null, 'GET', function (res) {
            setTimeout(function () {
                $("#BookTime").select2({
                    placeholder: '',
                    data: res,
                    closeOnSelect: true,
                    allowClear: true
                });
            }, 200);
        })
    }

    var submitAddNew = function () {

        $('#btn-book').on('click', function (e) {
            e.preventDefault();
            var elMessage = $('#ErrMessage');
            elMessage.hide();
            
            var dateString = $('#BookDate').val();
            var date = new Date(dateString);
            var formattedDate = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();

            var expertDateString = $('#ExpertDate').val();
            var expertDate = new Date(expertDateString);
            var formattedExpertDate = ("0" + expertDate.getDate()).slice(-2) + "/" + ("0" + (expertDate.getMonth() + 1)).slice(-2) + "/" + expertDate.getFullYear();
            
            var check = true;
            
            var data = {
                FullName: $('#FullName').val(),
                Email: $('#Email').val(),
                PhoneNumber: $('#PhoneNumber').val(),
                Note: $('#Note').val(),
                ServiceId: $('#ServiceId').val(),
                Date: dateString ? formattedDate : '',
                Time: $('#BookTime').val(),
                ExpertDate: expertDateString ? formattedExpertDate : '',
            }

            if (data.FullName === ''){
                elMessage.show();
                check = false;
            }

            if (data.Email === ''){
                elMessage.show();
                check = false;
            }


            if (data.PhoneNumber === ''){
                elMessage.show();
                check = false;
            }

            if (data.ServiceId === ''){
                elMessage.show();
                check = false;
            }

            if (data.Date === ''){
                elMessage.show();
                check = false;
            }

            if (data.Time === ''){
                elMessage.show();
                check = false;
            }
            
            if (check){
                coreAjax(
                    check
                    , '/Service/Book'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        toastMessage('success', res);
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
        getListServiceForDropdown();
        getListAvailableTimeForDropdown();
        submitAddNew();
    });

})(jQuery);