using PAT.Common.GUI.Drawing;
using PAT.Common.ModelCommon;
using PAT.Common.ModelCommon.PNCommon;
using PAT.Common.ModelCommon.WSNCommon;
using PAT.Common.Utility;
using PAT.GUI.Helper;
using PAT.GUI.ModuleGUI;
using PAT.Module.KWSN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using Tools.Diagrams;

namespace PAT.GUI.Utility
{
    public class BuildUtils
    {
        #region embedded code on PN model
        // {0} - sensor ID
        private const string PROGRAM_SENSOR_RECEIVE =
            "var sub = util.getMin(b{0}, sbr{0});"
            + "\nif (testMode == 0)"
            + "\n\tsub = util.getRandInt(1, sub);"
            + "\nq{0} = q{0} + sub;"
            + "\nb{0} = b{0} - sub;"
            + "\n\nif (b{0} > 0)\n\tInput{0} = 1;";

        // {0} - channel ID / {1} - to sensor ID / {2} b/q
        private const string PROGRAM_CHANNEL_SEND =
            "var sub = util.getMin(b{0}, r{0});"
            + "\nif (testMode == 0)"
            + "\n\tsub = util.getRandInt(1, sub);"
            + "\n{2}{1} = {2}{1} + sub;"
            + "\nb{0} = b{0} - sub;"
            + "\nif (b{0} > 0)\n\tMain{0} = 1;";

        //// {0} - sensor from / {1} - channel ID / {2} - main/sensor
        //private const string PROGRAM_CHANNEL_RECEIVE =
        //    "var sub = util.getMin(q{0}, sqr{0});"
        //    + "\nif (sub > 0) {{"
        //    + "\n\tif (testMode == 0)"
        //    + "\n\t\tsub = util.getRandInt(1, sub);"
        //    + "\n\tb{1} = b{1} + sub;"
        //    + "\n\tq{0} = q{0} - sub;"
        //    + "\n}}"
        //    + "\n\nif (q{0} > 0)\n\t{2}{0} = 1;";

        // {0} - pkg/q of sensor - {1} sensor id - {2} - channel grp - {3} - main/sensor
        private const string PROGRAM_SENSOR_SEND =
            "var sub = util.getMin({0}, sqr{1});"
            + "\nif (testMode == 0)"
            + "\n\tsub = util.getRandInt(1, sub);"
            + "{2}"
            + "\n{0} = {0} - sub;"
            + "\n\nif ({0} > 0)\n\t{3}{1} = 1;";

        // {0} - sensor id / {1} - pkg/q of sensor id / {2} - grp
        private const string PROGRAM_SENSOR_SEND_UN =
            "var sub;"
            + "\nif (testMode == 0)"
            + "\n\tsub = util.getRandInt(1, sub);"
            + "{2}"
            + "\n\nif ({1} > 0)\n\tMain{0} = 1;";

        // {0} - sensor from / {1} - channel ID / {2} main/sensor
        private const string PROGRAM_CHANNEL_RECEIVE_SOURCE =
            "var sub = util.getMin(sqr{0}, pkg);"
            + "\n\tif (testMode == 0)"
            + "\n\t\tsub = util.getRandInt(1, sub);"
            + "\n\tb{1} = b{1} + sub;"
            + "\n\tpkg = pkg - sub;"
            + "\n\nif(pkg > 0)\n\t{2}{0} = 1;";

        // {0} - from sensor / {1} to sensor / {2} main/sensor
        private const string PROGRAM_CHANNEL_ASTRACTION_SOURCE =
            "var sub = util.getMin(pkg, sqr{0});"
            + "\nb{1} = b{1} + sub;"
            + "\npkg = pkg - sub;"
            + "\n\nif(pkg > 0)\n\t{2}{0} = 1;";

        // {0} - channel id - {1} - b1/q1 of sensor - {2} output of sensor/main
        private const string PROGRAM_CHANNEL_RECEIVE_BR =
            "var sub = util.getMin(b{0}, r{0});"
            + "\n{1} = {1} + sub;"
            + "\nb{0} = b{0} - sub;"
            + "\n\nif (b{0} > 0)\n\t{2}{0} = 1;";

        // {0} channel id, {1} pkg/qk of from sensor id
        private const string PROGRAM_CHANNEL_RECEIVE_UNI =
            "var sub = util.getMin({1}, r{0});"
            + "\nb{0} = b{0} + sub;"
            + "\n{1} = {1} - sub;"
            +"\nif ({1} > 0)\n\tConnIn{0} = 1;";

