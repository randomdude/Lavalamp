
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

function updateSensorCount(e)
{
    var i;
    
    sensorCount = parseInt(document.getElementById('sensorCount').value);
    
    $("#test").fadeOut(1000);
    $("#test").empty();
    for(i=0; i<sensorCount; i++)
    {
        $("#test").append( $("#nodeBoxBase").clone() )
    }
    $("#test").siblings().fadeIn(1000).show();
 
}