﻿Imports CommonDB
Imports PublicUtility
Imports LibEntity
Imports System.Windows.Forms
Imports Microsoft.Reporting.WinForms

Public Class FrmGSRMaterial : Inherits DevExpress.XtraEditors.XtraForm
    Dim dbF As DBSql = New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)
    Dim _db As DBSql = New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim dbAS As New DBFunction(PublicConst.EnumServers.NDV_DB2_AS400)
    Dim param(1) As SqlClient.SqlParameter
    Public Shared searchJCode As String = ""
    Dim jumpNote As Integer = 1
    Dim _validating As Boolean = 0
    Dim checkConnectAS As Boolean = True
    Dim myVendor As String = ""

    Private Sub FrmGSRMaterial_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Enter Then
            SendKeys.Send("{Tab}")
        End If
        If e.KeyCode = Keys.N And e.Control And mnuNew.Enabled Then
            mnuNew.PerformClick()
        End If
        If e.KeyCode = Keys.D And e.Control And mnuDelete.Enabled Then
            mnuDelete.PerformClick()
        End If

        If e.KeyCode = Keys.F5 And mnuShowAll.Enabled Then
            mnuShowAll.PerformClick()
        End If

        If e.KeyCode = Keys.E And e.Control And mnuExport.Enabled Then
            mnuExport.PerformClick()
        End If
    End Sub

    Private Sub FrmGSRMaterial_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        mnuNew.PerformClick()
        For i As Integer = 0 To gridD.Columns.Count - 1
            gridD.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
        If checkUser() Then
            gridD.Columns("PO").Visible = True
        End If

        If dbAS.CheckConnection = False Then
            checkConnectAS = False
        End If

    End Sub

    'Function CheckJcode(ByVal check As String) As Boolean
    '    For i As Integer = 0 To gridD.Rows.Count - 2
    '        If gridD.CurrentRow.Index <> gridD.Rows(i).Index And UCase(gridD.CurrentRow.Cells("JCode").Value) = UCase(gridD.Rows(i).Cells("JCode").Value) Then
    '            Return True
    '        End If
    '    Next
    '    Dim JC As String = gridD.CurrentRow.Cells("JCode").Value
    '    Dim ID As String = txtID.Text
    '    Dim sql As String = String.Format("select * from {0} " +
    '    "where ID = '{1}' and ID <> '' and JCode = '{2}'", PublicTable.Table_GSR_GoodsOrderDetail, ID, JC)
    '    Dim dt As DataTable = _db.FillDataTable(sql)
    '    If dt.Rows.Count <> 0 Then
    '        Return True
    '    End If
    '    Return False
    'End Function

    Function checkUser() As Boolean
        Dim sqlcondi As String = String.Format("select IsAll from {0} where UserID = '{1}'",
                                               PublicTable.Table_GSR_UserRight,
                                               CurrentUser.UserID)
        Dim dtcondi As DataTable = _db.FillDataTable(sqlcondi)
        If dtcondi.Rows.Count <> 0 Then
            If dtcondi.Rows(0).Item("IsAll") = True Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
        Return False
    End Function

    Function checkVendor(ByVal check As String) As Boolean
        For Each r As DataGridViewRow In gridD.Rows
            If r.Cells("LastVendorCode").Value IsNot DBNull.Value Then
                If r.Cells("LastVendorCode").Value IsNot Nothing Then
                    If r.Cells("LastVendorCode").Value <> check Then
                        Return True
                    End If
                End If
            End If
        Next
        Return False
    End Function

    Function CreateID() As String
        Dim ID As String = ""
        Dim stt As String = ""
        Dim o As Object = Nothing
        Dim yyMMdd As String = Date.Now.ToString("yyMMdd")
        Dim sql As String = String.Format(" select Max(substring(ID, 7, 2)) FROM {0} " +
                                          " where left(ID, 6) = '{1}' ",
                                          PublicTable.Table_GSR_GoodsOrderHead,
                                          yyMMdd)
        o = _db.ExecuteScalar(sql)
        If o IsNot DBNull.Value And o IsNot Nothing Then
            o = Convert.ToInt32(o) + 1
            stt = AddLeft("0", o, 2)
            ID = yyMMdd + stt + "_" + CurrentUser.UserID
        Else
            ID = yyMMdd + "01" + "_" + CurrentUser.UserID
        End If
        Return ID
    End Function

    Function CreateOrderID() As Integer
        Dim ID As String = txtID.Text
        Dim sql As String = String.Format("select max(OrderID) as OrderID from {0} where ID = '{1}'",
                                          PublicTable.Table_GSR_GoodsOrderDetail, ID)
        Dim dt As DataTable = _db.FillDataTable(sql)
        If txtID.Text = "" Then
            Return 1
        ElseIf dt.Rows.Count = 0 Then
            Return 1
        ElseIf gridD.CurrentRow.Index = 0 Then
            Return 1
        Else
            Return dt.Rows(0).Item("OrderID") + 1
        End If
    End Function

    Function CreateVendor(ByVal vendorCode As String) As String
        'Khanh
        Dim groupA(0 To 7) As String
        groupA(0) = "K00005"
        groupA(1) = "K00011"
        groupA(2) = "K00001"
        groupA(3) = "K00014"
        groupA(4) = "K00008"
        groupA(5) = "K00022"
        groupA(6) = "K00026"
        groupA(7) = "K00013"

        'Thoa
        Dim groupB(0 To 8) As String
        groupB(0) = "K00021"
        groupB(1) = "K00003"
        groupB(2) = "K00024"
        groupB(3) = "K00012"
        groupB(4) = "K00009"
        groupB(5) = "K00027"
        groupB(6) = "K00019"
        groupB(7) = "K00006"
        groupB(8) = "K00017"

        For i As Integer = 0 To 7 Step 1
            If groupA(i) = vendorCode Then
                Return "0"
            End If
        Next

        For j As Integer = 0 To 8 Step 1
            If groupB(j) = vendorCode Then
                Return "1"
            End If
        Next

        Return "False"
    End Function

    Sub Save(ByVal ID As String)
        Try
            _db.BeginTransaction()
            Dim obj As New GSR_GoodsOrderHead()
            obj.ID_K = ID
            _db.GetObject(obj)
            obj.Reason = txtReason.Text

            If _db.ExistObject(obj) Then
                obj.UpdateDate = DateTime.Now
                obj.UpdateUser = CurrentUser.UserID
                _db.Update(obj)
            Else
                obj.ID_K = ID
                obj.EmployeeID = CurrentUser.UserID
                obj.CreateDate = DateTime.Now
                obj.CreateUser = CurrentUser.UserID
                obj.OrderDate = DateTime.Now.Date

                obj.Department = cboSection.Text

                _db.Insert(obj)
            End If

            Dim objD As New GSR_GoodsOrderDetail
            objD.ID_K = obj.ID_K
            objD.OrderID_K = IIf(gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value, 1,
                                 gridD.CurrentRow.Cells("OrderID").Value)
            _db.GetObject(objD)
            objD.JCode = UCase(gridD.CurrentRow.Cells("JCode").Value)
            objD.JName = IIf(gridD.CurrentRow.Cells("JName").Value Is DBNull.Value, "",
                             gridD.CurrentRow.Cells("JName").Value)
            objD.MinQty = IIf(gridD.CurrentRow.Cells("MinQty").Value Is DBNull.Value, 0,
                              gridD.CurrentRow.Cells("MinQty").Value)
            objD.Unit = IIf(gridD.CurrentRow.Cells("Unit").Value Is DBNull.Value, "",
                            gridD.CurrentRow.Cells("Unit").Value)
            objD.Note = IIf(gridD.CurrentRow.Cells("Note").Value Is DBNull.Value, "",
                            gridD.CurrentRow.Cells("Note").Value)
            objD.LastVendorCode = IIf(gridD.CurrentRow.Cells("LastVendorCode").Value Is DBNull.Value, "",
                                      gridD.CurrentRow.Cells("LastVendorCode").Value)
            objD.LastVendorName = IIf(gridD.CurrentRow.Cells("LastVendorName").Value Is DBNull.Value, "",
                                      gridD.CurrentRow.Cells("LastVendorName").Value)
            If _db.ExistObject(objD) Then
                _db.Update(objD)
            Else
                objD.ID_K = obj.ID_K
                objD.OrderID_K = IIf(gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value, 1,
                                     gridD.CurrentRow.Cells("OrderID").Value)
                _db.Insert(objD)
            End If
            _db.Commit()
        Catch ex As Exception
            _db.RollBack()
            MessageBox.Show(ex.Message, "insert/update")
        End Try
    End Sub

    Private Sub mnuNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNew.Click
        _validating = 0
        Reset()
        ResetgridD()
        _validating = 1
    End Sub

    Sub Reset()
        txtReason.Text = ""
        txtID.Text = ""
        myVendor = ""
        cboSection.Text = ""
        Dim sql1 As String = String.Format(" select h.ID, h.OrderDate, h.Reason, h.IsLock, " +
                                           " FullName = u.EmpName, h.Department " +
                                           " from {0} h " +
                                           " left join OT_Employee u " +
                                           " on h.EmployeeID = u.EmpID " +
                                           " where h.ID = '{1}'",
                                           PublicTable.Table_GSR_GoodsOrderHead, "")
        GridControl1.DataSource = _db.FillDataTable(sql1)
        GridControlSetFormat(GridView1)
    End Sub

    Sub ResetgridD()
        Dim sql As String = String.Format(" select d.OrderID, d.JCode, d.JName, d.MinQty," +
                                          " d.Unit , d.Quantity, d.DeliveryDate, d.Note, " +
                                          " d.LastVendorCode, d.LastVendorName, d.POID " +
                                          " from {0} h " +
                                          " inner join {1} d " +
                                          " on h.ID = d.ID " +
                                          " where h.ID = '{2}'",
                                          PublicTable.Table_GSR_GoodsOrderHead,
                                          PublicTable.Table_GSR_GoodsOrderDetail, "")
        Dim bd As New BindingSource
        bd.DataSource = _db.FillDataTable(sql)
        gridD.DataSource = bd
    End Sub

    Private Sub mnuShowAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowAll.Click
        _validating = 0
        Dim startDate
        If chkOrderDate.Checked = True Then
            startDate = dtpOrderDate.Value.ToString("yyMMdd")
        Else
            startDate = "010101"
        End If

        Dim endDate = dtpOrderDateEnd.Value.ToString("yyMMdd")
        param(0) = New SqlClient.SqlParameter("@startDate", startDate)
        param(1) = New SqlClient.SqlParameter("@endDate", endDate)

        Dim condiUser As String

        If checkUser() Then
            condiUser = ""
        Else
            condiUser = CurrentUser.UserID
        End If

        Dim sql As String

        sql = String.Format(" select h.ID, h.OrderDate, h.Reason, h.IsLock, FullName = u.EmpName, h.Department " +
                           " from {0} h " +
                           " left join OT_Employee u " +
                           " on h.EmployeeID = u.EmpID " +
                           " where left(h.ID, 6) between @startDate and @endDate " +
                           " and h.EmployeeID like '%{1}%'" +
                           " order by h.ID Desc",
                            PublicTable.Table_GSR_GoodsOrderHead,
                            condiUser)

        Dim bd As New BindingSource
        bd.DataSource = _db.FillDataTable(sql, param)

        GridControl1.DataSource = _db.FillDataTable(sql, param)
        GridControlSetFormat(GridView1)
    End Sub

    Dim jump As Boolean = True
    Private Sub GridView1_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView1.RowClick
        _validating = 0
        jump = False
        txtID.Text = GridView1.GetFocusedRowCellValue("ID") 'gridH.CurrentRow.Cells("ID").Value
        Dim st As String = txtID.Text 'gridH.CurrentRow.Cells("ID").Value
        Dim sql As String = String.Format(" select d.OrderID, d.JCode, d.JName, d.MinQty ," +
                                            " d.Quantity, d.Unit, d.DeliveryDate, d.Note, d.LastVendorCode, d.LastVendorName, d.POID " +
                                            " from {0} h " +
                                            " inner join {1} d " +
                                            " on h.ID = d.ID " +
                                            " where h.ID = '{2}'",
                                            PublicTable.Table_GSR_GoodsOrderHead,
                                            PublicTable.Table_GSR_GoodsOrderDetail, st)
        Dim bd As New BindingSource
        bd.DataSource = _db.FillDataTable(sql)
        gridD.DataSource = bd
        bnGridD.BindingSource = bd
        jump = True
        _validating = 1
    End Sub

    Private Sub mnuExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExport.Click
        Dim startDate
        If chkOrderDate.Checked = True Then
            startDate = dtpOrderDate.Value.ToString("yyMMdd")
        Else
            startDate = "010101"
        End If


        Dim endDate = dtpOrderDateEnd.Value.ToString("yyMMdd")
        param(0) = New SqlClient.SqlParameter("@startDate", startDate)
        param(1) = New SqlClient.SqlParameter("@endDate", endDate)

        Dim sql As String
        Dim dt As DataTable
        Dim condiUser As String
        If checkUser() Then
            condiUser = ""
        Else
            condiUser = CurrentUser.UserID
        End If

        sql = String.Format(" select h.ID as [GSR NO.], h.Department as Section, " +
                            " h.EmployeeID as [Emloyee ID], h.OrderDate as [Order Date], " +
                            " d.OrderID as [Order ID], d.JCode, d.JName, '' as [Means of transport], " +
                            " d.MinQty as MOQ, d.Unit, d.Quantity as [Quantity Ordered], " +
                            " d.DeliveryDate as [Date of Material Group's request], d.Note " +
                            " from {0} h " +
                            " inner join {1} d " +
                            " on h.ID = d.ID " +
                            " where left(h.ID, 6) between @startDate and @endDate " +
                            " and h.EmployeeID like '%{2}%'",
                            PublicTable.Table_GSR_GoodsOrderHead,
                            PublicTable.Table_GSR_GoodsOrderDetail,
                            condiUser)
        dt = _db.FillDataTable(sql, param)

        If dt.Rows.Count <> 0 Then
            ExportEXCEL(dt)
        Else
            ShowWarning("Data is Empty. Please choose Order date again.")
        End If
    End Sub

    Private Sub mnuPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPrint.Click
        If GridView1.GetFocusedRowCellValue("ID") Is Nothing Then
            ShowWarning("Please choose Order to print! Click Show all and choose")
            Exit Sub
        End If
        Dim ID As String = GridView1.GetFocusedRowCellValue("ID") 'gridH.CurrentRow.Cells("ID").Value
        PrintReport(ID)
    End Sub

    Public Sub PrintReport(ByVal ID As String)
        Dim sql As String = String.Format("select h.ID, h.Department, h.OrderDate, h.Reason, " +
                                        " d.JCode, d.JName, d.MinQty, '' as Air, d.Quantity, d.Unit," +
                                        " d.DeliveryDate, d.ReceivedQty," +
                                        " d.ReceivedUser, d.ReceivedDate, d.Note, " +
                                        " h.EmployeeID, FullName = u.EmpName " +
                                        " from {0} h " +
                                        " inner join {1} d " +
                                        " on h.ID = d.ID " +
                                        " left join OT_Employee u " +
                                        " on h.EmployeeID = u.EmpID " +
                                        " where h.ID = '{2}'",
                                         PublicTable.Table_GSR_GoodsOrderHead,
                                         PublicTable.Table_GSR_GoodsOrderDetail, ID)

        Dim dt As DataTable = _db.FillDataTable(sql)
        dt.TableName = "GSR"

        'Dim ds As New DataSet()
        'ds.Tables.Add(dt)
        'ds.DataSetName = "GSRMaterial"
        'ds.WriteXmlSchema("GSRMaterial.xsd")

        Dim frmReport As New FrmReport()
        frmReport.ReportViewer.LocalReport.ReportPath = "Reports\GSR\rptGSR.rdlc"
        frmReport.ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("GSRMaterial", dt))
        frmReport.ReportViewer.RefreshReport()
        frmReport.Show()
    End Sub

    Function checkLock() As Boolean
        Dim IDLock As String = txtID.Text
        Dim obj As New GSR_GoodsOrderHead()
        obj.ID_K = IDLock
        _db.GetObject(obj)
        If obj.IsLock Then
            Return True
        End If
        Return False
    End Function

    Private Sub mnuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        If gridD.Rows.Count = 0 Then
            ShowWarning("Please choose GSR to delete!")
            Exit Sub
        End If
        If checkLock() Then
            ShowWarning("GSR is locked. Can not delete it.")
            Exit Sub
        End If
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            _db.BeginTransaction()
            Try
                Dim ID As String = GridView1.GetFocusedRowCellValue("ID") 'gridH.CurrentRow.Cells("ID").Value
                Dim sqlH As String = String.Format("Delete from {0} where ID = '{1}'",
                                                   PublicTable.Table_GSR_GoodsOrderHead, ID)
                _db.ExecuteNonQuery(sqlH)
                Dim sqlD As String = String.Format("Delete from {0} where ID = '{1}'",
                                                   PublicTable.Table_GSR_GoodsOrderDetail, ID)
                _db.ExecuteNonQuery(sqlD)
                Dim sqlGH As String = String.Format("Delete from {0} where ID = '{1}'",
                                                    PublicTable.Table_GSR_GoodsDetail, ID)
                _db.ExecuteNonQuery(sqlGH)

                'Dim bs As New BindingSource()
                'bs = gridH.DataSource
                'Dim row As DataRowView = DirectCast(bs.Current, DataRowView)
                'row.Delete()
                GridView1.DeleteSelectedRows()
                ResetgridD()
                _db.Commit()

                MessageBox.Show("Successful.", "Delete")
            Catch ex As Exception
                _db.RollBack()
                ShowError(ex, mnuDelete.Text, Me.Name)
            End Try
        End If
    End Sub

    Private Sub gridD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridD.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub gridD_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles gridD.CellValidating
        If _validating = 0 Then Exit Sub
        If gridD.Rows(e.RowIndex).IsNewRow Then Return

        If e.ColumnIndex = gridD.Columns("Quantity").Index Then
            If gridD.CurrentRow.Cells("JName").Value Is DBNull.Value Then Exit Sub
            Dim newInteger As Decimal
            If Not Decimal.TryParse(e.FormattedValue.ToString(), newInteger) OrElse newInteger = 0 Then
                e.Cancel = True
                ShowWarning("Quantity is not correct !")
                Exit Sub
            End If
            Dim IDGSR As String = txtID.Text
            Dim orderID = gridD.CurrentRow.Cells("OrderID").Value
            Dim sql As String = String.Format("select MinQty " +
            "from {0} " +
            "where ID = '{1}' and OrderID = {2}", PublicTable.Table_GSR_GoodsOrderDetail, IDGSR, orderID)
            Dim dt As DataTable = _db.FillDataTable(sql)
            Dim QuantityParCarton As Decimal = dt.Rows(0).Item("MinQty")

            If newInteger < QuantityParCarton Then
                e.Cancel = True
                ShowWarning("Input Quantity > MinQty, please!")
                Exit Sub
            End If

        End If

    End Sub

    Private Sub gridD_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridD.CellValueChanged
        If gridD.CurrentRow Is Nothing Then Exit Sub
        If e.ColumnIndex = gridD.Columns("OrderID").Index _
        Or e.ColumnIndex = gridD.Columns("Unit").Index _
        Or e.ColumnIndex = gridD.Columns("JName").Index _
        Or e.ColumnIndex = gridD.Columns("MinQty").Index _
        Or e.ColumnIndex = gridD.Columns("LastVendorCode").Index _
        Or e.ColumnIndex = gridD.Columns("LastVendorName").Index _
        Then Exit Sub

        If cboSection.Text = "" Then
            ShowWarning("Bạn chưa chọn Section đặt hàng !")
            Return
        End If
        If e.ColumnIndex = gridD.Columns("Note").Index And jumpNote = 0 Then
            jumpNote = 1
            Exit Sub
        End If

        'If gridH.CurrentRow IsNot Nothing Then
        If GridView1.GetFocusedRowCellValue("ID") IsNot Nothing Then
            If checkLock() Then
                ShowWarning("GSR is locked. Can not Update!")
                Exit Sub
            End If
        End If

        Try
            Dim newID As String = CreateID()
            If e.ColumnIndex = gridD.Columns("JCode").Index Then

                Dim jCode As String = UCase(gridD.CurrentRow.Cells("JCode").Value)

                'If CheckJcode(jCode) Then
                '    MessageBox.Show("JCode existed. Please check again!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    Exit Sub
                'End If

                Dim obj As New FPICS_MaterialInfor
                obj.ItemCode_K = jCode
                If _db.ExistObject(obj) Then
                    _db.GetObject(obj)
                    If Not IsDBNull(obj.Status) Then
                        If obj.Status <> "ACTIVE" Then
                            ShowWarning("JCode này đã ngưng sử dụng !")
                            Return
                        End If
                    Else
                        ShowWarning("JCode này đã ngưng sử dụng !")
                        Return
                    End If
                Else
                    ShowWarning("JCode không tồn tại !")
                    Return
                End If

                Dim dt0 As New DataTable
                Dim sql0 = String.Format("select A.ItemCode ITEMCODE, ITEMNAME = LTrim(RTrim(A.ItemName)) + ' ' + LTrim(RTrim(A.Remarks)), " +
                    "A.UnitCode UNITCODE, MINQTY = A.BuyingMinimumQuantity, C.VendorCode VENDORCODE, C.VendorName VENDORNAME " +
                    "from t_ASMaterialItem A " +
                    "inner join t_ASMaterialItemVendor B " +
                    "on A.ItemCode = B.ItemCode  " +
                    "inner join t_ASMaterialVendor C " +
                    "on B.VendorCode = C.Vendorcode " +
                    "where A.ItemCode = '{0}'", jCode)
                dt0 = dbF.FillDataTable(sql0)


                If dt0.Rows.Count = 0 Then

                    sql0 = String.Format("Select A.MOITCD ITEMCODE, " +
                            "Concat(Concat(LTRIM(RTRIM(A.MOITNM)),' '), LTRIM(RTRIM(A.MORMKS))) ITEMNAME, " +
                            "A.MOUTCD as UNITCODE, A.MOBMNQ as MINQTY, " +
                            "C.MGVNCD as VENDORCODE, C.MGVNAB as VENDORNAME, " +
                            "F.MQMKCD MAKERCODE, F.MQMFCD MAKERFACTORYCODE " +
                            "from NDVDTA.MMOITMP A " +
                            "inner join " +
                            "(Select E.MQITCD, E.MQMKCD, E.MQMFCD, E.MQVNCD from " +
                            "(Select MQITCD, Max(MQLPDT) MQLPDT from NDVDTA.MMQITVP group by MQITCD) D " +
                            "inner join NDVDTA.MMQITVP E " +
                            "on D.MQITCD = E.MQITCD and D.MQLPDT = E.MQLPDT) F " +
                            "on A.MOITCD = F.MQITCD " +
                            "inner join NDVDTA.MMGVNDP C on  C.MGVNCD = F.MQVNCD " +
                            "where A.MODEDT = 0 and A.MOITCD = '{0}'", jCode)
                    dt0 = dbAS.FillDataTable(sql0)
                End If

                If dt0.Rows.Count <> 0 Then
                    Dim vendorCode As String = dt0.Rows(dt0.Rows.Count - 1).Item("VENDORCODE")
                    If checkVendor(vendorCode) Then
                        ShowWarning("Bạn chỉ đặt được các Jcode cùng nhà cung cấp !")
                        Exit Sub
                    End If

                    gridD.CurrentRow.Cells("OrderID").Value = IIf(gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value,
                                                                  CreateOrderID(), gridD.CurrentRow.Cells("OrderID").Value)
                    gridD.CurrentRow.Cells("JName").Value = dt0.Rows(dt0.Rows.Count - 1).Item("ITEMNAME")
                    gridD.CurrentRow.Cells("Unit").Value = dt0.Rows(dt0.Rows.Count - 1).Item("UNITCODE")
                    gridD.CurrentRow.Cells("MinQty").Value = dt0.Rows(dt0.Rows.Count - 1).Item("MINQTY")
                    gridD.CurrentRow.Cells("LastVendorCode").Value = dt0.Rows(dt0.Rows.Count - 1).Item("VENDORCODE")
                    gridD.CurrentRow.Cells("LastVendorName").Value = dt0.Rows(dt0.Rows.Count - 1).Item("VENDORNAME")

                Else
                    ShowWarning("JCode is not exist")
                    Exit Sub
                End If

                'lấy từ fpic

                'Dim sql1 As String = String.Format(" SELECT St_Qty = StockQuantity " +
                '                        " FROM [FPiCS-B03].[dbo].[t_ASMaterialStockAssessment] " +
                '                        " WHERE ItemCode = '{0}' AND ProcessMonth = (select max(ProcessMonth) from [t_ASMaterialStockAssessment]) ", jCode)

                'lấy từ ERP
                Dim sql1 As String = String.Format(" select St_Qty = isnull(Qty,0) + isnull(HeldQty,0) " +
                                                   " from PCM_Stock where JCode = '{0}'",
                                                   jCode)

                Dim Sst As Decimal = 0
                'Dim sql1 As String = String.Format("select sum(TNCUQT) as St_Qty from NDVDTA.MTNINCP where TNITCD = '{0}' and TNCUQT>0 ", jCode)

                If checkConnectAS Then
                    'Dim dt1 As DataTable = dbAS.FillDataTable(sql1)
                    Dim dt1 As DataTable = _db.FillDataTable(sql1)
                    If dt1.Rows.Count = 0 Then
                        Sst = 0.0
                    ElseIf dt1.Rows(0).Item("St_Qty").ToString = "" Then
                        Sst = 0.0
                    Else
                        Sst = dt1.Rows(0).Item("St_Qty")
                    End If
                End If

                Dim NotReceived As Decimal = 0
                'Dim sql2 As String = String.Format("select sum(A.TEORQT - (B.TFRGQT + B.TFTPQT)) as BALANCEQTY " +
                '"from NDVDTA.MTEPOLP A left join NDVDTA.MTFPODP B ON A.TEPONB=B.TFPONB " +
                '"left join NDVDTA.MTDPOHP C ON A.TEPONB = C.TDPONB " +
                '"where C.TDCNDT = 0 and A.TEITCD = '{0}'", jCode)

                'If checkConnectAS Then
                '    Dim dt2 As DataTable = dbAS.FillDataTable(sql2)

                '    If dt2.Rows.Count = 0 Then
                '        NotReceived = 0.0
                '    ElseIf dt2.Rows(0).Item("BALANCEQTY").ToString = "" Then
                '        NotReceived = 0.0
                '    Else
                '        NotReceived = dt2.Rows(0).Item("BALANCEQTY")
                '    End If
                'End If

                Dim Picked As Decimal = 0
                Dim sqlPicked As String = String.Format("select PickedQty from {0} where JCode = '{1}'",
                                                        PublicTable.Table_PCM_Stock, jCode)
                Dim dtPicked As DataTable = _db.FillDataTable(sqlPicked)

                If dtPicked.Rows.Count = 0 Then
                    Picked = 0
                Else
                    Picked = dtPicked.Rows(0).Item("PickedQty")
                End If

                'Thêm vào tồn chuyền
                Dim stockWorkshop As Decimal = IIf(gridD.CurrentRow.Cells("StockWS").Value Is DBNull.Value, 0,
                                                   gridD.CurrentRow.Cells("StockWS").Value)

                Dim TSDate As String = DateTime.Now.ToString("yyyyMMdd")

                'Mới thêm vào
                Dim stock As Decimal = Sst

                'Dim slqOut As String = String.Format("select TPITCD, TPTRTY from NDVDTA.MTPTRNP where TPITCD = '{0}' and TPTRTY Like '220' and TPTRDT = '{1}'", jCode, TSDate)
                'If checkConnectAS Then
                '    Dim dtOut As DataTable = dbAS.FillDataTable(slqOut)
                '    If dtOut.Rows.Count = 0 Then
                '        stock = Sst - Picked
                '    Else
                '        stock = Sst
                '    End If
                'End If

                jumpNote = 0
                gridD.CurrentRow.Cells("Note").Value = Trim(dt0.Rows(dt0.Rows.Count - 1).Item("VENDORNAME")) & ", " & "Stock + WS + NotRec = " _
                & Math.Round(stock, 0).ToString() & " + " & Math.Round(stockWorkshop, 0).ToString() & " + " & Math.Round(NotReceived, 0).ToString() & " = " &
                Math.Round(stock + stockWorkshop + NotReceived, 0).ToString + ". Need ="
                If txtID.Text = "" Then
                    Save(newID)
                    txtID.Text = newID
                Else
                    Save(txtID.Text)
                End If
                Dim bs As New BindingSource()
                bs = gridD.DataSource
                bnGridD.BindingSource = bs
            End If
        Catch ex As Exception
            'MessageBox.Show("JCode may not exist. Please check again!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            MessageBox.Show(ex.Message)
        End Try

        If e.ColumnIndex = gridD.Columns("Quantity").Index Then

            If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Or gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value Then
                MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                Dim objD As New GSR_GoodsOrderDetail()
                objD.ID_K = txtID.Text
                objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

                _db.GetObject(objD)
                objD.Quantity = IIf(gridD.CurrentRow.Cells("Quantity").Value Is DBNull.Value, 0, gridD.CurrentRow.Cells("Quantity").Value)
                _db.Update(objD)
            End If
        End If

        If e.ColumnIndex = gridD.Columns("Note").Index Then
            If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Then
                MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                Dim objD As New GSR_GoodsOrderDetail()
                objD.ID_K = txtID.Text
                objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

                _db.GetObject(objD)
                objD.Note = IIf(gridD.CurrentRow.Cells("Note").Value Is DBNull.Value, "", gridD.CurrentRow.Cells("Note").Value)
                _db.Update(objD)
            End If
        End If

        If e.ColumnIndex = gridD.Columns("DeliveryDate").Index Then
            If gridD.CurrentRow.Cells("JCode").Value Is DBNull.Value Then
                MessageBox.Show("Please input JCode!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                Dim objD As New GSR_GoodsOrderDetail()
                objD.ID_K = txtID.Text
                objD.OrderID_K = gridD.CurrentRow.Cells("OrderID").Value

                _db.GetObject(objD)
                objD.DeliveryDate = IIf(gridD.CurrentRow.Cells("DeliveryDate").Value Is DBNull.Value, DateTime.Now, gridD.CurrentRow.Cells("DeliveryDate").Value)

                Dim objHoliday As New FPICS_HolidayDate
                objHoliday.HolidayDate_K = objD.DeliveryDate.Date
                If Not _db.ExistObject(objHoliday) Then
                    _db.Update(objD)
                Else
                    If ShowQuestion("DeliveryDate không nên chọn ngày nghỉ congo ty! Bạn thật sự muốn chọn ngày này ?") = DialogResult.Yes Then
                        _db.Update(objD)
                    End If
                End If

            End If
        End If

    End Sub

#Region "Form event"

    Private Sub txtReason_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReason.Enter
        SetColorEnter(txtReason)
    End Sub
    Private Sub txtReason_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReason.Leave
        SetColorLeave(txtReason)
    End Sub
#End Region

    Private Sub dtpOrderDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpOrderDate.ValueChanged
        If dtpOrderDate.Value > dtpOrderDateEnd.Value Then
            dtpOrderDateEnd.Value = dtpOrderDate.Value
        End If
    End Sub

    Private Sub dtpOrderDateEnd_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpOrderDateEnd.ValueChanged
        If dtpOrderDate.Value > dtpOrderDateEnd.Value Then
            dtpOrderDate.Value = dtpOrderDateEnd.Value
        End If
    End Sub

    Private Sub gridD_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridD.UserDeletingRow
        If e.Row.Index < 0 Then Exit Sub

        If checkLock() Then
            MessageBox.Show("GSR is locked. Can not delete it.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            Dim ID As String = txtID.Text
            Dim orderID As Integer = IIf(gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value, 0, gridD.CurrentRow.Cells("OrderID").Value)
            _db.BeginTransaction()
            Try
                Dim sqlD As String = String.Format("Delete from {0} where ID = '{1}' and OrderID = '{2}'", PublicTable.Table_GSR_GoodsOrderDetail, ID, orderID)
                _db.ExecuteNonQuery(sqlD)
                Dim sqlGH As String = String.Format("Delete from {0} where ID = '{1}' and OrderID = '{2}'", PublicTable.Table_GSR_GoodsDetail, ID, orderID)
                _db.ExecuteNonQuery(sqlGH)
                _db.Commit()
            Catch ex As Exception
                _db.RollBack()
                ShowError(ex, "gridD_UserDeletingRow", Me.Name)
            End Try
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub gridD_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gridD.MouseDoubleClick
        Try
            Dim frmSearchJCode As New FrmSearchJCode()
            frmSearchJCode.ShowDialog()
            If searchJCode = "" Then
                Exit Sub
            Else
                gridD.CurrentRow.Cells("JCode").Value = searchJCode
                searchJCode = ""
            End If

            Dim col As Integer = gridD.CurrentCell.ColumnIndex
            Dim row As Integer = gridD.CurrentCell.RowIndex

            Dim sql As String = String.Format("select d.OrderID, d.JCode, d.JName, d.MinQty, d.Unit," +
                                              " '' as Air, d.Quantity, d.DeliveryDate, d.Note, d.LastVendorCode, " +
                                              " d.LastVendorName, d.POID " +
                                            " from {0} h " +
                                            " inner join {1} d " +
                                            " on h.ID = d.ID " +
                                            " where h.ID = '{2}'",
                                              PublicTable.Table_GSR_GoodsOrderHead,
                                              PublicTable.Table_GSR_GoodsOrderDetail,
                                              txtID.Text)
            Dim bd As New BindingSource
            bd.DataSource = _db.FillDataTable(sql)
            gridD.DataSource = bd
            gridD.CurrentCell = gridD.Item(col, row)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "gridD_MouseDoubleClick")
        End Try
    End Sub

    Private Sub mnuExportJC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportJC.Click
        ExportJC()
    End Sub

    Sub ExportJC()
        Dim sql As String = String.Format("Select A.MOITCD ITEMCODE, " +
                "Concat(Concat(LTRIM(RTRIM(A.MOITNM)),' '), LTRIM(RTRIM(A.MORMKS))) ITEMNAME, " +
                "A.MOUTCD as UNITCODE, A.MOBMNQ as MINQTY, " +
                "C.MGVNCD as VENDORCODE, C.MGVNAB as VENDORNAME, " +
                "F.MQMKCD MAKERCODE, G.MIMKNM MAKERNAME, F.MQMFCD MAKERFACTORYCODE, H.MJMFAB MAKERFACTORYNAME " +
                "from NDVDTA.MMOITMP A " +
                "inner join " +
                "(Select E.MQITCD, E.MQMKCD, E.MQMFCD, E.MQVNCD from " +
                "(Select MQITCD, Max(MQLPDT) MQLPDT from NDVDTA.MMQITVP group by MQITCD) D " +
                "inner join NDVDTA.MMQITVP E " +
                "on D.MQITCD = E.MQITCD and D.MQLPDT = E.MQLPDT) F " +
                "on A.MOITCD = F.MQITCD " +
                "inner join NDVDTA.MMGVNDP C on C.MGVNCD = F.MQVNCD " +
                "left join NDVDTA.MMIMKRP G on F.MQMKCD = G.MIMKCD " +
                 "left join NDVDTA.MMJMFCP H on  F.MQMFCD = H.MJMFCD " +
                "where A.MODEDT = 0 order by A.MOITCD ")
        Dim dt As DataTable = dbAS.FillDataTable(sql)
        ExportEXCEL(dt)
    End Sub

    Private Sub mnuDeleteUnit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteUnit.Click
        If gridD.RowCount = 1 Then Exit Sub

        If checkLock() Then
            ShowWarning("GSR is locked. Can not delete it !")
            Exit Sub
        End If

        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            Dim ID As String = txtID.Text
            Dim orderID As Integer = IIf(gridD.CurrentRow.Cells("OrderID").Value Is DBNull.Value, 0,
                                         gridD.CurrentRow.Cells("OrderID").Value)
            Dim sqlD As String = String.Format("Delete from {0} where ID = '{1}' and OrderID = '{2}'",
                                               PublicTable.Table_GSR_GoodsOrderDetail,
                                               ID, orderID)
            _db.ExecuteNonQuery(sqlD)
            Dim bs As New BindingSource()
            bs = gridD.DataSource
            Dim row As DataRowView = DirectCast(bs.Current, DataRowView)
            row.Delete()
        End If
    End Sub

    Private Sub gridD_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridD.CellEnter
        If jump = False Then Exit Sub
        If gridD.CurrentRow.Cells(e.ColumnIndex).[ReadOnly] Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub gridD_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridD.DataError

        MessageBox.Show("Error happened " _
           & e.Context.ToString())

        If (e.Context = DataGridViewDataErrorContexts.Commit) _
            Then
            MessageBox.Show("Commit error")
        End If
        If (e.Context = DataGridViewDataErrorContexts _
            .CurrentCellChange) Then
            MessageBox.Show("Cell change")
        End If
        If (e.Context = DataGridViewDataErrorContexts.Parsing) _
            Then
            MessageBox.Show("parsing error")
        End If
        If (e.Context =
            DataGridViewDataErrorContexts.LeaveControl) Then
            MessageBox.Show("leave control error")
        End If

        If (TypeOf (e.Exception) Is ConstraintException) Then
            Dim view As DataGridView = CType(sender, DataGridView)
            view.Rows(e.RowIndex).ErrorText = "an error"
            view.Rows(e.RowIndex).Cells(e.ColumnIndex) _
                .ErrorText = "an error"

            e.ThrowException = False
        End If

    End Sub


    Private Sub gridD_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridD.CellContentClick
        gridD.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        GridControlReadOnly(GridView1, True)
        GridView1.Columns("Department").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format("UPDATE GSR_GoodsOrderHead
                                               SET {0} = @Value
                                               WHERE ID = '{1}'",
                                               e.Column.FieldName,
                                               GridView1.GetFocusedRowCellValue("ID")), para)
        End If
    End Sub
End Class