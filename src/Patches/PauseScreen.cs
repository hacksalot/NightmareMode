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

namespace NightmareMode.Patches
{
   internal class PauseScreenPatch
   {
      [HarmonyPatch( typeof( PauseScreen ), "OnShow" )]
      private class PauseScreen_OnSpawn
      {
         private static void Postfix( PauseScreen __instance, bool show ) {
            if( !show || !Controller.IsEnforcing ) return;
            var inst = Traverse.Create(__instance);
            var buttons = inst.Field("buttons").GetValue<KButtonMenu.ButtonInfo[]>();
            var btnList = buttons.ToList();
            var btn = btnList.Find(b => b.text == NMStrings.NIGHTMARE_MODE_OPTIONS);
            if( btn == null ) { 
               KButtonMenu.ButtonInfo inf = null;
               inf = new KButtonMenu.ButtonInfo(
                   NMStrings.NIGHTMARE_MODE_OPTIONS,
                   Action.NumActions,
                   () => {
                      UI.OptionsDialog.Init( Options.Current, __instance.transform.parent.gameObject, showHeader: false );
                   } );
               btnList.Insert( btnList.Count - 2, inf );
               inst.Field( "buttons" ).SetValue( btnList.ToArray() );
            }

            btnList [2].isEnabled = !Options.Current.PreventSaveAs;
            btnList [2].toolTip = Options.Current.PreventSaveAs ? NMStrings.SAVE_AS_DISABLED : null;
            __instance.RefreshButtons();
         }

         //private static void WarnToggleNightmare( System.Action onConfirm, System.Action onCancel ) {
         //   var confDlg = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, PauseScreen.Instance.gameObject);
         //   confDlg.PopupConfirmDialog( string.Format( NMStrings.WARN_TOGGLE_NIGHTMARE, Controller.IsEnforcing ? "dis" : "en" ), onConfirm, onCancel );
         //   confDlg.gameObject.SetActive( true );
         //}
      }
   }
}
