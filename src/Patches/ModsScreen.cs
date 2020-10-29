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

using Harmony;
using System.Collections.Generic;
using UnityEngine;
using NightmareMode.Core;
using Newtonsoft.Json;
using System.IO;

namespace NightmareMode.Patches
{
   internal class ModsScreenPatch
   {
      [HarmonyPatch( typeof( ModsScreen ), "BuildDisplay" )]
      private class ModsScreen_BuildDisplay
      {
         protected static KButton optionsButton;

         private static GameObject thisObject;

         private static void Postfix( ModsScreen __instance, List<DisplayedMod> ___displayedMods ) {
            thisObject = __instance.gameObject;
            foreach( DisplayedMod mod in ___displayedMods ) {
               if( mod.rect_transform.GetComponentInChildren<LocText>().text == "Nightmare Mode" ) {
                  optionsButton = GameObject.Instantiate( mod.rect_transform.GetComponentInChildren<KButton>() );
                  optionsButton.transform.SetParent( mod.rect_transform, false );
                  optionsButton.GetComponentInChildren<LocText>().text = "Options";
                  optionsButton.transform.SetSiblingIndex( 3 );
                  optionsButton.onClick += OptionsButton_onClick;
                  break;
               }
            }
         }

         private static void OptionsButton_onClick() {
            UI.OptionsDialog.Init( Options.Default, thisObject, myOnClose: (action) => {
               if(action == "ok") { 
                  string str_obj = JsonConvert.SerializeObject( Options.Default );
                  File.WriteAllText( Controller.OptionsFilePath, str_obj );
               }
            } );
         }
      }

      private struct DisplayedMod
      {
         public RectTransform rect_transform;
         public int mod_index;
      }
   }
}
