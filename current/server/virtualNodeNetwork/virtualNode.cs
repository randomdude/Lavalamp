using System;

namespace virtualNodeNetwork
{
    public class virtualNode
    {
        public int id;
        public string name;

        public Action<logLevel, string> onLog;
        public Action<networkPacket> onSendPacket;
        public Action<virtualNode, nodeState> onStateChange;
        public Action<virtualNode> onCryptoError;

        public nodeState state;
        private int p = 0x112233;

        public virtualNode(int newId, string newName)
        {
            id = newId;
            name = newName;
        }

        protected void log(string toLog)
        {
            log(logLevel.info, toLog );
        }

        private void log(logLevel level, string toLog)
        {
            if (onLog != null)
                onLog.Invoke(level, toLog);
        }

        protected void sendPacket(networkPacket toSend)
        {
            if (onSendPacket != null)
                onSendPacket(toSend);
        }

        public void processPacket(networkPacket packet)
        {
            if (packet.destinationNodeID != id)
                throw new ArgumentException();

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

        private void stateChange( nodeState newState)
        {
            if (onStateChange != null)
                onStateChange.Invoke(this, newState);

            state = newState;
        }

        private void cryptoError()
        {
            if (onCryptoError != null)
                onCryptoError.Invoke(this);
        }
    }

    public enum nodeState
    {
        idle,                       // The node is idle.
        firstHandshakeInProgress    // The node has sent its response to the first challenge packet, and is waiting for the controller to reply to it with a command.
    }

    public enum commandByte
    {
        unknown,
        ping
    }
}