using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class CommonlyUsedVarVisitor : AutoVisitor
    {
		Dictionary<String, int> myMap = new Dictionary<String, int>();

		public string mostCommonlyUsedVar()
        {
			return myMap.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
		}

		public override void VisitVarDefNode(VarDefNode w)
		{
			foreach (var v in w.vars)
				if (myMap.ContainsKey(v.Name))
				{
					myMap[v.Name]++;
				}
				else {
					myMap[v.Name] = 1;
				}
		}
	}
}
