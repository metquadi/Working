﻿Imports System.Drawing
Imports CommonDB
Imports PublicUtility
Public Class FrmSampleSendingWarning
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim _isEdit As Boolean = True
    Dim _isAdmin As Boolean = False

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

    Private Sub FrmSampleSendingWarning_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteStartDate.EditValue = Date.Now
        dteEndDate.EditValue = Date.Now
        Dim obj As New Main_UserRight
        obj.UserID_K = CurrentUser.UserID
        obj.FormID_K = Tag
        _db.GetObject(obj)
        If obj.IsEdit = False Then
            ViewAccess()
            _isEdit = False
        End If
        If obj.IsAdmin Then
            _isAdmin = True
        End If
        grcProgressBar.Visible = False
        btnShow.PerformClick()
        CheckAndSend()
    End Sub
    Sub ViewAccess()
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnImport.Enabled = False
    End Sub

    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        Dim para(1) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteStartDate.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteEndDate.DateTime.Date)
        GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_QA_SSW_ExportList", para)
        GridControlSetFormat(GridView1, 4, "OQATime")
        GridView1.Columns("PromiseTime").Width = 120
        GridView1.Columns("TGNhanPR3").Width = 120
        GridView1.Columns("ProductName").Width = 150
        GridView1.Columns("ProcessNameE").Width = 100
        GridView1.Columns("Note").Width = 150
        GridControlSetFormatDateAndTime(GridView1, "PromiseTime")
        GridView1.Columns("PromiseTime").ColumnEdit = GridControlDateTimeEdit()
        GridControlSetFormatDateAndTime(GridView1, "TGNhanPR3")
        GridView1.Columns("TGNhanPR3").ColumnEdit = GridControlDateTimeEdit()
        GridView1.Columns("Result").ColumnEdit = cbbOkNg
        GridView1.Columns("OQATime").ColumnEdit = GridControlTimeEdit()
        GridView1.Columns("OQAMachine").ColumnEdit = cbbOQAMachine()
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        GridControlReadOnly(GridView1, True)
        GridView1.Columns("PromiseTime").OptionsColumn.ReadOnly = False
        GridView1.Columns("OQATime").OptionsColumn.ReadOnly = False
        GridView1.Columns("OQAMachine").OptionsColumn.ReadOnly = False
        GridView1.Columns("Result").OptionsColumn.ReadOnly = False
        GridView1.Columns("Note").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            For Each r As Integer In GridView1.GetSelectedRows
                Dim obj As New QA_SSW_ExportList
                obj.ProductCode_K = GridView1.GetRowCellValue(r, "ProductCode")
                obj.LotNumber_K = GridView1.GetRowCellValue(r, "LotNumber")
                _db.Delete(obj)
            Next
            GridView1.DeleteSelectedRows()
        End If
    End Sub

    Private Sub btnImport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnImport.ItemClick
        grcProgressBar.Visible = True
        Dim dtImp = ImportEXCEL(dteStartDate.DateTime.ToString("dd-MMM"))
        If dtImp.Rows.Count > 0 Then
            Dim row = 0
            Try
                _db.BeginTransaction()
                Dim succ = 0
                ProgressBarControl1.Properties.Step = 1
                ProgressBarControl1.Properties.PercentView = True
                ProgressBarControl1.Properties.Maximum = dtImp.Rows.Count
                ProgressBarControl1.Properties.Minimum = 0
                ProgressBarControl1.Properties.ShowTitle = True
                For Each r As DataRow In dtImp.Rows
                    row += 1
                    If IsDBNull(r("ProductCode")) Or IsDBNull(r("LotNumber")) Then Continue For
                    Dim obj As New QA_SSW_ExportList
                    obj.NgayXuat = r("NgayXuat")
                    obj.ProductCode_K = r("ProductCode")
                    obj.LotNumber_K = r("LotNumber")
                    obj.CreateUser = CurrentUser.UserID
                    obj.CreateDate = DateTime.Now
                    If Not _db.ExistObject(obj) Then
                        _db.Insert(obj)
                        succ += 1
                    End If

                    ProgressBarControl1.PerformStep()
                    ProgressBarControl1.Update()
                Next
                ProgressBarControl1.EditValue = 0
                _db.Commit()
                ShowSuccess(succ)
            Catch ex As Exception
                _db.RollBack()
                ShowWarning(ex.Message + " - Row " + row.ToString)
            End Try
        End If
        grcProgressBar.Visible = False
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(GridView1)
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format("UPDATE QA_SSW_ExportList
                                                SET {0} = @Value
                                                WHERE ProductCode = '{1}'
                                                AND LotNumber = '{2}'",
                                                e.Column.FieldName,
                                                GridView1.GetFocusedRowCellValue("ProductCode"),
                                                GridView1.GetFocusedRowCellValue("LotNumber")),
                                                para)
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        CheckAndSend()
    End Sub
    Sub CheckAndSend()
        If _isAdmin Then
            btnShow.PerformClick()
            Dim dtLate As New DataTable
            dtLate.Columns.Add("NgayXuat")
            dtLate.Columns.Add("ProductCode")
            dtLate.Columns.Add("LotNumber")
            For r = 0 To GridView1.RowCount
                If Not IsDate(GridView1.GetRowCellValue(r, "TGNhanPR3")) Then
                    If IsDate(GridView1.GetRowCellValue(r, "PromiseTime")) Then
                        Dim promiseTime As DateTime = GridView1.GetRowCellValue(r, "PromiseTime")
                        If DateTime.Now > promiseTime.AddMinutes(15) Then
                            dtLate.Rows.Add(GridView1.GetRowCellValue(r, "NgayXuat"),
                                        GridView1.GetRowCellValue(r, "ProductCode"),
                                        GridView1.GetRowCellValue(r, "LotNumber"))
                        End If
                    End If
                End If
            Next
            If dtLate.Rows.Count > 0 Then
                Dim dtMail = _db.FillDataTable("SELECT Mail FROM QA_SSW_EmailWarning")
                Dim listTo As New List(Of String)
                For Each r As DataRow In dtMail.Rows
                    listTo.Add(r("Mail"))
                Next
                'listTo.Add(CurrentUser.Mail)
                Dim titleMail = "[Auto Mail] - Bạn cần kiểm tra thời gian giao mẫu"
                Dim contentMail = "Bạn có lot hàng chưa giao mẫu đúng kỳ hạn, xin vui lòng kiểm tra lại:" +
                "<style>
                    th, td {
                      border: 1px solid black;
                      border-collapse: collapse;
                      width: 150px;
                    }
                    table{
                      border: 1px solid black;
                      border-collapse: collapse;
                    }
                </style>
                <br><br>
                <table>
                    <tr>
                        <th>Ngày xuất</th>
                        <th>Product Code</th>
                        <th>Lot Number</th>
                    </tr>"
                For Each r As DataRow In dtLate.Rows
                    contentMail += String.Format("<tr>
                                                <td>{0}</td>
                                                <td>{1}</td>
                                                <td>{2}</td>
                                             </tr>",
                                             Date.Parse(r("NgayXuat")).ToString("dd-MMM-yyyy"),
                                             r("ProductCode"),
                                             r("LotNumber"))
                Next
                contentMail += "</table>"
                SendMailBaoCaoAttach(Nothing, listTo, Nothing, Submit.Info, titleMail, contentMail, Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub GridView1_RowStyle(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles GridView1.RowStyle
        If Not IsDate(GridView1.GetRowCellValue(e.RowHandle, "TGNhanPR3")) Then
            If IsDate(GridView1.GetRowCellValue(e.RowHandle, "PromiseTime")) Then
                Dim promiseTime As DateTime = GridView1.GetRowCellValue(e.RowHandle, "PromiseTime")
                If DateTime.Now > promiseTime.AddMinutes(15) Then
                    e.Appearance.BackColor = Color.Red
                    e.Appearance.ForeColor = Color.White
                End If
            End If
        End If
    End Sub
End Class