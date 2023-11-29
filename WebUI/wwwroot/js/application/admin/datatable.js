var _Datatable = initDataTable('grid-datatable');

var isExistedAddNew = function (index, value) {
    var data = _Datatable.rows().data();
    delete data.context;
    delete data.selector;
    delete data.ajax;
    var dataAsArray = [];
    var dataForCheck = [];

    for (var i = 0; i < data.length; i++) {
        dataAsArray.push(data[i]);
    }

    for (var j = 0; j < dataAsArray.length; j++) {
        dataForCheck.push(data[j][index].toLowerCase());
    }

    return dataForCheck.includes(value.toLowerCase());
}

var isExistedUpdate = function (index, currentValue, newValue) {
    var data = _Datatable.rows().data();
    delete data.context;
    delete data.selector;
    delete data.ajax;
    var dataAsArray = [];
    var dataForCheck = [];

    for (var i = 0; i < data.length; i++) {
        dataAsArray.push(data[i]);
    }

    for (var j = 0; j < dataAsArray.length; j++) {
        if (data[j][index].toLowerCase() !== currentValue.toLowerCase()) {
            dataForCheck.push(data[j][index].toLowerCase());
        }
    }

    return dataForCheck.includes(newValue.toLowerCase());
}