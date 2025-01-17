﻿Imports CommonDB
Imports DevExpress.XtraGrid.Views.Grid
Imports LibEntity
Imports PublicUtility

Public Class FrmPermissionControl
    Dim db As DBSql
    Public isGroup As Boolean = False
    Public ID As String = ""
    Public UserID As String = ""
    Public GroupID As String = ""
    Public _grid As GridView


    Private Sub FrmPermissionControl_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        db = New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
        LoadAll()
    End Sub

    Sub LoadAll()

        Dim objU As New Main_UserRight
        objU.FormID_K = ID
        objU.UserID_K = UserID
        db.GetObject(objU)
        ckoEdit.Checked = objU.IsEdit
        ckoIsAdmin.Checked = objU.IsAdmin

        Dim controlText As String = ""
        Select Case PublicConst.Language
            Case PublicConst.EnumLanguage.China
                controlText = "TextChina"
            Case PublicConst.EnumLanguage.English
                controlText = "TextEnglish"
            Case PublicConst.EnumLanguage.Japan
                controlText = "TextJapan"
            Case PublicConst.EnumLanguage.VietNam
                controlText = "TextVietnam"
        End Select

        Dim sql As String = String.Format(" select control.ControlName,{2} as ControlText, cast(0 as bit) as 'Check' from {0} control " +
                                          " inner join {3} lg on lg.ControlName=control.ControlName and lg.FormID=control.FormID " +
                                          " where control.FormID='{1}' ",
                                          PublicTable.Table_Main_ControlRight,
                                          ID,
                                          controlText,
                                          PublicTable.Table_Main_Language)
        GridControl1.DataSource = db.FillDataTable(sql)
        'Set Permission 
        Dim sqlControl As String = String.Format(" select * from {0} where userID='{1}' and FormID='{2}' ",
                                                     PublicTable.Table_Main_UserRightDetail, UserID, ID)
        Dim userP() As Main_UserRightDetail = db.GetObjects(Of Main_UserRightDetail)(sqlControl)
        If userP IsNot Nothing Then
            For Each u As Main_UserRightDetail In userP
                For row As Integer = 0 To GridView1.RowCount - 1
                    If u.ControlName_K = GridView1.GetRowCellValue(row, "ControlName") Then
                        GridView1.SetRowCellValue(row, "Check", True)
                        Exit For
                    End If
                Next
            Next
        End If

    End Sub


    Private Sub mnuUnCheck_Click(sender As System.Object, e As System.EventArgs) Handles mnuUnCheck.Click
        If GridView1.RowCount > 0 Then
            If Not isGroup Then
                For row As Integer = 0 To GridView1.RowCount - 1
                    GridView1.SetRowCellValue(row, "Check", False)
                    Dim obj As New Main_UserRightDetail
                    For Each r As Integer In _grid.GetSelectedRows
                        obj.UserID_K = _grid.GetRowCellValue(r, "UserID")
                        obj.FormID_K = ID
                        obj.ControlName_K = GridView1.GetRowCellValue(row, "ControlName")
                        db.Delete(obj)
                    Next
                Next
            End If
        End If
    End Sub

    Private Sub mnuCheckAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuCheckAll.Click
        If GridView1.RowCount > 0 Then
            For row As Integer = 0 To GridView1.RowCount - 1
                GridView1.SetRowCellValue(row, "Check", True)
                Dim obj As New Main_UserRightDetail
                For Each r As Integer In _grid.GetSelectedRows
                    obj.UserID_K = _grid.GetRowCellValue(r, "UserID")
                    obj.FormID_K = ID
                    obj.ControlName_K = GridView1.GetRowCellValue(row, "ControlName")
                    obj.CreateDate = DateTime.Now
                    obj.CreateUser = CurrentUser.UserID
                    If Not db.ExistObject(obj) Then
                        db.Insert(obj)
                    End If
                Next
            Next
        End If

    End Sub

    Private Sub FrmPermissionControl_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.Column.FieldName = "Check" Then
            If GridView1.GetFocusedRowCellValue("Check") IsNot DBNull.Value Then
                If GridView1.GetFocusedRowCellValue("Check") Then
                    GridView1.SetFocusedRowCellValue("Check", False)
                Else
                    GridView1.SetFocusedRowCellValue("Check", True)
                End If
            Else
                GridView1.SetFocusedRowCellValue("Check", True)
            End If

            Dim cko As Boolean = GridView1.GetFocusedRowCellValue("Check")
            Dim obj As New Main_UserRightDetail
            For Each r As Integer In _grid.GetSelectedRows
                obj.UserID_K = _grid.GetRowCellValue(r, "UserID")
                obj.FormID_K = ID
                obj.ControlName_K = GridView1.GetFocusedRowCellValue("ControlName")
                obj.CreateDate = DateTime.Now
                obj.CreateUser = CurrentUser.UserID
                If cko Then
                    If Not db.ExistObject(obj) Then
                        db.Insert(obj)
                    End If
                Else
                    db.Delete(obj)
                End If
            Next
        End If
    End Sub

    Private Sub grid_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Delete Then
            If ShowQuestionDelete() = Windows.Forms.DialogResult.No Then Return

            Dim obj As New Main_ControlRight
            obj.FormID_K = ID
            obj.ControlName_K = GridView1.GetFocusedRowCellValue("ControlName")
            db.Delete(obj)

            Dim objL As New Main_Language
            objL.ControlName_K = obj.ControlName_K
            objL.FormID_K = ID
            db.Delete(objL)

            GridView1.DeleteSelectedRows()
        End If
    End Sub

    Private Sub FrmPermissionControl_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        For Each r As Integer In _grid.GetSelectedRows
            Dim objU As New Main_UserRight
            objU.FormID_K = ID
            objU.UserID_K = _grid.GetRowCellValue(r, "UserID")
            objU.IsEdit = ckoEdit.Checked
            objU.IsAdmin = ckoIsAdmin.Checked
            db.Update(objU)
        Next
    End Sub

    Private Sub mnuDelete_Click(sender As Object, e As EventArgs) Handles mnuDelete.Click
        For Each row In GridView1.GetSelectedRows
            Dim obj As New Main_Language
            obj.FormID_K = ID
            obj.ControlName_K = GridView1.GetRowCellValue(row, "ControlName")
            db.Delete(obj)
        Next
        LoadAll()
    End Sub
End Class