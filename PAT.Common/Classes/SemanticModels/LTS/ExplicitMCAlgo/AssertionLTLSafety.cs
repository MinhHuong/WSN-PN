using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PAT.Common.Classes.BA;
using PAT.Common.Classes.DataStructure;
using PAT.Common.Classes.LTS;
using PAT.Common.Classes.ModuleInterface;
using PAT.Common.Classes.Ultility;
using PAT.Common.GUI;

namespace PAT.Common.Classes.SemanticModels.LTS.Assertion
{
    public partial class AssertionLTL 
    {
        /// <summary>
        /// Run the verification and get the result.
        /// </summary>
        /// <returns></returns>
        public virtual void RunVerificationSafety()
        {
            if (SelectedEngineName == Constants.ENGINE_DEPTH_FIRST_SEARCH)
            {
                DFSVerification();
            }
            else
            {
                BFSVerification();
            }           
        }

        /// <summary>
        /// Verify the LTL property using DFS algorithm
        /// </summary>
        public void DFSVerification()
        {
            // Adding probability indicators
            double probPathCongestion = 1d;
            List<string> counterExamples = new List<string>();
            List<double> probOnPaths = new List<double>();
            // double CPT = 0.2d; // CPT: Congestion Probability Threshold

            #region Identifying a counter-example trace
            Stack<int> depthStack = new Stack<int>(1024);
            depthStack.Push(0);
            List<int> depthList = new List<int>(1024);
            #endregion

            List<ConfigurationBase> counterExampleTrace = new List<ConfigurationBase>();

            Stack<EventBAPairSafety> TaskStack = new Stack<EventBAPairSafety>();
            Stack<double> probTaskStack = new Stack<double>();

            EventBAPairSafety initialstep = EventBAPairSafety.GetInitialPairs(BA, InitialStep);
            TaskStack.Push(initialstep);
            probTaskStack.Push(probPathCongestion);
            
            //Dictionary<string, bool> Visited = new Dictionary<string, bool>();
            StringHashTable Visited = new StringHashTable(Ultility.Ultility.MC_INITIAL_SIZE);

            while (TaskStack.Count != 0)
            {
                if (CancelRequested)
                {
                    this.VerificationOutput.NoOfStates = Visited.Count;
                    return;
                }

                EventBAPairSafety now = TaskStack.Pop();
                double probNow = probTaskStack.Pop();
                string ID = now.GetCompressedState();

                #region Identifying a counter-example trace
                int depth = depthStack.Pop();

                if (depth > 0)
                {
                    while (depthList[depthList.Count - 1] >= depth) // step back from the counter-example trace
                    {
                        int lastIndex = depthList.Count - 1;
                        depthList.RemoveAt(lastIndex);
                        counterExampleTrace.RemoveAt(lastIndex);
                    }
                }

                //this.VerificationOutput.CounterExampleTrace.Add(now.configuration);
                counterExampleTrace.Add(now.configuration);
                depthList.Add(depth);
                #endregion

                // No state to visit
                if (now.States.Count == 0)
                {
                    this.VerificationOutput.NoOfStates = Visited.Count;
                    this.VerificationOutput.VerificationResult = VerificationResultType.INVALID;

                    // Add probability of choosing path leading to congestion, displayed on VerificationOuput
                    probOnPaths.Add(probNow);
                    StringBuilder sb = new StringBuilder();
                    foreach(ConfigurationBase cb in counterExampleTrace)
                    {
                        sb.Append(cb.Event);
                        sb.Append(" -> ");
                    }
                    counterExamples.Add(sb.ToString());

                    //this.VerificationOutput.ProbPathCongestion = probPathCongestion;

                    // Specify which sensor (CongestionX) provokes the congestion, displayed on VerificationOutput
                    //this.VerificationOutput.CongestedSensor = now.configuration.Event;

                    continue;
                    //return;
                }

                if (!Visited.ContainsKey(ID))
                {
                    Visited.Add(ID);

                    ConfigurationBase[] steps = now.configuration.MakeOneMove().ToArray();
                    this.VerificationOutput.Transitions += steps.Length;
                    EventBAPairSafety[] products = now.Next(BA,steps);
                    foreach (EventBAPairSafety step in products)
                    {
                        TaskStack.Push(step);
                        probTaskStack.Push(probNow * GetProbabilityFromChannel(step));
                        depthStack.Push(depth + 1);
                    }
                }
            }

            if(this.VerificationOutput.VerificationResult != VerificationResultType.INVALID)
            {
                this.VerificationOutput.CounterExampleTrace = null;
                this.VerificationOutput.NoOfStates = Visited.Count;
                this.VerificationOutput.VerificationResult = VerificationResultType.VALID;
            }
        }

