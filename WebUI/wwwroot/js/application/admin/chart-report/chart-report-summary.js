let currentDate = new Date();
let monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
let currentMonthName = monthNames[currentDate.getMonth()];

let globalData = {
    CurrentMonth: (new Date().getMonth() + 1).toString(),
    CurrentYear: new Date().getFullYear().toString(),
    CurrentMonthName: currentMonthName
};

let DOM = {
    ChartByYear: 'report-by-year',
    ChartByMonthOfYear: 'report-by-month',
};

let URL = {
    GetListReportByMonthOfYearFilter: '/Admin/Order/GetListOrderReport',
};

(function ($) {
    'use strict';

    let initChart = function () {
        
    }

    let bindingControl = function () {
        singleSelectDropdownFilterServer('Year', 'Select Year', 'Year', globalData.CurrentYear);
        singleSelectDropdownFilterServer('Month', 'Select Month', 'Month', globalData.CurrentMonth);
        generateChartReportByMonthOfYear(globalData.CurrentMonth, globalData.CurrentYear, false);
    }

    let bindingEvents = function () {
        $('#btn-generate-report-by-month').on('click', function (e) {
            e.preventDefault();
            let year = $('#Year').val();
            let month = $('#Month').val();
            if (parseInt(year) > 0 && parseInt(month)){
                generateChartReportByMonthOfYear(month, year, true);
            }else{
                toastMessage('error', 'Please select Year and Month!');
            }
        });
    }

    let singleSelectDropdownFilterServer = function (id, placeholder, dataType, selectItem) {
        coreAjax(true, '/Admin/Common/GetDataDropDown?dataType=' + dataType, null, 'GET', function (res) {
            setTimeout(function () {
                $("#" + id).select2({
                    placeholder: placeholder,
                    data: res,
                    allowClear: true
                });
            }, 200);

            if (selectItem) {
                setTimeout(function () {
                    $('#' + id).val(selectItem).trigger('change')
                }, 200);
            }
        })
    }
    
    let generateChartReportByMonthOfYear = function (month, year, isReload) {
        coreAjax(true, URL.GetListReportByMonthOfYearFilter + '?Year=' + year + '&Month=' + month, null, 'GET', function (res) {
            let dataPoints = [];
            let textYearOption = $('#YearOfMonth option:selected').text() ? $('#YearOfMonth option:selected').text() : globalData.CurrentYear.toString();
            let textMontOption = $('#Month option:selected').text() ? $('#Month option:selected').text() : globalData.CurrentMonthName;
            let titleChart = 'Data Report ' + textMontOption + ' ' + textYearOption;
            res.forEach(e => {
                dataPoints.push({
                    indexLabel: '' + formatMoney(e.valueY),
                    y: e.valueY,
                    label: e.valueX
                })
            });
            renderChart(DOM.ChartByMonthOfYear, 'line', dataPoints, '{y}', 'Day {label} - {y}', 'Day <b>{label}</b> - {y}','', titleChart, 'VND', 'Days of Month');
        });
    }
    
    //Load functions
    $(document).ready(function () {
        bindingControl();
        bindingEvents();
        initChart();
    });

})(jQuery);
