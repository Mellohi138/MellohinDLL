using ModularSkillScripts;
using System.Collections.Generic;
using System.Linq;

namespace Mellohin.Consequence;

public class ConsequenceReplaceSkillOnDashboard : IModularConsequence
{
	public void ExecuteConsequence(ModularSA modular, string section, string circledSection, string[] circles)
	{
		BattleUnitModel unitModel = modular.modsa_unitModel;
		if (unitModel == null) return;

		List<(int, int, int)> skillSlotList = [];
		int[] skillIDList = new int[circles.Length];
		for(int i = 0; i < circles.Length; i++) skillIDList[i] = modular.GetNumFromParamString(circles[i]);
		foreach (SinActionModel sinSlot in unitModel.GetSinActionList())
		{
			for(int i = 0; i < sinSlot.currentSinList.Count; i++)
			{
				SkillModel skillModel = sinSlot.currentSinList[i].GetSkill();
				if (skillIDList.Contains(skillModel.GetID()))
				{
					goto SkipEverything;
				}

				if (skillModel.IsEgoSkill()) continue;

				skillSlotList.Add(new(sinSlot.GetSlotIndex(), i, skillModel.GetSkillTier()));
			}
		}

		skillSlotList.Sort(new SkillComparer());
		SinActionModel targetAction = unitModel.GetSinActionList()[skillSlotList[0].Item1];
		targetAction.currentSinList[skillSlotList[0].Item2] = new(skillIDList[0], unitModel, targetAction, true);

	SkipEverything:
		_ = skillSlotList;
	}

	private class SkillComparer : IComparer<(int, int, int)>
	{
		public int Compare((int, int, int) x, (int, int, int) y)
		{
			if (x.Item3 > y.Item3)
			{
				return 1;
			}
			else if (x.Item3 == y.Item3)
			{
				if (x.Item1 < y.Item1) 
				{
					return -1;
				}
				return 0;
			}
			return -1;
		}
	}
}
