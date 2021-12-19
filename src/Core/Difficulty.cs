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

using System.Collections.Generic;
using System.Linq;

namespace NightmareMode.Core
{
   internal class Difficulty
   {
      public const int MAX_VANILLA_SCORE = 100;
      public const int MIN_VANILLA_SCORE = 0;

      public static int CalculateRating( Regimen h ) {
         int base_score = CalculateBaselineRating();
         int xtra_score = CalculateAdjustedRating(h);
         int meta_score = CalculateMetaRating();
         return base_score + xtra_score + meta_score;
      }

      private static int CalculateBaselineRating() {
         int score = MAX_VANILLA_SCORE; // 100%

         foreach( KeyValuePair<string, Klei.CustomSettings.SettingConfig> qualitySetting
                  in CustomGameSettings.Instance.QualitySettings ) {
            var lvl = CustomGameSettings.Instance.GetCurrentQualitySetting(qualitySetting.Value);

            switch( qualitySetting.Key ) {
               case "CarePackages":
                  if( lvl.id != "Disabled" ) score -= 10; break;
               case "StressBreaks":
                  if( lvl.id != "Default" ) score -= 10; break;
               case "ImmuneSystem":
               case "CalorieBurn":
               case "Morale":
               case "Stress":
                  List<Klei.CustomSettings.SettingLevel> levels = qualitySetting.Value.GetLevels();
                  var topLevel = levels[levels.Count - 1];
                  if( topLevel.id != lvl.id ) {
                     for( int idx = 0; idx < levels.Count; idx++ ) {
                        Klei.CustomSettings.SettingLevel lvl2 = levels[idx];
                        if( lvl2.id == lvl.id ) {
                           var diff = (levels.Count - 1) - idx;
                           score -= ( diff * 5 );
                           break;
                        }
                     }
                  }
                  break;
               default:
                  break;
            }
         }

         return score;
      }

      private static int CalculateAdjustedRating( Regimen h ) {
         int score = 0;

         foreach( KeyValuePair<string, Klei.CustomSettings.SettingConfig> qualitySetting
                  in CustomGameSettings.Instance.QualitySettings ) {
            switch( qualitySetting.Key ) {
               case "FastWorkersMode":
                  var lvl = CustomGameSettings.Instance.GetCurrentQualitySetting(qualitySetting.Value);
                  if( lvl.id != "Disabled" ) score -= 25;
                  break;
               case "SandboxMode":
                  lvl = CustomGameSettings.Instance.GetCurrentQualitySetting( qualitySetting.Value );
                  if( lvl.id != "Disabled" ) score -= 1000;
                  break;
               case "World":
               case "WorldgenSeed":
               default:
                  break;
            }
         }

         if( h.SingleSaveFile ) {
            score += 35;
            if( !h.SaveOnThreat ) score -= 10;
            if( !h.SaveOnDeath ) score -= 10;
         }

         return score;
      }

      private static int CalculateMetaRating() {
         int score = 0;
         var activeMods = Global.Instance.modManager.mods.Select(m => m.title).ToList();
         ModChecker.Check( activeMods, out ModChecker.ModCheckResults res );
         foreach( ModChecker.ModDesc md in res.KnownMods.Values ) {
            score += md.Cost;
         }
         return score;
      }
   }
}
