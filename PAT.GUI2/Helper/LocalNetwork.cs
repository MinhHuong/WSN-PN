using PAT.Module.KWSN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAT.GUI.Helper
{
    public class LocalNetwork
    {
        public Hashtable mMapData = new Hashtable();
        public bool mSensorAbstracted;
        public bool mChannelAbstracted;
        public List<WSNSensor> mSensors;
        public List<WSNChannel> mChannels;

        public LocalNetwork(Hashtable map, bool sensorAbstracted, bool chanelAbstracted, 
            List<WSNSensor> sensors, List<WSNChannel> channels) {
            mMapData = map;
            mSensorAbstracted = sensorAbstracted;
            mChannelAbstracted = chanelAbstracted;
            mSensors = sensors;
            mChannels = channels;        
        }

        public WSNPNData getMapping(object id) {
            WSNPNData ret = null;
            do {
                if (mMapData.ContainsKey(id) == false)
                    break;

                ret = (WSNPNData)mMapData[id];
            } while (false);

            return ret;
        }

        public string getPlaceNameForToken()
        {
            string ret = "Output";
            if (mSensorAbstracted)
                ret = "Sensor";
            return ret;
        }
    }
}
