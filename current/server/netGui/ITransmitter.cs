﻿using System;
using netbridge;

namespace netGui
{
    public interface ITransmitter : IDisposable
    {
        bool portOpen();
		String doIdentify(Int16 nodeId);
        Int16 doGetSensorCount(Int16 nodeId);
		void doPing(Int16 nodeId);
		Object doGetValue(sensorType thisSensorType,  Int16 nodeId, Int16 sensorId);
        bool doGetGenericDigitalIn(Int16 nodeId, Int16 sensorId);
		void doSetGenericOut(Int16 nodeId, Int16 toThis, Int16 sensorId);

		sensorType doGetSensorType(Int16 nodeId, Int16 sensorId);
		void doSetPWMSpeed(Int16 nodeId, Int16 speed, Int16 sensorId);
		void doSetNodeP(Int16 nodeId,  Byte[] newP);
        void doSetNodeId(Int16 nodeId, Int16 newNodeId);
        void doFlashReRead(Int16 nodeId);
        void doSetNodeKey(Int16 nodeId, Byte[]  key);
    }
}
