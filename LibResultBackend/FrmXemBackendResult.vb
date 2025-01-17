﻿Imports CommonDB
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports PublicUtility
Public Class FrmXemBackendResult
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim _Ngay As DateTime
    Dim _CongDoan As String = ""
    Dim _ProductCode As String = ""
    Dim _LotNumber As String = ""
    Dim _SoCongDoan As String = ""
    Private Sub FrmShowResultBackend_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteStartDate.EditValue = GetStartDayOfMonth(DateTime.Now)
        dteEndDate.EditValue = GetEndDayOfMonth(DateTime.Now)
    End Sub

    Private Sub dteStartDate_EditValueChanged(sender As Object, e As EventArgs) Handles dteStartDate.EditValueChanged
        If dteStartDate.DateTime > dteEndDate.DateTime Then
            dteEndDate.EditValue = dteStartDate.DateTime
        End If
    End Sub

    Private Sub dteEndDate_EditValueChanged(sender As Object, e As EventArgs) Handles dteEndDate.EditValueChanged
        If dteEndDate.DateTime < dteStartDate.DateTime Then
            dteStartDate.EditValue = dteEndDate.DateTime
        End If
    End Sub

    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        If cbbCongDoan.Text = "Normal" Or cbbCongDoan.Text = "Dập C/L" Then
            grcRutMauPET.Visible = True
            Dim paraPET(1) As SqlClient.SqlParameter
            paraPET(0) = New SqlClient.SqlParameter("@StartDate", dteStartDate.DateTime.Date)
            paraPET(1) = New SqlClient.SqlParameter("@EndDate", dteEndDate.DateTime.Date)
            Dim dtPET As DataTable = _db.FillDataTable("SELECT SUM(ThoiGianKiem), COUNT(GhiChu) 
                                                        FROM QC_KQCV_BackendResult
                                                        WHERE CongDoan = N'Dập C/L'
                                                        AND GhiChu LIKE N'%P%'
                                                        AND GhiChu NOT LIKE N'%P[0-9]%'
                                                        AND Ngay BETWEEN @StartDate AND @EndDate", paraPET)
            txtTimePET.Text = IIf(IsDBNull(dtPET.Rows(0)(0)), 0, dtPET.Rows(0)(0))
            txtNumPET.Text = IIf(IsDBNull(dtPET.Rows(0)(1)), 0, dtPET.Rows(0)(1))
        Else
            grcRutMauPET.Visible = False
        End If
        GridView1.Columns.Clear()
        Dim para(4) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteStartDate.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteEndDate.DateTime.Date)
        If txtProductCode.Text.Trim = "" Then
            para(2) = New SqlClient.SqlParameter("@ProductCode", DBNull.Value)
        Else
            para(2) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
        End If
        If cbbCongDoan.Text = "Normal" Then
            para(3) = New SqlClient.SqlParameter("@CongDoan", DBNull.Value)
        Else
            para(3) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
        End If
        para(4) = New SqlClient.SqlParameter("@Action", "ShowAll")

        If chbDetail.Checked Then
            If cbbCongDoan.Text <> "" And cbbCongDoan.Text <> "Normal" Then
                para(4) = New SqlClient.SqlParameter("@Action", cbbCongDoan.Text)
                GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_QC_KQCV_BackendResult", para)
                GridControlSetFormat(GridView1, 2)
                GridView1.Columns("ID").Visible = False
            Else
                ShowWarning("Chưa chọn tên công đoạn !")
                cbbCongDoan.Select()
            End If
        Else
            GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_QC_KQCV_BackendResult", para)
            GridControlSetFormat(GridView1, 2)
            GridControlSetFormatPercentage(GridView1, "TyLe", 2)
            GridView1.Columns("PhuongPhapXuLy").Width = 500
            GridView1.OptionsView.ShowFooter = False
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        Dim listGrid As New List(Of GridView)
        Dim isContent As Boolean = False
        If GridView1.RowCount > 0 Then
            listGrid.Add(GridView1)
            isContent = True
        End If
        If GridView2.RowCount > 0 Then
            listGrid.Add(GridView2)
            isContent = True
        End If
        If GridView3.RowCount > 0 Then
            listGrid.Add(GridView3)
            isContent = True
        End If
        If isContent Then
            GridControlExportExcels(listGrid, True, , "Xem Backend ", False)
        End If
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If chbDetail.Checked Then
            Return
        End If
        If ShowQuestionDelete() = DialogResult.Yes Then
            Dim para(4) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Ngay", GridView1.GetFocusedRowCellValue("Ngay"))
            para(1) = New SqlClient.SqlParameter("@CongDoan", GridView1.GetFocusedRowCellValue("CongDoan"))
            para(2) = New SqlClient.SqlParameter("@ProductCode", GridView1.GetFocusedRowCellValue("ProductCode"))
            para(3) = New SqlClient.SqlParameter("@LotNumber", GridView1.GetFocusedRowCellValue("LotNumber"))
            para(4) = New SqlClient.SqlParameter("@SoCongDoan", GridView1.GetFocusedRowCellValue("SoCongDoan"))
            _db.ExecuteNonQuery("DELETE QC_KQCV_BackendResult 
                                 WHERE Ngay = @Ngay 
                                 AND CongDoan = @CongDoan
                                 AND ProductCode = @ProductCode
                                 AND LotNumber = @LotNumber
                                 AND SoCongDoan = @SoCongDoan", para)
            _db.ExecuteNonQuery("DELETE QC_KQCV_BackendResult_Detail
                                 WHERE Ngay = @Ngay 
                                 AND CongDoan = @CongDoan
                                 AND ProductCode = @ProductCode
                                 AND LotNumber = @LotNumber
                                 AND SoCongDoan = @SoCongDoan", para)
            GridView1.DeleteSelectedRows()
        End If
    End Sub

    Private Sub GridView1_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView1.RowClick
        If chbDetail.Checked Then
            Return
        End If
        _Ngay = GridView1.GetFocusedRowCellValue("Ngay")
        _CongDoan = GridView1.GetFocusedRowCellValue("CongDoan")
        _ProductCode = GridView1.GetFocusedRowCellValue("ProductCode")
        _LotNumber = GridView1.GetFocusedRowCellValue("LotNumber")
        _SoCongDoan = GridView1.GetFocusedRowCellValue("SoCongDoan")

        GridView2.Columns.Clear()
        GridView3.Columns.Clear()
        Dim para(6) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", _Ngay)
        para(1) = New SqlClient.SqlParameter("@EndDate", _Ngay)
        para(2) = New SqlClient.SqlParameter("@CongDoan", _CongDoan)
        para(3) = New SqlClient.SqlParameter("@ProductCode", _ProductCode)
        para(4) = New SqlClient.SqlParameter("@LotNumber", _LotNumber)
        para(5) = New SqlClient.SqlParameter("@Action", _CongDoan)
        para(6) = New SqlClient.SqlParameter("@SoCongDoan", _SoCongDoan)
        If _SoCongDoan.Contains(" ") Then
            para(6) = New SqlClient.SqlParameter("@SoCongDoan", DBNull.Value)
        End If
        GridControl2.DataSource = _db.ExecuteStoreProcedureTB("sp_QC_KQCV_BackendResult", para)
        GridControlSetFormat(GridView2, 3)
        GridView2.OptionsView.ShowFooter = False
        GridView2.Columns("ID").Visible = False
        GridView2.Columns("Ngay").Visible = False
        GridView2.Columns("CongDoan").Visible = False
        GridView2.Columns("ProductCode").Visible = False
        GridView2.Columns("LotNumber").Visible = False
        GridView2.Columns("SoCongDoan").Visible = False
        GridControlSetWidth(GridView2, 50)
        GridView2.Columns("PhuongPhapXuLy").Width = 500

        If GridView2.Columns("DoDayNguyenLieu") IsNot Nothing Then
            GridControlSetFormatNumber(GridView2, "DoDayNguyenLieu", 3)
        End If

        If Not IsDBNull(GridView1.GetFocusedRowCellValue("NhanVienKiem")) Then
            GridControl3.DataSource = TachCodeNhanVien(GridView1.GetFocusedRowCellValue("NhanVienKiem"))
            GridControlSetFormat(GridView3)
            GridView3.Columns("Code").Width = 50
            GridView3.Columns("MSNV").Width = 55
            GridView3.Columns("HoTen").Width = 135
            GridView3.Columns("ChucVu").Width = 85
        End If
    End Sub
    Function TachCodeNhanVien(msnv As String) As DataTable
        Dim dtCode As DataTable = _db.FillDataTable("SELECT MSNV, HoTen, Code, ChucVu 
                                                    FROM QC_KQCV_MasterNhanVien
                                                    UNION
                                                    SELECT EmpID, EmpName, Code, null AS ChucVu
                                                    FROM KQQC_Code WHERE Code <> ''
                                                    AND Code NOT IN (SELECT Code FROM QC_KQCV_MasterNhanVien)")
        msnv = msnv + " "
        Dim dtShow As New DataTable
        dtShow.Columns.Add("Code")
        dtShow.Columns.Add("MSNV")
        dtShow.Columns.Add("HoTen")
        dtShow.Columns.Add("ChucVu")

        Dim n As Integer = 0
        Dim lenChar As Integer = 0

        For index = n To msnv.Length - 1
            If msnv(index) >= "0" And msnv(index) <= "z" Then
                lenChar += 1
            ElseIf msnv(index) = " " Then
                Dim msnvNew As String = msnv.Substring(index - lenChar, lenChar)
                If msnvNew Is Nothing Or msnvNew = " " Or msnvNew = "" Then
                    Continue For
                Else
                    For Each r As DataRow In dtCode.Rows
                        If r("Code") = msnvNew Then
                            For Each rCheck As DataRow In dtShow.Rows
                                If msnvNew = rCheck("Code") Then
                                    lenChar = 0
                                    GoTo Label
                                End If
                            Next
                            dtShow.Rows.Add(msnvNew, r("MSNV"), r("HoTen"), r("ChucVu"))
                        End If
                    Next
                    lenChar = 0
                End If
            End If
Label:
            n += 1
        Next
        Return dtShow
    End Function

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        If chbDetail.Checked Then
            Return
        End If
        GridControlReadOnly(GridView2, False)
        GridView2.Columns("DanhGia").OptionsColumn.ReadOnly = True
        GridControlSetColorEdit(GridView2)

        If _CongDoan <> "Ép đơn tầng (hàng khác)" Then
            GridView2.Columns("TanSoKiemTra").ColumnEdit = cbbTanSoKiemTra
        Else
            GridView2.Columns("TanSoKiemTra").Visible = False
        End If
        Dim dtMaLoi As DataTable = _db.FillDataTable("select * from QC_KQCV_MasterError")
        GridView2.Columns("MaLoiGH").ColumnEdit = GridControlLoadLookUpEdit(dtMaLoi, "MaLoi", "MaLoi")
        GridView2.Columns("MaLoiNG").ColumnEdit = GridControlLoadLookUpEdit(dtMaLoi, "MaLoi", "MaLoi")

        Dim dtMethod As DataTable = _db.FillDataTable("select KiTu from QC_KQCV_MasterMethod")
        Dim riEditor As New RepositoryItemComboBox()
        For Each r As DataRow In dtMethod.Rows
            riEditor.Items.Add(r("KiTu"))
        Next
        riEditor.DropDownRows = 20
        GridControl2.RepositoryItems.Add(riEditor)
        GridView2.Columns("PhuongPhapXuLy").ColumnEdit = riEditor

        If GridView2.Columns("DoDayNguyenLieu") IsNot Nothing Then
            GridControlSetFormatNumber(GridView2, "DoDayNguyenLieu", 3)
        End If
    End Sub

    Private Sub GridView2_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView2.CellValueChanged
        If GridView2.Editable = True And e.Column.ReadOnly = False Then
            If e.Column.FieldName = "Ngay" Then
                Dim obj As New QC_KQCV_BackendResult
                obj.Ngay_K = e.Value
                obj.CongDoan_K = cbbCongDoan.Text
                obj.ProductCode_K = txtProductCode.Text
                obj.LotNumber_K = _LotNumber
                obj.SoCongDoan_K = _SoCongDoan
                If Not _db.ExistObject(obj) Then
                    'Insert Head
                    Dim paraH(6) As SqlClient.SqlParameter
                    paraH(0) = New SqlClient.SqlParameter("@Action", "InsertSoLuongLo")
                    paraH(1) = New SqlClient.SqlParameter("@StartDate", e.Value)
                    paraH(2) = New SqlClient.SqlParameter("@EndDate", e.Value)
                    paraH(3) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text)
                    paraH(4) = New SqlClient.SqlParameter("@LotNumber", _LotNumber)
                    paraH(5) = New SqlClient.SqlParameter("@CongDoan", cbbCongDoan.Text)
                    paraH(6) = New SqlClient.SqlParameter("@SoCongDoan", _SoCongDoan)
                    obj.DanhGia = "OK"
                    obj.CreateUser = CurrentUser.UserID
                    obj.CreateDate = DateTime.Now
                    _db.Insert(obj)
                    _db.ExecuteStoreProcedure("sp_QC_KQCV_BackendResult", paraH)

                    'Insert Detail
                    Dim paraCopy(2) As SqlClient.SqlParameter
                    paraCopy(0) = New SqlClient.SqlParameter("@Action", "CopyByDay")
                    paraCopy(1) = New SqlClient.SqlParameter("@ID", GridView2.GetFocusedRowCellValue("ID"))
                    paraCopy(2) = New SqlClient.SqlParameter("@Ngay", e.Value)
                    _db.ExecuteStoreProcedure("sp_QC_KQCV_BackendResult", paraCopy)
                    _db.ExecuteNonQuery(String.Format("DELETE QC_KQCV_BackendResult_Detail
                                                       WHERE ID = '{0}'",
                                                       GridView2.GetFocusedRowCellValue("ID")))
                    GridView2.DeleteSelectedRows()
                    Return
                End If
            End If

            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format("UPDATE QC_KQCV_BackendResult_Detail 
                                               SET {0} = @Value
                                               WHERE ID = '{1}'",
                                               e.Column.FieldName,
                                               GridView2.GetFocusedRowCellValue("ID")), para)

            If e.Column.FieldName = "SoLuongLoiGH" Or e.Column.FieldName = "SoLuongLoiNG" Then
                DanhGiaLoi()
            ElseIf e.Column.FieldName = "ThoiGianKiem" Or e.Column.FieldName = "SoLuongKiem" Then
                UpdateValues(e.Column.FieldName)
            ElseIf e.Column.FieldName = "SoCongDoan" Or e.Column.FieldName = "SoMay" Or
                   e.Column.FieldName = "NhanVienKiem" Or e.Column.FieldName = "PhuongPhapXuLy" Or
                   e.Column.FieldName = "XacNhanBoPhanLienQuan" Or e.Column.FieldName = "GhiChu" Then
                UpdateMergeString(e.Column.FieldName)
            ElseIf e.Column.FieldName = "LanKiem" Then
                UpdateDanhGiaLoi()
            End If
        End If
    End Sub

    Sub DanhGiaLoi()
        Dim obj As New QC_KQCV_BackendResult
        obj.Ngay_K = _Ngay.Date
        obj.CongDoan_K = _CongDoan
        obj.ProductCode_K = _ProductCode
        obj.LotNumber_K = _LotNumber
        obj.SoCongDoan_K = _SoCongDoan
        _db.GetObject(obj)
        Dim para(7) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", _Ngay.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", _Ngay.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", _ProductCode)
        para(3) = New SqlClient.SqlParameter("@LotNumber", _LotNumber)
        para(4) = New SqlClient.SqlParameter("@CongDoan", _CongDoan)
        para(5) = New SqlClient.SqlParameter("@SoCongDoan", _SoCongDoan)
        para(6) = New SqlClient.SqlParameter("@LanKiem", GridView2.GetFocusedRowCellValue("LanKiem"))
        para(7) = New SqlClient.SqlParameter("@TanSo", GridView2.GetFocusedRowCellValue("TanSoKiemTra"))
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
    Sub UpdateValues(columnName As String)
        Dim para(6) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", _Ngay.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", _Ngay.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", _ProductCode)
        para(3) = New SqlClient.SqlParameter("@LotNumber", _LotNumber)
        para(4) = New SqlClient.SqlParameter("@CongDoan", _CongDoan)
        para(5) = New SqlClient.SqlParameter("@SoCongDoan", _SoCongDoan)
        para(6) = New SqlClient.SqlParameter("@LanKiem", GridView2.GetFocusedRowCellValue("LanKiem"))

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
        obj.Ngay_K = _Ngay.Date
        obj.CongDoan_K = _CongDoan
        obj.ProductCode_K = _ProductCode
        obj.LotNumber_K = _LotNumber
        obj.SoCongDoan_K = _SoCongDoan
        _db.GetObject(obj)
        If columnName = "ThoiGianKiem" Then
            obj.ThoiGianKiem = values
        Else
            obj.SoLuongKiem = values
        End If
        _db.Update(obj)
    End Sub
    Sub UpdateMergeString(columnName As String)
        Dim para(5) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", _Ngay.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", _Ngay.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", _ProductCode)
        para(3) = New SqlClient.SqlParameter("@LotNumber", _LotNumber)
        para(4) = New SqlClient.SqlParameter("@CongDoan", _CongDoan)
        para(5) = New SqlClient.SqlParameter("@SoCongDoan", _SoCongDoan)

        Dim values As Object = _db.ExecuteScalar(String.Format("SELECT STUFF(
                                                                ( SELECT DISTINCT ' '+  {0}
                                                                FROM QC_KQCV_BackendResult_Detail AS D 
                                                                WHERE D.Ngay = H.Ngay
                                                                AND D.CongDoan = H.CongDoan
                                                                AND D.ProductCode = H.ProductCode
                                                                AND D.LotNumber = H.LotNumber
                                                                AND SoCongDoan = @SoCongDoan
                                                                FOR XML PATH ('') )
                                                                , 1, 1, '')  AS SoCongDoan
                                                                FROM QC_KQCV_BackendResult AS H
                                                                WHERE H.Ngay BETWEEN @StartDate AND @EndDate
                                                                AND H.CongDoan = @CongDoan
                                                                AND H.ProductCode = @ProductCode
                                                                AND H.LotNumber = @LotNumber
                                                                AND SoCongDoan = @SoCongDoan", columnName), para)
        Dim obj As New QC_KQCV_BackendResult
        obj.Ngay_K = _Ngay.Date
        obj.CongDoan_K = _CongDoan
        obj.ProductCode_K = _ProductCode
        obj.LotNumber_K = _LotNumber
        obj.SoCongDoan_K = _SoCongDoan
        _db.GetObject(obj)
        'If columnName = "SoCongDoan" Then
        '    obj.SoCongDoan_K = values
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
    Sub UpdateDanhGiaLoi()
        Dim para(7) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", _Ngay.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", _Ngay.Date)
        para(2) = New SqlClient.SqlParameter("@ProductCode", _ProductCode)
        para(3) = New SqlClient.SqlParameter("@LotNumber", _LotNumber)
        para(4) = New SqlClient.SqlParameter("@CongDoan", _CongDoan)
        para(5) = New SqlClient.SqlParameter("@LanKiem", "")
        para(6) = New SqlClient.SqlParameter("@TanSo", "")
        para(7) = New SqlClient.SqlParameter("@SoCongDoan", _SoCongDoan)
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
End Class