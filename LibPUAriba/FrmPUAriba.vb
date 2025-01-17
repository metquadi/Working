﻿Imports CommonDB
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports PublicUtility
Public Class FrmPUAriba
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Private Sub FrmPUAriba_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        Dim para(5) As SqlClient.SqlParameter
        If rdoAllStatus.Checked Then
            para(0) = New SqlClient.SqlParameter("@POStatus", DBNull.Value)
        ElseIf rdoConfirmed.Checked Then
            para(0) = New SqlClient.SqlParameter("@POStatus", "Confirmed")
        ElseIf rdoOrdered.Checked Then
            para(0) = New SqlClient.SqlParameter("@POStatus", "Ordered")
        ElseIf rdoReceived.Checked Then
            para(0) = New SqlClient.SqlParameter("@POStatus", "Received")
        ElseIf rdoReceiving.Checked Then
            para(0) = New SqlClient.SqlParameter("@POStatus", "Receiving")
        ElseIf rdoRejected.Checked Then
            para(0) = New SqlClient.SqlParameter("@POStatus", "Rejected")
        ElseIf rdoShipped.Checked Then
            para(0) = New SqlClient.SqlParameter("@POStatus", "Shipped")
        End If
        para(1) = New SqlClient.SqlParameter("@StartRequiDate", GetStartDate(dteStartDate.DateTime))
        para(2) = New SqlClient.SqlParameter("@EndRequiDate", GetEndDate(dteEndDate.DateTime))
        If rdoAllLineType.Checked Then
            para(3) = New SqlClient.SqlParameter("@LineType", DBNull.Value)
        ElseIf rdoCatalogItem.Checked Then
            para(3) = New SqlClient.SqlParameter("@LineType", "Catalog Item")
        ElseIf rdoNonCatalogItem.Checked Then
            para(3) = New SqlClient.SqlParameter("@LineType", "Non-Catalog Item")
        End If
        If rdoAllProgress.Checked Then
            para(4) = New SqlClient.SqlParameter("@Progress", DBNull.Value)
        ElseIf rdoHoanTat.Checked Then
            para(4) = New SqlClient.SqlParameter("@Progress", "HoanTat")
        ElseIf rdoChuaHoanTat.Checked Then
            para(4) = New SqlClient.SqlParameter("@Progress", "ChuaHoanTat")
        End If
        If rdoUnclassified.Checked Then
            para(5) = New SqlClient.SqlParameter("@POID", "Unclassified")
        ElseIf rdoNotUnclassified.Checked Then
            para(5) = New SqlClient.SqlParameter("@POID", "NotUnclassified")
        Else
            para(5) = New SqlClient.SqlParameter("@POID", DBNull.Value)
        End If
        'Dim dt As DataTable = _db.ExecuteStoreProcedureTB("sp_PU_Ariba", para)
        'For r = 0 To dt.Rows.Count - 1
        '    If r < dt.Rows.Count - 1 Then
        '        If dt.Rows(r + 1)("RequisitionID").ToString.Contains(dt.Rows(r)("RequisitionID")) And
        '        dt.Rows(r + 1)("RequisitionID").ToString.Length > dt.Rows(r)("RequisitionID").ToString.Length Then
        '            dt.Rows.RemoveAt(r)
        '        End If
        '    End If
        'Next
        GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_PU_Ariba", para)
        GridControlSetFormat(GridView1, 2)

        GridView1.Columns("RequestDate").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("ConfirmedDate").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("ShippingTerm").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("ItemCode").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("ItemName").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("Unit").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("Quantity").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("POBalance").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("UnitPrice").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("Currency").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("NetAmount").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("InvoiceNo").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("InvoiceDate").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("ReceivingDate").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("InvoiceQuantity").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("SupplierID").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("SupplierName").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("TaxCode").AppearanceHeader.BackColor = Color.LawnGreen
        GridView1.Columns("Amount").AppearanceHeader.BackColor = Color.LawnGreen
    End Sub


    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        GridControlReadOnly(GridView1, False)
        GridView1.Columns("RequisitionID").OptionsColumn.ReadOnly = True
        GridView1.Columns("RequisitionLine").OptionsColumn.ReadOnly = True
        GridView1.Columns("POID").OptionsColumn.ReadOnly = True
        GridView1.Columns("POBalance").OptionsColumn.ReadOnly = True
        GridView1.Columns("Amount").OptionsColumn.ReadOnly = True
        GridControlSetColorEdit(GridView1)
        GridView1.Columns("POStatusPU").ColumnEdit = cbbPOStatus
        GridView1.Columns("TaxCode").ColumnEdit = cbbTaxCode
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable = True And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format(" UPDATE PU_Ariba
                                                SET {0} = @Value
                                                WHERE RequisitionID = '{1}'
                                                AND RequisitionLine = '{2}'",
                                                e.Column.FieldName,
                                                GridView1.GetFocusedRowCellValue("RequisitionID"),
                                                GridView1.GetFocusedRowCellValue("RequisitionLine")), para)
        End If
    End Sub

    Private Sub btnImport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnImport.ItemClick
        Dim dt As DataTable = ImportEXCEL(True)
        If dt.Rows.Count > 0 Then
            Dim count = 0
            Try
                _db.BeginTransaction()
                Dim succ = 0
                For Each r As DataRow In dt.Rows
                    count += 1
                    Dim obj As New PU_Ariba
                    obj.RequisitionID_K = r("(REQ) Requisition ID")
                    If obj.RequisitionID_K.ToString.Length > 8 Then
                        _db.ExecuteNonQuery(String.Format("DELETE PU_Ariba
                                                           WHERE RequisitionID LIKE '{0}%'
                                                           AND RequisitionID < '{1}'",
                                                           obj.RequisitionID_K.Substring(0, 8),
                                                           obj.RequisitionID_K))
                    End If
                    obj.RequisitionLine_K = r("(REQ) Requisition Line Number")
                    If Not IsDBNull(r("(REQ) PO Id")) Then
                        obj.POID = r("(REQ) PO Id")
                    End If
                    If Not IsDBNull(r("(REQ) Requisition Status")) Then
                        obj.RequisitionStatus = r("(REQ) Requisition Status")
                    End If
                    If Not IsDBNull(r("(REQ) Requisition Title")) Then
                        obj.RequisitionTitle = r("(REQ) Requisition Title")
                    End If
                    If Not IsDBNull(r("(REQ) Description")) Then
                        obj.Description = r("(REQ) Description")
                    End If
                    If Not IsDBNull(r("(PO) PO Status")) Then
                        obj.POStatusPU = r("(PO) PO Status")
                    End If
                    If IsDate(r("(REQ)Requisition Date (Date)")) Then
                        obj.RequisitionDate = r("(REQ)Requisition Date (Date)")
                    End If
                    If IsDate(r("(PO)Ordered Date (Date)")) Then
                        obj.OrderedDate = r("(PO)Ordered Date (Date)")
                    End If
                    If IsDate(r("(PO)Need By Date (Date)")) Then
                        obj.NeedByDate = r("(PO)Need By Date (Date)")
                    End If
                    If Not IsDBNull(r("(REQ) Receiver")) Then
                        obj.Receiver = r("(REQ) Receiver")
                    End If
                    If Not IsDBNull(r("(REQ)Purchasing Company (Purchase Organization Id)")) Then
                        obj.PurchasingCompany = r("(REQ)Purchasing Company (Purchase Organization Id)")
                    End If
                    If Not IsDBNull(r("(REQ)Cost Center (Cost Center)")) Then
                        obj.CostCenter = r("(REQ)Cost Center (Cost Center)")
                    End If
                    If Not IsDBNull(r("(REQ) Line Type")) Then
                        obj.LineType = r("(REQ) Line Type")
                    End If
                    If Not IsDBNull(r("(REQ) Assigned Buyer")) Then
                        obj.AssignedBuyer = r("(REQ) Assigned Buyer")
                    End If
                    If IsDate(r("(INV)Reconciled Date (Date)")) Then
                        obj.ReconciledDate = r("(INV)Reconciled Date (Date)")
                    End If
                    If _db.ExistObject(obj) Then
                        _db.Update(obj)
                    Else
                        obj.CreateUser = CurrentUser.UserID
                        obj.CreateDate = DateTime.Now
                        _db.Insert(obj)
                        succ += 1
                    End If
                Next
                ShowSuccess(succ)
                _db.Commit()
            Catch ex As Exception
                ShowWarning("Row " + count.ToString + ": " + ex.Message)
                _db.RollBack()
            End Try
        Else
            ShowWarning("Không có dữ liệu !")
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(GridView1)
    End Sub

    Private Sub GridView1_RowStyle(sender As Object, e As RowStyleEventArgs) Handles GridView1.RowStyle
        If GridView1.GetRowCellValue(e.RowHandle, "POStatusPU") IsNot DBNull.Value Then
            If (GridView1.GetRowCellValue(e.RowHandle, "POStatusPU") = "Confirmed" Or
                GridView1.GetRowCellValue(e.RowHandle, "POStatusPU") = "Ordered") And
                IsDBNull(GridView1.GetRowCellValue(e.RowHandle, "ConfirmedDate")) Then
                e.Appearance.BackColor = Color.FromArgb(255, 255, 192)
            End If
        End If
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If ShowQuestion("Bạn có chắc chắn muốn xóa dữ liệu ?") = DialogResult.Yes Then
            For Each r As Integer In GridView1.GetSelectedRows
                Dim obj As New PU_Ariba
                obj.RequisitionID_K = GridView1.GetRowCellValue(r, "RequisitionID")
                obj.RequisitionLine_K = GridView1.GetRowCellValue(r, "RequisitionLine")
                _db.Delete(obj)
            Next
            GridView1.DeleteSelectedRows()
        End If
    End Sub
End Class