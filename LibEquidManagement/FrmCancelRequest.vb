﻿Imports CommonDB
'Imports LibEntity
Imports PublicUtility

Public Class FrmCancelRequest
    Public _myID As String = ""
    Dim _db As New DBSql(PublicUtility.PublicConst.EnumServers.NDV_SQL_ERP_NDV)

    Private Sub FrmCancelRequest_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        LoadEmail()
        dtpDate.EditValue = Date.Now

        If _myID = "" Then
            _myID = Me.AccessibleName
        End If
        LoadHead()
    End Sub

    Sub ResetControl()
        bttSubmit.Visible = False
        bttSave.Visible = False

        bttChecked.Visible = False
        bttApproved.Visible = False
        bttPICQA.Visible = False
        bttCheckedQA.Visible = False
        bttApprovedQA.Visible = False

        bttRejectChecked.Visible = False
        bttRejectApproved.Visible = False
        bttRejectPICQA.Visible = False
        bttRejectCheckedQA.Visible = False
        bttRejectApprovedQA.Visible = False

        txtCmtPIC.ReadOnly = True
        txtCmtChecked.ReadOnly = True
        txtCmtApproved.ReadOnly = True
        txtCmtPICQA.ReadOnly = True
        txtCmtCheckedQA.ReadOnly = True
        txtCmtApprovedQA.ReadOnly = True

        lblDatePIC.Text = ""
        lblDateChecked.Text = ""
        lblDateApproved.Text = ""
        lblDatePICQA.Text = ""
        lblDateCheckedQA.Text = ""
        lblDateApprovedQA.Text = ""

        bttThemThietBi.Visible = False
        txtLyDoHuy.ReadOnly = True
        cboLoai.Enabled = False

        cboChecked.Enabled = False
        cboApproved.Enabled = False
    End Sub

    Sub LoadHead()
        ResetControl()
        If _myID = "" Then
            bttSave.Visible = True
            bttSubmit.Visible = True
            bttThemThietBi.Visible = True
            txtLyDoHuy.ReadOnly = False
            cboLoai.Enabled = True
            cboChecked.Enabled = True
            cboApproved.Enabled = True
            txtCmtPIC.ReadOnly = False
            txtSection.Text = CurrentUser.SortSection
            Return
        End If
        Dim obj As New QAE_Cancel
        obj.ID_K = _myID
        _db.GetObject(obj)

        txtID.Text = _myID
        txtSection.Text = obj.Section
        dtpDate.EditValue = obj.RequestDate
        cboLoai.Text = obj.Loai

        txtMailPIC.Text = obj.PIC
        cboChecked.Text = obj.Checked
        cboApproved.Text = obj.Approved
        txtMailPICQA.Text = obj.QAPIC
        txtMailCheckedQA.Text = obj.QAChecked
        txtMailApprovedQA.Text = obj.QAApproved

        txtCmtPIC.Text = obj.PICComment
        txtCmtChecked.Text = obj.CheckedComment
        txtCmtApproved.Text = obj.ApprovedComment
        txtCmtPICQA.Text = obj.QAPICComment
        txtCmtCheckedQA.Text = obj.QACheckedComment
        txtCmtApprovedQA.Text = obj.QAApprovedComment

        txtMaSoThietBi.Text = obj.MaThietBi
        txtTenThietBi.Text = obj.TenThietBiDo
        txtNhaSanXuat.Text = obj.NhaSanXuat
        txtLyDoHuy.Text = obj.LyDo
        dtpNgayMuaHang.EditValue = obj.NgayMuaHang

        'Load Approved
        If obj.PICDate > DateTime.MinValue Then
            lblDatePIC.Text = obj.PICDate.ToString("dd-MM-yyyy HH:mm")
            lblDatePIC.Visible = True
            bttSave.Visible = False
            bttSubmit.Visible = False
        Else
            If CurrentUser.Mail = obj.CurrentID And obj.CurrentID = obj.PIC Then
                lblDatePIC.Visible = False
                bttSave.Visible = True
                bttSubmit.Visible = True
                bttThemThietBi.Visible = True
                txtLyDoHuy.ReadOnly = False
                cboLoai.Enabled = True
                cboChecked.Enabled = True
                cboApproved.Enabled = True

                txtCmtPIC.ReadOnly = False
                GoTo GotoDetail
            End If
        End If

        If obj.CheckedDate > DateTime.MinValue Then
            lblDateChecked.Visible = True
            lblDateChecked.Text = obj.CheckedDate.ToString("dd-MM-yyyy HH:mm")
        Else
            If CurrentUser.Mail = obj.CurrentID And obj.CurrentID = obj.Checked Then
                lblDateChecked.Visible = False
                bttChecked.Visible = True
                bttRejectChecked.Visible = True
                txtCmtChecked.ReadOnly = False
                GoTo GotoDetail
            End If
        End If

        If obj.ApprovedDate > DateTime.MinValue Then
            lblDateApproved.Visible = True
            lblDateApproved.Text = obj.ApprovedDate.ToString("dd-MM-yyyy HH:mm")
        Else
            If obj.CurrentID = CurrentUser.Mail And obj.CurrentID = obj.Approved Then
                lblDateApproved.Visible = False
                bttApproved.Visible = True
                bttRejectApproved.Visible = True
                txtCmtApproved.ReadOnly = False
                GoTo GotoDetail
            End If
        End If

        If obj.QAPICDate > DateTime.MinValue Then
            lblDatePICQA.Visible = True
            lblDatePICQA.Text = obj.QAPICDate.ToString("dd-MM-yyyy HH:mm")
        Else
            If obj.CurrentID = CurrentUser.Mail And obj.CurrentID = obj.QAPIC Then
                lblDatePICQA.Visible = False
                bttPICQA.Visible = True
                bttRejectPICQA.Visible = True
                txtCmtPICQA.ReadOnly = False
                GoTo GotoDetail
            End If
        End If

        If obj.QACheckedDate > DateTime.MinValue Then
            lblDateCheckedQA.Visible = True
            lblDateCheckedQA.Text = obj.QACheckedDate.ToString("dd-MM-yyyy HH:mm")
        Else
            If CurrentUser.Mail = obj.CurrentID And obj.CurrentID = obj.QAChecked Then
                lblDateCheckedQA.Visible = False
                bttCheckedQA.Visible = True
                bttRejectCheckedQA.Visible = True
                txtCmtCheckedQA.ReadOnly = False
                GoTo GotoDetail
            End If
        End If

        If obj.QAApprovedDate > DateTime.MinValue Then
            lblDateApprovedQA.Visible = True
            lblDateApprovedQA.Text = obj.QAApprovedDate.ToString("dd-MM-yyyy HH:mm")
        Else
            If CurrentUser.Mail = obj.CurrentID And obj.CurrentID = obj.QAApproved Then
                lblDateApprovedQA.Visible = False
                bttApprovedQA.Visible = True
                bttRejectApprovedQA.Visible = True
                txtCmtApprovedQA.ReadOnly = False
                GoTo GotoDetail
            End If
        End If

