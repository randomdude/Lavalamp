using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using transmitterDriver;

namespace netGui
{
    public class Node
    {
        public ITransmitter Mydriver = null;
        private FrmMain _ownerWindow = null;
 
        #region delegates
        public delegate frmWait makeNewFrmWaitDelegateType();
        private readonly makeNewFrmWaitDelegateType makeNewFrmWaitDelegate;
        public delegate void closeFrmWaitDelegateType(frmWait thisone);
        private readonly closeFrmWaitDelegateType closeFrmWaitDelegate;

        public FrmMain OwnerWindow
        {
            get { return _ownerWindow; }
            set { _ownerWindow = value; }
        }

        private frmWait SafelyMakeNewFrmWait()
        {
            if (_ownerWindow != null && _ownerWindow.InvokeRequired)
                return (frmWait)_ownerWindow.Invoke(makeNewFrmWaitDelegate);
            else
                return makeNewFrmWaitDelegate();
        }

        private void closeFormWait(frmWait thisone)
        {
            thisone.Close();
        }

        private void safelyCloseFormWait(frmWait thisone)
        {
            if (_ownerWindow != null && _ownerWindow.InvokeRequired)
                _ownerWindow.Invoke(closeFrmWaitDelegate, thisone);
            else
                closeFormWait(thisone);
        }
        #endregion

        public Node(ITransmitter driver, Int16 newid)
        {
            id = newid;
            Mydriver = driver;
            makeNewFrmWaitDelegate = makeNewFrmWait;
            closeFrmWaitDelegate = closeFormWait;
            fillProperties();
            
        }
        public Node(Int16 newid)
        {
            id = newid;
            makeNewFrmWaitDelegate = makeNewFrmWait;
            closeFrmWaitDelegate = closeFormWait;
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

        private frmWait makeNewFrmWait()
        {
            frmWait holdup = new frmWait();
            if (null == _ownerWindow)
            {
                holdup.Show();
            }
            else
            {
                holdup.Visible = false;
                holdup.Show(_ownerWindow);
                holdup.center();
                holdup.Visible = true;
            }
            holdup.Update();
            return holdup;
        }

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
            frmWait holdup = SafelyMakeNewFrmWait();

            try
            {
                return Mydriver.doGetSensorCount(id);
            }
            finally
            {
                safelyCloseFormWait(holdup);
            }
        }

        public void doPing()
        {
            frmWait holdup = SafelyMakeNewFrmWait();

            try
            {
                Mydriver.doPing(id);
            }
            finally
            {
                safelyCloseFormWait(holdup);
            }
        }

        private string doGetName()
        {
            frmWait holdup = SafelyMakeNewFrmWait();

            try
            {
                string toReturn = Mydriver.doIdentify(id);
                return toReturn;
            }
            finally
            {
                safelyCloseFormWait(holdup);
            }
        }
        public sensorType doGetSensorType(Int16 sensorId)
        {
            frmWait holdup = SafelyMakeNewFrmWait();

            try
            {
                return Mydriver.doGetSensorType(id, sensorId);
            }
            finally
            {
                safelyCloseFormWait(holdup);
            }            
        }

        public Object updateValue(Int16 sensorId, bool silently)
        {
            frmWait holdup = null;
            if (!silently)
                holdup = SafelyMakeNewFrmWait();

            try
            {
                // todo: add code to accept different types of input
                return Mydriver.doGetGenericDigitalIn(id, sensorId);
            }
            finally
            {
                if (!silently)
                    safelyCloseFormWait(holdup);
            }
        }
        public void setValue(Int16 sensorId, object toThis, bool silently)
        {
            frmWait holdup = null;
            if (!silently)
                holdup = SafelyMakeNewFrmWait();

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
                    safelyCloseFormWait(holdup);
            }
        }

        public void doSetNodeP(byte[] newP)
        {
            frmWait holdup = SafelyMakeNewFrmWait();

            try
            {
                Mydriver.doSetNodeP(id, newP);
            }
            finally
            {
                safelyCloseFormWait(holdup);
            }
        }

        public void doSetNodeKey(key newKey)
        {
            frmWait holdup = SafelyMakeNewFrmWait();

            try 
            {
                Mydriver.doSetNodeKey(id, newKey.keyArray);
                Mydriver.doFlashReRead(id);
            }
            finally
            {
                safelyCloseFormWait(holdup);
            }
        }

        public void doSetNodeId(Int16 newId)
        {
            frmWait holdup = SafelyMakeNewFrmWait();

            try 
            {
                Mydriver.doSetNodeId(id, newId);
            }
            finally
            {
                safelyCloseFormWait(holdup);
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
