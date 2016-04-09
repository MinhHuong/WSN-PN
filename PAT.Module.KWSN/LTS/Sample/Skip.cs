﻿using System.Collections.Generic;
using PAT.Common.Classes.Expressions;
using PAT.Common.Classes.Expressions.ExpressionClass;
using PAT.Common.Classes.Ultility;
using PAT.Common.Classes.ModuleInterface;

namespace PAT.KWSN.LTS{
    public sealed class Skip : Process
    {       
        public Skip() 
        {
            ProcessID = Constants.SKIP;
        }
        
        public override void MoveOneStep(Valuation GlobalEnv, List<Configuration> list)
        {
            System.Diagnostics.Debug.Assert(list.Count == 0);

            list.Add(new Configuration(new Stop(), Constants.TERMINATION, null, GlobalEnv, false));
        }
    
        public override string ToString()
        {
            return "Skip";
        }
    
        public override Process ClearConstant(Dictionary<string, Expression> constMapping)
        {
            return this;
        }
    }
}