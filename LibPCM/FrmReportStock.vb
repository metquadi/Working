﻿Imports CommonDB
Imports PublicUtility
Imports LibEntity
Imports DataGridViewAutoFilter

Imports System.Windows.Forms
Imports System.Text
Imports vb = Microsoft.VisualBasic
Imports System.Globalization
Imports System.Drawing
Imports Microsoft.Office.Interop
Imports System.IO

Public Class FrmReportStock
    Dim db As New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)
    Dim nvd As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    'Dim dbAS As New DBFunction(PublicConst.EnumServers.NDV_DB2_AS400)
    Dim dbAcc As DBSql = New DBSql(PublicConst.EnumServers.NDV_SQL_Factory)
    Dim cls As New clsApplication
    Public day As DateTime
    Dim dtAll As DataTable
    Dim FileTmp As String = Application.StartupPath + "\Template Excel\Template PCM\"
    Dim FileExp As String = Application.StartupPath + "\Template Excel\Template PCM\Export PCM\"


    Private Sub mnuShowAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuShowAll.Click
        dtAll = dtShowAll()
        Dim bd As New BindingSource
        bd.DataSource = dtAll
        gridD.DataSource = bd
        bnGrid.BindingSource = bd
    End Sub

    Function dtShowAll() As DataTable
        Dim sql As String = String.Format("SELECT  YMD , " +
        "JCode, " +
        "PrcName, " +
        "SubPrcName, " +
        "Cc, " +
        "PdCode, " +
        "EndLot, " +
        "StockWS, " +
        "StockAS, " +
        "JEName, " +
        "JVName, " +
        "Unit, " +
        "JLots " +
        "FROM {0} " +
        "WHERE   YMD = '{1}' " +
        "ORDER BY JCode , " +
        "PrcName , " +
        "PdCode", PublicTable.Table_PCM_GetEndLot, day.ToString("yyyyMMdd"))
        Return nvd.FillDataTable(sql)
    End Function

    Private Sub txtJCode_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtJCode.TextChanged
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

    Private Sub txtPdCode_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPdCode.TextChanged
        If dtAll Is Nothing Then
            Return
        Else
            Dim dv As DataView = New DataView(dtAll)
            dv.RowFilter = "[PdCode] LIKE '%" + txtPdCode.Text + "%'"
            Dim bd As New BindingSource()
            bd.DataSource = dv
            gridD.DataSource = bd
        End If
    End Sub

    Private Sub FrmReportStock_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("Do you want to close this form?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub gridD_CellPainting(sender As System.Object, e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles gridD.CellPainting
        If (e.ColumnIndex = gridD.Columns("StockAS").Index) AndAlso e.RowIndex <> -1 Then

            Using gridBrush As Brush = New SolidBrush(Me.gridD.GridColor), backColorBrush As Brush = New SolidBrush(e.CellStyle.BackColor)
                Using gridLinePen As Pen = New Pen(gridBrush)
                    ' Clear cell  
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds)
                    ' Draw line (bottom border and right border of current cell)  
                    'If next row cell has different content, only draw bottom border line of current cell  
                    If e.RowIndex <= gridD.Rows.Count - 2 AndAlso gridD.Rows(e.RowIndex + 1).Cells("JCode").Value.ToString() <> gridD.Rows(e.RowIndex).Cells("JCode").Value.ToString() Then
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                    End If
                    If e.RowIndex = gridD.Rows.Count - 1 Then
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                    End If
                    ' Draw right border line of current cell  
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)
                    ' draw/fill content in current cell, and fill only one cell of multiple same cells  
                    If Not e.Value Is Nothing And Not e.Value Is DBNull.Value Then
                        If e.RowIndex > 0 AndAlso gridD.Rows(e.RowIndex - 1).Cells("JCode").Value.ToString() = gridD.Rows(e.RowIndex).Cells("JCode").Value.ToString() Then

                        Else
                            e.Graphics.DrawString(CType(e.Value, String), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 5, StringFormat.GenericDefault)
                            'e.CellStyle.re()
                        End If
                    End If
                    e.Handled = True
                End Using
            End Using
        End If

        If (e.ColumnIndex = gridD.Columns("StockWS").Index) AndAlso e.RowIndex <> -1 Then

            Using gridBrush As Brush = New SolidBrush(Me.gridD.GridColor), backColorBrush As Brush = New SolidBrush(e.CellStyle.BackColor)
                Using gridLinePen As Pen = New Pen(gridBrush)
                    ' Clear cell  
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds)
                    ' Draw line (bottom border and right border of current cell)  
                    'If next row cell has different content, only draw bottom border line of current cell  
                    If e.RowIndex <= gridD.Rows.Count - 2 AndAlso (gridD.Rows(e.RowIndex + 1).Cells("JCode").Value.ToString() <> gridD.Rows(e.RowIndex).Cells("JCode").Value.ToString() _
                        Or gridD.Rows(e.RowIndex + 1).Cells("PrcName").Value.ToString() <> gridD.Rows(e.RowIndex).Cells("PrcName").Value.ToString()) Then
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                    End If

                    If e.RowIndex = gridD.Rows.Count - 1 Then
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                    End If

                    ' Draw right border line of current cell  
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)
                    ' draw/fill content in current cell, and fill only one cell of multiple same cells  
                    If Not e.Value Is Nothing And Not e.Value Is DBNull.Value Then
                        If e.RowIndex > 0 AndAlso (gridD.Rows(e.RowIndex - 1).Cells("JCode").Value.ToString() = gridD.Rows(e.RowIndex).Cells("JCode").Value.ToString() _
                            And gridD.Rows(e.RowIndex - 1).Cells("PrcName").Value.ToString() = gridD.Rows(e.RowIndex).Cells("PrcName").Value.ToString()) Then
                        Else
                            e.Graphics.DrawString(CType(e.Value, String), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 5, StringFormat.GenericDefault)
                            'e.CellStyle.re()
                        End If
                    End If
                    e.Handled = True
                End Using
            End Using
        End If

    End Sub

    Private Sub FrmReportStock_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        gridD.AutoGenerateColumns = False
        mnuShowAll.PerformClick()
        cboJLots.Items.Add("")
        cboJLots.Items.Add("<>0")
        cboJLots.Items.Add("=0")
    End Sub

    Private Sub nmuGetEndLot_Click(sender As System.Object, e As System.EventArgs) Handles nmuGetEndLot.Click
        Dim posRow As Integer = 0
        Try
            Dim sqlInput As String = String.Format("Select top 1 JCode from {0} where left(YMD, 8) = '{1}'", PublicTable.Table_PCM_WorkshopOut, day.ToString("yyyyMMdd"))
            Dim dtInput As DataTable = nvd.FillDataTable(sqlInput)
            If dtInput.Rows.Count = 0 Then Exit Sub

            nvd.BeginTransaction()
            Me.Cursor = Cursors.WaitCursor
            Dim param(1) As SqlClient.SqlParameter
            param(0) = New SqlClient.SqlParameter("@dayInputStockWS", day.ToString("yyyyMMdd"))
            param(1) = New SqlClient.SqlParameter("@dayDailyStock", day.AddDays(-1).ToString("yyyyMMdd"))
            Dim dt As DataTable = nvd.ExecuteStoreProcedureTB("sp_PCM_GetEndLot", param)

            Dim dayFpics As String = day.AddMonths(-3).ToString("yyyy-MM-dd")
            Dim dayNow As String = day.ToString("yyyy-MM-dd 06:00:00")

            Dim sqlDelete As String = String.Format("DELETE FROM {0} WHERE YMD = '{1}'", PublicTable.Table_PCM_GetEndLot, day.ToString("yyyyMMdd"))
            nvd.ExecuteNonQuery(sqlDelete)

            If dt.Rows.Count <> 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    posRow += 1
                    Application.DoEvents()
                    Dim PdCode As String = dt.Rows(i)("PdCode").ToString()
                    Dim Cc As String = ""
                    Cc = Trim(dt.Rows(i)("Cc").ToString())

                    Dim SubPrcName As String = vb.Right(dt.Rows(i)("SubPrcName"), 4)
                    'If vb.Right(dt.Rows(i)("SubPrcName"), 4) <> "1003" Then
                    '    SubPrcName = vb.Right(dt.Rows(i)("SubPrcName"), 4)
                    'Else
                    '    If vb.Left(Cc, 1) = "C" Then
                    '        SubPrcName = "8041"
                    '    ElseIf vb.Left(Cc, 1) = "S" Then
                    '        SubPrcName = "8047"
                    '    ElseIf vb.Left(Cc, 1) = "T" Then
                    '        SubPrcName = "8045"
                    '    End If
                    'End If

                    'Check PdCode has exist winthin 3 months
                    Dim EndLot As Integer = 0
                    Dim MaxLotB00 As String = ""
                    Dim CountLotB00 As Integer = 0
                    Dim MaxLot As String = ""
                    Dim CountLot As Integer = 0
                    If PdCode <> "" Then
                        Dim sqlchkPd As String = String.Format("SELECT  top 1 " +
                                            "ProductCode " +
                                            "FROM t_ProcessResult_99 " +
                                            "WHERE   EndDate >= '{0}' " +
                                            "AND EndDate <= '{1}' " +
                                            "AND ProductCode = '{2}'", dayFpics, dayNow, PdCode)
                        Dim dtchkPd As DataTable = db.FillDataTable(sqlchkPd)
                        If dtchkPd.Rows.Count = 0 Then Continue For

                        Dim sqlLotB00 As String = String.Format("SELECT MAX(LotNumber) MaxLotB00, Count(LotNumber) CountLotB00 " +
                        "FROM t_ProcessResult_99 " +
                        "WHERE   ProductCode = '{0}' " +
                        "AND EndDate <= '{1}' " +
                        "AND ProcessCode = '9051'", PdCode, dayNow)
                        Dim dtLotB00 As DataTable = db.FillDataTable(sqlLotB00)
                        If dtLotB00.Rows.Count <> 0 Then
                            MaxLotB00 = IIf(dtLotB00.Rows(0)("MaxLotB00") Is DBNull.Value, "0000", dtLotB00.Rows(0)("MaxLotB00"))
                            CountLotB00 = IIf(dtLotB00.Rows(0)("CountLotB00") Is DBNull.Value, 0, dtLotB00.Rows(0)("CountLotB00"))
                        End If

                        Dim sqlLot As String = String.Format("Select Max(LotNumber) MaxLot, Count(LotNumber) CountLot " +
                        "FROM    t_ProcessResult_99 " +
                        "WHERE   ProductCode = '{0}' " +
                        "AND ProcessCode = '{2}' " +
                        "AND ComponentCode = '{3}' " +
                        "AND EndDate <= '{1}' " +
                        "AND LotNumber > '{4}'", PdCode, dayNow, _
                        SubPrcName, Cc, MaxLotB00)
                        Dim dtLot As DataTable = db.FillDataTable(sqlLot)
                        If dtLot.Rows.Count <> 0 Then
                            MaxLot = IIf(dtLot.Rows(0)("MaxLot") Is DBNull.Value, MaxLotB00, dtLot.Rows(0)("MaxLot"))
                            CountLot = IIf(dtLot.Rows(0)("CountLot") Is DBNull.Value, 0, dtLot.Rows(0)("CountLot"))
                        End If
                        EndLot = CountLotB00 + CountLot
                    End If

                    Dim objGetLot As New PCM_GetEndLot
                    objGetLot.YMD_K = day.ToString("yyyyMMdd")
                    objGetLot.EndLot = EndLot
                    objGetLot.JCode_K = dt.Rows(i)("JCode")
                    objGetLot.PrcName_K = dt.Rows(i)("PrcName")
                    objGetLot.SubPrcName_K = dt.Rows(i)("SubPrcName")
                    objGetLot.Cc_K = dt.Rows(i)("Cc")
                    objGetLot.PdCode_K = dt.Rows(i)("PdCode")
                    objGetLot.StockWS = dt.Rows(i)("Stock")
                    objGetLot.StockAS = dt.Rows(i)("StockAS")
                    objGetLot.JEName = dt.Rows(i)("JEName")
                    objGetLot.JVName = dt.Rows(i)("JVName")
                    objGetLot.Unit = dt.Rows(i)("Unit")
                    objGetLot.JLots = CInt(MaxLot) - EndLot
                    nvd.Insert(objGetLot)

                Next
            End If
            MessageBox.Show("Finished", "Show All")
            dtAll = dtShowAll()
            Dim bd As New BindingSource
            bd.DataSource = dtAll
            gridD.DataSource = bd
            bnGrid.BindingSource = bd

            nvd.Commit()
            Me.Cursor = Cursors.Arrow
        Catch ex As Exception
            nvd.RollBack()
            Me.Cursor = Cursors.Arrow
            MessageBox.Show(posRow & " " & ex.Message, "Show All")
        End Try
    End Sub

    Private Sub mnuExport_Click(sender As System.Object, e As System.EventArgs) Handles mnuExport.Click
        Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        'Copy template
        UpLoadFile(FileTmp + "GetEndLot.xlsx", FileExp + "GetEndLot.xlsx", True)
        'Variance
        Dim app As New Excel.Application
        Dim workBook As Excel.Workbook
        Dim workSheet As Excel.Worksheet
        Dim workRange As Excel.Range
        'Open file
        'app.DisplayAlerts = False
        app.Visible = False

        If (File.Exists(FileExp + "GetEndLot.xlsx")) Then
            workBook = app.Workbooks.Open(FileExp + "GetEndLot.xlsx")
        Else
            workBook = app.Workbooks.Add(Type.Missing)
        End If
        workSheet = CType(workBook.Sheets(1), Excel.Worksheet)
        workSheet.Name = "GetEndLot"

        If gridD.Rows.Count = 0 Then Exit Sub

        'Detail
        Dim changeColor As Integer = 1
        Dim iRow As Integer = 2
        Dim No As Integer = 1

        Dim MergeStart As Integer = 2
        Dim MergeEnd As Integer = 2

        Dim MergeStartWS As Integer = 2
        Dim MergeEndWS As Integer = 2

        Me.Cursor = Cursors.WaitCursor
        For i As Integer = 0 To gridD.Rows.Count - 1
            Application.DoEvents()

            workSheet.Cells(iRow, 1) = i + 1

            If iRow > 2 Then 'Merge
                'Merge Stock WS
                If gridD.Rows(i).Cells("JCode").Value <> gridD.Rows(i - 1).Cells("JCode").Value Or gridD.Rows(i).Cells("PrcName").Value <> gridD.Rows(i - 1).Cells("PrcName").Value Then
                    workSheet.Cells(iRow, 8) = gridD.Rows(i).Cells("StockWS").Value
                    MergeStartWS = MergeEndWS
                    MergeEndWS = iRow - 1
                ElseIf i = gridD.Rows.Count - 1 Then
                    MergeStartWS = MergeEndWS
                    MergeEndWS = iRow
                End If

                If gridD.Rows(i).Cells("JCode").Value <> gridD.Rows(i - 1).Cells("JCode").Value Or gridD.Rows(i).Cells("PrcName").Value <> gridD.Rows(i - 1).Cells("PrcName").Value Or i = gridD.Rows.Count - 1 Then
                    workRange = workSheet.Range(workSheet.Cells(MergeStartWS, 8), workSheet.Cells(MergeEndWS, 8))
                    workRange.Merge(Type.Missing)
                    workRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop

                    'workSheet.Range("A1:L1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                    If changeColor Mod 2 <> 0 Then
                        workSheet.Range("A" & MergeStartWS & ":" & "L" & MergeEndWS).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen)
                    End If
                    changeColor += 1
                    MergeEndWS += 1
                End If

                'Merge Stock AS
                If gridD.Rows(i).Cells("JCode").Value <> gridD.Rows(i - 1).Cells("JCode").Value Then
                    workSheet.Cells(iRow, 9) = gridD.Rows(i).Cells("StockAS").Value
                    MergeStart = MergeEnd
                    MergeEnd = iRow - 1
                ElseIf i = gridD.Rows.Count - 1 Then
                    MergeStart = MergeEnd
                    MergeEnd = iRow
                End If

                If gridD.Rows(i).Cells("JCode").Value <> gridD.Rows(i - 1).Cells("JCode").Value Or i = gridD.Rows.Count - 1 Then
                    workRange = workSheet.Range(workSheet.Cells(MergeStart, 9), workSheet.Cells(MergeEnd, 9))
                    workRange.Merge(Type.Missing)
                    workRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop

                    MergeEnd += 1
                End If

            Else
                workSheet.Cells(iRow, 8) = gridD.Rows(i).Cells("StockWS").Value
                workSheet.Cells(iRow, 9) = gridD.Rows(i).Cells("StockAS").Value
            End If

            workSheet.Cells(iRow, 2) = gridD.Rows(i).Cells("JCode").Value
            workSheet.Cells(iRow, 3) = gridD.Rows(i).Cells("PrcName").Value
            workSheet.Cells(iRow, 4) = gridD.Rows(i).Cells("SubPrcName").Value
            workSheet.Cells(iRow, 5) = gridD.Rows(i).Cells("Cc").Value
            workSheet.Cells(iRow, 6) = gridD.Rows(i).Cells("PdCode").Value
            workSheet.Cells(iRow, 7) = gridD.Rows(i).Cells("EndLot").Value

            workSheet.Cells(iRow, 10) = gridD.Rows(i).Cells("JEName").Value
            workSheet.Cells(iRow, 11) = gridD.Rows(i).Cells("JVName").Value
            workSheet.Cells(iRow, 12) = gridD.Rows(i).Cells("Unit").Value

            No += 1
            iRow += 1
        Next
        Me.Cursor = Cursors.Arrow

        workBook.Save()
        workBook.Close()
        app.Quit()
        MessageBox.Show("Export successfully!", "Export Excel")
        app.Visible = True
        app.Workbooks.Open(FileExp + "GetEndLot.xlsx")
        System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

    End Sub

    Private Sub mnuJLots_Click(sender As System.Object, e As System.EventArgs) Handles mnuJLots.Click
        If gridD.CurrentRow Is Nothing Then Exit Sub
        Dim dayNow As String = day.ToString("yyyy-MM-dd 06:00:00")

        Dim sqlMaxLotB00 As String = String.Format("SELECT  MAX(LotNumber) MaxLotB00 " +
        "FROM t_ProcessResult_99 " +
        "WHERE   ProductCode = '{0}' " +
        "AND EndDate <= '{1}' " +
        "AND ProcessCode = '9051'", gridD.CurrentRow.Cells("PdCode").Value, dayNow)
        Dim MaxLotB00 = db.FillDataTable(sqlMaxLotB00).Rows(0).Item("MaxLotB00")

        Dim sql As String = String.Format("SELECT  LotNumber " +
        "FROM t_ProcessResult_99 " +
        "WHERE   ProductCode = '{0}' " +
        "AND EndDate <= '{1}' " +
        "AND ProcessCode = '9051' " +
        "UNION ALL " +
        "Select LotNumber " +
        "FROM    t_ProcessResult_99 " +
        "WHERE   ProductCode = '{0}' " +
        "AND ProcessCode = '{2}' " +
        "AND ComponentCode = '{3}' " +
        "AND EndDate <= '{1}' " +
        "AND LotNumber > '{4}'", gridD.CurrentRow.Cells("PdCode").Value, dayNow, _
        vb.Right(gridD.CurrentRow.Cells("SubPrcName").Value, 4), gridD.CurrentRow.Cells("Cc").Value, MaxLotB00)
        Dim dt As DataTable = db.FillDataTable(Sql)

        Dim lstJumpLot As String = ""
        If dt.Rows.Count <> 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                If i <> 0 Then
                    Dim subLot As Integer = CInt(dt.Rows(i)("LotNumber")) - CInt(dt.Rows(i - 1)("LotNumber"))
                    If subLot > 1 Then
                        For j As Integer = 1 To subLot - 1
                            lstJumpLot = lstJumpLot & (dt.Rows(i - 1)("LotNumber") + j) & " "
                        Next
                    End If
                End If
            Next
        End If
        MessageBox.Show("Jumped Lots: " & lstJumpLot, "Jumped Lots")
    End Sub

    Private Sub gridD_RowPrePaint(sender As System.Object, e As System.Windows.Forms.DataGridViewRowPrePaintEventArgs) Handles gridD.RowPrePaint
        If gridD.Rows(e.RowIndex).Cells("JLots").Value Is DBNull.Value Then
            gridD.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.White
        ElseIf gridD.Rows(e.RowIndex).Cells("JLots").Value > 0 Then
            gridD.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Yellow
        Else
            gridD.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.White
        End If
    End Sub

    Private Sub cboJLots_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboJLots.SelectedIndexChanged
        If dtAll Is Nothing Then
            Return
        Else
            Dim dv As DataView = New DataView(dtAll)
            If cboJLots.Text = "<>0" Or cboJLots.Text = "=0" Then
                dv.RowFilter = "JLots " + Trim(cboJLots.Text)
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
End Class