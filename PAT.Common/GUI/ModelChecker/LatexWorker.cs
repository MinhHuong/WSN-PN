using PAT.Common.Classes.ModuleInterface;
using PAT.Common.ModelCommon;
using PAT.Common.ModelCommon.PNCommon;
using PAT.Common.ModelCommon.WSNCommon;
using PAT.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace PAT.Common.GUI.ModelChecker
{
    public class LatexWorker
    {
        private static string ROOT_LATEX_PATH = Utilities.ROOT_WORKING_PATH + "\\latex";
        public static string TMP_LATEX_PATH = ROOT_LATEX_PATH + "\\tmp";
        public PNExtendInfo mExtendInfo;
        //public LatexResult1 mParseRes = null;
        public string mFileName, mPathTmp;
        public String modelatex;
        // pnmodellatex;

        // 0 deadlock
        // 1 channel congestion
        // 2 sensor congestion
        private LatexResult[] mParseRes = new LatexResult[3];
        private const string TAG = "LatexWorker";

        public LatexWorker(PNExtendInfo extendInfo, string filename)
        {
            mFileName = filename;
            mExtendInfo = extendInfo;
            //mParseRes = latexresult;

            // Init latex result
            mParseRes[0] = new LatexResult(AssertType.DEADLOCK_FREE, 0, 0, 0, 0d, "\\unk", false);
            mParseRes[1] = new LatexResult(AssertType.CONGESTION_CHANNEL, 0, 0, 0, 0d, "\\unk", false);
            mParseRes[2] = new LatexResult(AssertType.CONGESTION_SENSOR, 0, 0, 0, 0d, "\\unk", false);

            // Init folder
            try
            {
                Directory.CreateDirectory(ROOT_LATEX_PATH);
                Directory.CreateDirectory(TMP_LATEX_PATH);
                checkLatex();
            }
            catch (Exception e) { }

        }

        private void checkLatex()
        {
            //Check the existence of ID folder
            string rootPath = TMP_LATEX_PATH + "\\" + mExtendInfo.mID;
            mPathTmp = rootPath + "\\" + mExtendInfo.mID + "_" + mFileName.Substring(0, mFileName.Length - 3) + ".tex";

            // Init root path
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            // Init file
            if (!File.Exists(mPathTmp))
            {
                FileStream fileStream = new FileStream(mPathTmp, FileMode.Create);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Mode of network (broadcast/unicast/multicast)
        /// </summary>
        /// <returns></returns>
        private string getMode()
        {
            do
            {
                if (mExtendInfo == null)
                    break;

                String mode = mExtendInfo.mMode.ToString();

                if (mode == "UNICAST") modelatex = "\\uc";
                else if (mode == "BROADCAST") modelatex = "\\bc";
                else if (mode == "MULTICAST") modelatex = "\\mc";
            } while (false);

            return modelatex;
        }

        /// <summary>
        /// Get abstract level (Non abstracted, sensor abstracted, channel abstracted)
        /// </summary>
        /// <returns></returns>
        private string getAbstractLevel()
        {
            string ret = "";
            do
            {
                String absLevel = mExtendInfo.mAbsLevel;

                // mlqvu -- pre process for absLevel
                if (absLevel == "11") ret = "\\na";
                else if (absLevel == "10") ret = "\\ca";
                else if (absLevel == "01") ret = "\\sa";
            } while (false);
            return ret;
        }

        public void writeDeadlockFree(StreamWriter bwFile)
        {
            do
            {
                string abstractLevel = getAbstractLevel();
                if (abstractLevel.Equals("\\na"))
                {
                    if (mParseRes[0].mRes != "\\vl" && mParseRes[0].mRes != "\\nv" && mParseRes[0].mClicked)
                        bwFile.Write(String.Format("\\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\\n", mParseRes[0].mTime / 1.0d));
                    else
                        bwFile.Write(String.Format("{0:0.00}" + " & " + mParseRes[0].mTransition.ToString() + " & " + mParseRes[0].mState.ToString() + " & " + mParseRes[0].mRes + " \\\\\n", mParseRes[0].mMemo / 1024.0f));
                    break;
                }

                if (mParseRes[0].mRes != "\\vl" && mParseRes[0].mRes != "\\nv" && mParseRes[0].mClicked)
                {
                    bwFile.Write(String.Format("& & & & \\multirow{{2}}{{*}}{{" + abstractLevel + "}} & \\dl  & \\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\\n", mParseRes[0].mTime / 1.0d));
                    break;
                }

                bwFile.Write(String.Format("& & & & \\multirow{{2}}{{*}}{{" + abstractLevel + "}} & \\dl  & " + "{0:0.00}" + " & " + mParseRes[0].mTransition.ToString() + " & " + mParseRes[0].mState.ToString() + " & " + mParseRes[0].mRes + " \\\\\n", mParseRes[0].mMemo / 1024.0f));
            } while (false);
        }

        public void writeChannelCongestion(StreamWriter bwFile)
        {
            do
            {
                if (mParseRes[1].mRes != "\\vl" && mParseRes[1].mRes != "\\nv" && mParseRes[1].mClicked)
                {
                    bwFile.Write(String.Format("& & & & & \\chan & \\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\\n", mParseRes[1].mTime / 1.0d));
                    break;
                }

                bwFile.Write(String.Format("& & & & & \\chan & " + "{0:0.00}" + " & " + mParseRes[1].mTransition.ToString() + " & " + mParseRes[1].mState.ToString() + " & " + mParseRes[1].mRes + " \\\\\n", mParseRes[1].mMemo / 1024.0f));
            } while (false);
        }

        public void writeSensorCongestion(StreamWriter bwFile)
        {
            do
            {
                if (mParseRes[2].mRes != "\\vl" && mParseRes[2].mRes != "\\nv" && mParseRes[2].mClicked)
                {
                    bwFile.Write(String.Format("& & & & & \\sen & \\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\\n", mParseRes[2].mTime / 1.0d));
                    break;
                }

                bwFile.Write(String.Format("& & & & & \\sen  & " + "{0:0.00}" + " & " + mParseRes[2].mTransition.ToString() + " & " + mParseRes[2].mState.ToString() + " & " + mParseRes[2].mRes + " \\\\\n", mParseRes[2].mMemo / 1024.0f));
            } while (false);
        }

        public void Write()
        {
            FileStream myFile = null;
            StreamWriter bwFile = null;
            try
            {
                myFile = new FileStream(mPathTmp, FileMode.Create);
                bwFile = new StreamWriter(myFile);
                string abstractLevel = getAbstractLevel();
                switch (abstractLevel)
                {
                    case "\\na":
                        bwFile.Write(String.Format("% " + mExtendInfo.mNumberSensor.ToString() + " nodes - " + mExtendInfo.mNumberPacket.ToString() + " Packet - " + getMode() + "\n"));
                        bwFile.Write(String.Format("\\multirow{{10}}{{*}}{{" + mExtendInfo.mNumberSensor.ToString() + "}} & \\multirow{{10}}{{*}}{{" + mExtendInfo.mNumberPacket.ToString() + "}} & \\multirow{{10}}{{*}}{{" + mExtendInfo.mSensorMaxBufferSize.ToString() + "}} &\\multirow{{10}}{{*}}{{" + modelatex + "}} &\\multirow{{3}}{{*}}{{" + getAbstractLevel() + "}} & \\dl \n& "));
                        writeDeadlockFree(bwFile);
                        writeChannelCongestion(bwFile);
                        writeSensorCongestion(bwFile);

                        bwFile.Write(String.Format("\\cline{{5-10}}\n"));
                        break;

                    case "\\ca":
                        writeDeadlockFree(bwFile);
                        writeSensorCongestion(bwFile);
                        bwFile.Write(String.Format("\\cline{{5-10}}\n"));
                        break;

                    case "\\sa":
                        writeDeadlockFree(bwFile);
                        writeChannelCongestion(bwFile);
                        bwFile.Write(String.Format("\\hline\r\n\n"));
                        break;

                    default:
                        break;

                }
            }
            catch (Exception e)
            {
                DevLog.e("latexworker1", e.Message);
            }
            finally
            {
                if (bwFile != null)
                    bwFile.Close();

                if (myFile != null)
                    myFile.Close();
            }
        }

        public void checkForMethod()
        {
            getAbstractLevel();
            bool needNewWrite = (File.Exists(mPathTmp) && Utilities.GetFileSizeOnDisk(mPathTmp) > 0);
            if (needNewWrite)
            {
                int count = 0;
                int forceNumOfLine = (getAbstractLevel().Equals("\\na")) ? 3 : 2;

                for (int i = 0; i < forceNumOfLine; i++)
                    if (mParseRes[i].mTransition != 0) count++;

                if (count == forceNumOfLine)
                    Write();
                else
                    Update();
            }
            else
                Write();
        }

        public void Update()
        {
            string abstractLevel = getAbstractLevel();

            //chuoi DeadlockFree non-abstraction
            string res1 = String.Format("& {0:0.00} & " + mParseRes[0].mTransition + " & " + mParseRes[0].mState + " & " + mParseRes[0].mRes + " \\", mParseRes[0].mMemo / 1024.0f);
            //chuoi channel congestion non-abstraction & channel/sensor abstraction
            string res2 = String.Format("& & & & & \\chan & {0:0.00} & " + mParseRes[1].mTransition + " & " + mParseRes[1].mState + " & " + mParseRes[1].mRes + " \\", mParseRes[1].mMemo / 1024.0f);
            //chuoi sensor congestion non-abstraction & channel/sensor abstraction
            string res3 = String.Format("& & & & & \\sen & {0:0.00} & " + mParseRes[2].mTransition + " & " + mParseRes[2].mState + " & " + mParseRes[2].mRes + " \\", mParseRes[2].mMemo / 1024.0f);
            //chuoi DeadlockFree channel/sensor abstract
            string res4 = String.Format("& & & & \\multirow{{2}}{{*}}{{" + abstractLevel + "}} & \\dl  & {0:0.00} & " + mParseRes[0].mTransition + " & " + mParseRes[0].mState + " & " + mParseRes[0].mRes + " \\", mParseRes[0].mMemo / 1024.0f);
            //chuoi timeout deadlockfree non-abstraction
            string res5 = String.Format("\\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\", mParseRes[0].mTime / 1.0d);
            //chuoi timeout deadlockfree channel/sensor abstraction
            string res6 = String.Format("& & & & \\multirow{{2}}{{*}}{{" + abstractLevel + "}} & \\dl  & \\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\", mParseRes[0].mTime / 1.0d);
            //chuoi timeout channel congestion non-abstrction & channel/sensor abstraction
            string res7 = String.Format("& & & & & \\chan & \\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\", mParseRes[1].mTime / 1.0d);
            //chuoi timeout sensor congestion non-abstrction & channel/sensor abstraction
            string res8 = String.Format("& & & & & \\sen & \\multicolumn{{4}}{{|c|}}{{\\cellcolor{{blue!20}}Time out at {0:0.00}}} \\\\", mParseRes[2].mTime / 1.0d);
            List<string> lines = new List<string>();
            FileStream myFile = new FileStream(mPathTmp, FileMode.Open);
            StreamReader reader = new StreamReader(myFile);
            string line;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);
            reader.Close();
            myFile.Close();

            switch (abstractLevel)
            {
                case "\\na":
                    if (mParseRes[0].mTransition != 0 && mParseRes[0].mRes != "\\vl" && mParseRes[0].mRes != "\\nv")
                        lines[2] = res5;
                    if (mParseRes[0].mTransition != 0 && (mParseRes[0].mRes == "\\vl" || mParseRes[0].mRes == "\\nv"))
                        lines[2] = res1;
                    if (mParseRes[1].mTransition != 0 && mParseRes[1].mRes != "\\vl" && mParseRes[1].mRes != "\\nv")
                        lines[3] = res7;
                    if (mParseRes[1].mTransition != 0 && (mParseRes[1].mRes == "\\vl" || mParseRes[1].mRes == "\\nv"))
                        lines[3] = res2;
                    if (mParseRes[2].mTransition != 0 && mParseRes[2].mRes != "\\vl" && mParseRes[2].mRes != "\\nv")
                        lines[4] = res8;
                    if (mParseRes[2].mTransition != 0 && (mParseRes[2].mRes == "\\vl" || mParseRes[2].mRes == "\\nv"))
                        lines[4] = res3;
                    break;

                case "\\ca":
                    if (mParseRes[0].mTransition != 0 && mParseRes[0].mRes != "\\vl" && mParseRes[0].mRes != "\\nv")
                        lines[0] = res6;
                    if (mParseRes[0].mTransition != 0 && (mParseRes[0].mRes == "\\vl" || mParseRes[0].mRes == "\\nv"))
                        lines[0] = res4;
                    if (mParseRes[2].mTransition != 0 && mParseRes[2].mRes != "\\vl" && mParseRes[2].mRes != "\\nv")
                        lines[1] = res8;
                    if (mParseRes[2].mTransition != 0 && (mParseRes[2].mRes == "\\vl" || mParseRes[2].mRes == "\\nv"))
                        lines[1] = res3;
                    break;

                case "\\sa":
                    if (mParseRes[0].mTransition != 0 && mParseRes[0].mRes != "\\vl" && mParseRes[0].mRes != "\\nv")
                        lines[0] = res6;
                    if (mParseRes[0].mTransition != 0 && (mParseRes[0].mRes == "\\vl" || mParseRes[0].mRes == "\\nv"))
                        lines[0] = res4;
                    if (mParseRes[1].mTransition != 0 && mParseRes[1].mRes != "\\vl" && mParseRes[1].mRes != "\\nv")
                        lines[1] = res7;
                    if (mParseRes[1].mTransition != 0 && (mParseRes[1].mRes == "\\vl" || mParseRes[1].mRes == "\\nv"))
                        lines[1] = res2;
                    break;

                default:
                    break;

            }

            FileStream myWriteFile = null;
            StreamWriter writer = null;
            try
            {
                myWriteFile = new FileStream(mPathTmp, FileMode.Create);
                writer = new StreamWriter(myWriteFile);

                foreach (String item in lines)
                    writer.WriteLine(item);

                writer.Close();
                myWriteFile.Close();
            }
            catch (Exception e)
            {
                DevLog.e(TAG, e.Message);
            }
            finally
            {
                if (writer == null)
                    writer.Close();

                if (myWriteFile == null)
                    myWriteFile.Close();
            }
        }

        internal void Update(AssertionBase assertion)
        {
            // 2015-11-21-thu-update latex
            AssertType assertType = AssertType.NONE;
            string non = "deadlockfree", channel = "_";
            string assertion_text = assertion.ToString();

            if (assertion_text.Contains(non)) assertType = AssertType.DEADLOCK_FREE;
            else if (assertion_text.Contains(channel)) assertType = AssertType.CONGESTION_CHANNEL;
            else assertType = AssertType.CONGESTION_SENSOR;

            string result = assertion.getResult();
            string resultlatex = "";

            // Get result of assersion
            do
            {
                if (assertType.Equals(AssertType.DEADLOCK_FREE))
                {
                    if (result.Equals("VALID")) resultlatex = "\\vl";
                    else if (result.Equals("INVALID")) resultlatex = "\\nv";
                    else resultlatex = "\\unk";
                    break;
                }

                if (result.Equals("VALID")) resultlatex = "\\nv";
                else if (result.Equals("INVALID")) resultlatex = "\\vl";
                else resultlatex = "\\unk";
            } while (false);

            // Mapping record to variable
            do
            {
                int posType = -1;
                if (assertType.Equals(AssertType.DEADLOCK_FREE))
                    posType = 0;
                else if (assertType.Equals(AssertType.CONGESTION_CHANNEL))
                    posType = 1;
                else if (assertType.Equals(AssertType.CONGESTION_SENSOR))
                    posType = 2;

                if (posType < 0)
                    break;

                mParseRes[posType].mTime = assertion.getTimes();
                mParseRes[posType].mMemo = (float)assertion.getMems();
                mParseRes[posType].mTransition = assertion.getTransitions();
                mParseRes[posType].mState = assertion.getStates();
                mParseRes[posType].mClicked = true;
                mParseRes[posType].mRes = resultlatex;
            } while (false);
        }
    }
}
