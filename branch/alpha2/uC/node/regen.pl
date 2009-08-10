
use Fcntl;

$templateFile=$ARGV[0];
$sensorlimit=100;

open SOURCE, ("< " . $templateFile )
  or die "Couldn't open input file " . $templateFile . " for reading\n";

print "; ************************************************ \n";
print "; **         THIS FILE IS AUTOGENERATED!        ** \n";
print "; ** YOU PROBABLY REALLY DON'T WANT TO EDIT IT! ** \n";
print "; ************************************************ \n";
print "; Source: " . $templateFile . "\n";


while (defined( my $line = <SOURCE>) )
{
 # is this the start of an autogenerate block?
 if ($line=~m/\(AUTOGEN_BEGIN_REPLICATING_BLOCK\)/ )
 {
  # read in the whole block
  $autogenblock="";
  $exitwhile=0;
  
  while ((defined( my $line = <SOURCE>)) && (!$exitwhile) )
  {
   # is this the end of the block?
   if ($line=~m/\(AUTOGEN_END_REPLICATING_BLOCK\)/ )
   {
    # Yeah, this is the end. Process this block.
    processBlock($autogenblock);
    $exitwhile=1;   
   } else {
    # this is a line in the autogen block.
    #print($line);
    $autogenblock = $autogenblock . $line;
   }
  }
 
 } else {
 # This is just a normal line of text.
 print $line;
 }
}

close(SOURCE)
  or die "Can't close input file";
  
sub processBlock
{
 $autogenblock=$_[0];
 
 for ($sensor=0; $sensor<$sensorlimit; $sensor++)
 {
  $replaced=$autogenblock;
  $replaced=~s/\(AUTOGEN_EVERY_SENSOR_ID\)/$sensor/g;
  print $replaced;
 }
 return;
}