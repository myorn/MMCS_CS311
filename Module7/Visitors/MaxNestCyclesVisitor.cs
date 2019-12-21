using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class MaxNestCyclesVisitor : AutoVisitor
    {
        public int MaxNest = 0;
		public int CurNest = 0;
		public override void VisitCycleNode(CycleNode c)
		{
			CurNest++;
			c.Expr.Visit(this);
			c.Stat.Visit(this);

			if (CurNest > MaxNest) MaxNest = CurNest;
			CurNest--;
		}
	}
}
