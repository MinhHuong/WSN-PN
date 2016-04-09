using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAT.Common.GUI.ModelChecker
{

    public enum AssertType 
    {
        NONE,

        CONGESTION_SENSOR,
        CONGESTION_CHANNEL,
        DEADLOCK_FREE
    }

    public class LatexResult
    {
        //0: deadlockfree, 1:ChannelCongestion, 2: SensorCongestion
        public AssertType mType; // assertion type
        public float mMemo; // memory using
        public long mTransition;
        public long mState;
        public double mTime;
        public string mRes;
        public bool mClicked;

        public LatexResult(AssertType type, float memo, long trans, long state, double time, string res, bool clicked) {
            mType = type;
            mMemo = memo;
            mTransition = trans;
            mState = state;
            mTime = time;
            mRes = res;
            mClicked = clicked;
        }

        public static string convertAssertType(AssertType type)
        {
            string ret = "";
            switch (type) { 
                case AssertType.DEADLOCK_FREE:
                    ret = "DeadlockFree";
                    break;
                case AssertType.CONGESTION_CHANNEL:
                    ret = "CongestionChannel";
                    break;
                case AssertType.CONGESTION_SENSOR:
                    ret = "CongestionSensor";
                    break;

                default:
                    break;
            }

            return ret;
        }
    }
}
