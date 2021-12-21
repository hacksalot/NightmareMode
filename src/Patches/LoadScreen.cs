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
using System;
using NightmareMode.Core;

namespace NightmareMode.Patches
{
   internal class LoadScreenPatches
   {
      [HarmonyPatch( typeof( LoadScreen ), "GetColoniesDetails" )]
      private class LoadScreen_GetColoniesDetails
      {
         /// <summary>
         /// Prefix method to modify the list of SaveFileEntry objects passed
         /// in to ONI's LoadScreen.GetColoniesDetails method.
         /// </summary>
         /// <remarks>
         /// ONI's LoadScreen.GetColoniesDetails takes a flat list of "all save
         /// files" and builds a hierarchical dictionary of current save files
         /// organized by colony GUID. This dictionary is then used to populate
         /// the Load Screen UI. We want to adjust this behavior to selectively
         /// omit older save files from the UI, for Nightmare Mode games, when
         /// the "Hide Previous Saves" option is set.
         /// </remarks>
         private static void Prefix( LoadScreen __instance, ref List<SaveLoader.SaveFileEntry> files ) {
            // Group the flat list of save file entries into a hierarchical
            Dictionary<string, List<SaveLoader.SaveFileEntry>> dict = new Dictionary<string, List<SaveLoader.SaveFileEntry>>();
            for( int index = 0; index < files.Count; ++index ) {
               SaveLoader.SaveFileEntry ent = files[index];

               // Get the colony GUID for this save file
               Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(ent.path);
               string sguid = fileInfo.second.colonyGuid.ToString();

               // If this colony GUID isn't present, add a new entry for it, with a corresponding empty list
               if( !dict.ContainsKey( sguid ) ) {
                  dict.Add( sguid, new List<SaveLoader.SaveFileEntry>() );
               }

               dict [sguid].Add( ent );
            }

            // Once we've processed the list of save files into a hierarchical structure,
            // go through and remove all the older save entries
            foreach( KeyValuePair<string, List<SaveLoader.SaveFileEntry>> pair in dict ) {
               Regimen r = null;
               if( Controller.ManagedGames.TryGetValue( pair.Key, out r ) && r.HidePreviousSaves ) {
                  pair.Value.Sort( (Comparison<SaveLoader.SaveFileEntry>) ( ( x, y ) => y.timeStamp.CompareTo( x.timeStamp ) ) );
                  if( pair.Value.Count > 1 ) {
                     pair.Value.RemoveRange( 1, pair.Value.Count - 1 );
                  }
               }
            }

            // Now reassemble the original flat list, minus the older save entries
            files.Clear();
            foreach( KeyValuePair<string, List<SaveLoader.SaveFileEntry>> pair in dict ) {
               files.AddRange( pair.Value );
            }
         }
      }
   }
}
