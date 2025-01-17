﻿Imports CommonDB
Imports PublicUtility
Public Class FrmMaterialMaster
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)

    Private Sub FrmMaterialMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnShow.PerformClick()
    End Sub
    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        Dim dt = _db.FillDataTable("SELECT h.JCode, d.ItemNumberName, d.StandardName, d.Remarks AS RemarkFpics, 
                                    d.UnitCode, h.Kind, h.LTPoBWH, h.LTUseBWH, h.TonAnToanBWH, h.MOQBWH,
                                    h.LTPoNDV, h.LTUseNDV, h.TonAnToanNDV, h.MOQNDV, h.LeadTimeSX,
                                    h.Package, h.HanSuDung, h.Person, h.Process, h.Remark
                                    FROM PLM_MaterialMaster AS h
                                    LEFT JOIN [10.153.1.30].[FPiCS-B03].[dbo].[t_ASMaterialItem] AS d
                                    ON h.JCode COLLATE DATABASE_DEFAULT = d.ItemCode COLLATE DATABASE_DEFAULT
                                    ORDER BY JCode")
        GridControl1.DataSource = dt
        GridControlSetFormat(GridView1, 1)
        GridView1.Columns("ItemNumberName").Width = 150
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        GridControlReadOnly(GridView1, True)
        GridView1.Columns("Kind").OptionsColumn.ReadOnly = False
        GridView1.Columns("LTPoBWH").OptionsColumn.ReadOnly = False
        GridView1.Columns("LTUseBWH").OptionsColumn.ReadOnly = False
        GridView1.Columns("TonAnToanBWH").OptionsColumn.ReadOnly = False
        GridView1.Columns("MOQBWH").OptionsColumn.ReadOnly = False
        GridView1.Columns("LTPoNDV").OptionsColumn.ReadOnly = False
        GridView1.Columns("LTUseNDV").OptionsColumn.ReadOnly = False
        GridView1.Columns("TonAnToanNDV").OptionsColumn.ReadOnly = False
        GridView1.Columns("MOQNDV").OptionsColumn.ReadOnly = False
        GridView1.Columns("LeadTimeSX").OptionsColumn.ReadOnly = False
        GridView1.Columns("Package").OptionsColumn.ReadOnly = False
        GridView1.Columns("HanSuDung").OptionsColumn.ReadOnly = False
        GridView1.Columns("Person").OptionsColumn.ReadOnly = False
        GridView1.Columns("Process").OptionsColumn.ReadOnly = False
        GridView1.Columns("Remark").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format("UPDATE PLM_MaterialMaster
                                                SET {0} = @Value
                                                WHERE JCode = '{1}'",
                                                e.Column.FieldName,
                                                GridView1.GetFocusedRowCellValue("JCode")), para)
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(GridView1)
    End Sub
End Class