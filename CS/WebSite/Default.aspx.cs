using System;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Services;
using DevExpress.Web.ASPxScheduler.Services;

public partial class _Default : System.Web.UI.Page 
{
    ITimeRulerFormatStringService prevTimeRulerFormatStringService;
    CustomTimeRulerFormatStringService customTimeRulerFormatStringService;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        CreateTimeRulerFormatStringService();
        ASPxScheduler1.RemoveService(typeof(ITimeRulerFormatStringService));
        ASPxScheduler1.AddService(typeof(ITimeRulerFormatStringService), customTimeRulerFormatStringService);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        DataHelper.SetupDefaultMappings(ASPxScheduler1);
        DataHelper.ProvideRowInsertion(ASPxScheduler1, schedulerDataSource.AppointmentDataSource);
        schedulerDataSource.AttachTo(ASPxScheduler1);
    }

    public void CreateTimeRulerFormatStringService()
    {
        this.prevTimeRulerFormatStringService = (ITimeRulerFormatStringService)ASPxScheduler1.GetService(typeof(ITimeRulerFormatStringService));
        this.customTimeRulerFormatStringService = new CustomTimeRulerFormatStringService(prevTimeRulerFormatStringService);
    }

    protected void OnAppointmentFormShowing(object sender, AppointmentFormEventArgs e) {
        if(e.Container.Appointment.Type == AppointmentType.Pattern) {
            e.Container = new MyAppointmentFormTemplateContainer((ASPxScheduler)sender);
        }
    }
    protected void OnBeforeExecuteCallbackCommand(object sender, DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs e) {
        if(e.CommandId == SchedulerCallbackCommandId.AppointmentSave) {
            e.Command = new MyAppointmentFormCallbackCommand(ASPxScheduler1);
        }
    }

}
public class MyAppointmentFormCallbackCommand : AppointmentFormSaveCallbackCommand { 
    public MyAppointmentFormCallbackCommand(ASPxScheduler control) : base(control) {
    }
    protected override AppointmentFormController CreateAppointmentFormController(DevExpress.XtraScheduler.Appointment apt) {
        return new MyAppointmentFormController(Control, apt);
    }
}
public class MyAppointmentFormTemplateContainer : AppointmentFormTemplateContainer {
    public MyAppointmentFormTemplateContainer(ASPxScheduler scheduler) : base(scheduler) { 
    }
    protected override AppointmentFormController CreateController(ASPxScheduler control, Appointment appointment) {
        return new MyAppointmentFormController(control, appointment);
    }
}
public class MyAppointmentFormController : AppointmentFormController {
    public MyAppointmentFormController(ASPxScheduler control, DevExpress.XtraScheduler.Appointment apt)
        : base(control, apt) {
        if(EditedPattern != null && EditedPattern.RecurrenceInfo.Type == RecurrenceType.Weekly) {
            int transition = DetectCrossDayTransition(EditedPattern);
            WeekDays weekDays = EditedPattern.RecurrenceInfo.WeekDays;
            if(transition < 0) 
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, true);
            else if(transition > 0)
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, false);
        }
    }
    public override void ApplyRecurrence(Appointment patternCopy) {
        base.ApplyRecurrence(patternCopy);
    }
    protected override void ApplyChangesCore() {
        if(EditedPattern != null && EditedPattern.RecurrenceInfo.Type == RecurrenceType.Weekly) {
            int transition = DetectCrossDayTransition(EditedPattern);
            WeekDays weekDays = EditedPattern.RecurrenceInfo.WeekDays;
            if(transition < 0)
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, false);
            else if(transition > 0)
                EditedPattern.RecurrenceInfo.WeekDays = ShiftWeekDays(weekDays, true);
        }
        base.ApplyChangesCore();
    }
    protected virtual int DetectCrossDayTransition(Appointment pattern) {
        TimeZoneHelper helper = new TimeZoneHelper(Control.OptionsBehavior.ClientTimeZoneId);
        DateTime serverTime = EditedPattern.Start;
        DateTime clientTime = helper.ToClientTime(EditedPattern.Start);
        if(serverTime.Date < clientTime.Date)
            return -1;
        else if(serverTime.Date > clientTime.Date)
            return 1;
        return 0;
    }
    #region #shiftweekdays
    WeekDays ShiftWeekDays(WeekDays weekDays, bool shiftToNextDay) {
        uint sourceWeekDays = (uint)weekDays;
        if(shiftToNextDay)
            return (WeekDays)(((sourceWeekDays >> 6) & ((uint)WeekDays.EveryDay)) | 
                (((sourceWeekDays << 1) & ((uint)WeekDays.EveryDay))));
        else
            return (WeekDays)(((sourceWeekDays << 6) & ((uint)WeekDays.EveryDay)) | 
                (((sourceWeekDays >> 1) & ((uint)WeekDays.EveryDay))));
    }
    #endregion #shiftweekdays

}

public class CustomTimeRulerFormatStringService : TimeRulerFormatStringServiceWrapper
{
    public CustomTimeRulerFormatStringService(ITimeRulerFormatStringService service)
        : base(service)
    {
    }

    public override string GetHourFormat(TimeRuler ruler)
    {
        return "HH:mm";
    }
    public override string GetHourOnlyFormat(TimeRuler ruler)
    {
        return "HH";
    }
    public override string GetTimeDesignatorOnlyFormat(TimeRuler ruler)
    {
        return "   ";
    }
}
