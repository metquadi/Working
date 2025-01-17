﻿Imports System.Globalization
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.InteropServices

Imports CommonDB
Imports LibEntity
Imports PublicUtility
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Threading
Imports System.Reflection

Public Class FrmDailyOutputPlanningCopy : Inherits DevExpress.XtraEditors.XtraForm

    Private DB As DBSql
    Private dbFpics As DBSql

    Private arrPrcDblSide() As String

    Private bCboLoading As Boolean = False

    Private EnuActionForm As ActionForm = ActionForm.FormLoad

    Enum ActionForm
        None = 0
        FormLoad = 1
        Edit = 2
        Back = 3
        Export = 4
        Refresh = 5
        Delete = 6
        PlanningDetail = 7
        PlanningTotal = 8
    End Enum

    Private EditRight As Boolean = False
    Private BackRight As Boolean = False
    Private ExportRight As Boolean = False
    Private RefreshRight As Boolean = False
    Private DeleteRight As Boolean = False
    Private PlanningDetailRight As Boolean = False
    Private PlanningTotalRight As Boolean = False

    ReadOnly Property GetFormEvents As ActionForm
        Get
            Return EnuActionForm
        End Get
    End Property

    WriteOnly Property SetFormEvents As ActionForm
        Set(ByVal value As ActionForm)

            EnuActionForm = value

            gridDailyOutputPlanning.ReadOnly = True
            gridDailyOutputPlanning.AllowUserToAddRows = False

            cboCustomer.Enabled = True
            dtpFromDate.Enabled = True
            dtpToDate.Enabled = True

            Try
                Select Case value
                    Case ActionForm.Edit
                        cboCustomer.Enabled = False
                        dtpFromDate.Enabled = False
                        dtpToDate.Enabled = False

                        gridDailyOutputPlanning.ReadOnly = False
                        gridDailyOutputPlanning.AllowUserToAddRows = True

                        gridDailyOutputPlanning.Columns("CustomerName").ReadOnly = True

                        LoadLeadTime()

                        PerformStatusActionForm(ActionForm.Edit)
                    Case ActionForm.Refresh, ActionForm.Delete, ActionForm.Back
                        LoadLeadTime()
                        LoadAll()
                        rd_CheckedChanged(Nothing, Nothing)
                        PerformStatusActionForm(ActionForm.FormLoad)
                        EnuActionForm = ActionForm.None
                    Case ActionForm.FormLoad
                        InitArrPrcDblSide()
                        LoadComboBox()
                        LoadLeadTime()
                        PerformStatusActionForm(ActionForm.FormLoad)
                        EnuActionForm = ActionForm.None
                End Select
            Catch ex As Exception
                ShowError(ex, "SetFormEvents", Me.Name)
            End Try
        End Set
    End Property

