/********************************
iVeri Animation

Created for: iVeri Payment Technologies
Author: Brendon M. van Wasbeek

Scripts for animating website elements

// HTML:
<div id="elID">
<some_element style="must have content width"></some_element>
</div>

********************************/

// Global Variables
var isIE = (navigator.appName == "Microsoft Internet Explorer") ? true : false;
var elementArr = new Array();
var elementStatusArr = new Array();
var _startX = 0;
var _startY = 0;
var _offsetX = 0;
var _offsetY = 0;
var _dragElement;
var _oldZIndex = 0;
var _boundaryLeft = 0;
var _boundaryRight = 00;
var _boundaryTop = 0;
var _boundaryBottom = 0;
var _dragElementLeft = 0;
var _dragElementRight = 0;
var _dragElementTop = 0;
var _dragElementBottom = 0;
var _dragType = "all";

// Arrray Prototypes
Array.prototype.remove = function(from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};
Array.prototype.removeByValue = function(value) {
    for (var x in this) {
        if (this[x] == value) {
            this.remove(x);
            //break; in case off multiple occurences, keeps it clean
        }
    }
    return true;
};
Array.prototype.indexOfByValue = function(value) {
    for (var x in this) {
        if (this[x] == value) {
            return x;
            //break; in case off multiple occurences, keeps it clean
        }
    }
    return -1;
};
Array.prototype.exists = function(value) {
    for (var x in this) {
        if (this[x] == value) {
            return true;
        }
    }
    return false;
};

// Initialisation
function IveriAnimaX() 
{
    //this.wait = IveriAnimaX_Wait;
    this.fadeOut = IveriAnimaX_FadeOut;
    this.fadeIn = IveriAnimaX_FadeIn;
    this.fadeHalt = IveriAnimaX_HaltFade;
    this.hideHorizontal = IveriAnimaX_HideHorizontal;
    this.hideVertical = IveriAnimaX_HideVertical;
    this.showHorizontal = IveriAnimaX_ShowHorizontal;
    this.showVertical = IveriAnimaX_ShowVertical;
    this.dragHorizontal = IveriAnimaX_DragHorizontal;
    this.dragVertical = IveriAnimaX_DragVertical;
}

// Wait function
/*
function IveriAnimaX_Wait(elID,timeoutMs) 
{
    var complete = false;
    var startTime = new Date().getTime();
    
    while ((!complete) && (new Date().getTime() - startTime <= timeoutMs)) {
        if (elementStatusArr[parseInt(elementArr.indexOfByValue(elID))] == "IDLE")
            complete = true;
    }
    return complete;
}*/

// Horizontal and Vertical sliding
function IveriAnimaX_HideHorizontal(elID, millisecMoveDelay, pixelMoveAmount, minWidth) {
    var el = document.getElementById(elID);
    var elWidth = el.offsetWidth; //(el.style.width == "") ? el.scrollWidth : parseInt(el.style.width.replace("px", ""));

    var hideInterval = setInterval(function() {
        if (elWidth - pixelMoveAmount <= minWidth) {
            clearInterval(hideInterval);
            elWidth = minWidth;
        }
        else
            elWidth = elWidth - pixelMoveAmount;
        el.style.width = elWidth + "px";
    }, millisecMoveDelay)
}

function IveriAnimaX_ShowHorizontal(elID, millisecMoveDelay, pixelMoveAmount) {
    var el = document.getElementById(elID);
    var elWidth = el.scrollWidth;
    var currentWidth = el.offsetWidth; //parseInt(el.style.width.replace("px", ""));

    if (currentWidth < elWidth) {
        var showInterval = setInterval(function() {
            if (elWidth - currentWidth <= pixelMoveAmount) {
                clearInterval(showInterval);
                currentWidth = elWidth;
            }
            else
                currentWidth = currentWidth + pixelMoveAmount;
            el.style.width = currentWidth + "px";
        }, millisecMoveDelay)
    }
}

function IveriAnimaX_HideVertical(elID, millisecMoveDelay, pixelMoveAmount, minHeight) {
    var el = document.getElementById(elID);
    var elHeight = el.offsetHeight; //(el.style.height == "") ? el.scrollHeight : parseInt(el.style.height.replace("px", ""));

    var hideInterval = setInterval(function() {
        if (elHeight - pixelMoveAmount <= minHeight) {
            clearInterval(hideInterval);
            elHeight = minHeight;
        }
        else
            elHeight = elHeight - pixelMoveAmount;
        el.style.height = elHeight + "px";
    }, millisecMoveDelay)
}

function IveriAnimaX_ShowVertical(elID, millisecMoveDelay, pixelMoveAmount, minHeight) {
    var el = document.getElementById(elID);
    var elHeight = el.scrollHeight;
    var currentHeight = el.offsetHeight; // (el.style.height == "") ? minHeight : parseInt(el.style.height.replace("px", ""));

    if (currentHeight < elHeight) {
        var showInterval = setInterval(function() {
            if (elHeight - currentHeight <= pixelMoveAmount) {
                clearInterval(showInterval);
                currentHeight = elHeight;
            }
            else
                currentHeight = currentHeight + pixelMoveAmount;
            el.style.height = currentHeight + "px";
        }, millisecMoveDelay)
    }
}


