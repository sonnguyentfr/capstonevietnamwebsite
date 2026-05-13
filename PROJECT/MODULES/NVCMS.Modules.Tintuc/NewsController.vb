Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.HeThong
Namespace NVCMS.Modules.TinTuc

    Public Class NV_NewsController

        Public Function Insert(ByVal objNews As NV_NewsInfo) As Integer
            Return DataProvider.Instance.NV_News_add(objNews)
        End Function

        '------------------------------------------'
        Public Sub Update(ByVal objNews As NV_NewsInfo)
            DataProvider.Instance.NV_News_update(objNews)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub
        '------------------------------------------'
        Public Sub UpdateContent(ByVal NewId As Integer, ByVal Content As String)
            DataProvider.Instance.NV_News_updateContent(NewId, Content)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub
        Public Sub UpdateStatus(ByVal NewId As Integer, ByVal Status As Integer, ByVal userid As Integer)
            DataProvider.Instance.NV_News_updateStatus(NewId, Status, userid)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub

        Public Sub UpdateStatusDate(ByVal NewId As Integer, ByVal Status As Integer, ByVal atDate As DateTime)
            DataProvider.Instance.NV_News_updateStatusDate(NewId, Status, atDate)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub

        Public Sub UpdateStatusNone(ByVal NewId As Integer, ByVal Status As Integer)
            DataProvider.Instance.NV_News_updateStatusNone(NewId, Status)
        End Sub

        Public Sub UpdateStatusUser(ByVal NewId As Integer, ByVal Status As Integer, ByVal userid As Integer)
            DataProvider.Instance.NV_News_updateStatusUser(NewId, Status, userid)
        End Sub

        Public Sub UpdateVisible(ByVal NewId As Integer, ByVal IsVisible As Boolean)
            DataProvider.Instance.NV_News_updateVisible(NewId, IsVisible)
        End Sub

        '------------------------------------------'
        Public Sub Delete(ByVal NewId As Integer)
            DataProvider.Instance.NV_News_delete(NewId)
        End Sub

        Public Sub Approve(ByVal NewId As Integer)
            DataProvider.Instance.NV_News_Approve(NewId)
        End Sub

        '------------------------------------------'
        Public Function GetByID(ByVal NewId As Integer) As NV_NewsInfo
            Return CType(CBO.FillObject(Of NV_NewsInfo)(DataProvider.Instance.NV_News_selectbyid(NewId), True), NV_NewsInfo)
        End Function

        '------------------------------------------'
        Public Function GetAll(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectall(PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function GetByCategoryId(ByVal CategoryId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectbycategory(CategoryId), GetType(NV_NewsInfo))
        End Function

        Public Function Find(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_find(datefrom, dateto, title, categoryid, PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function NV_News_FindContent(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_FindContent(control, PortalId, datefrom, dateto, key, categoryid), GetType(NV_NewsInfo))
        End Function

        Public Function FindByStatus(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_findbystatus(datefrom, dateto, title, categoryid, PortalId, status, UserId), GetType(NV_NewsInfo))
        End Function

        Public Function SelectHotCat(ByVal categoryid As Integer, Count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selecthotcat(categoryid, Count), GetType(NV_NewsInfo))
        End Function

        Public Function SelectHotSite(ByVal PortalId As Integer) As NV_NewsInfo
            Return CType(CBO.FillObject(Of NV_NewsInfo)(DataProvider.Instance.NV_News_selecthotsite(PortalId), True), NV_NewsInfo)
        End Function

        Public Function select5hotsite(ByVal PortalId As Integer, Optional ByVal NewsId As Integer = 0) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_select5hotsite(PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function selecthotsite(ByVal PortalId As Integer, ByVal Count As Integer, Optional ByVal NewsId As Integer = 0) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selecthotsite(PortalId, Count, NewsId), GetType(NV_NewsInfo))
        End Function

        Public Function select3hotsiteByCat(ByVal catId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_select3hotsiteByCat(catId), GetType(NV_NewsInfo))
        End Function

        Public Function selectTopHotCatNews(ByVal subtractIds As String, ByVal catId As Integer, count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selecthotCatNews(subtractIds, catId, count), GetType(NV_NewsInfo))
        End Function

        Public Function selecttopsitenews(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selecttopsitenews(PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function selecttopcatnews(ByVal PortalId As Integer, ByVal Count As Integer, exceptNewsId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selecttopcatnews(PortalId, Count, exceptNewsId), GetType(NV_NewsInfo))
        End Function

        Public Function select5lastestnews(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_select5lastestnews(PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function selectlastestnews(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectlastestnews(PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function selectCustomeNews(ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal Count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectCustomeNews(CategoryId, PortalId, Count), GetType(NV_NewsInfo))
        End Function

        Public Function selecttopcatbycatid(ByVal categoryid As Integer) As NV_NewsInfo
            Return CBO.FillObject(Of NV_NewsInfo)(DataProvider.Instance.NV_News_selecttopcatbycatid(categoryid), True)
        End Function

        Public Function selecttopcatbyphongbanid(ByVal phongbanId As Integer) As NV_NewsInfo
            Return CBO.FillObject(Of NV_NewsInfo)(DataProvider.Instance.NV_News_selecttopcatbyphongbanid(phongbanId), True)
        End Function

        Public Function selecttopnewsbycatid(ByVal categoryid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selecttopnewsbycatid(categoryid), GetType(NV_NewsInfo))
        End Function

        Public Function selecttopnormalnews(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selecttopnormalnews(PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function selectapprovenew(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectapprovenew(startdate, enddate, title, categoryid, PortalId, UserId), GetType(NV_NewsInfo))
        End Function

        Public Function selectnewsinsamecat(ByVal exceptNewid As Integer, ByVal catid As Integer, ByVal count As Integer, Optional ByVal includeChildrenCat As Boolean = True) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectnewsinsamecat(exceptNewid, catid, count, includeChildrenCat), GetType(NV_NewsInfo))
        End Function

        Public Function selectnewsinsamephongban(ByVal exceptNewid As Integer, ByVal arrPhongBan As String, ByVal count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectnewsinsamephongban(exceptNewid, arrPhongBan, count), GetType(NV_NewsInfo))
        End Function

        Public Function selectothertopsitenews(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_selectothertopsitenews(PortalId), GetType(NV_NewsInfo))
        End Function

        Public Function select6lastestcatnews(ByVal PortalId As Integer, ByVal CateId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_select6lastestcatnews(PortalId, CateId), GetType(NV_NewsInfo))
        End Function
        '------------------------------------------'

        Public Function NewsByCategory_GetByNewsId(ByVal newsid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsByCategory_GetByNewsId(newsid), GetType(NewsByCategoryInfo))
        End Function

        'TrungNS
        Public Function AdminFindSourceText_Count(souretext As String, portalid As Integer) As Integer
            Return DataProvider.Instance.AdminFindSourceText_Count(souretext, portalid)
        End Function
        Public Function SelectCount(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal arrPhongBan As String, ByVal isImage As Integer) As Integer
            Return DataProvider.Instance.Select_Count(subtractIds, CategoryId, PortalId, arrPhongBan, isImage)
        End Function
        Public Function SelectIndex(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal isImage As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Select_Index(subtractIds, CategoryId, PortalId, PageIndex, PageSize, arrPhongBan, isImage), GetType(NV_NewsInfo))
        End Function
        Public Function FindContent_Count(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As Integer
            Return DataProvider.Instance.FindContent_Count(control, PortalId, datefrom, dateto, key, categoryid, arrPhongBan, uid, isImage, type)
        End Function
        Public Function FindContent_Index(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindContent_Index(control, PortalId, datefrom, dateto, key, categoryid, PageIndex, PageSize, arrPhongBan, uid, isImage, type), GetType(NV_NewsInfo))
        End Function
        Public Function FindContentExact_Count(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As Integer
            Return DataProvider.Instance.FindContentExact_Count(control, PortalId, datefrom, dateto, key, categoryid, arrPhongBan, uid, isImage, type)
        End Function
        Public Function FindContentExact_Index(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindContentExact_Index(control, PortalId, datefrom, dateto, key, categoryid, PageIndex, PageSize, arrPhongBan, uid, isImage, type), GetType(NV_NewsInfo))
        End Function
        Public Function FindByStatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return DataProvider.Instance.Findbystatus_Count(datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan)
        End Function
        Public Function FindByStatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Findbystatus_Index(datefrom, dateto, title, categoryid, PortalId, status, UserId, PageIndex, PageSize, arrPhongBan), GetType(NV_NewsInfo))
        End Function
        Public Function SelectApproveNews_Count(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, status As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal isImage As Boolean) As Integer
            Return DataProvider.Instance.SelectApproveNews_Count(startdate, enddate, title, categoryid, status, PortalId, UserId, isImage)
        End Function
        Public Function SelectApproveNews_Index(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, Status As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.SelectApproveNews_Index(startdate, enddate, title, categoryid, Status, PortalId, UserId, PageIndex, PageSize, isImage), GetType(NV_NewsInfo))
        End Function
        Public Function FindNews_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, isImage As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return DataProvider.Instance.FindNews_Count(datefrom, dateto, title, categoryid, isImage, PortalId, status, UserId, arrPhongBan)
        End Function
        Public Function FindNews_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, isImage As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindNews_Index(datefrom, dateto, title, categoryid, isImage, PortalId, status, UserId, PageIndex, PageSize, arrPhongBan), GetType(NV_NewsInfo))
        End Function
        Public Function FindImages_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return DataProvider.Instance.FindImages_Count(datefrom, dateto, title, PortalId, status, UserId, arrPhongBan)
        End Function
        Public Function FindImages_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindImages_Index(datefrom, dateto, title, PortalId, status, UserId, PageIndex, PageSize, arrPhongBan), GetType(NV_NewsInfo))
        End Function
        Public Sub UpdatePublishedDate(ByVal NewId As Integer, ByVal publishedDate As DateTime, ByVal userid As Integer)
            DataProvider.Instance.NV_News_updatePublishedDate(NewId, publishedDate, userid)
        End Sub
        Public Sub UpdateUsersGet(ByVal NewId As Integer, ByVal usersGet As String)
            DataProvider.Instance.NV_News_updateUsersGet(NewId, usersGet)
        End Sub
        Public Sub UpdateUsersView(ByVal NewId As Integer, ByVal usersView As String)
            DataProvider.Instance.NV_News_updateUsersView(NewId, usersView)
        End Sub
        'New LOGIC
        Public Function SelectApproveNews_CountExt(ByVal UserId As Integer, ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal CreatedUser As Integer, ByVal isImage As Boolean) As Integer
            Return DataProvider.Instance.SelectApproveNews_CountExt(UserId, startdate, enddate, title, categoryid, PortalId, CreatedUser, isImage)
        End Function
        Public Function SelectApproveNews_IndexExt(ByVal UserId As Integer, ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal CreatedUser As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.SelectApproveNews_IndexExt(UserId, startdate, enddate, title, categoryid, PortalId, CreatedUser, PageIndex, PageSize, isImage), GetType(NV_NewsInfo))
        End Function
        Public Function FindByStatus_CountExt(ByVal ToUserID As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return DataProvider.Instance.Findbystatus_CountExt(ToUserID, datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan)
        End Function
        Public Function FindByStatus_IndexExt(ByVal ToUserID As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Findbystatus_IndexExt(ToUserID, datefrom, dateto, title, categoryid, PortalId, status, UserId, PageIndex, PageSize, arrPhongBan), GetType(NV_NewsInfo))
        End Function

        Public Sub UpdateArchiving(ByVal newid As Integer, ByVal isArchived As Boolean, ByVal storagefolder As String)
            DataProvider.Instance.NV_News_updateArchiving(newid, isArchived, storagefolder)
        End Sub
        Public Sub UpdateTaping(ByVal newid As Integer, ByVal isTaped As Boolean)
            DataProvider.Instance.NV_News_updateTaping(newid, isTaped)
        End Sub
        Public Function FindByPhongBanId_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal arrPhongBan As String, ByVal isImage As Boolean) As Integer
            Return DataProvider.Instance.FindByPhongBanId_Count(datefrom, dateto, key, arrPhongBan, isImage)
        End Function
        Public Function FindByPhongBanId_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal isImage As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindByPhongBanId_Index(datefrom, dateto, key, PageIndex, PageSize, arrPhongBan, isImage), GetType(NV_NewsInfo))
        End Function
        Public Function FindHome_Count(ByVal key As String) As Integer
            Return DataProvider.Instance.FindHome_Count(key)
        End Function
        Public Function FindHome_Index(ByVal key As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindHome_Index(key, PageIndex, PageSize), GetType(NV_NewsInfo))
        End Function
        'Files handler
        Public Function FindFiles_Count(ByVal folderID As Integer, ByVal type As Integer, ByVal key As String, ByVal fromDate As DateTime, ByVal toDate As DateTime) As Integer
            Return DataProvider.Instance.FindFiles_Count(folderID, type, key, fromDate, toDate)
        End Function
        Public Function FindFiles_Index(ByVal folderID As Integer, ByVal type As Integer, ByVal key As String, ByVal fromDate As DateTime, ByVal toDate As DateTime, ByVal sortDir As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindFiles_Index(folderID, type, key, fromDate, toDate, sortDir, PageIndex, PageSize), GetType(V_FileInfo))
        End Function

        Public Function AdminFind_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean) As Integer
            Return DataProvider.Instance.AdminFind_Count(datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan, isActive, isImage)
        End Function
        Public Function AdminFind_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.AdminFind_Index(datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan, isActive, isImage, PageIndex, PageSize), GetType(NV_NewsInfo))
        End Function
        Public Function FindDatBai_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal requestedtitle As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return DataProvider.Instance.FindDatBai_Count(datefrom, dateto, requestedtitle, categoryid, PortalId, UserId, arrPhongBan)
        End Function
        Public Function FindDatBai_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal requestedtitle As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindDatBai_Index(datefrom, dateto, requestedtitle, categoryid, PortalId, UserId, PageIndex, PageSize, arrPhongBan), GetType(NV_NewsInfo))
        End Function
        Public Sub UpdateArchiving(ByVal NewId As Integer, ByVal Status As Integer, ByVal userid As Integer, ByVal atDate As DateTime)
            DataProvider.Instance.NV_News_updateArchiving(NewId, Status, userid, atDate)
        End Sub
        Public Sub UpdateLock(ByVal NewId As Integer, ByVal lock As Boolean, ByVal userid As Integer)
            DataProvider.Instance.NV_News_updateLock(NewId, lock, userid)
        End Sub
        Public Function GetLock(ByVal PortalId As Integer, Optional ByVal newsid As Integer = 0) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_GetLocks(PortalId, newsid), GetType(NV_NewsInfo))
        End Function
        Public Function SelectTopView(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer, ByVal fromdate As DateTime, Optional ByVal categoryid As Integer = 0, Optional ByVal arrPhongBan As String = "") As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_SelectTopView(PortalId, Count, NewsId, fromdate, categoryid, arrPhongBan), GetType(NV_NewsInfo))
        End Function
        Public Function SelectTopGet(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer, ByVal fromdate As DateTime, Optional ByVal categoryid As Integer = 0, Optional ByVal arrPhongBan As String = "") As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_SelectTopGet(PortalId, Count, NewsId, fromdate, categoryid, arrPhongBan), GetType(NV_NewsInfo))
        End Function
        Public Function FindByPhongBanStatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return DataProvider.Instance.FindByPhongBanStatus_Count(datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan)
        End Function
        Public Function FindByPhongBanStatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindByPhongBanStatus_Index(datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan, PageIndex, PageSize), GetType(NV_NewsInfo))
        End Function
        Public Sub UpdateProcessUser(ByVal NewId As Integer, ByVal userid As Integer)
            DataProvider.Instance.NV_News_updateProcessUserID(NewId, userid)
        End Sub
        Public Function FindNotUsed_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal arrExceptPB As String) As Integer
            Return DataProvider.Instance.FindNotUse_Count(datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan, isActive, isImage, arrExceptPB)
        End Function
        Public Function FindNotUsed_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal arrExceptPB As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindNotUse_Index(datefrom, dateto, title, categoryid, PortalId, status, UserId, arrPhongBan, isActive, isImage, arrExceptPB, PageIndex, PageSize), GetType(NV_NewsInfo))
        End Function

        Public Function AddTags(ByVal vocabularyid As Integer, name As String, des As String, weight As Integer, createduserid As Integer) As Integer
            Return DataProvider.Instance.addTerm(vocabularyid, name, des, weight, createduserid)
        End Function
        Public Sub IncrementViewCount(ByVal NewId As Integer)
            DataProvider.Instance.NV_News_IncrementViewCount(NewId)
        End Sub
