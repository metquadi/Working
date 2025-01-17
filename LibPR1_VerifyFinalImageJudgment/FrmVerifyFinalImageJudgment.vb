﻿Imports PublicUtility
Imports CommonDB
Public Class FrmVerifyFinalImageJudgment
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)

    Private Sub FrmVerifyFinalImageJudgment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnShow.PerformClick()
        Dim obj As New Main_UserRight
        obj.FormID_K = Tag
        obj.UserID_K = CurrentUser.UserID
        _db.GetObject(obj)
        If obj.IsEdit = False And obj.IsAdmin = False Then
            ViewAccess()
        End If
        If obj.IsAdmin = False Then
            ViewAccessLock()
        End If
        Dim lockObj As New PR1_VFIJ_Lock
        lockObj.ID_K = "01"
        _db.GetObject(lockObj)
        chbLock.Checked = lockObj.Lock
    End Sub
    Sub ViewAccess()
        btnEdit.Enabled = False
        btnImport.Enabled = False
    End Sub
    Sub ViewAccessLock()
        chbLock.ReadOnly = True
    End Sub

    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_PR1_VFIJ_VerifyFinalImageJudgment")
        GridControlSetFormat(BandedGridView1, 2)
        GridControlSetFormatDateAndTime(BandedGridView1, "ChangeDate")
        BandedGridView1.Columns("ChangeDate").Width = 100
        BandedGridView1.Columns("CoAOIHayKhong").ColumnEdit = cbbOX
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        GridControlReadOnly(BandedGridView1, False)
        BandedGridView1.Columns("ProductCode").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("LotNumber").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("Method").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("CustomerName").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("ProductName").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("ProcessNameE").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("ChangeDate").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("Total_Ship").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("TongHinhAnhLoiMayAOIBat_Image").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("Total_Actual").OptionsColumn.ReadOnly = True
        GridControlSetColorEdit(BandedGridView1)
    End Sub

    Dim _isEdit As Boolean = True
    Dim _PdCode As New List(Of String)
    Dim _LotNo As New List(Of String)
    Private Sub BandedGridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles BandedGridView1.CellValueChanged
        If _isEdit And BandedGridView1.Editable And e.Column.ReadOnly = False Then
            If e.Column.FieldName = "CoAOIHayKhong" And chbLock.Checked Then
                If Not IsDBNull(BandedGridView1.ActiveEditor.OldEditValue) And
                    _PdCode.Contains(BandedGridView1.GetFocusedRowCellValue("ProductCode")) = False And
                    _LotNo.Contains(BandedGridView1.GetFocusedRowCellValue("LotNumber")) = False Then
                    ShowWarning("Lock !")
                    _isEdit = False
                    BandedGridView1.SetFocusedRowCellValue("CoAOIHayKhong", BandedGridView1.ActiveEditor.OldEditValue)
                    _isEdit = True
                    Return
                Else
                    If _PdCode.Contains(BandedGridView1.GetFocusedRowCellValue("ProductCode")) = False Then
                        _PdCode.Add(BandedGridView1.GetFocusedRowCellValue("ProductCode"))
                    End If
                    If _LotNo.Contains(BandedGridView1.GetFocusedRowCellValue("LotNumber")) = False Then
                        _LotNo.Add(BandedGridView1.GetFocusedRowCellValue("LotNumber"))
                    End If
                End If
            End If
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format(" UPDATE PR1_VFIJ_VerifyFinalImageJudgment
                                                SET {0} = @Value
                                                WHERE ProductCode = '{1}'
                                                AND LotNumber = '{2}'",
                                                e.Column.FieldName,
                                                BandedGridView1.GetFocusedRowCellValue("ProductCode"),
                                                BandedGridView1.GetFocusedRowCellValue("LotNumber")),
                                                para)
            Dim valSum = 0
            If e.Column.FieldName.Contains("_Ship") Then
                valSum = _db.ExecuteScalar(String.Format("SELECT ISNULL(Err001_Ship, 0) + ISNULL(Err002_Ship, 0) + 
                                            ISNULL(Err003_Ship, 0) + ISNULL(Err004_Ship, 0) + 
                                            ISNULL(Err005_Ship, 0) + ISNULL(Err006_Ship, 0) + 
                                            ISNULL(Err007_Ship, 0) + ISNULL(Err009_Ship, 0) + 
                                            ISNULL(Err627_Ship, 0) + ISNULL(Err639_Ship, 0) + 
                                            ISNULL(Other_Ship, 0)
                                            FROM PR1_VFIJ_VerifyFinalImageJudgment
                                            WHERE ProductCode = '{0}'
                                            AND LotNumber = '{1}'",
                                            BandedGridView1.GetFocusedRowCellValue("ProductCode"),
                                            BandedGridView1.GetFocusedRowCellValue("LotNumber")))
                BandedGridView1.SetFocusedRowCellValue("Total_Ship", valSum)
            ElseIf e.Column.FieldName.Contains("_Image") Then
                valSum = _db.ExecuteScalar(String.Format("SELECT ISNULL(Err005_Image, 0) + ISNULL(Err001_Image, 0) + ISNULL(Err002_004_009_Image, 0) + 
                                            ISNULL(Err007_Image, 0) + ISNULL(Err003_Image, 0) + ISNULL(Err006_Image, 0) + 
                                            ISNULL(Err639_Image, 0) + ISNULL(Err627_Image, 0)
                                            FROM PR1_VFIJ_VerifyFinalImageJudgment
                                            WHERE ProductCode = '{0}'
                                            AND LotNumber = '{1}'",
                                            BandedGridView1.GetFocusedRowCellValue("ProductCode"),
                                            BandedGridView1.GetFocusedRowCellValue("LotNumber")))
                BandedGridView1.SetFocusedRowCellValue("TongHinhAnhLoiMayAOIBat_Image", valSum)
            ElseIf e.Column.FieldName.Contains("_Actual") Then
                valSum = _db.ExecuteScalar(String.Format("SELECT ISNULL(Err001_Actual, 0) + ISNULL(Err002_Actual, 0) + ISNULL(Err003_Actual, 0) 
                                            + ISNULL(Err004_Actual, 0) + ISNULL(Err005_Actual, 0) + ISNULL(Err006_Actual, 0)
                                            + ISNULL(Err007_Actual, 0) + ISNULL(Err009_Actual, 0) + ISNULL(Err627_Actual, 0) 
                                            + ISNULL(Err639_Actual, 0) + ISNULL(Other_Actual, 0)
                                            FROM PR1_VFIJ_VerifyFinalImageJudgment
                                            WHERE ProductCode = '{0}'
                                            AND LotNumber = '{1}'",
                                            BandedGridView1.GetFocusedRowCellValue("ProductCode"),
                                            BandedGridView1.GetFocusedRowCellValue("LotNumber")))
                BandedGridView1.SetFocusedRowCellValue("Total_Actual", valSum)
            End If
        End If
    End Sub

    Private Sub btnGetData_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnGetData.ItemClick
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Type", "GetData")
        _db.ExecuteStoreProcedure("sp_PR1_VFIJ_VerifyFinalImageJudgment", para)
        ShowSuccess()
        btnShow.PerformClick()
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(BandedGridView1)
    End Sub

    Private Sub btnImport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnImport.ItemClick
        Dim dtImp = ImportEXCEL(True)
        If dtImp.Rows.Count > 0 Then
            Try
                _db.BeginTransaction()
                For Each r As DataRow In dtImp.Rows
                    Dim obj As New PR1_VFIJ_VerifyFinalImageJudgment
                    obj.ProductCode_K = r("Product Code")
                    obj.LotNumber_K = r("Lot Number")
                    _db.GetObject(obj)
                    obj.ChangeDate = IIf(IsDBNull(r("Change Date")), Nothing, r("Change Date"))
                    obj.CoAOIHayKhong = IIf(IsDBNull(r("Có AOI hay không (O: có, X: không)")), "", r("Có AOI hay không (O: có, X: không)"))
                    obj.TotalBarcodeKiemThucTe = IIf(IsDBNull(r("Total Barcode kiểm thực tế")), Nothing, r("Total Barcode kiểm thực tế"))
                    obj.TongKetLoiDenBarcode = IIf(IsDBNull(r("Tổng kết lỗi đến Barcode")), Nothing, r("Tổng kết lỗi đến Barcode"))
                    obj.Err001_Ship = IIf(IsDBNull(r("Err001_Ship")), 0, r("Err001_Ship"))
                    obj.Err002_Ship = IIf(IsDBNull(r("Err002_Ship")), 0, r("Err002_Ship"))
                    obj.Err003_Ship = IIf(IsDBNull(r("Err003_Ship")), 0, r("Err003_Ship"))
                    obj.Err004_Ship = IIf(IsDBNull(r("Err004_Ship")), 0, r("Err004_Ship"))
                    obj.Err005_Ship = IIf(IsDBNull(r("Err005_Ship")), 0, r("Err005_Ship"))
                    obj.Err006_Ship = IIf(IsDBNull(r("Err006_Ship")), 0, r("Err006_Ship"))
                    obj.Err007_Ship = IIf(IsDBNull(r("Err007_Ship")), 0, r("Err007_Ship"))
                    obj.Err009_Ship = IIf(IsDBNull(r("Err009_Ship")), 0, r("Err009_Ship"))
                    obj.Err627_Ship = IIf(IsDBNull(r("Err627_Ship")), 0, r("Err627_Ship"))
                    obj.Err639_Ship = IIf(IsDBNull(r("Err639_Ship")), 0, r("Err639_Ship"))
                    obj.Other_Ship = IIf(IsDBNull(r("Other_Ship")), 0, r("Other_Ship"))
                    obj.Err005_Image = IIf(IsDBNull(r("Err005_Image")), 0, r("Err005_Image"))
                    obj.Err001_Image = IIf(IsDBNull(r("Err001_Image")), 0, r("Err001_Image"))
                    obj.Err002_004_009_Image = IIf(IsDBNull(r("Err002_004_009_Image")), 0, r("Err002_004_009_Image"))
                    obj.Err007_Image = IIf(IsDBNull(r("Err007_Image")), 0, r("Err007_Image"))
                    obj.Err003_Image = IIf(IsDBNull(r("Err003_Image")), 0, r("Err003_Image"))
                    obj.Err006_Image = IIf(IsDBNull(r("Err006_Image")), 0, r("Err006_Image"))
                    obj.Err639_Image = IIf(IsDBNull(r("Err639_Image")), 0, r("Err639_Image"))
                    obj.Err627_Image = IIf(IsDBNull(r("Err627_Image")), 0, r("Err627_Image"))
                    obj.Err001_Actual = IIf(IsDBNull(r("Err001_Actual")), 0, r("Err001_Actual"))
                    obj.Err002_Actual = IIf(IsDBNull(r("Err002_Actual")), 0, r("Err002_Actual"))
                    obj.Err003_Actual = IIf(IsDBNull(r("Err003_Actual")), 0, r("Err003_Actual"))
                    obj.Err004_Actual = IIf(IsDBNull(r("Err004_Actual")), 0, r("Err004_Actual"))
                    obj.Err005_Actual = IIf(IsDBNull(r("Err005_Actual")), 0, r("Err005_Actual"))
                    obj.Err006_Actual = IIf(IsDBNull(r("Err006_Actual")), 0, r("Err006_Actual"))
                    obj.Err007_Actual = IIf(IsDBNull(r("Err007_Actual")), 0, r("Err007_Actual"))
                    obj.Err009_Actual = IIf(IsDBNull(r("Err009_Actual")), 0, r("Err009_Actual"))
                    obj.Err627_Actual = IIf(IsDBNull(r("Err627_Actual")), 0, r("Err627_Actual"))
                    obj.Err639_Actual = IIf(IsDBNull(r("Err639_Actual")), 0, r("Err639_Actual"))
                    obj.Other_Actual = IIf(IsDBNull(r("Other_Actual")), 0, r("Other_Actual"))
                    obj.MSNVTongKet = IIf(IsDBNull(r("MSNV Tổng Kết")), "", r("MSNV Tổng Kết"))
                    obj.GhiChu = IIf(IsDBNull(r("Ghi Chú")), "", r("Ghi Chú"))
                    If _db.ExistObject(obj) Then
                        _db.Update(obj)
                    Else
                        _db.Insert(obj)
                    End If
                Next
                _db.Commit()
            Catch ex As Exception
                _db.RollBack()
                ShowWarning(ex.Message)
            End Try
        Else
            ShowWarning("Không có dữ liệu !")
        End If
    End Sub

    Private Sub chbLock_CheckedChanged(sender As Object, e As EventArgs) Handles chbLock.CheckedChanged
        Dim obj As New PR1_VFIJ_Lock
        obj.ID_K = "01"
        _db.GetObject(obj)
        If chbLock.Checked Then
            obj.Lock = True
        Else
            obj.Lock = False
        End If
        obj.UserChange = CurrentUser.UserID
        obj.DateChange = DateTime.Now
        _db.Update(obj)
    End Sub
End Class