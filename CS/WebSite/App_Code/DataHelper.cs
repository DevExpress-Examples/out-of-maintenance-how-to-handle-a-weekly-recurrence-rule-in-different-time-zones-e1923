using System;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;

/// <summary>
/// Summary description for DataHelper
/// </summary>
public static class DataHelper {
    public static void SetupDefaultMappings(ASPxScheduler control) {
        SetupDefaultMappingsSiteMode(control);
    }
    //
    public static void SetupCustomEventsMappings(ASPxScheduler control) {
        SetupDefaultMappingsSiteMode(control);
    }
    
    private static void SetupDefaultMappingsSiteMode(ASPxScheduler control) {
        ASPxSchedulerStorage storage = control.Storage;
        storage.BeginUpdate();
        try {
            ASPxResourceMappingInfo resourceMappings = storage.Resources.Mappings;
            resourceMappings.ResourceId = "Id";
            resourceMappings.Caption = "Caption";

            ASPxAppointmentMappingInfo appointmentMappings = storage.Appointments.Mappings;
            appointmentMappings.AppointmentId = "Id";
            appointmentMappings.Start = "StartTime";
            appointmentMappings.End = "EndTime";
            appointmentMappings.Subject = "Subject";
            appointmentMappings.AllDay = "AllDay";
            appointmentMappings.Description = "Description";
            appointmentMappings.Label = "Label";
            appointmentMappings.Location = "Location";
            appointmentMappings.RecurrenceInfo = "RecurrenceInfo";
            appointmentMappings.ReminderInfo = "ReminderInfo";
            appointmentMappings.ResourceId = "OwnerId";
            appointmentMappings.Status = "Status";
            appointmentMappings.Type = "EventType";
        }
        finally {
            storage.EndUpdate();
        }
    }
    public static void ProvideRowInsertion(ASPxScheduler control, DataSourceControl dataSource) {
        AccessDataSource accessDataSource = dataSource as AccessDataSource;
        if (accessDataSource != null) {
            AccessRowInsertionProvider provider = new AccessRowInsertionProvider();
            provider.ProvideRowInsertion(control, accessDataSource);
            return;
        }

        ObjectDataSource objectDataSource = dataSource as ObjectDataSource;
        if (objectDataSource != null) {
            ObjectDataSourceRowInsertionProvider provider = new ObjectDataSourceRowInsertionProvider();
            provider.ProvideRowInsertion(control, objectDataSource);
        }
    }

}

public class AccessRowInsertionProvider {
    int lastInsertedAppointmentId;

    public void ProvideRowInsertion(ASPxScheduler control, AccessDataSource dataSource) {
        dataSource.Inserted += new SqlDataSourceStatusEventHandler(AppointmentsDataSource_Inserted);
        control.AppointmentRowInserting += new ASPxSchedulerDataInsertingEventHandler(ControlOnAppointmentRowInserting);
        control.AppointmentRowInserted += new ASPxSchedulerDataInsertedEventHandler(ControlOnAppointmentRowInserted);
        control.AppointmentsInserted += new PersistentObjectsEventHandler(ControlOnAppointmentsInserted);
    }

    void ControlOnAppointmentRowInserting(object sender, ASPxSchedulerDataInsertingEventArgs e) {
        // Autoincremented primary key case
        e.NewValues.Remove("ID");
    }
    void ControlOnAppointmentRowInserted(object sender, ASPxSchedulerDataInsertedEventArgs e) {
        // Autoincremented primary key case
        e.KeyFieldValue = this.lastInsertedAppointmentId;
    }
    void AppointmentsDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e) {
        // Autoincremented primary key case
        OleDbConnection connection = (OleDbConnection)e.Command.Connection;
        using (OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY", connection)) {
            this.lastInsertedAppointmentId = (int)cmd.ExecuteScalar();
        }
    }
    void ControlOnAppointmentsInserted(object sender, PersistentObjectsEventArgs e) {
        // Autoincremented primary key case
        int count = e.Objects.Count;
        System.Diagnostics.Debug.Assert(count == 1);
        Appointment apt = (Appointment)e.Objects[0];
        ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
        storage.SetAppointmentId(apt, lastInsertedAppointmentId);
    }
}
public class ObjectDataSourceRowInsertionProvider {

    public void ProvideRowInsertion(ASPxScheduler control, ObjectDataSource dataSource) {
        control.AppointmentInserting += new PersistentObjectCancelEventHandler(ControlOnAppointmentInserting);
    }
    protected void ControlOnAppointmentInserting(object sender, PersistentObjectCancelEventArgs e) {
        ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
        Appointment apt = (Appointment)e.Object;
        storage.SetAppointmentId(apt, "a" + apt.GetHashCode());
    }
}

