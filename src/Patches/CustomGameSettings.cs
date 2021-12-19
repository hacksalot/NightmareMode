/**
The MIT License

Copyright (c) 2020-2022 James Devlin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace NightmareMode.Patches
{
   internal class CustomGameSettingsPatch
   {
      [HarmonyPatch(typeof(CustomGameSettings), "SetNosweatDefaults")]
      private class SetNosweatDefaults
      {
         private static void Postfix(CustomGameSettings __instance)
         {
            __instance.customGameMode = CustomGameSettings.CustomGameMode.Custom;
            foreach (KeyValuePair<string, Klei.CustomSettings.SettingConfig> qualitySetting in __instance.QualitySettings)
            {
               List<Klei.CustomSettings.SettingLevel> levels = qualitySetting.Value.GetLevels();
               string chosen;
               switch (qualitySetting.Key)
               {
                  case "SandboxMode":
                  case "CarePackages":
                  case "FastWorkersMode":
                     chosen = "Disabled"; break;
                  case "StressBreaks":
                     chosen = "Default"; break;
                  case "World":
                     chosen = levels[Random.Range(0, levels.Count)].id; break;
                  case "WorldgenSeed":
                     chosen = qualitySetting.Value.id; break;
                  default: // "ImmuneSystem" "CalorieBurn" "Morale" "Stress"
                     chosen = levels[levels.Count - 1].id; break;
               }
               __instance.SetQualitySetting(qualitySetting.Value, chosen);
            }
         }
      }
   }
}
