
$("document").ready(function () {

    var opened = false;
   

    $("#user").click(function () {

        if(opened == false)
        {
            console.log("opened");
            $("#usermnu").addClass("user-acc-open");
            $("#page-wrap").css("filter", "blur(5px)").css("pointer-events", "none");
            $("#user").css("filter", "none").css("pointer-events", "auto");

            opened = true;
        }

    });

    $("#close-usr").click(function () {

        $("#usermnu").removeClass("user-acc-open");
        $("#page-wrap").css("filter", "none").css("pointer-events", "auto");
        opened = false;
    });


    $("#closbtn").click(function () {

        $("#usermnu").removeClass("user-acc-open");
        $("#page-wrap").css("filter", "none").css("pointer-events", "auto");
        opened = false;
    });


    $("#usermnu").blur(function () {
        this.removeClass("user-acc-open");
    });

});


