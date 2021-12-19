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
using NightmareMode.Core;

namespace NightmareMode.Patches
{
   internal class LoadScreenPatches
   {
      [HarmonyPatch( typeof( LoadScreen ), "GetFilesList" )]
      private class LoadScreen_GetFilesList
      {
         private static void Postfix( LoadScreen __instance, Dictionary<string, List<SaveGameFileDetails>> ___saveFiles )
         {
            foreach( var kv in ___saveFiles )
            {
               Regimen r = ONISaveGame.GetNightmareHint(kv.Value[0].FileName);
               if( r != null && r.HidePreviousSaves )
                  kv.Value.RemoveRange( 1, kv.Value.Count - 1 );
            }
         }
      }
      
#pragma warning disable CS0649 // warning CS0649: Field 'foobar' is never assigned to

      private struct SaveGameFileDetails
      {
         public string BaseName;
         public string FileName;
         public string UniqueID;
         public System.DateTime FileDate;
         public SaveGame.Header FileHeader;
         public SaveGame.GameInfo FileInfo;
      }
   }

#pragma warning restore CS0649
}
