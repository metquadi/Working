﻿Imports System.Drawing
Imports System.Windows.Forms
Imports CommonDB
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports PublicUtility
Public Class FrmLaserPlasmaBH_2
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim _copyVal As Object
    Dim _copyCol As String = ""
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
    Private Sub FrmLaserPlasmaBH_2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteStartDate.EditValue = GetStartDayOfMonth(Date.Now)
        dteEndDate.EditValue = GetEndDayOfMonth(Date.Now)
        Dim obj As New Main_UserRight
        obj.FormID_K = Tag
        obj.UserID_K = CurrentUser.UserID
        _db.GetObject(obj)
        If obj.IsEdit = False Then
            ViewAccess()
        End If
    End Sub
    Sub ViewAccess()
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
    End Sub

    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        Dim para(3) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteStartDate.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteEndDate.DateTime.Date)
        para(2) = New SqlClient.SqlParameter("@PTH", "LPBNew")
        If txtProductCode.Text.Trim <> "" Then
            para(3) = New SqlClient.SqlParameter("@ProductCode", txtProductCode.Text.PadLeft(5, "0"))
        Else
            para(3) = New SqlClient.SqlParameter("@ProductCode", DBNull.Value)
        End If
        GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_KQQC_LoadLaserPlasmaBH", para)
        GridControlSetFormat(GridView1, 3)
        GridView1.Columns("Ngay").Width = 100
        GridView1.Columns("CongDoan").ColumnEdit = cbbProcess
        GridView1.Columns("TanSo").ColumnEdit = cbbTanSo
        GridView1.Columns("Ngay").ColumnEdit = dateRI
    End Sub

    Private Sub btnNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnNew.ItemClick
        If GridControl1.DataSource Is Nothing Then
            btnShow.PerformClick()
        End If
        GridControlReadOnly(GridView1, False)
        GridView1.Columns("ID").OptionsColumn.ReadOnly = True
        GridView1.Columns("CustomerName").OptionsColumn.ReadOnly = True
        GridView1.Columns("Method").OptionsColumn.ReadOnly = True
        GridControlSetColorEdit(GridView1)
        GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        If GridControl1.DataSource Is Nothing Then
            btnShow.PerformClick()
        End If
        GridControlReadOnly(GridView1, False)
        GridView1.Columns("ID").OptionsColumn.ReadOnly = True
        GridView1.Columns("CustomerName").OptionsColumn.ReadOnly = True
        GridView1.Columns("Method").OptionsColumn.ReadOnly = True
        GridControlSetColorEdit(GridView1)
        GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            For Each r As Integer In GridView1.GetSelectedRows
                Dim obj As New KQQC_LaserPlasmaBH
                obj.ID_K = GridView1.GetRowCellValue(r, "ID")
                _db.Delete(obj)
            Next
            GridView1.DeleteSelectedRows()
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(GridView1)
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            If e.RowHandle < 0 Then
                'If IsDBNull(GridView1.GetFocusedRowCellValue("SoTo")) Then
                '    ShowWarning("Phải nhập Số Tờ và Ngày trước !")
                '    ReturnOldValue(GridView1)
                '    Return
                'End If
                If IsDBNull(GridView1.GetFocusedRowCellValue("ID")) Then
                    Dim id = CreateID()
                    _db.ExecuteNonQuery(String.Format("INSERT INTO KQQC_LaserPlasmaBH
                                (ID, SoLanTest, DanhGiaSauCung, CreateUser, CreateDate)
                        VALUES  ('{0}', 1, 'OK', '{1}', GETDATE())",
                        id,
                        CurrentUser.UserID))
                    GridView1.SetFocusedRowCellValue("ID", id)
                    SetValue(GridView1, "SoLanTest", 1)
                    SetValue(GridView1, "DanhGiaSauCung", "OK")
                End If
            End If

            If e.Column.FieldName = "MSSP" Then
                Dim prodC = e.Value.ToString.PadLeft(5, "0")
                _db.ExecuteNonQuery(String.Format("UPDATE KQQC_LaserPlasmaBH
                                                    SET MSSP = '{0}'
                                                    WHERE ID = '{1}'",
                                                    prodC,
                                                    GridView1.GetFocusedRowCellValue("ID")))
                SetValue(GridView1, "MSSP", prodC)
                Dim wtObj As New WT_Product
                wtObj.ProductCode_K = prodC
                If _db.ExistObject(wtObj) Then
                    _db.GetObject(wtObj)
                    GridView1.SetFocusedRowCellValue("CustomerName", wtObj.CustomerName)
                    GridView1.SetFocusedRowCellValue("Method", wtObj.Method)
                End If
                Return
            ElseIf e.Column.FieldName = "SoMayGC" Then
                Dim smGC = e.Value.ToString.PadLeft(2, "0")
                _db.ExecuteNonQuery(String.Format("UPDATE KQQC_LaserPlasmaBH
                                                    SET SoMayGC = '{0}'
                                                    WHERE ID = '{1}'",
                                                    smGC,
                                                    GridView1.GetFocusedRowCellValue("ID")))
                SetValue(GridView1, "SoMayGC", smGC)
                Return
            End If

            If e.Column.FieldName = "SoLuongKiem" Or e.Column.FieldName = "SoTam" Then
                Dim slKiem = GridView1.GetFocusedRowCellValue("SoLuongKiem")
                Dim soTam = GridView1.GetFocusedRowCellValue("SoTam")
                If IsNumeric(slKiem) And IsNumeric(soTam) Then
                    If Integer.TryParse(slKiem / soTam, True) And soTam <= 2 And slKiem <= 28 And slKiem / soTam <= 14 Then
                        'Set về lại mặc đinh NULL (cho trường hợp sửa dữ liệu)
                        For i = 1 To 2
                            For j = 1 To 14
                                Dim col = "T" + i.ToString + "Cav" + j.ToString
                                _db.ExecuteNonQuery(String.Format("UPDATE KQQC_LaserPlasmaBH
                                                            SET {0} = NULL
                                                            WHERE ID = '{1}'",
                                                            col,
                                                            GridView1.GetFocusedRowCellValue("ID")))
                                GridView1.Columns(col).OptionsColumn.ReadOnly = True
                                GridView1.SetFocusedRowCellValue(col, DBNull.Value)
                                GridView1.Columns(col).OptionsColumn.ReadOnly = False
                            Next
                        Next
                        'Bắt đầu set OK cho số tấm và số lượng kiểm đã nhập
                        For i = 1 To soTam
                            For j = 1 To slKiem / soTam
                                Dim col = "T" + i.ToString + "Cav" + j.ToString
                                _db.ExecuteNonQuery(String.Format("UPDATE KQQC_LaserPlasmaBH
                                                                    SET {0} = 'OK'
                                                                    WHERE ID = '{1}'",
                                                                    col,
                                                                    GridView1.GetFocusedRowCellValue("ID")))
                                GridView1.Columns(col).OptionsColumn.ReadOnly = True
                                GridView1.SetFocusedRowCellValue(col, "OK")
                                GridView1.Columns(col).OptionsColumn.ReadOnly = False
                            Next
                        Next
                    Else
                        ReturnOldValue(GridView1)
                        ShowWarning("Không xác định được Số lượng kiểm trên mỗi tấm !")
                        Return
                    End If
                End If
            End If

            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format("UPDATE KQQC_LaserPlasmaBH
                                                SET {0} = @Value
                                                WHERE ID = '{1}'",
                                                e.Column.FieldName,
                                                GridView1.GetFocusedRowCellValue("ID")),
                                                para)
        End If
    End Sub
    Sub SetValue(gridV As GridView, col As String, val As Object)
        gridV.Columns(col).OptionsColumn.ReadOnly = True
        gridV.SetFocusedRowCellValue(col, val)
        gridV.Columns(col).OptionsColumn.ReadOnly = False
    End Sub
    Function CreateID() As String
        Dim val = _db.ExecuteScalar(String.Format("SELECT ISNULL(Right(MAX(ID), 4), 0)
                                                    FROM KQQC_LaserPlasmaBH
                                                    WHERE ID LIKE '{0}%'",
                                                    Date.Now.ToString("yyMMdd")))
        Return Date.Now.ToString("yyMMdd") + (Integer.Parse(val) + 1).ToString.PadLeft(4, "0")
    End Function

    Private Sub GridView1_RowStyle(sender As Object, e As RowStyleEventArgs) Handles GridView1.RowStyle
        If GridView1.GetRowCellValue(e.RowHandle, "DanhGiaSauCung") IsNot Nothing Then
            If GridView1.GetRowCellValue(e.RowHandle, "DanhGiaSauCung").ToString.ToUpper = "NG" Then
                e.Appearance.BackColor = Color.Red
                e.Appearance.ForeColor = Color.White
            End If
        End If
    End Sub
    Sub ReturnOldValue(gridV As GridView)
        Dim oldVal As Object = gridV.ActiveEditor.OldEditValue
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = True
        gridV.SetFocusedRowCellValue(gridV.FocusedColumn.FieldName, oldVal)
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = False
    End Sub
    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        _copyVal = GridView1.GetFocusedRowCellValue(GridView1.FocusedColumn.FieldName)
        _copyCol = GridView1.FocusedColumn.FieldName
    End Sub

    Private Sub GridView1_ShownEditor(sender As Object, e As EventArgs) Handles GridView1.ShownEditor
        Dim view = CType(sender, GridView)
        Dim editor = TryCast(view.ActiveEditor, TextEdit)
        If editor Is Nothing Then Return
        editor.ContextMenuStrip = ContextMenuStrip1
    End Sub

    Private Sub btnPaste_Click(sender As Object, e As EventArgs) Handles btnPaste.Click
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Value", _copyVal)
        GridView1.Columns(_copyCol).OptionsColumn.ReadOnly = True
        For Each r As Integer In GridView1.GetSelectedRows
            _db.ExecuteNonQuery(String.Format("UPDATE KQQC_LaserPlasmaBH
                                                SET {0} = @Value
                                                WHERE ID = '{1}'",
                                            _copyCol,
                                            GridView1.GetRowCellValue(r, "ID")),
                                            para)
            GridView1.SetRowCellValue(r, _copyCol, _copyVal)
        Next
        GridView1.Columns(_copyCol).OptionsColumn.ReadOnly = False
    End Sub
End Class