﻿Imports CommonDB
Imports LibEntity
Imports PublicUtility
Imports System.Windows.Forms


Public Class FrmDataKTPL : Inherits DevExpress.XtraEditors.XtraForm

    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim _dbFpics As New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)


    Sub LoadShipTo()
        Dim sql As String = String.Format("select Shipto from QA_ShipTo")
        cboShipto.DataSource = _db.FillDataTable(sql)
        cboShipto.ValueMember = "ShipTo"
        cboShipto.DisplayMember = "ShipTo"
        cboShipto.SelectedIndex = -1
    End Sub

    Function GetLotNo(ByVal pdcode As String, ByVal lotno As String) As String
        Dim obj As New t_ManufactureLot
        obj.ProductCode_K = pdcode
        obj.LotNumber_K = lotno
        obj.ComponentCode_K = "B00"
        _dbFpics.GetObject(obj)
        If obj.EntryDate > DateTime.MinValue Then
            Return lotno & "-" & Microsoft.VisualBasic.Right(obj.EntryDate.Year, 1) & obj.EntryDate.ToString("MM")
        Else
            Return lotno
        End If
    End Function

    Private Sub mnuImport_Click(sender As System.Object, e As System.EventArgs) Handles mnuImport.Click

        'If grid.Rows.Count > 0 Then
        '    ShowWarning("Dữ liệu đã import rồi không được import lại.")
        '    Return
        'End If
        If ShowQuestion("Get dữ liệu sẽ xóa dữ liệu cũ và lấy lại mới hoàn toàn." & vbCrLf &
                        "Bạn có chắc muốn get dữ liệu không ?") = Windows.Forms.DialogResult.Yes Then

            Dim para(1) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Start", GetStartDate(dtpStart.Value))
            para(1) = New SqlClient.SqlParameter("@End", GetEndDate(dtpStart.Value))
            _db.ExecuteStoreProcedure("sp_MDQA_GetKTPL", para)

            mnuShowAll.PerformClick()
        End If
    End Sub

    Private Sub mnuShowAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuShowAll.Click
        Dim sql As String = String.Format("[sp_MDQA_LoadKTPL_Head]")
        Dim para(2) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Start", GetStartDate(dtpStart.Value))
        para(1) = New SqlClient.SqlParameter("@End", GetStartDate(dtpEnd.Value))
        If txtPdCode.Text <> "" Then
            para(2) = New SqlClient.SqlParameter("@ProductCode", txtPdCode.Text)
        Else
            para(2) = New SqlClient.SqlParameter("@ProductCode", DBNull.Value)
        End If
        Dim bd As New BindingSource
        bd.DataSource = _db.ExecuteStoreProcedureTB(sql, para)
        bdn.BindingSource = bd
        grid.DataSource = bd
    End Sub

    Private Sub gridD_CellValueChanged(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridD.CellValueChanged

        If e.RowIndex >= 0 Then
            Dim obj As New MDQA_KTPL_Detail
            obj.ID = grid.CurrentRow.Cells("ID").Value
            If gridD.CurrentRow.Cells("IDD").Value IsNot DBNull.Value Then
                obj.IDD_K = gridD.CurrentRow.Cells("IDD").Value
            Else
                obj.IDD_K = Guid.NewGuid.ToString
                gridD.CurrentRow.Cells("IDD").Value = obj.IDD_K
                obj.CreateDate = DateTime.Now
            End If

            If gridD.CurrentRow.Cells("ThoiGian").Value IsNot DBNull.Value Then
                obj.ThoiGian = gridD.CurrentRow.Cells("ThoiGian").Value
            Else
                obj.ThoiGian = 0
            End If

            If gridD.CurrentRow.Cells("STT").Value IsNot DBNull.Value Then
                obj.STT = gridD.CurrentRow.Cells("STT").Value
                If e.ColumnIndex = gridD.Columns("STT").Index Then
                    Dim obje As MDQA_Employee = _db.GetObject(Of MDQA_Employee)(String.Format("select * from MDQA_Employee where STT={0}", obj.STT))
                    If Not _db.ExistObject(obje) Then
                        ShowWarning("Số STT viên không nằm trong danh sách.")
                        Return
                    Else
                        obj.EmpID = obje.EmpID_K
                        gridD.CurrentRow.Cells("EmpID").Value = obje.EmpID_K
                    End If
                End If
            End If
         

            If gridD.CurrentRow.Cells("EmpID").Value IsNot DBNull.Value Then
                obj.EmpID = gridD.CurrentRow.Cells("EmpID").Value
            End If

            If gridD.CurrentRow.Cells("SL").Value IsNot DBNull.Value Then
                obj.SL = gridD.CurrentRow.Cells("SL").Value
            End If

            If gridD.CurrentRow.Cells("DefectCode").Value IsNot DBNull.Value Then
                obj.DefectCode = gridD.CurrentRow.Cells("DefectCode").Value
                obj.DefectCode = obj.DefectCode.PadLeft(3, "0")
                gridD.CurrentRow.Cells("DefectCode").Value = obj.DefectCode
                If e.ColumnIndex = gridD.Columns("DefectCode").Index Then
                    Dim obje As New MDQA_DefectCode
                    obje.DefectCode_K = obj.DefectCode
                    If Not _db.ExistObject(obje) Then
                        ShowWarning("Mã lỗi không nằm trong danh sách.")
                        Return
                    End If
                End If
            End If

            If gridD.CurrentRow.Cells("DefectQty").Value IsNot DBNull.Value Then
                obj.DefectQty = gridD.CurrentRow.Cells("DefectQty").Value
            End If

            If gridD.CurrentRow.Cells("ShortNo").Value IsNot DBNull.Value Then
                obj.ShortNo = gridD.CurrentRow.Cells("ShortNo").Value
                Dim dt = _db.FillDataTable(String.Format("SELECT Shift, h.EmpID
                                                          FROM OT_Employee AS h
                                                          LEFT JOIN  MDQA_Employee_PR3 AS d
                                                          ON h.EmpID = d.EmpID
                                                          WHERE ShortNo = '{0}'", obj.ShortNo))
                Dim shift As Object = ""
                Dim empID As Object = ""
                If dt.Rows.Count > 0 Then
                    shift = dt.Rows(0)("Shift")
                    empID = dt.Rows(0)("EmpID")
                Else
                    shift = obj.ShortNo
                    empID = obj.ShortNo
                End If
                obj.EmpIDPr3 = empID
                obj.Shift = shift
                gridD.CurrentRow.Cells("EmpIDPr3").Value = empID
                gridD.CurrentRow.Cells("Shift").Value = shift
            End If

            If obj.IDD_K <> "" And obj.EmpID <> "" Then
                If _db.ExistObject(obj) Then
                    _db.Update(obj)
                Else
                    _db.Insert(obj)
                End If
                If e.ColumnIndex = gridD.Columns("ThoiGian").Index Or
                   e.ColumnIndex = gridD.Columns("SL").Index Or
                   e.ColumnIndex = gridD.Columns("DefectQty").Index Then
                    Dim slMau As Decimal = 0
                    Dim slLoi As Decimal = 0
                    Dim slthoigian As Decimal = 0
                    For Each r As DataGridViewRow In gridD.Rows
                        If r.Cells("ThoiGian").Value IsNot DBNull.Value Then
                            slthoigian += r.Cells("ThoiGian").Value
                        End If
                        If r.Cells("SL").Value IsNot DBNull.Value Then
                            slMau += r.Cells("SL").Value
                        End If
                        If r.Cells("DefectQty").Value IsNot DBNull.Value Then
                            slLoi += r.Cells("DefectQty").Value
                        End If
                    Next
                    grid.CurrentRow.Cells("ThoiGianRM").Value = slthoigian
                    grid.CurrentRow.Cells("TotalAQL").Value = slMau
                    grid.CurrentRow.Cells("TotalDefect").Value = slLoi
                    If slLoi > 0 Then
                        grid.CurrentRow.Cells("Evaluate").Value = "NG"
                    Else
                        grid.CurrentRow.Cells("Evaluate").Value = "OK"
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub grid_CellValueChanged(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grid.CellValueChanged
        If e.RowIndex >= 0 Then
            Dim obj As New MDQA_KTPL
            obj.ID_K = grid.CurrentRow.Cells("ID").Value
            _db.GetObject(obj)
            If obj.CreateDate = DateTime.MinValue Then
                obj.CreateDate = DateTime.Now
            End If
            If e.ColumnIndex = grid.Columns("SoTrang").Index Then
                If grid.CurrentRow.Cells("SoTrang").Value IsNot DBNull.Value Then
                    obj.SoTrang = grid.CurrentRow.Cells("SoTrang").Value
                Else
                    obj.SoTrang = ""
                End If
            ElseIf e.ColumnIndex = grid.Columns("Phong").Index Then
                If grid.CurrentRow.Cells("Phong").Value IsNot DBNull.Value Then
                    obj.Phong = grid.CurrentRow.Cells("Phong").Value
                Else
                    obj.Phong = ""
                End If
            ElseIf e.ColumnIndex = grid.Columns("ThoiGianRM").Index Then
                If grid.CurrentRow.Cells("ThoiGianRM").Value IsNot DBNull.Value Then
                    obj.ThoiGianRM = grid.CurrentRow.Cells("ThoiGianRM").Value
                Else
                    obj.ThoiGianRM = ""
                End If
            ElseIf e.ColumnIndex = grid.Columns("LotQty").Index Then
                If grid.CurrentRow.Cells("LotQty").Value IsNot DBNull.Value Then
                    obj.LotQty = grid.CurrentRow.Cells("LotQty").Value
                Else
                    obj.LotQty = 0
                End If
            ElseIf e.ColumnIndex = grid.Columns("AQL").Index Then
                If grid.CurrentRow.Cells("AQL").Value IsNot DBNull.Value Then
                    obj.AQL = grid.CurrentRow.Cells("AQL").Value
                Else
                    obj.AQL = ""
                End If
            ElseIf e.ColumnIndex = grid.Columns("GhiChu").Index Then
                If grid.CurrentRow.Cells("GhiChu").Value IsNot DBNull.Value Then
                    obj.GhiChu = grid.CurrentRow.Cells("GhiChu").Value
                Else
                    obj.GhiChu = ""
                End If
            ElseIf e.ColumnIndex = grid.Columns("Ngay").Index Then
                If grid.CurrentRow.Cells("Ngay").Value IsNot DBNull.Value Then
                    obj.Ngay = grid.CurrentRow.Cells("Ngay").Value

                    If obj.Ngay < DateTime.Now.AddMonths(-2).Date Or obj.Ngay > DateTime.Now.Date Then
                        ShowWarning("Không được nhập quá 2 tháng của quá khứ hoặc lớn hơn ngày hiện tại !")
                        Return
                    End If
                Else
                    obj.Ngay = Nothing
                End If

            ElseIf e.ColumnIndex = grid.Columns("NgayEntek").Index Then
                If grid.CurrentRow.Cells("NgayEntek").Value IsNot DBNull.Value Then
                    obj.NgayEntek = grid.CurrentRow.Cells("NgayEntek").Value

                    If obj.NgayEntek < DateTime.Now.AddMonths(-2).Date Or obj.Ngay > DateTime.Now.Date Then
                        ShowWarning("Không được nhập quá 2 tháng của quá khứ hoặc lớn hơn ngày hiện tại !")
                        Return
                    End If
                Else
                    obj.NgayEntek = Nothing
                End If

            ElseIf e.ColumnIndex = grid.Columns("LotNo").Index Then
                If grid.CurrentRow.Cells("LotNo").Value IsNot DBNull.Value Then
                    obj.LotNo = grid.CurrentRow.Cells("LotNo").Value
                Else
                    obj.LotNo = ""
                End If
            ElseIf e.ColumnIndex = grid.Columns(CuDiemXuat.Name).Index Then
                If grid.CurrentRow.Cells("CuDiemXuat").Value IsNot DBNull.Value Then
                    obj.CuDiemXuat = grid.CurrentRow.Cells("CuDiemXuat").Value
                Else
                    obj.CuDiemXuat = ""
                End If
            End If
            _db.Update(obj)
        End If
    End Sub

    Private Sub mnuXoa_Click(sender As System.Object, e As System.EventArgs) Handles mnuXoa.Click
        If grid.CurrentRow IsNot Nothing Then
            Dim objLock As New MDQA_Lock
            objLock.Ngay_K = grid.CurrentRow.Cells("Ngay").Value
            _db.GetObject(objLock)
            If objLock.Lock Then
                ShowWarning("Dữ liệu ngày này đã bị khóa không thể sửa.")
                Return
            End If
            If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
                For Each r As DataGridViewRow In grid.SelectedRows
                    Dim obj As New MDQA_KTPL
                    obj.ID_K = r.Cells("ID").Value
                    _db.Delete(obj)
                Next
                mnuShowAll.PerformClick()
            End If
        End If
    End Sub

    Private Sub mnuXoaD_Click(sender As System.Object, e As System.EventArgs) Handles mnuXoaD.Click
        If grid.CurrentRow IsNot Nothing Then
            Dim objLock As New MDQA_Lock
            objLock.Ngay_K = grid.CurrentRow.Cells("Ngay").Value
            _db.GetObject(objLock)
            If objLock.Lock Then
                ShowWarning("Dữ liệu ngày này đã bị khóa không thể sửa.")
                Return
            End If
            If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
                For Each r As DataGridViewRow In gridD.SelectedRows
                    Dim obj As New MDQA_KTPL_Detail
                    obj.IDD_K = r.Cells("IDD").Value
                    _db.Delete(obj)
                Next
                grid_CellClick(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub grid_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grid.CellClick
        Dim sql = String.Format("SELECT IDD, ID, STT, EmpID, SL, ThoiGian, DefectCode, DefectQty, ShortNo, EmpIDPr3, Shift
                                 FROM MDQA_KTPL_Detail
                                 WHERE ID = '{0}'
                                 ORDER BY CreateDate",
                                 grid.CurrentRow.Cells("ID").Value)
        Dim bd As New BindingSource
        bd.DataSource = _db.FillDataTable(sql)
        bdnD.BindingSource = bd
        gridD.DataSource = bd
        If e IsNot Nothing Then
            If e.ColumnIndex = grid.Columns("BaoCao").Index Then
                Dim obj As New MDQA_KTPL
                obj.ID_K = grid.CurrentRow.Cells("ID").Value
                _db.GetObject(obj)
                obj.BaoCao = Not obj.BaoCao
                _db.Update(obj)
                grid.CurrentRow.Cells("BaoCao").Value = obj.BaoCao
            End If
        End If
    End Sub

    Private Sub FrmDataKTPL_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtghichu.Focused = False Then
                SendKeys.Send("{Tab}")
            End If
        End If
        If e.KeyCode = Keys.S And e.Control Then
            tssSave.PerformClick()
        End If
    End Sub 

    Private Sub FrmDataKTPL_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        mnuShowAll.PerformClick()
        LoadShipTo()
    End Sub
    

    Private Sub txtPdCode_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPdCode.TextChanged
        If bdn.BindingSource IsNot Nothing Then
            If txtPdCode.Text = "" Then
                bdn.BindingSource.Filter = ""
                If txtLotNo.Text = "" Then
                    bdn.BindingSource.Filter = ""
                Else
                    bdn.BindingSource.Filter = String.Format("LotNo like '%{0}%' ", txtLotNo.Text)
                End If
            Else
                If txtLotNo.Text = "" Then
                    bdn.BindingSource.Filter = String.Format("productcode like '%{0}%'", txtPdCode.Text)
                Else
                    bdn.BindingSource.Filter = String.Format("productcode like '%{0}%' and LotNo like '%{1}%' ",
                                                             txtPdCode.Text, txtLotNo.Text)
                End If
            End If
        End If
    End Sub

    Private Sub txtLotNo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtLotNo.TextChanged
        If bdn.BindingSource IsNot Nothing Then
            If txtLotNo.Text = "" Then
                If txtPdCode.Text = "" Then
                    bdn.BindingSource.Filter = ""
                Else
                      bdn.BindingSource.Filter = String.Format("productcode like '%{0}%'", txtPdCode.Text)
                End If
            Else
                If txtPdCode.Text = "" Then
                    bdn.BindingSource.Filter = String.Format("LotNo like '%{0}%' ", txtLotNo.Text)
                Else
                    bdn.BindingSource.Filter = String.Format("productcode like '%{0}%' and LotNo like '%{1}%' ",
                                                             txtPdCode.Text, txtLotNo.Text)
                End If
            End If
        End If
    End Sub

    Private Sub mnuBC_Click(sender As System.Object, e As System.EventArgs) Handles mnuBC.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.BaoCao = True
            _db.Update(obj)
            r.Cells("BaoCao").Value = 1
        Next
        ShowSuccess()
    End Sub

    Private Sub mnuNOBC_Click(sender As System.Object, e As System.EventArgs) Handles mnuNOBC.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.BaoCao = False
            _db.Update(obj)
            r.Cells("BaoCao").Value = 0
        Next
        ShowSuccess()
    End Sub 
     
    Private Sub tssExport_Click(sender As System.Object, e As System.EventArgs) Handles tssExport.Click
        ExportEXCEL(grid)
    End Sub

    Private Sub tssCopy_Click(sender As System.Object, e As System.EventArgs) Handles tssCopy.Click
        If grid.CurrentRow IsNot Nothing Then
            Dim objLock As New MDQA_Lock
            objLock.Ngay_K = grid.CurrentRow.Cells("Ngay").Value
            _db.GetObject(objLock)
            If objLock.Lock Then
                ShowWarning("Dữ liệu ngày này đã bị khóa không thể sửa.")
                Return
            End If

            If ShowQuestion(String.Format("Bạn muốn copy mã:{0} và lot:{1} ",
                                          grid.CurrentRow.Cells("ProductCode").Value,
                                          grid.CurrentRow.Cells("LotNo").Value)) = Windows.Forms.DialogResult.Yes Then
                Dim obj As New MDQA_KTPL
                obj.ID_K = grid.CurrentRow.Cells("ID").Value
                _db.GetObject(obj)
                obj.ID_K = Guid.NewGuid.ToString()
                obj.AQL = 0
                obj.LotQty = 0
                obj.ThoiGianRM = 0
                obj.GhiChu = ""
                _db.Insert(obj)

                mnuShowAll.PerformClick()
                txtPdCode.Text = ""
                txtLotNo.Text = ""
                txtPdCode.Text = obj.ProductCode
                txtLotNo.Text = obj.LotNo
            End If
        End If
    End Sub

    Private Sub tssEdit_Click(sender As System.Object, e As System.EventArgs) Handles tssEdit.Click
        Dim objLock As New MDQA_Lock
        objLock.Ngay_K = grid.CurrentRow.Cells("Ngay").Value
        _db.GetObject(objLock)
        If objLock.Lock Then
            ShowWarning("Dữ liệu ngày này đã bị khóa không thể sửa.")
            Return
        End If

        gridD.AllowUserToAddRows = Not gridD.AllowUserToAddRows

        gridD.Columns("empID").ReadOnly = True
        gridD.Columns("STT").ReadOnly = Not gridD.Columns("STT").ReadOnly
        gridD.Columns("SL").ReadOnly = Not gridD.Columns("SL").ReadOnly
        gridD.Columns("ThoiGian").ReadOnly = Not gridD.Columns("ThoiGian").ReadOnly
        gridD.Columns("DefectCode").ReadOnly = Not gridD.Columns("DefectCode").ReadOnly
        gridD.Columns("DefectQty").ReadOnly = Not gridD.Columns("DefectQty").ReadOnly

        grid.Columns("SoTrang").ReadOnly = Not grid.Columns("SoTrang").ReadOnly
        'grid.Columns("Nhom").ReadOnly = Not grid.Columns("Nhom").ReadOnly
        grid.Columns("Phong").ReadOnly = Not grid.Columns("Phong").ReadOnly
        grid.Columns("LotQty").ReadOnly = Not grid.Columns("LotQty").ReadOnly
        grid.Columns("AQL").ReadOnly = Not grid.Columns("AQL").ReadOnly
        grid.Columns("ThoiGianRM").ReadOnly = Not grid.Columns("ThoiGianRM").ReadOnly
        grid.Columns("GhiChu").ReadOnly = Not grid.Columns("GhiChu").ReadOnly
        grid.Columns("Lotno").ReadOnly = Not grid.Columns("Lotno").ReadOnly
        grid.Columns(CuDiemXuat.Name).ReadOnly = Not grid.Columns("CuDiemXuat").ReadOnly
        grid.Columns("Ngay").ReadOnly = Not grid.Columns("Ngay").ReadOnly
        grid.Columns("NgayEntek").ReadOnly = Not grid.Columns("NgayEntek").ReadOnly
    End Sub

    Private Sub txtghichu_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtghichu.KeyDown
        If e.KeyCode = Keys.Enter Then
            KeyPreview = False
            tssSave.PerformClick()
            KeyPreview = True
        End If
    End Sub
    Function GetEntek(ByVal PdCode As String, ByVal lotNo As String) As DateTime
        Dim enddate As DateTime = Nothing

        Dim sql As String = String.Format(" select ProcessCode, EndDate from [t_ProcessResult] " +
                                          " where ProductCode='{0}' and Lotnumber='{1}' " +
                                          " and ProcessCode in ('9315','9316','7054','7055','9327') ",
                                          PdCode, lotNo)
        Dim dtResult As DataTable = _dbFpics.FillDataTable(sql)
        If dtResult.Rows.Count > 0 Then
            If dtResult.Rows(0).Item("EndDate") IsNot DBNull.Value Then
                Return dtResult.Rows(0).Item("EndDate")
            Else
                Return Nothing
            End If 
        End If
        Return enddate
    End Function
    Private Sub tssSave_Click(sender As System.Object, e As System.EventArgs) Handles tssSave.Click
        Dim objLock As New MDQA_Lock
        objLock.Ngay_K = GetStartDate(dtpStart.Value)
        _db.GetObject(objLock)
        If objLock.Lock Then
            ShowWarning("Dữ liệu ngày này đã bị khóa không thể sửa.")
            Return
        End If

        If txtPdCode.Text = "" Then
            ShowWarning("Bạn chưa nhập mã sản phẩm.")
            txtPdCode.Focus()
            Return
        End If
        If txtLotNo.Text = "" Then
            ShowWarning("Bạn chưa LotNo.")
            txtLotNo.Focus()
            Return
        End If
        If txtTrang.Text = "" Then
            ShowWarning("Bạn chưa nhập số trang")
            txtTrang.Focus()
            Return
        End If 
        If cboPhong.Text = "" Then
            ShowWarning("Bạn chưa nhập phòng")
            cboPhong.Focus()
            Return
        End If
        If txtLotQty.Text = "" Then
            ShowWarning("Bạn chưa nhập LotQty")
            txtLotQty.Focus()
            Return
        End If
        If txtAQL.Text = "" Then
            ShowWarning("Bạn chưa nhập AQL")
            txtAQL.Focus()
            Return
        End If
        'If txtThoiGianRM.Text = "" Then
        '    ShowWarning("Bạn chưa nhập hoiGianRM")
        '    txtThoiGianRM.Focus()
        '    Return
        'End If
        If cboLoai.Text = "" Then
            ShowWarning("Bạn chưa nhập loại hàng.")
            cboLoai.Focus()
            Return
        End If
        If ShowQuestionSave() = Windows.Forms.DialogResult.Yes Then
            Dim obj As New MDQA_KTPL
            obj.ID_K = Guid.NewGuid.ToString
            obj.ProductCode = txtPdCode.Text.PadLeft(5, "0")

            If Not txtLotNo.Text.Contains("-") Then
                obj.LotNo = GetLotNo(obj.ProductCode, txtLotNo.Text.PadLeft(5, "0"))
            Else
                obj.LotNo = GetLotNo(obj.ProductCode, txtLotNo.Text.Split("-")(0).PadLeft(5, "0"))
                obj.LotNo = obj.LotNo & "-" & txtLotNo.Text.Split("-")(1)
            End If

            obj.Ngay = GetStartDate(dtpStart.Value) 'GetDate9051(txtPdCode.Text, txtLotNo.Text)
            obj.CreateDate = DateTime.Now

            obj.ProductName = _dbFpics.ExecuteScalar(String.Format(" SELECT top 1 [ProductNameE] " +
                                                " FROM [m_Product] where ProductCode='{0}' " +
                                                " order by RevisionCode ",
                                                obj.ProductCode))

            obj.Customer = _dbFpics.ExecuteScalar(String.Format(" SELECT top 1 [CustomerNameE] " +
                                                " FROM [m_Product] p inner join m_Customer c on c.CustomerCode=p.CustomerCode " +
                                                " where ProductCode='{0}' " +
                                                " order by RevisionCode ",
                                                obj.ProductCode))

            obj.SoTrang = txtTrang.Text
            If cboPhong.Text = "1" Or cboPhong.Text = "2" Or cboPhong.Text = "3" Then
                obj.Phong = "INS" & cboPhong.Text
            Else
                obj.Phong = "318"
            End If

            obj.LotQty = txtLotQty.Text
            obj.AQL = txtAQL.Text
            If IsNumeric(txtFPC.Text) Then
                obj.FPC = txtFPC.Text
            End If
            obj.ThoiGianRM = 0
            obj.LoaiHang = cboLoai.Text
            obj.CuDiemXuat = cboShipto.Text
            obj.GhiChu = txtghichu.Text
            obj.NgayEntek = GetEntek(obj.ProductCode, Microsoft.VisualBasic.Left(obj.LotNo, 5))
            _db.Insert(obj)
            mnuShowAll.PerformClick()
            txtPdCode.Focus()

            'txtPdCode.Text = ""
            'txtLotNo.Text = ""
            txtTrang.Text = ""
            cboPhong.Text = ""
            txtLotQty.Text = ""
            txtAQL.Text = ""
            txtThoiGianRM.Text = ""
            txtghichu.Text = ""
            txtFPC.Text = ""


        End If
    End Sub

    Private Sub txtPdCode_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtPdCode.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtPdCode.Text <> "" Then
                txtPdCode.Text = txtPdCode.Text.PadLeft(5, "0")
            End If
        End If
    End Sub

    Private Sub txtLotNo_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtLotNo.Text <> "" Then
                txtLotNo.Text = txtLotNo.Text.PadLeft(5, "0")
            End If
        End If
    End Sub

    Private Sub mnuHangThuong_Click(sender As System.Object, e As System.EventArgs) Handles mnuHangThuong.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.LoaiHang = "HANG-THUONG"
            _db.Update(obj)
            r.Cells("LoaiHang").Value = "HANG-THUONG"
        Next
        ShowSuccess()
    End Sub

    Private Sub mnuTachLo_Click(sender As System.Object, e As System.EventArgs) Handles mnuTachLo.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.LoaiHang = "TACH-LO"
            _db.Update(obj)
            r.Cells("LoaiHang").Value = "TACH-LO"
        Next
        ShowSuccess()
    End Sub

    Private Sub mnuKiemLai_Click(sender As System.Object, e As System.EventArgs) Handles mnuKiemLai.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.LoaiHang = "KIEM-LAI"
            _db.Update(obj)
            r.Cells("LoaiHang").Value = "KIEM-LAI"
        Next
        ShowSuccess()
    End Sub

    Private Sub mnuXOUT_Click(sender As System.Object, e As System.EventArgs) Handles mnuXOUT.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.LoaiHang = "X-OUT"
            _db.Update(obj)
            r.Cells("LoaiHang").Value = "X-OUT"
        Next
        ShowSuccess()
    End Sub

    Private Sub gridD_CellEnter(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridD.CellEnter
        If e.ColumnIndex = gridD.Columns("EmpIDPr3").Index And gridD.Focused Then
            SendKeys.Send("{Tab}")
        ElseIf e.ColumnIndex = gridD.Columns("Shift").Index And gridD.Focused Then
            SendKeys.Send("{Tab}")
        ElseIf e.ColumnIndex = gridD.Columns("EmpID").Index And gridD.Focused Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub mnuThuNghiem_Click(sender As System.Object, e As System.EventArgs) Handles mnuThuNghiem.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.LoaiHang = "THU-NGHIEM"
            _db.Update(obj)
            r.Cells("LoaiHang").Value = "THU-NGHIEM"
        Next
        ShowSuccess()
    End Sub
     

    Private Sub mnuUpdateEntek_Click_1(sender As System.Object, e As System.EventArgs) Handles mnuUpdateEntek.Click
        For Each r As DataGridViewRow In grid.SelectedRows
            Dim obj As New MDQA_KTPL
            obj.ID_K = r.Cells("ID").Value
            _db.GetObject(obj)
            obj.NgayEntek = GetEntek(obj.ProductCode, Microsoft.VisualBasic.Left(obj.LotNo, 5))
            _db.Update(obj)
            r.Cells("NgayEntek").Value = obj.NgayEntek
        Next
        ShowSuccess()
        mnuShowAll.PerformClick()
    End Sub

    Private Sub txtPdCode_Leave(sender As Object, e As EventArgs) Handles txtPdCode.Leave
        If txtPdCode.Text <> "" Then
            mnuShowAll.PerformClick()
        End If
    End Sub
End Class