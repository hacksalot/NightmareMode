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

using System.IO;

namespace NightmareMode.Core
{
   internal class ONISaveGame
   {
      public static bool HasNightmareHint( string filePath ) {
         using( var stream = new FileStream( filePath, FileMode.Open ) )
         using( BinaryReader reader = new BinaryReader( stream ) ) {
            reader.BaseStream.Seek( -24, SeekOrigin.End );
            return reader.ReadUInt32() == 0xFEEDBEEF;
         }
      }

      public static Regimen GetNightmareHint( string filePath ) {
         Regimen r = null;
         using( var stream = new FileStream( filePath, FileMode.Open ) )
         using( BinaryReader reader = new BinaryReader( stream ) ) {
            reader.BaseStream.Seek( -24, SeekOrigin.End );
            if( reader.ReadUInt32() != 0xFEEDBEEF )
               return null;
            int nmVersion = reader.ReadInt32();
            r = new Regimen() {
               ModHandling = (Regimen.ModPolicy) reader.ReadInt32(),
               MaxSaveFreq = reader.ReadInt32(),
               SingleSaveFile = reader.ReadBoolean(),
               HidePreviousSaves = reader.ReadBoolean(),
               SaveOnExit = reader.ReadBoolean(),
               SaveOnDeath = reader.ReadBoolean(),
               SaveOnThreat = reader.ReadBoolean(),
               ProhibitSandbox = reader.ReadBoolean(),
               ProhibitDebug = reader.ReadBoolean(),
               PreventSaveAs = reader.ReadBoolean()
            };
         }
         return r;
      }

      public static void SetNightmareHint( string filePath, Regimen r ) {
         if( !File.Exists( filePath ) ) return;
         using( var stream = new FileStream( filePath, FileMode.Append ) )
         using( BinaryWriter writer = new BinaryWriter( stream ) ) {
            writer.Write( 0xFEEDBEEF );          // 4 bytes Nightmare Mode sentinel value
            writer.Write( 1 );                   // 4 bytes Nightmare Mode version (currently 1)
            writer.Write( (int) r.ModHandling ); // 4 bytes
            writer.Write( r.MaxSaveFreq );       // 4 bytes
            writer.Write( r.SingleSaveFile );    // 1 byte
            writer.Write( r.HidePreviousSaves ); // 1 byte
            writer.Write( r.SaveOnExit );        // 1 byte
            writer.Write( r.SaveOnDeath );       // 1 byte
            writer.Write( r.SaveOnThreat );      // 1 byte
            writer.Write( r.ProhibitSandbox );   // 1 byte
            writer.Write( r.ProhibitDebug );     // 1 byte
            writer.Write( r.PreventSaveAs );     // 1 byte
            writer.Flush();
         }
      }
   }
}
