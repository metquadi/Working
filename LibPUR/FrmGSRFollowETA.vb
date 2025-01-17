﻿Imports CommonDB
Imports PublicUtility
Imports LibEntity
Imports System.Windows.Forms
Imports vb = Microsoft.VisualBasic
Imports System.Globalization
Imports Microsoft.Reporting.WinForms
Imports System.IO
Imports System.Drawing

Public Class FrmGSRFollowETA : Inherits DevExpress.XtraEditors.XtraForm
    Dim db As New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)
    Dim nvd As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim dbAS As New DBFunction(PublicConst.EnumServers.NDV_DB2_AS400)
    Dim param(1) As SqlClient.SqlParameter
    Public printResult As DataTable
    Dim status As Boolean = 0
    Dim checkConnectAS As Boolean = True
    Dim AllowEdit As Boolean = True


    Function checkUser() As Boolean
        Dim sqlcondi As String = String.Format("select IsAll from {0} where UserID = '{1}'",
                                               PublicTable.Table_GSR_UserRight, CurrentUser.UserID)
        Dim dtcondi As DataTable = nvd.FillDataTable(sqlcondi)
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

    Private Sub FrmDelivery_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        dtpOrderDate.Value = GetStartDayOfMonth(Date.Now)

        Me.Cursor = Cursors.WaitCursor
        Dim Daycom As Integer = DateTime.Now.DayOfWeek

        Dim frmETA As New FrmETAList
        frmETA.ShowDialog()


        bttConfirm.Enabled = mnuSave.Enabled
        bttUpdatePO.Enabled = mnuSave.Enabled

        mnuShowAll.PerformClick()
        Me.Cursor = Cursors.Arrow

    End Sub

    Function checkLock() As Boolean
        Dim IDLock As String = GridView1.GetFocusedRowCellValue("ID")
        Dim obj As New GSR_GoodsOrderHead()
        obj.ID_K = IDLock
        nvd.GetObject(obj)
        If obj.IsLock = "True" Then
            Return True
        End If
        Return False
    End Function

    Function RemoveSpace(ByVal s As String) As String
        While s.IndexOf(" ") >= 0
            'tim trong chuoi vi tri co 1 khoang trong  
            s = s.Replace(" ", "")
        End While
        'sau do thay the bang ""          
        Return s
    End Function

    Private Sub mnuShowAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowAll.Click
        Me.Cursor = Cursors.WaitCursor
        status = 0
        Dim startDate As Date = dtpOrderDate.Value.Date
        Dim endDate As Date = dtpOrderDateEnd.Value.Date

        param(0) = New SqlClient.SqlParameter("@startDate", startDate)
        param(1) = New SqlClient.SqlParameter("@endDate", endDate)


        Dim sqlPO As String = String.Format("select distinct POID " +
                                                "from {0} " +
                                                "where left(ID, 6) between @startDate and @endDate",
                                                PublicTable.Table_GSR_GoodsOrderDetail)
        Dim dtPO As DataTable = nvd.FillDataTable(sqlPO, param)

        Dim sql As String = String.Format("select distinct InvoiceID " +
                                                "from {0} " +
                                                "where left(ID, 6) between @startDate and @endDate",
                                              PublicTable.Table_GSR_GoodsDetail)
        Dim dt As DataTable = nvd.FillDataTable(sql, param)

        cboInvoice.DataSource = dt
        cboInvoice.DisplayMember = "InvoiceID"
        cboInvoice.ValueMember = "InvoiceID"
        cboInvoice.SelectedIndex = -1

        cboPOFile.DataSource = dtPO
        cboPOFile.DisplayMember = "POID"
        cboPOFile.ValueMember = "POID"
        cboPOFile.SelectedIndex = -1

        Dim para(3) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", startDate)
        para(1) = New SqlClient.SqlParameter("@EndDate", endDate)
        If rdoOrderDate.Checked Then
            para(2) = New SqlClient.SqlParameter("@Type", "OrderDate")
        ElseIf rdoDeliveryDate.Checked Then
            para(2) = New SqlClient.SqlParameter("@Type", "DeliveryDate")
        ElseIf rdoReceiveddate.Checked Then
            para(2) = New SqlClient.SqlParameter("@Type", "ReceivedDate")
        End If
        If ckoAlarm.Checked Then
            para(3) = New SqlClient.SqlParameter("@Alarm", "Alarm")
        Else
            para(3) = New SqlClient.SqlParameter("@Alarm", DBNull.Value)
        End If
        'UPdate Shipping term
        Dim paraT(1) As SqlClient.SqlParameter
        paraT(0) = New SqlClient.SqlParameter("@StartDate", startDate)
        paraT(1) = New SqlClient.SqlParameter("@EndDate", endDate)
        nvd.ExecuteStoreProcedure("sp_GSR_UpdateShippingTerm", paraT)

        GridView1.Columns.Clear()
        GridControl1.DataSource = nvd.ExecuteStoreProcedureTB("GSR_LoadGSRFollow", para)
        If CurrentUser.UserID <> "15180" And
            CurrentUser.UserID <> "07283" Then
            GridView1.Columns("PdCodeList").Visible = False
            GridView1.Columns("LotCount").Visible = False
        End If
        GridControlSetFormat(GridView1, 6)
        GridControlSetFormatNumber(GridView1, "UnitPrice", 4)
        GridControlSetColorReadonly(GridView1)
        GridView1.Columns("ID").BestFit()
        GridView1.Columns("OrderID").Width = 50
        GridView1.Columns("IsLock").Width = 50
        GridView1.Columns("EmployeeID").Width = 50
        GridView1.Columns("Department").Width = 50
        GridView1.Columns("JCode").Width = 50
        GridView1.Columns("Unit").Width = 50

        status = 1
        Me.Cursor = Cursors.Arrow
        'Catch ex As Exception
        '    Me.Cursor = Cursors.Arrow
        '    ShowError(ex, mnuShowAll.Text, Me.Name)
        'End Try
    End Sub



    Function checkPO(ByVal POID As String, ByVal ID As String, ByVal OrderID As Integer) As Boolean
        Dim startPO = New DateTime(DateTime.Now.Year, 1, 1)
        Dim endPO = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
        param(0) = New SqlClient.SqlParameter("@startPO", startPO.ToString("yyMMdd"))
        param(1) = New SqlClient.SqlParameter("@endPO", endPO.ToString("yyMMdd"))
        Dim sql As String = String.Format("select ID " +
                        "from {0} " +
                        "where left(ID, 6) between @startPO and @endPO and POID = '{1}' and ID <> '{2}'",
                        PublicTable.Table_GSR_GoodsOrderDetail, POID, ID)
        Dim dt As DataTable = nvd.FillDataTable(sql, param)
        Dim sql2 As String = String.Format("select ID " +
                        "from {0} " +
                        "where POID = '{1}' and ID = '{2}' and OrderID <> {3}",
                        PublicTable.Table_GSR_GoodsOrderDetail, POID, ID, OrderID)
        Dim dt2 As DataTable = nvd.FillDataTable(sql2)

        If (dt.Rows.Count <> 0 Or dt2.Rows.Count <> 0) And POID <> "" Then
            Return True
        End If
        Return False
    End Function


    Function RecursiveDate(ByVal ExpectedDate As DateTime) As DateTime
        If ExpectedDate.DayOfWeek = 0 Or ExpectedDate.DayOfWeek = 6 Then
            Return RecursiveDate(ExpectedDate.AddDays(1))
        Else
            Return ExpectedDate
        End If
    End Function

    Function convertDateAS(ByVal stnum As String) As DateTime
        Dim provider As CultureInfo = CultureInfo.InvariantCulture
        Dim strDate As String = vb.Mid(stnum, 5, 2) + "/" + vb.Right(stnum, 2) + "/" + vb.Left(stnum, 4)
        Dim receivedDate As DateTime = DateTime.ParseExact(strDate, "MM/dd/yyyy", provider)
        Return receivedDate
    End Function

    Function checkddMMyyyy(ByVal stnum As String) As Boolean
        Dim provider As CultureInfo = CultureInfo.InvariantCulture
        Try
            Dim receivedDate As DateTime = DateTime.ParseExact(stnum, "dd/MM/yyyy", provider)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Function convertddMMyyyy(ByVal stnum As String) As DateTime
        Dim receivedDate As DateTime
        Try
            Dim provider As CultureInfo = CultureInfo.InvariantCulture
            receivedDate = DateTime.ParseExact(stnum, "dd/MM/yyyy", provider)
        Catch ex As Exception
            MessageBox.Show("Date must be format dd/MM/yyyy", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            receivedDate = DateTime.MinValue
        End Try
        Return receivedDate
    End Function


    Private Sub dtpOrderDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpOrderDate.ValueChanged
        If dtpOrderDateEnd.Value < dtpOrderDate.Value Then
            dtpOrderDateEnd.Value = dtpOrderDate.Value
        End If
    End Sub

    Private Sub dtpOrderDateEnd_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpOrderDateEnd.ValueChanged
        If dtpOrderDateEnd.Value < dtpOrderDate.Value Then
            dtpOrderDate.Value = dtpOrderDateEnd.Value
        End If
    End Sub

    Private Sub mnuExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExport.Click
        GridControlExportExcel(GridView1)
    End Sub

    Private Sub mnuPrintGSR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPrintGSR.Click
        If GridView1.SelectedRowsCount = 0 Then
            ShowWarning("Please choose Order to print! Click Show all and choose")
            Return
        End If
        Dim ID As String = GridView1.GetFocusedRowCellValue("ID")
        Dim frm As New FrmGSRMaterial
        frm.PrintReport(ID)
    End Sub

    Private Sub mnuLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLock.Click
        If GridView1.SelectedRowsCount = 0 Then
            ShowWarning("Please choose GSR to Lock!")
            Exit Sub
        End If
        Dim obj As New GSR_GoodsOrderHead
        obj.ID_K = GridView1.GetFocusedRowCellValue("ID")
        nvd.GetObject(obj)
        If obj.IsLock Then
            ShowWarning("GSR is locked!")
            Exit Sub
        End If
        obj.IsLock = True
        obj.LockDate = DateTime.Now
        obj.LockUser = CurrentUser.UserID
        If ShowQuestion("Do you want to lock this GSR?") = DialogResult.No Then
            Exit Sub
        End If
        nvd.Update(obj)
        ShowSuccess()


    End Sub

    Private Sub mnuUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUnlock.Click
        If GridView1.SelectedRowsCount = 0 Then
            ShowWarning("Please choose GSR to Unlock!")
            Exit Sub
        End If
        Dim obj As New GSR_GoodsOrderHead
        obj.ID_K = GridView1.GetFocusedRowCellValue("ID")
        nvd.GetObject(obj)
        If obj.IsLock = False Then
            ShowWarning("GSR is unlocked!")
            Exit Sub
        End If
        obj.IsLock = False
        If ShowQuestion("Do you want to unlock this GSR?") = DialogResult.No Then
            Exit Sub
        End If
        nvd.Update(obj)
        mnuShowAll.PerformClick()
        ShowSuccess()
    End Sub


    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click

        Me.Cursor = Cursors.WaitCursor
        For row As Integer = 0 To GridView1.RowCount - 1
            Application.DoEvents()

            Dim quantity As Integer = IIf(GridView1.GetRowCellValue(row, "Quantity") Is DBNull.Value, 0,
                                              GridView1.GetRowCellValue(row, "Quantity"))
            Dim recQty As Integer = IIf(GridView1.GetRowCellValue(row, "ReceivedQty") Is DBNull.Value, 0,
                                            GridView1.GetRowCellValue(row, "ReceivedQty"))
            If GridView1.GetRowCellValue(row, "POID") Is DBNull.Value Then
                'Or quantity = recQty
                Continue For
            End If

            Dim objD As New GSR_GoodsOrderDetail
            objD.ID_K = GridView1.GetRowCellValue(row, "ID")
            objD.OrderID_K = GridView1.GetRowCellValue(row, "OrderID")
            nvd.GetObject(objD)

            Dim sqlGH As String = String.Format("Delete from {0} 
                                                where ID = '{1}' 
                                                and OrderID = '{2}' 
                                                and JCode = '{3}'",
                                                        PublicTable.Table_GSR_GoodsDetail,
                                                        objD.ID_K, objD.OrderID_K, objD.JCode)
            nvd.ExecuteNonQuery(sqlGH)

            Dim POID As String = GridView1.GetRowCellValue(row, "POID")
            Dim JCode As String = GridView1.GetRowCellValue(row, "JCode")

            'Cập nhật UnitPrice
            Dim sqlUnitPrice As String = String.Format("Select TFBUNP as UNITPRICE " +
                                                                "from NDVDTA.MTFPODP " +
                                                                "Where TFPONB = '{0}'", POID)
            Dim dtUnitPrice As DataTable = dbAS.FillDataTable(sqlUnitPrice)
            If dtUnitPrice.Rows.Count <> 0 Then
                If dtUnitPrice.Rows(0)("UNITPRICE") <> GridView1.GetRowCellValue(row, "UnitPrice") Then
                    objD.UnitPrice = dtUnitPrice.Rows(0).Item("UNITPRICE")
                    nvd.Update(objD)
                    GridView1.SetRowCellValue(row, "UnitPrice", objD.UnitPrice)
                End If
            End If

            Dim sqlAS As String = String.Format("
                SELECT A.*, B.THVEIV AS INVOICEID, B.THAPNB AS AP, C.TFBUNP AS UNITPRICE, 
                    B.THCUCD AS CurrencyCode, C.TFBUNP * B.THEXRT AS UNITPRICEUSD 
                FROM (
                    SELECT TPPONB, TPACQT AS RECEIVEDQTY, TPPUDT AS INVOICEDATE, TPPUNB, TPTRSE, TPITCD 
                    FROM NDVDTA.MTPTRNP 
                    WHERE TPPONB = '{0}' 
                    AND TPITCD = '{1}' 
                    AND TPTRTY Like '110' 
                    AND TPPUDT > '{2}'
                ) AS A 
                LEFT JOIN NDVDTA.MTHVIVP B 
                on A.TPPUNB = B.THPUNB 
                AND A.TPPUNB = B.THPUNB 
                AND A.TPTRSE = B.THAPSE 
                LEFT JOIN NDVDTA.MTFPODP AS C 
                ON A.TPPUNB = C.TFPONB",
                POID, JCode, Date.Now.AddMonths(-12).ToString("yyyyMMdd"))
            Dim dtAS As DataTable = dbAS.FillDataTable(sqlAS)

            If dtAS.Rows.Count = 0 Then
                Continue For
            End If

            Dim index As Integer = 1
            Dim countRec As Integer = 0
            Dim sum As Decimal = 0
            Dim Invoice As String = ""
            If dtAS.Rows.Count <> 0 Then
                objD.ReceivedDate = convertDateAS(dtAS.Rows(0).Item("INVOICEDATE"))
                objD.ReceivedUser = CurrentUser.UserID

                For i As Integer = 0 To dtAS.Rows.Count - 1
                    Dim obj As New GSR_GoodsDetail()
                    obj.ID_K = objD.ID_K
                    obj.OrderNo_K = index
                    obj.OrderID_K = objD.OrderID_K
                    obj.JCode = objD.JCode
                    obj.POID = POID
                    obj.ReceivedDate = convertDateAS(dtAS.Rows(i).Item("INVOICEDATE"))
                    obj.ReceivedQuantity = dtAS.Rows(i).Item("RECEIVEDQTY")
                    obj.InvoiceID = IIf(dtAS.Rows(i).Item("INVOICEID") Is DBNull.Value, "",
                                        dtAS.Rows(i).Item("INVOICEID"))
                    If obj.InvoiceID <> "" Then
                        If Invoice = "" Then
                            Invoice = obj.InvoiceID
                        End If
                    End If
                    obj.AP = IIf(dtAS.Rows(i).Item("AP") Is DBNull.Value, "",
                                 dtAS.Rows(i).Item("AP"))
                    countRec = countRec + 1
                    sum = sum + dtAS.Rows(i).Item("RECEIVEDQTY")
                    If nvd.ExistObject(obj) Then
                        nvd.Update(obj)
                    Else
                        nvd.Insert(obj)
                    End If
                    index += 1
                    'If dtAS.Rows(i).Item("CurrencyCode") IsNot DBNull.Value Then
                    '    objD.CurrencyCode = dtAS.Rows(i).Item("CurrencyCode")
                    'End If
                    'If dtAS.Rows(i).Item("UnitPrice") IsNot DBNull.Value Then
                    '    objD.UnitPrice = dtAS.Rows(i).Item("UnitPrice")
                    'End If
                    'If dtAS.Rows(i).Item("UnitPriceUSD") IsNot DBNull.Value Then
                    '    objD.UnitPriceUSD = dtAS.Rows(i).Item("UnitPriceUSD")
                    'End If
                Next
                objD.InvoiceID = Invoice
                objD.ReceivedQty = sum
                objD.CountReceived = countRec

            End If

            If nvd.ExistObject(objD) Then
                nvd.Update(objD)
            Else
                objD.ID_K = GridView1.GetRowCellValue(row, "ID")
                objD.OrderID_K = GridView1.GetRowCellValue(row, "OrderID")
                nvd.Insert(objD)
            End If

        Next
        Me.Cursor = Cursors.Arrow
        ShowSuccess()

    End Sub

    Private Sub mnuEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        GridControlReadOnly(GridView1, True)

        GridView1.Columns("POID").OptionsColumn.ReadOnly = False
        GridView1.Columns("NotePU").OptionsColumn.ReadOnly = False
        GridView1.Columns("Leadtime").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        If GridView1.RowCount = 0 Then Exit Sub
        If checkLock() Then
            ShowWarning("GSR đã khóa không thể xóa !")
            Exit Sub
        End If

        If ShowQuestion("Bạn muốn xóa những dòng đang chọn ?") = Windows.Forms.DialogResult.Yes Then

            Try
                nvd.BeginTransaction()
                For Each i As Integer In GridView1.GetSelectedRows
                    Dim ID As String = GridView1.GetRowCellValue(i, "ID")
                    Dim orderID As Integer = GridView1.GetRowCellValue(i, "OrderID")
                    Dim sqlD As String = String.Format("Delete from {0} where ID = '{1}' and OrderID = '{2}'",
                                                       PublicTable.Table_GSR_GoodsOrderDetail, ID, orderID)
                    nvd.ExecuteNonQuery(sqlD)
                    Dim sqlGH As String = String.Format("Delete from {0} where ID = '{1}' and OrderID = '{2}'",
                                                        PublicTable.Table_GSR_GoodsDetail, ID, orderID)
                    nvd.ExecuteNonQuery(sqlGH)
                Next
                nvd.Commit()
                mnuShowAll.PerformClick()
            Catch ex As Exception
                nvd.RollBack()
                ShowError(ex, mnuDelete.Text, Me.Name)
            End Try
        End If
    End Sub

    Private Sub mnuETA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuETA.Click
        Dim frmETA As New FrmETAList()
        frmETA.statusShow = 1
        frmETA.Show()
    End Sub

    Private Sub lblAttachInvoice_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAttachInvoice.LinkClicked
        If IsDBNull(cboInvoice.SelectedItem) Then Exit Sub
        Dim opd = New OpenFileDialog()
        opd.Filter = "File (*.*)|*.*|All file (*.*)|*.*"
        opd.Title = "Select a file to attach"
        If (opd.ShowDialog() = DialogResult.OK) Then
            Try
                Dim objInvoiceFile As New GSR_InvoiceFile()
                Dim sSourceFile As String = String.Empty
                Dim sExtendFile As String = String.Empty
                objInvoiceFile.InvoiceID_K = Trim(cboInvoice.SelectedItem)
                sSourceFile = opd.FileName

                If (File.Exists(sSourceFile)) Then
                    Dim FileInfo = New FileInfo(sSourceFile)
                    objInvoiceFile.InvoiceFileName = FileInfo.Name
                    sExtendFile = FileInfo.Extension
                    objInvoiceFile.InvoiceFileServer = "" & objInvoiceFile.InvoiceID_K & sExtendFile
                End If
                If nvd.ExistObject(objInvoiceFile) Then
                    nvd.Update(objInvoiceFile)
                Else
                    nvd.Insert(objInvoiceFile)
                End If

                If (objInvoiceFile.InvoiceFileName <> String.Empty) Then
                    clsApplication.UpLoadFile(sSourceFile, clsApplication.FileUploadPath + "\\" + objInvoiceFile.InvoiceFileServer, True)
                End If
                MessageBox.Show("Attach successfully", "Attach Invoice")
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Attach Invoice")
            End Try
        End If
    End Sub

    Private Sub lblOpenInvoice_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblOpenInvoice.LinkClicked
        Try
            If IsDBNull(cboInvoice.SelectedItem) Then Exit Sub
            Dim objInvoiceFile As New GSR_InvoiceFile
            objInvoiceFile.InvoiceID_K = Trim(cboInvoice.SelectedItem)
            nvd.GetObject(objInvoiceFile)
            If Not String.IsNullOrEmpty(objInvoiceFile.InvoiceFileName) Then
                If (File.Exists(clsApplication.FileUploadPath + "\\" + objInvoiceFile.InvoiceFileServer)) Then
                    clsApplication.OpenFile(clsApplication.FileUploadPath + "\\" + objInvoiceFile.InvoiceFileServer)
                End If
            End If
        Catch ex As Exception
            ShowError(ex, "lblOpenInvoice_LinkClicked", Me.Name)
        End Try
    End Sub

    Private Sub cboInvoice_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboInvoice.SelectedValueChanged
        If IsDBNull(cboInvoice.SelectedValue) Then Exit Sub
        If status = 0 Then Exit Sub
        Dim objInvoiceFile As New GSR_InvoiceFile
        objInvoiceFile.InvoiceID_K = Trim(cboInvoice.SelectedValue)
        nvd.GetObject(objInvoiceFile)
        If Not String.IsNullOrEmpty(objInvoiceFile.InvoiceFileName) Then
            lblOpenInvoice.Enabled = True
        Else
            lblOpenInvoice.Enabled = False
        End If
    End Sub

    Private Sub lblAttachPO_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAttachPO.LinkClicked
        If IsDBNull(cboPOFile.SelectedItem) Then Exit Sub
        Dim opd = New OpenFileDialog()
        opd.Filter = "File (*.*)|*.*|All file (*.*)|*.*"
        opd.Title = "Select a file to attach"
        If (opd.ShowDialog() = DialogResult.OK) Then
            Try
                Dim objPOFile As New GSR_POFile
                Dim sSourceFile As String = String.Empty
                Dim sExtendFile As String = String.Empty
                objPOFile.POID_K = Trim(cboPOFile.SelectedItem)
                sSourceFile = opd.FileName

                If (File.Exists(sSourceFile)) Then
                    Dim FileInfo = New FileInfo(sSourceFile)
                    objPOFile.POFileName = FileInfo.Name
                    sExtendFile = FileInfo.Extension
                    objPOFile.POFileServer = "" & objPOFile.POID_K & sExtendFile
                End If
                If nvd.ExistObject(objPOFile) Then
                    nvd.Update(objPOFile)
                Else
                    nvd.Insert(objPOFile)
                End If

                If (objPOFile.POFileName <> String.Empty) Then
                    clsApplication.UpLoadFile(sSourceFile, clsApplication.FileUploadPath + "\\" + objPOFile.POFileServer, True)
                End If
                MessageBox.Show("Attach successfully", "Attach PO")
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Attach PO")
            End Try
        End If
    End Sub

    Private Sub lblOpenPO_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblOpenPO.LinkClicked
        Try
            If IsDBNull(cboPOFile.SelectedItem) Then Exit Sub
            Dim objPOFile As New GSR_POFile
            objPOFile.POID_K = Trim(cboPOFile.SelectedItem)
            nvd.GetObject(objPOFile)
            If Not String.IsNullOrEmpty(objPOFile.POFileName) Then
                If (File.Exists(clsApplication.FileUploadPath + "\\" + objPOFile.POFileServer)) Then
                    clsApplication.OpenFile(clsApplication.FileUploadPath + "\\" + objPOFile.POFileServer)
                End If
            End If
        Catch ex As Exception
            ShowError(ex, "lblOpenPO_LinkClicked", Me.Name)
        End Try
    End Sub

    Private Sub cboPOFile_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPOFile.SelectedValueChanged
        If IsDBNull(cboPOFile.SelectedValue) Then Exit Sub
        If status = 0 Then Exit Sub
        Dim objPOFile As New GSR_POFile
        objPOFile.POID_K = Trim(cboPOFile.SelectedValue)
        nvd.GetObject(objPOFile)
        If Not String.IsNullOrEmpty(objPOFile.POFileName) Then
            lblOpenPO.Enabled = True
        Else
            lblOpenPO.Enabled = False
        End If
    End Sub

    Private Sub mnuSumINV_Click(sender As System.Object, e As System.EventArgs) Handles mnuSumINV.Click
        Dim frmINV As New FrmSumINV()
        Dim startDate As String = dtpOrderDate.Value.ToString("yyMMdd")
        Dim endDate As String = dtpOrderDateEnd.Value.ToString("yyMMdd")

        frmINV.Show()
    End Sub


    Private Sub mnuPrintDLVR_Click(sender As System.Object, e As System.EventArgs) Handles mnuPrintDLVR.Click
        If GridView1.SelectedRowsCount = 0 Then
            ShowWarning("Bạn chưa chọn nhà cung cấp để in !")
            Return
        End If

        Dim startDate As Date = dtpOrderDate.Value.Date
        Dim endDate As Date = dtpOrderDateEnd.Value.Date

        Dim para(3) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@startDate", startDate)
        para(1) = New SqlClient.SqlParameter("@endDate", endDate)
        If rdoOrderDate.Checked Then
            para(2) = New SqlClient.SqlParameter("@Type", "OrderDate")
        ElseIf rdoDeliveryDate.Checked Then
            para(2) = New SqlClient.SqlParameter("@Type", "DeliveryDate")
        ElseIf rdoReceiveddate.Checked Then
            para(2) = New SqlClient.SqlParameter("@Type", "ReceivedDate")
        End If
        para(3) = New SqlClient.SqlParameter("@VendorName", GridView1.GetFocusedRowCellValue("LastVendorName"))


        Dim dtDelivery As DataTable = nvd.ExecuteStoreProcedureTB("[GSR_LoadGSRFollow_Report]", para)
        'dtDelivery.TableName = "Delivery"
        'dtDelivery.WriteXmlSchema("Delivery.xsd")

        Dim dtGiaoHang As DataTable = nvd.ExecuteStoreProcedureTB("[GSR_LoadGSRFollow_Report_GiaoHang]", para)
        'dtGiaoHang.TableName = "DeliveryGiaoHang"
        'dtGiaoHang.WriteXmlSchema("DeliveryGiaoHang.xsd")

        Dim dtGia As DataTable = nvd.ExecuteStoreProcedureTB("[GSR_LoadGSRFollow_Report_Gia]", para)
        'dtGia.TableName = "DeliveryGia"
        'dtGia.WriteXmlSchema("DeliveryGia.xsd")

        Dim frmReport As New FrmReport()
        frmReport.ReportViewer.LocalReport.DataSources.Clear()
        frmReport.ReportViewer.LocalReport.ReportPath = "Reports\GSR\rptDlvr.rdlc"
        frmReport.ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("Delivery", dtDelivery))
        frmReport.ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DeliveryGiaoHang", dtGiaoHang))
        frmReport.ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DeliveryGia", dtGia))

        frmReport.ReportViewer.LocalReport.SetParameters(New ReportParameter("startDate", dtpOrderDate.Value))
        frmReport.ReportViewer.LocalReport.SetParameters(New ReportParameter("endDate", dtpOrderDateEnd.Value))
        frmReport.ReportViewer.LocalReport.SetParameters(New ReportParameter("VendorName", GridView1.GetFocusedRowCellValue("LastVendorName").ToString))
        frmReport.ReportViewer.RefreshReport()
        frmReport.Show()
    End Sub

    Private Sub bttUpdatePO_Click(sender As System.Object, e As System.EventArgs) Handles bttUpdatePO.Click
        Cursor = Cursors.AppStarting
        mnuShowAll.PerformClick()
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Date", DateTime.Now.AddMonths(-1))
        For r As Integer = 0 To GridView1.RowCount - 1
            If GridView1.GetRowCellValue(r, "POID") Is DBNull.Value Then
                Dim sql As String = String.Format(" Select TFPONB as POID  " +
                                                   " from NDVDTA.MTDPOHP A " +
                                                   " left join NDVDTA.MTEPOLP B on A.TDPONB=B.TEPONB " +
                                                   " left join NDVDTA.MTFPODP C on A.TDPONB=C.TFPONB " +
                                                   " where A.TDCNDT = 0 and TEITCD='{0}' and TFSCQT={1} and TFCRDT>='{2}'  " +
                                                   " order by TFCRDT desc " +
                                                   "  ",
                                                   GridView1.GetRowCellValue(r, "JCode"),
                                                   GridView1.GetRowCellValue(r, "Quantity"),
                                                   DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"))
                Dim myPO As DataTable = dbAS.FillDataTable(sql)
                If myPO.Rows.Count > 0 Then
                    For Each rs As DataRow In myPO.Rows
                        Dim sqlCheck As String = String.Format("select POID from GSR_GoodsOrderDetail where POID='{0}' and DeliveryDate>@Date ",
                                                           rs("POID"))
                        If nvd.ExecuteScalar(sqlCheck, para) Is Nothing Then
                            Dim objD As New GSR_GoodsOrderDetail
                            objD.ID_K = GridView1.GetRowCellValue(r, "ID")
                            objD.OrderID_K = GridView1.GetRowCellValue(r, "OrderID")
                            nvd.GetObject(objD)
                            objD.POID = rs("POID")
                            nvd.Update(objD)
                            Exit For
                        End If
                    Next
                End If
            Else
                If GridView1.GetRowCellValue(r, "POID") = "" Then
                    Dim sql As String = String.Format(" Select  TFPONB as POID   " +
                                               " from NDVDTA.MTDPOHP A " +
                                               " left join NDVDTA.MTEPOLP B on A.TDPONB=B.TEPONB " +
                                               " left join NDVDTA.MTFPODP C on A.TDPONB=C.TFPONB " +
                                               " where A.TDCNDT = 0  and TEITCD='{0}' and TFSCQT={1} and TFCRDT>='{2}' " +
                                               " order by TFCRDT desc " +
                                               "  ",
                                               GridView1.GetRowCellValue(r, "JCode"),
                                               GridView1.GetRowCellValue(r, "Quantity"),
                                               DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"))
                    Dim myPO As DataTable = dbAS.FillDataTable(sql)
                    If myPO.Rows.Count > 0 Then
                        For Each rs As DataRow In myPO.Rows
                            Dim sqlCheck As String = String.Format("select POID from GSR_GoodsOrderDetail where POID='{0}' and DeliveryDate>@Date ",
                                                         rs("POID"))
                            If nvd.ExecuteScalar(sqlCheck, para) Is Nothing Then
                                Dim objD As New GSR_GoodsOrderDetail
                                objD.ID_K = GridView1.GetRowCellValue(r, "ID")
                                objD.OrderID_K = GridView1.GetRowCellValue(r, "OrderID")
                                nvd.GetObject(objD)
                                objD.POID = rs("POID")
                                nvd.Update(objD)
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        Next

        Cursor = Cursors.Default
        ShowSuccess()
    End Sub

    Private Sub bttConfirm_Click(sender As System.Object, e As System.EventArgs) Handles bttConfirm.Click
        If dtpConfirm.Checked Then
            For Each r As Integer In GridView1.GetSelectedRows
                Dim objD As New GSR_GoodsOrderDetail
                objD.ID_K = GridView1.GetRowCellValue(r, "ID")
                objD.OrderID_K = GridView1.GetRowCellValue(r, "OrderID")
                nvd.GetObject(objD)
                objD.ConfirmDate = dtpConfirm.Value.Date
                nvd.Update(objD)
            Next
            mnuShowAll.PerformClick()
        Else
            For Each r As Integer In GridView1.GetSelectedRows
                Dim objD As New GSR_GoodsOrderDetail
                objD.ID_K = GridView1.GetRowCellValue(r, "ID")
                objD.OrderID_K = GridView1.GetRowCellValue(r, "OrderID")
                nvd.GetObject(objD)
                objD.ConfirmDate = DateTime.MinValue
                nvd.Update(objD)
            Next
            mnuShowAll.PerformClick()
        End If
    End Sub

    Private Sub mnuLoiNV_Click(sender As System.Object, e As System.EventArgs) Handles mnuLoiNV.Click
        If GridView1.GetFocusedRowCellValue("NotePU") IsNot DBNull.Value Then
            If GridView1.GetFocusedRowCellValue("NotePU") <> "" Then
                GridView1.SetFocusedRowCellValue("NotePU", DBNull.Value)
            Else
                GridView1.SetFocusedRowCellValue("NotePU", mnuLoiNV.Text)
            End If
        Else
            GridView1.SetFocusedRowCellValue("NotePU", mnuLoiNV.Text)
        End If
    End Sub

    Private Sub mnuLoiNCC_Click(sender As System.Object, e As System.EventArgs) Handles mnuLoiNCC.Click
        If GridView1.GetFocusedRowCellValue("NotePU") IsNot DBNull.Value Then
            If GridView1.GetFocusedRowCellValue("NotePU") <> "" Then
                GridView1.SetFocusedRowCellValue("NotePU", DBNull.Value)
            Else
                GridView1.SetFocusedRowCellValue("NotePU", mnuLoiNCC.Text)
            End If
        Else
            GridView1.SetFocusedRowCellValue("NotePU", mnuLoiNCC.Text)
        End If
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Dim frm As New FrmDeliveryDetail()
        frm.idDLVR = GridView1.GetFocusedRowCellValue("ID")
        frm.jcodeDLVR = GridView1.GetFocusedRowCellValue("JCode")
        frm.orderDLVR = GridView1.GetFocusedRowCellValue("OrderID")
        frm.ShowDialog()
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable = False Or e.Column.ReadOnly = True Then Exit Sub

        If e.Column.ReadOnly = False Then

            If checkLock() Then
                ShowWarning("GSR is locked. Can not Update!")
                Exit Sub
            End If
            If e.Column.FieldName = "POID" Then
                Dim POID As String
                If GridView1.GetFocusedRowCellValue("POID") Is DBNull.Value Then
                    POID = ""
                Else
                    POID = Trim(UCase(GridView1.GetFocusedRowCellValue("POID")))
                End If
                Dim ID As String = Trim(GridView1.GetFocusedRowCellValue("ID"))
                Dim OrderID As Integer = Trim(GridView1.GetFocusedRowCellValue("OrderID"))
                Dim JCode As String = Trim(GridView1.GetFocusedRowCellValue("JCode"))

                If Trim(POID).Length <> 8 And Trim(POID).Length <> 0 Then
                    ShowWarning("PO is wrong! Please input again")
                    Exit Sub
                End If

                Dim objD As New GSR_GoodsOrderDetail
                objD.ID_K = GridView1.GetFocusedRowCellValue("ID")
                objD.OrderID_K = GridView1.GetFocusedRowCellValue("OrderID")
                nvd.GetObject(objD)
                objD.POID = POID
                nvd.Update(objD)
                If POID = "" Then
                    Return
                End If

                Dim sqlGH As String = String.Format("Delete from {0} where ID = '{1}' and OrderID = '{2}' and JCode = '{3}'",
                                                        PublicTable.Table_GSR_GoodsDetail, objD.ID_K,
                                                        objD.OrderID_K,
                                                        objD.JCode)
                nvd.ExecuteNonQuery(sqlGH)

                'cột TDPRPO là PO đại diện
                Dim sqlASCurrency As String = String.Format(" Select TDPRPO, A.TDCUCD as CURRENCYCODE, B.TEITCD ITEMCODE," +
                                                                " C.TFSCDT as SCHEDULEDATE, C.TFBUNP as UNITPRICE, B.TEORQT as ORDERQUANTITY " +
                                                                " from NDVDTA.MTDPOHP A " +
                                                                " Left Join NDVDTA.MTEPOLP B ON A.TDPONB=B.TEPONB " +
                                                                " Left Join NDVDTA.MTFPODP C ON A.TDPONB=C.TFPONB " +
                                                                " Where A.TDPONB = '{0}'", POID)

                Dim dtASCurrency As DataTable = dbAS.FillDataTable(sqlASCurrency)
                If dtASCurrency.Rows.Count <> 0 Then
                    Dim QtyDLVR = IIf(GridView1.GetFocusedRowCellValue("Quantity") Is DBNull.Value, 0,
                                          GridView1.GetFocusedRowCellValue("Quantity"))
                    Dim QtyPO = dtASCurrency.Rows(0).Item("ORDERQUANTITY")

                    Dim JCodeDLVR = IIf(GridView1.GetFocusedRowCellValue("JCode") Is DBNull.Value, "",
                                            GridView1.GetFocusedRowCellValue("JCode"))
                    Dim ItemCodePO = dtASCurrency.Rows(0).Item("ITEMCODE")
                    If JCodeDLVR <> ItemCodePO Then
                        MessageBox.Show("JCode is not the same: " & ItemCodePO & " .Check again, please!", "Error Input")
                        Exit Sub
                    End If
                    GridView1.SetFocusedRowCellValue("TDPRPO", dtASCurrency.Rows(0).Item("TDPRPO"))
                    GridView1.SetFocusedRowCellValue("CurrencyCode", dtASCurrency.Rows(0).Item("CURRENCYCODE"))
                    GridView1.SetFocusedRowCellValue("ScheduleDate", convertDateAS(dtASCurrency.Rows(0).Item("SCHEDULEDATE")))
                    GridView1.SetFocusedRowCellValue("UnitPrice", dtASCurrency.Rows(0).Item("UNITPRICE"))
                    GridView1.SetFocusedRowCellValue("TotalAmount", GridView1.GetFocusedRowCellValue("UnitPrice") * GridView1.GetFocusedRowCellValue("Quantity"))
                Else
                    If Trim(POID).Length <> 0 Then
                        ShowWarning("PO is wrong! Please input again !")
                        Exit Sub
                    End If
                End If

                Dim sqlAS As String = String.Format("Select A.*, B.THVEIV as INVOICEID, B.THAPNB as AP " +
                                               " from (select TPPONB, TPACQT as RECEIVEDQTY, TPPUDT as INVOICEDATE, TPPUNB, TPTRSE, TPITCD from NDVDTA.MTPTRNP " +
                                               " where TPPONB ='{0}' and TPITCD = '{1}' and TPTRTY Like '110') as A " +
                                               " left join NDVDTA.MTHVIVP B on A.TPPUNB = B.THPUNB and A.TPPUNB = B.THPUNB and A.TPTRSE = B.THAPSE", POID, JCode)
                Dim dtAS As DataTable = dbAS.FillDataTable(sqlAS)

                Dim index As Integer = 1
                Dim countRec As Integer = 0
                Dim sum As Decimal = 0

                If dtAS.Rows.Count <> 0 Then
                    For i As Integer = 0 To dtAS.Rows.Count - 1
                        Dim obj As New GSR_GoodsDetail()
                        obj.ID_K = objD.ID_K
                        obj.OrderNo_K = index
                        obj.OrderID_K = objD.OrderID_K
                        obj.JCode = objD.JCode
                        obj.POID = POID
                        obj.ReceivedDate = convertDateAS(dtAS.Rows(i).Item("INVOICEDATE"))
                        obj.ReceivedQuantity = dtAS.Rows(i).Item("RECEIVEDQTY")
                        obj.InvoiceID = IIf(dtAS.Rows(i).Item("INVOICEID") Is DBNull.Value, "", dtAS.Rows(i).Item("INVOICEID"))
                        obj.AP = IIf(dtAS.Rows(i).Item("AP") Is DBNull.Value, "", dtAS.Rows(i).Item("AP"))
                        countRec = countRec + 1
                        sum = sum + dtAS.Rows(i).Item("RECEIVEDQTY")
                        If nvd.ExistObject(obj) Then
                            nvd.Update(obj)
                        Else
                            nvd.Insert(obj)
                        End If
                        index += 1
                    Next

                    GridView1.SetFocusedRowCellValue("ReceivedDate", convertDateAS(dtAS.Rows(0).Item("INVOICEDATE")))
                    GridView1.SetFocusedRowCellValue("ReceivedQty", sum)
                    GridView1.SetFocusedRowCellValue("InvoiceID", dtAS.Rows(0).Item("INVOICEID"))
                    GridView1.SetFocusedRowCellValue("ReceivedUser", CurrentUser.UserID)

                    Dim timespan As TimeSpan = GridView1.GetFocusedRowCellValue("ReceivedDate") - GridView1.GetFocusedRowCellValue("DeliveryDate")
                    Dim latedays As Integer = Convert.ToInt32(timespan.Days())
                    GridView1.SetFocusedRowCellValue("LateDays", latedays)
                Else
                    GridView1.SetFocusedRowCellValue("ReceivedDate", DBNull.Value)
                    GridView1.SetFocusedRowCellValue("ReceivedQty", 0)
                    GridView1.SetFocusedRowCellValue("InvoiceID", DBNull.Value)
                    GridView1.SetFocusedRowCellValue("ReceivedUser", DBNull.Value)
                    GridView1.SetFocusedRowCellValue("LateDays", DBNull.Value)
                End If

                GridView1.SetFocusedRowCellValue("CountReceived", countRec)
                GridView1.SetFocusedRowCellValue("RemainingQty", GridView1.GetFocusedRowCellValue("Quantity") - GridView1.GetFocusedRowCellValue("ReceivedQty"))

                objD.POID = POID
                objD.ReceivedQty = IIf(GridView1.GetFocusedRowCellValue("ReceivedQty") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("ReceivedQty"))
                objD.CountReceived = IIf(GridView1.GetFocusedRowCellValue("CountReceived") Is DBNull.Value, 0, GridView1.GetFocusedRowCellValue("CountReceived"))
                Dim dateNow As New DateTime(1, 1, 1)
                objD.ReceivedDate = IIf(GridView1.GetFocusedRowCellValue("ReceivedDate") Is DBNull.Value, dateNow, GridView1.GetFocusedRowCellValue("ReceivedDate"))
                objD.ReceivedUser = IIf(GridView1.GetFocusedRowCellValue("ReceivedUser") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("ReceivedUser"))
                objD.CurrencyCode = IIf(GridView1.GetFocusedRowCellValue("CurrencyCode") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("CurrencyCode"))
                Dim unitPrice As Decimal = 0
                objD.UnitPrice = IIf(GridView1.GetFocusedRowCellValue("UnitPrice") Is DBNull.Value, unitPrice, GridView1.GetFocusedRowCellValue("UnitPrice"))
                objD.ScheduleDate = IIf(GridView1.GetFocusedRowCellValue("ScheduleDate") Is DBNull.Value, dateNow, GridView1.GetFocusedRowCellValue("ScheduleDate"))
                objD.InvoiceID = IIf(GridView1.GetFocusedRowCellValue("InvoiceID") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("InvoiceID"))
                objD.TDPRPO = IIf(GridView1.GetFocusedRowCellValue("TDPRPO") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("TDPRPO"))

                If nvd.ExistObject(objD) Then
                    nvd.Update(objD)
                Else
                    objD.ID_K = GridView1.GetFocusedRowCellValue("ID")
                    objD.OrderID_K = GridView1.GetFocusedRowCellValue("OrderID")
                    nvd.Insert(objD)
                End If
            ElseIf e.Column.FieldName = "Quantity" Then
                Dim objD As New GSR_GoodsOrderDetail()
                objD.ID_K = GridView1.GetFocusedRowCellValue("ID")
                objD.OrderID_K = GridView1.GetFocusedRowCellValue("OrderID")
                nvd.GetObject(objD)
                objD.Quantity = IIf(GridView1.GetFocusedRowCellValue("Quantity") Is DBNull.Value, 0,
                                    GridView1.GetFocusedRowCellValue("Quantity"))
                nvd.Update(objD)
            ElseIf e.Column.FieldName = "Leadtime" Then
                Dim objD As New GSR_Leadtime()
                objD.ItemCode_K = GridView1.GetFocusedRowCellValue("JCode")
                nvd.GetObjectNotReset(objD)
                objD.Leadtime = IIf(GridView1.GetFocusedRowCellValue("Leadtime") Is DBNull.Value, 0,
                                    GridView1.GetFocusedRowCellValue("Leadtime"))
                If nvd.ExistObject(objD) Then
                    nvd.Update(objD)
                Else
                    nvd.Insert(objD)
                End If
            ElseIf e.Column.FieldName = "NotePU" Then
                Dim objD As New GSR_GoodsOrderDetail()
                objD.ID_K = GridView1.GetFocusedRowCellValue("ID")
                objD.OrderID_K = GridView1.GetFocusedRowCellValue("OrderID")
                nvd.GetObject(objD)
                objD.NotePU = IIf(GridView1.GetFocusedRowCellValue("NotePU") Is DBNull.Value, "",
                              GridView1.GetFocusedRowCellValue("NotePU"))
                nvd.Update(objD)
            ElseIf e.Column.FieldName = "MinQty" Then
                Dim objD As New GSR_GoodsOrderDetail()
                objD.ID_K = GridView1.GetFocusedRowCellValue("ID")
                objD.OrderID_K = GridView1.GetFocusedRowCellValue("OrderID")

                nvd.GetObject(objD)
                objD.MinQty = IIf(GridView1.GetFocusedRowCellValue("MinQty") Is DBNull.Value, 0,
                              GridView1.GetFocusedRowCellValue("MinQty"))
                nvd.Update(objD)
            ElseIf e.Column.FieldName = "DeliveryDate" Then
                Dim objD As New GSR_GoodsOrderDetail()
                objD.ID_K = GridView1.GetFocusedRowCellValue("ID")
                objD.OrderID_K = GridView1.GetFocusedRowCellValue("OrderID")
                nvd.GetObject(objD)
                If GridView1.GetFocusedRowCellValue("DeliveryDate") Is DBNull.Value Then
                    objD.DeliveryDate = DateTime.MinValue
                ElseIf String.IsNullOrEmpty(GridView1.GetFocusedRowCellValue("DeliveryDate")) Then
                    objD.DeliveryDate = DateTime.MinValue
                Else
                    objD.DeliveryDate = convertddMMyyyy(Trim(GridView1.GetFocusedRowCellValue("DeliveryDate")))
                End If
                nvd.Update(objD)
            End If

        End If
    End Sub

    Private Sub mnuExportExcel_Click(sender As Object, e As EventArgs) Handles mnuExportExcel.Click
        GridControlExportExcel(GridView1)
    End Sub

    Private Sub GridView1_RowStyle(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles GridView1.RowStyle
        If GridView1.GetRowCellValue(e.RowHandle, "ConfirmDate") Is DBNull.Value Then
            e.Appearance.BackColor = Color.Yellow
        End If
    End Sub
End Class