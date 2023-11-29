(function ($) {
    'use strict';
    
    //Load functions
    $(document).ready(function () {
        listenMessage(function (type, message) {
            if (type==='order'){
                console.log(message);
                getListOrder();
            }
        })
        getListOrder();
    });

})(jQuery);