﻿Imports CommonDB
Imports DevExpress
Imports DevExpress.Data
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports PublicUtility
Public Class FrmReportDetail_2
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim _dtDongHangDetail As DataTable
    Dim colName As String
    Dim _ProductCode As String = ""
    Dim _DongHang As String = ""

    Private Sub FrmReportDetail_2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteStart.EditValue = DateTime.Now.AddDays(-1)
        dteEnd.EditValue = DateTime.Now.AddDays(-1)
        dteStartGetData.EditValue = DateTime.Now
        dteEndGetData.EditValue = DateTime.Now
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

    Private Sub rdoNgay_CheckedChanged(sender As Object, e As EventArgs) Handles rdoNgay.CheckedChanged
        If rdoNgay.Checked Then
            dteStart.EditValue = DateTime.Now.AddDays(-1)
            dteEnd.EditValue = dteStart.DateTime
        End If
    End Sub

    Private Sub rdoTuan_CheckedChanged(sender As Object, e As EventArgs) Handles rdoTuan.CheckedChanged
        If rdoTuan.Checked Then
            dteStart.EditValue = DateTime.Now.AddDays(-1)
            dteStart.EditValue = GetFirstDayOfWeek(dteStart.DateTime)
            dteEnd.EditValue = LastDayOfWeek(dteStart.DateTime)
        End If
    End Sub

    Private Sub rdoThang_CheckedChanged(sender As Object, e As EventArgs) Handles rdoThang.CheckedChanged
        If rdoThang.Checked Then
            dteStart.EditValue = DateTime.Now.AddDays(-1)
            dteStart.EditValue = GetStartDayOfMonth(dteStart.DateTime)
            dteEnd.EditValue = GetEndDayOfMonth(dteStart.DateTime)
        End If
    End Sub

    Private Sub rdoQuy_CheckedChanged(sender As Object, e As EventArgs) Handles rdoQuy.CheckedChanged
        If rdoQuy.Checked Then
            dteStart.EditValue = DateTime.Now.AddDays(-1)
            dteStart.EditValue = GetStartDayOfQuarter(dteStart.DateTime)
            dteEnd.EditValue = GetEndDayOfQuarter(dteStart.DateTime)
        End If
    End Sub

    Private Sub rdoNam_CheckedChanged(sender As Object, e As EventArgs) Handles rdoNam.CheckedChanged
        If rdoNam.Checked Then
            dteStart.EditValue = DateTime.Now.AddDays(-1)
            dteStart.EditValue = GetStartDayOfYear(dteStart.DateTime)
            dteEnd.EditValue = GetEndDayOfYear(dteStart.DateTime)
        End If
    End Sub

    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        SplashScreenManager1.ShowWaitForm()
        GridView1.Columns.Clear()
        GridView2.Columns.Clear()
        GridView3.Columns.Clear()
        _ProductCode = ""
        _DongHang = ""

        Dim tb As New DataTable
        Dim myCustomer As Object = DBNull.Value
        Dim mySize As Object = DBNull.Value
        Dim myMethod As Object = DBNull.Value
        Dim sql As String = ""
        Dim para(4) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@StartDate", GetStartDate(dteStart.DateTime))
        para(1) = New SqlClient.SqlParameter("@EndDate", GetEndDate(dteEnd.DateTime))

        If cbbSize.Text <> "" And cbbSize.Text <> "All" Then
            mySize = cbbSize.Text
        End If
        If cbbLoai.Text = "All" Or cbbLoai.Text = "" Then
            para(2) = New SqlClient.SqlParameter("@Method", DBNull.Value)
            myMethod = DBNull.Value
        ElseIf cbbLoai.Text = "Single Side" Then
            para(2) = New SqlClient.SqlParameter("@Method", "01")
            myMethod = "01"
        ElseIf cbbLoai.Text = "Double Side" Then
            para(2) = New SqlClient.SqlParameter("@Method", "02")
            myMethod = "02"
        End If
        If cbbKhachHang.Text <> "" And cbbKhachHang.Text <> "All" Then
            If cbbKhachHang.Text <> "Other" Then
                para(3) = New SqlClient.SqlParameter("@Customer", cbbKhachHang.Text.Trim)
                myCustomer = cbbKhachHang.Text
            Else
                para(3) = New SqlClient.SqlParameter("@Customer", "CANON")
                myCustomer = "CANON"
            End If
        Else
            para(3) = New SqlClient.SqlParameter("@Customer", DBNull.Value)
        End If
        If cbbSize.Text <> "" And cbbSize.Text <> "All" Then
            para(4) = New SqlClient.SqlParameter("@Size", cbbSize.Text)
        Else
            para(4) = New SqlClient.SqlParameter("@Size", DBNull.Value)
        End If

        If rdoCustomer.Checked Then
            If rdoRandom.Checked Then
                sql = "sp_RI_GetbyCustomer_2"
            Else
                sql = "sp_RI_GetbyCustomer_2_OG"
            End If
            If Not rdoNgay.Checked Then
                If rdoRandom.Checked Then
                    sql = "sp_RI_GetbyCustomer_Sum_2"
                Else
                    sql = "sp_RI_GetbyCustomer_Sum_2_OG"
                End If
            End If
            tb = _db.ExecuteStoreProcedureTB(sql, para)
            If tb.Rows.Count > 0 Then
                GridControl1.DataSource = tb
                GridControlSetFormat(GridView1, 5)
                GridControlSetFormatPercentage(GridView1, "TLCP", 2)
                GridView1.BestFitColumns()
                'Đổi HGST -> WDC
                For r As Integer = 0 To GridView1.DataRowCount - 1
                    If GridView1.GetRowCellValue(r, "Customer") = "HGST" Then
                        GridView1.SetRowCellValue(r, "Customer", "WDC")
                    End If
                Next
            Else
                ShowWarning("Không có dữ liệu")
                SplashScreenManager1.CloseWaitForm()
                Return
            End If
        ElseIf rdoProduct.Checked Then
            If rdoRandom.Checked Then
                sql = "sp_RI_GetbyProductDay_2"
            Else
                sql = "sp_RI_GetbyProductDay_2_OG"
            End If
            If rdoNgay.Checked = False Then
                If rdoRandom.Checked Then
                    sql = "sp_RI_GetbyProductSum_2"
                Else
                    sql = "sp_RI_GetbyProductSum_2_OG"
                End If
            End If
            If cbbLoai.Text = "All" Or cbbLoai.Text = "" Then
                para(2) = New SqlClient.SqlParameter("@Method", DBNull.Value)
            Else
                para(2) = New SqlClient.SqlParameter("@Method", cbbLoai.Text)
            End If

            tb = _db.ExecuteStoreProcedureTB(sql, para)
            If tb.Rows.Count > 0 Then
                GridControl1.DataSource = tb
                'GridControlSetFormat(GridView1, GridView1.Columns("TLCP").VisibleIndex)
                GridControlSetFormat(GridView1, 5)
                GridControlSetFormatPercentage(GridView1, "TLCP", 2)
                GridView1.BestFitColumns()
                'Đổi HGST -> WDC
                For r As Integer = 0 To GridView1.DataRowCount - 1
                    If GridView1.GetRowCellValue(r, "Customer") = "HGST" Then
                        GridView1.SetRowCellValue(r, "Customer", "WDC")
                    End If
                Next

                'Đổi Psc
                For r As Integer = 0 To GridView1.DataRowCount - 1
                    If GridView1.GetRowCellValue(r, "Size") = "1" Then
                        GridView1.SetRowCellValue(r, "Size", "Pcs")
                    End If
                Next
            Else
                ShowWarning("Không có dữ liệu")
                SplashScreenManager1.CloseWaitForm()
                Return
            End If
        Else
            If cbbLoai.Text = "All" Or cbbLoai.Text = "" Then
                para(2) = New SqlClient.SqlParameter("@Method", DBNull.Value)
            Else
                para(2) = New SqlClient.SqlParameter("@Method", cbbLoai.Text)
            End If
            If rdoNgay.Checked Then
                If rdoRandom.Checked Then
                    sql = "sp_RI_GetbyLotNo"
                Else
                    sql = "sp_RI_GetbyLotNo_OG"
                End If
            Else
                If rdoRandom.Checked Then
                    sql = "sp_RI_GetbyLotNoSum"
                Else
                    sql = "sp_RI_GetbyLotNoSum_OG"
                End If
            End If

            tb = _db.ExecuteStoreProcedureTB(sql, para)
            If tb.Rows.Count > 0 Then
                GridControl1.DataSource = tb
                GridControlSetFormat(GridView1, 5)
                GridControlSetFormatPercentage(GridView1, "TLCP", 2)
                GridControlSetFormatNumber(GridView1, "M2PerPcs", 8)
                GridView1.BestFitColumns()
                'Đổi HGST -> WDC
                For r As Integer = 0 To GridView1.DataRowCount - 1
                    If GridView1.GetRowCellValue(r, "Customer") = "HGST" Then
                        GridView1.SetRowCellValue(r, "Customer", "WDC")
                    End If
                Next

                'Đổi Psc
                For r As Integer = 0 To GridView1.DataRowCount - 1
                    If GridView1.GetRowCellValue(r, "Size") = "1" Then
                        GridView1.SetRowCellValue(r, "Size", "Pcs")
                    End If
                Next
            Else
                ShowWarning("Không có dữ liệu")
                SplashScreenManager1.CloseWaitForm()
                Return
            End If
        End If

        Dim myCustomerT As String = ""
        If cbbKhachHang.Text = "SEAGATE" And cbbLoai.Text = "Single Side" Then
            myCustomerT = "SEAS"
        ElseIf cbbKhachHang.Text = "SEAGATE" And cbbLoai.Text = "Double Side" Then
            myCustomerT = "SEAD"
        ElseIf cbbKhachHang.Text = "SEAGATE" And cbbLoai.Text = "" Then
            myCustomerT = "SEAAll"
        ElseIf cbbKhachHang.Text = "TOSHIBA" And cbbLoai.Text = "Single Side" Then
            myCustomerT = "TSBS"
        ElseIf cbbKhachHang.Text = "TOSHIBA" And cbbLoai.Text = "Double Side" Then
            myCustomerT = "TSBD"
        ElseIf cbbKhachHang.Text = "TOSHIBA" And cbbLoai.Text = "" Then
            myCustomerT = "TSBAll"
        ElseIf cbbKhachHang.Text = "WDC" Then
            myCustomerT = "WDC"
        ElseIf cbbKhachHang.Text = "WESTERN DIGITAL" Then
            myCustomerT = "WDAll"
        ElseIf cbbKhachHang.Text = "OTHER" Then
            myCustomerT = "OTHER"
        ElseIf (cbbKhachHang.Text = "All" Or cbbKhachHang.Text = "") And cbbLoai.Text = "Single Side" Then
            myCustomerT = "Single"
        ElseIf (cbbKhachHang.Text = "All" Or cbbKhachHang.Text = "") And cbbLoai.Text = "Double Side" Then
            myCustomerT = "Double"
        ElseIf (cbbKhachHang.Text = "All" Or cbbKhachHang.Text = "") Then
            myCustomerT = "All"
        End If

        If rdoNgay.Checked Then
            Dim paraS(10) As SqlClient.SqlParameter
            paraS(0) = New SqlClient.SqlParameter("@StartDate", GetStartDate(dteStart.DateTime))
            paraS(1) = New SqlClient.SqlParameter("@EndDate", GetEndDate(dteEnd.DateTime))
            paraS(2) = New SqlClient.SqlParameter("@StartLT", GetStartDayOfMonth(dteStart.DateTime))
            paraS(3) = New SqlClient.SqlParameter("@EndLT", GetEndDate(dteEnd.DateTime))
            paraS(4) = New SqlClient.SqlParameter("@StartM", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-1)))
            paraS(5) = New SqlClient.SqlParameter("@EndM", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-1)))
            paraS(6) = New SqlClient.SqlParameter("@Customer", myCustomer)
            paraS(7) = New SqlClient.SqlParameter("@Size", mySize)
            paraS(8) = New SqlClient.SqlParameter("@Method", myMethod)
            'paraS(9) = New SqlClient.SqlParameter("@YYYY", GetQuaterByFinancial(dteStart.DateTime))
            paraS(9) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
            paraS(10) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)

            If rdoRandom.Checked Then
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyDay_2", paraS)
            Else
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyDay_2_OG", paraS)
            End If
            GridControlSetFormat(GridView3, 2)
            GridControlSetFormatPercentage(GridView3, "TLCP", 2)
            GridView3.BestFitColumns()
            GridView3.Columns("ID").Visible = False
        ElseIf rdoTuan.Checked Then
            Dim paraS(10) As SqlClient.SqlParameter
            paraS(0) = New SqlClient.SqlParameter("@StartT", GetStartDate(dteStart.DateTime))
            paraS(1) = New SqlClient.SqlParameter("@EndT", GetEndDate(dteEnd.DateTime))
            paraS(2) = New SqlClient.SqlParameter("@StartT1", GetStartDate(dteStart.DateTime.AddDays(-7)))
            paraS(3) = New SqlClient.SqlParameter("@EndT1", GetEndDate(dteEnd.DateTime.AddDays(-7)))
            paraS(4) = New SqlClient.SqlParameter("@StartT2", GetStartDate(dteStart.DateTime.AddDays(-14)))
            paraS(5) = New SqlClient.SqlParameter("@EndT2", GetEndDate(dteEnd.DateTime.AddDays(-14)))
            paraS(6) = New SqlClient.SqlParameter("@Customer", myCustomer)
            paraS(7) = New SqlClient.SqlParameter("@Size", mySize)
            paraS(8) = New SqlClient.SqlParameter("@Method", myMethod)
            paraS(9) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
            paraS(10) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)

            If rdoRandom.Checked Then
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyTuan_2", paraS)
            Else
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyTuan_2_OG", paraS)
            End If
            GridControlSetFormat(GridView3, 2)
            GridControlSetFormatPercentage(GridView3, "TLCP", 2)
            GridView3.BestFitColumns()
            GridView3.Columns("ID").Visible = False
        ElseIf rdoThang.Checked Then
            Dim paraS(10) As SqlClient.SqlParameter
            paraS(0) = New SqlClient.SqlParameter("@StartT", GetStartDate(dteStart.DateTime))
            paraS(1) = New SqlClient.SqlParameter("@EndT", GetEndDate(dteEnd.DateTime))
            paraS(2) = New SqlClient.SqlParameter("@StartT1", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-1)))
            paraS(3) = New SqlClient.SqlParameter("@EndT1", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-1)))
            paraS(4) = New SqlClient.SqlParameter("@StartT2", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-2)))
            paraS(5) = New SqlClient.SqlParameter("@EndT2", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-2)))
            paraS(6) = New SqlClient.SqlParameter("@Customer", myCustomer)
            paraS(7) = New SqlClient.SqlParameter("@Size", mySize)
            paraS(8) = New SqlClient.SqlParameter("@Method", myMethod)
            paraS(9) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
            paraS(10) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)

            If rdoRandom.Checked Then
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyThang_2", paraS)
            Else
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyThang_2_OG", paraS)
            End If
            GridControlSetFormat(GridView3, 2)
            GridControlSetFormatPercentage(GridView3, "TLCP", 2)
            GridView3.BestFitColumns()
            GridView3.Columns("ID").Visible = False
        ElseIf rdoQuy.Checked Then
            Dim paraS(14) As SqlClient.SqlParameter
            paraS(0) = New SqlClient.SqlParameter("@StartQ", GetStartDate(dteStart.DateTime))
            paraS(1) = New SqlClient.SqlParameter("@EndQ", GetEndDate(dteEnd.DateTime))
            paraS(2) = New SqlClient.SqlParameter("@StartQ1", GetStartDayOfQuarter(dteStart.DateTime.AddMonths(-3)))
            paraS(3) = New SqlClient.SqlParameter("@EndQ1", GetEndDayOfQuarter(dteStart.DateTime.AddMonths(-3)))

            paraS(4) = New SqlClient.SqlParameter("@StartT1", GetStartDayOfMonth(dteStart.DateTime))
            paraS(5) = New SqlClient.SqlParameter("@EndT1", GetEndDayOfMonth(dteStart.DateTime))
            paraS(6) = New SqlClient.SqlParameter("@StartT2", GetStartDayOfMonth(dteStart.DateTime.AddMonths(1)))
            paraS(7) = New SqlClient.SqlParameter("@EndT2", GetEndDayOfMonth(dteStart.DateTime.AddMonths(1)))
            paraS(8) = New SqlClient.SqlParameter("@StartT3", GetStartDayOfMonth(dteStart.DateTime.AddMonths(2)))
            paraS(9) = New SqlClient.SqlParameter("@EndT3", GetEndDayOfMonth(dteStart.DateTime.AddMonths(2)))
            paraS(10) = New SqlClient.SqlParameter("@Customer", myCustomer)
            paraS(11) = New SqlClient.SqlParameter("@Size", mySize)
            paraS(12) = New SqlClient.SqlParameter("@Method", myMethod)
            paraS(13) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
            paraS(14) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)

            If rdoRandom.Checked Then
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyQuy_2", paraS)
            Else
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyQuy_2_OG", paraS)
            End If
            GridControlSetFormat(GridView3, 2)
            GridControlSetFormatPercentage(GridView3, "TLCP", 2)
            GridView3.BestFitColumns()
            GridView3.Columns("ID").Visible = False

            For r As Integer = 0 To GridView3.RowCount - 1
                If GridView3.GetRowCellValue(r, "Thang") = "Q1" Then
                    GridView3.SetRowCellValue(r, "Thang", GetQuaterByFinancialXX(dteStart.DateTime.AddMonths(-3)))
                ElseIf GridView3.GetRowCellValue(r, "Thang") = "Q" Then
                    GridView3.SetRowCellValue(r, "Thang", GetQuaterByFinancialXX(dteStart.DateTime))
                End If
            Next
        ElseIf rdoNam.Checked Then
            Dim paraS(28) As SqlClient.SqlParameter
            paraS(0) = New SqlClient.SqlParameter("@StartT1", GetStartDayOfMonth(dteStart.DateTime))
            paraS(1) = New SqlClient.SqlParameter("@EndT1", GetEndDayOfMonth(dteStart.DateTime))
            paraS(2) = New SqlClient.SqlParameter("@StartT2", GetStartDayOfMonth(dteStart.DateTime.AddMonths(1)))
            paraS(3) = New SqlClient.SqlParameter("@EndT2", GetEndDayOfMonth(dteStart.DateTime.AddMonths(1)))
            paraS(4) = New SqlClient.SqlParameter("@StartT3", GetStartDayOfMonth(dteStart.DateTime.AddMonths(2)))
            paraS(5) = New SqlClient.SqlParameter("@EndT3", GetEndDayOfMonth(dteStart.DateTime.AddMonths(2)))
            paraS(6) = New SqlClient.SqlParameter("@StartT4", GetStartDayOfMonth(dteStart.DateTime.AddMonths(3)))
            paraS(7) = New SqlClient.SqlParameter("@EndT4", GetEndDayOfMonth(dteStart.DateTime.AddMonths(3)))
            paraS(8) = New SqlClient.SqlParameter("@StartT5", GetStartDayOfMonth(dteStart.DateTime.AddMonths(4)))
            paraS(9) = New SqlClient.SqlParameter("@EndT5", GetEndDayOfMonth(dteStart.DateTime.AddMonths(4)))
            paraS(10) = New SqlClient.SqlParameter("@StartT6", GetStartDayOfMonth(dteStart.DateTime.AddMonths(5)))
            paraS(11) = New SqlClient.SqlParameter("@EndT6", GetEndDayOfMonth(dteStart.DateTime.AddMonths(5)))
            paraS(12) = New SqlClient.SqlParameter("@StartT7", GetStartDayOfMonth(dteStart.DateTime.AddMonths(6)))
            paraS(13) = New SqlClient.SqlParameter("@EndT7", GetEndDayOfMonth(dteStart.DateTime.AddMonths(6)))
            paraS(14) = New SqlClient.SqlParameter("@StartT8", GetStartDayOfMonth(dteStart.DateTime.AddMonths(7)))
            paraS(15) = New SqlClient.SqlParameter("@EndT8", GetEndDayOfMonth(dteStart.DateTime.AddMonths(7)))
            paraS(16) = New SqlClient.SqlParameter("@StartT9", GetStartDayOfMonth(dteStart.DateTime.AddMonths(8)))
            paraS(17) = New SqlClient.SqlParameter("@EndT9", GetEndDayOfMonth(dteStart.DateTime.AddMonths(8)))
            paraS(18) = New SqlClient.SqlParameter("@StartT10", GetStartDayOfMonth(dteStart.DateTime.AddMonths(9)))
            paraS(19) = New SqlClient.SqlParameter("@EndT10", GetEndDayOfMonth(dteStart.DateTime.AddMonths(9)))
            paraS(20) = New SqlClient.SqlParameter("@StartT11", GetStartDayOfMonth(dteStart.DateTime.AddMonths(10)))
            paraS(21) = New SqlClient.SqlParameter("@EndT11", GetEndDayOfMonth(dteStart.DateTime.AddMonths(10)))
            paraS(22) = New SqlClient.SqlParameter("@StartT12", GetStartDayOfMonth(dteStart.DateTime.AddMonths(11)))
            paraS(23) = New SqlClient.SqlParameter("@EndT12", GetEndDayOfMonth(dteStart.DateTime.AddMonths(11)))
            paraS(24) = New SqlClient.SqlParameter("@Customer", myCustomer)
            paraS(25) = New SqlClient.SqlParameter("@Size", mySize)
            paraS(26) = New SqlClient.SqlParameter("@Method", myMethod)
            paraS(27) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
            paraS(28) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)

            If rdoRandom.Checked Then
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyNam_2", paraS)
            Else
                GridControl3.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyNam_2_OG", paraS)
            End If
            GridControlSetFormat(GridView3, 2)
            GridControlSetFormatPercentage(GridView3, "TLCP", 2)
            GridView3.BestFitColumns()
            GridView3.Columns("ID").Visible = False
        End If

        'Load grid 2
        If chbDongHang.Checked Then
            'Load chi tiết dòng hàng
            GridView2.Columns.Clear()
            If cbbLoai.Text = "All" Or cbbLoai.Text = "" Then
                para(2) = New SqlClient.SqlParameter("@Method", DBNull.Value)
            Else
                para(2) = New SqlClient.SqlParameter("@Method", cbbLoai.Text)
            End If

            If rdoRandom.Checked Then
                GridControl2.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_LoadByDongHang", para)
            Else
                GridControl2.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_LoadByDongHang_OG", para)
            End If
            GridControlSetFormat(GridView2, 1)
            GridControlSetFormatPercentage(GridView2, "TLCP", 2)
            GridView2.BestFitColumns()
        Else
            'Load chi tiết khách hàng
            Dim paraT(1) As SqlClient.SqlParameter
            paraT(0) = New SqlClient.SqlParameter("@Start", GetStartDate(dteStart.DateTime))
            paraT(1) = New SqlClient.SqlParameter("@End", GetEndDate(dteEnd.DateTime))

            If rdoRandom.Checked Then
                GridControl2.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyCustomer_2", paraT)
            Else
                GridControl2.DataSource = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyCustomer_2_OG", paraT)
            End If
            GridControlSetFormat(GridView2, 2)
            GridControlSetFormatPercentage(GridView2, "TLCP", 2)
            GridView2.BestFitColumns()
            GridView2.Columns("ID").Visible = False
        End If

        'Đổi tỉ lệ
        If rdoPercent.Checked Then
            'Gridview1
            For Each column As GridColumn In GridView1.Columns
                If column.VisibleIndex > GridView1.Columns("TLCP").VisibleIndex Then
                    If chbPercentExport.Checked Then
                        For r = 0 To GridView1.RowCount - 1
                            If GridView1.GetRowCellValue(r, column) IsNot DBNull.Value Then
                                GridView1.SetRowCellValue(r, column, Math.Round((GridView1.GetRowCellValue(r, column) / GridView1.GetRowCellValue(r, "InsQty")), 2))
                            End If
                        Next
                    Else
                        For r = 0 To GridView1.RowCount - 1
                            If GridView1.GetRowCellValue(r, column) IsNot DBNull.Value Then
                                GridView1.SetRowCellValue(r, column, Math.Round((GridView1.GetRowCellValue(r, column) / GridView1.GetRowCellValue(r, "InsQty") * 100), 2))
                            End If
                        Next
                    End If
                End If
            Next
            'Gridview2
            For Each column As GridColumn In GridView2.Columns
                If column.VisibleIndex > GridView2.Columns("TLCP").VisibleIndex Then
                    If chbPercentExport.Checked Then
                        For r = 0 To GridView2.RowCount - 1
                            If GridView2.GetRowCellValue(r, column) IsNot DBNull.Value Then
                                GridView2.SetRowCellValue(r, column, Math.Round((GridView2.GetRowCellValue(r, column) / GridView2.GetRowCellValue(r, "InsQty")), 2))
                            End If
                        Next
                    Else
                        For r = 0 To GridView2.RowCount - 1
                            If GridView2.GetRowCellValue(r, column) IsNot DBNull.Value Then
                                GridView2.SetRowCellValue(r, column, Math.Round((GridView2.GetRowCellValue(r, column) / GridView2.GetRowCellValue(r, "InsQty") * 100), 2))
                            End If
                        Next
                    End If
                End If
            Next
            'Gridview3
            For Each column As GridColumn In GridView3.Columns
                If column.VisibleIndex > GridView3.Columns("TLCP").VisibleIndex Then
                    If rdoPercent.Checked Then
                        If chbPercentExport.Checked Then
                            For r As Integer = 0 To GridView3.RowCount - 1
                                If GridView3.GetRowCellValue(r, column) IsNot DBNull.Value And GridView3.GetRowCellValue(r, "InsQty") IsNot DBNull.Value Then
                                    GridView3.SetRowCellValue(r, column, Math.Round((GridView3.GetRowCellValue(r, column) / GridView3.GetRowCellValue(r, "InsQty")), 2))
                                End If
                            Next
                        Else
                            For r As Integer = 0 To GridView3.RowCount - 1
                                If GridView3.GetRowCellValue(r, column) IsNot DBNull.Value And GridView3.GetRowCellValue(r, "InsQty") IsNot DBNull.Value Then
                                    GridView3.SetRowCellValue(r, column, Math.Round((GridView3.GetRowCellValue(r, column) / GridView3.GetRowCellValue(r, "InsQty") * 100), 2))
                                End If
                            Next
                        End If
                    End If
                End If
            Next
        End If


        '--------------
        If GridView1.RowCount > 0 Then
            Dim cGrid1 As GridColumn = GridView1.Columns("TLCP")
            cGrid1.SummaryItem.SummaryType = SummaryItemType.Custom
            cGrid1.SummaryItem.DisplayFormat = "{0:p}"
            For Each c As GridColumn In GridView1.Columns
                If GridView1.Columns(c.FieldName).VisibleIndex > GridView1.Columns("TLCP").VisibleIndex Then
                    Dim cGrid2 As GridColumn = GridView1.Columns(c.FieldName)
                    cGrid2.SummaryItem.SummaryType = SummaryItemType.Custom
                    cGrid2.SummaryItem.DisplayFormat = "{0:p}"
                End If
            Next
        End If

        If GridView2.RowCount > 0 Then
            Dim cGrid2 As GridColumn = GridView2.Columns("TLCP")
            cGrid2.SummaryItem.SummaryType = SummaryItemType.Custom
            cGrid2.SummaryItem.DisplayFormat = "{0:p}"
        End If

        If GridView3.RowCount > 0 Then
            Dim cGrid3 As GridColumn = GridView3.Columns("TLCP")
            cGrid3.SummaryItem.SummaryType = SummaryItemType.Custom
            cGrid3.SummaryItem.DisplayFormat = "{0:p}"
        End If

        '---------------
        'Anh Tâm chỉ :)))
        'Dim siTLCPGrid1 As New GridColumnSummaryItem()
        'siTLCPGrid1.SummaryType = SummaryItemType.Custom
        'siTLCPGrid1.FieldName = "TLCP"
        'siTLCPGrid1.DisplayFormat = "{0:p}"
        ''siTLCP.DisplayFormat = "{0:n2}"
        'GridView1.Columns("TLCP").Summary.Add(siTLCPGrid1)
        '---------------
        SplashScreenManager1.CloseWaitForm()
    End Sub
    Private Sub GridView1_CustomSummaryCalculate(sender As Object, e As DevExpress.Data.CustomSummaryEventArgs) Handles GridView1.CustomSummaryCalculate
        Dim a As GridColumnSummaryItem = e.Item
        If e.IsTotalSummary Then
            Select Case e.SummaryProcess
                Case CustomSummaryProcess.Start
                Case CustomSummaryProcess.Calculate
                Case CustomSummaryProcess.Finalize
                    If a.FieldName = "TLCP" Then
                        e.TotalValue = GridView1.Columns("OKQty").SummaryItem.SummaryValue / GridView1.Columns("InsQty").SummaryItem.SummaryValue
                        'ElseIf a.FieldName = "0641" Then
                    ElseIf GridView1.Columns(a.FieldName).VisibleIndex > GridView1.Columns("TLCP").VisibleIndex Then
                        Dim val As Decimal = 0
                        For r = 0 To GridView1.RowCount - 1
                            val += IIf(IsDBNull(GridView1.GetRowCellValue(r, a.FieldName)), 0, GridView1.GetRowCellValue(r, a.FieldName))
                        Next
                        'Dim sumStart As Decimal = 0
                        'For r = 0 To GridView1.RowCount - 1
                        '    sumStart += IIf(IsDBNull(GridView1.GetRowCellValue(r, "StartQty")), 0, GridView1.GetRowCellValue(r, "StartQty"))
                        'Next
                        If GridView1.Columns("StartQty").SummaryItem.SummaryValue <> 0 Then
                            e.TotalValue = val / GridView1.Columns("StartQty").SummaryItem.SummaryValue
                        End If
                    End If
            End Select
        End If
    End Sub

    Private Sub GridView2_CustomSummaryCalculate(sender As Object, e As CustomSummaryEventArgs) Handles GridView2.CustomSummaryCalculate
        Dim gridSum As GridSummaryItem = e.Item
        If e.IsTotalSummary Then
            Select Case e.SummaryProcess
                'calculation entry point
                Case CustomSummaryProcess.Start
                    'consequent calculations
                Case CustomSummaryProcess.Calculate
                    'final summary value
                Case CustomSummaryProcess.Finalize
                    If gridSum.FieldName = "TLCP" Then
                        e.TotalValue = GridView2.Columns("OKQty").SummaryItem.SummaryValue / GridView2.Columns("InsQty").SummaryItem.SummaryValue
                    End If
            End Select
        End If
    End Sub

    Private Sub GridView3_CustomSummaryCalculate(sender As Object, e As CustomSummaryEventArgs) Handles GridView3.CustomSummaryCalculate
        Dim gridSum As GridSummaryItem = e.Item
        If e.IsTotalSummary Then
            Select Case e.SummaryProcess
                'calculation entry point
                Case CustomSummaryProcess.Start
                    'consequent calculations
                Case CustomSummaryProcess.Calculate
                    'final summary value
                Case CustomSummaryProcess.Finalize
                    If gridSum.FieldName = "TLCP" Then
                        e.TotalValue = GridView3.Columns("OKQty").SummaryItem.SummaryValue / GridView3.Columns("InsQty").SummaryItem.SummaryValue
                    End If
            End Select
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        Dim listGridView As New List(Of GridView)
        If GridView1.RowCount > 0 Then
            listGridView.Add(GridView1)
        End If
        If GridView2.RowCount > 0 Then
            listGridView.Add(GridView2)
        End If
        If GridView3.RowCount > 0 Then
            listGridView.Add(GridView3)
        End If
        GridControlExportExcels(listGridView, True, , "TLCP", False)
    End Sub

    Private Sub btnTarget_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnTarget.ItemClick
        Dim frm As New FrmTargeCode
        frm.Show()
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        If rdoLotNo.Checked And GridView1.RowCount > 0 And rdoNgay.Checked And
            GridView1.Columns("LotNumber") IsNot Nothing Then
            GridControlReadOnly(GridView1, True)
            GridView1.Columns("Ngay").OptionsColumn.ReadOnly = False
            GridView1.Columns("StartQty").OptionsColumn.ReadOnly = False
            GridView1.Columns("InsQty").OptionsColumn.ReadOnly = False
            GridView1.Columns("Ngay").Width = 100
            GridControlSetColorEdit(GridView1)
        End If
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable = True And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            If e.Column.FieldName = "Ngay" Then
                Dim sqlUpdateDefectDay As String = ""
                If rdoRandom.Checked Then
                    sqlUpdateDefectDay = String.Format("UPDATE RI_DefectCode_Day_2
                                                        SET Ngay = @Value
                                                        WHERE ProductCode = '{0}' 
                                                        AND LotNumber = '{1}'",
                                                        GridView1.GetFocusedRowCellValue("ProductCode"),
                                                        GridView1.GetFocusedRowCellValue("LotNumber"))
                Else
                    sqlUpdateDefectDay = String.Format("UPDATE RI_DefectCode_Day_2_OG
                                                        SET Ngay = @Value
                                                        WHERE ProductCode = '{0}' 
                                                        AND LotNumber = '{1}'",
                                                        GridView1.GetFocusedRowCellValue("ProductCode"),
                                                        GridView1.GetFocusedRowCellValue("LotNumber"))
                End If
                _db.ExecuteNonQuery(sqlUpdateDefectDay, para)
                '-----------

                Dim sqlUpdateDefectPd As String = ""
                If rdoRandom.Checked Then
                    sqlUpdateDefectPd = String.Format("UPDATE RI_DefectCode_Pd_2 
                                                        SET Ngay = @Value 
                                                        WHERE ProductCode = '{0}' 
                                                        AND LotNumber = '{1}'",
                                                        GridView1.GetFocusedRowCellValue("ProductCode"),
                                                        GridView1.GetFocusedRowCellValue("LotNumber"))
                Else
                    sqlUpdateDefectPd = String.Format("UPDATE RI_DefectCode_Pd_2_OG 
                                                        SET Ngay = @Value 
                                                        WHERE ProductCode = '{0}' 
                                                        AND LotNumber = '{1}'",
                                                        GridView1.GetFocusedRowCellValue("ProductCode"),
                                                        GridView1.GetFocusedRowCellValue("LotNumber"))
                End If
                _db.ExecuteNonQuery(sqlUpdateDefectPd, para)
                '------------

                Dim sqlUpdateKQHDay As String = ""
                If rdoRandom.Checked Then
                    sqlUpdateKQHDay = String.Format("UPDATE RI_KQH_Day_2 
                                                    SET Ngay = @Value 
                                                    WHERE ProductCode = '{0}' 
                                                    AND LotNumber = '{1}'",
                                                    GridView1.GetFocusedRowCellValue("ProductCode"),
                                                    GridView1.GetFocusedRowCellValue("LotNumber"))
                Else
                    sqlUpdateKQHDay = String.Format("UPDATE RI_KQH_Day_2_OG 
                                                    SET Ngay = @Value 
                                                    WHERE ProductCode = '{0}' 
                                                    AND LotNumber = '{1}'",
                                                    GridView1.GetFocusedRowCellValue("ProductCode"),
                                                    GridView1.GetFocusedRowCellValue("LotNumber"))
                End If
                _db.ExecuteNonQuery(sqlUpdateKQHDay, para)
                '-----------

                Dim sqlUpdateKQHPd As String = ""
                If rdoRandom.Checked Then
                    sqlUpdateKQHPd = String.Format("UPDATE RI_KQH_Pd_2 
                                                    SET Ngay = @Value 
                                                    WHERE ProductCode = '{0}' 
                                                    AND LotNumber = '{1}'",
                                                    GridView1.GetFocusedRowCellValue("ProductCode"),
                                                    GridView1.GetFocusedRowCellValue("LotNumber"))
                Else
                    sqlUpdateKQHPd = String.Format("UPDATE RI_KQH_Pd_2_OG 
                                                    SET Ngay = @Value 
                                                    WHERE ProductCode = '{0}' 
                                                    AND LotNumber = '{1}'",
                                                    GridView1.GetFocusedRowCellValue("ProductCode"),
                                                    GridView1.GetFocusedRowCellValue("LotNumber"))
                End If
                _db.ExecuteNonQuery(sqlUpdateKQHPd, para)
            Else
                Dim sqlUpdateKQHDay2 As String = ""
                If rdoRandom.Checked Then
                    sqlUpdateKQHDay2 = String.Format("UPDATE RI_KQH_Day_2 
                                                        SET {0} = @Value 
                                                        WHERE ProductCode = '{1}' 
                                                        AND LotNumber = '{2}'",
                                                        e.Column.FieldName,
                                                        GridView1.GetFocusedRowCellValue("ProductCode"),
                                                        GridView1.GetFocusedRowCellValue("LotNumber"))
                Else
                    sqlUpdateKQHDay2 = String.Format("UPDATE RI_KQH_Day_2_OG 
                                                        SET {0} = @Value 
                                                        WHERE ProductCode = '{1}' 
                                                        AND LotNumber = '{2}'",
                                                        e.Column.FieldName,
                                                        GridView1.GetFocusedRowCellValue("ProductCode"),
                                                        GridView1.GetFocusedRowCellValue("LotNumber"))
                End If
                _db.ExecuteNonQuery(sqlUpdateKQHDay2, para)
                '------------

                Dim sqlUpdateKQHPd2 As String = ""
                If rdoRandom.Checked Then
                    sqlUpdateKQHPd2 = String.Format("UPDATE RI_KQH_Pd_2 
                                                    SET {0} = @Value 
                                                    WHERE ProductCode = '{1}' 
                                                    AND LotNumber = '{2}'",
                                                    e.Column.FieldName,
                                                    GridView1.GetFocusedRowCellValue("ProductCode"),
                                                    GridView1.GetFocusedRowCellValue("LotNumber"))
                Else
                    sqlUpdateKQHPd2 = String.Format("UPDATE RI_KQH_Pd_2_OG 
                                                    SET {0} = @Value 
                                                    WHERE ProductCode = '{1}' 
                                                    AND LotNumber = '{2}'",
                                                    e.Column.FieldName,
                                                    GridView1.GetFocusedRowCellValue("ProductCode"),
                                                    GridView1.GetFocusedRowCellValue("LotNumber"))
                End If
                _db.ExecuteNonQuery(sqlUpdateKQHPd2, para)
            End If
        End If
    End Sub

    <Obsolete>
    Private Sub btnTLCP_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnTLCP.ItemClick
        If GridView2.RowCount > 0 Then
            If chbDongHang.Checked And _DongHang <> "" And GridView2.Columns("DongHang") IsNot Nothing Then
                If _dtDongHangDetail.Rows.Count > 0 Then
                    Dim frmDefect As New FrmChartsNew
                    Dim dtTam As DataTable = _dtDongHangDetail.Copy
                    frmDefect._tb = dtTam
                    frmDefect._title = "Top defect mode " + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) + " - " +
                                                            IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + " - " +
                                                            "Dòng hàng " + _DongHang
                    frmDefect.ChartDefectDongHang(dtTam)
                    frmDefect.Show()
                End If
            ElseIf GridView2.Columns("Customer") IsNot Nothing Then
                Dim frmTLCP As New FrmChartsNew
                'Dim dt As DataTable = GridControl2.DataSource
                Dim dtTam As DataTable = GridConvertDataTable(GridView2)
                For r = 0 To dtTam.Rows.Count - 1
                    dtTam.Rows(r)("TLCP") = dtTam.Rows(r)("TLCP") * 100
                Next
                frmTLCP._tb = dtTam
                frmTLCP._title = "TLCP" + " <" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) + " - " +
                                                IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + "> ( " +
                                                dteStart.DateTime.ToString("dd.MM") + " ~ " + dteEnd.DateTime.ToString("dd.MM") + " )"
                frmTLCP.LoadChartTLCP(dtTam)
                frmTLCP.Show()
            End If
        End If
    End Sub
    <Obsolete>
    Private Sub btnDefect_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDefect.ItemClick
        If rdoNgay.Checked Or rdoNam.Checked Then
            If GridView3.RowCount > 0 Then
                Dim target As Decimal = 0
                Dim obj As New PS_Target
                obj.ID_K = GetQuaterByFinancial(dteStart.DateTime)
                _db.GetObject(obj)

                Dim frmDefect As New FrmChartsNew
                'Dim dt As DataTable = GridControl3.DataSource
                Dim dtTam As DataTable = GridConvertDataTable(GridView3)
                If dtTam.Rows(0)(0) = "Target" Then
                    dtTam.Rows.RemoveAt(0)
                End If
                For r = 0 To dtTam.Rows.Count - 1
                    dtTam.Rows(r)("TLCP") = Math.Round(dtTam.Rows(r)("TLCP") * 100, 2)
                Next
                frmDefect._tb = dtTam

                If chbDongHang.Checked Then
                    frmDefect._title = "Biểu đồ lỗi dòng hàng (" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) +
                                        " - " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + ") - (từ " + dteStart.DateTime.ToString("dd.MM") +
                                        " - " + dteEnd.DateTime.ToString("dd.MM") + ") - Dòng hàng " + _DongHang
                Else
                    frmDefect._title = IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) + " ( " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + " ) "
                End If

                If cbbLoai.Text = "Single Side" Then
                    If cbbKhachHang.Text = "SEAGATE" Then
                        target = obj.SEAS
                    ElseIf cbbKhachHang.Text = "TOSHIBA" Then
                        target = obj.TSBS
                    ElseIf cbbKhachHang.Text = "HGST" Then
                        target = obj.HGSTS
                    ElseIf cbbKhachHang.Text = "WESTERN DIGITAL" Then
                        target = obj.WD
                    Else
                        target = obj.TotalS
                    End If
                ElseIf cbbLoai.Text = "Double Side" Then
                    If cbbKhachHang.Text = "SEAGATE" Then
                        target = obj.SEAD
                    ElseIf cbbKhachHang.Text = "TOSHIBA" Then
                        target = obj.TSBD
                    ElseIf cbbKhachHang.Text = "HGST" Then
                        target = obj.HGSTD
                    ElseIf cbbKhachHang.Text = "WESTERN DIGITAL" Then
                        target = obj.WD
                    Else
                        target = obj.TotalD
                    End If
                Else
                    If cbbKhachHang.Text = "SEAGATE" Then
                        target = obj.SEAS
                    ElseIf cbbKhachHang.Text = "TOSHIBA" Then
                        target = obj.TotalS
                    ElseIf cbbKhachHang.Text = "HGST" Then
                        target = obj.HGSTS
                    ElseIf cbbKhachHang.Text = "WESTERN DIGITAL" Then
                        target = obj.WD
                    Else
                        target = obj.Total
                    End If
                End If

                frmDefect.LoadChartDefectDay(target, dtTam)
                frmDefect.Show()
            End If
        ElseIf rdoQuy.Checked Then
            If GridView3.RowCount > 0 Then
                Dim frmDefect As New FrmChartsNew
                'Dim dt As DataTable = GridControl3.DataSource
                Dim dtTam As DataTable = GridConvertDataTable(GridView3)
                frmDefect._tb = dtTam
                If chbDongHang.Checked Then
                    frmDefect._title = "Biểu đồ lỗi dòng hàng (" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) +
                                        " - " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + ") - (từ " + dteStart.DateTime.ToString("dd.MM") +
                                        " - " + dteEnd.DateTime.ToString("dd.MM") + ") - Dòng hàng " + _DongHang
                Else
                    frmDefect._title = "Top defect mode of " + "(" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) + " - " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + " - " + IIf(_ProductCode = "", "All Product", _ProductCode) + ") product"
                End If
                frmDefect.LoadChartDefectQuy(dtTam)
                frmDefect.Show()
            End If
        Else
            If GridView3.RowCount > 0 Then
                Dim frmDefect As New FrmChartsNew
                'Dim dt As DataTable = GridControl3.DataSource
                Dim dtTam As DataTable = GridConvertDataTable(GridView3)
                frmDefect._tb = dtTam
                If chbDongHang.Checked Then
                    frmDefect._title = "Biểu đồ lỗi dòng hàng (" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) +
                                        " - " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + ") - (từ " + dteStart.DateTime.ToString("dd.MM") +
                                        " - " + dteEnd.DateTime.ToString("dd.MM") + ") - Dòng hàng " + _DongHang
                Else
                    frmDefect._title = "Top defect mode of " + "(" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) + " - " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + " - " + IIf(_ProductCode = "", "All Product", _ProductCode) + ") product"
                End If
                frmDefect.LoadChartDefectTuanThang(dtTam)
                frmDefect.Show()
            End If
        End If
    End Sub

    Private Sub GridView1_RowClick(sender As Object, e As RowClickEventArgs) Handles GridView1.RowClick
        If (rdoProduct.Checked Or rdoLotNo.Checked) And GridView1.GetFocusedRowCellValue("ProductCode") <> "" Then
            Dim product As String = GridView1.GetFocusedRowCellValue("ProductCode")
            _ProductCode = product

            Dim myCustomer As Object = DBNull.Value
            Dim mySize As Object = DBNull.Value
            Dim myMethod As Object = DBNull.Value

            If cbbSize.Text <> "" And cbbSize.Text <> "All" Then
                mySize = cbbSize.Text
            End If
            If cbbLoai.Text = "All" Or cbbLoai.Text = "" Then
                myMethod = DBNull.Value
            ElseIf cbbLoai.Text = "Single Side" Then
                myMethod = "01"
            ElseIf cbbLoai.Text = "Double Side" Then
                myMethod = "02"
            End If
            If cbbKhachHang.Text <> "" And cbbKhachHang.Text <> "All" Then
                If cbbKhachHang.Text <> "Other" Then
                    myCustomer = cbbKhachHang.Text
                Else
                    myCustomer = "CANON"
                End If
            End If

            Dim myCustomerT As String = ""
            If cbbKhachHang.Text = "SEAGATE" And cbbLoai.Text = "Single Side" Then
                myCustomerT = "SEAS"
            ElseIf cbbKhachHang.Text = "SEAGATE" And cbbLoai.Text = "Double Side" Then
                myCustomerT = "SEAD"
            ElseIf cbbKhachHang.Text = "SEAGATE" And cbbLoai.Text = "" Then
                myCustomerT = "SEAAll"
            ElseIf cbbKhachHang.Text = "TOSHIBA" And cbbLoai.Text = "Single Side" Then
                myCustomerT = "TSBS"
            ElseIf cbbKhachHang.Text = "TOSHIBA" And cbbLoai.Text = "Double Side" Then
                myCustomerT = "TSBD"
            ElseIf cbbKhachHang.Text = "TOSHIBA" And cbbLoai.Text = "" Then
                myCustomerT = "TSBAll"
            ElseIf cbbKhachHang.Text = "WDC" Then
                myCustomerT = "WDC"
            ElseIf cbbKhachHang.Text = "WESTERN DIGITAL" Then
                myCustomerT = "WDAll"
            ElseIf cbbKhachHang.Text = "OTHER" Then
                myCustomerT = "OTHER"
            ElseIf (cbbKhachHang.Text = "All" Or cbbKhachHang.Text = "") And cbbLoai.Text = "Single Side" Then
                myCustomerT = "Single"
            ElseIf (cbbKhachHang.Text = "All" Or cbbKhachHang.Text = "") And cbbLoai.Text = "Double Side" Then
                myCustomerT = "Double"
            ElseIf (cbbKhachHang.Text = "All" Or cbbKhachHang.Text = "") Then
                myCustomerT = "All"
            End If

            GridView3.Columns.Clear()
            If rdoNgay.Checked Then
                Dim paraS(11) As SqlClient.SqlParameter
                paraS(0) = New SqlClient.SqlParameter("@StartDate", GetStartDate(dteStart.DateTime))
                paraS(1) = New SqlClient.SqlParameter("@EndDate", GetEndDate(dteEnd.DateTime))
                paraS(2) = New SqlClient.SqlParameter("@StartLT", GetStartDayOfMonth(dteStart.DateTime))
                paraS(3) = New SqlClient.SqlParameter("@EndLT", GetEndDate(dteEnd.DateTime))
                paraS(4) = New SqlClient.SqlParameter("@StartM", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-1)))
                paraS(5) = New SqlClient.SqlParameter("@EndM", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-1)))
                paraS(6) = New SqlClient.SqlParameter("@Customer", myCustomer)
                paraS(7) = New SqlClient.SqlParameter("@Size", mySize)
                paraS(8) = New SqlClient.SqlParameter("@Method", myMethod)
                paraS(9) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
                paraS(10) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)
                paraS(11) = New SqlClient.SqlParameter("@Product", product)

                Dim dtNgay As New DataTable
                If rdoRandom.Checked Then
                    dtNgay = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyDay_Product", paraS)
                Else
                    dtNgay = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyDay_Product_OG", paraS)
                End If
                If dtNgay.Rows.Count > 0 Then
                    GridControl3.DataSource = dtNgay
                    GridControlSetFormat(GridView3, 2)
                    GridView3.Columns("ID").Visible = False
                End If
            ElseIf rdoTuan.Checked Then
                Dim paraS(11) As SqlClient.SqlParameter
                paraS(0) = New SqlClient.SqlParameter("@StartT", GetStartDate(dteStart.DateTime))
                paraS(1) = New SqlClient.SqlParameter("@EndT", GetEndDate(dteEnd.DateTime))
                paraS(2) = New SqlClient.SqlParameter("@StartT1", GetStartDate(dteStart.DateTime.AddDays(-7)))
                paraS(3) = New SqlClient.SqlParameter("@EndT1", GetEndDate(dteEnd.DateTime.AddDays(-7)))
                paraS(4) = New SqlClient.SqlParameter("@StartT2", GetStartDate(dteStart.DateTime.AddDays(-14)))
                paraS(5) = New SqlClient.SqlParameter("@EndT2", GetEndDate(dteEnd.DateTime.AddDays(-14)))
                paraS(6) = New SqlClient.SqlParameter("@Customer", myCustomer)
                paraS(7) = New SqlClient.SqlParameter("@Size", mySize)
                paraS(8) = New SqlClient.SqlParameter("@Method", myMethod)
                paraS(9) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
                paraS(10) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)
                paraS(11) = New SqlClient.SqlParameter("@Product", product)

                Dim dtTuan As New DataTable
                If rdoRandom.Checked Then
                    dtTuan = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyTuan_Product", paraS)
                Else
                    dtTuan = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyTuan_Product_OG", paraS)
                End If
                If dtTuan.Rows.Count > 0 Then
                    GridControl3.DataSource = dtTuan
                    GridControlSetFormat(GridView3, 2)
                    GridView3.Columns("ID").Visible = False
                End If
            ElseIf rdoThang.Checked Then
                Dim paraS(11) As SqlClient.SqlParameter
                paraS(0) = New SqlClient.SqlParameter("@StartT", GetStartDate(dteStart.DateTime))
                paraS(1) = New SqlClient.SqlParameter("@EndT", GetEndDate(dteEnd.DateTime))
                paraS(2) = New SqlClient.SqlParameter("@StartT1", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-1)))
                paraS(3) = New SqlClient.SqlParameter("@EndT1", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-1)))
                paraS(4) = New SqlClient.SqlParameter("@StartT2", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-2)))
                paraS(5) = New SqlClient.SqlParameter("@EndT2", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-2)))
                paraS(6) = New SqlClient.SqlParameter("@Customer", myCustomer)
                paraS(7) = New SqlClient.SqlParameter("@Size", mySize)
                paraS(8) = New SqlClient.SqlParameter("@Method", myMethod)
                paraS(9) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
                paraS(10) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)
                paraS(11) = New SqlClient.SqlParameter("@Product", product)

                Dim dtThang As New DataTable
                If rdoRandom.Checked Then
                    dtThang = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyThang_Product", paraS)
                Else
                    dtThang = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyThang_Product_OG", paraS)
                End If
                If dtThang.Rows.Count > 0 Then
                    GridControl3.DataSource = dtThang
                    GridControlSetFormat(GridView3, 2)
                    GridView3.Columns("ID").Visible = False
                End If
            ElseIf rdoQuy.Checked Then
                Dim paraS(15) As SqlClient.SqlParameter
                paraS(0) = New SqlClient.SqlParameter("@StartQ", GetStartDate(dteStart.DateTime))
                paraS(1) = New SqlClient.SqlParameter("@EndQ", GetEndDate(dteEnd.DateTime))
                paraS(2) = New SqlClient.SqlParameter("@StartQ1", GetStartDayOfQuarter(dteStart.DateTime.AddMonths(-3)))
                paraS(3) = New SqlClient.SqlParameter("@EndQ1", GetEndDayOfQuarter(dteStart.DateTime.AddMonths(-3)))

                paraS(4) = New SqlClient.SqlParameter("@StartT1", GetStartDayOfMonth(dteStart.DateTime))
                paraS(5) = New SqlClient.SqlParameter("@EndT1", GetEndDayOfMonth(dteStart.DateTime))
                paraS(6) = New SqlClient.SqlParameter("@StartT2", GetStartDayOfMonth(dteStart.DateTime.AddMonths(1)))
                paraS(7) = New SqlClient.SqlParameter("@EndT2", GetEndDayOfMonth(dteStart.DateTime.AddMonths(1)))
                paraS(8) = New SqlClient.SqlParameter("@StartT3", GetStartDayOfMonth(dteStart.DateTime.AddMonths(2)))
                paraS(9) = New SqlClient.SqlParameter("@EndT3", GetEndDayOfMonth(dteStart.DateTime.AddMonths(2)))
                paraS(10) = New SqlClient.SqlParameter("@Customer", myCustomer)
                paraS(11) = New SqlClient.SqlParameter("@Size", mySize)
                paraS(12) = New SqlClient.SqlParameter("@Method", myMethod)
                paraS(13) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
                paraS(14) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)
                paraS(15) = New SqlClient.SqlParameter("@Product", product)

                Dim dtQuy As New DataTable
                If rdoRandom.Checked Then
                    dtQuy = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyQuy_Product", paraS)
                Else
                    dtQuy = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyQuy_Product_OG", paraS)
                End If
                If dtQuy.Rows.Count > 0 Then
                    GridControl3.DataSource = dtQuy
                    GridControlSetFormat(GridView3, 2)
                    GridView3.Columns("ID").Visible = False
                End If
            ElseIf rdoNam.Checked Then
                Dim paraS(29) As SqlClient.SqlParameter
                paraS(0) = New SqlClient.SqlParameter("@StartT1", GetStartDayOfMonth(dteStart.DateTime))
                paraS(1) = New SqlClient.SqlParameter("@EndT1", GetEndDayOfMonth(dteStart.DateTime))
                paraS(2) = New SqlClient.SqlParameter("@StartT2", GetStartDayOfMonth(dteStart.DateTime.AddMonths(1)))
                paraS(3) = New SqlClient.SqlParameter("@EndT2", GetEndDayOfMonth(dteStart.DateTime.AddMonths(1)))
                paraS(4) = New SqlClient.SqlParameter("@StartT3", GetStartDayOfMonth(dteStart.DateTime.AddMonths(2)))
                paraS(5) = New SqlClient.SqlParameter("@EndT3", GetEndDayOfMonth(dteStart.DateTime.AddMonths(2)))
                paraS(6) = New SqlClient.SqlParameter("@StartT4", GetStartDayOfMonth(dteStart.DateTime.AddMonths(3)))
                paraS(7) = New SqlClient.SqlParameter("@EndT4", GetEndDayOfMonth(dteStart.DateTime.AddMonths(3)))
                paraS(8) = New SqlClient.SqlParameter("@StartT5", GetStartDayOfMonth(dteStart.DateTime.AddMonths(4)))
                paraS(9) = New SqlClient.SqlParameter("@EndT5", GetEndDayOfMonth(dteStart.DateTime.AddMonths(4)))
                paraS(10) = New SqlClient.SqlParameter("@StartT6", GetStartDayOfMonth(dteStart.DateTime.AddMonths(5)))
                paraS(11) = New SqlClient.SqlParameter("@EndT6", GetEndDayOfMonth(dteStart.DateTime.AddMonths(5)))
                paraS(12) = New SqlClient.SqlParameter("@StartT7", GetStartDayOfMonth(dteStart.DateTime.AddMonths(6)))
                paraS(13) = New SqlClient.SqlParameter("@EndT7", GetEndDayOfMonth(dteStart.DateTime.AddMonths(6)))
                paraS(14) = New SqlClient.SqlParameter("@StartT8", GetStartDayOfMonth(dteStart.DateTime.AddMonths(7)))
                paraS(15) = New SqlClient.SqlParameter("@EndT8", GetEndDayOfMonth(dteStart.DateTime.AddMonths(7)))
                paraS(16) = New SqlClient.SqlParameter("@StartT9", GetStartDayOfMonth(dteStart.DateTime.AddMonths(8)))
                paraS(17) = New SqlClient.SqlParameter("@EndT9", GetEndDayOfMonth(dteStart.DateTime.AddMonths(8)))
                paraS(18) = New SqlClient.SqlParameter("@StartT10", GetStartDayOfMonth(dteStart.DateTime.AddMonths(9)))
                paraS(19) = New SqlClient.SqlParameter("@EndT10", GetEndDayOfMonth(dteStart.DateTime.AddMonths(9)))
                paraS(20) = New SqlClient.SqlParameter("@StartT11", GetStartDayOfMonth(dteStart.DateTime.AddMonths(10)))
                paraS(21) = New SqlClient.SqlParameter("@EndT11", GetEndDayOfMonth(dteStart.DateTime.AddMonths(10)))
                paraS(22) = New SqlClient.SqlParameter("@StartT12", GetStartDayOfMonth(dteStart.DateTime.AddMonths(11)))
                paraS(23) = New SqlClient.SqlParameter("@EndT12", GetEndDayOfMonth(dteStart.DateTime.AddMonths(11)))
                paraS(24) = New SqlClient.SqlParameter("@Customer", myCustomer)
                paraS(25) = New SqlClient.SqlParameter("@Size", mySize)
                paraS(26) = New SqlClient.SqlParameter("@Method", myMethod)
                paraS(27) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
                paraS(28) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)
                paraS(29) = New SqlClient.SqlParameter("@Product", product)

                Dim dtNam As New DataTable
                If rdoRandom.Checked Then
                    dtNam = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyNam_Product", paraS)
                Else
                    dtNam = _db.ExecuteStoreProcedureTB("sp_RI_GetSumbyNam_Product_OG", paraS)
                End If
                If dtNam.Rows.Count > 0 Then
                    GridControl3.DataSource = dtNam
                    GridControlSetFormat(GridView3, 2)
                    GridView3.Columns("ID").Visible = False
                End If
            End If

            If GridView3.RowCount > 0 Then
                Dim c As GridColumn = GridView3.Columns("TLCP")
                c.DisplayFormat.FormatType = Utils.FormatType.Numeric
                c.DisplayFormat.FormatString = "{0:p}"

                Dim cGrid3 As GridColumn = GridView3.Columns("TLCP")
                cGrid3.SummaryItem.SummaryType = SummaryItemType.Custom
                cGrid3.SummaryItem.DisplayFormat = "{0:p}"

                GridView3.BestFitColumns()

                If rdoPercent.Checked Then
                    For Each column As GridColumn In GridView3.Columns
                        If column.VisibleIndex > GridView3.Columns("TLCP").VisibleIndex Then
                            For r As Integer = 0 To GridView3.DataRowCount - 1
                                If GridView3.GetRowCellValue(r, column) IsNot DBNull.Value And GridView3.GetRowCellValue(r, "InsQty") IsNot DBNull.Value Then
                                    GridView3.SetRowCellValue(r, column, Math.Round(((GridView3.GetRowCellValue(r, column) * 100) / GridView3.GetRowCellValue(r, "InsQty")), 2))
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub dteStartGetData_EditValueChanged(sender As Object, e As EventArgs) Handles dteStartGetData.EditValueChanged
        If dteStartGetData.DateTime > dteEndGetData.DateTime Then
            dteEndGetData.EditValue = dteStartGetData.DateTime
        End If
    End Sub

    Private Sub dteEndGetData_EditValueChanged(sender As Object, e As EventArgs) Handles dteEndGetData.EditValueChanged
        If dteEndGetData.DateTime < dteStartGetData.DateTime Then
            dteStartGetData.EditValue = dteEndGetData.DateTime
        End If
    End Sub

    Private Sub btnLayDuLieu_Click(sender As Object, e As EventArgs) Handles btnLayDuLieu.Click
        SplashScreenManager1.ShowWaitForm()
        Try
            _db.BeginTransaction()
            Dim startDay As DateTime = GetStartDate(dteStartGetData.DateTime)
            Dim endDay As DateTime = GetStartDate(dteEndGetData.DateTime)
            While startDay <= endDay
                Dim para(0) As SqlClient.SqlParameter
                para(0) = New SqlClient.SqlParameter("@Today", startDay)
                If rdoRandom.Checked Then
                    _db.ExecuteStoreProcedure("sp_RI_GetAutoDay_2", para)
                Else
                    _db.ExecuteStoreProcedure("sp_RI_GetAutoDay_2_OG", para)
                End If

                'Xóa Code bị dư -> Chưa khả thi
                'Dim paraDt(0) As SqlClient.SqlParameter
                'paraDt(0) = New SqlClient.SqlParameter("@Ngay", startDay)
                'Dim dt As New DataTable
                'If rdoRandom.Checked Then
                '    dt = _db.FillDataTable("SELECT ProductCode, LotNumber
                '                        FROM RI_DefectCode_Day_2
                '                        WHERE Ngay = @Ngay
                '                        GROUP BY ProductCode, LotNumber", paraDt)
                'Else
                '    dt = _db.FillDataTable("SELECT ProductCode, LotNumber
                '                        FROM RI_DefectCode_Day_2_OG
                '                        WHERE Ngay = @Ngay
                '                        GROUP BY ProductCode, LotNumber", paraDt)
                'End If
                'For Each r As DataRow In dt.Rows
                '    Dim paraDel(2) As SqlClient.SqlParameter
                '    paraDel(0) = New SqlClient.SqlParameter("@ProductCode", r("ProductCode"))
                '    paraDel(1) = New SqlClient.SqlParameter("@LotNumber", r("LotNumber"))
                '    paraDel(2) = New SqlClient.SqlParameter("@Today", startDay)
                '    If rdoRandom.Checked Then
                '        _db.ExecuteStoreProcedure("sp_RI_GetAutoDay_2_DeleteCode", paraDel)
                '    Else
                '        _db.ExecuteStoreProcedure("sp_RI_GetAutoDay_2_DeleteCode_OG", paraDel)
                '    End If
                'Next
                startDay = startDay.AddDays(1)
            End While
            _db.Commit()
            ShowSuccess()
        Catch ex As Exception
            _db.RollBack()
            ShowWarning(ex.Message)
        End Try
        SplashScreenManager1.CloseWaitForm()
    End Sub

    Public Sub ViewAccess()
        btnEdit.Enabled = False
        btnLayDuLieu.Enabled = False
    End Sub
    Public Sub FullAccess()
        btnEdit.Enabled = True
        btnLayDuLieu.Enabled = True
    End Sub

    Private Sub FrmReportDetail_2_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Dim MainUser As New Main_UserRight
        MainUser.FormID_K = Tag
        MainUser.UserID_K = CurrentUser.UserID
        _db.GetObject(MainUser)
        If MainUser.IsEdit = "1" Then
            FullAccess()
        Else
            ViewAccess()
        End If
    End Sub

    Private Sub GridView2_RowClick(sender As Object, e As RowClickEventArgs) Handles GridView2.RowClick
        If chbDongHang.Checked And GridView2.Columns("DongHang") IsNot Nothing Then
            GridView3.Columns.Clear()
            Dim dongHang As String = GridView2.GetFocusedRowCellValue("DongHang")
            _DongHang = dongHang

            Dim myCustomer As Object = DBNull.Value
            Dim mySize As Object = DBNull.Value
            Dim myMethod As Object = DBNull.Value
            Dim myCustomerT As Object = DBNull.Value

            If cbbSize.Text <> "" And cbbSize.Text <> "All" Then
                mySize = cbbSize.Text
            End If
            If cbbLoai.Text = "All" Or cbbLoai.Text = "" Then
                myMethod = DBNull.Value
            ElseIf cbbLoai.Text = "Single Side" Then
                myMethod = "01"
            ElseIf cbbLoai.Text = "Double Side" Then
                myMethod = "02"
            End If
            If cbbKhachHang.Text <> "" And cbbKhachHang.Text <> "All" Then
                If cbbKhachHang.Text <> "Other" Then
                    myCustomer = cbbKhachHang.Text
                Else
                    myCustomer = "CANON"
                End If
            End If

            Dim paraS(11) As SqlClient.SqlParameter
            paraS(0) = New SqlClient.SqlParameter("@StartT", GetStartDate(dteStart.DateTime))
            paraS(1) = New SqlClient.SqlParameter("@EndT", GetEndDate(dteEnd.DateTime))
            paraS(2) = New SqlClient.SqlParameter("@StartT1", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-1)))
            paraS(3) = New SqlClient.SqlParameter("@EndT1", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-1)))
            paraS(4) = New SqlClient.SqlParameter("@StartT2", GetStartDayOfMonth(dteStart.DateTime.AddMonths(-2)))
            paraS(5) = New SqlClient.SqlParameter("@EndT2", GetEndDayOfMonth(dteStart.DateTime.AddMonths(-2)))
            paraS(6) = New SqlClient.SqlParameter("@Customer", myCustomer)
            paraS(7) = New SqlClient.SqlParameter("@Size", mySize)
            paraS(8) = New SqlClient.SqlParameter("@Method", myMethod)
            paraS(9) = New SqlClient.SqlParameter("@YYYY", ("QAll" + GetQuaterByFinancial(dteStart.DateTime).Substring(2, 4)))
            paraS(10) = New SqlClient.SqlParameter("@CustomerTarget", myCustomerT)
            paraS(11) = New SqlClient.SqlParameter("@DongHang", dongHang)

            Dim dtThang As New DataTable
            If rdoRandom.Checked Then
                dtThang = _db.ExecuteStoreProcedureTB("sp_RI_LoadSumByThang_DongHang", paraS)
            Else
                dtThang = _db.ExecuteStoreProcedureTB("sp_RI_LoadSumByThang_DongHang_OG", paraS)
            End If
            If dtThang.Rows.Count > 0 Then
                GridControl3.DataSource = dtThang
                GridControlSetFormat(GridView3, 2)
                GridView3.Columns("ID").Visible = False
                If rdoPercent.Checked Then
                    For Each column As GridColumn In GridView3.Columns
                        If column.VisibleIndex > GridView3.Columns("TLCP").VisibleIndex Then
                            If rdoPercent.Checked Then
                                If chbPercentExport.Checked Then
                                    For r As Integer = 0 To GridView3.RowCount - 1
                                        If GridView3.GetRowCellValue(r, column) IsNot DBNull.Value Then
                                            GridView3.SetRowCellValue(r, column, Math.Round((GridView3.GetRowCellValue(r, column) / GridView3.GetRowCellValue(r, "InsQty")), 2))
                                        End If
                                    Next
                                Else
                                    For r As Integer = 0 To GridView3.RowCount - 1
                                        If GridView3.GetRowCellValue(r, column) IsNot DBNull.Value Then
                                            GridView3.SetRowCellValue(r, column, Math.Round((GridView3.GetRowCellValue(r, column) / GridView3.GetRowCellValue(r, "InsQty") * 100), 2))
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next
                End If
                GridControlSetFormatPercentage(GridView3, "TLCP", 2)

                Dim cGrid3 As GridColumn = GridView3.Columns("TLCP")
                cGrid3.SummaryItem.SummaryType = SummaryItemType.Custom
                cGrid3.SummaryItem.DisplayFormat = "{0:p}"

                GridView3.BestFitColumns()
            End If

            'Lấy dữ liệu cho chart Detail
            Dim para(5) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@StartDate", GetStartDate(dteStart.DateTime))
            para(1) = New SqlClient.SqlParameter("@EndDate", GetEndDate(dteEnd.DateTime))

            If cbbLoai.Text = "All" Or cbbLoai.Text = "" Then
                para(2) = New SqlClient.SqlParameter("@Method", DBNull.Value)
            Else
                para(2) = New SqlClient.SqlParameter("@Method", cbbLoai.Text)
            End If
            If cbbKhachHang.Text <> "" And cbbKhachHang.Text <> "All" Then
                If cbbKhachHang.Text <> "Other" Then
                    para(3) = New SqlClient.SqlParameter("@Customer", cbbKhachHang.Text.Trim)
                    myCustomer = cbbKhachHang.Text
                Else
                    para(3) = New SqlClient.SqlParameter("@Customer", "CANON")
                    myCustomer = "CANON"
                End If
            Else
                para(3) = New SqlClient.SqlParameter("@Customer", DBNull.Value)
            End If
            If cbbSize.Text <> "" And cbbSize.Text <> "All" Then
                para(4) = New SqlClient.SqlParameter("@Size", cbbSize.Text)
            Else
                para(4) = New SqlClient.SqlParameter("@Size", DBNull.Value)
            End If
            para(5) = New SqlClient.SqlParameter("@DongHang", dongHang)

            If rdoRandom.Checked Then
                _dtDongHangDetail = _db.ExecuteStoreProcedureTB("sp_RI_LoadByDongHang_Detail", para)
            Else
                _dtDongHangDetail = _db.ExecuteStoreProcedureTB("sp_RI_LoadByDongHang_Detail_OG", para)
            End If

            If rdoPercent.Checked Then
                For Each c As DataColumn In _dtDongHangDetail.Columns
                    If _dtDongHangDetail.Columns.IndexOf(c.ColumnName) > _dtDongHangDetail.Columns.IndexOf("TLCP") Then
                        For Each r As DataRow In _dtDongHangDetail.Rows
                            If Not IsDBNull(r(c)) Then
                                r(c) = Math.Round(r(c) / r("InsQty") * 100, 2)
                            End If
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    <Obsolete>
    Private Sub btnBieuDoDongHang_ItemClick(sender As Object, e As XtraBars.ItemClickEventArgs) Handles btnBieuDoDongHang.ItemClick
        If GridView2.RowCount > 0 Then
            If chbDongHang.Checked And GridView2.Columns("DongHang") IsNot Nothing Then
                If _DongHang = "" Then
                    'Dim dt As DataTable = GridControl2.DataSource
                    'Dim dtCopy As DataTable = dt.Copy
                    Dim dtCopy As DataTable = GridConvertDataTable(GridView2)
                    Dim frm As New FrmChartsNew
                    frm._title = "Biểu đồ lỗi dòng hàng (" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) +
                                 " - " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + ") - (từ " + dteStart.DateTime.ToString("dd.MM") +
                                 " - " + dteEnd.DateTime.ToString("dd.MM") + ") - Dòng hàng " + IIf(_DongHang = "", "All", _DongHang)
                    'frm._title = "Biểu đồ lỗi dòng hàng (từ " + dteStart.DateTime.ToString("dd.MM") + " - " + dteEnd.DateTime.ToString("dd.MM") + ")"
                    frm.ChartDongHang(dtCopy)
                    frm._tb = dtCopy
                    frm.Show()
                Else
                    If _dtDongHangDetail.Rows.Count > 0 Then
                        Dim dt As DataTable = _dtDongHangDetail.Copy
                        Dim frm As New FrmChartsNew
                        frm._title = "Biểu đồ lỗi dòng hàng (" + IIf(cbbKhachHang.Text = "", "All Customer", cbbKhachHang.Text) +
                                     " - " + IIf(cbbLoai.Text = "", "All Side", cbbLoai.Text) + ") - (từ " + dteStart.DateTime.ToString("dd.MM") +
                                     " - " + dteEnd.DateTime.ToString("dd.MM") + ") - Dòng hàng " + IIf(_DongHang = "", "All", _DongHang)
                        frm.ChartDongHangDetail(dt)
                        frm._tb = dt
                        frm.Show()
                    End If
                End If
            End If
        End If
    End Sub
    Private Function GridConvertDataTable(gridv As GridView) As DataTable
        Dim dt As New DataTable()
        For Each column As GridColumn In gridv.VisibleColumns
            dt.Columns.Add(column.FieldName, column.ColumnType)
        Next column
        For i As Integer = 0 To gridv.DataRowCount - 1
            Dim row As DataRow = dt.NewRow()
            For Each column As GridColumn In gridv.VisibleColumns
                row(column.FieldName) = gridv.GetRowCellValue(i, column)
            Next column
            dt.Rows.Add(row)
        Next i
        Return dt
    End Function
End Class