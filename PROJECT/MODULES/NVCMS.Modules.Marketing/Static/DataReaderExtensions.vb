Imports System.Data

Namespace NVCMS.Modules.Marketing

    Public Module DataReaderExtensions

        <Runtime.CompilerServices.Extension()>
        Public Function GetInt(reader As IDataReader,
                               column As String) As Integer

            Dim ordinal = reader.GetOrdinal(column)

            If reader.IsDBNull(ordinal) Then
                Return 0
            End If

            Return Convert.ToInt32(reader.GetValue(ordinal))

        End Function


        <Runtime.CompilerServices.Extension()>
        Public Function GetLong(reader As IDataReader,
                                column As String) As Long

            Dim ordinal = reader.GetOrdinal(column)

            If reader.IsDBNull(ordinal) Then
                Return 0
            End If

            Return Convert.ToInt64(reader.GetValue(ordinal))

        End Function


        <Runtime.CompilerServices.Extension()>
        Public Function GetDecimal(reader As IDataReader,
                                   column As String) As Decimal

            Dim ordinal = reader.GetOrdinal(column)

            If reader.IsDBNull(ordinal) Then
                Return 0D
            End If

            Return Convert.ToDecimal(reader.GetValue(ordinal))

        End Function


        <Runtime.CompilerServices.Extension()>
        Public Function GetStringSafe(reader As IDataReader,
                                      column As String) As String

            Dim ordinal = reader.GetOrdinal(column)

            If reader.IsDBNull(ordinal) Then
                Return String.Empty
            End If

            Return reader.GetString(ordinal)

        End Function


        <Runtime.CompilerServices.Extension()>
        Public Function GetNullableDate(reader As IDataReader,
                                        column As String) As Nullable(Of DateTime)

            Dim ordinal = reader.GetOrdinal(column)

            If reader.IsDBNull(ordinal) Then
                Return Nothing
            End If

            Return CType(reader.GetValue(ordinal), DateTime)

        End Function


        <Runtime.CompilerServices.Extension()>
        Public Function GetBool(reader As IDataReader,
                                column As String) As Boolean

            Dim ordinal = reader.GetOrdinal(column)

            If reader.IsDBNull(ordinal) Then
                Return False
            End If

            Return Convert.ToBoolean(reader.GetValue(ordinal))

        End Function

    End Module

End Namespace