#Region "User Function"

    Private Sub PerformStatusActionForm(ByVal enuActionForm As ActionForm)
        mnuEdit.Enabled = EditRight
        mnuBack.Enabled = BackRight
        mnuExport.Enabled = ExportRight
        mnuRefresh.Enabled = RefreshRight
        mnuDelete.Enabled = DeleteRight
        mnuPlanningDetail.Enabled = PlanningDetailRight
        mnuPlanningTotal.Enabled = PlanningTotalRight

        If (enuActionForm = ActionForm.FormLoad _
            Or enuActionForm = ActionForm.Back _
            Or enuActionForm = ActionForm.Delete) Then
            mnuBack.Enabled = False
        End If

        If (enuActionForm = ActionForm.Edit) Then
            mnuEdit.Enabled = False
            mnuRefresh.Enabled = False
            mnuPlanningDetail.Enabled = False
            mnuPlanningTotal.Enabled = False
        End If
    End Sub

    Private Sub InitArrPrcDblSide()
        ReDim arrPrcDblSide(6)
        arrPrcDblSide(0) = "ET_SS_Half"
        arrPrcDblSide(1) = "Laser_Abrasion"
        arrPrcDblSide(2) = "HP_DS_Exposure_R_to_R_Prox"
        arrPrcDblSide(3) = "Laser_Abrasion_Blind_Via"
        arrPrcDblSide(4) = "R_to_R_PTH"
        arrPrcDblSide(5) = "HP_DS_Exposure_R_to_R_Prox_2"
        arrPrcDblSide(6) = "Outgoing Inspect"
    End Sub

    Private Sub LoadComboBox()
        bCboLoading = True
        Dim sql As String = String.Format("SELECT CustomerCode, CustomerName FROM {0}", PublicTable.Table_PD_MsCustomer)
        Dim tbl As DataTable = DB.FillDataTable(sql)
        tbl.Rows.InsertAt(tbl.NewRow(), 0)
        cboCustomer.DisplayMember = "CustomerName"
        cboCustomer.ValueMember = "CustomerCode"
        cboCustomer.DataSource = tbl
        bCboLoading = False
    End Sub

    Private Sub LoadLeadTime()
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@CustomerCode", String.Empty)

        Dim sql As String = String.Format("SELECT ID,LeadTimeName,CustomerCode FROM {0} WHERE (ISNULL(@CustomerCode,'') = '' OR CustomerCode = @CustomerCode)", PublicTable.Table_PD_MsLeadTime)
        Dim tbl As DataTable = DB.FillDataTable(sql, para)

        tbl.Rows.InsertAt(tbl.NewRow(), 0)
        LeadTimeID.ValueMember = "ID"
        LeadTimeID.DisplayMember = "LeadTimeName"
        LeadTimeID.DataSource = tbl
    End Sub

    Private Sub LoadAll()
        If tblDailyOutputPlanning IsNot Nothing Then
            tblDailyOutputPlanning.Clear()
            gridDailyOutputPlanning.DataSource = Nothing
        End If

        Dim para(3) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Action", "LoadAll")
        para(1) = New SqlClient.SqlParameter("@FromDate", dtpFromDate.Value.Date)
        para(2) = New SqlClient.SqlParameter("@ToDate", dtpToDate.Value.Date)
        para(3) = New SqlClient.SqlParameter("@CustomerCode", cboCustomer.SelectedValue)

        Dim tbl As DataTable = DB.ExecuteStoreProcedureTB("sp_PD_DailyOutputPlanning_1", para)

        AddDayColumns()

        CreateDailyOutput(tbl)

        bsDailyOutputPlanning.DataSource = tblDailyOutputPlanning
        gridDailyOutputPlanning.DataSource = bsDailyOutputPlanning
        bnDailyOutputPlanning.BindingSource = bsDailyOutputPlanning

        If gridRestDate IsNot Nothing Then gridRestDate.DataSource = Nothing

        AddDayRowToGridTotal()
        CalQuantityOnGridTotal()

        Dim auto As New AutoCompleteStringCollection
        For Each r As DataRow In tblDailyOutputPlanning.Rows
            Dim sPdCode As String = r("ProductCode")
            auto.Add(sPdCode)
        Next
        txtProductCode.AutoCompleteCustomSource = auto
    End Sub

    Private Function CheckDataBeforeSave()
        Return True
    End Function

    Private Sub AddDayColumns()
        Dim lst As New List(Of String)

        'Add các cột ngày
        For Each c As DataColumn In tblDailyOutputPlanning.Columns
            If c.ColumnName <> "ProductCode" _
                And c.ColumnName <> "StartLot" _
                And c.ColumnName <> "LeadTimeID" _
                And c.ColumnName <> "ProductCode_K" _
                And c.ColumnName <> "CustomerName" Then
                lst.Add(c.ColumnName)
            End If
        Next

        'Remove các cột ngày
        For Each item In lst
            tblDailyOutputPlanning.Columns.Remove(item)
            gridDailyOutputPlanning.Columns.Remove(item)
        Next

        Dim fromDate As DateTime = dtpFromDate.Value.Date
        Dim toDate As DateTime = dtpToDate.Value.Date

        Dim lstRestDate As List(Of DataRow) = clsUtil.GetRestDate(DB)
        'Add lại các cột ngày dựa trên FromDate -> ToDate
        While fromDate <= toDate
            Dim c As New DataColumn(fromDate.ToString("dd-MMM-yy"), Type.GetType("System.String"))
            If Not tblDailyOutputPlanning.Columns.Contains(c.ColumnName) Then
                tblDailyOutputPlanning.Columns.Add(c)
                Dim cg As New DataGridViewTextBoxColumn
                cg.DataPropertyName = c.ColumnName
                cg.Name = c.ColumnName
                cg.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                cg.SortMode = DataGridViewColumnSortMode.NotSortable
                If Not gridDailyOutputPlanning.Columns.Contains(cg.Name) Then
                    gridDailyOutputPlanning.Columns.Add(cg)
                    'Tô màu đối với ngày nghỉ
                    Dim oRestDate = (From p In lstRestDate Where p("RestDate") = fromDate
                                             Select p("RestDate")).FirstOrDefault()
                    If oRestDate IsNot Nothing Then
                        gridDailyOutputPlanning.Columns(cg.Name).HeaderCell.Style.BackColor = Color.Orange  'Màu tiêu đề
                    End If
                End If
            End If
            fromDate = fromDate.AddDays(1)
        End While

        'Add EndLot
        Dim cEndLot As New DataColumn("EndLot", Type.GetType("System.String"))
        tblDailyOutputPlanning.Columns.Add(cEndLot)
        Dim cgEndLot As New DataGridViewTextBoxColumn
        cgEndLot.DataPropertyName = cEndLot.ColumnName
        cgEndLot.Name = cEndLot.ColumnName
        cgEndLot.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        cgEndLot.SortMode = DataGridViewColumnSortMode.NotSortable
        cgEndLot.ReadOnly = True
        If Not gridDailyOutputPlanning.Columns.Contains(cgEndLot.Name) Then
            gridDailyOutputPlanning.Columns.Add(cgEndLot)
        End If
    End Sub

    Private Sub CreateDailyOutput(ByVal tbl As DataTable)
        Dim para(3) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Action", "LoadDetail")
        para(1) = New SqlClient.SqlParameter("@FromDate", dtpFromDate.Value.Date)
        para(2) = New SqlClient.SqlParameter("@ToDate", dtpToDate.Value.Date)
        para(3) = New SqlClient.SqlParameter("@CustomerCode", cboCustomer.SelectedValue)

        Dim tblDetail As DataTable = DB.ExecuteStoreProcedureTB("sp_PD_DailyOutputPlanning_1", para)

        For Each r As DataRow In tbl.Rows
            Dim rAdd As DataRow = tblDailyOutputPlanning.NewRow()
            Dim rs() As DataRow = Nothing

            rAdd("ProductCode") = r("ProductCode")
            rAdd("ProductCode_K") = r("ProductCode_K")
            rAdd("CustomerName") = r("CustomerName")

            Dim iLotTotal As Int32 = 0

            For Each c As DataColumn In tblDailyOutputPlanning.Columns
                If c.ColumnName = "ProductCode" _
                    Or c.ColumnName = "ProductCode_K" _
                    Or c.ColumnName = "CustomerName" _
                    Or c.ColumnName = "StartLot" _
                    Or c.ColumnName = "LeadTimeID" _
                    Or c.ColumnName = "EndLot" Then
                    Continue For
                End If

                Dim planningDate As DateTime = DateTime.ParseExact(c.ColumnName, "dd-MMM-yy", CultureInfo.InvariantCulture)
                Dim sProductCode As String = r("ProductCode")

                rs = tblDetail.Select(String.Format("ProductCode='{0}' And PlanningDate='{1}'", sProductCode, planningDate.ToString("yyyy-MM-dd")))

                If rs.Length > 0 Then
                    rAdd(c.ColumnName) = rs(0)("Quantity")
                    iLotTotal += IIf(rs(0)("Quantity") Is DBNull.Value, String.Empty, GetQuantityDay(rs(0)("Quantity")))
                End If
            Next

            Dim fromDate As DateTime = dtpFromDate.Value.Date
            Dim toDate As DateTime = dtpToDate.Value.Date

            'Tìm StartLot
            While fromDate <= toDate
                rs = tblDetail.Select(String.Format("ProductCode='{0}' And PlanningDate='{1}'", r("ProductCode"), fromDate.ToString("yyyy-MM-dd")))
                If rs.Length > 0 Then
                    rAdd("LeadTimeID") = rs(0)("LeadTimeID")
                    rAdd("StartLot") = rs(0)("StartLot")
                    Exit While
                End If
                fromDate = fromDate.AddDays(1)
            End While

            'Tính giá trị EndLot
            If rAdd("StartLot") IsNot DBNull.Value Then
                Dim iStartLot As Int32 = If(rAdd("StartLot") = String.Empty, 0, CType(rAdd("StartLot"), Int32))
                If iStartLot > 0 Then
                    rAdd("EndLot") = ((iStartLot + iLotTotal) - IIf(iLotTotal > 0, 1, 0)).ToString()
                End If
            End If

            tblDailyOutputPlanning.Rows.Add(rAdd)
        Next
    End Sub

    Private Sub SaveFileAndRelease(ByRef app As Excel.Application _
                               , ByRef workRange As Excel.Range _
                               , ByRef workSheet As Excel.Worksheet _
                               , ByRef workBook As Excel.Workbook)
        'Save file
        If app IsNot Nothing Then app.DisplayAlerts = False
        If workBook IsNot Nothing Then workBook.Save()

        'Release all objects
        GC.Collect()
        GC.WaitForPendingFinalizers()
        If workRange IsNot Nothing Then Marshal.FinalReleaseComObject(workRange)
        If workSheet IsNot Nothing Then Marshal.FinalReleaseComObject(workSheet)
        If workBook IsNot Nothing Then Marshal.FinalReleaseComObject(workBook)
        If app IsNot Nothing Then Marshal.FinalReleaseComObject(app)
    End Sub

    Private Function GetProcessMaxWidth(ByVal item As DataRow) As Int32
        Dim lst As New List(Of Int32)

        For Each c As DataColumn In item.Table.Columns
            If c.ColumnName <> "ProductCode" _
                   And c.ColumnName <> "ProductCode_K" _
                   And c.ColumnName <> "LeadTimeID" _
                   And c.ColumnName <> "CustomerName" _
                   And c.ColumnName <> "StartLot" _
                   And c.ColumnName <> "EndLot" Then
                'Lấy thời gian làm việc
                Dim dPlanningDate As DateTime = DateTime.ParseExact(c.ColumnName, "dd-MMM-yy", CultureInfo.InvariantCulture)
                Dim oWorkingHour As Object = DB.ExecuteScalar(String.Format("SELECT WorkingHour FROM dbo.PD_DailyOutputPlanning_1 D LEFT JOIN dbo.PD_MsCustomer C ON D.CustomerCode = C.CustomerCode WHERE C.CustomerName = '{0}' AND ProductCode = '{1}' And PlanningDate='{2}'", _
                                                                     item("CustomerName"), item("ProductCode"), dPlanningDate.ToString("yyyy-MM-dd")))
                Dim iWorkingHour As Byte = If(oWorkingHour Is Nothing, 0, oWorkingHour)
                Dim iDivision As Byte = iWorkingHour / 4
                Dim iQty As Int32 = If(item(c.ColumnName) Is DBNull.Value, 0, GetMaxQuantity(item(c.ColumnName), iDivision))
                lst.Add(iQty)
            End If
        Next

        Dim iMaxQty As Int32 = lst.Max()
        Dim iZ = (iMaxQty \ 2) + (iMaxQty Mod 2)
        Return iZ
    End Function

    Private Function GetProcessMaxWidth(ByVal item As DataGridViewRow) As Int32
        Dim lst As New List(Of Int32)

        For Each c As DataGridViewColumn In item.DataGridView.Columns
            If c.Name <> ProductCode.Name _
                   And c.Name <> ProductCode_K.Name _
                   And c.Name <> LeadTimeID.Name _
                   And c.Name <> CustomerName.Name _
                   And c.Name <> StartLot.Name _
                   And c.Name <> "EndLot" _
                   And c.Name <> Tick.Name Then
                'Lấy thời gian làm việc
                Dim dPlanningDate As DateTime = DateTime.ParseExact(c.Name, "dd-MMM-yy", CultureInfo.InvariantCulture)
                Dim oWorkingHour As Object = DB.ExecuteScalar(String.Format("SELECT WorkingHour FROM dbo.PD_DailyOutputPlanning_1 D LEFT JOIN dbo.PD_MsCustomer C ON D.CustomerCode = C.CustomerCode WHERE C.CustomerName = '{0}' AND ProductCode = '{1}' And PlanningDate='{2}'", _
                                                                     item.Cells("CustomerName").Value, item.Cells("ProductCode").Value, dPlanningDate.ToString("yyyy-MM-dd")))
                Dim iWorkingHour As Byte = If(oWorkingHour Is Nothing, 0, oWorkingHour)
                Dim iDivision As Byte = iWorkingHour / 4
                Dim iQty As Int32 = If(item.Cells(c.Name).Value Is DBNull.Value, 0, GetMaxQuantity(item.Cells(c.Name).Value, iDivision))
                lst.Add(iQty)
            End If
        Next

        Dim iMaxQty As Int32 = lst.Max()
        Dim iZ = (iMaxQty \ 2) + (iMaxQty Mod 2)
        Return iZ
    End Function

    Private Sub FormatAllBorders(ByRef workRange As Excel.Range, _
                             ByRef workSheet As Excel.Worksheet, _
                             ByVal fromCell As Object, _
                             ByVal toCell As Object)

        workRange = workSheet.Range(fromCell, toCell)
        workRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
        workRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
        workRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
        workRange.Columns.AutoFit()
    End Sub

    Private Sub FormatDetail(ByRef workRange As Excel.Range, _
                              ByRef workSheet As Excel.Worksheet, _
                              ByVal fromCell As Object, _
                              ByVal toCell As Object, _
                              ByVal iProcessMaxWidth As Int32, _
                              ByVal iEndRow As Int32, _
                              ByVal iEndCol As Int32)

        workRange = workSheet.Range(fromCell, toCell)

        workRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
        workRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

        workRange.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = 2
        workRange.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = 2
        workRange.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = 2
        workRange.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = 2
        workRange.Borders(Excel.XlBordersIndex.xlInsideHorizontal).Weight = Excel.XlBorderWeight.xlHairline
        workRange.Borders(Excel.XlBordersIndex.xlInsideVertical).Weight = Excel.XlBorderWeight.xlHairline

        Dim iSRow As Int32 = 3
        While iSRow <= iEndRow
            workRange = workSheet.Range(workSheet.Cells(iSRow, 4), workSheet.Cells(iSRow + iProcessMaxWidth - 1, iEndCol))
            workRange.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = 2
            iSRow += iProcessMaxWidth
        End While

        Dim iSCol As Int32 = 4
        While iSCol <= iEndCol
            workRange = workSheet.Range(workSheet.Cells(2, 4), workSheet.Cells(iEndRow, iSCol + 5))
            workRange.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = 2
            iSCol += 6
        End While
        workRange = workSheet.Range(workSheet.Cells(1, 4), workSheet.Cells(iEndRow, iEndCol))
        workRange.Columns.ColumnWidth = 5
    End Sub

    Function CheckFileOpened(ByVal sFileName As String) As Boolean
        Try
            Dim s As Stream = File.Open(sFileName, FileMode.Open, FileAccess.Read, FileShare.None)
            s.Close()
        Catch ex As Exception
            Return True
        End Try
        Return False
    End Function

    Private Sub SetFormulaDoubleSide(ByRef app As Excel.Application, _
                                ByRef bIsFileOpened As Boolean, _
                                ByVal sCusNameOfDetail As String, _
                                ByVal item As DataRow, _
                                ByVal iEndCol As Int32, _
                                ByRef workSheet As Excel.Worksheet, _
                                ByVal iRow As Int32, _
                                ByVal iProcessMaxWidth As Int32)



        'ProductCode
        Dim spdc As String = item("ProductCode")
        'ProcessName
        Dim sprn As String = workSheet.Range(workSheet.Cells(iRow, 3), workSheet.Cells(iRow, 3)).Value

        If Not arrPrcDblSide.Contains(sprn) Then Exit Sub

        Dim wbDblSide As Excel.Workbook = Nothing 'Workbook

        If Not bIsFileOpened Then
            Dim sPath As String = String.Format(clsUtil.FileExp + "{0}.xlsx", "_DOUBLESIDE_PLAN")
            If File.Exists(sPath) Then
                wbDblSide = app.Workbooks.Open(sPath, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing)
                bIsFileOpened = True
            End If
        Else
            wbDblSide = app.Workbooks("_DOUBLESIDE_PLAN.xlsx")
            wbDblSide.Activate()
        End If

        If wbDblSide Is Nothing Then Exit Sub

        'Active sheet ở file double side tương đương với tên công đoạn
        Dim wsDblSide As Excel.Worksheet = wbDblSide.Sheets(sprn) 'Worksheet
        wsDblSide.Select(Type.Missing)

        'Tìm ô thỏa điều kiện =ProductCode & =Date
        Dim timep As System.TimeSpan = dtpToDate.Value.Date.Subtract(dtpFromDate.Value.Date)

        ''Duyệt dòng trên nhóm file tổng
        ''Số dòng tối đa hiện tại = 200
        For i_ As Int32 = 3 To 200

            If wsDblSide.Cells(i_, 1).ToString() = String.Empty Then Exit For

            Dim sProductCode As String = wsDblSide.Range(wsDblSide.Cells(i_, 1), wsDblSide.Cells(i_, 1)).Value 'cột ProductCode trên file excel
            Dim sCusNameOfTotal As String = wsDblSide.Range(wsDblSide.Cells(i_, 2), wsDblSide.Cells(i_, 2)).Value 'cột Customer trên file excel

            If sCusNameOfTotal <> sCusNameOfDetail Or sProductCode <> item("ProductCode") Then Continue For

            'Khai báo khoản cần tính min -> 'Lấy min(lô)
            Dim wR As Excel.Range = wsDblSide.Range(wsDblSide.Cells(i_, 3), wsDblSide.Cells(i_, 3)) 'cột startlot = 3
            wR.Formula = "=MIN('" + clsUtil.FileExp + "[" + sCusNameOfDetail + ".xlsx]" + sProductCode + String.Format("'!${0}${1}:${2}${3})",
                                                                                                                       clsUtil.Number2String(4, True),
                                                                                                                       iRow,
                                                                                                                       clsUtil.Number2String(iEndCol, True),
                                                                                                                       iRow + iProcessMaxWidth - 1)
            '''''''''''''''''''''''''''''''''''''''''''
            Dim col_ As Int32 = 10 'cột bắt đầu lô của file plan detail
            Dim iLen As Int32 = 3 + ((timep.Days + 1) * 3)
            For j_ As Int32 = 4 To iLen
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Đếm lô
                'Dim bBName As String = clsUtil.Number2String(col_, True)
                'Dim bEName As String = clsUtil.Number2String(col_ + 1, True)
                Dim sLotStr As String = String.Empty
                For i1 = iRow To iRow + iProcessMaxWidth - 1
                    For j1 = col_ To col_ + 1
                        wR = workSheet.Range(workSheet.Cells(i1, j1), workSheet.Cells(i1, j1))
                        Dim sLot As String = If(wR.Value Is Nothing, String.Empty, wR.Value)
                        If sLot <> String.Empty Then sLotStr += sLot + ", "
                    Next
                Next
                If sLotStr.Length > 0 Then
                    sLotStr = Trim(sLotStr)
                    sLotStr = sLotStr.Substring(0, sLotStr.Length - 1)
                End If
                wsDblSide.Cells(i_, j_) = sLotStr
                wR = wsDblSide.Range(wsDblSide.Cells(i_, j_), wsDblSide.Cells(i_, j_))
                wR.Rows.AutoFit()
                'Dim sFormula = "=+COUNT('" + clsUtil.FileExp + "[" + sCusNameOfDetail + ".xlsx]" + sProductCode + String.Format("'!${0}${1}:${2}${3})",
                '                                                                                                                bBName,
                '                                                                                                                iRow,
                '                                                                                                                bEName,
                '                                                                                                                iRow + iProcessMaxWidth - 1)
                'wR.Formula = sFormula
                col_ += 2
            Next
        Next

    End Sub

    Private Sub SetFormulaTotal(ByRef app As Excel.Application, _
                                ByRef bIsTotalFileOpened As Boolean, _
                                ByVal sCusNameOfDetail As String, _
                                ByVal item As DataRow, _
                                ByVal iEndCol As Int32, _
                                ByRef workSheet As Excel.Worksheet, _
                                ByVal iRow As Int32, _
                                ByVal iProcessMaxWidth As Int32)

        'Xác định nhóm của Process
        Dim sProcessCode As String = workSheet.Range(workSheet.Cells(iRow, 2), workSheet.Cells(iRow, 2)).Value
        Dim sProcessNumber As String = workSheet.Range(workSheet.Cells(iRow, 1), workSheet.Cells(iRow, 1)).Value

        Dim _sql = String.Format(
                "SELECT PL.ProcessCode,PL.ReportGroupCode,G.ReportGroupName " +
                "FROM " +
                "dbo.PD_ProcessLeadTime PL " +
                "LEFT JOIN dbo.PD_MsReportGroup G " +
                "ON PL.ReportGroupCode=G.ReportGroupCode " +
                "WHERE LeadTimeID = '{0}' " +
                        "AND ProcessCode = '{1}' " +
                        "AND ISNULL(ProcessNumber,'') = '{2}' " +
                        "AND ISNULL(PL.ReportGroupCode,'') != '' ", item("LeadTimeID"), sProcessCode, sProcessNumber)

        Dim tbl As DataTable = DB.FillDataTable(_sql)

        If tbl.Rows.Count > 0 Then
            Dim wbTotal As Excel.Workbook = Nothing 'Workbook

            If Not bIsTotalFileOpened Then
                Dim sPath As String = String.Format(clsUtil.FileExp + "{0}.xlsx", "_TOTAL")
                If File.Exists(sPath) Then
                    wbTotal = app.Workbooks.Open(sPath, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing)
                    bIsTotalFileOpened = True
                End If
            Else
                wbTotal = app.Workbooks("_TOTAL.xlsx")
                wbTotal.Activate()
            End If

            If wbTotal IsNot Nothing Then
                'Active sheet ở file tổng tương ứng với nhóm
                Dim wsTotal As Excel.Worksheet = wbTotal.Sheets(tbl.Rows(0)("ReportGroupName")) 'Worksheet
                wsTotal.Select(Type.Missing)
                'Tìm ô thỏa điều kiện =Customer & =ProductCode & =Date
                ''Tìm vị trí cột customer
                Dim timep As System.TimeSpan = dtpToDate.Value.Date.Subtract(dtpFromDate.Value.Date)
                Dim iColCus As Int32 = timep.Days + 5 '=4
                ''Duyệt dòng trên nhóm file tổng
                ''Số dòng tối đa hiện tại = 200
                For i_ As Int32 = 3 To 200

                    If wsTotal.Cells(i_, 1).ToString() = String.Empty Then Exit For

                    Dim sCusNameOfTotal As String = wsTotal.Range(wsTotal.Cells(i_, iColCus), wsTotal.Cells(i_, iColCus)).Value
                    Dim sProductCode As String = wsTotal.Range(wsTotal.Cells(i_, 1), wsTotal.Cells(i_, 1)).Value

                    If sCusNameOfTotal = sCusNameOfDetail And sProductCode = item("ProductCode") Then
                        'Khai báo khoản cần tính min -> 'Lấy min(lô)
                        Dim wR As Excel.Range = wsTotal.Range(wsTotal.Cells(i_, 3), wsTotal.Cells(i_, 3)) 'startlot=2
                        wR.Formula = "=MIN('" + clsUtil.FileExp + "[" + sCusNameOfDetail + ".xlsx]" + sProductCode + String.Format("'!${0}${1}:${2}${3})", clsUtil.Number2String(4, True), iRow, clsUtil.Number2String(iEndCol, True), iRow + iProcessMaxWidth - 1)
                        '''''''''''''''''''''''''''''''''''''''''''
                        For j_ As Int32 = 4 To timep.Days + 4 'j_=3 / timep.Days + 3
                            wR = wsTotal.Range(wsTotal.Cells(i_, j_), wsTotal.Cells(i_, j_))
                            Dim output As DateTime = wsTotal.Range(wsTotal.Cells(1, j_), wsTotal.Cells(1, j_)).Value
                            Dim col_ As Int32 = 10
                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            While col_ <= iEndCol
                                Dim output_ As DateTime = workSheet.Range(workSheet.Cells(1, col_), workSheet.Cells(1, col_)).Value
                                If output.ToString("yyyy-MM-dd") = output_.ToString("yyyy-MM-dd") Then
                                    'Đếm lô
                                    Dim bCName As String = clsUtil.Number2String(col_, True)
                                    Dim bEName As String = clsUtil.Number2String(col_ + 5, True)
                                    Dim sFormula = "=+COUNT('" + clsUtil.FileExp + "[" + sCusNameOfDetail + ".xlsx]" + sProductCode + String.Format("'!${0}${1}:${2}${3})", bCName, iRow, bEName, iRow + iProcessMaxWidth - 1)
                                    wR.Formula = sFormula
                                    Exit While
                                End If
                                col_ += 6
                            End While
                        Next
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub SetValues(ByRef obj As PD_DailyOutputPlanning_1, _
                          ByVal r As DataGridViewRow, _
                          ByVal c As DataGridViewColumn, _
                          ByVal sCusCode As String, _
                          ByVal planningDate As DateTime)
        obj.CustomerCode_K = sCusCode
        obj.ProductCode_K = IIf(r.Cells("ProductCode").Value Is DBNull.Value, String.Empty, r.Cells("ProductCode").Value)
        obj.PlanningDate_K = planningDate

        obj.Quantity = IIf(r.Cells(c.Name).Value Is DBNull.Value, String.Empty, r.Cells(c.Name).Value)
        obj.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(obj.PlanningDate_K, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday)
        obj.StartLot = IIf(r.Cells("StartLot").Value Is DBNull.Value, String.Empty, r.Cells("StartLot").Value)
        obj.LeadTimeID = IIf(r.Cells("LeadTimeID").Value Is DBNull.Value, String.Empty, r.Cells("LeadTimeID").Value)

        'Cập nhật giờ làm việc cho ngày nghỉ
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Date", planningDate.Date)
        Dim sql As String = String.Format("SELECT [HolidayDate] FROM [FPICS_HolidayDate] WHERE HolidayDate = @Date")
        Dim tbl As DataTable = DB.FillDataTable(sql, para)
        If tbl.Rows.Count > 0 Then 'Nếu là ngày nghỉ
            Dim restDate As DateTime = CType(tbl.Rows(0)(0), DateTime)
            If restDate.DayOfWeek = DayOfWeek.Saturday Then : obj.WorkingHour = If(obj.Quantity <> String.Empty, 24, 0)
            ElseIf restDate.DayOfWeek = DayOfWeek.Sunday Then : obj.WorkingHour = If(obj.Quantity <> String.Empty, 24, 0)
            End If
        Else 'Nếu là ngày bình thường
            obj.WorkingHour = 24
        End If

        obj.CreateDate = DateTime.Now
        obj.CreateUser = CurrentUser.UserID
    End Sub

    Private Function GetQuantityDay(ByVal sQuantity As String) As Int32
        If sQuantity = String.Empty Then Return 0
        sQuantity = sQuantity.Trim().Replace("--", "-")
        Dim arr() As String = sQuantity.Split("-")
        If arr.Length > 0 Then
            Dim iQuantity As Int32 = 0
            For Each s In arr
                iQuantity += CType(s, Int32)
            Next
            Return iQuantity
        End If
        Return 0
    End Function

    Private Function GetMaxQuantity(ByVal sQuantity As String, ByVal iDivision As Byte) As Int32
        If sQuantity = String.Empty Then Return 0
        sQuantity = sQuantity.Trim().Replace("--", "-")
        Dim arr() As String = sQuantity.Split("-")
        If arr.Length > 0 Then
            If (iDivision Mod 2) = 0 Then 'trường hợp chẵn: 2,4,6
                Return If(arr.Max() > 5, arr.Max(), arr.Max() * 2)
            Else 'trường hợp lẽ: 1,3,5
                Dim lst1 As New List(Of Int32)
                For k As Byte = 0 To arr.Length - 1
                    If k = iDivision \ 2 Then
                        lst1.Add(arr(k) * 2)
                    Else
                        lst1.Add(arr(k))
                    End If
                Next
                Return If(lst1.Max() > 5, lst1.Max(), lst1.Max() * 2)
            End If
        End If
        Return 0
    End Function

    Private Function GetQuantityShift(ByVal shiftOption As Int32, ByVal sQuantity As String) As Int32
        If sQuantity = String.Empty Then Return 0
        sQuantity = sQuantity.Trim().Replace("--", "-")
        Dim arr() As String = sQuantity.Split("-")
        If arr.Length > 0 Then
            Return arr(shiftOption - 1)
        End If
        Return 0
    End Function

    Private Sub SumQuantityByRow()
        If gridTotal.Rows.Count = 0 Then Exit Sub
        Dim iTotal As Int32 = 0
        For Each r As DataGridViewRow In gridTotal.Rows
            iTotal += IIf(r.Cells("Total").Value Is DBNull.Value, 0, r.Cells("Total").Value)
        Next
        lblTotal.Text = iTotal.ToString()
    End Sub

    Private Sub SumQuantityByColumn()
        If gridTotal.Rows.Count = 0 Then Exit Sub
        Dim iShift1 As Int32 = 0
        Dim iShift2 As Int32 = 0
        Dim iShift3 As Int32 = 0
        For Each r As DataGridViewRow In gridTotal.Rows
            iShift1 += IIf(r.Cells("Shift1").Value Is DBNull.Value, 0, r.Cells("Shift1").Value)
            iShift2 += IIf(r.Cells("Shift2").Value Is DBNull.Value, 0, r.Cells("Shift2").Value)
            iShift3 += IIf(r.Cells("Shift3").Value Is DBNull.Value, 0, r.Cells("Shift3").Value)
        Next
        lbl1.Text = iShift1.ToString()
        lbl2.Text = iShift2.ToString()
        lbl3.Text = iShift3.ToString()
    End Sub

    Private Sub AddDayRowToGridTotal()
        If gridTotal IsNot Nothing Then gridTotal.DataSource = Nothing

        'Khởi tạo bảng
        Dim tbl As New DataTable
        Dim col1 As New DataColumn("WorkingDate", Type.GetType("System.DateTime"))
        Dim col2 As New DataColumn("Shift1", Type.GetType("System.Int32"))
        Dim col3 As New DataColumn("Shift2", Type.GetType("System.Int32"))
        Dim col4 As New DataColumn("Shift3", Type.GetType("System.Int32"))
        Dim col5 As New DataColumn("Total", Type.GetType("System.Int32"))
        tbl.Columns.AddRange({col1, col2, col3, col4, col5})

        Dim fromDate As DateTime = dtpFromDate.Value.Date
        Dim toDate As DateTime = dtpToDate.Value.Date

        While fromDate <= toDate
            Dim rAdd As DataRow = tbl.NewRow()
            rAdd("WorkingDate") = fromDate
            tbl.Rows.Add(rAdd)
            fromDate = fromDate.AddDays(1)
        End While

        gridTotal.AutoGenerateColumns = False
        gridTotal.DataSource = tbl

        'Tô màu dòng ngày nghỉ
        Dim lstRestDate As List(Of DataRow) = clsUtil.GetRestDate(DB)
        For Each r As DataGridViewRow In gridTotal.Rows
            Dim oRestDate = (From p In lstRestDate Where p("RestDate") = r.Cells("WorkingDate").Value
                                             Select p("RestDate")).FirstOrDefault()
            If oRestDate IsNot Nothing Then
                r.DefaultCellStyle.ForeColor = Color.Orange
            End If
        Next
    End Sub

    Private Sub CalQuantityOnGridTotal()
        'Reset bảng total
        Dim tbl As DataTable = CType(gridTotal.DataSource, DataTable)
        For Each r As DataRow In tbl.Rows
            r("Shift1") = DBNull.Value
            r("Shift2") = DBNull.Value
            r("Shift3") = DBNull.Value
            r("Total") = DBNull.Value
        Next

        If tblDailyOutputPlanning.Rows.Count = 0 Then Exit Sub

        'Duyệt bảng output -> tính ra số lượng lô xuất mỗi ngày/mỗi ca
        For Each r As DataRow In tblDailyOutputPlanning.Rows
            For Each c As DataColumn In tblDailyOutputPlanning.Columns
                If c.ColumnName <> "ProductCode" _
                    And c.ColumnName <> "StartLot" _
                    And c.ColumnName <> "LeadTimeID" _
                    And c.ColumnName <> "ProductCode_K" _
                    And c.ColumnName <> "CustomerName" _
                    And c.ColumnName <> "EndLot" Then

                    Dim planningDate As DateTime = DateTime.ParseExact(c.ColumnName, "dd-MMM-yy", CultureInfo.InvariantCulture)
                    Dim rs() As DataRow = tbl.Select(String.Format("WorkingDate='{0}'", planningDate.ToString("yyyy-MM-dd")))

                    If rs.Length > 0 Then
                        If r(c.ColumnName) Is DBNull.Value Then
                            rs(0)("Shift1") = IIf(rs(0)("Shift1") Is DBNull.Value, 0, rs(0)("Shift1")) + 0
                            rs(0)("Shift2") = IIf(rs(0)("Shift2") Is DBNull.Value, 0, rs(0)("Shift2")) + 0
                            rs(0)("Shift3") = IIf(rs(0)("Shift3") Is DBNull.Value, 0, rs(0)("Shift3")) + 0
                            rs(0)("Total") = IIf(rs(0)("Total") Is DBNull.Value, 0, rs(0)("Total")) + 0
                        Else
                            rs(0)("Shift1") = IIf(rs(0)("Shift1") Is DBNull.Value, 0, rs(0)("Shift1")) + GetQuantityShift(1, r(c.ColumnName))
                            rs(0)("Shift2") = IIf(rs(0)("Shift2") Is DBNull.Value, 0, rs(0)("Shift2")) + GetQuantityShift(2, r(c.ColumnName))
                            rs(0)("Shift3") = IIf(rs(0)("Shift3") Is DBNull.Value, 0, rs(0)("Shift3")) + GetQuantityShift(3, r(c.ColumnName))
                            rs(0)("Total") = IIf(rs(0)("Total") Is DBNull.Value, 0, rs(0)("Total")) + GetQuantityDay(r(c.ColumnName))
                        End If
                    End If
                End If
            Next
        Next
        SumQuantityByRow()
        SumQuantityByColumn()
    End Sub

    Private Sub CalEndLot(ByRef r As DataGridViewRow)

        Dim iLotTotal As Int32 = 0

        For Each c As DataGridViewColumn In gridDailyOutputPlanning.Columns
            If c.Name = "ProductCode" _
                Or c.Name = "ProductCode_K" _
                Or c.Name = "CustomerName" _
                Or c.Name = "StartLot" _
                Or c.Name = "LeadTimeID" _
                Or c.Name = "EndLot" _
                Or c.Name = "Tick" Then
                Continue For
            End If

            iLotTotal += If(r.Cells(c.Name).Value Is DBNull.Value, 0, GetQuantityDay(r.Cells(c.Name).Value))
        Next

        'Tính giá trị EndLot
        If r.Cells("StartLot").Value IsNot DBNull.Value Then
            Dim iStartLot As Int32 = If(r.Cells("StartLot").Value = String.Empty, 0, CType(r.Cells("StartLot").Value, Int32))
            If iStartLot > 0 Then
                r.Cells("EndLot").Value = ((iStartLot + iLotTotal) - 1).ToString()
            End If
        End If
    End Sub

    Private Sub CreatePlanningDetail()

        'Khởi tạo excel
        Dim app As New Excel.Application
        Dim workBook As Excel.Workbook = Nothing
        Dim workSheet As Excel.Worksheet = Nothing
        Dim workRange As Excel.Range = Nothing
        app.Visible = True
        '-----------------------------------------

        Try
            'lấy dữ liệu thời gian chạy của ngày
            Dim tblWT As DataTable = DB.FillDataTable("select ID, WorkingDay, RestDay from PD_ConfigWT")
            Dim iWorkingDay As Int32 = tblWT.Rows(0)("WorkingDay")
            Dim iRestDay As Int32 = tblWT.Rows(0)("RestDay")
            '-----------------------------------

            Dim lstRestDate As List(Of DataRow) = clsUtil.GetRestDate(DB)
            Dim lstDetail As List(Of DataRow) = Me._tblMain.AsEnumerable().ToList()

            Dim tv As New DataView(Me._tblMain)
            Dim tb As DataTable = tv.ToTable(True, "CustomerName")

            'Duyệt khách hàng
            For Each rCus As DataRow In tb.Rows

                Dim sCusNameOfDetail As String = rCus(0)

                'Chép file excel mẫu
                Dim sSourcePath As String = String.Format(clsUtil.FileTmp + "{0}.xlsx", sCusNameOfDetail)
                Dim sDestPath As String = String.Format(clsUtil.FileExp + "{0}.xlsx", sCusNameOfDetail)

                clsUtil.UpLoadFile(sSourcePath, sDestPath, True)

                If File.Exists(sDestPath) Then
                    workBook = app.Workbooks.Open(sDestPath, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing)
                Else
                    MessageBox.Show(String.Format("Template file {0} does not exist", sCusNameOfDetail), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                Dim lstByCustomer As List(Of DataRow) = lstDetail.Where(Function(p) p("CustomerName") = sCusNameOfDetail).ToList()

                Dim bIsTotalFileOpened As Boolean = CheckFileOpened(clsUtil.FileExp + String.Format("{0}.xlsx", "_TOTAL"))
                Dim bIsDblSideFileOpened As Boolean = CheckFileOpened(clsUtil.FileExp + String.Format("{0}.xlsx", "_DOUBLESIDE_PLAN"))

                For Each item In lstByCustomer
                    'Lấy LeadTime
                    Dim lstPdLeadTime As List(Of DataRow) = DB.FillDataTable(String.Format("SELECT * FROM {0} WHERE LeadTimeID = '{1}' ORDER BY Idx", PublicTable.Table_PD_ProcessLeadTime, item("LeadTimeID"))).AsEnumerable().ToList()
                    If lstPdLeadTime.Count = 0 Then Continue For
                    lstPdLeadTime.RemoveAt(lstPdLeadTime.Count - 1)

                    workSheet = workBook.Worksheets.Add(After:=workBook.Sheets(workBook.Sheets.Count))
                    workSheet.Name = item("ProductCode")

                    'Đóng băng
                    workSheet.Application.ActiveWindow.SplitColumn = 9
                    workSheet.Application.ActiveWindow.SplitRow = 2
                    workSheet.Application.ActiveWindow.FreezePanes = True

                    'Tính số dòng lớn nhất trên 1 công đoạn
                    Dim iProcessMaxWidth As Int32 = GetProcessMaxWidth(item)
                    If iProcessMaxWidth = 0 Then Continue For
                    iProcessMaxWidth += 1

                    'Lấy danh sách công đoạn
                    Dim tblPrcErp As DataTable = DB.FillDataTable(String.Format("SELECT ProcessCode, ProcessNumber = ISNULL(ProcessNumber,''), ProcessName AS ProcessNameE FROM {0} WHERE LeadTimeID = '{1}' ORDER BY Idx", PublicTable.Table_PD_ProcessLeadTime, item("LeadTimeID")))
                    'Dim tblPrcFpc As DataTable = dbFpics.FillDataTable("SELECT ProcessCode, ProcessNameE FROM dbo.m_Process")

                    'Dim qrProcess = From r In tblPrcErp
                    '                Group Join ms In tblPrcFpc On r("ProcessCode") Equals ms("ProcessCode") Into g = Group
                    '                From e In g.DefaultIfEmpty()
                    '                Select New With {
                    '                    .ProcessCode = r("ProcessCode"),
                    '                    .ProcessNameE = If(e IsNot Nothing, e("ProcessNameE"), String.Empty),
                    '                    .ProcessNumber = r("ProcessNumber")
                    '                }

                    Dim lstProcess As DataTable = tblPrcErp.Copy() 'clsUtil.CopyToDataTable(qrProcess)

                    If lstProcess.Rows.Count = 0 Then
                        Continue For
                    End If

                    'Vẽ header
                    workSheet.Cells(1, 1) = "'Pn"
                    workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(2, 1))
                    workRange.Merge()

                    workSheet.Cells(1, 2) = "'PcCode"
                    workRange = workSheet.Range(workSheet.Cells(1, 2), workSheet.Cells(2, 2))
                    workRange.Merge()

                    workSheet.Cells(1, 3) = "'PcName"
                    workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(2, 3))
                    workRange.Merge()

                    workSheet.Cells(1, 4) = "'LAST WEEK PLAN"
                    workRange = workSheet.Range(workSheet.Cells(1, 4), workSheet.Cells(2, 9))
                    workRange.Merge()

                    Dim fromDate As DateTime = dtpFromDate.Value.Date
                    Dim toDate As DateTime = dtpToDate.Value.Date

                    Dim iCol As Int32 = 10

                    While fromDate <= toDate
                        'Cột tên ngày
                        workSheet.Cells(1, iCol) = fromDate.ToString("dd-MMM-yy")
                        workRange = workSheet.Range(workSheet.Cells(1, iCol), workSheet.Cells(1, iCol + 5))
                        workRange.Merge()

                        'Cột tên thứ
                        workSheet.Cells(2, iCol) = fromDate.DayOfWeek.ToString().Substring(0, 3)
                        workRange = workSheet.Range(workSheet.Cells(2, iCol), workSheet.Cells(2, iCol + 5))
                        workRange.Merge()

                        fromDate = fromDate.AddDays(1)
                        iCol += 6
                    End While

                    FormatAllBorders(workRange, workSheet, workSheet.Cells(1, 1), workSheet.Cells(2, iCol - 1))
                    Dim iEndCol As Int32 = iCol - 1

                    'Vẽ khung công đoạn
                    Dim iRow As Int32 = 3
                    For Each pc As DataRow In lstProcess.Rows
                        iCol = 1
                        While iCol <= 3
                            workSheet.Cells(iRow, iCol) = "'" + If(iCol = 1, pc("ProcessNumber"), If(iCol = 2, pc("ProcessCode"), pc("ProcessNameE")))
                            workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iCol))
                            workRange.Merge()
                            iCol += 1
                        End While
                        If pc("ProcessNumber") = String.Empty Then
                            workRange = workSheet.Range(workSheet.Cells(iRow, 1), workSheet.Cells(iRow + iProcessMaxWidth - 1, iEndCol))
                            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue)
                        End If
                        iRow += iProcessMaxWidth
                    Next

                    FormatAllBorders(workRange, workSheet, workSheet.Cells(1, 1), workSheet.Cells(iRow - 1, 3))

                    FormatDetail(workRange, workSheet, workSheet.Cells(3, 4), workSheet.Cells(iRow - 1, iEndCol), iProcessMaxWidth, iRow - 1, iEndCol)

                    'Thiết lập độ rộng cột ProcessName
                    workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(iRow - 1, 3))
                    workRange.ColumnWidth = 15
                    workRange.WrapText = True

                    'Thiết lập độ rộng cột lô
                    workRange = workSheet.Range(workSheet.Cells(1, 4), workSheet.Cells(iRow - 1, iEndCol))
                    workRange.ColumnWidth = 4

                    'Tô màu phần LAST WEEK PLAN
                    workRange = workSheet.Range(workSheet.Cells(1, 4), workSheet.Cells(iRow - 1, 9))
                    workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightPink)

                    'Rải lô gia công ở công đoạn kiểm phẩm
                    fromDate = dtpFromDate.Value.Date
                    toDate = dtpToDate.Value.Date
                    Dim startLot As Int32 = CType(item("StartLot"), Int32)

                    iRow = iRow - iProcessMaxWidth
                    iCol = 10
                    While fromDate <= toDate

                        'Tô màu nếu là ngày nghỉ
                        Dim oRestDate = (From p In lstRestDate Where p("RestDate") = fromDate
                                            Select p("RestDate")).FirstOrDefault()
                        Dim sRestDate As String = If(oRestDate Is Nothing, String.Empty, CType(oRestDate, DateTime).ToString("yyyy-MM-dd"))
                        If sRestDate <> String.Empty Then
                            workRange = workSheet.Range(workSheet.Cells(1, iCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iCol + 5))
                            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Orange)
                        End If

                        Dim sColName As String = fromDate.ToString("dd-MMM-yy")

                        If item(sColName) IsNot DBNull.Value Then
                            Dim iQty As Int32 = GetQuantityDay(item(sColName))

                            If iQty = 0 Then
                                iCol += 6
                            Else
                                Dim fromLot As Int32 = startLot

                                Dim arr(2) As Int32
                                arr(0) = GetQuantityShift(1, item(sColName))
                                arr(1) = GetQuantityShift(2, item(sColName))
                                arr(2) = GetQuantityShift(3, item(sColName))

                                Dim arrLotList(5) As Int32
                                Dim j As Int32 = 0

                                'If sRestDate = String.Empty Then
                                '    For Each a As Int32 In arr
                                '        If a > 5 Then
                                '            arrLotList(j) = (a \ 2) + (a Mod 2)
                                '            arrLotList(j + 1) = (a \ 2)
                                '        Else
                                '            arrLotList(j) = a
                                '        End If
                                '        j += 2
                                '    Next
                                'Else
                                'Lấy thời gian làm việc
                                Dim iWorkingHour As Byte = 0
                                Dim oWorkingHour As Object = Nothing

                                If sRestDate = String.Empty Then 'ngày làm việc bình thường
                                    iWorkingHour = iWorkingDay
                                Else 'ngày nghỉ
                                    oWorkingHour = DB.ExecuteScalar(String.Format("SELECT WorkingHour FROM dbo.PD_DailyOutputPlanning_1 D LEFT JOIN dbo.PD_MsCustomer C ON D.CustomerCode = C.CustomerCode WHERE C.CustomerName = '{0}' AND ProductCode = '{1}' And PlanningDate='{2}'", _
                                                                                    item("CustomerName"), item("ProductCode"), sRestDate))
                                    iWorkingHour = If(oWorkingHour Is Nothing, 0, iRestDay)
                                    'iWorkingHour = 0 => nghĩa là ngày nghỉ này không có gia công
                                End If

                                Dim iDivision As Byte = iWorkingHour / 4
                                If (iDivision Mod 2) = 0 Then
                                    For Each a As Int32 In arr
                                        If a > 5 Then
                                            arrLotList(j) = (a \ 2) + (a Mod 2)
                                            arrLotList(j + 1) = (a \ 2)
                                        Else
                                            arrLotList(j) = a
                                        End If
                                        j += 2
                                    Next
                                Else
                                    For k As Byte = 0 To arr.Length - 1
                                        If k = iDivision \ 2 Then
                                            arrLotList(j) = arr(k)
                                            arrLotList(j + 1) = 0
                                        Else
                                            If arr(k) > 5 Then
                                                arrLotList(j) = (arr(k) \ 2) + (arr(k) Mod 2)
                                                arrLotList(j + 1) = (arr(k) \ 2)
                                            Else
                                                arrLotList(j) = arr(k)
                                            End If
                                        End If
                                        j += 2
                                    Next
                                End If
                                'End If

                                'Gán số lô dựa trên arrLotList
                                For Each b As Int32 In arrLotList
                                    If b = 0 Then
                                        iCol += 1
                                        Continue For
                                    End If
                                    Dim idx As Int32 = iRow
                                    j = 1
                                    While j <= b
                                        workSheet.Cells(idx, iCol) = fromLot
                                        j += 1
                                        fromLot += 1
                                        idx += 1
                                    End While
                                    iCol += 1
                                Next

                                startLot = startLot + iQty
                            End If
                        Else
                            iCol += 6
                        End If

                        'Tăng biến đếm ngày
                        fromDate = fromDate.AddDays(1)
                    End While

                    'Thiết lập công thức ngược về file _TOTAL cho công đoạn kiểm phẩm
                    If rdPlanTotal.Checked Or rdAllPlan.Checked Then SetFormulaTotal(app, bIsTotalFileOpened, sCusNameOfDetail, item, iEndCol, workSheet, iRow, iProcessMaxWidth)

                    'Active file đang chạy
                    app.Workbooks(sCusNameOfDetail + ".xlsx").Activate()

                    'Rải lô gia công ngược về công đoạn bắt đầu
                    iEndCol = iEndCol + 1

                    '***Tạo 1 list chứa vị trí cột có gia công hay không/****/
                    Dim lstColWork As New List(Of String)
                    iCol = 10

                    While iCol < iEndCol
                        Dim sValue As String = workSheet.Range(workSheet.Cells(1, iCol), workSheet.Cells(1, iCol)).Value
                        Dim oRestDate = (From p In lstRestDate Where p("RestDate") = sValue
                                         Select p("RestDate")).FirstOrDefault()
                        Dim sRestDate As String = If(oRestDate Is Nothing, String.Empty, CType(oRestDate, DateTime).ToString("yyyy-MM-dd"))

                        Dim iDivision As Byte = 6

                        Dim iWorkingHour As Byte = 0
                        Dim oWorkingHour As Object = Nothing

                        If sRestDate = String.Empty Then 'ngày làm việc bình thường
                            iWorkingHour = iWorkingDay
                        Else 'ngày nghỉ
                            oWorkingHour = DB.ExecuteScalar(String.Format("SELECT WorkingHour FROM dbo.PD_DailyOutputPlanning_1 D LEFT JOIN dbo.PD_MsCustomer C ON D.CustomerCode = C.CustomerCode WHERE C.CustomerName = '{0}' AND ProductCode = '{1}' And PlanningDate='{2}'", _
                                                                                 item("CustomerName"), item("ProductCode"), CType(sValue, DateTime).ToString("yyyy-MM-dd")))
                            iWorkingHour = If(oWorkingHour Is Nothing, 0, iRestDay)
                            'Nếu iWorkingHour = 0 => nghĩa là ngày nghỉ ngày không có gia công
                        End If

                        iDivision = iWorkingHour / 4
                        For i As Int32 = 1 To 6
                            If i <= iDivision Then : lstColWork.Add((iCol + i - 1).ToString() + "-1")
                            Else : lstColWork.Add((iCol + i - 1).ToString() + "-0")
                            End If
                        Next
                        iCol += 6
                    End While
                    '***

                    'Khai báo danh sách lưu vị trí các công đoạn phụ
                    Dim lstSubProcess As New List(Of DataRow)
                    Dim tblSubProcess As New DataTable
                    Dim c1 As New DataColumn("ProcessCode", Type.GetType("System.String"))
                    Dim c2 As New DataColumn("Idx", Type.GetType("System.Int32"))
                    tblSubProcess.Columns.AddRange({c1, c2})

                    ''
                    'Biến lưu giữ nhóm công đoạn
                    Dim lstPrcGroup As New List(Of Int32)

                    'Rải lô cho các công đoạn chính
                    Dim iRowR As Int32 = iRow 'Dòng copy
                    While iRow > 3
                        iRow = iRow - iProcessMaxWidth 'Dòng paste

                        'Lấy mã công đoạn hiện tại
                        Dim sPn As String = workSheet.Range(workSheet.Cells(iRow, 1), workSheet.Cells(iRow, 1)).Value
                        Dim sPrcCode As String = workSheet.Range(workSheet.Cells(iRow, 2), workSheet.Cells(iRow, 2)).Value

                        'Ghi nhận những công đoạn phụ cân rải lô & vị trí của nó
                        'Lưu ý mã 8041
                        If sPn.Length = 0 Then
                            If sPrcCode = "8047" _
                                Or sPrcCode = "1003" _
                                Or sPrcCode = "8041" _
                                Or sPrcCode = "8045" _
                                Or sPrcCode = "9005" Then
                                Dim oExist As Object = (From p In lstSubProcess Where p("ProcessCode") = sPrcCode
                                            Select p("ProcessCode")).FirstOrDefault()
                                If oExist Is Nothing Then
                                    Dim rAdd As DataRow = tblSubProcess.NewRow()
                                    rAdd("ProcessCode") = workSheet.Range(workSheet.Cells(iRow, 2), workSheet.Cells(iRow, 2)).Value
                                    rAdd("Idx") = iRow
                                    lstSubProcess.Add(rAdd)
                                End If
                            End If
                            Continue While
                        End If

                        'Công đoạn đột lỗ định vị
                        If sPrcCode = "4126" _
                            Or sPrcCode = "4026" _
                            Or sPrcCode = "9038" Then
                            Dim rAdd As DataRow = tblSubProcess.NewRow()
                            rAdd("ProcessCode") = workSheet.Range(workSheet.Cells(iRow, 2), workSheet.Cells(iRow, 2)).Value
                            rAdd("Idx") = iRow
                            lstSubProcess.Add(rAdd)
                        End If

                        ''Lấy thời gian lệch công đoạn
                        Dim iLeadTime As Int32 = 0
                        For i As Int32 = lstPdLeadTime.Count - 1 To 0 Step -1
                            Dim r As DataRow = lstPdLeadTime(i)
                            If r("ProcessCode") = sPrcCode Then
                                'Tìm trong lstPrcGroup
                                Dim id As Int32 = r("ProcessGroup")
                                Dim sPrcGroup As List(Of Int32) = lstPrcGroup.Where(Function(s) s = id).ToList()
                                'Nếu không tìm thấy nghĩa là đổi group -> clear
                                If sPrcGroup.Count = 0 Then
                                    lstPrcGroup.Clear()
                                End If
                                'Add nhóm
                                lstPrcGroup.Add(id)
                                iLeadTime = If(lstPrcGroup.Count = 1, r("LeadTime"), 0)
                                For j As Int32 = lstPdLeadTime.Count - 1 To i Step -1
                                    lstPdLeadTime.RemoveAt(j)
                                Next
                                Exit For
                            End If
                        Next

                        iLeadTime = iLeadTime / 4

                        '***Bắt đầu chạy lô/****/
                        'Cột paste
                        Dim iFromCol = 10
                        Dim iToCol = 10

                        'Cột copy
                        Dim iFromColR As Int32 = 10
                        Dim iToColR As Int32 = 10

                        'Xử lý trường hợp leadtime = 0
                        If iLeadTime = 0 Then
                            'Copy
                            workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.Copy(Type.Missing)
                            'Paste
                            workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        Else
                            'Tìm vị trí copy tương ứng với leadtime
                            iLeadTime += 1
                            Dim iCount As Int32 = 0
                            While iFromColR < iEndCol
                                'Kiểm tra iFromColR ở 2 giá trị: -0 & -1
                                Dim sFromColR As String = iFromColR.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault() IsNot Nothing Then iCount += 1
                                If iCount = iLeadTime Then : Exit While
                                Else : iFromColR += 1
                                End If
                            End While
                            iToColR = iFromColR

                            'Tìm vị trí paste đầu tiên
                            While iFromCol < iEndCol
                                'Kiểm tra iFromCol ở 2 giá trị: -0 & -1
                                Dim sFromCol As String = iFromCol.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault() IsNot Nothing Then
                                    Exit While
                                End If
                                iFromCol += 1
                            End While
                            iToCol = iFromCol

                            If iFromCol >= iEndCol Or iFromColR >= iEndCol Then Continue While

                            'Duyệt vị trí copy & paste tương ứng với ngày làm việc/ngày nghỉ
                            Dim bEnd As Boolean = False

                            Dim i As Int32 = iFromColR 'Biến giữ vị trí copy
                            Dim j As Int32 = iFromCol  'Biến giữ vị trí paste

                            While Not bEnd

                                Dim sFromColR As String = i.ToString() + "-1"
                                Dim _existFromR = lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault()

                                Dim sFromCol As String = j.ToString() + "-1"
                                Dim _existFrom = lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault()

                                If _existFromR IsNot Nothing And _existFrom IsNot Nothing Then 'Copy đi làm & Paste đi làm
                                    'Tăng biến copy
                                    iToColR = i
                                    i += 1
                                    'Tăng biến paste
                                    iToCol = j
                                    j += 1
                                ElseIf _existFromR IsNot Nothing And _existFrom Is Nothing Then 'Copy đi làm & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị i
                                    i = iToColR + 1
                                    iFromColR = iToColR + 1

                                    '& Tìm lại vị trí paste
                                    While j < iEndCol
                                        Dim sValue As String = j.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j += 1
                                    End While

                                    iFromCol = j
                                    iToCol = j

                                ElseIf _existFromR Is Nothing And _existFrom IsNot Nothing Then 'Copy nghỉ & Paste làm
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị j
                                    j = iToCol + 1
                                    iFromCol = iToCol + 1

                                    '& Tìm lại vị trí copy
                                    While i < iEndCol
                                        Dim sValue As String = i.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i += 1
                                    End While

                                    iFromColR = i
                                    iToColR = i

                                ElseIf _existFromR Is Nothing And _existFrom Is Nothing Then 'Copy nghỉ & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)
                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    '& Tìm vị trí paste mới
                                    While j < iEndCol
                                        Dim sValue As String = j.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j += 1
                                    End While

                                    iFromCol = j
                                    iToCol = j

                                    '& vị trí copy mới
                                    While i < iEndCol
                                        Dim sValue As String = i.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i += 1
                                    End While

                                    iFromColR = i
                                    iToColR = i

                                End If

                                'Copy phần còn lại
                                If iToColR = iEndCol Then
                                    'Kết thúc
                                    bEnd = True
                                End If

                            End While
                        End If
                        '***/

                        'Sau khi rải xong lô của 1 công đoạn -> thiết lập công thức tính ngược về file tổng
                        If rdPlanTotal.Checked Or rdAllPlan.Checked Then SetFormulaTotal(app, bIsTotalFileOpened, sCusNameOfDetail, item, iEndCol, workSheet, iRow, iProcessMaxWidth)

                        'Sau khi rải xong lô của 1 công đoạn -> thiết lập công thức tính ngược về file plan hàng 2 mặt
                        If rdPlanDblSide.Checked Or rdAllPlan.Checked Then SetFormulaDoubleSide(app, bIsDblSideFileOpened, sCusNameOfDetail, item, iEndCol, workSheet, iRow, iProcessMaxWidth)

                        'Active file đang chạy
                        app.Workbooks(sCusNameOfDetail + ".xlsx").Activate()
                        iRowR = iRow
                    End While
                    '***Kết thúc rải lô công đoạn chính

                    '***Rải lô cho các công đoạn phụ

                    lstPdLeadTime = DB.FillDataTable(String.Format("SELECT * FROM {0} WHERE LeadTimeID = '{1}' ORDER BY Idx", PublicTable.Table_PD_ProcessLeadTime, item("LeadTimeID"))).AsEnumerable().ToList()
                    If lstPdLeadTime.Count = 0 Then Continue For

                    iRowR = lstSubProcess(0)("Idx") + iProcessMaxWidth 'Dòng copy
                    For i As Int32 = 0 To lstSubProcess.Count - 1

                        If lstSubProcess(i)("ProcessCode") = "4126" _
                            Or lstSubProcess(i)("ProcessCode") = "4026" _
                            Or lstSubProcess(i)("ProcessCode") = "9038" Then
                            Continue For
                        End If

                        iRow = lstSubProcess(i)("Idx") 'Dòng paste

                        'Nếu là mã 8041 thì = với đột lỗ định vị -> lấy lại dòng cần copy là mã 4126/4026
                        If lstSubProcess(i)("ProcessCode") = "8041" Then
                            iRowR = (From p In lstSubProcess Where p("ProcessCode") = "4126" Or p("ProcessCode") = "4026"
                                                    Select p("Idx")).FirstOrDefault()
                        ElseIf lstSubProcess(i)("ProcessCode") = "8045" Then 'công đoạn này lệch với Tape Mask 24h
                            iRowR = (From p In lstSubProcess Where p("ProcessCode") = "9038"
                                                    Select p("Idx")).FirstOrDefault()
                        End If

                        ''Lấy mã công đoạn hiện tại
                        Dim sPrcCode As String = lstSubProcess(i)("ProcessCode")

                        ''Lấy thời gian lệch công đoạn
                        Dim iLeadTime As Int32 = 0
                        For j As Int32 = lstPdLeadTime.Count - 1 To 0 Step -1
                            Dim r As DataRow = lstPdLeadTime(j)
                            If r("ProcessCode") = sPrcCode Then
                                iLeadTime = r("LeadTime")
                                lstPdLeadTime.Remove(r)
                                Exit For
                            Else
                                lstPdLeadTime.Remove(r)
                            End If
                        Next

                        iLeadTime = iLeadTime / 4

                        '****Bắt đầu chạy lô
                        'Cột paste
                        Dim iFromCol = 10
                        Dim iToCol = 10

                        'Cột copy
                        Dim iFromColR As Int32 = 10
                        Dim iToColR As Int32 = 10

                        'Xử lý trường hợp leadtime = 0
                        If iLeadTime = 0 Then
                            'Copy
                            workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.Copy(Type.Missing)
                            'Paste
                            workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        Else
                            'Tìm vị trí copy tương ứng với leadtime
                            iLeadTime += 1
                            Dim iCount As Int32 = 0
                            While iFromColR < iEndCol
                                'Kiểm tra iFromColR ở 2 giá trị: -0 & -1
                                Dim sFromColR As String = iFromColR.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault() IsNot Nothing Then iCount += 1
                                If iCount = iLeadTime Then : Exit While
                                Else : iFromColR += 1
                                End If
                            End While
                            iToColR = iFromColR

                            'Tìm vị trí paste đầu tiên
                            While iFromCol < iEndCol
                                'Kiểm tra iFromCol ở 2 giá trị: -0 & -1
                                Dim sFromCol As String = iFromCol.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault() IsNot Nothing Then
                                    Exit While
                                End If
                                iFromCol += 1
                            End While
                            iToCol = iFromCol

                            If iFromCol >= iEndCol Or iFromColR >= iEndCol Then Continue For

                            'Duyệt vị trí copy & paste tương ứng với ngày làm việc/ngày nghỉ
                            Dim bEnd As Boolean = False

                            Dim i_ As Int32 = iFromColR 'Biến giữ vị trí copy
                            Dim j_ As Int32 = iFromCol  'Biến giữ vị trí paste

                            While Not bEnd

                                Dim sFromColR As String = i_.ToString() + "-1"
                                Dim _existFromR = lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault()

                                Dim sFromCol As String = j_.ToString() + "-1"
                                Dim _existFrom = lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault()

                                If _existFromR IsNot Nothing And _existFrom IsNot Nothing Then 'Copy đi làm & Paste đi làm
                                    'Tăng biến copy
                                    iToColR = i_
                                    i_ += 1
                                    'Tăng biến paste
                                    iToCol = j_
                                    j_ += 1
                                ElseIf _existFromR IsNot Nothing And _existFrom Is Nothing Then 'Copy đi làm & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị i
                                    i_ = iToColR + 1
                                    iFromColR = iToColR + 1

                                    '& Tìm lại vị trí paste
                                    While j_ < iEndCol
                                        Dim sValue As String = j_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j_ += 1
                                    End While

                                    iFromCol = j_
                                    iToCol = j_

                                ElseIf _existFromR Is Nothing And _existFrom IsNot Nothing Then 'Copy nghỉ & Paste làm
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị j
                                    j_ = iToCol + 1
                                    iFromCol = iToCol + 1

                                    '& Tìm lại vị trí copy
                                    While i_ < iEndCol
                                        Dim sValue As String = i_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i_ += 1
                                    End While

                                    iFromColR = i_
                                    iToColR = i_

                                ElseIf _existFromR Is Nothing And _existFrom Is Nothing Then 'Copy nghỉ & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)
                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    '& Tìm vị trí paste mới
                                    While j_ < iEndCol
                                        Dim sValue As String = j_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j_ += 1
                                    End While

                                    iFromCol = j_
                                    iToCol = j_

                                    '& vị trí copy mới
                                    While i_ < iEndCol
                                        Dim sValue As String = i_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i_ += 1
                                    End While

                                    iFromColR = i_
                                    iToColR = i_

                                End If

                                'Copy phần còn lại
                                If iToColR = iEndCol Then
                                    'Kết thúc
                                    bEnd = True
                                End If

                            End While
                        End If
                        '****/

                        'Sau khi rải xong lô của 1 công đoạn -> thiết lập công thức tính ngược về file tổng
                        If rdPlanTotal.Checked Or rdAllPlan.Checked Then SetFormulaTotal(app, bIsTotalFileOpened, sCusNameOfDetail, item, iEndCol, workSheet, iRow, iProcessMaxWidth)

                        'Active file đang chạy
                        app.Workbooks(sCusNameOfDetail + ".xlsx").Activate()
                        iRowR = iRow
                    Next
                    '***Kết thúc rải lô công đoạn phụ
                Next

                'Ẩn sheet1
                workSheet = workBook.Sheets(1)
                workSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Next

            SaveFileAndRelease(app, workRange, workSheet, workBook)

            MessageBox.Show("Lập kế hoạch sản xuất hoàn tất", "Information!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex, "Export planning detail", Me.Text)
        End Try
    End Sub

    Private Sub CreatePlanningDetail_BCK()

        'Khởi tạo excel
        Dim app As New Excel.Application
        Dim workBook As Excel.Workbook = Nothing
        Dim workSheet As Excel.Worksheet = Nothing
        Dim workRange As Excel.Range = Nothing
        app.Visible = True
        '-----------------------------------------

        Try
            'lấy dữ liệu thời gian chạy của ngày
            Dim tblWT As DataTable = DB.FillDataTable("select ID, WorkingDay, RestDay from PD_ConfigWT")
            Dim iWorkingDay As Int32 = tblWT.Rows(0)("WorkingDay")
            Dim iRestDay As Int32 = tblWT.Rows(0)("RestDay")
            '-----------------------------------

            Dim lstRestDate As List(Of DataRow) = clsUtil.GetRestDate(DB)
            Dim lstDetail As List(Of DataRow) = Me._tblMain.AsEnumerable().ToList()

            Dim tv As New DataView(Me._tblMain)
            Dim tb As DataTable = tv.ToTable(True, "CustomerName")

            'Duyệt khách hàng
            For Each rCus As DataRow In tb.Rows

                Dim sCusNameOfDetail As String = rCus(0)

                'Chép file excel mẫu
                Dim sSourcePath As String = String.Format(clsUtil.FileTmp + "{0}.xlsx", sCusNameOfDetail)
                Dim sDestPath As String = String.Format(clsUtil.FileExp + "{0}.xlsx", sCusNameOfDetail)

                clsUtil.UpLoadFile(sSourcePath, sDestPath, True)

                If File.Exists(sDestPath) Then
                    workBook = app.Workbooks.Open(sDestPath, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing)
                Else
                    MessageBox.Show(String.Format("Template file {0} does not exist", sCusNameOfDetail), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                Dim lstByCustomer As List(Of DataRow) = lstDetail.Where(Function(p) p("CustomerName") = sCusNameOfDetail).ToList()

                Dim bIsTotalFileOpened As Boolean = CheckFileOpened(clsUtil.FileExp + String.Format("{0}.xlsx", "_TOTAL"))

                For Each item In lstByCustomer
                    'Lấy LeadTime
                    Dim lstPdLeadTime As List(Of DataRow) = DB.FillDataTable(String.Format("SELECT * FROM {0} WHERE LeadTimeID = '{1}' ORDER BY Idx", PublicTable.Table_PD_ProcessLeadTime, item("LeadTimeID"))).AsEnumerable().ToList()
                    If lstPdLeadTime.Count = 0 Then Continue For
                    lstPdLeadTime.RemoveAt(lstPdLeadTime.Count - 1)

                    workSheet = workBook.Worksheets.Add(After:=workBook.Sheets(workBook.Sheets.Count))
                    workSheet.Name = item("ProductCode")

                    'Đóng băng
                    workSheet.Application.ActiveWindow.SplitColumn = 9
                    workSheet.Application.ActiveWindow.SplitRow = 2
                    workSheet.Application.ActiveWindow.FreezePanes = True

                    'Tính số dòng lớn nhất trên 1 công đoạn
                    Dim iProcessMaxWidth As Int32 = GetProcessMaxWidth(item)
                    If iProcessMaxWidth = 0 Then Continue For
                    iProcessMaxWidth += 1

                    'Lấy danh sách công đoạn
                    Dim tblPrcErp As DataTable = DB.FillDataTable(String.Format("SELECT ProcessCode, ProcessNumber = ISNULL(ProcessNumber,'') FROM {0} WHERE LeadTimeID = '{1}' ORDER BY Idx", PublicTable.Table_PD_ProcessLeadTime, item("LeadTimeID")))
                    Dim tblPrcFpc As DataTable = dbFpics.FillDataTable("SELECT ProcessCode, ProcessNameE FROM dbo.m_Process")

                    Dim qrProcess = From r In tblPrcErp
                                    Group Join ms In tblPrcFpc On r("ProcessCode") Equals ms("ProcessCode") Into g = Group
                                    From e In g.DefaultIfEmpty()
                                    Select New With {
                                        .ProcessCode = r("ProcessCode"),
                                        .ProcessNameE = If(e IsNot Nothing, e("ProcessNameE"), String.Empty),
                                        .ProcessNumber = r("ProcessNumber")
                                    }

                    Dim lstProcess As DataTable = clsUtil.CopyToDataTable(qrProcess)

                    If lstProcess.Rows.Count = 0 Then
                        Continue For
                    End If

                    'Vẽ header
                    workSheet.Cells(1, 1) = "'Pn"
                    workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(2, 1))
                    workRange.Merge()

                    workSheet.Cells(1, 2) = "'PcCode"
                    workRange = workSheet.Range(workSheet.Cells(1, 2), workSheet.Cells(2, 2))
                    workRange.Merge()

                    workSheet.Cells(1, 3) = "'PcName"
                    workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(2, 3))
                    workRange.Merge()

                    workSheet.Cells(1, 4) = "'LAST WEEK PLAN"
                    workRange = workSheet.Range(workSheet.Cells(1, 4), workSheet.Cells(2, 9))
                    workRange.Merge()

                    Dim fromDate As DateTime = dtpFromDate.Value.Date
                    Dim toDate As DateTime = dtpToDate.Value.Date

                    Dim iCol As Int32 = 10

                    While fromDate <= toDate
                        'Cột tên ngày
                        workSheet.Cells(1, iCol) = fromDate.ToString("dd-MMM-yy")
                        workRange = workSheet.Range(workSheet.Cells(1, iCol), workSheet.Cells(1, iCol + 5))
                        workRange.Merge()

                        'Cột tên thứ
                        workSheet.Cells(2, iCol) = fromDate.DayOfWeek.ToString().Substring(0, 3)
                        workRange = workSheet.Range(workSheet.Cells(2, iCol), workSheet.Cells(2, iCol + 5))
                        workRange.Merge()

                        fromDate = fromDate.AddDays(1)
                        iCol += 6
                    End While

                    FormatAllBorders(workRange, workSheet, workSheet.Cells(1, 1), workSheet.Cells(2, iCol - 1))
                    Dim iEndCol As Int32 = iCol - 1

                    'Vẽ khung công đoạn
                    Dim iRow As Int32 = 3
                    For Each pc As DataRow In lstProcess.Rows
                        iCol = 1
                        While iCol <= 3
                            workSheet.Cells(iRow, iCol) = "'" + If(iCol = 1, pc("ProcessNumber"), If(iCol = 2, pc("ProcessCode"), pc("ProcessNameE")))
                            workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iCol))
                            workRange.Merge()
                            iCol += 1
                        End While
                        If pc("ProcessNumber") = String.Empty Then
                            workRange = workSheet.Range(workSheet.Cells(iRow, 1), workSheet.Cells(iRow + iProcessMaxWidth - 1, iEndCol))
                            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue)
                        End If
                        iRow += iProcessMaxWidth
                    Next

                    FormatAllBorders(workRange, workSheet, workSheet.Cells(1, 1), workSheet.Cells(iRow - 1, 3))

                    FormatDetail(workRange, workSheet, workSheet.Cells(3, 4), workSheet.Cells(iRow - 1, iEndCol), iProcessMaxWidth, iRow - 1, iEndCol)

                    'Thiết lập độ rộng cột ProcessName
                    workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(iRow - 1, 3))
                    workRange.ColumnWidth = 15
                    workRange.WrapText = True

                    'Thiết lập độ rộng cột lô
                    workRange = workSheet.Range(workSheet.Cells(1, 4), workSheet.Cells(iRow - 1, iEndCol))
                    workRange.ColumnWidth = 4

                    'Tô màu phần LAST WEEK PLAN
                    workRange = workSheet.Range(workSheet.Cells(1, 4), workSheet.Cells(iRow - 1, 9))
                    workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightPink)

                    'Rải lô gia công ở công đoạn kiểm phẩm
                    fromDate = dtpFromDate.Value.Date
                    toDate = dtpToDate.Value.Date
                    Dim startLot As Int32 = CType(item("StartLot"), Int32)

                    iRow = iRow - iProcessMaxWidth
                    iCol = 10
                    While fromDate <= toDate

                        'Tô màu nếu là ngày nghỉ
                        Dim oRestDate = (From p In lstRestDate Where p("RestDate") = fromDate
                                            Select p("RestDate")).FirstOrDefault()
                        Dim sRestDate As String = If(oRestDate Is Nothing, String.Empty, CType(oRestDate, DateTime).ToString("yyyy-MM-dd"))
                        If sRestDate <> String.Empty Then
                            workRange = workSheet.Range(workSheet.Cells(1, iCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iCol + 5))
                            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Orange)
                        End If

                        Dim sColName As String = fromDate.ToString("dd-MMM-yy")

                        If item(sColName) IsNot DBNull.Value Then
                            Dim iQty As Int32 = GetQuantityDay(item(sColName))

                            If iQty = 0 Then
                                iCol += 6
                            Else
                                Dim fromLot As Int32 = startLot

                                Dim arr(2) As Int32
                                arr(0) = GetQuantityShift(1, item(sColName))
                                arr(1) = GetQuantityShift(2, item(sColName))
                                arr(2) = GetQuantityShift(3, item(sColName))

                                Dim arrLotList(5) As Int32
                                Dim j As Int32 = 0

                                'If sRestDate = String.Empty Then
                                '    For Each a As Int32 In arr
                                '        If a > 5 Then
                                '            arrLotList(j) = (a \ 2) + (a Mod 2)
                                '            arrLotList(j + 1) = (a \ 2)
                                '        Else
                                '            arrLotList(j) = a
                                '        End If
                                '        j += 2
                                '    Next
                                'Else
                                'Lấy thời gian làm việc
                                Dim iWorkingHour As Byte = 0
                                Dim oWorkingHour As Object = Nothing

                                If sRestDate = String.Empty Then 'ngày làm việc bình thường
                                    iWorkingHour = iWorkingDay
                                Else 'ngày nghỉ
                                    oWorkingHour = DB.ExecuteScalar(String.Format("SELECT WorkingHour FROM dbo.PD_DailyOutputPlanning_1 D LEFT JOIN dbo.PD_MsCustomer C ON D.CustomerCode = C.CustomerCode WHERE C.CustomerName = '{0}' AND ProductCode = '{1}' And PlanningDate='{2}'", _
                                                                                    item("CustomerName"), item("ProductCode"), sRestDate))
                                    iWorkingHour = If(oWorkingHour Is Nothing, 0, iRestDay)
                                    'iWorkingHour = 0 => nghĩa là ngày nghỉ này không có gia công
                                End If

                                Dim iDivision As Byte = iWorkingHour / 4
                                If (iDivision Mod 2) = 0 Then
                                    For Each a As Int32 In arr
                                        If a > 5 Then
                                            arrLotList(j) = (a \ 2) + (a Mod 2)
                                            arrLotList(j + 1) = (a \ 2)
                                        Else
                                            arrLotList(j) = a
                                        End If
                                        j += 2
                                    Next
                                Else
                                    For k As Byte = 0 To arr.Length - 1
                                        If k = iDivision \ 2 Then
                                            arrLotList(j) = arr(k)
                                            arrLotList(j + 1) = 0
                                        Else
                                            If arr(k) > 5 Then
                                                arrLotList(j) = (arr(k) \ 2) + (arr(k) Mod 2)
                                                arrLotList(j + 1) = (arr(k) \ 2)
                                            Else
                                                arrLotList(j) = arr(k)
                                            End If
                                        End If
                                        j += 2
                                    Next
                                End If
                                'End If

                                'Gán số lô dựa trên arrLotList
                                For Each b As Int32 In arrLotList
                                    If b = 0 Then
                                        iCol += 1
                                        Continue For
                                    End If
                                    Dim idx As Int32 = iRow
                                    j = 1
                                    While j <= b
                                        workSheet.Cells(idx, iCol) = fromLot
                                        j += 1
                                        fromLot += 1
                                        idx += 1
                                    End While
                                    iCol += 1
                                Next

                                startLot = startLot + iQty
                            End If
                        Else
                            iCol += 6
                        End If

                        'Tăng biến đếm ngày
                        fromDate = fromDate.AddDays(1)
                    End While

                    'Thiết lập công thức ngược về file _TOTAL cho công đoạn kiểm phẩm
                    SetFormulaTotal(app, bIsTotalFileOpened, sCusNameOfDetail, item, iEndCol, workSheet, iRow, iProcessMaxWidth)

                    'Active file đang chạy
                    app.Workbooks(sCusNameOfDetail + ".xlsx").Activate()

                    'Rải lô gia công ngược về công đoạn bắt đầu
                    iEndCol = iEndCol + 1

                    '***Tạo 1 list chứa vị trí cột có gia công hay không/****/
                    Dim lstColWork As New List(Of String)
                    iCol = 10

                    While iCol < iEndCol
                        Dim sValue As String = workSheet.Range(workSheet.Cells(1, iCol), workSheet.Cells(1, iCol)).Value
                        Dim oRestDate = (From p In lstRestDate Where p("RestDate") = sValue
                                         Select p("RestDate")).FirstOrDefault()
                        Dim sRestDate As String = If(oRestDate Is Nothing, String.Empty, CType(oRestDate, DateTime).ToString("yyyy-MM-dd"))

                        Dim iDivision As Byte = 6

                        Dim iWorkingHour As Byte = 0
                        Dim oWorkingHour As Object = Nothing

                        If sRestDate = String.Empty Then 'ngày làm việc bình thường
                            iWorkingHour = iWorkingDay
                        Else 'ngày nghỉ
                            oWorkingHour = DB.ExecuteScalar(String.Format("SELECT WorkingHour FROM dbo.PD_DailyOutputPlanning_1 D LEFT JOIN dbo.PD_MsCustomer C ON D.CustomerCode = C.CustomerCode WHERE C.CustomerName = '{0}' AND ProductCode = '{1}' And PlanningDate='{2}'", _
                                                                                 item("CustomerName"), item("ProductCode"), CType(sValue, DateTime).ToString("yyyy-MM-dd")))
                            iWorkingHour = If(oWorkingHour Is Nothing, 0, iRestDay)
                            'Nếu iWorkingHour = 0 => nghĩa là ngày nghỉ ngày không có gia công
                        End If

                        iDivision = iWorkingHour / 4
                        For i As Int32 = 1 To 6
                            If i <= iDivision Then : lstColWork.Add((iCol + i - 1).ToString() + "-1")
                            Else : lstColWork.Add((iCol + i - 1).ToString() + "-0")
                            End If
                        Next
                        iCol += 6
                    End While
                    '***

                    'Khai báo danh sách lưu vị trí các công đoạn phụ
                    Dim lstSubProcess As New List(Of DataRow)
                    Dim tblSubProcess As New DataTable
                    Dim c1 As New DataColumn("ProcessCode", Type.GetType("System.String"))
                    Dim c2 As New DataColumn("Idx", Type.GetType("System.Int32"))
                    tblSubProcess.Columns.AddRange({c1, c2})

                    ''
                    'Biến lưu giữ nhóm công đoạn
                    Dim lstPrcGroup As New List(Of Int32)

                    'Rải lô cho các công đoạn chính
                    Dim iRowR As Int32 = iRow 'Dòng copy
                    While iRow > 3
                        iRow = iRow - iProcessMaxWidth 'Dòng paste

                        'Lấy mã công đoạn hiện tại
                        Dim sPn As String = workSheet.Range(workSheet.Cells(iRow, 1), workSheet.Cells(iRow, 1)).Value
                        Dim sPrcCode As String = workSheet.Range(workSheet.Cells(iRow, 2), workSheet.Cells(iRow, 2)).Value

                        'Ghi nhận những công đoạn phụ cân rải lô & vị trí của nó
                        'Lưu ý mã 8041
                        If sPn.Length = 0 Then
                            If sPrcCode = "8047" _
                                Or sPrcCode = "1003" _
                                Or sPrcCode = "8041" _
                                Or sPrcCode = "9005" Then
                                Dim oExist As Object = (From p In lstSubProcess Where p("ProcessCode") = sPrcCode
                                            Select p("ProcessCode")).FirstOrDefault()
                                If oExist Is Nothing Then
                                    Dim rAdd As DataRow = tblSubProcess.NewRow()
                                    rAdd("ProcessCode") = workSheet.Range(workSheet.Cells(iRow, 2), workSheet.Cells(iRow, 2)).Value
                                    rAdd("Idx") = iRow
                                    lstSubProcess.Add(rAdd)
                                End If
                            End If
                            Continue While
                        End If

                        'Công đoạn đột lỗ định vị
                        If sPrcCode = "4126" Or sPrcCode = "4026" Then
                            Dim rAdd As DataRow = tblSubProcess.NewRow()
                            rAdd("ProcessCode") = workSheet.Range(workSheet.Cells(iRow, 2), workSheet.Cells(iRow, 2)).Value
                            rAdd("Idx") = iRow
                            lstSubProcess.Add(rAdd)
                        End If

                        ''Lấy thời gian lệch công đoạn
                        Dim iLeadTime As Int32 = 0
                        For i As Int32 = lstPdLeadTime.Count - 1 To 0 Step -1
                            Dim r As DataRow = lstPdLeadTime(i)
                            If r("ProcessCode") = sPrcCode Then
                                'Tìm trong lstPrcGroup
                                Dim id As Int32 = r("ProcessGroup")
                                Dim sPrcGroup As List(Of Int32) = lstPrcGroup.Where(Function(s) s = id).ToList()
                                'Nếu không tìm thấy nghĩa là đổi group -> clear
                                If sPrcGroup.Count = 0 Then
                                    lstPrcGroup.Clear()
                                End If
                                'Add nhóm
                                lstPrcGroup.Add(id)
                                iLeadTime = If(lstPrcGroup.Count = 1, r("LeadTime"), 0)
                                For j As Int32 = lstPdLeadTime.Count - 1 To i Step -1
                                    lstPdLeadTime.RemoveAt(j)
                                Next
                                Exit For
                            End If
                        Next

                        iLeadTime = iLeadTime / 4

                        '***Bắt đầu chạy lô/****/
                        'Cột paste
                        Dim iFromCol = 10
                        Dim iToCol = 10

                        'Cột copy
                        Dim iFromColR As Int32 = 10
                        Dim iToColR As Int32 = 10

                        'Xử lý trường hợp leadtime = 0
                        If iLeadTime = 0 Then
                            'Copy
                            workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.Copy(Type.Missing)
                            'Paste
                            workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        Else
                            'Tìm vị trí copy tương ứng với leadtime
                            iFromColR += 1
                            Dim iCount As Int32 = 0
                            While iFromColR < iEndCol
                                'Kiểm tra iFromColR ở 2 giá trị: -0 & -1
                                Dim sFromColR As String = iFromColR.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault() IsNot Nothing Then iCount += 1
                                If iCount = iLeadTime Then : Exit While
                                Else : iFromColR += 1
                                End If
                            End While
                            iToColR = iFromColR

                            'Tìm vị trí paste đầu tiên
                            While iFromCol < iEndCol
                                'Kiểm tra iFromCol ở 2 giá trị: -0 & -1
                                Dim sFromCol As String = iFromCol.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault() IsNot Nothing Then
                                    Exit While
                                End If
                                iFromCol += 1
                            End While
                            iToCol = iFromCol

                            If iFromCol >= iEndCol Or iFromColR >= iEndCol Then Continue While

                            'Duyệt vị trí copy & paste tương ứng với ngày làm việc/ngày nghỉ
                            Dim bEnd As Boolean = False

                            Dim i As Int32 = iFromColR 'Biến giữ vị trí copy
                            Dim j As Int32 = iFromCol  'Biến giữ vị trí paste

                            While Not bEnd

                                Dim sFromColR As String = i.ToString() + "-1"
                                Dim _existFromR = lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault()

                                Dim sFromCol As String = j.ToString() + "-1"
                                Dim _existFrom = lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault()

                                If _existFromR IsNot Nothing And _existFrom IsNot Nothing Then 'Copy đi làm & Paste đi làm
                                    'Tăng biến copy
                                    iToColR = i
                                    i += 1
                                    'Tăng biến paste
                                    iToCol = j
                                    j += 1
                                ElseIf _existFromR IsNot Nothing And _existFrom Is Nothing Then 'Copy đi làm & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị i
                                    i = iToColR + 1
                                    iFromColR = iToColR + 1

                                    '& Tìm lại vị trí paste
                                    While j < iEndCol
                                        Dim sValue As String = j.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j += 1
                                    End While

                                    iFromCol = j
                                    iToCol = j

                                ElseIf _existFromR Is Nothing And _existFrom IsNot Nothing Then 'Copy nghỉ & Paste làm
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị j
                                    j = iToCol + 1
                                    iFromCol = iToCol + 1

                                    '& Tìm lại vị trí copy
                                    While i < iEndCol
                                        Dim sValue As String = i.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i += 1
                                    End While

                                    iFromColR = i
                                    iToColR = i

                                ElseIf _existFromR Is Nothing And _existFrom Is Nothing Then 'Copy nghỉ & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)
                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    '& Tìm vị trí paste mới
                                    While j < iEndCol
                                        Dim sValue As String = j.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j += 1
                                    End While

                                    iFromCol = j
                                    iToCol = j

                                    '& vị trí copy mới
                                    While i < iEndCol
                                        Dim sValue As String = i.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i += 1
                                    End While

                                    iFromColR = i
                                    iToColR = i

                                End If

                                'Copy phần còn lại
                                If iToColR = iEndCol Then
                                    'Kết thúc
                                    bEnd = True
                                End If

                            End While
                        End If
                        '***/

                        'Sau khi rải xong lô của 1 công đoạn -> thiết lập công thức tính ngược về file tổng
                        SetFormulaTotal(app, bIsTotalFileOpened, sCusNameOfDetail, item, iEndCol, workSheet, iRow, iProcessMaxWidth)

                        'Active file đang chạy
                        app.Workbooks(sCusNameOfDetail + ".xlsx").Activate()
                        iRowR = iRow
                    End While
                    '***Kết thúc rải lô công đoạn chính

                    '***Rải lô cho các công đoạn phụ

                    lstPdLeadTime = DB.FillDataTable(String.Format("SELECT * FROM {0} WHERE LeadTimeID = '{1}' ORDER BY Idx", PublicTable.Table_PD_ProcessLeadTime, item("LeadTimeID"))).AsEnumerable().ToList()
                    If lstPdLeadTime.Count = 0 Then Continue For

                    iRowR = lstSubProcess(0)("Idx") + iProcessMaxWidth 'Dòng copy
                    For i As Int32 = 0 To lstSubProcess.Count - 1

                        If lstSubProcess(i)("ProcessCode") = "4126" _
                            Or lstSubProcess(i)("ProcessCode") = "4026" Then
                            Exit For
                        End If

                        iRow = lstSubProcess(i)("Idx") 'Dòng paste

                        'Nếu là mã 8041 thì = với đột lỗ định vị -> lấy lại dòng cần copy là mã 4126/4026
                        If lstSubProcess(i)("ProcessCode") = "8041" Then
                            iRowR = lstSubProcess(lstSubProcess.Count - 1)("Idx")
                        End If

                        ''Lấy mã công đoạn hiện tại
                        Dim sPrcCode As String = lstSubProcess(i)("ProcessCode")

                        ''Lấy thời gian lệch công đoạn
                        Dim iLeadTime As Int32 = 0
                        For j As Int32 = lstPdLeadTime.Count - 1 To 0 Step -1
                            Dim r As DataRow = lstPdLeadTime(j)
                            If r("ProcessCode") = sPrcCode Then
                                iLeadTime = r("LeadTime")
                                lstPdLeadTime.Remove(r)
                                Exit For
                            Else
                                lstPdLeadTime.Remove(r)
                            End If
                        Next

                        iLeadTime = iLeadTime / 4

                        '****Bắt đầu chạy lô
                        'Cột paste
                        Dim iFromCol = 10
                        Dim iToCol = 10

                        'Cột copy
                        Dim iFromColR As Int32 = 10
                        Dim iToColR As Int32 = 10

                        'Xử lý trường hợp leadtime = 0
                        If iLeadTime = 0 Then
                            'Copy
                            workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.Copy(Type.Missing)
                            'Paste
                            workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iEndCol - 1))
                            workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        Else
                            'Tìm vị trí copy tương ứng với leadtime
                            iFromColR += 1
                            Dim iCount As Int32 = 0
                            While iFromColR < iEndCol
                                'Kiểm tra iFromColR ở 2 giá trị: -0 & -1
                                Dim sFromColR As String = iFromColR.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault() IsNot Nothing Then iCount += 1
                                If iCount = iLeadTime Then : Exit While
                                Else : iFromColR += 1
                                End If
                            End While
                            iToColR = iFromColR

                            'Tìm vị trí paste đầu tiên
                            While iFromCol < iEndCol
                                'Kiểm tra iFromCol ở 2 giá trị: -0 & -1
                                Dim sFromCol As String = iFromCol.ToString() + "-1"
                                If lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault() IsNot Nothing Then
                                    Exit While
                                End If
                                iFromCol += 1
                            End While
                            iToCol = iFromCol

                            If iFromCol >= iEndCol Or iFromColR >= iEndCol Then Continue For

                            'Duyệt vị trí copy & paste tương ứng với ngày làm việc/ngày nghỉ
                            Dim bEnd As Boolean = False

                            Dim i_ As Int32 = iFromColR 'Biến giữ vị trí copy
                            Dim j_ As Int32 = iFromCol  'Biến giữ vị trí paste

                            While Not bEnd

                                Dim sFromColR As String = i_.ToString() + "-1"
                                Dim _existFromR = lstColWork.Where(Function(s) s = sFromColR).FirstOrDefault()

                                Dim sFromCol As String = j_.ToString() + "-1"
                                Dim _existFrom = lstColWork.Where(Function(s) s = sFromCol).FirstOrDefault()

                                If _existFromR IsNot Nothing And _existFrom IsNot Nothing Then 'Copy đi làm & Paste đi làm
                                    'Tăng biến copy
                                    iToColR = i_
                                    i_ += 1
                                    'Tăng biến paste
                                    iToCol = j_
                                    j_ += 1
                                ElseIf _existFromR IsNot Nothing And _existFrom Is Nothing Then 'Copy đi làm & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị i
                                    i_ = iToColR + 1
                                    iFromColR = iToColR + 1

                                    '& Tìm lại vị trí paste
                                    While j_ < iEndCol
                                        Dim sValue As String = j_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j_ += 1
                                    End While

                                    iFromCol = j_
                                    iToCol = j_

                                ElseIf _existFromR Is Nothing And _existFrom IsNot Nothing Then 'Copy nghỉ & Paste làm
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)

                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    'Gán lại giá trị j
                                    j_ = iToCol + 1
                                    iFromCol = iToCol + 1

                                    '& Tìm lại vị trí copy
                                    While i_ < iEndCol
                                        Dim sValue As String = i_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i_ += 1
                                    End While

                                    iFromColR = i_
                                    iToColR = i_

                                ElseIf _existFromR Is Nothing And _existFrom Is Nothing Then 'Copy nghỉ & Paste nghỉ
                                    'Copy
                                    workRange = workSheet.Range(workSheet.Cells(iRowR, iFromColR), workSheet.Cells(iRowR + iProcessMaxWidth - 1, iToColR))
                                    workRange.Copy(Type.Missing)
                                    'Paste
                                    workRange = workSheet.Range(workSheet.Cells(iRow, iFromCol), workSheet.Cells(iRow + iProcessMaxWidth - 1, iToCol))
                                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

                                    '& Tìm vị trí paste mới
                                    While j_ < iEndCol
                                        Dim sValue As String = j_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        j_ += 1
                                    End While

                                    iFromCol = j_
                                    iToCol = j_

                                    '& vị trí copy mới
                                    While i_ < iEndCol
                                        Dim sValue As String = i_.ToString() + "-1"
                                        If lstColWork.Where(Function(s) s = sValue).FirstOrDefault() IsNot Nothing Then
                                            Exit While
                                        End If
                                        i_ += 1
                                    End While

                                    iFromColR = i_
                                    iToColR = i_

                                End If

                                'Copy phần còn lại
                                If iToColR = iEndCol Then
                                    'Kết thúc
                                    bEnd = True
                                End If

                            End While
                        End If
                        '****/

                        'Sau khi rải xong lô của 1 công đoạn -> thiết lập công thức tính ngược về file tổng
                        SetFormulaTotal(app, bIsTotalFileOpened, sCusNameOfDetail, item, iEndCol, workSheet, iRow, iProcessMaxWidth)

                        'Active file đang chạy
                        app.Workbooks(sCusNameOfDetail + ".xlsx").Activate()
                        iRowR = iRow
                    Next
                    '***Kết thúc rải lô công đoạn phụ
                Next

                'Ẩn sheet1
                workSheet = workBook.Sheets(1)
                workSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Next

            SaveFileAndRelease(app, workRange, workSheet, workBook)

        Catch ex As Exception
            ShowError(ex, "Export planning detail", Me.Text)
        End Try
    End Sub

