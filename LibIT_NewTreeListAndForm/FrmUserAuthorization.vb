﻿Imports CommonDB
Imports PublicUtility
Imports System.Drawing
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils.Layout

Public Class FrmUserAuthorization
    Dim _db As New DBSql(PublicUtility.PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim rowClick As Boolean = False
    Private Sub FrmTreeListAuthorization_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnShow.PerformClick()
    End Sub
    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        GridControl1.DataSource = _db.ExecuteStoreProcedureTB("sp_Main_LoadUser")
        GridControlSetFormat(GridView1, 2)
        GridView1.Columns("FullName").Width = 150
        GridView1.Columns("Section").Width = 100
        GridView1.Columns("GroupName").Width = 100
        GridView1.Columns("Observation").Width = 100
        GridView1.Columns("Email").Width = 100
        ShowTreeListAuthoriz()
    End Sub
    Private Sub btnNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnNew.ItemClick
        txtUserID.Text = ""
        txtUserName.Text = ""
        txtPassword.Text = ""
        txtUserID.Select()
    End Sub
    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        txtUserID.Text = GridView1.GetFocusedRowCellValue("UserID")
        txtUserName.Text = GridView1.GetFocusedRowCellValue("UserName")
        txtPassword.Text = DecryptPassword(GridView1.GetFocusedRowCellValue("Password"))
        If Not chbPassword.Checked Then
            txtPassword.Properties.PasswordChar = "*"c
        End If
        GridControlReadOnly(GridView1, True)
        GridView1.Columns("Email").OptionsColumn.ReadOnly = False
        GridView1.Columns("Lemon").OptionsColumn.ReadOnly = False
        GridView1.Columns("AS400").OptionsColumn.ReadOnly = False
        GridView1.Columns("Ariba").OptionsColumn.ReadOnly = False
        GridView1.Columns("CIS").OptionsColumn.ReadOnly = False
        GridView1.Columns("PCNoOriginal").OptionsColumn.ReadOnly = False
        GridView1.Columns("GlobalID").OptionsColumn.ReadOnly = False
        GridView1.Columns("GlobalPass").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
    End Sub
    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            For Each r As Integer In GridView1.GetSelectedRows
                Dim obj As New Main_User
                obj.UserID_K = GridView1.GetRowCellValue(r, "UserID")
                _db.Delete(obj)
            Next
            GridView1.DeleteSelectedRows()
            rowClick = False
        End If
    End Sub
    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        GridControlExportExcel(GridView1)
    End Sub

    Sub ShowTreeListAuthoriz()
        TreeList1.DataSource = _db.FillDataTable("  SELECT FormID AS ID, ParentID, CAST(0 AS BIT) AS Status, TextVietNam
                                                    FROM Main_FormRight
                                                    ORDER BY TextVietNam")
        TreeList1.CollapseAll()
        TreeList1.ExpandToLevel(0)
        TreeList1.StateImageList = ImageCollection1
        TreeList1.Columns("Status").Width = 20
        TreeList1.Columns("TextVietNam").Width = 200
        TreeList1.Columns("Status").OptionsColumn.AllowEdit = True
        TreeList1.Columns("TextVietNam").OptionsColumn.AllowEdit = False
        TreeList1.FindNodeByKeyID("0000000001").Expanded = False
    End Sub

    Private Sub TreeList1_GetStateImage(sender As Object, e As DevExpress.XtraTreeList.GetStateImageEventArgs) Handles TreeList1.GetStateImage
        If e.Node.ParentNode Is Nothing Then
            e.NodeImageIndex = 0
        ElseIf e.Node.HasChildren Then
            e.NodeImageIndex = 1
        Else
            e.NodeImageIndex = 2
        End If
    End Sub

    Private Sub TreeList1_CellValueChanged(sender As Object, e As DevExpress.XtraTreeList.CellValueChangedEventArgs) Handles TreeList1.CellValueChanging
        If TreeList1.OptionsBehavior.Editable = False Or e.Column.ReadOnly = True Then
            Return
        End If
        If Not rowClick Then
            ShowWarning("No user selected !")
            Return
        End If

        For Each m As Integer In GridView1.GetSelectedRows
            Dim userID = GridView1.GetRowCellValue(m, "UserID")
            If TreeList1.FocusedNode.HasChildren Then
                Dim dtChild = dataChild(TreeList1.FocusedNode.GetValue("ID"))
                If e.Value = True Then
                    'Loop1
                    For Each r As DataRow In dtChild.Rows
                        ThemPhanQuyen(r("FormID"), userID)
                        'Loop2
                        Dim dtChild2 = dataChild(r("FormID"))
                        For Each r2 As DataRow In dtChild2.Rows
                            ThemPhanQuyen(r2("FormID"), userID)
                            'Loop3
                            Dim dtChild3 = dataChild(r2("FormID"))
                            For Each r3 As DataRow In dtChild3.Rows
                                ThemPhanQuyen(r3("FormID"), userID)
                                'Loop4
                                Dim dtChild4 = dataChild(r3("FormID"))
                                For Each r4 As DataRow In dtChild4.Rows
                                    ThemPhanQuyen(r4("FormID"), userID)
                                    'Loop5
                                    Dim dtChild5 = dataChild(r4("FormID"))
                                    For Each r5 As DataRow In dtChild5.Rows
                                        ThemPhanQuyen(r5("FormID"), userID)
                                        'Loop6
                                        '..............
                                    Next
                                Next
                            Next
                        Next
                    Next
                Else
                    'Loop1
                    For Each r As DataRow In dtChild.Rows
                        XoaPhanQuyen(r("FormID"), userID)
                        'Loop2
                        Dim dtChild2 = dataChild(r("FormID"))
                        For Each r2 As DataRow In dtChild2.Rows
                            XoaPhanQuyen(r2("FormID"), userID)
                            'Loop3
                            Dim dtChild3 = dataChild(r2("FormID"))
                            For Each r3 As DataRow In dtChild3.Rows
                                XoaPhanQuyen(r3("FormID"), userID)
                                'Loop4
                                Dim dtChild4 = dataChild(r3("FormID"))
                                For Each r4 As DataRow In dtChild4.Rows
                                    XoaPhanQuyen(r4("FormID"), userID)
                                    'Loop5
                                    Dim dtChild5 = dataChild(r4("FormID"))
                                    For Each r5 As DataRow In dtChild5.Rows
                                        XoaPhanQuyen(r5("FormID"), userID)
                                        'Loop6
                                        '..............
                                    Next
                                Next
                            Next
                        Next
                    Next
                End If
            End If

            If TreeList1.FocusedNode.ParentNode IsNot Nothing Then
                If e.Value = True Then
                    'Loop1
                    Dim id1 = ThemQuyenParent(TreeList1.FocusedNode.GetValue("ID"), userID)
                    If TreeList1.FindNodeByKeyID(id1).ParentNode IsNot Nothing Then
                        'Loop2
                        Dim id2 = ThemQuyenParent(id1, userID)
                        If TreeList1.FindNodeByKeyID(id2).ParentNode IsNot Nothing Then
                            'Loop3
                            Dim id3 = ThemQuyenParent(id2, userID)
                            If TreeList1.FindNodeByKeyID(id3).ParentNode IsNot Nothing Then
                                'Loop4
                                Dim id4 = ThemQuyenParent(id3, userID)
                                If TreeList1.FindNodeByKeyID(id4).ParentNode IsNot Nothing Then
                                    'Loop5
                                    Dim id5 = ThemQuyenParent(id4, userID)
                                    If TreeList1.FindNodeByKeyID(id5).ParentNode IsNot Nothing Then
                                        'Loop6
                                        '...............
                                    End If
                                End If
                            End If
                        End If
                    End If
                Else
                    'Loop1
                    Dim id1 = XoaQuyenParent(TreeList1.FocusedNode.GetValue("ID"), 1, userID)
                    If TreeList1.FindNodeByKeyID(id1).ParentNode IsNot Nothing Then
                        'Loop2
                        Dim id2 = XoaQuyenParent(id1, 2, userID)
                        If TreeList1.FindNodeByKeyID(id2).ParentNode IsNot Nothing Then
                            'Loop3
                            Dim id3 = XoaQuyenParent(id2, 3, userID)
                            If TreeList1.FindNodeByKeyID(id3).ParentNode IsNot Nothing Then
                                'Loop4
                                Dim id4 = XoaQuyenParent(id3, 4, userID)
                                If TreeList1.FindNodeByKeyID(id4).ParentNode IsNot Nothing Then
                                    'Loop5
                                    Dim id5 = XoaQuyenParent(id4, 5, userID)
                                    If TreeList1.FindNodeByKeyID(id5).ParentNode IsNot Nothing Then
                                        'Loop6
                                        '...............
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            Dim obj As New Main_UserRight
            obj.UserID_K = GridView1.GetRowCellValue(m, "UserID")
            obj.FormID_K = TreeList1.FocusedNode.GetValue("ID")
            If e.Value = True Then
                If Not _db.ExistObject(obj) Then
                    obj.CreateUser = CurrentUser.UserID
                    obj.CreateDate = DateTime.Now
                    obj.CreateMachine = My.Computer.Name
                    _db.Insert(obj)
                End If
            Else
                _db.Delete(obj)
            End If
        Next
        TreeList1.SetFocusedRowCellValue("Status", e.Value)
    End Sub
    Function dataChild(parentID) As DataTable
        Return _db.FillDataTable(String.Format("SELECT FormID
                                                FROM Main_FormRight
                                                WHERE ParentID = '{0}'", parentID))
    End Function
    Sub ThemPhanQuyen(id, userID)
        Dim obj As New Main_UserRight
        obj.UserID_K = userID
        obj.FormID_K = id
        If Not _db.ExistObject(obj) Then
            obj.CreateUser = CurrentUser.UserID
            obj.CreateDate = DateTime.Now
            obj.CreateMachine = My.Computer.Name
            _db.Insert(obj)
        End If
        TreeList1.SetRowCellValue(TreeList1.FindNodeByKeyID(id), TreeList1.Columns("Status"), True)
    End Sub
    Sub XoaPhanQuyen(id, userID)
        Dim obj As New Main_UserRight
        obj.UserID_K = userID
        obj.FormID_K = id
        _db.Delete(obj)
        TreeList1.SetRowCellValue(TreeList1.FindNodeByKeyID(id), TreeList1.Columns("Status"), False)
    End Sub
    Function parentVal(id) As Object
        Return _db.ExecuteScalar(String.Format("SELECT ParentID 
                                                FROM Main_FormRight
                                                WHERE FormID = '{0}'",
                                                id))
    End Function
    Function ThemQuyenParent(id, userID)
        Dim obj As New Main_UserRight
        obj.UserID_K = userID
        obj.FormID_K = parentVal(id)
        If Not _db.ExistObject(obj) Then
            obj.CreateUser = CurrentUser.UserID
            obj.CreateDate = DateTime.Now
            obj.CreateMachine = My.Computer.Name
            _db.Insert(obj)
        End If
        TreeList1.SetRowCellValue(TreeList1.FindNodeByKeyID(obj.FormID_K), TreeList1.Columns("Status"), True)
        Return obj.FormID_K
    End Function
    Function XoaQuyenParent(childID, level, userID)
        If CheckNonOtherChild(childID, userID, level) Then
            Dim obj As New Main_UserRight
            obj.UserID_K = userID
            obj.FormID_K = parentVal(childID)
            _db.Delete(obj)
        End If
        Return parentVal(childID)
    End Function
    Function CheckNonOtherChild(childID, userID, level) As Boolean
        Dim para(0) As SqlClient.SqlParameter
        If level > 1 Then
            para(0) = New SqlClient.SqlParameter("@FormID", DBNull.Value)
        Else
            para(0) = New SqlClient.SqlParameter("@FormID", childID)
        End If
        Dim dtChild = _db.FillDataTable(String.Format("SELECT FormID
                                                        FROM Main_FormRight
                                                        WHERE ParentID = '{0}'
                                                        AND (@FormID IS NULL OR FormID <> @FormID)",
                                                        parentVal(childID)),
                                                        para)
        For Each r As DataRow In dtChild.Rows
            Dim obj As New Main_UserRight
            obj.UserID_K = userID
            obj.FormID_K = r("FormID")
            If _db.ExistObject(obj) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub GridView1_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView1.RowClick
        rowClick = True

        'Dim dtAuth = _db.FillDataTable(
        '    String.Format("SELECT h.FormID, 
        '                        IIF(d.FormID IS NULL, CAST(0 AS BIT), CAST(1 AS BIT)) AS Status
        '                    FROM Main_FormRight AS h
        '                    LEFT JOIN (
        '                     SELECT FormID
        '                     FROM Main_UserRight
        '                     WHERE UserID = '{0}'
        '                    ) AS d
        '                    ON h.FormID = d.FormID",
        '                    GridView1.GetFocusedRowCellValue("UserID")))
        'For Each r As DataRow In dtAuth.Rows
        '    TreeList1.SetRowCellValue(TreeList1.FindNodeByKeyID(r("FormID")), TreeList1.Columns("Status"), r("Status"))
        'Next

        Dim dtAuth2 = _db.FillDataTable(
            String.Format("SELECT h.FormID AS ID, h.ParentID,
                                IIF(d.FormID IS NULL, CAST(0 AS BIT), CAST(1 AS BIT)) AS Status, h.TextVietNam
                            FROM Main_FormRight AS h
                            LEFT JOIN (
                                 SELECT FormID
                                 FROM Main_UserRight
                                 WHERE UserID = '{0}'
                            ) AS d
                            ON h.FormID = d.FormID
                            ORDER BY h.TextVietNam",
                            GridView1.GetFocusedRowCellValue("UserID")))
        TreeList1.DataSource = dtAuth2
        TreeList1.CollapseAll()
        TreeList1.ExpandToLevel(0)

        TreeList1.FindNodeByKeyID("0000000001").Expanded = False
    End Sub

    Private Sub TablePanel1_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles TablePanel1.Paint
        Dim panel As TablePanel = CType(sender, TablePanel)
        Dim viewInfo As TablePanelObjectInfoArgs = panel.GetViewInfo()
        Using cache As GraphicsCache = New GraphicsCache(e.Graphics)
            Dim gridPen As Pen = cache.GetPen(Color.Gray, 1)
            cache.DrawRectangle(gridPen, viewInfo.DisplayRect)
            DrawHorzGridLines(cache, viewInfo.Layout.Grid, gridPen)
            DrawVertGridLines(cache, viewInfo.Layout.Grid, gridPen)
        End Using
    End Sub
    Private Sub DrawHorzGridLines(ByVal cache As GraphicsCache, ByVal grid As TablePanelGrid, ByVal gridPen As Pen)
        Dim horzLines As TablePanelHorzGridLine() = grid.HorzLines
        For n As Integer = 0 To horzLines.Length - 1
            cache.DrawLine(gridPen, horzLines(n).Start, horzLines(n).[End])
        Next
    End Sub
    Private Sub DrawVertGridLines(ByVal cache As GraphicsCache, ByVal grid As TablePanelGrid, ByVal gridPen As Pen)
        Dim vertLines As TablePanelVertGridLine() = grid.VertLines
        For n As Integer = 0 To vertLines.Length - 1
            cache.DrawLine(gridPen, vertLines(n).Start, vertLines(n).[End])
        Next
    End Sub

    Private Sub btnSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnSave.ItemClick
        If txtUserID.Text.Trim = "" Or txtUserName.Text.Trim = "" Or txtPassword.Text.Trim = "" Then
            ShowWarning("Fields is required !")
            Return
        End If
        Dim obj As New Main_User
        obj.UserID_K = txtUserID.Text
        If _db.ExistObject(obj) Then
            ShowWarning("User ID already exist !")
            Return
        End If
        Dim existUserName = _db.ExecuteScalar(String.Format("SELECT TOP 1 UserName
                                                            FROM Main_User
                                                            WHERE UserName LIKE '%{0}%'",
                                                            txtUserName.Text))
        If existUserName IsNot Nothing Then
            ShowWarning("User Name already exist !")
        Else
            obj.UserName = txtUserName.Text
            obj.Password = EncryptPassword(txtPassword.Text)
            obj.CreateUser = CurrentUser.UserID
            obj.CreateDate = Date.Now
            obj.CreateMachine = My.Computer.Name
            _db.Insert(obj)
            btnShow.PerformClick()
            ShowSuccess()
        End If
    End Sub

    Private Sub btnCopyRight_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnCopyRight.ItemClick
        If txtUserID.Text.Trim <> "" And rowClick Then
            Dim obj As New Main_User
            obj.UserID_K = txtUserID.Text.Trim
            If _db.ExistObject(obj) Then
                _db.ExecuteNonQuery(String.Format("
                    DELETE Main_UserRight
                    WHERE UserID = '{0}'
                    INSERT INTO Main_UserRight (UserID, FormID, IsEdit, IsAdmin, CreateUser, CreateDate, CreateMachine)
                    SELECT '{0}' AS UserID, FormID, IsEdit, IsAdmin, '{1}', GETDATE(), '{2}'
                    FROM Main_UserRight
                    WHERE UserID = '{3}'
                    
                    DELETE Main_UserRightDetail
                    WHERE UserID = '{0}'
                    INSERT INTO Main_UserRightDetail (UserID, FormID, ControlName, CreateUser, CreateDate, CreateMachine)
                    SELECT '{0}' AS UserID, FormID, ControlName, '{1}', GETDATE(), '{2}'
                    FROM Main_UserRightDetail
                    WHERE UserID = '{3}'",
                    txtUserID.Text,
                    CurrentUser.UserID,
                    My.Computer.Name,
                    GridView1.GetFocusedRowCellValue("UserID")))
                ShowSuccess()
            Else
                ShowWarning("Invalid user copied !")
            End If
        End If
    End Sub

    Private Sub btnAddRight_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnAddRight.ItemClick
        If txtUserID.Text.Trim <> "" And rowClick Then
            Dim obj As New Main_User
            obj.UserID_K = txtUserID.Text.Trim
            If _db.ExistObject(obj) Then
                _db.ExecuteNonQuery(
                    String.Format("
                    INSERT INTO Main_UserRight (UserID, FormID, IsEdit, IsAdmin, CreateUser, CreateDate, CreateMachine)
                    SELECT '{0}' AS UserID, FormID, IsEdit, IsAdmin, '{1}', GETDATE(), '{2}'
                    FROM Main_UserRight
                    WHERE UserID = '{3}'
                    AND FormID NOT IN (
	                    SELECT FormID
	                    FROM Main_UserRight
	                    WHERE UserID = '{0}'
                    )
                    
                    INSERT INTO Main_UserRightDetail (UserID, FormID, ControlName, CreateUser, CreateDate, CreateMachine)
                    SELECT '{0}' AS UserID, h.FormID, h.ControlName, '{1}', GETDATE(), '{2}'
                    FROM Main_UserRightDetail AS h
                    LEFT JOIN (
                        SELECT FormID, ControlName
                        FROM Main_UserRightDetail
                        WHERE UserID = '{0}'
                    ) AS d
                    ON h.FormID = d.FormID
                    AND h.ControlName = d.ControlName
                    WHERE h.UserID = '{3}'
                    AND d.FormID IS NULL",
                    txtUserID.Text,
                    CurrentUser.UserID,
                    My.Computer.Name,
                    GridView1.GetFocusedRowCellValue("UserID")))
                ShowSuccess()
            Else
                ShowWarning("Invalid user copied !")
            End If
        End If
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format("UPDATE Main_User
                                                SET {0} = @Value
                                                WHERE UserID = '{1}'",
                                                e.Column.FieldName,
                                                GridView1.GetFocusedRowCellValue("UserID")),
                                                para)
            If e.Column.FieldName = "Email" Then
                Dim obj As New OT_Mail
                obj.EmpID_K = GridView1.GetFocusedRowCellValue("UserID")
                If GridView1.GetFocusedRowCellValue("Email") IsNot DBNull.Value Then
                    obj.Mail = GridView1.GetFocusedRowCellValue("Email")
                Else
                    obj.Mail = ""
                End If
                If _db.ExistObject(obj) Then
                    _db.Update(obj)
                Else
                    _db.Insert(obj)
                End If
            End If
        End If
    End Sub

    Private Sub chbPassword_CheckedChanged(sender As Object, e As EventArgs) Handles chbPassword.CheckedChanged
        txtPassword.Properties.PasswordChar = If(txtPassword.Properties.PasswordChar = "*"c, ControlChars.NullChar, "*"c)
    End Sub

    Private Sub TreeList1_DoubleClick(sender As Object, e As EventArgs) Handles TreeList1.DoubleClick
        If TreeList1.FocusedNode.HasChildren = False Then
            If rowClick Then
                Dim frm As New FrmPermissionControl()
                frm.isGroup = False
                frm.ID = TreeList1.FocusedNode.GetValue("ID")
                frm._grid = GridView1
                frm.UserID = GridView1.GetFocusedRowCellValue("UserID")
                frm.ShowDialog()
            Else
                ShowWarning("No user selected !")
            End If
        End If
    End Sub

    Private Sub mnuGroupOfSiteStock_Click(sender As Object, e As EventArgs) Handles mnuGroupOfSiteStock.Click
        If GridView1.SelectedRowsCount = 0 Then Return
        Dim frm As New FrmGroupOfUserSiteStock
        frm._uID = GridView1.GetFocusedRowCellValue("UserID")
        frm.ShowDialog()
    End Sub

    Private Sub mnuGroupOfWT_Click(sender As Object, e As EventArgs) Handles mnuGroupOfWT.Click
        If GridView1.SelectedRowsCount = 0 Then Return
        Dim frm As New FrmGroupOfUser
        frm._uID = GridView1.GetFocusedRowCellValue("UserID")
        frm.ShowDialog()
    End Sub

    Private Sub GroupOfTrainingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupOfTrainingToolStripMenuItem.Click
        If GridView1.SelectedRowsCount = 0 Then Return
        Dim frm As New FrmGroupOfUserTrain
        frm._uID = GridView1.GetFocusedRowCellValue("UserID")
        frm._depts = GridView1.GetFocusedRowCellValue("Section")
        frm.ShowDialog()
    End Sub

    Private Sub mnuSPC_Click(sender As Object, e As EventArgs) Handles mnuSPC.Click
        If GridView1.SelectedRowsCount = 0 Then Return
        Dim frm As New FrmGroupOfUserSPC
        frm._uID = GridView1.GetFocusedRowCellValue("UserID")
        frm.ShowDialog()
    End Sub

    Private Sub btnKillConnect_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnKillConnect.ItemClick
        _db.ExecuteStoreProcedure("sp_Admin_KillAllConnection")
    End Sub
End Class