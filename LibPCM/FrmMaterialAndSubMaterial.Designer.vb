﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMaterialAndSubMaterial
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMaterialAndSubMaterial))
        Me.grpInput = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtMonthStd = New System.Windows.Forms.TextBox()
        Me.lblNormWeek = New System.Windows.Forms.Label()
        Me.txtWeekStd = New System.Windows.Forms.TextBox()
        Me.lblSubPrc = New System.Windows.Forms.Label()
        Me.lblAddDtbtQty = New System.Windows.Forms.Label()
        Me.txtAddDtbtQty = New System.Windows.Forms.TextBox()
        Me.lblStdDtbtQty = New System.Windows.Forms.Label()
        Me.txtStdDtbtQty = New System.Windows.Forms.TextBox()
        Me.lblMinQty = New System.Windows.Forms.Label()
        Me.txtMinQty = New System.Windows.Forms.TextBox()
        Me.lblUnit = New System.Windows.Forms.Label()
        Me.txtUnit = New System.Windows.Forms.TextBox()
        Me.lblVJName = New System.Windows.Forms.Label()
        Me.txtJVName = New System.Windows.Forms.TextBox()
        Me.lblJEName = New System.Windows.Forms.Label()
        Me.txtJEName = New System.Windows.Forms.TextBox()
        Me.lblJCode = New System.Windows.Forms.Label()
        Me.txtJCode = New System.Windows.Forms.TextBox()
        Me.cboSubPrc = New System.Windows.Forms.ComboBox()
        Me.lblECode = New System.Windows.Forms.Label()
        Me.txtECode = New System.Windows.Forms.TextBox()
        Me.mnuDelete = New System.Windows.Forms.ToolStripButton()
        Me.mnuExport = New System.Windows.Forms.ToolStripButton()
        Me.tlsMenu = New System.Windows.Forms.ToolStrip()
        Me.mnuNew = New System.Windows.Forms.ToolStripButton()
        Me.mnuEdit = New System.Windows.Forms.ToolStripButton()
        Me.mnuShowAll = New System.Windows.Forms.ToolStripButton()
        Me.mnuSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuImport = New System.Windows.Forms.ToolStripButton()
        Me.mnuUpdateEmpID = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.grpGrid = New System.Windows.Forms.GroupBox()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtOldID = New System.Windows.Forms.TextBox()
        Me.txtNewID = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.bttOK = New System.Windows.Forms.Button()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn8 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn5 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn7 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn4 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn3 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn2 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn6 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn1 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn9 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.CalendarColumn1 = New UtilityControl.CalendarColumn()
        Me.grpInput.SuspendLayout()
        Me.tlsMenu.SuspendLayout()
        Me.grpGrid.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpInput
        '
        Me.grpInput.Controls.Add(Me.Label4)
        Me.grpInput.Controls.Add(Me.txtMonthStd)
        Me.grpInput.Controls.Add(Me.lblNormWeek)
        Me.grpInput.Controls.Add(Me.txtWeekStd)
        Me.grpInput.Controls.Add(Me.lblSubPrc)
        Me.grpInput.Controls.Add(Me.lblAddDtbtQty)
        Me.grpInput.Controls.Add(Me.txtAddDtbtQty)
        Me.grpInput.Controls.Add(Me.lblStdDtbtQty)
        Me.grpInput.Controls.Add(Me.txtStdDtbtQty)
        Me.grpInput.Controls.Add(Me.lblMinQty)
        Me.grpInput.Controls.Add(Me.txtMinQty)
        Me.grpInput.Controls.Add(Me.lblUnit)
        Me.grpInput.Controls.Add(Me.txtUnit)
        Me.grpInput.Controls.Add(Me.lblVJName)
        Me.grpInput.Controls.Add(Me.txtJVName)
        Me.grpInput.Controls.Add(Me.lblJEName)
        Me.grpInput.Controls.Add(Me.txtJEName)
        Me.grpInput.Controls.Add(Me.lblJCode)
        Me.grpInput.Controls.Add(Me.txtJCode)
        Me.grpInput.Controls.Add(Me.cboSubPrc)
        Me.grpInput.Controls.Add(Me.lblECode)
        Me.grpInput.Controls.Add(Me.txtECode)
        Me.grpInput.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpInput.Location = New System.Drawing.Point(0, 55)
        Me.grpInput.Name = "grpInput"
        Me.grpInput.Size = New System.Drawing.Size(992, 58)
        Me.grpInput.TabIndex = 1
        Me.grpInput.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(896, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 14)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Month Std"
        '
        'txtMonthStd
        '
        Me.txtMonthStd.Location = New System.Drawing.Point(896, 27)
        Me.txtMonthStd.Name = "txtMonthStd"
        Me.txtMonthStd.Size = New System.Drawing.Size(63, 20)
        Me.txtMonthStd.TabIndex = 10
        '
        'lblNormWeek
        '
        Me.lblNormWeek.AutoSize = True
        Me.lblNormWeek.Location = New System.Drawing.Point(827, 13)
        Me.lblNormWeek.Name = "lblNormWeek"
        Me.lblNormWeek.Size = New System.Drawing.Size(53, 14)
        Me.lblNormWeek.TabIndex = 10
        Me.lblNormWeek.Text = "Week Std"
        '
        'txtWeekStd
        '
        Me.txtWeekStd.Location = New System.Drawing.Point(827, 27)
        Me.txtWeekStd.Name = "txtWeekStd"
        Me.txtWeekStd.Size = New System.Drawing.Size(63, 20)
        Me.txtWeekStd.TabIndex = 9
        '
        'lblSubPrc
        '
        Me.lblSubPrc.AutoSize = True
        Me.lblSubPrc.Location = New System.Drawing.Point(67, 13)
        Me.lblSubPrc.Name = "lblSubPrc"
        Me.lblSubPrc.Size = New System.Drawing.Size(69, 14)
        Me.lblSubPrc.TabIndex = 1
        Me.lblSubPrc.Text = "Sub Process"
        '
        'lblAddDtbtQty
        '
        Me.lblAddDtbtQty.AutoSize = True
        Me.lblAddDtbtQty.Location = New System.Drawing.Point(762, 13)
        Me.lblAddDtbtQty.Name = "lblAddDtbtQty"
        Me.lblAddDtbtQty.Size = New System.Drawing.Size(63, 14)
        Me.lblAddDtbtQty.TabIndex = 8
        Me.lblAddDtbtQty.Text = "AddDtbtQty"
        '
        'txtAddDtbtQty
        '
        Me.txtAddDtbtQty.Location = New System.Drawing.Point(762, 27)
        Me.txtAddDtbtQty.Name = "txtAddDtbtQty"
        Me.txtAddDtbtQty.Size = New System.Drawing.Size(63, 20)
        Me.txtAddDtbtQty.TabIndex = 8
        '
        'lblStdDtbtQty
        '
        Me.lblStdDtbtQty.AutoSize = True
        Me.lblStdDtbtQty.Location = New System.Drawing.Point(704, 13)
        Me.lblStdDtbtQty.Name = "lblStdDtbtQty"
        Me.lblStdDtbtQty.Size = New System.Drawing.Size(59, 14)
        Me.lblStdDtbtQty.TabIndex = 7
        Me.lblStdDtbtQty.Text = "StdDtbtQty"
        '
        'txtStdDtbtQty
        '
        Me.txtStdDtbtQty.Location = New System.Drawing.Point(704, 27)
        Me.txtStdDtbtQty.Name = "txtStdDtbtQty"
        Me.txtStdDtbtQty.Size = New System.Drawing.Size(56, 20)
        Me.txtStdDtbtQty.TabIndex = 7
        '
        'lblMinQty
        '
        Me.lblMinQty.AutoSize = True
        Me.lblMinQty.Location = New System.Drawing.Point(659, 13)
        Me.lblMinQty.Name = "lblMinQty"
        Me.lblMinQty.Size = New System.Drawing.Size(43, 14)
        Me.lblMinQty.TabIndex = 6
        Me.lblMinQty.Text = "Min Qty"
        '
        'txtMinQty
        '
        Me.txtMinQty.Location = New System.Drawing.Point(659, 27)
        Me.txtMinQty.Name = "txtMinQty"
        Me.txtMinQty.Size = New System.Drawing.Size(43, 20)
        Me.txtMinQty.TabIndex = 6
        '
        'lblUnit
        '
        Me.lblUnit.AutoSize = True
        Me.lblUnit.Location = New System.Drawing.Point(621, 13)
        Me.lblUnit.Name = "lblUnit"
        Me.lblUnit.Size = New System.Drawing.Size(25, 14)
        Me.lblUnit.TabIndex = 5
        Me.lblUnit.Text = "Unit"
        '
        'txtUnit
        '
        Me.txtUnit.Location = New System.Drawing.Point(621, 27)
        Me.txtUnit.Name = "txtUnit"
        Me.txtUnit.Size = New System.Drawing.Size(36, 20)
        Me.txtUnit.TabIndex = 5
        '
        'lblVJName
        '
        Me.lblVJName.AutoSize = True
        Me.lblVJName.Location = New System.Drawing.Point(444, 13)
        Me.lblVJName.Name = "lblVJName"
        Me.lblVJName.Size = New System.Drawing.Size(94, 14)
        Me.lblVJName.TabIndex = 4
        Me.lblVJName.Text = "Vietnamese Name"
        '
        'txtJVName
        '
        Me.txtJVName.Location = New System.Drawing.Point(444, 27)
        Me.txtJVName.Name = "txtJVName"
        Me.txtJVName.Size = New System.Drawing.Size(175, 20)
        Me.txtJVName.TabIndex = 4
        '
        'lblJEName
        '
        Me.lblJEName.AutoSize = True
        Me.lblJEName.Location = New System.Drawing.Point(267, 13)
        Me.lblJEName.Name = "lblJEName"
        Me.lblJEName.Size = New System.Drawing.Size(71, 14)
        Me.lblJEName.TabIndex = 3
        Me.lblJEName.Text = "English Name"
        '
        'txtJEName
        '
        Me.txtJEName.Location = New System.Drawing.Point(267, 27)
        Me.txtJEName.Name = "txtJEName"
        Me.txtJEName.Size = New System.Drawing.Size(175, 20)
        Me.txtJEName.TabIndex = 3
        '
        'lblJCode
        '
        Me.lblJCode.AutoSize = True
        Me.lblJCode.Location = New System.Drawing.Point(203, 13)
        Me.lblJCode.Name = "lblJCode"
        Me.lblJCode.Size = New System.Drawing.Size(37, 14)
        Me.lblJCode.TabIndex = 2
        Me.lblJCode.Text = "JCode"
        '
        'txtJCode
        '
        Me.txtJCode.Location = New System.Drawing.Point(202, 27)
        Me.txtJCode.MaxLength = 10
        Me.txtJCode.Name = "txtJCode"
        Me.txtJCode.Size = New System.Drawing.Size(62, 20)
        Me.txtJCode.TabIndex = 2
        '
        'cboSubPrc
        '
        Me.cboSubPrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubPrc.DropDownWidth = 200
        Me.cboSubPrc.FormattingEnabled = True
        Me.cboSubPrc.Location = New System.Drawing.Point(67, 27)
        Me.cboSubPrc.Name = "cboSubPrc"
        Me.cboSubPrc.Size = New System.Drawing.Size(130, 22)
        Me.cboSubPrc.TabIndex = 1
        '
        'lblECode
        '
        Me.lblECode.AutoSize = True
        Me.lblECode.Location = New System.Drawing.Point(6, 13)
        Me.lblECode.Name = "lblECode"
        Me.lblECode.Size = New System.Drawing.Size(55, 14)
        Me.lblECode.TabIndex = 0
        Me.lblECode.Text = "Emp Code"
        '
        'txtECode
        '
        Me.txtECode.Location = New System.Drawing.Point(6, 27)
        Me.txtECode.MaxLength = 5
        Me.txtECode.Name = "txtECode"
        Me.txtECode.Size = New System.Drawing.Size(56, 20)
        Me.txtECode.TabIndex = 0
        '
        'mnuDelete
        '
        Me.mnuDelete.AutoSize = False
        Me.mnuDelete.Image = CType(resources.GetObject("mnuDelete.Image"), System.Drawing.Image)
        Me.mnuDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.Size = New System.Drawing.Size(60, 50)
        Me.mnuDelete.Text = "Delete"
        Me.mnuDelete.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'mnuExport
        '
        Me.mnuExport.AutoSize = False
        Me.mnuExport.Image = CType(resources.GetObject("mnuExport.Image"), System.Drawing.Image)
        Me.mnuExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuExport.Name = "mnuExport"
        Me.mnuExport.Size = New System.Drawing.Size(60, 50)
        Me.mnuExport.Text = "Export"
        Me.mnuExport.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tlsMenu
        '
        Me.tlsMenu.AutoSize = False
        Me.tlsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tlsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNew, Me.mnuEdit, Me.mnuShowAll, Me.mnuSave, Me.mnuExport, Me.mnuDelete, Me.ToolStripSeparator1, Me.mnuImport, Me.mnuUpdateEmpID, Me.ToolStripSeparator2})
        Me.tlsMenu.Location = New System.Drawing.Point(0, 0)
        Me.tlsMenu.Name = "tlsMenu"
        Me.tlsMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.tlsMenu.Size = New System.Drawing.Size(992, 55)
        Me.tlsMenu.TabIndex = 0
        Me.tlsMenu.Text = "ToolStrip1"
        '
        'mnuNew
        '
        Me.mnuNew.AutoSize = False
        Me.mnuNew.Image = CType(resources.GetObject("mnuNew.Image"), System.Drawing.Image)
        Me.mnuNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuNew.Name = "mnuNew"
        Me.mnuNew.Size = New System.Drawing.Size(60, 50)
        Me.mnuNew.Text = "New"
        Me.mnuNew.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'mnuEdit
        '
        Me.mnuEdit.AutoSize = False
        Me.mnuEdit.Image = CType(resources.GetObject("mnuEdit.Image"), System.Drawing.Image)
        Me.mnuEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.Size = New System.Drawing.Size(60, 50)
        Me.mnuEdit.Text = "Edit"
        Me.mnuEdit.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'mnuShowAll
        '
        Me.mnuShowAll.AutoSize = False
        Me.mnuShowAll.Image = CType(resources.GetObject("mnuShowAll.Image"), System.Drawing.Image)
        Me.mnuShowAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuShowAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuShowAll.Name = "mnuShowAll"
        Me.mnuShowAll.Size = New System.Drawing.Size(60, 50)
        Me.mnuShowAll.Text = "Show all"
        Me.mnuShowAll.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuShowAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'mnuSave
        '
        Me.mnuSave.AutoSize = False
        Me.mnuSave.Image = CType(resources.GetObject("mnuSave.Image"), System.Drawing.Image)
        Me.mnuSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(60, 50)
        Me.mnuSave.Text = "Save"
        Me.mnuSave.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 55)
        '
        'mnuImport
        '
        Me.mnuImport.AutoSize = False
        Me.mnuImport.Image = CType(resources.GetObject("mnuImport.Image"), System.Drawing.Image)
        Me.mnuImport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuImport.Name = "mnuImport"
        Me.mnuImport.Size = New System.Drawing.Size(60, 50)
        Me.mnuImport.Text = "Import"
        Me.mnuImport.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.mnuImport.ToolTipText = "Import"
        '
        'mnuUpdateEmpID
        '
        Me.mnuUpdateEmpID.AutoSize = False
        Me.mnuUpdateEmpID.Image = CType(resources.GetObject("mnuUpdateEmpID.Image"), System.Drawing.Image)
        Me.mnuUpdateEmpID.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuUpdateEmpID.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuUpdateEmpID.Name = "mnuUpdateEmpID"
        Me.mnuUpdateEmpID.Size = New System.Drawing.Size(80, 50)
        Me.mnuUpdateEmpID.Text = "UpdateEmpID"
        Me.mnuUpdateEmpID.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuUpdateEmpID.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 55)
        '
        'grpGrid
        '
        Me.grpGrid.Controls.Add(Me.GridControl1)
        Me.grpGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpGrid.Location = New System.Drawing.Point(0, 113)
        Me.grpGrid.Name = "grpGrid"
        Me.grpGrid.Size = New System.Drawing.Size(992, 348)
        Me.grpGrid.TabIndex = 2
        Me.grpGrid.TabStop = False
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(3, 16)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(986, 329)
        Me.GridControl1.TabIndex = 1
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsFind.AlwaysVisible = True
        Me.GridView1.OptionsSelection.MultiSelect = True
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[True]
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(525, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 14)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "OldCode"
        '
        'txtOldID
        '
        Me.txtOldID.Location = New System.Drawing.Point(528, 24)
        Me.txtOldID.MaxLength = 5
        Me.txtOldID.Name = "txtOldID"
        Me.txtOldID.ReadOnly = True
        Me.txtOldID.Size = New System.Drawing.Size(56, 20)
        Me.txtOldID.TabIndex = 3
        '
        'txtNewID
        '
        Me.txtNewID.Location = New System.Drawing.Point(633, 24)
        Me.txtNewID.MaxLength = 5
        Me.txtNewID.Name = "txtNewID"
        Me.txtNewID.Size = New System.Drawing.Size(56, 20)
        Me.txtNewID.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(590, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 14)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "====>"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(634, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 14)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "NewCode"
        '
        'bttOK
        '
        Me.bttOK.Location = New System.Drawing.Point(707, 4)
        Me.bttOK.Name = "bttOK"
        Me.bttOK.Size = New System.Drawing.Size(96, 37)
        Me.bttOK.TabIndex = 8
        Me.bttOK.Text = "Chọn"
        Me.bttOK.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "VenderName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Vender Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'DataGridViewAutoFilterTextBoxColumn8
        '
        Me.DataGridViewAutoFilterTextBoxColumn8.DataPropertyName = "Reason"
        Me.DataGridViewAutoFilterTextBoxColumn8.HeaderText = "Reason"
        Me.DataGridViewAutoFilterTextBoxColumn8.Name = "DataGridViewAutoFilterTextBoxColumn8"
        Me.DataGridViewAutoFilterTextBoxColumn8.ReadOnly = True
        '
        'DataGridViewAutoFilterTextBoxColumn5
        '
        Me.DataGridViewAutoFilterTextBoxColumn5.DataPropertyName = "Unit"
        Me.DataGridViewAutoFilterTextBoxColumn5.HeaderText = "Unit"
        Me.DataGridViewAutoFilterTextBoxColumn5.Name = "DataGridViewAutoFilterTextBoxColumn5"
        Me.DataGridViewAutoFilterTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn5.Visible = False
        '
        'DataGridViewAutoFilterTextBoxColumn7
        '
        Me.DataGridViewAutoFilterTextBoxColumn7.DataPropertyName = "OrderDate"
        Me.DataGridViewAutoFilterTextBoxColumn7.HeaderText = "Order Date"
        Me.DataGridViewAutoFilterTextBoxColumn7.Name = "DataGridViewAutoFilterTextBoxColumn7"
        Me.DataGridViewAutoFilterTextBoxColumn7.ReadOnly = True
        '
        'DataGridViewAutoFilterTextBoxColumn4
        '
        Me.DataGridViewAutoFilterTextBoxColumn4.DataPropertyName = "Quantity"
        Me.DataGridViewAutoFilterTextBoxColumn4.HeaderText = "Quantity"
        Me.DataGridViewAutoFilterTextBoxColumn4.Name = "DataGridViewAutoFilterTextBoxColumn4"
        Me.DataGridViewAutoFilterTextBoxColumn4.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewAutoFilterTextBoxColumn3
        '
        Me.DataGridViewAutoFilterTextBoxColumn3.DataPropertyName = "Air"
        Me.DataGridViewAutoFilterTextBoxColumn3.HeaderText = "Air"
        Me.DataGridViewAutoFilterTextBoxColumn3.Name = "DataGridViewAutoFilterTextBoxColumn3"
        Me.DataGridViewAutoFilterTextBoxColumn3.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "DeliveryDate"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Delivery Date"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'DataGridViewAutoFilterTextBoxColumn2
        '
        Me.DataGridViewAutoFilterTextBoxColumn2.DataPropertyName = "PackingUnit"
        Me.DataGridViewAutoFilterTextBoxColumn2.HeaderText = "Packing Unit"
        Me.DataGridViewAutoFilterTextBoxColumn2.Name = "DataGridViewAutoFilterTextBoxColumn2"
        Me.DataGridViewAutoFilterTextBoxColumn2.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn2.Visible = False
        '
        'DataGridViewAutoFilterTextBoxColumn6
        '
        Me.DataGridViewAutoFilterTextBoxColumn6.DataPropertyName = "FullName"
        Me.DataGridViewAutoFilterTextBoxColumn6.HeaderText = "Employee"
        Me.DataGridViewAutoFilterTextBoxColumn6.Name = "DataGridViewAutoFilterTextBoxColumn6"
        Me.DataGridViewAutoFilterTextBoxColumn6.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewAutoFilterTextBoxColumn1
        '
        Me.DataGridViewAutoFilterTextBoxColumn1.DataPropertyName = "JName"
        Me.DataGridViewAutoFilterTextBoxColumn1.HeaderText = "JName"
        Me.DataGridViewAutoFilterTextBoxColumn1.Name = "DataGridViewAutoFilterTextBoxColumn1"
        Me.DataGridViewAutoFilterTextBoxColumn1.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "ID"
        Me.DataGridViewTextBoxColumn4.HeaderText = "GSR ID"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "JCode"
        Me.DataGridViewTextBoxColumn1.HeaderText = "JCode"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'DataGridViewAutoFilterTextBoxColumn9
        '
        Me.DataGridViewAutoFilterTextBoxColumn9.DataPropertyName = "IsLock"
        Me.DataGridViewAutoFilterTextBoxColumn9.HeaderText = "IsLock"
        Me.DataGridViewAutoFilterTextBoxColumn9.Name = "DataGridViewAutoFilterTextBoxColumn9"
        Me.DataGridViewAutoFilterTextBoxColumn9.ReadOnly = True
        '
        'CalendarColumn1
        '
        Me.CalendarColumn1.HeaderText = "Delivery Date"
        Me.CalendarColumn1.Name = "CalendarColumn1"
        Me.CalendarColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CalendarColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'FrmMaterialAndSubMaterial
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(992, 461)
        Me.Controls.Add(Me.bttOK)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtNewID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtOldID)
        Me.Controls.Add(Me.grpGrid)
        Me.Controls.Add(Me.grpInput)
        Me.Controls.Add(Me.tlsMenu)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "FrmMaterialAndSubMaterial"
        Me.Tag = "021203"
        Me.Text = "Material And Sub Material"
        Me.grpInput.ResumeLayout(False)
        Me.grpInput.PerformLayout()
        Me.tlsMenu.ResumeLayout(False)
        Me.tlsMenu.PerformLayout()
        Me.grpGrid.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpInput As System.Windows.Forms.GroupBox
    Friend WithEvents lblECode As System.Windows.Forms.Label
    Friend WithEvents txtECode As System.Windows.Forms.TextBox
    Friend WithEvents mnuDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuExport As System.Windows.Forms.ToolStripButton
    Friend WithEvents tlsMenu As System.Windows.Forms.ToolStrip
    Friend WithEvents mnuShowAll As System.Windows.Forms.ToolStripButton
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn8 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn5 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn7 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn4 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn3 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn2 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn6 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn1 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewAutoFilterTextBoxColumn9 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents grpGrid As System.Windows.Forms.GroupBox
    Friend WithEvents CalendarColumn1 As UtilityControl.CalendarColumn
    Friend WithEvents lblAddDtbtQty As System.Windows.Forms.Label
    Friend WithEvents txtAddDtbtQty As System.Windows.Forms.TextBox
    Friend WithEvents lblStdDtbtQty As System.Windows.Forms.Label
    Friend WithEvents txtStdDtbtQty As System.Windows.Forms.TextBox
    Friend WithEvents lblMinQty As System.Windows.Forms.Label
    Friend WithEvents txtMinQty As System.Windows.Forms.TextBox
    Friend WithEvents lblUnit As System.Windows.Forms.Label
    Friend WithEvents txtUnit As System.Windows.Forms.TextBox
    Friend WithEvents lblVJName As System.Windows.Forms.Label
    Friend WithEvents txtJVName As System.Windows.Forms.TextBox
    Friend WithEvents lblJEName As System.Windows.Forms.Label
    Friend WithEvents txtJEName As System.Windows.Forms.TextBox
    Friend WithEvents lblJCode As System.Windows.Forms.Label
    Friend WithEvents txtJCode As System.Windows.Forms.TextBox
    Friend WithEvents cboSubPrc As System.Windows.Forms.ComboBox
    Friend WithEvents lblSubPrc As System.Windows.Forms.Label
    Friend WithEvents mnuSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblNormWeek As System.Windows.Forms.Label
    Friend WithEvents txtWeekStd As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuUpdateEmpID As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtOldID As System.Windows.Forms.TextBox
    Friend WithEvents txtNewID As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents mnuImport As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtMonthStd As System.Windows.Forms.TextBox
    Friend WithEvents bttOK As System.Windows.Forms.Button
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
End Class
