﻿Imports CommonDB
Imports LibEntity
Imports PublicUtility
Imports LibFpicsDataView

Public Class FrmPlanHourNew : Inherits DevExpress.XtraEditors.XtraForm

    Private DB As DBSql
    Private _dbFPics As New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)
    Private bs As BindingSource
    Private _isEdit As Boolean = False
    Private notWorkSunday As String = "or DATEPART(DW, HolidayDate)=1"

    Private EnuActionForm As ActionForm = ActionForm.FormLoad

    Enum ActionForm
        None = 0
        FormLoad = 1
        Edit = 2
        Export = 3
        Run = 4
    End Enum

    ReadOnly Property GetFormEvents As ActionForm
        Get
            Return EnuActionForm
        End Get
    End Property

    WriteOnly Property SetFormEvents As ActionForm
        Set(ByVal value As ActionForm)
            EnuActionForm = value
            gridP.ReadOnly = True
            gridP.AllowUserToAddRows = False
            Me.Cursor = Cursors.WaitCursor
            Try
                Select Case value
                    Case ActionForm.Edit
                        gridP.ReadOnly = False
                        SetReadOnlyGrid()
                    Case ActionForm.FormLoad

                End Select
                Me.Cursor = Cursors.Arrow
            Catch ex As Exception
                Me.Cursor = Cursors.Arrow
                ShowError(ex, "SetFormEvents", Me.Name)
            End Try
        End Set
    End Property

#Region "User Function"

    Private Sub SetReadOnlyGrid()
        gridP.Columns(RemarkP.Name).ReadOnly = False
        gridP.Columns(LotListP.Name).ReadOnly = False
        gridP.Columns(TGGC.Name).ReadOnly = False
		gridP.Columns(SLThietBi.Name).ReadOnly = False
		gridP.Columns(SlThietBiA.Name).ReadOnly = False
		gridP.Columns(LeadtimeSub.Name).ReadOnly = False
		gridP.Columns(TGDung.Name).ReadOnly = False

		gridP.Columns(ProductCodeP.Name).ReadOnly = True
		gridP.Columns(RevisionCode.Name).ReadOnly = True
		gridP.Columns(ProcessCodeP.Name).ReadOnly = True
		gridP.Columns(ProcessNameP.Name).ReadOnly = True
		gridP.Columns(ProcessNumberP.Name).ReadOnly = True

		gridP.Columns(leadtimeTDG.Name).ReadOnly = False
		gridP.Columns(LeadtimeThietBi.Name).ReadOnly = True
	End Sub
     

#End Region