// Fading In and Out

// Shouldn't be too difficult to design, but no real need to implement it asynchronously
/*
function IveriAnimaX_FadeOut_Async(elID, millisecFadeDelay, percentFadeAmount, percentMinOpacity) {
    IveriAnimaX_FadeOut_Sync(elID, millisecFadeDelay, percentFadeAmount, percentMinOpacity, new Date().getTime());
}

function IveriAnimaX_FadeIn_Async(elID, millisecFadeDelay, percentFadeAmount, percentMaxOpacity, startTime) {
    IveriAnimaX_FadeIn_Sync(elID, millisecFadeDelay, percentFadeAmount, percentMaxOpacity, new Date().getTime());
}*/

function IveriAnimaX_FadeIn(elID, millisecFadeDelay, percentFadeAmount, percentMaxOpacity) 
{
    if (!elementArr.exists(elID))
        elementArr[elementArr.length] = elID;
    elementStatusArr[parseInt(elementArr.indexOfByValue(elID))] = "IN";
    
    var el = document.getElementById(elID);
    var opacity;

    if (isIE) {
        opacity = (el.style.filter == "") ? 100 : parseInt(el.style.filter.split("=")[1].replace(")", ""));
    }
    else {
        percentFadeAmount = percentFadeAmount / 100;
        percentMaxOpacity = percentMaxOpacity / 100;
        opacity = (el.style.opacity == "") ? 1.0 : parseFloat(el.style.opacity);
    }

    // If at maximum opacity no need to do anything, otherwise perform the fade
    if ((opacity >= percentMaxOpacity) || (opacity >= percentMaxOpacity)) {
        // At maximum opacity
        var index = elementArr.indexOfByValue(elID);
        elementStatusArr[index] = "IDLE";
    }
    else {
        FadeIn(el, opacity, millisecFadeDelay, percentFadeAmount, percentMaxOpacity);
    }
}

function IveriAnimaX_FadeOut(elID, millisecFadeDelay, percentFadeAmount, percentMinOpacity) 
{
    if (!elementArr.exists(elID))
        elementArr[elementArr.length] = elID;
    elementStatusArr[parseInt(elementArr.indexOfByValue(elID))] = "OUT";
    
    var el = document.getElementById(elID);
    var opacity;
    if (isIE) {
        opacity = (el.style.filter == "") ? 100 : parseInt(el.style.filter.split("=")[1].replace(")", ""));
    }
    else {
        percentFadeAmount = percentFadeAmount / 100;
        percentMinOpacity = percentMinOpacity / 100;
        opacity = (el.style.opacity == "") ? 1.0 : parseFloat(el.style.opacity);
    }
    
    // If at minimum opacity no need to do anything, otherwise perform the fade
    if ((opacity <= percentMinOpacity) || (opacity <= percentMinOpacity)) {
        // At minimum opacity
        var index = elementArr.indexOfByValue(elID);
        elementStatusArr[index] = "IDLE";
    }
    else {
        FadeOut(el, opacity, millisecFadeDelay, percentFadeAmount, percentMinOpacity);
    }
}

function IveriAnimaX_HaltFade(elID)
{
    if (elementArr.indexOfByValue(elID) != -1) 
        elementStatusArr[parseInt(elementArr.indexOfByValue(elID))] = "IDLE";
}

function FadeOut(el, opacity, millisecFadeDelay, percentFadeAmount, percentMinOpacity) 
{
    if (elementStatusArr[parseInt(elementArr.indexOfByValue(el.id))] == "OUT") {
        if (opacity - percentFadeAmount <= percentMinOpacity) {
            // Fadeout has reached its target
            opacity = percentMinOpacity;
            (isIE) ? (el.style.filter = "alpha(opacity=" + opacity + ")") : (el.style.opacity = opacity);
            elementStatusArr[parseInt(elementArr.indexOfByValue(el.id))] = "IDLE";
        }
        else {
            opacity = opacity - percentFadeAmount;
            (isIE) ? (el.style.filter = "alpha(opacity=" + opacity + ")") : (el.style.opacity = opacity);
            setTimeout(function() {
                FadeOut(el, opacity, millisecFadeDelay, percentFadeAmount, percentMinOpacity);
            }, millisecFadeDelay);
        }
    }
    else { }
        // Don't do anything, the fade has ended
}

function FadeIn(el, opacity, millisecFadeDelay, percentFadeAmount, percentMaxOpacity) 
{
    if (elementStatusArr[parseInt(elementArr.indexOfByValue(el.id))] == "IN") {
        if (opacity + percentFadeAmount >= percentMaxOpacity) {
            // Fadein has reached its target
            opacity = percentMaxOpacity;
            (isIE) ? (el.style.filter = "alpha(opacity=" + opacity + ")") : (el.style.opacity = opacity);
            elementStatusArr[parseInt(elementArr.indexOfByValue(el.id))] = "IDLE";
        }
        else {
            opacity = opacity + percentFadeAmount;
            (isIE) ? (el.style.filter = "alpha(opacity=" + opacity + ")") : (el.style.opacity = opacity);
            setTimeout(function() {
                FadeIn(el, opacity, millisecFadeDelay, percentFadeAmount, percentMaxOpacity);
            }, millisecFadeDelay);
        }
    }
    else { }
        // Don't do anything, the fade has ended
}


