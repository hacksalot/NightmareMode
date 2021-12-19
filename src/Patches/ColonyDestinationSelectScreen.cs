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
using NightmareMode.Core;
using System.Linq;
using UnityEngine;

namespace NightmareMode.Patches
{
   internal class ColonyDestinationSelectScreenPatch
   {
      [HarmonyPatch( typeof( ColonyDestinationSelectScreen ), "LaunchClicked" )]
      private class ColonyDestinationSelectScreen_LaunchClicked
      {
         private static bool Prefix( ColonyDestinationSelectScreen __instance ) {

            if( !Controller.InNightmareFlow ) return true;

            BeginNightmare( __instance, ( action, gobj ) => {

               if( action == "ok" ) {
                  if( ValidateMods( __instance, null ) )
                     DoOriginal( __instance );
               }

            } );

            return false;
         }
         private static bool ValidateMods( ColonyDestinationSelectScreen __instance, GameObject parent ) {
            parent = __instance.gameObject;
            var activeModObjects = Global.Instance.modManager.mods.Where(m => m.enabled);
            if( Options.Current.ModHandling == Regimen.ModPolicy.ProhibitAll && activeModObjects.Count() > 1 ) {
               Controller.MessageBox( NMStrings.MODS_ARE_PROHIBITED, NMStrings.TITLE_PROHIBITED_MODS, () => { }, null, parent ); ;
               return false;
            }

            var activeMods = activeModObjects.Select(m => m.title).ToList();
            ModChecker.Check( activeMods, out ModChecker.ModCheckResults res );

            if( Options.Current.ModHandling == Regimen.ModPolicy.QoL ) {
               if( res.HasUnknown || res.NetCost != 0 ) {
                  var bad = res.KnownMods.Where( m => m.Value.Cost != 0 ).Select( m => m.Value.Title ).ToList();
                  bad.AddRange( res.UnknownMods.Select( u => u.Value.Title ) );
                  Controller.MessageBox( string.Format( NMStrings.QOL_MODS_ONLY, string.Join( "\n", bad ) ), NMStrings.TITLE_PROHIBITED_MODS, () => { }, null, parent ); ;
                  return false;
               }
            }

            if( Options.Current.ModHandling == Regimen.ModPolicy.Curated ) {
               if( res.HasUnknown ) {
                  string offendingMods = string.Join("\n", res.UnknownMods.Select(m => m.Value.Title));
                  string msg = string.Format(NMStrings.WARN_UNKNOWN_MODS, offendingMods);
                  Controller.MessageBox( msg, NMStrings.TITLE_UNKNOWN_MODS, () => { }, null, parent );
                  return false;
               }
            }

            return true;
         }

         private static void BeginNightmare( ColonyDestinationSelectScreen __instance, System.Action<string, GameObject> callback ) {
            GameObject go = null;
            go = UI.OptionsDialog.Init(
               Options.Current,
               __instance.gameObject.transform.parent.gameObject,
               NMStrings.OPTIONS_HEADER_FLOW_MSG,
               NMStrings.BEGIN_NIGHTMARE,
               ( action ) => {
                  callback( action, go );
               }
            );
         }

         private static void DoOriginal( ColonyDestinationSelectScreen inst ) {
            Traverse.Create( inst ).Method( "NavigateForward" ).GetValue( null );
         }
      }
   }
}
