Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Caching

Public Class HttpCacheHelper
    Public Shared Function GetFromCache(ByVal key As String) As Object
        If HttpContext.Current.Cache Is Nothing Then Return Nothing
        Return HttpContext.Current.Cache(key)
    End Function

    Public Shared Sub SaveToCache(ByVal key As String, ByVal item As Object, ByVal expiry As TimeSpan)
        If HttpContext.Current.Cache IsNot Nothing Then
            HttpContext.Current.Cache.Insert(key, item, Nothing, DateTime.UtcNow.Add(expiry), Cache.NoSlidingExpiration)
        End If
    End Sub

    Public Shared Sub SaveToCacheDependency(ByVal database As String, ByVal tableNames As String(), ByVal cacheName As String, ByVal data As Object, ByVal expiry As TimeSpan)
        If HttpContext.Current.Cache Is Nothing Then Return

        Dim dependencies = New AggregateCacheDependency()
        For Each tableName In tableNames
            dependencies.Add(New SqlCacheDependency(database, tableName))
        Next

        If data IsNot Nothing Then
            HttpContext.Current.Cache.Insert(cacheName, data, dependencies, DateTime.UtcNow.Add(expiry), Cache.NoSlidingExpiration)
        End If
    End Sub

    Public Shared Sub SaveToCacheDependency(ByVal database As String, ByVal tableName As String, ByVal cacheName As String, ByVal data As Object, ByVal expiry As TimeSpan)
        If HttpContext.Current.Cache Is Nothing Then Return

        Dim dependency = New SqlCacheDependency(database, tableName)

        If data IsNot Nothing Then
            HttpContext.Current.Cache.Insert(cacheName, data, dependency, DateTime.UtcNow.Add(expiry), Cache.NoSlidingExpiration)
        End If
    End Sub

    Public Shared Sub RemoveCache(ByVal key As String)
        If HttpContext.Current.Cache IsNot Nothing Then
            HttpContext.Current.Cache.Remove(key)
        End If
    End Sub
End Class