GotoDetail:
    End Sub

    Sub ConfirmUpdateOutlook(sumit As Submit, confirm As ConfirmOT, comment As String, cc As List(Of String), obj As QAE_Cancel)
        Try
            _db.BeginTransaction()
            Dim lstTo As New List(Of String)
            Dim lstCC As New List(Of String)
            Dim lstBCC As New List(Of String)

            Dim arrCc() As String = Nothing
            Dim title = "(" & obj.Section & ")" & " YÊU CẦU SỬA CHỮA/HỦY THIẾT BỊ ĐO " & obj.RequestDate.ToString("dd-MM-yyyy")
            If sumit = Submit.Reject Then
                obj.PICDate = Nothing
                obj.CheckedDate = Nothing
                obj.ApprovedDate = Nothing
                obj.QAPICDate = Nothing
                obj.QACheckedDate = Nothing
                obj.QAApprovedDate = Nothing
                obj.CurrentID = obj.PIC

                lstTo.Add(obj.PIC)
                SendMailOutlook(title, Nothing, sumit, lstTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)

            Else
                Select Case confirm
                    Case ConfirmQAE.PIC
                        lstTo.Add(obj.Checked)
                        SendMailOutlook(title, Nothing, sumit, lstTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                        obj.CurrentID = obj.Checked
                        GoTo EndConfirm
                    Case ConfirmQAE.Check
                        If obj.Approved <> "" Then
                            lstTo.Add(obj.Approved)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentID = obj.Approved
                            GoTo EndConfirm
                        End If
                    Case ConfirmQAE.Approved
                        If obj.QAPIC <> "" Then
                            lstTo.Add(obj.QAPIC)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentID = obj.QAPIC
                            GoTo EndConfirm
                        End If
                    Case ConfirmQAE.QAPIC
                        If obj.QAChecked <> "" Then
                            lstTo.Add(obj.QAChecked)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentID = obj.QAChecked
                            GoTo EndConfirm
                        End If
                    Case ConfirmQAE.QACheck
                        If obj.QAApproved <> "" Then
                            lstTo.Add(obj.QAApproved)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentID = obj.QAApproved
                            GoTo EndConfirm
                        End If
                    Case ConfirmQAE.QAApproved
                        If obj.PIC <> "" Then
                            lstTo.Add(obj.PIC)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentID = ""
                            GoTo EndConfirm
                        End If
                End Select
            End If
EndConfirm:
            _db.Update(obj)
            _db.Commit()
            NextRequest()
        Catch ex As Exception
            _db.RollBack()
            ShowError(ex, "Confirm", Me.Name)
        End Try
    End Sub

    Private Sub NextRequest()
        Dim obj As Object = _db.ExecuteScalar(String.Format(" select ID from {0} " +
                                                            " where CurrentID='{1}' order by ID ",
                                                    PublicTable.Table_QAE_Cancel,
                                                    CurrentUser.Mail))
        If obj IsNot Nothing Then
            _myID = obj
        End If
        LoadHead()
    End Sub

    Sub LoadEmail()
        Dim sql As String = String.Format(" select m.EmpID,m.Mail from OT_Mail m " +
                                          " inner join OT_Employee e " +
                                          " on m.EmpID=e.EmpID" +
                                          " where e.SectionSort='{0}' and m.Mail<>'' " +
                                          " order by m.Mail ",
                                          CurrentUser.SortSection)
        Dim tb As DataTable = _db.FillDataTable(sql)
        cboChecked.DataSource = _db.FillDataTable(sql)
        cboChecked.ValueMember = "EmpID"
        cboChecked.DisplayMember = "Mail"
        cboChecked.SelectedIndex = -1

        cboApproved.DataSource = tb.Copy
        cboApproved.ValueMember = "EmpID"
        cboApproved.DisplayMember = "Mail"
        cboApproved.SelectedIndex = -1

        Dim obj As New QAE_Approver
        obj.ID_K = 1
        _db.GetObject(obj)

        txtMailPIC.Text = CurrentUser.Mail
        txtMailPICQA.Text = obj.PIC
        txtMailCheckedQA.Text = obj.Checked
        txtMailApprovedQA.Text = obj.Approved

        'Test Mail
        'cboChecked.Text = CurrentUser.Mail
        'cboApproved.Text = CurrentUser.Mail
        'txtMailPICQA.Text = CurrentUser.Mail
        'txtMailCheckedQA.Text = CurrentUser.Mail
        'txtMailApprovedQA.Text = CurrentUser.Mail
    End Sub

    Private Sub bttSave_Click(sender As Object, e As EventArgs) Handles bttSave.Click
        SaveData()
    End Sub

    Function GetID() As String
        Dim myID As String = ""
        Dim myDate As String = "C" & Date.Today.ToString("yyMMdd") & "-"
        Dim STT As Object = _db.ExecuteScalar(String.Format(" select isnull(max(right(ID,2)),0)+1 as STT " +
                                                            " from QAE_Cancel " +
                                                            " where ID like '{0}%'", myDate))
        If IsNumeric(STT) Then
            myID = myDate & STT.ToString().PadLeft(2, "0")
        Else
            myID = myDate & "01"
        End If
        Return myID
    End Function

    Function SaveData() As Boolean
        If txtMailPIC.Text = "" Or
                cboChecked.Text = "" Or
                cboApproved.Text = "" Or
            txtMailPICQA.Text = "" Or
            txtMailCheckedQA.Text = "" Or
            txtMailApprovedQA.Text = "" Then
            ShowWarning("Địa chỉ mail không được để trống !")
            cboChecked.Focus()
            Return False
        End If

        If cboLoai.Text = "" Then
            ShowWarning("Bạn chưa chọn loại là Hủy hay Sửa chữa !")
            cboLoai.Select()
            Return False
        End If
        If txtMaSoThietBi.Text.Trim = "" Then
            ShowWarning("Bạn chưa chọn thiết bị đo !")
            txtMaSoThietBi.Select()
            Return False
        End If
        If txtLyDoHuy.Text.Trim = "" Then
            ShowWarning("Bạn nhập lý do !")
            txtLyDoHuy.Select()
            Return False
        End If

        Dim obj As New QAE_Cancel
        If _myID <> "" Then
            obj.ID_K = _myID
            _db.GetObjectNotReset(obj)
        Else
            _myID = GetID()
            obj.ID_K = _myID
            txtID.Text = _myID
        End If
        obj.CurrentID = CurrentUser.Mail
        obj.Section = txtSection.Text
        obj.RequestDate = dtpDate.DateTime.Date
        obj.Loai = cboLoai.Text

        obj.PIC = txtMailPIC.Text
        obj.PICComment = txtCmtPIC.Text
        obj.Checked = cboChecked.Text
        obj.Approved = cboApproved.Text
        obj.QAPIC = txtMailPICQA.Text
        obj.QAChecked = txtMailCheckedQA.Text
        obj.QAApproved = txtMailApprovedQA.Text

        obj.MaThietBi = txtMaSoThietBi.Text
        obj.TenThietBiDo = txtTenThietBi.Text
        obj.NhaSanXuat = txtNhaSanXuat.Text
        obj.LyDo = txtLyDoHuy.Text
        obj.NgayMuaHang = dtpNgayMuaHang.EditValue

        If _db.ExistObject(obj) Then
            _db.Update(obj)
        Else
            obj.CreatedDate = DateTime.Now
            obj.CreatedUser = CurrentUser.UserID
            _db.Insert(obj)
        End If
        ShowSuccess()
        Return True
    End Function

    Private Sub bttSubmit_Click(sender As Object, e As EventArgs) Handles bttSubmit.Click
        If SaveData() Then
            Dim obj As New QAE_Cancel
            obj.ID_K = _myID
            _db.GetObject(obj)
            obj.PICComment = txtCmtPIC.Text
            obj.PICDate = DateTime.Now
            obj.CurrentID = ""

            ConfirmUpdateOutlook(Submit.Confirm, ConfirmQAE.PIC, obj.PICComment, Nothing, obj)
        End If
    End Sub

    Private Sub bttChecked_Click(sender As Object, e As EventArgs) Handles bttChecked.Click
        Dim obj As New QAE_Cancel
        obj.ID_K = _myID
        _db.GetObject(obj)
        obj.CheckedComment = txtCmtChecked.Text
        obj.CheckedDate = DateTime.Now
        obj.CurrentID = ""

        ConfirmUpdateOutlook(Submit.Confirm, ConfirmQAE.Check, obj.CheckedComment, Nothing, obj)
    End Sub

    Private Sub bttApproved_Click(sender As Object, e As EventArgs) Handles bttApproved.Click
        Dim obj As New QAE_Cancel
        obj.ID_K = _myID
        _db.GetObject(obj)
        obj.ApprovedComment = txtCmtApproved.Text
        obj.ApprovedDate = DateTime.Now
        obj.CurrentID = ""

        ConfirmUpdateOutlook(Submit.Confirm, ConfirmQAE.Approved, obj.ApprovedComment, Nothing, obj)
    End Sub

    Private Sub bttPICQA_Click(sender As Object, e As EventArgs) Handles bttPICQA.Click
        Dim obj As New QAE_Cancel
        obj.ID_K = _myID
        _db.GetObject(obj)
        obj.QAPICComment = txtCmtPICQA.Text
        obj.QAPICDate = DateTime.Now
        obj.CurrentID = ""

        ConfirmUpdateOutlook(Submit.Confirm, ConfirmQAE.QAPIC, obj.QAPICComment, Nothing, obj)
    End Sub

    Private Sub bttCheckedQA_Click(sender As Object, e As EventArgs) Handles bttCheckedQA.Click
        Dim obj As New QAE_Cancel
        obj.ID_K = _myID
        _db.GetObject(obj)
        obj.QACheckedComment = txtCmtCheckedQA.Text
        obj.QACheckedDate = DateTime.Now
        obj.CurrentID = ""

        ConfirmUpdateOutlook(Submit.Confirm, ConfirmQAE.QACheck, obj.QACheckedComment, Nothing, obj)
    End Sub

    Private Sub bttApprovedQA_Click(sender As Object, e As EventArgs) Handles bttApprovedQA.Click
        Dim obj As New QAE_Cancel
        obj.ID_K = _myID
        _db.GetObject(obj)
        obj.QAApprovedComment = txtCmtApprovedQA.Text
        obj.QAApprovedDate = DateTime.Now
        obj.CurrentID = ""

        'Load Status "Hủy bỏ" vào form EquipList
        Dim objEquipList As New QAE_EquidList
        objEquipList.EquipCode_K = txtMaSoThietBi.Text
        _db.GetObject(objEquipList)
        objEquipList.Status = "Hủy bỏ"
        _db.Update(objEquipList)

        ConfirmUpdateOutlook(Submit.Confirm, ConfirmQAE.QAApproved, obj.QAApprovedComment, Nothing, obj)
    End Sub

    Private Sub bttRejectChecked_Click(sender As Object, e As EventArgs) Handles bttRejectChecked.Click
        If txtCmtChecked.Text.Trim() <> "" Then
            Dim obj As New QAE_Cancel
            obj.ID_K = _myID
            _db.GetObject(obj)
            obj.CheckedComment = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtCmtChecked.Text)

            obj.CurrentID = ""
            ConfirmUpdateOutlook(Submit.Reject, ConfirmQAE.Check, obj.CheckedComment, Nothing, obj)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtCmtChecked.Focus()
        End If
    End Sub

    Private Sub bttRejectApproved_Click(sender As Object, e As EventArgs) Handles bttRejectApproved.Click
        If txtCmtApproved.Text.Trim() <> "" Then
            Dim obj As New QAE_Cancel
            obj.ID_K = _myID
            _db.GetObject(obj)
            obj.ApprovedComment = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtCmtApproved.Text)

            obj.CurrentID = ""
            ConfirmUpdateOutlook(Submit.Reject, ConfirmQAE.Approved, obj.ApprovedComment, Nothing, obj)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtCmtApproved.Focus()
        End If
    End Sub

    Private Sub bttRejectPICQA_Click(sender As Object, e As EventArgs) Handles bttRejectPICQA.Click
        If txtCmtPICQA.Text.Trim() <> "" Then
            Dim obj As New QAE_Cancel
            obj.ID_K = _myID
            _db.GetObject(obj)
            obj.QAPICComment = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtCmtPICQA.Text)

            obj.CurrentID = ""
            ConfirmUpdateOutlook(Submit.Reject, ConfirmQAE.QAPIC, obj.QAPICComment, Nothing, obj)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtCmtPICQA.Focus()
        End If
    End Sub

    Private Sub bttRejectCheckedQA_Click(sender As Object, e As EventArgs) Handles bttRejectCheckedQA.Click
        If txtCmtCheckedQA.Text.Trim() <> "" Then
            Dim obj As New QAE_Cancel
            obj.ID_K = _myID
            _db.GetObject(obj)
            obj.QACheckedComment = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtCmtCheckedQA.Text)

            obj.CurrentID = ""
            ConfirmUpdateOutlook(Submit.Reject, ConfirmQAE.QACheck, obj.QACheckedComment, Nothing, obj)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtCmtCheckedQA.Focus()
        End If
    End Sub

    Private Sub bttRejectApprovedQA_Click(sender As Object, e As EventArgs) Handles bttRejectApprovedQA.Click
        If txtCmtApprovedQA.Text.Trim() <> "" Then
            Dim obj As New QAE_Cancel
            obj.ID_K = _myID
            _db.GetObject(obj)
            obj.QAApprovedComment = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtCmtApprovedQA.Text)

            obj.CurrentID = ""
            ConfirmUpdateOutlook(Submit.Reject, ConfirmQAE.QAApproved, obj.QAApprovedComment, Nothing, obj)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtCmtApprovedQA.Focus()
        End If
    End Sub

    Private Sub bttThemThietBi_Click(sender As Object, e As EventArgs) Handles bttThemThietBi.Click
        Dim frm As New FrmEquipList
        frm._isOption = True
        frm.ShowDialog()
        If frm._isID <> "" Then
            txtMaSoThietBi.Text = frm._isID
            Dim obj As New QAE_EquidList
            obj.EquipCode_K = frm._isID
            _db.GetObject(obj)

            txtTenThietBi.Text = obj.EquipNameV
            txtNhaSanXuat.Text = obj.Manufacture
            dtpNgayMuaHang.EditValue = obj.PurchaseDate
        End If
    End Sub
End Class