/**
The MIT License

Copyright (c) 2020 James M. Devlin

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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NightmareMode.Patches
{
   internal class NotificationPatch
   {
      [HarmonyPatch( typeof( Notification ), "Notification" )]
      [HarmonyPatch( MethodType.Constructor )]
      [HarmonyPatch( new Type [ ] {
         typeof(string), typeof(NotificationType), typeof(HashedString), typeof(Func<List<Notification>, object, string>),
         typeof(object), typeof(bool), typeof(float), typeof(Notification.ClickCallback),
         typeof(object), typeof(Transform), typeof(bool)
      } )]
      private class Notification_Notification
      {
         private static void Postfix(
           string title,
           NotificationType type,
           HashedString group,
           Func<List<Notification>, object, string> tooltip,
           object tooltip_data,
           bool expires,
           float delay,
           Notification.ClickCallback custom_click_callback,
           object custom_click_data,
           Transform click_focus,
           bool volume_attenuation ) {
            if( !Controller.IsEnforcing || !Controller.HasLoaded ) return;
            if( type == NotificationType.DuplicantThreatening && Options.Current.SaveOnThreat ) {
               if( Controller.ElapsedLastSave.TotalSeconds > 15 ) {
                  Log.Out( "Notification.Notification({0}, {1})", title, type.ToString() );
                  SpeedControlScreen.Instance.Pause();
                  SaveLoader.Instance.Save( Controller.SaveFilePath, true, true );
                  SpeedControlScreen.Instance.Unpause();
               }
            }
            else if( type == NotificationType.Bad ) {
               if(Controller.ElapsedLastSave.TotalSeconds > 30) {
                  Log.Out( "Notification.Notification({0}, {1})", title, type.ToString() );
                  SpeedControlScreen.Instance.Pause();
                  SaveLoader.Instance.Save( Controller.SaveFilePath, true, true );
                  SpeedControlScreen.Instance.Unpause();
               }
            }
         }
      }
   }
}

// Search in sources for "NotificationType.Bad" (whole word match)
// NotificationType.Bad =
//    [Duplicant Status Items]
//    - NervouseBreakdown
//    - NoRationsAvailable
//    - Starving
//    - SevereWounds
//    - Fighting
//    - Fleeing
//    - LashingOut
//    [Creature Status Items]
//    - Hypothermia
//    - Hyperthermia
//    [Building Status Items]
//    - AngerDamage
//    - Overheated
//    - Overloaded
//    - LogicOverloaded
//    [Misc]
//    - Space
//    [GameFlow]
//    - ColonyLost