// Drag functions
// This script is used to create a drag panel with a boundary around it
// Firstly the boundary needs to be set statically
// currently the class of the tag needs to be 'drag' so it can be identified.

function IveriAnimaX_DragHorizontal(elID, boundLeft, boundRight, recoilPercentage, recoilSpeed) {
    _dragType = "horizontal";
    _boundaryLeft = parseInt(boundLeft);
    _boundaryRight = parseInt(boundRight);
    InitDragDrop();
}

function IveriAnimaX_DragVertical(elID, boundTop, boundBottom, recoilPercentage, recoilSpeed) {
    _dragType = "vertical";
    _boundaryTop = parseInt(boundTop);
    _boundaryBottom = parseInt(boundBottom);
    InitDragDrop();
}


function InitDragDrop() {
    document.onmousedown = OnMouseDown;
    document.onmouseup = OnMouseUp;
}

function OnMouseDown(e) {
    // IE is retarded and doesn't pass the event object
    if (e == null)
        e = window.event;

    // IE uses srcElement, others use target
    var target = e.target != null ? e.target : e.srcElement;

    // for IE, left click == 1
    // for Firefox, left click == 0
    if ((e.button == 1 && window.event != null ||
		    e.button == 0) &&
		    target.className == 'drag') {

        if (_dragType != "vertical") {
            // Get the clicked element's position
            _offsetX = ExtractNumber(target.style.left);
            // Get the mouse position
            _startX = e.clientX;
        }
        if (_dragType != "horizontal") {
            // Get the clicked element's position
            _offsetY = ExtractNumber(target.style.top);
            // Get the mouse position
            _startY = e.clientY;
        }

        // Save the target for OnMouseMove access
        _dragElement = target;

        // Bring to the front
        _oldZIndex = target.style.zIndex;
        target.style.zIndex = 10000;

        // tell our code to start moving the element with the mouse
        document.onmousemove = OnMouseMove;

        // cancel out any text selections
        document.body.focus();

        // prevent text selection in IE
        document.onselectstart = function() { return false; };
        // prevent IE from trying to drag an image
        target.ondragstart = function() { return false; };

        // prevent text selection (except IE)
        return false;
    }
}

function ExtractNumber(value) {
    var n = parseInt(value);

    return n == null || isNaN(n) ? 0 : n;
}

function OnMouseMove(e) {
    if (e == null)
        var e = window.event;

    if (_dragType != "vertical") {
        // Set the position variables for checking boundaries
        _dragElementLeft = trimPx(_dragElement.style.left);
        _dragElementRight = parseInt(_dragElementLeft) + parseInt(trimPx(_dragElement.style.width));
        // Check if the boundaries are breached
        if ((_dragElementLeft >= _boundaryLeft) && (_dragElementRight <= _boundaryRight))
            _dragElement.style.left = (_offsetX + e.clientX - _startX) + 'px';
    }
    if (_dragType != "horizontal") {
        // Set the position variables for checking boundaries
        _dragElementTop = trimPx(_dragElement.style.top);
        _dragElementBottom = parseInt(_dragElementTop) + parseInt(trimPx(_dragElement.style.height));
        // Check if the boundaries are breached
        if ((_dragElementTop >= _boundaryTop) && (_dragElementBottom <= _boundaryBottom))
            _dragElement.style.top = (_offsetY + e.clientY - _startY) + 'px';

    }    
}

function trimPx(px) {
    return px.substring(0, px.indexOf("px", 0));
}

function OnMouseUp(e) {
    if (_dragElement != null) {
        _dragElement.style.zIndex = _oldZIndex;

        // we're done with these events until the next OnMouseDown
        document.onmousemove = null;
        document.onselectstart = null;
        _dragElement.ondragstart = null;

        if (_dragType != "vertical") {
            if (_dragElementLeft < _boundaryLeft)
                _dragElement.style.left = parseInt(_boundaryLeft) + parseInt(1) + 'px';
            if (_dragElementRight > _boundaryRight)
                _dragElement.style.left = parseInt(_boundaryRight) - parseInt(trimPx(_dragElement.style.width)) - parseInt(1) + 'px';
        }
        if (_dragType != "horizontal") {
            if (_dragElementTop < _boundaryTop)
                _dragElement.style.top = parseInt(_boundaryTop) + parseInt(1) + 'px';
            if (_dragElementBottom > _boundaryBottom)
                _dragElement.style.top = parseInt(_boundaryBottom) - parseInt(trimPx(_dragElement.style.height)) - parseInt(1) + 'px';
        }
        // this is how we know we're not dragging
        _dragElement = null;
    }
}