<%@ Control Language="vb" AutoEventWireup="true" CodeFile="DefaultDataSources.ascx.vb" Inherits="DefaultDataSources" %>
<asp:AccessDataSource ID="dbAppointmentDataSource" runat="server" DataFile="~/App_Data/CarsDB.mdb"
	DeleteCommand="DELETE FROM [CarScheduling] WHERE [ID] = ?"
	InsertCommand="INSERT INTO [CarScheduling] ([CarId], [Status], [Subject], [Description], [Label], [StartTime], [EndTime], [Location], [AllDay], [EventType], [RecurrenceInfo], [ReminderInfo], [Price], [ContactInfo]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
	SelectCommand="SELECT [ID], [CarId], [Status], [Subject], [Description], [Label], [StartTime], [EndTime], [Location], [AllDay], [EventType], [RecurrenceInfo], [ReminderInfo], [Price], [ContactInfo] FROM [CarScheduling]"
	UpdateCommand="UPDATE [CarScheduling] SET [CarId] = ?, [Status] = ?, [Subject] = ?, [Description] = ?, [Label] = ?, [StartTime] = ?, [EndTime] = ?, [Location] = ?, [AllDay] = ?, [EventType] = ?, [RecurrenceInfo] = ?, [ReminderInfo] = ?, [Price] = ?, [ContactInfo] = ? WHERE [ID] = ?">
	<DeleteParameters>
		<asp:Parameter Name="ID" Type="Int32" />
	</DeleteParameters>
	<UpdateParameters>
		<asp:Parameter Name="CarId" Type="Int32" />
		<asp:Parameter Name="Status" Type="Int32" />
		<asp:Parameter Name="Subject" Type="String" />
		<asp:Parameter Name="Description" Type="String" />
		<asp:Parameter Name="Label" Type="Int32" />
		<asp:Parameter Name="StartTime" Type="DateTime" />
		<asp:Parameter Name="EndTime" Type="DateTime" />
		<asp:Parameter Name="Location" Type="String" />
		<asp:Parameter Name="AllDay" Type="Boolean" />
		<asp:Parameter Name="EventType" Type="Int32" />
		<asp:Parameter Name="RecurrenceInfo" Type="String" />
		<asp:Parameter Name="ReminderInfo" Type="String" />
		<asp:Parameter Name="Price" Type="Double"/>
		<asp:Parameter Name="ContactInfo" Type="String"/>
		<asp:Parameter Name="ID" Type="Int32" />
	</UpdateParameters>
	<InsertParameters>
		<asp:Parameter Name="CarId" Type="Int32" />
		<asp:Parameter Name="Status" Type="Int32" />
		<asp:Parameter Name="Subject" Type="String" />
		<asp:Parameter Name="Description" Type="String" />
		<asp:Parameter Name="Label" Type="Int32" />
		<asp:Parameter Name="StartTime" Type="DateTime" />
		<asp:Parameter Name="EndTime" Type="DateTime" />
		<asp:Parameter Name="Location" Type="String" />
		<asp:Parameter Name="AllDay" Type="Boolean" />
		<asp:Parameter Name="EventType" Type="Int32" />
		<asp:Parameter Name="RecurrenceInfo" Type="String" />
		<asp:Parameter Name="ReminderInfo" Type="String" />
		<asp:Parameter Name="Price" Type="Double"/>
		<asp:Parameter Name="ContactInfo" Type="String"/>
	</InsertParameters>
</asp:AccessDataSource>
<asp:AccessDataSource ID="dbResourceDataSource" runat="server" DataFile="~/App_Data/CarsDB.mdb"
	SelectCommand="SELECT [ID], [Model] FROM [Cars] WHERE ID < 6">
 </asp:AccessDataSource>
<asp:AccessDataSource ID="dbUsageTypeDataSource" runat="server" DataFile="~/App_Data/CarsDB.mdb"
	SelectCommand="SELECT [Name], [Color] FROM [UsageType]">
 </asp:AccessDataSource>

<asp:ObjectDataSource ID="temporaryAppointmentDataSource" runat="server" DataObjectTypeName="CustomEvent" TypeName="CustomEventDataSource" DeleteMethod="DeleteMethodHandler" SelectMethod="SelectMethodHandler" InsertMethod="InsertMethodHandler" OnObjectCreated="temporaryAppointmentDataSource_ObjectCreated" UpdateMethod="UpdateMethodHandler">
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="temporaryResourceDataSource" runat="server" DataObjectTypeName="CustomResource" TypeName="CustomResourceDataSource" DeleteMethod="DeleteMethodHandler" SelectMethod="SelectMethodHandler" InsertMethod="InsertMethodHandler" OnObjectCreated="temporaryResourceDataSource_ObjectCreated" UpdateMethod="UpdateMethodHandler">
</asp:ObjectDataSource>
