Include ..\AGENTS.md

# Always Daylight — Mod-Specific Agent Instructions

## Identity
- **Assembly:** `alwaysdaylight`
- **Namespace:** `AlwaysDaylight`
- **Framework:** Harmony (ID `"Calloatti.AlwaysDaylight"`)
- **Config:** SimpleConfig (`ModStarter.Config`) — config file in mod folder
- **ModId:** `Calloatti.AlwaysDaylight`
- **Min Game Version:** 1.0.0.0 — uses `timberborn-decompiled-1.0.*`

## What This Mod Does
Forces the day stage to always be `Day` and optionally disables fog. Patches `DayStageCycle.GetCurrentTransition` to always return a Day→Sunset transition at 0% progress, and `Sun.LateUpdateSingleton` to set `Fog = false`.

## Source Architecture (`Version-1.0/Source/`)

| File | Role |
|---|---|
| `AlwaysDaylight.cs` | `IModStarter` entry point + Harmony patches |

## Key Patches
- `DayStageCycle.GetCurrentTransition` — Harmony Prefix: returns a no-transition Day stage when `AlwaysDaylight` config is true
- `Sun.LateUpdateSingleton` — Harmony Postfix: disables fog when `DisableFog` config is true
