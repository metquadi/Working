﻿Imports System.Drawing
Imports CommonDB
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils.Layout
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports PublicUtility
Public Class FrmRegisterPlannedYearly
    Dim _db As New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
    Public _id As String = ""
    Dim _isEdit As Boolean = False

    'Vẽ nét liền cho 2 TablePanel
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
    '----- Kết thúc vẽ -----
    Public Enum Confirm
        Pre
        Check
        Approve
        Manager
        PreKQ
        CheckKQ
        ApproveKQ
        Reverse
    End Enum
    Private Sub FrmRegisterPlanned_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim obj As New Main_UserRight
        obj.UserID_K = CurrentUser.UserID
        obj.FormID_K = Tag
        _db.GetObject(obj)
        If obj.IsEdit Or obj.IsAdmin Then
            _isEdit = True
        End If

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
        Dim dt = _db.FillDataTable("SELECT h.EmpID, d.SectionSort, d.Observation, h.Mail
                                    FROM OT_Mail AS h
                                    LEFT JOIN OT_Employee AS d
                                    ON h.EmpID = d.EmpID
                                    ORDER BY h.Mail")
        cbbMailCheck.Properties.DataSource = dt
        cbbMailCheck.Properties.DisplayMember = "Mail"
        cbbMailCheck.Properties.ValueMember = "Mail"
        cbbMailCheck.Properties.NullText = Nothing
        cbbMailCheck.Properties.PopulateViewColumns()
        cbbMailCheck.Properties.View.Columns("Mail").Width = 200

        cbbMailApprove.Properties.DataSource = dt
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

        btnSaveManager.Visible = False
        btnSubmitManager.Visible = False
        lblDateManager.Visible = False
        mmeCmtManager.ReadOnly = True

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

        btnDelete.Visible = False
        btnDeleteEmp.Visible = False
        btnReverse.Visible = False
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
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            If obj.Method = "Reverse" Or obj.Method = "ReverseButEdit" Then
                lblMethod.Text = "Reverse"
            Else
                lblMethod.Text = "Normal"
            End If
            txtMailPre.Text = obj.MailPre
            cbbMailCheck.Text = obj.MailCheck
            cbbMailApprove.Text = obj.MailApprove
            txtMailManager.Text = obj.MailManager
            txtMailPreKQ.Text = obj.MailPreKQ
            txtMailCheckKQ.Text = obj.MailCheckKQ
            txtMailApproveKQ.Text = obj.MailApproveKQ
            mmeCmtPre.Text = obj.CommentPre
            mmeCmtCheck.Text = obj.CommentCheck
            mmeCmtApprove.Text = obj.CommentApprove
            mmeCmtManager.Text = obj.CommentManager
            mmeCmtPreKQ.Text = obj.CommentPreKQ
            mmeCmtCheckKQ.Text = obj.CommentCheckKQ
            mmeCmtApproveKQ.Text = obj.CommentApproveKQ
            txtSection.Text = obj.Section
            dteDate.EditValue = obj.DateSave
            If obj.DateApprove > Date.MinValue And obj.CurrentMail <> "" Then
                btnReverse.Visible = True
            End If

            LoadProgramList()
            LoadPlanActual()
            LoadEmployee(DBNull.Value)
            LoadScore()
            LoadResult()

            If obj.DatePre > DateTime.MinValue Then
                lblDatePre.Text = obj.DatePre.ToString("dd-MMM-yyyy HH:mm")
                lblDatePre.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailPre And
                    obj.Method <> "Reverse" Then
                    cbbMailCheck.ReadOnly = False
                    cbbMailApprove.ReadOnly = False
                    btnSave.Visible = True
                    btnSubmit.Visible = True
                    mmeCmtPre.ReadOnly = False
                    btnDelete.Visible = True
                    btnDeleteEmp.Visible = True
                    EnableProgramList()
                    EnablePlanActual()
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateCheck > DateTime.MinValue Then
                lblDateCheck.Text = obj.DateCheck.ToString("dd-MMM-yyyy HH:mm")
                lblDateCheck.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailCheck And obj.Method <> "Reverse" Then
                    btnCheck.Visible = True
                    btnRejectCheck.Visible = True
                    mmeCmtCheck.ReadOnly = False
                    btnDelete.Visible = True
                    btnDeleteEmp.Visible = True
                    cbbMailApprove.ReadOnly = False
                    EnableProgramList()
                    EnablePlanActual()
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateApprove > DateTime.MinValue Then
                lblDateApprove.Text = obj.DateApprove.ToString("dd-MMM-yyyy HH:mm")
                lblDateApprove.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailApprove And obj.Method <> "Reverse" Then
                    btnApprove.Visible = True
                    btnRejectApprove.Visible = True
                    mmeCmtApprove.ReadOnly = False
                    Return
                End If
            End If

            If obj.DateManager > DateTime.MinValue Then
                lblDateManager.Text = obj.DateManager.ToString("dd-MMM-yyyy HH:mm")
                lblDateManager.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailManager And obj.Method <> "Reverse" Then
                    btnSaveManager.Visible = True
                    btnSubmitManager.Visible = True
                    mmeCmtManager.ReadOnly = False
                    EnableResult()
                    Return
                End If
            End If

            If obj.DatePreKQ > DateTime.MinValue Then
                lblDatePreKQ.Text = obj.DatePreKQ.ToString("dd-MMM-yyyy HH:mm")
                lblDatePreKQ.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailPreKQ And obj.Method <> "Reverse" Then
                    btnSaveKQ.Visible = True
                    btnSubmitKQ.Visible = True
                    mmeCmtPreKQ.ReadOnly = False
                    EnablePlanActual()
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateCheckKQ > DateTime.MinValue Then
                lblDateCheckKQ.Text = obj.DateCheckKQ.ToString("dd-MMM-yyyy HH:mm")
                lblDateCheckKQ.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailCheckKQ And obj.Method <> "Reverse" Then
                    btnCheckKQ.Visible = True
                    btnRejectCheckKQ.Visible = True
                    mmeCmtCheckKQ.ReadOnly = False
                    EnablePlanActual()
                    EnableScore()
                    EnableResult()
                    Return
                End If
            End If

            If obj.DateApproveKQ > DateTime.MinValue Then
                lblDateApproveKQ.Text = obj.DateApproveKQ.ToString("dd-MMM-yyyy HH:mm")
                lblDateApproveKQ.Visible = True
            Else
                If obj.CurrentMail = CurrentUser.Mail And obj.CurrentMail = obj.MailApproveKQ And obj.Method <> "Reverse" Then
                    btnApproveKQ.Visible = True
                    btnRejectApproveKQ.Visible = True
                    mmeCmtApproveKQ.ReadOnly = False
                    Return
                End If
            End If
        End If
    End Sub
    Sub LoadProgramList()
        GridControl1.DataSource = _db.FillDataTable(String.Format("
            SELECT IDTrainingCode, TrainingProgram, TrainingField, TrainingMethod, Trainer, Trainee,
                TrainingMan, FromDate, ToDate, FromTime, ToTime, Duration, Place
            FROM GA_TRM_RegisterPlanned_Detail
            WHERE ID = '{0}'", _id))
        GridControlSetFormat(GridView1, 0)
        GridView1.Columns("TrainingField").Width = 100
        GridView1.Columns("TrainingProgram").Width = 200
        GridView1.Columns("Trainer").Width = 60
        GridView1.Columns("TrainingMan").Width = 150
        GridView1.Columns("FromTime").ColumnEdit = GridControlTimeEdit()
        GridView1.Columns("ToTime").ColumnEdit = GridControlTimeEdit()

        '------------------ Load Program -------------------
        slueProgram.DataSource = _db.FillDataTable("SELECT TrainingCode, TrainingField, TrainingProgram, TrainingContent,
                                                        TrainingMethod, Trainer, Trainee, TrainingFrequency
                                                    FROM GA_TRM_ProgramMaster
                                                    WHERE TrainingCode NOT LIKE 'N%'")
        slueProgram.DisplayMember = "TrainingCode"
        slueProgram.ValueMember = "TrainingCode"
        slueProgram.NullText = Nothing
        slueProgram.PopulateViewColumns()
        slueProgram.View.Columns("TrainingCode").Width = 60
        slueProgram.View.Columns("TrainingField").Width = 110
        slueProgram.View.Columns("TrainingProgram").Width = 330
        slueProgram.View.Columns("TrainingContent").Width = 280
        slueProgram.View.Columns("TrainingMethod").Width = 120
        slueProgram.View.Columns("Trainee").Width = 150
        slueProgram.View.Columns("TrainingFrequency").Width = 150
        GridView1.Columns("IDTrainingCode").ColumnEdit = slueProgram

        '------------------- Load Người Training -------------------
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
        GridView1.Columns("IDTrainingCode").OptionsColumn.ReadOnly = False
        GridView1.Columns("TrainingMan").OptionsColumn.ReadOnly = False
        GridView1.Columns("FromDate").OptionsColumn.ReadOnly = False
        GridView1.Columns("ToDate").OptionsColumn.ReadOnly = False
        GridView1.Columns("FromTime").OptionsColumn.ReadOnly = False
        GridView1.Columns("ToTime").OptionsColumn.ReadOnly = False
        GridView1.Columns("Duration").OptionsColumn.ReadOnly = False
        GridView1.Columns("Place").OptionsColumn.ReadOnly = False
        GridControlSetColorEdit(GridView1)
        GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
    End Sub
    Sub LoadPlanActual()
        Dim startD As Date = GetStartDayOfYear(Date.Now)
        Dim endD As Date = GetEndDayOfYear(Date.Now)
        Dim col As String = ""
        While startD <= endD
            col += startD.ToString("[MMM-yyyy]") + ","
            startD = startD.AddMonths(1)
        End While
        col = col.Substring(0, col.Length - 1)
        Dim para(1) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@id", _id)
        para(1) = New SqlClient.SqlParameter("@col", col)
        GridControl2.DataSource = _db.ExecuteStoreProcedureTB("sp_GA_TRM_RegisterPlanned", para)
        GridControlSetFormat(GridView2, 2)
        GridView2.Columns("TrainingProgram").Width = 200
        For Each c As GridColumn In GridView2.Columns
            If IsDate(c.FieldName) Then
                GridView2.Columns(c.FieldName).ColumnEdit = cbbSeleteState
                GridView2.Columns(c.FieldName).OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
            End If
        Next
        GridView2.Columns("Trainees").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
        GridView2.Columns("Cost").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
    End Sub
    Sub EnablePlanActual()
        GridControlReadOnly(GridView2, False)
        GridView2.Columns("IDTrainingCode").OptionsColumn.ReadOnly = True
        GridView2.Columns("TrainingProgram").OptionsColumn.ReadOnly = True
        GridView2.Columns("Status").OptionsColumn.ReadOnly = True
        GridControlSetColorEdit(GridView2)
    End Sub
    Sub LoadEmployee(trainingCodeID)
        Dim para(1) As SqlClient.SqlParameter
        If trainingCodeID Is Nothing Then
            para(0) = New SqlClient.SqlParameter("@trainingCodeID", DBNull.Value)
        Else
            para(0) = New SqlClient.SqlParameter("@trainingCodeID", trainingCodeID)
        End If
        para(1) = New SqlClient.SqlParameter("@Section", DBNull.Value)
        GridControl3.DataSource = _db.FillDataTable(String.Format("SELECT h.IDTrainingCode, g.TrainingProgram, h.EmpID, d.EName
                                                        FROM GA_TRM_RegisterPlanned_Detail_Employee AS h
                                                        LEFT JOIN HRM_Emloyee AS d
                                                        ON h.EmpID = d.ECode
                                                        LEFT JOIN GA_TRM_ProgramMaster AS g
                                                        ON h.IDTrainingCode = g.TrainingCode
                                                        WHERE ID = '{0}'
                                                        AND (@trainingCodeID IS NULL OR h.IDTrainingCode = @trainingCodeID)
                                                        ORDER BY h.IDTrainingCode, h.EmpID",
                                                        _id), para)
        GridControlSetFormat(GridView3)
        GridView3.Columns("IDTrainingCode").Width = 100
        GridView3.Columns("TrainingProgram").Width = 200
        GridView3.Columns("EName").Width = 200

        'Dim obj As New GA_TRM_MailGAReceiveInformation
        'obj.ID_K = "1"
        '_db.GetObject(obj)
        'If CurrentUser.Mail <> obj.MailPIC Then
        '    para(1) = New SqlClient.SqlParameter("@Section", CurrentUser.Section)
        'End If
        If _isEdit = False Then
            para(1) = New SqlClient.SqlParameter("@Section", CurrentUser.Section)
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
        GridView3.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
    End Sub
    Sub LoadScore()
        GridView4.Columns.Clear()
        Dim para(0) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@id", _id)
        Dim dtExample = _db.ExecuteStoreProcedureTB("sp_GA_TRM_TrainingResult", para)
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
        GridControl5.DataSource = _db.FillDataTable(String.Format(" 
            SELECT h.IDTrainingCode, g.TrainingProgram, m.ToDate, h.EmpID, d.EName, 
            h.Level1ReactionEvaluate, h.Level2LearningPoint, 
            h.Level2LearningEvaluate, h.Level3TransferEvaluate, h.ConfirmedBy, h.Remark
            FROM GA_TRM_RegisterPlanned_Detail_Employee AS h
            LEFT JOIN HRM_Emloyee AS d
            ON h.EmpID = d.ECode
            LEFT JOIN GA_TRM_ProgramMaster AS g
            ON h.IDTrainingCode = g.TrainingCode
            LEFT JOIN GA_TRM_RegisterPlanned_Detail AS m
            ON h.ID = m.ID
            AND h.IDTrainingCode = m.IDTrainingCode
            WHERE h.ID = '{0}'
            AND h.IDTrainingCode LIKE 'D%'
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
        BandedGridView1.Columns("ConfirmedBy").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False
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
    Function CreateNewID() As String
        Dim valMax = _db.ExecuteScalar(String.Format("SELECT ISNULL(RIGHT(MAX(ID), 2), 0)
                                                      FROM GA_TRM_RegisterPlanned
                                                      WHERE ID LIKE '%{0}%'",
                                                      Date.Now.ToString("yyMM")))
        valMax = (Integer.Parse(valMax) + 1).ToString.PadLeft(2, "0")
        Return "TRMY_" + Date.Now.ToString("yyMM") + "_" + valMax
    End Function
    Function CheckData(isProgramList As Boolean, isPlanActual As Boolean, typePlan As String,
                       isEmployee As Boolean, isScore As Boolean, isResult As Boolean) As Boolean
        If txtMailPre.Text = "" Or cbbMailCheck.Text = "" Or cbbMailApprove.Text = "" Or
            txtMailPreKQ.Text = "" Or txtMailCheckKQ.Text = "" Or txtMailApproveKQ.Text = "" Then
            ShowWarning("Địa chỉ mail không được để trống !")
            cbbMailCheck.Select()
            Return False
        End If

        If isProgramList Then
            Dim valProg = _db.ExecuteScalar(String.Format("SELECT TOP 1 ID
                                                        FROM GA_TRM_RegisterPlanned_Detail
                                                        WHERE ID = '{0}'", _id))
            If valProg Is Nothing Then
                ShowWarning("Bạn chưa nhập các hạng mục đào tạo !")
                tabF028.Show()
                Return False
            End If
        End If

        If isPlanActual Then
            Dim valPlnAct = _db.ExecuteScalar(String.Format("SELECT TOP 1 h.IDTrainingCode
                                                            FROM GA_TRM_RegisterPlanned_Detail AS h
                                                            LEFT JOIN (
	                                                            SELECT ID, IDTrainingCode, MIN(Month) AS Month
	                                                            FROM GA_TRM_RegisterPlanned_Detail_Time
	                                                            WHERE Status = '{0}'
	                                                            GROUP BY ID, IDTrainingCode
                                                            ) AS d
                                                            ON h.ID = d.ID 
                                                            AND h.IDTrainingCode = d.IDTrainingCode
                                                            WHERE h.ID = '{1}'
                                                            AND d.ID IS NULL
                                                            ORDER BY h.IDTrainingCode",
                                                            typePlan,
                                                            _id))
            If valPlnAct IsNot Nothing Then
                ShowWarning(String.Format("Chưa nhập thời gian dự kiến hoặc thực tế của hạng mục đào tạo {0} !", valPlnAct))
                tabF028.Show()
                tabPlanActual.Show()
                Return False
            End If
        End If

        If isEmployee Then
            Dim valEmp = _db.ExecuteScalar(String.Format("
                SELECT TOP 1 h.IDTrainingCode
                FROM GA_TRM_RegisterPlanned_Detail AS h
                LEFT JOIN GA_TRM_RegisterPlanned_Detail_Employee AS d
                ON h.ID = d.ID
                AND h.IDTrainingCode = d.IDTrainingCode
                WHERE h.ID = '{0}'
                AND h.IDTrainingCode LIKE 'D%'
                AND d.ID IS NULL
                GROUP BY h.IDTrainingCode
                ORDER BY h.IDTrainingCode", _id))
            If valEmp IsNot Nothing Then
                ShowWarning(String.Format("Chưa thêm nhân viên cho hạng mục {0} !", valEmp))
                tabF028.Show()
                tabEmployee.Show()
                Return False
            End If
        End If

        If isScore Then
            Dim dtScore = _db.FillDataTable(String.Format("
                SELECT TOP 1 h.EmpID, h.IDTrainingCode
                FROM GA_TRM_RegisterPlanned_Detail_Employee AS h
                LEFT JOIN GA_TRM_RegisterPlanned_Detail_Employee_Score AS d
                ON h.ID = d.ID
                AND h.IDTrainingCode = d.IDTrainingCode
                AND h.EmpID = d.EmpID
                WHERE h.ID = '{0}'
                AND d.LevelOfCompetency = 'Result'
                AND (d.Score IS NULL OR d.Score < 1)
                ORDER BY h.EmpID, h.IDTrainingCode", _id))
            If dtScore.Rows.Count > 0 Then
                ShowWarning(String.Format("Chưa đánh giá kết quả cho NV: {0} ở hạng mục {1} !",
                                          dtScore.Rows(0)("EmpID"),
                                          dtScore.Rows(0)("IDTrainingCode")))
                tabF027.Show()
                Return False
            End If
        End If

        If isResult Then
            Dim dtResult = _db.FillDataTable(String.Format("
                SELECT TOP 1 IDTrainingCode, EmpID
                FROM GA_TRM_RegisterPlanned_Detail_Employee
                WHERE ID = '{0}'
                AND IDTrainingCode LIKE 'D%'
                AND (Level3TransferEvaluate = '' OR Level3TransferEvaluate IS NULL)
                ORDER BY IDTrainingCode, EmpID", _id))
            If dtResult.Rows.Count > 0 Then
                ShowWarning(String.Format("Chưa xác nhận hiệu quả Cấp độ 3 của NV: {0} ở hạng mục {1} !",
                                          dtResult.Rows(0)("EmpID"),
                                          dtResult.Rows(0)("IDTrainingCode")))
                tabF030.Show()
                Return False
            End If
        End If

        Return True
    End Function
    Sub SaveData()
        Dim obj As New GA_TRM_RegisterPlanned
        If _id = "" Then
            _id = CreateNewID()
            obj.ID_K = _id
            txtID.Text = _id
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
        obj.MailManager = txtMailManager.Text
        obj.MailPreKQ = txtMailPreKQ.Text
        obj.MailCheckKQ = txtMailCheckKQ.Text
        obj.MailApproveKQ = txtMailApproveKQ.Text
        obj.CommentPre = mmeCmtPre.Text
        obj.CommentCheck = mmeCmtCheck.Text
        obj.CommentApprove = mmeCmtApprove.Text
        obj.CommentManager = mmeCmtManager.Text
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
        If CheckData(False, False, "", False, False, False) Then
            SaveData()
            LoadHead()
            ShowSuccess()
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If CheckData(True, True, "Plan", True, False, False) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DatePre = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Pre, obj.CommentPre, obj)
        End If
    End Sub
    Sub ConfirmUpdateOutlook(submit As Submit, confirm As Confirm, comment As String, obj As GA_TRM_RegisterPlanned)
        Try
            _db.BeginTransaction()
            Dim listTo As New List(Of String)
            Dim listCC As New List(Of String)
            Dim listBCC As New List(Of String)
            Dim arrCC() As String = Nothing
            Dim titleMail As String = ""

            If submit = Submit.Reject Then
                titleMail = "Reject - Register Training Management Yearly - " + obj.Section + " - " + obj.DateSave.ToString("dd-MMM-yyyy")
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
                SendMailOutlook(titleMail, Nothing, Submit.Reject, listTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
            Else
                titleMail = "Register Training Management Yearly - " + obj.Section + " - " + obj.DateSave.ToString("dd-MMM-yyyy")
                Dim objMailGA As New GA_TRM_MailGAReceiveInformation
                objMailGA.ID_K = "1"
                _db.GetObject(objMailGA)
                Select Case confirm
                    Case Confirm.Pre
                        If obj.MailCheck <> "" Then
                            listTo.Add(obj.MailCheck)
                            SendMailOutlook(titleMail, Nothing, Submit.Confirm, listTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailCheck
                            GoTo EndConfirm
                        End If
                        GoTo Check
                    Case Confirm.Check
Check:
                        If obj.MailApprove <> "" Then
                            listTo.Add(obj.MailApprove)
                            SendMailOutlook(titleMail, Nothing, Submit.Confirm, listTo, Nothing, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailApprove
                            GoTo EndConfirm
                        End If
                        GoTo Approve
                    Case Confirm.Approve
Approve:
                        If obj.MailManager <> "" Then
                            listTo.Add(obj.MailManager)
                            listCC.Add(objMailGA.MailPIC)
                            'listCC.Add(objMailGA.MailApproved) Vân GA
                            SendMailOutlook(titleMail, Nothing, Submit.Confirm, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailManager
                            GoTo EndConfirm
                        End If
                        GoTo Manager
                    Case Confirm.Manager
Manager:
                        If obj.MailPreKQ <> "" Then
                            listTo.Add(obj.MailPreKQ)
                            listCC.Add(objMailGA.MailPIC)
                            'listCC.Add(objMailGA.MailApproved) Vân GA
                            SendMailOutlook(titleMail, Nothing, Submit.Confirm, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailPreKQ
                            GoTo EndConfirm
                        End If
                        GoTo PreKQ
                    Case Confirm.PreKQ
PreKQ:
                        If obj.MailCheckKQ <> "" Then
                            listTo.Add(obj.MailCheckKQ)
                            SendMailOutlook(titleMail, Nothing, Submit.Confirm, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = obj.MailCheckKQ
                            GoTo EndConfirm
                        End If
                        GoTo CheckKQ
                    Case Confirm.CheckKQ
CheckKQ:
                        If obj.MailApproveKQ <> "" Then
                            listTo.Add(obj.MailApproveKQ)
                            SendMailOutlook(titleMail, Nothing, Submit.Confirm, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
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
                            SendMailOutlook(titleMail, Nothing, Submit.Info, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
                            obj.CurrentMail = ""
                            GoTo EndConfirm
                        End If
                        GoTo Reverse
                    Case Confirm.Reverse
Reverse:
                        If obj.MailPre <> "" Then
                            listTo.Add(obj.MailPre)
                            listCC.Add(objMailGA.MailPIC)
                            'listCC.Add(objMailGA.MailApproved) Vân GA
                            SendMailOutlook(titleMail, "Reverse kế hoạch đăng kí hạng mục đào tạo", Submit.Reject, listTo, listCC, Nothing, Nothing, Tag, obj.ID_K)
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
                                                                FROM GA_TRM_RegisterPlanned
                                                                WHERE CurrentMail = '{0}'
                                                                ORDER BY ID",
                                                                CurrentUser.Mail))
        If idNext IsNot Nothing Then
            _id = idNext
        End If
        LoadHead()
    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        If CheckData(True, True, "Plan", True, False, False) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateCheck = DateTime.Now
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Check, obj.CommentCheck, obj)
        End If
    End Sub

    Private Sub btnRejectCheck_Click(sender As Object, e As EventArgs) Handles btnRejectCheck.Click
        If mmeCmtCheck.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentCheck = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtCheck.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.Check, obj.CommentCheck, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtCheck.Select()
        End If
    End Sub

    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        If CheckData(True, True, "Plan", True, False, False) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateApprove = DateTime.Now
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Approve, obj.CommentApprove, obj)
        End If
    End Sub

    Private Sub btnRejectApprove_Click(sender As Object, e As EventArgs) Handles btnRejectApprove.Click
        If mmeCmtApprove.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentApprove = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtApprove.Text)
            obj.CurrentMail = ""

            ConfirmUpdateOutlook(Submit.Reject, Confirm.Approve, obj.CommentApprove, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtApprove.Select()
        End If
    End Sub

    Private Sub btnSaveManager_Click(sender As Object, e As EventArgs) Handles btnSaveManager.Click
        If CheckData(True, True, "Plan", True, False, False) Then
            SaveData()
            LoadHead()
            ShowSuccess()
        End If
    End Sub

    Private Sub btnSubmitManager_Click(sender As Object, e As EventArgs) Handles btnSubmitManager.Click
        If CheckData(True, True, "Actual", True, False, True) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateManager = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.Manager, obj.CommentManager, obj)
        End If
    End Sub

    Private Sub btnSaveKQ_Click(sender As Object, e As EventArgs) Handles btnSaveKQ.Click
        If CheckData(True, True, "Actual", True, True, True) Then
            SaveData()
            LoadHead()
            ShowSuccess()
        End If
    End Sub

    Private Sub btnSubmitKQ_Click(sender As Object, e As EventArgs) Handles btnSubmitKQ.Click
        If CheckData(True, True, "Actual", True, True, True) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DatePreKQ = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.PreKQ, obj.CommentPreKQ, obj)
        End If
    End Sub

    Private Sub btnCheckKQ_Click(sender As Object, e As EventArgs) Handles btnCheckKQ.Click
        If CheckData(True, True, "Actual", True, True, True) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateCheckKQ = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Confirm, Confirm.CheckKQ, obj.CommentCheckKQ, obj)
        End If
    End Sub

    Private Sub btnRejectCheckKQ_Click(sender As Object, e As EventArgs) Handles btnRejectCheckKQ.Click
        If mmeCmtCheckKQ.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentCheckKQ = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtCheckKQ.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.CheckKQ, obj.CommentCheckKQ, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtCheckKQ.Select()
        End If
    End Sub

    Private Sub btnApproveKQ_Click(sender As Object, e As EventArgs) Handles btnApproveKQ.Click
        If CheckData(True, True, "Actual", True, True, True) Then
            SaveData()
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.DateApproveKQ = DateTime.Now
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Info, Confirm.ApproveKQ, obj.CommentApproveKQ, obj)
        End If
    End Sub

    Private Sub btnRejectApproveKQ_Click(sender As Object, e As EventArgs) Handles btnRejectApproveKQ.Click
        If mmeCmtApproveKQ.Text.Trim <> "" Then
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.CommentApproveKQ = String.Format("({0}) {1}", DateTime.Now.ToString("dd-MMM-yyyy"), mmeCmtApproveKQ.Text)
            obj.CurrentMail = ""
            ConfirmUpdateOutlook(Submit.Reject, Confirm.ApproveKQ, obj.CommentApproveKQ, obj)
        Else
            ShowWarning("Bạn phải nhập lý do từ chối yêu cầu !" + vbCrLf + "Please comment detail.")
            mmeCmtApproveKQ.Select()
        End If
    End Sub
    Private Sub slueProgram_EditValueChanged(sender As Object, e As EventArgs) Handles slueProgram.EditValueChanged
        Dim lc As SearchLookUpEdit = CType(sender, SearchLookUpEdit)
        Dim obj As New GA_TRM_RegisterPlanned_Detail
        obj.ID_K = _id
        obj.IDTrainingCode_K = lc.Properties.View.GetFocusedRowCellValue("TrainingCode")
        If _db.ExistObject(obj) Then
            ReturnOldValue(GridView1)
            ShowWarning("Hạng mục đào tạo này đã được chọn trước đó !")
        Else
            GridView1.SetFocusedRowCellValue("TrainingField", lc.Properties.View.GetFocusedRowCellValue("TrainingField"))
            GridView1.SetFocusedRowCellValue("TrainingProgram", lc.Properties.View.GetFocusedRowCellValue("TrainingProgram"))
            GridView1.SetFocusedRowCellValue("TrainingMethod", lc.Properties.View.GetFocusedRowCellValue("TrainingMethod"))
            GridView1.SetFocusedRowCellValue("Trainer", lc.Properties.View.GetFocusedRowCellValue("Trainer"))
            GridView1.SetFocusedRowCellValue("Trainee", lc.Properties.View.GetFocusedRowCellValue("Trainee"))
        End If
    End Sub

    Private Sub GridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.Editable And e.Column.ReadOnly = False Then
            Dim obj As New GA_TRM_RegisterPlanned_Detail
            obj.ID_K = _id
            obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
            If e.RowHandle < 0 Then
                If e.Column.FieldName = "IDTrainingCode" Then
                    'Để tránh trường hợp bị thêm Hạng mục mới thay vì sửa Hạng mục
                    If IsDBNull(GridView1.ActiveEditor.OldEditValue) Or
                            GridView1.ActiveEditor.OldEditValue Is Nothing Then
                        obj.TrainingField = GridView1.GetFocusedRowCellValue("TrainingField")
                        obj.TrainingProgram = GridView1.GetFocusedRowCellValue("TrainingProgram")
                        obj.TrainingMethod = GridView1.GetFocusedRowCellValue("TrainingMethod")
                        obj.Trainer = GridView1.GetFocusedRowCellValue("Trainer")
                        obj.Trainee = GridView1.GetFocusedRowCellValue("Trainee")
                        obj.CreateUser = CurrentUser.UserID
                        obj.CreateDate = DateTime.Now
                        _db.Insert(obj)
                        CreateSeleteTime(obj.IDTrainingCode_K)
                    Else
                        GoTo UpdateID
                    End If
                Else
                    GoTo UpdateField
                End If
            Else
                If e.Column.FieldName = "IDTrainingCode" Then
UpdateID:
                    _db.ExecuteNonQuery(String.Format("UPDATE GA_TRM_RegisterPlanned_Detail
                                                       SET IDTrainingCode = '{0}',
                                                       TrainingField = N'{1}',
                                                       TrainingProgram = N'{2}',
                                                       TrainingMethod = N'{3}',
                                                       Trainer = '{4}',
                                                       Trainee = N'{5}'
                                                       WHERE ID = '{6}'
                                                       AND IDTrainingCode = '{7}'",
                                                       GridView1.GetFocusedRowCellValue("IDTrainingCode"),
                                                       GridView1.GetFocusedRowCellValue("TrainingField"),
                                                       GridView1.GetFocusedRowCellValue("TrainingProgram"),
                                                       GridView1.GetFocusedRowCellValue("TrainingMethod"),
                                                       GridView1.GetFocusedRowCellValue("Trainer"),
                                                       GridView1.GetFocusedRowCellValue("Trainee"),
                                                       _id,
                                                       GridView1.ActiveEditor.OldEditValue))
                Else
UpdateField:
                    Dim para(0) As SqlClient.SqlParameter
                    para(0) = New SqlClient.SqlParameter("@Value", e.Value)
                    _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlanned_Detail
                                                        SET {0} = @Value
                                                        WHERE ID = '{1}'
                                                        AND IDTrainingCode = '{2}'",
                                                        e.Column.FieldName,
                                                        _id,
                                                        GridView1.GetFocusedRowCellValue("IDTrainingCode")),
                                                        para)
                    If e.Column.FieldName = "ToDate" Then
                        LoadResult()
                        EnableResult()
                    End If
                    Return
                End If
            End If
            LoadPlanActual()
            LoadScore()
            LoadResult()
            EnableProgramList()
            EnablePlanActual()
            EnableScore()
            EnableResult()
        End If
    End Sub
    Sub CreateSeleteTime(trainingCode)
        Dim obj As New GA_TRM_RegisterPlanned_Detail_Content
        obj.ID_K = _id
        obj.IDTrainingCode_K = trainingCode
        obj.Status_K = "Plan"
        If Not _db.ExistObject(obj) Then
            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlanned_Detail_Content 
                                                (ID, IDTrainingCode, Status)
                                                VALUES('{0}', '{1}', '{2}')",
                                                _id,
                                                obj.IDTrainingCode_K,
                                                obj.Status_K))
        End If
        obj.Status_K = "Actual"
        If Not _db.ExistObject(obj) Then
            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlanned_Detail_Content 
                                                (ID, IDTrainingCode, Status)
                                                VALUES('{0}', '{1}', '{2}')",
                                                _id,
                                                obj.IDTrainingCode_K,
                                                obj.Status_K))
        End If
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            Dim obj As New GA_TRM_RegisterPlanned_Detail
            obj.ID_K = _id
            obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
            _db.Delete(obj)
            GridView1.DeleteSelectedRows()
            LoadPlanActual()
            LoadEmployee(DBNull.Value)
            LoadScore()
            LoadResult()
            EnableProgramList()
            EnablePlanActual()
            EnableScore()
            EnableResult()
        End If
    End Sub

    Private Sub GridView2_RowCellStyle(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles GridView2.RowCellStyle
        If Not IsDBNull(e.CellValue) And IsDate(e.Column.FieldName) Then
            If e.CellValue = "Plan" Then
                e.Appearance.BackColor = Color.Yellow
            ElseIf e.CellValue = "Delayed" Then
                e.Appearance.BackColor = Color.Green
            ElseIf e.CellValue = "Completed" Then
                e.Appearance.BackColor = Color.Blue
                e.Appearance.ForeColor = Color.White
            ElseIf e.CellValue = "Cancelled" Then
                e.Appearance.BackColor = Color.Red
            ElseIf e.CellValue = "Not Completed" Then
                e.Appearance.BackColor = Color.Orange
            ElseIf e.CellValue = "New Plan" Then
                e.Appearance.BackColor = Color.Purple
            End If
        End If
    End Sub

    Private Sub GridView2_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView2.CellValueChanged
        If GridView2.Editable And e.Column.ReadOnly = False Then
            '---------
            Dim hObj As New GA_TRM_RegisterPlanned
            hObj.ID_K = _id
            _db.GetObject(hObj)
            If GridView2.GetFocusedRowCellValue("Status") = "Plan" Then
                If hObj.DateApprove > Date.MinValue Then
                    ShowWarning("Kế hoạch đã được Approved, không thể sửa !")
                    ReturnOldValue(GridView2)
                    Return
                End If
            ElseIf GridView2.GetFocusedRowCellValue("Status") = "Actual" Then
                If hObj.DateApprove = Date.MinValue Then
                    ShowWarning("Kế hoạch chưa được Approved nên chưa được nhập thực tế !")
                    ReturnOldValue(GridView2)
                    Return
                End If
            End If
            '---------
            If IsDate(e.Column.FieldName) Then
                If GridView2.GetFocusedRowCellValue("Status") = "Plan" Then
                    If Not IsDBNull(e.Value) Then
                        If e.Value <> "Plan" And e.Value <> "X" Then
                            ShowWarning("Dòng này chỉ được nhập 'Plan' !")
                            ReturnOldValue(GridView2)
                            Return
                        End If
                    End If
                ElseIf GridView2.GetFocusedRowCellValue("Status") = "Actual" Then
                    If Not IsDBNull(e.Value) Then
                        If e.Value = "Plan" And e.Value <> "X" Then
                            ShowWarning("Dòng này phải nhập khác 'Plan' !")
                            ReturnOldValue(GridView2)
                            Return
                        End If
                    End If
                End If

                Dim obj As New GA_TRM_RegisterPlanned_Detail_Time
                obj.ID_K = _id
                obj.IDTrainingCode_K = GridView2.GetFocusedRowCellValue("IDTrainingCode")
                obj.Status_K = GridView2.GetFocusedRowCellValue("Status")
                obj.Month_K = Date.Parse(e.Column.FieldName)
                If IsDBNull(e.Value) Then
                    _db.Delete(obj)
                ElseIf e.Value = "X" And GridView2.ActiveEditor.OldEditValue IsNot DBNull.Value Then
                    _db.Delete(obj)
                ElseIf e.Value.ToString.ToUpper <> "X" Then
                    obj.State = e.Value
                    If _db.ExistObject(obj) Then
                        _db.Update(obj)
                    Else
                        _db.Insert(obj)
                    End If
                End If
            Else
                Dim para(0) As SqlClient.SqlParameter
                para(0) = New SqlClient.SqlParameter("@Value", e.Value)
                _db.ExecuteNonQuery(String.Format("UPDATE GA_TRM_RegisterPlanned_Detail_Content
                                                   SET {0} = @Value
                                                   WHERE ID = '{1}'
                                                   AND IDTrainingCode = '{2}'
                                                   AND Status = '{3}'",
                                                   e.Column.FieldName,
                                                   _id,
                                                   GridView2.GetFocusedRowCellValue("IDTrainingCode"),
                                                   GridView2.GetFocusedRowCellValue("Status")), para)
            End If
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

    Private Sub GridView1_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView1.RowClick
        LoadEmployee(GridView1.GetFocusedRowCellValue("IDTrainingCode"))

        Dim obj As New GA_TRM_RegisterPlanned
        obj.ID_K = _id
        _db.GetObject(obj)
        'If (obj.DateCheck = DateTime.MinValue Or
        '(obj.DateCheckKQ = DateTime.MinValue And obj.DateApprove > DateTime.MinValue)) And
        '    obj.CurrentMail = CurrentUser.Mail Then
        If obj.DateCheck = Date.MinValue And obj.CurrentMail = CurrentUser.Mail Then
            'Chỉ Edit khi là Prepared hoặc Checked
            EnableEmployee()
        End If
    End Sub

    Private Sub GridView3_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView3.CellValueChanged
        If GridView3.Editable And e.Column.ReadOnly = False Then
            Dim obj As New GA_TRM_RegisterPlanned_Detail_Employee
            obj.ID_K = _id
            If e.RowHandle < 0 Then
                If IsDBNull(GridView3.GetFocusedRowCellValue("IDTrainingCode")) Then
                    obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
                    obj.EmpID_K = e.Value
                    If Not _db.ExistObject(obj) Then
                        _db.Insert(obj)
                        GridView3.SetFocusedRowCellValue("IDTrainingCode", obj.IDTrainingCode_K)

                        Dim dObj As New GA_TRM_RegisterPlanned_Detail_Employee_Score
                        dObj.ID_K = _id
                        dObj.IDTrainingCode_K = obj.IDTrainingCode_K
                        dObj.EmpID_K = obj.EmpID_K
                        dObj.LevelOfCompetency_K = "Current"
                        If Not _db.ExistObject(dObj) Then
                            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlanned_Detail_Employee_Score 
                                                                (ID, IDTrainingCode, EmpID, LevelOfCompetency)
                                                                VALUES ('{0}', '{1}', '{2}', '{3}')",
                                                                _id,
                                                                dObj.IDTrainingCode_K,
                                                                dObj.EmpID_K,
                                                                dObj.LevelOfCompetency_K))
                        End If
                        dObj.LevelOfCompetency_K = "Plan"
                        If Not _db.ExistObject(dObj) Then
                            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlanned_Detail_Employee_Score 
                                                                (ID, IDTrainingCode, EmpID, LevelOfCompetency)
                                                                VALUES ('{0}', '{1}', '{2}', '{3}')",
                                                                _id,
                                                                dObj.IDTrainingCode_K,
                                                                dObj.EmpID_K,
                                                                dObj.LevelOfCompetency_K))
                        End If
                        dObj.LevelOfCompetency_K = "Result"
                        If Not _db.ExistObject(dObj) Then
                            _db.ExecuteNonQuery(String.Format(" INSERT INTO GA_TRM_RegisterPlanned_Detail_Employee_Score 
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
            _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlanned_Detail_Employee
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

    Private Sub slueEmployee_EditValueChanged(sender As Object, e As EventArgs) Handles slueEmployee.EditValueChanged
        Dim lc As SearchLookUpEdit = CType(sender, SearchLookUpEdit)
        Dim obj As New GA_TRM_RegisterPlanned_Detail_Employee
        obj.ID_K = _id
        obj.IDTrainingCode_K = GridView1.GetFocusedRowCellValue("IDTrainingCode")
        obj.EmpID_K = lc.Properties.View.GetFocusedRowCellValue("ECode")
        If _db.ExistObject(obj) Then
            'Dim oldVal As Object = GridView3.ActiveEditor.OldEditValue
            'GridView3.Columns("EmpID").OptionsColumn.ReadOnly = True
            'GridView3.SetFocusedRowCellValue("EmpID", oldVal)
            'GridView3.Columns("EmpID").OptionsColumn.ReadOnly = False
            ReturnOldValue(GridView3)
            ShowWarning("Nhân viên này đã được chọn trước đó !")
        Else
            GridView3.SetFocusedRowCellValue("EName", lc.Properties.View.GetFocusedRowCellValue("EName"))
        End If
    End Sub

    Private Sub btnDeleteEmp_Click(sender As Object, e As EventArgs) Handles btnDeleteEmp.Click
        If ShowQuestionDelete() = Windows.Forms.DialogResult.Yes Then
            Dim obj As New GA_TRM_RegisterPlanned_Detail_Employee
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

    Private Sub btnShowEmp_Click(sender As Object, e As EventArgs) Handles btnShowEmp.Click
        LoadEmployee(DBNull.Value)
    End Sub

    Private Sub GridView4_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView4.CellValueChanged
        If GridView4.Editable And e.Column.ReadOnly = False Then
            If IsNumeric(e.Value) = False Then
                ShowWarning("Phải là định dạng số và > 0 !")
                ReturnOldValue(GridView4)
                Return
            End If
            '---------
            Dim hObj As New GA_TRM_RegisterPlanned
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
            Dim obj As New GA_TRM_RegisterPlanned_Detail_Employee_Score
            obj.ID_K = _id
            obj.IDTrainingCode_K = GetTrainingCode(e.Column.FieldName)
            obj.EmpID_K = GridView4.GetFocusedRowCellValue("EmpID")
            obj.LevelOfCompetency_K = GridView4.GetFocusedRowCellValue("LevelOfCompetency")
            If _db.ExistObject(obj) Then
                Dim para(0) As SqlClient.SqlParameter
                para(0) = New SqlClient.SqlParameter("@Value", e.Value)
                _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlanned_Detail_Employee_Score
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
        Dim codeID As Object = _db.ExecuteScalar(String.Format("SELECT TrainingCode
                                                                FROM GA_TRM_ProgramMaster
                                                                WHERE TrainingProgram LIKE N'%{0}%'",
                                                                trainingProg))
        Return codeID
    End Function

    Private Sub BandedGridView1_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles BandedGridView1.CellValueChanged
        If BandedGridView1.Editable And e.Column.ReadOnly = False Then
            '---------
            Dim hObj As New GA_TRM_RegisterPlanned
            hObj.ID_K = _id
            _db.GetObject(hObj)
            If e.Column.FieldName = "Level3TransferEvaluate" Then
                If hObj.DateApprove = Date.MinValue Then
                    ShowWarning("Kế hoạch chưa được Approved nên chưa được đánh giá kết quả !")
                    ReturnOldValue(BandedGridView1)
                    Return
                End If
            End If
            '---------
            If IsDate(BandedGridView1.GetFocusedRowCellValue("ToDate")) Then
                If Date.Now < Date.Parse(BandedGridView1.GetFocusedRowCellValue("ToDate")).AddMonths(6) Then
                    ShowWarning("Phải đủ 6 tháng mới được đánh giá kết quả")
                    ReturnOldValue(BandedGridView1)
                    Return
                End If
            End If
            Dim para(0) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@Value", e.Value)
            _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlanned_Detail_Employee
                                                SET {0} = @Value
                                                WHERE ID = '{1}'
                                                AND IDTrainingCode = '{2}'
                                                AND EmpID = '{3}'",
                                                e.Column.FieldName,
                                                _id,
                                                BandedGridView1.GetFocusedRowCellValue("IDTrainingCode"),
                                                BandedGridView1.GetFocusedRowCellValue("EmpID")),
                                                para)
            If e.Column.FieldName = "Level3TransferEvaluate" Then
                Dim confVal As String = ""
                If Not IsDBNull(e.Value) Then
                    confVal = CurrentUser.UserID + " - At: " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm")
                Else
                    confVal = ""
                End If
                _db.ExecuteNonQuery(String.Format(" UPDATE GA_TRM_RegisterPlanned_Detail_Employee
                                                    SET ConfirmedBy = '{0}'
                                                    WHERE ID = '{1}'
                                                    AND IDTrainingCode = '{2}'
                                                    AND EmpID = '{3}'",
                                                    confVal,
                                                    _id,
                                                    BandedGridView1.GetFocusedRowCellValue("IDTrainingCode"),
                                                    BandedGridView1.GetFocusedRowCellValue("EmpID")))
                BandedGridView1.SetFocusedRowCellValue("ConfirmedBy", confVal)
            End If
        End If
    End Sub

    Sub ReturnOldValue(gridV As GridView)
        Dim oldVal As Object = gridV.ActiveEditor.OldEditValue
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = True
        gridV.SetFocusedRowCellValue(gridV.FocusedColumn.FieldName, oldVal)
        gridV.Columns(gridV.FocusedColumn.FieldName).OptionsColumn.ReadOnly = False
    End Sub

    Private Sub btnReverse_Click(sender As Object, e As EventArgs) Handles btnReverse.Click
        Try
            _db.BeginTransaction()
            'Đóng ID cũ
            Dim obj As New GA_TRM_RegisterPlanned
            obj.ID_K = _id
            _db.GetObject(obj)
            obj.Method = "Reverse"
            obj.CurrentMail = ""
            _db.Update(obj)

            'Tạo ID mới
            Dim para(1) As SqlClient.SqlParameter
            para(0) = New SqlClient.SqlParameter("@oldID", _id)
            _id = CreateNewID()
            para(1) = New SqlClient.SqlParameter("@newID", _id)
            _db.ExecuteStoreProcedure("sp_GA_TRM_Reverse", para)
            _db.Commit()

            Dim newObj As New GA_TRM_RegisterPlanned
            newObj.ID_K = _id
            _db.GetObject(newObj)
            ConfirmUpdateOutlook(Submit.Info, Confirm.Reverse, "", newObj)
            LoadHead()

            'ShowSuccess("Kế hoạch đã được Reverse.")
        Catch ex As Exception
            _db.RollBack()
            ShowWarning(ex.Message)
        End Try
    End Sub

    Private Sub btnPrintReport_Click(sender As Object, e As EventArgs) Handles btnPrintReport.Click
        '----------------------- F030 ----------------------
        'Dim dataF030 = _db.FillDataTable("SELECT
        '    CAST(NULL AS VARCHAR) AS EmpID,
        '    CAST(NULL AS NVARCHAR) AS EmpName,
        '    CAST(NULL AS VARCHAR) AS Level1EvalYes,
        '    CAST(NULL AS VARCHAR) AS Level1EvalNo,
        '    CAST(NULL AS INT) AS Level2Point,
        '    CAST(NULL AS VARCHAR) AS Level2EvalOK,
        '    CAST(NULL AS VARCHAR) AS Level2EvalNG,
        '    CAST(NULL AS VARCHAR) AS Level3EvalOK,
        '    CAST(NULL AS VARCHAR) AS Level3EvalNG,
        '    CAST(NULL AS NVARCHAR) AS Remark
        '")
        'dataF030.TableName = "Report_Training_HR-F-030"
        'dataF030.WriteXmlSchema("Report_Training_HR-F-030.xsd")
        Dim dtF030 = _db.FillDataTable(String.Format("SELECT IDTrainingCode, TrainingProgram,
                                                    TrainingMan, FromDate, ToDate, FromTime, ToTime, Duration, Place
                                                    FROM GA_TRM_RegisterPlanned_Detail
                                                    WHERE ID = '{0}'
                                                    AND IDTrainingCode LIKE 'D%'", _id))
        For Each r As DataRow In dtF030.Rows
            Dim dtDetail = _db.FillDataTable(String.Format("SELECT h.EmpID, d.EmpName, 
		                IIF(h.Level1ReactionEvaluate = 'Yes', 'X', '') AS Level1EvalYes,
		                IIF(h.Level1ReactionEvaluate = 'No', 'X', '') AS Level1EvalNo,
		                h.Level2LearningPoint AS Level2Point,
		                IIF(h.Level2LearningEvaluate = 'OK', 'OK', '') AS Level2EvalOK,
		                IIF(h.Level2LearningEvaluate = 'NG', 'NG', '') AS Level2EvalNG,
		                IIF(h.Level3TransferEvaluate = 'OK', 'OK', '') AS Level3EvalOK,
		                IIF(h.Level3TransferEvaluate = 'NG', 'NG', '') AS Level3EvalNG,
		                h.Remark
                FROM GA_TRM_RegisterPlanned_Detail_Employee AS h
                LEFT JOIN OT_Employee AS d
                ON h.EmpID = d.EmpID
                WHERE ID = '{0}'
                AND IDTrainingCode = '{1}'",
                _id,
                r("IDTrainingCode")))
            Dim rpF030 As New RpF030
            rpF030.DataSource = dtDetail
            rpF030.DataMember = ""
            rpF030.Parameters("paraTrainingProgram").Value = r("TrainingProgram")
            rpF030.Parameters("paraTrainingMan").Value = r("TrainingMan")
            rpF030.Parameters("paraFromDate").Value = r("FromDate")
            rpF030.Parameters("paraToDate").Value = r("ToDate")
            rpF030.Parameters("paraFromTime").Value = r("FromTime")
            rpF030.Parameters("paraToTime").Value = r("ToTime")
            rpF030.Parameters("paraDuration").Value = r("Duration")
            rpF030.Parameters("paraPlace").Value = r("Place")
            Dim RePrToolF030 As New ReportPrintTool(rpF030)
            RePrToolF030.ShowPreview()
        Next

        '-------------------- F028 ------------------------
        'Dim dataF028 As DataTable = _db.FillDataTable("SELECT
        '    CAST(NULL AS INT) AS STT,
        '    CAST(NULL AS NVARCHAR) AS TrainingField,
        '    CAST(NULL AS NVARCHAR) AS TrainingCode,
        '    CAST(NULL AS NVARCHAR) AS TrainingProgram,
        '    CAST(NULL AS NVARCHAR) AS TrainingMethod,
        '    CAST(NULL AS NVARCHAR) AS Trainer,
        '    CAST(NULL AS NVARCHAR) AS Trainee,
        '    CAST(NULL AS NVARCHAR) AS Status,
        '    CAST(NULL AS INT) AS Trainees,
        '    CAST(NULL AS INT) AS Cost,
        '    CAST(NULL AS NVARCHAR) AS Jan,
        '    CAST(NULL AS NVARCHAR) AS Feb,
        '    CAST(NULL AS NVARCHAR) AS Mar,
        '    CAST(NULL AS NVARCHAR) AS Apr,
        '    CAST(NULL AS NVARCHAR) AS May,
        '    CAST(NULL AS NVARCHAR) AS Jun,
        '    CAST(NULL AS NVARCHAR) AS Jul,
        '    CAST(NULL AS NVARCHAR) AS Aug,
        '    CAST(NULL AS NVARCHAR) AS Sep,
        '    CAST(NULL AS NVARCHAR) AS Oct,
        '    CAST(NULL AS NVARCHAR) AS Nov,
        '    CAST(NULL AS NVARCHAR) AS Dec
        '")
        'dataF028.TableName = "Report_Training_HR-F-028"
        'dataF028.WriteXmlSchema("Report_Training_HR-F-028.xsd")
        Dim startD As Date = GetStartDayOfYear(dteDate.DateTime)
        Dim endD As Date = GetEndDayOfYear(dteDate.DateTime)
        Dim col As String = ""
        While startD <= endD
            col += startD.ToString("[MMM]") + ","
            startD = startD.AddMonths(1)
        End While
        col = col.Substring(0, col.Length - 1)
        Dim para(1) As SqlClient.SqlParameter
        para(0) = New SqlClient.SqlParameter("@id", _id)
        para(1) = New SqlClient.SqlParameter("@col", col)
        Dim dtRp = _db.ExecuteStoreProcedureTB("sp_GA_TRM_ReportF028", para)
        Dim count = 1
        If dtRp.Rows.Count > 0 Then
            For r = 0 To dtRp.Rows.Count - 1
                dtRp.Rows(r)("STT") = count
                If r < dtRp.Rows.Count - 1 Then
                    If dtRp.Rows(r)("TrainingCode") = dtRp.Rows(r + 1)("TrainingCode") Then
                        dtRp.Rows(r + 1)("STT") = count
                    Else
                        count += 1
                    End If
                End If
            Next
            Dim rpF028 As New RpF028
            rpF028.DataSource = dtRp
            rpF028.DataMember = ""
            Dim dtDep = _db.FillDataTable(String.Format("SELECT TOP 1  '(' + DepartmentID + ')' AS DepartmentID, 
                                                        '('+ Note + ')' AS Note
                                                    FROM Main_Department
                                                    WHERE DepartmentName = '{0}'",
                                                    txtSection.Text))
            rpF028.Parameters("paraDepNote").Value = dtDep.Rows(0)("Note")
            rpF028.Parameters("paraDepID").Value = dtDep.Rows(0)("DepartmentID")
            rpF028.Parameters("paraYear").Value = GetStartDayOfYear(dteDate.DateTime)
            Dim RePrToolF028 As New ReportPrintTool(rpF028)
            RePrToolF028.ShowPreview()
        End If
    End Sub

    Private Sub BandedGridView1_RowStyle(sender As Object, e As RowStyleEventArgs) Handles BandedGridView1.RowStyle
        If IsDate(BandedGridView1.GetRowCellValue(e.RowHandle, "ToDate")) Then
            If Date.Now >= Date.Parse(BandedGridView1.GetRowCellValue(e.RowHandle, "ToDate")).AddMonths(6) Then
                If IsDBNull(BandedGridView1.GetRowCellValue(e.RowHandle, "Level3TransferEvaluate")) Then
                    e.Appearance.BackColor = Color.Red
                    e.Appearance.ForeColor = Color.White
                ElseIf Not IsDBNull(BandedGridView1.GetRowCellValue(e.RowHandle, "Level3TransferEvaluate")) Then
                    If BandedGridView1.GetRowCellValue(e.RowHandle, "Level3TransferEvaluate") = "" Then
                        e.Appearance.BackColor = Color.Red
                        e.Appearance.ForeColor = Color.White
                    End If
                End If
            End If
        End If
    End Sub
End Class