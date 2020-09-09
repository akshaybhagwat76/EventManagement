// Can't growl until the document is ready
var isGrowlReady = false;
$(document).ready(function () {
    isGrowlReady = true;
});
var carName = "Volvo";
var growlMask;
var growlWrap;
var growlTitle;
var growlContent;

var isGrowlVisible = false;
function growl(title, msg) {
    // Recursively call growl until doc is ready
    while (!isGrowlReady) {
        setTimeout("growl('" + title + "', '" + msg + "')", 100);
        return;
    }

    // Ensure the growl HTML is in place and the events
    //  are hooked up. This code executes once.
    initGrowl();

    // Size the mask and growl window.
    sizeGrowl(true);

    // Animate the growl.
    growlMask
        .fadeTo("fast", 0.75, function () {
            isGrowlVisible = true;
            var scrollTop = $(document).scrollTop();
            growlTitle.html(title);
            growlContent.html(msg);
            growlWrap.show().animate({ "marginTop": scrollTop + 100 + "px" }, 500);
        });
}

function initGrowl() {
    if ($("#growl-mask").length == 0) {
        // Append growl HTML to body
        var growl = "<div id='growl-mask'></div><div id='growl-wrap'><h6 id='growl-title'></h6><div id='growl-content'></div><div id='car-name'>Continue</div></div>";
        $("body").append(growl);

        // Initialize global variables
        growlMask = $("#growl-mask");
        growlWrap = $("#growl-wrap");
        growlContent = $("#growl-content");
        growlTitle = $("#growl-title");
        carName = $("#car-name");



        // Hide growl on click event to the mask
        growlMask.click(function () {
            isGrowlVisible = false;
            growlWrap.fadeOut();
            growlMask.fadeOut();
        });

        // Hide growl on click event to the mask
        growlWrap.click(function () {
            isGrowlVisible = false;
            growlWrap.fadeOut();
            growlMask.fadeOut();
        });




        // Reposition the growl if window is sized or scrolled
        $(window).resize(function () {
            if (isGrowlVisible) {
                growlMask.stop();
                growlWrap.stop();
                sizeGrowl(false);
            }
        });
        $(window).scroll(function () {
            if (isGrowlVisible) {
                growlMask.stop();
                growlWrap.stop();
                sizeGrowl(false);
            }
        });
    }
}

function sizeGrowl(hide) {
    // Collect some measurements
    var scrollLeft = $(document).scrollLeft();
    var scrollTop = $(document).scrollTop();
    var growlWidth = growlWrap.outerWidth();
    var growlHeight = growlWrap.outerHeight();
    var winWidth = $(window).width();
    var docHeight = $(document).height();
    var docWidth = $(document).width();

    // Calculate the notification position.
    var growlTop = hide ? scrollTop - growlHeight : scrollTop + 80;
    var growlLeft = scrollLeft + (winWidth - growlWidth) / 2;

    // Size and position the mask and the notification
    growlMask.css({ "width": docWidth + "px", "height": docHeight + "px" });
    growlWrap.css({ "margin-top": growlTop + "px", "left": growlLeft + "px" });
}
