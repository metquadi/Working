﻿Imports System.Drawing
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils.Layout
Imports CommonDB
Imports PublicUtility
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FrmRegisterPlannedForNewComers
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Public _id As String = ""

    '--- Vẽ nét liền 2 bảng ---
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
    Private Sub TablePanel2_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles TablePanel2.Paint
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
    '--- Kết thúc vẽ ---
    Public Enum Confirm
        Pre
        Check
        Approve
        PreKQ
        CheckKQ
        ApproveKQ
    End Enum
    Private Sub FrmRegisterPlannedForNewComers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _id = "" Then
            _id = Me.AccessibleName
        End If
        dteDate.EditValue = Date.Now
        txtSection.Text = CurrentUser.Section
        LoadMail()
        LoadHead()
    End Sub
    Sub LoadMail()
        txtMailPre.Text = CurrentUser.Mail
        Dim dtMail = _db.FillDataTable("SELECT h.EmpID, d.SectionSort, d.Observation, h.Mail
                                    FROM OT_Mail AS h
                                    LEFT JOIN OT_Employee AS d
                                    ON h.EmpID = d.EmpID
                                    ORDER BY h.Mail")
        cbbMailCheck.Properties.DataSource = dtMail
        cbbMailCheck.Properties.DisplayMember = "Mail"
        cbbMailCheck.Properties.ValueMember = "Mail"
        cbbMailCheck.Properties.NullText = Nothing
        cbbMailCheck.Properties.PopulateViewColumns()
        cbbMailCheck.Properties.View.Columns("Mail").Width = 200

        cbbMailApprove.Properties.DataSource = dtMail
        cbbMailApprove.Properties.DisplayMember = "Mail"
        cbbMailApprove.Properties.ValueMember = "Mail"
        cbbMailApprove.Properties.NullText = Nothing
        cbbMailApprove.Properties.PopulateViewColumns()
        cbbMailApprove.Properties.View.Columns("Mail").Width = 200

        'cbbMailCheck.Text = CurrentUser.Mail
        'cbbMailApprove.Text = CurrentUser.Mail
        'txtMailPreKQ.Text = CurrentUser.Mail
        'txtMailCheckKQ.Text = CurrentUser.Mail
        'txtMailApproveKQ.Text = CurrentUser.Mail
    End Sub
    Sub UnEnableControl()
        cbbMailCheck.ReadOnly = True
        cbbMailApprove.ReadOnly = True
        btnSave.Visible = False
        btnSubmit.Visible = False
        lblDatePre.Visible = False
        mmeCmtPre.ReadOnly = True

        btnCheck.Visible = False
        btnRejectCheck.Visible = False
        lblDateCheck.Visible = False
        mmeCmtCheck.ReadOnly = True

        btnApprove.Visible = False
        btnRejectApprove.Visible = False
        lblDateApprove.Visible = False
        mmeCmtApprove.ReadOnly = True

        btnSaveKQ.Visible = False
        btnSubmitKQ.Visible = False
        lblDatePreKQ.Visible = False
        mmeCmtPreKQ.ReadOnly = True

        btnCheckKQ.Visible = False
        btnRejectCheckKQ.Visible = False
        lblDateCheckKQ.Visible = False
        mmeCmtCheckKQ.ReadOnly = True

        btnApproveKQ.Visible = False
        btnRejectApproveKQ.Visible = False
        lblDateApproveKQ.Visible = False
        mmeCmtApproveKQ.ReadOnly = True

        btnDeleteProgram.Visible = False
        btnDeleteProgramDetail.Visible = False
        btnDeleteEmployee.Visible = False
    End Sub
    Sub LoadHead()
        UnEnableControl()
        If _id = "" Then
            cbbMailCheck.ReadOnly = False
            cbbMailApprove.ReadOnly = False
            btnSave.Visible = True
            mmeCmtPre.ReadOnly = False
        Else
            txtID.Text = _id
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            txtMailPre.Text = obj.MailPre
            cbbMailCheck.Text = obj.MailCheck
            cbbMailApprove.Text = obj.MailApprove
            txtMailPreKQ.Text = obj.MailPreKQ
            txtMailCheckKQ.Text = obj.MailCheckKQ
            txtMailApproveKQ.Text = obj.MailApproveKQ
            mmeCmtPre.Text = obj.CommentPre
            mmeCmtCheck.Text = obj.CommentCheck
            mmeCmtApprove.Text = obj.CommentApprove
            mmeCmtPreKQ.Text = obj.CommentPreKQ
            mmeCmtCheckKQ.Text = obj.CommentCheckKQ
            mmeCmtApproveKQ.Text = obj.CommentApproveKQ
            txtSection.Text = obj.Section
            dteDate.EditValue = obj.DateSave

            LoadProgramList()
            LoadProgramDetail(DBNull.Value)
            LoadEmployee(DBNull.Value)
            LoadScore()
            LoadResult()

            If obj.DatePre > Date.MinValue Then
                lblDatePre.Text = obj.DatePre.ToString("dd-MMM-yyyy HH:mm")
                lblDatePre.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailPre Then
                    cbbMailCheck.ReadOnly = False
                    cbbMailApprove.ReadOnly = False
                    btnSave.Visible = True
                    btnSubmit.Visible = True
                    mmeCmtPre.ReadOnly = False
                    btnDeleteProgram.Visible = True
                    btnDeleteEmployee.Visible = True
                    btnDeleteProgramDetail.Visible = True
                    EnableProgramList()
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateCheck > Date.MinValue Then
                lblDateCheck.Text = obj.DateCheck.ToString("dd-MMM-yyyy HH:mm")
                lblDateCheck.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailCheck Then
                    cbbMailApprove.ReadOnly = False
                    btnCheck.Visible = True
                    btnRejectCheck.Visible = True
                    mmeCmtCheck.ReadOnly = False
                    btnDeleteProgram.Visible = True
                    btnDeleteEmployee.Visible = True
                    btnDeleteProgramDetail.Visible = True
                    EnableProgramList()
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateApprove > Date.MinValue Then
                lblDateApprove.Text = obj.DateApprove.ToString("dd-MMM-yyyy HH:mm")
                lblDateApprove.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailApprove Then
                    btnApprove.Visible = True
                    btnRejectApprove.Visible = True
                    mmeCmtApprove.ReadOnly = False
                    Return
                End If
            End If

            If obj.DatePreKQ > Date.MinValue Then
                lblDatePreKQ.Text = obj.DatePreKQ.ToString("dd-MMM-yyyy HH:mm")
                lblDatePreKQ.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailPreKQ Then
                    btnSaveKQ.Visible = True
                    btnSubmitKQ.Visible = True
                    mmeCmtPreKQ.ReadOnly = False
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateCheckKQ > Date.MinValue Then
                lblDateCheckKQ.Text = obj.DateCheckKQ.ToString("dd-MMM-yyyy HH:mm")
                lblDateCheckKQ.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailCheckKQ Then
                    btnCheckKQ.Visible = True
                    btnRejectCheckKQ.Visible = True
                    mmeCmtCheckKQ.ReadOnly = False
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateApproveKQ > Date.MinValue Then
                lblDateApproveKQ.Text = obj.DateApproveKQ.ToString("dd-MMM-yyyy HH:mm")
                lblDateApproveKQ.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailApproveKQ Then
                    btnApproveKQ.Visible = True
                    btnRejectApproveKQ.Visible = True
                    mmeCmtApproveKQ.ReadOnly = False
                    Return
                End If
            End If
        End If
    End Sub
    Sub LoadProgramList()
        GridControl1.DataSource = _db.FillDataTable(String.Format(" SELECT IDTrainingCode, TrainingProgram, TrainingMan, 
                                                                        FromDate, ToDate, Duration, Place
                                                                    FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                                                    WHERE ID = '{0}'",
                                                                    _id))
        GridControlSetFormat(GridView1)
        GridView1.Columns("TrainingProgram").Width = 250
        GridView1.Columns("TrainingMan").Width = 150

        '-- Load List Program nếu đã có trong master --
        Dim existVal = _db.ExecuteScalar(String.Format("SELECT TOP 1 TrainingCode
                                                        FROM GA_TRM_ProgramMaster
                                                        WHERE TrainingCode LIKE 'N%'
                                                        AND Section = N'{0}'",
                                                        CurrentUser.Section))
        If existVal IsNot Nothing Then
            slueTrainingList.DataSource = _db.FillDataTable(
                                                String.Format("SELECT TrainingCode, TrainingProgram
                                                FROM GA_TRM_ProgramMaster
                                                WHERE TrainingCode LIKE 'N%'
                                                AND Section = N'{0}'",
                                                CurrentUser.Section))
            slueTrainingList.ValueMember = "TrainingProgram"
            slueTrainingList.DisplayMember = "TrainingProgram"
            slueTrainingList.NullText = Nothing
            GridView1.Columns("TrainingProgram").ColumnEdit = slueTrainingList
        End If

        '-- Load người training --
        slueTrainingMan.DataSource = _db.FillDataTable("SELECT ECode, EName, Observation
                                                        FROM HRM_Emloyee
                                                        WHERE ResignedDate IS NULL")
        slueTrainingMan.ValueMember = "EName"
        slueTrainingMan.DisplayMember = "EName"
        slueTrainingMan.NullText = Nothing
        GridView1.Columns("TrainingMan").ColumnEdit = slueTrainingMan
    End Sub
    Sub EnableProgramList()
        GridControlReadOnly(GridView1, True)
        GridView1.Columns("TrainingProgram").OptionsColumn.ReadOnly = False
        GridView1.Columns("TrainingMan").OptionsColumn.ReadOnly = False
        GridView1.Columns("FromDate").OptionsColumn.ReadOnly = False
        GridView1.Columns("ToDate").OptionsColumn.ReadOnly = False
        GridView1.Columns("Duration").OptionsColumn.ReadOnly = False
        GridView1.Columns("Place").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
        GridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
    End Sub
    Sub LoadProgramDetail(trainingCode)
        Dim para(0) As SqlClient.SqlParameter
        If trainingCode Is Nothing Then
            para(0) = New SqlClient.SqlParameter("@TrainingCode", DBNull.Value)
        Else
            para(0) = New SqlClient.SqlParameter("@TrainingCode", trainingCode)
        End If
        GridControl2.DataSource = _db.FillDataTable(String.Format(" SELECT IDTrainingCode, ProgramDetailID, ProgramDetailName,
                                                                        Duration, Trainer, TestMethod, Venue, Remark
                                                                    FROM GA_TRM_RegisterPlannedNewComer_ProgramDetail
                                                                    WHERE ID = '{0}'
                                                                    AND (@TrainingCode IS NULL OR IDTrainingCode = @TrainingCode)
                                                                    ORDER BY IDTrainingCode, ProgramDetailID",
                                                                    _id),
                                                                    para)
        GridControlSetFormat(GridView2)
        GridView2.Columns("ProgramDetailID").Width = 100
        GridView2.Columns("ProgramDetailName").Width = 300
        GridView2.Columns("ProgramDetailName").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        GridView2.Columns("Duration").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        GridView2.Columns("Trainer").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        GridView2.Columns("TestMethod").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        GridView2.Columns("Venue").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        GridView2.Columns("Remark").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
    End Sub
    Sub EnableProgramDetail()
        GridControlReadOnly(GridView2, True)
        GridView2.Columns("ProgramDetailName").OptionsColumn.ReadOnly = False
        GridView2.Columns("Duration").OptionsColumn.ReadOnly = False
        GridView2.Columns("Trainer").OptionsColumn.ReadOnly = False
        GridView2.Columns("TestMethod").OptionsColumn.ReadOnly = False
        GridView2.Columns("Venue").OptionsColumn.ReadOnly = False
        GridView2.Columns("Remark").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView2)

        Dim existVal = _db.ExecuteScalar(String.Format("SELECT TOP 1 TrainingCode
                                                        FROM GA_TRM_ProgramMaster
                                                        WHERE TrainingCode LIKE 'N%'
                                                        AND Section = N'{0}'",
                                                        CurrentUser.Section))
        If existVal Is Nothing Then
            GridView2.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        End If
    End Sub
    Sub LoadEmployee(trainingCode)
        Dim para(1) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@Section", DBNull.Value)
        If trainingCode Is Nothing Then
            para(1) = New SqlClient.SqlParameter("@TrainingCode", DBNull.Value)
        Else
            para(1) = New SqlClient.SqlParameter("@TrainingCode", trainingCode)
        End If
        GridControl3.DataSource = _db.FillDataTable(String.Format(" SELECT h.IDTrainingCode, g.TrainingProgram, h.EmpID, d.EmpName
                                                                    FROM GA_TRM_RegisterPlannedNewComer_Employee AS h
                                                                    LEFT JOIN OT_Employee AS d
                                                                    ON h.EmpID = d.EmpID
                                                                    LEFT JOIN (
                                                                        SELECT IDTrainingCode, TrainingProgram
                                                                        FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                                                        WHERE ID = '{0}'
                                                                        GROUP BY IDTrainingCode, TrainingProgram
                                                                    ) AS g
                                                                    ON h.IDTrainingCode = g.IDTrainingCode
                                                                    WHERE h.ID = '{0}'
                                                                    AND (@TrainingCode IS NULL OR h.IDTrainingCode = @TrainingCode)
                                                                    ORDER BY h.IDTrainingCode, h.EmpID",
                                                                    _id),
                                                                    para)
        GridControlSetFormat(GridView3)
        GridView3.Columns("TrainingProgram").Width = 250
        GridView3.Columns("EmpName").Width = 150
        GridView3.Columns("EmpID").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False

        Dim obj As New GA_TRM_MailGAReceiveInformation
        obj.ID_K = "1"
        _db.GetObject(obj)
        If CurrentUser.Mail <> obj.MailPIC Then
            para(0) = New SqlClient.SqlParameter("@Section", CurrentUser.Section)
        End If
        slueEmployee.DataSource = _db.FillDataTable("SELECT ECode, EName, Section, Observation
                                                    FROM HRM_Emloyee
                                                    WHERE ResignedDate IS NULL
                                                    AND (@Section IS NULL OR Section = @Section)", para)
        slueEmployee.DisplayMember = "ECode"
        slueEmployee.ValueMember = "ECode"
        slueEmployee.NullText = Nothing
        GridView3.Columns("EmpID").ColumnEdit = slueEmployee
    End Sub
    Sub EnableEmployee()
        GridControlReadOnly(GridView3, True)
        GridView3.Columns("EmpID").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView3)
        GridView3.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
    End Sub
    Sub LoadScore()
        GridView4.Columns.Clear()
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@id", _id)
        Dim dtExample = _db.ExecuteStoreProcedureTB("sp_GA_TRM_NewComer_Score", para)
        If dtExample.Rows.Count = 0 And dtExample.Columns("EmpID") Is Nothing Then
            dtExample.Columns.Add("EmpID")
            dtExample.Columns.Add("EmpName")
            dtExample.Columns.Add("Observation")
            dtExample.Columns.Add("StartDate")
            dtExample.Columns.Add("LevelOfCompetency")
        End If
        GridControl4.DataSource = dtExample
        GridControlSetFormat(GridView4, 4)
        GridView4.Columns("EmpName").Width = 150
        GridView4.Columns("LevelOfCompetency").Width = 80
        For Each c As GridColumn In GridView4.Columns
            If c.VisibleIndex > GridView4.Columns("LevelOfCompetency").VisibleIndex Then
                GridView4.Columns(c.FieldName).OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
                GridView4.Columns(c.FieldName).ColumnEdit = cbbScore14
            End If
        Next
        GridView4.BestFitColumns()
    End Sub
    Sub EnableScore()
        GridControlReadOnly(GridView4, False)
        GridView4.Columns("EmpID").OptionsColumn.ReadOnly = True
        GridView4.Columns("EmpName").OptionsColumn.ReadOnly = True
        GridView4.Columns("Observation").OptionsColumn.ReadOnly = True
        GridView4.Columns("StartDate").OptionsColumn.ReadOnly = True
        GridView4.Columns("LevelOfCompetency").OptionsColumn.ReadOnly = True
        GridControlSetColorEdit(GridView4)
    End Sub
    Sub LoadResult()
        GridControl5.DataSource = _db.FillDataTable(String.Format(" SELECT h.IDTrainingCode, g.TrainingProgram, h.EmpID, d.EName, 
                                                                    h.Level1ReactionEvaluate, h.Level2LearningPoint, 
                                                                    h.Level2LearningEvaluate, h.Level3TransferEvaluate, h.Remark
                                                                    FROM GA_TRM_RegisterPlannedNewComer_Employee AS h
                                                                    LEFT JOIN HRM_Emloyee AS d
                                                                    ON h.EmpID = d.ECode
                                                                    LEFT JOIN (
                                                                        SELECT IDTrainingCode, TrainingProgram
                                                                        FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                                                        WHERE ID = '{0}'
                                                                        GROUP BY IDTrainingCode, TrainingProgram
                                                                    ) AS g
                                                                    ON h.IDTrainingCode = g.IDTrainingCode
                                                                    WHERE ID = '{0}'
                                                                    --AND h.IDTrainingCode LIKE 'D%'
                                                                    ORDER BY h.IDTrainingCode, h.EmpID",
                                                                    _id))
        GridControlSetFormat(BandedGridView1, 1)
        BandedGridView1.Columns("TrainingProgram").Width = 350
        BandedGridView1.Columns("EName").Width = 200
        BandedGridView1.Columns("Remark").Width = 200
        BandedGridView1.Columns("Level1ReactionEvaluate").ColumnEdit = cbbYesNo
        BandedGridView1.Columns("Level2LearningEvaluate").ColumnEdit = cbbOkNg
        BandedGridView1.Columns("Level3TransferEvaluate").ColumnEdit = cbbOkNg
        BandedGridView1.Columns("Level1ReactionEvaluate").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        BandedGridView1.Columns("Level2LearningPoint").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        BandedGridView1.Columns("Level2LearningEvaluate").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        BandedGridView1.Columns("Level3TransferEvaluate").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        BandedGridView1.Columns("Remark").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        BandedGridView1.Columns("Level2LearningPoint").ColumnEdit = cbbScore13
    End Sub
    Sub EnableResult()
        GridControlReadOnly(BandedGridView1, True)
        BandedGridView1.Columns("Level1ReactionEvaluate").OptionsColumn.ReadOnly = False
        BandedGridView1.Columns("Level2LearningEvaluate").OptionsColumn.ReadOnly = False
        BandedGridView1.Columns("Level2LearningPoint").OptionsColumn.ReadOnly = False
        BandedGridView1.Columns("Level3TransferEvaluate").OptionsColumn.ReadOnly = False
        BandedGridView1.Columns("Remark").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(BandedGridView1)
    End Sub
    Sub ReturnOldValue(gridV As GridView)
        Dim oldVal As Object = gridV.ActiveEditor.OldEditValue
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = True
        gridV.SetFocusedRowCellValue(gridV.FocusedColumn.FieldName, oldVal)
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = False
    End Sub
    Function CreateNewID() As String
        Dim valMax = _db.ExecuteScalar(String.Format("SELECT ISNULL(RIGHT(MAX(ID), 2), 0)
                                                      FROM GA_TRM_RegisterPlannedNewComer
                                                      WHERE ID LIKE '%{0}%'",
                                                      Date.Now.ToString("yyMM")))
        valMax = (Integer.Parse(valMax) + 1).ToString.PadLeft(2, "0")
        Return "TRMN_" + Date.Now.ToString("yyMM") + "_" + valMax
    End Function
    Function CheckData(isProgram As Boolean, isProgramDetail As Boolean, isEmployee As Boolean, isScore As Boolean, isResult As Boolean) As Boolean
        If txtMailPre.Text = "" Or cbbMailCheck.Text = "" Or cbbMailApprove.Text = "" Or
            txtMailPreKQ.Text = "" Or txtMailCheckKQ.Text = "" Or txtMailApproveKQ.Text = "" Then
            ShowWarning("Địa chỉ mail không được để trống !")
            cbbMailCheck.Select()
            Return False
        End If

        If isProgram Then
            Dim valProg = _db.ExecuteScalar(String.Format(" SELECT TOP 1 *
                                                        FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                                        WHERE ID = '{0}'",
                                                        _id))
            If valProg Is Nothing Then
                ShowWarning("Chưa có hạng mục đào tạo nào được thêm !")
                tabF065.Show()
                Return False
            End If
        End If

        If isProgramDetail Then
            Dim valDetail = _db.ExecuteScalar(String.Format("SELECT TOP 1 *
                                                        FROM GA_TRM_RegisterPlannedNewComer_ProgramDetail
                                                        WHERE ID = '{0}'",
                                                        _id))
            If valDetail Is Nothing Then
                ShowWarning("Một số hạng mục đào tạo chưa có chi tiết !")
                tabF065.Show()
                tabProgramDetail.Show()
                Return False
            End If
        End If

        If isEmployee Then
            Dim valEmp = _db.ExecuteScalar(String.Format("SELECT TOP 1 *
                                                    FROM GA_TRM_RegisterPlannedNewComer_Employee
                                                    WHERE ID = '{0}'",
                                                    _id))
            If valEmp Is Nothing Then
                ShowWarning("Một số hạng mục đào tạo chưa được thêm nhân viên !")
                tabF065.Show()
                tabEmployee.Show()
                Return False
            End If
        End If

        If isScore Then
            Dim valScore = _db.ExecuteScalar(String.Format("SELECT TOP 1 *
                                                        FROM GA_TRM_RegisterPlannedNewComer_Employee_Score
                                                        WHERE ID = '{0}'
                                                        AND Score > 0",
                                                        _id))
            If valScore Is Nothing Then
                ShowWarning("Bạn chưa xác định năng lực và nhu cầu đào tạo của một số nhân viên !")
                tabF027.Show()
                Return False
            End If
        End If

        If isResult Then
            Dim valResult = _db.ExecuteScalar(String.Format("SELECT TOP 1 *
                                                        FROM GA_TRM_RegisterPlannedNewComer_Employee
                                                        WHERE ID = '{0}'
                                                        AND Level3TransferEvaluate <> ''",
                                                        _id))
            If valResult Is Nothing Then
                ShowWarning("Bạn chưa xác nhận hiệu quả của một số nhân viên !")
                tabF030.Show()
                Return False
            End If
        End If

        Return True
    End Function
    Sub SaveData()
        Dim obj As New GA_TRM_RegisterPlannedNewComer
        If _id = "" Then
            _id = CreateNewID()
            txtID.Text = _id
            obj.ID_K = _id
            obj.CurrentMail = CurrentUser.Mail
        Else
            obj.ID_K = _id
            _db.GetObject(obj)
        End If
        obj.DateSave = dteDate.DateTime.Date
        obj.Section = txtSection.Text
        obj.MailPre = txtMailPre.Text
        obj.MailCheck = cbbMailCheck.Text
        obj.MailApprove = cbbMailApprove.Text
        obj.MailPreKQ = txtMailPreKQ.Text
        obj.MailCheckKQ = txtMailCheckKQ.Text
        obj.MailApproveKQ = txtMailApproveKQ.Text
        obj.CommentPre = mmeCmtPre.Text
        obj.CommentCheck = mmeCmtCheck.Text
        obj.CommentApprove = mmeCmtApprove.Text
        obj.CommentPreKQ = mmeCmtPreKQ.Text
        obj.CommentCheckKQ = mmeCmtCheckKQ.Text
        obj.CommentApproveKQ = mmeCmtApproveKQ.Text
        If _db.ExistObject(obj) Then
            _db.Update(obj)
        Else
            obj.CreateUser = CurrentUser.UserID
            obj.CreateDate = DateTime.Now
            _db.Insert(obj)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If CheckData(False, False, False, False, False) Then
            SaveData()
            LoadHead()
            ShowSuccess()
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If CheckData(True, True, True, False, False) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DatePre = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Pre, obj)
        End If
    End Sub
    Sub ConfirmUpdateOutlook(submitPara As Submit, confirmPara As Confirm, obj As GA_TRM_RegisterPlannedNewComer)
        Try
            _db.BeginTransaction()
            Dim listTo As New List(Of String)
            Dim listCC As New List(Of String)
            Dim listBCC As New List(Of String)
            Dim arrCC() As String = Nothing
            Dim titleMail As String = ""

            If submitPara = Submit.Reject Then
                titleMail = "Reject - Register Training Management For New Comers - " + obj.Section + " - " + obj.DateSave.ToString("dd-MMM-yyyy")
                If obj.DatePreKQ = Nothing Then
                    obj.DatePre = Nothing
                    obj.DateCheck = Nothing
                    obj.DateApprove = Nothing
                    obj.CurrentMail = obj.MailPre
                    listTo.Add(obj.MailPre)
                Else
                    obj.DatePreKQ = Nothing
                    obj.DateCheckKQ = Nothing
                    obj.DateApproveKQ = Nothing
                    obj.CurrentMail = obj.MailPreKQ
                    listTo.Add(obj.MailPreKQ)
                End If
                SendMailOutlook(titleMail, Nothing, submitPara, listTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
            Else
                titleMail = "Register Training Management For New Comers - " + obj.Section + " - " + obj.DateSave.ToString("dd-MMM-yyyy")
                Dim objMailGA As New GA_TRM_MailGAReceiveInformation
                objMailGA.ID_K = "1"
                _db.GetObject(objMailGA)
                Select Case confirmPara
                    Case Confirm.Pre
                        If obj.MailCheck <> "" Then
                            listTo.Add(obj.MailCheck)
                            SendMailOutlook(titleMail, Nothing, submitPara, listTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailCheck
                            GoTo EndConfirm
                        End If
                        GoTo Check
                    Case Confirm.Check
Check:
                        If obj.MailApprove <> "" Then
                            listTo.Add(obj.MailApprove)
                            SendMailOutlook(titleMail, Nothing, submitPara, listTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailApprove
                            GoTo EndConfirm
                        End If
                        GoTo Approve
                    Case Confirm.Approve
Approve:
                        If obj.MailPreKQ <> "" Then
                            listTo.Add(obj.MailPreKQ)
                            listCC.Add(objMailGA.MailPIC)
                            'listCC.Add(objMailGA.MailApproved) Vân GA
                            SendMailOutlook(titleMail, Nothing, submitPara, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailPreKQ
                            GoTo EndConfirm
                        End If
                        GoTo PreKQ
                    Case Confirm.PreKQ
PreKQ:
                        If obj.MailCheckKQ <> "" Then
                            listTo.Add(obj.MailCheckKQ)
                            SendMailOutlook(titleMail, Nothing, submitPara, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailCheckKQ
                            GoTo EndConfirm
                        End If
                        GoTo CheckKQ
                    Case Confirm.CheckKQ
CheckKQ:
                        If obj.MailApproveKQ <> "" Then
                            listTo.Add(obj.MailApproveKQ)
                            SendMailOutlook(titleMail, Nothing, submitPara, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailApproveKQ
                            GoTo EndConfirm
                        End If
                        GoTo ApproveKQ
                    Case Confirm.ApproveKQ
ApproveKQ:
                        If obj.MailPre <> "" Then
                            listTo.Add(objMailGA.MailPIC)
                            'listCC.Add(objMailGA.MailApproved) Vân GA
                            listCC.Add(obj.MailPre)
                            SendMailOutlook(titleMail, Nothing, submitPara, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
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
            ShowWarning(ex.Message)
        End Try
    End Sub
    Sub NextRequest()
        Dim idNext As Object = _db.ExecuteScalar(String.Format("SELECT ID
                                                                FROM GA_TRM_RegisterPlannedNewComer
                                                                WHERE CurrentMail = '{0}'
                                                                ORDER BY ID",
                                                                CurrentUser.Mail))
        If idNext IsNot Nothing Then
            _id = idNext
        End If
        LoadHead()
    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        If CheckData(True, True, True, False, False) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateCheck = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Check, obj)
        End If
    End Sub

    Private Sub btnRejectCheck_Click(sender As Object, e As EventArgs) Handles btnRejectCheck.Click
        If mmeCmtCheck.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentCheck = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtCheck.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.Check, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtCheck.Select()
        End If
    End Sub

    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        If CheckData(True, True, True, False, False) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateApprove = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Approve, obj)
        End If
    End Sub

    Private Sub btnRejectApprove_Click(sender As Object, e As EventArgs) Handles btnRejectApprove.Click
        If mmeCmtApprove.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentApprove = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtApprove.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.Approve, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtApprove.Select()
        End If
    End Sub

    Private Sub btnSaveKQ_Click(sender As Object, e As EventArgs) Handles btnSaveKQ.Click
        If CheckData(True, True, True, True, True) Then
            SaveData()
            LoadHead()
            ShowSuccess()
        End If
    End Sub

    Private Sub btnSubmitKQ_Click(sender As Object, e As EventArgs) Handles btnSubmitKQ.Click
        If CheckData(True, True, True, True, True) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DatePreKQ = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.PreKQ, obj)
        End If
    End Sub

    Private Sub btnCheckKQ_Click(sender As Object, e As EventArgs) Handles btnCheckKQ.Click
        If CheckData(True, True, True, True, True) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateCheckKQ = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.CheckKQ, obj)
        End If
    End Sub

    Private Sub btnRejectCheckKQ_Click(sender As Object, e As EventArgs) Handles btnRejectCheckKQ.Click
        If mmeCmtCheckKQ.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentCheckKQ = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtCheckKQ.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.CheckKQ, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtCheckKQ.Select()
        End If
    End Sub

    Private Sub btnApproveKQ_Click(sender As Object, e As EventArgs) Handles btnApproveKQ.Click
        If CheckData(True, True, True, True, True) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateApproveKQ = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Info, Confirm.ApproveKQ, obj)
        End If
    End Sub

    Private Sub btnRejectApproveKQ_Click(sender As Object, e As EventArgs) Handles btnRejectApproveKQ.Click
        If mmeCmtApproveKQ.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentApproveKQ = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtApproveKQ.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.ApproveKQ, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtApproveKQ.Select()
        End If
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            If e.RowHandle < 0 Then
                If IsDBNull(GridView1.GetFocusedRowCellValue("IDTrainingCode")) Then
                    If Not IsDBNull(GridView1.GetFocusedRowCellValue("TrainingProgram")) Then
                        Dim obj As New GA_TRM_RegisterPlannedNewComer_ProgramList
                        obj.ID_K = _id
                        obj.IDTrainingCode_K = CreateNewIDProg()
                        obj.TrainingProgram = GridView1.GetFocusedRowCellValue("TrainingProgram")
                        _db.Insert(obj)
                        GridView1.SetFocusedRowCellValue("IDTrainingCode", obj.IDTrainingCode_K)
                        GoTo Load
                    Else
                        ShowWarning("Bạn phải nhập tên chương trình đào tạo trước (Training Program) !")
                        ReturnOldValue(GridView1)
                    End If
                End If
            End If
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlannedNewComer_ProgramList
                                                SET {0} = @Value
                                                WHERE ID = '{1}'
                                                AND IDTrainingCode = '{2}'",
                                                e.Column.FieldName,
                                                _id,
                                                GridView1.GetFocusedRowCellValue("IDTrainingCode")),
                                                para)
            If e.Column.FieldName = "TrainingProgram" Then
