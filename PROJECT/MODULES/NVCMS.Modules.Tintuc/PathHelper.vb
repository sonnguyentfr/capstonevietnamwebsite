

Namespace NVCMS.Modules.TinTuc

    ''' <summary>
    ''' Supports features that are not implemented in the System.Web.VirtualPathUtility and SystemIO.Pat classes
    ''' </summary>
    Public Class PathHelper
        ''' <summary>
        ''' Adds a symbol at the begining of a path if the symblol does not exist
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="symbolToAdd"></param>
        ''' <returns>The modified path</returns>
        Public Shared Function AddStartingSlash(ByVal path As String, ByVal symbolToAdd As Char) As String
            If path.StartsWith(symbolToAdd.ToString()) Then
                Return path
            Else
                Return symbolToAdd & path
            End If
        End Function

        ''' <summary>
        ''' Removes a symblol from the begining of a path
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="symbolToRemove"></param>
        ''' <returns></returns>
        ''' 
        Public Shared Function RemoveStartingSlash(ByVal path As String, ByVal symbolToRemove As Char) As String
            If path.StartsWith(symbolToRemove.ToString()) Then
                Return path.Substring(1)
            Else
                Return path
            End If
        End Function


        ''' <summary>
        ''' Adds a symbol at the end of a path if the symbol does not exist
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="symbolToAdd"></param>
        ''' <returns>The modified path</returns>
        Public Shared Function AddEndingSlash(ByVal path As String, ByVal symbolToAdd As Char) As String
            If path.EndsWith(symbolToAdd.ToString()) Then
                Return path
            Else
                Return path & symbolToAdd
            End If
        End Function

        ''' <summary>
        ''' Removes a backslash from the end of a path
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns>The modified path</returns>
        Public Shared Function RemoveEndingSlash(ByVal path As String, ByVal symbolToRemove As Char) As String
            If path.EndsWith(symbolToRemove.ToString()) Then
                Return path.Substring(0, path.Length - 1)
            Else
                Return path
            End If
        End Function

        ''' <summary>
        ''' Gets the name of a directory.
        ''' Example C:\Folder1\Folder2 ==> the function returns 'Folder2' 
        ''' </summary>
        ''' <param name="physicalPath"></param>
        ''' <returns></returns>
        Public Shared Function GetDirectoryName(ByVal physicalPath As String) As String
            If physicalPath.EndsWith("\") Then
                Dim lastIndexOfSlash As Integer = physicalPath.Substring(0, physicalPath.Length - 1).LastIndexOf("\")

                'if (lastIndexOfSlash == -1)
                '{// If the passsd path is C:\ for example
                '    return string.Empty;
                '}

                Dim name As String = physicalPath.Substring(lastIndexOfSlash + 1)
                Return name.Replace("\", "")
            Else
                Dim lastIndexOfSlash As Integer = physicalPath.LastIndexOf("\")
                Dim name As String = physicalPath.Substring(lastIndexOfSlash + 1)
                Return name
            End If
        End Function

        ''' <summary>
        ''' Checks whether the passed path is a physical path 
        ''' </summary>
        ''' <param name="path">The paths looks likee: 'C:\Path\Dir' </param>
        ''' <returns></returns>
        Public Shared Function IsPhysicalPath(ByVal path As String) As Boolean
            Return path.Contains(":\")
        End Function

        ''' <summary>
        ''' Checks whether the passed path is a shared folder's path 
        ''' </summary>
        ''' <param name="path">The path looks like: '\\Path\Dir'</param>
        ''' <returns></returns>
        Public Shared Function IsSharedPath(ByVal path As String) As Boolean
            Return path.StartsWith("\\")


        End Function

        ''' <summary>
        ''' Checks whether a path is child of another path
        ''' </summary>
        ''' <param name="virtualParent">Should be the virtual parent directory's path</param>
        ''' <param name="virtualChild">Should be the virtual child path. This parameter can be a path to file as well</param>
        ''' <returns></returns>
        Public Shared Function IsParentOf(ByVal virtualParent As String, ByVal virtualChild As String) As Boolean
            If virtualChild.Equals(virtualParent, StringComparison.CurrentCultureIgnoreCase) Then
                Return False
            End If

            ' else if
            If virtualChild.StartsWith(virtualParent, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If

            ' else
            Return False
        End Function
    End Class

End Namespace