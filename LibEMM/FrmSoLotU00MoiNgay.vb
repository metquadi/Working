﻿Imports System.Windows.Forms
Imports CommonDB
Imports PublicUtility
'Imports LibEntity
Imports DataGridViewAutoFilter


Public Class FrmSoLotU00MoiNgay : Inherits DevExpress.XtraEditors.XtraForm
	Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)

	Private Sub mnuMasterIQC_Click(sender As Object, e As EventArgs) Handles mnuMasterIQC.Click
		Dim frm As New FrmMasterSoLoU00
		frm.ShowDialog()
		mnuShowAll.PerformClick()
	End Sub

	Private Sub mnuShowAll_Click(sender As Object, e As EventArgs) Handles mnuShowAll.Click
		Dim para(1) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@StartDate", GetStartDate(dtpStart.Value.Date))
		para(1) = New SqlClient.SqlParameter("@EndDate", GetEndDate(dtpEnd.Value.Date))
		Dim bd As New BindingSource
		Dim dtAll As DataTable = _db.ExecuteStoreProcedureTB("[sp_EMM_LoadSoLotU00]", para)
		bd.DataSource = dtAll
		grid.DataSource = bd
		bdn.BindingSource = bd
	End Sub

	Private Sub mnuExport_Click(sender As Object, e As EventArgs) Handles mnuExport.Click
		ExportEXCEL(grid)
	End Sub
End Class