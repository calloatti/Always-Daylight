using Calloatti.Config;
using HarmonyLib;
using System.Reflection;
using Timberborn.Modding;
using Timberborn.ModManagerScene;
using Timberborn.SkySystem;
using UnityEngine;

namespace AlwaysDaylight
{
  public class ModStarter : IModStarter
  {
    public static SimpleConfig Config { get; private set; }

    public void StartMod(IModEnvironment modEnvironment)
    {
      Config = new SimpleConfig(modEnvironment.ModPath);
      var harmony = new Harmony("Calloatti.AlwaysDaylight");
      harmony.PatchAll();
    }
  }

  [HarmonyPatch(typeof(DayStageCycle), nameof(DayStageCycle.GetCurrentTransition))]
  internal static class DayStageCyclePatch
  {
    private static bool Prefix(ref DayStageTransition __result)
    {
      if (ModStarter.Config?.GetBool("AlwaysDaylight") == true)
      {
        __result = new DayStageTransition(
            currentDayStage: DayStage.Day,
            currentDayStageHazardousWeatherId: null,
            nextDayStage: DayStage.Sunset,
            nextDayStageHazardousWeatherId: null,
            transitionProgress: 0f
        );
        return false;
      }
      return true;
    }
  }

  [HarmonyPatch(typeof(Sun), nameof(Sun.LateUpdateSingleton))]
  internal static class SunPatch
  {
    private static void Postfix(Sun __instance)
    {
      if (ModStarter.Config?.GetBool("DisableFog") == true)
        __instance.Fog = false;
    }
  }

  [HarmonyPatch(typeof(Sun), nameof(Sun.UpdateShadows))]
  internal static class SunShadowPatch
  {
    private static bool Prefix(Sun __instance)
    {
      if (ModStarter.Config?.GetBool("DisableShadows") == true)
      {
        var sunField = AccessTools.Field(typeof(Sun), "_sun");
        if (sunField != null)
        {
          var sun = sunField.GetValue(__instance) as Light;
          if (sun != null)
            sun.shadows = LightShadows.None;
        }
        return false;
      }
      return true;
    }
  }

  [HarmonyPatch(typeof(Sun), nameof(Sun.UpdateColors))]
  internal static class SunShadowStrengthPatch
  {
    private static void Postfix(Sun __instance, DayStageTransition dayStageTransition)
    {
      if (ModStarter.Config?.GetBool("DisableShadows") == true)
      {
        var sunField = AccessTools.Field(typeof(Sun), "_sun");
        if (sunField != null)
        {
          var sun = sunField.GetValue(__instance) as Light;
          if (sun != null)
            sun.shadowStrength = 0f;
        }
      }
    }
  }
}