Public Class Parameters

    Private _health As Integer
    Public Property Health As Integer
        Get
            Return _health
        End Get
        Set(value As Integer)
            _health = value
        End Set
    End Property

    Sub New()
        _health = Par.INITIAL_HEALTH
    End Sub

End Class