        // {0} - channel id, {1} pkg/qk of from sensor, {2} from sensor id, {3} to sensor id
        private const string PROGRAM_CHANNEL_ASTRACTION =
            "var sub;"
            + "\nif (b{0} == 0 && {1} > 0) {{"
            + "\n\tsub = util.getMin({1}, sqr{2});"
            + "\n\tb{0} = b{0} + sub;"
            + "\n\t{1} = {1} - sub;"
            + "\n\tConnIn{0} = 1;\n}}"
            + " else {{\n\tsub = util.getMin(b{0}, r{0});"
            + "\n\tb{3} = b{3} + sub;"
            + "\n\tb{0} = b{0} - sub;"
            + "\n\tif (b{0} > 0 || {1} > 0)\n\t\tConnIn{0} = 1;"
            + "\n}}";

        // {0} - channel id, {1} pkg/qk of from sensor, {2} from sensor id, {3} to sensor id, {4} grp
        private const string PROGRAM_CHANNEL_ASTRACTION_MC =
            "var sub;"
            + "\nif (b{0} == 0 && {1} > 0) {{"
            + "\n\tsub = util.getMin({1}, sqr{2});"
            + "\n\tb{0} = b{0} + sub;"
            + "\n\t{1} = {1} - sub;"
            + "\n\tConnIn{0} = 1;\n}}"
            + " else {{\n\tsub = util.getMin(b{0}, r{0});"
            + "{4}"
            + "\n\tb{0} = b{0} - sub;"
            + "\n\tif (b{0} > 0 || {1} > 0)\n\t\tConnIn{0} = 1;"
            + "\n}}";
        #endregion

        /// <summary>
        /// Get transition by Name and ID
        /// </summary>
        /// <param name="wsnData">Root xml content transision</param>
        /// <param name="name">Name of transition</param>
        /// <param name="id">ID of transition</param>
        /// <returns></returns>
        private static XmlNode getTransition(WSNPNData wsnData, string name, string id)
        {
            return wsnData.transitions.SelectSingleNode(
                String.Format("./Transition[@Name='{0}{1}']", name, id));
        }

        /// <summary>
        /// Get transition by Name and ID
        /// </summary>
        /// <param name="wsnData">Root xml content transision</param>
        /// <param name="name">Name of transition</param>
        /// <param name="id">ID of transition</param>
        /// <returns></returns>
        private static XmlNode getTransition(WSNPNData wsnData, string name, int id)
        {
            return getTransition(wsnData, name, id.ToString());
        }

        /// <summary>
        /// Set inner text for xmlNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="name">Name of node</param>
        /// <param name="text">Text value</param>
        private static void setXmlNodeData(XmlNode xmlNode, string name, string text)
        {
            XmlNode node = xmlNode.SelectSingleNode("./" + name);
            node.InnerText = text;
        }

