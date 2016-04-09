using PAT.Common.ModelCommon;
using PAT.Common.ModelCommon.PNCommon;
using PAT.Common.ModelCommon.WSNCommon;
using PAT.Common.Utility;
using PAT.GUI.SVModule.Base;
using PAT.KWSN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PAT.GUI.SVModule
{
    class GraphXML
    {
        private const string TAG = "GraphXML";

        private List<Sensor> listSensors; //contains sensor
        private List<Link> listLinks; // contains string (id link)
        private List<String> stringSensors;
        private List<String> stringLinks;
        private List<String> Pathfull;

        public GraphXML()
        {
            listSensors = new List<Sensor>();
            listLinks = new List<Link>();
            stringSensors = new List<String>();
            stringLinks = new List<String>();

        }



        public void createXML(XmlTextWriter writer, List<Sensor> listSensors, List<Link> listLinks, List<String> Pathfull, List<String> Parameters)
        {
            writer.WriteStartElement(XmlTag.TAG_WSN);
            writer.WriteStartElement(XmlTag.TAG_DECLARATION);
            writer.WriteEndElement();
            writer.WriteStartElement(XmlTag.TAG_NETWORK);

            writer.WriteAttributeString(XmlTag.ATTR_mID, Parameters[0]);
            writer.WriteAttributeString(XmlTag.ATTR_NUMOFSENSORS, Parameters[1]);
            writer.WriteAttributeString(XmlTag.ATTR_NUMOFPACKETS, Parameters[2]);
            writer.WriteAttributeString(XmlTag.ATTR_SENSOR_MAX_BUFFER_SIZE, Parameters[3]);
            writer.WriteAttributeString(XmlTag.ATTR_SENSOR_MAX_QUEUE_SIZE, Parameters[4]);
            writer.WriteAttributeString(XmlTag.ATTR_CHANNEL_MAX_BUFFER_SIZE, Parameters[5]);

            writer.WriteStartElement(XmlTag.TAG_PROCESS);
            writer.WriteAttributeString(XmlTag.ATTR_NAME, "Network_1");
            writer.WriteAttributeString(XmlTag.ATTR_PRO_PARAM, "");
            writer.WriteAttributeString(XmlTag.ATTR_ZOOM, "1");
            writer.WriteAttributeString("StateCounter", "8");

            //Create sensors
            writer.WriteStartElement(XmlTag.TAG_SENSORS);
            foreach (Sensor test in listSensors)
                createSensor(test.getId().ToString(), test.getX().ToString(), test.getY().ToString(), test.getWidth().ToString(), writer, test.getstype().ToString(), test.getsending_rate().ToString(), test.getprocessing_rate().ToString(), test.getXLabel().ToString(), test.getYLabel().ToString(), test.getCGN().ToString());
            writer.WriteEndElement(); //End sensors

            //Create links
            writer.WriteStartElement(XmlTag.TAG_CHANNELS);
            Random r = new Random();
            foreach (Link testLink in listLinks)
                createLink(Pathfull, testLink.getLType(), testLink.getSource().getId().ToString(), testLink.getDest().getId().ToString(), r.Next(10).ToString(), r.Next().ToString(), r.Next().ToString(), writer, testLink.getTranfer_rate().ToString(), testLink.getCGN().ToString());

            writer.WriteEndElement(); //End <Links>

            writer.WriteEndElement(); //End <Process>

            writer.WriteEndElement(); //End <Network>

            writer.WriteEndElement(); //End <WSN>
        }

        public void createRandomXML(XmlTextWriter writer, int NUM_SENSOR, int NUM_LINK, List<String> Parameters)
        {

            writer.WriteStartElement(XmlTag.TAG_WSN);
            writer.WriteStartElement(XmlTag.TAG_DECLARATION);
            writer.WriteEndElement();
            writer.WriteStartElement(XmlTag.TAG_NETWORK);

            writer.WriteAttributeString(XmlTag.ATTR_NUMOFSENSORS, Parameters[0]);
            writer.WriteAttributeString(XmlTag.ATTR_NUMOFPACKETS, Parameters[1]);
            writer.WriteAttributeString(XmlTag.ATTR_SENSOR_MAX_BUFFER_SIZE, Parameters[2]);
            writer.WriteAttributeString(XmlTag.ATTR_SENSOR_MAX_QUEUE_SIZE, Parameters[3]);
            writer.WriteAttributeString(XmlTag.ATTR_CHANNEL_MAX_BUFFER_SIZE, Parameters[4]);

            writer.WriteStartElement(XmlTag.TAG_PROCESS);
            writer.WriteAttributeString(XmlTag.ATTR_NAME, "Network_1");
            writer.WriteAttributeString(XmlTag.ATTR_PRO_PARAM, "");
            writer.WriteAttributeString(XmlTag.ATTR_ZOOM, "1");
            writer.WriteAttributeString("StateCounter", "8");

            //Create sensors
            writer.WriteStartElement(XmlTag.TAG_SENSORS);

            randomSensors(NUM_SENSOR, writer);
            writer.WriteEndElement(); //End sensors

            //Create links
            writer.WriteStartElement(XmlTag.TAG_CHANNELS);
            randomLinks(NUM_LINK, NUM_SENSOR, writer);
            writer.WriteEndElement(); //End <Links>


            writer.WriteEndElement(); //End <Process>
            writer.WriteEndElement(); //End <Network>
            writer.WriteEndElement(); //End <WSN>
        }

        private void createSensor(string sID, string sX, string sY, string sWidth, XmlTextWriter writer, string sType, string smaxSendingRate, string smaxProcessingRate, string xLabel, string yLabel, string CGNSensor)
        {
            //Start Sensor
            //1 là source; 2 là sink; 3 là trung gian;
            writer.WriteStartElement(XmlTag.TAG_SENSOR);
            writer.WriteAttributeString(XmlTag.ATTR_NAME, "Sensor " + sID);
            writer.WriteAttributeString("Init", "False");
            writer.WriteAttributeString(XmlTag.ATTR_SENSOR_TYPE, sType.ToString());
            writer.WriteAttributeString(XmlTag.ATTR_ID, sID);
            writer.WriteAttributeString(XmlTag.ATTR_MAX_SENDING_RATE, smaxSendingRate);
            writer.WriteAttributeString(XmlTag.ATTR_MAX_PROCESSING_RATE, smaxProcessingRate);
            writer.WriteAttributeString(XmlTag.ATTR_CONGESTION_LEVEL, CGNSensor);

            //Postion
            writer.WriteStartElement(XmlTag.TAG_POSITION);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_X, sX);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_Y, sY);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_WIDTH, sWidth);
            writer.WriteEndElement();

            //Label
            writer.WriteStartElement(XmlTag.TAG_LABEL);
            writer.WriteStartElement(XmlTag.TAG_POSITION);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_X, sX);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_Y, sY);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_WIDTH, sWidth);
            writer.WriteEndElement();
            writer.WriteEndElement();

            //End </Sensor>
            writer.WriteEndElement();
        }

        private void createLink(List<String> Pathfull, string lType, string lFrom, string lTo, string lX, string lY, string lWidth, XmlTextWriter writer, string lSendingRate, string CGNLink)
        {
            //Start Link
            writer.WriteStartElement(XmlTag.TAG_CHANNEL);
            writer.WriteAttributeString(XmlTag.ATTR_CHANNEL_KIND, lType);
            writer.WriteAttributeString(XmlTag.ATTR_LINK_TYPE, "0");
            writer.WriteAttributeString(XmlTag.ATTR_MAX_SENDING_RATE, lSendingRate);
            writer.WriteAttributeString(XmlTag.ATTR_ID, lFrom + "_" + lTo);
            writer.WriteAttributeString(XmlTag.ATTR_CONGESTION_LEVEL, CGNLink);

            //From
            writer.WriteStartElement(XmlTag.TAG_CHANNEL_FROM);
            writer.WriteString("Sensor " + lFrom);
            writer.WriteEndElement();

            //To
            writer.WriteStartElement(XmlTag.TAG_CHANNEL_TO);
            writer.WriteString("Sensor " + lTo);
            writer.WriteEndElement();

            //Path
            if (lType == "Virtual")
            {
                DevLog.d(TAG, "=============Export XML===============");
                DevLog.d(TAG, "From: " + lFrom + " to: " + lTo);
                writer.WriteStartElement(XmlTag.TAG_PATH);
                for (int i = 0; i < Pathfull.Count; i++)
                {
                    string temp = "";
                    string path = "";
                    path += Pathfull[i].ToString();
                    char[] d = new char[] { ';' };
                    string[] s1 = Pathfull[i].Split(d, StringSplitOptions.RemoveEmptyEntries);
                    for (int p = 0; p < s1.Length; p++)
                    {
                        temp += s1[p];
                        char[] c = new char[] { '-' };
                        string[] s2 = temp.Split(c, StringSplitOptions.RemoveEmptyEntries);
                        if ((s2[0].ToString() == lFrom) && (s2[s2.Length - 1].ToString() == lTo))
                        {
                            DevLog.d(TAG, "" + path);
                            writer.WriteString(path);
                            break;
                        }
                    }
                }
                writer.WriteEndElement();
            }

            //Select
            writer.WriteStartElement("Select");
            writer.WriteEndElement();

            //Event    
            writer.WriteStartElement("Event");
            writer.WriteEndElement();

            //ClockGuard
            writer.WriteStartElement("ClockGuard");
            writer.WriteEndElement();

            //Guard
            writer.WriteStartElement(XmlTag.TAG_GUARD);
            writer.WriteEndElement();

            //Program
            writer.WriteStartElement(XmlTag.TAG_PROGRAM);
            writer.WriteEndElement();

            //ClockReset
            writer.WriteStartElement("ClockReset");
            writer.WriteEndElement();

            //Label
            writer.WriteStartElement(XmlTag.TAG_LABEL);
            writer.WriteStartElement(XmlTag.TAG_POSITION);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_X, lX);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_Y, lY);
            writer.WriteAttributeString(XmlTag.ATTR_POSITION_WIDTH, lWidth);
            writer.WriteEndElement();
            writer.WriteEndElement();

            //End </Link>
            writer.WriteEndElement();
        }


        private void randomSensors(int numS, XmlTextWriter writer)
        {
            //random x, y 
            //so sanh trong array
            Random r = new Random();
            int maxRandom = 8;
            int countRandom = 0;
            //int maxRandom = numS / 20;
            //if (maxRandom < 15) maxRandom = 15;

            double khoangcach = 0.3;

            for (int i = 1; i <= numS; i++)
            {
                string id = i.ToString();
                //string w = (r.NextDouble()).ToString();
                string w = (r.Next(maxRandom) + r.NextDouble()).ToString();
                string x = "";
                string y = "";
                if (countRandom < 20)
                {
                    countRandom++;
                }
                else
                {
                    maxRandom++;
                    countRandom = 0;
                }

                string pos;
                int count = 0;
                do
                {
                loop:
                    x = (r.Next(maxRandom) + r.NextDouble()).ToString("0.0");
                    y = (r.Next(maxRandom) + r.NextDouble()).ToString("0.0");
                    pos = x + "-" + y;
                    Sensor sen = new Sensor(i, double.Parse(x), double.Parse(y));
                    bool flag = true;
                    if (!stringSensors.Contains(pos))
                    {
                        if (listSensors != null)
                        {
                            foreach (Sensor sensorR in listSensors)
                            {
                                if (TinhKhoangCach(sen.getX(), sen.getY(), sensorR.getX(), sensorR.getY()) <= khoangcach)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (flag)
                        {
                            stringSensors.Add(pos);
                            listSensors.Add(sen);
                            break;
                        }
                        else
                        {
                            count++;
                            if (count > 100)
                            {
                                khoangcach = khoangcach + 0.1;
                            }
                            goto loop;
                        }
                    }
                }
                while (stringSensors.Contains(pos));


                if (i == 1)
                {
                    createSensor(id, x, y, w, writer, "1", r.Next(2, 100).ToString(), r.Next(2, 100).ToString(), r.Next(2, 10).ToString(), r.Next(2, 10).ToString(), "0");
                }
                else if (i == numS)
                {
                    createSensor(id, x, y, w, writer, "2", r.Next(2, 100).ToString(), r.Next(2, 100).ToString(), r.Next(2, 10).ToString(), r.Next(2, 10).ToString(), "0");
                }
                else
                {
                    createSensor(id, x, y, w, writer, "3", r.Next(2, 100).ToString(), r.Next(2, 100).ToString(), r.Next(2, 10).ToString(), r.Next(2, 10).ToString(), "0");
                }
            }
        }



        private void randomLinks(int numL, int numS, XmlTextWriter writer)
        {
            //random x, y 
            //so sanh trong array
            Random r = new Random();
            string id = "";
            string id2 = "";
            string from = "";
            string to = "";

            int tempfrom = 0;
            int tempto = 0;
            int linkrandom = numS * 2 - 2;
            double khoangcach = 0.3;

            for (int i = 1; i <= numL; i++)
            {
                int count = 0;
                do                          //NUM_SENSOR là link, 1 là source   (V)
                {
                loop:
                    if (i < numS)
                    {
                        from = i.ToString();
                    }
                    else
                    {
                        tempfrom = (r.Next(1, numS + 1));
                        if (tempfrom == numS)
                        {
                            goto loop;
                        }
                        else
                        {
                            from = tempfrom.ToString();
                        }
                    }

                    if (i == numS)
                    {
                        to = i.ToString();
                    }
                    else
                    {
                        if (numS < i && i <= linkrandom)
                        {
                            int j = i - numS + 1;
                            to = j.ToString();
                        }
                        else
                        {
                            tempto = (r.Next(1, numS + 1));
                            if (tempto == 1)
                            {
                                goto loop;
                            }
                            else
                            {
                                to = tempto.ToString();
                            }
                        }
                    }

                    if (from == "1" && to == numS.ToString())
                    {
                        goto loop;
                    }
                    id = from + "_" + to;
                    id2 = to + "_" + from;
                    if (stringLinks.Contains(id2) || from.Equals(to))
                    {
                        continue;
                    }
                    else if (!stringLinks.Contains(id))
                    {
                        double x1 = 0;
                        double y1 = 0;
                        double x2 = 0;
                        double y2 = 0;
                        foreach (Sensor sen in listSensors)
                        {
                            if (x1 != 0 && y1 != 0 && x2 != 0 && y2 != 0)
                            {
                                break;
                            }
                            if (from == sen.getId().ToString())
                            {
                                x1 = sen.getX();
                                y1 = sen.getY();
                                continue;
                            }
                            if (to == sen.getId().ToString())
                            {
                                x2 = sen.getX();
                                y2 = sen.getY();
                                continue;
                            }
                        }
                        if (TinhKhoangCach(x1, y1, x2, y2) < khoangcach)
                        {
                            stringLinks.Add(id);
                            break;
                        }
                        else
                        {
                            count++;
                            if (count > 100)
                            {
                                khoangcach = khoangcach + 0.1;
                            }
                            goto loop;
                        }
                    }
                }
                while (stringLinks.Contains(id) || from.Equals(to) || stringLinks.Contains(id2));       //của V
                //while (stringLinks.Contains(id) && !from.Equals(to)) ;        //của Sỹ
                stringSensors.Add(id);
                createLink(Pathfull, "Real", from, to, r.Next().ToString(), r.Next().ToString(), r.Next().ToString(), writer, r.Next(2, 100).ToString(), "0");
            }
        }
        public double TinhKhoangCach(double Xf, double Yf, double Xt, double Yt)
        {
            double KhoangCach;
            double dX = Math.Abs(Xt - Xf);// x2-x1
            double dY = Math.Abs(Yt - Yf);//y2-y1
            KhoangCach = Math.Round(Math.Sqrt(dX * dX + dY * dY), 2);
            //int result = Convert.ToInt16(KhoangCach);
            return KhoangCach;
            //return result;
        }
    }
}
