﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmOutMaterialAndSubMaterial
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmOutMaterialAndSubMaterial))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.grpInput = New System.Windows.Forms.GroupBox()
        Me.txtSoPhieu = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblFilterECode = New System.Windows.Forms.Label()
        Me.cboECodeFilter = New System.Windows.Forms.ComboBox()
        Me.lblFilterSubPrcName = New System.Windows.Forms.Label()
        Me.lblFilterMatCode = New System.Windows.Forms.Label()
        Me.cboSubPrcNameFilter = New System.Windows.Forms.ComboBox()
        Me.cboJCodeFilter = New System.Windows.Forms.ComboBox()
        Me.chkLocked3 = New System.Windows.Forms.CheckBox()
        Me.chkLocked2 = New System.Windows.Forms.CheckBox()
        Me.chkLocked1 = New System.Windows.Forms.CheckBox()
        Me.txtOutputNumber = New System.Windows.Forms.TextBox()
        Me.lblOutputNumber = New System.Windows.Forms.Label()
        Me.dtpOrderDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDay = New System.Windows.Forms.Label()
        Me.mnuExport = New System.Windows.Forms.ToolStripButton()
        Me.mnuPrint = New System.Windows.Forms.ToolStripButton()
        Me.tlsMenu = New System.Windows.Forms.ToolStrip()
        Me.mnuShowAll = New System.Windows.Forms.ToolStripButton()
        Me.mnuDelete = New System.Windows.Forms.ToolStripButton()
        Me.btnCheckLock = New System.Windows.Forms.ToolStripButton()
        Me.mnuCheckQty = New System.Windows.Forms.ToolStripButton()
        Me.mnuEdit = New System.Windows.Forms.ToolStripButton()
        Me.mnuNJCStock = New System.Windows.Forms.ToolStripButton()
        Me.mnuExportSum = New System.Windows.Forms.ToolStripButton()
        Me.mnuTLImportLemon = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.bnGrid = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsStock = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tslblTotal1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsTotal1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tslblTotal2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsTotal2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tslblTotal3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsTotal3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsInsStock = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsInsStockD = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsNonInsStock = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsNonInsStockD = New System.Windows.Forms.ToolStripStatusLabel()
        Me.grpGrid = New System.Windows.Forms.GroupBox()
        Me.PanelSearch = New System.Windows.Forms.Panel()
        Me.txtJVNameSearch = New System.Windows.Forms.TextBox()
        Me.txtJENameSearch = New System.Windows.Forms.TextBox()
        Me.txtJCodeSearch = New System.Windows.Forms.TextBox()
        Me.gridSearch = New System.Windows.Forms.DataGridView()
        Me.lblShowJCode = New System.Windows.Forms.Label()
        Me.cboShowJCode = New System.Windows.Forms.ComboBox()
        Me.rdoEx1 = New System.Windows.Forms.RadioButton()
        Me.rdoEx2 = New System.Windows.Forms.RadioButton()
        Me.rdoEx3 = New System.Windows.Forms.RadioButton()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JCodeSearch = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JENameSearch = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JVNameSearch = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.gridD = New UtilityControl.CustomDataGridView()
        Me.YMD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ECode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DeptName = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.JCode = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.SubPrcName = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.JCodeTemp = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SubPrcNameTemp = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PrcName = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.JEName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JVName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Unit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MinQty = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.StdDtbtQty = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.NormWeekJ = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.ReviseQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UseDay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalDtbtQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Qty1 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.Qty2 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.Qty3 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.Note = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.NgaySuDung = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Adjust = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActReceive = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AVGUsing = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Total0131 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RequestQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RequestDate = New UtilityControl.CalendarColumn()
        Me.DeptGSR = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn8 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn5 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn7 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn4 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn3 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn2 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn6 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn1 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewAutoFilterTextBoxColumn9 = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.CalendarColumn1 = New UtilityControl.CalendarColumn()
        Me.grpInput.SuspendLayout()
        Me.tlsMenu.SuspendLayout()
        CType(Me.bnGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.bnGrid.SuspendLayout()
        Me.grpGrid.SuspendLayout()
        Me.PanelSearch.SuspendLayout()
        CType(Me.gridSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridD, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpInput
        '
        Me.grpInput.Controls.Add(Me.txtSoPhieu)
        Me.grpInput.Controls.Add(Me.Label1)
        Me.grpInput.Controls.Add(Me.lblFilterECode)
        Me.grpInput.Controls.Add(Me.cboECodeFilter)
        Me.grpInput.Controls.Add(Me.lblFilterSubPrcName)
        Me.grpInput.Controls.Add(Me.lblFilterMatCode)
        Me.grpInput.Controls.Add(Me.cboSubPrcNameFilter)
        Me.grpInput.Controls.Add(Me.cboJCodeFilter)
        Me.grpInput.Controls.Add(Me.chkLocked3)
        Me.grpInput.Controls.Add(Me.chkLocked2)
        Me.grpInput.Controls.Add(Me.chkLocked1)
        Me.grpInput.Controls.Add(Me.txtOutputNumber)
        Me.grpInput.Controls.Add(Me.lblOutputNumber)
        Me.grpInput.Controls.Add(Me.dtpOrderDate)
        Me.grpInput.Controls.Add(Me.lblDay)
        Me.grpInput.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpInput.Location = New System.Drawing.Point(0, 55)
        Me.grpInput.Name = "grpInput"
        Me.grpInput.Size = New System.Drawing.Size(984, 56)
        Me.grpInput.TabIndex = 1
        Me.grpInput.TabStop = False
        '
        'txtSoPhieu
        '
        Me.txtSoPhieu.Location = New System.Drawing.Point(794, 28)
        Me.txtSoPhieu.Name = "txtSoPhieu"
        Me.txtSoPhieu.Size = New System.Drawing.Size(98, 20)
        Me.txtSoPhieu.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(791, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 14)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Số phiếu:"
        '
        'lblFilterECode
        '
        Me.lblFilterECode.AutoSize = True
        Me.lblFilterECode.Location = New System.Drawing.Point(719, 11)
        Me.lblFilterECode.Name = "lblFilterECode"
        Me.lblFilterECode.Size = New System.Drawing.Size(64, 14)
        Me.lblFilterECode.TabIndex = 10
        Me.lblFilterECode.Text = "Filter ECode"
        Me.lblFilterECode.Visible = False
        '
        'cboECodeFilter
        '
        Me.cboECodeFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboECodeFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboECodeFilter.FormattingEnabled = True
        Me.cboECodeFilter.Location = New System.Drawing.Point(719, 28)
        Me.cboECodeFilter.Name = "cboECodeFilter"
        Me.cboECodeFilter.Size = New System.Drawing.Size(69, 22)
        Me.cboECodeFilter.TabIndex = 9
        Me.cboECodeFilter.Visible = False
        '
        'lblFilterSubPrcName
        '
        Me.lblFilterSubPrcName.AutoSize = True
        Me.lblFilterSubPrcName.Location = New System.Drawing.Point(565, 11)
        Me.lblFilterSubPrcName.Name = "lblFilterSubPrcName"
        Me.lblFilterSubPrcName.Size = New System.Drawing.Size(95, 14)
        Me.lblFilterSubPrcName.TabIndex = 8
        Me.lblFilterSubPrcName.Text = "Filter SubPrcName"
        '
        'lblFilterMatCode
        '
        Me.lblFilterMatCode.AutoSize = True
        Me.lblFilterMatCode.Location = New System.Drawing.Point(474, 11)
        Me.lblFilterMatCode.Name = "lblFilterMatCode"
        Me.lblFilterMatCode.Size = New System.Drawing.Size(75, 14)
        Me.lblFilterMatCode.TabIndex = 7
        Me.lblFilterMatCode.Text = "Filter MatCode"
        '
        'cboSubPrcNameFilter
        '
        Me.cboSubPrcNameFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboSubPrcNameFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSubPrcNameFilter.FormattingEnabled = True
        Me.cboSubPrcNameFilter.Location = New System.Drawing.Point(565, 28)
        Me.cboSubPrcNameFilter.Name = "cboSubPrcNameFilter"
        Me.cboSubPrcNameFilter.Size = New System.Drawing.Size(148, 22)
        Me.cboSubPrcNameFilter.TabIndex = 6
        '
        'cboJCodeFilter
        '
        Me.cboJCodeFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboJCodeFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboJCodeFilter.FormattingEnabled = True
        Me.cboJCodeFilter.Location = New System.Drawing.Point(474, 28)
        Me.cboJCodeFilter.Name = "cboJCodeFilter"
        Me.cboJCodeFilter.Size = New System.Drawing.Size(85, 22)
        Me.cboJCodeFilter.TabIndex = 5
        '
        'chkLocked3
        '
        Me.chkLocked3.AutoSize = True
        Me.chkLocked3.Enabled = False
        Me.chkLocked3.Location = New System.Drawing.Point(370, 33)
        Me.chkLocked3.Name = "chkLocked3"
        Me.chkLocked3.Size = New System.Drawing.Size(95, 18)
        Me.chkLocked3.TabIndex = 4
        Me.chkLocked3.Text = "Locked Time 3"
        Me.chkLocked3.UseVisualStyleBackColor = True
        '
        'chkLocked2
        '
        Me.chkLocked2.AutoSize = True
        Me.chkLocked2.Enabled = False
        Me.chkLocked2.Location = New System.Drawing.Point(275, 33)
        Me.chkLocked2.Name = "chkLocked2"
        Me.chkLocked2.Size = New System.Drawing.Size(95, 18)
        Me.chkLocked2.TabIndex = 3
        Me.chkLocked2.Text = "Locked Time 2"
        Me.chkLocked2.UseVisualStyleBackColor = True
        '
        'chkLocked1
        '
        Me.chkLocked1.AutoSize = True
        Me.chkLocked1.Enabled = False
        Me.chkLocked1.Location = New System.Drawing.Point(180, 33)
        Me.chkLocked1.Name = "chkLocked1"
        Me.chkLocked1.Size = New System.Drawing.Size(95, 18)
        Me.chkLocked1.TabIndex = 2
        Me.chkLocked1.Text = "Locked Time 1"
        Me.chkLocked1.UseVisualStyleBackColor = True
        '
        'txtOutputNumber
        '
        Me.txtOutputNumber.Enabled = False
        Me.txtOutputNumber.Location = New System.Drawing.Point(102, 33)
        Me.txtOutputNumber.Name = "txtOutputNumber"
        Me.txtOutputNumber.Size = New System.Drawing.Size(72, 20)
        Me.txtOutputNumber.TabIndex = 1
        '
        'lblOutputNumber
        '
        Me.lblOutputNumber.AutoSize = True
        Me.lblOutputNumber.Enabled = False
        Me.lblOutputNumber.Location = New System.Drawing.Point(99, 16)
        Me.lblOutputNumber.Name = "lblOutputNumber"
        Me.lblOutputNumber.Size = New System.Drawing.Size(82, 14)
        Me.lblOutputNumber.TabIndex = 1
        Me.lblOutputNumber.Text = "Output Number:"
        '
        'dtpOrderDate
        '
        Me.dtpOrderDate.CustomFormat = "dd-MM-yyyy"
        Me.dtpOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOrderDate.Location = New System.Drawing.Point(6, 30)
        Me.dtpOrderDate.Name = "dtpOrderDate"
        Me.dtpOrderDate.Size = New System.Drawing.Size(88, 20)
        Me.dtpOrderDate.TabIndex = 0
        '
        'lblDay
        '
        Me.lblDay.AutoSize = True
        Me.lblDay.Location = New System.Drawing.Point(6, 16)
        Me.lblDay.Name = "lblDay"
        Me.lblDay.Size = New System.Drawing.Size(29, 14)
        Me.lblDay.TabIndex = 0
        Me.lblDay.Text = "Day:"
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
        'mnuPrint
        '
        Me.mnuPrint.AutoSize = False
        Me.mnuPrint.Image = CType(resources.GetObject("mnuPrint.Image"), System.Drawing.Image)
        Me.mnuPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuPrint.Name = "mnuPrint"
        Me.mnuPrint.Size = New System.Drawing.Size(60, 50)
        Me.mnuPrint.Text = "Print"
        Me.mnuPrint.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tlsMenu
        '
        Me.tlsMenu.AutoSize = False
        Me.tlsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tlsMenu.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.tlsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowAll, Me.mnuExport, Me.mnuPrint, Me.mnuDelete, Me.btnCheckLock, Me.mnuCheckQty, Me.mnuEdit, Me.mnuNJCStock, Me.mnuExportSum, Me.mnuTLImportLemon, Me.ToolStripSeparator2})
        Me.tlsMenu.Location = New System.Drawing.Point(0, 0)
        Me.tlsMenu.Name = "tlsMenu"
        Me.tlsMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.tlsMenu.Size = New System.Drawing.Size(984, 55)
        Me.tlsMenu.TabIndex = 0
        Me.tlsMenu.Text = "ToolStrip1"
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
        'btnCheckLock
        '
        Me.btnCheckLock.AutoSize = False
        Me.btnCheckLock.Image = CType(resources.GetObject("btnCheckLock.Image"), System.Drawing.Image)
        Me.btnCheckLock.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnCheckLock.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCheckLock.Name = "btnCheckLock"
        Me.btnCheckLock.Size = New System.Drawing.Size(65, 50)
        Me.btnCheckLock.Text = "Check Lock"
        Me.btnCheckLock.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.btnCheckLock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'mnuCheckQty
        '
        Me.mnuCheckQty.AutoSize = False
        Me.mnuCheckQty.Image = CType(resources.GetObject("mnuCheckQty.Image"), System.Drawing.Image)
        Me.mnuCheckQty.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuCheckQty.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuCheckQty.Name = "mnuCheckQty"
        Me.mnuCheckQty.Size = New System.Drawing.Size(65, 50)
        Me.mnuCheckQty.Text = "Check Qty"
        Me.mnuCheckQty.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuCheckQty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
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
        'mnuNJCStock
        '
        Me.mnuNJCStock.AutoSize = False
        Me.mnuNJCStock.Image = CType(resources.GetObject("mnuNJCStock.Image"), System.Drawing.Image)
        Me.mnuNJCStock.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuNJCStock.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuNJCStock.Name = "mnuNJCStock"
        Me.mnuNJCStock.Size = New System.Drawing.Size(60, 50)
        Me.mnuNJCStock.Text = "NJCStock"
        Me.mnuNJCStock.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuNJCStock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'mnuExportSum
        '
        Me.mnuExportSum.AutoSize = False
        Me.mnuExportSum.Image = CType(resources.GetObject("mnuExportSum.Image"), System.Drawing.Image)
        Me.mnuExportSum.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuExportSum.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuExportSum.Name = "mnuExportSum"
        Me.mnuExportSum.Size = New System.Drawing.Size(60, 50)
        Me.mnuExportSum.Text = "ExportSum"
        Me.mnuExportSum.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuExportSum.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.mnuExportSum.ToolTipText = "ExportSum"
        '
        'mnuTLImportLemon
        '
        Me.mnuTLImportLemon.Image = CType(resources.GetObject("mnuTLImportLemon.Image"), System.Drawing.Image)
        Me.mnuTLImportLemon.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuTLImportLemon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuTLImportLemon.Name = "mnuTLImportLemon"
        Me.mnuTLImportLemon.Size = New System.Drawing.Size(113, 52)
        Me.mnuTLImportLemon.Text = "TempImportLemon"
        Me.mnuTLImportLemon.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        Me.mnuTLImportLemon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.mnuTLImportLemon.ToolTipText = "ExportSum"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 55)
        '
        'bnGrid
        '
        Me.bnGrid.AddNewItem = Nothing
        Me.bnGrid.CountItem = Me.BindingNavigatorCountItem
        Me.bnGrid.DeleteItem = Nothing
        Me.bnGrid.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bnGrid.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.bnGrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.ToolStripStatusLabel1, Me.tsStock, Me.tslblTotal1, Me.tsTotal1, Me.tslblTotal2, Me.tsTotal2, Me.tslblTotal3, Me.tsTotal3, Me.tsInsStock, Me.tsInsStockD, Me.tsNonInsStock, Me.tsNonInsStockD})
        Me.bnGrid.Location = New System.Drawing.Point(3, 547)
        Me.bnGrid.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.bnGrid.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.bnGrid.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.bnGrid.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.bnGrid.Name = "bnGrid"
        Me.bnGrid.PositionItem = Me.BindingNavigatorPositionItem
        Me.bnGrid.Size = New System.Drawing.Size(978, 31)
        Me.bnGrid.TabIndex = 39
        Me.bnGrid.Text = "BindingNavigator1"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(35, 28)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(28, 28)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(28, 28)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 31)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 23)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 31)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(28, 28)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(28, 28)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 31)
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(39, 26)
        Me.ToolStripStatusLabel1.Text = "Stock:"
        '
        'tsStock
        '
        Me.tsStock.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsStock.ForeColor = System.Drawing.Color.Blue
        Me.tsStock.Name = "tsStock"
        Me.tsStock.Size = New System.Drawing.Size(14, 26)
        Me.tsStock.Text = "0"
        '
        'tslblTotal1
        '
        Me.tslblTotal1.Name = "tslblTotal1"
        Me.tslblTotal1.Size = New System.Drawing.Size(44, 26)
        Me.tslblTotal1.Text = "Total 1:"
        '
        'tsTotal1
        '
        Me.tsTotal1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsTotal1.ForeColor = System.Drawing.Color.Blue
        Me.tsTotal1.Name = "tsTotal1"
        Me.tsTotal1.Size = New System.Drawing.Size(14, 26)
        Me.tsTotal1.Text = "0"
        '
        'tslblTotal2
        '
        Me.tslblTotal2.Name = "tslblTotal2"
        Me.tslblTotal2.Size = New System.Drawing.Size(44, 26)
        Me.tslblTotal2.Text = "Total 2:"
        '
        'tsTotal2
        '
        Me.tsTotal2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsTotal2.ForeColor = System.Drawing.Color.Blue
        Me.tsTotal2.Name = "tsTotal2"
        Me.tsTotal2.Size = New System.Drawing.Size(14, 26)
        Me.tsTotal2.Text = "0"
        '
        'tslblTotal3
        '
        Me.tslblTotal3.Name = "tslblTotal3"
        Me.tslblTotal3.Size = New System.Drawing.Size(44, 26)
        Me.tslblTotal3.Text = "Total 3:"
        '
        'tsTotal3
        '
        Me.tsTotal3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsTotal3.ForeColor = System.Drawing.Color.Blue
        Me.tsTotal3.Name = "tsTotal3"
        Me.tsTotal3.Size = New System.Drawing.Size(14, 26)
        Me.tsTotal3.Text = "0"
        '
        'tsInsStock
        '
        Me.tsInsStock.Name = "tsInsStock"
        Me.tsInsStock.Size = New System.Drawing.Size(57, 26)
        Me.tsInsStock.Text = "Ins Stock:"
        '
        'tsInsStockD
        '
        Me.tsInsStockD.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsInsStockD.ForeColor = System.Drawing.Color.Blue
        Me.tsInsStockD.Name = "tsInsStockD"
        Me.tsInsStockD.Size = New System.Drawing.Size(14, 26)
        Me.tsInsStockD.Text = "0"
        '
        'tsNonInsStock
        '
        Me.tsNonInsStock.Name = "tsNonInsStock"
        Me.tsNonInsStock.Size = New System.Drawing.Size(85, 26)
        Me.tsNonInsStock.Text = "Non-Ins Stock:"
        '
        'tsNonInsStockD
        '
        Me.tsNonInsStockD.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsNonInsStockD.ForeColor = System.Drawing.Color.Blue
        Me.tsNonInsStockD.Name = "tsNonInsStockD"
        Me.tsNonInsStockD.Size = New System.Drawing.Size(14, 26)
        Me.tsNonInsStockD.Text = "0"
        '
        'grpGrid
        '
        Me.grpGrid.Controls.Add(Me.PanelSearch)
        Me.grpGrid.Controls.Add(Me.bnGrid)
        Me.grpGrid.Controls.Add(Me.gridD)
        Me.grpGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpGrid.Location = New System.Drawing.Point(0, 111)
        Me.grpGrid.Name = "grpGrid"
        Me.grpGrid.Size = New System.Drawing.Size(984, 581)
        Me.grpGrid.TabIndex = 40
        Me.grpGrid.TabStop = False
        '
        'PanelSearch
        '
        Me.PanelSearch.Controls.Add(Me.txtJVNameSearch)
        Me.PanelSearch.Controls.Add(Me.txtJENameSearch)
        Me.PanelSearch.Controls.Add(Me.txtJCodeSearch)
        Me.PanelSearch.Controls.Add(Me.gridSearch)
        Me.PanelSearch.Location = New System.Drawing.Point(182, 90)
        Me.PanelSearch.Name = "PanelSearch"
        Me.PanelSearch.Size = New System.Drawing.Size(381, 212)
        Me.PanelSearch.TabIndex = 40
        Me.PanelSearch.Visible = False
        '
        'txtJVNameSearch
        '
        Me.txtJVNameSearch.Location = New System.Drawing.Point(213, 4)
        Me.txtJVNameSearch.Name = "txtJVNameSearch"
        Me.txtJVNameSearch.Size = New System.Drawing.Size(143, 20)
        Me.txtJVNameSearch.TabIndex = 4
        '
        'txtJENameSearch
        '
        Me.txtJENameSearch.Location = New System.Drawing.Point(66, 4)
        Me.txtJENameSearch.Name = "txtJENameSearch"
        Me.txtJENameSearch.Size = New System.Drawing.Size(143, 20)
        Me.txtJENameSearch.TabIndex = 3
        '
        'txtJCodeSearch
        '
        Me.txtJCodeSearch.Location = New System.Drawing.Point(3, 4)
        Me.txtJCodeSearch.Name = "txtJCodeSearch"
        Me.txtJCodeSearch.Size = New System.Drawing.Size(59, 20)
        Me.txtJCodeSearch.TabIndex = 2
        '
        'gridSearch
        '
        Me.gridSearch.AllowUserToAddRows = False
        Me.gridSearch.AllowUserToDeleteRows = False
        Me.gridSearch.AllowUserToOrderColumns = True
        Me.gridSearch.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LemonChiffon
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LemonChiffon
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Blue
        Me.gridSearch.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.gridSearch.BackgroundColor = System.Drawing.Color.WhiteSmoke
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridSearch.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.gridSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSearch.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.JCodeSearch, Me.JENameSearch, Me.JVNameSearch})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridSearch.DefaultCellStyle = DataGridViewCellStyle3
        Me.gridSearch.EnableHeadersVisualStyles = False
        Me.gridSearch.Location = New System.Drawing.Point(0, 29)
        Me.gridSearch.Name = "gridSearch"
        Me.gridSearch.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridSearch.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.gridSearch.RowHeadersWidth = 10
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Blue
        Me.gridSearch.RowsDefaultCellStyle = DataGridViewCellStyle5
        Me.gridSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridSearch.Size = New System.Drawing.Size(381, 180)
        Me.gridSearch.TabIndex = 1
        '
        'lblShowJCode
        '
        Me.lblShowJCode.AutoSize = True
        Me.lblShowJCode.Location = New System.Drawing.Point(677, 4)
        Me.lblShowJCode.Name = "lblShowJCode"
        Me.lblShowJCode.Size = New System.Drawing.Size(81, 14)
        Me.lblShowJCode.TabIndex = 42
        Me.lblShowJCode.Text = "Show MatCode"
        '
        'cboShowJCode
        '
        Me.cboShowJCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboShowJCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboShowJCode.FormattingEnabled = True
        Me.cboShowJCode.Location = New System.Drawing.Point(677, 21)
        Me.cboShowJCode.Name = "cboShowJCode"
        Me.cboShowJCode.Size = New System.Drawing.Size(85, 22)
        Me.cboShowJCode.TabIndex = 41
        '
        'rdoEx1
        '
        Me.rdoEx1.AutoSize = True
        Me.rdoEx1.Location = New System.Drawing.Point(768, 24)
        Me.rdoEx1.Name = "rdoEx1"
        Me.rdoEx1.Size = New System.Drawing.Size(46, 18)
        Me.rdoEx1.TabIndex = 43
        Me.rdoEx1.TabStop = True
        Me.rdoEx1.Text = "Ex 1"
        Me.rdoEx1.UseVisualStyleBackColor = True
        '
        'rdoEx2
        '
        Me.rdoEx2.AutoSize = True
        Me.rdoEx2.Location = New System.Drawing.Point(814, 24)
        Me.rdoEx2.Name = "rdoEx2"
        Me.rdoEx2.Size = New System.Drawing.Size(46, 18)
        Me.rdoEx2.TabIndex = 44
        Me.rdoEx2.TabStop = True
        Me.rdoEx2.Text = "Ex 2"
        Me.rdoEx2.UseVisualStyleBackColor = True
        '
        'rdoEx3
        '
        Me.rdoEx3.AutoSize = True
        Me.rdoEx3.Location = New System.Drawing.Point(861, 24)
        Me.rdoEx3.Name = "rdoEx3"
        Me.rdoEx3.Size = New System.Drawing.Size(46, 18)
        Me.rdoEx3.TabIndex = 45
        Me.rdoEx3.TabStop = True
        Me.rdoEx3.Text = "Ex 3"
        Me.rdoEx3.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "JCode"
        Me.DataGridViewTextBoxColumn1.HeaderText = "JCode"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn1.Width = 50
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "DeliveryDate"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Delivery Date"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn2.Width = 150
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "VenderName"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Vender Name"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.DataGridViewTextBoxColumn3.Width = 150
        '
        'JCodeSearch
        '
        Me.JCodeSearch.DataPropertyName = "JCode"
        Me.JCodeSearch.HeaderText = "JCode"
        Me.JCodeSearch.MinimumWidth = 8
        Me.JCodeSearch.Name = "JCodeSearch"
        Me.JCodeSearch.ReadOnly = True
        Me.JCodeSearch.Width = 50
        '
        'JENameSearch
        '
        Me.JENameSearch.DataPropertyName = "JEName"
        Me.JENameSearch.HeaderText = "JEName"
        Me.JENameSearch.MinimumWidth = 8
        Me.JENameSearch.Name = "JENameSearch"
        Me.JENameSearch.ReadOnly = True
        Me.JENameSearch.Width = 150
        '
        'JVNameSearch
        '
        Me.JVNameSearch.DataPropertyName = "JVName"
        Me.JVNameSearch.HeaderText = "JVName"
        Me.JVNameSearch.MinimumWidth = 8
        Me.JVNameSearch.Name = "JVNameSearch"
        Me.JVNameSearch.ReadOnly = True
        Me.JVNameSearch.Width = 150
        '
        'gridD
        '
        Me.gridD.AllowUserToDeleteRows = False
        Me.gridD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridD.BackgroundColor = System.Drawing.Color.WhiteSmoke
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridD.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.gridD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridD.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.YMD, Me.ECode, Me.DeptName, Me.JCode, Me.SubPrcName, Me.JCodeTemp, Me.SubPrcNameTemp, Me.PrcName, Me.JEName, Me.JVName, Me.Unit, Me.MinQty, Me.StdDtbtQty, Me.NormWeekJ, Me.ReviseQty, Me.UseDay, Me.TotalDtbtQty, Me.Qty1, Me.Qty2, Me.Qty3, Me.Note, Me.NgaySuDung, Me.Adjust, Me.TotalQty, Me.ActReceive, Me.AVGUsing, Me.Total0131, Me.RequestQty, Me.RequestDate, Me.DeptGSR})
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridD.DefaultCellStyle = DataGridViewCellStyle11
        Me.gridD.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.gridD.EnableHeadersVisualStyles = False
        Me.gridD.Location = New System.Drawing.Point(3, 16)
        Me.gridD.Name = "gridD"
        Me.gridD.RowHeadersWidth = 20
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Blue
        Me.gridD.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.gridD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridD.Size = New System.Drawing.Size(975, 534)
        Me.gridD.TabIndex = 0
        '
        'YMD
        '
        Me.YMD.DataPropertyName = "YMD"
        Me.YMD.Frozen = True
        Me.YMD.HeaderText = "YMD"
        Me.YMD.MinimumWidth = 8
        Me.YMD.Name = "YMD"
        Me.YMD.ReadOnly = True
        Me.YMD.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.YMD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.YMD.Visible = False
        Me.YMD.Width = 150
        '
        'ECode
        '
        Me.ECode.DataPropertyName = "ECode"
        Me.ECode.Frozen = True
        Me.ECode.HeaderText = "ECode"
        Me.ECode.MinimumWidth = 8
        Me.ECode.Name = "ECode"
        Me.ECode.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ECode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ECode.Visible = False
        Me.ECode.Width = 55
        '
        'DeptName
        '
        Me.DeptName.DataPropertyName = "DeptName"
        Me.DeptName.Frozen = True
        Me.DeptName.HeaderText = "DeptName"
        Me.DeptName.MinimumWidth = 8
        Me.DeptName.Name = "DeptName"
        Me.DeptName.ReadOnly = True
        Me.DeptName.Visible = False
        Me.DeptName.Width = 60
        '
        'JCode
        '
        Me.JCode.DataPropertyName = "JCode"
        Me.JCode.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.JCode.Frozen = True
        Me.JCode.HeaderText = "JCode"
        Me.JCode.MinimumWidth = 8
        Me.JCode.Name = "JCode"
        Me.JCode.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.JCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.JCode.Width = 150
        '
        'SubPrcName
        '
        Me.SubPrcName.DataPropertyName = "SubPrcName"
        Me.SubPrcName.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.SubPrcName.Frozen = True
        Me.SubPrcName.HeaderText = "SubPrcName"
        Me.SubPrcName.MinimumWidth = 8
        Me.SubPrcName.Name = "SubPrcName"
        Me.SubPrcName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SubPrcName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.SubPrcName.Width = 150
        '
        'JCodeTemp
        '
        Me.JCodeTemp.DataPropertyName = "JCodeTemp"
        Me.JCodeTemp.Frozen = True
        Me.JCodeTemp.HeaderText = "MatCode Temp"
        Me.JCodeTemp.MinimumWidth = 8
        Me.JCodeTemp.Name = "JCodeTemp"
        Me.JCodeTemp.ReadOnly = True
        Me.JCodeTemp.Visible = False
        Me.JCodeTemp.Width = 150
        '
        'SubPrcNameTemp
        '
        Me.SubPrcNameTemp.DataPropertyName = "SubPrcNameTemp"
        Me.SubPrcNameTemp.Frozen = True
        Me.SubPrcNameTemp.HeaderText = "SubPrcNameTemp"
        Me.SubPrcNameTemp.MinimumWidth = 8
        Me.SubPrcNameTemp.Name = "SubPrcNameTemp"
        Me.SubPrcNameTemp.ReadOnly = True
        Me.SubPrcNameTemp.Visible = False
        Me.SubPrcNameTemp.Width = 150
        '
        'PrcName
        '
        Me.PrcName.DataPropertyName = "PrcName"
        Me.PrcName.HeaderText = "PrcName"
        Me.PrcName.MinimumWidth = 8
        Me.PrcName.Name = "PrcName"
        Me.PrcName.ReadOnly = True
        Me.PrcName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.PrcName.Width = 80
        '
        'JEName
        '
        Me.JEName.DataPropertyName = "JEName"
        Me.JEName.HeaderText = "JEName"
        Me.JEName.MinimumWidth = 8
        Me.JEName.Name = "JEName"
        Me.JEName.ReadOnly = True
        Me.JEName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.JEName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.JEName.Width = 150
        '
        'JVName
        '
        Me.JVName.DataPropertyName = "JVName"
        Me.JVName.HeaderText = "JVName"
        Me.JVName.MinimumWidth = 8
        Me.JVName.Name = "JVName"
        Me.JVName.ReadOnly = True
        Me.JVName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.JVName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.JVName.Width = 150
        '
        'Unit
        '
        Me.Unit.DataPropertyName = "Unit"
        Me.Unit.HeaderText = "Unit"
        Me.Unit.MinimumWidth = 8
        Me.Unit.Name = "Unit"
        Me.Unit.ReadOnly = True
        Me.Unit.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Unit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.Unit.Width = 40
        '
        'MinQty
        '
        Me.MinQty.DataPropertyName = "MinQty"
        Me.MinQty.HeaderText = "Min Qty"
        Me.MinQty.MinimumWidth = 8
        Me.MinQty.Name = "MinQty"
        Me.MinQty.ReadOnly = True
        Me.MinQty.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.MinQty.Width = 50
        '
        'StdDtbtQty
        '
        Me.StdDtbtQty.DataPropertyName = "StdDtbtQty"
        Me.StdDtbtQty.HeaderText = "Std Dtbt Qty"
        Me.StdDtbtQty.MinimumWidth = 8
        Me.StdDtbtQty.Name = "StdDtbtQty"
        Me.StdDtbtQty.ReadOnly = True
        Me.StdDtbtQty.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.StdDtbtQty.Width = 60
        '
        'NormWeekJ
        '
        Me.NormWeekJ.DataPropertyName = "NormWeek"
        Me.NormWeekJ.HeaderText = "Norm Week"
        Me.NormWeekJ.MinimumWidth = 8
        Me.NormWeekJ.Name = "NormWeekJ"
        Me.NormWeekJ.ReadOnly = True
        Me.NormWeekJ.Width = 60
        '
        'ReviseQty
        '
        Me.ReviseQty.DataPropertyName = "ReviseQty"
        Me.ReviseQty.HeaderText = "Revise Qty"
        Me.ReviseQty.MinimumWidth = 8
        Me.ReviseQty.Name = "ReviseQty"
        Me.ReviseQty.ReadOnly = True
        Me.ReviseQty.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ReviseQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ReviseQty.Visible = False
        Me.ReviseQty.Width = 50
        '
        'UseDay
        '
        Me.UseDay.DataPropertyName = "UseDay"
        DataGridViewCellStyle7.Format = "N0"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.UseDay.DefaultCellStyle = DataGridViewCellStyle7
        Me.UseDay.HeaderText = "Use Day Qty"
        Me.UseDay.MinimumWidth = 8
        Me.UseDay.Name = "UseDay"
        Me.UseDay.ReadOnly = True
        Me.UseDay.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.UseDay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.UseDay.Width = 40
        '
        'TotalDtbtQty
        '
        Me.TotalDtbtQty.DataPropertyName = "TotalDtbtQty"
        DataGridViewCellStyle8.Format = "N2"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.TotalDtbtQty.DefaultCellStyle = DataGridViewCellStyle8
        Me.TotalDtbtQty.HeaderText = "Total Dtbt Qty"
        Me.TotalDtbtQty.MinimumWidth = 8
        Me.TotalDtbtQty.Name = "TotalDtbtQty"
        Me.TotalDtbtQty.ReadOnly = True
        Me.TotalDtbtQty.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TotalDtbtQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.TotalDtbtQty.Width = 60
        '
        'Qty1
        '
        Me.Qty1.DataPropertyName = "Qty1"
        Me.Qty1.HeaderText = "Time 1"
        Me.Qty1.MinimumWidth = 8
        Me.Qty1.Name = "Qty1"
        Me.Qty1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Qty1.Width = 60
        '
        'Qty2
        '
        Me.Qty2.DataPropertyName = "Qty2"
        Me.Qty2.HeaderText = "Time 2"
        Me.Qty2.MinimumWidth = 8
        Me.Qty2.Name = "Qty2"
        Me.Qty2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Qty2.Width = 60
        '
        'Qty3
        '
        Me.Qty3.DataPropertyName = "Qty3"
        Me.Qty3.HeaderText = "Time 3"
        Me.Qty3.MinimumWidth = 8
        Me.Qty3.Name = "Qty3"
        Me.Qty3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Qty3.Width = 60
        '
        'Note
        '
        Me.Note.DataPropertyName = "Note"
        Me.Note.HeaderText = "Note"
        Me.Note.MinimumWidth = 8
        Me.Note.Name = "Note"
        Me.Note.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Note.Width = 60
        '
        'NgaySuDung
        '
        Me.NgaySuDung.DataPropertyName = "NgaySuDung"
        Me.NgaySuDung.HeaderText = "Ngày sử dụng"
        Me.NgaySuDung.MinimumWidth = 8
        Me.NgaySuDung.Name = "NgaySuDung"
        Me.NgaySuDung.Width = 80
        '
        'Adjust
        '
        Me.Adjust.DataPropertyName = "Adjust"
        Me.Adjust.HeaderText = "Adjust"
        Me.Adjust.MinimumWidth = 8
        Me.Adjust.Name = "Adjust"
        Me.Adjust.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Adjust.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.Adjust.Visible = False
        Me.Adjust.Width = 60
        '
        'TotalQty
        '
        Me.TotalQty.DataPropertyName = "TotalQty"
        Me.TotalQty.HeaderText = "Total Qty"
        Me.TotalQty.MinimumWidth = 8
        Me.TotalQty.Name = "TotalQty"
        Me.TotalQty.ReadOnly = True
        Me.TotalQty.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TotalQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.TotalQty.Width = 60
        '
        'ActReceive
        '
        Me.ActReceive.DataPropertyName = "ActReceive"
        Me.ActReceive.HeaderText = "Act Receive"
        Me.ActReceive.MinimumWidth = 8
        Me.ActReceive.Name = "ActReceive"
        Me.ActReceive.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ActReceive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ActReceive.Visible = False
        Me.ActReceive.Width = 60
        '
        'AVGUsing
        '
        Me.AVGUsing.DataPropertyName = "AVGUsing"
        DataGridViewCellStyle9.Format = "N2"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.AVGUsing.DefaultCellStyle = DataGridViewCellStyle9
        Me.AVGUsing.HeaderText = "Average Using"
        Me.AVGUsing.MinimumWidth = 8
        Me.AVGUsing.Name = "AVGUsing"
        Me.AVGUsing.ReadOnly = True
        Me.AVGUsing.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AVGUsing.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.AVGUsing.Visible = False
        Me.AVGUsing.Width = 60
        '
        'Total0131
        '
        Me.Total0131.DataPropertyName = "Total0131"
        DataGridViewCellStyle10.Format = "N2"
        DataGridViewCellStyle10.NullValue = Nothing
        Me.Total0131.DefaultCellStyle = DataGridViewCellStyle10
        Me.Total0131.HeaderText = "Total 01-31"
        Me.Total0131.MinimumWidth = 8
        Me.Total0131.Name = "Total0131"
        Me.Total0131.ReadOnly = True
        Me.Total0131.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Total0131.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.Total0131.Visible = False
        Me.Total0131.Width = 60
        '
        'RequestQty
        '
        Me.RequestQty.DataPropertyName = "RequestQty"
        Me.RequestQty.HeaderText = "Request Qty"
        Me.RequestQty.MinimumWidth = 8
        Me.RequestQty.Name = "RequestQty"
        Me.RequestQty.ReadOnly = True
        Me.RequestQty.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.RequestQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.RequestQty.Width = 60
        '
        'RequestDate
        '
        Me.RequestDate.DataPropertyName = "RequestDate"
        Me.RequestDate.HeaderText = "Request Date"
        Me.RequestDate.MinimumWidth = 8
        Me.RequestDate.Name = "RequestDate"
        Me.RequestDate.ReadOnly = True
        Me.RequestDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.RequestDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.RequestDate.Width = 150
        '
        'DeptGSR
        '
        Me.DeptGSR.DataPropertyName = "DeptGSR"
        Me.DeptGSR.HeaderText = "DeptGSR"
        Me.DeptGSR.MinimumWidth = 8
        Me.DeptGSR.Name = "DeptGSR"
        Me.DeptGSR.ReadOnly = True
        Me.DeptGSR.Width = 50
        '
        'DataGridViewAutoFilterTextBoxColumn8
        '
        Me.DataGridViewAutoFilterTextBoxColumn8.DataPropertyName = "Reason"
        Me.DataGridViewAutoFilterTextBoxColumn8.HeaderText = "Reason"
        Me.DataGridViewAutoFilterTextBoxColumn8.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn8.Name = "DataGridViewAutoFilterTextBoxColumn8"
        Me.DataGridViewAutoFilterTextBoxColumn8.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn8.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn5
        '
        Me.DataGridViewAutoFilterTextBoxColumn5.DataPropertyName = "Unit"
        Me.DataGridViewAutoFilterTextBoxColumn5.HeaderText = "Unit"
        Me.DataGridViewAutoFilterTextBoxColumn5.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn5.Name = "DataGridViewAutoFilterTextBoxColumn5"
        Me.DataGridViewAutoFilterTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn5.Visible = False
        Me.DataGridViewAutoFilterTextBoxColumn5.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn7
        '
        Me.DataGridViewAutoFilterTextBoxColumn7.DataPropertyName = "OrderDate"
        Me.DataGridViewAutoFilterTextBoxColumn7.HeaderText = "Order Date"
        Me.DataGridViewAutoFilterTextBoxColumn7.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn7.Name = "DataGridViewAutoFilterTextBoxColumn7"
        Me.DataGridViewAutoFilterTextBoxColumn7.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn7.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn4
        '
        Me.DataGridViewAutoFilterTextBoxColumn4.DataPropertyName = "Quantity"
        Me.DataGridViewAutoFilterTextBoxColumn4.HeaderText = "Quantity"
        Me.DataGridViewAutoFilterTextBoxColumn4.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn4.Name = "DataGridViewAutoFilterTextBoxColumn4"
        Me.DataGridViewAutoFilterTextBoxColumn4.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn4.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn3
        '
        Me.DataGridViewAutoFilterTextBoxColumn3.DataPropertyName = "Air"
        Me.DataGridViewAutoFilterTextBoxColumn3.HeaderText = "Air"
        Me.DataGridViewAutoFilterTextBoxColumn3.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn3.Name = "DataGridViewAutoFilterTextBoxColumn3"
        Me.DataGridViewAutoFilterTextBoxColumn3.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn3.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn2
        '
        Me.DataGridViewAutoFilterTextBoxColumn2.DataPropertyName = "PackingUnit"
        Me.DataGridViewAutoFilterTextBoxColumn2.HeaderText = "Packing Unit"
        Me.DataGridViewAutoFilterTextBoxColumn2.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn2.Name = "DataGridViewAutoFilterTextBoxColumn2"
        Me.DataGridViewAutoFilterTextBoxColumn2.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn2.Visible = False
        Me.DataGridViewAutoFilterTextBoxColumn2.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn6
        '
        Me.DataGridViewAutoFilterTextBoxColumn6.DataPropertyName = "FullName"
        Me.DataGridViewAutoFilterTextBoxColumn6.HeaderText = "Employee"
        Me.DataGridViewAutoFilterTextBoxColumn6.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn6.Name = "DataGridViewAutoFilterTextBoxColumn6"
        Me.DataGridViewAutoFilterTextBoxColumn6.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn6.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn1
        '
        Me.DataGridViewAutoFilterTextBoxColumn1.DataPropertyName = "JName"
        Me.DataGridViewAutoFilterTextBoxColumn1.HeaderText = "JName"
        Me.DataGridViewAutoFilterTextBoxColumn1.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn1.Name = "DataGridViewAutoFilterTextBoxColumn1"
        Me.DataGridViewAutoFilterTextBoxColumn1.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoFilterTextBoxColumn1.Width = 150
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "ID"
        Me.DataGridViewTextBoxColumn4.HeaderText = "GSR ID"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 150
        '
        'DataGridViewAutoFilterTextBoxColumn9
        '
        Me.DataGridViewAutoFilterTextBoxColumn9.DataPropertyName = "IsLock"
        Me.DataGridViewAutoFilterTextBoxColumn9.HeaderText = "IsLock"
        Me.DataGridViewAutoFilterTextBoxColumn9.MinimumWidth = 8
        Me.DataGridViewAutoFilterTextBoxColumn9.Name = "DataGridViewAutoFilterTextBoxColumn9"
        Me.DataGridViewAutoFilterTextBoxColumn9.ReadOnly = True
        Me.DataGridViewAutoFilterTextBoxColumn9.Width = 150
        '
        'CalendarColumn1
        '
        Me.CalendarColumn1.HeaderText = "Delivery Date"
        Me.CalendarColumn1.MinimumWidth = 8
        Me.CalendarColumn1.Name = "CalendarColumn1"
        Me.CalendarColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CalendarColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.CalendarColumn1.Width = 150
        '
        'FrmOutMaterialAndSubMaterial
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 692)
        Me.Controls.Add(Me.rdoEx3)
        Me.Controls.Add(Me.rdoEx2)
        Me.Controls.Add(Me.rdoEx1)
        Me.Controls.Add(Me.lblShowJCode)
        Me.Controls.Add(Me.cboShowJCode)
        Me.Controls.Add(Me.grpGrid)
        Me.Controls.Add(Me.grpInput)
        Me.Controls.Add(Me.tlsMenu)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "FrmOutMaterialAndSubMaterial"
        Me.Tag = "021205"
        Me.Text = "Output Material And Sub Material"
        Me.grpInput.ResumeLayout(False)
        Me.grpInput.PerformLayout()
        Me.tlsMenu.ResumeLayout(False)
        Me.tlsMenu.PerformLayout()
        CType(Me.bnGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.bnGrid.ResumeLayout(False)
        Me.bnGrid.PerformLayout()
        Me.grpGrid.ResumeLayout(False)
        Me.grpGrid.PerformLayout()
        Me.PanelSearch.ResumeLayout(False)
        Me.PanelSearch.PerformLayout()
        CType(Me.gridSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridD, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpInput As System.Windows.Forms.GroupBox
    Friend WithEvents lblDay As System.Windows.Forms.Label
    Friend WithEvents mnuExport As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents tlsMenu As System.Windows.Forms.ToolStrip
    Friend WithEvents mnuShowAll As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuDelete As System.Windows.Forms.ToolStripButton
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
    Friend WithEvents CalendarColumn1 As UtilityControl.CalendarColumn
    Friend WithEvents chkLocked3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLocked2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLocked1 As System.Windows.Forms.CheckBox
    Friend WithEvents txtOutputNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputNumber As System.Windows.Forms.Label
    Friend WithEvents dtpOrderDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents bnGrid As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsStock As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblFilterSubPrcName As System.Windows.Forms.Label
    Friend WithEvents lblFilterMatCode As System.Windows.Forms.Label
    Friend WithEvents cboSubPrcNameFilter As System.Windows.Forms.ComboBox
    Friend WithEvents cboJCodeFilter As System.Windows.Forms.ComboBox
    Friend WithEvents grpGrid As System.Windows.Forms.GroupBox
    Friend WithEvents lblFilterECode As System.Windows.Forms.Label
    Friend WithEvents cboECodeFilter As System.Windows.Forms.ComboBox
    Friend WithEvents gridD As UtilityControl.CustomDataGridView
    Friend WithEvents PanelSearch As System.Windows.Forms.Panel
    Friend WithEvents gridSearch As System.Windows.Forms.DataGridView
    Friend WithEvents txtJCodeSearch As System.Windows.Forms.TextBox
    Friend WithEvents txtJVNameSearch As System.Windows.Forms.TextBox
    Friend WithEvents txtJENameSearch As System.Windows.Forms.TextBox
    Friend WithEvents JCodeSearch As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JENameSearch As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JVNameSearch As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnCheckLock As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuCheckQty As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents tslblTotal1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsTotal1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tslblTotal2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsTotal2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tslblTotal3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsTotal3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblShowJCode As System.Windows.Forms.Label
    Friend WithEvents cboShowJCode As System.Windows.Forms.ComboBox
    Friend WithEvents mnuNJCStock As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuExportSum As System.Windows.Forms.ToolStripButton
    Friend WithEvents rdoEx1 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoEx2 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoEx3 As System.Windows.Forms.RadioButton
    Friend WithEvents tsInsStock As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsInsStockD As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsNonInsStock As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsNonInsStockD As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents mnuTLImportLemon As System.Windows.Forms.ToolStripButton
	Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
	Friend WithEvents txtSoPhieu As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents YMD As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ECode As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DeptName As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents JCode As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents SubPrcName As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents JCodeTemp As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SubPrcNameTemp As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrcName As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents JEName As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JVName As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Unit As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MinQty As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents StdDtbtQty As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents NormWeekJ As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents ReviseQty As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UseDay As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TotalDtbtQty As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Qty1 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents Qty2 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents Qty3 As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents Note As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents NgaySuDung As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Adjust As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TotalQty As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActReceive As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AVGUsing As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Total0131 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RequestQty As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RequestDate As UtilityControl.CalendarColumn
    Friend WithEvents DeptGSR As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
End Class
