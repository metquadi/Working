﻿Imports CommonDB
Imports PublicUtility

Public Class FrmContractReview
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)

    Private Sub mnuNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuNew.ItemClick
        Dim frm As New FrmContractLabor
        frm.Show()
    End Sub

    Private Sub mnuConfirm_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuConfirm.ItemClick
        Dim frm As New FrmContractLabor
        frm._ID = GridView1.GetFocusedRowCellValue("ID")
        frm.Show()
    End Sub

    Private Sub mnuShowAll_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuShowAll.ItemClick
        Dim para(2) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", dteStartDate.DateTime.Date)
        para(1) = New SqlClient.SqlParameter("@EndDate", dteEndDate.DateTime.Date)
        If rdoProcess.Checked Then
            para(2) = New SqlClient.SqlParameter("@Action", "Process")
        Else
            para(2) = New SqlClient.SqlParameter("@Action", "")
        End If
        GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_GA_CT_ReviewContract", para)
        GridControlSetFormat(GridView1, 3)
        GridView1.Columns("ID").Width = 100
    End Sub

    Private Sub mnuExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuExport.ItemClick
        If GridView1.RowCount > 0 Then
            GridControlExportExcel(GridView1)
        End If
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        mnuConfirm.PerformClick()
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If ShowQuestion("Bạn có chắc chắn muốn xóa dữ liệu ?") = DialogResult.Yes Then
            _db.ExecuteNonQuery(String.Format("DELETE GA_CT_ReviewContract
                                               WHERE ID = '{0}'", GridView1.GetFocusedRowCellValue("ID")))
            GridView1.DeleteSelectedRows()
        End If
    End Sub

    Private Sub FrmContractReview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteStartDate.EditValue = GetStartDayOfMonth(DateTime.Now)
        dteEndDate.EditValue = GetEndDayOfMonth(DateTime.Now)
        mnuShowAll.PerformClick()
        Dim obj As New Main_UserRight
        obj.UserID_K = CurrentUser.UserID
        obj.FormID_K = Tag
        _db.GetObject(obj)
        If obj.IsEdit = False And obj.IsAdmin = False Then
            ViewAccess()
        End If
    End Sub
    Sub ViewAccess()
        btnDelete.Enabled = False
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
End Class