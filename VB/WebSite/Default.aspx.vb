Imports System
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Services
Imports DevExpress.Web.ASPxScheduler.Services

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private prevTimeRulerFormatStringService As ITimeRulerFormatStringService
    Private customTimeRulerFormatStringService As CustomTimeRulerFormatStringService

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        CreateTimeRulerFormatStringService()
        ASPxScheduler1.RemoveService(GetType(ITimeRulerFormatStringService))
        ASPxScheduler1.AddService(GetType(ITimeRulerFormatStringService), customTimeRulerFormatStringService)

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        DataHelper.SetupDefaultMappings(ASPxScheduler1)
        DataHelper.ProvideRowInsertion(ASPxScheduler1, schedulerDataSource.AppointmentDataSource)
        schedulerDataSource.AttachTo(ASPxScheduler1)
    End Sub

    Public Sub CreateTimeRulerFormatStringService()
        Me.prevTimeRulerFormatStringService = DirectCast(ASPxScheduler1.GetService(GetType(ITimeRulerFormatStringService)), ITimeRulerFormatStringService)
        Me.customTimeRulerFormatStringService = New CustomTimeRulerFormatStringService(prevTimeRulerFormatStringService)
    End Sub

    Protected Sub OnAppointmentFormShowing(ByVal sender As Object, ByVal e As AppointmentFormEventArgs)
        If e.Container.Appointment.Type = AppointmentType.Pattern Then
            e.Container = New MyAppointmentFormTemplateContainer(DirectCast(sender, ASPxScheduler))
        End If
    End Sub
    Protected Sub OnBeforeExecuteCallbackCommand(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs)
        If e.CommandId = SchedulerCallbackCommandId.AppointmentSave Then
            e.Command = New MyAppointmentFormCallbackCommand(ASPxScheduler1)
        End If
    End Sub

End Class
Public Class MyAppointmentFormCallbackCommand
    Inherits AppointmentFormSaveCallbackCommand

    Public Sub New(ByVal control As ASPxScheduler)
        MyBase.New(control)
    End Sub
    Protected Overrides Function CreateAppointmentFormController(ByVal apt As DevExpress.XtraScheduler.Appointment) As AppointmentFormController
        Return New MyAppointmentFormController(Control, apt)
    End Function
End Class
Public Class MyAppointmentFormTemplateContainer
    Inherits AppointmentFormTemplateContainer

    Public Sub New(ByVal scheduler As ASPxScheduler)
        MyBase.New(scheduler)
    End Sub
    Protected Overrides Function CreateController(ByVal control As ASPxScheduler, ByVal appointment As Appointment) As AppointmentFormController
        Return New MyAppointmentFormController(control, appointment)
    End Function
End Class
Public Class MyAppointmentFormController
    Inherits AppointmentFormController

    Public Sub New(ByVal control As ASPxScheduler, ByVal apt As DevExpress.XtraScheduler.Appointment)
        MyBase.New(control, apt)
        If EditedPattern IsNot Nothing AndAlso EditedPattern.RecurrenceInfo.Type = RecurrenceType.Weekly Then
            Dim transition As Integer = DetectCrossDayTransition(EditedPattern)
            Dim weekDays As WeekDays = EditedPattern.RecurrenceInfo.WeekDays
            If transition < 0 Then
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, True)
            ElseIf transition > 0 Then
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, False)
            End If
        End If
    End Sub
    Public Overrides Sub ApplyRecurrence(ByVal patternCopy As Appointment)
        MyBase.ApplyRecurrence(patternCopy)
    End Sub
    Protected Overrides Sub ApplyChangesCore()
        If EditedPattern IsNot Nothing AndAlso EditedPattern.RecurrenceInfo.Type = RecurrenceType.Weekly Then
            Dim transition As Integer = DetectCrossDayTransition(EditedPattern)
            Dim weekDays As WeekDays = EditedPattern.RecurrenceInfo.WeekDays
            If transition < 0 Then
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, False)
            ElseIf transition > 0 Then
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, True)
            End If
        End If
        MyBase.ApplyChangesCore()
    End Sub
    Protected Overridable Function DetectCrossDayTransition(ByVal pattern As Appointment) As Integer
        Dim helper As New TimeZoneHelper(Control.OptionsBehavior.ClientTimeZoneId)
        Dim serverTime As Date = EditedPattern.Start
        Dim clientTime As Date = helper.ToClientTime(EditedPattern.Start)
        If serverTime.Date < clientTime.Date Then
            Return -1
        ElseIf serverTime.Date > clientTime.Date Then
            Return 1
        End If
        Return 0
    End Function
    #Region "#shiftweekdays"
    Private Function ShiftWeekDays(ByVal weekDays As WeekDays, ByVal shiftToNextDay As Boolean) As WeekDays
        Dim sourceWeekDays As UInteger = CUInt(weekDays)
        If shiftToNextDay Then
            Return CType(((sourceWeekDays >> 6) And (CUInt(WeekDays.EveryDay))) Or (((sourceWeekDays << 1) And (CUInt(WeekDays.EveryDay)))), WeekDays)
        Else
            Return CType(((sourceWeekDays << 6) And (CUInt(WeekDays.EveryDay))) Or (((sourceWeekDays >> 1) And (CUInt(WeekDays.EveryDay)))), WeekDays)
        End If
    End Function
    #End Region ' #shiftweekdays

End Class

Public Class CustomTimeRulerFormatStringService
    Inherits TimeRulerFormatStringServiceWrapper

    Public Sub New(ByVal service As ITimeRulerFormatStringService)
        MyBase.New(service)
    End Sub

    Public Overrides Function GetHourFormat(ByVal ruler As TimeRuler) As String
        Return "HH:mm"
    End Function
    Public Overrides Function GetHourOnlyFormat(ByVal ruler As TimeRuler) As String
        Return "HH"
    End Function
    Public Overrides Function GetTimeDesignatorOnlyFormat(ByVal ruler As TimeRuler) As String
        Return "   "
    End Function
End Class
