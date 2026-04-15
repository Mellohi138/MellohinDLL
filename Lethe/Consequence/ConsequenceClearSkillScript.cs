using ModularSkillScripts;

namespace Mellohin.Consequence;

public class ConsequenceClearSkillScript : IModularConsequence
{
	public void ExecuteConsequence(ModularSA modular, string section, string circledSection, string[] circles)
	{
		if (circles[0] != null && circles[0] == "all")
		{
			if (modular.modsa_unitModel == null) return;

			foreach (SinActionModel sinslot in modular.modsa_unitModel.GetSinActionList())
			{
				foreach (UnitSinModel sinModel in sinslot.currentSinList)
				{
					sinModel.GetSkill().ClearTemporarySkillAbility();
				}
			}
		}
		else modular.modsa_skillModel?.ClearTemporarySkillAbility();
	}
}