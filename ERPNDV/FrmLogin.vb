﻿Imports DevExpress.Skins
Imports LibEntity
Imports PublicUtility
Imports System.IO

Public Class FrmLogin

#Region "Variable"
    Dim _db As DBSql
    Public IsConfirm As Boolean = False
#End Region

#Region "User function"

#End Region

#Region "Form function"

    Private Sub bttExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttExit.Click
        Application.ExitThread()
        Application.Exit()
    End Sub

    Private Sub FrmLogin_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{Tab}")
        End If
        If e.Control And e.KeyCode = Keys.S Then
            Dim frm As New FrmConfig
            frm.ShowDialog()
        End If
    End Sub


    Private Sub bttLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttLogin.Click

        If txtUserName.Text.Trim() = "" Then
            'ShowWarningNotEmpty("UserName")
            txtUserName.Focus()
            Return
        End If
        If txtPassword.Text.Trim() = "" Then
            'ShowWarningNotEmpty("Password")
            txtPassword.Focus()
            Return
        End If

        Dim config = New ConfigNDV()
        If File.Exists(Application.StartupPath + PublicConst.CONFIG) Then
            config = BinaryDeserialize(Application.StartupPath + PublicConst.CONFIG)
            If config.SQL_ERP_NDV <> "" Then
                PublicConst.SQL_DB_ERP_NDV = DecryptPassword(config.SQL_ERP_NDV)
                PublicConst.SQL_DB_ERP_NDV_Use = DecryptPassword(config.SQL_ERP_NDV)
            End If
            If config.SQL_Factory <> "" Then
                PublicConst.SQL_DB_Factory = DecryptPassword(config.SQL_Factory)
            End If
            If config.SQL_FPICS <> "" Then
                PublicConst.SQL_DB_FPICS = DecryptPassword(config.SQL_FPICS)
            End If
            If config.DB2_AS400 <> "" Then
                PublicConst.DB2_DB_AS400 = DecryptPassword(config.DB2_AS400)
            End If
            If config.SQL_ThaiSon <> "" Then
                PublicConst.SQL_DB_ThaiSon = DecryptPassword(config.SQL_ThaiSon)
            End If
        End If

        If RadioGroup1.Properties.Items(RadioGroup1.SelectedIndex).Description = "VI" Then
            PublicConst.Language = PublicConst.EnumLanguage.VietNam
        Else
            PublicConst.Language = PublicConst.EnumLanguage.English
        End If

        Dim userID As String = Microsoft.VisualBasic.LCase(txtUserName.Text)
        Dim password As String = txtPassword.Text
        Dim obj As New Main_User()

        If userID = PublicConst.AdminUser And password = PublicConst.AdminPassword Then
            CurrentUser.UserID = PublicConst.AdminID
            CurrentUser.Password = PublicConst.AdminPassword
            CurrentUser.UserName = PublicConst.AdminUser
            CurrentUser.FullName = PublicConst.AdminName
            'CType(Owner, FrmMain).login = True
            FrmMain.login = True

            Me.Close()
            Return
        End If

        Try
            _db = New DBSql(PublicConst.EnumServers.NDV_SQL_ERP_NDV)
            If Not _db.CheckConnection() Then
                'CType(Owner, FrmMain).login = False
                FrmMain.login = False
                ShowWarning("Can't connect to server. Please contact admin IT: ")
                Return
            End If

        Catch ex As Exception
            FrmMain.login = False
            ShowWarning("Can't connect to server. Please contact admin IT: " & ex.Message)
            Return
        End Try

        Dim sql As String = String.Format("select * from {0} where UserName='{1}'",
                                          PublicTable.Table_Main_User,
                                          userID.Replace("'", ""))

        obj = _db.GetObject(Of Main_User)(sql)
        If LCase(obj.UserName) = userID And (DecryptPassword(obj.Password) = password Or password = obj.Password) Then
            CurrentUser.UserID = obj.UserID_K
            CurrentUser.UserName = obj.UserName
            CurrentUser.Password = DecryptPassword(obj.Password)

            Dim objmail As New OT_Mail
            objmail.EmpID_K = obj.UserID_K
            _db.GetObject(objmail)
            Dim objemp As New OT_Employee
            objemp.EmpID_K = obj.UserID_K
            _db.GetObject(objemp)
            Dim objC As New Main_Connection
            objC.ID_K = "TEMP"
            _db.GetObject(objC)

            CurrentUser.FullName = objemp.EmpName
            CurrentUser.Sex = objemp.Sex
            CurrentUser.Shift = objemp.Shift
            CurrentUser.GroupName = objemp.GroupName
            CurrentUser.Section = objemp.Section

            CurrentUser.PCNO = PublicFunction.GetHostName
            CurrentUser.GlobalID = obj.GlobalID
            CurrentUser.GlobalPass = obj.GlobalPass
            CurrentUser.ServerIP = _db.Server
            CurrentUser.TempFolder = objC.StringConnection

            If objmail.Mail <> "" Then
                CurrentUser.Mail = objmail.Mail
                CurrentUser.MailLotus = objmail.MailLotus
                CurrentUser.Mailoutlook = objmail.MailOutlook
            End If
            CurrentUser.Dept = objemp.Dept
            CurrentUser.Observation = objemp.Observation
            CurrentUser.SortSection = objemp.SectionSort

            'Set riêng cho Japanese
            If CurrentUser.UserID.Contains("J") Then
                If CurrentUser.UserID = "J0022" Then
                    CurrentUser.Observation = "Business control"
                    CurrentUser.SortSection = "B"
                    CurrentUser.Dept = "B"
                    CurrentUser.Section = "Business control"
                    CurrentUser.FullName = "Moro, Yuichi"
                    CurrentUser.Mail = "yuichi.moro@nitto.com"
                ElseIf CurrentUser.UserID = "J0023" Then
                    CurrentUser.Observation = "General Manager"
                    CurrentUser.SortSection = "CIS"
                    CurrentUser.Dept = "CIS"
                    CurrentUser.Section = "CIS Business Development"
                    CurrentUser.FullName = "Tamai, Seiichi"
                    CurrentUser.Mail = "seiichi.tamai@nitto.com"
                ElseIf CurrentUser.UserID = "J0024" Then
                    CurrentUser.Observation = "Manager"
                    CurrentUser.SortSection = "TE"
                    CurrentUser.Dept = "T"
                    CurrentUser.Section = "Technical Engineering"
                    CurrentUser.FullName = "Saijo, Hiroyuki"
                    CurrentUser.Mail = "hiroyuki.saijo@nitto.com"
                ElseIf CurrentUser.UserID = "J0025" Then
                    CurrentUser.Observation = "General Director"
                    CurrentUser.SortSection = "F"
                    CurrentUser.Dept = "F"
                    CurrentUser.Section = "Factory"
                    CurrentUser.FullName = "Yoshifumi SHINOGI"
                    CurrentUser.Mail = "yoshifumi.shinogi@nitto.com"
                ElseIf CurrentUser.UserID = "J0026" Then
                    CurrentUser.Observation = "Support"
                    CurrentUser.SortSection = "F"
                    CurrentUser.Dept = "F"
                    CurrentUser.Section = "Factory"
                    CurrentUser.FullName = "Ichikawa, Kazushi"
                    CurrentUser.Mail = "kazushi.ichikawa@nitto.com"
                Else
                    CurrentUser.FullName = obj.FullName
                End If
            End If
            'save setting user
            Try

                SaveSetting("ERPNDV", "Login", "UserName", obj.UserName)
                SaveSetting("ERPNDV", "Login", "UserID", obj.UserID_K)
                SaveSetting("ERPNDV", "Login", "Password", txtPassword.Text)
                SaveSetting("ERPNDV", "Login", "Remember", ckoRemember.Checked)

            Catch ex As Exception
                'Write log file
            End Try
            'FrmMain.login = True


            Dim frm As New FrmMain
            frm.Show()

            Me.Visible = False
        Else
            ShowWarningNotCorrect("Username or password")
            txtUserName.Focus()
            Return
        End If


    End Sub

    Private Sub txtPassword_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            bttLogin.PerformClick()
        End If
    End Sub

    Private Sub FrmLogin_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Dim oSkin As New LookAndFeelSettingsHelper
        oSkin.RestoreSettings()
        DevExpress.UserSkins.BonusSkins.Register()
        SkinManager.EnableFormSkins()

        CurrentUser.PCNO = PublicFunction.GetHostName

        Dim separators As String = ","
        Dim commands As String = Microsoft.VisualBasic.Command()
        Dim args() As String = commands.Split(separators.ToCharArray)

        Dim myPath As String = Application.StartupPath
        Dim localDisk As String = Microsoft.VisualBasic.Left(myPath, 1)
        If localDisk <> "C" And localDisk <> "D" And
              CurrentUser.PCNO <> "V00365" And CurrentUser.PCNO <> "V00348" Then
            ShowWarning("Bạn chỉ được phép chạy chương trình trên Local (ổ C:\Programs_NDV\ERPNDV\). Vui lòng liên hệ IT.")
            Application.Exit()
        End If

        If args.Length > 2 Then
            IsConfirm = True
        Else
            If args.Length = 1 Then
                If Not myPath.Contains("Debug") And CurrentUser.PCNO <> "V00365" Then
                    ShowWarning("Bạn phải mở bằng ERPUpdate để được cập nhật mới nhất. Please open ERPUpdate !")
                    Application.ExitThread()
                    Application.Exit()
                End If
            End If
        End If

            lblVersion.Text = PublicConst.Version

        PublicConst.Language = PublicConst.EnumLanguage.VietNam

        txtUserName.Text = GetSetting("ERPNDV", "Login", "UserName")
        Dim remember As String = GetSetting("ERPNDV", "Login", "Remember")
        If remember = "True" Then
            txtPassword.Text = GetSetting("ERPNDV", "Login", "Password")
            ckoRemember.Checked = True
        End If
        Dim id As String = GetSetting("ERPNDV", "Login", "UserID")

        If txtUserName.Text <> "" Then
            txtPassword.Focus()
        Else
            txtUserName.Focus()
        End If
        If txtPassword.Text <> "" Then
            bttLogin.Focus()
        End If
        If IsConfirm Then
            bttLogin.PerformClick()
        End If
        txtUserName.Focus()



        lblCompany.Text = String.Format("Copyright(C) 2011-{0} Nitto Denko Viet Nam", Date.Now.Year)
    End Sub


#End Region

    Private Sub bttSetting_Click(sender As System.Object, e As System.EventArgs) Handles bttSetting.Click
        Dim frm As New FrmConfig
        frm.ShowDialog()
    End Sub

End Class
