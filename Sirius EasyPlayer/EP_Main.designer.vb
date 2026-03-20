<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
		Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuNew = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuOpenPL = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRepair = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSavePL = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuOpenPlayer = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuCheckForNew = New System.Windows.Forms.ToolStripMenuItem()
		Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCut = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuPaste = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuUndo = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuViewBest = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuViewAll = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuControlTableEditor = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRegistryEditor = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuRecreate = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRepairMetadata = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuChangeLocation = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuChangePrecedence = New System.Windows.Forms.ToolStripMenuItem()
		Me.HelpMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuViewReadme = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuViewLicense = New System.Windows.Forms.ToolStripMenuItem()
		Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
		Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.VScrollBar1 = New System.Windows.Forms.VScrollBar()
		Me.picLibraryDisplay = New System.Windows.Forms.PictureBox()
		Me.lstPlayList = New System.Windows.Forms.ListBox()
		Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuCMCut = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMCopy = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMPaste = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMDelete = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuCMEdit = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMPlay = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMChooseBest = New System.Windows.Forms.ToolStripMenuItem()
		Me.OrderByToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMSort = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMShuffle = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMSync = New System.Windows.Forms.ToolStripMenuItem()
		Me.pnlDisplay = New System.Windows.Forms.Panel()
		Me.lblArtist = New System.Windows.Forms.Label()
		Me.lblAlbum = New System.Windows.Forms.Label()
		Me.lblDuration = New System.Windows.Forms.Label()
		Me.picAlbumArt = New System.Windows.Forms.PictureBox()
		Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
		Me.lblElapsedTime = New System.Windows.Forms.Label()
		Me.pnlMusicPlayer = New System.Windows.Forms.Panel()
		Me.lblHeader_1 = New System.Windows.Forms.Label()
		Me.lblHeader_0 = New System.Windows.Forms.Label()
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
		Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuCMFindArt = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMPasteAlbumArt = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuCMCancel2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.timClearMessage = New System.Windows.Forms.Timer(Me.components)
		Me.ContextMenuStrip3 = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuCMAddToPlaylist = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMPlayItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCMCompatibility = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuCMCancel3 = New System.Windows.Forms.ToolStripMenuItem()
		Me.timElapsedTime = New System.Windows.Forms.Timer(Me.components)
		Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
		Me.MenuStrip1.SuspendLayout()
		Me.StatusStrip1.SuspendLayout()
		CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainer1.Panel1.SuspendLayout()
		Me.SplitContainer1.Panel2.SuspendLayout()
		Me.SplitContainer1.SuspendLayout()
		Me.Panel1.SuspendLayout()
		CType(Me.picLibraryDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.ContextMenuStrip1.SuspendLayout()
		Me.pnlDisplay.SuspendLayout()
		CType(Me.picAlbumArt, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.ContextMenuStrip2.SuspendLayout()
		Me.ContextMenuStrip3.SuspendLayout()
		Me.SuspendLayout()
		'
		'MenuStrip1
		'
		Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ToolStripMenuItem3, Me.ToolStripMenuItem1, Me.HelpMenuToolStripMenuItem})
		Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.MenuStrip1.Name = "MenuStrip1"
		Me.MenuStrip1.Size = New System.Drawing.Size(859, 24)
		Me.MenuStrip1.TabIndex = 0
		Me.MenuStrip1.Text = "MenuStrip1"
		'
		'FileToolStripMenuItem
		'
		Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNew, Me.mnuOpenPL, Me.mnuRepair, Me.mnuSavePL, Me.ToolStripSeparator1, Me.mnuOpenPlayer, Me.ToolStripSeparator2, Me.mnuCheckForNew})
		Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
		Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
		Me.FileToolStripMenuItem.Text = "&File"
		'
		'mnuNew
		'
		Me.mnuNew.Name = "mnuNew"
		Me.mnuNew.Size = New System.Drawing.Size(187, 22)
		Me.mnuNew.Text = "New &Playlist"
		'
		'mnuOpenPL
		'
		Me.mnuOpenPL.Name = "mnuOpenPL"
		Me.mnuOpenPL.Size = New System.Drawing.Size(187, 22)
		Me.mnuOpenPL.Text = "&Open Playlist..."
		'
		'mnuRepair
		'
		Me.mnuRepair.Name = "mnuRepair"
		Me.mnuRepair.Size = New System.Drawing.Size(187, 22)
		Me.mnuRepair.Text = "&Repair Playlist"
		'
		'mnuSavePL
		'
		Me.mnuSavePL.Name = "mnuSavePL"
		Me.mnuSavePL.Size = New System.Drawing.Size(187, 22)
		Me.mnuSavePL.Text = "&Save Playlist..."
		'
		'ToolStripSeparator1
		'
		Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
		Me.ToolStripSeparator1.Size = New System.Drawing.Size(184, 6)
		'
		'mnuOpenPlayer
		'
		Me.mnuOpenPlayer.Name = "mnuOpenPlayer"
		Me.mnuOpenPlayer.Size = New System.Drawing.Size(187, 22)
		Me.mnuOpenPlayer.Text = "Open &Music Player"
		'
		'ToolStripSeparator2
		'
		Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
		Me.ToolStripSeparator2.Size = New System.Drawing.Size(184, 6)
		'
		'mnuCheckForNew
		'
		Me.mnuCheckForNew.Name = "mnuCheckForNew"
		Me.mnuCheckForNew.Size = New System.Drawing.Size(187, 22)
		Me.mnuCheckForNew.Text = "Check for &New Music"
		'
		'EditToolStripMenuItem
		'
		Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCut, Me.mnuCopy, Me.mnuPaste, Me.mnuUndo, Me.mnuDelete})
		Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
		Me.EditToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
		Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
		Me.EditToolStripMenuItem.Text = "&Edit"
		'
		'mnuCut
		'
		Me.mnuCut.Name = "mnuCut"
		Me.mnuCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.Delete), System.Windows.Forms.Keys)
		Me.mnuCut.Size = New System.Drawing.Size(156, 22)
		Me.mnuCut.Text = "&Cut"
		'
		'mnuCopy
		'
		Me.mnuCopy.Name = "mnuCopy"
		Me.mnuCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Insert), System.Windows.Forms.Keys)
		Me.mnuCopy.Size = New System.Drawing.Size(156, 22)
		Me.mnuCopy.Text = "C&opy"
		'
		'mnuPaste
		'
		Me.mnuPaste.Name = "mnuPaste"
		Me.mnuPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.Insert), System.Windows.Forms.Keys)
		Me.mnuPaste.Size = New System.Drawing.Size(156, 22)
		Me.mnuPaste.Text = "&Paste"
		'
		'mnuUndo
		'
		Me.mnuUndo.Name = "mnuUndo"
		Me.mnuUndo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
		Me.mnuUndo.Size = New System.Drawing.Size(156, 22)
		Me.mnuUndo.Text = "&Undo"
		'
		'mnuDelete
		'
		Me.mnuDelete.Name = "mnuDelete"
		Me.mnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete
		Me.mnuDelete.Size = New System.Drawing.Size(156, 22)
		Me.mnuDelete.Text = "&Delete"
		'
		'ToolStripMenuItem3
		'
		Me.ToolStripMenuItem3.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewBest, Me.mnuViewAll})
		Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
		Me.ToolStripMenuItem3.Size = New System.Drawing.Size(44, 20)
		Me.ToolStripMenuItem3.Text = "&View"
		'
		'mnuViewBest
		'
		Me.mnuViewBest.CheckOnClick = True
		Me.mnuViewBest.Name = "mnuViewBest"
		Me.mnuViewBest.Size = New System.Drawing.Size(211, 22)
		Me.mnuViewBest.Text = "&Best Quality Versions Only"
		'
		'mnuViewAll
		'
		Me.mnuViewAll.Name = "mnuViewAll"
		Me.mnuViewAll.Size = New System.Drawing.Size(211, 22)
		Me.mnuViewAll.Text = "&All Versions"
		'
		'ToolStripMenuItem1
		'
		Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuControlTableEditor, Me.mnuRegistryEditor, Me.ToolStripSeparator6, Me.mnuRecreate, Me.mnuRepairMetadata, Me.mnuChangeLocation, Me.mnuChangePrecedence})
		Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
		Me.ToolStripMenuItem1.Size = New System.Drawing.Size(47, 20)
		Me.ToolStripMenuItem1.Text = "&Tools"
		'
		'mnuControlTableEditor
		'
		Me.mnuControlTableEditor.Name = "mnuControlTableEditor"
		Me.mnuControlTableEditor.Size = New System.Drawing.Size(276, 22)
		Me.mnuControlTableEditor.Text = "Control Table &Editor"
		'
		'mnuRegistryEditor
		'
		Me.mnuRegistryEditor.Name = "mnuRegistryEditor"
		Me.mnuRegistryEditor.Size = New System.Drawing.Size(276, 22)
		Me.mnuRegistryEditor.Text = "&Registry Editor"
		'
		'ToolStripSeparator6
		'
		Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
		Me.ToolStripSeparator6.Size = New System.Drawing.Size(273, 6)
		'
		'mnuRecreate
		'
		Me.mnuRecreate.Name = "mnuRecreate"
		Me.mnuRecreate.Size = New System.Drawing.Size(276, 22)
		Me.mnuRecreate.Text = "Recreate &Library"
		'
		'mnuRepairMetadata
		'
		Me.mnuRepairMetadata.Name = "mnuRepairMetadata"
		Me.mnuRepairMetadata.Size = New System.Drawing.Size(276, 22)
		Me.mnuRepairMetadata.Text = "Repair Song &Metadata"
		'
		'mnuChangeLocation
		'
		Me.mnuChangeLocation.Name = "mnuChangeLocation"
		Me.mnuChangeLocation.Size = New System.Drawing.Size(276, 22)
		Me.mnuChangeLocation.Text = "&Change Library Location"
		'
		'mnuChangePrecedence
		'
		Me.mnuChangePrecedence.Name = "mnuChangePrecedence"
		Me.mnuChangePrecedence.Size = New System.Drawing.Size(276, 22)
		Me.mnuChangePrecedence.Text = "Change Music File Version Precedence"
		'
		'HelpMenuToolStripMenuItem
		'
		Me.HelpMenuToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout, Me.mnuViewReadme, Me.mnuViewLicense})
		Me.HelpMenuToolStripMenuItem.Name = "HelpMenuToolStripMenuItem"
		Me.HelpMenuToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
		Me.HelpMenuToolStripMenuItem.Text = "&Help"
		'
		'mnuAbout
		'
		Me.mnuAbout.Name = "mnuAbout"
		Me.mnuAbout.Size = New System.Drawing.Size(196, 22)
		Me.mnuAbout.Text = "&About Sirius EasyPlayer"
		'
		'mnuViewReadme
		'
		Me.mnuViewReadme.Name = "mnuViewReadme"
		Me.mnuViewReadme.Size = New System.Drawing.Size(196, 22)
		Me.mnuViewReadme.Text = "View &README"
		'
		'mnuViewLicense
		'
		Me.mnuViewLicense.Name = "mnuViewLicense"
		Me.mnuViewLicense.Size = New System.Drawing.Size(196, 22)
		Me.mnuViewLicense.Text = "View &LICENSE"
		'
		'StatusStrip1
		'
		Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
		Me.StatusStrip1.Location = New System.Drawing.Point(0, 515)
		Me.StatusStrip1.Name = "StatusStrip1"
		Me.StatusStrip1.Size = New System.Drawing.Size(859, 22)
		Me.StatusStrip1.TabIndex = 1
		Me.StatusStrip1.Text = "StatusStrip1"
		'
		'lblStatus
		'
		Me.lblStatus.AutoSize = False
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.Size = New System.Drawing.Size(657, 17)
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'SplitContainer1
		'
		Me.SplitContainer1.BackColor = System.Drawing.Color.White
		Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Left
		Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
		Me.SplitContainer1.Name = "SplitContainer1"
		'
		'SplitContainer1.Panel1
		'
		Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
		Me.SplitContainer1.Panel1.Controls.Add(Me.picLibraryDisplay)
		Me.SplitContainer1.Panel1MinSize = 400
		'
		'SplitContainer1.Panel2
		'
		Me.SplitContainer1.Panel2.Controls.Add(Me.lstPlayList)
		Me.SplitContainer1.Panel2.Controls.Add(Me.pnlDisplay)
		Me.SplitContainer1.Panel2.Controls.Add(Me.pnlMusicPlayer)
		Me.SplitContainer1.Panel2.Controls.Add(Me.lblHeader_1)
		Me.SplitContainer1.Panel2.Controls.Add(Me.lblHeader_0)
		Me.SplitContainer1.Size = New System.Drawing.Size(859, 491)
		Me.SplitContainer1.SplitterDistance = 422
		Me.SplitContainer1.TabIndex = 2
		'
		'Panel1
		'
		Me.Panel1.BackColor = System.Drawing.Color.Tan
		Me.Panel1.Controls.Add(Me.VScrollBar1)
		Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
		Me.Panel1.Location = New System.Drawing.Point(392, 0)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(28, 489)
		Me.Panel1.TabIndex = 4
		'
		'VScrollBar1
		'
		Me.VScrollBar1.Location = New System.Drawing.Point(4, 0)
		Me.VScrollBar1.Name = "VScrollBar1"
		Me.VScrollBar1.Size = New System.Drawing.Size(20, 402)
		Me.VScrollBar1.TabIndex = 0
		'
		'picLibraryDisplay
		'
		Me.picLibraryDisplay.Dock = System.Windows.Forms.DockStyle.Fill
		Me.picLibraryDisplay.Location = New System.Drawing.Point(0, 0)
		Me.picLibraryDisplay.Name = "picLibraryDisplay"
		Me.picLibraryDisplay.Size = New System.Drawing.Size(420, 489)
		Me.picLibraryDisplay.TabIndex = 3
		Me.picLibraryDisplay.TabStop = False
		'
		'lstPlayList
		'
		Me.lstPlayList.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lstPlayList.ContextMenuStrip = Me.ContextMenuStrip1
		Me.lstPlayList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
		Me.lstPlayList.FormattingEnabled = True
		Me.lstPlayList.Location = New System.Drawing.Point(0, 24)
		Me.lstPlayList.Name = "lstPlayList"
		Me.lstPlayList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
		Me.lstPlayList.Size = New System.Drawing.Size(294, 481)
		Me.lstPlayList.TabIndex = 0
		'
		'ContextMenuStrip1
		'
		Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCMCut, Me.mnuCMCopy, Me.mnuCMPaste, Me.mnuCMDelete, Me.ToolStripSeparator3, Me.mnuCMEdit, Me.mnuCMPlay, Me.mnuCMChooseBest, Me.OrderByToolStripMenuItem, Me.mnuCMSync})
		Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
		Me.ContextMenuStrip1.Size = New System.Drawing.Size(192, 208)
		'
		'mnuCMCut
		'
		Me.mnuCMCut.Name = "mnuCMCut"
		Me.mnuCMCut.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMCut.Text = "&Cut"
		Me.mnuCMCut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'mnuCMCopy
		'
		Me.mnuCMCopy.Name = "mnuCMCopy"
		Me.mnuCMCopy.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMCopy.Text = "C&opy"
		Me.mnuCMCopy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'mnuCMPaste
		'
		Me.mnuCMPaste.Name = "mnuCMPaste"
		Me.mnuCMPaste.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMPaste.Text = "&Paste"
		Me.mnuCMPaste.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'mnuCMDelete
		'
		Me.mnuCMDelete.Name = "mnuCMDelete"
		Me.mnuCMDelete.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMDelete.Text = "&Delete"
		'
		'ToolStripSeparator3
		'
		Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
		Me.ToolStripSeparator3.Size = New System.Drawing.Size(188, 6)
		'
		'mnuCMEdit
		'
		Me.mnuCMEdit.Name = "mnuCMEdit"
		Me.mnuCMEdit.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMEdit.Text = "Edit"
		'
		'mnuCMPlay
		'
		Me.mnuCMPlay.Name = "mnuCMPlay"
		Me.mnuCMPlay.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMPlay.Text = "Pla&y"
		'
		'mnuCMChooseBest
		'
		Me.mnuCMChooseBest.Name = "mnuCMChooseBest"
		Me.mnuCMChooseBest.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMChooseBest.Text = "C&hoose Best Versions"
		'
		'OrderByToolStripMenuItem
		'
		Me.OrderByToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCMSort, Me.mnuCMShuffle})
		Me.OrderByToolStripMenuItem.Name = "OrderByToolStripMenuItem"
		Me.OrderByToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
		Me.OrderByToolStripMenuItem.Text = "Order &By"
		'
		'mnuCMSort
		'
		Me.mnuCMSort.Name = "mnuCMSort"
		Me.mnuCMSort.Size = New System.Drawing.Size(190, 22)
		Me.mnuCMSort.Text = "&Sort by Album/Artiost"
		'
		'mnuCMShuffle
		'
		Me.mnuCMShuffle.Name = "mnuCMShuffle"
		Me.mnuCMShuffle.Size = New System.Drawing.Size(190, 22)
		Me.mnuCMShuffle.Text = "S&huffle"
		'
		'mnuCMSync
		'
		Me.mnuCMSync.Name = "mnuCMSync"
		Me.mnuCMSync.Size = New System.Drawing.Size(191, 22)
		Me.mnuCMSync.Text = "&Sync to Mobile Device"
		'
		'pnlDisplay
		'
		Me.pnlDisplay.Controls.Add(Me.lblArtist)
		Me.pnlDisplay.Controls.Add(Me.lblAlbum)
		Me.pnlDisplay.Controls.Add(Me.lblDuration)
		Me.pnlDisplay.Controls.Add(Me.picAlbumArt)
		Me.pnlDisplay.Controls.Add(Me.ProgressBar1)
		Me.pnlDisplay.Controls.Add(Me.lblElapsedTime)
		Me.pnlDisplay.Location = New System.Drawing.Point(0, 24)
		Me.pnlDisplay.Name = "pnlDisplay"
		Me.pnlDisplay.Size = New System.Drawing.Size(429, 400)
		Me.pnlDisplay.TabIndex = 4
		Me.pnlDisplay.Visible = False
		'
		'lblArtist
		'
		Me.lblArtist.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblArtist.Location = New System.Drawing.Point(50, 308)
		Me.lblArtist.Name = "lblArtist"
		Me.lblArtist.Size = New System.Drawing.Size(249, 23)
		Me.lblArtist.TabIndex = 11
		Me.lblArtist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblAlbum
		'
		Me.lblAlbum.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblAlbum.Location = New System.Drawing.Point(50, 19)
		Me.lblAlbum.Name = "lblAlbum"
		Me.lblAlbum.Size = New System.Drawing.Size(249, 23)
		Me.lblAlbum.TabIndex = 10
		Me.lblAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblDuration
		'
		Me.lblDuration.AutoSize = True
		Me.lblDuration.BackColor = System.Drawing.Color.Transparent
		Me.lblDuration.Location = New System.Drawing.Point(276, 349)
		Me.lblDuration.Name = "lblDuration"
		Me.lblDuration.Size = New System.Drawing.Size(34, 13)
		Me.lblDuration.TabIndex = 7
		Me.lblDuration.Text = "00:00"
		'
		'picAlbumArt
		'
		Me.picAlbumArt.Location = New System.Drawing.Point(49, 45)
		Me.picAlbumArt.Name = "picAlbumArt"
		Me.picAlbumArt.Size = New System.Drawing.Size(250, 250)
		Me.picAlbumArt.TabIndex = 6
		Me.picAlbumArt.TabStop = False
		'
		'ProgressBar1
		'
		Me.ProgressBar1.ForeColor = System.Drawing.Color.Chartreuse
		Me.ProgressBar1.Location = New System.Drawing.Point(86, 352)
		Me.ProgressBar1.Name = "ProgressBar1"
		Me.ProgressBar1.Size = New System.Drawing.Size(190, 10)
		Me.ProgressBar1.TabIndex = 9
		'
		'lblElapsedTime
		'
		Me.lblElapsedTime.AutoSize = True
		Me.lblElapsedTime.BackColor = System.Drawing.Color.Transparent
		Me.lblElapsedTime.Location = New System.Drawing.Point(53, 349)
		Me.lblElapsedTime.Name = "lblElapsedTime"
		Me.lblElapsedTime.Size = New System.Drawing.Size(34, 13)
		Me.lblElapsedTime.TabIndex = 8
		Me.lblElapsedTime.Text = "00:00"
		'
		'pnlMusicPlayer
		'
		Me.pnlMusicPlayer.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.pnlMusicPlayer.Location = New System.Drawing.Point(0, 389)
		Me.pnlMusicPlayer.Name = "pnlMusicPlayer"
		Me.pnlMusicPlayer.Size = New System.Drawing.Size(431, 100)
		Me.pnlMusicPlayer.TabIndex = 3
		Me.pnlMusicPlayer.Visible = False
		'
		'lblHeader_1
		'
		Me.lblHeader_1.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.lblHeader_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblHeader_1.Location = New System.Drawing.Point(144, 0)
		Me.lblHeader_1.Name = "lblHeader_1"
		Me.lblHeader_1.Size = New System.Drawing.Size(147, 23)
		Me.lblHeader_1.TabIndex = 2
		Me.lblHeader_1.Text = "Album Name"
		Me.lblHeader_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblHeader_0
		'
		Me.lblHeader_0.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.lblHeader_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblHeader_0.Location = New System.Drawing.Point(0, 0)
		Me.lblHeader_0.Name = "lblHeader_0"
		Me.lblHeader_0.Size = New System.Drawing.Size(145, 23)
		Me.lblHeader_0.TabIndex = 1
		Me.lblHeader_0.Text = "Song Name"
		Me.lblHeader_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.FileName = "OpenFileDialog1"
		'
		'ContextMenuStrip2
		'
		Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCMFindArt, Me.mnuCMPasteAlbumArt, Me.ToolStripSeparator4, Me.mnuCMCancel2})
		Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
		Me.ContextMenuStrip2.Size = New System.Drawing.Size(161, 76)
		'
		'mnuCMFindArt
		'
		Me.mnuCMFindArt.Name = "mnuCMFindArt"
		Me.mnuCMFindArt.Size = New System.Drawing.Size(160, 22)
		Me.mnuCMFindArt.Text = "&Find Album Art"
		'
		'mnuCMPasteAlbumArt
		'
		Me.mnuCMPasteAlbumArt.Name = "mnuCMPasteAlbumArt"
		Me.mnuCMPasteAlbumArt.Size = New System.Drawing.Size(160, 22)
		Me.mnuCMPasteAlbumArt.Text = "&Paste Album Art"
		'
		'ToolStripSeparator4
		'
		Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
		Me.ToolStripSeparator4.Size = New System.Drawing.Size(157, 6)
		'
		'mnuCMCancel2
		'
		Me.mnuCMCancel2.Name = "mnuCMCancel2"
		Me.mnuCMCancel2.Size = New System.Drawing.Size(160, 22)
		Me.mnuCMCancel2.Text = "&Cancel"
		'
		'timClearMessage
		'
		Me.timClearMessage.Interval = 10000
		'
		'ContextMenuStrip3
		'
		Me.ContextMenuStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCMAddToPlaylist, Me.mnuCMPlayItem, Me.mnuCMCompatibility, Me.ToolStripMenuItem2, Me.mnuCMCancel3})
		Me.ContextMenuStrip3.Name = "ContextMenuStrip3"
		Me.ContextMenuStrip3.Size = New System.Drawing.Size(206, 98)
		'
		'mnuCMAddToPlaylist
		'
		Me.mnuCMAddToPlaylist.Name = "mnuCMAddToPlaylist"
		Me.mnuCMAddToPlaylist.Size = New System.Drawing.Size(205, 22)
		Me.mnuCMAddToPlaylist.Text = "&Add to Playlist"
		'
		'mnuCMPlayItem
		'
		Me.mnuCMPlayItem.Name = "mnuCMPlayItem"
		Me.mnuCMPlayItem.Size = New System.Drawing.Size(205, 22)
		Me.mnuCMPlayItem.Text = "&Play"
		'
		'mnuCMCompatibility
		'
		Me.mnuCMCompatibility.CheckOnClick = True
		Me.mnuCMCompatibility.Name = "mnuCMCompatibility"
		Me.mnuCMCompatibility.Size = New System.Drawing.Size(205, 22)
		Me.mnuCMCompatibility.Text = "Use &Compaitibility Mode"
		'
		'ToolStripMenuItem2
		'
		Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
		Me.ToolStripMenuItem2.Size = New System.Drawing.Size(202, 6)
		'
		'mnuCMCancel3
		'
		Me.mnuCMCancel3.Name = "mnuCMCancel3"
		Me.mnuCMCancel3.Size = New System.Drawing.Size(205, 22)
		Me.mnuCMCancel3.Text = "Cancel"
		'
		'timElapsedTime
		'
		Me.timElapsedTime.Interval = 1000
		'
		'ToolStripSeparator5
		'
		Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
		Me.ToolStripSeparator5.Size = New System.Drawing.Size(200, 6)
		'
		'frmMain
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(859, 537)
		Me.Controls.Add(Me.SplitContainer1)
		Me.Controls.Add(Me.StatusStrip1)
		Me.Controls.Add(Me.MenuStrip1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MainMenuStrip = Me.MenuStrip1
		Me.Name = "frmMain"
		Me.Text = "Sirius EasyPlayer"
		Me.MenuStrip1.ResumeLayout(False)
		Me.MenuStrip1.PerformLayout()
		Me.StatusStrip1.ResumeLayout(False)
		Me.StatusStrip1.PerformLayout()
		Me.SplitContainer1.Panel1.ResumeLayout(False)
		Me.SplitContainer1.Panel2.ResumeLayout(False)
		CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainer1.ResumeLayout(False)
		Me.Panel1.ResumeLayout(False)
		CType(Me.picLibraryDisplay, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ContextMenuStrip1.ResumeLayout(False)
		Me.pnlDisplay.ResumeLayout(False)
		Me.pnlDisplay.PerformLayout()
		CType(Me.picAlbumArt, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ContextMenuStrip2.ResumeLayout(False)
		Me.ContextMenuStrip3.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents MenuStrip1 As MenuStrip
	Friend WithEvents StatusStrip1 As StatusStrip
	Friend WithEvents SplitContainer1 As SplitContainer
	Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
	Friend WithEvents picLibraryDisplay As PictureBox
	Friend WithEvents lblStatus As ToolStripStatusLabel
	Friend WithEvents Panel1 As Panel
	Friend WithEvents VScrollBar1 As VScrollBar
	Friend WithEvents lstPlayList As ListBox
	Friend WithEvents lblHeader_1 As Label
	Friend WithEvents lblHeader_0 As Label
	Friend WithEvents mnuOpenPL As ToolStripMenuItem
	Friend WithEvents mnuSavePL As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
	Friend WithEvents OpenFileDialog1 As OpenFileDialog
	Friend WithEvents SaveFileDialog1 As SaveFileDialog
	Friend WithEvents mnuRepair As ToolStripMenuItem
	Friend WithEvents mnuNew As ToolStripMenuItem
	Friend WithEvents mnuOpenPlayer As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
	Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
	Friend WithEvents mnuCut As ToolStripMenuItem
	Friend WithEvents mnuCopy As ToolStripMenuItem
	Friend WithEvents mnuPaste As ToolStripMenuItem
	Friend WithEvents mnuUndo As ToolStripMenuItem
	Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
	Friend WithEvents mnuCMCut As ToolStripMenuItem
	Friend WithEvents mnuCMCopy As ToolStripMenuItem
	Friend WithEvents mnuCMPaste As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
	Friend WithEvents mnuCMEdit As ToolStripMenuItem
	Friend WithEvents HelpMenuToolStripMenuItem As ToolStripMenuItem
	Friend WithEvents mnuAbout As ToolStripMenuItem
	Friend WithEvents mnuDelete As ToolStripMenuItem
	Friend WithEvents mnuCMDelete As ToolStripMenuItem
	Friend WithEvents pnlMusicPlayer As Panel
	Friend WithEvents mnuCMPlay As ToolStripMenuItem
	Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
	Friend WithEvents mnuCMFindArt As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
	Friend WithEvents mnuCMCancel2 As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
	Friend WithEvents mnuControlTableEditor As ToolStripMenuItem
	Friend WithEvents mnuRecreate As ToolStripMenuItem
	Friend WithEvents mnuCheckForNew As ToolStripMenuItem
	Friend WithEvents timClearMessage As Timer
	Friend WithEvents mnuRepairMetadata As ToolStripMenuItem
	Friend WithEvents mnuCMPasteAlbumArt As ToolStripMenuItem
	Friend WithEvents ContextMenuStrip3 As ContextMenuStrip
	Friend WithEvents mnuCMAddToPlaylist As ToolStripMenuItem
	Friend WithEvents mnuCMPlayItem As ToolStripMenuItem
	Friend WithEvents pnlDisplay As Panel
	Friend WithEvents lblDuration As Label
	Friend WithEvents picAlbumArt As PictureBox
	Friend WithEvents ProgressBar1 As ProgressBar
	Friend WithEvents lblElapsedTime As Label
	Friend WithEvents timElapsedTime As Timer
	Friend WithEvents lblAlbum As Label
	Friend WithEvents lblArtist As Label
	Friend WithEvents mnuChangeLocation As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
	Friend WithEvents mnuRegistryEditor As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
	Friend WithEvents mnuCMCompatibility As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
	Friend WithEvents mnuCMCancel3 As ToolStripMenuItem
	Friend WithEvents mnuCMChooseBest As ToolStripMenuItem
	Friend WithEvents OrderByToolStripMenuItem As ToolStripMenuItem
	Friend WithEvents mnuCMSort As ToolStripMenuItem
	Friend WithEvents mnuCMShuffle As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
	Friend WithEvents mnuViewBest As ToolStripMenuItem
	Friend WithEvents mnuViewAll As ToolStripMenuItem
	Friend WithEvents mnuChangePrecedence As ToolStripMenuItem
	Friend WithEvents mnuCMSync As ToolStripMenuItem
	Friend WithEvents mnuViewReadme As ToolStripMenuItem
	Friend WithEvents mnuViewLicense As ToolStripMenuItem
End Class
