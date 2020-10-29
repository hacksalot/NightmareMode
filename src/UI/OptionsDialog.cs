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

using System.Collections.Generic;
using UnityEngine;
using PeterHan.PLib.UI;
using NightmareMode.Core;

namespace NightmareMode.UI
{
   internal class OptionsDialog
   {
      private static System.Action<string> _onClose;

      public static GameObject Init( Regimen r, GameObject parent, string headerMsg = null, string okBtnText = null, System.Action<string> myOnClose = null, bool showHeader = true ) {
         _clientRules = r;
         if( _rules == null ) _rules = new Regimen();
         _rules.Assign( r );
         _onClose = myOnClose;
         // Create the dialog along with two buttons.
         var pDialog = new PDialog("ModOptions") {
            Title = "Nightmare Mode Settings", Size = new Vector2(320f,200f), SortKey = 150.0f,
            DialogBackColor = PUITuning.Colors.OptionsBackground,
            Parent = parent, DialogClosed = OnClose, MaxSize = new Vector2(640f,480f),
            RoundToNearestEven = true
         }
            .AddButton("ok", okBtnText ?? STRINGS.UI.CONFIRMDIALOG.OK, "Apply these options and start your colony!", PUITuning.Colors.ButtonPinkStyle)
            .AddButton(PDialog.DIALOG_KEY_CLOSE, STRINGS.UI.CONFIRMDIALOG.CANCEL, PUIStrings.TOOLTIP_CANCEL, PUITuning.Colors.ButtonBlueStyle);

         var body = pDialog.Body;
         int CATEGORY_MARGIN = 8;
         var margin = new RectOffset(CATEGORY_MARGIN, CATEGORY_MARGIN, CATEGORY_MARGIN, CATEGORY_MARGIN);
         var baseMargin = new RectOffset(0,0,0,0);
         var smallMargin = new RectOffset(8,20,8,8);

         body.Margin = new RectOffset();

         var scrollBody = new PPanel("ScrollContent") {
            Spacing = 0,
            Direction = PanelDirection.Vertical,
            Alignment = TextAnchor.UpperLeft,
            FlexSize = Vector2.right,
            Margin = baseMargin,
            BackColor = Color.blue
         };

         var basePanel = new PGridPanel("Category_" + "Basic") {
            Margin = baseMargin, BackColor = PUITuning.Colors.OptionsBackground,
            FlexSize = Vector2.right
         };
         basePanel.AddColumn( new GridColumnSpec() );
         basePanel.AddColumn( new GridColumnSpec() );
         basePanel.AddRow( new GridRowSpec() );

         PGridPanel rightPanel, leftPanel;

         PGridPanel topPanel = null;
         if( showHeader ) {
            basePanel.AddRow( new GridRowSpec() );
            topPanel = new PGridPanel( "Category_" + "Top" ) {
               Margin = baseMargin, BackColor = PUITuning.Colors.DialogDarkBackground
            };
            topPanel.AddColumn( new GridColumnSpec( flex: 1.0f ) );
            topPanel.AddRow( new GridRowSpec() );

            topPanel.AddChild( new PLabel( "Label" ) {
               Text = headerMsg ?? "Default setttings for Nightmare Mode games. You can adjust these here\n or when you begin a new Nightmare Mode colony.",
               TextStyle = PUITuning.Fonts.TextLightStyle,
               TextAlignment = TextAnchor.UpperLeft,
               ToolTip = "",
            }, new GridComponentSpec( 0, 0 ) {
               Margin = margin,
               Alignment = TextAnchor.UpperLeft
            } );
         }

         rightPanel = new PGridPanel( "Category_" + "Left" ) {
            Margin = baseMargin, BackColor = PUITuning.Colors.DialogDarkBackground,
            FlexSize = Vector2.right
         };
         leftPanel = new PGridPanel( "Category_" + "Right" ) {
            Margin = baseMargin, BackColor = PUITuning.Colors.OptionsBackground,
            FlexSize = Vector2.right
         };

         leftPanel.AddColumn( new GridColumnSpec( flex: 0.5f ) );
         leftPanel.AddRow( new GridRowSpec() );
         leftPanel.AddRow( new GridRowSpec() );

         var basicPanel = new PGridPanel("Category_" + "Basic") {
            Margin = baseMargin, BackColor = PUITuning.Colors.DialogDarkBackground,
            FlexSize = Vector2.right
         };
         var modsPanel = new PGridPanel("Category_" + "Mods") {
            Margin = baseMargin, BackColor = PUITuning.Colors.DialogDarkBackground,
            FlexSize = Vector2.right
         };
         basicPanel.AddColumn( new GridColumnSpec( flex: 1.0f ) );
         basicPanel.AddRow( new GridRowSpec() );
         basicPanel.AddRow( new GridRowSpec() );
         basicPanel.AddRow( new GridRowSpec() );
         basicPanel.AddRow( new GridRowSpec() );

         modsPanel.AddColumn( new GridColumnSpec( flex: 1.0f ) );
         modsPanel.AddRow( new GridRowSpec() );
         modsPanel.AddRow( new GridRowSpec() );
         modsPanel.AddChild( new PLabel( "Label" ) {
            Text = "Modding Policy", ToolTip = "",
            //TextStyle = PUITuning.Fonts.TextLightStyle
         }, new GridComponentSpec( 0, 0 ) {
            Margin = margin, Alignment = TextAnchor.MiddleLeft
         } );

         rightPanel.AddColumn( new GridColumnSpec( flex: 1.0f ) );
         for( int i = 0; i < 7; i++ )
            rightPanel.AddRow( new GridRowSpec() );

         scrollBody.AddChild( basePanel );
         var marginL = new RectOffset(0,5,0,0);
         var marginR = new RectOffset(5,0,0,0);
         var marginVert = new RectOffset(0,0,10,0);
         var marginZero = new RectOffset(0,5,0,10);

         int row = 0;

         if( showHeader )
            basePanel.AddChild( topPanel, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = marginZero, ColumnSpan = 2 } );
         basePanel.AddChild( leftPanel, new GridComponentSpec( row, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = marginL } );
         basePanel.AddChild( rightPanel, new GridComponentSpec( row, 1 ) { Alignment = TextAnchor.UpperLeft, Margin = marginR } );

