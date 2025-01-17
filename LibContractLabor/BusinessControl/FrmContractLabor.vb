﻿Imports PublicUtility
Imports CommonDB
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors
Imports System.IO
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils.Layout

Public Class FrmContractLabor : Inherits DevExpress.XtraEditors.XtraForm
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Public _ID As String = ""
    Dim TitleRP As String = "Review Contract Labor"
    Dim _path As String = CurrentUser.TempFolder & "GA_ContractLabor\"

    Public Enum ConfirmContract
        PIC
        LL
        GLS
        MGS
        DMS
        MGG
        DMG
    End Enum
    Private Sub FrmContractLabor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dteDate.EditValue = Date.Now
        LoadEmail()
        If _ID = "" Then
            _ID = Me.AccessibleName
            txtID.Text = _ID
        End If
        LoadAll()
    End Sub
    Function LoadDataMail() As DataTable
        Dim sql As String = String.Format(" SELECT m.Mail, e.Section 
                                            FROM OT_Mail AS m 
                                            LEFT JOIN OT_Employee AS e 
                                            ON e.EmpID = m.EmpID 
                                            WHERE m.Mail <> ''
                                            ORDER BY Mail")
        Return _db.FillDataTable(sql)
    End Function

    Sub LoadComboboxMail(sender As SearchLookUpEdit)
        sender.Properties.DataSource = LoadDataMail()
        sender.Properties.ValueMember = "Mail"
        sender.Properties.DisplayMember = "Mail"
        sender.EditValue = ""
    End Sub

    Sub LoadComboboxSection(sender As ComboBoxEdit)
        Dim para(1) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Action", "GetSection")
        para(1) = New SqlClient.SqlParameter("@StartDate", dteDate.DateTime.Date)
        Dim dtSection As DataTable = _db.ExecuteStoreProcedureTB("sp_GA_CT_ReviewContract", para)
        cbbSection.Properties.Items.Clear()
        For Each r As DataRow In dtSection.Rows
            cbbSection.Properties.Items.Add(r("Section"))
        Next
        sender.EditValue = ""
    End Sub

    Sub LoadEmail()
        Dim oMailIT As New OT_SectionMail
        oMailIT.Section_K = "General Affairs"
        _db.GetObject(oMailIT)

        LoadComboboxMail(cboLLSection)
        LoadComboboxMail(cboGLSection)
        LoadComboboxMail(cboMGSection)
        LoadComboboxMail(cboDMSection)

        LoadComboboxSection(cbbSection)

        txtPICMail.Text = CurrentUser.Mail
        cboLLSection.Text = ""
        cboGLSection.Text = ""
        cboMGSection.Text = ""
        cboDMSection.Text = ""
        txtMGMail.Text = oMailIT.Manager
        txtDMMail.Text = oMailIT.DManager

        'txtPICMail.Text = CurrentUser.Mail
        'cboLLSection.Text = CurrentUser.Mail
        'cboGLSection.Text = CurrentUser.Mail
        'cboMGSection.Text = CurrentUser.Mail
        'cboDMSection.Text = CurrentUser.Mail
        'txtMGMail.Text = CurrentUser.Mail
        'txtDMMail.Text = CurrentUser.Mail
    End Sub

    Function GetID() As String
        Dim myID As String = "GAF127" & "-" & DateTime.Now.ToString("yyMM") & "-"
        Dim sql As String = String.Format(" SELECT ISNULL(MAX(RIGHT(ID,2)),0) 
                                            FROM GA_CT_ReviewContract 
                                            WHERE ID LIKE '{0}%'",
                                            myID)
        Dim obj As Object = _db.ExecuteScalar(sql)
        Return myID & (CType(obj, Integer) + 1).ToString().PadLeft(2, "0")
    End Function

    Sub SetDiableControl()
        txtPICCmt.ReadOnly = True
        txtLLCmt.ReadOnly = True
        txtGLCmt.ReadOnly = True
        txtMGCmt.ReadOnly = True
        txtDMCmt.ReadOnly = True
        txtMGGCmt.ReadOnly = True
        txtDMGCmt.ReadOnly = True

        lblDatePIC.Visible = False
        lblDateLL.Visible = False
        lblDateGLS.Visible = False
        lblDateDMS.Visible = False
        lblDateMGS.Visible = False
        lblDateMGG.Visible = False
        lblDateDMG.Visible = False

        btnSubmit.Visible = False
        btnSave.Visible = False

        btnConfirmLLS.Visible = False
        btnRejectLLS.Visible = False

        btnRejectGLS.Visible = False
        btnConfirmGLS.Visible = False

        btnRejectMGS.Visible = False
        btnConfirmMGS.Visible = False

        btnRejectDMS.Visible = False
        btnConfirmDMS.Visible = False

        btnRejectMGG.Visible = False
        btnConfirmMGG.Visible = False

        btnRejectDMG.Visible = False
        btnConfirmDMG.Visible = False

        btnAddFile.Visible = False
        btnDeleteFile.Visible = False
    End Sub
    Private Sub dteDate_EditValueChanged(sender As Object, e As EventArgs) Handles dteDate.EditValueChanged
        If dteDate.DateTime > DateTime.MinValue Then
            LoadComboboxSection(cbbSection)
        End If
    End Sub
    Private Sub cbbSection_EditValueChanged(sender As Object, e As EventArgs) Handles cbbSection.EditValueChanged
        If cbbSection.Text <> "" Then
            Dim para(2) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Action", "GetEmployee")
            para(1) = New SqlClient.SqlParameter("@Section", cbbSection.Text)
            para(2) = New SqlClient.SqlParameter("@StartDate", dteDate.DateTime.Date)
            Dim dtEmp = _db.ExecuteStoreProcedureTB("sp_GA_CT_ReviewContract", para)
            Dim dtNew As DataTable = dtEmp.Copy
            If CheckHas1Year(dtEmp) Then
                Dim dtView As DataView = dtEmp.DefaultView
                dtView.RowFilter = "LoaiHDCu = 'HĐHN' OR LoaiHDCu = 'HĐTV'"
                dtNew = dtView.ToTable
                gridBandLoaiHDLD1Nam.Visible = True
                gridBandTinhTrangHopDongCu.Visible = False
                gridBandTinhTrangHopDongMoi.Visible = False
            Else
                gridBandLoaiHDLD1Nam.Visible = False
                gridBandTinhTrangHopDongCu.Visible = True
                gridBandTinhTrangHopDongMoi.Visible = True
            End If
            GridControl1.DataSource = dtNew
            GridControlSetFormat(BandedGridView1)
            BandedGridView1.OptionsView.ShowFooter = False
            EnableGA(BandedGridView1)
        End If
    End Sub
    Public Sub LoadAll()
        SetDiableControl()
        If _ID = "" Then
            LoadDetail("")
            EnableGA(BandedGridView1)
            BandedGridView1.OptionsView.ShowColumnHeaders = True
            btnSave.Visible = True
            txtPICCmt.Text = "Phiếu đánh giá nhân viên đề nghị ký hợp đồng lao động" +
                                Environment.NewLine +
                                "雇用契約締結申請する従業員の評定書"
            txtPICCmt.ReadOnly = False
            dteDate.EditValue = DateTime.Now
            Return
        End If
        'Load Head----------------------
        Dim head As New GA_CT_ReviewContract
        head.ID_K = _ID
        _db.GetObject(head)
        txtID.Text = head.ID_K
        dteDate.EditValue = head.CreatedDate
        cbbSection.Text = head.Section
        cbbSection.ReadOnly = True

        txtPICMail.Text = head.PIC
        cboLLSection.Text = head.LL
        cboGLSection.Text = head.GL
        cboMGSection.Text = head.MG
        cboDMSection.Text = head.DM
        txtMGMail.Text = head.GAMG
        txtDMMail.Text = head.GADM

        txtPICCmt.Text = head.PICCmt
        txtLLCmt.Text = head.LLCmt
        txtGLCmt.Text = head.GLCmt
        txtMGCmt.Text = head.MGCmt
        txtDMCmt.Text = head.DMCmt
        txtMGGCmt.Text = head.GAMGCmt
        txtDMGCmt.Text = head.GADMCmt

        linkAttach.Text = head.AttachFileName
        linkAttach.Tag = head.AttachFileServer

        'Load Detail---------------------- 
        LoadDetail(head.ID_K)

        'Set Status Phê duyệt--------------------
        If head.PICDate > DateTime.MinValue Then
            lblDatePIC.Text = head.PICDate.ToString("dd-MM-yyyy HH:mm")
            lblDatePIC.Visible = True
            btnSave.Visible = False
            btnSubmit.Visible = False
        Else
            If head.CurrentMail = CurrentUser.Mail And head.PIC = CurrentUser.Mail Then
                BandedGridView1.OptionsView.ShowColumnHeaders = True
                btnSave.Visible = True
                btnSubmit.Visible = True
                txtPICCmt.ReadOnly = False
                EnableGA(BandedGridView1)
                GoTo GoDetail
            End If
        End If

        dteDate.ReadOnly = True

        If head.LLDate > DateTime.MinValue Then
            lblDateLL.Text = head.LLDate.ToString("dd-MM-yyyy HH:mm")
            lblDateLL.Visible = True

            btnConfirmLLS.Visible = False
            btnRejectLLS.Visible = False
            txtLLCmt.ReadOnly = True
        Else
            If head.CurrentMail = CurrentUser.Mail And head.LL = CurrentUser.Mail Then
                lblDateLL.Visible = False
                btnConfirmLLS.Visible = True
                btnRejectLLS.Visible = True
                txtLLCmt.ReadOnly = False
                EnableSection(BandedGridView1)
                GoTo GoDetail
            End If
        End If

        If head.GLDate > DateTime.MinValue Then
            lblDateGLS.Text = head.GLDate.ToString("dd-MM-yyyy HH:mm")
            lblDateGLS.Visible = True

            btnConfirmGLS.Visible = False
            btnRejectGLS.Visible = False
            txtGLCmt.ReadOnly = True
        Else
            If head.CurrentMail = CurrentUser.Mail And head.GL = CurrentUser.Mail Then
                lblDateGLS.Visible = False
                btnConfirmGLS.Visible = True
                btnRejectGLS.Visible = True
                txtGLCmt.ReadOnly = False
                EnableSection(BandedGridView1)
                GoTo GoDetail
            End If
        End If

        If head.MGDate > DateTime.MinValue Then
            lblDateMGS.Text = head.MGDate.ToString("dd-MM-yyyy HH:mm")
            lblDateMGS.Visible = True
        Else
            If head.CurrentMail = CurrentUser.Mail And head.MG = CurrentUser.Mail Then
                btnConfirmMGS.Visible = True
                btnRejectMGS.Visible = True
                txtMGCmt.ReadOnly = False
                EnableSection(BandedGridView1)
                GoTo GoDetail
            End If
        End If

        If head.DMDate > DateTime.MinValue Then
            lblDateDMS.Text = head.DMDate.ToString("dd-MM-yyyy HH:mm")
            lblDateDMS.Visible = True
        Else
            If head.CurrentMail = CurrentUser.Mail And head.DM = CurrentUser.Mail Then
                btnConfirmDMS.Visible = True
                btnRejectDMS.Visible = True
                txtDMCmt.ReadOnly = False
                EnableSection(BandedGridView1)
                GoTo GoDetail
            End If
        End If

        cboLLSection.ReadOnly = True
        cboGLSection.ReadOnly = True
        cboMGSection.ReadOnly = True
        cboDMSection.ReadOnly = True
        GridControlReadOnly(BandedGridView1, True)
        GridControlSetColorReadonly(BandedGridView1)
        gridSucKhoe.AppearanceHeader.BackColor = Color.Empty
        gridViPhamNQCT.AppearanceHeader.BackColor = Color.Empty
        gridChatLuongCV.AppearanceHeader.BackColor = Color.Empty
        gridKienThucCV.AppearanceHeader.BackColor = Color.Empty
        gridHanhKiem.AppearanceHeader.BackColor = Color.Empty
        gridQuanHeDongNghiep.AppearanceHeader.BackColor = Color.Empty
        gridSangKien.AppearanceHeader.BackColor = Color.Empty
        gridLapKeHoach.AppearanceHeader.BackColor = Color.Empty
        gridLanhDao.AppearanceHeader.BackColor = Color.Empty
        gridKQDGHDLD.AppearanceHeader.BackColor = Color.Empty
        gridGhiChu.AppearanceHeader.BackColor = Color.Empty

        If head.GAMGDate > DateTime.MinValue Then
            lblDateMGG.Text = head.GAMGDate.ToString("dd-MM-yyyy HH:mm")
            lblDateMGG.Visible = True
        Else
            If head.CurrentMail = CurrentUser.Mail And head.GAMG = CurrentUser.Mail Then
                btnConfirmMGG.Visible = True
                btnRejectMGG.Visible = True
                txtMGGCmt.ReadOnly = False
                GoTo GoDetail
            End If
        End If

        If head.GADMDate > DateTime.MinValue Then
            lblDateDMG.Text = head.GADMDate.ToString("dd-MM-yyyy HH:mm")
            lblDateDMG.Visible = True
        Else
            If head.CurrentMail.Contains(CurrentUser.Mail) And head.GADM = CurrentUser.Mail Then
                btnConfirmDMG.Visible = True
                btnRejectDMG.Visible = True
                txtDMGCmt.ReadOnly = False
                GoTo GoDetail
            End If
        End If
GoDetail:
    End Sub

    Sub ConfirmUpdateOutlook(sumit As Submit, confirm As ConfirmContract, comment As String, cc As List(Of String), obj As GA_CT_ReviewContract)
        Try
            _db.BeginTransaction()
            Dim lstTo As New List(Of String)
            Dim lstCC As New List(Of String)
            Dim lstBCC As New List(Of String)

            Dim arrCc() As String = Nothing
            Dim title = "(" & obj.Section & ") " & TitleRP & " " & obj.CreatedDate.ToString("dd-MM-yyyy")
            If sumit = Submit.Reject Then
                title = "(Reject) (" & obj.Section & ") " & TitleRP & " " & obj.CreatedDate.ToString("dd-MM-yyyy")
                obj.PICDate = Nothing
                obj.LLDate = Nothing
                obj.GLDate = Nothing
                obj.MGDate = Nothing
                obj.DMDate = Nothing
                obj.GAMGDate = Nothing
                obj.GADMDate = Nothing
                obj.CurrentMail = obj.PIC
                lstTo.Add(obj.PIC)
                SendMailOutlook(title, Nothing, sumit, lstTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
            Else
                title = "(Approved) (" & obj.Section & ") " & TitleRP & " " & obj.CreatedDate.ToString("dd-MM-yyyy")
                Select Case confirm
                    Case ConfirmContract.PIC
                        If obj.LL <> "" Then
                            lstTo.Add(obj.LL)
                            SendMailOutlook(title, Nothing, sumit, lstTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.LL
                            GoTo EndConfirm
                        End If
                        GoTo LL
                    Case ConfirmContract.LL
LL:
                        If obj.GL <> "" Then
                            lstTo.Add(obj.GL)
                            SendMailOutlook(title, Nothing, sumit, lstTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.GL
                            GoTo EndConfirm
                        End If
                        GoTo GLS
                    Case ConfirmContract.GLS
GLS:
                        If obj.MG <> "" Then
                            lstTo.Add(obj.MG)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MG
                            GoTo EndConfirm
                        End If
                        GoTo MGS
                    Case ConfirmContract.MGS
MGS:
                        If obj.DM <> "" Then
                            lstTo.Add(obj.DM)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.DM
                            GoTo EndConfirm
                        End If
                        GoTo DMS
                    Case ConfirmContract.DMS
DMS:
                        If obj.GAMG <> "" Then
                            lstTo.Add(obj.GAMG)
                            SendMailOutlook(title, Nothing, sumit, lstTo, cc, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.GAMG
                            GoTo EndConfirm
                        End If
                        GoTo MGG
                    Case ConfirmContract.MGG
MGG:
                        If obj.GADM <> "" Then
                            lstTo.Add(obj.GADM)
                            SendMailOutlook(title, Nothing, sumit, lstTo, lstCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.GADM
                            GoTo EndConfirm
                        End If
                    Case ConfirmContract.DMG
DMG:
                        lstTo.Add(obj.PIC)
                        SendMailOutlook(title, Nothing, sumit, lstTo, lstCC, Nothing, Nothing, Tag, obj.ID_K)
                        obj.CurrentMail = ""
                        GoTo EndConfirm
                End Select
            End If
EndConfirm:
            _db.Update(obj)
            _db.Commit()
        Catch ex As Exception
            _db.RollBack()
            ShowError(ex, "Confirm", Me.Name)
        End Try
    End Sub
    Sub LoadNext(ByVal myMail As String)
        Dim obj As Object = _db.ExecuteScalar(String.Format(" SELECT TOP 1 ID " +
                                                            " FROM GA_CT_ReviewContract " +
                                                            " WHERE CurrentMail='{0}' ",
                                                            myMail))
        If obj Is DBNull.Value Or obj Is Nothing Then
            LoadAll()
        Else
            _ID = obj
            LoadAll()
        End If
    End Sub

    Function CheckDieuKien(GA As Boolean, Section As Boolean) As Boolean
        If GA Then
            'Or cboGLSection.Text = "" Or cboMGSection.Text = "" 
            If txtPICMail.Text = "" Or txtMGMail.Text = "" Or txtDMMail.Text = "" Then
                ShowWarning("Địa chỉ email không được để trống !")
                Return False
            End If
            'If CheckNoOperator() Then
            '    If cboDMSection.Text = "" Then
            '        ShowWarning("Địa chỉ email không được để trống !")
            '        Return False
            '    End If
            'End If
            For r As Integer = 0 To BandedGridView1.RowCount - 1
                If BandedGridView1.GetRowCellValue(r, "SucKhoe") Is DBNull.Value Then
                    ShowWarning(String.Format("MSNV: {0} chưa đánh giá sức khỏe !", BandedGridView1.GetRowCellValue(r, "EmpID")))
                    Return False
                End If
                If BandedGridView1.GetRowCellValue(r, "ViPhamNQCT") Is DBNull.Value Then
                    ShowWarning(String.Format("MSNV: {0} chưa nhập vi phạm NQCT !", BandedGridView1.GetRowCellValue(r, "EmpID")))
                    Return False
                End If
            Next
        End If
        If Section Then
            For r As Integer = 0 To BandedGridView1.RowCount - 1
                Dim warningCLCV = String.Format("MSNV: {0} chưa đánh giá chất lượng công việc !", BandedGridView1.GetRowCellValue(r, "EmpID"))
                If IsNumeric(BandedGridView1.GetRowCellValue(r, "ChatLuongCV")) Then
                    If BandedGridView1.GetRowCellValue(r, "ChatLuongCV") <= 0 Then
                        ShowWarning(warningCLCV)
                        Return False
                    End If
                Else
                    ShowWarning(warningCLCV)
                    Return False
                End If

                Dim warningKTCV = String.Format("MSNV: {0} chưa đánh giá kiến thức công việc !", BandedGridView1.GetRowCellValue(r, "EmpID"))
                If IsNumeric(BandedGridView1.GetRowCellValue(r, "KienThucCV")) Then
                    If BandedGridView1.GetRowCellValue(r, "KienThucCV") <= 0 Then
                        ShowWarning(warningKTCV)
                        Return False
                    End If
                Else
                    ShowWarning(warningKTCV)
                    Return False
                End If

                Dim warningQHDN = String.Format("MSNV: {0} chưa đánh giá quan hệ với đồng nghiệp !", BandedGridView1.GetRowCellValue(r, "EmpID"))
                If IsNumeric(BandedGridView1.GetRowCellValue(r, "QuanHeDongNghiep")) Then
                    If BandedGridView1.GetRowCellValue(r, "QuanHeDongNghiep") <= 0 Then
                        ShowWarning(warningQHDN)
                        Return False
                    End If
                Else
                    ShowWarning(warningQHDN)
                    Return False
                End If

                If BandedGridView1.GetRowCellValue(r, "Observation") <> "Operator" Then
                    Dim warningSK = String.Format("MSNV: {0} chưa đánh giá sáng kiến !", BandedGridView1.GetRowCellValue(r, "EmpID"))
                    If IsNumeric(BandedGridView1.GetFocusedRowCellValue("SangKien")) Then
                        If BandedGridView1.GetFocusedRowCellValue("SangKien") <= 0 Then
                            ShowWarning(warningSK)
                            Return False
                        End If
                    Else
                        ShowWarning(warningSK)
                        Return False
                    End If
                End If

                If BandedGridView1.GetRowCellValue(r, "Observation") <> "Operator" Then
                    Dim warningLKH = String.Format("MSNV: {0} chưa đánh giá khả năng lập kế hoạch, tổ chức !", BandedGridView1.GetRowCellValue(r, "EmpID"))
                    If IsNumeric(BandedGridView1.GetFocusedRowCellValue("LapKeHoach")) Then
                        If BandedGridView1.GetFocusedRowCellValue("LapKeHoach") <= 0 Then
                            ShowWarning(warningLKH)
                            Return False
                        End If
                    Else
                        ShowWarning(warningLKH)
                        Return False
                    End If
                End If

                If BandedGridView1.GetRowCellValue(r, "Observation") <> "Operator" Then
                    Dim warningLD = String.Format("MSNV: {0} chưa đánh giá khả năng giám sát và lãnh đạo !", BandedGridView1.GetRowCellValue(r, "EmpID"))
                    If IsNumeric(BandedGridView1.GetFocusedRowCellValue("LanhDao")) Then
                        If BandedGridView1.GetFocusedRowCellValue("LanhDao") <= 0 Then
                            ShowWarning(warningLD)
                            Return False
                        End If
                    Else
                        ShowWarning(warningLD)
                        Return False
                    End If
                End If

                If BandedGridView1.GetRowCellValue(r, "KQDG") Is DBNull.Value Then
                    ShowWarning(String.Format("MSNV: {0} chưa đánh giá kết quả ký hợp đồng lao động !", BandedGridView1.GetRowCellValue(r, "EmpID")))
                    Return False
                End If
            Next
        End If
        Return True
    End Function
    Sub LoadDetail(myID As String)
        Dim para(1) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@ID", myID)
        para(1) = New SqlClient.SqlParameter("@Action", "GetDetail")
        Dim dtEmp = _db.ExecuteStoreProcedureTB("sp_GA_CT_ReviewContract", para)
        If CheckHas1Year(dtEmp) Then
            gridBandLoaiHDLD1Nam.Visible = True
            gridBandTinhTrangHopDongCu.Visible = False
            gridBandTinhTrangHopDongMoi.Visible = False
        Else
            gridBandLoaiHDLD1Nam.Visible = False
            gridBandTinhTrangHopDongCu.Visible = True
            gridBandTinhTrangHopDongMoi.Visible = True
        End If
        GridControl1.DataSource = dtEmp
        GridControlSetFormat(BandedGridView1)
        BandedGridView1.OptionsView.ShowFooter = False
        BandedGridView1.Columns("KQDG").ColumnEdit = cbbKyHopDong
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If cbbSection.Text = "" Then
            Return
        End If
        If CheckDieuKien(True, False) Then
            Try
                _db.BeginTransaction()
                'Save Head---------------------------------
                Dim objHead As New GA_CT_ReviewContract
                objHead.ID_K = txtID.Text
                If objHead.ID_K = "" Then
                    objHead.ID_K = GetID()
                    _ID = objHead.ID_K
                    txtID.Text = _ID
                Else
                    _db.GetObject(objHead)
                End If
                objHead.Section = cbbSection.Text
                objHead.CreatedDate = dteDate.DateTime.Date
                objHead.CreatedUser = CurrentUser.UserID
                objHead.PIC = txtPICMail.Text
                objHead.LL = cboLLSection.Text
                objHead.GL = cboGLSection.Text
                objHead.MG = cboMGSection.Text
                'If CheckNoOperator() Then
                objHead.DM = cboDMSection.Text
                'End If
                objHead.GAMG = txtMGMail.Text
                objHead.GADM = txtDMMail.Text

                objHead.PICCmt = txtPICCmt.Text
                If _db.ExistObject(objHead) Then
                    _db.Update(objHead)
                Else
                    objHead.CurrentMail = objHead.PIC
                    _db.Insert(objHead)
                End If

                'Save Detail----------------------------  
                _db.ExecuteNonQuery(String.Format("DELETE GA_CT_ReviewContract_Detail
                                                   WHERE ID = '{0}'", objHead.ID_K))
                For r As Integer = 0 To BandedGridView1.RowCount - 1
                    Dim objDetail As New GA_CT_ReviewContract_Detail
                    objDetail.ID_K = objHead.ID_K
                    objDetail.EmpID_K = BandedGridView1.GetRowCellValue(r, "EmpID")
                    If BandedGridView1.GetRowCellValue(r, "LoaiHDCu") IsNot DBNull.Value Then
                        objDetail.LoaiHDCu = BandedGridView1.GetRowCellValue(r, "LoaiHDCu")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "FromOld") IsNot DBNull.Value Then
                        objDetail.FromOld = BandedGridView1.GetRowCellValue(r, "FromOld")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "ToOld") IsNot DBNull.Value Then
                        objDetail.ToOld = BandedGridView1.GetRowCellValue(r, "ToOld")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "LoaiHDNew") IsNot DBNull.Value Then
                        objDetail.LoaiHDNew = BandedGridView1.GetRowCellValue(r, "LoaiHDNew")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "FromNew") IsNot DBNull.Value Then
                        objDetail.FromNew = BandedGridView1.GetRowCellValue(r, "FromNew")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "ToNew") IsNot DBNull.Value Then
                        objDetail.ToNew = BandedGridView1.GetRowCellValue(r, "ToNew")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "SucKhoe") IsNot DBNull.Value Then
                        objDetail.SucKhoe = BandedGridView1.GetRowCellValue(r, "SucKhoe")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "ViPhamNQCT") IsNot DBNull.Value Then
                        objDetail.ViPhamNQCT = BandedGridView1.GetRowCellValue(r, "ViPhamNQCT")
                    End If
                    'If BandedGridView1.GetRowCellValue(r, "ChatLuongCV") IsNot DBNull.Value Then
                    '    objDetail.ChatLuongCV = BandedGridView1.GetRowCellValue(r, "ChatLuongCV")
                    'End If
                    'If BandedGridView1.GetRowCellValue(r, "KienThucCV") IsNot DBNull.Value Then
                    '    objDetail.KienThucCV = BandedGridView1.GetRowCellValue(r, "KienThucCV")
                    'End If
                    'If BandedGridView1.GetRowCellValue(r, "HanhKiem") IsNot DBNull.Value Then
                    '    objDetail.HanhKiem = BandedGridView1.GetRowCellValue(r, "HanhKiem")
                    'End If
                    'If BandedGridView1.GetRowCellValue(r, "QuanHeDongNghiep") IsNot DBNull.Value Then
                    '    objDetail.QuanHeDongNghiep = BandedGridView1.GetRowCellValue(r, "QuanHeDongNghiep")
                    'End If
                    'If BandedGridView1.GetRowCellValue(r, "SangKien") IsNot DBNull.Value Then
                    '    objDetail.SangKien = BandedGridView1.GetRowCellValue(r, "SangKien")
                    'End If
                    'If BandedGridView1.GetRowCellValue(r, "LapKeHoach") IsNot DBNull.Value Then
                    '    objDetail.LapKeHoach = BandedGridView1.GetRowCellValue(r, "LapKeHoach")
                    'End If
                    'If BandedGridView1.GetRowCellValue(r, "LanhDao") IsNot DBNull.Value Then
                    '    objDetail.LanhDao = BandedGridView1.GetRowCellValue(r, "LanhDao")
                    'End If
                    If BandedGridView1.GetRowCellValue(r, "KQDG") IsNot DBNull.Value Then
                        objDetail.KQDG = BandedGridView1.GetRowCellValue(r, "KQDG")
                    End If
                    If BandedGridView1.GetRowCellValue(r, "GhiChu") IsNot DBNull.Value Then
                        objDetail.GhiChu = BandedGridView1.GetRowCellValue(r, "GhiChu")
                    End If

                    If _db.ExistObject(objDetail) Then
                        _db.Update(objDetail)
                    Else
                        objDetail.CreateUser = CurrentUser.UserID
                        objDetail.CreateDate = DateTime.Now.Date
                        _db.Insert(objDetail)
                    End If
                Next

                _db.Commit()
                ShowSuccess()
                LoadAll()
            Catch ex As Exception
                _db.RollBack()
                ShowWarning(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If txtPICCmt.Text = "" Then
            ShowWarning("Bạn chưa nhập comment. Bạn nên ghi cụ thể để cấp trên phê duyệt nhanh hơn.")
            txtPICCmt.Select()
            Return
        Else
            btnSave.PerformClick()
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.PICCmt = txtPICCmt.Text
            obj.PICDate = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, ConfirmContract.PIC, obj.PICCmt, Nothing, obj)
            LoadNext(obj.PIC)
        End If
    End Sub

    Private Sub btnConfirmLLS_Click(sender As Object, e As EventArgs) Handles btnConfirmLLS.Click
        If CheckDieuKien(False, True) Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.LLCmt = txtLLCmt.Text
            obj.LLDate = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, ConfirmContract.LL, obj.LLCmt, Nothing, obj)
            LoadNext(obj.LL)
        End If
    End Sub

    Private Sub btnRejectLLS_Click(sender As Object, e As EventArgs) Handles btnRejectLLS.Click
        If txtLLCmt.Text.Trim() <> "" Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.LLCmt = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtLLCmt.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, ConfirmContract.LL, obj.LLCmt, Nothing, obj)
            LoadNext(obj.LL)
        Else
            ShowWarning("Bạn phải ghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtLLCmt.Focus()
        End If
    End Sub

    Private Sub btnConfirmGLS_Click(sender As Object, e As EventArgs) Handles btnConfirmGLS.Click
        If CheckDieuKien(False, True) Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.GLCmt = txtGLCmt.Text
            obj.GLDate = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, ConfirmContract.GLS, obj.GLCmt, Nothing, obj)
            LoadNext(obj.GL)
        End If
    End Sub

    Private Sub btnRejectGLS_Click(sender As Object, e As EventArgs) Handles btnRejectGLS.Click
        If txtGLCmt.Text.Trim() <> "" Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.GLCmt = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtGLCmt.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, ConfirmContract.GLS, obj.GLCmt, Nothing, obj)
            LoadNext(obj.GL)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtGLCmt.Focus()
        End If
    End Sub

    Private Sub btnConfirmMGS_Click(sender As Object, e As EventArgs) Handles btnConfirmMGS.Click
        If CheckDieuKien(False, True) Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.MGCmt = txtMGCmt.Text
            obj.MGDate = DateTime.Now
            ConfirmUpdateOutlook(Submit.Confirm, ConfirmContract.MGS, obj.MGCmt, Nothing, obj)
            LoadNext(obj.MG)
        End If
    End Sub

    Private Sub btnRejectMGS_Click(sender As Object, e As EventArgs) Handles btnRejectMGS.Click
        If txtMGCmt.Text.Trim() <> "" Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.MGCmt = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtMGCmt.Text)
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Reject, ConfirmContract.MGS, obj.MGCmt, Nothing, obj)
            LoadNext(obj.MG)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtMGCmt.Focus()
        End If
    End Sub

    Private Sub btnConfirmDMS_Click(sender As Object, e As EventArgs) Handles btnConfirmDMS.Click
        If CheckDieuKien(False, True) Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.DMCmt = txtDMCmt.Text
            obj.DMDate = DateTime.Now
            ConfirmUpdateOutlook(Submit.Confirm, ConfirmContract.DMS, obj.DMCmt, Nothing, obj)
            LoadNext(obj.DM)
        End If
    End Sub

    Private Sub btnRejectDMS_Click(sender As Object, e As EventArgs) Handles btnRejectDMS.Click
        If txtDMCmt.Text.Trim() <> "" Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.DMCmt = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtDMCmt.Text)
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Reject, ConfirmContract.DMS, obj.DMCmt, Nothing, obj)
            LoadNext(obj.DM)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtDMCmt.Focus()
        End If
    End Sub

    Private Sub btnConfirmMGG_Click(sender As Object, e As EventArgs) Handles btnConfirmMGG.Click
        Dim obj As New GA_CT_ReviewContract
        obj.ID_K = _ID
        _db.GetObject(obj)
        obj.GAMGCmt = txtMGGCmt.Text
        obj.GAMGDate = DateTime.Now
        ConfirmUpdateOutlook(Submit.Confirm, ConfirmContract.MGG, obj.GAMGCmt, Nothing, obj)
        LoadNext(obj.GAMG)
    End Sub

    Private Sub btnRejectMGG_Click(sender As Object, e As EventArgs) Handles btnRejectMGG.Click
        If txtMGGCmt.Text.Trim() <> "" Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.GAMGCmt = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtMGGCmt.Text)
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Reject, ConfirmContract.MGG, obj.GAMGCmt, Nothing, obj)
            LoadNext(obj.GAMG)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtMGGCmt.Focus()
        End If
    End Sub

    Private Sub btnConfirmDMG_Click(sender As Object, e As EventArgs) Handles btnConfirmDMG.Click
        Dim obj As New GA_CT_ReviewContract
        obj.ID_K = _ID
        _db.GetObject(obj)
        obj.GADMCmt = txtDMGCmt.Text
        obj.GADMDate = DateTime.Now
        ConfirmUpdateOutlook(Submit.Confirm, ConfirmContract.DMG, obj.DMCmt, Nothing, obj)

        'Cập nhật Status loại hợp đồng
        Dim dt = _db.FillDataTable(String.Format("SELECT EmpID, LoaiHDCu, KQDG
                                                  FROM GA_CT_ReviewContract_Detail
                                                  WHERE ID = '{0}'", _ID))
        For Each r As DataRow In dt.Rows
            Dim col As String = ""
            If r("LoaiHDCu") = "HĐHN" Or r("LoaiHDCu") = "HĐTV" Then
                col = "HD1YearStatus"
            ElseIf r("LoaiHDCu") = "1 Năm" Then
                col = "HD3YearStatus"
            ElseIf r("LoaiHDCu") = "3 Năm" Then
                col = "HDVTHStatus"
            Else
                'case bắt lỗi khi đến Moro
                col = "HDTVStatus"
            End If
            _db.ExecuteNonQuery(String.Format("UPDATE GA_CT_Employee
                                               SET {0} = '{1}'
                                               WHERE EmpID = '{2}'",
                                               col,
                                               r("KQDG"),
                                               r("EmpID")))
        Next
        '-----------------------------

        LoadNext(obj.DM)
    End Sub

    Private Sub btnRejectDMG_Click(sender As Object, e As EventArgs) Handles btnRejectDMG.Click
        If txtDMGCmt.Text.Trim() <> "" Then
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.GADMCmt = String.Format("({0}) {1}", DateTime.Now().ToString("dd/MM/yyyy HH:mm"), txtMGGCmt.Text)
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Reject, ConfirmContract.DMG, obj.GADMCmt, Nothing, obj)
            LoadNext(obj.GADM)
        Else
            ShowWarning("Bạn phải nghi chú lý do từ chối yêu cầu." + vbCrLf + "Please comment detail.")
            txtDMGCmt.Focus()
        End If
    End Sub

    Sub EnableSection(sender As GridView)
        GridControlReadOnly(sender, True)
        sender.Columns("ChatLuongCV").OptionsColumn.ReadOnly = False
        sender.Columns("KienThucCV").OptionsColumn.ReadOnly = False
        sender.Columns("HanhKiem").OptionsColumn.ReadOnly = False
        sender.Columns("QuanHeDongNghiep").OptionsColumn.ReadOnly = False
        sender.Columns("SangKien").OptionsColumn.ReadOnly = False
        sender.Columns("LapKeHoach").OptionsColumn.ReadOnly = False
        sender.Columns("LanhDao").OptionsColumn.ReadOnly = False
        sender.Columns("KQDG").OptionsColumn.ReadOnly = False
        sender.Columns("GhiChu").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(sender)

        gridSucKhoe.AppearanceHeader.BackColor = Color.Empty
        gridViPhamNQCT.AppearanceHeader.BackColor = Color.Empty

        gridChatLuongCV.AppearanceHeader.BackColor = Color.Wheat
        gridKienThucCV.AppearanceHeader.BackColor = Color.Wheat
        gridHanhKiem.AppearanceHeader.BackColor = Color.Wheat
        gridQuanHeDongNghiep.AppearanceHeader.BackColor = Color.Wheat
        gridSangKien.AppearanceHeader.BackColor = Color.Wheat
        gridLapKeHoach.AppearanceHeader.BackColor = Color.Wheat
        gridLanhDao.AppearanceHeader.BackColor = Color.Wheat
        gridKQDGHDLD.AppearanceHeader.BackColor = Color.Wheat
        gridGhiChu.AppearanceHeader.BackColor = Color.Wheat

        btnAddFile.Visible = True
        btnDeleteFile.Visible = True
    End Sub
    Sub EnableGA(sender As GridView)
        GridControlReadOnly(sender, True)
        sender.Columns("SucKhoe").OptionsColumn.ReadOnly = False
        sender.Columns("ViPhamNQCT").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(sender)

        gridSucKhoe.AppearanceHeader.BackColor = Color.Wheat
        gridViPhamNQCT.AppearanceHeader.BackColor = Color.Wheat

        gridChatLuongCV.AppearanceHeader.BackColor = Color.Empty
        gridKienThucCV.AppearanceHeader.BackColor = Color.Empty
        gridHanhKiem.AppearanceHeader.BackColor = Color.Empty
        gridQuanHeDongNghiep.AppearanceHeader.BackColor = Color.Empty
        gridSangKien.AppearanceHeader.BackColor = Color.Empty
        gridLapKeHoach.AppearanceHeader.BackColor = Color.Empty
        gridLanhDao.AppearanceHeader.BackColor = Color.Empty
        gridKQDGHDLD.AppearanceHeader.BackColor = Color.Empty
        gridGhiChu.AppearanceHeader.BackColor = Color.Empty
    End Sub

    Private Sub BandedGridView1_CellValueChanged_1(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles BandedGridView1.CellValueChanged
        If BandedGridView1.Editable And e.Column.ReadOnly = False Then
            Try
                _db.BeginTransaction()
                Dim para(0) As SqlClient.SqlParameter
                para(0) = New SqlClient.SqlParameter("@Value", e.Value)
                Dim sqlUpdate As String = String.Format("UPDATE GA_CT_ReviewContract_Detail
                                                     SET {0} = @Value
                                                     WHERE ID = '{1}'
                                                     AND EmpID = '{2}'",
                                                     e.Column.FieldName,
                                                     txtID.Text,
                                                     BandedGridView1.GetFocusedRowCellValue("EmpID"))
                _db.ExecuteNonQuery(sqlUpdate, para)

                Dim objDetail As New GA_CT_ReviewContract_Detail
                objDetail.ID_K = txtID.Text
                objDetail.EmpID_K = BandedGridView1.GetFocusedRowCellValue("EmpID")
                _db.GetObject(objDetail)
                If e.Column.FieldName = "ChatLuongCV" Or e.Column.FieldName = "KienThucCV" Or
                e.Column.FieldName = "HanhKiem" Or e.Column.FieldName = "QuanHeDongNghiep" Or
                e.Column.FieldName = "SangKien" Or e.Column.FieldName = "LapKeHoach" Or
                e.Column.FieldName = "LanhDao" Then
                    If BandedGridView1.GetFocusedRowCellValue("Observation") = "Operator" Then
                        Dim sqlScalar As String = String.Format("
                        SELECT SUM(ISNULL(ChatLuongCV, 0) + ISNULL(KienThucCV, 0) + ISNULL(HanhKiem, 0) + 
                            ISNULL(QuanHeDongNghiep, 0)) * 1.0 / 4
                        FROM GA_CT_ReviewContract_Detail
                        WHERE ID = '{0}'
                        AND	 EmpID = '{1}'",
                        txtID.Text,
                        BandedGridView1.GetFocusedRowCellValue("EmpID"))
                        Dim DTB As Object = Math.Round(_db.ExecuteScalar(sqlScalar), 1)
                        objDetail.DiemTrungBinh = DTB
                        BandedGridView1.SetFocusedRowCellValue("DiemTrungBinh", DTB)
                        objDetail.KQHTCV = XepLoai(DTB)
                        BandedGridView1.SetFocusedRowCellValue("KQHTCV", objDetail.KQHTCV)

                        '------------------
                        If e.Column.FieldName = "SangKien" Or e.Column.FieldName = "LapKeHoach" Or e.Column.FieldName = "LanhDao" Then
                            If e.Value > 0 Then
                                _db.RollBack()
                                ShowWarning("Không đánh giá nội dung này cho Operator !")
                                '_db.ExecuteNonQuery(String.Format("UPDATE GA_CT_ReviewContract_Detail
                                '                                SET {0} = 0
                                '                                WHERE ID = '{1}'
                                '                                AND	 EmpID = '{2}'",
                                '                                e.Column.FieldName,
                                '                                txtID.Text,
                                '                                BandedGridView1.GetFocusedRowCellValue("EmpID")))
                                'BandedGridView1.SetFocusedRowCellValue(e.Column.FieldName, 0)
                                ReturnOldValue(BandedGridView1)
                                Return
                            End If
                        End If
                    Else
                        Dim sqlScalar As String = String.Format("
                        SELECT SUM(ISNULL(ChatLuongCV, 0) + ISNULL(KienThucCV, 0) + ISNULL(HanhKiem, 0) + ISNULL(QuanHeDongNghiep, 0) + 
                            ISNULL(SangKien, 0) + ISNULL(LapKeHoach, 0) + ISNULL(LanhDao, 0)) * 1.0 / 7
                        FROM GA_CT_ReviewContract_Detail
                        WHERE ID = '{0}'
                        AND	 EmpID = '{1}'",
                        txtID.Text,
                        BandedGridView1.GetFocusedRowCellValue("EmpID"))
                        Dim DTB As Object = Math.Round(_db.ExecuteScalar(sqlScalar), 1)
                        objDetail.DiemTrungBinh = DTB
                        BandedGridView1.SetFocusedRowCellValue("DiemTrungBinh", DTB)
                        objDetail.KQHTCV = XepLoai(DTB)
                        BandedGridView1.SetFocusedRowCellValue("KQHTCV", objDetail.KQHTCV)
                    End If
                End If
                _db.Update(objDetail)
                _db.Commit()
            Catch ex As Exception
                _db.RollBack()
                ShowWarning(ex.Message)
            End Try
        End If
    End Sub
    Function XepLoai(point As Decimal) As String
        If point < 5 Then
            Return "E"
        ElseIf point >= 5 And point < 7 Then
            Return "D"
        ElseIf point >= 7 And point < 8 Then
            Return "C"
        ElseIf point >= 8 And point < 9 Then
            Return "B"
        ElseIf point >= 9 And point <= 10 Then
            Return "A"
        Else
            Return "A"
        End If
    End Function
    Function CheckNoOperator() As Boolean
        For r = 0 To BandedGridView1.RowCount - 1
            If BandedGridView1.GetRowCellValue(r, "Observation") = "Operator" Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub btnAddFile_Click(sender As Object, e As EventArgs) Handles btnAddFile.Click
        Dim frm As New OpenFileDialog
        frm.ShowDialog()
        If frm.FileName <> "" Then
            linkAttach.Text = frm.SafeFileName
            linkAttach.Tag = _path + DateTime.Now.ToString("yyyyMMMdd") + "_" +
                                     CurrentUser.UserID + "_" + _ID + "_" + "ContractLabor" + "_" + frm.SafeFileName
            Dim obj As New GA_CT_ReviewContract
            obj.ID_K = _ID
            _db.GetObject(obj)
            obj.AttachFileName = linkAttach.Text
            obj.AttachFileServer = linkAttach.Tag
            _db.Update(obj)
            File.Copy(frm.FileName, linkAttach.Tag, True)
        End If
    End Sub

    Private Sub btnDeleteFile_Click(sender As Object, e As EventArgs) Handles btnDeleteFile.Click
        If File.Exists(linkAttach.Tag) Then
            File.Delete(linkAttach.Tag)
        End If
        Dim obj As New GA_CT_ReviewContract
        obj.ID_K = _ID
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

    Private Sub TablePanel1_Paint(sender As Object, e As PaintEventArgs) Handles TablePanel1.Paint
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
    Function CheckHas1Year(dtEmp As DataTable) As Boolean
        For Each r As DataRow In dtEmp.Rows
            If r("LoaiHDCu") = "HĐHN" Or r("LoaiHDCu") = "HĐTV" Then
                Return True
            End If
        Next
        Return False
    End Function
    Sub ReturnOldValue(gridV As GridView)
        Dim oldVal As Object = gridV.ActiveEditor.OldEditValue
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = True
        gridV.SetFocusedRowCellValue(gridV.FocusedColumn.FieldName, oldVal)
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = False
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

    End Sub
End Class