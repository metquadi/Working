﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSumDataKT1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSumDataKT1))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.mnuExport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuShowAll = New System.Windows.Forms.ToolStripButton()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.tlsMenu = New System.Windows.Forms.ToolStrip()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPdCode = New System.Windows.Forms.TextBox()
        Me.txtLotNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grid = New System.Windows.Forms.DataGridView()
        Me.bdn = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Ngay = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.Customer = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.ProductCode = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.ProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LotNo = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.SoTrang = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.Nhom = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.Phong = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.LotQty = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.AQL = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.TotalAQL = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DiffAQL = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.TotalDefect = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.Evaluate = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.EmpID = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.SL = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.ThoiGIan = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DefectCode = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.DefectQty = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.EmpIDPr3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Shift = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ThoiGianRM = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.GhiChu = New DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn()
        Me.tlsMenu.SuspendLayout()
        CType(Me.grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bdn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.bdn.SuspendLayout()
        Me.SuspendLayout()
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
        Me.mnuExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.mnuExport.ToolTipText = "Export(Ctrl+E)"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 55)
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
        Me.mnuShowAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.mnuShowAll.ToolTipText = "Show all (F5)"
        '
        'dtpStart
        '
        Me.dtpStart.CustomFormat = "dd-MM-yyyy"
        Me.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStart.Location = New System.Drawing.Point(233, 9)
        Me.dtpStart.Margin = New System.Windows.Forms.Padding(2)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(98, 20)
        Me.dtpStart.TabIndex = 23
        '
        'tlsMenu
        '
        Me.tlsMenu.AutoSize = False
        Me.tlsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tlsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowAll, Me.mnuExport, Me.ToolStripSeparator6})
        Me.tlsMenu.Location = New System.Drawing.Point(0, 0)
        Me.tlsMenu.Name = "tlsMenu"
        Me.tlsMenu.Size = New System.Drawing.Size(973, 55)
        Me.tlsMenu.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(196, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Start"
        '
        'txtPdCode
        '
        Me.txtPdCode.Location = New System.Drawing.Point(351, 29)
        Me.txtPdCode.Name = "txtPdCode"
        Me.txtPdCode.Size = New System.Drawing.Size(76, 20)
        Me.txtPdCode.TabIndex = 26
        '
        'txtLotNo
        '
        Me.txtLotNo.Location = New System.Drawing.Point(433, 29)
        Me.txtLotNo.Name = "txtLotNo"
        Me.txtLotNo.Size = New System.Drawing.Size(76, 20)
        Me.txtLotNo.TabIndex = 27
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(348, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 30
        Me.Label2.Text = "PdCode"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(430, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 13)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "LotNo"
        '
        'grid
        '
        Me.grid.AllowUserToAddRows = False
        Me.grid.AllowUserToDeleteRows = False
        Me.grid.AllowUserToResizeRows = False
        Me.grid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grid.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.Ngay, Me.Customer, Me.ProductCode, Me.ProductName, Me.LotNo, Me.SoTrang, Me.Nhom, Me.Phong, Me.LotQty, Me.AQL, Me.TotalAQL, Me.DiffAQL, Me.TotalDefect, Me.Evaluate, Me.EmpID, Me.SL, Me.ThoiGIan, Me.DefectCode, Me.DefectQty, Me.EmpIDPr3, Me.Shift, Me.ThoiGianRM, Me.GhiChu})
        Me.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.grid.EnableHeadersVisualStyles = False
        Me.grid.Location = New System.Drawing.Point(0, 54)
        Me.grid.Margin = New System.Windows.Forms.Padding(2)
        Me.grid.Name = "grid"
        Me.grid.ReadOnly = True
        Me.grid.RowHeadersWidth = 20
        Me.grid.Size = New System.Drawing.Size(973, 410)
        Me.grid.TabIndex = 32
        '
        'bdn
        '
        Me.bdn.AddNewItem = Nothing
        Me.bdn.CountItem = Me.BindingNavigatorCountItem
        Me.bdn.DeleteItem = Nothing
        Me.bdn.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bdn.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2})
        Me.bdn.Location = New System.Drawing.Point(0, 466)
        Me.bdn.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.bdn.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.bdn.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.bdn.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.bdn.Name = "bdn"
        Me.bdn.PositionItem = Me.BindingNavigatorPositionItem
        Me.bdn.Size = New System.Drawing.Size(973, 25)
        Me.bdn.TabIndex = 33
        Me.bdn.Text = "BindingNavigator1"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(35, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
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
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(196, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 13)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "End"
        '
        'dtpEnd
        '
        Me.dtpEnd.CustomFormat = "dd-MM-yyyy"
        Me.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEnd.Location = New System.Drawing.Point(233, 30)
        Me.dtpEnd.Margin = New System.Windows.Forms.Padding(2)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(98, 20)
        Me.dtpEnd.TabIndex = 34
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.Visible = False
        '
        'Ngay
        '
        Me.Ngay.DataPropertyName = "Ngay"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.Format = "dd-MM-yyyy"
        Me.Ngay.DefaultCellStyle = DataGridViewCellStyle1
        Me.Ngay.HeaderText = "Ngày"
        Me.Ngay.Name = "Ngay"
        Me.Ngay.ReadOnly = True
        Me.Ngay.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Ngay.Width = 60
        '
        'Customer
        '
        Me.Customer.DataPropertyName = "Customer"
        Me.Customer.HeaderText = "Customer"
        Me.Customer.Name = "Customer"
        Me.Customer.ReadOnly = True
        '
        'ProductCode
        '
        Me.ProductCode.DataPropertyName = "ProductCode"
        Me.ProductCode.HeaderText = "ProductCode"
        Me.ProductCode.Name = "ProductCode"
        Me.ProductCode.ReadOnly = True
        Me.ProductCode.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ProductCode.Width = 60
        '
        'ProductName
        '
        Me.ProductName.DataPropertyName = "ProductName"
        Me.ProductName.HeaderText = "ProductName"
        Me.ProductName.Name = "ProductName"
        Me.ProductName.ReadOnly = True
        '
        'LotNo
        '
        Me.LotNo.DataPropertyName = "LotNo"
        Me.LotNo.HeaderText = "LotNo"
        Me.LotNo.Name = "LotNo"
        Me.LotNo.ReadOnly = True
        Me.LotNo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LotNo.Width = 60
        '
        'SoTrang
        '
        Me.SoTrang.DataPropertyName = "SoTrang"
        Me.SoTrang.HeaderText = "SoTrang"
        Me.SoTrang.Name = "SoTrang"
        Me.SoTrang.ReadOnly = True
        Me.SoTrang.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SoTrang.Width = 50
        '
        'Nhom
        '
        Me.Nhom.DataPropertyName = "Nhom"
        Me.Nhom.HeaderText = "Nhom"
        Me.Nhom.Name = "Nhom"
        Me.Nhom.ReadOnly = True
        Me.Nhom.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Nhom.Width = 50
        '
        'Phong
        '
        Me.Phong.DataPropertyName = "Phong"
        Me.Phong.HeaderText = "Phong"
        Me.Phong.Name = "Phong"
        Me.Phong.ReadOnly = True
        Me.Phong.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Phong.Width = 50
        '
        'LotQty
        '
        Me.LotQty.DataPropertyName = "LotQty"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "N0"
        Me.LotQty.DefaultCellStyle = DataGridViewCellStyle2
        Me.LotQty.HeaderText = "LotQty"
        Me.LotQty.Name = "LotQty"
        Me.LotQty.ReadOnly = True
        Me.LotQty.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LotQty.Width = 50
        '
        'AQL
        '
        Me.AQL.DataPropertyName = "AQL"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "N0"
        Me.AQL.DefaultCellStyle = DataGridViewCellStyle3
        Me.AQL.HeaderText = "AQL"
        Me.AQL.Name = "AQL"
        Me.AQL.ReadOnly = True
        Me.AQL.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AQL.Width = 50
        '
        'TotalAQL
        '
        Me.TotalAQL.DataPropertyName = "TotalAQL"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N0"
        Me.TotalAQL.DefaultCellStyle = DataGridViewCellStyle4
        Me.TotalAQL.HeaderText = "TotalAQL"
        Me.TotalAQL.Name = "TotalAQL"
        Me.TotalAQL.ReadOnly = True
        Me.TotalAQL.Width = 60
        '
        'DiffAQL
        '
        Me.DiffAQL.DataPropertyName = "DiffAQL"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N0"
        Me.DiffAQL.DefaultCellStyle = DataGridViewCellStyle5
        Me.DiffAQL.HeaderText = "DiffAQL"
        Me.DiffAQL.Name = "DiffAQL"
        Me.DiffAQL.ReadOnly = True
        Me.DiffAQL.Width = 60
        '
        'TotalDefect
        '
        Me.TotalDefect.DataPropertyName = "TotalDefect"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N0"
        Me.TotalDefect.DefaultCellStyle = DataGridViewCellStyle6
        Me.TotalDefect.HeaderText = "TotalDefect"
        Me.TotalDefect.Name = "TotalDefect"
        Me.TotalDefect.ReadOnly = True
        Me.TotalDefect.Width = 60
        '
        'Evaluate
        '
        Me.Evaluate.DataPropertyName = "Evaluate"
        Me.Evaluate.HeaderText = "Evaluate"
        Me.Evaluate.Name = "Evaluate"
        Me.Evaluate.ReadOnly = True
        Me.Evaluate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Evaluate.Width = 60
        '
        'EmpID
        '
        Me.EmpID.DataPropertyName = "EmpID"
        Me.EmpID.HeaderText = "EmpID"
        Me.EmpID.Name = "EmpID"
        Me.EmpID.ReadOnly = True
        Me.EmpID.Width = 50
        '
        'SL
        '
        Me.SL.DataPropertyName = "SL"
        Me.SL.HeaderText = "Số mẫu rút"
        Me.SL.Name = "SL"
        Me.SL.ReadOnly = True
        Me.SL.Width = 50
        '
        'ThoiGIan
        '
        Me.ThoiGIan.DataPropertyName = "ThoiGIan"
        Me.ThoiGIan.HeaderText = "Thời gian"
        Me.ThoiGIan.Name = "ThoiGIan"
        Me.ThoiGIan.ReadOnly = True
        Me.ThoiGIan.Width = 50
        '
        'DefectCode
        '
        Me.DefectCode.DataPropertyName = "DefectCode"
        Me.DefectCode.HeaderText = "DefectCode"
        Me.DefectCode.Name = "DefectCode"
        Me.DefectCode.ReadOnly = True
        Me.DefectCode.Width = 50
        '
        'DefectQty
        '
        Me.DefectQty.DataPropertyName = "DefectQty"
        Me.DefectQty.HeaderText = "DefectQty"
        Me.DefectQty.Name = "DefectQty"
        Me.DefectQty.ReadOnly = True
        Me.DefectQty.Width = 50
        '
        'EmpIDPr3
        '
        Me.EmpIDPr3.DataPropertyName = "EmpIDPr3"
        Me.EmpIDPr3.HeaderText = "MSNV Sốt Lỗi"
        Me.EmpIDPr3.Name = "EmpIDPr3"
        Me.EmpIDPr3.ReadOnly = True
        '
        'Shift
        '
        Me.Shift.DataPropertyName = "Shift"
        Me.Shift.HeaderText = "Nhóm"
        Me.Shift.Name = "Shift"
        Me.Shift.ReadOnly = True
        '
        'ThoiGianRM
        '
        Me.ThoiGianRM.DataPropertyName = "ThoiGianRM"
        Me.ThoiGianRM.HeaderText = "Thời gian RM"
        Me.ThoiGianRM.Name = "ThoiGianRM"
        Me.ThoiGianRM.ReadOnly = True
        Me.ThoiGianRM.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ThoiGianRM.Width = 90
        '
        'GhiChu
        '
        Me.GhiChu.DataPropertyName = "GhiChu"
        Me.GhiChu.HeaderText = "Ghi chú"
        Me.GhiChu.Name = "GhiChu"
        Me.GhiChu.ReadOnly = True
        Me.GhiChu.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'FrmSumDataKT1
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(973, 491)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.bdn)
        Me.Controls.Add(Me.grid)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtLotNo)
        Me.Controls.Add(Me.txtPdCode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.tlsMenu)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "FrmSumDataKT1"
        Me.Tag = "0243QA06"
        Me.Text = "Sum DataKT1"
        Me.tlsMenu.ResumeLayout(False)
        Me.tlsMenu.PerformLayout()
        CType(Me.grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bdn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.bdn.ResumeLayout(False)
        Me.bdn.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnuExport As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuShowAll As System.Windows.Forms.ToolStripButton
    Friend WithEvents dtpStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents tlsMenu As System.Windows.Forms.ToolStrip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPdCode As System.Windows.Forms.TextBox
    Friend WithEvents txtLotNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents grid As System.Windows.Forms.DataGridView
    Friend WithEvents bdn As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dtpEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents ID As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ngay As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents Customer As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents ProductCode As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents ProductName As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LotNo As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents SoTrang As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents Nhom As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents Phong As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents LotQty As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents AQL As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents TotalAQL As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DiffAQL As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents TotalDefect As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents Evaluate As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents EmpID As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents SL As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents ThoiGIan As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DefectCode As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents DefectQty As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents EmpIDPr3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Shift As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ThoiGianRM As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
    Friend WithEvents GhiChu As DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn
End Class
