﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmChemicalReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmChemicalReport))
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        Me.btnRun = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.btnReport = New DevExpress.XtraBars.BarLargeButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
        Me.GroupControl5 = New DevExpress.XtraEditors.GroupControl()
        Me.linkLabelAttachFile = New DevExpress.XtraEditors.HyperlinkLabelControl()
        Me.btnDeleteFile = New DevExpress.XtraEditors.SimpleButton()
        Me.btnAddFile = New DevExpress.XtraEditors.SimpleButton()
        Me.GroupControl4 = New DevExpress.XtraEditors.GroupControl()
        Me.dteCDTo = New DevExpress.XtraEditors.DateEdit()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.dteCDFrom = New DevExpress.XtraEditors.DateEdit()
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
        Me.GroupControl3 = New DevExpress.XtraEditors.GroupControl()
        Me.dteExpiryDate = New DevExpress.XtraEditors.DateEdit()
        Me.dteIssueDate = New DevExpress.XtraEditors.DateEdit()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.GroupControl2 = New DevExpress.XtraEditors.GroupControl()
        Me.lueChemicalLicence = New DevExpress.XtraEditors.LookUpEdit()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl1.SuspendLayout()
        CType(Me.GroupControl5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl5.SuspendLayout()
        CType(Me.GroupControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl4.SuspendLayout()
        CType(Me.dteCDTo.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dteCDTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dteCDFrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dteCDFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl3.SuspendLayout()
        CType(Me.dteExpiryDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dteExpiryDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dteIssueDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dteIssueDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl2.SuspendLayout()
        CType(Me.lueChemicalLicence.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnRun, Me.btnReport})
        Me.BarManager1.MaxItemId = 3
        '
        'Bar1
        '
        Me.Bar1.BarName = "Tools"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 0
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.btnRun), New DevExpress.XtraBars.LinkPersistInfo(Me.btnReport)})
        Me.Bar1.OptionsBar.DrawBorder = False
        Me.Bar1.OptionsBar.DrawDragBorder = False
        Me.Bar1.Text = "Tools"
        '
        'btnRun
        '
        Me.btnRun.Caption = "Run"
        Me.btnRun.Id = 0
        Me.btnRun.ImageOptions.Image = CType(resources.GetObject("btnRun.ImageOptions.Image"), System.Drawing.Image)
        Me.btnRun.ImageOptions.LargeImage = CType(resources.GetObject("btnRun.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(60, 60)
        '
        'btnReport
        '
        Me.btnReport.Caption = "Report Detail"
        Me.btnReport.Id = 1
        Me.btnReport.ImageOptions.Image = CType(resources.GetObject("btnReport.ImageOptions.Image"), System.Drawing.Image)
        Me.btnReport.ImageOptions.LargeImage = CType(resources.GetObject("btnReport.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(85, 60)
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(978, 60)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 461)
        Me.barDockControlBottom.Manager = Me.BarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(978, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 60)
        Me.barDockControlLeft.Manager = Me.BarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 401)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(978, 60)
        Me.barDockControlRight.Manager = Me.BarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 401)
        '
        'GroupControl1
        '
        Me.GroupControl1.Controls.Add(Me.GroupControl5)
        Me.GroupControl1.Controls.Add(Me.GroupControl4)
        Me.GroupControl1.Controls.Add(Me.GroupControl3)
        Me.GroupControl1.Controls.Add(Me.GroupControl2)
        Me.GroupControl1.Location = New System.Drawing.Point(158, 0)
        Me.GroupControl1.Name = "GroupControl1"
        Me.GroupControl1.ShowCaption = False
        Me.GroupControl1.Size = New System.Drawing.Size(820, 60)
        Me.GroupControl1.TabIndex = 4
        Me.GroupControl1.Text = "GroupControl1"
        '
        'GroupControl5
        '
        Me.GroupControl5.Controls.Add(Me.linkLabelAttachFile)
        Me.GroupControl5.Controls.Add(Me.btnDeleteFile)
        Me.GroupControl5.Controls.Add(Me.btnAddFile)
        Me.GroupControl5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupControl5.Location = New System.Drawing.Point(643, 2)
        Me.GroupControl5.Name = "GroupControl5"
        Me.GroupControl5.ShowCaption = False
        Me.GroupControl5.Size = New System.Drawing.Size(175, 56)
        Me.GroupControl5.TabIndex = 3
        Me.GroupControl5.Text = "GroupControl5"
        '
        'linkLabelAttachFile
        '
        Me.linkLabelAttachFile.Location = New System.Drawing.Point(15, 36)
        Me.linkLabelAttachFile.Name = "linkLabelAttachFile"
        Me.linkLabelAttachFile.Size = New System.Drawing.Size(12, 13)
        Me.linkLabelAttachFile.TabIndex = 1
        Me.linkLabelAttachFile.Text = "..."
        '
        'btnDeleteFile
        '
        Me.btnDeleteFile.Location = New System.Drawing.Point(92, 8)
        Me.btnDeleteFile.Name = "btnDeleteFile"
        Me.btnDeleteFile.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteFile.TabIndex = 0
        Me.btnDeleteFile.Text = "Delete"
        '
        'btnAddFile
        '
        Me.btnAddFile.Location = New System.Drawing.Point(9, 8)
        Me.btnAddFile.Name = "btnAddFile"
        Me.btnAddFile.Size = New System.Drawing.Size(75, 23)
        Me.btnAddFile.TabIndex = 0
        Me.btnAddFile.Text = "Add"
        '
        'GroupControl4
        '
        Me.GroupControl4.Controls.Add(Me.dteCDTo)
        Me.GroupControl4.Controls.Add(Me.LabelControl4)
        Me.GroupControl4.Controls.Add(Me.dteCDFrom)
        Me.GroupControl4.Controls.Add(Me.LabelControl5)
        Me.GroupControl4.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupControl4.Location = New System.Drawing.Point(405, 2)
        Me.GroupControl4.Name = "GroupControl4"
        Me.GroupControl4.ShowCaption = False
        Me.GroupControl4.Size = New System.Drawing.Size(238, 56)
        Me.GroupControl4.TabIndex = 2
        Me.GroupControl4.Text = "GroupControl4"
        '
        'dteCDTo
        '
        Me.dteCDTo.EditValue = Nothing
        Me.dteCDTo.Location = New System.Drawing.Point(127, 28)
        Me.dteCDTo.Name = "dteCDTo"
        Me.dteCDTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteCDTo.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteCDTo.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dteCDTo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteCDTo.Properties.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.dteCDTo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteCDTo.Properties.Mask.EditMask = "dd-MMM-yyyy"
        Me.dteCDTo.Properties.ReadOnly = True
        Me.dteCDTo.Size = New System.Drawing.Size(100, 20)
        Me.dteCDTo.TabIndex = 1
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(42, 10)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(41, 13)
        Me.LabelControl4.TabIndex = 0
        Me.LabelControl4.Text = "CD From"
        '
        'dteCDFrom
        '
        Me.dteCDFrom.EditValue = Nothing
        Me.dteCDFrom.Location = New System.Drawing.Point(14, 28)
        Me.dteCDFrom.Name = "dteCDFrom"
        Me.dteCDFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteCDFrom.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteCDFrom.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dteCDFrom.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteCDFrom.Properties.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.dteCDFrom.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteCDFrom.Properties.Mask.EditMask = "dd-MMM-yyyy"
        Me.dteCDFrom.Properties.ReadOnly = True
        Me.dteCDFrom.Size = New System.Drawing.Size(100, 20)
        Me.dteCDFrom.TabIndex = 1
        '
        'LabelControl5
        '
        Me.LabelControl5.Location = New System.Drawing.Point(163, 10)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(29, 13)
        Me.LabelControl5.TabIndex = 0
        Me.LabelControl5.Text = "CD To"
        '
        'GroupControl3
        '
        Me.GroupControl3.Controls.Add(Me.dteExpiryDate)
        Me.GroupControl3.Controls.Add(Me.dteIssueDate)
        Me.GroupControl3.Controls.Add(Me.LabelControl3)
        Me.GroupControl3.Controls.Add(Me.LabelControl2)
        Me.GroupControl3.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupControl3.Location = New System.Drawing.Point(162, 2)
        Me.GroupControl3.Name = "GroupControl3"
        Me.GroupControl3.ShowCaption = False
        Me.GroupControl3.Size = New System.Drawing.Size(243, 56)
        Me.GroupControl3.TabIndex = 1
        Me.GroupControl3.Text = "GroupControl3"
        '
        'dteExpiryDate
        '
        Me.dteExpiryDate.EditValue = Nothing
        Me.dteExpiryDate.Location = New System.Drawing.Point(127, 28)
        Me.dteExpiryDate.Name = "dteExpiryDate"
        Me.dteExpiryDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteExpiryDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteExpiryDate.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dteExpiryDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteExpiryDate.Properties.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.dteExpiryDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteExpiryDate.Properties.Mask.EditMask = "dd-MMM-yyyy"
        Me.dteExpiryDate.Properties.ReadOnly = True
        Me.dteExpiryDate.Size = New System.Drawing.Size(100, 20)
        Me.dteExpiryDate.TabIndex = 1
        '
        'dteIssueDate
        '
        Me.dteIssueDate.EditValue = Nothing
        Me.dteIssueDate.Location = New System.Drawing.Point(14, 28)
        Me.dteIssueDate.MenuManager = Me.BarManager1
        Me.dteIssueDate.Name = "dteIssueDate"
        Me.dteIssueDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteIssueDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dteIssueDate.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dteIssueDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteIssueDate.Properties.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.dteIssueDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dteIssueDate.Properties.Mask.EditMask = "dd-MMM-yyyy"
        Me.dteIssueDate.Properties.ReadOnly = True
        Me.dteIssueDate.Size = New System.Drawing.Size(100, 20)
        Me.dteIssueDate.TabIndex = 1
        '
        'LabelControl3
        '
        Me.LabelControl3.Location = New System.Drawing.Point(148, 9)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(56, 13)
        Me.LabelControl3.TabIndex = 0
        Me.LabelControl3.Text = "Expiry Date"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(42, 9)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(52, 13)
        Me.LabelControl2.TabIndex = 0
        Me.LabelControl2.Text = "Issue Date"
        '
        'GroupControl2
        '
        Me.GroupControl2.Controls.Add(Me.lueChemicalLicence)
        Me.GroupControl2.Controls.Add(Me.LabelControl1)
        Me.GroupControl2.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupControl2.Location = New System.Drawing.Point(2, 2)
        Me.GroupControl2.Name = "GroupControl2"
        Me.GroupControl2.ShowCaption = False
        Me.GroupControl2.Size = New System.Drawing.Size(160, 56)
        Me.GroupControl2.TabIndex = 0
        Me.GroupControl2.Text = "GroupControl2"
        '
        'lueChemicalLicence
        '
        Me.lueChemicalLicence.Location = New System.Drawing.Point(11, 29)
        Me.lueChemicalLicence.MenuManager = Me.BarManager1
        Me.lueChemicalLicence.Name = "lueChemicalLicence"
        Me.lueChemicalLicence.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.lueChemicalLicence.Properties.DropDownRows = 20
        Me.lueChemicalLicence.Properties.NullText = ""
        Me.lueChemicalLicence.Size = New System.Drawing.Size(137, 20)
        Me.lueChemicalLicence.TabIndex = 1
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(35, 9)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(80, 13)
        Me.LabelControl1.TabIndex = 0
        Me.LabelControl1.Text = "Chemical Licence"
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 60)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.MenuManager = Me.BarManager1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(978, 401)
        Me.GridControl1.TabIndex = 9
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsFind.AlwaysVisible = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'FrmChemicalReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(978, 461)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.GroupControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "FrmChemicalReport"
        Me.Tag = "0216CL02"
        Me.Text = "Chemical Report New"
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl1.ResumeLayout(False)
        CType(Me.GroupControl5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl5.ResumeLayout(False)
        Me.GroupControl5.PerformLayout()
        CType(Me.GroupControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl4.ResumeLayout(False)
        Me.GroupControl4.PerformLayout()
        CType(Me.dteCDTo.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dteCDTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dteCDFrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dteCDFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl3.ResumeLayout(False)
        Me.GroupControl3.PerformLayout()
        CType(Me.dteExpiryDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dteExpiryDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dteIssueDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dteIssueDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl2.ResumeLayout(False)
        Me.GroupControl2.PerformLayout()
        CType(Me.lueChemicalLicence.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents btnRun As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents btnReport As DevExpress.XtraBars.BarLargeButtonItem
    Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents GroupControl2 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents GroupControl3 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents GroupControl4 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueChemicalLicence As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dteExpiryDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents dteIssueDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents dteCDTo As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dteCDFrom As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GroupControl5 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents btnDeleteFile As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnAddFile As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents linkLabelAttachFile As DevExpress.XtraEditors.HyperlinkLabelControl
End Class
