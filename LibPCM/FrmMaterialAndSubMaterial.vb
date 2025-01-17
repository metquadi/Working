﻿Imports CommonDB
Imports PublicUtility
Imports LibEntity
Imports System.Windows.Forms
Imports vb = Microsoft.VisualBasic
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmMaterialAndSubMaterial : Inherits DevExpress.XtraEditors.XtraForm
    Dim _dbFpics As New DBSql(PublicConst.EnumServers.NDV_SQL_Fpics)
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim param(1) As SqlClient.SqlParameter
    Dim _cellClick As Integer = 0
    Public _isOption As Boolean = False
    Public _gridRow As GridView = Nothing
    Public _myID As String = ""
    Public _mySection As String = ""


    Private Sub FrmCreateDepartment_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Enter Then
            SendKeys.Send("{Tab}")
        End If

        If e.KeyCode = Keys.S And e.Control And mnuSave.Enabled Then
            mnuSave.PerformClick()
        End If
    End Sub

#Region "Form event"

    Private Sub txtECode_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtECode.Enter
        SetColorEnter(txtECode)
    End Sub
    Private Sub txtECode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtECode.Leave
        SetColorLeave(txtECode)
    End Sub

    Private Sub cboSubPrc_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubPrc.Enter
        SetColorEnter(cboSubPrc)
    End Sub
    Private Sub cboSubPrc_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubPrc.Leave
        SetColorLeave(cboSubPrc)
    End Sub

    Private Sub txtJCode_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJCode.Enter
        SetColorEnter(txtJCode)
    End Sub
    Private Sub txtJCode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJCode.Leave
        SetColorLeave(txtJCode)
    End Sub

    Private Sub txtJEName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJEName.Enter
        SetColorEnter(txtJEName)
    End Sub
    Private Sub txtJEName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJEName.Leave
        SetColorLeave(txtJEName)
    End Sub

    Private Sub txtJVName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJVName.Enter
        SetColorEnter(txtJVName)
    End Sub
    Private Sub txtJVName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJVName.Leave
        SetColorLeave(txtJVName)
    End Sub

    Private Sub txtUnit_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUnit.Enter
        SetColorEnter(txtUnit)
    End Sub
    Private Sub txtUnit_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUnit.Leave
        SetColorLeave(txtUnit)
    End Sub

    Private Sub txtMinQty_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinQty.Enter
        SetColorEnter(txtMinQty)
    End Sub
    Private Sub txtMinQty_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinQty.Leave
        SetColorLeave(txtMinQty)
    End Sub

    Private Sub txtStdDtbtQty_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStdDtbtQty.Enter
        SetColorEnter(txtStdDtbtQty)
    End Sub
    Private Sub txtStdDtbtQty_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStdDtbtQty.Leave
        SetColorLeave(txtStdDtbtQty)
    End Sub

    Private Sub txtAddDtbtQty_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddDtbtQty.Enter
        SetColorEnter(txtAddDtbtQty)
    End Sub
    Private Sub txtAddDtbtQty_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddDtbtQty.Leave
        SetColorLeave(txtAddDtbtQty)
    End Sub
