using PAT.Common;
using PAT.Common.Classes.ModuleInterface;
using PAT.Common.GUI;
using PAT.Common.GUI.ModelChecker;
using PAT.Common.ModelCommon.PNCommon;
using PAT.Common.Utility;
using PAT.GUI.Docking;
using PAT.GUI.KWSNDrawing;
using PAT.GUI.PNDrawing;
using PAT.GUI.Properties;
using PAT.GUI.SVModule.Clustering;
using PAT.GUI.Utility;
using PAT.PN.Assertions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using PAT.Common.ModelCommon.WSNCommon;
using System.Media;
namespace PAT.GUI
{
    public interface IVerify
    {
        void onVerifyNewNetwork();

        double getTimeCluster();
    }
    public partial class VerifyAllClusters : Form, ISpecificationWorker //, ResourceFormInterface
    {
        private const string TAG = "VerifyAllClusters";
        private List<String> mlistAssert = new List<String>();
        private List<string> listPathDirectory = new List<string>();
        int test;
        bool mFlagVerifyDensity = false;
        bool mFlagVerifyImbalance = false;
        bool mFlagVerifyAll = true;
        bool mFlagClickStop = false;
        int mTimeOut = 0;
        int mTimeBroken = 0;
        int posDensity = -1;
        int posImbalance = -1;
        int pos = -1;
        int gridViewDensityLength = 0;
        int gridViewImbalanceLength = 0;
        int gridViewLength = 0;
        double percentProgress = 0;
        double mTotalTimeClustering = 0;
        String mType;
        String mthisCluster;
        Stopwatch mTimeVerify;
        TimeSpan mTotalTimeVerify;
        //double mTotalTimeCluster;
        //string mTimeClustering;
        int mCountLine = 0;
        protected AssertionBase Assertion;
        private PNExtendInfo mExtendInfo;
        private int VerificationIndex = -1;
        protected SpecificationBase Spec;
        private ModuleFacadeBase mModule;
        private ICluster mListener;
        private IVerify mVerifyListener;
        //private IVerify mTempVerifyListener;
        private SpecificationWorker mSpecWorker;
        // private LatexWorker mLatexWorker;
        //private ISpecificationWorker mISpec;
        private const string mystring = "(*)";
        private const string ValidString = "VALID";
        private const string NotValidString = "NOT valid";
        private const string TimeoutString = "TIME OUT";
        private const string BrokenString = "BROKEN";
        int numClusters = 0;
        private static Font fnt = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point);
        private static Font fntBold = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold, GraphicsUnit.Point);
        //StringBuilder sb = new StringBuilder();

        public VerifyAllClusters(List<String> listDensityClusters, List<String> listImbalanceClusters, List<String> listPath, IVerify listenerVerify, ICluster listenerCluster)
        {
            do
            {
                InitializeComponent();
                listPathDirectory.AddRange(listPath);
                mListener = listenerCluster;
                mVerifyListener = listenerVerify;
                //mTempVerifyListener = listenerVerify;
                mType = "";
                //mTotalTimeCluster = mVerifyListener.getTimeCluster();
                mTotalTimeClustering = mVerifyListener.getTimeCluster();
                OutputTxtBox.AppendText("Elapsed time clustering: " + mTotalTimeClustering + "s");
                OutputTxtBox.AppendText("\nHave: " + listDensityClusters.Count + " density clusters");
                OutputTxtBox.AppendText("\nHave: " + listImbalanceClusters.Count + " imbalance clusters");
                OutputTxtBox.AppendText("\nTotal have: " + (listDensityClusters.Count + listImbalanceClusters.Count) + " clusters");
                if ((listDensityClusters.Count == 0) && (listImbalanceClusters.Count == 0))
                {
                    Verify_Btn.Enabled = false;
                    VerifyAll_Btn.Enabled = false;
                    break;
                }

                //add items listview resultVerify
                for (int i = 0; i < listImbalanceClusters.Count; i++)
                {
                    ImbalanceGridView.Rows.Add(listImbalanceClusters[i], null, null);
                    //resultVerify.Items[i].SubItems.Add("null");
                    //resultGridView.Rows.Add(listClusters[i],"null");
                }
                for (int i = 0; i < listDensityClusters.Count; i++)
                {
                    resultVerifyGridView.Rows.Add(listDensityClusters[i], null, null);
                }
                //resultVerifyGridView.Rows.Add("full_After_cluster", null, null);
                //MessageBox.Show(""+resultVerifyGridView.Columns[0].HeaderText,"msg");
                //MessageBox.Show("" + resultVerifyGridView.Columns[1].HeaderText, "msg");
                //MessageBox.Show("" + resultVerifyGridView.Columns[2].HeaderText, "msg");
                //MessageBox.Show("iba: " + ImbalanceGridView.Columns[0].HeaderText, "msg");
                //MessageBox.Show("iba: " + ImbalanceGridView.Columns[1].HeaderText, "msg");
                //MessageBox.Show("iba: " + ImbalanceGridView.Columns[2].HeaderText, "msg");
                Verify_Btn.Enabled = true;
                VerifyAll_Btn.Enabled = true;
                numClusters = listDensityClusters.Count + listImbalanceClusters.Count;
            } while (false);
        }

        public VerifyAllClusters(List<String> listDensityClusters, List<String> listImbalanceClusters, List<String> listPath, ICluster listener, double timeClustering)
        {
            do
            {
                InitializeComponent();
                listPathDirectory.AddRange(listPath);
                //VerifyAll_Btn.Enabled = false;
                mListener = listener;
                //mVerifyListener = mTempVerifyListener;
                mType = "";
                mTotalTimeClustering = timeClustering;
                OutputTxtBox.AppendText("Elapsed time clustering: " + mTotalTimeClustering + "s");
                OutputTxtBox.AppendText("\nHave: " + listDensityClusters.Count + " density clusters");
                OutputTxtBox.AppendText("\nHave: " + listImbalanceClusters.Count + " imbalance clusters");
                OutputTxtBox.AppendText("\nTotal have: " + (listDensityClusters.Count + listImbalanceClusters.Count) + " clusters");
                if ((listDensityClusters.Count == 0) && (listImbalanceClusters.Count == 0))
                {
                    Verify_Btn.Enabled = false;
                    VerifyAll_Btn.Enabled = false;
                    break;
                }

                //add items listview resultVerify
                //add items listview resultVerify
                for (int i = 0; i < listImbalanceClusters.Count; i++)
                {
                    ImbalanceGridView.Rows.Add(listImbalanceClusters[i], null, null);
                    //resultVerify.Items[i].SubItems.Add("null");
                    //resultGridView.Rows.Add(listClusters[i],"null");
                }
                for (int i = 0; i < listDensityClusters.Count; i++)
                {
                    resultVerifyGridView.Rows.Add(listDensityClusters[i], null, null);
                }
                //resultVerifyGridView.Rows.Add("full_After_cluster", null, null);
                //MessageBox.Show(""+resultVerifyGridView.Columns[0].HeaderText,"msg");
                //MessageBox.Show("" + resultVerifyGridView.Columns[3].HeaderText, "msg");
                Verify_Btn.Enabled = true;
                VerifyAll_Btn.Enabled = true;

            } while (false);
        }

        private void Close_Btn_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("If you agree, you can't verification all clusters but you can use function verify one cluster. Are you sure?", "Warning Verify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    this.Close();
            //}
            this.Close();
        }


        private void VerifyAll_Btn_Click(object sender, EventArgs e)
        {
            mFlagVerifyAll = true;
            mlistAssert.Clear();
            if (VerifyAll_Btn.Text == "DROP")
            {
                enableAllControls();
                //numericTimeout.Enabled = true;
                //mSpecWorker.startVerification("");
                statusLabel.Text = "Verification Cancelled";
                mSpecWorker.cancelAssertion();
                mSpecWorker.unlockShareData();
                //GC.Collect();
                mFlagClickStop = false;
            }
            else
            {
                VerifyAll_Btn.Text = "DROP";
                Close_Btn.Enabled = false;
                Verify_Btn.Enabled = false;
                numericTimeout.Enabled = false;
                STOP_Btn.Enabled = true;
                pos = -1;
                gridViewLength = 0;
                gridViewDensityLength = 0;
                gridViewImbalanceLength = 0;
                gridViewDensityLength = resultVerifyGridView.Rows.Count;
                gridViewImbalanceLength = ImbalanceGridView.Rows.Count;
                if ((gridViewDensityLength != 0) && (gridViewImbalanceLength != 0))
                {
                    mFlagVerifyDensity = true;
                    mFlagVerifyImbalance = false;
                    OutputTxtBox.AppendText("\n\n*****************START VERIFICATION AUTO**************\n\n\n");
                    //disableAllControls();
                    posDensity = 0;
                    posImbalance = -1;
                    mTimeVerify = new Stopwatch();
                    mTimeVerify.Start();
                    //mTotalTimeVerify = mTimeVerify.Elapsed;
                    verifyAt(posDensity, mFlagVerifyAll);
                }
                else
                {
                    if (gridViewDensityLength == 0)
                    {
                        mFlagVerifyImbalance = true;
                        mFlagVerifyDensity = false;
                        posImbalance = 0;
                        posDensity = -1;
                        OutputTxtBox.AppendText("\n\n*****************START VERIFICATION AUTO**************\n\n\n");
                        mTimeVerify = new Stopwatch();
                        mTimeVerify.Start();
                        //mTotalTimeVerify = mTimeVerify.Elapsed;
                        verifyAt(posImbalance, mFlagVerifyAll);
                    }
                    else if (gridViewImbalanceLength == 0)
                    {
                        mFlagVerifyImbalance = false;
                        mFlagVerifyDensity = true;
                        posDensity = 0;
                        posImbalance = -1;
                        OutputTxtBox.AppendText("\n\n*****************START VERIFICATION AUTO**************\n\n\n");
                        mTimeVerify = new Stopwatch();
                        mTimeVerify.Start();
                        //mTotalTimeVerify = mTimeVerify.Elapsed;
                        verifyAt(posDensity, mFlagVerifyAll);
                    }
                }
                //verifyStatus.Text = (position + 1) + "/" + gridViewLength;

            }
        }

        private void verifyAt(int position, bool isVerifyAll)
        {
            string tmpName = "";
            DevLog.d(TAG, "position: " + position);
            if (isVerifyAll == true)
            {
                mListener.onUpdateProgressbar(percentProgress);
                percentProgress += Math.Round(100.00 / Convert.ToDouble(numClusters), 2);
                mListener.onUpdateProgressbar(percentProgress);
                mlistAssert.Clear();
                //disableAllControls();

                int N = mListener.onDockContainer().Documents.Length;
                DevLog.d(TAG, "Number Documents: " + N);

                if (mFlagVerifyDensity == true)
                {
                    if ((posDensity + 1 > gridViewDensityLength) && (posImbalance + 1 == gridViewImbalanceLength))
                    {
                        //tmpName += ClusterHelper.CURRENT_PATH;
                        resultVerifyGridView.Rows[position].Cells[0].Selected = true;
                        EditorTabItem currentNewNetwork = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        currentNewNetwork.Save(currentNewNetwork.FileName);
                        mthisCluster = resultVerifyGridView.SelectedCells[0].Value.ToString();
                        DevLog.d(TAG, "New Network is:" + currentNewNetwork.FileName);
                        int N2 = mListener.onDockContainer().Documents.Length;
                        DevLog.d(TAG, "Number Documents: " + N2);
                        mListener.onConvertToPNAfterCluster(false, true, resultVerifyGridView.SelectedCells[0].Value.ToString());//only channel
                        EditorTabItem currentPNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        currentNewNetwork.Save(currentNewNetwork.FileName);
                        DevLog.d(TAG, "WSNItem:" + currentNewNetwork.FileName);
                        currentNewNetwork.Close();//Close WSNTabItem convert to PNTabItem

                        currentPNItem.Save(currentPNItem.FileName);
                        DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                        currentPNItem.Close();//Close PNTabItem
                        //currentNewNetwork.Close();//Close WSNTabItem convert to PNTabItem
                        DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                        currentPNItem.Close();//Close PNTabItem
                        OutputTxtBox.AppendText("Verifying " + resultVerifyGridView.SelectedCells[0].Value.ToString() + "......\n");
                        OutputTxtBox.AppendText("Please wait....\n");
                        if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                        {
                            if (SettingUtils.AUTO_SAVE)
                                mListener.onSave();

                            PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                            PNExtendInfo extendInfo = null;
                            if (cPNTabItem != null)
                            {
                                Spec = mListener.onParseSpecificationAfterCluster(currentPNItem);
                                DevLog.d(TAG, "Spec: " + Spec);
                                extendInfo = cPNTabItem.mExtendInfo;
                            }

                            mSpecWorker = new SpecificationWorker(extendInfo, Spec, this, this);
                            initLogic();
                            mSpecWorker.startVerification(mlistAssert[0], numericTimeout.Value);
                        }
                    }
                    else
                    {
                        tmpName += ClusterHelper.CURRENT_PATH + ClusterHelper.BEFORE_FOLDER;
                        resultVerifyGridView.Rows[position].Cells[0].Selected = true;
                        tmpName += @"\" + resultVerifyGridView.SelectedCells[0].Value + ".kwsn";
                        DevLog.d(TAG, "" + tmpName);
                        mListener.onOpenFile(tmpName);
                        mthisCluster = resultVerifyGridView.SelectedCells[0].Value.ToString();
                        OutputTxtBox.AppendText("\n\n\n========" + resultVerifyGridView.SelectedCells[0].Value.ToString().ToUpper() + "=======\n");
                        OutputTxtBox.AppendText("Please wait............\n");
                        OutputTxtBox.AppendText("Opening " + resultVerifyGridView.SelectedCells[0].Value + "......\n");
                        OutputTxtBox.AppendText("Open " + resultVerifyGridView.SelectedCells[0].Value + ".kwsn" + " successful\n");
                        EditorTabItem currentWSNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        int N2 = mListener.onDockContainer().Documents.Length;
                        DevLog.d(TAG, "Number Documents: " + N2);

                        mListener.onConvertToPNAfterCluster(false, true, resultVerifyGridView.SelectedCells[0].Value.ToString());//only channel
                        EditorTabItem currentPNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        currentWSNItem.Save(currentWSNItem.FileName);
                        DevLog.d(TAG, "WSNItem:" + currentWSNItem.FileName);
                        currentWSNItem.Close();//Close WSNTabItem convert to PNTabItem

                        currentPNItem.Save(currentPNItem.FileName);
                        DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                        currentPNItem.Close();//Close PNTabItem
                        OutputTxtBox.AppendText("Verifying " + resultVerifyGridView.SelectedCells[0].Value + "......\n");
                        OutputTxtBox.AppendText("Please wait....\n");
                        if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                        {
                            if (SettingUtils.AUTO_SAVE)
                                mListener.onSave();

                            PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                            PNExtendInfo extendInfo = null;
                            if (cPNTabItem != null)
                            {
                                Spec = mListener.onParseSpecificationAfterCluster(currentPNItem);
                                DevLog.d(TAG, "Spec: " + Spec);
                                extendInfo = cPNTabItem.mExtendInfo;
                            }

                            mSpecWorker = new SpecificationWorker(extendInfo, Spec, this, this);
                            initLogic();
                            mSpecWorker.startVerification(mlistAssert[0], numericTimeout.Value);
                        }
                    }
                }
                else if (mFlagVerifyImbalance == true)
                {
                    if ((posDensity + 1 == gridViewDensityLength) && (posImbalance + 1 > gridViewImbalanceLength))
                    {
                        //tmpName += ClusterHelper.CURRENT_PATH;
                        ImbalanceGridView.Rows[position].Cells[0].Selected = true;
                        EditorTabItem currentNewNetwork = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        currentNewNetwork.Save(currentNewNetwork.FileName);
                        mthisCluster = ImbalanceGridView.SelectedCells[0].Value.ToString();
                        DevLog.d(TAG, "New Network is:" + currentNewNetwork.FileName);
                        int N2 = mListener.onDockContainer().Documents.Length;
                        DevLog.d(TAG, "Number Documents: " + N2);
                        mListener.onConvertToPNAfterCluster(false, true, ImbalanceGridView.SelectedCells[0].Value.ToString());//only channel
                        EditorTabItem currentPNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        currentNewNetwork.Save(currentNewNetwork.FileName);
                        DevLog.d(TAG, "WSNItem:" + currentNewNetwork.FileName);
                        currentNewNetwork.Close();//Close WSNTabItem convert to PNTabItem

                        currentPNItem.Save(currentPNItem.FileName);
                        DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                        currentPNItem.Close();//Close PNTabItem
                        //currentNewNetwork.Close();//Close WSNTabItem convert to PNTabItem
                        DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                        currentPNItem.Close();//Close PNTabItem
                        OutputTxtBox.AppendText("Verifying " + ImbalanceGridView.SelectedCells[0].Value.ToString() + "......\n");
                        OutputTxtBox.AppendText("Please wait....\n");
                        if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                        {
                            if (SettingUtils.AUTO_SAVE)
                                mListener.onSave();

                            PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                            PNExtendInfo extendInfo = null;
                            if (cPNTabItem != null)
                            {
                                Spec = mListener.onParseSpecificationAfterCluster(currentPNItem);
                                DevLog.d(TAG, "Spec: " + Spec);
                                extendInfo = cPNTabItem.mExtendInfo;
                            }

                            mSpecWorker = new SpecificationWorker(extendInfo, Spec, this, this);
                            initLogic();
                            mSpecWorker.startVerification(mlistAssert[0], numericTimeout.Value);
                        }
                    }
                    else
                    {
                        tmpName += ClusterHelper.CURRENT_PATH + ClusterHelper.BEFORE_FOLDER;
                        ImbalanceGridView.Rows[position].Cells[0].Selected = true;
                        tmpName += @"\" + ImbalanceGridView.SelectedCells[0].Value + ".kwsn";
                        DevLog.d(TAG, "" + tmpName);
                        mListener.onOpenFile(tmpName);
                        mthisCluster = ImbalanceGridView.SelectedCells[0].Value.ToString();
                        OutputTxtBox.AppendText("\n\n\n========" + ImbalanceGridView.SelectedCells[0].Value.ToString().ToUpper() + "=======\n");
                        OutputTxtBox.AppendText("Please wait............\n");
                        OutputTxtBox.AppendText("Opening " + ImbalanceGridView.SelectedCells[0].Value + "......\n");
                        OutputTxtBox.AppendText("Open " + ImbalanceGridView.SelectedCells[0].Value + ".kwsn" + " successful\n");
                        EditorTabItem currentWSNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        int N2 = mListener.onDockContainer().Documents.Length;
                        DevLog.d(TAG, "Number Documents: " + N2);

                        mListener.onConvertToPNAfterCluster(false, true, ImbalanceGridView.SelectedCells[0].Value.ToString());//only channel
                        EditorTabItem currentPNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                        currentWSNItem.Save(currentWSNItem.FileName);
                        DevLog.d(TAG, "WSNItem:" + currentWSNItem.FileName);
                        currentWSNItem.Close();//Close WSNTabItem convert to PNTabItem

                        currentPNItem.Save(currentPNItem.FileName);
                        DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                        currentPNItem.Close();//Close PNTabItem
                        OutputTxtBox.AppendText("Verifying " + ImbalanceGridView.SelectedCells[0].Value + "......\n");
                        OutputTxtBox.AppendText("Please wait....\n");
                        if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                        {
                            if (SettingUtils.AUTO_SAVE)
                                mListener.onSave();

                            PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                            PNExtendInfo extendInfo = null;
                            if (cPNTabItem != null)
                            {
                                Spec = mListener.onParseSpecificationAfterCluster(currentPNItem);
                                DevLog.d(TAG, "Spec: " + Spec);
                                extendInfo = cPNTabItem.mExtendInfo;
                            }

                            mSpecWorker = new SpecificationWorker(extendInfo, Spec, this, this);
                            initLogic();
                            mSpecWorker.startVerification(mlistAssert[0], numericTimeout.Value);
                        }
                    }
                }
                //FormMain frmMain = autoVerify as FormMain;


            }
            else
            {
                mlistAssert.Clear();
                tmpName += ClusterHelper.CURRENT_PATH + ClusterHelper.BEFORE_FOLDER;
                if (mFlagVerifyDensity == true)
                {
                    tmpName += @"\" + resultVerifyGridView.SelectedCells[0].Value + ".kwsn";
                    mthisCluster = resultVerifyGridView.SelectedCells[0].Value.ToString();
                    DevLog.d(TAG, "" + tmpName);
                    mListener.onOpenFile(tmpName);
                    OutputTxtBox.AppendText("\n\n\n========" + resultVerifyGridView.SelectedCells[0].Value.ToString().ToUpper() + "=======\n");
                    OutputTxtBox.AppendText("Please wait............\n");
                    OutputTxtBox.AppendText("Opening " + resultVerifyGridView.SelectedCells[0].Value + "......\n");
                    OutputTxtBox.AppendText("Open " + resultVerifyGridView.SelectedCells[0].Value + ".kwsn" + " successful\n");
                    EditorTabItem currentWSNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                    int N2 = mListener.onDockContainer().Documents.Length;
                    DevLog.d(TAG, "Number Documents: " + N2);

                    mListener.onConvertToPNAfterCluster(false, true, resultVerifyGridView.SelectedCells[0].Value.ToString());//only channel
                    EditorTabItem currentPNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                    currentWSNItem.Save(currentWSNItem.FileName);
                    DevLog.d(TAG, "WSNItem:" + currentWSNItem.FileName);
                    currentWSNItem.Close();//Close WSNTabItem convert to PNTabItem
                    //Log.d(TAG, "Number Documents After: " + N);
                    currentPNItem.Save(currentPNItem.FileName);
                    DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                    currentPNItem.Close();//Close PNTabItem
                    OutputTxtBox.AppendText("Verifying " + resultVerifyGridView.SelectedCells[0].Value + "......\n");
                    OutputTxtBox.AppendText("Please wait....\n");
                    if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                    {
                        if (SettingUtils.AUTO_SAVE)
                            mListener.onSave();


                        PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                        PNExtendInfo extendInfo = null;
                        if (cPNTabItem != null)
                        {
                            Spec = mListener.onParseSpecificationAfterCluster(currentPNItem);
                            DevLog.d(TAG, "Spec: " + Spec);
                            extendInfo = cPNTabItem.mExtendInfo;
                            //extendInfo.mMode = NetMode.UNICAST;
                            //DevLog.d(TAG, "mMode: " + extendInfo.mMode);
                        }

                        mSpecWorker = new SpecificationWorker(extendInfo, Spec, this, this);
                        initLogic();
                        //for (int z = 0; z < mlistAssert.Count; z++)
                        //{

                        mSpecWorker.startVerification(mlistAssert[0], numericTimeout.Value);
                        //}
                        //MessageBox.Show("Xong!!", "");
                        //mSpecWorker = new SpecificationWorker(Spec, mISpec, mListener.onShowModelCheckingWindow(currentPNItem.TabText.TrimEnd('*'), extendInfo));
                    }
                }
                else if (mFlagVerifyImbalance == true)
                {
                    tmpName += @"\" + ImbalanceGridView.SelectedCells[0].Value + ".kwsn";
                    mthisCluster = ImbalanceGridView.SelectedCells[0].Value.ToString();
                    DevLog.d(TAG, "" + tmpName);
                    mListener.onOpenFile(tmpName);
                    OutputTxtBox.AppendText("\n\n\n========" + ImbalanceGridView.SelectedCells[0].Value.ToString().ToUpper() + "=======\n");
                    OutputTxtBox.AppendText("Please wait............\n");
                    OutputTxtBox.AppendText("Opening " + ImbalanceGridView.SelectedCells[0].Value + "......\n");
                    OutputTxtBox.AppendText("Open " + ImbalanceGridView.SelectedCells[0].Value + ".kwsn" + " successful\n");
                    EditorTabItem currentWSNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                    int N2 = mListener.onDockContainer().Documents.Length;
                    DevLog.d(TAG, "Number Documents: " + N2);

                    mListener.onConvertToPNAfterCluster(false, true, ImbalanceGridView.SelectedCells[0].Value.ToString());//only channel
                    EditorTabItem currentPNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                    currentWSNItem.Save(currentWSNItem.FileName);
                    DevLog.d(TAG, "WSNItem:" + currentWSNItem.FileName);
                    currentWSNItem.Close();//Close WSNTabItem convert to PNTabItem
                    //Log.d(TAG, "Number Documents After: " + N);
                    currentPNItem.Save(currentPNItem.FileName);
                    DevLog.d(TAG, "PNItem:" + currentPNItem.FileName);
                    currentPNItem.Close();//Close PNTabItem
                    OutputTxtBox.AppendText("Verifying " + ImbalanceGridView.SelectedCells[0].Value + "......\n");
                    OutputTxtBox.AppendText("Please wait....\n");
                    if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                    {
                        if (SettingUtils.AUTO_SAVE)
                            mListener.onSave();


                        PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                        PNExtendInfo extendInfo = null;
                        if (cPNTabItem != null)
                        {
                            Spec = mListener.onParseSpecificationAfterCluster(currentPNItem);
                            DevLog.d(TAG, "Spec: " + Spec);
                            extendInfo = cPNTabItem.mExtendInfo;
                            //extendInfo.mMode = NetMode.UNICAST;
                            //DevLog.d(TAG, "mMode: " + extendInfo.mMode);
                        }

                        mSpecWorker = new SpecificationWorker(extendInfo, Spec, this, this);
                        initLogic();
                        //for (int z = 0; z < mlistAssert.Count; z++)
                        //{

                        mSpecWorker.startVerification(mlistAssert[0], numericTimeout.Value);
                        //}
                        //MessageBox.Show("Xong!!", "");
                        //mSpecWorker = new SpecificationWorker(Spec, mISpec, mListener.onShowModelCheckingWindow(currentPNItem.TabText.TrimEnd('*'), extendInfo));
                    }
                }
            }
        }


        protected void initLogic()
        {
            foreach (KeyValuePair<String, AssertionBase> entry in mSpecWorker.mSpec.AssertionDatabase)
            {
                mlistAssert.Add(entry.Key);
                //DevLog.d(TAG, "value: " + entry.Value);
                //DevLog.d(TAG, "key: " + entry.Key);
            }
            //this.StatusLabel_Text.Text = Resources.Select_an_assertion_to_start_with;
        }

        private void Verify_Btn_Click(object sender, EventArgs e)
        {
            //---Minh---Edit all
            do
            {
                mFlagVerifyAll = false;
                MessageBox.Show("Density flag: " + mFlagVerifyDensity, "msg");
                MessageBox.Show("Imbalancae flag: " + mFlagVerifyImbalance, "msg");
                if (mFlagVerifyDensity == mFlagVerifyImbalance)
                {
                    MessageBox.Show("Please choose one cluster", "Choose Cluster Error");
                    break;
                }
                if (Verify_Btn.Text == "STOP")
                {
                    Verify_Btn.Text = "Verify";
                    Close_Btn.Enabled = true;
                    VerifyAll_Btn.Enabled = true;
                    resultVerifyGridView.Enabled = true;
                    numericTimeout.Enabled = true;
                    enableAllControls();
                    //numericTimeout.Enabled = true;
                    //mSpecWorker.startVerification("");
                    statusLabel.Text = "Verification Completed";
                    mSpecWorker.cancelAssertion();
                    mSpecWorker.unlockShareData();
                    //GC.Collect();
                }
                else
                {
                    //bool isOpened = false;//flag check a Document has been opened
                    Verify_Btn.Text = "STOP";
                    Close_Btn.Enabled = false;
                    VerifyAll_Btn.Enabled = false;
                    resultVerifyGridView.Enabled = false;
                    numericTimeout.Enabled = false;
                    pos = -1;
                    gridViewLength = 0;
                    //gridViewLength = resultVerifyGridView.Rows.Count;
                    if (mFlagVerifyDensity == true)
                    {
                        pos = posDensity;
                        gridViewLength = resultVerifyGridView.Rows.Count;
                    }
                    else if (mFlagVerifyImbalance == true)
                    {
                        pos = posImbalance;
                        gridViewLength = ImbalanceGridView.Rows.Count;
                    }
                    mTimeVerify = new Stopwatch();
                    mTimeVerify.Start();
                    #region //cmt
                    //    int N = mListener.onDockContainer().Documents.Length;
                    //    DevLog.d(TAG, "Number Documents: " + N);
                    //    string tmpName = "";
                    //    //FormMain frmMain = autoVerify as FormMain;
                    //    tmpName += ClusterHelper.CURRENT_PATH + ClusterHelper.BEFORE_FOLDER;
                    //    tmpName += @"\" + resultVerifyGridView.SelectedCells[0].Value + ".kwsn";
                    //    DevLog.d(TAG, "" + tmpName);

                    //    for (int i = 0; i < N; i++)
                    //    {
                    //        EditorTabItem item = mListener.onDockContainer().Documents[i] as EditorTabItem;
                    //        if (item.FileName == tmpName)
                    //        {
                    //            isOpened = true;//this Document has been opened
                    //            break;
                    //            //Log.d(TAG, "File is opened: " + item.FileName);
                    //        }
                    //    }
                    //    if (isOpened == true)
                    //    {
                    //        //OutputTxtBox.AppendText("Please wait............\n");
                    //        OutputTxtBox.AppendText(resultVerifyGridView.SelectedCells[0].Value + ".kwsn" + " has been opened......\n");
                    //        int N2 = mListener.onDockContainer().Documents.Length;
                    //        DevLog.d(TAG, "Number Documents: " + N2);

                    //        //convert to PN underground
                    //        int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                    //        //resultVerifyGridView.Rows[rowSelected].Cells[0].Value="ma";
                    //        //resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "tao";
                    //        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed";

                    //        DevLog.d(TAG, "row: " + rowSelected);
                    //        //Verify_Btn.Text = "STOP";

                    //        mListener.onConvertToPNAfterCluster(false, true, resultVerifyGridView.SelectedCells[0].Value.ToString());//only channel
                    //        EditorTabItem currentPNItem = mListener.onDockContainer().Documents[N2] as EditorTabItem;
                    //        mListener.onSave();
                    //        //currentWSNItem.Close();//Close WSNTabItem convert to PNTabItem
                    //        //Log.d(TAG, "Number Documents After: " + N);
                    //        currentPNItem.Close();//Close PNTabItem
                    //        //OutputTxtBox.AppendText("Done....\n");
                    //        //for(int m=0;m<N;m++)
                    //        //{
                    //        //    Log.d(TAG, "Document["+(m+1)+"]:" + frmMain.DockContainer.Documents[m]);
                    //        //}
                    //        OutputTxtBox.AppendText("Verifying " + resultVerifyGridView.SelectedCells[0].Value + "......\n");
                    //        OutputTxtBox.AppendText("Please wait....\n");
                    //        try
                    //        {
                    //            if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                    //            {
                    //                if (SettingUtils.AUTO_SAVE)
                    //                    mListener.onSave();


                    //                PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                    //                PNExtendInfo extendInfo = null;
                    //                if (cPNTabItem != null)
                    //                    extendInfo = cPNTabItem.mExtendInfo;
                    //                extendInfo.mMode = NetMode.UNICAST;
                    //                mListener.onShowModelCheckingWindow(currentPNItem.TabText.TrimEnd('*'), extendInfo);
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            MessageBox.Show("Can't open model checking form", "Error");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        mListener.onOpenFile(tmpName);
                    //        OutputTxtBox.AppendText("Please wait............\n");
                    //        OutputTxtBox.AppendText("Opening " + resultVerifyGridView.SelectedCells[0].Value + "......\n");
                    //        OutputTxtBox.AppendText("Open " + resultVerifyGridView.SelectedCells[0].Value + ".kwsn" + " successful\n");
                    //        //Log.d(TAG, "CurrentActiveTab: " + curretActiveTab);
                    //        EditorTabItem currentWSNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                    //        //Log.d(TAG, "CurrentEditorTabItem: " + frmMain.CurrentEditorTabItem.FileName);
                    //        int N2 = mListener.onDockContainer().Documents.Length;
                    //        DevLog.d(TAG, "Number Documents: " + N2);

                    //        //convert to PN underground
                    //        int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                    //        //resultVerifyGridView.Rows[rowSelected].Cells[0].Value="ma";
                    //        //resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "tao";
                    //        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed";

                    //        DevLog.d(TAG, "row: " + rowSelected);

                    //        //Close_Btn.Enabled = false;
                    //        //VerifyAll_Btn.Enabled = false;
                    //        //resultVerifyGridView.Enabled = false;
                    //        mListener.onConvertToPNAfterCluster(false, true, resultVerifyGridView.SelectedCells[0].Value.ToString());//only channel
                    //        EditorTabItem currentPNItem = mListener.onDockContainer().Documents[mListener.onDockContainer().Documents.Length - 1] as EditorTabItem;
                    //        mListener.onSave();
                    //        currentWSNItem.Close();//Close WSNTabItem convert to PNTabItem
                    //        //Log.d(TAG, "Number Documents After: " + N);
                    //        currentPNItem.Close();//Close PNTabItem
                    //        //OutputTxtBox.AppendText("Done....\n");
                    //        //for(int m=0;m<N;m++)
                    //        //{
                    //        //    Log.d(TAG, "Document["+(m+1)+"]:" + frmMain.DockContainer.Documents[m]);
                    //        //}
                    //        OutputTxtBox.AppendText("Verifying " + resultVerifyGridView.SelectedCells[0].Value + "......\n");
                    //        OutputTxtBox.AppendText("Please wait....\n");
                    //        //OutputTxtBox.AppendText("" + mISpec.onResult(0));

                    //        try
                    //        {
                    //            if (mListener.onParseSpecificationAfterCluster(currentPNItem) != null)
                    //            {
                    //                if (SettingUtils.AUTO_SAVE)
                    //                    mListener.onSave();


                    //                PNTabItem cPNTabItem = currentPNItem as PNTabItem;
                    //                PNExtendInfo extendInfo = null;
                    //                if (cPNTabItem != null)
                    //                    extendInfo = cPNTabItem.mExtendInfo;
                    //                extendInfo.mMode = NetMode.UNICAST;
                    //                DevLog.d(TAG, "mMode: " + extendInfo.mMode);
                    //                mListener.onShowModelCheckingWindow(currentPNItem.TabText.TrimEnd('*'), extendInfo);

                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            //GUIUtility.LogException(ex, null);
                    //            MessageBox.Show("Can't open model checking form", "Error");
                    //        }
                    //    }

                    //}
                    ////Log.d(TAG, "");
                    //OutputTxtBox.AppendText("Done....\n");
                    //OutputTxtBox.AppendText("======================\n");
                    //} 
                    #endregion
                    //pos = resultVerifyGridView.SelectedCells[0].RowIndex;
                    //MessageBox.Show("" + pos, "msg");
                    verifyAt(pos, mFlagVerifyAll);
                }
            } while (false);
        }

        private void STOP_Btn_Click(object sender, EventArgs e)
        {
            mFlagClickStop = true;
            if ((STOP_Btn.Enabled == true) && (mFlagClickStop == true))
            {
                enableAllControls();
                //numericTimeout.Enabled = true;
                //mSpecWorker.startVerification("");
                statusLabel.Text = "Verification Cancelled";
                mSpecWorker.cancelAssertion();
                mSpecWorker.unlockShareData();
                //STOP_Btn.Enabled = false;
                //GC.Collect();
            }

        }

        private bool createModule()
        {
            bool ret = false;
            do
            {
                string moduleName = "KWSN Model";
                if (mModule != null && mModule.ModuleName == moduleName)
                    break;

                string facadeClass = "PAT." + moduleName + ".ModuleFacade";
                string file = Path.Combine(Path.Combine(Utilities.ModuleFolderPath, moduleName), "PAT.Module." + moduleName + ".dll");

                Assembly assembly = Assembly.LoadFrom(file);
                mModule = (ModuleFacadeBase)assembly.CreateInstance(
                                                       facadeClass,
                                                       true,
                                                       BindingFlags.CreateInstance,
                                                       null, null,
                                                       null, null);

                if (mModule.GetType().Namespace != "PAT." + moduleName)
                {
                    mModule = null;
                    break;
                }

                // mModule.ShowModel += new ShowModelHandler(ShowModel);
                // mModule.ExampleMenualToolbarInitialize(this.MenuButton_Examples);
                mModule.ReadConfiguration();
                ret = true;
            } while (false);

            return ret;
        }

        private void convertToPN(string kwsnFileName)
        {
            do
            {
                WSNTabItem wsnTabItem = new WSNTabItem("KWSN Model", "KWSN", null);
                wsnTabItem.Open(kwsnFileName);

                string pnName = DateTime.Now.Millisecond.ToString() + ".pn"; // 13486468456456.pn
                PNGenerationHelper helper = new PNGenerationHelper(pnName, wsnTabItem);
                helper.GenerateXML(false, false);

                bool cne = createModule();
                if (cne == false)
                    break;



            } while (false);

        }

        public void finished()
        {

            // throw new NotImplementedException();
        }

        public bool moduleSpecificCheckPassed()
        {
            return true;
        }

        public void disableAllControls()
        {
            //resultVerifyGridView.Enabled = false;
            OutputTxtBox.Enabled = false;
            //STOP_Btn.Enabled = true;
        }

        public void enableAllControls()
        {
            //resultVerifyGridView.Enabled = true;
            OutputTxtBox.Enabled = true;
        }

        public void updateResStartVerify()
        {
        }

        public void updateResFinishedVerify()
        {
        }

        public void onResult(VerificationResultType type)
        {
            if (mFlagVerifyAll == true)
            {
                if (mFlagVerifyDensity == true)
                {
                    int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                    resultVerifyGridView.FirstDisplayedScrollingRowIndex = resultVerifyGridView.Rows[posDensity].Index;
                    if (type == VerificationResultType.UNKNOWN)
                    {
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posDensity + 1) + " of " + gridViewDensityLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.VALID)
                    {
                        resultVerifyGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Green;
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "VALID";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posDensity + 1) + " of " + gridViewDensityLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.WITHPROBABILITY)
                    {
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "TIME OUT";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posDensity + 1) + " of " + gridViewDensityLength;
                        mType = type.ToString();
                    }
                    else
                    {
                        DevLog.d(TAG, "Line: " + mCountLine);
                        resultVerifyGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Red;
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "NOT VALID";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posDensity + 1) + " of " + gridViewDensityLength;
                        mType = type.ToString();
                    }
                }
                else if (mFlagVerifyImbalance == true)
                {
                    int rowSelected = ImbalanceGridView.SelectedCells[0].RowIndex;
                    ImbalanceGridView.FirstDisplayedScrollingRowIndex = ImbalanceGridView.Rows[posImbalance].Index;
                    if (type == VerificationResultType.UNKNOWN)
                    {
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posImbalance + 1) + " of " + gridViewImbalanceLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.VALID)
                    {
                        ImbalanceGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Green;
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "VALID";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posImbalance + 1) + " of " + gridViewImbalanceLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.WITHPROBABILITY)
                    {
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "TIME OUT";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posImbalance + 1) + " of " + gridViewImbalanceLength;
                        mType = type.ToString();
                    }
                    else
                    {
                        ImbalanceGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Red;
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "NOT VALID";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (posImbalance + 1) + " of " + gridViewImbalanceLength;
                        mType = type.ToString();
                    }
                }
            }
            else
            {
                if (mFlagVerifyDensity == true)
                {
                    int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                    resultVerifyGridView.FirstDisplayedScrollingRowIndex = resultVerifyGridView.Rows[pos].Index;
                    if (type == VerificationResultType.UNKNOWN)
                    {
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.VALID)
                    {
                        resultVerifyGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Green;
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "VALID";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.WITHPROBABILITY)
                    {
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "TIME OUT";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                    else
                    {
                        DevLog.d(TAG, "Line: " + mCountLine);
                        resultVerifyGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Red;
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "NOT VALID";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                }
                else if (mFlagVerifyImbalance == true)
                {
                    int rowSelected = ImbalanceGridView.SelectedCells[0].RowIndex;
                    ImbalanceGridView.FirstDisplayedScrollingRowIndex = ImbalanceGridView.Rows[pos].Index;
                    if (type == VerificationResultType.UNKNOWN)
                    {
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.VALID)
                    {
                        ImbalanceGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Green;
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "VALID";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                    else if (type == VerificationResultType.WITHPROBABILITY)
                    {
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "TIME OUT";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                    else
                    {
                        DevLog.d(TAG, "Line: " + mCountLine);
                        ImbalanceGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Red;
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "NOT VALID";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Completed " + (pos + 1) + " of " + gridViewLength;
                        mType = type.ToString();
                    }
                }
            }

        }

        public void updateVerifyBtnLabel(string label)
        {
            //VerifyAll_Btn.Text = label;
        }

        public void performVerifyBtn()
        {
            if (mFlagVerifyAll == true)
            {
                VerifyAll_Btn.PerformClick();
            }
            else
            {
                Verify_Btn.PerformClick();
            }
        }

        public void updateStatusLabel(string status)
        {
            statusLabel.Text = mthisCluster + ": " + status;
            //mCountLine = OutputTxtBox.Lines.Length;
            //DevLog.d(TAG, "Line first status Label: " + mCountLine);
        }

        public int getCmbAdmissibleIndex()
        {
            int index = 0;
            return index == -1 ? 0 : index;
        }

        public int getCmbVerificationEngineIndex()
        {
            int index = 0;
            return index == -1 ? 0 : index;
        }

        public bool generateCounterExample()
        {
            return true;
        }

        public void closeForm()
        {
            //Close();
        }

        public void onAction(string action)
        {
            OutputTxtBox.Text = OutputTxtBox.Text + "\n" + action + "\n";
        }

        public void RnderingOutputTextbox()
        {
            OutputTxtBox.SelectAll();
            OutputTxtBox.SelectionFont = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point);
            OutputTxtBox.SelectionColor = System.Drawing.Color.Black;

            OutputTxtBox.SelectionStart = OutputTxtBox.Text.Length;
            OutputTxtBox.SelectionLength = OutputTxtBox.Text.Length;

            ColorText(0);

            OutputTxtBox.Select(OutputTxtBox.SelectionStart, OutputTxtBox.SelectionLength);
            OutputTxtBox.ScrollToCaret();
        }

        public bool isVerifyBtnEnabled()
        {
            return VerifyAll_Btn.Enabled;
        }

        public void eventResult()
        {
            mlistAssert.Clear();
            if (mFlagVerifyAll == true)
            {
                //GC.Collect();
                //System.Threading.Thread.Sleep(5000);
                if (mFlagVerifyDensity == true)
                {
                    mFlagVerifyDensity = false;
                    int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                    resultVerifyGridView.FirstDisplayedScrollingRowIndex = resultVerifyGridView.Rows[posDensity].Index;
                    if (mType == VerificationResultType.VALID.ToString())
                    {
                        OutputTxtBox.AppendText(resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is VALID");

                        if (posImbalance + 1 < gridViewImbalanceLength)
                        {
                            mFlagVerifyImbalance = true;
                            posImbalance++;
                            mSpecWorker.unlockShareData();
                            Spec.UnLockSharedData();
                            Spec.UnLockSpecificationData();
                            DevLog.d(TAG, "posImbalance: " + posImbalance);
                            //Application.DoEvents();
                            verifyAt(posImbalance, mFlagVerifyAll);
                        }
                        else
                        {
                            if ((posDensity + 1 == gridViewDensityLength) && (posImbalance + 1 == gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is not congestion at clusters");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                //OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                mVerifyListener.onVerifyNewNetwork();
                                resultVerifyGridView.Rows.Add("full_After_cluster", null, null);
                                posDensity = resultVerifyGridView.Rows.Count-1;
                                //Application.DoEvents();
                                mTimeVerify.Start();
                                mFlagVerifyDensity = true;
                                verifyAt(posDensity, mFlagVerifyAll);
                            }
                            else if ((posDensity + 1 > gridViewDensityLength) && (posImbalance + 1 == gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is not congestion");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                VerifyAll_Btn.Text = "Verify All";
                                Close_Btn.Enabled = true;
                                Verify_Btn.Enabled = true;
                                numericTimeout.Enabled = true;
                                resultVerifyGridView.Enabled = true;
                                STOP_Btn.Enabled = false;
                                mlistAssert.Clear();
                            }
                            else if (posImbalance + 1 == gridViewImbalanceLength)
                            {
                                mFlagVerifyDensity = true;
                                posDensity++;
                                mSpecWorker.unlockShareData();
                                Spec.UnLockSharedData();
                                Spec.UnLockSpecificationData();
                                //Application.DoEvents();
                                verifyAt(posDensity, mFlagVerifyAll);
                            }
                        }
                    }
                    else if (mType == VerificationResultType.INVALID.ToString())
                    {
                        if (posDensity + 1 > gridViewDensityLength)
                        {
                            //System.Threading.Thread.Sleep(5000);
                            //end = OutputTxtBox.GetLineFromCharIndex(OutputTxtBox.TextLength);
                            //count_row = end - start;
                            //DevLog.d(TAG, "count==: " + count_row);
                            OutputTxtBox.AppendText("New network is NOT valid");
                            VerifyAll_Btn.Text = "Verify All";
                            Close_Btn.Enabled = true;
                            Verify_Btn.Enabled = true;
                            numericTimeout.Enabled = true;
                            resultVerifyGridView.Enabled = true;
                            STOP_Btn.Enabled = false;
                            mTimeVerify.Stop();
                            mTotalTimeVerify = mTimeVerify.Elapsed;
                            //int count_NOT_VALID=0;
                            OutputTxtBox.AppendText("\n\n>>>>>>>Network is congestion\n");
                            //OutputTxtBox.AppendText(resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper());
                            OutputTxtBox.AppendText("\n\n================END VERIFICATION AUTO==============\n");
                            OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                            OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                            mSpecWorker.unlockShareData();
                            //enableAllControls();
                            RnderingOutputTextbox();
                            exportMSWord();
                        }
                        else
                        {
                            //System.Threading.Thread.Sleep(5000);
                            //DevLog.d(TAG, "mCountLine<: " + mCountLine);
                            OutputTxtBox.AppendText(resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is NOT valid");
                            VerifyAll_Btn.Text = "Verify All";
                            Close_Btn.Enabled = true;
                            Verify_Btn.Enabled = true;
                            numericTimeout.Enabled = true;
                            resultVerifyGridView.Enabled = true;
                            STOP_Btn.Enabled = false;
                            mTimeVerify.Stop();
                            mTotalTimeVerify = mTimeVerify.Elapsed;
                            //int count_NOT_VALID=0;
                            OutputTxtBox.AppendText("\n\n>>>>>>>Network is congestion at cluster: \n");
                            OutputTxtBox.AppendText(resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper());
                            OutputTxtBox.AppendText("\n\n================END VERIFICATION AUTO==============\n");
                            OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                            OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                            mSpecWorker.unlockShareData();
                            //enableAllControls();
                            RnderingOutputTextbox();
                            exportMSWord();
                        }
                    }
                    else if (mType == VerificationResultType.UNKNOWN.ToString())
                    {
                    }
                    else if (mType == VerificationResultType.WITHPROBABILITY.ToString())
                    {
                    }
                }
                else if (mFlagVerifyImbalance == true)
                {
                    mFlagVerifyImbalance = false;
                    int rowSelected = ImbalanceGridView.SelectedCells[0].RowIndex;
                    ImbalanceGridView.FirstDisplayedScrollingRowIndex = ImbalanceGridView.Rows[posImbalance].Index;
                    if (mType == VerificationResultType.VALID.ToString())
                    {
                        OutputTxtBox.AppendText(ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is VALID");
                        if (posDensity + 1 < gridViewDensityLength)
                        {
                            mFlagVerifyDensity = true;
                            posDensity++;
                            mSpecWorker.unlockShareData();
                            Spec.UnLockSharedData();
                            Spec.UnLockSpecificationData();
                            DevLog.d(TAG, "posDensity: " + posDensity);
                            //Application.DoEvents();
                            verifyAt(posDensity, mFlagVerifyAll);
                        }
                        else
                        {
                            if ((posDensity + 1 == gridViewDensityLength) && (posImbalance + 1 == gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is not congestion at clusters");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                //OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                mVerifyListener.onVerifyNewNetwork();
                                ImbalanceGridView.Rows.Add("full_After_cluster", null, null);
                                posImbalance = ImbalanceGridView.Rows.Count-1;
                                Application.DoEvents();
                                mTimeVerify.Start();
                                mFlagVerifyImbalance = true;
                                verifyAt(posImbalance, mFlagVerifyAll);
                            }
                            else if ((posDensity + 1 == gridViewDensityLength) && (posImbalance + 1 > gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is not congestion");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                VerifyAll_Btn.Text = "Verify All";
                                Close_Btn.Enabled = true;
                                Verify_Btn.Enabled = true;
                                numericTimeout.Enabled = true;
                                resultVerifyGridView.Enabled = true;
                                STOP_Btn.Enabled = false;
                                mlistAssert.Clear();
                            }
                            else if (posDensity + 1 == gridViewDensityLength)
                            {
                                mFlagVerifyImbalance = true;
                                posImbalance++;
                                mSpecWorker.unlockShareData();
                                Spec.UnLockSharedData();
                                Spec.UnLockSpecificationData();
                                //Application.DoEvents();
                                verifyAt(posImbalance, mFlagVerifyAll);
                            }
                        }
                    }
                    else if (mType == VerificationResultType.INVALID.ToString())
                    {
                        if (posImbalance + 1 > gridViewImbalanceLength)
                        {
                            //System.Threading.Thread.Sleep(5000);
                            //end = OutputTxtBox.GetLineFromCharIndex(OutputTxtBox.TextLength);
                            //count_row = end - start;
                            //DevLog.d(TAG, "count==: " + count_row);
                            OutputTxtBox.AppendText("New network is NOT valid");
                            VerifyAll_Btn.Text = "Verify All";
                            Close_Btn.Enabled = true;
                            Verify_Btn.Enabled = true;
                            numericTimeout.Enabled = true;
                            resultVerifyGridView.Enabled = true;
                            STOP_Btn.Enabled = false;
                            mTimeVerify.Stop();
                            mTotalTimeVerify = mTimeVerify.Elapsed;
                            //int count_NOT_VALID=0;
                            OutputTxtBox.AppendText("\n\n>>>>>>>Network is congestion\n");
                            //OutputTxtBox.AppendText(resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper());
                            OutputTxtBox.AppendText("\n\n================END VERIFICATION AUTO==============\n");
                            OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                            OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                            mSpecWorker.unlockShareData();
                            //enableAllControls();
                            RnderingOutputTextbox();
                            exportMSWord();
                        }
                        else
                        {
                            //System.Threading.Thread.Sleep(5000);
                            //DevLog.d(TAG, "mCountLine<: " + mCountLine);
                            OutputTxtBox.AppendText(ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is NOT valid");
                            VerifyAll_Btn.Text = "Verify All";
                            Close_Btn.Enabled = true;
                            Verify_Btn.Enabled = true;
                            numericTimeout.Enabled = true;
                            resultVerifyGridView.Enabled = true;
                            STOP_Btn.Enabled = false;
                            mTimeVerify.Stop();
                            mTotalTimeVerify = mTimeVerify.Elapsed;
                            //int count_NOT_VALID=0;
                            OutputTxtBox.AppendText("\n\n>>>>>>>Network is congestion at cluster: \n");
                            OutputTxtBox.AppendText(ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper());
                            OutputTxtBox.AppendText("\n\n================END VERIFICATION AUTO==============\n");
                            OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                            OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                            mSpecWorker.unlockShareData();
                            //enableAllControls();
                            RnderingOutputTextbox();
                            exportMSWord();
                        }
                    }
                    else if (mType == VerificationResultType.UNKNOWN.ToString())
                    {
                    }
                    else if (mType == VerificationResultType.WITHPROBABILITY.ToString())
                    {
                    }
                }
            }
            else//Verify one cluster
            {
                if (mFlagVerifyDensity == true)
                {
                    int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                    //resultVerifyGridView.FirstDisplayedScrollingRowIndex = resultVerifyGridView.Rows[pos].Index;
                    if (mType == VerificationResultType.VALID.ToString())
                    {
                        Verify_Btn.Text = "Verify";
                        Close_Btn.Enabled = true;
                        VerifyAll_Btn.Enabled = true;
                        numericTimeout.Enabled = true;
                        resultVerifyGridView.Enabled = true;
                        STOP_Btn.Enabled = false;
                        //mlistAssert.Clear();
                        mTimeVerify.Stop();
                        mTotalTimeVerify = mTimeVerify.Elapsed;
                        Application.DoEvents();
                        OutputTxtBox.AppendText("\n>>>>>>>" + resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is VALID");
                        OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                        OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                        OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                        mSpecWorker.unlockShareData();
                        RnderingOutputTextbox();
                        exportMSWord();
                    }
                    else if (mType == VerificationResultType.INVALID.ToString())
                    {
                        OutputTxtBox.AppendText(resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is NOT valid");
                        Verify_Btn.Text = "Verify";
                        Close_Btn.Enabled = true;
                        VerifyAll_Btn.Enabled = true;
                        numericTimeout.Enabled = true;
                        resultVerifyGridView.Enabled = true;
                        STOP_Btn.Enabled = false;
                        mTimeVerify.Stop();
                        mTotalTimeVerify = mTimeVerify.Elapsed;
                        //int count_NOT_VALID=0;
                        OutputTxtBox.AppendText("\n\n>>>>>>>Network is congestion at cluster: \n");
                        OutputTxtBox.AppendText(resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper());
                        OutputTxtBox.AppendText("\n\n================END VERIFICATION AUTO==============\n");
                        OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                        OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                        mSpecWorker.unlockShareData();
                        //enableAllControls();
                        RnderingOutputTextbox();
                        exportMSWord();
                    }
                }
                else if (mFlagVerifyImbalance == true)
                {
                    int rowSelected = ImbalanceGridView.SelectedCells[0].RowIndex;
                    //resultVerifyGridView.FirstDisplayedScrollingRowIndex = resultVerifyGridView.Rows[pos].Index;
                    if (mType == VerificationResultType.VALID.ToString())
                    {
                        Verify_Btn.Text = "Verify";
                        Close_Btn.Enabled = true;
                        VerifyAll_Btn.Enabled = true;
                        numericTimeout.Enabled = true;
                        resultVerifyGridView.Enabled = true;
                        STOP_Btn.Enabled = false;
                        //mlistAssert.Clear();
                        mTimeVerify.Stop();
                        mTotalTimeVerify = mTimeVerify.Elapsed;
                        Application.DoEvents();
                        OutputTxtBox.AppendText("\n>>>>>>>" + ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is VALID");
                        OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                        OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                        OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                        mSpecWorker.unlockShareData();
                        RnderingOutputTextbox();
                        exportMSWord();
                    }
                    else if (mType == VerificationResultType.INVALID.ToString())
                    {
                        OutputTxtBox.AppendText(ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is NOT valid");
                        Verify_Btn.Text = "Verify";
                        Close_Btn.Enabled = true;
                        VerifyAll_Btn.Enabled = true;
                        numericTimeout.Enabled = true;
                        resultVerifyGridView.Enabled = true;
                        STOP_Btn.Enabled = false;
                        mTimeVerify.Stop();
                        mTotalTimeVerify = mTimeVerify.Elapsed;
                        //int count_NOT_VALID=0;
                        OutputTxtBox.AppendText("\n\n>>>>>>>Network is congestion at cluster: \n");
                        OutputTxtBox.AppendText(ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper());
                        OutputTxtBox.AppendText("\n\n================END VERIFICATION AUTO==============\n");
                        OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                        OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                        mSpecWorker.unlockShareData();
                        //enableAllControls();
                        RnderingOutputTextbox();
                        exportMSWord();
                    }
                }
            }
        }

        public void eventCancel()
        {
            if (mFlagVerifyAll == true)
            {
                if (mFlagClickStop == true)
                {
                    if (mFlagVerifyDensity == true)
                    {
                        int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                        resultVerifyGridView.FirstDisplayedScrollingRowIndex = resultVerifyGridView.Rows[posDensity].Index;
                        resultVerifyGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Gray;
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Cancelled";

                    }
                    else if (mFlagVerifyImbalance == true)
                    {
                        int rowSelected = ImbalanceGridView.SelectedCells[0].RowIndex;
                        ImbalanceGridView.FirstDisplayedScrollingRowIndex = ImbalanceGridView.Rows[posImbalance].Index;
                        ImbalanceGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Gray;
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Cancelled";
                    }
                    VerifyAll_Btn.Text = "Verify All";
                    Close_Btn.Enabled = true;
                    Verify_Btn.Enabled = true;
                    numericTimeout.Enabled = true;
                    resultVerifyGridView.Enabled = true;
                    STOP_Btn.Enabled = false;
                    mTimeVerify.Stop();
                    mTotalTimeVerify = mTimeVerify.Elapsed;
                    Application.DoEvents();
                    OutputTxtBox.AppendText("\n" + mthisCluster.ToUpper() + " is cancelled");
                    OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                    OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                    OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                    mSpecWorker.unlockShareData();
                    statusLabel.Text = "Verification Cancelled";
                    RnderingOutputTextbox();
                    exportMSWord();
                }
                else//Drop
                {
                    //mTotalTimeVerify = DateTime.Now.TimeOfDay;
                    //OutputTxtBox.AppendText("\nTIME OUT or BROKEN at: " + mTotalTimeVerify.Hours + ":" + mTotalTimeVerify.Minutes + ":" + mTotalTimeVerify.Seconds);
                    if (mFlagVerifyDensity == true)
                    {
                        int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                        resultVerifyGridView.FirstDisplayedScrollingRowIndex = resultVerifyGridView.Rows[posDensity].Index;
                        resultVerifyGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Gray;
                        resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Cancelled";
                        if (mTimeBroken != 0)
                        {
                            OutputTxtBox.AppendText("\n" + resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is " + "BROKEN....");
                        }
                        else
                        {
                            OutputTxtBox.AppendText("\n" + resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is " + "TIME OUT....");
                        }
                        //mSpecWorker.unlockShareData();
                        //mSpecWorker.mSpec.UnLockSpecificationData();
                        //enableAllControls();
                        //RnderingOutputTextbox();
                        //posDensity++;
                        // Call verify at 2
                        if (posImbalance + 1 < gridViewImbalanceLength)
                        {
                            posImbalance++;
                            mFlagVerifyImbalance = true;
                            mFlagVerifyDensity = false;
                            mSpecWorker.unlockShareData();
                            Spec.UnLockSharedData();
                            Spec.UnLockSpecificationData();
                            verifyAt(posImbalance, mFlagVerifyAll);
                        }
                        else
                        {
                            if ((posDensity + 1 == gridViewDensityLength) && (posImbalance + 1 == gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is not congestion at clusters");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                //OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                mVerifyListener.onVerifyNewNetwork();
                                resultVerifyGridView.Rows.Add("full_After_cluster", null, null);
                                posDensity = resultVerifyGridView.Rows.Count-1;
                                //Application.DoEvents();
                                mTimeVerify.Start();
                                mFlagVerifyDensity = true;
                                verifyAt(posDensity, mFlagVerifyAll);
                            }
                            else if ((posDensity + 1 > gridViewDensityLength) && (posImbalance + 1 == gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is unknown");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                VerifyAll_Btn.Text = "Verify All";
                                Close_Btn.Enabled = true;
                                Verify_Btn.Enabled = true;
                                numericTimeout.Enabled = true;
                                resultVerifyGridView.Enabled = true;
                                STOP_Btn.Enabled = false;
                                mlistAssert.Clear();
                            }
                            else if (posImbalance + 1 == gridViewImbalanceLength)
                            {
                                mFlagVerifyDensity = true;
                                posDensity++;
                                mSpecWorker.unlockShareData();
                                Spec.UnLockSharedData();
                                Spec.UnLockSpecificationData();
                                //Application.DoEvents();
                                verifyAt(posDensity, mFlagVerifyAll);
                            }
                        }
                    }
                    else if (mFlagVerifyImbalance == true)
                    {
                        int rowSelected = ImbalanceGridView.SelectedCells[0].RowIndex;
                        ImbalanceGridView.FirstDisplayedScrollingRowIndex = ImbalanceGridView.Rows[posImbalance].Index;
                        ImbalanceGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Gray;
                        ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                        ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Cancelled";
                        if (mTimeBroken != 0)
                        {
                            OutputTxtBox.AppendText("\n" + ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is " + "BROKEN....");
                        }
                        else
                        {
                            OutputTxtBox.AppendText("\n" + ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is " + "TIME OUT....");
                        }
                        if (posDensity + 1 < gridViewDensityLength)
                        {
                            mFlagVerifyDensity = true;
                            posDensity++;
                            mSpecWorker.unlockShareData();
                            Spec.UnLockSharedData();
                            Spec.UnLockSpecificationData();
                            DevLog.d(TAG, "posDensity: " + posDensity);
                            //Application.DoEvents();
                            verifyAt(posDensity, mFlagVerifyAll);
                        }
                        else
                        {
                            if ((posDensity + 1 == gridViewDensityLength) && (posImbalance + 1 == gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is not congestion at clusters");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                //OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                mVerifyListener.onVerifyNewNetwork();
                                ImbalanceGridView.Rows.Add("full_After_cluster", null, null);
                                posImbalance = ImbalanceGridView.Rows.Count-1;
                                Application.DoEvents();
                                mTimeVerify.Start();
                                mFlagVerifyImbalance = true;
                                verifyAt(posImbalance, mFlagVerifyAll);
                            }
                            else if ((posDensity + 1 == gridViewDensityLength) && (posImbalance + 1 > gridViewImbalanceLength))
                            {
                                mTimeVerify.Stop();
                                mTotalTimeVerify = mTimeVerify.Elapsed;
                                OutputTxtBox.AppendText("\n>>>>>>Network is unknown");
                                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                                OutputTxtBox.AppendText("\n================END VERIFICATION AUTO==============\n");
                                mSpecWorker.unlockShareData();
                                RnderingOutputTextbox();
                                exportMSWord();
                                VerifyAll_Btn.Text = "Verify All";
                                Close_Btn.Enabled = true;
                                Verify_Btn.Enabled = true;
                                numericTimeout.Enabled = true;
                                resultVerifyGridView.Enabled = true;
                                STOP_Btn.Enabled = false;
                                mlistAssert.Clear();
                            }
                            else if (posDensity + 1 == gridViewDensityLength)
                            {
                                mFlagVerifyImbalance = true;
                                posImbalance++;
                                mSpecWorker.unlockShareData();
                                Spec.UnLockSharedData();
                                Spec.UnLockSpecificationData();
                                //Application.DoEvents();
                                verifyAt(posImbalance, mFlagVerifyAll);
                            }
                        }
                    }

                }

            }
            else//verify one cluster
            {
                if (mFlagVerifyDensity == true)
                {
                    int rowSelected = resultVerifyGridView.SelectedCells[0].RowIndex;
                    resultVerifyGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Gray;
                    resultVerifyGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                    resultVerifyGridView.Rows[rowSelected].Cells[2].Value = "Cancelled";
                    statusLabel.Text = "Verification Completed";
                    mTimeVerify.Stop();
                    mTotalTimeVerify = mTimeVerify.Elapsed;
                    OutputTxtBox.AppendText("\n" + resultVerifyGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is " + "TIME OUT OR BROKEN....");

                }
                else if (mFlagVerifyImbalance == true)
                {
                    int rowSelected = ImbalanceGridView.SelectedCells[0].RowIndex;
                    ImbalanceGridView.Rows[rowSelected].DefaultCellStyle.ForeColor = Color.Gray;
                    ImbalanceGridView.Rows[rowSelected].Cells[1].Value = "UNKNOWN";
                    ImbalanceGridView.Rows[rowSelected].Cells[2].Value = "Cancelled";
                    statusLabel.Text = "Verification Completed";
                    mTimeVerify.Stop();
                    mTotalTimeVerify = mTimeVerify.Elapsed;
                    OutputTxtBox.AppendText("\n" + ImbalanceGridView.Rows[rowSelected].Cells[0].Value.ToString().ToUpper() + " is " + "TIME OUT OR BROKEN....");
                }
                mSpecWorker.unlockShareData();
                OutputTxtBox.AppendText("\nElapsed time verification: " + mTotalTimeVerify.TotalSeconds + "s");
                OutputTxtBox.AppendText("\nTotal elapsed time clustering & verification: " + (mTotalTimeVerify.TotalSeconds + mTotalTimeClustering) + "s");
                RnderingOutputTextbox();
                exportMSWord();
            }
        }

        public void updateLatexResult(AssertionBase assertion)
        {

        }

        private void ColorText(int start)
        {
            int index = int.MaxValue;
            int indexCase = 0;
            int index1 = OutputTxtBox.Find(mystring, start, -1, RichTextBoxFinds.WholeWord);

            if (index1 > 0 && index1 < index)
            {
                index = index1;
                indexCase = 0;
            }

            index1 = OutputTxtBox.Find("-> (", start, -1, RichTextBoxFinds.None);
            if (index1 > 0 && index1 < index)
            {
                index = index1;
                indexCase = 1;
            }

            index1 = OutputTxtBox.Find(ValidString, start, -1, RichTextBoxFinds.WholeWord | RichTextBoxFinds.MatchCase);
            if (index1 > 0 && index1 < index)
            {
                index = index1;
                indexCase = 2;
            }

            index1 = OutputTxtBox.Find(NotValidString, start, -1, RichTextBoxFinds.WholeWord | RichTextBoxFinds.MatchCase);
            if (index1 > 0 && index1 < index)
            {
                index = index1;
                indexCase = 3;
            }

            index1 = OutputTxtBox.Find(Common.Classes.Ultility.Constants.VERFICATION_RESULT_STRING, start, -1, RichTextBoxFinds.WholeWord | RichTextBoxFinds.MatchCase);
            if (index1 >= 0 && index1 < index)
            {
                index = index1;
                indexCase = 4;
            }

            index1 = OutputTxtBox.Find(BrokenString, start, -1, RichTextBoxFinds.WholeWord | RichTextBoxFinds.MatchCase);
            if (index1 > 0 && index1 < index)
            {
                index = index1;
                indexCase = 5;
            }

            index1 = OutputTxtBox.Find(TimeoutString, start, -1, RichTextBoxFinds.WholeWord | RichTextBoxFinds.MatchCase);
            if (index1 > 0 && index1 < index)
            {
                index = index1;
                indexCase = 6;
            }

            if (index != int.MaxValue)
            {
                switch (indexCase)
                {
                    case 0:
                        OutputTxtBox.SelectionStart = index;
                        OutputTxtBox.SelectionLength = mystring.Length;
                        OutputTxtBox.SelectionFont = fnt;
                        OutputTxtBox.SelectionColor = System.Drawing.Color.Red;
                        OutputTxtBox.SelectionBackColor = System.Drawing.Color.Yellow;
                        ColorText(index + mystring.Length);
                        return;

                    case 1:
                        int endIndex = OutputTxtBox.Find(")*", start, -1, RichTextBoxFinds.None);
                        if (endIndex > 0)
                        {
                            index = index + 3;
                            if (endIndex - index + 2 > 0)
                            {
                                OutputTxtBox.SelectionStart = index;

                                OutputTxtBox.SelectionLength = endIndex - index + 2;
                                OutputTxtBox.SelectionFont = fnt;
                                OutputTxtBox.SelectionColor = System.Drawing.Color.Red;
                                OutputTxtBox.SelectionBackColor = System.Drawing.Color.Yellow;
                            }
                            ColorText(endIndex + 2);
                        }
                        return;

                    case 2:
                        OutputTxtBox.SelectionStart = index;
                        OutputTxtBox.SelectionLength = ValidString.Length;
                        OutputTxtBox.SelectionFont = fntBold;
                        OutputTxtBox.SelectionColor = System.Drawing.Color.Green;
                        ColorText(index + 2);
                        return;

                    case 3:
                        OutputTxtBox.SelectionStart = index;
                        OutputTxtBox.SelectionLength = NotValidString.Length;
                        OutputTxtBox.SelectionFont = fntBold;
                        OutputTxtBox.SelectionColor = System.Drawing.Color.Red;
                        ColorText(index + 2);
                        return;

                    case 4:
                        OutputTxtBox.SelectionStart = index;
                        OutputTxtBox.SelectionLength = Common.Classes.Ultility.Constants.VERFICATION_RESULT_STRING.Length;
                        OutputTxtBox.SelectionFont = fntBold;
                        OutputTxtBox.SelectionColor = System.Drawing.Color.Blue;
                        ColorText(index + Common.Classes.Ultility.Constants.VERFICATION_RESULT_STRING.Length);
                        return;
                    case 5:
                        OutputTxtBox.SelectionStart = index;
                        OutputTxtBox.SelectionLength = BrokenString.Length;
                        OutputTxtBox.SelectionFont = fntBold;
                        OutputTxtBox.SelectionColor = System.Drawing.Color.Orange;
                        ColorText(index + 2);
                        return;
                    case 6:
                        OutputTxtBox.SelectionStart = index;
                        OutputTxtBox.SelectionLength = TimeoutString.Length;
                        OutputTxtBox.SelectionFont = fntBold;
                        OutputTxtBox.SelectionColor = System.Drawing.Color.Brown;
                        ColorText(index + 2);
                        return;
                }
            }
        }


        public void exportMSWord()
        {
            System.IO.FileStream file;
            SoundPlayer pingCompleted;
            file = new System.IO.FileStream(ClusterHelper.CURRENT_PATH + "_Log_Verify_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".doc", System.IO.FileMode.Create);
            OutputTxtBox.SaveFile(file, RichTextBoxStreamType.RichText);
            file.Close();
            if (mType == VerificationResultType.INVALID.ToString())
            {
                pingCompleted = new SoundPlayer(Application.StartupPath + "\\Not_valid.wav");
                //DevLog.d(TAG, Utilities.LOCAL_PATH + "\\SVModule\\Resources\\Not_valid.wav");
                //DevLog.d(TAG, "Local: "+Utilities.LOCAL_PATH);
            }
            else
            {
                pingCompleted = new SoundPlayer(Application.StartupPath + "\\Finish_all.wav");
                //DevLog.d(TAG, Utilities.LOCAL_PATH + "\\SVModule\\Resources\\Finish_all.wav");

            }
            pingCompleted.Play();
        }


        public void getTimeOut(int TimeOut)
        {
            OutputTxtBox.AppendText("\n" + mthisCluster.ToUpper() + " is TIME OUT at: " + TimeOut + "s");
            mTimeOut = TimeOut;
            mTimeBroken = 0;
        }

        public void getTimeBroken(int timeBroken)
        {
            OutputTxtBox.AppendText("\n" + mthisCluster.ToUpper() + " is BROKEN at: " + timeBroken + "s");
            mTimeBroken = timeBroken;
            mTimeOut = 0;
        }

        private void resultVerifyGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            posDensity = -1;
            posImbalance = -1;
            mFlagVerifyDensity = true;//a clusters in Density is selected
            mFlagVerifyImbalance = false;
            if (resultVerifyGridView.SelectedCells[0].ColumnIndex != 0)
            {
                MessageBox.Show("Please choose one cluster in column 'DENSITY CLUSTERS'", "Name cluster invalid");
            }
            else
            {
                posDensity = resultVerifyGridView.SelectedCells[0].RowIndex;
            }
        }

        private void ImbalanceGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            posImbalance = -1;
            posDensity = -1;
            mFlagVerifyImbalance = true;//a clusters in Imbalance is selected
            mFlagVerifyDensity = false;
            if (ImbalanceGridView.SelectedCells[0].ColumnIndex != 0)
            {
                MessageBox.Show("Please choose one cluster in column 'IMBALANCE CLUSTERS'", "Name cluster invalid");
            }
            else
            {
                posImbalance = ImbalanceGridView.SelectedCells[0].RowIndex;
            }
        }
    }
}
