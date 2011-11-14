using System;
using System.Collections.Generic;
using System.Text;

namespace virtualNodeNetwork
{
    public class CSharpNode : virtualNodeBase
    {
        public CSharpNode(int newId, string newName) : base(newId, newName) { }
        public CSharpNode(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors) : base(newId, newName, newSensors) { }

        private readonly byte[] _currentPacket = new byte[networkPacket.lengthInBytes];
        private int _currentPacketPos = 0;
        private int _consecutiveSyncBytes = 0;

        /// <summary>
        /// The synchronization symbol which can be repeated in order to reset to the start of a packet.
        /// </summary>
        private const byte _synchSymbol = 0xAA;
        
        /// <summary>
        /// Handle an incoming byte, adding it to any in-progress Packet
        /// </summary>
        /// <param name="byteIn">The byte to process</param>
        public void processByte(byte byteIn)
        {
            _currentPacket[_currentPacketPos] = byteIn;
            _currentPacketPos++;

            // We need to keep count of how many consecutive sync bytes we have seen, since if we
            // are out of sync they will not appear to be in the same packet.
            // For example, with a sync token of 0xAA:
            // Packet 1: 00 AA AA AA AA AA AA AA
            // Packet 2: AA ..             
            if (byteIn == _synchSymbol)
            {
                // OK, We've got another sync byte..
                _consecutiveSyncBytes++;

                // If we have an entire sync symbol then we need to rewind to the start of our packet
                // buffer, ready to accept new bytes form the start of the packet.
                _currentPacketPos = 0;
                syncPacket();
            }
            else
            {
                _consecutiveSyncBytes = 0;

                // If we have read an entire packet, then we need to parse the packet and process it.
                if (_currentPacketPos == networkPacket.lengthInBytes)
                {
                    networkPacket packet = new networkPacket(_currentPacket);
                    processPacket(packet);

                    _currentPacketPos = 0;
                }

            }

        }
        
        /// <summary>
        /// Handle an incoming networkPacket
        /// </summary>
        /// <param name="packet"></param>
        private void processPacket(networkPacket packet)
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

            initialChallengeResponsePacket resp = new initialChallengeResponsePacket(CSharpNetwork.controllerID, chal.challenge + 1, p);

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
                case commandByte.identify:
                    processIdentify(cmdPkt);
                    break;
                case commandByte.getSensor:
                    processGetSensor(cmdPkt);
                    break;
                case commandByte.setSensor:
                    processSetSensor(cmdPkt);
                    break;
                case commandByte.getSensorType:
                    processGetSensorType(cmdPkt);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void processPing(commandPacket packet)
        {
            // OK, a ping. Respond to it.
            log("Packet is a ping command.");

            pingResponsePacket resp = new pingResponsePacket(CSharpNetwork.controllerID, packet.challengeResponse + 1);

            // And send the packet with some test payload
            resp.payload = 0xefcdab;
            log("Ping packet response: " + resp.ToString());
            sendPacket(resp);

            stateChange(nodeState.idle);
        }

        private void processIdentify(commandPacket packet)
        {
            // OK, a ping. Respond to it.
            doIdentifyRequestPacket identifyPacket = new doIdentifyRequestPacket(packet);

            log("Packet is a name request packet. Decoded packet:");
            log(identifyPacket.ToString());

            // Verify some indicators of abnormality
            if (identifyPacket.unused != 0x00)
            {
                log(logLevel.warn, "unused byte is set to non-null");
                return;
            }

            doIdentifyResponsePacket resp = new doIdentifyResponsePacket(CSharpNetwork.controllerID, packet.challengeResponse + 1);

            // And send the packet with a part of our name string.
            ASCIIEncoding enc = new ASCIIEncoding();
            int startChar = identifyPacket.byteOffset * 3;
            int byteIndex = 0;
            for (int i = startChar; i < startChar + 3 ; i++)
            {
                byte toWrite = 0;
                if (i > name.Length - 1)
                {
                    toWrite = 0;
                }
                else
                {
                    toWrite = enc.GetBytes(name.ToCharArray(), i, 1)[0];
                }
                switch (byteIndex++)
                {
                    case 0:
                        resp.nameByte0 = toWrite;
                        break;
                    case 1:
                        resp.nameByte1 = toWrite;
                        break;
                    case 2:
                        resp.nameByte2 = toWrite;
                        break;
                }
            }

            log("Packet response: " + resp.ToString());
            sendPacket(resp);

            stateChange(nodeState.idle);
        }

        private void processGetSensor(commandPacket cmdPkt)
        {
            log("Packet is a GET_SENSOR command.");

            getSensorPacket req = new getSensorPacket(cmdPkt);

            challengeResponsePacket resp = new challengeResponsePacket(CSharpNetwork.controllerID, cmdPkt.challengeResponse + 1);

            if (req.isGetSensorCount)
            {
                log("Packet is requesting sensor count");
                resp.payload = sensors.Count;
            }
            else
            {
                // Find the value we wish to send, and set it in our packet
                resp.payload = sensors[req.sensorToInterrogate].getVal();
            }

            sendPacket(resp);
            stateChange(nodeState.idle);
        }

        private void processSetSensor(commandPacket cmdPkt)
        {
            log("Packet is a SET_SENSOR command.");

            setSensorPacket req = new setSensorPacket(cmdPkt);

            if (req.sensorToInterrogate == 0 ||
                req.sensorToInterrogate > sensors.Count )
            {
                // TODO: Respond with an error code, as the hardware should
                throw new NotImplementedException();
            }

            // TODO: Check sensor types

            virtualNodeSensor toChange = sensors[req.sensorToInterrogate];
            changeSensor(toChange, req.newValue);
            toChange.setValue(req.newValue);

            challengeResponsePacket resp = new challengeResponsePacket(CSharpNetwork.controllerID, cmdPkt.challengeResponse + 1);

            sendPacket(resp);

            stateChange(nodeState.idle);
        }
        
        private void processGetSensorType(commandPacket cmdPkt)
        {
            log("Packet is a GET_SENSOR_TYPE command.");

            getSensorTypePacket req = new getSensorTypePacket(cmdPkt);

            getSensorTypeResponsePacket resp = new getSensorTypeResponsePacket(CSharpNetwork.controllerID,
                                                                               cmdPkt.challengeResponse + 1);

            if (req.sensorToInterrogate == 0)
            {
                // This is illegal.
                throw new NotImplementedException();
            }

            if (!sensors.ContainsKey(req.sensorToInterrogate))
            {
                // Sensor to interrogate is not present. Signal this by setting the bottom bit of byte6
                // and setting byte8 to an error code.
                resp.isErrored = true;
                resp.errorCode = (int) errorCodes.sensorNotFound;
            }
            else
            {
                resp.payload = (int) sensors[req.sensorToInterrogate].type;
            }

            sendPacket(resp);

            stateChange(nodeState.idle);
        }    

    }

    enum errorCodes
    {
        sensorNotFound = 0x01
    }
}