        private double GetProbabilityFromChannel(EventBAPairSafety now)
        {
            double result = 1d;

            // Will not work if the name of ChannelX_Y has been changed before this verification is launched
            Regex reg_channel = new Regex(@"Channel([0-9]+_[0-9]+)");
            Match match = reg_channel.Match(now.configuration.Event);
            if (match.Success)
            {
                string _regProb = "prob" + match.Groups[1].Value + "=([0-9]+)";
                Regex regProb = new Regex(@_regProb);
                EventStepSim stepSim = new EventStepSim(now.configuration);
                Match matchProb = regProb.Match(stepSim.StepToString);
                if (matchProb.Success)
                {
                    result = double.Parse(matchProb.Groups[1].Value) / 100;
                }
            }

            return result;
        }

        /// <summary>
        /// Verify the LTL property using BFS algorithm
        /// </summary>
        public void BFSVerification()
        {
            //Dictionary<string, bool> Visited = new Dictionary<string, bool>();
            StringHashTable Visited = new StringHashTable(Ultility.Ultility.MC_INITIAL_SIZE);
            Queue<EventBAPairSafety> working = new Queue<EventBAPairSafety>();
            Queue<List<ConfigurationBase>> paths = new Queue<List<ConfigurationBase>>(1024);

            EventBAPairSafety initialstep = EventBAPairSafety.GetInitialPairs(BA,InitialStep);
            working.Enqueue(initialstep);

            List<ConfigurationBase> path = new List<ConfigurationBase>();
            path.Add(InitialStep);
            paths.Enqueue(path);
            Visited.Add(initialstep.GetCompressedState());
            
            do
            {
                if (CancelRequested)
                {
                    VerificationOutput.NoOfStates = Visited.Count;
                    return;
                }

                EventBAPairSafety current = working.Dequeue();
                List<ConfigurationBase> currentPath = paths.Dequeue();
            
                if (current.States.Count == 0)
                {
                    VerificationOutput.VerificationResult = VerificationResultType.INVALID;
                    VerificationOutput.NoOfStates = Visited.Count;
                    VerificationOutput.CounterExampleTrace = currentPath;
                    return;
                }

                ConfigurationBase[] steps = current.configuration.MakeOneMove().ToArray();
                VerificationOutput.Transitions += steps.Length;
                EventBAPairSafety[] products = current.Next(BA, steps);
                foreach (EventBAPairSafety step in products)
                {
                    string stepID = step.GetCompressedState();
                    if (!Visited.ContainsKey(stepID))
                    {
                        Visited.Add(stepID);
                        working.Enqueue(step);

                        List<ConfigurationBase> newPath = new List<ConfigurationBase>(currentPath);
                        newPath.Add(step.configuration);
                        paths.Enqueue(newPath);
                    }
                }
               
            } while (working.Count > 0);

            VerificationOutput.NoOfStates = Visited.Count;
            VerificationOutput.VerificationResult = VerificationResultType.VALID;
        }

        public virtual string GetResultStringSafety()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Constants.VERFICATION_RESULT_STRING);

            if (this.VerificationOutput.VerificationResult == VerificationResultType.VALID)
            {
                sb.AppendLine("The Assertion (" + ToString() + ") is VALID.");
            }
            else
            {
                if (this.VerificationOutput.VerificationResult == VerificationResultType.UNKNOWN)
                {
                    sb.AppendLine("The Assertion (" + ToString() + ") is NEITHER PROVED NOR DISPROVED.");
                }
                else
                {
                    sb.AppendLine("The Assertion (" + ToString() + ") is NOT valid.");
                    sb.AppendLine("A counterexample is presented as follows.");
                    //GetCounterxampleString(sb);
                    VerificationOutput.GetCounterxampleString(sb);
                }
            }

            sb.AppendLine();
            sb.AppendLine("********Verification Setting********");
            sb.AppendLine("Admissible Behavior: " + SelectedBahaviorName);
            if (SelectedEngineName == Constants.ENGINE_DEPTH_FIRST_SEARCH)
            {
                sb.AppendLine("Method: Refinement Based Safety Analysis using DFS - The LTL formula is a safety property!"); 
            }
            else
            {
                sb.AppendLine("Method: Refinement Based Safety Analysis using BFS - The LTL formula is a safety property!");                    
            }            
            sb.AppendLine("System Abstraction: " + MustAbstract);
            sb.AppendLine();

            return sb.ToString();
        }
    }
}