(function ($) {
    'use strict';

    var onViewDetail = function () {
        $('.btn-view-detail').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            coreAjaxGetPartialView(true, '/Admin/Order/DetailPartial?id=' + id, null, function (response) {
                $("#modal-body").html(response);
                $('#modal-popup').modal('show');
            }, function () {

            });
        });
    }
    
    var onUpdate = function () {
        $('.btn-edit').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            coreAjaxGetPartialView(true, '/Admin/Order/UpdatePartial?id=' + id, null, function (response) {
                $("#modal-body-update").html(response);
                $('#modal-popup-update').modal('show');
            }, function () {

            });
        });
    }

    //Load functions
    $(document).ready(function () {
        onViewDetail();
        onUpdate();
    });

})(jQuery);