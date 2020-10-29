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

namespace NightmareMode.Core
{
   internal class Regimen
   {
      public bool Certify { get; set; }
      public ModPolicy ModHandling { get; set; }
      public int MaxSaveFreq { get; set; }
      public bool ProhibitSandbox { get; set; }
      public bool ProhibitDebug { get; set; }
      public bool SaveOnExit { get; set; }
      public bool SaveOnDeath { get; set; }
      public bool SaveOnThreat { get; set; }
      public bool SingleSaveFile { get; set; }
      public bool HidePreviousSaves { get; set; }
      public bool PreventSaveAs { get; set; }

      public Regimen() {
         Certify = true;
         MaxSaveFreq = 15;
         ProhibitSandbox = true;
         ProhibitDebug = true;
         SaveOnDeath = true;
         SaveOnExit = true;
         SaveOnThreat = true;
         SingleSaveFile = true;
         PreventSaveAs = true;
         HidePreviousSaves = true;
         ModHandling = ModPolicy.Curated;
      }
      
      public void Assign( Regimen rhs ) {
         Certify = rhs.Certify;
         MaxSaveFreq = rhs.MaxSaveFreq;
         ProhibitSandbox = rhs.ProhibitSandbox;
         ProhibitDebug = rhs.ProhibitDebug;
         SaveOnExit = rhs.SaveOnExit;
         SaveOnDeath = rhs.SaveOnDeath;
         SaveOnThreat = rhs.SaveOnThreat;
         SingleSaveFile = rhs.SingleSaveFile;
         PreventSaveAs = rhs.PreventSaveAs;
         HidePreviousSaves = rhs.HidePreviousSaves;
         ModHandling = rhs.ModHandling;
      }

      public enum ModPolicy
      {
         AllowAll,
         ProhibitAll,
         Curated,
         QoL
      }
   }
}
