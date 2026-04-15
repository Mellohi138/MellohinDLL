using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Mellohin.Acquirer;
using Mellohin.Consequence;
using Mellohin.Timing;
using ModularSkillScripts;
using System;

namespace Mellohin;

[BepInPlugin(GUID, NAME, VERSION)]
public class MellohinMain : BasePlugin
{
    public const string GUID = $"{AUTHOR}.{NAME}";
    public const string NAME = "MellohinDLL";
    public const string VERSION = "1.0.0";
    public const string AUTHOR = "Mellohi";
    public static ManualLogSource Logger;

	public override void Load()
    {
		Harmony harmony = new(NAME);
		Logger = new(NAME);
		BepInEx.Logging.Logger.Sources.Add(Logger);

		AddTiming(harmony, typeof(CustomPatches), null, -1);
		AddTiming(harmony, typeof(TimingOnEquipDefense), "OnEquipDefense", 9000);

		MainClass.consequenceDict["replaceallaffinity"] = new ConsequenceReplaceAllAffinity();
		MainClass.consequenceDict["addkeyword"] = new ConsequenceAddKeyword();
		MainClass.consequenceDict["clearskillscript"] = new ConsequenceClearSkillScript();
		MainClass.consequenceDict["replaceskillondashboard"] = new ConsequenceReplaceSkillOnDashboard();
		MainClass.consequenceDict["upgradeskillondashboard"] = new ConsequenceUpgradeSkillOnDashboard();

		MainClass.acquirerDict["getskillslotindex"] = new AcquierGetSkillSlotIndex();
		MainClass.acquirerDict["issinnerfielded"] = new AcquierIsSinnerFielded();
	}

	private static void AddTiming(Harmony harmony, Type type, string name, int id)
	{
		harmony.PatchAll(type);
		if (name != null)
		{
			MainClass.timingDict.Add(name, id);
		}
	}
}
