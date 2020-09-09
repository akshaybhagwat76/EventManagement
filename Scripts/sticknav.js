jQuery(function(){
    createSticky(jQuery("#sticky-wrap"));
});

function createSticky(sticky) {
  
    if (typeof sticky != "undefined") {

        var pos = sticky.offset().top ,
            win = jQuery(window);

        win.on("scroll", function() {
            if( win.scrollTop() > parseInt(pos) ) {
            
                sticky.addClass("stickyhead");
                var parentWidth = sticky.parent().width();
                var parentLeft = sticky.parent().offset().left;
                sticky.css({width: parentWidth+'px',left: parentLeft+"px"});
                
            } else {
                sticky.removeClass("stickyhead");
                sticky.removeAttr('style');
            }           
        });         
    }
}


/* sticky nav added to script folder */