#End Region

#Region "Form Function"

    Private Sub FrmDailyOutputPlanning_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        EditRight = mnuEdit.Enabled
        BackRight = mnuBack.Enabled
        ExportRight = mnuExport.Enabled
        RefreshRight = mnuRefresh.Enabled
        DeleteRight = mnuDelete.Enabled
        PlanningDetailRight = mnuPlanningDetail.Enabled
        PlanningTotalRight = mnuPlanningTotal.Enabled

        DB = New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
        dbFpics = New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)

        SetFormEvents = ActionForm.FormLoad
    End Sub

    Private Sub FrmDailyOutputPlanning_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.KeyCode = Keys.F5 And mnuRefresh.Enabled Then
            mnuRefresh.PerformClick()
        End If

        If e.KeyCode = Keys.E And e.Control And mnuEdit.Enabled Then
            mnuEdit.PerformClick()
        End If

        If e.KeyCode = Keys.D And e.Control And mnuDelete.Enabled Then
            mnuDelete.PerformClick()
        End If

    End Sub


    Private Sub mnuRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRefresh.Click
        If dtpFromDate.Value.Date > dtpToDate.Value.Date Then
            MessageBox.Show("<FromDate> must be less than or equal to <ToDate>", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        SetFormEvents = ActionForm.Refresh
    End Sub

    Private Sub mnuEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        If String.IsNullOrEmpty(cboCustomer.Text) Then
            MessageBox.Show("<Customer> can not be empty", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If dtpFromDate.Value.Date > dtpToDate.Value.Date Then
            MessageBox.Show("<FromDate> must be less than or equal to <ToDate>", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        mnuRefresh.PerformClick()
        SetFormEvents = ActionForm.Edit
    End Sub

    Private Sub mnuBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBack.Click
        If (MessageBox.Show("Do you want to back", "Back", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
            SetFormEvents = ActionForm.Back
        End If
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        If String.IsNullOrEmpty(cboCustomer.Text) Then
            MessageBox.Show("<Customer> can not be empty", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If dtpFromDate.Value.Date > dtpToDate.Value.Date Then
            MessageBox.Show("<FromDate> must be less than or equal to <ToDate>", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If gridDailyOutputPlanning.Rows.Count = 0 Then Exit Sub
        If gridDailyOutputPlanning.SelectedRows Is Nothing Then Exit Sub

        If ShowQuestionDelete() = DialogResult.No Then Exit Sub
        Try
            DB.BeginTransaction()
            For Each r As DataGridViewRow In gridDailyOutputPlanning.SelectedRows
                Dim sCustomerCode As String = cboCustomer.SelectedValue
                Dim sProductCode As String = r.Cells("ProductCode").Value
                For Each c As DataGridViewColumn In gridDailyOutputPlanning.Columns
                    If c.Name = "ProductCode" _
                        Or c.Name = "ProductCode_K" _
                        Or c.Name = "CustomerName" _
                        Or c.Name = "StartLot" _
                        Or c.Name = "LeadTimeID" _
                        Or c.Name = "EndLot" _
                        Or c.Name = "Tick" Then
                        Continue For
                    End If
                    Dim obj As New PD_DailyOutputPlanning_1
                    obj.CustomerCode_K = sCustomerCode
                    obj.ProductCode_K = sProductCode
                    obj.PlanningDate_K = DateTime.ParseExact(c.Name, "dd-MMM-yy", CultureInfo.InvariantCulture)
                    DB.Delete(obj)
                Next
                gridDailyOutputPlanning.Rows.Remove(r)
            Next
            DB.Commit()
            CalQuantityOnGridTotal()
        Catch ex As Exception
            DB.RollBack()
            ShowError(ex, mnuDelete.Text, Me.Name)
        End Try
    End Sub

    Private Sub mnuExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExport.Click
        If gridDailyOutputPlanning.Rows.Count > 0 Then
            ExportEXCEL(gridDailyOutputPlanning, True)
        End If
    End Sub

    Private Sub CreatePlanningTotal()
        If gridDailyOutputPlanning.Rows.Count = 0 Then
            Exit Sub
        End If
        If cboCustomer.SelectedValue IsNot DBNull.Value Then
            MessageBox.Show("<Customer> must be empty", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        Try
            Dim lstTicked As New List(Of DataRow)
            If rdSeparately.Checked Then
                gridDailyOutputPlanning.EndEdit()
                For Each rTick As DataGridViewRow In gridDailyOutputPlanning.Rows
                    If rTick.Cells("Tick").Value Then
                        lstTicked.Add(CType(rTick.DataBoundItem, DataRowView).Row)
                    End If
                Next
            End If

            If rdSeparately.Checked And lstTicked.Count = 0 Then
                MessageBox.Show("No rows to export", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If (MessageBox.Show(String.Format("Do you want to {0} <Planning Total>", If(rdAll.Checked, rdAll.Text, rdSeparately.Text)), "Question!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No) Then
                Exit Sub
            End If

            clsUtil.UpLoadFile(clsUtil.FileTmp + "_TOTAL.xlsx", clsUtil.FileExp + "_TOTAL.xlsx", True)

            'Khởi tạo excel
            Dim app As New Excel.Application
            Dim workBook As Excel.Workbook = Nothing
            Dim workSheet As Excel.Worksheet = Nothing
            Dim workRange As Excel.Range = Nothing

            If File.Exists(clsUtil.FileExp + "_TOTAL.xlsx") Then
                workBook = app.Workbooks.Open(clsUtil.FileExp + "_TOTAL.xlsx", Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing)
            Else
                MessageBox.Show("Template file does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                app.Quit()
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            app.Visible = True

            'Lấy dữ liệu Lotsize
            Dim sqlLotSize As String =
                                        " Select P.ProductCode, LotSize = P.StandardLotSize " +
                                        " From " +
                                        " m_Product P inner join " +
                                        " (Select ProductCode, RevisionCode = Max(RevisionCode) From m_Product Where CompleteFlag = 'T' Group by ProductCode) B on P.ProductCode = B.ProductCode And P.RevisionCode = B.RevisionCode " +
                                        " Left Join (Select * From m_Component Where ComponentCode = 'B00') CP On P.ProductCode = CP.ProductCode And P.RevisionCode = CP.RevisionCode " +
                                        " Order By P.ProductCode, P.RevisionCode "
            Dim lstLotSize As List(Of DataRow) = dbFpics.FillDataTable(sqlLotSize).AsEnumerable().ToList()

            'Lấy dữ liệu Report Group
            Dim lstRptGroup As List(Of DataRow) = DB.FillDataTable(String.Format("SELECT ReportGroupCode, ReportGroupName, ShortName FROM {0} ORDER BY Idx", PublicTable.Table_PD_MsReportGroup)).AsEnumerable().ToList()
            Dim lstRptGroup_Org As New List(Of DataRow)(lstRptGroup)

            'Danh sách mã phim kính
            Dim tblPdFilm As DataTable = DB.FillDataTable(String.Format("SELECT ProductCode FROM {0}", PublicTable.Table_PD_MsPdFilm))
            Dim lstPdFilm As List(Of DataRow) = tblPdFilm.AsEnumerable().ToList()
            '--------------------------------------------------------------------

            Dim lstMain As List(Of DataRow) = Nothing
            Dim tbl As DataTable = Nothing

            If rdAll.Checked Then
                Dim para(2) As SqlClient.SqlParameter
                para(0) = New SqlClient.SqlParameter("@Action", "LoadReportGroup")
                para(1) = New SqlClient.SqlParameter("@FromDate", dtpFromDate.Value.Date)
                para(2) = New SqlClient.SqlParameter("@ToDate", dtpToDate.Value.Date)
                tbl = DB.ExecuteStoreProcedureTB("sp_PD_DailyOutputPlanning_1", para)
                lstMain = tbl.AsEnumerable().ToList()
            ElseIf rdSeparately.Checked Then
                lstMain = lstTicked
                tbl = lstMain.CopyToDataTable()
            End If

            'Lấy dữ liệu công đoạn
            Dim tbv As New DataView(tbl)
            Dim lstGrp As List(Of DataRow) = tbv.ToTable(True, "CustomerName").AsEnumerable().ToList()
            '-----------------------------------------------------------------------------------------

            '******Tạo sheets đầu tiên******
            'lấy công đoạn đầu tiên
            Dim item As DataRow = lstRptGroup(0)
            '---------------------------------------------------------------------------------
            workSheet = CType(workBook.Sheets(2), Excel.Worksheet)
            workSheet.Name = item("ReportGroupName")

            'Đóng băng
            'workSheet.Application.ActiveWindow.SplitColumn = 3
            'workSheet.Application.ActiveWindow.SplitRow = 2
            'workSheet.Application.ActiveWindow.FreezePanes = True
            '---------------------------------------------------------------------------------

            'Tạo phần tên cột header
            Dim fromDate As DateTime = dtpFromDate.Value.Date
            Dim toDate As DateTime = dtpToDate.Value.Date
            workSheet.Cells(1, 1) = "CODE"
            workSheet.Cells(1, 2) = "LOTSIZE"
            workSheet.Cells(1, 3) = "START"
            Dim iCol As Int16 = 4 '=3
            While fromDate <= toDate
                workSheet.Cells(1, iCol) = fromDate
                workRange = workSheet.Range(workSheet.Cells(1, iCol), workSheet.Cells(1, iCol))
                workRange.NumberFormat = "dd-MMM"
                workSheet.Cells(2, iCol) = fromDate.DayOfWeek.ToString().Substring(0, 3)
                fromDate = fromDate.AddDays(1)
                iCol += 1
            End While
            '---------------------------------------------------------------------------------

            'tạo biến lưu giữa đầu & cuối cột ngày kế hoạch
            '=> sử dụng cho sheet "GC"
            Dim iGCFromCol As Int32 = 4
            Dim iGCToCol As Int32 = iCol - 1

            'Merge tiêu đề
            workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(2, 1))
            workRange.Merge()
            workRange = workSheet.Range(workSheet.Cells(1, 2), workSheet.Cells(2, 2))
            workRange.Merge()
            workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(2, 3))
            workRange.Merge()
            '---------------------------------------------------------------------------------

            'Font = 11
            workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(2, iCol - 1))
            workRange.Font.Size = 11
            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightPink)

            'Tính khoản cách ngày
            Dim iDis As Byte = iCol - 3 '=2
            'Copy tiêu đề
            workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(2, iCol - 1)) '=workSheet.Cells(1, 2)
            workRange.Copy(Type.Missing)
            'Paste sang phần kế bên
            workRange = workSheet.Range(workSheet.Cells(1, iCol + 1), workSheet.Cells(1, iCol + 1))
            workRange.PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)

            'Detail
            Dim iEndCol As Int16 = iCol
            iCol = 1

            'Tìm & lưu các mã là phim kính
            Dim sSumPdFilm As String = String.Empty
            Dim iRow As Nullable(Of Int32) = 3
            For Each detail In lstMain
                'Tìm xem Product này có là mã để sum phim kính hay không
                Dim sPdCode As String = detail("ProductCode")
                Dim oPdCode = (From p In lstPdFilm Where p("ProductCode") = sPdCode
                                       Select p("ProductCode")).FirstOrDefault()

                If oPdCode IsNot Nothing Then
                    sSumPdFilm += "{0}" + iRow.ToString() + ","
                End If

                'Tìm Lotsize
                Dim oLotsize = (From p In lstLotSize Where p("ProductCode") = sPdCode
                                       Select p("LotSize")).FirstOrDefault()
                Dim sLotsize As String = If(oLotsize Is Nothing, String.Empty, CType(oLotsize, String))

                workSheet.Cells(iRow, iCol) = "'" + detail("ProductCode")
                workSheet.Cells(iRow, 2) = sLotsize
                workSheet.Cells(iRow, iEndCol) = "'" + detail("CustomerName")
                iRow += 1
                Application.DoEvents()
            Next
            If sSumPdFilm.Length > 0 Then sSumPdFilm = sSumPdFilm.Substring(0, sSumPdFilm.Length - 1)
            '----------------------------------------------------------------------------------------

            'Thêm phần Trial
            For iT As Int32 = 1 To 10
                workSheet.Cells(iRow, iCol) = "'-"
                workSheet.Cells(iRow, iEndCol) = "'TRIAL"
                iRow += 1
                Application.DoEvents()
            Next

            'Format khung kế bên
            FormatAllBorders(workRange, workSheet, workSheet.Cells(1, iEndCol + 1), workSheet.Cells(iRow - 1, iEndCol + iDis))
            '
            'Copy cột StartLot
            workRange = workSheet.Range(workSheet.Cells(3, iEndCol + 1), workSheet.Cells(3, iEndCol + 1))
            workRange.Formula = String.Format("=C3") '=B3
            workRange.Copy(Type.Missing)
            workRange = workSheet.Range(workSheet.Cells(4, iEndCol + 1), workSheet.Cells(iRow - 1, iEndCol + 1))
            workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
            Dim iDS As Int32 = 4 '=3
            Dim sColS As String = clsUtil.Number2String(iEndCol + 1, True)
            For iD As Int32 = iEndCol + 2 To iEndCol + iDis
                'Thiết lập công thức cho ô đầu của cột
                Dim sColE As String = clsUtil.Number2String(iDS, True)
                workRange = workSheet.Range(workSheet.Cells(3, iD), workSheet.Cells(3, iD))
                Dim sFormula As String = String.Format("=IF({0}3=0,"""",SUM(D3:{0}3,{1}3)-1)", sColE, sColS) '=3 (C)
                workRange.Formula = sFormula
                workRange.Copy(Type.Missing)
                'Sau đó copy cho toàn bộ dòng
                workRange = workSheet.Range(workSheet.Cells(4, iD), workSheet.Cells(iRow - 1, iD))
                workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                'Tăng iDS
                iDS += 1
            Next
            '

            '[24-02-2016]Tạo biến lưu giữ cột sẽ gán giá trị gia công của lô
            Dim dtLeadTimeLot As DataTable = DB.FillDataTable("SELECT * FROM PD_LeadtimeLot")
            Dim iColLeadtimeLot As Int32 = iEndCol + iDis + 1
            Dim iRowLeadtimeLot As Int32 = iRow - 10 - 1

            'Font = 12
            workRange = workSheet.Range(workSheet.Cells(3, 1), workSheet.Cells(iRow - 1, iEndCol - 1))
            workRange.Font.Size = 12
            workRange = workSheet.Range(workSheet.Cells(3, 1), workSheet.Cells(iRow - 1, 2))
            workRange.Font.Bold = True
            workRange = workSheet.Range(workSheet.Cells(3, 1), workSheet.Cells(iRow - 1, 1))
            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue)
            workRange = workSheet.Range(workSheet.Cells(3, 2), workSheet.Cells(iRow - 1, 2))
            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Thistle)
            workRange = workSheet.Range(workSheet.Cells(3, 3), workSheet.Cells(iRow - 1, 3))
            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Orange)

            Dim iEndRow As Int16 = iRow
            iRow += 1

            'Footer
            For Each g In lstGrp
                workSheet.Cells(iRow, 1) = g("CustomerName")
                'Set sum formula
                iCol = 4 '=3
                Dim sColHeader As String = clsUtil.Number2String(iCol, True)
                Dim sColEndHeader As String = clsUtil.Number2String(iEndCol, True)
                workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
                workRange.Formula = String.Format("=SUMIF(${0}{1}:${0}{2}," + ControlChars.Quote + "={3}" + ControlChars.Quote + ",{4}{1}:{4}{2})", sColEndHeader, 2, iEndRow, g("CustomerName"), sColHeader)
                workRange.Copy(Type.Missing)
                iCol = 5 '=4
                While iCol < iEndCol
                    workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    iCol += 1
                End While
                iRow += 1
            Next

            'Chèm thêm dòng tổng cho hàng TRIAL
            workSheet.Cells(iRow, 1) = "'TRIAL"
            'Set sum formula
            iCol = 4 '=3
            Dim sColHeader_Trial As String = clsUtil.Number2String(iCol, True)
            Dim sColEndHeader_Trial As String = clsUtil.Number2String(iEndCol, True)
            workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
            workRange.Formula = String.Format("=SUMIF(${0}{1}:${0}{2}," + ControlChars.Quote + "={3}" + ControlChars.Quote + ",{4}{1}:{4}{2})", sColEndHeader_Trial, 2, iEndRow, "TRIAL", sColHeader_Trial)
            workRange.Copy(Type.Missing)
            iCol = 5 '=4
            While iCol < iEndCol
                workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
                workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                iCol += 1
            End While
            iRow += 1

            workSheet.Cells(iRow, 1) = "TOTAL"
            'Set sum formula total
            iCol = 4 '=3
            Dim sColHeader_ As String = clsUtil.Number2String(iCol, True)
            workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
            workRange.Formula = String.Format("=SUM({0}{1}:{0}{2})", sColHeader_, 2, iEndRow)
            workRange.Copy(Type.Missing)
            iCol = 5 '=4
            While iCol < iEndCol
                workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
                workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                iCol += 1
            End While

            'Font = 13
            workRange = workSheet.Range(workSheet.Cells(iEndRow + 1, 1), workSheet.Cells(iRow, iEndCol - 1))
            workRange.Font.Size = 13
            workRange.Font.Bold = True

            workRange = workSheet.Range(workSheet.Cells(iRow, 1), workSheet.Cells(iRow, iEndCol - 1))
            workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Orange)

            workRange = workSheet.Range(workSheet.Cells(iRow, 3), workSheet.Cells(iRow, iEndCol - 1))
            workRange.Cells.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)

            'Vẽ khung toàn bộ
            FormatAllBorders(workRange, workSheet, workSheet.Cells(1, 1), workSheet.Cells(iRow, iEndCol - 1))

            workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(iRow, iEndCol - 1))
            workRange.RowHeight = 15.75D

            workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(iRow, iEndCol - 1))
            workRange.ColumnWidth = 10.57D

            '[24-02-2016]ghi nhận vị trí cần copy để tính thời gian gia công
            Dim iRowLT As Int32 = iRow
            Dim iColLT As Int32 = iEndCol
            '---------------------------------------------------

            Dim iRowPK As Int32 = 0 'Dòng phim kính

            'Dòng FILM KÍNH
            If sSumPdFilm.Length > 0 _
                And (workSheet.Name = "Rọi sáng" Or workSheet.Name = "Rọi sáng 1") Then
                iRow += 2
                workSheet.Cells(iRow, 1) = "PHIM KÍNH"
                iRowPK = iRow

                iCol = 4 '=3
                Dim sColHeader_Film As String = clsUtil.Number2String(iCol, True)
                workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
                Dim sFormula As String = String.Format(sSumPdFilm, sColHeader_Film)
                workRange.Formula = String.Format("=SUM({0})", sFormula)
                workRange.Copy(Type.Missing)

                iCol = 5 '=4
                While iCol < iEndCol
                    workRange = workSheet.Range(workSheet.Cells(iRow, iCol), workSheet.Cells(iRow, iCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    iCol += 1
                End While

                'Font = 13
                workRange = workSheet.Range(workSheet.Cells(iRow, 1), workSheet.Cells(iRow, iEndCol - 1))
                workRange.Font.Size = 13
                workRange.Font.Bold = True
                workRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            End If

            '[19-05-2016]>>thêm sheet tính giờ công dư/thiếu
            'mảng lưu vị trí các công đoạn
            Dim arraypos(10) As Int32
            arraypos(0) = 3 'photo
            arraypos(1) = 8 'etching
            arraypos(2) = 13 'pth
            arraypos(3) = 18 'đột lỗ
            arraypos(4) = 23 'thử mạch
            arraypos(5) = 28 'preset
            arraypos(6) = 33 'entek
            arraypos(7) = 38 'mạ vàng
            arraypos(8) = 43 'cell line s0 + đột 3
            arraypos(9) = 48 'cell line u0
            arraypos(10) = 53 'kpl + k1
            '-----------------------------------------------

            '[24-02-2016]Thêm khung dữ liệu tính thời gian gia công của lô
            workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(iRowLT, iColLT - 1))
            workRange.Copy(Type.Missing)
            workRange = workSheet.Range(workSheet.Cells(iRow + 2, 1), workSheet.Cells(iRow + 2, 1))
            workRange.PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
            Dim iRowEndLT As Int32 = iRow + 3 + lstMain.Count + 10 'dòng cuối của danh sách mã hàng
            '>>thiết lập công thức tính thời gian gia công
            Dim tp As TimeSpan = dtpToDate.Value.Date - dtpFromDate.Value.Date
            Dim ilongplan As Int32 = tp.Days + 1 'tổng số ngày của kế hoạch
            iRowLT = iRow + 4 'dòng cần tính dữ liệu
            Dim iColRun As Int32 = 4 'cột bắt đầu kế hoạch=3
            While _
                iColRun <= (ilongplan + 3) '=2
                Dim sColHeaderLeft_LT As String = clsUtil.Number2String(iColRun, True) 'cột lô gia công sản phẩm
                Dim sColHeaderRight_LT As String = clsUtil.Number2String(iColLeadtimeLot, True) 'cột thời gian gia công 1 lô sản phẩm
                workRange = workSheet.Range(workSheet.Cells(iRowLT, iColRun), workSheet.Cells(iRowLT, iColRun))
                workRange.Formula = String.Format("={0}{1}*${2}{3}", sColHeaderLeft_LT, 3, sColHeaderRight_LT, 3)
                'copy công thức
                workRange.Copy(Type.Missing)
                'paste
                workRange = workSheet.Range(workSheet.Cells(iRowLT + 1, iColRun), workSheet.Cells(iRowEndLT, iColRun))
                workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                iColRun += 1
            End While
            '-------------------------------------------------

            'tìm dòng TOTAL cuối cùng
            Dim endtotalrow As Int32 = iRowEndLT 'biến lưu giá trị dòng cuối cùng của sheet
            While True
                workRange = workSheet.Range(workSheet.Cells(endtotalrow, 1), workSheet.Cells(endtotalrow, 1))
                Dim svalue As String = If(workRange.Value Is Nothing, String.Empty, workRange.Value)
                If _
                    Not String.IsNullOrEmpty(svalue) _
                    AndAlso svalue.Contains("TOTAL") Then
                    Exit While
                End If
                endtotalrow += 1
            End While
            '-------------------------------------------------

            '*****Sau đó copy sheets đầu thành những sheet khác & đổi tên theo công đoạn*****
            'loại ra công đoạn đầu đã được tạo ra ở trên
            lstRptGroup.Remove(item)
            Dim iSheet As Int32 = 2
            For Each item In lstRptGroup
                workSheet = DirectCast(workBook.Sheets(iSheet), Excel.Worksheet)
                workSheet.Copy(Missing.Value, workSheet)
                iSheet += 1
                workSheet = DirectCast(workBook.Sheets(iSheet), Excel.Worksheet)
                workSheet.Name = item("ReportGroupName")
                If workSheet.Name <> "Rọi sáng" And _
                    workSheet.Name <> "Rọi sáng 1" And _
                     sSumPdFilm.Length > 0 Then
                    workRange = workSheet.Range(workSheet.Cells(iRowPK, 1), workSheet.Cells(iRowPK, iEndCol - 1))
                    workRange.Clear()
                End If
            Next
            '--------------------------------------------------------------------------------

            '[26-02-2016]>>đưa giá trị leadtime vào cột này
            Dim itotalsheet As Int32 = workBook.Worksheets.Count
            For iSheet_1 As Int32 = 2 To itotalsheet
                workSheet = workBook.Sheets(iSheet_1)
                workSheet.Select(Type.Missing)
                For iLT As Int32 = 3 To iRowLeadtimeLot
                    workRange = workSheet.Range(workSheet.Cells(iLT, 1), workSheet.Cells(iLT, 1))
                    Dim sPrdCode As String = workRange.Value
                    Dim sPrcCode As String = (From a In lstRptGroup_Org
                                              Where a("ReportGroupName") = workSheet.Name
                                              Select a("ShortName")).FirstOrDefault()
                    Dim rslt() As DataRow = dtLeadTimeLot.Select(String.Format("ProductCode='{0}'", sPrdCode))
                    If rslt.Length > 0 Then workSheet.Cells(iLT, iColLeadtimeLot) = rslt(0)(sPrcCode)
                Next
            Next

            'khởi tạo tiêu đề ngày kế hoạch
            '=>copy
            workSheet = CType(workBook.Sheets(2), Excel.Worksheet)
            workRange = workSheet.Range(workSheet.Cells(1, iGCFromCol), workSheet.Cells(2, iGCToCol))
            workRange.Copy(Type.Missing)
            '=>paste
            workSheet = CType(workBook.Sheets(1), Excel.Worksheet)
            workRange = workSheet.Range(workSheet.Cells(1, 5), workSheet.Cells(1, 5))
            workRange.PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
            '-----------------------------------------------

            'Vẽ khung toàn bộ
            FormatAllBorders(workRange, workSheet, workSheet.Cells(1, 5), workSheet.Cells(57, iGCToCol + 1))
            '-----------------------------------------------

            'Cập nhật lại (iGCToCol)
            iGCToCol += 1
            '----------------------

            'tìm ngày làm việc cuối của tuần trước
            Dim lstRestDate As List(Of DataRow) = clsUtil.GetRestDate(DB)
            Dim workDate As DateTime = DateTime.Now.Date.AddDays(-DateTime.Now.DayOfWeek)
            While True
                Dim oRestDate = (From p In lstRestDate Where p("RestDate") = workDate Select p("RestDate")).FirstOrDefault()
                If _
                    oRestDate Is Nothing Then
                    Exit While
                End If
                workDate = workDate.AddDays(-1)
            End While
            '& lấy danh sách nhân viên PR theo ngày này
            'Note: Nghỉ thai sản/Thai sản (7hr/ngày)
            Dim sqlop As String = String.Format(
                                    "select G.GroupName, G2.Group2Name, TypeOfOP = Case when A.Note = N'Thai sản (7hr/ngày)' then 'TS' else 'BT' end " +
                                    "from WT_Rollcall A " +
                                    "left join WT_Group G on A.GroupID = G.GroupID " +
                                    "left join WT_Group2 G2 on A.Group2ID = G2.Group2ID " +
                                    "where A.RollcallDate = '{0}' and A.Observation = 'Operator' and A.Note <> N'Nghỉ thai sản' ", workDate.ToString("yyyyMMdd"))
            Dim tblop As DataTable = DB.FillDataTable(sqlop)
            'thiết lập công thức
            For Each iposexcel In arraypos
                'tìm cột excel ngày hiện tại
                Dim icurdaycol As Int32 = 5
                While icurdaycol <= iGCToCol
                    workRange = workSheet.Range(workSheet.Cells(1, icurdaycol), workSheet.Cells(1, icurdaycol))
                    Dim colDate As DateTime = CType(workRange.Value, DateTime)
                    If colDate.Date = DateTime.Now.Date Then
                        Exit While
                    End If
                    icurdaycol += 1
                End While
                'kiểm tra nếu những cột excel sau ngày hiện tại thì giờ có/dư thiếu = giờ cần
                If _
                    icurdaycol > iGCToCol _
                    Or icurdaycol = 5 Then
                    'công thức giờ có (không tăng ca)
                    Dim irow1 As Int32 = iposexcel + 1
                    workRange = workSheet.Range(workSheet.Cells(irow1, 5), workSheet.Cells(irow1, 5))
                    workRange.Formula = String.Format("=IF(E2=""Sun"",0.5*(($C${0}*$C$62*{1})+($D${0}*$D$62*{1})),($C${0}*$C$62*{1})+($D${0}*$D$62*{1}))", iposexcel.ToString(), If(iposexcel = 43 Or iposexcel = 48, "$C$61", "$B$61"))
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow1, 6), workSheet.Cells(irow1, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    'công thức giờ có (tăng ca max)
                    Dim irow2 As Int32 = iposexcel + 2
                    workRange = workSheet.Range(workSheet.Cells(irow2, 5), workSheet.Cells(irow2, 5))
                    workRange.Formula = String.Format("=IF(E2=""Sun"",0.5*(($C${0}*$C$62*{1})+($D${0}*$D$62*{1})+($C${0}*$B$59*$B$60)),($C${0}*$C$62*{1})+($D${0}*$D$62*{1})+($C${0}*$B$59*$B$60))", iposexcel.ToString(), If(iposexcel = 43 Or iposexcel = 48, "$C$61", "$B$61"))
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow2, 6), workSheet.Cells(irow2, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    'công thức dư/thiếu (không tăng ca)
                    Dim irow3 As Int32 = iposexcel + 3
                    workRange = workSheet.Range(workSheet.Cells(irow3, 5), workSheet.Cells(irow3, 5))
                    workRange.Formula = String.Format("=E{0}-E{1}", (iposexcel + 1).ToString(), iposexcel.ToString())
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow3, 6), workSheet.Cells(irow3, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    'công thức dư/thiếu (tăng ca max)
                    Dim irow4 As Int32 = iposexcel + 4
                    workRange = workSheet.Range(workSheet.Cells(irow4, 5), workSheet.Cells(irow4, 5))
                    workRange.Formula = String.Format("=E{0}-E{1}", (iposexcel + 2).ToString(), iposexcel.ToString())
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow4, 6), workSheet.Cells(irow4, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                Else
                    Dim scolname As String = clsUtil.Number2String(icurdaycol, True)
                    'công thức giờ có (không tăng ca)
                    Dim irow1 As Int32 = iposexcel + 1
                    workRange = workSheet.Range(workSheet.Cells(irow1, 5), workSheet.Cells(irow1, 5))
                    workRange.Formula = String.Format("=-E{0}", iposexcel.ToString())
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow1, 5), workSheet.Cells(irow1, icurdaycol - 1))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    '----
                    workRange = workSheet.Range(workSheet.Cells(irow1, icurdaycol), workSheet.Cells(irow1, icurdaycol))
                    workRange.Formula = String.Format("=IF({1}2=""Sun"",0.5*(($C${0}*$C$62*{2})+($D${0}*$D$62*{2})),($C${0}*$C$62*{2})+($D${0}*$D$62*{2}))", iposexcel.ToString(), scolname, If(iposexcel = 43 Or iposexcel = 48, "$C$61", "$B$61"))
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow1, icurdaycol), workSheet.Cells(irow1, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    'công thức giờ có (tăng ca max)
                    Dim irow2 As Int32 = iposexcel + 2
                    workRange = workSheet.Range(workSheet.Cells(irow2, 5), workSheet.Cells(irow2, 5))
                    workRange.Formula = String.Format("=-E{0}", iposexcel.ToString())
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow2, 5), workSheet.Cells(irow2, icurdaycol - 1))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    '---
                    workRange = workSheet.Range(workSheet.Cells(irow2, icurdaycol), workSheet.Cells(irow2, icurdaycol))
                    workRange.Formula = String.Format("=IF({1}2=""Sun"",0.5*(($C${0}*$C$62*{2})+($D${0}*$D$62*{2})+($C${0}*$B$59*$B$60)),($C${0}*$C$62*{2})+($D${0}*$D$62*{2})+($C${0}*$B$59*$B$60))", iposexcel.ToString(), scolname, If(iposexcel = 43 Or iposexcel = 48, "$C$61", "$B$61"))
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow2, icurdaycol), workSheet.Cells(irow2, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    'công thức dư/thiếu (không tăng ca)
                    Dim irow3 As Int32 = iposexcel + 3
                    workRange = workSheet.Range(workSheet.Cells(irow3, 5), workSheet.Cells(irow3, 5))
                    workRange.Formula = String.Format("=-E{0}", iposexcel.ToString())
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow3, 5), workSheet.Cells(irow3, icurdaycol - 1))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    '---
                    workRange = workSheet.Range(workSheet.Cells(irow3, icurdaycol), workSheet.Cells(irow3, icurdaycol))
                    workRange.Formula = String.Format("={2}{0}-{2}{1}", (iposexcel + 1).ToString(), iposexcel.ToString(), scolname)
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow3, icurdaycol), workSheet.Cells(irow3, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    'công thức dư/thiếu (tăng ca max)
                    Dim irow4 As Int32 = iposexcel + 4
                    workRange = workSheet.Range(workSheet.Cells(irow4, 5), workSheet.Cells(irow4, 5))
                    workRange.Formula = String.Format("=-E{0}", iposexcel.ToString())
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow4, 5), workSheet.Cells(irow4, icurdaycol - 1))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                    '---
                    workRange = workSheet.Range(workSheet.Cells(irow4, icurdaycol), workSheet.Cells(irow4, icurdaycol))
                    workRange.Formula = String.Format("={2}{0}-{2}{1}", (iposexcel + 2).ToString(), iposexcel.ToString(), scolname)
                    workRange.Copy(Type.Missing)
                    workRange = workSheet.Range(workSheet.Cells(irow4, icurdaycol), workSheet.Cells(irow4, iGCToCol))
                    workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                End If

                'thiết lập công thức tính giờ cần
                Select Case iposexcel
                    Case 3 'Rọi sáng 1 & Rọi sáng 2
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Rọi sáng 1'!D{0} + 'Rọi sáng 2'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='Photo' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='Photo' AND TypeOfOP='TS'")
                    Case 8 'Eching 1 & Eching 2
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Eching 1'!D{0} + 'Eching 2'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='Etching/Haft' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='Etching/Haft' AND TypeOfOP='TS'")
                    Case 13 'PTH
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='PTH'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='PTH' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='PTH' AND TypeOfOP='TS'")
                    Case 18 'Đột lỗ
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Đột lỗ'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='Preset/CL Punch' AND Group2Name='CL Punch-A' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='Preset/CL Punch' AND Group2Name='CL Punch-A' AND TypeOfOP='TS'")
                    Case 23 'Thử mạch 1
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Thử mạch 1'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='Entek/Checker' AND Group2Name='Checker-A' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='Entek/Checker' AND Group2Name='Checker-A' AND TypeOfOP='TS'")
                    Case 28 'Preset
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Preset'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='Preset/CL Punch' AND Group2Name='Preset-A' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='Preset/CL Punch' AND Group2Name='Preset-A' AND TypeOfOP='TS'")
                    Case 33 'Entek
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Entek'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='Entek/Checker' AND Group2Name='ENTEK-A' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='Entek/Checker' AND Group2Name='ENTEK-A' AND TypeOfOP='TS'")
                    Case 38 'Ni-Au
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Ni-Au'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='Gold Plating/Soft 2' AND Group2Name='Gold plating-A' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='Gold Plating/Soft 2' AND Group2Name='Gold plating-A' AND TypeOfOP='TS'")
                    Case 43 'Cell Line S00 & Đột 3
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Cell Line S00'!D{0} + 'Đột 3'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "(GroupName like '%Cell line S0%' OR GroupName like '%Cell line_S00%' OR GroupName='GC-Dap') AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "(GroupName like '%Cell line S0%' OR GroupName like '%Cell line_S00%' OR GroupName='GC-Dap') AND TypeOfOP='TS'")
                    Case 48 'Cell Line U00
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Cell Line U00'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "(GroupName like '%Cell line U0%' OR GroupName like '%Cell line_U00%') AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "(GroupName like '%Cell line U0%' OR GroupName like '%Cell line_U00%') AND TypeOfOP='TS'")
                    Case 53 'Phân loại & Kiểm 1
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 5), workSheet.Cells(iposexcel, 5))
                        workRange.Formula = String.Format("='Phân loại'!D{0} + 'Kiểm 1'!D{0}", endtotalrow.ToString())
                        workRange.Copy(Type.Missing)
                        workRange = workSheet.Range(workSheet.Cells(iposexcel, 6), workSheet.Cells(iposexcel, iGCToCol))
                        workRange.PasteSpecial(Excel.XlPasteType.xlPasteFormulas, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
                        'lấy số lượng nhân viên OP & OP thay sản
                        workSheet.Cells(iposexcel, 3) = tblop.Compute("Count(GroupName)", "GroupName='INS' AND TypeOfOP='BT'")
                        workSheet.Cells(iposexcel, 4) = tblop.Compute("Count(GroupName)", "GroupName='INS' AND TypeOfOP='TS'")
                End Select
            Next
            '-----------------------------------------------

            Me.Cursor = Cursors.Arrow

            SaveFileAndRelease(app, workRange, workSheet, workBook)

        Catch ex As Exception
            Me.Cursor = Cursors.Arrow
            Throw ex
        End Try
    End Sub

    Private Sub CreatePlanDblSide()
        Dim lstTicked As New List(Of DataRow)
        If rdSeparately.Checked Then
            gridDailyOutputPlanning.EndEdit()
            For Each rTick As DataGridViewRow In gridDailyOutputPlanning.Rows
                If rTick.Cells("Tick").Value Then
                    lstTicked.Add(CType(rTick.DataBoundItem, DataRowView).Row)
                End If
            Next
        End If
        'Dim msg As String = "Không có dữ liệu trên lưới"
        'If rdSeparately.Checked And lstTicked.Count = 0 Then
        '    MessageBox.Show(msg, "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Exit Sub
        'End If
        'msg = "Có phải bạn muốn lập tiến độ hàng hai mặt"
        'If (MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No) Then
        '    Exit Sub
        'End If
        Try
            Dim sFileName As String = "_DOUBLESIDE_PLAN.xlsx"
            clsUtil.UpLoadFile(clsUtil.FileTmp + sFileName, clsUtil.FileExp + sFileName, True)

            'Khởi tạo excel
            Dim app As New Excel.Application
            Dim workBook As Excel.Workbook = Nothing
            Dim workSheet As Excel.Worksheet = Nothing
            Dim workRange As Excel.Range = Nothing

            If File.Exists(clsUtil.FileExp + sFileName) Then
                workBook = app.Workbooks.Open(clsUtil.FileExp + sFileName, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing)
            Else
                MessageBox.Show("File này không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                app.Quit()
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            app.Visible = True

            workSheet = CType(workBook.Sheets(1), Excel.Worksheet)
            workSheet.Name = arrPrcDblSide(0)

            'Lấy danh sách sản phẩm Hitachi
            Dim lstMain As List(Of DataRow) = Nothing
            Dim tbl As DataTable = Nothing
            If rdAll.Checked Then
                tbl = CType(CType(gridDailyOutputPlanning.DataSource, BindingSource).DataSource, DataTable).Select("LeadTimeID=19 OR LeadTimeID=22 OR LeadTimeID=23").CopyToDataTable()
                lstMain = tbl.AsEnumerable().ToList()
            ElseIf rdSeparately.Checked Then
                lstMain = lstTicked
                tbl = lstMain.CopyToDataTable()
            End If
            '----------------------------------------------------------------------------

            'Đóng băng
            workSheet.Application.ActiveWindow.SplitColumn = 3
            workSheet.Application.ActiveWindow.SplitRow = 2
            workSheet.Application.ActiveWindow.FreezePanes = True

            'Khởi tạo header & format cho file excel
            Dim fromDate As DateTime = dtpFromDate.Value.Date
            Dim toDate As DateTime = dtpToDate.Value.Date

            'Cột ProductCode
            workSheet.Cells(1, 1) = "CODE"
            workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(2, 1))
            workRange.Merge(Type.Missing)

            'Cột Customer
            workSheet.Cells(1, 2) = "CUSTOMER"
            workRange = workSheet.Range(workSheet.Cells(1, 2), workSheet.Cells(2, 2))
            workRange.ColumnWidth = 13
            workRange.Merge(Type.Missing)

            'Cột StartLot
            workSheet.Cells(1, 3) = "START"
            workRange = workSheet.Range(workSheet.Cells(1, 3), workSheet.Cells(2, 3))
            workRange.Merge(Type.Missing)

            Dim lstRestDate As List(Of DataRow) = clsUtil.GetRestDate(DB)

            Dim icol As Int32 = 4
            While fromDate <= toDate
                'Tô màu tiêu đề
                workRange = workSheet.Range(workSheet.Cells(1, icol), workSheet.Cells(1, icol + 2))
                workRange.Font.Size = 11
                workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightPink)
                workRange = workSheet.Range(workSheet.Cells(2, icol), workSheet.Cells(2, icol + 2))
                workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(ColorTranslator.FromHtml("#DBEEF3"))
                '----------------------------------------------------
                'Tô màu nếu là ngày nghỉ
                Dim oRestDate = (From p In lstRestDate Where p("RestDate") = fromDate
                                    Select p("RestDate")).FirstOrDefault()
                Dim sRestDate As String = If(oRestDate Is Nothing, String.Empty, CType(oRestDate, DateTime).ToString("yyyy-MM-dd"))
                If sRestDate <> String.Empty Then
                    workRange = workSheet.Range(workSheet.Cells(1, icol), workSheet.Cells(2 + lstMain.Count, icol + 2))
                    workRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Orange)
                End If
                '----------------------
                workSheet.Cells(1, icol) = fromDate.ToString("dd/MMM/yy")
                workRange = workSheet.Range(workSheet.Cells(1, icol), workSheet.Cells(1, icol + 2))
                workRange.Merge()
                workRange.ColumnWidth = 5
                workSheet.Cells(2, icol) = "C1"
                workSheet.Cells(2, icol + 1) = "C2"
                workSheet.Cells(2, icol + 2) = "C3"
                fromDate = fromDate.AddDays(1)
                icol += 3
            End While
            icol -= 1

            'Chi tiết sản phẩm
            Dim iEndCol As Int16 = icol
            icol = 1
            Dim iRow As Nullable(Of Int32) = 3
            For Each detail In lstMain
                workSheet.Cells(iRow, 1) = "'" + detail("ProductCode")
                workSheet.Cells(iRow, 2) = "'" + detail("CustomerName")
                iRow += 1
                Application.DoEvents()
            Next

            'Set format
            workRange = workSheet.Range(workSheet.Cells(1, 1), workSheet.Cells(tbl.Rows.Count + 2, iEndCol))
            workRange.Borders.Color = ColorTranslator.ToWin32(Color.Black)
            workRange.Borders.Weight = Excel.XlBorderWeight.xlThin

            '*****Sau đó copy sheets đầu thành những sheet khác & đổi tên theo công đoạn*****
            Dim iSheet As Int32 = 1
            For item As Int32 = 1 To arrPrcDblSide.Length - 1
                workSheet = DirectCast(workBook.Sheets(iSheet), Excel.Worksheet)
                workSheet.Copy(Missing.Value, workSheet)
                iSheet += 1
                workSheet = DirectCast(workBook.Sheets(iSheet), Excel.Worksheet)
                workSheet.Name = arrPrcDblSide(item)
            Next
            '--------------------------------------------------------------------------------

            SaveFileAndRelease(app, workRange, workSheet, workBook)

            Me.Cursor = Cursors.Arrow
        Catch ex As Exception
            Me.Cursor = Cursors.Arrow
            Throw ex
        End Try
    End Sub

    Private Sub mnuPlanningTotal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPlanningTotal.Click
        Try
            CreatePlanningTotal()
            CreatePlanDblSide()
        Catch ex As Exception
            ShowError(ex, mnuPlanningDetail.Text, Me.Text)
        End Try
    End Sub

    Private _tblMain As DataTable

    Private Sub mnuPlanningDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPlanningDetail.Click
        Try
            If gridDailyOutputPlanning.Rows.Count = 0 Then Exit Sub

            If cboCustomer.SelectedValue IsNot DBNull.Value Then
                MessageBox.Show("<Customer> must be empty", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            '_TOTAL
            If CheckFileOpened(clsUtil.FileExp + String.Format("{0}.xlsx", "_TOTAL")) Then
                MessageBox.Show("Bạn phải đóng file _TOTAL.xlsx trước", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            '_DOUBLESIDE_PLAN
            If CheckFileOpened(clsUtil.FileExp + String.Format("{0}.xlsx", "_DOUBLESIDE_PLAN")) Then
                MessageBox.Show("Bạn phải đóng file _DOUBLESIDE_PLAN.xlsx trước", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim lstTicked As New List(Of DataRow)
            If rdSeparately.Checked Then
                gridDailyOutputPlanning.EndEdit()
                For Each rTick As DataGridViewRow In gridDailyOutputPlanning.Rows
                    If rTick.Cells("Tick").Value Then
                        lstTicked.Add(CType(rTick.DataBoundItem, DataRowView).Row)
                    End If
                Next
            End If

            If rdSeparately.Checked And lstTicked.Count = 0 Then
                MessageBox.Show("No rows to export", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If MessageBox.Show(String.Format("Do you want to {0} <Planning Detail>", If(rdAll.Checked, rdAll.Text, rdSeparately.Text)), "Question!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If

            If _tblMain IsNot Nothing Then _tblMain.Clear()

            If rdAll.Checked Then
                _tblMain = tblDailyOutputPlanning.Copy()
            ElseIf rdSeparately.Checked Then
                _tblMain = lstTicked.CopyToDataTable()
            End If

            Dim func As ThreadStart = Nothing
            Dim thread As Thread

            func = New ThreadStart(AddressOf CreatePlanningDetail)

            thread = New Thread(func)
            thread.Start()

        Catch ex As Exception
            ShowError(ex, mnuPlanningDetail.Text, Me.Name)
        End Try

    End Sub

#End Region

    Private Sub gridDailyOutputPlanning_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridDailyOutputPlanning.CellValueChanged
        Try
            If GetFormEvents <> ActionForm.Edit Then Exit Sub
            If gridDailyOutputPlanning.CurrentRow Is Nothing Then Exit Sub

            Dim r As DataGridViewRow = gridDailyOutputPlanning.CurrentRow

            If e.ColumnIndex = ProductCode_K.Index _
                Or e.ColumnIndex = gridDailyOutputPlanning.Columns("EndLot").Index Then Exit Sub

            bsDailyOutputPlanning.EndEdit()

            Dim obj As PD_DailyOutputPlanning_1

            If r.Cells("ProductCode").Value Is DBNull.Value Then Exit Sub

            If e.ColumnIndex = ProductCode.Index Then
                'Gán mặc định giá trị leadtime
                Dim sql As String = String.Format("SELECT LeadTimeID FROM {0} WHERE ProductCode = '{1}'", PublicTable.Table_PD_MsProductLeadTime, r.Cells("ProductCode").Value)
                Dim tbl As DataTable = DB.FillDataTable(sql)
                If tbl.Rows.Count = 1 Then
                    r.Cells("LeadTimeID").Value = tbl.Rows(0)(0)
                Else
                End If
            End If

            obj = New PD_DailyOutputPlanning_1

            Dim sCustomerCode_K = cboCustomer.SelectedValue
            Dim sProductCode_K As String = IIf(r.Cells("ProductCode_K").Value Is DBNull.Value, String.Empty, r.Cells("ProductCode_K").Value)
            Dim planningDate_K As DateTime = DateTime.MinValue

            Dim currentCol As DataGridViewColumn = gridDailyOutputPlanning.Columns(e.ColumnIndex)

            If currentCol.Name <> "ProductCode" _
                And currentCol.Name <> "StartLot" _
                And currentCol.Name <> "LeadTimeID" Then
                planningDate_K = DateTime.ParseExact(gridDailyOutputPlanning.Columns(e.ColumnIndex).Name, "dd-MMM-yy", CultureInfo.InvariantCulture)
            Else
                Try
                    DB.BeginTransaction()
                    For Each c As DataGridViewColumn In r.DataGridView.Columns
                        If c.Name <> ProductCode.Name _
                            And c.Name <> ProductCode_K.Name _
                            And c.Name <> StartLot.Name _
                            And c.Name <> LeadTimeID.Name _
                            And c.Name <> CustomerName.Name _
                            And c.Name <> "EndLot" _
                            And c.Name <> Tick.Name Then
                            planningDate_K = DateTime.ParseExact(c.Name, "dd-MMM-yy", CultureInfo.InvariantCulture)
                            Dim sCondition As String = String.Format("CustomerCode='{0}' And ProductCode='{1}' And PlanningDate='{2}'", sCustomerCode_K, sProductCode_K, planningDate_K.ToString("yyyy-MM-dd"))
                            obj = New PD_DailyOutputPlanning_1
                            SetValues(obj, r, c, sCustomerCode_K, planningDate_K)
                            DB.Update(obj, sCondition)
                        End If
                    Next
                    r.Cells("ProductCode_K").Value = obj.ProductCode_K
                    DB.Commit()
                Catch ex As Exception
                    DB.RollBack()
                End Try
                Exit Sub
            End If

            obj.CustomerCode_K = sCustomerCode_K
            obj.ProductCode_K = sProductCode_K
            obj.PlanningDate_K = planningDate_K

            DB.GetObject(obj)

            If obj.ProductCode_K Is Nothing Then
                SetValues(obj, r, gridDailyOutputPlanning.Columns(e.ColumnIndex), sCustomerCode_K, planningDate_K)
                If obj.Quantity <> String.Empty Then
                    DB.Insert(obj)
                End If
            Else
                Dim sCondition As String = String.Format("CustomerCode='{0}' And ProductCode='{1}' And PlanningDate='{2}'", sCustomerCode_K, sProductCode_K, planningDate_K.ToString("yyyy-MM-dd"))
                SetValues(obj, r, gridDailyOutputPlanning.Columns(e.ColumnIndex), sCustomerCode_K, planningDate_K)
                If obj.Quantity <> String.Empty Then
                    DB.Update(obj, sCondition)
                Else
                    DB.Delete(obj)
                End If
            End If

            r.Cells("ProductCode_K").Value = obj.ProductCode_K

            'Tính toán tổng số lô/ngày hoặc /ca
            CalQuantityOnGridTotal()
            CalEndLot(r)
        Catch ex As Exception
            ShowError(ex, "gridDailyOutputPlanning_CellValueChanged", Me.Name)
        End Try
    End Sub

    Private Sub cboCustomer_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCustomer.SelectedValueChanged
        If Not bCboLoading Then
            SetFormEvents = ActionForm.Refresh
        End If
    End Sub

    Private Sub dtpFromDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFromDate.ValueChanged
        SetFormEvents = ActionForm.Refresh
    End Sub

    Private Sub dtpToDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpToDate.ValueChanged
        SetFormEvents = ActionForm.Refresh
    End Sub

    Private Sub gridDailyOutputPlanning_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridDailyOutputPlanning.CellClick
        Try
            If cboCustomer.Text = String.Empty Then Exit Sub
            If GetFormEvents = ActionForm.Edit Then Exit Sub
            If gridDailyOutputPlanning.SelectedRows Is Nothing Then Exit Sub
            If gridDailyOutputPlanning.CurrentRow Is Nothing Then Exit Sub
            If gridDailyOutputPlanning.CurrentRow.IsNewRow Then Exit Sub
            'Nếu dòng được chọn có ngày nghỉ
            'thì đưa ngày nghỉ này sang lưới con bên trái
            'để nhập thời gian làm việc ngày nghỉ
            Dim r As DataGridViewRow = gridDailyOutputPlanning.CurrentRow
            'Khởi tạo bảng
            Dim tbl As New DataTable
            Dim colRestDate As New DataColumn("RestDate", Type.GetType("System.DateTime"))
            Dim colDayWeek As New DataColumn("DayWeek", Type.GetType("System.String"))
            Dim colWorkingHour As New DataColumn("WorkingHour", Type.GetType("System.Int32"))
            tbl.Columns.AddRange({colRestDate, colDayWeek, colWorkingHour})

            Dim lstRestDate As List(Of DataRow) = clsUtil.GetRestDate(DB)
            For Each c As DataColumn In tblDailyOutputPlanning.Columns
                If c.ColumnName <> ProductCode.Name _
                    And c.ColumnName <> ProductCode_K.Name _
                    And c.ColumnName <> StartLot.Name _
                    And c.ColumnName <> LeadTimeID.Name _
                    And c.ColumnName <> CustomerName.Name _
                    And c.ColumnName <> "EndLot" _
                    And c.ColumnName <> Tick.Name Then
                    Dim dPlanningDate As DateTime = DateTime.ParseExact(c.ColumnName, "dd-MMM-yy", CultureInfo.InvariantCulture)
                    Dim oRestDate = (From p In lstRestDate Where p("RestDate") = dPlanningDate
                                             Select p("RestDate")).FirstOrDefault()
                    If oRestDate Is Nothing Then : Continue For
                    Else
                        Dim oWorkingHour As Object = DB.ExecuteScalar(String.Format("SELECT WorkingHour FROM dbo.PD_DailyOutputPlanning_1 WHERE CustomerCode = '{0}' AND ProductCode = '{1}' And PlanningDate='{2}'", _
                                                                                  cboCustomer.SelectedValue, r.Cells("ProductCode").Value, CType(oRestDate, DateTime).ToString("yyyy-MM-dd")))
                        If oWorkingHour IsNot Nothing Then
                            Dim rNew As DataRow = tbl.NewRow()
                            rNew("RestDate") = oRestDate
                            rNew("DayWeek") = CType(oRestDate, DateTime).DayOfWeek.ToString()
                            rNew("WorkingHour") = oWorkingHour
                            tbl.Rows.Add(rNew)
                        End If
                    End If
                End If
                gridRestDate.AutoGenerateColumns = False
                gridRestDate.DataSource = tbl
            Next
        Catch ex As Exception
            ShowError(ex, "grid_CellClick", Me.Name)
        End Try
    End Sub

    Private Sub gridRestDate_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridRestDate.CellValueChanged
        Try
            If gridRestDate.CurrentRow Is Nothing Then Exit Sub

            gridRestDate.EndEdit(True)

            Dim r As DataGridViewRow = gridDailyOutputPlanning.CurrentRow
            Dim obj As New PD_DailyOutputPlanning_1

            Dim sCustomerCode_K = cboCustomer.SelectedValue
            Dim sProductCode_K As String = IIf(r.Cells("ProductCode").Value Is DBNull.Value, String.Empty, r.Cells("ProductCode").Value)
            Dim planningDate_K As DateTime = gridRestDate.CurrentRow.Cells("RestDate").Value

            obj.CustomerCode_K = sCustomerCode_K
            obj.ProductCode_K = sProductCode_K
            obj.PlanningDate_K = planningDate_K

            DB.GetObject(obj)

            If obj.ProductCode_K IsNot Nothing Then
                obj.WorkingHour = gridRestDate.CurrentRow.Cells("WorkingHour").Value
                DB.Update(obj)
            End If
        Catch ex As Exception
            ShowError(ex, "gridRestDate_CellValueChanged", Me.Name)
        End Try
    End Sub

    Private Sub gridDailyOutputPlanning_CellLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridDailyOutputPlanning.CellLeave
        Try
            If GetFormEvents <> ActionForm.Edit Then Exit Sub

            Dim r As DataGridViewRow = gridDailyOutputPlanning.CurrentRow
            If r Is Nothing Then Exit Sub

            If e.ColumnIndex = ProductCode.Index Then
                gridDailyOutputPlanning.EndEdit()
                If r.Cells("ProductCode").Value IsNot DBNull.Value Then
                    r.Cells("ProductCode").Value = r.Cells("ProductCode").Value.ToString().PadLeft(5, "0")
                End If
            End If
        Catch ex As Exception
            ShowError(ex, "gridDailyOutputPlanning_CellLeave", Me.Name)
        End Try
    End Sub

    Private Function GetRevision(ByVal sPrdCode As String) As String
        Dim DTRevision As DataTable = New DataTable
        Dim StrSQL As String
        StrSQL = String.Format(
                 " SELECT TOP 1 RevisionCode FROM m_Product " +
                 " WHERE ProductCode = '{0}' " +
                 " GROUP BY ProductCode,RevisionCode " +
                 " ORDER BY RevisionCode DESC ", sPrdCode)
        DTRevision = dbFpics.FillDataTable(StrSQL)
        If DTRevision.Rows.Count > 0 Then Return DTRevision.Rows(0)("RevisionCode")
        Return String.Empty
    End Function

    Private Sub txtProductCode_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtProductCode.TextChanged
        If gridDailyOutputPlanning.DataSource Is Nothing Then Exit Sub
        Dim sFilter As String = String.Empty
        If txtProductCode.Text <> "" Then
            sFilter += String.Format("ProductCode like '%{0}%'", txtProductCode.Text)
        End If
        tblDailyOutputPlanning.DefaultView.RowFilter = sFilter
    End Sub

    Private Sub rd_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdSeparately.CheckedChanged, rdAll.CheckedChanged
        If GetFormEvents = ActionForm.FormLoad Then Exit Sub
        gridDailyOutputPlanning.Columns("Tick").Visible = rdSeparately.Checked
        gridDailyOutputPlanning.ReadOnly = Not rdSeparately.Checked
        For Each c As DataGridViewColumn In gridDailyOutputPlanning.Columns
            If c.Name <> Tick.Name Then
                gridDailyOutputPlanning.Columns(c.Name).ReadOnly = True
            End If
        Next
    End Sub

    Private Sub mnuCopy_Click(sender As System.Object, e As System.EventArgs) Handles mnuCopy.Click
        Dim msg As String = "Do you want to copy plan output data"
        If MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        DB.BeginTransaction()
        Try
            'xóa dữ liệu
            Dim sql As String = String.Format("delete from {0} where PlanningDate between '{1}' and '{2}'", _
                                              PublicTable.Table_PD_DailyOutputPlanning_1, _
                                              dtpFromDate.Value.ToString("yyyy-MM-dd"), _
                                              dtpToDate.Value.ToString("yyyy-MM-dd"))
            DB.ExecuteNonQuery(sql)
            'copy dữ liệu
            sql = String.Format(
                " INSERT INTO [dbo].[PD_DailyOutputPlanning_1] " +
                        " ([CustomerCode] " +
                        " ,[ProductCode] " +
                        " ,[Quantity] " +
                        " ,[PlanningDate] " +
                        " ,[WeekNumber] " +
                        " ,[StartLot] " +
                        " ,[LeadTimeID] " +
                        " ,[WorkingHour] " +
                        " ,[CreateUser] " +
                        " ,[CreateDate]) " +
                " SELECT " +
                        " [CustomerCode] " +
                        " ,[ProductCode] " +
                        " ,[Quantity] " +
                        " ,[PlanningDate] " +
                        " ,[WeekNumber] " +
                        " ,[StartLot] " +
                        " ,[LeadTimeID] " +
                        " ,[WorkingHour] " +
                        " ,[CreateUser] " +
                        " ,[CreateDate] " +
                " FROM " +
                " [dbo].[PD_DailyOutputPlanning] " +
                " WHERE PlanningDate between '{0}' and '{1}'", _
                                    dtpFromDate.Value.ToString("yyyy-MM-dd"), _
                                              dtpToDate.Value.ToString("yyyy-MM-dd"))
            DB.ExecuteNonQuery(sql)

            DB.Commit()

            MessageBox.Show("Copy data successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            SetFormEvents = ActionForm.Refresh

        Catch ex As Exception
            DB.RollBack()
            ShowError(ex, mnuCopy.Text, Me.Name)
        End Try
    End Sub

    Private Sub SetValues4Import(ByRef obj As PD_DailyOutputPlanning_1, _
                          ByVal r As DataRow, _
                          ByVal c1 As DataColumn, _
                          ByVal c2 As DataColumn, _
                          ByVal c3 As DataColumn, _
                          ByVal sPdCode As String, _
                          ByVal sCusCode As String, _
                          ByVal planningDate As DateTime)

        obj.CustomerCode_K = sCusCode
        obj.ProductCode_K = sPdCode
        obj.PlanningDate_K = planningDate

        'kiểm tra val1, val2 & val3 có phải là số hay không
        If Not Microsoft.VisualBasic.IsNumeric(If(If(r(c1.ColumnName) Is DBNull.Value, String.Empty, r(c1.ColumnName)) = String.Empty, "0", r(c1.ColumnName))) Then
            Throw New Exception(String.Format("Nhập sai số lô gia công của ca 1 (số lô gia công phải là chữ số) ở mã: {0} ngày {1}", sPdCode, planningDate.ToString("dd/MM/yyyy")))
            Exit Sub
        End If
        If Not Microsoft.VisualBasic.IsNumeric(If(If(r(c2.ColumnName) Is DBNull.Value, String.Empty, r(c2.ColumnName)) = String.Empty, "0", r(c2.ColumnName))) Then
            Throw New Exception(String.Format("Nhập sai số lô gia công của ca 2 (số lô gia công phải là chữ số) ở mã: {0} ngày {1}", sPdCode, planningDate.ToString("dd/MM/yyyy")))
            Exit Sub
        End If
        If Not Microsoft.VisualBasic.IsNumeric(If(If(r(c3.ColumnName) Is DBNull.Value, String.Empty, r(c3.ColumnName)) = String.Empty, "0", r(c3.ColumnName))) Then
            Throw New Exception(String.Format("Nhập sai số lô gia công của ca 3 (số lô gia công phải là chữ số) ở mã: {0} ngày {1}", sPdCode, planningDate.ToString("dd/MM/yyyy")))
            Exit Sub
        End If
        '------------------------------------------------

        Dim val1 As String = If(If(r(c1.ColumnName) Is DBNull.Value, String.Empty, r(c1.ColumnName)) = String.Empty, "-", r(c1.ColumnName))
        Dim val2 As String = If(If(r(c2.ColumnName) Is DBNull.Value, String.Empty, r(c2.ColumnName)) = String.Empty, "-", r(c2.ColumnName))
        Dim val3 As String = If(If(r(c3.ColumnName) Is DBNull.Value, String.Empty, r(c3.ColumnName)) = String.Empty, "-", r(c3.ColumnName))

        obj.Quantity = If(val1 = "-" And val2 = "-" And val3 = "-", String.Empty, If(val1 = "-", "0", val1) & "-" & If(val2 = "-", "0", val2) & "-" & If(val3 = "-", "0", val3))
        obj.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(obj.PlanningDate_K, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday)
        obj.StartLot = If(r("StartLot") Is DBNull.Value, String.Empty, r("StartLot"))

        'tìm leadtimeid
        Dim oLeadTimeID As Object = DB.ExecuteScalar( _
                                                    String.Format("select ID from PD_MsLeadTime where LeadTimeName = '{0}'", If(r("LeadTime") Is DBNull.Value, String.Empty, r("LeadTime"))) _
                                                    )
        obj.LeadTimeID = If(oLeadTimeID Is Nothing, String.Empty, oLeadTimeID)
        '--------------

        'Cập nhật giờ làm việc cho ngày nghỉ
        Dim sql As String = String.Format("SELECT [HolidayDate] as RestDate FROM [FPICS_HolidayDate] WHERE [HolidayDate] = '{0}'",
                                          planningDate.ToString("yyyy-MM-dd"))
        Dim tbl As DataTable = DB.FillDataTable(sql)
        If tbl.Rows.Count > 0 Then 'Nếu là ngày nghỉ
            'Dim restDate As DateTime = CType(tbl.Rows(0)(0), DateTime)
            'If restDate.DayOfWeek = DayOfWeek.Saturday Then : obj.WorkingHour = If(obj.Quantity <> String.Empty, 24, 0)
            'ElseIf restDate.DayOfWeek = DayOfWeek.Sunday Then : obj.WorkingHour = If(obj.Quantity <> String.Empty, 24, 0)
            'End If
            obj.WorkingHour = If(obj.Quantity <> String.Empty, 24, 0)
        Else 'Nếu là ngày bình thường
            obj.WorkingHour = 24
        End If

        obj.CreateDate = DateTime.Now
        obj.CreateUser = CurrentUser.UserID

    End Sub

    Private Sub mnuImport_Click(sender As System.Object, e As System.EventArgs) Handles mnuImport.Click

        If (MessageBox.Show("Có phải bạn muốn import dữ liệu", "Question???", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No) Then
            Exit Sub
        End If

        Dim tbl As DataTable = ImportEXCEL("import")

        If tbl.Rows.Count = 0 Then Exit Sub

        DB.BeginTransaction()

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim iCol As Int32 = 4

            '3 cột đầu tiền là Customer(0) + ProductCode(1) + StartLot(2)
            'Import dữ liệu mới

            Dim lstDate As New List(Of DateTime)

            While iCol < tbl.Columns.Count - 1

                Dim planDate As DateTime = DateTime.ParseExact(tbl.Columns(iCol).ColumnName, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                lstDate.Add(planDate)

                For Each r As DataRow In tbl.Rows

                    Dim obj As New PD_DailyOutputPlanning_1

                    Dim sCustomer As String = If(r("Customer") Is DBNull.Value, String.Empty, r("Customer"))
                    Dim sProductCode_K As String = If(r("ProductCode") Is DBNull.Value, String.Empty, r("ProductCode").ToString().PadLeft(5, "0"))

                    If sCustomer = String.Empty _
                        Or sProductCode_K = String.Empty Then Continue For

                    'lấy customercode
                    Dim sql As String = String.Format("select CustomerCode from PD_MsCustomer where CustomerName = '{0}'", sCustomer)
                    Dim oCustomerCode As Object = DB.ExecuteScalar(sql)

                    If oCustomerCode Is Nothing Then
                        Throw New Exception(String.Format("Thông tin khách hàng của mã {0} không đúng", sProductCode_K))
                        Exit Sub
                    End If

                    Dim planningDate_K As DateTime = planDate
                    Dim sCustomerCode_K As String = oCustomerCode

                    obj.CustomerCode_K = sCustomerCode_K
                    obj.ProductCode_K = sProductCode_K
                    obj.PlanningDate_K = planningDate_K

                    DB.GetObject(obj)

                    If obj.ProductCode_K Is Nothing Then
                        SetValues4Import(obj, r, tbl.Columns(iCol), tbl.Columns(iCol + 1), tbl.Columns(iCol + 2), sProductCode_K, sCustomerCode_K, planningDate_K)
                        If obj.Quantity <> String.Empty Then
                            DB.Insert(obj)
                        End If
                    Else
                        Dim sCondition As String = String.Format("CustomerCode='{0}' And ProductCode='{1}' And PlanningDate='{2}'", sCustomerCode_K, sProductCode_K, planningDate_K.ToString("yyyy-MM-dd"))
                        SetValues4Import(obj, r, tbl.Columns(iCol), tbl.Columns(iCol + 1), tbl.Columns(iCol + 2), sProductCode_K, sCustomerCode_K, planningDate_K)
                        If obj.Quantity <> String.Empty Then
                            DB.Update(obj, sCondition)
                        Else
                            DB.Delete(obj)
                        End If
                    End If

                Next

                iCol += 3

            End While

            DB.Commit()

            Me.Cursor = Cursors.Arrow

            MessageBox.Show("Import data successfully", "Information!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)

            dtpFromDate.Value = lstDate(0)
            dtpToDate.Value = lstDate(lstDate.Count - 1)

            SetFormEvents = ActionForm.Refresh

        Catch ex As Exception
            Me.Cursor = Cursors.Arrow
            DB.RollBack()
            ShowError(ex, mnuImport.Text, Me.Name)
        End Try
    End Sub

    Private Function GetProcessNumChild(ByVal sProductCode As String, ByVal sComponentCode As String, ByVal scompleteprc As String) As String
        'lấy revision mới nhất
        Dim sRev As Object = dbFpics.ExecuteScalar(String.Format("SELECT RevisionCode = MAX(RevisionCode) FROM dbo.m_Product WHERE ProductCode = '{0}'", sProductCode))
        sRev = If(sRev Is Nothing Or sRev Is DBNull.Value, String.Empty, sRev)
        'lấy processnumber tương ứng
        Dim sql As String = String.Format(" select ProcessNumber from m_ComponentProcess cp " +
                                          " WHERE cp.ProductCode='{0}' And cp.RevisionCode='{1}' And cp.[ComponentCode] = '{2}' And cp.ProcessCode = '{3}'", _
                                          sProductCode, sRev, sComponentCode, scompleteprc)
        Dim tbl As DataTable = dbFpics.FillDataTable(sql)
        If tbl.Rows.Count = 0 Then Return String.Empty
        Return tbl.Rows(0)("ProcessNumber")
    End Function

    Private Sub mnuActResult_Click(sender As System.Object, e As System.EventArgs) Handles mnuActResult.Click
        Try
            Dim opd = New OpenFileDialog()
            opd.Multiselect = False
            opd.Filter = "File (*.*)|*.*|All file (*.*)|*.*"
            opd.Title = "Select a file to attach"

            If opd.ShowDialog() = DialogResult.Cancel Then
                Exit Sub
            End If

            Dim sfile As String = opd.FileName

            Dim app As New Excel.Application
            Dim wBook As Excel.Workbook = Nothing
            Dim wSheet As Excel.Worksheet = Nothing
            Dim wRange As Excel.Range = Nothing

            wBook = app.Workbooks.Open(opd.FileName, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing)

            app.Visible = False

            Me.Cursor = Cursors.WaitCursor

            'xử lý file _total
            Dim tblCompleteProcess As DataTable = DB.FillDataTable("select * from PD_CompleteProcess")
            Dim tblReportGroup As DataTable = DB.FillDataTable("select * from PD_MsReportGroup order by idx")

            'lấy thời gian bắt đầu và kết thúc của kế hoạch
            wSheet = CType(wBook.Sheets(2), Excel.Worksheet)
            Dim icolrun As Int32 = 4 '=3
            Dim fromDate As DateTime = CType(wSheet.Range(wSheet.Cells(1, icolrun), wSheet.Cells(1, icolrun)).Value, DateTime)
            Dim toDate As DateTime = fromDate
            Dim bendrun As Boolean = False
            icolrun += 1
            While Not bendrun
                Dim ovalue As Object = wSheet.Range(wSheet.Cells(1, icolrun), wSheet.Cells(1, icolrun)).Value
                Dim svalue As String = If(ovalue Is DBNull.Value Or ovalue Is Nothing, String.Empty, ovalue)
                If svalue = String.Empty Then : Exit While
                Else : toDate = CType(ovalue, DateTime)
                End If
                icolrun += 1
            End While
            '----------------------------------------------

            'lấy kết quả sản xuất
            Dim para(1) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@FromDate", fromDate.AddMonths(-6))
            para(1) = New SqlClient.SqlParameter("@ToDate", toDate.AddDays(7))
            Dim sqlresult As String = String.Format(
                                                    " SELECT ProductCode, LotNumber, ProcessCode, ProcessNumber " +
                                                    " FROM " +
                                                    " dbo.t_ProcessResult T WHERE EndDate>=@FromDate AND EndDate<@ToDate ")
            Dim tblresult As DataTable = dbFpics.FillDataTable(sqlresult, para)
            '----------------------------------------------

            For Each r As DataRow In tblReportGroup.Rows

                Try
                    wSheet = wBook.Sheets(r("ReportGroupName"))
                Catch ex As Exception
                    Continue For
                End Try
                wSheet.Select()

                'duyệt mã sản phẩm file excel
                'chú ý trường hợp S00 & S01

                Dim iCol As Int32 = 1
                Dim iRow As Int32 = 3

                Dim oProductCode As Object = wSheet.Range(wSheet.Cells(iRow, iCol), wSheet.Cells(iRow, iCol)).Value
                Dim sProductCode As String = If(oProductCode Is DBNull.Value Or oProductCode Is Nothing, String.Empty, oProductCode)

                While sProductCode <> "" And sProductCode <> "-"

                    sProductCode = Microsoft.VisualBasic.Left(sProductCode, 5).PadLeft(5, "0")

                    'lấy ra công đoạn kết thúc
                    Dim scolumn As String = r("ShortName")
                    Dim ocompleteprc As Object = (From p In tblCompleteProcess Where p("ProductCode") = sProductCode
                                             Select p(scolumn)).FirstOrDefault()
                    '------------------------------------------------------------

                    'nếu chưa thiết lập công đoạn kết thúc thì qua mã khác
                    If ocompleteprc Is Nothing Or ocompleteprc Is DBNull.Value Then
                        iRow += 1
                        oProductCode = wSheet.Range(wSheet.Cells(iRow, iCol), wSheet.Cells(iRow, iCol)).Value
                        sProductCode = If(oProductCode Is DBNull.Value Or oProductCode Is Nothing, String.Empty, oProductCode)
                        Continue While
                    End If
                    '------------------------------------------------------------

                    Dim scompleteprc As String = CType(ocompleteprc, String)

                    'tách công đoạn kết thúc (nếu có)
                    'tách dấu "-" là =
                    'tách dấu ">" là >=
                    Dim scommar As String = ""
                    Dim sprcnumber As String = ""
                    Dim arr() As String = scompleteprc.Split("-")
                    If arr.Length > 1 Then
                        scompleteprc = arr(0)
                        sprcnumber = arr(1)
                        scommar = "-"
                    Else
                        arr = scompleteprc.Split(">")
                        If arr.Length > 1 Then
                            scompleteprc = arr(0)
                            sprcnumber = arr(1)
                            scommar = ">"
                        End If
                    End If
                    '------------------------------------------------------------

                    'xử lý trường hợp catS00 & catC00
                    If r("ShortName") = "catS00" Then
                        'tìm processnumber của S00
                        sprcnumber = GetProcessNumChild(sProductCode, "S00", scompleteprc)
                        scommar = "-"
                    ElseIf r("ShortName") = "catC00" Then
                        'tìm processnumber của C00
                        sprcnumber = GetProcessNumChild(sProductCode, "C00", scompleteprc)
                        scommar = "-"
                    End If
                    '-----------------------------------

                    'duyệt lô mỗi ngày để tính số lô còn lại
                    Dim iColLot As Int32 = 4 '=3

                    'ngày kế hoạch
                    Dim oplandate As Object = wSheet.Range(wSheet.Cells(1, iColLot), wSheet.Cells(1, iColLot)).Value
                    Dim splandate As String = If(oplandate Is DBNull.Value Or oplandate Is Nothing, String.Empty, oplandate)
                    '-------------

                    'lô bắt đầu
                    Dim ostartlot As Object = wSheet.Range(wSheet.Cells(iRow, 3), wSheet.Cells(iRow, 3)).Value '=2
                    Dim istartlot As Int32 = If(ostartlot Is DBNull.Value Or ostartlot Is Nothing, 0, ostartlot)
                    '-------------

                    If istartlot = 0 Then
                        iRow += 1
                        oProductCode = wSheet.Range(wSheet.Cells(iRow, iCol), wSheet.Cells(iRow, iCol)).Value
                        sProductCode = If(oProductCode Is DBNull.Value Or oProductCode Is Nothing, String.Empty, oProductCode)
                        Continue While
                    End If

                    Dim istartlotlastest As Int32 = istartlot

                    Dim irunlot As Int32 = istartlot
                    Dim ifinishedlot As Int32 = 0

                    While splandate <> String.Empty And istartlot > 0

                        Dim oLotNumber As Object = wSheet.Range(wSheet.Cells(iRow, iColLot), wSheet.Cells(iRow, iColLot)).Value
                        Dim iLotNumber As Int32 = If(oLotNumber Is DBNull.Value Or oLotNumber Is Nothing, 0, oLotNumber)

                        While iLotNumber > 0 And irunlot <= (istartlot + iLotNumber) - 1
                            Dim sql As String = ""
                            Dim srunlot As String = irunlot.ToString().PadLeft(5, "0")
                            Dim icount As Int32 = 0
                            If scommar = "" Then
                                Dim rsresult() As DataRow = tblresult.Select(String.Format("ProductCode='{0}' AND LotNumber='{1}' AND ProcessCode ='{2}'", _
                                                                                           sProductCode, srunlot, scompleteprc))
                                icount = rsresult.Length
                            ElseIf scommar = "-" Then
                                Dim rsresult() As DataRow = tblresult.Select(String.Format("ProductCode='{0}' AND LotNumber='{1}' AND ProcessCode='{2}' AND ProcessNumber='{3}'", _
                                                                                            sProductCode, srunlot, scompleteprc, sprcnumber))
                                icount = rsresult.Length
                            ElseIf scommar = ">" Then
                                Dim rsresult() As DataRow = tblresult.Select(String.Format("ProductCode='{0}' AND LotNumber='{1}' AND ProcessCode='{2}' AND ProcessNumber='{3}'", _
                                                                                            sProductCode, srunlot, scompleteprc, sprcnumber))
                                icount = rsresult.Length
                            End If

                            If icount > 0 Then ifinishedlot += 1
                            irunlot += 1

                        End While

                        'cập nhật lại số lô
                        If iLotNumber > 0 And ifinishedlot > 0 Then
                            wSheet.Cells(iRow, iColLot) = iLotNumber - ifinishedlot
                            istartlotlastest += ifinishedlot
                        End If
                        ifinishedlot = 0
                        '------------------------------------------------------

                        iColLot += 1
                        istartlot = irunlot
                        oplandate = wSheet.Range(wSheet.Cells(1, iColLot), wSheet.Cells(1, iColLot)).Value
                        splandate = If(oplandate Is DBNull.Value Or oplandate Is Nothing, String.Empty, oplandate)
                    End While
                    '--------------------------------------

                    '//cập nhật lại startlot sau khi trừ ~ lô đã gia công
                    wSheet.Cells(iRow, 3) = istartlotlastest '=2
                    '//--------------------------------------------------

                    iRow += 1
                    oProductCode = wSheet.Range(wSheet.Cells(iRow, iCol), wSheet.Cells(iRow, iCol)).Value
                    sProductCode = If(oProductCode Is DBNull.Value Or oProductCode Is Nothing, String.Empty, oProductCode)

                End While
            Next

            MessageBox.Show("Get data successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            app.Visible = True

            SaveFileAndRelease(app, wRange, wSheet, wBook)
            Me.Cursor = Cursors.Arrow

        Catch ex As Exception
            Me.Cursor = Cursors.Arrow
            ShowError(ex, mnuActResult.Text, Me.Text)
        End Try
       
    End Sub

    Private _adjustWT As FrmAdjustWT

    Private Sub mnuAdjustWT_Click(sender As System.Object, e As System.EventArgs) Handles mnuAdjustWT.Click
        If _adjustWT IsNot Nothing Then _adjustWT.Close()
        _adjustWT = New FrmAdjustWT
        _adjustWT.Show()
    End Sub

End Class