Load:
                LoadProgramDetail(DBNull.Value)
                LoadEmployee(DBNull.Value)
                LoadScore()
                LoadResult()
                EnableScore()
                EnableResult()
            End If
        End If
    End Sub
    Function CreateNewIDProg() As String
        Dim valMax = _db.ExecuteScalar("SELECT ISNULL(RIGHT(MAX(IDTrainingCode), 9), 0)
                                        FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                        WHERE ISNUMERIC(RIGHT(IDTrainingCode, 9)) = 1")
        If IsNumeric(valMax) Then
            valMax = (Integer.Parse(valMax) + 1).ToString.PadLeft(9, "0")
        Else
            valMax = "000000001"
        End If
        Return "N" + valMax 'N000000001
    End Function

    Private Sub btnDeleteProgram_Click(sender As Object, e As EventArgs) Handles btnDeleteProgram.Click
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer_ProgramList
            obj.ID_K = _id
            obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
            _db.Delete(obj)
            GridView1.DeleteSelectedRows()
            LoadProgramDetail(DBNull.Value)
            LoadEmployee(DBNull.Value)
            LoadScore()
            LoadResult()
            EnableScore()
            EnableResult()
        End If
    End Sub

    Private Sub GridView1_RowClick(sender As Object, e As RowClickEventArgs) Handles GridView1.RowClick
        LoadProgramDetail(GridView1.GetFocusedRowCellValue("IDTrainingCode"))
        LoadEmployee(GridView1.GetFocusedRowCellValue("IDTrainingCode"))
        Dim obj As New GA_TRM_RegisterPlannedNewComer
        obj.ID_K = _id
        _db.GetObject(obj)
        If CurrentUser.Mail = obj.CurrentMail And obj.DateCheck = Date.MinValue Then
            EnableProgramDetail()
            EnableEmployee()
        End If
    End Sub

    Private Sub GridView2_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView2.CellValueChanged
        If GridView2.Editable And e.Column.ReadOnly = False Then
            If e.RowHandle < 0 Then
                If IsDBNull(GridView2.GetFocusedRowCellValue("ProgramDetailID")) Then
                    If Not IsDBNull(GridView2.GetFocusedRowCellValue("ProgramDetailName")) Then
                        Dim obj As New GA_TRM_RegisterPlannedNewComer_ProgramDetail
                        obj.ID_K = _id
                        obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
                        obj.ProgramDetailID_K = CreateNewIDProgDetail()
                        obj.ProgramDetailName = GridView2.GetFocusedRowCellValue("ProgramDetailName")
                        _db.Insert(obj)
                        GridView2.SetFocusedRowCellValue("IDTrainingCode", obj.IDTrainingCode_K)
                        GridView2.SetFocusedRowCellValue("ProgramDetailID", obj.ProgramDetailID_K)
                    Else
                        ShowWarning("Bạn phải nhập tên chương trình đào tạo trước (Program Detail Name) !")
                        ReturnOldValue(GridView2)
                    End If
                End If
            End If
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlannedNewComer_ProgramDetail
                                                SET {0} = @Value
                                                WHERE ID = '{1}'
                                                AND IDTrainingCode = '{2}'
                                                AND ProgramDetailID = '{3}'",
                                                e.Column.FieldName,
                                                _id,
                                                GridView2.GetFocusedRowCellValue("IDTrainingCode"),
                                                GridView2.GetFocusedRowCellValue("ProgramDetailID")),
                                                para)
        End If
    End Sub
    Function CreateNewIDProgDetail() As String
        Dim trainingCode = GridView1.GetFocusedRowCellValue("IDTrainingCode")
        Dim valMax = _db.ExecuteScalar("SELECT ISNULL(RIGHT(MAX(ProgramDetailID), 10), 0)
                                        FROM GA_TRM_RegisterPlannedNewComer_ProgramDetail
                                        WHERE ISNUMERIC(RIGHT(ProgramDetailID, 10)) = 1")
        valMax = (Integer.Parse(valMax) + 1).ToString.PadLeft(10, "0")
        Return "D" + valMax 'D0000000001
    End Function

    Private Sub slueTrainingList_EditValueChanged(sender As Object, e As EventArgs) Handles slueTrainingList.EditValueChanged
        Dim lc As SearchLookUpEdit = CType(sender, SearchLookUpEdit)
        Dim obj As New GA_TRM_RegisterPlannedNewComer_ProgramList
        obj.ID_K = _id
        obj.IDTrainingCode_K = lc.Properties.View.GetFocusedRowCellValue("TrainingCode")
        If _db.ExistObject(obj) Then
            ReturnOldValue(GridView1)
            ShowWarning("Hạng mục đào tạo này đã được chọn trước đó !")
        Else
            obj.CreateUser = CurrentUser.UserID
            obj.CreateDate = DateTime.Now
            _db.Insert(obj)
            GridView1.SetFocusedRowCellValue("IDTrainingCode", obj.IDTrainingCode_K)
            AutoInsertDetail(obj.IDTrainingCode_K)
            LoadProgramDetail(obj.IDTrainingCode_K)
        End If
    End Sub
    Sub AutoInsertDetail(trainingCode)
        Dim dtDetail = _db.FillDataTable(String.Format("SELECT TrainingCodeDetail, TrainingProgramDetail
                                                        FROM GA_TRM_ProgramMaster_Detail
                                                        WHERE TrainingCode = '{0}'",
                                                        trainingCode))
        For Each r As DataRow In dtDetail.Rows
            Dim obj As New GA_TRM_RegisterPlannedNewComer_ProgramDetail
            obj.ID_K = _id
            obj.IDTrainingCode_K = trainingCode
            obj.ProgramDetailID_K = r("TrainingCodeDetail")
            obj.ProgramDetailName = r("TrainingProgramDetail")
            obj.CreateUser = CurrentUser.UserID
            obj.CreateDate = DateTime.Now
            If Not _db.ExistObject(obj) Then
                _db.Insert(obj)
            End If
        Next
    End Sub

    Private Sub btnShowAllProgramDetail_Click(sender As Object, e As EventArgs) Handles btnShowAllProgramDetail.Click
        LoadProgramDetail(DBNull.Value)
    End Sub

    Private Sub btnDeleteProgramDetail_Click(sender As Object, e As EventArgs) Handles btnDeleteProgramDetail.Click
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer_ProgramDetail
            obj.ID_K = _id
            obj.IDTrainingCode_K = GridView2.GetFocusedRowCellValue("IDTrainingCode")
            obj.ProgramDetailID_K = GridView2.GetFocusedRowCellValue("ProgramDetailID")
            _db.Delete(obj)
            GridView2.DeleteSelectedRows()
        End If
    End Sub

    Private Sub slueEmployee_EditValueChanged(sender As Object, e As EventArgs) Handles slueEmployee.EditValueChanged
        Dim lc As SearchLookUpEdit = CType(sender, SearchLookUpEdit)
        Dim obj As New GA_TRM_RegisterPlannedNewComer_Employee
        obj.ID_K = _id
        obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
        obj.EmpID_K = lc.Properties.View.GetFocusedRowCellValue("ECode")
        If _db.ExistObject(obj) Then
            ReturnOldValue(GridView3)
            ShowWarning("Nhân viên này đã được chọn trước đó !")
        Else
            GridView3.SetFocusedRowCellValue("EmpName", lc.Properties.View.GetFocusedRowCellValue("EName"))
        End If
    End Sub

    Private Sub GridView3_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView3.CellValueChanged
        If GridView3.Editable And e.Column.ReadOnly = False Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer_Employee
            obj.ID_K = _id
            If e.RowHandle < 0 Then
                If IsDBNull(GridView3.GetFocusedRowCellValue("IDTrainingCode")) Then
                    obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
                    obj.EmpID_K = e.Value
                    If Not _db.ExistObject(obj) Then
                        _db.Insert(obj)
                        GridView3.SetFocusedRowCellValue("IDTrainingCode", obj.IDTrainingCode_K)

                        Dim dObj As New GA_TRM_RegisterPlannedNewComer_Employee_Score
                        dObj.ID_K = _id
                        dObj.IDTrainingCode_K = obj.IDTrainingCode_K
                        dObj.EmpID_K = obj.EmpID_K
                        dObj.LevelOfCompetency_K = "Current"
                        If Not _db.ExistObject(dObj) Then
                            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlannedNewComer_Employee_Score 
                                                                (ID, IDTrainingCode, EmpID, LevelOfCompetency)
                                                                VALUES ('{0}', '{1}', '{2}', '{3}')",
                                                                _id,
                                                                dObj.IDTrainingCode_K,
                                                                dObj.EmpID_K,
                                                                dObj.LevelOfCompetency_K))
                        End If
                        dObj.LevelOfCompetency_K = "Plan"
                        If Not _db.ExistObject(dObj) Then
                            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlannedNewComer_Employee_Score 
                                                                (ID, IDTrainingCode, EmpID, LevelOfCompetency)
                                                                VALUES ('{0}', '{1}', '{2}', '{3}')",
                                                                _id,
                                                                dObj.IDTrainingCode_K,
                                                                dObj.EmpID_K,
                                                                dObj.LevelOfCompetency_K))
                        End If
                        dObj.LevelOfCompetency_K = "Result"
                        If Not _db.ExistObject(dObj) Then
                            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlannedNewComer_Employee_Score 
                                                                (ID, IDTrainingCode, EmpID, LevelOfCompetency)
                                                                VALUES ('{0}', '{1}', '{2}', '{3}')",
                                                                _id,
                                                                dObj.IDTrainingCode_K,
                                                                dObj.EmpID_K,
                                                                dObj.LevelOfCompetency_K))
                        End If
                    Else
                        ShowWarning("Nhân viên này đã được chọn trước đó !")
                    End If
                End If
            End If
            _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlannedNewComer_Employee
                                                SET EmpID = '{0}'
                                                WHERE ID = '{1}'
                                                AND IDTrainingCode = '{2}'
                                                AND EmpID = '{3}'",
                                                e.Value,
                                                _id,
                                                GridView3.GetFocusedRowCellValue("IDTrainingCode"),
                                                GridView3.ActiveEditor.OldEditValue))
            LoadScore()
            EnableScore()
            LoadResult()
            EnableResult()
        End If
    End Sub

    Private Sub btnShowAllEmployee_Click(sender As Object, e As EventArgs) Handles btnShowAllEmployee.Click
        LoadEmployee(DBNull.Value)
    End Sub

    Private Sub btnDeleteEmployee_Click(sender As Object, e As EventArgs) Handles btnDeleteEmployee.Click
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            Dim obj As New GA_TRM_RegisterPlannedNewComer_Employee
            obj.ID_K = _id
            obj.IDTrainingCode_K = GridView3.GetFocusedRowCellValue("IDTrainingCode")
            obj.EmpID_K = GridView3.GetFocusedRowCellValue("EmpID")
            _db.Delete(obj)
            GridView3.DeleteSelectedRows()
            LoadScore()
            EnableScore()
            LoadResult()
            EnableResult()
        End If
    End Sub

    Private Sub GridView4_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView4.CellValueChanged
        If GridView4.Editable And e.Column.ReadOnly = False Then
            '---------
            Dim hObj As New GA_TRM_RegisterPlannedNewComer
            hObj.ID_K = _id
            _db.GetObject(hObj)
            If GridView4.GetFocusedRowCellValue("LevelOfCompetency") = "Plan" Or
                GridView4.GetFocusedRowCellValue("LevelOfCompetency") = "Current" Then
                If hObj.DateApprove > Date.MinValue Then
                    ShowWarning("Kế hoạch đã được Approved, không thể sửa !")
                    ReturnOldValue(GridView4)
                    Return
                End If
            ElseIf GridView4.GetFocusedRowCellValue("LevelOfCompetency") = "Result" Then
                If hObj.DateApprove = Date.MinValue Then
                    ShowWarning("Kế hoạch chưa được Approved nên chưa được nhập kết quả !")
                    ReturnOldValue(GridView4)
                    Return
                End If
            End If
            '---------
            Dim obj As New GA_TRM_RegisterPlannedNewComer_Employee_Score
            obj.ID_K = _id
            obj.IDTrainingCode_K = GetTrainingCode(e.Column.FieldName)
            obj.EmpID_K = GridView4.GetFocusedRowCellValue("EmpID")
            obj.LevelOfCompetency_K = GridView4.GetFocusedRowCellValue("LevelOfCompetency")
            If _db.ExistObject(obj) Then
                Dim para(0) As SqlClient.SqlParameter
                para(0) = New SqlClient.SqlParameter("@Value", e.Value)
                _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlannedNewComer_Employee_Score
                                                    SET Score = @Value
                                                    WHERE ID = '{0}'
                                                    AND IDTrainingCode = '{1}'
                                                    AND EmpID = '{2}'
                                                    AND LevelOfCompetency = '{3}'",
                                                    _id,
                                                    obj.IDTrainingCode_K,
                                                    obj.EmpID_K,
                                                    obj.LevelOfCompetency_K), para)
            Else
                ShowWarning("Nhân viên không đăng kí hạng mục này !")
                ReturnOldValue(GridView4)
            End If
        End If
    End Sub
    Function GetTrainingCode(trainingProg) As String
        Dim codeID As Object = _db.ExecuteScalar(String.Format("SELECT IDTrainingCode
                                                                FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                                                WHERE ID = '{0}'
                                                                AND TrainingProgram LIKE N'%{1}%'",
                                                                _id,
                                                                trainingProg))
        Return codeID
    End Function

    Private Sub BandedGridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles BandedGridView1.CellValueChanged
        If BandedGridView1.Editable And e.Column.ReadOnly = False Then
            '---------
            Dim hObj As New GA_TRM_RegisterPlannedNewComer
            hObj.ID_K = _id
            _db.GetObject(hObj)
            If e.Column.FieldName = "Level3TransferEvaluate" Then
                If hObj.DateApprove = Date.MinValue Then
                    ShowWarning("Kế hoạch chưa được Approved nên chưa được đánh giá !")
                    ReturnOldValue(BandedGridView1)
                    Return
                End If
            End If
            '---------
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlannedNewComer_Employee
                                                SET {0} = @Value
                                                WHERE ID = '{1}'
                                                AND IDTrainingCode = '{2}'
                                                AND EmpID = '{3}'",
                                                e.Column.FieldName,
                                                _id,
                                                BandedGridView1.GetFocusedRowCellValue("IDTrainingCode"),
                                                BandedGridView1.GetFocusedRowCellValue("EmpID")),
                                                para)
        End If
    End Sub

    Private Sub txtMailPre_EditValueChanged(sender As Object, e As EventArgs) Handles txtMailPre.EditValueChanged
        txtMailPreKQ.Text = txtMailPre.Text
    End Sub

    Private Sub cbbMailCheck_EditValueChanged(sender As Object, e As EventArgs) Handles cbbMailCheck.EditValueChanged
        txtMailCheckKQ.Text = cbbMailCheck.Text
    End Sub

    Private Sub cbbMailApprove_EditValueChanged(sender As Object, e As EventArgs) Handles cbbMailApprove.EditValueChanged
        txtMailApproveKQ.Text = cbbMailApprove.Text
    End Sub

    Private Sub btnPrintReport_Click(sender As Object, e As EventArgs) Handles btnPrintReport.Click
        '----------------------- F030 ----------------------
        Dim dtF030 = _db.FillDataTable(String.Format("SELECT IDTrainingCode, TrainingProgram,
                                                    TrainingMan, FromDate, ToDate, Duration, Place
                                                    FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                                    WHERE ID = '{0}'", _id))
        For Each r As DataRow In dtF030.Rows
            Dim dtDetail30 = _db.FillDataTable(String.Format("SELECT h.EmpID, d.EmpName, 
		                IIF(h.Level1ReactionEvaluate = 'Yes', 'X', '') AS Level1EvalYes,
		                IIF(h.Level1ReactionEvaluate = 'No', 'X', '') AS Level1EvalNo,
		                h.Level2LearningPoint AS Level2Point,
		                IIF(h.Level2LearningEvaluate = 'OK', 'OK', '') AS Level2EvalOK,
		                IIF(h.Level2LearningEvaluate = 'NG', 'NG', '') AS Level2EvalNG,
		                IIF(h.Level3TransferEvaluate = 'OK', 'OK', '') AS Level3EvalOK,
		                IIF(h.Level3TransferEvaluate = 'NG', 'NG', '') AS Level3EvalNG,
		                h.Remark
                FROM GA_TRM_RegisterPlannedNewComer_Employee AS h
                LEFT JOIN OT_Employee AS d
                ON h.EmpID = d.EmpID
                WHERE ID = '{0}'
                AND IDTrainingCode = '{1}'",
                _id,
                r("IDTrainingCode")))
            Dim rpF030 As New RpF030
            rpF030.DataSource = dtDetail30
            rpF030.DataMember = ""
            'parameters
            rpF030.Parameters("paraTrainingProgram").Value = r("TrainingProgram")
            rpF030.Parameters("paraTrainingMan").Value = r("TrainingMan")
            If IsDate(r("FromDate")) Then
                rpF030.Parameters("paraFromDate").Value = Date.Parse(r("FromDate")).ToString("dd-MMM-yyyy")
            Else
                rpF030.Parameters("paraFromDate").Value = DBNull.Value
            End If
            If IsDate(r("ToDate")) Then
                rpF030.Parameters("paraToDate").Value = Date.Parse(r("ToDate")).ToString("dd-MMM-yyyy")
            Else
                rpF030.Parameters("paraToDate").Value = DBNull.Value
            End If
            rpF030.Parameters("paraDuration").Value = r("Duration")
            rpF030.Parameters("paraPlace").Value = r("Place")
            rpF030.Parameters("paraTrainingProgram").Visible = False
            rpF030.Parameters("paraTrainingMan").Visible = False
            rpF030.Parameters("paraFromDate").Visible = False
            rpF030.Parameters("paraToDate").Visible = False
            rpF030.Parameters("paraDuration").Visible = False
            rpF030.Parameters("paraPlace").Visible = False
            Dim RePrToolF030 As New ReportPrintTool(rpF030)
            RePrToolF030.ShowPreview()
        Next

        '------------------ F065 --------------------
        'Dim dataF065 = _db.FillDataTable("SELECT
        '    CAST(NULL AS NVARCHAR) AS ProgramDetailName,
        '    CAST(NULL AS NVARCHAR) AS Duration,
        '    CAST(NULL AS NVARCHAR) AS Trainer,
        '    CAST(NULL AS NVARCHAR) AS TestMethod,
        '    CAST(NULL AS NVARCHAR) AS Venue,
        '    CAST(NULL AS NVARCHAR) AS Remark
        '")
        'dataF065.TableName = "Report_Training_HR-F-065"
        'dataF065.WriteXmlSchema("Report_Training_HR-F-065.xsd")
        Dim dtF065 = _db.FillDataTable(String.Format("SELECT IDTrainingCode, FromDate, ToDate
                                                    FROM GA_TRM_RegisterPlannedNewComer_ProgramList
                                                    WHERE ID = '{0}'", _id))
        For Each r As DataRow In dtF065.Rows
            Dim dtDetail65 = _db.FillDataTable(String.Format("SELECT ProgramDetailName, Duration,
                Trainer, TestMethod, Venue, Remark
                FROM GA_TRM_RegisterPlannedNewComer_ProgramDetail
                WHERE ID = '{0}'
                AND IDTrainingCode = '{1}'",
                _id,
                r("IDTrainingCode")))
            Dim rpF065 As New RpF065
            rpF065.DataSource = dtDetail65
            rpF065.DataMember = ""
            'parameters
            rpF065.Parameters("paraFromDate").Value = r("FromDate")
            rpF065.Parameters("paraToDate").Value = r("ToDate")
            Dim dtInfo = _db.FillDataTable(String.Format("SELECT TOP 1 d.Dept, d.Section, d.GroupA, d.StartingDate
                FROM GA_TRM_RegisterPlannedNewComer_Employee AS h
                LEFT JOIN HRM_Emloyee AS d
                ON h.EmpID = d.ECode
                WHERE h.ID = '{0}'
                AND h.IDTrainingCode = '{1}'",
                _id,
                r("IDTrainingCode")))
            If dtInfo.Rows.Count > 0 Then
                rpF065.Parameters("paraDept").Value = dtInfo.Rows(0)("Dept")
                rpF065.Parameters("paraSection").Value = dtInfo.Rows(0)("Section")
                rpF065.Parameters("paraGroup").Value = dtInfo.Rows(0)("GroupA")
                rpF065.Parameters("paraStartDate").Value = dtInfo.Rows(0)("StartingDate")
            End If
            Dim dtStuff = _db.FillDataTable(String.Format("SELECT h.EmpID, d.EName INTO #Temp
                FROM GA_TRM_RegisterPlannedNewComer_Employee AS h
                LEFT JOIN HRM_Emloyee AS d
                ON h.EmpID = d.ECode
                WHERE h.ID = '{0}'
                AND h.IDTrainingCode = '{1}'
                ORDER BY h.EmpID

                SELECT STUFF((SELECT (' - ' + EName)
						                FROM #Temp
						                FOR XML PATH(''), TYPE
						                ).value('.', 'NVARCHAR(MAX)')
						                ,1,3,'') AS ListName,
		                STUFF((SELECT (' - ' + EmpID)
						                FROM #Temp
						                FOR XML PATH(''), TYPE
						                ).value('.', 'NVARCHAR(MAX)')
						                ,1,3,'') AS ListEmpID",
                                        _id,
                                        r("IDTrainingCode")))
            If dtStuff.Rows.Count > 0 Then
                rpF065.Parameters("paraStuffName").Value = dtStuff.Rows(0)("ListName")
                rpF065.Parameters("paraStuffEmpID").Value = dtStuff.Rows(0)("ListEmpID")
            End If
            Dim RePrToolF065 As New ReportPrintTool(rpF065)
            RePrToolF065.ShowPreview()
        Next
    End Sub

End Class