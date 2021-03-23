﻿Imports System.Windows.Forms
Imports CommonDB
Imports PublicUtility
'Imports LibEntity
Imports DataGridViewAutoFilter

Imports System.Text
Imports vb = Microsoft.VisualBasic
Imports System.Globalization
Public Class FrmCheckQty
    Public myToday As DateTime = DateTime.Now
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)


    Sub LoadAll()
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Today", myToday.Date)
        Dim bd As New BindingSource
        Dim dtAll As DataTable = _db.ExecuteStoreProcedureTB("sp_EMM_CheckQtyTempB", para)
        bd.DataSource = dtAll
        gridD.DataSource = bd
        bdn.BindingSource = bd
    End Sub

    Private Sub mnuShowAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuShowAll.Click
        LoadAll()
    End Sub

    Private Sub mnuExport_Click(sender As System.Object, e As System.EventArgs) Handles mnuExport.Click
        ExportEXCEL(gridD)
    End Sub
End Class