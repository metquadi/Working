﻿Imports System.Windows.Forms
Imports CommonDB
Imports PublicUtility
'Imports LibEntity
Imports DataGridViewAutoFilter



Public Class FrmNangLucIQC : Inherits DevExpress.XtraEditors.XtraForm
	Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)



	Private Sub grid_CellValueChanged(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles grid.CellValueChanged
		If e.RowIndex >= 0 Then
			Dim obj As New EMM_NangLucIQC
			obj.Ngay_K = dtpStart.Value.Date
			obj.HangMucSub_K = grid.CurrentRow.Cells(HangMucSub.Name).Value
			_db.GetObjectNotReset(obj)
			obj.HangMuc = grid.CurrentRow.Cells(HangMuc.Name).Value
			If grid.CurrentRow.Cells(TGC.Name).Value IsNot DBNull.Value Then
				obj.TGC = grid.CurrentRow.Cells(TGC.Name).Value
			Else
				obj.TGC = 0
			End If
			If grid.CurrentRow.Cells(SoLot.Name).Value IsNot DBNull.Value Then
				obj.SoLot = grid.CurrentRow.Cells(SoLot.Name).Value
			Else
				obj.SoLot = 0
			End If
			If grid.CurrentRow.Cells(GhiChu.Name).Value IsNot DBNull.Value Then
				obj.GhiChu = grid.CurrentRow.Cells(GhiChu.Name).Value
			Else
				obj.GhiChu = ""
			End If
			If _db.ExistObject(obj) Then
				_db.Update(obj)
			Else
				_db.Insert(obj)
			End If
			grid.CurrentRow.Cells(TongTG.Name).Value = obj.TGC * obj.SoLot
		End If
	End Sub

	Private Sub mnuShowAll_Click(sender As Object, e As EventArgs) Handles mnuShowAll.Click
		Dim para(0) As SqlClient.SqlParameter
		para(0) = New SqlClient.SqlParameter("@Ngay", GetStartDate(dtpStart.Value.Date))
		Dim bd As New BindingSource
		Dim dtAll As DataTable = _db.ExecuteStoreProcedureTB("sp_EMM_NangLucIQC", para)
		bd.DataSource = dtAll
		grid.DataSource = bd
		bdn.BindingSource = bd
	End Sub

	Private Sub mnuExport_Click(sender As Object, e As EventArgs) Handles mnuExport.Click
		ExportEXCEL(grid)
	End Sub

	Private Sub mnuMasterIQC_Click(sender As Object, e As EventArgs) Handles mnuMasterIQC.Click
		Dim frm As New FrmMasterNangLucIQC
		frm.ShowDialog()
		mnuShowAll.PerformClick()
	End Sub

	Private Sub dtpStart_ValueChanged(sender As Object, e As EventArgs) Handles dtpStart.ValueChanged
		mnuShowAll.PerformClick()
	End Sub
End Class