#End Region

    Dim dtAll As DataTable
    Private Sub mnuShowAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowAll.Click
        Dim condition As String = ""

        Dim sql As String = String.Format(" select GroupCode, ECode, JCode, PrcName, SubPrcName, JEName, JVName, " +
                                          " Unit, MinQty, StdDtbtQty, AddDtbtQty, NormWeek,MonthStd as NormMonth " +
                                          " from {0} order by Old, ECode",
                                          PublicTable.Table_PCM_MterNotJCode)
        If _isOption Then
            condition = String.Format(" where ECode='{0}'", _myID)
            sql = String.Format(" select GroupCode, ECode, JCode, PrcName, SubPrcName, JEName, JVName, " +
                                          " Unit, MinQty, StdDtbtQty, AddDtbtQty, NormWeek,MonthStd as NormMonth " +
                                          " from {0} {1} order by Old, ECode",
                                          PublicTable.Table_PCM_MterNotJCode,
                                          condition)
            dtAll = _db.FillDataTable(sql)
            If dtAll.Rows.Count = 0 Then
                condition = String.Format(" where ECode in (select ECode from [PCM_Dept] where [DeptName]='{0}') ", _mySection)
                sql = String.Format(" select GroupCode, ECode, JCode, PrcName, SubPrcName, JEName, JVName, " +
                                              " Unit, MinQty, StdDtbtQty, AddDtbtQty, NormWeek,MonthStd as NormMonth " +
                                              " from {0} {1} order by Old, ECode",
                                              PublicTable.Table_PCM_MterNotJCode,
                                              condition)
                dtAll = _db.FillDataTable(sql)
            End If
        Else
            dtAll = _db.FillDataTable(sql)
        End If

        _db.ExecuteStoreProcedure("sp_LOS_UpdateJCodeChemical")
        GridControl1.DataSource = dtAll
        GridControlSetFormat(GridView1)

        mnuNew.PerformClick()
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        'Try 

        Dim obj As New PCM_MterNotJCode()
        If Trim(txtECode.Text) = "" Then
            ShowWarning("Bạn chưa nhập Emp Code")
            Exit Sub
        Else
            obj.ECode_K = Trim(txtECode.Text)
        End If

        If IsDBNull(cboSubPrc.SelectedValue) Then
            ShowWarning("Bạn chưa nhập Process")
            Exit Sub
        Else
            obj.SubPrcName_K = cboSubPrc.SelectedValue
        End If

        If Trim(txtJCode.Text) = "" Then
            ShowWarning("Bạn chưa nhập JCode")
            Exit Sub
        Else
            obj.JCode_K = UCase(Trim(txtJCode.Text))
        End If

        obj.JEName = Trim(txtJEName.Text)
        obj.JVName = Trim(txtJVName.Text)
        obj.Unit = Trim(txtUnit.Text)
        If IsNumeric(Trim(txtMinQty.Text)) Then
            obj.MinQty = Trim(txtMinQty.Text)
        End If

        If IsNumeric(Trim(txtStdDtbtQty.Text)) Then
            obj.StdDtbtQty = Trim(txtStdDtbtQty.Text)
        End If

        If IsNumeric(Trim(txtAddDtbtQty.Text)) Then
            obj.AddDtbtQty = Trim(txtAddDtbtQty.Text)
        End If

        If IsNumeric(Trim(txtWeekStd.Text)) Then
            obj.NormWeek = Trim(txtWeekStd.Text)
        End If
        If IsNumeric(Trim(txtMonthStd.Text)) Then
            obj.MonthStd = Trim(txtMonthStd.Text)
        End If

        Dim sqlPrcName As String = String.Format("select DeptCode, PrcName from {0} where ECode = '{1}' and SubPrcName = '{2}'",
                                                 PublicTable.Table_PCM_Dept, obj.ECode_K, obj.SubPrcName_K)
        Dim dt As DataTable = _db.FillDataTable(sqlPrcName)
        If dt.Rows.Count <> 0 Then
            obj.PrcName = dt.Rows(0).Item("PrcName")
            obj.GroupCode = vb.Left(dt.Rows(0).Item("DeptCode"), 2) + "1"
        End If

        If _db.ExistObject(obj) Then
            If ShowQuestionUpdate() = DialogResult.No Then
                Return
            End If

            obj.UpdateDate = DateTime.Now
            obj.UpdateUser = CurrentUser.UserID
            _db.Update(obj)



            ShowSuccess()
        Else
            If ShowQuestionSave() = DialogResult.No Then
                Return
            End If

            obj.ECode_K = Trim(txtECode.Text)
            obj.SubPrcName_K = Trim(cboSubPrc.Text)
            obj.JCode_K = UCase(Trim(txtJCode.Text))
            obj.CreateDate = DateTime.Now
            obj.CreateUser = CurrentUser.UserID
            _db.Insert(obj)



            ShowSuccess()
        End If
        mnuShowAll.PerformClick()
        mnuNew.PerformClick()
        txtECode.Focus()
        'Catch ex As Exception
        '    '_db.RollBack()
        '    ShowError(ex, "mnuSave_Click", Name)
        'End Try
    End Sub

    Private Sub mnuExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExport.Click
        GridControlExportExcel(GridView1)
    End Sub

    Private Sub mnuNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNew.Click
        txtECode.Text = ""
        cboSubPrc.DataSource = Nothing
        txtJCode.Text = ""
        txtJEName.Text = ""
        txtJVName.Text = ""
        txtUnit.Text = ""
        txtMinQty.Text = ""
        txtStdDtbtQty.Text = ""
        txtAddDtbtQty.Text = ""
        txtWeekStd.Text = ""
        txtECode.Focus()
        _cellClick = 0
        mnuEdit.Enabled = True
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        If GridView1.RowCount = 0 Then Exit Sub

        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            For Each r As Integer In GridView1.GetSelectedRows
                Dim _obj As New PCM_MterNotJCode
                _obj.ECode_K = GridView1.GetRowCellValue(r, "ECode")
                _obj.SubPrcName_K = GridView1.GetRowCellValue(r, "SubPrcName")
                _obj.JCode_K = GridView1.GetRowCellValue(r, "JCode")
                _db.Delete(_obj)
            Next
            mnuShowAll.PerformClick()
            mnuNew.PerformClick()
        End If
    End Sub

    Private Sub FrmMaterialAndSubMaterial_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown


        mnuShowAll.PerformClick()

        If _isOption Then
            bttOK.Visible = True
            mnuDelete.Enabled = False
            mnuEdit.Enabled = False
            mnuImport.Enabled = False
            mnuNew.Enabled = False
        Else
            bttOK.Visible = False
        End If
    End Sub

    Private Sub txtECode_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtECode.Validating
        Dim ECode As String = Trim(txtECode.Text)
        Dim sql As String = String.Format("select distinct SubPrcName from {0} where ECode = '{1}' and Old = 'False'", PublicTable.Table_PCM_Dept, ECode)
        Dim dt As DataTable = _db.FillDataTable(sql)
        cboSubPrc.DataSource = dt
        cboSubPrc.DisplayMember = "SubPrcName"
        cboSubPrc.ValueMember = "SubPrcName"
    End Sub

    Private Sub txtJCode_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtJCode.Validating
        Dim JCode As String = Trim(txtJCode.Text)
        Dim sql As String = String.Format("select JEName, JVName, Unit, MinQty, StdDtbtQty, AddDtbtQty from {0} where JCode = '{1}'", PublicTable.Table_PCM_MterNotJCode, JCode)
        Dim dt As DataTable = _db.FillDataTable(sql)
        If dt.Rows.Count <> 0 Then
            txtJEName.Text = IIf(dt.Rows(0).Item("JEName") Is DBNull.Value, "", dt.Rows(0).Item("JEName"))
            txtJVName.Text = IIf(dt.Rows(0).Item("JVName") Is DBNull.Value, "", dt.Rows(0).Item("JVName"))
            txtUnit.Text = IIf(dt.Rows(0).Item("Unit") Is DBNull.Value, "", dt.Rows(0).Item("Unit"))
            txtMinQty.Text = IIf(dt.Rows(0).Item("MinQty") Is DBNull.Value, "", dt.Rows(0).Item("MinQty"))
            txtStdDtbtQty.Text = IIf(dt.Rows(0).Item("StdDtbtQty") Is DBNull.Value, "", dt.Rows(0).Item("StdDtbtQty"))
            txtAddDtbtQty.Text = IIf(dt.Rows(0).Item("AddDtbtQty") Is DBNull.Value, "", dt.Rows(0).Item("AddDtbtQty"))
        End If
    End Sub

    'Private Sub mnuUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUpdate.Click
    '    If _cellClick <> 1 Then Exit Sub
    '    Try
    '        Dim _ECodeOld As String
    '        Dim _SubPrcNameOld As String
    '        _ECodeOld = GridView1.CurrentRow.Cells("ECode").Value
    '        _SubPrcNameOld = GridView1.CurrentRow.Cells("SubPrcName").Value
    '        Dim _PrcName As String
    '        Dim sqlPrcName As String = String.Format("select PrcName from {0} where ECode = '{1}' and SubPrcName = '{2}'", PublicTable.Table_PCM_Dept, Trim(txtECode.Text), cboSubPrc.SelectedValue)
    '        Dim dt As DataTable = _db.FillDataTable(sqlPrcName)
    '        If dt.Rows.Count <> 0 Then
    '            _PrcName = dt.Rows(0).Item("PrcName")
    '        Else
    '            MessageBox.Show("ECode and SubPrcName do not exist", "ECode, SubPrcName")
    '            Exit Sub
    '        End If

    '        UpdateAll(_ECodeOld, _SubPrcNameOld, _PrcName, Trim(txtECode.Text), cboSubPrc.SelectedValue)

    '        MsgBox("Save successfully.", MsgBoxStyle.OkOnly, "Save")
    '        mnuNew.PerformClick()
    '        txtECode.Focus()
    '        mnuShowAll.PerformClick()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "insert/update")
    '    End Try
    'End Sub

    Sub UpdateAll(ByVal _ECodeOld As String, ByVal _SubPrcNameOld As String, ByVal _PrcName As String, ByVal _ECodeNew As String, ByVal _SubPrcNameNew As String)
        Dim sqlMterNotJCode As String = String.Format("Select ECode, SubPrcName, JCode 
                    from {0} where ECode = '{1}' 
                    and SubPrcName = '{2}'",
                    PublicTable.Table_PCM_MterNotJCode,
                    _ECodeOld,
                    _SubPrcNameOld)
        Dim dtMterNotJCode As DataTable = _db.FillDataTable(sqlMterNotJCode)
        If dtMterNotJCode.Rows.Count = 0 Then Exit Sub
        For i As Integer = 0 To dtMterNotJCode.Rows.Count - 1
            Try
                _db.BeginTransaction()
                Dim obj As New PCM_MterNotJCode()
                obj.ECode_K = dtMterNotJCode.Rows(i).Item("ECode")
                obj.SubPrcName_K = dtMterNotJCode.Rows(i).Item("SubPrcName")
                obj.JCode_K = dtMterNotJCode.Rows(i).Item("JCode")
                _db.GetObject(obj)
                _db.Delete(obj)
                obj.SubPrcName_K = _SubPrcNameNew
                obj.ECode_K = _ECodeNew
                obj.Old = False
                obj.PrcName = _PrcName
                If _db.ExistObject(obj) Then
                    _db.Update(obj)
                Else
                    _db.Insert(obj)
                End If
                _db.Commit()
            Catch ex As Exception
                _db.RollBack()
                MessageBox.Show(ex.Message, "Update MterTrayU00")
            End Try
        Next
    End Sub

    Private Sub txtJCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJCode.TextChanged

        Dim sqlItem As String = String.Format(" SELECT [ItemCode],[ItemName] " +
                                              " FROM [t_ASMaterialItem] where ItemCode='{0}' ",
                                              txtJCode.Text)
        Dim dtItem As DataTable = _dbFpics.FillDataTable(sqlItem)
        If dtItem.Rows.Count > 0 Then
            txtJEName.Text = IIf(dtItem.Rows(0).Item("ItemName") Is DBNull.Value, "",
                                 dtItem.Rows(0).Item("ItemName"))
            txtJVName.Text = txtJEName.Text
        End If
    End Sub

    Private Sub mnuEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        _cellClick = 1
        mnuEdit.Enabled = False
    End Sub

    Private Sub mnuUpdateEmpID_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateEmpID.Click
        If GridView1.RowCount > 0 And _cellClick = 1 Then
            If ShowQuestionUpdate("Bạn muốn cập nhật ECode không ?") = Windows.Forms.DialogResult.Yes Then
                Dim sql As String = String.Format(" update  [PCM_MterNotJCode] " +
                                                  " set ECode='{0}' " +
                                                  " where ECode='{1}' and JCode='{2}' and SubPrcName='{3}' ",
                                                  txtNewID.Text, txtOldID.Text,
                                                  txtJCode.Text, cboSubPrc.Text)
                _db.ExecuteNonQuery(sql)
                mnuShowAll.PerformClick()
                ShowSuccess()
            End If
        End If
    End Sub

    Private Sub mnuImport_Click(sender As System.Object, e As System.EventArgs) Handles mnuImport.Click
        Dim dtData As DataTable = ImportEXCEL(True)

        If dtData.Rows.Count > 0 Then
            Try
                _db.BeginTransaction()

                If ShowQuestion("Bạn có muốn xóa toàn bộ master cũ để import master mới không ?" & vbCrLf &
                               " Chú ý: Xóa rồi không thể phục hồi, nên lưu file excel trước") = Windows.Forms.DialogResult.Yes Then
                    Dim sql As String = String.Format("delete from PCM_MterNotJCode")
                    _db.ExecuteNonQuery(sql)
                End If
                For Each r As DataRow In dtData.Rows
                    Dim obj As New PCM_MterNotJCode
                    obj.ECode_K = r("ECode")
                    If r("Group Code") IsNot DBNull.Value Then
                        obj.GroupCode = r("Group Code")
                    End If
                    obj.JCode_K = r("JCode")
                    obj.PrcName = r("Prc Name")
                    obj.SubPrcName_K = r("Sub Prc Name")
                    If r("JE Name") IsNot DBNull.Value Then
                        obj.JEName = r("JE Name")
                    End If
                    If r("JV Name") IsNot DBNull.Value Then
                        obj.JVName = r("JV Name")
                    End If
                    If r("Unit") IsNot DBNull.Value Then
                        obj.Unit = r("Unit")
                    End If
                    obj.MinQty = r("Min Qty")
                    obj.Old = False

                    obj.StdDtbtQty = r("Std Dtbt Qty")
                    obj.AddDtbtQty = r("Add Dtbt Qty")
                    obj.NormWeek = r("Norm Week")
                    obj.MonthStd = r("Norm Month")
                    obj.CreateDate = DateTime.Now
                    obj.CreateUser = CurrentUser.UserID
                    If _db.ExistObject(obj) Then
                        _db.Update(obj)
                    Else
                        _db.Insert(obj)
                    End If
                Next
                _db.Commit()
                ShowSuccess()
                mnuShowAll.PerformClick()
            Catch ex As Exception
                _db.RollBack()
                ShowError(ex, "mnuImport_Click", Name)
            End Try
        Else
            ShowWarning("Không có dữ liệu. Vui lòng kiểm tra lại.")
        End If
    End Sub

    Private Sub bttOK_Click(sender As System.Object, e As System.EventArgs) Handles bttOK.Click
        If _isOption Then
            _gridRow = GridView1
            Close()
        End If
    End Sub

    Private Sub GridView1_Click(sender As Object, e As EventArgs) Handles GridView1.Click
        If _cellClick = 0 Then Exit Sub
        txtECode.Text = GridView1.GetFocusedRowCellValue("ECode")
        cboSubPrc.Text = GridView1.GetFocusedRowCellValue("SubPrcName")

        Dim ECode As String = Trim(txtECode.Text)
        Dim sql As String = String.Format("select SubPrcName from {0} where ECode = '{1}'",
                                          PublicTable.Table_PCM_Dept, ECode)
        Dim dt As DataTable = _db.FillDataTable(sql)
        cboSubPrc.DataSource = dt
        cboSubPrc.DisplayMember = "SubPrcName"
        cboSubPrc.ValueMember = "SubPrcName"

        cboSubPrc.SelectedValue = IIf(GridView1.GetFocusedRowCellValue("SubPrcName") Is DBNull.Value, -1,
                                      GridView1.GetFocusedRowCellValue("SubPrcName"))

        txtJCode.Text = IIf(GridView1.GetFocusedRowCellValue("JCode") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("JCode"))
        txtJEName.Text = IIf(GridView1.GetFocusedRowCellValue("JEName") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("JEName"))
        txtJVName.Text = IIf(GridView1.GetFocusedRowCellValue("JVName") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("JVName"))
        txtUnit.Text = IIf(GridView1.GetFocusedRowCellValue("Unit") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("Unit"))
        txtMinQty.Text = IIf(GridView1.GetFocusedRowCellValue("MinQty") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("MinQty"))
        txtStdDtbtQty.Text = IIf(GridView1.GetFocusedRowCellValue("StdDtbtQty") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("StdDtbtQty"))
        txtAddDtbtQty.Text = IIf(GridView1.GetFocusedRowCellValue("AddDtbtQty") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("AddDtbtQty"))
        txtWeekStd.Text = IIf(GridView1.GetFocusedRowCellValue("NormWeek") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("NormWeek"))
        txtMonthStd.Text = IIf(GridView1.GetFocusedRowCellValue("NormMonth") Is DBNull.Value, "", GridView1.GetFocusedRowCellValue("NormMonth"))
    End Sub
End Class