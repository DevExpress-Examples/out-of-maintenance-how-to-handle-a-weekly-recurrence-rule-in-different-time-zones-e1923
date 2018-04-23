using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using System.Collections;
using System.Data;
using System.Drawing;

public partial class DefaultDataSources : System.Web.UI.UserControl {
	bool initAppointments = true;
	string uniqueSessionPrefix = String.Empty;

	public bool InitAppointments { get { return initAppointments; } set { initAppointments = value; } }
	public string UniqueSessionPrefix { get { return uniqueSessionPrefix; } set { uniqueSessionPrefix = value; } }
	string CustomEventsSessionName { get { return UniqueSessionPrefix + "CustomEvents"; } }
	string CustomResourcesSessionName { get { return UniqueSessionPrefix + "CustomResources"; } }

    public DataSourceControl AppointmentDataSource { get { return temporaryAppointmentDataSource; } }
    public DataSourceControl ResourceDataSource { get { return temporaryResourceDataSource; } }

	public void AttachTo(ASPxScheduler control) {
		control.ResourceDataSource = this.ResourceDataSource;
		control.AppointmentDataSource = this.AppointmentDataSource;
		control.DataBind();
	}

	protected void Page_Load(object sender, EventArgs e) {
	}

	#region Site Mode implementation
	protected void temporaryAppointmentDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
		CustomEventList events = GetCustomEvents();
		e.ObjectInstance = new CustomEventDataSource(events);
	}
	protected void temporaryResourceDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
		CustomResourceList resources = GetCustomResources();
		e.ObjectInstance = new CustomResourceDataSource(resources);
	}
	public CustomEventList GetCustomEvents() {
		CustomEventList events = Session[CustomEventsSessionName] as CustomEventList;
		if (events != null)
			return events;

        events =  new CustomEventList();
		Session[CustomEventsSessionName] = events;
		return events;
	}
	protected CustomResourceList GetCustomResources() {
		CustomResourceList resources = Session[CustomResourcesSessionName] as CustomResourceList;
		if (resources != null)
			return resources;

        resources = CreateCustomResurces();		
		Session[CustomResourcesSessionName] = resources;
		return resources;
	}
    public static CustomResourceList CreateCustomResurces() {
        CustomResourceList resources = new CustomResourceList();
        resources.Add(CreateCustomResource(1, "SL500 Roadster"));
        resources.Add(CreateCustomResource(2, "CLK55 AMG Cabriolet"));
        resources.Add(CreateCustomResource(3, "C230 Kompressor Sport Coupe"));
        resources.Add(CreateCustomResource(4, "530i"));
        resources.Add(CreateCustomResource(5, "Corniche"));
        return resources;
    }
    static CustomResource CreateCustomResource(int id, string caption) {
        CustomResource result = new CustomResource();
        result.Id = id;
        result.Caption = caption;
        return result;
    }

	#endregion
}
