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

using UnityEngine;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace NightmareMode.Core
{
   internal class Controller
   {
      #region Public Properties
      public static bool IsEnforcing { get; set; } = false;
      public static string ModeIconPath { get; private set; }
      public static string OptionsFilePath { get; private set; }
      public static string ModsFilePath { get; private set; }
      public static string SaveFilePath { get; private set; }
      public static System.DateTime LastSaved { get; private set; }
      public static System.TimeSpan ElapsedLastSave { get => System.DateTime.UtcNow - LastSaved; }
      public static bool InNightmareFlow { get; set; } = false;
      public static bool HasLoaded { get; set; } = false;

      #endregion Public Properties

      #region Initialization

      static Controller() {
         string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         ModeIconPath = Path.Combine( folder, "dupe_freakout.png" );
         OptionsFilePath = Path.Combine( folder, "options.json" );
         ModsFilePath = Path.Combine( folder, "known-mods.txt" );
         LastSaved = System.DateTime.UtcNow;
      }

      public static void OnLoad() {
         Log.Out( "Controller.OnLoad()" );

         // Initializing PLib is not necessary in this case. See Note [1].
         // PeterHan.PLib.PUtil.InitLibrary( true );

         if( File.Exists(OptionsFilePath)) { 
            string opts = File.ReadAllText( OptionsFilePath );
            Options.Default = JsonConvert.DeserializeObject<Options>( opts );
         }

         string nightmareSaveFolder = Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), "nightmare");
         if( !Directory.Exists( nightmareSaveFolder ) )
            Directory.CreateDirectory( nightmareSaveFolder );
      }

      #endregion Initialization

      #region Game Lifecycle

      public static void OnLoadGame( string filePath ) {
         Log.Out( "Controller.OnLoadGame({0})", filePath );

         Options.Current = ONISaveGame.GetNightmareHint( filePath );
         IsEnforcing = Options.Current != null;
         Log.Out( "Controller.OnLoadGame(IsEnforcing={0})", IsEnforcing );
         if( !IsEnforcing ) return;

         SaveFilePath = filePath;
         SaveGame.Instance.AutoSaveCycleInterval = 1;
         if( DebugHandler.enabled )
            DebugHandler.SetDebugEnabled( false );
         LastSaved = System.DateTime.Now;
         HasLoaded = true;
      }

      public static bool OnSaveGamePre( ref string filename, ref bool isAutoSave, bool updateSavePointer ) {

         Log.Out( "SaveLoader.GetSavePrefix() = {0}", SaveLoader.GetSavePrefix() );
         Log.Out( "SaveLoader.GetAutoSavePrefix() = {0}", SaveLoader.GetAutoSavePrefix() );
         Log.Out( "SaveLoader.GetActiveSaveFilePath() = {0}", SaveLoader.GetActiveSaveFilePath() );
         Log.Out( "SaveLoader.GetAutosaveFilePath() = {0}", SaveLoader.GetAutosaveFilePath() );
         Log.Out( "SaveLoader.GetActiveSaveFolder() = {0}", SaveLoader.GetActiveSaveFolder() );

         if( SaveGame.Instance == null ) return false;

         Log.Out( "Controller.OnSaveGamePre({0},{1},{2})", filename, isAutoSave, updateSavePointer );

         if( !HasLoaded && Controller.InNightmareFlow == true ) {
            OnNewGame( filename );
         }

         if( !IsEnforcing ) return true;

         if( Options.Current.SingleSaveFile ) {
            isAutoSave = false;
            filename = Path.Combine(
               SaveLoader.GetSavePrefixAndCreateFolder(),
               "nightmare",
               SaveGame.Instance.BaseName + ".sav"
            );
         }
         else if( !string.IsNullOrEmpty(Options.Default.NightmareSaveFolder) ) {
            string fname = Path.GetFileName( filename );
            if(!Path.IsPathRooted(Options.Default.NightmareSaveFolder )) {
               string saveFolder = Path.GetDirectoryName( filename );
               if(!saveFolder.EndsWith(Options.Default.NightmareSaveFolder )) {
                  filename = Path.Combine( saveFolder, Options.Default.NightmareSaveFolder, fname );
               }
               else {
                  filename = Path.Combine( saveFolder, fname );
               }
               
            }
            else {
               filename = Path.Combine( Options.Default.NightmareSaveFolder, fname );
            }
         }

         if( IsNewGame )
            return true;

         return ( System.DateTime.Now - LastSaved > System.TimeSpan.FromSeconds( Options.Current.MaxSaveFreq ) );
      }

      public static bool IsNewGame { get; set; } = false;

      public static void OnSaveGamePost( string filePath ) {
         Log.Out( "Controller.OnSaveGamePost({0})", filePath );
         if(IsEnforcing) { 
            LastSaved = System.DateTime.Now;
            ONISaveGame.SetNightmareHint( filePath, Options.Current );
         }
      }

      private static void OnNewGame( string filename ) {
         Log.Out( "Controller.OnNewGame({0})", filename );
         IsNewGame = true;
         IsEnforcing = true;
         HasLoaded = true;
         SaveFilePath = Path.Combine( SaveLoader.GetSavePrefixAndCreateFolder(),
            "nightmare", SaveGame.Instance.BaseName + ".sav" );
      }

      #endregion Game Lifecycle

      #region Utility

      public static void MessageBox( string msg, string title, System.Action onConfirm, System.Action onCancel, GameObject parent ) =>
         ( (ConfirmDialogScreen) KScreenManager.Instance
            .StartScreen( ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent ) )
            .PopupConfirmDialog( msg, onConfirm, onCancel, null, null, title );

#if false
      public static void MessageBox(string msg, System.Action onConfirm, System.Action onCancel, UnityEngine.GameObject parent)
      {
         var confDlg = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent);
         confDlg.PopupConfirmDialog(msg, onConfirm, onCancel, null, null, title_text: "WARNING!");
         confDlg.gameObject.SetActive(true);
      }
#endif

      #endregion Utility
   }
}

// [1]
// Normally when using PLib, the mod must call InitLibrary:
//
//    PeterHan.PLib.PUtil.InitLibrary( true );
//
// Per https://github.com/peterhaneve/ONIMods/blob/main/PLib/README.md
//
// This is not necessary here because of the thin subset of PLib
// functionality the Nightmare Mode mod uses.
//
