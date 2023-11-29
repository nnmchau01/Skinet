var getListProduct = function () {
    coreAjaxGetPartialView(true, '/Admin/Product/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

var getListUser = function () {
    coreAjaxGetPartialView(true, '/Admin/User/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

var getListCate = function () {
    coreAjaxGetPartialView(true, '/Admin/Category/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

var getListBrand = function () {
    coreAjaxGetPartialView(true, '/Admin/Brand/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

var getListPromotion = function () {
    coreAjaxGetPartialView(true, '/Admin/Promotion/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

var getListOrder = function () {
    coreAjaxGetPartialView(true, '/Admin/Order/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

var initDataTable = function (tableId) {
    return $('#' + tableId).DataTable({
        aaSorting: [],
        responsive: {
            details: {
                display: $.fn.dataTable.Responsive.display.childRowImmediate,
                type: ''
            }
        }
    });
}

var getListRating = function () {
    coreAjaxGetPartialView(true, '/Admin/Rating/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

let renderChart = function (chartId, type, dataPoints, uffix, indexLabel, toolTipContent, backgroundColor, titleChart, axisYTitle, axisXTitle, isReload) {
    let options = {
        title: {
            text: titleChart
        },
        exportEnabled: true,
        backgroundColor: backgroundColor,
        animationEnabled: true,
        interactivityEnabled: true,
        axisY: {
            title: axisYTitle,
            uffix: uffix
        },
        axisX: {
            title: axisXTitle
        },
        data: [
            {
                type: type, // Change type to doughnut, line, splineArea, etc, column, area, bar, pie, etc
                indexLabel: indexLabel, //"{y} " + suffixText,
                toolTipContent: toolTipContent, //"<b>{label}</b>: {y} " + suffixTextEnd,
                dataPoints: dataPoints
            }
        ]
    };

    (new CanvasJS.Chart(chartId, options)).render();
}

var getListService = function () {
    coreAjaxGetPartialView(true, '/Admin/Service/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}

var getListBooking = function () {
    coreAjaxGetPartialView(true, '/Admin/Booking/GetListPartial', null, function (response) {
        $("#div-datatable").html(response);
    }, function () {

    });
}
