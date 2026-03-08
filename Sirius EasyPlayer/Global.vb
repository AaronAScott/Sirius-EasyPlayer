Option Strict Off
Option Explicit On
Module GlobalModule
	'**********************************************************
	' Public Constants for Visual Basic Programs
	' GLOBAL.VB
	' Written: April 2007
	' Programmer: Aaron Scott
	'
	' Copyright (C) 1999-2007 Sirius Software
	' All Rights Reserved

	' Required modules: none
	'**********************************************************


	''''''''''''''''''''''''''''
	' Visual Basic Public constant file. This file can be loaded
	' into a code module.
	'
	' Some constants are commented out because they have
	' duplicates (e.g., NONE appears several places).
	'
	' If you are updating a Visual Basic application written with
	' an older version, you should replace your Public constants
	' with the constants in this file.
	'
	''''''''''''''''''''''''''''

	' General

	' Clipboard formats
	Public Const CF_LINK As Short = &HBF00S
	Public Const CF_TEXT As Short = 1
	Public Const CF_BITMAP As Short = 2
	Public Const CF_METAFILE As Short = 3
	Public Const CF_DIB As Short = 8
	Public Const CF_PALETTE As Short = 9

	' DragOver
	Public Const ENTER As Short = 0
	Public Const LEAVE As Short = 1
	Public Const OVER As Short = 2

	' Drag (controls)
	Public Const Cancel As Short = 0
	Public Const BEGIN_DRAG As Short = 1
	Public Const END_DRAG As Short = 2

	' Show parameters
	Public Const MODAL As Short = 1
	Public Const MODELESS As Short = 0

	' Arrange Method
	' for MDI Forms
	Public Const CASCADE As Short = 0
	Public Const TILE_HORIZONTAL As Short = 1
	Public Const TILE_VERTICAL As Short = 2
	Public Const ARRANGE_ICONS As Short = 3

	'ZOrder Method
	Public Const BRINGTOFRONT As Short = 0
	Public Const SENDTOBACK As Short = 1

	' Key Codes
	Public Const KEY_LBUTTON As Short = &H1S
	Public Const KEY_RBUTTON As Short = &H2S
	Public Const KEY_CANCEL As Short = &H3S
	Public Const KEY_MBUTTON As Short = &H4S ' NOT contiguous with L & RBUTTON
	Public Const KEY_BACK As Short = &H8S
	Public Const KEY_TAB As Short = &H9S
	Public Const KEY_CLEAR As Short = &HCS
	Public Const KEY_RETURN As Short = &HDS
	Public Const KEY_SHIFT As Short = &H10S
	Public Const KEY_CONTROL As Short = &H11S
	Public Const KEY_MENU As Short = &H12S
	Public Const KEY_PAUSE As Short = &H13S
	Public Const KEY_CAPITAL As Short = &H14S
	Public Const KEY_ESCAPE As Short = &H1BS
	Public Const KEY_SPACE As Short = &H20S
	Public Const KEY_PRIOR As Short = &H21S
	Public Const KEY_NEXT As Short = &H22S
	Public Const KEY_END As Short = &H23S
	Public Const KEY_HOME As Short = &H24S
	Public Const KEY_LEFT As Short = &H25S
	Public Const KEY_UP As Short = &H26S
	Public Const KEY_RIGHT As Short = &H27S
	Public Const KEY_DOWN As Short = &H28S
	Public Const KEY_SELECT As Short = &H29S
	Public Const KEY_PRINT As Short = &H2AS
	'Public Const KEY_EXECUTE = &H2B
	Public Const KEY_SNAPSHOT As Short = &H2CS
	Public Const KEY_INSERT As Short = &H2DS
	Public Const KEY_DELETE As Short = &H2ES
	Public Const KEY_HELP As Short = &H2FS

	' KEY_A thru KEY_Z are the same as their ASCII equivalents: 'A' thru 'Z'
	' KEY_0 thru KEY_9 are the same as their ASCII equivalents: '0' thru '9'

	Public Const KEY_NUMPAD0 As Short = &H60S
	Public Const KEY_NUMPAD1 As Short = &H61S
	Public Const KEY_NUMPAD2 As Short = &H62S
	Public Const KEY_NUMPAD3 As Short = &H63S
	Public Const KEY_NUMPAD4 As Short = &H64S
	Public Const KEY_NUMPAD5 As Short = &H65S
	Public Const KEY_NUMPAD6 As Short = &H66S
	Public Const KEY_NUMPAD7 As Short = &H67S
	Public Const KEY_NUMPAD8 As Short = &H68S
	Public Const KEY_NUMPAD9 As Short = &H69S
	Public Const KEY_MULTIPLY As Short = &H6AS
	Public Const KEY_ADD As Short = &H6BS
	Public Const KEY_SEPARATOR As Short = &H6CS
	Public Const KEY_SUBTRACT As Short = &H6DS
	Public Const KEY_DECIMAL As Short = &H6ES
	Public Const KEY_DIVIDE As Short = &H6FS
	Public Const KEY_F1 As Short = &H70S
	Public Const KEY_F2 As Short = &H71S
	Public Const KEY_F3 As Short = &H72S
	Public Const KEY_F4 As Short = &H73S
	Public Const KEY_F5 As Short = &H74S
	Public Const KEY_F6 As Short = &H75S
	Public Const KEY_F7 As Short = &H76S
	Public Const KEY_F8 As Short = &H77S
	Public Const KEY_F9 As Short = &H78S
	Public Const KEY_F10 As Short = &H79S
	Public Const KEY_F11 As Short = &H7AS
	Public Const KEY_F12 As Short = &H7BS
	Public Const KEY_F13 As Short = &H7CS
	Public Const KEY_F14 As Short = &H7DS
	Public Const KEY_F15 As Short = &H7ES
	Public Const KEY_F16 As Short = &H7FS

	Public Const KEY_NUMLOCK As Short = &H90S

	' Variant VarType tags

	Public Const V_EMPTY As Short = 0
	Public Const V_NULL As Short = 1
	Public Const V_INTEGER As Short = 2
	Public Const V_LONG As Short = 3
	Public Const V_SINGLE As Short = 4
	Public Const V_DOUBLE As Short = 5
	Public Const V_CURRENCY As Short = 6
	Public Const V_DATE As Short = 7
	Public Const V_STRING As Short = 8


	' LOGFONT constants
	Public Const FF_DECORATIVE As Integer = 80
	Public Const FF_MODERN As Integer = 48
	Public Const FF_SWISS As Integer = 32
	Public Const FF_ROMAN As Integer = 16
	Public Const FF_DONTCARE As Integer = 0

	' Event Parameters

	' ErrNum (LinkError)
	Public Const WRONG_FORMAT As Short = 1
	Public Const DDE_SOURCE_CLOSED As Short = 6
	Public Const TOO_MANY_LINKS As Short = 7
	Public Const DATA_TRANSFER_FAILED As Short = 8

	' QueryUnload
	Public Const FORM_CONTROLMENU As Short = 0
	Public Const FORM_CODE As Short = 1
	Public Const APP_WINDOWS As Short = 2
	Public Const APP_TASKMANAGER As Short = 3
	Public Const FORM_MDIFORM As Short = 4

	' Properties

	' Colors
	Public Const BLACK As Integer = &H0
	Public Const Red As Integer = &HFF
	Public Const Green As Integer = &HFF00
	Public Const YELLOW As Integer = &HFFFF
	Public Const Blue As Integer = &HFF0000
	Public Const MAGENTA As Integer = &HFF00FF
	Public Const CYAN As Integer = &HFFFF00
	Public Const WHITE As Integer = &HFFFFFF

	' System Colors
	Public Const SCROLL_BARS As Integer = &H80000000 ' Scroll-bars gray area.
	Public Const DESKTOP As Integer = &H80000001 ' Desktop.
	Public Const ACTIVE_TITLE_BAR As Integer = &H80000002 ' Active window caption.
	Public Const INACTIVE_TITLE_BAR As Integer = &H80000003 ' Inactive window caption.
	Public Const MENU_BAR As Integer = &H80000004 ' Menu background.
	Public Const WINDOW_BACKGROUND As Integer = &H80000005 ' Window background.
	Public Const WINDOW_FRAME As Integer = &H80000006 ' Window frame.
	Public Const MENU_TEXT As Integer = &H80000007 ' Text in menus.
	Public Const WINDOW_TEXT As Integer = &H80000008 ' Text in windows.
	Public Const TITLE_BAR_TEXT As Integer = &H80000009 ' Text in caption, size box, scroll-bar arrow box..
	Public Const ACTIVE_BORDER As Integer = &H8000000A ' Active window border.
	Public Const INACTIVE_BORDER As Integer = &H8000000B ' Inactive window border.
	Public Const APPLICATION_WORKSPACE As Integer = &H8000000C ' Background color of multiple document interface (MDI) applications.
	Public Const HIGHLIGHT As Integer = &H8000000D ' Items selected item in a control.
	Public Const HIGHLIGHT_TEXT As Integer = &H8000000E ' Text of item selected in a control.
	Public Const BUTTON_FACE As Integer = &H8000000F ' Face shading on command buttons.
	Public Const BUTTON_SHADOW As Integer = &H80000010 ' Edge shading on command buttons.
	Public Const GRAY_TEXT As Integer = &H80000011 ' Grayed (disabled) text.  This color is set to 0 if the current display driver does not support a solid gray color.
	Public Const BUTTON_TEXT As Integer = &H80000012 ' Text on push buttons.

	' Enumerated Types

	' Align (picture box)
	Public Const NONE As Short = 0
	Public Const ALIGN_TOP As Short = 1
	Public Const ALIGN_BOTTOM As Short = 2

	' Alignment
	Public Const LEFT_JUSTIFY As Short = 0 ' 0 - Left Justify
	Public Const RIGHT_JUSTIFY As Short = 1 ' 1 - Right Justify
	Public Const CENTER As Short = 2 ' 2 - Center

	' BorderStyle (form)
	'Public Const NONE = 0          ' 0 - None
	Public Const FIXED_SINGLE As Short = 1 ' 1 - Fixed Single
	Public Const SIZABLE As Short = 2 ' 2 - Sizable (Forms only)
	Public Const FIXED_DOUBLE As Short = 3 ' 3 - Fixed Double (Forms only)

	' BorderStyle (Shape and Line)
	'Public Const TRANSPARENT = 0    '0 - Transparent
	'Public Const SOLID = 1          '1 - Solid
	'Public Const DASH = 2         ' 2 - Dash
	'Public Const DOT = 3          ' 3 - Dot
	'Public Const DASH_DOT = 4     ' 4 - Dash-Dot
	'Public Const DASH_DOT_DOT = 5 ' 5 - Dash-Dot-Dot
	'Public Const INSIDE_SOLID = 6 ' 6 - Inside Solid

	' DrawMode
	Public Const BLACKNESS As Short = 1 ' 1 - Blackness
	Public Const NOT_MERGE_PEN As Short = 2 ' 2 - Not Merge Pen
	Public Const MASK_NOT_PEN As Short = 3 ' 3 - Mask Not Pen
	Public Const NOT_COPY_PEN As Short = 4 ' 4 - Not Copy Pen
	Public Const MASK_PEN_NOT As Short = 5 ' 5 - Mask Pen Not
	Public Const INVERT As Short = 6 ' 6 - Invert
	Public Const XOR_PEN As Short = 7 ' 7 - Xor Pen
	Public Const NOT_MASK_PEN As Short = 8 ' 8 - Not Mask Pen
	Public Const MASK_PEN As Short = 9 ' 9 - Mask Pen
	Public Const NOT_XOR_PEN As Short = 10 ' 10 - Not Xor Pen
	Public Const NOP As Short = 11 ' 11 - Nop
	Public Const MERGE_NOT_PEN As Short = 12 ' 12 - Merge Not Pen
	Public Const COPY_PEN As Short = 13 ' 13 - Copy Pen
	Public Const MERGE_PEN_NOT As Short = 14 ' 14 - Merge Pen Not
	Public Const MERGE_PEN As Short = 15 ' 15 - Merge Pen
	Public Const WHITENESS As Short = 16 ' 16 - Whiteness

	' DrawStyle
	Public Const SOLID As Short = 0 ' 0 - Solid
	Public Const DASH As Short = 1 ' 1 - Dash
	Public Const DOT As Short = 2 ' 2 - Dot
	Public Const DASH_DOT As Short = 3 ' 3 - Dash-Dot
	Public Const DASH_DOT_DOT As Short = 4 ' 4 - Dash-Dot-Dot
	Public Const INVISIBLE As Short = 5 ' 5 - Invisible
	Public Const INSIDE_SOLID As Short = 6 ' 6 - Inside Solid

	' FillStyle
	' Public Const SOLID = 0           ' 0 - Solid
	Public Const Transparent As Short = 1 ' 1 - Transparent
	Public Const HORIZONTAL_LINE As Short = 2 ' 2 - Horizontal Line
	Public Const VERTICAL_LINE As Short = 3 ' 3 - Vertical Line
	Public Const UPWARD_DIAGONAL As Short = 4 ' 4 - Upward Diagonal
	Public Const DOWNWARD_DIAGONAL As Short = 5 ' 5 - Downward Diagonal
	Public Const CROSS As Short = 6 ' 6 - Cross
	Public Const DIAGONAL_CROSS As Short = 7 ' 7 - Diagonal Cross

	' LinkMode (forms and controls)
	' Public Const NONE = 0         ' 0 - None
	Public Const LINK_SOURCE As Short = 1 ' 1 - Source (forms only)
	Public Const LINK_AUTOMATIC As Short = 1 ' 1 - Automatic (controls only)
	Public Const LINK_MANUAL As Short = 2 ' 2 - Manual (controls only)
	Public Const LINK_NOTIFY As Short = 3 ' 3 - Notify (controls only)

	' LinkMode (kept for VB1.0 compatibility, use new constants instead)
	Public Const HOT As Short = 1 ' 1 - Hot (controls only)
	Public Const SERVER As Short = 1 ' 1 - Server (forms only)
	Public Const COLD As Short = 2 ' 2 - Cold (controls only)


	' ScaleMode
	Public Const user As Short = 0 ' 0 - User
	Public Const TWIPS As Short = 1 ' 1 - Twip
	Public Const POINTS As Short = 2 ' 2 - Point
	Public Const PIXELS As Short = 3 ' 3 - Pixel
	Public Const CHARACTERS As Short = 4 ' 4 - Character
	Public Const INCHES As Short = 5 ' 5 - Inch
	Public Const MILLIMETERS As Short = 6 ' 6 - Millimeter
	Public Const CENTIMETERS As Short = 7 ' 7 - Centimeter

	' ScrollBar
	' Public Const NONE     = 0 ' 0 - None
	Public Const HORIZONTAL As Short = 1 ' 1 - Horizontal
	Public Const VERTICAL As Short = 2 ' 2 - Vertical
	Public Const BOTH As Short = 3 ' 3 - Both

	' Shape
	Public Const SHAEP_RECTANGLE As Short = 0
	Public Const SHAEP_SQUARE As Short = 1
	Public Const SHAEP_OVAL As Short = 2
	Public Const SHAEP_CIRCLE As Short = 3
	Public Const SHAEP_ROUNDED_RECTANGLE As Short = 4
	Public Const SHAEP_ROUNDED_SQUARE As Short = 5

	' WindowState
	Public Const NORMAL As Short = 0 ' 0 - Normal
	Public Const MINIMIZED As Short = 1 ' 1 - Minimized
	Public Const MAXIMIZED As Short = 2 ' 2 - Maximized

	' Check Value
	Public Const UNCHECKED As Short = 0 ' 0 - Unchecked
	Public Const CHECKED As Short = 1 ' 1 - Checked
	Public Const GRAYED As Short = 2 ' 2 - Grayed

	' Shift parameter masks
	Public Const SHIFT_MASK As Short = 1
	Public Const CTRL_MASK As Short = 2
	Public Const ALT_MASK As Short = 4

	' Button parameter masks
	Public Const LEFT_BUTTON As Short = 1
	Public Const RIGHT_BUTTON As Short = 2
	Public Const MIDDLE_BUTTON As Short = 4

	' Function Parameters
	' MsgBox parameters
	Public Const MB_OK As Short = 0 ' OK button only
	Public Const MB_OKCANCEL As Short = 1 ' OK and Cancel buttons
	Public Const MB_ABORTRETRYIGNORE As Short = 2 ' Abort, Retry, and Ignore buttons
	Public Const MB_YESNOCANCEL As Short = 3 ' Yes, No, and Cancel buttons
	Public Const MB_YESNO As Short = 4 ' Yes and No buttons
	Public Const MB_RETRYCANCEL As Short = 5 ' Retry and Cancel buttons

	Public Const MB_ICONSTOP As Short = 16 ' Critical message
	Public Const MB_ICONQUESTION As Short = 32 ' Warning query
	Public Const MB_ICONEXCLAMATION As Short = 48 ' Warning message
	Public Const MB_ICONINFORMATION As Short = 64 ' Information message

	Public Const MB_APPLMODAL As Short = 0 ' Application Modal Message Box
	Public Const MB_DEFBUTTON1 As Short = 0 ' First button is default
	Public Const MB_DEFBUTTON2 As Short = 256 ' Second button is default
	Public Const MB_DEFBUTTON3 As Short = 512 ' Third button is default
	Public Const MB_SYSTEMMODAL As Short = 4096 'System Modal

	' MsgBox return values
	Public Const IDOK As Short = 1 ' OK button pressed
	Public Const IDCANCEL As Short = 2 ' Cancel button pressed
	Public Const IDABORT As Short = 3 ' Abort button pressed
	Public Const IDRETRY As Short = 4 ' Retry button pressed
	Public Const IDIGNORE As Short = 5 ' Ignore button pressed
	Public Const IDYES As Short = 6 ' Yes button pressed
	Public Const IDNO As Short = 7 ' No button pressed

	' SetAttr, Dir, GetAttr functions
	Public Const ATTR_NORMAL As Short = 0
	Public Const ATTR_READONLY As Short = 1
	Public Const ATTR_HIDDEN As Short = 2
	Public Const ATTR_SYSTEM As Short = 4
	Public Const ATTR_VOLUME As Short = 8
	Public Const ATTR_DIRECTORY As Short = 16
	Public Const ATTR_ARCHIVE As Short = 32

	'Grid
	'ColAlignment,FixedAlignment Properties
	Public Const GRID_ALIGNLEFT As Short = 0
	Public Const GRID_ALIGNRIGHT As Short = 1
	Public Const GRID_ALIGNCENTER As Short = 2

	'Fillstyle Property
	Public Const GRID_SINGLE As Short = 0
	Public Const GRID_REPEAT As Short = 1


	'Data control
	'Error event Response arguments
	Public Const DATA_ERRCONTINUE As Short = 0
	Public Const DATA_ERRDISPLAY As Short = 1

	'Editmode property values
	Public Const DATA_EDITNONE As Short = 0
	Public Const DATA_EDITMODE As Short = 1
	Public Const DATA_EDITADD As Short = 2

	' Options property values
	Public Const DATA_DENYWRITE As Short = &H1S
	Public Const DATA_DENYREAD As Short = &H2S
	Public Const DATA_READONLY As Short = &H4S
	Public Const DATA_APPENDONLY As Short = &H8S
	Public Const DATA_INCONSISTENT As Short = &H10S
	Public Const DATA_CONSISTENT As Short = &H20S
	Public Const DATA_SQLPASSTHROUGH As Short = &H40S

	'Validate event Action arguments
	Public Const DATA_ACTIONCANCEL As Short = 0
	Public Const DATA_ACTIONMOVEFIRST As Short = 1
	Public Const DATA_ACTIONMOVEPREVIOUS As Short = 2
	Public Const DATA_ACTIONMOVENEXT As Short = 3
	Public Const DATA_ACTIONMOVELAST As Short = 4
	Public Const DATA_ACTIONADDNEW As Short = 5
	Public Const DATA_ACTIONUPDATE As Short = 6
	Public Const DATA_ACTIONDELETE As Short = 7
	Public Const DATA_ACTIONFIND As Short = 8
	Public Const DATA_ACTIONBOOKMARK As Short = 9
	Public Const DATA_ACTIONCLOSE As Short = 10
	Public Const DATA_ACTIONUNLOAD As Short = 11


	'OLE Client Control
	'Actions
	Public Const OLE_CREATE_EMBED As Short = 0
	Public Const OLE_CREATE_NEW As Short = 0 'from ole1 control
	Public Const OLE_CREATE_LINK As Short = 1
	Public Const OLE_CREATE_FROM_FILE As Short = 1 'from ole1 control
	Public Const OLE_COPY As Short = 4
	Public Const OLE_PASTE As Short = 5
	Public Const OLE_UPDATE As Short = 6
	Public Const OLE_ACTIVATE As Short = 7
	Public Const OLE_CLOSE As Short = 9
	Public Const OLE_DELETE As Short = 10
	Public Const OLE_SAVE_TO_FILE As Short = 11
	Public Const OLE_READ_FROM_FILE As Short = 12
	Public Const OLE_INSERT_OBJ_DLG As Short = 14
	Public Const OLE_PASTE_SPECIAL_DLG As Short = 15
	Public Const OLE_FETCH_VERBS As Short = 17
	Public Const OLE_SAVE_TO_OLE1FILE As Short = 18

	'OLEType
	Public Const OLE_LINKED As Short = 0
	Public Const OLE_EMBEDDED As Short = 1
	Public Const OLE_NONE As Short = 3

	'OLETypeAllowed
	Public Const OLE_EITHER As Short = 2

	'UpdateOptions
	Public Const OLE_AUTOMATIC As Short = 0
	Public Const OLE_FROZEN As Short = 1
	Public Const OLE_MANUAL As Short = 2

	'AutoActivate modes
	'Note that OLE_ACTIVATE_GETFOCUS only applies to objects that
	'support "inside-out" activation.  See related Verb notes below.
	Public Const OLE_ACTIVATE_MANUAL As Short = 0
	Public Const OLE_ACTIVATE_GETFOCUS As Short = 1
	Public Const OLE_ACTIVATE_DOUBLECLICK As Short = 2

	'SizeModes
	Public Const OLE_SIZE_CLIP As Short = 0
	Public Const OLE_SIZE_STRETCH As Short = 1
	Public Const OLE_SIZE_AUTOSIZE As Short = 2

	'DisplayTypes
	Public Const OLE_DISPLAY_CONTENT As Short = 0
	Public Const OLE_DISPLAY_ICON As Short = 1

	'Update Event Constants
	Public Const OLE_CHANGED As Short = 0
	Public Const OLE_SAVED As Short = 1
	Public Const OLE_CLOSED As Short = 2
	'UPGRADE_NOTE: OLE_RENAMED was upgraded to OLE_RENAMED_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Const OLE_RENAMED_Renamed As Short = 3

	'Special Verb Values
	Public Const VERB_PRIMARY As Short = 0
	Public Const VERB_SHOW As Short = -1
	Public Const VERB_OPEN As Short = -2
	Public Const VERB_HIDE As Short = -3
	Public Const VERB_INPLACEUIACTIVATE As Short = -4
	Public Const VERB_INPLACEACTIVATE As Short = -5
	'The last two verbs are for objects that support "inside-out" activation,
	'meaning they can be edited in-place, and that they support being left
	'in-place-active even when the input focus moves to another control or form.
	'These objects actually have 2 levels of being active.  "InPlace Active"
	'means that the object is ready for the user to click inside it and start
	'working with it.  "In-Place UI-Active" means that, in addition, if the object
	'has any other UI associated with it, such as floating palette windows,
	'that those windows are visible and ready for use.  Any number of objects
	'can be "In-Place Active" at a time, although only one can be
	'"InPlace UI-Active".

	'You can cause an object to move to either one of states programmatically by
	'setting the Verb property to the appropriate verb and setting
	'Action=OLE_ACTIVATE.

	'Also, if you set AutoActivate = OLE_ACTIVATE_GETFOCUS, the server will
	'automatically be put into "InPlace UI-Active" state when the user clicks
	'on or tabs into the control.

	'VerbFlag Bit Masks
	Public Const VERBFLAG_GRAYED As Short = &H1S
	Public Const VERBFLAG_DISABLED As Short = &H2S
	Public Const VERBFLAG_CHECKED As Short = &H8S
	Public Const VERBFLAG_SEPARATOR As Short = &H800S

	'MiscFlag Bits - Or these together as desired for special behaviors

	'MEMSTORAGE causes the control to use memory to store the object while
	'           it is loaded.  This is faster than the default (disk-tempfile),
	'           but can consume a lot of memory for objects whose data takes
	'           up a lot of space, such as the bitmap for a paint program.
	Public Const OLE_MISCFLAG_MEMSTORAGE As Short = &H1S

	'DISABLEINPLACE overrides the control's default behavior of allowing
	'           in-place activation for objects that support it.  If you
	'           are having problems activating an object inplace, you can
	'           force it to always activate in a separate window by setting this
	'           bit
	Public Const OLE_MISCFLAG_DISABLEINPLACE As Short = &H2S

	'Common Dialog Control
	'Action Property
	Public Const DLG_FILE_OPEN As Short = 1
	Public Const DLG_FILE_SAVE As Short = 2
	Public Const DLG_COLOR As Short = 3
	Public Const DLG_FONT As Short = 4
	Public Const DLG_PRINT As Short = 5
	Public Const DLG_HELP As Short = 6

	'File Open/Save Dialog Flags
	Public Const OFN_READONLY As Integer = &H1
	Public Const OFN_OVERWRITEPROMPT As Integer = &H2
	Public Const OFN_HIDEREADONLY As Integer = &H4
	Public Const OFN_NOCHANGEDIR As Integer = &H8
	Public Const OFN_SHOWHELP As Integer = &H10
	Public Const OFN_NOVALIDATE As Integer = &H100
	Public Const OFN_ALLOWMULTISELECT As Integer = &H200
	Public Const OFN_EXTENSIONDIFFERENT As Integer = &H400
	Public Const OFN_PATHMUSTEXIST As Integer = &H800
	Public Const OFN_FILEMUSTEXIST As Integer = &H1000
	Public Const OFN_CREATEPROMPT As Integer = &H2000
	Public Const OFN_SHAREAWARE As Integer = &H4000
	Public Const OFN_NOREADONLYRETURN As Integer = &H8000

	'Color Dialog Flags
	Public Const CC_RGBINIT As Integer = &H1
	Public Const CC_FULLOPEN As Integer = &H2
	Public Const CC_PREVENTFULLOPEN As Integer = &H4
	Public Const CC_SHOWHELP As Integer = &H8

	'Fonts Dialog Flags
	Public Const CF_SCREENFONTS As Integer = &H1
	Public Const CF_PRINTERFONTS As Integer = &H2
	Public Const CF_BOTH As Integer = &H3
	Public Const CF_SHOWHELP As Integer = &H4
	Public Const CF_INITTOLOGFONTSTRUCT As Integer = &H40
	Public Const CF_USESTYLE As Integer = &H80
	Public Const CF_EFFECTS As Integer = &H100
	Public Const CF_APPLY As Integer = &H200
	Public Const CF_ANSIONLY As Integer = &H400
	Public Const CF_NOVECTORFONTS As Integer = &H800
	Public Const CF_NOSIMULATIONS As Integer = &H1000
	Public Const CF_LIMITSIZE As Integer = &H2000
	Public Const CF_FIXEDPITCHONLY As Integer = &H4000
	Public Const CF_WYSIWYG As Integer = &H8000 'must also have CF_SCREENFONTS & CF_PRINTERFONTS
	Public Const CF_FORCEFONTEXIST As Integer = &H10000
	Public Const CF_SCALABLEONLY As Integer = &H20000
	Public Const CF_TTONLY As Integer = &H40000
	Public Const CF_NOFACESEL As Integer = &H80000
	Public Const CF_NOSTYLESEL As Integer = &H100000
	Public Const CF_NOSIZESEL As Integer = &H200000

	'Printer Dialog Flags
	Public Const PD_ALLPAGES As Integer = &H0
	Public Const PD_SELECTION As Integer = &H1
	Public Const PD_PAGENUMS As Integer = &H2
	Public Const PD_NOSELECTION As Integer = &H4
	Public Const PD_NOPAGENUMS As Integer = &H8
	Public Const PD_COLLATE As Integer = &H10
	Public Const PD_PRINTTOFILE As Integer = &H20
	Public Const PD_PRINTSETUP As Integer = &H40
	Public Const PD_NOWARNING As Integer = &H80
	Public Const PD_RETURNDC As Integer = &H100
	Public Const PD_RETURNIC As Integer = &H200
	Public Const PD_RETURNDEFAULT As Integer = &H400
	Public Const PD_SHOWHELP As Integer = &H800
	Public Const PD_USEDEVMODECOPIES As Integer = &H40000
	Public Const PD_DISABLEPRINTTOFILE As Integer = &H80000
	Public Const PD_HIDEPRINTTOFILE As Integer = &H100000

	'Help Constants
	Public Const HELP_CONTEXT As Short = &H1S
	Public Const HELP_QUIT As Short = &H2S
	Public Const HELP_CONTENTS As Integer = &H3
	Public Const HELP_HELPONHELP As Short = &H4S
	Public Const HELP_SETINDEX As Short = &H5S
	Public Const HELP_SETCONTENTS As Integer = &H5
	Public Const HELP_CONTEXTPOPUP As Integer = &H8
	Public Const HELP_FORCEFILE As Integer = &H9
	Public Const HELP_FINDER As Integer = &HB
	Public Const HELP_KEY As Short = &H101S
	Public Const HELP_COMMAND As Integer = &H102
	Public Const HELP_INDEX As Integer = &H105
	Public Const HELP_PARTIALKEY As Integer = &H105
	Public Const HELP_MULTIKEY As Integer = &H201
	Public Const HELP_SETWINPOS As Integer = &H203

	'Error Constants
	Public Const CDERR_DIALOGFAILURE As Short = -32768

	Public Const CDERR_GENERALCODES As Short = &H7FFFS
	Public Const CDERR_STRUCTSIZE As Short = &H7FFES
	Public Const CDERR_INITIALIZATION As Short = &H7FFDS
	Public Const CDERR_NOTEMPLATE As Short = &H7FFCS
	Public Const CDERR_NOHINSTANCE As Short = &H7FFBS
	Public Const CDERR_LOADSTRFAILURE As Short = &H7FFAS
	Public Const CDERR_FINDRESFAILURE As Short = &H7FF9S
	Public Const CDERR_LOADRESFAILURE As Short = &H7FF8S
	Public Const CDERR_LOCKRESFAILURE As Short = &H7FF7S
	Public Const CDERR_MEMALLOCFAILURE As Short = &H7FF6S
	Public Const CDERR_MEMLOCKFAILURE As Short = &H7FF5S
	Public Const CDERR_NOHOOK As Short = &H7FF4S

	'Added for CMDIALOG.VBX
	Public Const CDERR_CANCEL As Short = &H7FF3S
	Public Const CDERR_NODLL As Short = &H7FF2S
	Public Const CDERR_ERRPROC As Short = &H7FF1S
	Public Const CDERR_ALLOC As Short = &H7FF0S
	Public Const CDERR_HELP As Short = &H7FEFS

	Public Const PDERR_PRINTERCODES As Short = &H6FFFS
	Public Const PDERR_SETUPFAILURE As Short = &H6FFES
	Public Const PDERR_PARSEFAILURE As Short = &H6FFDS
	Public Const PDERR_RETDEFFAILURE As Short = &H6FFCS
	Public Const PDERR_LOADDRVFAILURE As Short = &H6FFBS
	Public Const PDERR_GETDEVMODEFAIL As Short = &H6FFAS
	Public Const PDERR_INITFAILURE As Short = &H6FF9S
	Public Const PDERR_NODEVICES As Short = &H6FF8S
	Public Const PDERR_NODEFAULTPRN As Short = &H6FF7S
	Public Const PDERR_DNDMMISMATCH As Short = &H6FF6S
	Public Const PDERR_CREATEICFAILURE As Short = &H6FF5S
	Public Const PDERR_PRINTERNOTFOUND As Short = &H6FF4S

	Public Const CFERR_CHOOSEFONTCODES As Short = &H5FFFS
	Public Const CFERR_NOFONTS As Short = &H5FFES

	Public Const FNERR_FILENAMECODES As Short = &H4FFFS
	Public Const FNERR_SUBCLASSFAILURE As Short = &H4FFES
	Public Const FNERR_INVALIDFILENAME As Short = &H4FFDS
	Public Const FNERR_BUFFERTOOSMALL As Short = &H4FFCS

	Public Const FRERR_FINDREPLACECODES As Short = &H3FFFS
	Public Const CCERR_CHOOSECOLORCODES As Short = &H2FFFS


	'---------------------------------------------------------
	'      Table of Contents for Visual Basic Professional
	'
	'       1.  3-D Controls
	'           (Frame/Panel/Option/Check/Command/Group Push)
	'       2.  Animated Button
	'       3.  Gauge Control
	'       4.  Graph Control Section
	'       5.  Key Status Control
	'       6.  Spin Button
	'       7.  MCI Control (Multimedia)
	'       8.  Masked Edit Control
	'       9.  Comm Control
	'       10. Outline Control
	'---------------------------------------------------------


	'-------------------------------------------------------------------
	'3D Controls
	'-------------------------------------------------------------------
	'Alignment (Check Box)
	Public Const SSCB_TEXT_RIGHT As Short = 0 '0 - Text to the right
	Public Const SSCB_TEXT_LEFT As Short = 1 '1 - Text to the left

	'Alignment (Option Button)
	Public Const SSOB_TEXT_RIGHT As Short = 0 '0 - Text to the right
	Public Const SSOB_TEXT_LEFT As Short = 1 '1 - Text to the left

	'Alignment (Frame)
	Public Const SSFR_LEFT_JUSTIFY As Short = 0 '0 - Left justify text
	Public Const SSFR_RIGHT_JUSTIFY As Short = 1 '1 - Right justify text
	Public Const SSFR_CENTER As Short = 2 '2 - Center text

	'Alignment (Panel)
	Public Const SSPN_LEFT_TOP As Short = 0 '0 - Text to left and top
	Public Const SSPN_LEFT_MIDDLE As Short = 1 '1 - Text to left and middle
	Public Const SSPN_LEFT_BOTTOM As Short = 2 '2 - Text to left and bottom
	Public Const SSPN_RIGHT_TOP As Short = 3 '3 - Text to right and top
	Public Const SSPN_RIGHT_MIDDLE As Short = 4 '4 - Text to right and middle
	Public Const SSPN_RIGHT_BOTTOM As Short = 5 '5 - Text to right and bottom
	Public Const SSPN_CENTER_TOP As Short = 6 '6 - Text to center and top
	Public Const SSPN_CENTER_MIDDLE As Short = 7 '7 - Text to center and middle
	Public Const SSPN_CENTER_BOTTOM As Short = 8 '8 - Text to center and bottom

	'Autosize (Command Button)
	Public Const SS_AUTOSIZE_NONE As Short = 0 '0 - No Autosizing
	Public Const SSPB_AUTOSIZE_PICTOBUT As Short = 1 '0 - Autosize Picture to Button
	Public Const SSPB_AUTOSIZE_BUTTOPIC As Short = 2 '0 - Autosize Button to Picture

	'Autosize (Ribbon Button)
	'Public Const SS_AUTOSIZE_NONE      = 0  '0 - No Autosizing
	Public Const SSRI_AUTOSIZE_PICTOBUT As Short = 1 '0 - Autosize Picture to Button
	Public Const SSRI_AUTOSIZE_BUTTOPIC As Short = 2 '0 - Autosize Button to Picture

	'Autosize (Panel)
	'Public Const SS_AUTOSIZE_NONE    = 0    '0 - No Autosizing
	Public Const SSPN_AUTOSIZE_WIDTH As Short = 1 '1 - Autosize Panel width to Caption
	Public Const SSPN_AUTOSIZE_HEIGHT As Short = 2 '2 - Autosize Panel height to Caption
	Public Const SSPN_AUTOSIZE_CHILD As Short = 3 '3 - Autosize Child to Panel

	'BevelInner (Panel)
	Public Const SS_BEVELINNER_NONE As Short = 0 '0 - No Inner Bevel
	Public Const SS_BEVELINNER_INSET As Short = 1 '1 - Inset Inner Bevel
	Public Const SS_BEVELINNER_RAISED As Short = 2 '2 - Raised Inner Bevel

	'BevelOuter (Panel)
	Public Const SS_BEVELOUTER_NONE As Short = 0 '0 - No Outer Bevel
	Public Const SS_BEVELOUTER_INSET As Short = 1 '1 - Inset Outer Bevel
	Public Const SS_BEVELOUTER_RAISED As Short = 2 '2 - Raised Outer Bevel

	'FloodType (Panel)
	Public Const SS_FLOODTYEP_NONE As Short = 0 '0 - No flood
	Public Const SS_FLOODTYEP_L_TO_R As Short = 1 '1 - Left to light
	Public Const SS_FLOODTYEP_R_TO_L As Short = 2 '2 - Right to left
	Public Const SS_FLOODTYEP_T_TO_B As Short = 3 '3 - Top to bottom
	Public Const SS_FLOODTYEP_B_TO_T As Short = 4 '4 - Bottom to top
	Public Const SS_FLOODTYEP_CIRCLE As Short = 5 '5 - Widening circle

	'Font3D (Panel, Command Button, Option Button, Check Box, Frame)
	Public Const SS_FONT3D_NONE As Short = 0 '0 - No 3-D text
	Public Const SS_FONT3D_RAISED_LIGHT As Short = 1 '1 - Raised with light shading
	Public Const SS_FONT3D_RAISED_HEAVY As Short = 2 '2 - Raised with heavy shading
	Public Const SS_FONT3D_INSET_LIGHT As Short = 3 '3 - Inset with light shading
	Public Const SS_FONT3D_INSET_HEAVY As Short = 4 '4 - Inset with heavy shading

	'PictureDnChange (Ribbon Button)
	Public Const SS_PICDN_NOCHANGE As Short = 0 '0 - Use 'Up'bitmap with no change
	Public Const SS_PICDN_DITHER As Short = 1 '1 - Dither 'Up'bitmap
	Public Const SS_PICDN_INVERT As Short = 2 '2 - Invert 'Up'bitmap

	'ShadowColor (Panel, Frame)
	Public Const SS_SHADOW_DARKGREY As Short = 0 '0 - Dark grey shadow
	Public Const SS_SHADOW_BLACK As Short = 1 '1 - Black shadow

	'ShadowStyle (Frame)
	Public Const SS_SHADOW_INSET As Short = 0 '0 - Shadow inset
	Public Const SS_SHADOW_RAISED As Short = 1 '1 - Shadow raised


	'---------------------------------------
	'Animated Button
	'---------------------------------------
	'Cycle property
	Public Const ANI_ANIMATED As Short = 0
	Public Const ANI_MULTISTATE As Short = 1
	Public Const ANI_TWO_STATE As Short = 2

	'Click Filter property
	Public Const ANI_ANYWHERE As Short = 0
	Public Const ANI_IMAGE_AND_TEXT As Short = 1
	Public Const ANI_IMAGE As Short = 2
	Public Const ANI_TEXT As Short = 3

	'PicDrawMode Property
	Public Const ANI_XPOS_YPOS As Short = 0
	Public Const ANI_AUTOSIZE As Short = 1
	Public Const ANI_STRETCH As Short = 2

	'SpecialOp Property
	Public Const ANI_CLICK As Short = 1

	'TextPosition Property
	Public Const ANI_CENTER As Short = 0
	Public Const ANI_LEFT As Short = 1
	Public Const ANI_RIGHT As Short = 2
	Public Const ANI_BOTTON As Short = 3
	Public Const ANI_TOP As Short = 4


	'---------------------------------------
	'GAUGE
	'---------------------------------------
	'Style Property
	Public Const GAUGE_HORIZ As Short = 0
	Public Const GAUGE_VERT As Short = 1
	Public Const GAUGE_SEMI As Short = 2
	Public Const GAUGE_FULL As Short = 3


	'----------------------------------------
	'Graph Control
	'----------------------------------------
	'General
	Public Const G_NONE As Short = 0
	Public Const G_DEFAULT As Short = 0

	Public Const G_OFF As Short = 0
	Public Const G_ON As Short = 1

	Public Const G_MONO As Short = 0
	Public Const G_COLOR As Short = 1

	'Graph Types
	Public Const G_PIE2D As Short = 1
	Public Const G_PIE3D As Short = 2
	Public Const G_BAR2D As Short = 3
	Public Const G_BAR3D As Short = 4
	Public Const G_GANTT As Short = 5
	Public Const G_LINE As Short = 6
	Public Const G_LOGLIN As Short = 7
	Public Const G_AREA As Short = 8
	Public Const G_SCATTER As Short = 9
	Public Const G_POLAR As Short = 10
	Public Const G_HLC As Short = 11

	'Colors
	Public Const G_BLACK As Short = 0
	Public Const G_BLUE As Short = 1
	Public Const G_GREEN As Short = 2
	Public Const G_CYAN As Short = 3
	Public Const G_RED As Short = 4
	Public Const G_MAGENTA As Short = 5
	Public Const G_BROWN As Short = 6
	Public Const G_LIGHT_GRAY As Short = 7
	Public Const G_DARK_GRAY As Short = 8
	Public Const G_LIGHT_BLUE As Short = 9
	Public Const G_LIGHT_GREEN As Short = 10
	Public Const G_LIGHT_CYAN As Short = 11
	Public Const G_LIGHT_RED As Short = 12
	Public Const G_LIGHT_MAGENTA As Short = 13
	Public Const G_YELLOW As Short = 14
	Public Const G_WHITE As Short = 15
	Public Const G_AUTOBW As Short = 16

	'Patterns
	Public Const G_SOLID As Short = 0
	Public Const G_HOLLOW As Short = 1
	Public Const G_HATCH1 As Short = 2
	Public Const G_HATCH2 As Short = 3
	Public Const G_HATCH3 As Short = 4
	Public Const G_HATCH4 As Short = 5
	Public Const G_HATCH5 As Short = 6
	Public Const G_HATCH6 As Short = 7
	Public Const G_BITMAP1 As Short = 16
	Public Const G_BITMAP2 As Short = 17
	Public Const G_BITMAP3 As Short = 18
	Public Const G_BITMAP4 As Short = 19
	Public Const G_BITMAP5 As Short = 20
	Public Const G_BITMAP6 As Short = 21
	Public Const G_BITMAP7 As Short = 22
	Public Const G_BITMAP8 As Short = 23
	Public Const G_BITMAP9 As Short = 24
	Public Const G_BITMAP10 As Short = 25
	Public Const G_BITMAP11 As Short = 26
	Public Const G_BITMAP12 As Short = 27
	Public Const G_BITMAP13 As Short = 28
	Public Const G_BITMAP14 As Short = 29
	Public Const G_BITMAP15 As Short = 30
	Public Const G_BITMAP16 As Short = 31

	'Symbols
	Public Const G_CROSS_PLUS As Short = 0
	Public Const G_CROSS_TIMES As Short = 1
	Public Const G_TRIANGLE_UP As Short = 2
	Public Const G_SOLID_TRIANGLE_UP As Short = 3
	Public Const G_TRIANGLE_DOWN As Short = 4
	Public Const G_SOLID_TRIANGLE_DOWN As Short = 5
	Public Const G_SQUARE As Short = 6
	Public Const G_SOLID_SQUARE As Short = 7
	Public Const G_DIAMOND As Short = 8
	Public Const G_SOLID_DIAMOND As Short = 9

	'Line Styles
	'Public Const G_SOLID = 0
	Public Const G_DASH As Short = 1
	Public Const G_DOT As Short = 2
	Public Const G_DASHDOT As Short = 3
	Public Const G_DASHDOTDOT As Short = 4

	'Grids
	Public Const G_HORIZONTAL As Short = 1
	Public Const G_VERTICAL As Short = 2

	'Statistics
	Public Const G_MEAN As Short = 1
	Public Const G_MIN_MAX As Short = 2
	Public Const G_STD_DEV As Short = 4
	Public Const G_BEST_FIT As Short = 8

	'Data Arrays
	Public Const G_GRAPH_DATA As Short = 1
	Public Const G_COLOR_DATA As Short = 2
	Public Const G_EXTRA_DATA As Short = 3
	Public Const G_LABEL_TEXT As Short = 4
	Public Const G_LEGEND_TEXT As Short = 5
	Public Const G_PATTERN_DATA As Short = 6
	Public Const G_SYMBOL_DATA As Short = 7
	Public Const G_XPOS_DATA As Short = 8
	Public Const G_ALL_DATA As Short = 9

	'Draw Mode
	Public Const G_NO_ACTION As Short = 0
	Public Const G_CLEAR As Short = 1
	Public Const G_DRAW As Short = 2
	Public Const G_BLIT As Short = 3
	Public Const G_COPY As Short = 4
	Public Const G_PRINT As Short = 5
	Public Const G_WRITE As Short = 6

	'Print Options
	Public Const G_BORDER As Short = 2

	'Pie Chart Options             '
	Public Const G_NO_LINES As Short = 1
	Public Const G_COLORED As Short = 2
	Public Const G_PERCENTS As Short = 4

	'Bar Chart Options             '
	'Public Const G_HORIZONTAL = 1
	Public Const G_STACKED As Short = 2
	Public Const G_PERCENTAGE As Short = 4
	Public Const G_Z_CLUSTERED As Short = 6

	'Gantt Chart Options           '
	Public Const G_SPACED_BARS As Short = 1

	'Line/Polar Chart Options      '
	Public Const G_SYMBOLS As Short = 1
	Public Const G_STICKS As Short = 2
	Public Const G_LINES As Short = 4

	'Area Chart Options            '
	Public Const G_ABSOLUTE As Short = 1
	Public Const G_PERCENT As Short = 2

	'HLC Chart Options             '
	Public Const G_NO_CLOSE As Short = 1
	Public Const G_NO_HIGH_LOW As Short = 2


	'---------------------------------------
	'Key Status Control
	'---------------------------------------
	'Style
	Public Const KEYSTAT_CAPSLOCK As Short = 0
	Public Const KEYSTAT_NUMLOCK As Short = 1
	Public Const KEYSTAT_INSERT As Short = 2
	Public Const KEYSTAT_SCROLLLOCK As Short = 3


	'---------------------------------------
	'MCI Control (Multimedia)
	'---------------------------------------
	'NOTE:
	'Please use the updated Multimedia constants
	'in the WINMMSYS.TXT file from the \VB\WINAPI
	'subdirectory.

	'Mode Property
	'Public Const MCI_MODE_NOT_OPEN = 11
	'Public Const MCI_MODE_STOP = 12
	'Public Const MCI_MODE_PLAY = 13
	'Public Const MCI_MODE_RECORD = 14
	'Public Const MCI_MODE_SEEK = 15
	'Public Const MCI_MODE_PAUSE = 16
	'Public Const MCI_MODE_READY = 17

	'NotifyValue Property
	'Public Const MCI_NOTIFY_SUCCESSFUL = 1
	'Public Const MCI_NOTIFY_SUPERSEDED = 2
	'Public Const MCI_ABORTED = 4
	'Public Const MCI_FAILURE = 8

	'Orientation Property
	'Public Const MCI_ORIENT_HORZ = 0
	'Public Const MCI_ORIENT_VERT = 1

	'RecordMode Porperty
	'Public Const MCI_RECORD_INSERT = 0
	'Public Const MCI_RECORD_OVERWRITE = 1

	'TimeFormat Property
	'Public Const MCI_FORMAT_MILLISECONDS = 0
	'Public Const MCI_FORMAT_HMS = 1
	'Public Const MCI_FORMAT_MSF = 2
	'Public Const MCI_FORMAT_FRAMES = 3
	'Public Const MCI_FORMAT_SMPTE_24 = 4
	'Public Const MCI_FORMAT_SMPTE_25 = 5
	'Public Const MCI_FORMAT_SMPTE_30 = 6
	'Public Const MCI_FORMAT_SMPTE_30DROP = 7
	'Public Const MCI_FORMAT_BYTES = 8
	'Public Const MCI_FORMAT_SAMPLES = 9
	'Public Const MCI_FORMAT_TMSF = 10


	'---------------------------------------
	'Spin Button
	'---------------------------------------
	'SpinOrientation
	Public Const SPIN_VERTICAL As Short = 0
	Public Const SPIN_HORIZONTAL As Short = 1


	'---------------------------------------
	'Masked Edit Control
	'---------------------------------------
	'ClipMode
	Public Const ME_INCLIT As Short = 0
	Public Const ME_EXCLIT As Short = 1


	'---------------------------------------
	'Comm Control
	'---------------------------------------
	'Handshaking
	Public Const MSCOMM_HANDSHAKE_NONE As Short = 0
	Public Const MSCOMM_HANDSHAKE_XONXOFF As Short = 1
	Public Const MSCOMM_HANDSHAKE_RTS As Short = 2
	Public Const MSCOMM_HANDSHAKE_RTSXONXOFF As Short = 3

	'Event constants
	Public Const MSCOMM_EV_SEND As Short = 1
	Public Const MSCOMM_EV_RECEIVE As Short = 2
	Public Const MSCOMM_EV_CTS As Short = 3
	Public Const MSCOMM_EV_DSR As Short = 4
	Public Const MSCOMM_EV_CD As Short = 5
	Public Const MSCOMM_EV_RING As Short = 6
	Public Const MSCOMM_EV_EOF As Short = 7

	'Error code constants
	Public Const MSCOMM_ER_BREAK As Short = 1001
	Public Const MSCOMM_ER_CTSTO As Short = 1002
	Public Const MSCOMM_ER_DSRTO As Short = 1003
	Public Const MSCOMM_ER_FRAME As Short = 1004
	Public Const MSCOMM_ER_OVERRUN As Short = 1006
	Public Const MSCOMM_ER_CDTO As Short = 1007
	Public Const MSCOMM_ER_RXOVER As Short = 1008
	Public Const MSCOMM_ER_RXPARITY As Short = 1009
	Public Const MSCOMM_ER_TXFULL As Short = 1010


	'---------------------------------------
	' MAPI SESSION CONTROL CONSTANTS
	'---------------------------------------
	'Action
	Public Const SESSION_SIGNON As Short = 1
	Public Const SESSION_SIGNOFF As Short = 2


	'---------------------------------------
	' MAPI MESSAGE CONTROL CONSTANTS
	'---------------------------------------
	'Action
	Public Const MESSAGE_FETCH As Short = 1 ' Load all messages from message store
	Public Const MESSAGE_SENDDLG As Short = 2 ' Send mail bring up default mapi dialog
	Public Const MESSAGE_SEND As Short = 3 ' Send mail without default mapi dialog
	Public Const MESSAGE_SAVEMSG As Short = 4 ' Save message in the compose buffer
	Public Const MESSAGE_COPY As Short = 5 ' Copy current message to compose buffer
	Public Const MESSAGE_COMPOSE As Short = 6 ' Initialize compose buffer (previous
	' data is lost
	Public Const MESSAGE_REPLY As Short = 7 ' Fill Compose buffer as REPLY
	Public Const MESSAGE_REPLYALL As Short = 8 ' Fill Compose buffer as REPLY ALL
	Public Const MESSAGE_FORWARD As Short = 9 ' Fill Compose buffer as FORWARD
	Public Const MESSAGE_DELETE As Short = 10 ' Delete current message
	Public Const MESSAGE_SHOWADBOOK As Short = 11 ' Show Address book
	Public Const MESSAGE_SHOWDETAILS As Short = 12 ' Show details of the current recipient
	Public Const MESSAGE_RESOLVENAME As Short = 13 ' Resolve the display name of the recipient
	Public Const RECIPIENT_DELETE As Short = 14 ' Fill Compose buffer as FORWARD
	Public Const ATTACHMENT_DELETE As Short = 15 ' Delete current message


	'---------------------------------------
	'  ERROR CONSTANT DECLARATIONS (MAPI CONTROLS)
	'---------------------------------------
	Public Const SUCCESS_SUCCESS As Short = 32000
	Public Const MAPI_USER_ABORT As Short = 32001
	Public Const MAPI_E_FAILURE As Short = 32002
	Public Const MAPI_E_LOGIN_FAILURE As Short = 32003
	Public Const MAPI_E_DISK_FULL As Short = 32004
	Public Const MAPI_E_INSUFFICIENT_MEMORY As Short = 32005
	Public Const MAPI_E_ACCESS_DENIED As Short = 32006
	Public Const MAPI_E_TOO_MANY_SESSIONS As Short = 32008
	Public Const MAPI_E_TOO_MANY_FILES As Short = 32009
	Public Const MAPI_E_TOO_MANY_RECIPIENTS As Short = 32010
	Public Const MAPI_E_ATTACHMENT_NOT_FOUND As Short = 32011
	Public Const MAPI_E_ATTACHMENT_OPEN_FAILURE As Short = 32012
	Public Const MAPI_E_ATTACHMENT_WRITE_FAILURE As Short = 32013
	Public Const MAPI_E_UNKNOWN_RECIPIENT As Short = 32014
	Public Const MAPI_E_BAD_RECIPTYPE As Short = 32015
	Public Const MAPI_E_NO_MESSAGES As Short = 32016
	Public Const MAPI_E_INVALID_MESSAGE As Short = 32017
	Public Const MAPI_E_TEXT_TOO_LARGE As Short = 32018
	Public Const MAPI_E_INVALID_SESSION As Short = 32019
	Public Const MAPI_E_TYEP_NOT_SUPPORTED As Short = 32020
	Public Const MAPI_E_AMBIGUOUS_RECIPIENT As Short = 32021
	Public Const MAPI_E_MESSAGE_IN_USE As Short = 32022
	Public Const MAPI_E_NETWORK_FAILURE As Short = 32023
	Public Const MAPI_E_INVALID_EDITFIELDS As Short = 32024
	Public Const MAPI_E_INVALID_RECIPS As Short = 32026
	Public Const MAPI_E_NOT_SUPPORTED As Short = 32026

	Public Const CONTROL_E_SESSION_EXISTS As Short = 32050
	Public Const CONTROL_E_INVALID_BUFFER As Short = 32051
	Public Const CONTROL_E_INVALID_READ_BUFFER_ACTION As Short = 32052
	Public Const CONTROL_E_NO_SESSION As Short = 32053
	Public Const CONTROL_E_INVALID_RECIPIENT As Short = 32054
	Public Const CONTROL_E_INVALID_COMPOSE_BUFFER_ACTION As Short = 32055
	Public Const CONTROL_E_FAILURE As Short = 32056
	Public Const CONTROL_E_NO_RECIPIENTS As Short = 32057
	Public Const CONTROL_E_NO_ATTACHMENTS As Short = 32058


	'---------------------------------------
	'  MISCELLANEOUS Public CONSTANT DECLARATIONS (MAPI CONTROLS)
	'---------------------------------------
	Public Const RECIPTYEP_ORIG As Short = 0
	Public Const RECIPTYEP_TO As Short = 1
	Public Const RECIPTYEP_CC As Short = 2
	Public Const RECIPTYEP_BCC As Short = 3

	Public Const ATTACHTYEP_DATA As Short = 0
	Public Const ATTACHTYEP_EOLE As Short = 1
	Public Const ATTACHTYEP_SOLE As Short = 2


	'-------------------------------------------------
	'  Outline
	'-------------------------------------------------
	' PictureType
	Public Const MSOUTLINE_PICTURE_CLOSED As Short = 0
	Public Const MSOUTLINE_PICTURE_OPEN As Short = 1
	Public Const MSOUTLINE_PICTURE_LEAF As Short = 2

	'Outline Control Error Constants
	Public Const MSOUTLINE_BADPICFORMAT As Short = 32000
	Public Const MSOUTLINE_BADINDENTATION As Short = 32001
	Public Const MSOUTLINE_MEM As Short = 32002
	Public Const MSOUTLINE_PARENTNOTEXPANDED As Short = 32003


	'
	' Data Access constants
	'

	' Option argument values (OpenRecordset, etc)
	Public Const DB_DENYWRITE As Short = &H1S
	Public Const DB_DENYREAD As Short = &H2S
	Public Const DB_READONLY As Short = &H4S
	Public Const DB_APPENDONLY As Short = &H8S
	Public Const DB_INCONSISTENT As Short = &H10S
	Public Const DB_CONSISTENT As Short = &H20S
	Public Const DB_SQLPASSTHROUGH As Short = &H40S

	' SetDataAccessOption
	Public Const DB_OPTIONINIPATH As Short = 1

	' Field Attributes
	Public Const DB_FIXEDFIELD As Short = &H1S
	Public Const DB_VARIABLEFIELD As Short = &H2S
	Public Const DB_AUTOINCRFIELD As Short = &H10S
	Public Const DB_UPDATABLEFIELD As Short = &H20S

	' Field Data Types
	Public Const DB_BOOLEAN As Short = 1
	Public Const DB_BYTE As Short = 2
	Public Const DB_INTEGER As Short = 3
	Public Const DB_LONG As Short = 4
	Public Const DB_CURRENCY As Short = 5
	Public Const DB_SINGLE As Short = 6
	Public Const DB_DOUBLE As Short = 7
	Public Const DB_DATE As Short = 8
	Public Const DB_TEXT As Short = 10
	Public Const DB_LONGBINARY As Short = 11
	Public Const DB_MEMO As Short = 12

	' TableDef Attributes
	Public Const DB_ATTACHEXCLUSIVE As Integer = &H10000
	Public Const DB_ATTACHSAVEPWD As Integer = &H20000
	Public Const DB_SYSTEMOBJECT As Integer = &H80000002
	Public Const DB_ATTACHEDTABLE As Integer = &H40000000
	Public Const DB_ATTACHEDODBC As Integer = &H20000000

	' ListTables TableType
	Public Const DB_TABLE As Short = 1
	Public Const DB_QUERYDEF As Short = 5

	' ListTables Attributes (for QueryDefs)
	Public Const DB_QACTION As Short = &HF0S
	Public Const DB_QCROSSTAB As Short = &H10S
	Public Const DB_QDELETE As Short = &H20S
	Public Const DB_QUPDATE As Short = &H30S
	Public Const DB_QAPPEND As Short = &H40S
	Public Const DB_QMAKETABLE As Short = &H50S

	' ListIndexes IndexAttributes values
	Public Const DB_UNIQUE As Short = 1
	Public Const DB_PRIMARY As Short = 2
	Public Const DB_PROHIBITNULL As Short = 4
	Public Const DB_IGNORENULL As Short = 8
	' ListIndexes FieldAttributes value
	Public Const DB_DESCENDING As Short = 1 'For each field in Index

	' CreateDatabase and CompactDatabase Language constants
	Public Const DB_LANG_GENERAL As String = ";LANGID=0x0809;CP=1252;COUNTRY=0"
	Public Const DB_LANG_SPANISH As String = ";LANGID=0x040A;CP=1252;COUNTRY=0"
	Public Const DB_LANG_DUTCH As String = ";LANGID=0x0413;CP=1252;COUNTRY=0"
	Public Const DB_LANG_SWEDFIN As String = ";LANGID=0x040C;CP=1252;COUNTRY=0" 'VB3 and Access 1.1 Databases
	Public Const DB_LANG_NORWDAN As String = ";LANGID=0x0414;CP=1252;COUNTRY=0" 'VB3 and Access 1.1 Databases
	Public Const DB_LANG_ICELANDIC As String = ";LANGID=0x040F;CP=1252;COUNTRY=0" 'VB3 and Access 1.1 Databases
	Public Const DB_LANG_NORDIC As String = ";LANGID=0x041D;CP=1252;COUNTRY=0" 'Access 1.0 Databases only

	' CreateDatabase and CompactDatabase options
	Public Const DB_VERSION10 As Short = 1 ' Microsoft Access Version 1.0
	Public Const DB_ENCRYPT As Short = 2 ' Make database encrypted.
	Public Const DB_DECRYPT As Short = 4 ' Decrypt database while compacting.

	'Collating order values
	Public Const DB_SORTGENERAL As Short = 256 ' Sort by EFGPI rules (English, French, German,Portuguese, Italian)
	Public Const DB_SORTSPANISH As Short = 258 ' Sort by Spanish rules
	Public Const DB_SORTDUTCH As Short = 259 ' Sort by Dutch rules
	Public Const DB_SORTSWEDFIN As Short = 260 ' Sort by Swedish, Finnish rules
	Public Const DB_SORTNORWDAN As Short = 261 ' Sort by Norwegian, Danish rules
	Public Const DB_SORTICELANDIC As Short = 262 ' Sort by Icelandic rules
	Public Const DB_SORTPDXINTL As Short = 4096 ' Sort by Paradox international rules
	Public Const DB_SORTPDXSWE As Short = 4097 ' Sort by Paradox Swedish, Finnish rules
	Public Const DB_SORTPDXNOR As Short = 4098 ' Sort by Paradox Norwegian, Danish rules
	Public Const DB_SORTUNDEFINED As Short = -1 ' Sort rules are undefined or unknown

	' Public constants for the "SetWindowPos" function

	Public Const SWP_NOMOVE As Short = 2
	Public Const SWP_NOSIZE As Short = 1
	Public Const FLAGS As Boolean = SWP_NOMOVE Or SWP_NOSIZE
	Public Const HWND_TOP As Short = 0
	Public Const HWND_TOPMOST As Short = -1
	Public Const HWND_NOTOPMOST As Short = -2

	' Public constants for the "play sound" function

	Public Const SND_ALIAS_ID As Integer = &H110000
	Public Const SND_ALIAS As Integer = &H10000
	Public Const SND_ASYNC As Short = &H1S
	Public Const SND_FILENAME As Integer = &H20000

	Public Const RESOURCE_CONNECTED As Short = &H1S
	Public Const RESOURCE_PUBLICNET As Short = &H2S
	Public Const RESOURCE_REMEMBERED As Short = &H3S

	Public Const RESOURCETYEP_ANY As Short = &H0S
	Public Const RESOURCETYEP_DISK As Short = &H1S
	Public Const RESOURCETYEP_PRINT As Short = &H2S
	Public Const RESOURCETYEP_UNKNOWN As Short = &HFFFFS

	Public Const RESOURCEUSAGE_CONNECTABLE As Short = &H1S
	Public Const RESOURCEUSAGE_CONTAINER As Short = &H2S
	Public Const RESOURCEUSAGE_RESERVED As Integer = &H80000000

	Public Const RESOURCEDISPLAYTYEP_GENERIC As Short = &H0S
	Public Const RESOURCEDISPLAYTYEP_DOMAIN As Short = &H1S
	Public Const RESOURCEDISPLAYTYEP_SERVER As Short = &H2S
	Public Const RESOURCEDISPLAYTYEP_SHARE As Short = &H3S
	Public Const RESOURCEDISPLAYTYEP_FILE As Short = &H4S
	Public Const RESOURCEDISPLAYTYEP_GROUP As Short = &H5S

	Public Const UNIVERSAL_NAME_INFO_LEVEL As Short = 1
	Public Const REMOTE_NAME_INFO_LEVEL As Short = 2

	Public Const WN_SUCCESS As Short = 0

	Public Const NETINFO_DLL16 As Short = 1
	Public Const NETINFO_DISKRED As Short = 4 ' Provider requires disk redirections to connect
	Public Const NETINFO_PRINTERRED As Short = 8 ' Provider requires printer redirections to connect

	' Important error values
	Public Const ERROR_EXTENDED_ERROR As Short = 1208
	Public Const ERROR_NO_MORE_ITEMS As Short = 259
	Public Const ERROR_MORE_DATA As Short = 234 '  dderror

	Public Structure NETRESOURCE
		Dim Scope As Integer
		Dim Type As Integer
		Dim DisplayType As Integer
		Dim Usage As Integer
		Dim LocalName As String
		Dim RemoteName As String
		Dim Comment As String
		Dim Provider As String
	End Structure

	Public Structure NETRESOURCELONG
		Dim Scope As Integer
		Dim Type As Integer
		Dim DisplayType As Integer
		Dim Usage As Integer
		Dim LocalName As Integer
		Dim RemoteName As Integer
		Dim Comment As Integer
		Dim Provider As Integer
	End Structure

	' Declare windows API functions which we will use
	Declare Function GetProfileString Lib "Kernel32.dll" Alias "GetProfileStringA" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer) As Integer
	Declare Function WriteProfileString Lib "Kernel32.dll" Alias "WriteProfileStringA" (ByVal lpszSection As String, ByVal lpszKeyName As String, ByVal lpszString As String) As Integer
	Declare Function GetPrivateProfileString Lib "Kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	Declare Function WritePrivateProfileString Lib "Kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
	Declare Function SetBkMode Lib "gdi32.dll" (ByVal hdc As Integer, ByVal nBkMode As Integer) As Integer
	Declare Function GetComputerName Lib "Kernel32" Alias "GetComputerNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	Declare Function SetWindowPos Lib "user32.dll" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	Declare Function GetSystemMenu Lib "user32" (ByVal hwnd As Integer, ByVal bRevert As Integer) As Integer
	Declare Function WinHelp Lib "user32" Alias "WinHelpA" (ByVal hwnd As Integer, ByVal lpHelpFile As String, ByVal wCommand As Integer, ByVal dwData As Integer) As Integer
	Declare Function RemoveMenu Lib "user32" (ByVal hMenu As Integer, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer
	Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Integer, ByVal nIndex As Integer) As Integer
	Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Long) As Integer
	Declare Function CreateDC Lib "gdi32" Alias "CreateDCA" (ByVal lpDriverName As String, ByVal lpDeviceName As String, ByVal lpOutput As String, ByRef lpInitData As Integer) As Integer
	Declare Function DeleteDC Lib "gdi32" (ByVal hdc As Integer) As Integer
	Declare Function GetWindowsDirectory Lib "Kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Integer) As Integer
	Declare Function PlaySound Lib "winmm.dll" Alias "PlaySoundA" (ByVal lpszName As String, ByVal hModule As Integer, ByVal dwFlags As Integer) As Integer
	Declare Function WNetOpenEnum Lib "mpr.dll" Alias "WNetOpenEnumA" (ByVal dwScope As Integer, ByVal dwType As Integer, ByVal dwUsage As Integer, ByRef lpNetResource As NETRESOURCE, ByRef lphEnum As Integer) As Integer
	Declare Function WNetOpenEnumRoot Lib "mpr.dll" Alias "WNetOpenEnumA" (ByVal dwScope As Integer, ByVal dwType As Integer, ByVal dwUsage As Integer, ByVal lpNetResource As Integer, ByRef lphEnum As Integer) As Integer
	Declare Function WNetEnumResource Lib "mpr.dll" Alias "WNetEnumResourceA" (ByVal hEnum As Integer, ByRef lpcCount As Integer, ByRef lpBuffer As Byte, ByRef lpBufferSize As Integer) As Integer
	Declare Function WNetCloseEnum Lib "mpr.dll" (ByVal hEnum As Integer) As Integer
	Declare Function GetTempFileName Lib "Kernel32" Alias "GetTempFileNameA" (ByVal lpszPath As String, ByVal lpPrefixString As String, ByVal wUnique As Integer, ByVal lpTempFileName As String) As Integer
	Declare Function GetLongPathName Lib "Kernel32" Alias "GetLongPathNameA" (ByVal lpShortPath As String, ByVal lpLongPath As String, ByVal nSize As Integer) As Integer



	Public Const MF_BYPOSITION As Integer = &H400

	' Define types used by system function calls

	Public Structure OVERLAPPED
		Dim Internal As Integer
		Dim InternalHigh As Integer
		Dim offset As Integer
		Dim OffsetHigh As Integer
		Dim hEvent As Integer
	End Structure

	' Declare mailslot functions we will use

	Declare Function CreateMailslot Lib "Kernel32" Alias "CreateMailslotA" (ByVal lpName As String, ByVal nMaxMessageSize As Integer, ByVal lReadTimeout As Integer, ByVal lpSecurityAttributes As Integer) As Integer
	Declare Function CloseHandle Lib "Kernel32" (ByVal hObject As Integer) As Integer
	Declare Function WriteFile Lib "Kernel32" (ByVal hFile As Integer, ByRef lpBuffer As Long, ByVal nNumberOfBytesToWrite As Integer, ByRef lpNumberOfBytesWritten As Integer, ByVal lpOverlapped As Integer) As Integer
	Declare Function ReadFile Lib "Kernel32" (ByVal hFile As Integer, ByRef lpBuffer As Long, ByVal nNumberOfBytesToRead As Integer, ByRef lpNumberOfBytesRead As Integer, ByVal lpOverlapped As Integer) As Integer
	Declare Function CreateFile Lib "Kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, ByVal dwShareMode As Integer, ByVal lpSecurityAttributes As Integer, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As Integer

	' Declare constants used by mailslot system function calls

	Public Const OPEN_EXISTING As Short = 3
	Public Const GENERIC_READ As Integer = &H80000000
	Public Const GENERIC_WRITE As Integer = &H40000000
	Public Const GENERIC_EXECUTE As Integer = &H20000000
	Public Const GENERIC_ALL As Integer = &H10000000
	Public Const INVALID_HANDLE_VALUE As Short = -1
	Public Const FILE_SHARE_READ As Short = &H1S
	Public Const FILE_SHARE_WRITE As Short = &H2S
	Public Const FILE_ATTRIBUTE_NORMAL As Short = &H80S

	' Declare user-defined types which we will
	' need to access some registry functions.

	Public Structure FILETIME
		Dim dwLowDateTime As Integer
		Dim dwHighDateTime As Integer
	End Structure

	Public Structure SECURITY_ATTRIBUTES
		Dim nLength As Integer
		Dim lpSecurityDescriptor As Integer
		Dim bInheritHandle As Integer
	End Structure

	' Declare the registry functions we will use

	' Note: the RegCreateKeyEx function must be redefined for
	' Win95(as opposed to NT).  We do not pass the SECURITY_ATTRIBUTES type, so
	' we redefine the call so we can pass a "0" which will appear
	' as a "Null" when passed byval.

	Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, ByRef phkResult As Integer, ByRef lpdwDisposition As Integer) As Integer
	'Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" (ByVal hKey As Long, ByVal lpSubKey As String, ByVal Reserved As Long, ByVal lpClass As String, ByVal dwOptions As Long, ByVal samDesired As Long, ByVal lpSecurityAttributes As Long, phkResult As Long, lpdwDisposition As Long) As Long
	Declare Function RegCreateKey Lib "advapi32.dll" Alias "RegCreateKeyA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByRef phkResult As Integer) As Integer
	Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As String, ByRef lpcbData As Integer) As Integer
	Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	Declare Function RegSetValueEx Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpData As String, ByVal cbData As Integer) As Integer
	Declare Function RegSetValue Lib "advapi32.dll" Alias "RegSetValueA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal dwType As Integer, ByVal lpData As String, ByVal cbData As Integer) As Integer
	Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
	Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" (ByVal hKey As Integer, ByVal lpSubKey As String) As Integer
	Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" (ByVal hKey As Integer, ByVal lpValueName As String) As Integer
	Declare Function RegEnumKeyEx Lib "advapi32.dll" Alias "RegEnumKeyExA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByRef lpcbName As Integer, ByVal lpReserved As Integer, ByVal lpClass As String, ByRef lpcbClass As Integer, ByRef lpftLastWriteTime As FILETIME) As Integer
	Declare Function RegEnumValue Lib "advapi32.dll" Alias "RegEnumValueA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpValueName As String, ByRef lpcbValueName As Integer, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Byte, ByRef lpcbData As Integer) As Integer

	' Declare the values of predefined registry keys

	Public Const HKEY_CLASSES_ROOT As Integer = &H80000000
	Public Const HKEY_CURRENT_CONFIG As Integer = &H80000005
	Public Const HKEY_CURRENT_USER As Integer = &H80000001
	Public Const HKEY_DYN_DATA As Integer = &H80000006
	Public Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	Public Const HKEY_PERFORMANCE_DATA As Integer = &H80000004
	Public Const HKEY_USERS As Integer = &H80000003

	Public Const READ_CONTROL As Integer = &H20000
	Public Const READ_WRITE As Short = 2
	Public Const STANDARD_RIGHTS_ALL As Integer = &H1F0000
	Public Const STANDARD_RIGHTS_EXECUTE As Integer = (READ_CONTROL)
	Public Const STANDARD_RIGHTS_READ As Integer = (READ_CONTROL)
	Public Const STANDARD_RIGHTS_REQUIRED As Integer = &HF0000
	Public Const STANDARD_RIGHTS_WRITE As Integer = (READ_CONTROL)

	Public Const KEY_QUERY_VALUE As Short = &H1S
	Public Const KEY_SET_VALUE As Short = &H2S
	Public Const KEY_CREATE_SUB_KEY As Short = &H4S
	Public Const KEY_CREATE_LINK As Short = &H20S
	Public Const KEY_ENUMERATE_SUB_KEYS As Short = &H8S
	Public Const KEY_EVENT As Short = &H1S
	Public Const KEY_NOTIFY As Short = &H10S
	Public Const SYNCHRONIZE As Integer = &H100000
	Public Const KEY_ALL_ACCESS As Integer = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) And (Not SYNCHRONIZE))
	Public Const KEY_READ As Integer = ((STANDARD_RIGHTS_READ Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not SYNCHRONIZE))
	Public Const KEY_EXECUTE As Integer = ((KEY_READ) And (Not SYNCHRONIZE))
	Public Const KEY_WRITE As Integer = ((STANDARD_RIGHTS_WRITE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY) And (Not SYNCHRONIZE))

	Public Const REG_BINARY As Short = 3
	Public Const REG_DWORD As Short = 4
	Public Const REG_DWORD_BIG_ENDIAN As Short = 5
	Public Const REG_DWORD_LITTLE_ENDIAN As Short = 4
	Public Const REG_EXPAND_SZ As Short = 2
	Public Const REG_LINK As Short = 6
	Public Const REG_MULTI_SZ As Short = 7
	Public Const REG_NONE As Short = 0
	Public Const REG_SZ As Short = 1

	' Create a public example of a SECURITY_ATTRIBUTE structure

	Public SecAtt As SECURITY_ATTRIBUTES

	' Declare other functions we will use

	Declare Function BitBlt Lib "gdi32" (ByVal hDestDC As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal xSrc As Integer, ByVal ySrc As Integer, ByVal dwRop As Integer) As Integer

	' Miscellaneous Public variables and constants

	Public CancelOperation As Boolean
	Public UnhandledExceptionTriggered As Boolean
	Public ProgramPath As String = ""
	Public Datapath As String = ""
	Public BackupPath As String = ""
	Public Databasename As String = ""
	Public LocalDatabase As String = ""
	Public ProgramName As String = ""
	Public Version As String = ""
	Public DBVersion As String = ""
	Public LicenseInfo As String = ""
	Public DbOpen As Boolean
	Public ProgramInUse As Boolean
	Public LoginName As String = ""
	Public UserName As String = ""
	Public UserIsSupervisor As Boolean
	Public LeftMargin As Single
	Public RightMargin As Single

	' Define the checksum values for the keys of each of the four basic programs.
	Public Const HA_Checksum = 378
	Public Const SEF_Checksum = 419
	Public Const SC_Checksum = 297
	Public Const DM_Checksum = 513

	' Define the licensing status conditions
	Public Enum LicensingStatus
		Expired
		Evaluation
		Licensed
	End Enum

	Public CustomColor1 As Color = Color.FromArgb(255, 128, 100, 56)
	Public CustomColor2 As Color = Color.FromArgb(255, 182, 189, 255)
	Public CustomColor3 As Color = Color.FromArgb(255, 228, 225, 112)
	Public CustomColor4 As Color = Color.FromArgb(255, 196, 219, 181)
	Public UserDefinedColor As Color = SystemColors.Window

	Public Dependencies As New Collection

	Public Const ADDRECORD As Short = 0
	Public Const MODIFYRECORD As Short = 1
	Public Const CHANGEID As Short = 2
	Public Const NOMATCH = -1
	Public Const e As Double = 2.71828182846
	Public Const pi As Double = 3.14159

	Public Class Dependency
		Public Name As String
		Public ObjectType As String
		Public FileToCopy As String
		Public Sub New(NameValue As String, Type As String, Optional FileName As String = "")
			Name = NameValue
			ObjectType = Type
			FileToCopy = FileName
		End Sub
	End Class
End Module