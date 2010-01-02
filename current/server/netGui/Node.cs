using System;
using System.Collections.Generic;
using netbridge;

namespace netGui
{
    public class Node
    {
        public ITransmitter Mydriver = null;
        private FrmMain ownerWindow = null;
 
        #region delegates
        public delegate frmWait makeNewFrmWaitDelegateType();
        private readonly makeNewFrmWaitDelegateType makeNewFrmWaitDelegate;
        public delegate void closeFrmWaitDelegateType(frmWait thisone);
        private readonly closeFrmWaitDelegateType closeFrmWaitDelegate;

        public FrmMain OwnerWindow
        {
            get { return ownerWindow; }
            set { ownerWindow = value; }
        }

        private frmWait SafelyMakeNewFrmWait()
        {
            if (ownerWindow.InvokeRequired)
                return (frmWait)ownerWindow.Invoke(makeNewFrmWaitDelegate);
            else
                return makeNewFrmWaitDelegate();
        }

        private void closeFormWait(frmWait thisone)
        {
            thisone.Close();
        }

        private void safelyCloseFormWait(frmWait thisone)
        {
            if (ownerWindow.InvokeRequired)
                ownerWindow.Invoke(closeFrmWaitDelegate, thisone);
            else
                closeFormWait(thisone);
        }
        #endregion

        public Node(ITransmitter driver, Int16 newid)
        {
            id = newid;
            Mydriver = driver;

            fillProperties();
            makeNewFrmWaitDelegate = MakeNewFrmWait;
            closeFrmWaitDelegate = closeFormWait;
        }
        public Node(Int16 newid)
        {
            id = newid;
            makeNewFrmWaitDelegate = MakeNewFrmWait;
            closeFrmWaitDelegate = closeFormWait;
        }

        public Int16 id;
        public string name;
        public Dictionary<Int16, sensor> sensors = new Dictionary<Int16, sensor>();

        private frmWait MakeNewFrmWait()
        {
            frmWait holdup = new frmWait();
            if (null == ownerWindow)
            {
                holdup.Show();
            }
            else
            {
                holdup.Visible = false;
                holdup.Show(ownerWindow);
                holdup.center();
                holdup.Visible = true;
            }
            holdup.Update();
            return holdup;
        }

        public void fillProperties()
        {
            doPing();
            name = doGetName();
            int sensorCount = getSensorCount();

            sensors.Clear();
            for (Int16 n=1; n < sensorCount+1; n++)
            {
                sensor newSensor = new sensor(this);
                newSensor.id = n;

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
        public string doGetName()
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

        public Object UpdateValue(Int16 sensorId, bool silently)
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
        public void SetValue(Int16 sensorId, object toThis, bool silently)
        {
            frmWait holdup = null;
            if (!silently)
                holdup = SafelyMakeNewFrmWait();

            try
            {
                if (toThis.GetType() == typeof(bool))
                {
                    if ((bool)toThis)
                        Mydriver.doSetGenericOut(id, 0x01, sensorId);
                    else
                        Mydriver.doSetGenericOut(id, 0x00, sensorId);
                }
                else if (toThis.GetType() == typeof(Int16))
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
    }
}
