﻿Imports CommonDB
Imports PublicUtility
Public Class FrmProgramMaster
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Private Sub FrmProgramMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnShow.PerformClick()
    End Sub
    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        GridControl1.DataSource = _db.FillDataTable("SELECT TrainingCode, TrainingField, TrainingProgram, TrainingContent,
                                                        TrainingMethod, Trainer, Trainee, TrainingFrequency
                                                     FROM GA_TRM_ProgramMaster
                                                    WHERE TrainingCode NOT LIKE 'N%'")
        GridControlSetFormat(GridView1)
        GridView1.Columns("TrainingField").Width = 100
        GridView1.Columns("TrainingProgram").Width = 300
        GridView1.Columns("TrainingContent").Width = 200
        GridView1.Columns("TrainingMethod").Width = 135
        GridView1.Columns("Trainee").Width = 180
        GridView1.Columns("TrainingFrequency").Width = 150
    End Sub

    Private Sub btnImport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnImport.ItemClick
        Dim dt = ImportEXCEL(True)
        If dt.Rows.Count > 0 Then
            Dim succ = 0
            Try
                _db.BeginTransaction()
                For Each r As DataRow In dt.Rows
                    If IsDBNull(r("Training Code")) Then Continue For
                    Dim obj As New GA_TRM_ProgramMaster
                    obj.TrainingCode_K = r("Training Code")
                    obj.TrainingField = r("Training Field")
                    obj.TrainingProgram = r("Training Program")
                    obj.TrainingContent = r("Training Content")
                    obj.TrainingMethod = r("Training Method")
                    obj.Trainer = r("Trainer")
                    obj.Trainee = r("Trainee")
                    obj.TrainingFrequency = r("Training Frequency")
                    obj.CreateUser = CurrentUser.UserID
                    obj.CreateDate = DateTime.Now
                    If _db.ExistObject(obj) Then
                        _db.Update(obj)
                    Else
                        _db.Insert(obj)
                        succ += 1
                    End If
                Next
                _db.Commit()
                ShowSuccess(succ)
            Catch ex As Exception
                _db.RollBack()
                ShowWarning(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(GridView1)
    End Sub

End Class