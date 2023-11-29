var counterRequest = 0;

$(document)
    .ajaxSend(function (event, xhr, options) {
        counterRequest++;
        console.log('send');
        if (getParamFromUrl("noBlockUI", options.url) === "true") {return;}

        //set loading and block ui
        if (counterRequest > 0) {
            console.log('send');
            $('#loading').show();
        }
    })
    .ajaxComplete(function (event, xhr, options) {
        console.log('send');
        counterRequest--;
        // unset loading and block ui
        if (counterRequest <= 0) {
            console.log('done');
            $('#loading').hide();
        }
    });