        /// <summary>
        /// Embed code to sensor
        /// </summary>
        /// <param name="data">Sensor data</param>
        /// <param name="sensor">Sensor object</param>
        /// <param name="channels">Channel Object list</param>
        /// <param name="sensorAbstract">Flag sensor has abstracted</param>
        public static void embedCodeToSensor(WSNPNData data, WSNSensor sensor, IList<WSNChannel> channels, bool sensorAbstract)
        {
            XmlNode transition;
            int id = sensor.ID;

            switch (sensor.NodeType)
            {
                case SensorType.Source:
                    if (sensorAbstract)
                        break;

                    //if (Build.mMode == NetMode.UNICAST || Build.mMode == NetMode.MULTICAST)
                    //{
                    //    // {0} - sensor id / {1} - pkg/q of sensor id / {2} - grp
                    //    StringBuilder grpChannels = new StringBuilder();
                    //    foreach (WSNChannel channel in channels)
                    //    {
                    //        if (((WSNSensor)channel.From).ID != id)
                    //            continue;
                    //        grpChannels.AppendFormat("\n\nsub = util.getMin(pkg, sqr{0});", id);
                    //        grpChannels.AppendFormat("\nb{0} = b{0} + sub;", channel.ID);
                    //        grpChannels.Append("\npkg = pkg - sub;");                                
                    //    }

                    //    transition = getTransition(data, "Send", id);
                    //    setXmlNodeData(transition, "Program", 
                    //        String.Format(PROGRAM_SENSOR_SEND_UN, id, "pkg", grpChannels));
                    //    setXmlNodeData(transition, "Guard", "pkg > 0");

                    //    break;
                    //}

                    break;

                case SensorType.Intermediate:
                    // Not code inside sensor abstracted
                    if (sensorAbstract)
                        break;

                    //if (Build.mMode == NetMode.UNICAST || Build.mMode == NetMode.MULTICAST)
                    //{
                    //    StringBuilder grpChannels = new StringBuilder();
                    //    String queue = "q" + id;
                    //    foreach (WSNChannel channel in channels)
                    //    {
                    //        if (((WSNSensor)channel.From).ID != id)
                    //            continue;
                    //        grpChannels.AppendFormat("\n\nsub = util.getMin({0}, sqr{1});", queue, id);
                    //        grpChannels.AppendFormat("\nb{0} = b{0} + sub;", channel.ID);
                    //        grpChannels.AppendFormat("\n{0} = {0} - sub;", queue);
                    //    }

                    //    transition = getTransition(data, "Send", id);
                    //    setXmlNodeData(transition, "Program",
                    //        String.Format(PROGRAM_SENSOR_SEND_UN, id, queue, grpChannels));
                    //    setXmlNodeData(transition, "Guard", String.Format("{0} > 0", queue));
                    //}

                    // Receive transition
                    transition = getTransition(data, "Receive", id);
                    setXmlNodeData(transition, "Program", String.Format(PROGRAM_SENSOR_RECEIVE, id));
                    setXmlNodeData(transition, "Guard", String.Format("b{0} > 0 && b{0} <= S_MAX_BUFFER", id));

                    // Congestion transition
                    transition = getTransition(data, "Congestion", id);
                    setXmlNodeData(transition, "Guard", String.Format("b{0} > S_MAX_BUFFER", id));
                    break;

                    //// Send transition
                    //transition = getTransition(data, "Send", id);
                    //setXmlNodeData(transition, "Guard", String.Format("q{0} > 0", id));
                    //setXmlNodeData(transition, "Program", "");
                    //break;

                case SensorType.Sink:
                    transition = getTransition(data, "Receive", id);
                    setXmlNodeData(transition, "Program", String.Format("q{0} = q{0} + b{0};\nb{0} = 0;", id));
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Initialize code for sensor in multicast mode
        /// </summary>
        /// <param name="channel">Channel object</param>
        /// <returns>Program's code of transition source</returns>
        private static string initSendMulticast(WSNChannel channel, Boolean sensorAbstracted)
        {
            string id = channel.ID;
            StringBuilder ret = new StringBuilder();
            WSNSensor fSensor = (WSNSensor)channel.From;

            ret.AppendFormat("var sub = util.getMin(b{0}, r{0});", id);
            ret.AppendFormat("\n{0} = {0} + sub;", (sensorAbstracted ? "q" : "b") + ((WSNSensor)channel.To).ID);

            foreach (int sensorID in channel.SubIdList)
                ret.AppendFormat("\n{0} = {0} + sub;", (sensorAbstracted ? "q" : "b") + sensorID);
            ret.AppendFormat("\nb{0} = b{0} - sub;", id);
            ret.AppendFormat("\n\nif (b{0} > 0)\n\tMain{0} = 1;", id);
            return ret.ToString();
        }

        //*********************** CHANNEL BUILD UTILS **********************

        /// <summary>
        /// Compute center XY of between fromData and toData
        /// </summary>
        /// <param name="fromData"></param>
        /// <param name="toData"></param>
        /// <returns>Float point</returns>
        private static PointF computeCenterXY(WSNPNData fromData, WSNPNData toData)
        {
            PointF p = new PointF();
            p.X = (fromData.getOutNodePosition().X + toData.getInNodePosition().X) / 2f;
            p.Y = (fromData.getOutNodePosition().Y + toData.getInNodePosition().Y) / 2f;
            return p;
        }

        /// <summary>
        /// Build connector for multicast mode
        /// </summary>
        /// <param name="docOut">Xmldocument output</param>
        /// <param name="transitions">trans contain channels instance</param>
        /// <param name="arcs">arcs xmlelement</param>
        /// <param name="mapData">map id to data</param>
        /// <param name="_sensors">sensor list</param>
        /// <param name="channels">channel list</param>
        public static void buildConnOutSensorMU(XmlDocument docOut, XmlElement arcs, LocalNetwork localNetwork)
        {
            int fromId, toId;
            WSNPNData fromData, toData;
            foreach (WSNChannel channel in localNetwork.mChannels)
            {
                if (channel.Neighbor)
                    continue;

                fromId = ((WSNSensor)channel.From).ID;
                toId = ((WSNSensor)channel.To).ID;

                // arc from
                fromData = localNetwork.getMapping(fromId);
                toData = localNetwork.getMapping(channel.ID);

                // first arc
                arcs.AppendChild(buildArc(docOut, fromData.getOutNodeName(),
                    toData.getInNodeName(), computeCenterXY(fromData, toData)));

                fromData = localNetwork.getMapping(channel.ID);
                toData = localNetwork.getMapping(toId);

                // second arc
                arcs.AppendChild(buildArc(docOut,
                    fromData.getOutNodeName(),
                    toData.getInNodeName(), computeCenterXY(fromData, toData)));

                foreach (int item in channel.SubIdList)
                {
                    toData = localNetwork.getMapping(item);
                    if (item == fromId || toData == null)
                        continue;

                    arcs.AppendChild(buildArc(docOut, fromData.getOutNodeName(),
                        toData.getInNodeName(), computeCenterXY(fromData, toData)));
                }
            }
        }

        /// <summary>
        /// Build connector base
        /// </summary>
        /// <param name="_docOut">Xmldocument output</param>
        /// <param name="transName">trans contain channels intance</param>
        /// <param name="xPos">x position</param>
        /// <param name="yPos">y position</param>
        /// <returns></returns>
        private static XmlElement buildTransistion(XmlDocument _docOut, String transName, float xPos, float yPos)
        {
            XmlElement label;
            XmlElement position;
            XmlElement position2;
            XmlElement tran;
            XmlElement prog;

            tran = _docOut.CreateElement(XmlTag.TAG_TRANSITION);
            tran.SetAttribute(XmlTag.ATTR_NAME, transName);
            label = _docOut.CreateElement(XmlTag.TAG_LABEL);
            position = _docOut.CreateElement(XmlTag.TAG_POSITION);
            position2 = _docOut.CreateElement(XmlTag.TAG_POSITION);

            position.SetAttribute(XmlTag.ATTR_POSITION_X, xPos.ToString());
            position2.SetAttribute(XmlTag.ATTR_POSITION_X, (xPos - 0.1).ToString());

            position.SetAttribute(XmlTag.ATTR_POSITION_Y, yPos.ToString());
            position2.SetAttribute(XmlTag.ATTR_POSITION_Y, (yPos + 0.22).ToString());

            position.SetAttribute(XmlTag.ATTR_POSITION_WIDTH, "0.25");
            label.AppendChild(position2);
            tran.AppendChild(position);
            tran.AppendChild(label);
            tran.AppendChild(_docOut.CreateElement(XmlTag.TAG_GUARD));

            prog = _docOut.CreateElement(XmlTag.TAG_PROGRAM);
            prog.InnerText = "";
            tran.AppendChild(prog);

            return tran;
        }

        private static XmlElement buildPlace(XmlDocument _docOut, String placeName, float x, float y)
        {
            XmlElement label;
            XmlElement position;
            XmlElement position2;
            XmlElement place;

            place = _docOut.CreateElement(XmlTag.TAG_PLACE);
            place.SetAttribute(XmlTag.ATTR_NAME, placeName);
            label = _docOut.CreateElement(XmlTag.TAG_LABEL);
            position = _docOut.CreateElement(XmlTag.TAG_POSITION);
            position2 = _docOut.CreateElement(XmlTag.TAG_POSITION);

            position.SetAttribute(XmlTag.ATTR_POSITION_X, x.ToString());
            position2.SetAttribute(XmlTag.ATTR_POSITION_X, (x - 0.1).ToString());

            position.SetAttribute(XmlTag.ATTR_POSITION_Y, y.ToString());
            position2.SetAttribute(XmlTag.ATTR_POSITION_Y, (y + 0.22).ToString());

            position.SetAttribute(XmlTag.ATTR_POSITION_WIDTH, "0.25");
            label.AppendChild(position2);
            place.AppendChild(position);
            place.AppendChild(label);
            place.AppendChild(_docOut.CreateElement(XmlTag.TAG_GUARD));

            return place;
        }

        /// <summary>
        /// Build connector for broadcast mode
        /// </summary>
        /// <param name="docOut">Xmldocument output</param>
        /// <param name="transitions">trans contain channels instance</param>
        /// <param name="canvas"></param>
        /// <param name="places"></param>
        /// <param name="arcs"></param>
        /// <param name="mapData"></param>
        /// <param name="sensors"></param>
        /// <param name="channels"></param>
        /// <param name="abstractSensor"></param>
        /// <param name="abstractChannel"></param>
        public static void buildConnOutSensorBR(XmlDocument docOut, XmlElement transitions, XmlElement places, XmlElement arcs, LocalNetwork localNetwork)
        {
            float x;
            float y;
            foreach (WSNSensor sensor in localNetwork.mSensors)
            {
                if (sensor.NodeType == SensorType.Sink)
                    continue;

                // Build first transition 
                string transName = "ConnOut_" + sensor.ID;
                WSNPNData sensorData = localNetwork.getMapping(sensor.ID);

                if (sensorData == null)
                    continue;

                x = sensorData.getOutNodePosition().X + 0.8f;
                y = sensorData.getOutNodePosition().Y;
                XmlElement trans = buildTransistion(docOut, transName, x, y);

                // Add code to transition connector
                // {0} - pkg/q of sensor - {1} sensor id - {2} - channel grp - {3} - main/sensor
                StringBuilder channelGrp = new StringBuilder();
                bool isSource = sensor.NodeType.Equals(SensorType.Source);
                string queue = isSource ? "pkg" : ("q" + sensor.ID);
                foreach (WSNChannel channel in localNetwork.mChannels)
                    if (((WSNSensor)channel.From).ID == sensor.ID)
                        channelGrp.AppendFormat("\nb{0} = b{0} + sub;", channel.ID);

                string program = String.Format(PROGRAM_SENSOR_SEND, queue,sensor.ID, channelGrp, localNetwork.getPlaceNameForToken());
                setXmlNodeData(trans, "Program", program);
                setXmlNodeData(trans, "Guard",
                    isSource ? "pkg > 0" : String.Format("q{0} > 0 && q{0} <= S_MAX_BUFFER", sensor.ID));

                transitions.AppendChild(trans);
                arcs.AppendChild(buildArc(
                    docOut, sensorData.getOutNodeName(), transName, new PointF(x + 0.4f, y)));

                foreach (WSNChannel channel in localNetwork.mChannels)
                {
                    if (((WSNSensor)channel.From).ID != sensor.ID)
                        continue;

                    string placeName = "BrOut" + channel.ID;
                    WSNPNData channelData = localNetwork.getMapping(channel.ID);
                    if (channelData == null)
                        continue;

                    float fx = (x + channelData.getInNodePosition().X) / 2f;
                    float fy = (y + channelData.getInNodePosition().Y) / 2f;

                    // Build place connect from Connector_ to channels
                    places.AppendChild(buildPlace(docOut, placeName, fx, fy));
                    arcs.AppendChild(buildArc(docOut, transName, placeName, new PointF((x + fx) / 2f, (y + fy) / 2f)));
                    arcs.AppendChild(buildArc(docOut, placeName, channelData.getInNodeName(),
                        (fx + channelData.getInNodePosition().X) / 2f, (fy + channelData.getInNodePosition().Y) / 2f));
                }
            }
        }

        public static void buildConnOutSensorUN(XmlDocument docOut, XmlElement transitions, XmlElement places, XmlElement arcs, LocalNetwork localNetwork)
        {
            float x, y;
            foreach (WSNSensor sensor in localNetwork.mSensors)
            {
                foreach (WSNChannel channel in localNetwork.mChannels)
                {
                    WSNSensor fromS = (WSNSensor) channel.From;
                    if (fromS.ID != sensor.ID)
                        continue;

                    String transName = "UniIn" + channel.ID;
                    WSNPNData outSData = localNetwork.getMapping(sensor.ID);
                    WSNPNData inSData = localNetwork.getMapping(channel.ID);
                    x = (outSData.getOutNodePosition().X + inSData.getInNodePosition().X) / 2f;
                    y = (outSData.getOutNodePosition().Y + inSData.getInNodePosition().Y) / 2f;
                    XmlElement trans = buildTransistion(docOut, transName, x, y);
                    transitions.AppendChild(trans);

                    arcs.AppendChild(buildArc(
                        docOut, outSData.getOutNodeName(), transName, 
                        (x + outSData.getOutNodePosition().X) / 2f, (y + outSData.getOutNodePosition().Y) / 2f));

                    String placeName = "ConnIn" + channel.ID;
                    x = inSData.getInNodePosition().X - 1f;
                    y = inSData.getInNodePosition().Y;
                    XmlElement place = buildPlace(docOut, placeName, x, y);
                    places.AppendChild(place);

                    arcs.AppendChild(buildArc(
                        docOut, transName, placeName, x - 0.5f, y));
                    arcs.AppendChild(buildArc(
                        docOut, placeName, inSData.getInNodeName(), x - 0.15f, y));
                }                      
            }
        }

        public static void buildConnOutSensorMC(XmlDocument docOut, XmlElement transitions, XmlElement places, XmlElement arcs, LocalNetwork localNetwork)
        {
            buildConnOutSensorUN(docOut, transitions, places, arcs, localNetwork);

            int fromId, toId;
            WSNPNData fromData, toData;
            foreach (WSNChannel channel in localNetwork.mChannels)
            {
                if (channel.Neighbor)
                    continue;

                fromId = ((WSNSensor)channel.From).ID;
                toId = ((WSNSensor)channel.To).ID;

                fromData = localNetwork.getMapping(channel.ID);
                toData = localNetwork.getMapping(toId);

                foreach (int item in channel.SubIdList)
                {
                    toData = localNetwork.getMapping(item);
                    if (item == fromId || toData == null)
                        continue;

                    arcs.AppendChild(buildArc(docOut, fromData.getOutNodeName(),
                        toData.getInNodeName(), computeCenterXY(fromData, toData)));
                }
            }
        }

        public static void buildConnInSensor(XmlDocument docOut, XmlElement transitions, XmlElement places, XmlElement arcs, LocalNetwork localNetwork)
        {
            float x, y;
            foreach (WSNChannel channel in localNetwork.mChannels)
            {
                WSNPNData cData = localNetwork.getMapping(channel.ID);
                WSNPNData sData = localNetwork.getMapping(((WSNSensor)channel.To).ID);
                x = (cData.getOutNodePosition().X + sData.getInNodePosition().X) / 2f;
                y = (cData.getOutNodePosition().Y + sData.getInNodePosition().Y) / 2f;
                arcs.AppendChild(buildArc(docOut, cData.getOutNodeName(), sData.getInNodeName(), new PointF(x, y)));
            }
        }

        public static void buildConnInSensorImprove(XmlDocument docOut, XmlElement transitions, XmlElement places, XmlElement arcs, LocalNetwork localNetwork)
        {
            foreach (WSNSensor sensor in localNetwork.mSensors)
            {
                List<WSNChannel> channels = getChannelsInSensor(sensor.ID, localNetwork.mChannels);
                if (channels.Count == 0)
                    continue;

                WSNPNData sData = (WSNPNData)localNetwork.getMapping(sensor.ID);
                if (channels.Count == 1)
                {
                    WSNPNData cData = (WSNPNData)localNetwork.getMapping(channels[0].ID);
                    arcs.AppendChild(buildArc(docOut, cData.getOutNodeName(),
                        sData.getInNodeName(), computeCenterXY(cData, sData)));
                    continue;
                }

                float cx, cy;
                float fx, fy;
                string transName = "ConnIn" + sensor.ID;
                foreach (WSNChannel channel in channels)
                {
                    WSNPNData cData = localNetwork.getMapping(channel.ID);
                    cx = cData.getOutNodePosition().X;
                    cy = cData.getOutNodePosition().Y;
                    fx = (cx + sData.getInNodePosition().X) / 2f;
                    fy = (cy + sData.getInNodePosition().Y) / 2f;

                    String placeName = "BrFrom" + channel.ID;
                    XmlElement outPlace = buildPlace(docOut, placeName, fx, fy);
                    places.AppendChild(outPlace);

                    // Build Arcs for place
                    arcs.AppendChild(buildArc(docOut, cData.getOutNodeName(),
                        placeName, new PointF(fx, fy)));
                    arcs.AppendChild(buildArc(docOut, placeName,
                        transName, new PointF(fx, fy)));
                }

                //int size = channels.Count;
                //x = x / size;
                //y = y / size;
                //x = (x + sData.getInNodePosition().X) / 3f;
                //y = (y + sData.getInNodePosition().Y) / 3f;
                XmlElement outTrans = buildTransistion(docOut, transName, sData.getInNodePosition().X - 0.8f, sData.getInNodePosition().Y);
                transitions.AppendChild(outTrans);

                // Build connect from ConnectionIn to sensor
                arcs.AppendChild(buildArc(docOut, transName, sData.getInNodeName(), sData.getInNodePosition().X - 0.4f, sData.getInNodePosition().Y));
            }
        }

        private static List<WSNChannel> getChannelsInSensor(int sensorId, List<WSNChannel> allChannels)
        {
            List<WSNChannel> ret = new List<WSNChannel>();
            foreach (WSNChannel channel in allChannels)
            {
                if (((WSNSensor)channel.To).ID == sensorId)
                    ret.Add(channel);
            }

            return ret;
        }

        /// <summary>
        /// Combine code for channel 
        /// </summary>
        /// <param name="data">Channel data</param>
        /// <param name="channel">Channel item</param>
        /// <param name="channelAbstract"></param>
        /// <param name="sensorAbstract"></param>
        public static void embedCodeToChannel(WSNPNData data, WSNChannel channel, bool channelAbstract, bool sensorAbstract)
        {
            do
            {
                string id = channel.ID;
                XmlNode transition; // get xml node to transition then edit content
                WSNSensor fSensor = (WSNSensor)channel.From;
                WSNSensor tSensor = (WSNSensor)channel.To;
                bool isSource = (fSensor.NodeType == SensorType.Source);

                if (channelAbstract)
                {
                    switch (Build.mMode)
                    {
                        case NetMode.BROADCAST:
                            {
                                // {0} - channel id - {1} - b1/q1 of sensor - {2} sensor/main
                                transition = getTransition(data, "Channel", id);
                                string program = String.Format(PROGRAM_CHANNEL_RECEIVE_BR,
                                    channel.ID, (sensorAbstract ? "q" : "b") + tSensor.ID, "BrOut");

                                setXmlNodeData(transition, "Program", program);
                                setXmlNodeData(transition, "Guard",
                                     String.Format("b{0} > 0 &&  b{0} <= C_MAX_BUFFER", channel.ID));
                                break;
                            }

                        case NetMode.UNICAST:
                            {
                                // {0} - channel id, {1} pkg/qk of from sensor, {2} from sensor id, {3} to sensor id
                                transition = getTransition(data, "Channel", id);
                                setXmlNodeData(transition, "Program",
                                        String.Format(PROGRAM_CHANNEL_ASTRACTION, id, isSource ? "pkg" : ("q" + fSensor.ID), fSensor.ID, tSensor.ID));
                                setXmlNodeData(transition, "Guard", String.Format("b{0} <= C_MAX_BUFFER", id));
                                break;
                            }

                        case NetMode.MULTICAST:
                            {
                                // {0} - channel id, {1} pkg/qk of from sensor, {2} from sensor id, {3} to sensor id, {4} grp
                                transition = getTransition(data, "Channel", id);
                                StringBuilder programCode = new StringBuilder();
                                programCode.AppendFormat("\n\tb{0} = b{0} + sub;", tSensor.ID);
                                foreach (int sensorID in channel.SubIdList)
                                    programCode.AppendFormat("\n\tb{0} = b{0} + sub;", sensorID);

                                transition = getTransition(data, "Channel", id);
                                setXmlNodeData(transition, "Program",
                                        String.Format(PROGRAM_CHANNEL_ASTRACTION_MC, id, isSource ? "pkg" : ("q" + fSensor.ID), fSensor.ID, tSensor.ID, programCode.ToString()));
                                setXmlNodeData(transition, "Guard", String.Format("b{0} <= C_MAX_BUFFER", id));
                                break;
                            }

                        default:
                            break;
                    }
                    break;
                }

                // Same code inside congestion transition
                // Embed code for transition's congestion
                transition = getTransition(data, "Congestion", id);
                setXmlNodeData(transition, "Guard", String.Format("b{0} > C_MAX_BUFFER", id));

                switch (channel.Type)
                {
                    case ChannelType.Broadcast:
                        {
                            transition = getTransition(data, "Send", id);
                            string program = String.Format(PROGRAM_CHANNEL_RECEIVE_BR,
                                channel.ID,
                                (sensorAbstract ? "q" : "b") + tSensor.ID, "Main");

                            setXmlNodeData(transition, "Program", program);
                            setXmlNodeData(transition, "Guard",
                                 String.Format("b{0} > 0 &&  b{0} <= C_MAX_BUFFER", channel.ID));
                            break;
                        }

                    case ChannelType.Unicast:
                        {
                            // {0} channel id, {1} pkg/qk of from sensor id
                            transition = getTransition(data, "Receive", id);
                            setXmlNodeData(transition, "Program",
                                String.Format(PROGRAM_CHANNEL_RECEIVE_UNI, id, isSource ? "pkg" : ("q" + fSensor.ID)));
                            setXmlNodeData(transition, "Guard",
                                isSource ? "pkg > 0" : String.Format("q{0} > 0 && q{0} <= S_MAX_BUFFER", fSensor.ID));

                            // Embed code for transition's send
                            // {0} - channel ID / {1} - to sensor ID / {2} b/q - {3} pkg/q of sensor - {4} - sensor/output - {5} from sensor id
                            transition = getTransition(data, "Send", id);
                            setXmlNodeData(transition, "Program",
                                String.Format(PROGRAM_CHANNEL_SEND, id, tSensor.ID, sensorAbstract ? "q" : "b"));
                            setXmlNodeData(transition, "Guard", String.Format("b{0} > 0 && b{0} <= C_MAX_BUFFER", id));
                            break;
                        }

                    case ChannelType.Multicast:
                        {
                            // {0} channel id, {1} pkg/qk of from sensor id
                            transition = getTransition(data, "Receive", id);
                            setXmlNodeData(transition, "Program",
                                String.Format(PROGRAM_CHANNEL_RECEIVE_UNI, id, isSource ? "pkg" : ("q" + fSensor.ID)));
                            setXmlNodeData(transition, "Guard",
                                isSource ? "pkg > 0" : String.Format("q{0} > 0 && q{0} <= S_MAX_BUFFER", fSensor.ID));

                            // Embed code for transition's send
                            transition = getTransition(data, "Send", channel.ID);
                            setXmlNodeData(transition, "Program", initSendMulticast(channel, sensorAbstract));
                            setXmlNodeData(transition, "Guard", String.Format("b{0} > 0 && b{0} <= C_MAX_BUFFER", id));
                            break;
                        }

                    default:
                        break;
                }
            } while (false);
        }

        /// <summary>
        /// Build arc base
        /// </summary>
        /// <param name="docOut">Xmldocument output</param>
        /// <param name="fromName">from place name</param>
        /// <param name="toName">to place name</param>
        /// <param name="xPos">x position</param>
        /// <param name="yPos">y position</param>
        /// <returns></returns>
        private static XmlElement buildArc(XmlDocument docOut, string fromName, string toName, PointF pos)
        {
            if (pos == null)
                pos = new PointF(0f, 0f);
            return buildArc(docOut, fromName, toName, pos.X, pos.Y);
        }

        private static XmlElement buildArc(XmlDocument docOut, string fromName, string toName, float x, float y)
        {
            XmlElement arc;
            XmlElement label;
            XmlElement position;

            arc = docOut.CreateElement(XmlTag.TAG_ARC);
            arc.SetAttribute(XmlTag.TAG_ARC_PRO_FROM, fromName);
            arc.SetAttribute(XmlTag.TAG_ARC_PRO_TO, toName);
            arc.SetAttribute(XmlTag.TAG_ARC_PRO_WEIGHT, "1");

            label = docOut.CreateElement(XmlTag.TAG_LABEL);
            position = docOut.CreateElement(XmlTag.TAG_POSITION);
            position.SetAttribute(XmlTag.ATTR_POSITION_X, x.ToString());
            position.SetAttribute(XmlTag.ATTR_POSITION_Y, y.ToString());
            position.SetAttribute(XmlTag.ATTR_POSITION_WIDTH, "0.25");
            label.AppendChild(position);
            arc.AppendChild(label);
            return arc;
        }

        public static string buildDeclaration(KWSN.Model.WSNExtendInfo mExtendInfo, List<WSNSensor> sensors, List<WSNChannel> channels)
        {
            StringBuilder decBuild = new StringBuilder();
            CGNLevel level;

            decBuild.AppendFormat("\n#define S_MAX_BUFFER  {0};", mExtendInfo.mSensorMaxBufferSize);
            decBuild.AppendFormat("\n#define S_MAX_QUEUE  {0};", mExtendInfo.mSensorMaxQueueSize);
            decBuild.AppendFormat("\n#define C_MAX_BUFFER  {0};", mExtendInfo.mChannelMaxBufferSize);
            decBuild.Append("\nvar util = new PATUtils();");
            decBuild.AppendFormat("\nvar pkg = {0};", mExtendInfo.mNumberPacket);
            decBuild.Append("\n\n// For debug testing");
            decBuild.Append("\nvar testMode = 1;");

            foreach (WSNSensor sensor in sensors)
            {
                decBuild.AppendFormat("\n\n//Configure for sensor {0}", sensor.ID);

                level = sensor.CongestionLevel;
                decBuild.AppendFormat("\nvar b{0} = {1};", sensor.ID, computeSize(level, mExtendInfo.mSensorMaxBufferSize));
                decBuild.AppendFormat("\nvar q{0} = {1};", sensor.ID, computeSize(level, mExtendInfo.mSensorMaxQueueSize));
                decBuild.AppendFormat("\nvar sbr{0} = {1};", sensor.ID, sensor.ProcessingRate);
                decBuild.AppendFormat("\nvar sqr{0} = {1};", sensor.ID, sensor.SendingRate);
            }

            foreach (WSNChannel channel in channels)
            {
                decBuild.AppendFormat("\n\n//Configure for channel {0}", channel.ID);
                decBuild.AppendFormat("\nvar b{0} = {1};", channel.ID, computeSize(channel.CongestionLevel, mExtendInfo.mChannelMaxBufferSize));
                decBuild.AppendFormat("\nvar r{0} = {1};", channel.ID, channel.SendingRate);

                // Add probability of choosing path to congestion (must * 100 because PAT specification does not accept double number)
                decBuild.AppendFormat("\nvar prob{0} = {1};", channel.ID, channel.ProbabilityPathCongestion * 100f);
            }

            return decBuild.ToString();
        }

        private static int computeSize(CGNLevel level, int msize)
        {
            int ret = 0;
            switch (level)
            {
                case CGNLevel.Low:
                    break;

                case CGNLevel.Medium:
                    ret = (int)(msize * 0.9f);
                    break;

                case CGNLevel.High:
                    ret = (int)(msize * 1.1f);
                    break;

                default:
                    break;
            }

            return ret;
        }
    }
}