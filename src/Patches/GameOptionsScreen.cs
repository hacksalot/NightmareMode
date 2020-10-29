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
using NightmareMode.Core;

namespace NightmareMode.Patches
{
   internal class GameOptionsScreenPatch
   {
      [HarmonyPatch( typeof( GameOptionsScreen ), "OnSpawn" )]
      private class GameOptionsScreen_OnSpawn
      {
         private static void Postfix( GameOptionsScreen __instance, KButton ___sandboxButton ) {
            if( Controller.IsEnforcing ) {
               ___sandboxButton.ClearOnClick();
               ___sandboxButton.onClick += OnClickSandbox;
               _inst = __instance;
            }
         }

         private static void OnClickSandbox() {
            var confDlg = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, _inst.gameObject);
            confDlg.PopupConfirmDialog(
               string.Format( NMStrings.WARN_ENTER_SANDBOX, Controller.IsEnforcing ? "dis" : "en" ), () => {
                  confDlg.Show( false );
                  confDlg.gameObject.SetActive( false );
                  Traverse.Create( _inst ).Method( "OnUnlockSandboxMode" ).GetValue( null );
               }, () => { } );
            confDlg.gameObject.SetActive( true );
         }

         private static GameOptionsScreen _inst;
      }
   }
}
