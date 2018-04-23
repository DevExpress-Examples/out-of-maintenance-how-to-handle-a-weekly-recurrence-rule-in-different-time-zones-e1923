Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel

<Serializable> _
Public Class CustomResourceList
    Inherits BindingList(Of CustomResource)

    Public Sub AddRange(ByVal resources As CustomResourceList)
        For Each customResource As CustomResource In resources
            Me.Add(customResource)
        Next customResource
    End Sub
    Public Function GetResourceIndex(ByVal resourceId As Object) As Integer
        For i As Integer = 0 To Count - 1
            If Me(i).Id Is resourceId Then
                Return i
            End If
        Next i
        Return -1
    End Function
End Class
Public Class CustomResourceDataSource

    Private resources_Renamed As CustomResourceList
    Public Sub New(ByVal resources As CustomResourceList)
        If resources Is Nothing Then
            DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("resources")
        End If
        Me.resources_Renamed = resources
    End Sub
    Public Sub New()
        Me.New(New CustomResourceList())
    End Sub
    Public Property Resources() As CustomResourceList
        Get
            Return resources_Renamed
        End Get
        Set(ByVal value As CustomResourceList)
            resources_Renamed = value
        End Set
    End Property

    #Region "ObjectDataSource methods"
    Public Sub InsertMethodHandler(ByVal customResource As CustomResource)
        Resources.Add(customResource)
    End Sub
    Public Sub DeleteMethodHandler(ByVal customResource As CustomResource)
        Dim resourceIndex As Integer = Resources.GetResourceIndex(customResource.Id)
        If resourceIndex >= 0 Then
            Resources.RemoveAt(resourceIndex)
        End If
    End Sub
    Public Sub UpdateMethodHandler(ByVal customResource As CustomResource)
        Dim resourceIndex As Integer = Resources.GetResourceIndex(customResource.Id)
        If resourceIndex >= 0 Then
            Resources.RemoveAt(resourceIndex)
            Resources.Insert(resourceIndex, customResource)
        End If
    End Sub
    Public Function SelectMethodHandler() As IEnumerable
        Dim result As New CustomResourceList()
        result.AddRange(Resources)
        Return result
    End Function
    #End Region
End Class

<Serializable> _
Public Class CustomResource

    Private id_Renamed As Object

    Private caption_Renamed As String

    Public Sub New()
    End Sub

    Public Property Id() As Object
        Get
            Return id_Renamed
        End Get
        Set(ByVal value As Object)
            id_Renamed = value
        End Set
    End Property
    Public Property Caption() As String
        Get
            Return caption_Renamed
        End Get
        Set(ByVal value As String)
            caption_Renamed = value
        End Set
    End Property
End Class