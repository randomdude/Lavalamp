
PHYSICAL LAYER
~~~~~~~~~~~~~~

The system is comprised of a 'transciever' unit, usually under PC control, and a
number of 'node' units.Communication is always initiated by the transciever node
as no collision detection/handling is implimented. 
Communication between the transciever and any given node is done by sending a 64
bit (8 byte) packet via manchester encoding. The raw sequence '0110' signals the
start of a 64bit packet. During idle times, the link contains Manchester-encoded
00 bytes (ie, a square wave).

Wire view:
                     _   _   _   _
idle            : |_| |_| |_| |_| |_|  ...
                     _   ___ 
start of packet : |_| |_|   |_| < manchester encoded data>
			      ^ packet starts here!

The transciever, however, uses a different mechanism to communicate with the host
device (usually a PC), as communication is based on 8 bit values.  The tranceiver
buffers 8 bytes, and on reception of the 8th byte, transmits the recieved 8 bytes
to the wireless segment. To guard against the PC and transciever becoming unsynchronised
in terms of where one packet starts and the previous packet ends, a special 'start
of frame' marker is used, consisting of eight 0xAA bytes. When the tranceiver recieves 
this magic sequence, it will reset its packet buffer to zero, and discard any unsent 
data.

PROTOCOL LAYER
~~~~~~~~~~~~~~

A typical transaction is as follows:

1) PC sends synchronisation sequence to transciever
2) PC sends data packet to transciever, consisting of a random 4-byte challenge N
3) Node recieves challenge, and responds with a packet consisting of N+1 and a new 
   3-byte challenge, P
4) PC recieves challenge, and responds with P+1, and a command packet
5) Node carries out command specified, and returns a packet consisting of P+2 and
   response data
6) Operation is complete.

Eagle-eyed readers will notice that a 4 byte N is used with a 3 byte P. It is expected
that in the next release, N will be shrunk to 3 bytes, generating a spare byte, which 
can be used for a checksum.

TRANSPORT LAYER (?)
~~~~~~~~~~~~~~~~~~~

Taking the example exchange in the protocol layer, we give an example of the packet
format at each stage. Fields in the packet are referred to as follows:

 -----------------------+-------------------------------
|	byte		|	purpose                	|
 -----------------------+-------------------------------
|   0x00 through 0x03	|	Sequence number		|
|         0x04 		|	Target Node ID		|
|         0x05 		|	Command byte		|
|   0x06 through 0x07	|	Argument specific	|
 -----------------------+-------------------------------

1) PC sends synchronisation sequence to transciever
	The PC sends 0xAAAAAAAAAAAAAAAA to the tranciever unit. This does not get
	send out to the node.
2) PC sends data packet to transciever, consisting of a random challenge N
	The PC generates a random number, N, and sets the 'Sequence number' field to
	it. The 'Target Node ID' is set to the ID of the node being addressed, and
	the rest of the packet is meaningless.
	* sequence number 	= N
	* Node ID 		= ID of node in question
	* 0x05 through 0x07 	= Inconsequential
3) Node recieves challenge, and responds with a packet consisting of N+1 and a new challenge, P
	The node incriments N, and generates a new random three-byte number, P. N is sent 
	as the new 'sequence number', and P is transmitted in bytes 0x05 through 07. 
	The 'Target Node ID' is set to 0x00, the ID of the transciever.
	* sequence number 	= N
	* Node ID 		= 0x00
	* 0x05 through 0x07 	= P
4) PC recieves challenge, and responds with P+1, and a command packet
	The PC uses P+1 as a sequence number, and sets the rest of the packet up to
	execute a command-that is, sets the 'Target node ID' to the node being 
	addressed, as before, and sets the 'Command byte' to identify a command.
	How the 'Argument specific' bytes are set is dependant on the command being
	executed.
	* sequence number 	= P+1
	* Node ID 		= ID of node in question
	* 0x05 			= Desired command
	* 0x06 through 0x07 	= command specific
5) Node carries out command specified, and returns a packet consisting of P+2 and response data
	The node does whatever it has been commanded to do, and returns 
	* sequence number = P+2
	* Node ID = 0x00
	* 0x05 through 0x07 - command specific	
6) Operation is complete.
	No further action is neccesary to teardown the session.

A worked example of a CMD_PING (id 0x01) being sent to node 0x01

2)
	PC sends :
	* SEQ number - 0x2923be84
	* Data bytes - 0x01 0xe1 0x6c 0xd6

	( here we see challenge 0x2923be84 being sent to node 0x01 )

3)	Node sends :
	* SEQ number - 0x2923be85
	* Data bytes - 0x00 0x00 0x00 0x04

	( here the node has responded with N+1 and issued challenge 0x000004 )

4)	PC sends:
	* SEQ number - 0x00000005
	* Data bytes- 0x01 0x01 0x00 0x00

	( here the PC has responded with P+1, and specified a command ID of 0x01 with argument data 0x0000 )

5)	Node sends :
	* SEQ number - 0x00000006
	* Data bytes- 0x00 0x41 0x42 0x43

	( here the node responds with P+2, and returns some data '0x414243 ).

ENCRYPTION/AUTHENTICATION LAYER
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The system uses XTEA as an encryption and hashing algoritm, relying on a preshared 128
bit key. Although it is possible to support that per-node keys,this is not implimented
currently due to the administration overhead and possibility for crosstalk between nodes.

The encryption/authentication layer is designed such that it is realistically impossible
for an attacker to:

* Impersonate a transciever and control nodes
* Dump wireless traffic and observe traffic

For example, it will protect against an attacker trying to turn your lights on, and from
an attacker who attempts to 'listen in' when you ask your power meter how much power you
have used.

Please note that whenever a wireless segment is used, it is possible for an attacker to 
disrupt communication by sending radio garbage at a power higher than your transmitter.
Please do not rely on communication in any important production system. For example, it
is quite silly to set up a swipe-card based door entry system that uses a base PC (over
RF) to decide if a given card should be allowed access, but more sensible to store that
list of authorized cards on the sensor node and use RF for updating, and recording logs
every day. Please use your intelligence when implimenting such a system,  using a wired
network if it is more sensible.


TODO:
~~~~~

* Impliment a command to get sensor type - at present you have to know previously that a sensor is
  eg. an input, so that you know that you should use the GET_SENSOR command on it.
* Change N to three-byte, test N and P exhaustively
* check sync still works