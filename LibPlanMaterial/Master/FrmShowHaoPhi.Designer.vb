﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmShowHaoPhi
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmShowHaoPhi))
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        Me.btnShow = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnEdit = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnImport = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.dteStartDate = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.dteEndDate = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemDateEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.BandedGridView1 = New DevExpress.XtraGrid.Views.BandedGrid.BandedGridView()
        Me.btnExport = New DevExpress.XtraBars.BarLargeButtonItem()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BandedGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnShow, Me.btnImport, Me.dteStartDate, Me.dteEndDate, Me.btnEdit, Me.btnExport})
        Me.BarManager1.MaxItemId = 7
        Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit2})
        '
        'Bar1
        '
        Me.Bar1.BarName = "Tools"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 0
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.btnShow), New DevExpress.XtraBars.LinkPersistInfo(Me.btnEdit), New DevExpress.XtraBars.LinkPersistInfo(Me.btnImport), New DevExpress.XtraBars.LinkPersistInfo(Me.btnExport), New DevExpress.XtraBars.LinkPersistInfo(Me.dteStartDate, True), New DevExpress.XtraBars.LinkPersistInfo(Me.dteEndDate)})
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
        'btnEdit
        '
        Me.btnEdit.Caption = "Edit"
        Me.btnEdit.Id = 5
        Me.btnEdit.ImageOptions.Image = CType(resources.GetObject("btnEdit.ImageOptions.Image"), System.Drawing.Image)
        Me.btnEdit.ImageOptions.LargeImage = CType(resources.GetObject("btnEdit.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(60, 60)
        '
        'btnImport
        '
        Me.btnImport.Caption = "Import"
        Me.btnImport.Id = 1
        Me.btnImport.ImageOptions.Image = CType(resources.GetObject("btnImport.ImageOptions.Image"), System.Drawing.Image)
        Me.btnImport.ImageOptions.LargeImage = CType(resources.GetObject("btnImport.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(60, 60)
        '
        'dteStartDate
        '
        Me.dteStartDate.Caption = "Start Date"
        Me.dteStartDate.Edit = Me.RepositoryItemDateEdit1
        Me.dteStartDate.Id = 2
        Me.dteStartDate.Name = "dteStartDate"
        Me.dteStartDate.Size = New System.Drawing.Size(100, 60)
        '
        'RepositoryItemDateEdit1
        '
        Me.RepositoryItemDateEdit1.AutoHeight = False
        Me.RepositoryItemDateEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.RepositoryItemDateEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit1.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.RepositoryItemDateEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit1.Mask.EditMask = "dd-MMM-yyyy"
        Me.RepositoryItemDateEdit1.Name = "RepositoryItemDateEdit1"
        '
        'dteEndDate
        '
        Me.dteEndDate.Caption = "End Date"
        Me.dteEndDate.Edit = Me.RepositoryItemDateEdit2
        Me.dteEndDate.Id = 3
        Me.dteEndDate.Name = "dteEndDate"
        Me.dteEndDate.Size = New System.Drawing.Size(90, 60)
        '
        'RepositoryItemDateEdit2
        '
        Me.RepositoryItemDateEdit2.AutoHeight = False
        Me.RepositoryItemDateEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.DisplayFormat.FormatString = "MMM-yyyy"
        Me.RepositoryItemDateEdit2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit2.EditFormat.FormatString = "MMM-yyyy"
        Me.RepositoryItemDateEdit2.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit2.Mask.EditMask = "MMM-yyyy"
        Me.RepositoryItemDateEdit2.Name = "RepositoryItemDateEdit2"
        Me.RepositoryItemDateEdit2.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(765, 60)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 414)
        Me.barDockControlBottom.Manager = Me.BarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(765, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 60)
        Me.barDockControlLeft.Manager = Me.BarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 354)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(765, 60)
        Me.barDockControlRight.Manager = Me.BarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 354)
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 60)
        Me.GridControl1.MainView = Me.BandedGridView1
        Me.GridControl1.MenuManager = Me.BarManager1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(765, 354)
        Me.GridControl1.TabIndex = 4
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.BandedGridView1})
        '
        'BandedGridView1
        '
        Me.BandedGridView1.GridControl = Me.GridControl1
        Me.BandedGridView1.Name = "BandedGridView1"
        Me.BandedGridView1.OptionsFind.AlwaysVisible = True
        Me.BandedGridView1.OptionsView.ShowGroupPanel = False
        '
        'btnExport
        '
        Me.btnExport.Caption = "Export"
        Me.btnExport.Id = 6
        Me.btnExport.ImageOptions.Image = CType(resources.GetObject("BarLargeButtonItem1.ImageOptions.Image"), System.Drawing.Image)
        Me.btnExport.ImageOptions.LargeImage = CType(resources.GetObject("BarLargeButtonItem1.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(60, 60)
        '
        'FrmShowHaoPhi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(765, 414)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "FrmShowHaoPhi"
        Me.Tag = "0254PL11"
        Me.Text = "Show Hao Phí"
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BandedGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents btnShow As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnImport As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents dteStartDate As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents dteEndDate As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemDateEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents btnEdit As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents BandedGridView1 As DevExpress.XtraGrid.Views.BandedGrid.BandedGridView
    Friend WithEvents btnExport As DevExpress.XtraBars.BarLargeButtonItem
End Class