#Region "Form Function"

    Private Sub FrmPlanHour_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        DB = New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
        SetFormEvents = ActionForm.FormLoad
    End Sub

    Private Sub FrmPlanHour_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 And mnuRun.Enabled Then
            mnuRun.PerformClick()
        End If
        If e.KeyCode = Keys.E And e.Control And mnuEdit.Enabled Then
            mnuEdit.PerformClick()
        End If
    End Sub

    'clean grid
    Private Sub CleanGrid()
        Try
            gridP.EndEdit()
            Me.bs.EndEdit()
            'Duyệt từng lô (cột) & từng dòng
            'Có sự khác nhau công thức giữa lô đầu tiên và các lô còn lại
            Dim tbl As DataTable = CType(CType(gridP.DataSource, BindingSource).DataSource, DataTable)
            For Each col As DataColumn In tbl.Columns
                If Not col.ColumnName.Contains("TGKT") Then Continue For
                Dim bFirstValue As Boolean = True
                For Each r As DataRow In tbl.Rows
                    If bFirstValue Then If r(col.ColumnName) Is DBNull.Value Then Continue For
                    If bFirstValue Then
                        bFirstValue = False
                        Continue For
                    End If
                    r(col.ColumnName) = DBNull.Value
                Next
            Next
            tbl.AcceptChanges()
        Catch ex As Exception
            ShowError(ex, mnuRun.Text, Me.Text)
        Finally
            SetFormEvents = ActionForm.Run
        End Try
    End Sub

    Sub RunAll(ByVal dtRes As DataTable)
        Dim lstRestDate As List(Of DataRow) = dtRes.AsEnumerable().OrderBy(Function(r) r("FromDate")).ToList()

        'Try
        gridP.EndEdit()
        Me.bs.EndEdit()
        'Duyệt từng lô (cột) & từng dòng
        'So sánh giữa TGGC & TGKT
        'Có sự khác nhau công thức giữa lô đầu tiên và các lô còn lại
        Dim tbl As DataTable = CType(CType(gridP.DataSource, BindingSource).DataSource, DataTable)
        Dim bFirstLot As Boolean = True
        Dim colBefore As DataColumn = Nothing
        For Each col As DataColumn In tbl.Columns
            If Not col.ColumnName.Contains("TGKT") Then Continue For
            Dim bFirstValue As Boolean = True
            Dim tgkt As Nullable(Of DateTime) = Nothing
            Dim time_remain As Int32 = 0 'remain của cột hiện tại
            Dim time_remain_bf As Int32 = 0 'remain của cột trước đó
            Dim myLot As String = Microsoft.VisualBasic.Left(col.ColumnName, 5)
            For Each r As DataRow In tbl.Rows
                'Tìm giá trị đầu tiên của cột
                'Kiểm tra thời gian kết thúc & cộng tiếp leadtime
                'If r("ProcessNumber") = "40" Then
                '    ShowWarning("OK")
                'End If
                Dim mLT As Decimal = 0
                Dim mLTFirst As Decimal = 0
                If IsNumeric(r(leadtimeTDG.DataPropertyName)) Then
                    mLT += r(leadtimeTDG.DataPropertyName)
                    mLTFirst += mLT
                End If

                If IsNumeric(r(LeadtimeSub.DataPropertyName)) Then
                    If r(LotListP.DataPropertyName) <> "" Then
                        If r(LotListP.DataPropertyName).ToString.Contains(myLot) Then
                            mLTFirst += r(LeadtimeSub.DataPropertyName)
                            mLT += r(LeadtimeSub.DataPropertyName)
                        End If
                    Else
                        mLTFirst += r(LeadtimeSub.DataPropertyName)
                        mLT += r(LeadtimeSub.DataPropertyName)
                    End If
                End If

                If bFirstValue Then If r(col.ColumnName) Is DBNull.Value Then Continue For
                If bFirstValue Then
                    tgkt = CType(r(col.ColumnName), Date)
                    bFirstValue = False
                    Continue For
                End If
                If bFirstLot Then 'xử lý trường hợp lô đầu tiên
                    Dim tgkt_2check As DateTime = tgkt.Value.AddHours(mLTFirst)
                    Dim myTime As Integer = mLTFirst * 60
                    'kiểm tra có phải ngày nghỉ không
                    'nếu là ngày nghỉ thì bỏ qua
                    Dim bfirst_sub As Boolean = True
                    While True
                        Dim sRestTime = (From p In lstRestDate
                                         Where (p("FromDate") <= tgkt And tgkt <= p("ToDate")) And
                                             (p("PN") = "00" Or p("PN") = r("ProcessNumber"))
                                         Select p).FirstOrDefault()
                        If sRestTime Is Nothing Then
                            myTime -= 1
                            If myTime <= 0 Then
                                r(col.ColumnName) = tgkt
                                Exit While
                            End If
                        End If
                        tgkt = CType(tgkt, Date).AddMinutes(1)
                    End While
                Else 'xử lý trường hợp lô tiếp theo
                    'tính tgkt 
                    Dim myTime As Integer = mLT * 60
                    'kiểm tra có phải ngày nghỉ không
                    'nếu là ngày nghỉ thì bỏ qua
                    Dim bfirst_sub As Boolean = True
                    While True
                        Dim sRestTime = (From p In lstRestDate
                                         Where (p("FromDate") <= tgkt And tgkt <= p("ToDate")) And
                                             (p("PN") = "00" Or p("PN") = r("ProcessNumber"))
                                         Select p).FirstOrDefault()
                        If sRestTime Is Nothing Then
                            myTime -= 1
                            If myTime <= 0 Then
                                r(col.ColumnName) = tgkt
                                Exit While
                            End If
                        End If
                        tgkt = CType(tgkt, Date).AddMinutes(1)
                    End While
                    '-----------------
                    bfirst_sub = True
                    Dim sProcess As String = CType(r(ProcessNameP.DataPropertyName), String)
                    If _
                        sProcess = "Lam Press (Vacuum)" _
                        Or sProcess = "Steel Rule Die_B/S" _
                        Or sProcess = "Curing" _
                        Or sProcess = "Autoclave No Make" Then
                        r(col.ColumnName) = tgkt
                    Else
                        'tính tgkt_before
                        Dim tgkt_before As Nullable(Of DateTime) = Nothing
                        If r(colBefore.ColumnName) IsNot DBNull.Value Then
                            tgkt_before = CType(r(colBefore.ColumnName), DateTime)
                            myTime = mLT * 60
                            'kiểm tra có phải ngày nghỉ không
                            'nếu là ngày nghỉ thì bỏ qua
                            Dim bfirst_sub_bf As Boolean = True
                            While True
                                Dim sRestTime = (From p In lstRestDate
                                                 Where (p("FromDate") <= tgkt_before And tgkt_before <= p("ToDate")) And
                                             (p("PN") = "00" Or p("PN") = r("ProcessNumber"))
                                                 Select p).FirstOrDefault()
                                If sRestTime Is Nothing Then
                                    myTime -= 1
                                    If myTime <= 0 Then
                                        Exit While
                                    End If
                                End If
                                tgkt_before = CType(tgkt_before, Date).AddMinutes(1)
                            End While
                        End If
                        '--------------
                        If tgkt_before Is Nothing Then
                            r(col.ColumnName) = tgkt
                        Else
                            r(col.ColumnName) = If(tgkt > tgkt_before, tgkt, tgkt_before)
                        End If
                    End If
                    tgkt = r(col.ColumnName)
                    'r(col.ColumnName.Replace("Leadtime", "Date")) = tgkt
                End If
            Next
            If bFirstLot Then
                bFirstLot = False
            End If
            colBefore = col
            Application.DoEvents()
        Next
        'Catch ex As Exception
        '    ShowError(ex, mnuRun.Text, Me.Text)
        'Finally
        '    SetFormEvents = ActionForm.Run
        'End Try
    End Sub
    'Không làm ngày nghỉ 3 ca
    Private Sub KNN_3Ca()
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))

        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate",
                                    PublicTable.Table_FPICS_HolidayDate)

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
        Dim mDate As DateTime = dtpPlanDate.Value.Date

        'While mDate <= dtpPlanDate.Value.AddDays(10)
        '    mrow = dtRes.NewRow
        '    Dim objH As New FPICS_HolidayDate
        '    objH.HolidayDate_K = mDate
        '    If Not DB.ExistObject(objH) Then
        '    End If
        '    mDate = mDate.AddDays(1)
        'End While

        For Each r As DataGridViewRow In gridP.Rows
            If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then '07:30-16:00 19/05
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then '07:30-16:00
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
        Next

        RunAll(dtRes)

    End Sub

    Private Sub KNN_3Ca_BK()
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))

        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate",
                                    PublicTable.Table_FPICS_HolidayDate)

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
        Dim mDate As DateTime = dtpPlanDate.Value.Date

        'While mDate <= dtpPlanDate.Value.AddDays(10)
        '    mrow = dtRes.NewRow
        '    Dim objH As New FPICS_HolidayDate
        '    objH.HolidayDate_K = mDate
        '    If Not DB.ExistObject(objH) Then
        '    End If
        '    mDate = mDate.AddDays(1)
        'End While

        For Each r As DataGridViewRow In gridP.Rows
            If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then '07:30-16:00 19/05
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then '07:30-16:00
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
        Next


        Dim lstRestDate As List(Of DataRow) = dtRes.AsEnumerable().OrderBy(Function(r) r("FromDate")).ToList()

        'Try
        gridP.EndEdit()
        Me.bs.EndEdit()
        'Duyệt từng lô (cột) & từng dòng
        'So sánh giữa TGGC & TGKT
        'Có sự khác nhau công thức giữa lô đầu tiên và các lô còn lại
        Dim tbl As DataTable = CType(CType(gridP.DataSource, BindingSource).DataSource, DataTable)
        Dim bFirstLot As Boolean = True
        Dim colBefore As DataColumn = Nothing
        For Each col As DataColumn In tbl.Columns
            If Not col.ColumnName.Contains("TGKT") Then Continue For
            Dim bFirstValue As Boolean = True
            Dim tgkt As Nullable(Of DateTime) = Nothing
            Dim time_remain As Int32 = 0 'remain của cột hiện tại
            Dim time_remain_bf As Int32 = 0 'remain của cột trước đó
            Dim myLot As String = Microsoft.VisualBasic.Left(col.ColumnName, 5)
            For Each r As DataRow In tbl.Rows
                'Tìm giá trị đầu tiên của cột
                'Kiểm tra thời gian kết thúc & cộng tiếp leadtime
                If r("ProcessNumber") = "40" Then
                    ShowWarning("OK")
                End If
                Dim mLT As Decimal = 0
                Dim mLTFirst As Decimal = 0
                If IsNumeric(r(leadtimeTDG.DataPropertyName)) Then
                    mLT += r(leadtimeTDG.DataPropertyName)
                    mLTFirst += mLT
                End If

                If IsNumeric(r(LeadtimeSub.DataPropertyName)) Then
                    If r(LotListP.DataPropertyName) <> "" Then
                        If r(LotListP.DataPropertyName).ToString.Contains(myLot) Then
                            mLTFirst += r(LeadtimeSub.DataPropertyName)
                            mLT += r(LeadtimeSub.DataPropertyName)
                        End If
                    Else
                        mLTFirst += r(LeadtimeSub.DataPropertyName)
                        mLT += r(LeadtimeSub.DataPropertyName)
                    End If
                End If

                If bFirstValue Then If r(col.ColumnName) Is DBNull.Value Then Continue For
                If bFirstValue Then
                    tgkt = r(col.ColumnName)
                    bFirstValue = False
                    Continue For
                End If
                If bFirstLot Then 'xử lý trường hợp lô đầu tiên
                    Dim tgkt_2check As DateTime = tgkt.Value.AddHours(mLTFirst)
                    'kiểm tra có phải ngày nghỉ không
                    'nếu là ngày nghỉ thì bỏ qua
                    Dim bfirst_sub As Boolean = True
                    While True
                        Dim sRestTime = (From p In lstRestDate
                                         Where ((p("FromDate") <= tgkt And tgkt <= p("ToDate")) Or
                                             (tgkt <= p("FromDate") And p("FromDate") <= tgkt_2check) Or
                                             (tgkt <= p("ToDate") And p("ToDate") <= tgkt_2check)) And
                                             (p("PN") = "00" Or p("PN") = r("ProcessNumber"))
                                         Select p).FirstOrDefault()
                        If sRestTime Is Nothing Then 'nếu không phải ngày nghỉ
                            If time_remain > 0 Then
                                tgkt = tgkt.Value.AddMinutes(time_remain)
                                time_remain = 0
                            Else
                                tgkt = tgkt.Value.AddMinutes(60 * mLTFirst)
                            End If
                            r(col.ColumnName) = tgkt
                            'r(col.ColumnName.Replace("Leadtime", "Date")) = tgkt
                            Exit While
                        Else 'nếu là ngày nghỉ
                            'tính lại thời gian gia công cho tới lúc bắt đầu nghỉ
                            'chỉ tính khi gặp ngày nghỉ đầu tiên (ngày liền kề tiếp theo không tính)
                            If bfirst_sub Then
                                Dim cur_time_minute As Int32 = CType(mLTFirst, Decimal) * 60 'tính giờ ra phút
                                Dim timespn As TimeSpan = CType(sRestTime("FromDate"), Date) - tgkt
                                If timespn.TotalMinutes > 0 Then
                                    time_remain = cur_time_minute - timespn.TotalMinutes
                                End If
                                bfirst_sub = False
                            End If
                            tgkt_2check = CType(sRestTime("ToDate"), Date).AddMinutes(1)
                            tgkt = CType(sRestTime("ToDate"), Date).AddMinutes(1)
                        End If
                    End While
                Else 'xử lý trường hợp lô tiếp theo
                    'tính tgkt
                    Dim tgkt_2check As Date = tgkt.Value.AddHours(mLT)
                    'kiểm tra có phải ngày nghỉ không
                    'nếu là ngày nghỉ thì bỏ qua
                    Dim bfirst_sub As Boolean = True
                    While True
                        Dim sRestTime = (From p In lstRestDate
                                         Where ((p("FromDate") <= tgkt And tgkt <= p("ToDate")) Or
                                             (tgkt <= p("FromDate") And p("FromDate") <= tgkt_2check) Or
                                             (tgkt <= p("ToDate") And p("ToDate") <= tgkt_2check)) And
                                             (p("PN") = "00" Or p("PN") = r("ProcessNumber"))
                                         Select p).FirstOrDefault()
                        If sRestTime Is Nothing Then 'nếu không phải ngày nghỉ
                            If time_remain > 0 Then
                                tgkt = tgkt.Value.AddMinutes(time_remain)
                                time_remain = 0
                            Else
                                tgkt = tgkt.Value.AddHours(mLT)
                            End If
                            Exit While
                        Else 'nếu là ngày nghỉ
                            'tính lại thời gian gia công cho tới lúc bắt đầu nghỉ
                            'chỉ tính khi gặp ngày nghỉ đầu tiên (ngày liền kề tiếp theo không tính)
                            If bfirst_sub Then
                                Dim cur_time_minute As Int32 = CType(mLT, Decimal) * 60 'tính giờ ra phút
                                Dim timespn As TimeSpan = CType(sRestTime("FromDate"), DateTime) - tgkt
                                If timespn.TotalMinutes > 0 Then
                                    time_remain = cur_time_minute - timespn.TotalMinutes
                                End If
                                bfirst_sub = False
                            End If
                            tgkt_2check = CType(sRestTime("ToDate"), DateTime).AddMinutes(1)
                            tgkt = CType(sRestTime("ToDate"), DateTime).AddMinutes(1)
                        End If
                    End While
                    '-----------------
                    bfirst_sub = True
                    Dim sProcess As String = CType(r(ProcessNameP.DataPropertyName), String)
                    If _
                        sProcess = "Lam Press (Vacuum)" _
                        Or sProcess = "Steel Rule Die_B/S" _
                        Or sProcess = "Curing" _
                        Or sProcess = "Autoclave No Make" Then
                        r(col.ColumnName) = tgkt
                    Else
                        'tính tgkt_before
                        Dim tgkt_before As Nullable(Of DateTime) = Nothing
                        If r(colBefore.ColumnName) IsNot DBNull.Value Then
                            tgkt_before = CType(r(colBefore.ColumnName), DateTime)
                            Dim tgkt_2check_bf As DateTime = CType(r(colBefore.ColumnName), DateTime).AddHours(mLT)
                            'kiểm tra có phải ngày nghỉ không
                            'nếu là ngày nghỉ thì bỏ qua
                            Dim bfirst_sub_bf As Boolean = True
                            While True
                                Dim sRestTime = (From p In lstRestDate
                                                 Where ((p("FromDate") <= tgkt_before And tgkt_before <= p("ToDate")) Or
                                             (tgkt_before <= p("FromDate") And p("FromDate") <= tgkt_2check_bf) Or
                                             (tgkt_before <= p("ToDate") And p("ToDate") <= tgkt_2check_bf)) And
                                             (p("PN") = "00" Or p("PN") = r("ProcessNumber"))
                                                 Select p).FirstOrDefault()
                                If sRestTime Is Nothing Then 'nếu không phải ngày nghỉ
                                    If time_remain_bf > 0 Then
                                        tgkt_before = tgkt_before.Value.AddMinutes(time_remain_bf)
                                        time_remain_bf = 0
                                    Else
                                        tgkt_before = tgkt_before.Value.AddHours(mLT)
                                    End If
                                    Exit While
                                Else 'nếu là ngày nghỉ
                                    'tính lại thời gian gia công cho tới lúc bắt đầu nghỉ
                                    'chỉ tính khi gặp ngày nghỉ đầu tiên (ngày liền kề tiếp theo không tính)
                                    If bfirst_sub Then
                                        Dim cur_time_minute As Int32 = CType(mLT, Decimal) * 60 'tính giờ ra phút
                                        Dim timespn As TimeSpan = CType(sRestTime("FromDate"), DateTime) - tgkt_before
                                        If timespn.TotalMinutes > 0 Then
                                            time_remain = cur_time_minute - timespn.TotalMinutes
                                        End If
                                        bfirst_sub = False
                                    End If
                                    tgkt_2check_bf = CType(sRestTime("ToDate"), DateTime).AddMinutes(1)
                                    tgkt_before = CType(sRestTime("ToDate"), DateTime).AddMinutes(1)
                                End If
                            End While
                        End If
                        '--------------
                        If tgkt_before Is Nothing Then
                            r(col.ColumnName) = tgkt
                        Else
                            r(col.ColumnName) = If(tgkt > tgkt_before, tgkt, tgkt_before)
                        End If
                    End If
                    tgkt = r(col.ColumnName)
                    'r(col.ColumnName.Replace("Leadtime", "Date")) = tgkt
                End If
            Next
            If bFirstLot Then
                bFirstLot = False
            End If
            colBefore = col
            Application.DoEvents()
        Next
        'Catch ex As Exception
        '    ShowError(ex, mnuRun.Text, Me.Text)
        'Finally
        '    SetFormEvents = ActionForm.Run
        'End Try
    End Sub
    'Không làm ngày nghỉ 2 ca
    Private Sub KNN_2Ca()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))

        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate",
                                    PublicTable.Table_FPICS_HolidayDate)

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
		Dim mDate As DateTime = dtpPlanDate.Value.Date
        While mDate <= dtpPlanDate.Value.AddDays(30)
            mrow = dtRes.NewRow
            Dim objH As New FPICS_HolidayDate
            objH.HolidayDate_K = mDate
            If Not DB.ExistObject(objH) Then
                mrow("FromDate") = New DateTime(mDate.Year, mDate.Month, mDate.Day, 22, 0, 0)
                Dim myDate As DateTime = CType(mrow("FromDate"), DateTime).AddDays(1)
                mrow("ToDate") = New DateTime(myDate.Year, myDate.Month, myDate.Day, 6, 0, 0)
                mrow("PN") = "00"
                dtRes.Rows.Add(mrow)
            End If
            mDate = mDate.AddDays(1)
        End While

        For Each r As DataGridViewRow In gridP.Rows
            If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
        Next


        RunAll(dtRes)
    End Sub

	'Không làm ngày nghỉ HC
	Private Sub KNN_HC()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))

        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate",
                                    PublicTable.Table_FPICS_HolidayDate)

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
		Dim mDate As DateTime = dtpPlanDate.Value.Date
        While mDate <= dtpPlanDate.Value.AddDays(30)
            mrow = dtRes.NewRow
            Dim objH As New FPICS_HolidayDate
            objH.HolidayDate_K = mDate
            If Not DB.ExistObject(objH) Then
                mrow("FromDate") = New DateTime(mDate.Year, mDate.Month, mDate.Day, 16, 30, 0)
                Dim myDate As DateTime = CType(mrow("FromDate"), DateTime).AddDays(1)
                mrow("ToDate") = New DateTime(myDate.Year, myDate.Month, myDate.Day, 6, 0, 0)
                mrow("PN") = "00"
                dtRes.Rows.Add(mrow)
            End If
            mDate = mDate.AddDays(1)
        End While

        For Each r As DataGridViewRow In gridP.Rows
            If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
        Next



        RunAll(dtRes)
    End Sub

	'Có làm ngày nghỉ, không làm ngày lễ 3ca
	Private Sub CNN_3Ca()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))
        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate and (IsHoliday=1 {1}) ",
                                    PublicTable.Table_FPICS_HolidayDate,
                                    IIf(ckoSunday.Checked, notWorkSunday, ""))

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
		Dim mDate As DateTime = dtpPlanDate.Value.Date
        'While mDate <= dtpPlanDate.Value.AddDays(10)
        '	mrow = dtRes.NewRow
        '	Dim objH As New FPICS_HolidayDate
        '	objH.HolidayDate_K = mDate
        '	If Not DB.ExistObject(objH) Then

        For Each r As DataGridViewRow In gridP.Rows
                    If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
                Next

        '          End If
        '	mDate = mDate.AddDays(1)
        'End While

        RunAll(dtRes)
    End Sub

	'Có làm ngày nghỉ, không làm ngày lễ 2ca
	Private Sub CNN_2Ca()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))
        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate and   (IsHoliday=1 {1})",
                                    PublicTable.Table_FPICS_HolidayDate,
                                    IIf(ckoSunday.Checked, notWorkSunday, ""))

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
        Dim mDate As DateTime = dtpPlanDate.Value.Date

        While mDate <= dtpPlanDate.Value.AddDays(30)
            mrow = dtRes.NewRow
            Dim objH As New FPICS_HolidayDate
            objH.HolidayDate_K = mDate
            If Not DB.ExistObject(objH) Then
                mrow("FromDate") = New DateTime(mDate.Year, mDate.Month, mDate.Day, 22, 0, 0)
                mrow("ToDate") = New DateTime(mDate.AddDays(1).Year, mDate.AddDays(1).Month, mDate.AddDays(1).Day, 6, 0, 0)
                mrow("PN") = "00"
                dtRes.Rows.Add(mrow)
            End If
            mDate = mDate.AddDays(1)
        End While

        For Each r As DataGridViewRow In gridP.Rows
            If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
        Next



        RunAll(dtRes)
    End Sub

	'Có làm ngày nghỉ, không làm ngày lễ HC
	Private Sub CNN_HC()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))
        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate and (IsHoliday=1 {0})",
                                    PublicTable.Table_FPICS_HolidayDate,
                                    IIf(ckoSunday.Checked, notWorkSunday, ""))

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
        Dim mDate As DateTime = dtpPlanDate.Value.Date

        While mDate <= dtpPlanDate.Value.AddDays(30)
            mrow = dtRes.NewRow
            Dim objH As New FPICS_HolidayDate
            objH.HolidayDate_K = mDate
            If Not DB.ExistObject(objH) Then
                mrow("FromDate") = New DateTime(mDate.Year, mDate.Month, mDate.Day, 16, 30, 0)
                mrow("ToDate") = New DateTime(mDate.AddDays(1).Year, mDate.AddDays(1).Month, mDate.AddDays(1).Day, 7, 30, 0)
                mrow("PN") = "00"
                dtRes.Rows.Add(mrow)
            End If
            mDate = mDate.AddDays(1)
        End While

        For Each r As DataGridViewRow In gridP.Rows
            If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
        Next


        RunAll(dtRes)
    End Sub

	'Có làm ngày nghỉ+lễ 3 ca
	Private Sub CNL_3Ca()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))
        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate and 1=2 ",
                                    PublicTable.Table_FPICS_HolidayDate)

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
		Dim mDate As DateTime = dtpPlanDate.Value.Date
        'While mDate <= dtpPlanDate.Value.AddDays(10)
        '	mrow = dtRes.NewRow
        '	Dim objH As New FPICS_HolidayDate
        '	objH.HolidayDate_K = mDate
        '	If Not DB.ExistObject(objH) Then

        For Each r As DataGridViewRow In gridP.Rows
                    If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
                Next

        '          End If
        '	mDate = mDate.AddDays(1)
        'End While

        RunAll(dtRes)
    End Sub

	'Có làm ngày nghỉ+lễ 2 ca
	Private Sub CNL_2Ca()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))
        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate  and 1=2 ",
                                    PublicTable.Table_FPICS_HolidayDate)

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
		Dim mDate As DateTime = dtpPlanDate.Value.Date
        While mDate <= dtpPlanDate.Value.AddDays(30)
            mrow = dtRes.NewRow
            Dim objH As New FPICS_HolidayDate
            objH.HolidayDate_K = mDate
            If Not DB.ExistObject(objH) Then
                mrow("FromDate") = New DateTime(mDate.Year, mDate.Month, mDate.Day, 22, 0, 0)
                mrow("ToDate") = New DateTime(mDate.AddDays(1).Year, mDate.AddDays(1).Month, mDate.AddDays(1).Day, 6, 0, 0)
                mrow("PN") = "00"
                dtRes.Rows.Add(mrow)
            End If
            mDate = mDate.AddDays(1)
        End While

        For Each r As DataGridViewRow In gridP.Rows
                    If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
                Next



        RunAll(dtRes)
    End Sub

	'Có làm ngày nghỉ+lễ HC
	Private Sub CNL_HC()
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@EndDate", DateTime.Now.AddDays(30))
        Dim sqlrestdate As String = String.Format(
                                    " SELECT '00' as PN, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate])-1, " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS FromDate, " +
                                    " DATEADD(second, 0, " +
                                                " DATEADD(minute, 0, " +
                                                " DATEADD(hour, 6, " +
                                                " DATEADD(day, day([HolidayDate]), " +
                                                " DATEADD(month, month([HolidayDate])-1, " +
                                                " DATEADD(Year, year([HolidayDate])-1900, 0)))))) AS ToDate " +
                                    " FROM {0} where [HolidayDate]>=getdate() and HolidayDate<=@EndDate  and 1=2  ",
                                    PublicTable.Table_FPICS_HolidayDate)

        Dim dtRes As DataTable = DB.FillDataTable(sqlrestdate, para)
        Dim mrow As DataRow = dtRes.NewRow
		Dim mDate As DateTime = dtpPlanDate.Value.Date
        While mDate <= dtpPlanDate.Value.AddDays(10)
            mrow = dtRes.NewRow
            Dim objH As New FPICS_HolidayDate
            objH.HolidayDate_K = mDate
            If Not DB.ExistObject(objH) Then
                mrow("FromDate") = New DateTime(mDate.Year, mDate.Month, mDate.Day, 16, 30, 0)
                mrow("ToDate") = New DateTime(mDate.AddDays(1).Year, mDate.AddDays(1).Month, mDate.AddDays(1).Day, 7, 30, 0)
                mrow("PN") = "00"
                dtRes.Rows.Add(mrow)
            End If
            mDate = mDate.AddDays(1)
        End While

        For Each r As DataGridViewRow In gridP.Rows
                    If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                If r.Cells(TGDung.Name).Value.ToString.Length >= 11 Then
                    Dim mTGDung As String = r.Cells(TGDung.Name).Value
                    For Each tg As String In mTGDung.Split(",")
                        If tg.Length = 17 Then
                            Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                            Dim myDate As String = Microsoft.VisualBasic.Right(tg, 5)
                            mrow = dtRes.NewRow
                            Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                            Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                            Dim sMonth As Integer = Microsoft.VisualBasic.Right(myDate, 2)
                            Dim sDay As Integer = Microsoft.VisualBasic.Left(myDate, 2)

                            mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(sTime, 2),
                                                                    Microsoft.VisualBasic.Right(sTime, 2),
                                                                    0)
                            mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                    Microsoft.VisualBasic.Left(eTime, 2),
                                                                    Microsoft.VisualBasic.Right(eTime, 2),
                                                                    0)
                            If mrow("ToDate") < mrow("FromDate") Then
                                mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                            End If
                            mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                            dtRes.Rows.Add(mrow)
                        ElseIf tg.Length = 11 Then
                            Dim myDate As DateTime = Date.Now.Date
                            While myDate <= Date.Now.AddDays(30).Date
                                Dim myHour As String = Microsoft.VisualBasic.Left(tg, 11)
                                mrow = dtRes.NewRow
                                Dim sTime As String = Microsoft.VisualBasic.Left(myHour, 5)
                                Dim eTime As String = Microsoft.VisualBasic.Right(myHour, 5)
                                Dim sMonth As Integer = myDate.Month
                                Dim sDay As Integer = myDate.Day

                                mrow("FromDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(sTime, 2),
                                                                        Microsoft.VisualBasic.Right(sTime, 2),
                                                                        0)
                                mrow("ToDate") = New DateTime(mDate.Year, sMonth, sDay,
                                                                        Microsoft.VisualBasic.Left(eTime, 2),
                                                                        Microsoft.VisualBasic.Right(eTime, 2),
                                                                        0)
                                If mrow("ToDate") < mrow("FromDate") Then
                                    mrow("ToDate") = CType(mrow("ToDate"), DateTime).AddDays(1)
                                End If
                                mrow("PN") = r.Cells(ProcessNumberP.Name).Value
                                dtRes.Rows.Add(mrow)
                                myDate = myDate.AddDays(1)
                            End While
                        End If
                    Next
                End If
            End If
                Next



        RunAll(dtRes)
    End Sub

	Private Sub mnuRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRun.Click
		If ckoXem.Checked = False Then
			_isEdit = False
			If gridP.Rows.Count = 0 Then
				MessageBox.Show("Không có dữ liệu trên lưới", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Exit Sub
			End If
			Me.CleanGrid()
            'không làm ngày nghỉ
            If rdKNN.Checked Then
                If rdo3Ca.Checked Then
                    Me.KNN_3Ca()
                ElseIf rdo2Ca.Checked Then
                    Me.KNN_2Ca()
                ElseIf rdoHC.Checked Then
                    Me.KNN_HC()
                End If
            ElseIf rdCNN.Checked Then 'có làm ngày nghỉ , không làm ngày lễ
                If rdo3Ca.Checked Then
                    Me.CNN_3Ca()
                ElseIf rdo2Ca.Checked Then
                    Me.CNN_2Ca()
                ElseIf rdoHC.Checked Then
                    Me.CNN_HC()
                End If
                'Fill color for holiday
                For Each c As DataGridViewColumn In gridP.Columns
                    If c.HeaderText.Contains("TGKT") Then
                        For Each r As DataGridViewRow In gridP.Rows
                            If IsDate(r.Cells(c.Index).Value) Then
                                Dim para(0) As SqlClient.SqlParameter
                                para(0) = New SqlClient.SqlParameter("@Value", r.Cells(c.Index).Value)
                                Dim obj As Object = DB.ExecuteScalar(String.Format(" select [HolidayDate] " +
                                                                                   " from [FPICS_HolidayDate]" +
                                                                                   " where @Value between StartDate and EndDate"), para)
                                If obj IsNot Nothing Then
                                    r.Cells(c.Index).Style.BackColor = Color.Orange
                                End If
                            End If
                        Next
                    End If
                Next
            ElseIf rdoHoliday.Checked Then 'có làm ngày nghỉ , có ngày lễ
                If rdo3Ca.Checked Then
                    Me.CNL_3Ca()
                ElseIf rdo2Ca.Checked Then
                    Me.CNL_2Ca()
                ElseIf rdoHC.Checked Then
                    Me.CNL_HC()
                End If
                'Fill color for holiday
                For Each c As DataGridViewColumn In gridP.Columns
                    If c.HeaderText.Contains("TGKT") Then
                        For Each r As DataGridViewRow In gridP.Rows
                            If IsDate(r.Cells(c.Index).Value) Then
                                Dim para(0) As SqlClient.SqlParameter
                                para(0) = New SqlClient.SqlParameter("@Value", r.Cells(c.Index).Value)
                                Dim obj As Object = DB.ExecuteScalar(String.Format(" select [HolidayDate] " +
                                                                                   " from [FPICS_HolidayDate]" +
                                                                                   " where @Value between StartDate and EndDate"), para)
                                If obj IsNot Nothing Then
                                    r.Cells(c.Index).Style.BackColor = Color.Orange
                                End If
                            End If
                        Next
                    End If
                Next
            End If

            mnuSavePlan.PerformClick()
        Else
			LoadKQ()
		End If
	End Sub

	Private Sub mnuEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        If ckoXem.Checked = False Then
            'mnuRun.PerformClick()
            SetFormEvents = ActionForm.Edit
            _isEdit = True
        Else
            gridKQ.ReadOnly = False
            SetGridReadWrite(True, gridKQ)
            gridKQ.Columns(LyDo.Name).ReadOnly = False
        End If
    End Sub

    Private Sub mnuExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExport.Click
        If ckoXem.Checked = False Then
            If gridP.Rows.Count > 0 Then
				ExportWithFormat(gridP)
			End If
        Else
            ExportEXCEL(gridKQ)
        End If
    End Sub

#End Region

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        If txtProductCode.Text = String.Empty Then
            ShowWarning("Chưa nhập ProductCode")
            txtProductCode.Focus()
            Exit Sub
        End If
        If txtLotNoList.Text = String.Empty Then
            ShowWarning("Chưa nhập Lotno")
            txtLotNoList.Focus()
            Exit Sub
        End If
        txtProductCode.Text = txtProductCode.Text.PadLeft(5, "0")
        Dim rc As Object = ""
        Dim lstCol As New List(Of DataGridViewColumn)
        For Each c As DataGridViewColumn In gridP.Columns
            If c.HeaderText.Contains("TGKT") Then
                lstCol.Add(c)
            End If
        Next
        If lstCol IsNot Nothing Then
            For Each c As DataGridViewColumn In lstCol
                gridP.Columns.Remove(c)
            Next
        End If

        Me.Cursor = Cursors.WaitCursor



        Dim sqlStatus As String = String.Format("SELECT  [ProductCode]  " +
												 " ,[LotNumber] " +
												 "  ,case when [StatusFlag] ='S' then 'Start'" +
												 " when [StatusFlag] ='P' then 'Pause'" +
												 " when [StatusFlag] ='W' then 'Wait'" +
												 " when [StatusFlag] ='O' then 'Out'" +
												 " when [StatusFlag] ='C' then 'Complete'" +
												 " end Status" +
											  " FROM [FPiCS-B03].[dbo].[t_Progress]" +
											  " where Quantity>0 and ComponentCode='B00' ")
        Dim dtStatus As DataTable = _dbFPics.FillDataTable(sqlStatus)
        Dim dtExport As DataTable = DB.FillDataTable(String.Format(" select ProductCode,StartLot,EndLot,Ngay " +
                                                                   " from [CL_ScheduleExport] " +
                                                                   " where ProductCode='{0}' and Ngay>getdate()",
                                                                   txtProductCode.Text.PadLeft(5, "0")))
        'Try
        'Tách lô sản phẩm
        Dim lstLotNo As New List(Of String)
        Dim arrLot() As String = Trim(txtLotNoList.Text.Replace(" ", "")).Split(",")
        For Each sLot As String In arrLot
            Dim arrLotRange() As String = Trim(sLot.Replace(" ", "")).Split("-")
            If arrLotRange.Length > 1 Then
                Dim iLot1 As Int32 = CType(arrLotRange(0), Int32)
                Dim iLot2 As Int32 = CType(arrLotRange(1), Int32)
                While iLot1 <= iLot2
                    lstLotNo.Add(iLot1.ToString().PadLeft(5, "0"))
                    iLot1 += 1
                End While
            Else
                lstLotNo.Add(sLot.PadLeft(5, "0"))
            End If
        Next


        Dim sqlRC As String = String.Format("SELECT max([RevisionCode]) as RevisionCode " +
                                           " FROM [t_ManufactureLot] " +
                                           " where [ProductCode]='{0}' and ComponentCode='B00' " +
                                           " and (LotNumber between '{1}' and '{2}' " +
                                           " or LotNumber between '{2}' and '{1}')",
                                            txtProductCode.Text,
                                            lstLotNo.First,
                                            lstLotNo.Last)
        Dim dtRC As DataTable = _dbFPics.FillDataTable(sqlRC)
        If dtRC.Rows(0).Item(0) IsNot DBNull.Value Then
            rc = dtRC.Rows(0).Item(0)
        Else
            sqlRC = String.Format("SELECT   max([RevisionCode]) " +
                                           " FROM [t_ManufactureLot] " +
                                           " where [ProductCode]='{0}' and ComponentCode='B00' ",
                                            txtProductCode.Text)
            dtRC = _dbFPics.FillDataTable(sqlRC)
            If dtRC.Rows(0).Item(0) IsNot DBNull.Value Then
                rc = dtRC.Rows(0).Item(0)
            Else
                ShowWarning("Mã sản phẩm không tồn tại !")
                Return
            End If
        End If


        'Khởi tạo bảng dữ liệu cho lưới
        Dim tbl As New DataTable
        'Lead leadtime product
        Dim sql As String = String.Format("SELECT  cp.[ProductCode]" +
                                         "  ,cp.[RevisionCode] " +
                                         "  ,cp.[ProcessNumber]" +
                                         "  ,cp.[ProcessCode] " +
                                         "  ,p.ProcessNameE as ProcessName" +
                                         "  ,0.0 as TGGC" +
                                         "  ,0.0 as SLThietBi" +
                                         "  ,0.0 as LeadtimeThietBi" +
                                         "  ,0.0 as SlThietBiA" +
                                         "  ,0.0 as LeadtimeTDG" +
                                         "  ,0.0 as LeadtimeSub" +
                                         "  ,'' as TGDung" +
                                          "  ,'' as LotList" +
                                         "  ,'' as Remark" +
                                         " FROM [m_ComponentProcess] cp" +
                                         " left join m_Process p" +
                                         " on cp.ProcessCode=p.ProcessCode" +
                                         " where ProductCode='{0}' and ComponentCode='B00' " +
                                         " and RevisionCode='{1}'" +
                                         " order by cp.ProcessNumber ",
                                         txtProductCode.Text,
                                         rc)
        tbl = _dbFPics.FillDataTable(sql)

		For Each sLot In lstLotNo
            Dim mStatus As Object = dtStatus.Compute("Max(Status)",
                                                     String.Format("ProductCode='{0}' and LotNumber='{1}'",
                                                                   txtProductCode.Text, sLot))
            Dim mStatusExport As Object = dtExport.Compute("Max(Ngay)",
                                                     String.Format("StartLot<='{0}' and EndLot>='{0}'",
                                                                     CType(sLot, Integer)))
            If mStatus Is DBNull.Value Then
                mStatus = ""
            End If
            If IsDate(mStatusExport) Then
                mStatusExport = CType(mStatusExport, DateTime).ToString("dd-MM")
            End If
            Dim colLeadtime As New DataColumn(sLot + "_" + "TGKT", Type.GetType("System.DateTime"))
            If Not tbl.Columns.Contains(colLeadtime.ColumnName) Then
                tbl.Columns.Add(colLeadtime)
                Dim cgLT As New DataGridViewTextBoxColumn
                cgLT.HeaderText = sLot + "_TGKT " & mStatus & " XH:" & mStatusExport
                cgLT.DataPropertyName = colLeadtime.ColumnName
                cgLT.Name = colLeadtime.ColumnName
                cgLT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                cgLT.DefaultCellStyle.Format = "HH:mm dd-MMM"
                cgLT.SortMode = DataGridViewColumnSortMode.NotSortable
                cgLT.Width = 70
                If Not gridP.Columns.Contains(cgLT.Name) Then gridP.Columns.Add(cgLT)
            End If

        Next

		Dim dtLeadtime As DataTable = DB.FillDataTable(String.Format(" select * from PD_ProcessHourNew " +
                                                                     " where ProductCode='{0}' and RevisionCode='{1}' " +
                                                                     " order by ProcessNumber ",
                                                    txtProductCode.Text,
                                                    rc))
        If dtLeadtime.Rows.Count = 0 Then
            ShowWarning(String.Format("Leadtime {0}-{1} chưa có sẽ lấy Leadtime RC trước đó !",
                                          txtProductCode.Text, rc))
            rc = DB.ExecuteScalar(String.Format("select MAX(revisionCode) as RC " +
                                                "from PD_ProcessHourNew " +
                                                "where ProductCode='{0}' ",
                                                txtProductCode.Text))
            If rc Is DBNull.Value Then
                ShowWarning("Mã này chưa có leadtime !")
                Return
            End If

            dtLeadtime = DB.FillDataTable(String.Format(" select * " +
                                                        "  from PD_ProcessHourNew " +
                                                        " where ProductCode='{0}' and RevisionCode='{1}' " +
                                                        " order by ProcessNumber ",
                                                    txtProductCode.Text,
                                                    rc))
            If dtLeadtime.Rows.Count > 0 Then

                Dim dic As New Dictionary(Of String, String)
                For Each r As DataRow In tbl.Rows
                    Dim mxPN As String = ""
                    If dic.Keys.Contains(r.Item("ProcessCode")) Then
                        mxPN = dic(r.Item("ProcessCode"))
                    End If
                    If mxPN = "" Then
                        mxPN = "00"
                    End If
                    'Tìm PN
                    Dim pn As Object = dtLeadtime.Compute("min(ProcessNumber)", String.Format("ProcessCode='{0}' and ProcessNumber>'{1}'",
                                                                                              r.Item("ProcessCode"),
                                                                                              mxPN))
                    If pn IsNot DBNull.Value Then
                        If dic.Keys.Contains(r.Item("ProcessCode")) Then
                            dic(r.Item("ProcessCode")) = pn
                        Else
                            dic.Add(r.Item("ProcessCode"), pn)
                        End If
                    End If
                    'Tìm Leadtime
                    r.Item("TGGC") = dtLeadtime.Compute("Max(TGGC)", String.Format("ProcessNumber='{0}'", pn))
                    r.Item("SLThietBi") = dtLeadtime.Compute("Max(SLThietBi)", String.Format("ProcessNumber='{0}'", pn))
                    r.Item("LeadtimeThietBi") = dtLeadtime.Compute("Max(LeadtimeThietBi)", String.Format("ProcessNumber='{0}'", pn))
                    r.Item("SLThietBiA") = dtLeadtime.Compute("Max(SLThietBiA)", String.Format("ProcessNumber='{0}'", pn))
                    r.Item("LeadtimeSub") = dtLeadtime.Compute("Max(LeadtimeSub)", String.Format("ProcessNumber='{0}'", pn))
                    r.Item("LeadtimeTDG") = dtLeadtime.Compute("Max(LeadtimeTDG)", String.Format("ProcessNumber='{0}'", pn))
                    r.Item("Remark") = dtLeadtime.Compute("Max(Remark)", String.Format("ProcessNumber='{0}'", pn))
                    r.Item(LotList.Name) = dtLeadtime.Compute("Max(LotList)", String.Format("ProcessNumber='{0}'", pn))
                    If r.Item(LotList.Name) Is DBNull.Value Then
                        r.Item(LotList.Name) = ""
                    End If
                Next
            Else
                ShowWarning(String.Format("{0}-{1} chưa có LeadtimeTDG !", txtProductCode.Text, rc))
            End If
        Else
            For Each r As DataRow In tbl.Rows
				r.Item("TGGC") = dtLeadtime.Compute("Max(TGGC)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
				r.Item("SLThietBi") = dtLeadtime.Compute("Max(SLThietBi)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
				r.Item("LeadtimeThietBi") = dtLeadtime.Compute("Max(LeadtimeThietBi)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
				r.Item("SLThietBiA") = dtLeadtime.Compute("Max(SLThietBiA)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
				r.Item("LeadtimeSub") = dtLeadtime.Compute("Max(LeadtimeSub)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
				r.Item("LeadtimeTDG") = dtLeadtime.Compute("Max(LeadtimeTDG)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
                r.Item("Remark") = dtLeadtime.Compute("Max(Remark)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
                r.Item(LotList.Name) = dtLeadtime.Compute("Max(LotList)", String.Format("ProcessNumber='{0}'", r.Item("ProcessNumber")))
                If r.Item(LotList.Name) Is DBNull.Value Then
                    r.Item(LotList.Name) = ""
                End If
            Next
		End If

        gridP.AutoGenerateColumns = False
        Me.bs = New BindingSource
        bs.DataSource = tbl
        gridP.DataSource = bs


		For Each c As DataGridViewColumn In gridP.Columns
            Dim sqlC As String = String.Format("SELECT  max([ProcessNumber])  " +
                                                "  FROM [FPiCS-B03].[dbo].[t_ProcessResult]" +
                                                "  where ProductCode='{0}' and EndDate is not null " +
                                                "  and ComponentCode='B00'" +
                                                "  and LotNumber='{1}'  ",
                                                  txtProductCode.Text,
                                                Microsoft.VisualBasic.Left(c.HeaderText, 5))
            Dim maxPN As Object = _dbFPics.ExecuteScalar(sqlC)
			If maxPN Is DBNull.Value Then
				maxPN = "00"
			End If
			For Each r As DataGridViewRow In gridP.Rows
				If r.Cells(ProcessNumberP.Name).Value <= maxPN Then
					r.Cells(c.Name).Style.BackColor = Color.Yellow
				Else
					Exit For
				End If
			Next
		Next

        If ckoPR.Checked Then
            For Each r As DataGridViewRow In gridP.Rows
                If r.Cells("ProcessCodeP").Value = "9051" Or
                    r.Cells("ProcessCodeP").Value = "9052" Or
                     r.Cells("ProcessCodeP").Value = "9304" Or
                    r.Cells("ProcessCodeP").Value = "9056" Then
                    r.Cells("TGDung").Value = "16:01-07:29"
                End If
            Next
        End If

        Me.Cursor = Cursors.Arrow
		'Catch ex As Exception
		'    Me.Cursor = Cursors.Arrow
		'    ShowError(ex, btnAdd.Text, Me.Name)
		'End Try
	End Sub

    Private Sub ckoXem_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ckoXem.CheckedChanged
        gridKQ.Visible = ckoXem.Checked
        gridP.Visible = Not ckoXem.Checked
    End Sub

    Private Sub dtpPlanDate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpPlanDate.ValueChanged
        LoadKQ()
    End Sub

    Sub LoadKQ()
        Dim sql As String = String.Format(" SELECT p.[PlanDate],p.[ProductCode],p.[LotNo],p.[ProcessNumber],p.ProcessCode,p.ProcessName," +
                                          " p.TGGC,p.SLThietBi,p.LeadtimeThietBi,p.SLThietBiA, " +
                                          " p.LeadtimeTDG,p.LeadtimeSub,p.TGDung, p.Remark," +
                                          " p.[TGKT] ,p.[TGTT],p.[SaiLech],p.[Status],p.LyDo " +
                                          " FROM  [PD_PlanHour] p " +
                                          " where p.PlanDate=@PlanDate " +
                                          " order by p.ProductCode,p.LotNo,p.ProcessNumber")
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@PlanDate", dtpPlanDate.Value.Date)

        DB.ExecuteStoreProcedure("sp_PD_UpdatePlanHour", para)

        Dim bdsource As New BindingSource
        bdsource.DataSource = DB.FillDataTable(sql, para)
        bdn.BindingSource = bdsource
        gridKQ.DataSource = bdsource
    End Sub

    Private Sub mnuSave_Click(sender As System.Object, e As System.EventArgs) Handles mnuSavePlan.Click
        If ShowQuestionSave() = Windows.Forms.DialogResult.Yes Then
            For c As Integer = gridP.Columns(RemarkP.Name).Index + 1 To gridP.ColumnCount - 1
                For Each r As DataGridViewRow In gridP.Rows
                    If r.Cells(c).Value IsNot DBNull.Value Then
                        Dim obj As New PD_PlanHour
                        obj.PlanDate_K = dtpPlanDate.Value.Date
                        obj.ProductCode_K = txtProductCode.Text.PadLeft(5, "0")
                        obj.LotNo_K = Microsoft.VisualBasic.Left(gridP.Columns(c).HeaderText, 5)
                        obj.ProcessNumber_K = r.Cells(ProcessNumberP.Name).Value
                        obj.ProcessCode = r.Cells(ProcessCodeP.Name).Value
                        obj.ProcessName = r.Cells(ProcessNameP.Name).Value
                        If r.Cells(TGGC.Name).Value IsNot DBNull.Value Then
                            obj.TGGC = r.Cells(TGGC.Name).Value
                        End If
                        If r.Cells(SLThietBi.Name).Value IsNot DBNull.Value Then
                            obj.SLThietBi = r.Cells(SLThietBi.Name).Value
                        End If
                        If r.Cells(LeadtimeThietBi.Name).Value IsNot DBNull.Value Then
                            obj.LeadtimeThietBi = r.Cells(LeadtimeThietBi.Name).Value
                        End If
                        If r.Cells(SlThietBiA.Name).Value IsNot DBNull.Value Then
                            obj.SLThietBiA = r.Cells(SlThietBiA.Name).Value
                        End If
                        If r.Cells(leadtimeTDG.Name).Value IsNot DBNull.Value Then
                            obj.LeadtimeTDG = r.Cells(leadtimeTDG.Name).Value
                        End If
                        If r.Cells(LeadtimeSub.Name).Value IsNot DBNull.Value Then
                            obj.LeadtimeSub = r.Cells(LeadtimeSub.Name).Value
                        End If
                        If r.Cells(RemarkP.Name).Value IsNot DBNull.Value Then
                            obj.Remark = r.Cells(RemarkP.Name).Value
                        End If
                        If r.Cells(LotListP.Name).Value IsNot DBNull.Value Then
                            obj.LotList = r.Cells(LotListP.Name).Value
                        End If
                        If r.Cells(TGDung.Name).Value IsNot DBNull.Value Then
                            obj.TGDung = r.Cells(TGDung.Name).Value
                        End If
                        obj.TGKT = r.Cells(c).Value
                        If DB.ExistObject(obj) Then
                            DB.Update(obj)
                        Else
                            DB.Insert(obj)
                        End If
                    End If
                Next
            Next
            ShowSuccess()
        End If
    End Sub

    Private Sub gridProcessHour_CellValueChanged(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridP.CellValueChanged
		If e.RowIndex >= 0 And _isEdit Then
			If e.ColumnIndex = gridP.Columns(SLThietBi.Name).Index Then
				gridP.CurrentRow.Cells(LeadtimeThietBi.Name).Value = 0
				If IsNumeric(gridP.CurrentRow.Cells(SLThietBi.Name).Value) Then
					If IsNumeric(gridP.CurrentRow.Cells(TGGC.Name).Value) And gridP.CurrentRow.Cells(SLThietBi.Name).Value > 0 Then
						gridP.CurrentRow.Cells(LeadtimeThietBi.Name).Value = gridP.CurrentRow.Cells(TGGC.Name).Value / gridP.CurrentRow.Cells(SLThietBi.Name).Value
					End If
				End If
			End If
			If e.ColumnIndex = gridP.Columns(SlThietBiA.Name).Index Then
				gridP.CurrentRow.Cells(leadtimeTDG.Name).Value = 0
				If IsNumeric(gridP.CurrentRow.Cells(SlThietBiA.Name).Value) Then
					If IsNumeric(gridP.CurrentRow.Cells(TGGC.Name).Value) And gridP.CurrentRow.Cells(SlThietBiA.Name).Value > 0 Then
						gridP.CurrentRow.Cells(leadtimeTDG.Name).Value = gridP.CurrentRow.Cells(TGGC.Name).Value / gridP.CurrentRow.Cells(SlThietBiA.Name).Value
					End If
				End If
			End If

            If gridP.Columns(RemarkP.Name).Index < e.ColumnIndex And
                gridP.CurrentRow.Cells(e.ColumnIndex).Value IsNot DBNull.Value Then
                Dim thisValue As DateTime = gridP.CurrentRow.Cells(e.ColumnIndex).Value
                For c As Integer = e.ColumnIndex + 1 To gridP.ColumnCount - 1
                    If gridP.CurrentRow.Cells(leadtimeTDG.Name).Value IsNot DBNull.Value Then
                        thisValue = thisValue.AddHours(gridP.CurrentRow.Cells(leadtimeTDG.Name).Value)
                    End If
                    If gridP.CurrentRow.Cells(LeadtimeSub.Name).Value IsNot DBNull.Value Then
                        thisValue = thisValue.AddHours(gridP.CurrentRow.Cells(LeadtimeSub.Name).Value)
                    End If
                    gridP.CurrentRow.Cells(c).Value = thisValue
                Next
            End If
        End If
	End Sub

    Private Sub mnuSaveLeadtime_Click(sender As System.Object, e As System.EventArgs) Handles mnuSaveLeadtime.Click
        If ShowQuestionSave() = Windows.Forms.DialogResult.Yes Then
			For c As Integer = gridP.Columns(leadtimeTDG.Name).Index + 1 To gridP.ColumnCount - 1
				For Each r As DataGridViewRow In gridP.Rows
					If r.Cells(c).Value IsNot DBNull.Value Then
						Dim obj As New PD_ProcessHourNew
						obj.ProductCode_K = txtProductCode.Text.PadLeft(5, "0")
						obj.ProcessNumber_K = r.Cells(ProcessNumberP.Name).Value
						obj.RevisionCode_K = r.Cells(RevisionCode.Name).Value
						obj.ProcessNumber_K = r.Cells(ProcessNumberP.Name).Value
                        DB.GetObjectNotReset(obj)
                        obj.ProcessCode = r.Cells(ProcessCodeP.Name).Value
						obj.ProcessName = r.Cells(ProcessNameP.Name).Value
						If r.Cells(TGGC.Name).Value IsNot DBNull.Value Then
							obj.TGGC = r.Cells(TGGC.Name).Value
						End If
						If r.Cells(SLThietBi.Name).Value IsNot DBNull.Value Then
							obj.SLThietBi = r.Cells(SLThietBi.Name).Value
						End If
						If r.Cells(LeadtimeThietBi.Name).Value IsNot DBNull.Value Then
							obj.LeadtimeThietBi = r.Cells(LeadtimeThietBi.Name).Value
						End If
						If r.Cells(SlThietBiA.Name).Value IsNot DBNull.Value Then
							obj.SLThietBiA = r.Cells(SlThietBiA.Name).Value
						End If
						If r.Cells(leadtimeTDG.Name).Value IsNot DBNull.Value Then
							obj.LeadtimeTDG = r.Cells(leadtimeTDG.Name).Value
						End If
						If r.Cells(LeadtimeSub.Name).Value IsNot DBNull.Value Then
							obj.LeadtimeSub = r.Cells(LeadtimeSub.Name).Value
						End If
                        If r.Cells(RemarkP.Name).Value IsNot DBNull.Value Then
                            obj.Remark = r.Cells(RemarkP.Name).Value
                        End If
                        If r.Cells(LotListP.Name).Value IsNot DBNull.Value Then
                            obj.LotList = r.Cells(LotListP.Name).Value
                        End If
                        If DB.ExistObject(obj) Then
							DB.Update(obj)
						Else
							DB.Insert(obj)
						End If
					End If
				Next
			Next
			ShowSuccess()
        End If
    End Sub

	Private Sub gridP_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridP.CellDoubleClick
		Dim lot As String = Microsoft.VisualBasic.Left(gridP.Columns(e.ColumnIndex).HeaderText, 5)
		Dim frm As New FrmPDIProgress
		frm._pdcode = txtProductCode.Text.PadLeft(5, "0")
		frm.Show()
	End Sub

    Private Sub MnuXoa_Click(sender As Object, e As EventArgs) Handles mnuXoa.Click
        For Each r As DataGridViewCell In gridP.SelectedCells
            r.Value = DBNull.Value
        Next
    End Sub

    Private Sub GridKQ_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles gridKQ.CellValueChanged
        If e.RowIndex >= 0 Then
            If gridKQ.Columns(e.ColumnIndex).ReadOnly = False Then
                Dim obj As New PD_PlanHour
                obj.PlanDate_K = gridKQ.CurrentRow.Cells("PlanDate").Value
                obj.ProductCode_K = gridKQ.CurrentRow.Cells("ProductCode").Value
                obj.ProcessNumber_K = gridKQ.CurrentRow.Cells("ProcessNumber").Value
                obj.LotNo_K = gridKQ.CurrentRow.Cells("LotNo").Value
                DB.GetObject(obj)
                If gridKQ.CurrentRow.Cells(LyDo.Name).Value IsNot DBNull.Value Then
                    obj.LyDo = gridKQ.CurrentRow.Cells(LyDo.Name).Value
                Else
                    obj.LyDo = ""
                End If
                DB.Update(obj)
                End If
            End If
    End Sub

    Private Sub mnuNewForm_Click(sender As Object, e As EventArgs) Handles mnuNewForm.Click
        Dim frm As New FrmPlanHourNew
        frm.Show()
    End Sub
End Class