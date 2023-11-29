(function ($) {
    'use strict';

    var onViewDetail = function () {
        $('.btn-view-detail').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            onClickLink(true, '/Product/Detail?id=' + id)
        });
    }

    var onDelete = function () {
        $('.btn-delete').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            sweetAlertDeleteConfirmation('/Admin/Product/Delete', id, $('#TitleDelete').val(), $('#TextDelete').val(),
                function() { getListProduct() })
        });
    }

    var onUpdate = function () {
        $('.btn-edit').on('click', function (e) {
            e.preventDefault();
            var id = $(this).attr('data-id')
            onClickLink(true, '/Admin/Product/Update?id=' + id)
        });
    }

    //Load functions
    $(document).ready(function () {
        onViewDetail();
        onDelete();
        onUpdate();
    });

})(jQuery);