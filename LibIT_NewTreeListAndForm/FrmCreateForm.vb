﻿Imports System.Drawing
Imports System.Windows.Forms
Imports CommonDB
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils.Layout
Imports DevExpress.XtraTreeList
Imports PublicUtility
Public Class FrmCreateForm
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim _parentID As String = ""
    Private Sub FrmCreateForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnShow.PerformClick()
    End Sub

    Private Sub btnShow_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShow.ItemClick
        TreeList1.DataSource = _db.FillDataTable(
            "SELECT FormID AS ID, ParentID, TextVietNam, TextEnglish, ParentID AS IDParent, 
            AssemblyName, FormName, FormID
            FROM Main_FormRight
            ORDER BY TextVietNam")
        TreeList1.OptionsView.AutoWidth = False
        TreeList1.Columns("TextVietNam").Width = 300
        TreeList1.Columns("TextEnglish").Width = 300
        TreeList1.Columns("AssemblyName").Width = 200
        TreeList1.Columns("FormName").Width = 200
        TreeList1.OptionsBehavior.Editable = False
        TreeList1.OptionsSelection.EnableAppearanceHotTrackedRow = DevExpress.Utils.DefaultBoolean.True
        TreeList1.CollapseAll()
        TreeList1.ExpandToLevel(0)
        TreeList1.Columns("TextVietNam").AppearanceHeader.BackColor = Color.Transparent
        TreeList1.Columns("TextEnglish").AppearanceHeader.BackColor = Color.Transparent
        TreeList1.Columns("AssemblyName").AppearanceHeader.BackColor = Color.Transparent
        TreeList1.Columns("FormName").AppearanceHeader.BackColor = Color.Transparent
        TreeList1.Columns("IDParent").AppearanceHeader.BackColor = Color.Transparent
    End Sub
    Private Sub btnNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnNew.ItemClick
        txtParentText.Text = TreeList1.FocusedNode.GetValue("TextVietNam")
        _parentID = TreeList1.FocusedNode.GetValue("ID")
        txtLibName.Text = ""
        txtFormName.Text = ""
        txtVietnamese.Text = ""
        txtEnglish.Text = ""
        txtLibName.Select()
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        TreeList1.OptionsBehavior.Editable = True
        TreeList1.Columns("TextVietNam").OptionsColumn.AllowEdit = True
        TreeList1.Columns("TextEnglish").OptionsColumn.AllowEdit = True
        TreeList1.Columns("AssemblyName").OptionsColumn.AllowEdit = True
        TreeList1.Columns("FormName").OptionsColumn.AllowEdit = True
        TreeList1.Columns("IDParent").OptionsColumn.AllowEdit = True
        TreeList1.Columns("FormID").OptionsColumn.AllowEdit = False

        TreeList1.Columns("TextVietNam").AppearanceHeader.BackColor = Color.Wheat
        TreeList1.Columns("TextEnglish").AppearanceHeader.BackColor = Color.Wheat
        TreeList1.Columns("AssemblyName").AppearanceHeader.BackColor = Color.Wheat
        TreeList1.Columns("FormName").AppearanceHeader.BackColor = Color.Wheat
        TreeList1.Columns("IDParent").AppearanceHeader.BackColor = Color.Wheat
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        Try
            _db.BeginTransaction()
            If TreeList1.FocusedNode.HasChildren Then
                If ShowQuestion("Module này có rất nhiều con, bạn có chắc chắn muốn xóa module này ?") = Windows.Forms.DialogResult.Yes Then
                    'Loop1
                    Dim dtChild1 = DataChild(TreeList1.FocusedNode.GetValue("ID"))
                    For Each r1 As DataRow In dtChild1.Rows
                        'Loop2
                        Dim dtChild2 = DataChild(r1("FormID"))
                        For Each r2 As DataRow In dtChild2.Rows
                            'Loop3
                            Dim dtChild3 = DataChild(r2("FormID"))
                            For Each r3 As DataRow In dtChild3.Rows
                                'Loop4
                                Dim dtChild4 = DataChild(r3("FormID"))
                                For Each r4 As DataRow In dtChild4.Rows
                                    'Loop5
                                    Dim dtChild5 = DataChild(r4("FormID"))
                                    For Each r5 As DataRow In dtChild5.Rows
                                        'Loop6
                                        Dim dtChild6 = DataChild(r5("FormID"))
                                        For Each r6 As DataRow In dtChild6.Rows
                                            'Loop7
                                            '-----------------
                                            DeleteChild(r6("FormID"))
                                            TreeList1.DeleteNode(TreeList1.FindNodeByKeyID(r6("FormID")))
                                        Next
                                        DeleteChild(r5("FormID"))
                                        TreeList1.DeleteNode(TreeList1.FindNodeByKeyID(r5("FormID")))
                                    Next
                                    DeleteChild(r4("FormID"))
                                    TreeList1.DeleteNode(TreeList1.FindNodeByKeyID(r4("FormID")))
                                Next
                                DeleteChild(r3("FormID"))
                                TreeList1.DeleteNode(TreeList1.FindNodeByKeyID(r3("FormID")))
                            Next
                            DeleteChild(r2("FormID"))
                            TreeList1.DeleteNode(TreeList1.FindNodeByKeyID(r2("FormID")))
                        Next
                        DeleteChild(r1("FormID"))
                        TreeList1.DeleteNode(TreeList1.FindNodeByKeyID(r1("FormID")))
                    Next
                    DeleteChild(TreeList1.FocusedNode.GetValue("ID"))
                    TreeList1.DeleteSelectedNodes()
                End If
                Return
            End If
            If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
                _db.ExecuteNonQuery(String.Format(" DELETE Main_FormRight
                                                WHERE FormID = '{0}'",
                                                TreeList1.FocusedNode.GetValue("ID")))
                TreeList1.DeleteSelectedNodes()
            End If
            _db.Commit()
        Catch ex As Exception
            _db.RollBack()
            ShowWarning(ex.Message)
        End Try
    End Sub
    Function DataChild(parentID) As DataTable
        Return _db.FillDataTable(String.Format("SELECT FormID
                                                FROM Main_FormRight
                                                WHERE ParentID = '{0}'",
                                                parentID))
    End Function
    Sub DeleteChild(formID)
        Dim obj As New Main_FormRight
        obj.FormID_K = formID
        _db.Delete(obj)
    End Sub

    Function GetNewID() As String
        Dim valMaxID As String = _db.ExecuteScalar("SELECT ISNULL(MAX(FormID), 0)
                                                    FROM Main_FormRight
                                                    WHERE LEN(FormID) = 10
                                                    AND ISNUMERIC(FormID) = 1")
        Return (CType(valMaxID, Integer) + 1).ToString.PadLeft(10, "0")
    End Function

    Private Sub TreeList1_GetStateImage(sender As Object, e As DevExpress.XtraTreeList.GetStateImageEventArgs) Handles TreeList1.GetStateImage
        If e.Node.ParentNode Is Nothing Then
            e.NodeImageIndex = 0
        ElseIf e.Node.HasChildren Then
            e.NodeImageIndex = 1
        Else
            e.NodeImageIndex = 2
        End If
    End Sub

    Private Sub btnSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnSave.ItemClick
        If txtVietnamese.Text.Trim = "" Then
            ShowWarning("Fields is required !")
            Return
        End If
        Dim id = GetNewID()
        Dim obj As New Main_FormRight
        obj.FormID_K = id
        obj.ParentID = _parentID
        obj.AssemblyName = txtLibName.Text
        obj.FormName = txtFormName.Text
        obj.TextVietNam = txtVietnamese.Text
        obj.TextEnglish = txtEnglish.Text
        obj.CreateUser = CurrentUser.UserID
        obj.CreateDate = DateTime.Now
        _db.Insert(obj)
        btnShow.PerformClick()
    End Sub

    Private Sub TreeList1_CellValueChanged(sender As Object, e As DevExpress.XtraTreeList.CellValueChangedEventArgs) Handles TreeList1.CellValueChanged
        If TreeList1.OptionsBehavior.Editable = True And e.Column.ReadOnly = False Then
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            If TreeList1.FocusedColumn.FieldName = "IDParent" Then
                _db.ExecuteNonQuery(
                    String.Format("UPDATE Main_FormRight
                                    SET ParentID = @Value
                                    WHERE FormID = '{0}'",
                                    TreeList1.FocusedNode.GetValue("ID")),
                                    para)
                Return
            End If
            _db.ExecuteNonQuery(String.Format("UPDATE Main_FormRight
                                                SET {0} = @Value
                                                WHERE FormID = '{1}'",
                                                e.Column.FieldName,
                                                TreeList1.FocusedNode.GetValue("ID")),
                                                para)
        End If
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

    Private Sub TreeList1_KeyDown(sender As Object, e As Windows.Forms.KeyEventArgs) Handles TreeList1.KeyDown
        If e.Control And e.KeyCode = Keys.C Then
            Dim treeList As TreeList = CType(sender, TreeList)
            Clipboard.SetText(treeList.FocusedNode.GetDisplayText(treeList.FocusedColumn))
            e.Handled = True
        End If
    End Sub

    Private Sub btnExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExport.ItemClick
        Dim path As String = Application.StartupPath & "\Export"
        Dim myName As String = Guid.NewGuid.ToString
        Dim myFile As String = String.Format("{0}\{1}.xlsx", path, myName)
        TreeList1.ExportToXlsx(myFile)
        Process.Start(myFile)
        'GridControlExportExcel()
    End Sub

    'Sub ShowChildByParentID()
    '    Dim dtAll As New DataTable
    '    'Loop1
    '    Dim dtChild1 = DataChild(glueModule.EditValue)
    '    dtAll.Merge(dtChild1)
    '    For Each r1 As DataRow In dtChild1.Rows
    '        'Loop2
    '        Dim dtChild2 = DataChild(r1("FormID"))
    '        dtAll.Merge(dtChild2)
    '        For Each r2 As DataRow In dtChild2.Rows
    '            'Loop3
    '            Dim dtChild3 = DataChild(r2("FormID"))
    '            dtAll.Merge(dtChild3)
    '            For Each r3 As DataRow In dtChild3.Rows
    '                'Loop4
    '                Dim dtChild4 = DataChild(r3("FormID"))
    '                dtAll.Merge(dtChild4)
    '                For Each r5 As DataRow In dtChild4.Rows
    '                    'Loop5
    '                    Dim dtChild5 = DataChild(r5("FormID"))
    '                    dtAll.Merge(dtChild5)
    '                    'Loop6
    '                    '-----------------
    '                Next
    '            Next
    '        Next
    '    Next

    '    Dim dtAllView = dtAll.DefaultView
    '    dtAllView.Sort = "Order1, Order2, Order3, Order4, Order5, Order6"

    '    GridControl1.DataSource = dtAllView.ToTable
    '    GridControlSetFormat(GridView1, 3)
    '    GridView1.Columns("AssemblyName").Width = 125
    '    GridView1.Columns("FormName").Width = 125
    '    GridView1.Columns("TextVietNam").Width = 150
    '    GridView1.Columns("TextEnglish").Width = 150
    '    GridView1.Columns("TextJapan").Width = 150
    '    GridView1.Columns("TextChina").Width = 150
    'End Sub
    'Function DataChild(parentID) As DataTable
    '    Return _db.FillDataTable(String.Format("SELECT FormID, ParentID, AssemblyName, FormName, TextVietNam, 
    '                                                TextEnglish, TextJapan, TextChina,
    '                                                Order1, Order2, Order3, Order4, Order5, Order6
    '                                            FROM Main_FormRight
    '                                            WHERE ParentID = '{0}'",
    '                                            parentID))
    'End Function
End Class