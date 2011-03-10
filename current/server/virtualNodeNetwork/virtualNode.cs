using System;

namespace virtualNodeNetwork
{
    public class virtualNode : virtualNodeBase
    {
        public virtualNode(int newId, string newName) : base(newId, newName) { }

        /// <summary>
        /// Handle an incoming networkPacket
        /// </summary>
        /// <param name="packet"></param>
        public void processPacket(networkPacket packet)
        {
            if (packet.destinationNodeID != id)
                return;

            switch (state)
            {
                case nodeState.idle:
                    processFirstPacket(packet);
                    break;
                case nodeState.firstHandshakeInProgress:
                    processCommandPacket(packet);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void processFirstPacket(networkPacket packet)
        {
            // We have just got the first packet of our handshake. We should inc the sequence number, and place
            // our challenge P in the data payload, and send the challenge back to the controller.
            initialChallengePacket chal = new initialChallengePacket(packet);

            log("Packet is initial challenge. Decoded packet:");
            log(chal.ToString());

            initialChallengeResponsePacket resp = new initialChallengeResponsePacket(virtualNetwork.controllerID, chal.challenge + 1, p);

            log("Sending response:");
            log(resp.ToString());

            sendPacket(resp);

            stateChange( nodeState.firstHandshakeInProgress );
        }

        private void processCommandPacket(networkPacket packet)
        {
            // We received a command packet. Authenticate it and then process it.
            commandPacket cmdPkt = new commandPacket(packet);

            log("Packet is a command packet. Decoded packet:");
            log(cmdPkt.ToString());

            // Verify the sequence number
            if (cmdPkt.challengeResponse != p + 1)
            {
                // Bad auth! drop the packet and reset to our first state.
                log("A crypto error has occurred");
                cryptoError();
                stateChange(nodeState.idle);
                return;
            }

            switch (cmdPkt.findCommandByteMeaning())
            {
                case commandByte.unknown:
                    log("Ignoring unknown command");
                    break;
                case commandByte.ping:
                    processPing(cmdPkt);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void processPing(commandPacket packet)
        {
            // OK, a ping. Respond to it.
            log("Packet is a ping command.");

            pingResponsePacket resp = new pingResponsePacket(virtualNetwork.controllerID, packet.challengeResponse + 1);

            // And send the packet with some test payload
            resp.payload = 0xefcdab;
            log("Ping packet response: " + resp.ToString());
            sendPacket(resp);

            stateChange(nodeState.idle);
        }
    }
}