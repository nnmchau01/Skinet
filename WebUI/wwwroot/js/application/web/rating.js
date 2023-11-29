(function ($) {
    'use strict';

    let $star_rating = $('.star-rating .fa');

    let setRatingStar = function() {
        return $star_rating.each(function() {
            if (parseInt($star_rating.siblings('input.rating-value').val()) >= parseInt($(this).data('rating'))) {
                return $(this).removeClass('fa-star-o').addClass('fa-star');
            } else {
                return $(this).removeClass('fa-star').addClass('fa-star-o');
            }
        });
    };

    $star_rating.on('click', function() {
        let currentValue = $('input.rating-value').val();
        let title = $("#ProductName").val();

        $star_rating.siblings('input.rating-value').val($(this).data('rating'));
        let starPoint = parseInt($(this).data('rating'));
        let data = {
            ProductId: $("#ProductId").val(),
            StarPoint: starPoint
        };

        if(currentValue !== starPoint){
            coreAjax(true, '/Rating/Update', JSON.stringify(data), 'POST', function (res) {
                if (res===true){
                    toastMessage('success', `You voted ${starPoint} star point \"${title}\"`);
                }else{
                    toastMessage('error', 'Something went wrong! Are you login?');
                    currentValue = 0;
                    $('input.rating-value').val(currentValue);
                    return setRatingStar();
                }
                getAverageStarPoint();
            }, function () {
                currentValue = 0;
                $('input.rating-value').val(currentValue);
                return setRatingStar();
            });
            return setRatingStar();
        }
    });

    let getRatingStart = function () {
        setTimeout(function () {
            let data = {
                ProductId: $('#ProductId').val()
            };
            coreAjax(true, '/Rating/Detail', JSON.stringify(data), 'POST', function (res) {
                $('input.rating-value').val(res);
                return setRatingStar();
            });
        }, 300)
    }

    let getAverageStarPoint = function () {
        let data = {
            ProductId: $('#ProductId').val(),
        };
        coreAjax(true, '/Rating/GetAverageStarPoint', JSON.stringify(data), 'POST', function (res) {
            var text = res.toFixed(1) + '/5';
            $('#AverageStarPoint').text(text);
        }, function () {
            $('#AverageStarPoint').text('0/5');
        });
    }
    
    //Load functions
    $(document).ready(function () {
        setRatingStar();
        getRatingStart();
        getAverageStarPoint();
    });

})(jQuery);