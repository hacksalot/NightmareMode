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

namespace NightmareMode.Patches
{
   internal class AppPatches
   {
      [HarmonyPatch( typeof( App ), "LoadScene" )]
      private class App_LoadScene
      {
         private static void Postfix( string scene_name ) {
            if( scene_name == "frontend" ) {
               Controller.HasLoaded = false;
               Controller.IsEnforcing = false;
               Controller.InNightmareFlow = false;
               Controller.IsNewGame = false;
            }
         }
      }

      [HarmonyPatch( typeof( App ), "Quit" )]
      private class App_Quit
      {
         private static void Postfix() {
            if( Controller.IsEnforcing && Options.Current != null && Options.Current.SaveOnExit ) {
               SpeedControlScreen.Instance.Pause();
               SaveLoader.Instance.Save( Controller.SaveFilePath, true, true );
               SpeedControlScreen.Instance.Unpause();
            }
         }
      }
   }
}
