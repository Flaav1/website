$("document").ready(function () {
    console.log("filter ready");
    var filteropened = false;

    $("#filter-btn").click(function () {

        if (filteropened == false) {
            console.log("filter-opened");
            $("#filter-content-hide").addClass("filter-content-show");
            filteropened = true;
        }
        else {
            console.log("filter-closed");
            $("#filter-content-hide").removeClass("filter-content-show");
            filteropened = false;
        }



    });



});