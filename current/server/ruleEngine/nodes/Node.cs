using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using transmitterDriver;

namespace ruleEngine.nodes
{
    using netGui;

    public class Node
    {
        public ITransmitter Mydriver = null;
 
        #region delegates
        public delegate void makeWait(object sender);

        public delegate void EndWait(object sender);
        private event makeWait OnRequiredWait;
        private event EndWait OnRequiredWaitEnd;

        public void addRequiredWaitWhen(makeWait makeWaitCallack, EndWait endWaitCallback)
        {
            OnRequiredWait += makeWaitCallack;
            OnRequiredWaitEnd += endWaitCallback;
        }


        private void safelyMakeWait()
        {
            if (OnRequiredWait != null)
                OnRequiredWait.Invoke(this);
        }

        private void safelyEndWait()
        {
            if( OnRequiredWaitEnd != null)
                OnRequiredWaitEnd.Invoke(this);
        }
        #endregion

        public Node(ITransmitter driver, Int16 newid)
        {
            id = newid;
            Mydriver = driver;
            fillProperties();
            
        }
        public Node(Int16 newid)
        {
            id = newid;
        }
         
        public Int16 id;   
        private string _name;
        public string name 
        { 
            get
            {
                if (String.IsNullOrEmpty(_name))
                    _name = doGetName();
                return _name;
            }  
        }
        public Dictionary<Int16, sensor> sensors = new Dictionary<Int16, sensor>();

        public void fillProperties()
        {
            doPing();
            int sensorCount = getSensorCount();

            sensors.Clear();
            for (Int16 n = 1; n < sensorCount + 1; n++)
            {
                sensor newSensor = new sensor(this) { id = n };

                sensors.Add(newSensor.id, newSensor);
            }
        }

        public int getSensorCount()
        {
            safelyMakeWait();

            try
            {
                return Mydriver.doGetSensorCount(id);
            }
            finally
            {
                safelyEndWait();
            }
        }

        public void doPing()
        {
             safelyMakeWait();

            try
            {
                Mydriver.doPing(id);
            }
            finally
            {
                safelyEndWait();
            }
        }

        private string doGetName()
        {
            safelyMakeWait();

            try
            {
                string toReturn = Mydriver.doIdentify(id);
                return toReturn;
            }
            finally
            {
                safelyEndWait();
            }
        }
        public sensorType doGetSensorType(Int16 sensorId)
        {
            safelyMakeWait();

            try
            {
                return Mydriver.doGetSensorType(id, sensorId);
            }
            finally
            {
                safelyEndWait();
            }            
        }

        public Object updateValue(Int16 sensorId, bool silently)
        {
            if (!silently)
                safelyMakeWait();

            try
            {
                // todo: add code to accept different types of input
                return Mydriver.doGetGenericDigitalIn(id, sensorId);
            }
            finally
            {
                if (!silently)
                    safelyEndWait();
            }
        }
        public void setValue(Int16 sensorId, object toThis, bool silently)
        {
            if (!silently)
                safelyMakeWait();

            try
            {
                if (toThis is bool)
                {
                    if ((bool)toThis)
                        Mydriver.doSetGenericOut(id, 0x01, sensorId);
                    else
                        Mydriver.doSetGenericOut(id, 0x00, sensorId);
                }
                else if (toThis is short)
                {
                    Mydriver.doSetGenericOut(id, ((Int16)toThis), sensorId);
                }
                else if (toThis.GetType() == typeof(pwm_brightness))
                {
                    Mydriver.doSetGenericOut(id, ((pwm_brightness)toThis).fadeto, sensorId);
                }
                else if (toThis.GetType() == typeof(pwm_speed))
                {
                    Mydriver.doSetPWMSpeed(id, ((pwm_speed)toThis).fadespeed, sensorId);
                }
                else
                {
                    Mydriver.doSetGenericOut(id, (Int16)toThis, sensorId);       // best guess
                }
            }
            finally
            {
                if (!silently)
                    safelyEndWait();
            }
        }

        public void doSetNodeP(byte[] newP)
        {
            safelyMakeWait();

            try
            {
                Mydriver.doSetNodeP(id, newP);
            }
            finally
            {
                safelyEndWait();
            }
        }

        public void doSetNodeKey(key newKey)
        {
            safelyMakeWait();

            try 
            {
                Mydriver.doSetNodeKey(id, newKey.keyArray);
                Mydriver.doFlashReRead(id);
            }
            finally
            {
                safelyEndWait();
            }
        }

        public void doSetNodeId(Int16 newId)
        {
            safelyMakeWait();

            try 
            {
                Mydriver.doSetNodeId(id, newId);
            }
            finally
            {
                safelyEndWait();
            }
        }

        [Pure]
        public List<sensor> getSensors()
        {
            return sensors.Values.ToList();
        }

        [Pure]
        public List<sensor> getSensorsOfType(sensorType sensorType)
        {
            return sensors.Values.Where(s => s.type.enumeratedType == sensorType.enumeratedType).ToList();
        }

        [Pure]
        public bool hasSensorOf(sensorType selectedType)
        {
            return getSensorsOfType(selectedType).Count != 0;
        }

        public override string ToString()
        {
            
            return name + ":" + id;
        }

    }
}
