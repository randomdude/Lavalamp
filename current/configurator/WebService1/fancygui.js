
var sensorCount=1;

function isDigit(tryThis)
{
    return (  !( tryThis < 48 || tryThis > 57) )
}

function isComprisedOfDigits(tryThis)
{
 var i;
 
 for(i = 0; i<tryThis.length; i++)
    if (! isDigit(tryThis.charCodeAt(i)))
        return false;
 
 return true;
}

function validateSensorCount(e, name)
{
    var newKey;
    
    if(window.event) // IE
	    newKey = e.keyCode;
    else if(e.which) // Netscape/Firefox/Opera
	    newKey = e.which;
	
    var sensorCountControl = document.getElementById(name);
    var newString = new String(sensorCountControl.value);
    
    if (newKey == 8)    // backspace
    {
        newKey = 48;
        newString = newString.substr(0, newString.length-1 );
    } else {
        newString = newString + String.fromCharCode(newKey) ;
    }
        
    if ( isComprisedOfDigits(newString ) )
    {
      sensorCountControl.style.backgroundColor='lightgreen';
    } else {
      sensorCountControl.style.backgroundColor='pink';
    }
}

function removeThisSensor(i)
{
  var targetBox = $("#nodeBoxes>#nodeBox")[i].show().hide();
}

function addSensor()
{
    var newBox = $("#nodeBoxBase").clone().css("display","block").attr("id", "nodeBox");
    var nodeCnt = $("#nodeBoxBase").length;
    
    $("#nodeBoxes").append( newBox );   
    newBox.attr("id", "tmpBox");
    tmpBox.onClick = "javascript:removeThisSensor(1)"    // todo - find better way of doing this!
    newBox.attr("id", "nodeBox");    
    
    newBox.hide().show("slow")
}

function removeSensor()
{
    // If other stuff is going on, let it finish first
    if (0 != $("#nodeBoxes>#nodeBox:animated").length)
        return;

    var targetBox = $("#nodeBoxes>#nodeBox:last");
    
    targetBox.show().hide("slow", function () 
      {
       $("#nodeBoxes>#nodeBox:last").attr("id","deletedNodeBox"); 
      } 
     );
}

function updateSensorCount(e)
{
/*    var i;
        
    $("#nodeBoxes").fadeOut(1000);
    $("#nodeBoxes").empty();
    for(i=0; i<sensorCount; i++)
    {
        $("#nodeBoxes").append( $("#nodeBoxBase").clone() )
    }
    $("#nodeBoxes").siblings().fadeIn(1000).show();
 */
}