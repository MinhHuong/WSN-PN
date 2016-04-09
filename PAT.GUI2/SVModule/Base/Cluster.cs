using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAT.GUI.SVModule.Base
{
    class Cluster
    {
        public string mNameCluster;
        public string mClusterType;//0 is density, 1 is imbalance
        public double mAlpha;// if this cluster is density, alpha is area
        public int mNumSensor;
        public Cluster(string name, string type, double alpha, int NoSensor)
        {
            mNameCluster = name;
            mClusterType = type;
            mAlpha = alpha;
            mNumSensor = NoSensor;
        }
        public string setNameCluster
        {
            set { mNameCluster = value; }
        }
        public string setTypeCluster
        {
            set { mClusterType = value; }
        }
        public double setAlpha
        {
            set { mAlpha = value; }
        }
        public int setNumSensor
        {
            set { mNumSensor = value; }
        }
        public string getNameCluster()
        {
            return mNameCluster;
        }
        public string getTypeCluster()
        {
            return mClusterType;
        }
        public double getAlpha()
        {
            return mAlpha;
        }
        public double getNumSensor()
        {
            return mNumSensor;
        }
    }
}
