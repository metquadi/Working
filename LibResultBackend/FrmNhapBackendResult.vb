﻿Imports CommonDB
Imports DevExpress.XtraEditors.Repository
Imports PublicUtility
Public Class FrmNhapBackendResult
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)

    Private Sub FrmResultBackend_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteNgay.EditValue = DateTime.Now
        grcProgressBar.Visible = False
    End Sub

    Private Sub FrmResultBackend_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control And e.KeyCode = Keys.S Then
            btnSave.PerformClick()
        ElseIf e.Control And e.KeyCode = Keys.Q Then
            CopyError()
        ElseIf e.Control And e.KeyCode = Keys.N Then
            btnNew.PerformClick()
        End If
    End Sub

    Private Sub btnNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnNew.ItemClick
        If cbbCongDoan.Text.Trim <> "" And txtProductCode.Text.Trim <> "" And
            txtLotNumber.Text.Trim <> "" And txtSoCongDoan.Text <> "" Then
            GridView1.Columns.Clear()
            Dim para(6) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Action", cbbCongDoan.Text)
            para(1) = New SqlClient.SqlParameter("@StartDate", dteNgay.DateTime.Date)
            para(2) = New SqlClient.SqlParameter("@EndDate", dteNgay.DateTime.Date)
            para(3) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
            para(4) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
            para(5) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
            para(6) = New SqlClient.SqlParameter("@SoCongDoan", txtSoCongDoan.Text)
            GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_QC_KQCV_BackendResult", para)
            GridControlSetFormat(GridView1, 1)
            GridView1.OptionsView.ShowFooter = False
            GridControlReadOnly(GridView1, False)
            GridView1.Columns("DanhGia").OptionsColumn.ReadOnly = True
            GridView1.Columns("ID").OptionsColumn.ReadOnly = True
            GridControlSetColorEdit(GridView1)
            GridView1.Columns("LanKiem").Width = 50
            GridView1.Columns("TanSoKiemTra").Width = 50
            GridView1.Columns("ThoiGianKiem").Width = 50
            GridView1.Columns("DanhGia").Width = 50
            GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top

            'Thêm combobox cho các columns
            'GridView1.Columns("LanKiem").ColumnEdit = cbbLanKiem
            If cbbCongDoan.Text <> "Ép đơn tầng (hàng khác)" Then
                GridView1.Columns("TanSoKiemTra").ColumnEdit = cbbTanSoKiemTra
            Else
                GridView1.Columns("TanSoKiemTra").Visible = False
                'GridView1.Columns("TanSoKiemTra").OptionsColumn.ReadOnly = True
            End If
            Dim dtMaLoi As DataTable = _db.FillDataTable("select * from QC_KQCV_MasterError")
            GridView1.Columns("MaLoiGH").ColumnEdit = GridControlLoadLookUpEdit(dtMaLoi, "MaLoi", "MaLoi")
            GridView1.Columns("MaLoiNG").ColumnEdit = GridControlLoadLookUpEdit(dtMaLoi, "MaLoi", "MaLoi")

            Dim dtMethod As DataTable = _db.FillDataTable("SELECT KiTu FROM QC_KQCV_MasterMethod")
            'GridView1.Columns("PhuongPhapXuLy").ColumnEdit = GridControlLoadLookUpEdit(dtMethod, "KiTu", "KiTu")
            Dim riEditor As New RepositoryItemComboBox()
            For Each r As DataRow In dtMethod.Rows
                riEditor.Items.Add(r("KiTu"))
            Next
            riEditor.DropDownRows = 20
            GridControl1.RepositoryItems.Add(riEditor)
            GridView1.Columns("PhuongPhapXuLy").ColumnEdit = riEditor

            If GridView1.Columns("DoDayNguyenLieu") IsNot Nothing Then
                GridControlSetFormatNumber(GridView1, "DoDayNguyenLieu", 3)
            End If

            GridView1.Columns("ID").Visible = False
            GridView1.Columns("CongDoan").Visible = False
            GridView1.Columns("ProductCode").Visible = False
            GridView1.Columns("LotNumber").Visible = False
            GridView1.Columns("SoCongDoan").Visible = False

            'ID 36 số
            'Guid.NewGuid.ToString() trong VB.NET
            'select NEWID() trong SQL Server

            Dim obj As New QC_KQCV_BackendResult
            obj.Ngay_K = dteNgay.DateTime.Date
            obj.CongDoan_K = cbbCongDoan.Text
            obj.ProductCode_K = txtProductCode.Text
            obj.LotNumber_K = txtLotNumber.Text
            obj.SoCongDoan_K = txtSoCongDoan.Text
            If Not _db.ExistObject(obj) Then
                obj.DanhGia = "OK"
                obj.CreateUser = CurrentUser.UserID
                obj.CreateDate = DateTime.Now
                _db.Insert(obj)
                para(0) = New SqlClient.SqlParameter("@Action", "InsertSoLuongLo")
                _db.ExecuteStoreProcedure("sp_QC_KQCV_BackendResult", para)
            End If
        End If
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If Not IsDBNull(GridView1.GetFocusedRowCellValue("ID")) Then
            If ShowQuestion("Bạn có chắc chắn muốn xóa dữ liệu ?") = DialogResult.Yes Then
                _db.ExecuteNonQuery(String.Format("DELETE QC_KQCV_BackendResult_Detail
                                                    WHERE ID = '{0}'", GridView1.GetFocusedRowCellValue("ID")))
                DanhGiaLoi(GridView1.GetFocusedRowCellValue("LanKiem"), GridView1.GetFocusedRowCellValue("TanSoKiemTra"))
                btnNew.PerformClick()
            End If
        End If
    End Sub
    Private Sub txtProductCode_Leave(sender As Object, e As EventArgs) Handles txtProductCode.Leave
        txtProductCode.Text = txtProductCode.Text.ToString.PadLeft(5, "0")
        txtLotNumber.Text = ""
    End Sub

    Private Sub txtLotNumber_Leave(sender As Object, e As EventArgs) Handles txtLotNumber.Leave
        txtSoCongDoan.Text = ""
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable = True And e.Column.ReadOnly = False Then
            If cbbCongDoan.Text <> "Ép đơn tầng (hàng khác)" Then
                If IsDBNull(GridView1.GetFocusedRowCellValue("LanKiem")) Then
                    ShowWarning("Bạn phải nhập lần kiểm và tần số trước !")
                    btnNew.PerformClick()
                    Return
                End If
            End If
            If e.RowHandle < 0 Then
                If IsDBNull(GridView1.GetFocusedRowCellValue("ID")) Then
                    Dim myid As String = Guid.NewGuid.ToString()
                    GridView1.SetFocusedRowCellValue("ID", myid)
                    GridView1.SetFocusedRowCellValue("Ngay", dteNgay.DateTime.Date)
                    GridView1.SetFocusedRowCellValue("SoCongDoan", txtSoCongDoan.Text)
                    Dim obj As New QC_KQCV_BackendResult_Detail
                    obj.ID_K = myid
                    obj.Ngay = dteNgay.DateTime.Date
                    obj.CongDoan = cbbCongDoan.Text
                    obj.ProductCode = txtProductCode.Text
                    obj.LotNumber = txtLotNumber.Text
                    obj.SoCongDoan = txtSoCongDoan.Text
                    obj.DanhGia = "OK"
                    If cbbCongDoan.Text = "Ép đơn tầng (hàng khác)" Then
                        obj.TanSoKiemTra = "NA"
                        GridView1.SetFocusedRowCellValue("TanSoKiemTra", "NA")
                    End If
                    obj.CreateUser = CurrentUser.UserID
                    obj.CreateDate = DateTime.Now
                    _db.Insert(obj)
                End If
            End If

            If e.Column.FieldName = "Ngay" Then
                Dim obj As New QC_KQCV_BackendResult
                obj.Ngay_K = e.Value
                obj.CongDoan_K = cbbCongDoan.Text
                obj.ProductCode_K = txtProductCode.Text
                obj.LotNumber_K = txtLotNumber.Text
                obj.SoCongDoan_K = txtSoCongDoan.Text
                If Not _db.ExistObject(obj) Then
                    'Insert Head
                    Dim paraH(6) As SqlClient.SqlParameter
                    paraH(0) = New SqlClient.SqlParameter("@Action", "InsertSoLuongLo")
                    paraH(1) = New SqlClient.SqlParameter("@StartDate", e.Value)
                    paraH(2) = New SqlClient.SqlParameter("@EndDate", e.Value)
                    paraH(3) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
                    paraH(4) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
                    paraH(5) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
                    paraH(6) = New SqlClient.SqlParameter("@SoCongDoan", txtSoCongDoan.Text)
                    obj.DanhGia = "OK"
                    obj.CreateUser = CurrentUser.UserID
                    obj.CreateDate = DateTime.Now
                    _db.Insert(obj)
                    _db.ExecuteStoreProcedure("sp_QC_KQCV_BackendResult", paraH)

                    'Insert Detail
                    Dim paraCopy(2) As SqlClient.SqlParameter
                    paraCopy(0) = New SqlClient.SqlParameter("@Action", "CopyByDay")
                    paraCopy(1) = New SqlClient.SqlParameter("@ID", GridView1.GetFocusedRowCellValue("ID"))
                    paraCopy(2) = New SqlClient.SqlParameter("@Ngay", e.Value)
                    _db.ExecuteStoreProcedure("sp_QC_KQCV_BackendResult", paraCopy)
                    _db.ExecuteNonQuery(String.Format("DELETE QC_KQCV_BackendResult_Detail
                                                       WHERE ID = '{0}'", GridView1.GetFocusedRowCellValue("ID")))
                    GridView1.DeleteSelectedRows()
                    Return
                End If
            End If

            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format("UPDATE QC_KQCV_BackendResult_Detail 
                                               SET {0} = @Value
                                               WHERE ID = '{1}'", e.Column.FieldName,
                                               GridView1.GetFocusedRowCellValue("ID")), para)

            If e.Column.FieldName = "SoLuongLoiGH" Or e.Column.FieldName = "SoLuongLoiNG" Then
                DanhGiaLoi(GridView1.GetFocusedRowCellValue("LanKiem"), GridView1.GetFocusedRowCellValue("TanSoKiemTra"))
            ElseIf e.Column.FieldName = "ThoiGianKiem" Or e.Column.FieldName = "SoLuongKiem" Then
                UpdateValues(e.Column.FieldName, GridView1.GetFocusedRowCellValue("LanKiem"))
            ElseIf e.Column.FieldName = "SoCongDoan" Or e.Column.FieldName = "SoMay" Or
                   e.Column.FieldName = "NhanVienKiem" Or e.Column.FieldName = "PhuongPhapXuLy" Or
                   e.Column.FieldName = "XacNhanBoPhanLienQuan" Or e.Column.FieldName = "GhiChu" Then
                UpdateMergeString(e.Column.FieldName)
            ElseIf e.Column.FieldName = "LanKiem" Then
                UpdateDanhGiaLoi()
            End If
        End If
    End Sub
    Sub DanhGiaLoi(lanKiem As String, tanSoKiemTra As String)
        Dim obj As New QC_KQCV_BackendResult
        obj.Ngay_K = dteNgay.DateTime.Date
        obj.CongDoan_K = cbbCongDoan.Text
        obj.ProductCode_K = txtProductCode.Text
        obj.LotNumber_K = txtLotNumber.Text
        obj.SoCongDoan_K = txtSoCongDoan.Text
        _db.GetObject(obj)
        Dim para(7) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteNgay.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteNgay.DateTime.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
        para(3) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
        para(4) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
        para(5) = New SqlClient.SqlParameter("@LanKiem", lanKiem)
        para(6) = New SqlClient.SqlParameter("@TanSo", tanSoKiemTra)
        para(7) = New SqlClient.SqlParameter("@SoCongDoan", txtSoCongDoan.Text)
        Dim soLoiGH As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiGH) 
                                                   FROM QC_KQCV_BackendResult_Detail
                                                   WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                   AND ProductCode = @ProductCode 
                                                   AND LotNumber = @LotNumber
                                                   AND CongDoan = @CongDoan
                                                   AND LanKiem = @LanKiem
                                                   AND TanSoKiemTra = @TanSo
                                                   AND SoCongDoan = @SoCongDoan", para)
        Dim soLoiNG As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiNG) 
                                                   FROM QC_KQCV_BackendResult_Detail
                                                   WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                   AND ProductCode = @ProductCode 
                                                   AND LotNumber = @LotNumber
                                                   AND CongDoan = @CongDoan
                                                   AND LanKiem = @LanKiem
                                                   AND TanSoKiemTra = @TanSo
                                                   AND SoCongDoan = @SoCongDoan", para)
        soLoiGH = IIf(IsDBNull(soLoiGH), 0, soLoiGH)
        soLoiNG = IIf(IsDBNull(soLoiNG), 0, soLoiNG)
        If soLoiGH + soLoiNG > 0 Then
            _db.ExecuteNonQuery("UPDATE QC_KQCV_BackendResult_Detail
                                 SET DanhGia = 'NG'
                                 WHERE Ngay BETWEEN @StartDate AND @EndDate
                                 AND ProductCode = @ProductCode 
                                 AND LotNumber = @LotNumber
                                 AND CongDoan = @CongDoan
                                 AND LanKiem = @LanKiem
                                 AND TanSoKiemTra = @TanSo
                                 AND SoCongDoan = @SoCongDoan", para)
            GridView1.SetFocusedRowCellValue("DanhGia", "NG")
        Else
            _db.ExecuteNonQuery("UPDATE QC_KQCV_BackendResult_Detail
                                 SET DanhGia = 'OK'
                                 WHERE Ngay BETWEEN @StartDate AND @EndDate
                                 AND ProductCode = @ProductCode 
                                 AND LotNumber = @LotNumber
                                 AND CongDoan = @CongDoan
                                 AND LanKiem = @LanKiem
                                 AND TanSoKiemTra = @TanSo
                                 AND SoCongDoan = @SoCongDoan", para)
            GridView1.SetFocusedRowCellValue("DanhGia", "OK")
        End If

        Dim soLoiGHAll As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiGH) 
                                                      FROM QC_KQCV_BackendResult_Detail
                                                      WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                      AND ProductCode = @ProductCode 
                                                      AND LotNumber = @LotNumber
                                                      AND CongDoan = @CongDoan
                                                      AND SoCongDoan = @SoCongDoan", para)
        Dim soLoiNGAll As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiNG) 
                                                      FROM QC_KQCV_BackendResult_Detail
                                                      WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                      AND ProductCode = @ProductCode 
                                                      AND LotNumber = @LotNumber
                                                      AND CongDoan = @CongDoan
                                                      AND SoCongDoan = @SoCongDoan", para)
        soLoiGHAll = IIf(IsDBNull(soLoiGHAll), 0, soLoiGHAll)
        soLoiNGAll = IIf(IsDBNull(soLoiNGAll), 0, soLoiNGAll)
        If soLoiGHAll + soLoiNGAll > 0 Then
            obj.DanhGia = "NG"
        Else
            obj.DanhGia = "OK"
        End If
        obj.SoLuongLoi = soLoiGHAll + soLoiNGAll
        If obj.SoLuongKiem > 0 Then
            obj.TyLe = (obj.SoLuongLoi / obj.SoLuongKiem)
        End If
        _db.Update(obj)
    End Sub
    Sub CopyError()
        Dim para(1) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Action", "Copy")
        para(1) = New SqlClient.SqlParameter("@ID", GridView1.GetFocusedRowCellValue("ID"))
        _db.ExecuteStoreProcedure("sp_QC_KQCV_BackendResult", para)
        btnNew.PerformClick()
    End Sub

    Sub UpdateValues(columnName As String, lanKiem As String)
        Dim para(6) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteNgay.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteNgay.DateTime.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
        para(3) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
        para(4) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
        para(5) = New SqlClient.SqlParameter("@SoCongDoan", txtSoCongDoan.Text)
        para(6) = New SqlClient.SqlParameter("@LanKiem", lanKiem)
        'para(5) = New SqlClient.SqlParameter("@TanSo", GridView1.GetFocusedRowCellValue("TanSoKiemTra"))

        Dim values As Object = _db.ExecuteScalar(String.Format("SELECT MAX({0}) AS ThoiGianKiem INTO #Tam
                                                                FROM QC_KQCV_BackendResult_Detail
                                                                WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                                AND ProductCode = @ProductCode
                                                                AND LotNumber = @LotNumber
                                                                AND CongDoan = @CongDoan
                                                                AND LanKiem = @LanKiem
                                                                AND SoCongDoan = @SoCongDoan
                                                                GROUP BY TanSoKiemTra
                                                                SELECT SUM(ThoiGianKiem) AS ThoiGianKiem
                                                                FROM #Tam
                                                                DROP TABLE #Tam", columnName), para)
        Dim obj As New QC_KQCV_BackendResult
        obj.Ngay_K = dteNgay.DateTime.Date
        obj.CongDoan_K = cbbCongDoan.Text
        obj.ProductCode_K = txtProductCode.Text
        obj.LotNumber_K = txtLotNumber.Text
        obj.SoCongDoan_K = txtSoCongDoan.Text
        _db.GetObject(obj)
        If columnName = "ThoiGianKiem" Then
            obj.ThoiGianKiem = values
        ElseIf columnName = "SoLuongKiem" Then
            obj.SoLuongKiem = values
        End If
        _db.Update(obj)
    End Sub
    Sub UpdateMergeString(columnName As String)
        Dim para(5) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteNgay.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteNgay.DateTime.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
        para(3) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
        para(4) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
        para(5) = New SqlClient.SqlParameter("@SoCongDoan", txtSoCongDoan.Text)

        Dim values As Object = _db.ExecuteScalar(String.Format("SELECT STUFF(
                                                                ( SELECT DISTINCT ' '+  {0}
                                                                FROM QC_KQCV_BackendResult_Detail AS D 
                                                                WHERE D.Ngay = H.Ngay
                                                                AND D.CongDoan = H.CongDoan
                                                                AND D.ProductCode = H.ProductCode
                                                                AND D.LotNumber = H.LotNumber
                                                                AND D.SoCongDoan = H.SoCongDoan
                                                                FOR XML PATH ('') )
                                                                , 1, 1, '')  AS SoCongDoan
                                                                FROM QC_KQCV_BackendResult AS H
                                                                WHERE H.Ngay BETWEEN @StartDate AND @EndDate
                                                                AND H.CongDoan = @CongDoan
                                                                AND H.ProductCode = @ProductCode
                                                                AND H.LotNumber = @LotNumber
                                                                AND H.SoCongDoan = @SoCongDoan", columnName), para)
        Dim obj As New QC_KQCV_BackendResult
        obj.Ngay_K = dteNgay.DateTime.Date
        obj.CongDoan_K = cbbCongDoan.Text
        obj.ProductCode_K = txtProductCode.Text
        obj.LotNumber_K = txtLotNumber.Text
        obj.SoCongDoan_K = txtSoCongDoan.Text
        _db.GetObject(obj)
        'If columnName = "SoCongDoan" Then
        '    obj.SoCongDoan = values
        'End If
        If columnName = "SoMay" Then
            obj.SoMay = values
        ElseIf columnName = "NhanVienKiem" Then
            obj.NhanVienKiem = values
        ElseIf columnName = "PhuongPhapXuLy" Then
            obj.PhuongPhapXuLy = values
        ElseIf columnName = "XacNhanBoPhanLienQuan" Then
            obj.XacNhanBoPhanLienQuan = values
        ElseIf columnName = "GhiChu" Then
            obj.GhiChu = values
        End If
        _db.Update(obj)
    End Sub

    Private Sub btnEditMaster_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEditMaster.ItemClick
        Dim frm As New FrmEditMaster
        frm.Show()
    End Sub
    Sub UpdateDanhGiaLoi()
        Dim para(7) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteNgay.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteNgay.DateTime.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
        para(3) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
        para(4) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
        para(5) = New SqlClient.SqlParameter("@LanKiem", "")
        para(6) = New SqlClient.SqlParameter("@TanSo", "")
        para(7) = New SqlClient.SqlParameter("@SoCongDoan", txtSoCongDoan.Text)
        Dim dtLanKiem As DataTable = _db.FillDataTable("SELECT LanKiem, TanSoKiemTra
                                                        FROM QC_KQCV_BackendResult_Detail
                                                        WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                        AND ProductCode = @ProductCode 
                                                        AND LotNumber = @LotNumber
                                                        AND CongDoan = @CongDoan
                                                        AND SoCongDoan = @SoCongDoan
                                                        GROUP BY LanKiem, TanSoKiemTra", para)
        For Each r As DataRow In dtLanKiem.Rows
            para(5) = New SqlClient.SqlParameter("@LanKiem", r("LanKiem"))
            para(6) = New SqlClient.SqlParameter("@TanSo", r("TanSoKiemTra"))
            Dim soLoiGH As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiGH) 
                                                       FROM QC_KQCV_BackendResult_Detail
                                                       WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                       AND ProductCode = @ProductCode 
                                                       AND LotNumber = @LotNumber
                                                       AND CongDoan = @CongDoan
                                                       AND LanKiem = @LanKiem
                                                       AND TanSoKiemTra = @TanSo
                                                       AND SoCongDoan = @SoCongDoan", para)
            Dim soLoiNG As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiNG) 
                                                       FROM QC_KQCV_BackendResult_Detail
                                                       WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                       AND ProductCode = @ProductCode 
                                                       AND LotNumber = @LotNumber
                                                       AND CongDoan = @CongDoan
                                                       AND LanKiem = @LanKiem
                                                       AND TanSoKiemTra = @TanSo
                                                       AND SoCongDoan = @SoCongDoan", para)
            soLoiGH = IIf(IsDBNull(soLoiGH), 0, soLoiGH)
            soLoiNG = IIf(IsDBNull(soLoiNG), 0, soLoiNG)
            If soLoiGH + soLoiNG > 0 Then
                _db.ExecuteNonQuery("UPDATE QC_KQCV_BackendResult_Detail
                                     SET DanhGia = 'NG'
                                     WHERE Ngay BETWEEN @StartDate AND @EndDate
                                     AND ProductCode = @ProductCode 
                                     AND LotNumber = @LotNumber
                                     AND CongDoan = @CongDoan
                                     AND LanKiem = @LanKiem
                                     AND TanSoKiemTra = @TanSo
                                     AND SoCongDoan = @SoCongDoan", para)
                GridView1.SetFocusedRowCellValue("DanhGia", "NG")
            Else
                _db.ExecuteNonQuery("UPDATE QC_KQCV_BackendResult_Detail
                                     SET DanhGia = 'OK'
                                     WHERE Ngay BETWEEN @StartDate AND @EndDate
                                     AND ProductCode = @ProductCode 
                                     AND LotNumber = @LotNumber
                                     AND CongDoan = @CongDoan
                                     AND LanKiem = @LanKiem
                                     AND TanSoKiemTra = @TanSo
                                     AND SoCongDoan = @SoCongDoan", para)
                GridView1.SetFocusedRowCellValue("DanhGia", "OK")
            End If
        Next
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        If GridControl1.DataSource IsNot Nothing Then
            GridView1.Columns("CongDoan").Visible = True
            GridView1.Columns("ProductCode").Visible = True
            GridView1.Columns("LotNumber").Visible = True
            GridView1.Columns("SoCongDoan").Visible = True
            GridView1.Columns("CongDoan").VisibleIndex = 1
            GridView1.Columns("ProductCode").VisibleIndex = 2
            GridView1.Columns("LotNumber").VisibleIndex = 3
            GridView1.Columns("SoCongDoan").VisibleIndex = 4
            GridControlExportExcel(GridView1)
            GridView1.Columns("CongDoan").Visible = False
            GridView1.Columns("ProductCode").Visible = False
            GridView1.Columns("LotNumber").Visible = False
            GridView1.Columns("SoCongDoan").Visible = False
        End If
    End Sub

    Private Sub btnImport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnImport.ItemClick
        Dim dt As DataTable = ImportEXCEL(True)
        If dt.Rows.Count > 0 Then
            grcProgressBar.Visible = True
            Dim row As Integer = 0
            Try
                _db.BeginTransaction()
                Dim succ As Integer = 0
                ProgressBarControl1.Properties.Step = 1
                ProgressBarControl1.Properties.PercentView = True
                ProgressBarControl1.Properties.Maximum = dt.Rows.Count
                ProgressBarControl1.Properties.Minimum = 0
                ProgressBarControl1.Properties.ShowTitle = True
                For Each r As DataRow In dt.Rows
                    row += 1
                    If row = 15 Then
                        Dim a As String = "asd"
                    End If
                    If IsDBNull(r("Ngay")) Or IsDBNull(r("Cong Doan")) Then Continue For
                    Dim obj As New QC_KQCV_BackendResult
                    obj.Ngay_K = r("Ngay")
                    obj.CongDoan_K = r("Cong Doan")
                    obj.ProductCode_K = r("Product Code").ToString.PadLeft(5, "0")
                    obj.LotNumber_K = r("Lot Number")
                    obj.SoCongDoan_K = r("So Cong Doan")
                    dteNgay.EditValue = obj.Ngay_K
                    txtProductCode.Text = obj.ProductCode_K
                    txtLotNumber.Text = obj.LotNumber_K
                    cbbCongDoan.Text = obj.CongDoan_K
                    txtSoCongDoan.Text = obj.SoCongDoan_K
                    If _db.ExistObject(obj) = False Then
                        obj.CreateDate = DateTime.Now
                        obj.CreateUser = CurrentUser.UserID
                        _db.Insert(obj)
                        succ += 1
                    End If
                    InsertDetail(r)
                    InsertSoLuongLo(obj.ProductCode_K, obj.LotNumber_K)

                    Dim tanSoKiemTra As String = "NA"
                    If r.Table.Columns.Contains("Tan So Kiem Tra") Then
                        tanSoKiemTra = r("Tan So Kiem Tra")
                    End If
                    UpdateValueColumn(r, r("Lan Kiem"), tanSoKiemTra)
                    ProgressBarControl1.PerformStep()
                    ProgressBarControl1.Update()
                Next
                ProgressBarControl1.EditValue = 0
                _db.Commit()
                ShowSuccess(succ)
            Catch ex As Exception
                _db.RollBack()
                ShowWarning(ex.Message + " - Dòng " + row.ToString)
            End Try
        Else
            ShowWarning("Không có dữ liệu !")
        End If
        grcProgressBar.Visible = False
    End Sub
    Sub InsertSoLuongLo(prodC As String, lotN As String)
        Dim para(2) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@ProductCode", prodC)
        para(1) = New SqlClient.SqlParameter("@LotNumber", lotN)
        para(2) = New SqlClient.SqlParameter("@Action", "InsertSoLuongLo")
        _db.ExecuteStoreProcedure("sp_QC_KQCV_BackendResult", para)
    End Sub
    Sub UpdateValueColumn(r As DataRow, lanKiem As String, tanSoKiemTra As String)
        'If r.Table.Columns.Contains("So Cong Doan") Then
        '    If Not IsDBNull(r("So Cong Doan")) Then UpdateMergeString("SoCongDoan")
        'End If
        If r.Table.Columns.Contains("So May") Then
            If Not IsDBNull(r("So May")) Then UpdateMergeString("SoMay")
        End If
        If Not IsDBNull(r("Nhan Vien Kiem")) Then UpdateMergeString("NhanVienKiem")
        If Not IsDBNull(r("Phuong Phap Xu Ly")) Then UpdateMergeString("PhuongPhapXuLy")
        If r.Table.Columns.Contains("Xac Nhan Bo Phan Lien Quan") Then
            If Not IsDBNull(r("Xac Nhan Bo Phan Lien Quan")) Then UpdateMergeString("XacNhanBoPhanLienQuan")
        End If
        If Not IsDBNull(r("Ghi Chu")) Then UpdateMergeString("GhiChu")

        If Not IsDBNull(r("Thoi Gian Kiem")) Then UpdateValues("ThoiGianKiem", lanKiem)
        If r.Table.Columns.Contains("So Luong Kiem") Then
            If Not IsDBNull(r("So Luong Kiem")) Then UpdateValues("SoLuongKiem", lanKiem)
        End If

        DanhGiaLoiImport()
        'DanhGiaLoi(lanKiem, tanSoKiemTra)
        'UpdateDanhGiaLoi()
    End Sub
    Sub InsertDetail(r As DataRow)
        Dim obj As New QC_KQCV_BackendResult_Detail
        obj.ID_K = Guid.NewGuid.ToString()
        obj.Ngay = r("Ngay")
        obj.CongDoan = r("Cong Doan")
        obj.ProductCode = r("Product Code").ToString.PadLeft(5, "0")
        obj.LotNumber = r("Lot Number")
        obj.LanKiem = r("Lan Kiem")
        obj.SoCongDoan = r("So Cong Doan")
        If obj.CongDoan = "Ép đơn tầng (hàng khác)" Then
            obj.TanSoKiemTra = "NA"
        Else
            obj.TanSoKiemTra = r("Tan So Kiem Tra")
        End If
        If r.Table.Columns.Contains("So May") Then
            If Not IsDBNull(r("So May")) Then obj.SoMay = r("So May")
        End If
        If Not IsDBNull(r("Thoi Gian Kiem")) Then obj.ThoiGianKiem = r("Thoi Gian Kiem")
        If r.Table.Columns.Contains("Ca Lam Viec") Then
            If Not IsDBNull(r("Ca Lam Viec")) Then obj.CaLamViec = r("Ca Lam Viec")
        End If
        If r.Table.Columns.Contains("So Luong Kiem") Then
            If Not IsDBNull(r("So Luong Kiem")) Then obj.SoLuongKiem = r("So Luong Kiem")
        End If
        If r.Table.Columns.Contains("So Luong Lo") Then
            If Not IsDBNull(r("So Luong Lo")) Then obj.SoLuongLo = r("So Luong Lo")
        End If
        If Not IsDBNull(r("Ma Loi GH")) Then obj.MaLoiGH = r("Ma Loi GH")
        If Not IsDBNull(r("So Luong Loi GH")) Then obj.SoLuongLoiGH = r("So Luong Loi GH")
        If Not IsDBNull(r("Vi Tri Loi GH")) Then obj.ViTriLoiGH = r("Vi Tri Loi GH")
        If Not IsDBNull(r("Dang Loi GH")) Then obj.DangLoiGH = r("Dang Loi GH")
        If Not IsDBNull(r("Ma Loi NG")) Then obj.MaLoiNG = r("Ma Loi NG")
        If Not IsDBNull(r("So Luong Loi NG")) Then obj.SoLuongLoiNG = r("So Luong Loi NG")
        If Not IsDBNull(r("Vi Tri Loi NG")) Then obj.ViTriLoiNG = r("Vi Tri Loi NG")
        If Not IsDBNull(r("Dang Loi NG")) Then obj.DangLoiNG = r("Dang Loi NG")
        If Not IsDBNull(r("Danh Gia")) Then obj.DanhGia = r("Danh Gia")
        If Not IsDBNull(r("Nhan Vien Kiem")) Then obj.NhanVienKiem = r("Nhan Vien Kiem")
        If r.Table.Columns.Contains("So Lo Dot FD") Then
            If Not IsDBNull(r("So Lo Dot FD")) Then obj.SoLoDotFD = r("So Lo Dot FD")
        End If
        If r.Table.Columns.Contains("Danh Gia So Lo Dot") Then
            If Not IsDBNull(r("Danh Gia So Lo Dot")) Then obj.DanhGiaSoLoDot = r("Danh Gia So Lo Dot")
        End If
        If r.Table.Columns.Contains("KQKT501") Then
            If Not IsDBNull(r("KQKT501")) Then obj.KQKT501 = r("KQKT501")
        End If
        If r.Table.Columns.Contains("KQKT505") Then
            If Not IsDBNull(r("KQKT505")) Then obj.KQKT505 = r("KQKT505")
        End If
        If r.Table.Columns.Contains("KQKT509") Then
            If Not IsDBNull(r("KQKT509")) Then obj.KQKT509 = r("KQKT509")
        End If
        If r.Table.Columns.Contains("KQKT601") Then
            If Not IsDBNull(r("KQKT601")) Then obj.KQKT601 = r("KQKT601")
        End If
        If r.Table.Columns.Contains("KQKT Khac") Then
            If Not IsDBNull(r("KQKT Khac")) Then obj.KQKTKhac = r("KQKT Khac")
        End If
        If r.Table.Columns.Contains("Loai Dem") Then
            If Not IsDBNull(r("Loai Dem")) Then obj.LoaiDem = r("Loai Dem")
        End If
        If r.Table.Columns.Contains("Han Su Dung") Then
            If Not IsDBNull(r("Han Su Dung")) Then obj.HanSuDung = r("Han Su Dung")
        End If
        If r.Table.Columns.Contains("Do Day Nguyen Lieu") Then
            If Not IsDBNull(r("Do Day Nguyen Lieu")) Then obj.DoDayNguyenLieu = r("Do Day Nguyen Lieu")
        End If
        If r.Table.Columns.Contains("Danh Gia Do Day Nguyen Lieu") Then
            If Not IsDBNull(r("Danh Gia Do Day Nguyen Lieu")) Then obj.DanhGiaDoDayNguyenLieu = r("Danh Gia Do Day Nguyen Lieu")
        End If
        If Not IsDBNull(r("Phuong Phap Xu Ly")) Then obj.PhuongPhapXuLy = r("Phuong Phap Xu Ly")
        If r.Table.Columns.Contains("Xac Nhan Bo Phan Lien Quan") Then
            If Not IsDBNull(r("Xac Nhan Bo Phan Lien Quan")) Then obj.XacNhanBoPhanLienQuan = r("Xac Nhan Bo Phan Lien Quan")
        End If
        If r.Table.Columns.Contains("Danh Gia Sau Xu Ly") Then
            If Not IsDBNull(r("Danh Gia Sau Xu Ly")) Then obj.DanhGiaSauXuLy = r("Danh Gia Sau Xu Ly")
        End If
        If r.Table.Columns.Contains("Xac Nhan PR") Then
            If Not IsDBNull(r("Xac Nhan PR")) Then obj.XacNhanPR = r("Xac Nhan PR")
        End If
        If r.Table.Columns.Contains("Xac Nhan TE") Then
            If Not IsDBNull(r("Xac Nhan TE")) Then obj.XacNhanTE = r("Xac Nhan TE")
        End If
        If Not IsDBNull(r("Ghi Chu")) Then obj.GhiChu = r("Ghi Chu")
        obj.CreateUser = CurrentUser.UserID
        obj.CreateDate = DateTime.Now
        _db.Insert(obj)
    End Sub
    Sub DanhGiaLoiImport()
        Dim obj As New QC_KQCV_BackendResult
        obj.Ngay_K = dteNgay.DateTime.Date
        obj.CongDoan_K = cbbCongDoan.Text
        obj.ProductCode_K = txtProductCode.Text
        obj.LotNumber_K = txtLotNumber.Text
        obj.SoCongDoan_K = txtSoCongDoan.Text
        _db.GetObject(obj)
        Dim para(5) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteNgay.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteNgay.DateTime.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
        para(3) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
        para(4) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
        para(5) = New SqlClient.SqlParameter("@SoCongDoan", txtSoCongDoan.Text)
        Dim soLoiGHAll As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiGH) 
                                                      FROM QC_KQCV_BackendResult_Detail
                                                      WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                      AND ProductCode = @ProductCode 
                                                      AND LotNumber = @LotNumber
                                                      AND CongDoan = @CongDoan
                                                      AND SoCongDoan = @SoCongDoan", para)
        Dim soLoiNGAll As Object = _db.ExecuteScalar("SELECT SUM(SoLuongLoiNG) 
                                                      FROM QC_KQCV_BackendResult_Detail
                                                      WHERE Ngay BETWEEN @StartDate AND @EndDate
                                                      AND ProductCode = @ProductCode 
                                                      AND LotNumber = @LotNumber
                                                      AND CongDoan = @CongDoan
                                                      AND SoCongDoan = @SoCongDoan", para)
        soLoiGHAll = IIf(IsDBNull(soLoiGHAll), 0, soLoiGHAll)
        soLoiNGAll = IIf(IsDBNull(soLoiNGAll), 0, soLoiNGAll)
        If soLoiGHAll + soLoiNGAll > 0 Then
            obj.DanhGia = "NG"
        Else
            obj.DanhGia = "OK"
        End If
        obj.SoLuongLoi = soLoiGHAll + soLoiNGAll
        If obj.SoLuongKiem > 0 Then
            obj.TyLe = (obj.SoLuongLoi / obj.SoLuongKiem)
        End If
        _db.Update(obj)
    End Sub

    'Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
    '    GridView1.Columns.Clear()
    '    Dim para(5) As SqlClient.SqlParameter
    '    para(0) = New SqlClient.SqlParameter("@StartDate", dteNgay.DateTime.Date)
    '    para(1) = New SqlClient.SqlParameter("@EndDate", dteNgay.DateTime.Date)
    '    para(2) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
    '    para(3) = New SqlClient.SqlParameter("@LotNumber", txtLotNumber.Text)
    '    para(4) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
    '    para(5) = New SqlClient.SqlParameter("@Action", cbbCongDoan.Text)
    '    Dim dt As DataTable = _db.ExecuteStoreProcedureTB("sp_QC_KQCV_BackendResult", para)
    '    If dt.Rows.Count > 0 Then
    '        GridControl1.DataSource = dt
    '        GridControlSetFormat(GridView1)
    '        If GridView1.Columns("DoDayNguyenLieu") IsNot Nothing Then
    '            GridControlSetFormatNumber(GridView1, "DoDayNguyenLieu", 3)
    '        End If
    '        GridView1.Columns("ID").Visible = False
    '    Else
    '        ShowWarning("Không có dữ liệu !")
    '        GridControlSetFormat(GridView1)
    '    End If
    'End Sub


    'Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
    '    If GridView1.RowCount > 0 Then
    '        GridControlReadOnly(GridView1, False)
    '        GridView1.Columns("DanhGia").OptionsColumn.ReadOnly = True
    '        GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
    '        Dim dtMaLoi As DataTable = _db.FillDataTable("select * from QC_KQCV_MasterError")
    '        GridView1.Columns("MaLoiGH").ColumnEdit = GridControlLoadLookUpEdit(dtMaLoi, "MaLoi", "MaLoi")
    '        GridView1.Columns("MaLoiNG").ColumnEdit = GridControlLoadLookUpEdit(dtMaLoi, "MaLoi", "MaLoi")

    '        Dim dtMethod As DataTable = _db.FillDataTable("select KiTu from QC_KQCV_MasterMethod")
    '        Dim riEditor As New RepositoryItemComboBox()
    '        For Each r As DataRow In dtMethod.Rows
    '            riEditor.Items.Add(r("KiTu"))
    '        Next
    '        riEditor.DropDownRows = 20
    '        GridControl1.RepositoryItems.Add(riEditor)
    '        GridView1.Columns("PhuongPhapXuLy").ColumnEdit = riEditor

    '        GridControlSetColorEdit(GridView1)
    '    End If
    'End Sub
End Class