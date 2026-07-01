using HarmonyLib;
using Timberborn.Modding;
using Timberborn.ModManagerScene;
using Timberborn.SkySystem;

namespace AlwaysDaylight
{
  /// <summary>
  /// Entry point for the native Timberborn Mod System.
  /// </summary>
  public class AlwaysDaylightModStarter : IModStarter
  {
    public void StartMod(IModEnvironment modEnvironment)
    {
      var harmony = new Harmony("Calloatti.AlwaysDaylight");
      harmony.PatchAll(typeof(AlwaysDaylightModStarter).Assembly);
    }
  }

  /// <summary>
  /// Patches the visual cycle to always report a midday state to the rendering systems.
  /// </summary>
  [HarmonyPatch(typeof(DayStageCycle), nameof(DayStageCycle.GetCurrentTransition))]
  internal static class DayStageCyclePatch
  {
    private static bool Prefix(ref DayStageTransition __result)
    {
      // Create a locked transition state: purely Day stage, 0% progress towards Sunset.
      // This leaves the actual underlying game clock completely untouched.
      __result = new DayStageTransition(
          currentDayStage: DayStage.Day,
          currentDayStageHazardousWeatherId: null,
          nextDayStage: DayStage.Sunset,
          nextDayStageHazardousWeatherId: null,
          transitionProgress: 0f
      );

      return false; // Skip the original calculation method
    }
  }
}