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

using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace NightmareMode.Core
{
   internal class ModChecker
   {
      public class ModDesc
      {
         public string Title { get; set; }
         public int Cost { get; set; }
      }

      public class ModCheckResults
      {
         public Dictionary<string, ModDesc> KnownMods;
         public Dictionary<string, ModDesc> UnknownMods;

         public int NetCost {
            get {
               return KnownMods != null ? KnownMods.Sum( m => m.Value.Cost ) : 0;
            }
         }

         public bool HasUnknown {
            get {
               return UnknownMods != null && UnknownMods.Count > 0;
            }
         }

         public bool HasCheat {
            get {
               return KnownMods.Any( kn => kn.Value.Cost < 0 );
            }
         }
      }

      public static void Check( IEnumerable<string> activeMods, out ModCheckResults res ) {
         string file = Controller.ModsFilePath;
         var mcr = new ModCheckResults
         {
            KnownMods = (from line in File.ReadLines(file, System.Text.Encoding.UTF8)
                         where !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#") &&
                           activeMods.Any(m => line.Substring(0, line.LastIndexOf('=')).Trim().ToLowerInvariant() == m.Trim().ToLowerInvariant())
                         select new ModDesc
                         {
                            Title = line.Substring(0, line.LastIndexOf('=')).Trim(),
                            Cost = int.Parse(line.Substring(line.LastIndexOf('=') + 1).Trim())
                         }).ToDictionary(i => i.Title, i => i),
         };

         mcr.UnknownMods = ( from mod in activeMods
                             where !mcr.KnownMods.Any( line => line.Value.Title.Trim().ToLowerInvariant() == mod.Trim().ToLowerInvariant() )
                             select new ModDesc {
                                Title = mod,
                                Cost = -100
                             } ).ToDictionary( i => i.Title, i => i );
         res = mcr;
      }
   }
}
