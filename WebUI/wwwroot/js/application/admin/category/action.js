(function ($) {
    'use strict';

    var onViewDetail = function () {
        $('.btn-view-detail').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            coreAjaxGetPartialView(true, '/Admin/Category/DetailPartial?id=' + id, null, function (response) {
                $("#modal-body").html(response);
                $('#modal-popup').modal('show');
            }, function () {

            });
        });
    }

    var onDelete = function () {
        $('.btn-delete').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            sweetAlertDeleteConfirmation('/Admin/Category/Delete', id, $('#TitleDelete').val(), $('#TextDelete').val(),
                function() { getListCate() })
        });
    }

    var onUpdate = function () {
        $('.btn-edit').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            coreAjaxGetPartialView(true, '/Admin/Category/UpdatePartial?id=' + id, null, function (response) {
                $("#modal-body").html(response);
                $('#modal-popup').modal('show');
            }, function () {

            });
        });
    }

    //Load functions
    $(document).ready(function () {
        onViewDetail();
        onDelete();
        onUpdate();
    });

})(jQuery);