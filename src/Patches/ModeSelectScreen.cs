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
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace NightmareMode.Patches
{
   internal class ModeSelectScreenPatch1
   {
      [HarmonyPatch( typeof( ModeSelectScreen ), "OnSpawn" )]
      private class OnSpawn
      {
         private static void Postfix( ModeSelectScreen __instance, MultiToggle ___nosweatButton, KBatchedAnimController ___nosweatAnim ) {
            LocText tt = ___nosweatButton.gameObject.GetComponentInChildren<LocText>();
            GameObject obj = new GameObject();
            Image img = obj.AddComponent<Image>();
            img.sprite = Sprite.Create( LoadPNG( Controller.ModeIconPath ), Rect.MinMaxRect( 0, 0, 398, 440 ), Vector2.zero );
            img.rectTransform.sizeDelta = new Vector2( 132.6f, 146.6f );
            obj.transform.SetParent( ___nosweatAnim.gameObject.transform.parent, false );
            GameObject.Destroy( ___nosweatAnim );
            if( tt != null ) tt.text = NMStrings.NIGHTMARE_MODE_NAME;
            Controller.IsEnforcing = false;
            Controller.InNightmareFlow = false;
         }

         private static Texture2D LoadPNG( string filePath ) {
            Texture2D tex = null;
            if( File.Exists( filePath ) ) {
               byte[] fileData = File.ReadAllBytes(filePath);
               tex = new Texture2D( 2, 2 );
               tex.LoadImage( fileData );
            }
            return tex;
         }
      }

      [HarmonyPatch( typeof( ModeSelectScreen ), "OnHoverEnterNosweat" )]
      private class OnHoverEnterNosweat
      {
         private static void Postfix( Image ___nosweatButtonHeader, LocText ___descriptionArea ) {
            ___nosweatButtonHeader.color = new Color( 0.7f, 0.09f, 0.09f, 1f );
            ___descriptionArea.text = NMStrings.NIGHTMARE_MODE_DESC;
         }
      }

      [HarmonyPatch( typeof( ModeSelectScreen ), "OnClickNosweat" )]
      private class OnClickNosweat
      {
         private static bool Prefix( ModeSelectScreen __instance ) {
            Options.Current.Assign( Options.Default );
            Controller.InNightmareFlow = true;
            DoOriginal( __instance );
            return false;
         }

         private static void DoOriginal( ModeSelectScreen inst ) {
            inst.Deactivate();
            //this.LoadWorldsData();
            Traverse.Create( inst ).Method( "LoadWorldsData" ).GetValue( null );
            CustomGameSettings.Instance.SetNosweatDefaults();
            //this.NavigateForward();
            Traverse.Create( inst ).Method( "NavigateForward" ).GetValue( null );
         }
      }
   }
}
