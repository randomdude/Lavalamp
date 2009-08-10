
class pin:
	name = 'name me';
	direction = 'input';
	state = False ;

class LavalampRuleItem_test:						# your class name _must_ begin with 'LavalampRuleItem_' !
	friendlyName = 'test python rule item'			# This is the name that lavalamp will use to display the rule item
	category     = 'test category'					# This optionally denotes the parent item in the toolbox

	myInputPin = pin();
	myOutputPin = pin();							# create two pins

	myInputPin.name = "myInputPin";
	myInputPin.direction = "input";					# give them unique names, and give them directions (both are mandatory)
	myOutputPin.name = "myOutputPin";
	myOutputPin.direction = "output";

	pins		  = [ myInputPin, myOutputPin ];	# set up the 'pins' array, which lavalamp will examine to make the ruleItem
	
	
	
	
	def eval(self):									# This is called to evaluate the block.
		if self.myInputPin.state==True:
			self.myOutputPin.state=True;
		else:
			self.myOutputPin.state=False;
			