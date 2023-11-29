var openModalCheckOrderWeb = function () {
    $('#OrderCode').val('');
    $('#modal-popup-order-detail-check').modal('show');
}

var formatMoney = function (number) {
    return number.toLocaleString('en-US', { style: 'currency', currency: 'USD' }).replaceAll('$', '').split('.')[0];
}

var getDataCart = function () {
    if (!localStorage.getItem('cart')) {
        return [];
    } else {
        return JSON.parse(localStorage.getItem('cart'));
    }
}

var setCartLocal = function (value, productId) {
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
}

var countValue = function (value, array) {
    var count = 0;
    $.each(array, function (i, v) {
        if (v.toLowerCase() == value.toLowerCase()) count++;
    });
    return count;
}

var validationForm = function (formId) {
    var isValid = true;

    $('#' + formId).find('.required').each(function () {
        var val = $(this).find('input').val()
        if (!val) {
            $(this).find('span').show();
            isValid = false;
        } else {
            $(this).find('span').hide();
        }
    });

    return isValid;
}

var onClickLink = function (isNewTab, href) {
    if (isNewTab)
        window.open(
            href,
            '_blank'
        );
    else
        window.location.href = href
}

var hiddenTextRequired = function (formId) {
    $('#' + formId).find('.required').each(function () {
        $(this).find('span').hide();
    })
}

var getFormDataJson = function (formId) {
    var unindexed_array = $('#' + formId).serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

var signInOrSignUp = function (isLogin) {
    if (isLogin) {
        hiddenTextRequired('frmLogin');
        $('#modal-sign-in').modal('show');
    } else {
        hiddenTextRequired('frmSubmit');
        $('#modal-sign-up').modal('show');
    }
}

var toastMessage = function (code, text) {
    var header, bgColor, icon, textColor;

    switch (code) {
        case 'success':
            header = 'Success';
            bgColor = '#3c763d';
            icon = 'success';
            textColor = '#f1f1f1';
            break;
        case 'error':
            header = 'Error';
            bgColor = '#F73F52';
            icon = 'error';
            textColor = '#f1f1f1';
            break;
        case 'warning':
            header = 'warning';
            bgColor = '#fcf8e3';
            icon = 'warning';
            textColor = '#8a6d3b';
            break;
        case 'info':
            header = 'Infomation';
            bgColor = '#d9edf7';
            icon = 'info';
            textColor = '#31708f';
            break;
    }

    $.toast({
        text: `${text}`,
        position: 'top-right',
        heading: `${header}`,
        bgColor: `${bgColor}`,
        icon: `${icon}`,
        textColor: `${textColor}`,
        hideAfter: 6000
    });
}

var getParamFromUrl = function (name, url) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(url);
    if (results == null) {
        return null;
    }
    return decodeURI(results[1]) || 0;
};

var coreAjaxGetPartialView = function (isValid, url, data, successCallBack, errorCallback) {
    if (isValid) {
        $.ajax({
            headers: {
                'AuthorizationAPI': 'Bearer ' + localStorage.getItem('token')
            },
            cache: false,
            url: url,
            type: 'GET',
            data: data,
            success: function (res) {
                if (successCallBack) {
                    successCallBack(res);
                }
            },
            error: function (err) {

                checkErrorResponse(err)

                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }
}

var coreAjaxPostPartialView = function (isValid, url, data, successCallBack, errorCallback) {
    if (isValid) {
        $.ajax({
            headers: {
                'AuthorizationAPI': 'Bearer ' + localStorage.getItem('token')
            },
            cache: false,
            url: url,
            type: 'POST',
            data: data,
            contentType: 'application/json',
            success: function (res) {
                if (successCallBack) {
                    successCallBack(res);
                }
            },
            error: function (err) {

                checkErrorResponse(err)

                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }
}

var coreAjax = function (isValid, url, data, method, successCallBack, errorCallback) {
    console.log(data)
    if (isValid) {
        $.ajax({
            cache: false,
            url: url,
            type: method,
            data: data,
            contentType: 'application/json',
            success: function (res) {
                if (successCallBack) {
                    successCallBack(res);
                }
            },
            error: function (err) {
                checkErrorResponse(err)
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }
}

var checkErrorResponse = function (err) {
    var isEnglish = false;
    var error = err.responseJSON;
    var statusCode = parseInt(err.status);

    switch (statusCode) {
        case 400:
            toastMessage('error', err.responseText);
            break;
        case 401:
            toastMessage('error', isEnglish ? 'You are not logged in or your session has expired. We will redirect the page to the login page in the next few seconds.' : 'Bạn chưa đăng nhập hoặc phiên đăng nhập hết hạn. Chúng tôi sẽ chuyển hướng trang sang trang đăng nhập trong vài giây tới.');
            setTimeout(function () {
                $('#modal-sign-in').modal('show');
            }, 1000);
            break;
        case 302:
            toastMessage('error', isEnglish ? 'You are not logged in or your session has expired. We will redirect the page to the login page in the next few seconds.' : 'Bạn chưa đăng nhập hoặc phiên đăng nhập hết hạn. Chúng tôi sẽ chuyển hướng trang sang trang đăng nhập trong vài giây tới.');
            setTimeout(function () {
                $('#modal-sign-in').modal('show');
            }, 1000);
            break;
        case 403:
            toastMessage('error', isEnglish ? 'Access is denied. Please contact the Admin team for more details.' : 'Truy cập bị từ chối. Vui lòng liên hệ với đội ngũ Admin để biết thêm chi tiết.');
            break;
        case 404:
            toastMessage('error', isEnglish ? 'The page you are looking for could not be found.' : 'Không thể tìm thấy trang bạn đang tìm kiếm.');
            break;
        default:
            toastMessage('error', isEnglish ? 'An error occurred. Please try again later.' : 'Có lỗi xảy ra. Vui lòng thử lại sau.');
            break;
    }
}

var coreAjaxWithFormData = function (isValid, url, data, method, successCallBack, errorCallback) {
    if (isValid) {
        $.ajax({
            cache: false,
            url: url,
            type: method,
            data: data,
            contentType: false,
            processData: false,
            success: function (res) {
                if (successCallBack) {
                    successCallBack(res);
                }
            },
            error: function (err) {
                checkErrorResponse(err)
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }
}

var sweetAlertDeleteConfirmation = function (url, id, title, text, callBack) {
    var data = {
        Id: id
    };
    
    Swal.fire({
        title: '<h2 class="custom-font">' + title + '</h2>',
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            coreAjax(true, url, JSON.stringify(data), 'POST', function (res) {
                toastMessage('success', 'Deleted successfully');
                callBack();
            }, function () {
            });
        }
    })
}

var onViewPromotions = function () {
    coreAjaxGetPartialView(true, '/Common/GetPromotionList', null, function (response) {
        $('#modal-popup-order-detail-check').modal('hide');
        $("#modal-body-order-detail").html(response);
        $('#modal-popup-order-detail').modal('show');
    }, function () {});
}

var onChangePrice = function () {
    $('.format-money-input').on('input', function () {
        var numberPattern = /^[0-9]+$/;
        var value = $(this).val().replaceAll('.', '')
        if(numberPattern.test(value)){
            var formattedNumber = value.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            $(this).val(formattedNumber)
        }else{
            $(this).val('')
        }
    })

    $('.format-money-input').each(function () {
        var numberPattern = /^[0-9]+$/;
        var value = $(this).val().replaceAll('.', '')
        if(numberPattern.test(value)){
            var formattedNumber = value.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            $(this).val(formattedNumber)
        }else{
            $(this).val('')
        }
    })
}