Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Xml


<Serializable> _
Public Class CustomEvent
	Private id_Renamed As Object
	Private start As DateTime
	Private [end] As DateTime
	Private subject_Renamed As String
	Private status_Renamed As Integer
	Private description_Renamed As String
	Private label_Renamed As Long
	Private location_Renamed As String
	Private allday_Renamed As Boolean
	Private eventType_Renamed As Integer
	Private recurrenceInfo_Renamed As String
	Private reminderInfo_Renamed As String
	Private ownerId_Renamed As Object
	Private price_Renamed As Double
	Private contactInfo_Renamed As String

	Public Sub New()
	End Sub

	Public Property StartTime() As DateTime
		Get
			Return start
		End Get
		Set
			start = Value
		End Set
	End Property
	Public Property EndTime() As DateTime
		Get
			Return [end]
		End Get
		Set
			[end] = Value
		End Set
	End Property
	Public Property Subject() As String
		Get
			Return subject_Renamed
		End Get
		Set
			subject_Renamed = Value
		End Set
	End Property
	Public Property Status() As Integer
		Get
			Return status_Renamed
		End Get
		Set
			status_Renamed = Value
		End Set
	End Property
	Public Property Description() As String
		Get
			Return description_Renamed
		End Get
		Set
			description_Renamed = Value
		End Set
	End Property
	Public Property Label() As Long
		Get
			Return label_Renamed
		End Get
		Set
			label_Renamed = Value
		End Set
	End Property
	Public Property Location() As String
		Get
			Return location_Renamed
		End Get
		Set
			location_Renamed = Value
		End Set
	End Property
	Public Property AllDay() As Boolean
		Get
			Return allday_Renamed
		End Get
		Set
			allday_Renamed = Value
		End Set
	End Property
	Public Property EventType() As Integer
		Get
			Return eventType_Renamed
		End Get
		Set
			eventType_Renamed = Value
		End Set
	End Property
	Public Property RecurrenceInfo() As String
		Get
			Return recurrenceInfo_Renamed
		End Get
		Set
			recurrenceInfo_Renamed = Value
		End Set
	End Property
	Public Property ReminderInfo() As String
		Get
			Return reminderInfo_Renamed
		End Get
		Set
			reminderInfo_Renamed = Value
		End Set
	End Property
	Public Property OwnerId() As Object
		Get
			Return ownerId_Renamed
		End Get
		Set
			ownerId_Renamed = Value
		End Set
	End Property
	Public Property Id() As Object
		Get
			Return id_Renamed
		End Get
		Set
			id_Renamed = Value
		End Set
	End Property
	Public Property Price() As Double
		Get
			Return price_Renamed
		End Get
		Set
			price_Renamed = Value
		End Set
	End Property
	Public Property ContactInfo() As String
		Get
			Return contactInfo_Renamed
		End Get
		Set
			contactInfo_Renamed = Value
		End Set
	End Property
End Class
<Serializable> _
Public Class CustomEventList
	Inherits BindingList(Of CustomEvent)
	Public Sub AddRange(ByVal events As CustomEventList)
		For Each customEvent As CustomEvent In events
			Me.Add(customEvent)
		Next customEvent
	End Sub
	Public Function GetEventIndex(ByVal eventId As Object) As Integer
		For i As Integer = 0 To Count - 1
			If Me(i).Id Is eventId Then
				Return i
			End If
		Next i
		Return -1
	End Function
End Class

Public Class CustomEventDataSource
	Private events_Renamed As CustomEventList
	Public Sub New(ByVal events_Renamed As CustomEventList)
		If events_Renamed Is Nothing Then
			DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("events")
		End If
		Me.events_Renamed = events_Renamed
	End Sub
	Public Sub New()
		Me.New(New CustomEventList())
	End Sub
	Public Property Events() As CustomEventList
		Get
			Return events_Renamed
		End Get
		Set
			events_Renamed = Value
		End Set
	End Property

	#Region "ObjectDataSource methods"
	Public Sub InsertMethodHandler(ByVal customEvent As CustomEvent)
		Events.Add(customEvent)
	End Sub
	Public Sub DeleteMethodHandler(ByVal customEvent As CustomEvent)
		Dim eventIndex As Integer = Events.GetEventIndex(customEvent.Id)
		If eventIndex >= 0 Then
			Events.RemoveAt(eventIndex)
		End If
	End Sub
	Public Sub UpdateMethodHandler(ByVal customEvent As CustomEvent)
		Dim eventIndex As Integer = Events.GetEventIndex(customEvent.Id)
		If eventIndex >= 0 Then
			Events.RemoveAt(eventIndex)
			Events.Insert(eventIndex, customEvent)
		End If
	End Sub
	Public Function SelectMethodHandler() As IEnumerable
		Dim result As CustomEventList = New CustomEventList()
		result.AddRange(Events)
		Return result
	End Function
	#End Region
End Class
