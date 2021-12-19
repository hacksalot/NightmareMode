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

namespace NightmareMode.Core
{
   internal class NMStrings
   {
      public const string SAVE_AS_DISABLED = "Save As is disabled by the Nightmare Mode mod.";
      public const string NIGHTMARE_MODE_DESC = "For veteran players who desire a game of maximum consequence and difficulty.";
      public const string NIGHTMARE_MODE_NAME = "NIGHTMARE";
      public const string NIGHTMARE_MODE_OPTIONS = "Nightmare Mode Options";
      public const string WARN_TOGGLE_NIGHTMARE = "Are you sure you wish to {0}able Nightmare Mode for the current game?";
      public const string WARN_ENTER_SANDBOX = "Sandbox functionality is disabled by Nightmare Mode.";
      public const string LAUNCH_NEW_COLONY_TOOLTIP = "Apply these options and start your colony!";

      public const string OPTIONS_SINGLE_SAVE = "Use a single save file";
      public const string OPTIONS_HIDE_PREVIOUS = "Hide previous saves on load";
      public const string OPTIONS_DISABLE_SAVEAS = "Disable the 'Save As' button";
      public const string OPTIONS_SAVE_ON_EXIT = "Save on exit";
      public const string OPTIONS_SAVE_ON_DEATH = "Save on death";
      public const string OPTIONS_SAVE_ON_THREAT = "Save on threat";
      public const string OPTIONS_CERTIFY = "Certify this game";
      public const string OPTIONS_DISABLE_SANDBOX = "Disable sandbox mode";
      public const string OPTIONS_DISABLE_DEBUG = "Disable debug mode";

      public const string OPTIONS_MODS_PROHIBIT = "Prohibit all mods";
      public const string OPTIONS_MODS_QOL = "Allow QoL mods";
      public const string OPTIONS_MODS_CURATED = "Allow curated mods";
      public const string OPTIONS_MODS_ALL = "Allow all mods";

      public const string LABEL_MOD_POLICY = "Modding Policy";
      public const string LABEL_SAVELOAD_POLICY = "Save/Load Policy";
      public const string LABEL_BASIC_OPTIONS = "Basic Options";

      public const string BEGIN_NIGHTMARE = "Let the nightmare begin";


      public const string OPTIONS_HEADER_MSG = "Default setttings for Nightmare Mode games. You can adjust these here\n or when you begin a new Nightmare Mode colony.";
      public const string OPTIONS_HEADER_FLOW_MSG = "Specify Nightmare Mode settings for this colony...if you dare.";

      public const string WARN_UNKNOWN_MODS = 
@"Nightmare Mode has detected one or more unknown mods.

{0}

Nightmare Mode can not certify your game unless you remove these mods or add them to known-mods.txt.";

      public const string WARN_CHEAT_MODS = 
@"Due to the presence of game-altering mods...

{0}

...your Nightmare Mode difficulty rating will be {1} by a total of:

{2} POINTS

To remove this adjustment, either disable these mods or adjust the values in known-mods.txt in the Nightmare Mode mod folder.";

      public const string SINGLE_SAVE_FILE_TOOLTIP =
@"Enforce the use of a single, most-recent save file for Nightmare
Mode games.

This option limits the player to a single save game containing the
most-recent state of the colony. Only affects Nightmare Mode games.";

      public const string HIDE_EARLIER_SAVES_TOOLTIP =
@"Prevent loading of earlier Nightmare Mode saves.

This option hides earlier save games(for Nightmare Mode colonies)
from the Load menu, preventing the player from loading into an
earlier version of the colony, even if a save game exists.";

      public const string EMBED_FINGERPRINT_TOOLTIP =
@"Embed an identifying 'fingerprint' into Nightmare Mode games.

This option emits a small identifying cookie into the save game
file so that Nightmare Mode save games can be identified by the
Nightmare Mode mod.";

      public const string SAVE_ON_EXIT_TOOLTIP =
@"Automatically save the game when the player quits.

This option causes the game to autosave whenever the player quits or
exits the game.";

      public const string SAVE_ON_DEATH_TOOPLTIP =
@"Automatically save the game when a duplicant dies.

This option causes the game to autosave whenever a duplicant dies. RIP.
If the Single Save File option is enabled, this constitutes permadeath.";



      public const string SAVE_ON_THREAT_TOOLTIP =
@"Automatically save the game when a duplicant is threatened.

This option will cause the game to autosave whenever any
duplicant is threatened with drowning, suffocating, or other
potentially hazardous conditions. If the Single Save File option
is enabled, this constitutes permadeath.";

      public const string PREVENT_SAVEAS_TOOLTIP =
"Disable the 'Save As' button on the Pause menu.";

      public const string MOD_POLICY_PROHIBIT_TOOLTIP =
@"Nightmare Mode will prohibit all mods (except Nightmare Mode itself)
during Nightmare Mode games.

This policy enforces a 'vanilla' play experience.";

      public const string MOD_POLICY_QOL_TOOLTIP =
@"Nightmare Mode will only allow 'quality of life' mods that do not
affect core gameplay or mechanics.";

      public const string MOD_POLICY_CURATED_TOOLTIP =
@"Nightmare Mode will allow mods from a known list of curated mods,
with adjustments to the Nightmare Mode difficulty rating based on
the specific mods chosen.";

      public const string MOD_POLICY_ALLOW_TOOLTIP = 
@"Nightmare Mode will allow all mods during Nightmare Mode sessions.";

      public const string DISABLE_DEBUG_TOOLTIP =
@"Prevents the use of Debug mode in Nightmare sessions.

This option prevents the player from entering Debug mode during
a Nightmare Mode session.";

      public const string DISABLE_SANDBOX_TOOLTIP =
@"Prevents the use of Sandbox mode in Nightmare sessions.

This option prevents the player from entering Sandbox mode during
a Nightmare Mode session.";

      public const string TITLE_UNKNOWN_MODS = "NIGHTMARE MODE - UNKNOWN MODS";
      public const string TITLE_PROHIBITED_MODS = "NIGHTMARE MODE - PROHIBITED MODS";
      public const string MODS_ARE_PROHIBITED =
"One or more mods are enabled, which is prohibited per the current Nightmare Mode ruleset. Either disable these mods, or change the mod policy to something other than 'Prohibit All'.";

      public const string QOL_MODS_ONLY =
@"One or more non-QoL mods are enabled, which is prohibited per the current Nightmare Mode ruleset.

{0}

Either disable these mods, or change the mod policy to something other than 'Allow QoL Mods'.";
   }
}
