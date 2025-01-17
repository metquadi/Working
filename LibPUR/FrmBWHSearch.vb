﻿Imports CommonDB
Imports PublicUtility
Imports System.Drawing

Public Class FrmBWHSearch : Inherits DevExpress.XtraEditors.XtraForm
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim isEdit As Boolean = True
    'Dim dbAS As New DBFunction(PublicConst.EnumServers.NDV_DB2_AS400)
    'Dim gridValueChange As Boolean = False


    'Private Sub mnuSearch_Click(sender As System.Object, e As System.EventArgs) Handles mnuSearch.Click
    '    Dim startDate
    '    If Trim(txtStartDate.Text).Length = 10 Then
    '        startDate = dtpOrderDate.Value.ToString("yyyy-MM-dd")
    '    Else
    '        startDate = "2001-01-01"
    '    End If

    '    Dim endDate
    '    If Trim(txtEndDate.Text).Length = 10 Then
    '        endDate = dtpOrderDateEnd.Value.ToString("yyyy-MM-dd")
    '    Else
    '        endDate = DateTime.Now.ToString("yyyy-MM-dd")
    '    End If

    '    Dim bdShowAll As New BindingSource()

    '    Dim SearchJCode As String
    '    If Trim(txtJCode.Text) = "" Then
    '        SearchJCode = ""
    '    Else
    '        SearchJCode = String.Format(" and d.JCode like '%{0}%' ", Trim(txtJCode.Text))
    '    End If

    '    Dim sql As String = String.Format("SELECT  h.ID , " +
    '    "d.OrderID, " +
    '    "h.EmployeeID, " +
    '    "h.Department, " +
    '    "d.JCode, " +
    '    "d.JName, " +
    '    "d.LeadTime, " +
    '    "d.MinQty, " +
    '    "d.Unit, " +
    '    "d.Air, " +
    '    "d.Quantity, " +
    '    "d.DeliveryDate, " +
    '    "d.ReceivedQty, " +
    '    "d.ReceivedDate, " +
    '    "d.RemainQty, " +
    '    "d.RemainDate, " +
    '    "d.Note " +
    '    "FROM    {0} h " +
    '    "INNER JOIN {1} d ON h.ID = d.ID " +
    '    "WHERE   h.OrderDate BETWEEN '{2}' AND '{3}'" + SearchJCode,
    '    PublicTable.Table_GSR_BWH, PublicTable.Table_GSR_BWHDetail, startDate, endDate)

    '    bdShowAll.DataSource = _db.FillDataTable(sql)
    '    gridD.DataSource = bdShowAll
    '    bnGrid.BindingSource = bdShowAll
    '    gridValueChange = True
    'End Sub

    'Private Sub mnuExport_Click(sender As System.Object, e As System.EventArgs) Handles mnuExport.Click
    '    ExportEXCEL(gridD)
    'End Sub

    'Private Sub dtpOrderDate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpOrderDate.ValueChanged
    '    If dtpOrderDateEnd.Value < dtpOrderDate.Value Then
    '        dtpOrderDateEnd.Value = dtpOrderDate.Value
    '    End If
    '    txtStartDate.Text = dtpOrderDate.Value.ToString("ddMMyyyy")
    'End Sub

    'Private Sub dtpOrderDateEnd_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpOrderDateEnd.ValueChanged
    '    If dtpOrderDateEnd.Value < dtpOrderDate.Value Then
    '        dtpOrderDate.Value = dtpOrderDateEnd.Value
    '    End If
    '    txtEndDate.Text = dtpOrderDateEnd.Value.ToString("ddMMyyyy")
    'End Sub

    'Private Sub gridD_CellValueChanged(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridD.CellValueChanged
    '    If gridValueChange = False Then Exit Sub

    '    If e.ColumnIndex = gridD.Columns("ReceivedQty").Index Then
    '        If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Or gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value Then
    '            MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        Else
    '            Dim objD As New GSR_BWHDetail
    '            objD.ID_K = gridD.CurrentRow.Cells("ID").Value
    '            objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

    '            _db.GetObject(objD)
    '            objD.ReceivedQty = IIf(gridD.CurrentRow.Cells("ReceivedQty").Value Is DBNull.Value, 0, gridD.CurrentRow.Cells("ReceivedQty").Value)
    '            objD.RemainQty = gridD.CurrentRow.Cells("Quantity").Value - objD.ReceivedQty
    '            _db.Update(objD)
    '            gridD.CurrentRow.Cells("RemainQty").Value = objD.RemainQty
    '        End If
    '    End If

    '    If e.ColumnIndex = gridD.Columns("ReceivedDate").Index Then
    '        If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Then
    '            MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        Else
    '            Dim objD As New GSR_BWHDetail
    '            objD.ID_K = gridD.CurrentRow.Cells("ID").Value
    '            objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

    '            _db.GetObject(objD)
    '            objD.ReceivedDate = IIf(gridD.CurrentRow.Cells("ReceivedDate").Value Is DBNull.Value, DateTime.Now, gridD.CurrentRow.Cells("ReceivedDate").Value)
    '            _db.Update(objD)
    '        End If
    '    End If

    '    If e.ColumnIndex = gridD.Columns("RemainQty").Index Then

    '        If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Or gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value Then
    '            MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        Else
    '            Dim objD As New GSR_BWHDetail
    '            objD.ID_K = gridD.CurrentRow.Cells("ID").Value
    '            objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

    '            _db.GetObject(objD)
    '            objD.RemainQty = IIf(gridD.CurrentRow.Cells("RemainQty").Value Is DBNull.Value, 0, gridD.CurrentRow.Cells("RemainQty").Value)
    '            _db.Update(objD)
    '        End If
    '    End If

    '    If e.ColumnIndex = gridD.Columns("RemainDate").Index Then
    '        If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Then
    '            MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        Else
    '            Dim objD As New GSR_BWHDetail
    '            objD.ID_K = gridD.CurrentRow.Cells("ID").Value
    '            objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

    '            _db.GetObject(objD)
    '            objD.RemainDate = IIf(gridD.CurrentRow.Cells("RemainDate").Value Is DBNull.Value, DateTime.Now, gridD.CurrentRow.Cells("RemainDate").Value)
    '            _db.Update(objD)
    '        End If
    '    End If

    '    If e.ColumnIndex = gridD.Columns("Note").Index Then
    '        If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Then
    '            MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        Else
    '            Dim objD As New GSR_BWHDetail
    '            objD.ID_K = gridD.CurrentRow.Cells("ID").Value
    '            objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

    '            _db.GetObject(objD)
    '            objD.Note = IIf(gridD.CurrentRow.Cells("Note").Value Is DBNull.Value, "", gridD.CurrentRow.Cells("Note").Value)
    '            _db.Update(objD)
    '        End If
    '    End If
    'End Sub

    'Private Sub gridD_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridD.CellContentClick
    '    gridD.CommitEdit(DataGridViewDataErrorContexts.Commit)
    'End Sub

    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        Dim para(2) As SqlClient.SqlParameter
        If rdoOrderDate.Checked Then
            para(0) = New SqlClient.SqlParameter("@StartDate", dteStart.DateTime.Date)
            para(1) = New SqlClient.SqlParameter("@EndDate", dteEnd.DateTime.Date)
        Else
            para(0) = New SqlClient.SqlParameter("@StartDateDelivery", dteStart.DateTime.Date)
            para(1) = New SqlClient.SqlParameter("@EndDateDelivery", dteEnd.DateTime.Date)
        End If
        para(2) = New SqlClient.SqlParameter("@Action", DBNull.Value)
        If chbAlarm.Checked Then
            para(2) = New SqlClient.SqlParameter("@Action", "Alarm")
        End If
        GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_PU_PUR", para)
        GridControlSetFormat(GridView1, 2)
        GridView1.Columns("ID").Width = 100
        GridView1.Columns("Note").Width = 300
    End Sub

    Private Sub FrmBWHSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteStart.EditValue = GetStartDayOfMonth(DateTime.Now)
        dteEnd.EditValue = GetEndDayOfMonth(DateTime.Now)
        Dim obj As New Main_UserRight
        obj.FormID_K = Tag
        obj.UserID_K = CurrentUser.UserID
        _db.GetObject(obj)
        If Not obj.IsEdit Then
            btnEdit.Enabled = False
            isEdit = False
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(GridView1)
    End Sub

    Private Sub dteStart_EditValueChanged(sender As Object, e As EventArgs) Handles dteStart.EditValueChanged
        If dteStart.DateTime > dteEnd.DateTime Then
            dteEnd.EditValue = dteStart.DateTime
        End If
    End Sub

    Private Sub dteEnd_EditValueChanged(sender As Object, e As EventArgs) Handles dteEnd.EditValueChanged
        If dteEnd.DateTime < dteStart.DateTime Then
            dteStart.EditValue = dteEnd.DateTime
        End If
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            If e.Column.FieldName = "LeadTimeWeek" Then
                _db.ExecuteNonQuery(String.Format("UPDATE GSR_BWHDetail
                                                   SET LeadTime = {0}
                                                   WHERE JCode = '{1}'",
                                                   e.Value,
                                                   GridView1.GetFocusedRowCellValue("JCode")))
                Return
            End If
            _db.ExecuteNonQuery(String.Format(" UPDATE GSR_BWHDetail
                                                SET {0} = @Value
                                                WHERE ID = '{1}'
                                                AND OrderID = '{2}'",
                                                e.Column.FieldName,
                                                GridView1.GetFocusedRowCellValue("ID"),
                                                GridView1.GetFocusedRowCellValue("OrderID")), para)
            If e.Column.FieldName = "ReceivedQty" Then
                Dim obj As New GSR_BWHDetail
                obj.ID_K = GridView1.GetFocusedRowCellValue("ID")
                obj.OrderID_K = GridView1.GetFocusedRowCellValue("OrderID")
                _db.GetObject(obj)
                obj.RemainQty = obj.Quantity - obj.ReceivedQty
                _db.Update(obj)
            End If
        End If
    End Sub

    Private Sub GridView1_RowStyle(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles GridView1.RowStyle
        If IsDBNull(GridView1.GetRowCellValue(e.RowHandle, "ETAHCM")) And
            GridView1.GetRowCellValue(e.RowHandle, "DeliveryDate") > New Date(2020, 9, 15) Then
            e.Appearance.BackColor = Color.Yellow
        End If
        If IsNumeric(GridView1.GetRowCellValue(e.RowHandle, "RemainQty")) Then
            If GridView1.GetRowCellValue(e.RowHandle, "RemainQty") > 0 And
            IsDBNull(GridView1.GetRowCellValue(e.RowHandle, "ETAHCMForRemainQty")) And
            GridView1.GetRowCellValue(e.RowHandle, "DeliveryDate") > New Date(2020, 9, 15) Then
                e.Appearance.BackColor = Color.Yellow
            End If
        End If
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        GridControlReadOnly(GridView1, True)
        GridView1.Columns("LeadTimeWeek").OptionsColumn.ReadOnly = False
        GridView1.Columns("Note").OptionsColumn.ReadOnly = False
        GridView1.Columns("ETAHCM").OptionsColumn.ReadOnly = False
        GridView1.Columns("ReceivedQty").OptionsColumn.ReadOnly = False
        GridView1.Columns("RemainQty").OptionsColumn.ReadOnly = False
        GridView1.Columns("ETAHCM").OptionsColumn.ReadOnly = False
        GridView1.Columns("ETAHCMForRemainQty").OptionsColumn.ReadOnly = False
        GridView1.Columns("Remark").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            For Each r As Integer In GridView1.GetSelectedRows
                Dim obj As New GSR_BWHDetail
                obj.ID_K = GridView1.GetRowCellValue(r, "ID")
                obj.OrderID_K = GridView1.GetRowCellValue(r, "OrderID")
                _db.Delete(obj)
            Next
            GridView1.DeleteSelectedRows()
        End If
    End Sub

    Private Sub btnPaste_Click(sender As Object, e As EventArgs) Handles btnPaste.Click
        Dim valCopy = My.Computer.Clipboard.GetText
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Value", valCopy)
        For Each r As Integer In GridView1.GetSelectedRows
            _db.ExecuteNonQuery(String.Format(" Update GSR_BWHDetail 
                                                SET ETAHCM = @Value 
                                                WHERE ID = '{0}'
                                                AND OrderID = '{1}'",
                                                GridView1.GetRowCellValue(r, "ID"),
                                                GridView1.GetRowCellValue(r, "OrderID")),
                                                para)
            GridView1.Columns("ETAHCM").OptionsColumn.ReadOnly = True
            GridView1.SetRowCellValue(r, "ETAHCM", valCopy)
            GridView1.Columns("ETAHCM").OptionsColumn.ReadOnly = False
        Next
    End Sub
End Class