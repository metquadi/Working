﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUserAuthorization
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmUserAuthorization))
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        Me.btnShow = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnNew = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnEdit = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnDelete = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnExport = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnCopyRight = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnAddRight = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnSave = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.TreeList1 = New DevExpress.XtraTreeList.TreeList()
        Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.mnuTrainning = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuGroupOfSiteStock = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGroupOfWT = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupOfTrainingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSPC = New System.Windows.Forms.ToolStripMenuItem()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.TablePanel1 = New DevExpress.Utils.Layout.TablePanel()
        Me.chbPassword = New DevExpress.XtraEditors.CheckEdit()
        Me.txtPassword = New DevExpress.XtraEditors.TextEdit()
        Me.txtUserName = New DevExpress.XtraEditors.TextEdit()
        Me.txtUserID = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.Bar3 = New DevExpress.XtraBars.Bar()
        Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
        Me.GroupControl2 = New DevExpress.XtraEditors.GroupControl()
        Me.btnKillConnect = New DevExpress.XtraBars.BarLargeButtonItem()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TreeList1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuTrainning.SuspendLayout()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TablePanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TablePanel1.SuspendLayout()
        CType(Me.chbPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUserID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl1.SuspendLayout()
        CType(Me.GroupControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BarManager1
        '
        Me.BarManager1.AllowShowToolbarsPopup = False
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnShow, Me.btnNew, Me.btnEdit, Me.btnDelete, Me.btnExport, Me.btnCopyRight, Me.btnAddRight, Me.btnSave, Me.btnKillConnect})
        Me.BarManager1.MaxItemId = 9
        '
        'Bar1
        '
        Me.Bar1.BarName = "Tools"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 0
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.btnShow), New DevExpress.XtraBars.LinkPersistInfo(Me.btnNew), New DevExpress.XtraBars.LinkPersistInfo(Me.btnEdit), New DevExpress.XtraBars.LinkPersistInfo(Me.btnDelete), New DevExpress.XtraBars.LinkPersistInfo(Me.btnExport), New DevExpress.XtraBars.LinkPersistInfo(Me.btnCopyRight, True), New DevExpress.XtraBars.LinkPersistInfo(Me.btnAddRight), New DevExpress.XtraBars.LinkPersistInfo(Me.btnKillConnect, True), New DevExpress.XtraBars.LinkPersistInfo(Me.btnSave, True)})
        Me.Bar1.OptionsBar.DrawBorder = False
        Me.Bar1.OptionsBar.DrawDragBorder = False
        Me.Bar1.Text = "Tools"
        '
        'btnShow
        '
        Me.btnShow.Caption = "Show"
        Me.btnShow.Id = 0
        Me.btnShow.ImageOptions.Image = CType(resources.GetObject("btnShow.ImageOptions.Image"), System.Drawing.Image)
        Me.btnShow.ImageOptions.LargeImage = CType(resources.GetObject("btnShow.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(60, 60)
        '
        'btnNew
        '
        Me.btnNew.Caption = "New"
        Me.btnNew.Id = 1
        Me.btnNew.ImageOptions.Image = CType(resources.GetObject("btnNew.ImageOptions.Image"), System.Drawing.Image)
        Me.btnNew.ImageOptions.LargeImage = CType(resources.GetObject("btnNew.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(60, 60)
        '
        'btnEdit
        '
        Me.btnEdit.Caption = "Edit"
        Me.btnEdit.Id = 2
        Me.btnEdit.ImageOptions.Image = CType(resources.GetObject("btnEdit.ImageOptions.Image"), System.Drawing.Image)
        Me.btnEdit.ImageOptions.LargeImage = CType(resources.GetObject("btnEdit.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(60, 60)
        '
        'btnDelete
        '
        Me.btnDelete.Caption = "Delete"
        Me.btnDelete.Id = 3
        Me.btnDelete.ImageOptions.Image = CType(resources.GetObject("btnDelete.ImageOptions.Image"), System.Drawing.Image)
        Me.btnDelete.ImageOptions.LargeImage = CType(resources.GetObject("btnDelete.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 60)
        '
        'btnExport
        '
        Me.btnExport.Caption = "Export"
        Me.btnExport.Id = 4
        Me.btnExport.ImageOptions.Image = CType(resources.GetObject("btnExport.ImageOptions.Image"), System.Drawing.Image)
        Me.btnExport.ImageOptions.LargeImage = CType(resources.GetObject("btnExport.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(60, 60)
        '
        'btnCopyRight
        '
        Me.btnCopyRight.Caption = "Copy Right"
        Me.btnCopyRight.Id = 5
        Me.btnCopyRight.ImageOptions.Image = CType(resources.GetObject("btnCopyRight.ImageOptions.Image"), System.Drawing.Image)
        Me.btnCopyRight.ImageOptions.LargeImage = CType(resources.GetObject("btnCopyRight.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnCopyRight.Name = "btnCopyRight"
        Me.btnCopyRight.Size = New System.Drawing.Size(80, 60)
        '
        'btnAddRight
        '
        Me.btnAddRight.Caption = "Add Right"
        Me.btnAddRight.Id = 6
        Me.btnAddRight.ImageOptions.Image = CType(resources.GetObject("btnAddRight.ImageOptions.Image"), System.Drawing.Image)
        Me.btnAddRight.ImageOptions.LargeImage = CType(resources.GetObject("btnAddRight.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnAddRight.Name = "btnAddRight"
        Me.btnAddRight.Size = New System.Drawing.Size(80, 60)
        '
        'btnSave
        '
        Me.btnSave.Caption = "Save"
        Me.btnSave.Id = 7
        Me.btnSave.ImageOptions.Image = CType(resources.GetObject("btnSave.ImageOptions.Image"), System.Drawing.Image)
        Me.btnSave.ImageOptions.LargeImage = CType(resources.GetObject("btnSave.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(60, 60)
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(1048, 60)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 372)
        Me.barDockControlBottom.Manager = Me.BarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(1048, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 60)
        Me.barDockControlLeft.Manager = Me.BarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 312)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1048, 60)
        Me.barDockControlRight.Manager = Me.BarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 312)
        '
        'TreeList1
        '
        Me.TreeList1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeList1.Location = New System.Drawing.Point(2, 2)
        Me.TreeList1.Name = "TreeList1"
        Me.TreeList1.OptionsFind.AlwaysVisible = True
        Me.TreeList1.OptionsView.AutoWidth = False
        Me.TreeList1.OptionsView.ShowColumns = False
        Me.TreeList1.OptionsView.ShowHorzLines = False
        Me.TreeList1.OptionsView.ShowIndicator = False
        Me.TreeList1.OptionsView.ShowVertLines = False
        Me.TreeList1.Size = New System.Drawing.Size(361, 308)
        Me.TreeList1.TabIndex = 0
        '
        'ImageCollection1
        '
        Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageCollection1.Images.SetKeyName(0, "home_16x16.png")
        Me.ImageCollection1.Images.SetKeyName(1, "open_16x16.png")
        Me.ImageCollection1.Images.SetKeyName(2, "window_16x16.png")
        '
        'GridControl1
        '
        Me.GridControl1.ContextMenuStrip = Me.mnuTrainning
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(2, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(679, 308)
        Me.GridControl1.TabIndex = 1
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'mnuTrainning
        '
        Me.mnuTrainning.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGroupOfSiteStock, Me.mnuGroupOfWT, Me.GroupOfTrainingToolStripMenuItem, Me.mnuSPC})
        Me.mnuTrainning.Name = "ContextMenuStrip1"
        Me.mnuTrainning.Size = New System.Drawing.Size(177, 92)
        '
        'mnuGroupOfSiteStock
        '
        Me.mnuGroupOfSiteStock.Name = "mnuGroupOfSiteStock"
        Me.mnuGroupOfSiteStock.Size = New System.Drawing.Size(176, 22)
        Me.mnuGroupOfSiteStock.Text = "Group of SiteStock"
        '
        'mnuGroupOfWT
        '
        Me.mnuGroupOfWT.Name = "mnuGroupOfWT"
        Me.mnuGroupOfWT.Size = New System.Drawing.Size(176, 22)
        Me.mnuGroupOfWT.Text = "Group of Worktime"
        '
        'GroupOfTrainingToolStripMenuItem
        '
        Me.GroupOfTrainingToolStripMenuItem.Name = "GroupOfTrainingToolStripMenuItem"
        Me.GroupOfTrainingToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.GroupOfTrainingToolStripMenuItem.Text = "Group of Training"
        '
        'mnuSPC
        '
        Me.mnuSPC.Name = "mnuSPC"
        Me.mnuSPC.Size = New System.Drawing.Size(176, 22)
        Me.mnuSPC.Text = "Group of SPC"
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsFind.AlwaysVisible = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'TablePanel1
        '
        Me.TablePanel1.Columns.AddRange(New DevExpress.Utils.Layout.TablePanelColumn() {New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 35.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 35.0!), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 35.0!)})
        Me.TablePanel1.Controls.Add(Me.chbPassword)
        Me.TablePanel1.Controls.Add(Me.txtPassword)
        Me.TablePanel1.Controls.Add(Me.txtUserName)
        Me.TablePanel1.Controls.Add(Me.txtUserID)
        Me.TablePanel1.Controls.Add(Me.LabelControl2)
        Me.TablePanel1.Controls.Add(Me.LabelControl1)
        Me.TablePanel1.Location = New System.Drawing.Point(641, 0)
        Me.TablePanel1.Name = "TablePanel1"
        Me.TablePanel1.Rows.AddRange(New DevExpress.Utils.Layout.TablePanelRow() {New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26.0!), New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26.0!)})
        Me.TablePanel1.ShowGrid = DevExpress.Utils.DefaultBoolean.[True]
        Me.TablePanel1.Size = New System.Drawing.Size(316, 54)
        Me.TablePanel1.TabIndex = 7
        '
        'chbPassword
        '
        Me.TablePanel1.SetColumn(Me.chbPassword, 2)
        Me.chbPassword.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chbPassword.Location = New System.Drawing.Point(214, 3)
        Me.chbPassword.MenuManager = Me.BarManager1
        Me.chbPassword.Name = "chbPassword"
        Me.chbPassword.Properties.Appearance.BackColor = System.Drawing.Color.Teal
        Me.chbPassword.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chbPassword.Properties.Appearance.ForeColor = System.Drawing.Color.White
        Me.chbPassword.Properties.Appearance.Options.UseBackColor = True
        Me.chbPassword.Properties.Appearance.Options.UseFont = True
        Me.chbPassword.Properties.Appearance.Options.UseForeColor = True
        Me.chbPassword.Properties.Caption = "Password"
        Me.TablePanel1.SetRow(Me.chbPassword, 0)
        Me.chbPassword.Size = New System.Drawing.Size(99, 20)
        Me.chbPassword.TabIndex = 8
        '
        'txtPassword
        '
        Me.TablePanel1.SetColumn(Me.txtPassword, 2)
        Me.txtPassword.Location = New System.Drawing.Point(214, 30)
        Me.txtPassword.MenuManager = Me.BarManager1
        Me.txtPassword.Name = "txtPassword"
        Me.TablePanel1.SetRow(Me.txtPassword, 1)
        Me.txtPassword.Size = New System.Drawing.Size(99, 20)
        Me.txtPassword.TabIndex = 5
        '
        'txtUserName
        '
        Me.TablePanel1.SetColumn(Me.txtUserName, 1)
        Me.txtUserName.Location = New System.Drawing.Point(108, 30)
        Me.txtUserName.MenuManager = Me.BarManager1
        Me.txtUserName.Name = "txtUserName"
        Me.TablePanel1.SetRow(Me.txtUserName, 1)
        Me.txtUserName.Size = New System.Drawing.Size(99, 20)
        Me.txtUserName.TabIndex = 4
        '
        'txtUserID
        '
        Me.TablePanel1.SetColumn(Me.txtUserID, 0)
        Me.txtUserID.Location = New System.Drawing.Point(3, 30)
        Me.txtUserID.MenuManager = Me.BarManager1
        Me.txtUserID.Name = "txtUserID"
        Me.TablePanel1.SetRow(Me.txtUserID, 1)
        Me.txtUserID.Size = New System.Drawing.Size(99, 20)
        Me.txtUserID.TabIndex = 3
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.BackColor = System.Drawing.Color.Teal
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl2.Appearance.Options.UseBackColor = True
        Me.LabelControl2.Appearance.Options.UseFont = True
        Me.LabelControl2.Appearance.Options.UseForeColor = True
        Me.LabelControl2.Appearance.Options.UseTextOptions = True
        Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TablePanel1.SetColumn(Me.LabelControl2, 1)
        Me.LabelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelControl2.Location = New System.Drawing.Point(108, 3)
        Me.LabelControl2.Name = "LabelControl2"
        Me.TablePanel1.SetRow(Me.LabelControl2, 0)
        Me.LabelControl2.Size = New System.Drawing.Size(99, 20)
        Me.LabelControl2.TabIndex = 1
        Me.LabelControl2.Text = "User Name"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.BackColor = System.Drawing.Color.Teal
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl1.Appearance.Options.UseBackColor = True
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.Appearance.Options.UseForeColor = True
        Me.LabelControl1.Appearance.Options.UseTextOptions = True
        Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TablePanel1.SetColumn(Me.LabelControl1, 0)
        Me.LabelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelControl1.Location = New System.Drawing.Point(3, 3)
        Me.LabelControl1.Name = "LabelControl1"
        Me.TablePanel1.SetRow(Me.LabelControl1, 0)
        Me.LabelControl1.Size = New System.Drawing.Size(99, 20)
        Me.LabelControl1.TabIndex = 0
        Me.LabelControl1.Text = "User ID"
        '
        'Bar3
        '
        Me.Bar3.BarName = "Status bar"
        Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
        Me.Bar3.DockCol = 0
        Me.Bar3.DockRow = 0
        Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
        Me.Bar3.OptionsBar.AllowQuickCustomization = False
        Me.Bar3.OptionsBar.DrawDragBorder = False
        Me.Bar3.OptionsBar.UseWholeRow = True
        Me.Bar3.Text = "Status bar"
        '
        'GroupControl1
        '
        Me.GroupControl1.Controls.Add(Me.TreeList1)
        Me.GroupControl1.Dock = System.Windows.Forms.DockStyle.Right
        Me.GroupControl1.Location = New System.Drawing.Point(683, 60)
        Me.GroupControl1.Name = "GroupControl1"
        Me.GroupControl1.ShowCaption = False
        Me.GroupControl1.Size = New System.Drawing.Size(365, 312)
        Me.GroupControl1.TabIndex = 13
        Me.GroupControl1.Text = "GroupControl1"
        '
        'GroupControl2
        '
        Me.GroupControl2.Controls.Add(Me.GridControl1)
        Me.GroupControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupControl2.Location = New System.Drawing.Point(0, 60)
        Me.GroupControl2.Name = "GroupControl2"
        Me.GroupControl2.ShowCaption = False
        Me.GroupControl2.Size = New System.Drawing.Size(683, 312)
        Me.GroupControl2.TabIndex = 14
        Me.GroupControl2.Text = "GroupControl2"
        '
        'btnKillConnect
        '
        Me.btnKillConnect.Caption = "Kill Connection"
        Me.btnKillConnect.Id = 8
        Me.btnKillConnect.ImageOptions.Image = CType(resources.GetObject("BarLargeButtonItem1.ImageOptions.Image"), System.Drawing.Image)
        Me.btnKillConnect.ImageOptions.LargeImage = CType(resources.GetObject("BarLargeButtonItem1.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnKillConnect.Name = "btnKillConnect"
        Me.btnKillConnect.Size = New System.Drawing.Size(90, 60)
        '
        'FrmUserAuthorization
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1048, 372)
        Me.Controls.Add(Me.GroupControl2)
        Me.Controls.Add(Me.GroupControl1)
        Me.Controls.Add(Me.TablePanel1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "FrmUserAuthorization"
        Me.Tag = "0258TL02"
        Me.Text = "User Authorization"
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TreeList1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuTrainning.ResumeLayout(False)
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TablePanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TablePanel1.ResumeLayout(False)
        Me.TablePanel1.PerformLayout()
        CType(Me.chbPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUserID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl1.ResumeLayout(False)
        CType(Me.GroupControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TreeList1 As DevExpress.XtraTreeList.TreeList
    Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents btnShow As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnNew As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnEdit As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnDelete As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnExport As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnCopyRight As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnAddRight As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents TablePanel1 As DevExpress.Utils.Layout.TablePanel
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtUserName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtUserID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnSave As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents chbPassword As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents mnuTrainning As Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuGroupOfSiteStock As Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGroupOfWT As Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupOfTrainingToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSPC As Windows.Forms.ToolStripMenuItem
    Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
    Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents GroupControl2 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents btnKillConnect As DevExpress.XtraBars.BarLargeButtonItem
End Class
