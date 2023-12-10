(function ($) {
    'use strict';
    
    var properties = [];
    
    var addProperty = function () {
        $('#Property').on('click', function () {
            $('#modal-property').modal('show');
        });

        $('#btn-add-more').on('click', function () {
            var tempProperties = [];
            $('.property-value').each(function () {
                var name = $(this).find('input[name="Name"]').val();
                var value = $(this).find('input[name="Value"]').val();

                tempProperties.push({
                    Name: name,
                    Value: value,
                    ProductId: $('#ProductId').val()
                })
            });

            coreAjaxPostPartialView(true, '/Admin/Product/AddNewRowProperty', JSON.stringify(tempProperties), function (response) {
                $("#div-add-new-property").html(response);
            }, function () {

            });
        });

        $('#btn-add-property').on('click', function () {
            if (validationProperty()) {
                $('.property-value').each(function () {
                    var name = $(this).find('input[name="Name"]').val();
                    var value = $(this).find('input[name="Value"]').val();

                    properties.push({
                        Name: name,
                        Value: value,
                        ProductId: $('#ProductId').val()
                    })
                });

                $('#total-property').text(properties.length);

                $('#modal-property').modal('hide');
            }
        });
    }

    var validationProperty = function () {
        var txtRequired = $('#Required').val();
        var txtDuplicated = $('#Duplicate').val();
        var rowCount = 1;
        var isValid = true;
        var tempNames = [];

        $('.property-value').each(function () {
            $('#span-name-' + rowCount).remove();
            $('#span-value-' + rowCount).remove();

            var name = $(this).find('input[name="Name"]').val();
            var value = $(this).find('input[name="Value"]').val();

            tempNames.push(name);

            if (name === '') {
                $('#col-name-' + rowCount).append('<span class="text-danger" id="span-name-' + rowCount + '">' + txtRequired + '</span>');
                isValid = false;
            }

            if (value === '') {
                $('#col-value-' + rowCount).append('<span class="text-danger" id="span-value-' + rowCount + '">' + txtRequired + '</span>');
                isValid = false;
            }

            if (countValue(name, tempNames) > 1) {
                $('#span-name-' + rowCount).remove();
                $('#col-name-' + rowCount).append('<span class="text-danger" id="span-name-' + rowCount + '">' + txtDuplicated + '</span>');
                isValid = false;
            }

            rowCount++;
        });

        return isValid;
    }

    var update = function () {
        $('#Price').on('keyup', function () {
            var p = parseFloat($('#Price').val())

            if (isNaN(p)) {
                $('#sPrice2').show()
                $('#Price').val('')
            }else{
                $('#sPrice2').hide()
            }
        })

        $('#Discount').on('keyup', function () {
            var p = parseInt($('#Discount').val())
            var current = parseInt($('#Price').val())
            
            if (isNaN(p)) {
                $('#sDiscount2').show()
                $('#Discount').val('');
                $('#sPriceAfterDiscount').hide()
            }else{
                $('#sDiscount2').hide();
                if (!isNaN(current)){
                    var is = $('#IsEnglishVersionApp').val() === 'true' ? 'is' : 'là';
                    $('#sPriceAfterDiscount').show()
                    $('#PriceAfterDiscountValue').text(p + '% ' + is + ' ' + calDiscountPrice(current, p))
                }
            }
        })
        
        $('#btn-save').on('click', function () {
            var isValid = true;
            
            $('.required').each(function () {
                $(this).hide();
            });

            if ($('#ProductName').val() === '') {
                $('#sProductName').show()
                isValid = false;
            }

            if ($('#Category').val() === '') {
                $('#sCategory').show()
                isValid = false;
            }

            if ($('#Brand').val() === '') {
                $('#sBrand').show()
                isValid = false;
            }

            var price = parseFloat($('#Price').val())

            if (isNaN(price) || price <= 0) {
                $('#sPrice').show()
                $('#Price').val('')
                isValid = false;
            }
            
            var discountPercent = parseFloat($('#Discount').val())

            if ((isNaN(discountPercent) || discountPercent <= 0) && $('#Discount').val() !== '') {
                $('#sDiscount2').show()
                $('#Discount').val('')
                isValid = false;
            }

            if (isValid){
                var formData = new FormData();
                
                formData.append('Id', $('#ProductId').val())
                formData.append('Name', $('#ProductName').val())
                formData.append('CategoryId', $('#Category').val())
                formData.append('BrandId', $('#Brand').val())
                formData.append('Description', $('#Description').val())
                formData.append('Price', price.toString())
                formData.append('Currency', $('input[name=Currency]:checked').val())
                formData.append('Stock', $('#Stock').val())
                
                for(var i = 0; i < $('#Images')[0].files.length; i++){
                    formData.append('Images', $('#Images')[0].files[i])
                }
                
                for (var k = 0; k < properties.length; k++) {
                    formData.append("Properties[" + k + "].Name", properties[k].Name);
                    formData.append("Properties[" + k + "].Value", properties[k].Value);
                    formData.append("Properties[" + k + "].ProductId", properties[k].ProductId);
                }
                coreAjaxWithFormData(isValid, '/Admin/Product/SubmitUpdate', formData, 'POST', function (res) {
                    toastMessage('success', 'Saved successfully');
                    window.location.href = '/Admin/Product/Index'
                }, function () {});
            }
        })
    }
    
    var calDiscountPrice = function (current, discount) {
        var discountPrice = current * (discount / 100);
        var formatter = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
        });
        return formatter.format(current - discountPrice).split('.')[0];
    }
    //Load functions
    $(document).ready(function () {
        update();
        addProperty();
    });

})(jQuery);