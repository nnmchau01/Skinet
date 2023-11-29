(function ($) {
    'use strict';
    let href = window.location.href;
    let page = getParamFromUrl('page', href);
    let startPage = page ? parseInt(page) : 1;

    let initPaging = function () {

        window.pagObj = $('#pagination').twbsPagination({
            first: '<i class="fa fa-angle-double-left"></i>',
            prev: '<i class="fa fa-angle-left"></i>',
            next: '<i class="fa fa-angle-right"></i>',
            last: '<i class="fa fa-angle-double-right"></i>',
            totalPages: parseInt($('#total-page').val()),
            visiblePages: 5,
            startPage: startPage,
            onPageClick: function (event, page) {
                if (startPage !== page) {
                    if (href.indexOf('page') > 0) {
                        let strSplit = href.split('page=');
                        window.location.href = strSplit[0] + 'page=' + page;
                    } else {
                        if (href.indexOf('?') > 0){
                            window.location.href = href + '&page=' + page;
                        }else{
                            window.location.href = href + '?page=' + page;
                        }
                    }
                }
            }
        });

        $('.page-item').each(function () {
            let text = $(this).text()
            if (text && parseInt(startPage) === parseInt(text)) {
                $(this).addClass('active')
            }
        })
    }

    //Load functions
    $(document).ready(function () {
        initPaging()
    });

})(jQuery);
