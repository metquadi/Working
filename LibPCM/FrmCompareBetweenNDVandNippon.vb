﻿Imports CommonDB
Imports PublicUtility
Imports LibEntity
Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Office.Interop

Public Class FrmCompareBetweenNDVandNippon : Inherits DevExpress.XtraEditors.XtraForm
    Dim _dbFpics As New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    'Dim dbAS As New DBFunction(PublicConst.EnumServers.NDV_DB2_AS400)

    Dim dtAll As DataTable


    Private Sub mnuShowAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowAll.Click
        Dim Day As DateTime = dtpOrderDate.Value.AddDays(-1)
        Dim DayCompare As String = Day.ToString("yyyyMMdd")
        Dim param(0) As SqlClient.SqlParameter
        param(0) = New SqlClient.SqlParameter("@day", DayCompare)

        dtAll = _db.ExecuteStoreProcedureTB("sp_PCM_CompareNipponNDV", param)
        Dim bd As New BindingSource
        bd.DataSource = dtAll
        gridD.DataSource = bd
        bnGrid.BindingSource = bd
    End Sub 

    Private Sub dtpOrderDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpOrderDate.ValueChanged
    
        mnuShowAll.PerformClick() 
    End Sub

    Private Sub txtJCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJCode.TextChanged
        If dtAll Is Nothing Then
            Return
        Else
            Dim dv As DataView = New DataView(dtAll)
            dv.RowFilter = "[JCode] LIKE '%" + txtJCode.Text + "%'"
            Dim bd As New BindingSource()
            bd.DataSource = dv
            gridD.DataSource = bd
        End If
    End Sub

    Private Sub txtJCode_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJCode.Enter
        SetColorEnter(txtJCode)
    End Sub

    Private Sub txtJCode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJCode.Leave
        SetColorLeave(txtJCode)
    End Sub

    Private Sub mnuExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExport.Click
        ExportEXCEL(gridD)
    End Sub

    Private Sub FrmCompareBetweenNDVandNippon_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        cboDifference.Items.Add("")
        cboDifference.Items.Add("<>0")
        cboDifference.Items.Add("=0")
    End Sub

    Private Sub cboDifference_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDifference.SelectedIndexChanged
        If dtAll Is Nothing Then
            Return
        Else
            Dim dv As DataView = New DataView(dtAll)
            If cboDifference.Text = "<>0" Or cboDifference.Text = "=0" Then
                dv.RowFilter = "Nipp_NDV " + Trim(cboDifference.Text)
            Else
                dv.RowFilter = ""
            End If
            Dim bd As New BindingSource()
            bd.DataSource = dv
            gridD.DataSource = bd
            bnGrid.BindingSource = bd
        End If
        txtJCode.Focus()
    End Sub

    Private Sub mnuStockOld_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuStockOld.Click
        'Dim sfDlg As New OpenFileDialog()
        'sfDlg.DefaultExt = ".xlsx"
        'sfDlg.Filter = "Excel 2007 file(.xlsx)|*.xlsx| Excel 2003 file(.xls)|*.xls"
        'sfDlg.FileName = "Importstock.xlsx"
        'sfDlg.InitialDirectory = String.Format("S:\COMMON\KHO\2.DAILYSTOCK\DAILY NL {0}\DAILY NL THANG {1}\DAILY {2}\",
        '                                       DateTime.Now.ToString("yyyy"),
        '                                       DateTime.Now.ToString("MM.yyyy"),
        '                                       DateTime.Now.ToString("dd.MM.yyyy"))
        'If sfDlg.ShowDialog() <> Windows.Forms.DialogResult.OK Then Exit Sub

        Try

            Dim dt As DataTable = ImportEXCEL(True)
            Me.Cursor = Cursors.WaitCursor

            If dt.Rows.Count = 0 Then
                ShowWarning("Không thấy dữ liệu cần import. Vui lòng xem lại file excel.")
                Exit Sub
            End If

            If dt.Rows.Count <> 0 Then
                _db.BeginTransaction()
                Dim Day As DateTime = dtpOrderDate.Value.AddDays(-1)

                Dim sqlDelete As String = String.Format("Delete from {0} where DDate = '{1}'",
                                                        PublicTable.Table_PCM_StockOld,
                                                        Day.ToString("yyyyMMdd"))
                _db.ExecuteNonQuery(sqlDelete)

                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim obj As New PCM_StockOld()
                    'PART NO.
                    If dt.Rows(i).Item(0) IsNot DBNull.Value Then
                        obj.DDate_K = Day.ToString("yyyyMMdd")
                        obj.JCode_K = Trim(dt.Rows(i).Item(0))
                        obj.JCode_K = obj.JCode_K.Trim
                    Else
                        Continue For
                    End If
                    'AVAIL QTY
                    If IsNumeric(dt.Rows(i).Item(4)) Then
                        obj.Qty = dt.Rows(i).Item(4)
                    End If
                    'PICKED QTY
                    If IsNumeric(dt.Rows(i).Item(5)) Then
                        obj.PickedQty = dt.Rows(i).Item(5)
                    End If
                    'DAMAGED(QTY)
                    If IsNumeric(dt.Rows(i).Item(6)) Then
                        obj.HeldQty = dt.Rows(i).Item(6)
                    End If
                    'Held QTY
                    If IsNumeric(dt.Rows(i).Item(7)) Then
                        obj.HeldQty += dt.Rows(i).Item(7)
                    End If
                    'ACT ISSUE
                    'If dt.Columns.Count >= 9 Then
                    '    If dt.Rows(i).Item(8) IsNot DBNull.Value Then
                    '        obj.ActQty = dt.Rows(i).Item(8)
                    '    End If
                    'End If

                    Dim sqlJCode As String = String.Format(" select ItemCode as JCode, ItemName as JName, " +
                                                           " UnitCode as Unit, BuyingMinimumQuantity as MinQty " +
                                                           " from t_ASMaterialItem " +
                                                           " Where ItemCode = '{0}'",
                                                           dt.Rows(i).Item(0))
                    Dim dtJCode As DataTable = _dbFpics.FillDataTable(sqlJCode)

                    Dim sqlSubCode As String = String.Format("select Code, Name, Unit from {0} " +
                    "where Code = '{1}'", PublicTable.Table_PCM_SubMter, dt.Rows(i).Item(0))
                    Dim dtSubCode As DataTable = _db.FillDataTable(sqlSubCode)

                    If dtJCode.Rows.Count <> 0 Then
                        obj.JName = IIf(dtJCode.Rows(0).Item("JName") Is DBNull.Value, "", dtJCode.Rows(0).Item("JName"))
                        obj.Unit = IIf(dtJCode.Rows(0).Item("Unit") Is DBNull.Value, "", dtJCode.Rows(0).Item("Unit"))
                        obj.MinQty = IIf(dtJCode.Rows(0).Item("MinQty") Is DBNull.Value, 0, dtJCode.Rows(0).Item("MinQty"))
                    ElseIf dtSubCode.Rows.Count <> 0 Then
                        obj.JName = IIf(dtSubCode.Rows(0).Item("Name") Is DBNull.Value, "", dtSubCode.Rows(0).Item("Name"))
                        obj.Unit = IIf(dtSubCode.Rows(0).Item("Unit") Is DBNull.Value, "", dtSubCode.Rows(0).Item("Unit"))
                        obj.MinQty = 0
                    Else
                        obj.JName = ""
                        obj.Unit = ""
                        obj.MinQty = 0
                    End If

                    If obj.JName = "" Then
                        If dt.Rows(i).Item(1) IsNot DBNull.Value Then
                            obj.JName = Trim(dt.Rows(i).Item(1))
                        End If
                    End If
                    If _db.ExistObject(obj) Then
                        _db.Update(obj)
                    Else
                        _db.Insert(obj)
                    End If
                Next
                Me.Cursor = Cursors.Arrow
                _db.Commit()

                ShowSuccess()

                If dtpOrderDate.Value.ToString("ddMMyy") = DateTime.Now.ToString("ddMMyy") Then
                    Try
                        'Cập nhật tồn
                        getStockMulti()
                        'Tạo file báo cáo 
                        Dim param(3) As SqlClient.SqlParameter
                        param(0) = New SqlClient.SqlParameter("@StartDate", DBNull.Value)
                        param(1) = New SqlClient.SqlParameter("@EndDate", DBNull.Value)
                        param(3) = New SqlClient.SqlParameter("@ActionDate", "ExportExpiryCode")

                        'DLVR
                        param(2) = New SqlClient.SqlParameter("@Action", "DLVR")
                        Dim tbData As New DataTable
                        tbData = _db.ExecuteStoreProcedureTB("sp_EMM_ShowAll", param)
                        Dim myFile As String = ExportToFile(tbData, "ExpiryCode-" & DateTime.Now.ToString("dd-MMM-yyyy") & ".xlsx")

                        'thì gửi mail cảnh báo
                        If CurrentUser.UserID = "00113" Or
                            CurrentUser.UserID = "12829" Or
                            CurrentUser.UserID = "00846" Then
                            Dim objAlarm As New EMM_AlarmExpiryCode
                            objAlarm.YMD_K = DateTime.Now.ToString("yyyyMMdd")
                            If _db.ExistObject(objAlarm) Then
                                If ShowQuestion("Hôm nay đã gởi rồi bạn muốn gởi lại không ?") = DialogResult.No Then
                                    Return
                                End If
                            End If

                            Dim arrCc() As String
                            arrCc = {"yuichi.moro@nitto.com",
                                     "huong.huynhthixuan@nitto.com",
                                     "uyen.daokhanh@nitto.com",
                                     "kieu.huynhthuy@nitto.com",
                                     "linh.phamthimy@nitto.com",
                                     "hang.doanthithu@nitto.com",
                                     "tuan.nguyentran@nitto.com",
                                     "mai.hoxuan@nitto.com",
                                     "son.dinhtan@nitto.com",
                                     "ngan.tokieu@nitto.com"}
                            Dim mLst As New List(Of String)
                            mLst.Add(CurrentUser.Mail)

                            'Send mail Lotus
                            'SendMail(CurrentUser.Mail, arrCc, "", PublicFunction.Submit.Confirm,
                            '         String.Format("Expiry Codes"),
                            '         "Danh sách những Item đã hết HSD và còn HSD trong vòng 10 ngày tới.", "022601", "HSD",
                            '         DateTime.Now.Date)

                            'Send Mail outlook
                            If ShowQuestion("Bạn muốn gởi cho chính mình thì chọn YES hoặc gởi báo cáo thì chọn NO ?") = DialogResult.Yes Then
                                SendMailOutlookReport("Expiry Codes " & DateTime.Now.ToString("dd-MMM-yyyy"),
                                                  "Danh sách những Item đã hết HSD và còn HSD trong vòng 10 ngày tới.",
                                                  mLst,
                                                  GetListMail(CurrentUser.Mail),
                                                  Nothing,
                                                  GetListFile(myFile))
                            Else
                                SendMailOutlookReport("Expiry Codes " & DateTime.Now.ToString("dd-MMM-yyyy"),
                                                  "Danh sách những Item đã hết HSD và còn HSD trong vòng 10 ngày tới.",
                                                  GetListMail(arrCc),
                                                  GetListMail(CurrentUser.Mail),
                                                  Nothing,
                                                  GetListFile(myFile))
                                objAlarm.YMD_K = DateTime.Now.ToString("yyyyMMdd")
                                objAlarm.Status = True
                                _db.Insert(objAlarm)
                            End If
                        End If
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try
                End If
            End If
        Catch ex As Exception
            _db.RollBack()
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub getStockMulti()
        Dim maxDate As String = ""
        maxDate = _db.ExecuteScalar("select max(DDate) as DDate from [dbo].[PCM_StockOld]")

        Dim sqlDelete As String = String.Format("Delete from EMM_ViewStock")
        Dim sqlUpdateStock As String = String.Format(" update [dbo].[PCM_StockOld] " +
                                                     " set totalQty= case when m.QtyOfCartonIQC = 0 Then  HeldQty + Qty" +
                                                                      " when QtyOfCartonIQC > (HeldQty + Qty) Then (HeldQty + Qty) * QtyOfCartonIQC" +
                                                                      " Else HeldQty + Qty" +
                                                                      " End" +
                                                    " from [dbo].[EMM_MasterJCode] m" +
                                                    " where [PCM_StockOld].DDate='{0}' " +
                                                    " and [PCM_StockOld].JCode=m.JCode",
                                                    maxDate)
        _db.ExecuteNonQuery(sqlUpdateStock)
        _db.ExecuteNonQuery(sqlDelete)

        Dim sqlJCode As String = String.Format("SELECT JCode FROM dbo.EMM_DLVRList  GROUP BY JCode")
        Dim dtJCode As DataTable = _db.FillDataTable(sqlJCode)

        Dim dtDLVRTemp As New DataTable
        dtDLVRTemp.Columns.Add("Code", GetType(String))
        dtDLVRTemp.Columns.Add("JCode", GetType(String))
        dtDLVRTemp.Columns.Add("Stock", GetType(Decimal))
		dtDLVRTemp.Columns.Add("StockExpired", GetType(Decimal))
		dtDLVRTemp.Columns.Add("StockAvailable", GetType(Decimal))

		For i As Int16 = 0 To dtJCode.Rows.Count - 1
            Dim sqlStock As String = String.Format(" Select  CAST(DDate as datetime) as DDate, JCode, Qty, HeldQty,TotalQty" +
                                                   " from {0} where JCode = '{1}' and DDate='{2}' ",
                                                   PublicTable.Table_PCM_StockOld,
                                                   dtJCode.Rows(i)("JCode"),
                                                   maxDate)
            Dim dtStock As DataTable = _db.FillDataTable(sqlStock)
            If dtStock.Rows.Count = 0 Then Continue For

            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@IncomingDate", dtStock.Rows(0)("DDate"))
            'If dtJCode.Rows(i)("JCode") = "J02976" Then
            '    ShowWarning("J02976")
            'End If
            If dtStock.Rows.Count <> 0 Then
                Dim QtyStock As Decimal = dtStock.Rows(0)("Qty") + dtStock.Rows(0)("HeldQty")
                Dim sqlDLVR As String = String.Format(" SELECT  " +
                                                    " d.Code, " +
                                                    " d.IncomingDate, " +
                                                    " d.JCode, " +
                                                    " TotalQtyAS = CASE WHEN  u.[CartonQty]   = 'X'   THEN d.CartonQuantity" +
                                                                   "   WHEN u.[TotalQty]  = 'X'  THEN d.TotalQuantity" +
                                                                   "   ELSE 0" +
                                                   " End" +
                                                    " FROM {0} d " +
                                                    " left join EMM_UnitAS u on u.UnitAS=d.UnitAS400 " +
                                                    " WHERE d.JCode = '{1}' " +
                                                    " AND  d.IncomingDate <= @IncomingDate and d.HaveStock='Y' " +
                                                    " ORDER BY d.IncomingDate DESC",
                                                    PublicTable.Table_EMM_DLVRList,
                                                    dtJCode.Rows(i)("JCode"))
                Dim dtDLVR As DataTable = _db.FillDataTable(sqlDLVR, para)
                Dim importdate As DateTime = DateTime.Now
				For Each row As DataRow In dtDLVR.Rows
					Dim r As DataRow = dtDLVRTemp.NewRow
					If row("TotalQtyAS") <= QtyStock Then
						r("StockAvailable") = QtyStock - row("TotalQtyAS")
						QtyStock = QtyStock - row("TotalQtyAS")
						importdate = row("IncomingDate")
					ElseIf row("TotalQtyAS") > QtyStock And QtyStock <> 0 Then
						r("StockAvailable") = QtyStock
						QtyStock = 0
						importdate = row("IncomingDate")
					Else
						If importdate <> row("IncomingDate") Then
							Exit For
						End If
					End If

					r("Code") = row("Code")
					r("JCode") = row("JCode")
					r("Stock") = dtStock.Rows(0)("Qty") + dtStock.Rows(0)("HeldQty")
					'r("StockExpired") = ""
					'r("StockAvailable") = ""
					dtDLVRTemp.Rows.Add(r)
				Next
			End If
        Next
        _db.BulkCopy(dtDLVRTemp, "EMM_ViewStock")
    End Sub

    Private Sub mnuSendEmail_Click(sender As System.Object, e As System.EventArgs) Handles mnuSendMail.Click
        If ShowQuestion("Bạn muốn gởi mail báo cáo nguyên liệu đến cấp trên ? ") = Windows.Forms.DialogResult.Yes Then
            Cursor = Cursors.AppStarting

            Dim startdate As DateTime = GetStartDate(DateTime.Now.AddDays(-1))
            Dim obj As New FPICS_HolidayDate
            obj.HolidayDate_K = startdate
            While _db.ExistObject(obj)
                obj.HolidayDate_K = obj.HolidayDate_K.AddDays(-1)
            End While
            dtpOrderDate.Value = obj.HolidayDate_K

            Dim lstmail As String = " vinh.vodong@nitto.com, trung.doquang@nitto.com, son.dinhtan@nitto.com, " +
                                    " yuichi.moro@nitto.com, quoc.nguyenbao@nitto.com, uyen.daokhanh@nitto.com," +
                                    " ho.lephi@nitto.com,hieu.nguyenvan@nitto.com "
            Dim lstcc As String = CurrentUser.Mail & ",ngan.tokieu@nitto.com, hang.doanthithu@nitto.com, tuan.nguyentran@nitto.com, linh.phamthimy@nitto.com" +
                                                      ", giau.vuthi@nitto.com ,kieu.huynhthuy@nitto.com,huong.huynhthixuan@nitto.com"


            Dim title As String = "[Information] warehouse output data of consumable goods"
            Dim content As String = String.Format(" Dear Sir/ Madam, " & olNewline & olNewline &
                                      " Pls find warehouse output data of consumable goods of {0} by opening below file" & olNewline & olNewline &
                                      " Thank you", dtpOrderDate.Value.ToString("dd-MMM-yyyy"))


            Dim param(3) As SqlClient.SqlParameter
            param(0) = New SqlClient.SqlParameter("@StartDate", obj.HolidayDate_K.ToString("yyyyMMdd"))
            param(1) = New SqlClient.SqlParameter("@EndDate", obj.HolidayDate_K.ToString("yyyyMMdd"))
            param(2) = New SqlClient.SqlParameter("@JCode", DBNull.Value)
            param(3) = New SqlClient.SqlParameter("@Report", "OK")


            Dim dtTableAll As DataTable = _db.ExecuteStoreProcedureTB("sp_PCM_VerticalSummary", param)
            'Dim dtViewJCode As New DataView(dtTableAll, "[JCode/ItemCode]  like 'J%'", "", DataViewRowState.CurrentRows)
            Dim dtViewItemCode As New DataView(dtTableAll, "[JCode/ItemCode]  NOT LIKE 'J%'", "", DataViewRowState.CurrentRows)

            'Dim myfileAll As String = ExportToFile(dtTableAll, "ItemCode and JCode " & dtpOrderDate.Value.ToString("dd-MMM-yyyy") & ".xlsx", "")
            'Dim myfileJCode As String = ExportToFile(dtViewJCode.ToTable(), "JCode " & dtpOrderDate.Value.ToString("dd-MMM-yyyy") & ".xlsx", "")
            Dim myfileItemCode As String = ExportToFile(dtViewItemCode.ToTable(), "ItemCode " & dtpOrderDate.Value.ToString("dd-MMM-yyyy") & ".xlsx", "")

            SendMailOutlookReport(title,
                                                    content,
                                                    GetListMail(lstmail),
                                                    GetListMail(lstcc),
                                                    Nothing,
                                                    GetListFile(String.Format("{0}", myfileItemCode)))

            Cursor = Cursors.Default

            ShowSuccess()
        End If
    End Sub

    Sub ExportEXCELSX_DVC(ByVal grid As DataGridView, ByVal gridS As DataGridView, ByVal sheetName As String, ByVal fileName As String, Optional isVisible As Boolean = True)
        Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        If grid.DataSource Is Nothing Then Exit Sub
        If grid.RowCount = 0 Then Exit Sub


        Dim myExcel As Excel.Application = New Excel.Application
        Dim wbook As Excel.Workbook
        Dim wSheet As Excel.Worksheet
        Dim wRange As Excel.Range
        wbook = myExcel.Application.Workbooks.Add(True)
        wbook.Sheets.Add(Type.Missing, myExcel.Application.Worksheets(myExcel.Application.Worksheets.Count), 1, Excel.XlSheetType.xlWorksheet)
        wSheet = myExcel.Application.Worksheets(1)
        wSheet.Activate()
        If sheetName <> "" Then
            wSheet.Name = sheetName
        End If
        myExcel.Visible = isVisible

        Dim i, j As Integer
        Dim rows As Integer = grid.Rows.Count
        Dim cols As Integer = grid.Columns.Count
        Dim DataArray(rows - 1, cols - 1) As Object

        Dim i_S As Integer, jS As Integer
        Dim rowsS As Integer = gridS.Rows.Count
        Dim colsS As Integer = gridS.Columns.Count
        Dim DataArrayS(rowsS - 1, colsS - 1) As Object

        For i = 0 To rows - 1
            For j = 0 To cols - 1
                If grid.Rows(i).Cells(j).Value Is System.DBNull.Value Then Continue For
                If String.IsNullOrEmpty(grid.Rows(i).Cells(j).Value) Then
                Else
                    If grid.Rows(i).Cells(j).Value.GetType().Name = "String" Then
                        DataArray(i, j) = "'" & grid.Rows(i).Cells(j).Value.ToString()
                    Else
                        DataArray(i, j) = grid.Rows(i).Cells(j).Value
                    End If

                End If
            Next
        Next

        For i_S = 0 To rowsS - 1
            For jS = 0 To colsS - 1
                If gridS.Rows(i_S).Cells(jS).Value Is System.DBNull.Value Then Continue For
                If String.IsNullOrEmpty(gridS.Rows(i_S).Cells(jS).Value) Then
                Else
                    If gridS.Rows(i_S).Cells(jS).Value.GetType().Name = "String" Then
                        DataArrayS(i_S, jS) = "'" & gridS.Rows(i_S).Cells(jS).Value.ToString()
                    Else
                        DataArrayS(i_S, jS) = gridS.Rows(i_S).Cells(jS).Value
                    End If

                End If
            Next
        Next

        For j = 0 To cols - 1
            wSheet.Cells(1, j + 1) = "'" + grid.Columns(j).HeaderText
        Next
        If DataArray IsNot Nothing Then
            wSheet.Range("A2").Resize(rows, cols).Value = DataArray
        End If
        If DataArrayS IsNot Nothing Then
            wSheet.Range("C" & i + 4).Resize(rowsS, colsS).Value = DataArrayS
        End If

        wRange = wSheet.Cells(i + 2, 7)
        wRange.Formula = "=sum(G2:G" & i + 1 & ")"
        wRange = wSheet.Cells(i + 2, 8)
        wRange.Formula = "=sum(H2:H" & i + 1 & ")"
        wRange = wSheet.Cells(i + 2, 9)
        wRange.Formula = "=sum(I2:I" & i + 1 & ")"
        wRange = wSheet.Cells(i + 2, 10)
        wRange.Formula = "=sum(J2:J" & i + 1 & ")"
        wRange = wSheet.Cells(i + 2, 11)
        wRange.Formula = "=sum(I2:I" & i + 1 & ")/sum(H2:H" & i + 1 & ")*100"
        wRange = wSheet.Cells(i + 2, 13)
        wRange.Formula = "=sum(M2:M" & i + 1 & ")"
        wRange = wSheet.Cells(i + 2, 14)
        wRange.Formula = "=sum(N2:N" & i + 1 & ")"
        For Each c As DataGridViewColumn In grid.Columns
            If c.DefaultCellStyle.Format = "N0" Then
                wRange = wSheet.Columns(c.Index + 1)
                wRange.NumberFormat = "#,###,###"
            ElseIf c.DefaultCellStyle.Format = "N2" Then
                wRange = wSheet.Columns(c.Index + 1)
                wRange.NumberFormat = "#,###,##0.00"
            End If
        Next

        wSheet.Activate()
        wSheet.Application.ActiveWindow.SplitRow = 1
        wSheet.Application.ActiveWindow.SplitColumn = 5
        wSheet.Application.ActiveWindow.FreezePanes = True
        Dim firstRow As Excel.Range = CType(wSheet.Rows(1), Excel.Range)
        firstRow.AutoFilter(1,
                    Type.Missing,
                    Excel.XlAutoFilterOperator.xlAnd,
                    Type.Missing,
                    True)
        Try

            If File.Exists(fileName) Then
                File.Delete(fileName)
            End If

            wbook.SaveAs(fileName) 

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        Catch ex As Exception
        End Try

    End Sub

    Private Sub mnuUpdateAdjust_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateAdjust.Click
        If ShowQuestion("Bạn muốn cập nhật số dư tồn vào Adjust ?") = Windows.Forms.DialogResult.Yes Then
            For Each r As DataGridViewRow In gridD.Rows
                If r.Cells("Nipp_NDV").Value > 0 And r.Cells("JCode").Value.ToString.Contains("J") Then
                    Dim para(2) As SqlClient.SqlParameter
                    para(0) = New SqlClient.SqlParameter("@Day", dtpOrderDate.Value.AddDays(-1).ToString("yyyyMMdd"))
                    para(1) = New SqlClient.SqlParameter("@JCode", r.Cells("JCode").Value)
                    para(2) = New SqlClient.SqlParameter("@Qty", r.Cells("Nipp_NDV").Value)
                    _db.ExecuteStoreProcedure("sp_PCM_UpdateAdjust", para)
                End If
            Next
            ShowSuccess()
            mnuShowAll.PerformClick()
        End If
    End Sub
End Class