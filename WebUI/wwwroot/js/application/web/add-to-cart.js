$(function () {
    addCartToLocal();
    initItemCartCheckout();
    onChangeValueQuantity();
    addCartToLocalDetail();
});

var addCartToLocal = function () {
    $('.add-to-cart').on('click', function (e) {
        e.preventDefault();

        var object = $(this);
        var productId = object.attr('product-id');

        var data = {
            "ProductId": productId,
            "Quantity": 1
        };

        if (!localStorage.getItem('cart')) {
            var firstCart = [];

            firstCart.push(data);
            localStorage.setItem('cart', JSON.stringify(firstCart));
        } else {
            var tempCart = JSON.parse(localStorage.getItem('cart'));
            var isExist = false;

            for (var element of tempCart) {
                if (data.ProductId === element.ProductId) {
                    element.Quantity += 1;
                    isExist = true;
                    break;
                }
            }

            if (!isExist) {
                tempCart.push(data);
            }

            localStorage.setItem('cart', JSON.stringify(tempCart));
        }

        $('#total-item-cart').text(getDataCart().length);
    })
}

var initItemCartCheckout = function () {
    if ($('#is-checkout-page').length > 0){
        
        onChangeValueQuantity();
    }
}

var onChangeValueQuantity = function () {
    $('.on-change-value-quantity').keyup(function () {
        var object = $(this);
        var productId = object.attr('product-id');
        var value = object.val();
        var tempCart = JSON.parse(localStorage.getItem('cart'));

        for (var element of tempCart) {
            if (productId === element.ProductId) {
                element.Quantity = value;
                break;
            }
        }

        tempCart = tempCart.filter(function (e) {
            return e.Quantity > 0;
        })

        localStorage.setItem('cart', JSON.stringify(tempCart));

        setTimeout(function () {
            coreAjaxPostPartialView(true, '/Cart/GetCart', localStorage.getItem('cart'), function (response) {
                $("#your-cart").html(response);
            }, function () {

            });
        }, 500);
    });
}

var addCartToLocalDetail = function () {
    $('.add-to-cart-detail').on('click', function (e) {
        e.preventDefault();

        var object = $(this);
        var productId = object.attr('product-id');

        var data = {
            "ProductId": productId,
            "Quantity": parseInt($("#quantity-detail").val())
        };

        if (!localStorage.getItem('cart')) {
            var firstCart = [];

            firstCart.push(data);
            localStorage.setItem('cart', JSON.stringify(firstCart));
        } else {
            var tempCart = JSON.parse(localStorage.getItem('cart'));
            var isExist = false;

            for (var element of tempCart) {
                if (data.ProductId === element.ProductId) {
                    element.Quantity = parseInt($("#quantity-detail").val());
                    isExist = true;
                    break;
                }
            }

            if (!isExist) {
                tempCart.push(data);
            }

            localStorage.setItem('cart', JSON.stringify(tempCart));
        }

        $('#total-item-cart').text(getDataCart().length);
    })
}