         leftPanel.AddChild( basicPanel, new GridComponentSpec( 0, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = baseMargin } );
         leftPanel.AddChild( modsPanel, new GridComponentSpec( 1, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = marginVert } );

         row = 0;

         rightPanel.AddChild( new PLabel( "Label" ) {
            Text = "Save/Load Policy", ToolTip = "",
            //TextStyle = PUITuning.Fonts.TextLightStyle
         }, new GridComponentSpec( row++, 0 ) {
            Margin = margin, Alignment = TextAnchor.MiddleLeft
         } );

         PCheckBox chkSingleSave = CreateCheck( "chkUseSingleSave", "Use a single save file", NMStrings.SINGLE_SAVE_FILE_TOOLTIP, out GameObject goSingleSave, _rules.SingleSaveFile );
         rightPanel.AddChild( chkSingleSave, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         PCheckBox chkHidePrevious = CreateCheck( "chkHidePreviousSaves", "Hide previous saves on load", NMStrings.HIDE_EARLIER_SAVES_TOOLTIP, out GameObject goHidePrevious, _rules.HidePreviousSaves );
         rightPanel.AddChild( chkHidePrevious, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         PCheckBox chkPreventSaveAs = CreateCheck( "chkPreventSaveAs", "Disable the 'Save As' button", NMStrings.PREVENT_SAVEAS_TOOLTIP, out GameObject goPreventSaveAs, _rules.PreventSaveAs );
         rightPanel.AddChild( chkPreventSaveAs, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         PCheckBox chkSaveOnExit = CreateCheck( "chkSaveOnExit", "Save on exit", NMStrings.SAVE_ON_EXIT_TOOLTIP, out GameObject goSaveOnExit, _rules.SaveOnExit );
         rightPanel.AddChild( chkSaveOnExit, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         PCheckBox chkSaveOnDeath = CreateCheck( "chkSaveOnDeath", "Save on death", NMStrings.SAVE_ON_DEATH_TOOPLTIP, out GameObject goSaveOnDeath, _rules.SaveOnDeath );
         rightPanel.AddChild( chkSaveOnDeath, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         PCheckBox chkSaveOnThreat = CreateCheck( "chkSaveOnThreat", "Save on threat", NMStrings.SAVE_ON_THREAT_TOOLTIP, out GameObject goSaveOnThreat, _rules.SaveOnThreat );
         rightPanel.AddChild( chkSaveOnThreat, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         row = 0;

         basicPanel.AddChild( new PLabel( "Label" ) {
            Text = "Basic Options", ToolTip = "",
         }, new GridComponentSpec( row++, 0 ) {
            Margin = margin, Alignment = TextAnchor.MiddleLeft
         } );

         PCheckBox chkCertifyGame = CreateCheck( "chkCertifyGame", "Certify this game", "", out GameObject outObjCert, _rules.Certify );
         basicPanel.AddChild( chkCertifyGame, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         PCheckBox chkDisableSandbox = CreateCheck( "chkDisableSandbox", "Disable Sandbox mode", NMStrings.DISABLE_SANDBOX_TOOLTIP, out GameObject outObj7, _rules.ProhibitSandbox );
         basicPanel.AddChild( chkDisableSandbox, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         PCheckBox chkDisableDebug = CreateCheck( "chkDisableDebug", "Disable Debug mode", NMStrings.DISABLE_DEBUG_TOOLTIP, out GameObject outObj8, _rules.ProhibitDebug );
         basicPanel.AddChild( chkDisableDebug, new GridComponentSpec( row++, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         List<SelectableOption> opts = new List<SelectableOption>();
         opts.Add( new SelectableOption( "Prohibit all mods", NMStrings.MOD_POLICY_PROHIBIT_TOOLTIP, Regimen.ModPolicy.ProhibitAll ) );
         opts.Add( new SelectableOption( "Allow QoL mods", NMStrings.MOD_POLICY_QOL_TOOLTIP, Regimen.ModPolicy.QoL ) );
         opts.Add( new SelectableOption( "Allow curated mods", NMStrings.MOD_POLICY_CURATED_TOOLTIP, Regimen.ModPolicy.Curated ) );
         opts.Add( new SelectableOption( "Allow all mods", NMStrings.MOD_POLICY_ALLOW_TOOLTIP, Regimen.ModPolicy.AllowAll ) );

         SelectableOption sel = opts.Find(i => (Regimen.ModPolicy) i.Value == r.ModHandling);

         PComboBox < SelectableOption> cbo = CreateCombo(opts, sel);
         modsPanel.AddChild( cbo, new GridComponentSpec( 1, 0 ) { Alignment = TextAnchor.UpperLeft, Margin = smallMargin } );

         body.AddChild( new PScrollPane() {
            ScrollHorizontal = false, ScrollVertical = true,
            Child = scrollBody, FlexSize = Vector2.right, TrackSize = 8,
            AlwaysShowHorizontal = false, AlwaysShowVertical = false
         } );
         cur_obj = pDialog.Build();
         var dialog = cur_obj.GetComponent<KScreen>();
         dialog.Activate();
         return cur_obj;
      }

      private static GameObject cur_obj;

      private static PCheckBox CreateCheck( string name, string text, string tooltip, out GameObject checkObj, bool val = true, PUIDelegates.OnChecked func = null ) {
         PCheckBox checkbox = new PCheckBox(name) {
            TextAlignment = TextAnchor.UpperLeft,
            Text = text,
            CheckSize = new Vector2( 16, 16 ),
            InitialState = val ? PCheckBox.STATE_CHECKED : PCheckBox.STATE_UNCHECKED,
            OnChecked = func ?? UpdateCheck, ToolTip = tooltip
         }.SetKleiBlueStyle();
         checkbox.OnRealize += Checkbox_OnRealize;
         checkbox.TextStyle = PUITuning.Fonts.TextLightStyle;
         checkObj = checkbox.Build();
         return checkbox;
      }

      private static void UpdateCheck( GameObject source, int state ) {
         switch( state ) {
            case PCheckBox.STATE_UNCHECKED:
               state = PCheckBox.STATE_CHECKED; break;
            case PCheckBox.STATE_PARTIAL:
               state = PCheckBox.STATE_UNCHECKED; break;
            case PCheckBox.STATE_CHECKED:
               state = PCheckBox.STATE_UNCHECKED; break;
         }
         // The next line the workaround for PCheckBox.GetCheckState not
         // working 100% reliably. Maintain our own state.
         _realChecks [source.name] = state == PCheckBox.STATE_CHECKED;
         PCheckBox.SetCheckState( source, state );
      }

      private static void OnClose( string action ) { // "ok" or "close"
         if( action == "ok" ) {
            UpdateData();
         }
         if( _onClose != null ) _onClose.Invoke( action );
      }

      private static PComboBox<SelectableOption> CreateCombo( IList<SelectableOption> options, SelectableOption selected = null ) {
         SelectableOption firstOption = null;
         int maxLen = 0;
         foreach( var option in options ) {
            int len = option.Title?.Trim()?.Length ?? 0;
            if( firstOption == null && len > 0 )
               firstOption = option;
            if( len > maxLen )
               maxLen = len;
         }
         var comboBox = new PComboBox<SelectableOption>( "Select" ) {
            BackColor = PUITuning.Colors.ButtonPinkStyle, InitialItem = selected ?? firstOption,
            Content = options, EntryColor = PUITuning.Colors.ButtonBlueStyle,
            TextStyle = PUITuning.Fonts.TextLightStyle,
            OnOptionSelected = (GameObject _, SelectableOption sel) => {
               if (selected != null) {
                  _rules.ModHandling = (Regimen.ModPolicy)sel.Value;
                  chosen = sel;
               }
            },
         }.SetMinWidthInCharacters( maxLen );
         var goCombo = comboBox.Build();
         if( comboBox != null && chosen != null )
            PComboBox<SelectableOption>.SetSelectedItem( goCombo, chosen, false );
         return comboBox;
      }

      private static void UpdateData() {
         _rules.Certify = _realChecks ["chkCertifyGame"];
         _rules.SingleSaveFile = _realChecks ["chkUseSingleSave"];
         _rules.HidePreviousSaves = _realChecks ["chkHidePreviousSaves"];
         _rules.SaveOnExit = _realChecks ["chkSaveOnExit"];
         _rules.SaveOnDeath = _realChecks ["chkSaveOnDeath"];
         _rules.SaveOnThreat = _realChecks ["chkSaveOnThreat"];
         _rules.PreventSaveAs = _realChecks ["chkPreventSaveAs"];
         _rules.ProhibitDebug = _realChecks ["chkDisableDebug"];
         _rules.ProhibitSandbox = _realChecks ["chkDisableSandbox"];
         _clientRules.Assign( _rules );
      }

      private static void Checkbox_OnRealize( GameObject realized ) {
         if( !_realChecks.TryGetValue( realized.name, out bool existing ) ) {
            _realChecks.Add( realized.name, true );
         }
      }

      private static Dictionary<string, bool> _realChecks = new Dictionary<string, bool>();
      private static IListableOption chosen;
      private static Regimen _clientRules;
      private static Regimen _rules;
   }
}
