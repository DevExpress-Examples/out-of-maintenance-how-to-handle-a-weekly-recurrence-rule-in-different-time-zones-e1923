<%@ Page Language="vb" AutoEventWireup="true"  CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.1" Namespace="DevExpress.Web.ASPxScheduler"
    TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v15.1.Core, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraScheduler" TagPrefix="dx" %>
<%@ Register Src="~/DefaultDataSources.ascx" TagName="DefaultDataSources" TagPrefix="dds" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dds:DefaultDataSources runat="server" ID="schedulerDataSource"/>


        <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" OnBeforeExecuteCallbackCommand="OnBeforeExecuteCallbackCommand" OnAppointmentFormShowing="OnAppointmentFormShowing" Width="600px">
            <OptionsBehavior ClientTimeZoneId="AUS Eastern Standard Time" />
            <Storage TimeZoneId="UTC">
                <Appointments>
                </Appointments>
            </Storage>
            <Views>
                <DayView>
                    <TimeRulers>
                        <dx:TimeRuler AlwaysShowTimeDesignator="True" Caption="Eastern Australian">
                        </dx:TimeRuler>
                        <dx:TimeRuler AlwaysShowTimeDesignator="True" Caption="GMT"
                            UseClientTimeZone="False" TimeZoneId="UTC">

                        </dx:TimeRuler>
                    </TimeRulers>
                    <DayViewStyles ScrollAreaHeight="400px">
                    </DayViewStyles>
                </DayView>
                <WorkWeekView>
                    <TimeRulers>
                        <dx:TimeRuler>
                        </dx:TimeRuler>
                    </TimeRulers>
                </WorkWeekView>
            </Views>
        </dxwschs:ASPxScheduler>

    </div>
    </form>
</body>
</html>