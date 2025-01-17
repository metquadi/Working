﻿Imports System.IO
Imports System.Windows.Forms
Imports CommonDB
Imports PublicUtility
Public Class FrmStandardTGGC
    Public _id As String = ""
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Dim _path As String = CurrentUser.TempFolder & "PP_StandardTGGC\"

    Private Sub FrmStandardTGGC_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _id = "" Then
            _id = Me.AccessibleName
        End If
        dteDate.DateTime = Date.Now
        LoadMail()
        LoadTenCongDoan()
        LoadHead()
    End Sub
    Sub LoadMail()
        txtMailPIC.Text = CurrentUser.Mail
        Dim dt As DataTable = _db.FillDataTable("SELECT h.EmpID, d.SectionSort, d.Observation, Mail
                                                 FROM OT_Mail AS h
                                                 LEFT JOIN OT_Employee AS d
                                                 ON h.EmpID = d.EmpID
                                                 ORDER BY Mail ")
        cbbMailChecked.Properties.DataSource = dt
        cbbMailChecked.Properties.DisplayMember = "Mail"
        cbbMailChecked.Properties.ValueMember = "Mail"
        cbbMailChecked.Properties.NullText = Nothing
        cbbMailChecked.Properties.PopulateViewColumns()
        cbbMailChecked.Properties.View.Columns("Mail").Width = 200

        'cbbMailApproved.Properties.DataSource = dt
        'cbbMailApproved.Properties.DisplayMember = "Mail"
        'cbbMailApproved.Properties.ValueMember = "Mail"
        'cbbMailApproved.Properties.NullText = Nothing
        'cbbMailApproved.Properties.PopulateViewColumns()
        'cbbMailApproved.Properties.View.Columns("Mail").Width = 200
        'Thêm Approved là Manager
        Dim mailApp As Object = _db.ExecuteScalar(String.Format("SELECT Manager
                                                                 FROM OT_SectionMail AS h
                                                                 LEFT JOIN OT_Employee AS d
                                                                 ON h.Section = d.Section
                                                                 WHERE EmpID = '{0}'", CurrentUser.UserID))
        txtMailApproved.Text = mailApp

        'cbbMailChecked.Text = CurrentUser.Mail
        'txtMailApproved.Text = CurrentUser.Mail
    End Sub
    Sub LoadTenCongDoan()
        Dim dt As DataTable = _db.FillDataTable(String.Format(" SELECT TenCongDoan
                                                                FROM PP_StandardTGGC_Master
                                                                WHERE PICUser = '{0}'
                                                                ORDER BY TenCongDoan",
                                                                CurrentUser.Mail))
        For Each r As DataRow In dt.Rows
            cbbTenCongDoan.Properties.Items.Add(r("TenCongDoan"))
        Next
    End Sub
    Sub ResetControl()
        cbbMailChecked.ReadOnly = True

        btnSave.Visible = False
        btnSubmit.Visible = False
        lblDatePic.Visible = False
        mmePic.ReadOnly = True

        btnCheck.Visible = False
        btnRejectChecked.Visible = False
        lblDateChecked.Visible = False
        mmeChecked.ReadOnly = True

        btnApproved.Visible = False
        btnRejectApproved.Visible = False
        lblDateApproved.Visible = False
        mmeApproved.ReadOnly = True

        cbbTenCongDoan.ReadOnly = True

        btnAddFile.Visible = False
        btnDeleteFile.Visible = False
        mmeContentReport.ReadOnly = False
        linkAttach.Text = ""
    End Sub
    Sub LoadHead()
        ResetControl()
        If _id = "" Then
            cbbMailChecked.ReadOnly = False
            btnSave.Visible = True
            mmePic.ReadOnly = False
            cbbTenCongDoan.ReadOnly = False
            Return
        End If

        txtID.Text = _id
        Dim obj As New PP_StandardTGGC
        obj.ID_K = _id
        _db.GetObject(obj)
        dteDate.EditValue = obj.ReportDate
        txtMailPIC.Text = obj.PreMail
        cbbMailChecked.Text = obj.CheckedMail
        txtMailApproved.Text = obj.ApprovedMail
        mmePic.Text = obj.PreComment
        mmeChecked.Text = obj.CheckedComment
        mmeApproved.Text = obj.ApprovedComment
        cbbTenCongDoan.Text = obj.TenCongDoan
        txtMailTo.Text = obj.ListMailTo

        linkAttach.Text = obj.AttachFileName
        linkAttach.Tag = obj.AttachFileServer
        mmeContentReport.Text = obj.ContentReport

        FillMaster()

        If obj.PreDate > DateTime.MinValue Then
            lblDatePic.Text = obj.PreDate.ToString("dd-MMM-yyyy HH:mm")
            lblDatePic.Visible = True
        Else
            If CurrentUser.Mail = obj.CurrentMail And obj.CurrentMail = obj.PreMail Then
                cbbMailChecked.ReadOnly = False
                btnSave.Visible = True
                btnSubmit.Visible = True
                mmePic.ReadOnly = False
                cbbTenCongDoan.ReadOnly = False
                btnAddFile.Visible = True
                btnDeleteFile.Visible = True
                mmeContentReport.ReadOnly = False
                Return
            End If
        End If

        If obj.CheckedDate > DateTime.MinValue Then
            lblDateChecked.Text = obj.CheckedDate.ToString("dd-MMM-yyyy HH:mm")
            lblDateChecked.Visible = True
        Else
            If CurrentUser.Mail = obj.CurrentMail And obj.CurrentMail = obj.CheckedMail Then
                btnCheck.Visible = True
                btnRejectChecked.Visible = True
                mmeChecked.ReadOnly = False
                btnAddFile.Visible = True
                btnDeleteFile.Visible = True
                Return
            End If
        End If

        If obj.ApprovedDate > DateTime.MinValue Then
            lblDateApproved.Text = obj.ApprovedDate.ToString("dd-MMM-yyyy HH:mm")
            lblDateApproved.Visible = True
        Else
            If CurrentUser.Mail = obj.CurrentMail And obj.CurrentMail = obj.ApprovedMail Then
                btnApproved.Visible = True
                btnRejectApproved.Visible = True
                mmeApproved.ReadOnly = False
                Return
            End If
        End If
    End Sub

    Private Sub cbbTenCongDoan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbbTenCongDoan.SelectedIndexChanged
        Dim mailTo As Object = _db.ExecuteScalar(String.Format("SELECT MailTo
                                                                FROM PP_StandardTGGC_Master
                                                                WHERE TenCongDoan = N'{0}'",
                                                                cbbTenCongDoan.Text))
        txtMailTo.Text = IIf(IsDBNull(mailTo), "", mailTo)
        FillMaster()
    End Sub
    Sub FillMaster()
        GridControl2.DataSource = _db.FillDataTable(String.Format(" SELECT AttachFileName, AttachFileServerName, 
                                                                            ContentReport, DateApproved
                                                                    FROM PP_StandardTGGC_File AS h
                                                                    LEFT JOIN PP_StandardTGGC_Master AS d
                                                                    ON h.ProcessID = d.ID
                                                                    WHERE d.TenCongDoan = N'{0}'
                                                                    AND h.AttachFileName <> N'{1}'
                                                                    ORDER BY DateApproved DESC",
                                                                    cbbTenCongDoan.Text,
                                                                    linkAttach.Text))
        GridControlSetFormat(GridView2, 0)
        GridView2.Columns("AttachFileName").ColumnEdit = GridControlLinkEdit()
        GridView2.Columns("AttachFileServerName").Visible = False
        GridView2.Columns("AttachFileName").Width = 250
        GridView2.Columns("ContentReport").Width = 200
    End Sub
    Function GetID() As String
        Dim val As Object = _db.ExecuteScalar(String.Format("SELECT Right(MAX(ID), 2)
                                                             FROM PP_StandardTGGC
                                                             WHERE ID LIKE '%{0}%'",
                                                             Date.Now.ToString("yyyyMM")))
        If Not IsDBNull(val) Then
            val = (Integer.Parse(val) + 1).ToString.PadLeft(2, "0")
            Return "SRPP" + Date.Now.ToString("yyyyMM") + "-" + val
        Else
            Return "SRPP" + Date.Now.ToString("yyyyMM") + "-01"
        End If
    End Function

    Private Sub btnAddFile_Click(sender As Object, e As EventArgs) Handles btnAddFile.Click
        Dim frmOpen As New OpenFileDialog
        frmOpen.ShowDialog()
        If frmOpen.FileName <> "" Then
            linkAttach.Text = frmOpen.SafeFileName
            linkAttach.Tag = _path + "StandardTGGC" + "_" + _id + "_" + frmOpen.SafeFileName
            Dim obj As New PP_StandardTGGC
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.AttachFileName = linkAttach.Text
            obj.AttachFileServer = linkAttach.Tag
            _db.Update(obj)
            File.Copy(frmOpen.FileName, linkAttach.Tag, True)
        End If
    End Sub
    Private Sub btnDeleteFile_Click(sender As Object, e As EventArgs) Handles btnDeleteFile.Click
        If File.Exists(linkAttach.Tag) Then
            File.Delete(linkAttach.Tag)
        End If
        Dim obj As New PP_StandardTGGC
        obj.ID_K = _id
        _db.GetObject(obj)
        obj.AttachFileName = ""
        obj.AttachFileServer = ""
        _db.Update(obj)
        linkAttach.Text = ""
        linkAttach.Tag = ""
    End Sub

    Private Sub linkAttach_Click(sender As Object, e As EventArgs) Handles linkAttach.Click
        If File.Exists(linkAttach.Tag) Then
            Process.Start(OpenfileTemp(linkAttach.Tag))
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If CheckData(False) Then
            SaveData()
            LoadHead()
            ShowSuccess()
        End If
    End Sub
    Function CheckData(isContent As Boolean) As Boolean
        If txtMailPIC.Text = "" Or cbbMailChecked.Text = "" Or txtMailApproved.Text = "" Then
            ShowWarning("Địa chỉ Mail không được để trống !")
            cbbMailChecked.Select()
            Return False
        ElseIf cbbTenCongDoan.Text = "" Then
            ShowWarning("Bạn chưa chọn tên công đoạn !")
            cbbTenCongDoan.Select()
            Return False
        End If

        If isContent Then
            If linkAttach.Text = "" Then
                ShowWarning("Bạn chưa Attach File !")
                btnAddFile.Select()
                Return False
            ElseIf mmeContentReport.Text.Trim = "" Then
                ShowWarning("Bạn chưa nhập nội dung Report !")
                mmeContentReport.Select()
                Return False
            End If
        End If
        Return True
    End Function
    Sub SaveData()
        Dim obj As New PP_StandardTGGC
        If _id <> "" Then
            obj.ID_K = _id
            _db.GetObject(obj)
        Else
            _id = GetID()
            txtID.Text = _id
            obj.ID_K = _id
            obj.CurrentMail = CurrentUser.Mail
        End If

        obj.ReportDate = dteDate.DateTime.Date
        obj.PreMail = txtMailPIC.Text
        obj.CheckedMail = cbbMailChecked.Text
        obj.ApprovedMail = txtMailApproved.Text
        obj.PreComment = mmePic.Text
        obj.CheckedComment = mmeChecked.Text
        obj.ApprovedComment = mmeApproved.Text
        obj.TenCongDoan = cbbTenCongDoan.Text
        obj.ListMailTo = txtMailTo.Text
        obj.ContentReport = mmeContentReport.Text
        If _db.ExistObject(obj) Then
            _db.Update(obj)
        Else
            obj.CreateUser = CurrentUser.UserID
            obj.CreateDate = DateTime.Now
            _db.Insert(obj)
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If CheckData(True) Then
            SaveData()
            Dim obj As New PP_StandardTGGC
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.PreDate = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Pic, obj.PreComment, Nothing, obj)
        End If
    End Sub
    Public Enum Confirm
        Pic
        Checked
        Approved
    End Enum

    Sub ConfirmUpdateOutlook(sumit As Submit, confirm As Confirm, comment As String, cc As List(Of String), obj As PP_StandardTGGC)
        Try
            _db.BeginTransaction()
            Dim lstTo As New List(Of String)
            Dim lstCC As New List(Of String)
            Dim lstBCC As New List(Of String)
            Dim arrCc() As String = Nothing
            Dim title = "(Reject) Standard TGGC - " + cbbTenCongDoan.Text + " - " & obj.ReportDate.ToString("dd-MMM-yyyy")

            If sumit = Submit.Reject Then
                obj.PreDate = Nothing
                obj.CheckedDate = Nothing
                obj.ApprovedDate = Nothing
                obj.CurrentMail = obj.PreMail
                lstTo.Add(obj.PreMail)
                SendMailOutlook(title, Nothing, Submit.Reject, lstTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
            Else
                title = "Standard TGGC - " + cbbTenCongDoan.Text + " - " & obj.ReportDate.ToString("dd-MMM-yyyy")
                Select Case confirm
                    Case Confirm.Pic
                        If obj.CheckedMail <> "" Then
                            lstTo.Add(obj.CheckedMail)
                            SendMailOutlook(title, Nothing, Submit.Confirm, lstTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.CheckedMail
                            GoTo EndConfirm
                        End If
                        GoTo Checked
                    Case Confirm.Checked
Checked:
                        If obj.ApprovedMail <> "" Then
                            lstTo.Add(obj.ApprovedMail)
                            SendMailOutlook(title, Nothing, Submit.Confirm, lstTo, lstCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.ApprovedMail
                            GoTo EndConfirm
                        End If
                        GoTo Approved
                    Case Confirm.Approved
Approved:
                        If obj.ListMailTo <> "" Then
                            lstTo.Add(obj.ListMailTo)
                            lstCC.Add(obj.PreMail)
                            SendMailOutlook(title, Nothing, Submit.Info, lstTo, lstCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = ""
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
    Sub NextRequest()
        Dim idNext As Object = _db.ExecuteScalar(String.Format("SELECT ID FROM PP_StandardTGGC
                                                                WHERE CurrentMail = '{0}' 
                                                                ORDER BY ID",
                                                                CurrentUser.Mail))
        If idNext IsNot Nothing Then
            _id = idNext
        End If
        LoadHead()
    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        If CheckData(True) Then
            SaveData()
            Dim obj As New PP_StandardTGGC
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CheckedDate = DateTime.Now
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Checked, obj.CheckedComment, Nothing, obj)
        End If
    End Sub

    Private Sub btnRejectChecked_Click(sender As Object, e As EventArgs) Handles btnRejectChecked.Click
        If mmeChecked.Text.Trim() <> "" Then
            Dim obj As New PP_StandardTGGC
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CheckedComment = String.Format("({0}) {1}", DateTime.Now().ToString("dd-MMM-yyyy HH:mm"), mmeChecked.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.Checked, obj.CheckedComment, Nothing, obj)
        Else
            ShowWarning("Bạn phải ghi chú lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeChecked.Select()
        End If
    End Sub

    Private Sub btnApproved_Click(sender As Object, e As EventArgs) Handles btnApproved.Click
        If CheckData(True) Then
            SaveData()
            Dim obj As New PP_StandardTGGC
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.ApprovedDate = DateTime.Now
            obj.CurrentMail = ""

            'Lưu file vào master với DateApproved mới nhất
            Dim idMaster As Object = _db.ExecuteScalar(String.Format("SELECT ID
                                                                      FROM PP_StandardTGGC_Master
                                                                      WHERE TenCongDoan = N'{0}'",
                                                                      cbbTenCongDoan.Text))

            Dim objM As New PP_StandardTGGC_File
            objM.FileID_K = GetNewFileID(idMaster)
            objM.ProcessID = idMaster
            objM.AttachFileName = linkAttach.Text
            objM.AttachFileServerName = linkAttach.Tag
            objM.ContentReport = mmeContentReport.Text
            objM.DateApproved = DateTime.Now
            objM.UserApproved = CurrentUser.UserID
            _db.Insert(objM)

            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Approved, obj.ApprovedComment, Nothing, obj)
        End If
    End Sub
    Function GetNewFileID(processID) As String
        Dim valID As Object = _db.ExecuteScalar(String.Format(" SELECT ISNULL(RIGHT(MAX(FileID), 5), 0)
                                                                FROM PP_StandardTGGC_File
                                                                WHERE ProcessID = '{0}'",
                                                                processID))
        valID = (Integer.Parse(valID) + 1).ToString.PadLeft(5, "0")
        Return processID + "_" + valID
    End Function

    Private Sub btnRejectApproved_Click(sender As Object, e As EventArgs) Handles btnRejectApproved.Click
        If mmeApproved.Text.Trim() <> "" Then
            Dim obj As New PP_StandardTGGC
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.ApprovedComment = String.Format("({0}) {1}", DateTime.Now().ToString("dd-MMM-yyyy HH:mm"), mmeApproved.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.Approved, obj.ApprovedComment, Nothing, obj)
        Else
            ShowWarning("Bạn phải ghi chú lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeApproved.Select()
        End If
    End Sub

    Private Sub GridView2_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView2.RowClick
        If GridView2.FocusedColumn.FieldName = "AttachFileName" Then
            If File.Exists(GridView2.GetFocusedRowCellValue("AttachFileServerName")) Then
                Process.Start(OpenfileTemp(GridView2.GetFocusedRowCellValue("AttachFileServerName")))
            End If
        End If
    End Sub
End Class