#Region "show Mr Doi Viet them"
        Public Function ShowBaiMoiDanhMuc(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, Count As Integer, isImage As Boolean) As ArrayList
            Dim stringcache = nvcmsBL.cacheShowBaiMoiDanhMuc & CategoryId & Count
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Show_ShowBaiMoiDanhMuc(subtractIds, CategoryId, PortalId, Count, isImage), GetType(NV_NewsInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Show_ShowBaiMoiDanhMuc(subtractIds, CategoryId, PortalId, Count, isImage), GetType(NV_NewsInfo))
        End Function
        Public Function ShowBaiMoiNhat(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Cou8nt As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Show_BaiMoiNhat(subtractIds, PortalId, Cou8nt), GetType(NV_NewsInfo))
        End Function
        Public Function ShowTopViewSite(ByVal PortalId As Integer, SoNgay As Integer, Count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Show_TopViewSite(PortalId, SoNgay, Count), GetType(NV_NewsInfo))
        End Function
        Public Function ShowSelectTopCatNewsHOT(ByVal subtractIds As String, ByVal catId As Integer, count As Integer, Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Show_SelectTopCatNewsHOT(subtractIds, catId, count, Portalid), GetType(NV_NewsInfo))
        End Function
        Public Sub UpdateTacgia(ByVal NewId As Integer, ByVal Tacgia As String)
            DataProvider.Instance.Admin_News_update_Tacgia(NewId, Tacgia)
        End Sub
        Public Sub UpdateCategory(ByVal NewId As Integer, ByVal CategoryId As Integer)
            DataProvider.Instance.Admin_News_update_Category(NewId, CategoryId)
        End Sub
        Public Sub UpdateNhuanBut(ByVal NewId As Integer, ByVal Credit As Integer)
            DataProvider.Instance.Admin_News_updateNhuanBut(NewId, Credit)
        End Sub
        Public Function ShowYearMonth(ByVal PortalId As Integer, ByVal Year As Integer, Month As Integer, Day As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Show_YearMonth(PortalId, Year, Month, Day), GetType(NV_NewsInfo))
        End Function
        Public Function User_GetTongView(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer) As Integer
            Return DataProvider.Instance.User_GetTongView(datefrom, dateto, UserId)
        End Function
#End Region
    End Class

End Namespace