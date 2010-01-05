
class pin:
	name = 'name me';
	direction = 'input';
	state = False ;

class option:
	name = 'name me';
	value = 'no value';

class LavalampRuleItem_test:						# your class name _must_ begin with 'LavalampRuleItem_' !
	friendlyName = 'test python rule item'			# This is the name that lavalamp will use to display the rule item
	category     = 'test category'					# This optionally denotes the parent item in the toolbox

	# Create two string parameters
	clampToZero = option();
	IAmTheSecondOption = option();

	clampToZero.name ="clampToZero";
	clampToZero.value="first value";
	IAmTheSecondOption.name ="IAmTheSecondOption";
	IAmTheSecondOption.value="second value";
		
	parameters = [ clampToZero, IAmTheSecondOption ]			# optionally set up the 'parameters' array, which lavalamp will use to request per-item settings from the user

	myInputPin = pin();
	myOutputPin = pin();							# create two pins

	myInputPin.name = "myInputPin";
	myInputPin.direction = "input";					# give them unique names, and give them directions (both are mandatory)
	myOutputPin.name = "myOutputPin";
	myOutputPin.direction = "output";

	pins		  = [ myInputPin, myOutputPin ];	# set up the 'pins' array, which lavalamp will examine to make the ruleItem
			
	def eval(self):									# This is called to evaluate the block.
		if self.myInputPin.state==True:
			if (self.clampToZero.value == "yes"):
				self.myOutputPin.state=False;
			else:
				self.myOutputPin.state=True;
		else:
			self.myOutputPin.state=False;
			