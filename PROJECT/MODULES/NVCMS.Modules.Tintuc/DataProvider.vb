Imports System
Imports DotNetNuke
Imports DotNetNuke.Entities.Content.Taxonomy

Namespace NVCMS.Modules.TinTuc
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' An abstract class for the data access layer
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.TinTuc", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

        Public MustOverride Function NVTest() As String

#Region "categories"
        Public MustOverride Sub NV_NewsCategories_add(ByVal categoryname As String, ByVal description As String, ByVal TabID As Integer, TabIdDetail As Integer, ByVal isactive As Integer, ByVal PortalId As Integer, ByVal ParentId As Integer, ByVal OrderNumber As Integer)

        Public MustOverride Sub NV_NewsCategories_delete(ByVal categoryid As Integer)

        Public MustOverride Sub NV_NewsCategories_update(ByVal categoryid As Integer, ByVal categoryname As String, ByVal description As String, ByVal TabID As Integer, TabIdDetail As Integer, ByVal isactive As Integer, ByVal PortalId As Integer, ByVal ParentId As Integer, ByVal OrderNumber As Integer)

        Public MustOverride Sub NV_NewsCategories_updateOrderNumber(ByVal categoryid As Integer, ByVal OrderNumber As Integer)

        Public MustOverride Function NV_NewsCategories_selectall(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_NewsCategories_selectByParentId(ByVal Parentid As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_NewsCategories_selectByParentIdExt(ByVal Parentid As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_NewsCategories_selectallVisible(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_NewsCategories_selectbyid(ByVal categoryid As Integer) As IDataReader

        Public MustOverride Function NV_NewsCategories_selectRandom() As IDataReader

        Public MustOverride Function NV_NewsCategories_selectbyTabID(ByVal tabid As Integer) As IDataReader

#End Region
#Region "news"
        Public MustOverride Function NV_News_add(ByVal objNews As NV_NewsInfo) As Integer

        Public MustOverride Sub NV_News_delete(ByVal categoryid As Integer)

        Public MustOverride Sub NV_News_Approve(ByVal newid As Integer)

        Public MustOverride Sub NV_News_update(ByVal objNews As NV_NewsInfo)

        Public MustOverride Sub NV_News_updateContent(ByVal newid As Integer, ByVal Content As String)

        Public MustOverride Sub NV_News_updateStatus(ByVal newid As Integer, ByVal Status As Integer, ByVal userid As Integer)

        Public MustOverride Sub NV_News_updateStatusDate(ByVal newid As Integer, ByVal Status As Integer, ByVal atDate As DateTime)

        Public MustOverride Sub NV_News_updateStatusNone(ByVal newid As Integer, ByVal Status As Integer)

        Public MustOverride Sub NV_News_updateStatusUser(ByVal newid As Integer, ByVal Status As Integer, ByVal userid As Integer)

        Public MustOverride Sub NV_News_updateVisible(ByVal NewId As Integer, ByVal IsVisible As Boolean)

        Public MustOverride Function NV_News_selectall(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_selectbyid(ByVal newid As Integer) As IDataReader

        Public MustOverride Function NV_News_selectbycategory(ByVal categoryid As Integer) As IDataReader

        Public MustOverride Function NV_News_find(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_FindContent(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer) As IDataReader

        Public MustOverride Function NV_News_findbystatus(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer) As IDataReader

        Public MustOverride Function NV_News_selecthotcat(ByVal categoryid As Integer, Count As Integer) As IDataReader

        Public MustOverride Function NV_News_selecthotsite(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_select5hotsite(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_selecthotsite(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer) As IDataReader

        Public MustOverride Function NV_News_select3hotsiteByCat(ByVal catId As Integer) As IDataReader

        Public MustOverride Function NV_News_selecthotCatNews(ByVal subtractIds As String, ByVal catId As Integer, count As Integer) As IDataReader

        Public MustOverride Function NV_News_selecttopsitenews(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_selecttopcatnews(ByVal PortalId As Integer, ByVal Count As Integer, exceptNewsId As Integer) As IDataReader

        Public MustOverride Function NV_News_select5lastestnews(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_select6lastestcatnews(ByVal PortalId As Integer, ByVal catId As Integer) As IDataReader

        Public MustOverride Function NV_News_selectlastestnews(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_selectCustomeNews(ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal Count As Integer) As IDataReader

        Public MustOverride Function NV_News_selecttopcatbycatid(ByVal categoryid As Integer) As IDataReader

        Public MustOverride Function NV_News_selecttopcatbyphongbanid(ByVal phongbanId As Integer) As IDataReader

        Public MustOverride Function NV_News_selecttopnewsbycatid(ByVal categoryid As Integer) As IDataReader

        Public MustOverride Function NV_News_selecttopnormalnews(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NV_News_selectapprovenew(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer) As IDataReader

        Public MustOverride Function NV_News_selectnewsinsamecat(ByVal exceptNewid As Integer, ByVal catid As Integer, ByVal count As Integer, ByVal includeChildrenCat As Boolean) As IDataReader

        Public MustOverride Function NV_News_selectnewsinsamephongban(ByVal exceptNewid As Integer, ByVal arrPhongBan As String, ByVal count As Integer) As IDataReader

        Public MustOverride Function NV_News_selectothertopsitenews(ByVal PortalId As Integer) As IDataReader

        'TrungNS:
        Public MustOverride Function AdminFindSourceText_Count(ByVal sourcetext As String, ByVal Portalid As Integer) As Integer
        Public MustOverride Function Select_Count(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal arrPhongBan As String, ByVal isImage As Integer) As Integer
        Public MustOverride Function Select_Index(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal isImage As Integer) As IDataReader

        Public MustOverride Function FindContent_Count(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As Integer
        Public MustOverride Function FindContent_Index(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As IDataReader

        Public MustOverride Function FindContentExact_Count(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As Integer
        Public MustOverride Function FindContentExact_Index(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As IDataReader

        Public MustOverride Function Findbystatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
        Public MustOverride Function Findbystatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader

        Public MustOverride Function SelectApproveNews_Count(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, status As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal isImage As Boolean) As Integer
        Public MustOverride Function SelectApproveNews_Index(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, Status As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As IDataReader

        Public MustOverride Function FindNews_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, isImage As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
        Public MustOverride Function FindNews_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, isImage As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader

        Public MustOverride Function FindImages_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
        Public MustOverride Function FindImages_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader

        Public MustOverride Sub NV_News_updatePublishedDate(ByVal newid As Integer, ByVal publishedDate As DateTime, ByVal userid As Integer)
        Public MustOverride Sub NV_News_updateUsersGet(ByVal newid As Integer, ByVal usersGet As String)
        Public MustOverride Sub NV_News_updateUsersView(ByVal newid As Integer, ByVal usersView As String)

        'New LOGIC
        Public MustOverride Function SelectApproveNews_CountExt(ByVal UserId As Integer, ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal createdUser As Integer, ByVal isImage As Boolean) As Integer
        Public MustOverride Function SelectApproveNews_IndexExt(ByVal UserId As Integer, ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal CreatedUser As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As IDataReader

        Public MustOverride Function Findbystatus_CountExt(ByVal ToUserID As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
        Public MustOverride Function Findbystatus_IndexExt(ByVal ToUserID As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader

        Public MustOverride Sub NV_News_updateArchiving(ByVal newid As Integer, ByVal isArchived As Boolean, ByVal storagefolder As String)
        Public MustOverride Sub NV_News_updateTaping(ByVal newid As Integer, ByVal isTaped As Boolean)


        Public MustOverride Function FindByPhongBanId_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal arrPhongBan As String, ByVal isImage As Boolean) As Integer
        Public MustOverride Function FindByPhongBanId_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal isImage As Boolean) As IDataReader
        Public MustOverride Function FindHome_Count(ByVal key As String) As Integer
        Public MustOverride Function FindHome_Index(ByVal key As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        'Files handler
        Public MustOverride Function FindFiles_Count(ByVal folderID As Integer, ByVal type As Integer, ByVal key As String, ByVal fromDate As DateTime, ByVal toDate As DateTime) As Integer
        Public MustOverride Function FindFiles_Index(ByVal folderID As Integer, ByVal type As Integer, ByVal key As String, ByVal fromDate As DateTime, ByVal toDate As DateTime, ByVal sortDir As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function AdminFind_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean) As Integer
        Public MustOverride Function AdminFind_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function FindDatBai_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal requestedtitle As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
        Public MustOverride Function FindDatBai_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal requestedtitle As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader

        Public MustOverride Sub NV_News_updateArchiving(ByVal newid As Integer, ByVal Status As Integer, ByVal userid As Integer, ByVal atDate As DateTime)

        Public MustOverride Sub NV_News_updateLock(ByVal NewId As Integer, ByVal lock As Boolean, ByVal userid As Integer)
        Public MustOverride Function NV_News_GetLocks(ByVal PortalId As Integer, Optional ByVal newsid As Integer = 0) As IDataReader

        Public MustOverride Function NV_News_SelectTopView(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer, fromdate As DateTime, ByVal categoryid As Integer, ByVal arrPhongBan As String) As IDataReader
        Public MustOverride Function NV_News_SelectTopGet(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer, fromdate As DateTime, ByVal categoryid As Integer, ByVal arrPhongBan As String) As IDataReader

        Public MustOverride Function FindByPhongBanStatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
        Public MustOverride Function FindByPhongBanStatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Sub NV_News_updateProcessUserID(ByVal newid As Integer, ByVal userid As Integer)

        Public MustOverride Function FindNotUse_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal arrExceptPB As String) As Integer
        Public MustOverride Function FindNotUse_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal arrExceptPB As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function addTerm(ByVal vocabularyid As Integer, name As String, des As String, weight As Integer, createduserid As Integer) As Integer
        Public MustOverride Sub NV_News_IncrementViewCount(ByVal newid As Integer)
        Public MustOverride Sub Admin_News_update_Tacgia(ByVal newid As Integer, ByVal Tacgia As String)
        Public MustOverride Sub Admin_News_update_Category(ByVal NewId As Integer, ByVal CategoryId As Integer)
        Public MustOverride Sub Admin_News_updateNhuanBut(ByVal newid As Integer, ByVal Credit As Integer)
        Public MustOverride Function User_GetTongView(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer) As Integer
#End Region
#Region "show"
        Public MustOverride Function Show_BaiMoiNhat(subtractIds As String, ByVal PortalId As Integer, ByVal Count As Integer) As IDataReader
        Public MustOverride Function Show_ShowBaiMoiDanhMuc(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, Count As Integer, isImage As Boolean) As IDataReader
        Public MustOverride Function Show_TopViewSite(ByVal PortalId As Integer, SoNgay As Integer, Count As Integer) As IDataReader
        Public MustOverride Function Show_SelectTopCatNews(ByVal PortalId As Integer, ByVal Count As Integer, exceptNewsId As Integer) As IDataReader
        Public MustOverride Function Show_SelectTopCatNewsHOT(ByVal subtractIds As String, ByVal CategoryId As Integer, count As Integer, Portalid As Integer) As IDataReader
        Public MustOverride Function Show_Select_Count(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, languageId As String, ByVal isImage As Boolean) As Integer
        Public MustOverride Function Show_Select_Index(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, languageId As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As IDataReader
        Public MustOverride Function Show_ShowSelectNewsInSameCat(ByVal exceptNewid As Integer, ByVal catid As Integer, ByVal count As Integer, ByVal includeChildrenCat As Boolean) As IDataReader
        Public MustOverride Function Show_YearMonth(ByVal PortalId As Integer, ByVal Year As Integer, Month As Integer, Day As Integer) As IDataReader
#End Region
#Region "NV_NewsFeedback"

        Public MustOverride Function NV_NewsFeedback_GetByID(ByVal NewsFeedbackId As Integer) As IDataReader

        Public MustOverride Function NV_NewsFeedback_GetAll() As IDataReader

        Public MustOverride Function NV_NewsFeedback_GetByNewsId(ByVal NewsId As Integer) As IDataReader

        Public MustOverride Function NV_NewsFeedback_GetByPortalId(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Sub NV_NewsFeedback_Insert(ByVal NewsId As Integer, ByVal FullName As String, ByVal Email As String, ByVal CreateDate As DateTime, ByVal PhoneNumber As String, ByVal Title As String, ByVal Content As String, ByVal Address As String, ByVal IPTrack As String, ByVal Status As Integer)

        Public MustOverride Sub NV_NewsFeedback_Delete(ByVal NewsFeedbackId As Integer)

        Public MustOverride Sub NV_NewsFeedback_Update(ByVal NewsFeedbackId As Integer, ByVal NewsId As Integer, ByVal FullName As String, ByVal Email As String, ByVal CreateDate As DateTime, ByVal PhoneNumber As String, ByVal Title As String, ByVal Content As String, ByVal Address As String, ByVal IPTrack As String, ByVal Status As Integer)

        Public MustOverride Function NV_NewsFeedback_GetByNewsId_Count(ByVal NewsId As Integer, ByVal Status As Integer) As Integer
        Public MustOverride Function NV_NewsFeedback_GetByNewsId_Index(ByVal NewsId As Integer, ByVal Status As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

#End Region
#Region "NV_NewsStatus"

        Public MustOverride Function NV_NewsStatus_GetByID(ByVal NewsStatusId As Integer) As IDataReader

        Public MustOverride Function NV_NewsStatus_GetAll() As IDataReader

        Public MustOverride Sub NV_NewsStatus_Insert(ByVal StatusName As String, ByVal Description As String)

        Public MustOverride Sub NV_NewsStatus_Delete(ByVal NewsStatusId As Integer)

        Public MustOverride Sub NV_NewsStatus_Update(ByVal NewsStatusId As Integer, ByVal StatusName As String, ByVal Description As String)

#End Region
#Region "NV_NewsByCategory"
        Public MustOverride Function NV_NewsByCategory_GetByID(ByVal Id As Integer) As IDataReader

        Public MustOverride Function NV_NewsByCategory_GetAll() As IDataReader

        Public MustOverride Function NV_NewsByCategory_Insert(ByVal NewsId As Integer, ByVal CategoryId As Integer, ByVal IsMainCategory As Boolean) As Integer

        Public MustOverride Sub NV_NewsByCategory_Delete(ByVal Id As Integer)

        Public MustOverride Sub NV_NewsByCategory_DeleteByNewsId(ByVal NewsId As Integer)

        Public MustOverride Sub NV_NewsByCategory_Update(ByVal Id As Integer, ByVal NewsId As Integer, ByVal CategoryId As Integer, ByVal IsMainCategory As Boolean)

        Public MustOverride Function NV_NewsByCategory_GetByNewsId(ByVal newsId As Integer) As IDataReader
#End Region
#Region "Phan quyen"

        Public MustOverride Function Permissions_GetAllUsersByRole(ByVal roleId As Integer) As IDataReader

        Public MustOverride Function Permissions_GetAllUsersByRoles(ByVal arrRoleId As String) As IDataReader

        Public MustOverride Function Permissions_AddUserPermissionByCategories(ByVal userId As Integer, ByVal categoryId As Integer, ByVal permissionType As Integer) As Integer

        Public MustOverride Sub Permissions_DeleteUserPermissionByRole(ByVal userId As Integer, ByVal roleId As Integer)

        Public MustOverride Sub Permissions_DeleteUserPermissionByRoleAndCategory(ByVal categoryId As Integer, ByVal roleId As Integer)

        Public MustOverride Function Permissions_GetAllCategoriesByUserIdAndRoleId(ByVal userId As Integer, ByVal roleId As Integer, ByVal languageId As String) As IDataReader

        Public MustOverride Function Permissions_GetNotAssignedCategoriesByUserIdAndRoleId(ByVal userId As Integer, ByVal roleId As Integer, ByVal languageId As String) As IDataReader

        Public MustOverride Function Permissions_GetAllAssignedUsersByRoleIdAndCategoryId(ByVal categoryId As Integer, ByVal roleId As Integer, ByVal languageId As String) As IDataReader

#End Region
#Region "NewsByTags"

        Public MustOverride Function NewsByTags_GetByTags_Index(ByVal Tags As String, PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function NewsByTags_GetByTags_Count(ByVal Tags As String, PortalId As Integer) As Integer

        Public MustOverride Function NewsByTags_GetByNewId(ByVal NewId As Integer) As IDataReader

        Public MustOverride Function NewsByTags_GetByTags(ByVal Tags As String) As IDataReader

        Public MustOverride Function NewsByTags_GetAll() As IDataReader

        Public MustOverride Function NewsByTags_GetByTags(ByVal subtractIds As String, Tags As String, Count As Integer) As IDataReader

        Public MustOverride Function NewsByTags_GetAllAutoComplate() As IDataReader

        Public MustOverride Sub NewsByTags_Insert(ByVal NewId As Integer, ByVal Tags As String, TagsTitle As String, ByVal PortalId As Integer)

        Public MustOverride Sub NewsByTags_DeleteByNewId(ByVal Newid As Integer)


#End Region
#Region "News_Version"
        Public MustOverride Function News_Version_GetById(ByVal Id As Integer) As IDataReader

        Public MustOverride Function News_Version_GetAll() As IDataReader

        Public MustOverride Function News_Version_Insert(ByVal objInfo As NewsVersionInfo) As Integer

        Public MustOverride Sub News_Version_Delete(ByVal Id As Integer)

        Public MustOverride Sub News_Version_Update(ByVal objInfo As NewsVersionInfo)

        Public MustOverride Sub News_Version_DeleteByNewsID(ByVal newsID As Integer)
#End Region
#Region "News_Process"
        Public MustOverride Function News_Process_GetById(ByVal ID As Integer) As IDataReader

        Public MustOverride Function News_Process_GetAll() As IDataReader

        Public MustOverride Function News_Process_Insert(ByVal objInfo As NewsProcessInfo) As Integer

        Public MustOverride Sub News_Process_Delete(ByVal ID As Integer)

        Public MustOverride Sub News_Process_Update(ByVal objInfo As NewsProcessInfo)

        Public MustOverride Function News_Process_GetByNewsId(ByVal newsId As Integer) As IDataReader

        Public MustOverride Function News_Process_GetCurrentProcess(ByVal newsId As Integer) As IDataReader

        Public MustOverride Function News_Process_GetLastProcessByStatus(ByVal newsId As Integer, ByVal status As Integer) As IDataReader

        Public MustOverride Sub News_Process_DeleteByNewsID(ByVal newsID As Integer)
#End Region
#Region "ViewNews"

        Public MustOverride Function NV_ViewNews_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function NV_ViewNews_GetAll() As IDataReader

        Public MustOverride Sub NV_ViewNews_Insert(ByVal userid As Integer, ByVal newsid As Integer)

        Public MustOverride Sub NV_ViewNews_Delete(ByVal id As Integer)

        Public MustOverride Sub NV_ViewNews_Update(ByVal id As Integer, ByVal userid As Integer, ByVal newsid As Integer)

        Public MustOverride Function NV_ViewNews_GetByUserId(ByVal userid As Integer) As IDataReader

        Public MustOverride Function NV_ViewNews_GetByNewsId(ByVal newsid As Integer) As IDataReader

        Public MustOverride Function NV_ViewNews_GetByNewsIdAndUserId(ByVal newsid As Integer, ByVal userid As Integer) As IDataReader
#End Region
#Region "News_Me"
        Public MustOverride Function NV_News_Me_add(ByVal categoryid As Integer, ByVal title As String, ByVal imagepath As String, ByVal summary As String, ByVal content As String, ByVal isactive As Integer, ByVal hotcat As Integer, ByVal hotsite As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Exsummary As String, ByVal TypeUrl As String) As Integer
        Public MustOverride Sub NV_News_Me_delete(ByVal categoryid As Integer)
        Public MustOverride Sub NV_News_Me_update(ByVal newid As Integer, ByVal categoryid As Integer, ByVal title As String, ByVal imagepath As String, ByVal summary As String, ByVal content As String, ByVal isactive As Integer, ByVal hotcat As Integer, ByVal hotsite As Integer, ByVal PortalId As Integer, ByVal Exsummary As String, ByVal TypeUrl As String)
        Public MustOverride Function NV_News_Me_selectall(ByVal PortalId As Integer) As IDataReader
        Public MustOverride Function NV_News_Me_selectbyid(ByVal newid As Integer) As IDataReader
        Public MustOverride Function News_Me_Findbystatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer) As Integer
        Public MustOverride Function News_Me_Findbystatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
#End Region
#Region "News_UserWF"

        Public MustOverride Function News_UserWF_GetById(ByVal ID As Integer) As IDataReader

        Public MustOverride Function News_UserWF_GetAll() As IDataReader

        Public MustOverride Function News_UserWF_GetByUserId(ByVal LoaiWF As LoaiWF, ByVal UserId As Integer) As IDataReader

        Public MustOverride Function News_UserWF_GetByPhongBanId(ByVal LoaiWF As LoaiWF, ByVal phongbanID As Integer) As IDataReader

        Public MustOverride Function News_UserWF_Insert(ByVal objInfo As News_UserWFInfo) As Integer

        Public MustOverride Sub News_UserWF_Delete(ByVal ID As Integer)

        Public MustOverride Sub News_UserWF_Update(ByVal objInfo As News_UserWFInfo)

        Public MustOverride Sub News_UserWF_DeleteByPhongBanId(ByVal phongbanID As Integer)
#End Region
#Region "News_UserProcess"

        Public MustOverride Function News_UserProcess_GetById(ByVal ID As Integer) As IDataReader

        Public MustOverride Function News_UserProcess_GetAll() As IDataReader

        Public MustOverride Function News_UserProcess_GetByUserId(ByVal UserId As Integer) As IDataReader

        Public MustOverride Function News_UserProcess_Insert(ByVal objInfo As News_UserProcessInfo) As Integer

        Public MustOverride Sub News_UserProcess_Delete(ByVal ID As Integer)

        Public MustOverride Sub News_UserProcess_Update(ByVal objInfo As News_UserProcessInfo)

        Public MustOverride Sub News_UserProcess_DeleteByNewsID(ByVal NewsID As Integer)
#End Region
#Region "NewsByMedia"

        Public MustOverride Function NewsByMedia_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function NewsByMedia_GetAllByNewid(ByVal newid As Integer) As IDataReader
        Public MustOverride Sub NewsByMedia_Insert(ByVal newid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub NewsByMedia_Delete(ByVal id As Integer)

        Public MustOverride Sub NewsByMedia_DeleteByNewId(ByVal NewIdid As Integer)

        Public MustOverride Sub NewsByMedia_DeleteByMediaId(ByVal Mediaid As Integer)

        Public MustOverride Sub NewsByMedia_Update(ByVal id As Integer, ByVal newid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub NewsByMedia_UpdateNewId(ByVal newid As Integer, newidnew As Integer)

#End Region
#Region "NVCMS_MediaItem"

        Public MustOverride Function MediaItem_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function MediaItem_GetAll(Portalid As Integer) As IDataReader

        Public MustOverride Function MediaItem_Insert(ByVal title As String, ByVal filename As String, ByVal forder As String, ByVal MediaUrl As String, ByVal Size As Integer, ByVal extension As String, ByVal createddate As DateTime, ByVal userid As Integer, ByVal portalid As Integer) As Integer

        Public MustOverride Sub MediaItem_Delete(ByVal id As Integer)

        Public MustOverride Sub MediaItem_UpdateTitle(ByVal id As Integer, ByVal title As String)


#End Region
#Region "News_Template"
        Public MustOverride Function News_Template_Get(ByVal PortalID As Integer, ByVal TemplateId As Integer) As IDataReader
        Public MustOverride Function News_Template_GetAll(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Sub News_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalID As Integer)
        Public MustOverride Sub News_Template_Update(ByVal TemplateId As Integer, ByVal TemplateName As String, ByVal FilePath As String)
        Public MustOverride Sub News_Template_Delete(ByVal TemplateId As Integer)
#End Region
#Region "News_Settings"

        Public MustOverride Function News_Settings_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function News_Settings_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function News_Settings_GetAllByType(Type As Integer, Count As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Sub News_Settings_Insert(ByVal NewId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub News_Settings_Delete(Type As Integer, PortalId As Integer)
        Public MustOverride Sub News_Settings_DeleteById(Id As Integer, PortalId As Integer)
        Public MustOverride Sub News_Settings_DeleteByNewId(ByVal NewId As Integer, Type As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub News_Settings_Update(ByVal id As Integer, ByVal NewId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub News_Settings_UpdateOrder(ByVal id As Integer, ByVal OrderNumber As Integer)

#End Region
#Region "NewsByShare"

        Public MustOverride Function NewsByShare_GetByNewID(ByVal NewId As Integer) As IDataReader
        Public MustOverride Function NewsByShare_GetCountByNewId(ByVal NewId As Integer) As Integer
        'Public MustOverride Function NewsByShare_GetAll() As IDataReader

        Public MustOverride Sub NewsByShare_Insert(ByVal NewId As Integer, ByVal LinkShare As String, ByVal CreatedDate As DateTime)

        'Public MustOverride Sub NewsByShare_Delete(ByVal id As Integer)

#End Region
#Region "NewsByView"

        Public MustOverride Function NewsByView_GetByNewID(ByVal Newid As Integer) As IDataReader
        Public MustOverride Sub NewsByView_Insert(ByVal NewId As Integer, ByVal ViewCount As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub NewsByView_Update(ByVal NewId As Integer)

#End Region
#Region "News_NhuanBut"

        Public MustOverride Function News_NhuanBut_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function News_NhuanBut_GetAll(ByVal NewId As Integer, KieuNhuanBut As Integer) As IDataReader

        Public MustOverride Function News_NhuanBut_GetCount(ByVal NewId As Integer, KieuNhuanBut As Integer) As Integer

        Public MustOverride Function News_NhuanBut_GetTongTien(ByVal NewId As Integer, KieuNhuanBut As Integer) As Integer

        Public MustOverride Sub News_NhuanBut_Insert(ByVal NewId As Integer, ByVal Type As Integer, ByVal UserId As Integer, ByVal Credit As Integer, ByVal Createdate As DateTime, ByVal CreateUser As Integer, ByVal UserChamNhuanBut As Integer, ByVal PortalId As Integer, KieuNhuanBut As Integer)

        Public MustOverride Sub News_NhuanBut_Delete(ByVal id As Integer)

        Public MustOverride Sub News_NhuanBut_Update(ByVal id As Integer, ByVal NewId As Integer, ByVal Type As Integer, ByVal UserId As Integer, ByVal Credit As Integer, ByVal Createdate As DateTime, ByVal CreateUser As Integer, ByVal UserChamNhuanBut As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub News_NhuanBut_UpdateNhuan(ByVal id As Integer, ByVal Credit As Integer, ByVal UserChamNhuanBut As Integer)
        Public MustOverride Sub News_NhuanBut_UpdateNhuanXuatBan(ByVal NewId As Integer, ByVal UserChamNhuanBut As Integer, UserChamNhuanButDate As DateTime, XuatBan As Boolean, KieuNhuanBut As Integer)

        Public MustOverride Function News_NhuanBut_Find_Count(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, KieuNhuanBut As Integer) As Integer
        Public MustOverride Function News_NhuanBut_Find_Index(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, KieuNhuanBut As Integer) As IDataReader
        Public MustOverride Function News_NhuanBut_Find_Index_Export(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, KieuNhuanBut As Integer) As Object
        Public MustOverride Function NhuanBut_User_GetTongTien(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer) As Integer

#End Region
#Region "News_Attach"
        Public MustOverride Sub News_AttachByPhongBan_Add(ByVal attachByPhongBan As News_AttachByPhongBanInfo)
        Public MustOverride Sub News_AttachByPhongBan_DeleteByAttachId(ByVal AttachId As Integer)
        Public MustOverride Sub News_AttachByPhongBan_DeleteByPhongBanId(ByVal PhongBanId As Integer)
        Public MustOverride Function News_AttachByPhongBan_GetByAttachId(ByVal attachId As Integer) As IDataReader

        Public MustOverride Sub NewsByAttach_Add(ByVal newsByAttach As NewsByAttachInfo)
        Public MustOverride Sub NewsByAttach_DeleteByAttachId(ByVal AttachId As Integer)
        Public MustOverride Sub NewsByAttach_DeleteByNewsId(ByVal NewsId As Integer)

        Public MustOverride Function News_Attach_Get(ByVal AttachId As Integer) As IDataReader
        Public MustOverride Function News_Attach_GetByNewId(ByVal PortalId As Integer, ByVal NewsId As Integer, ByVal UserId As Integer) As IDataReader
        Public MustOverride Function News_Attach_GetMaxId() As IDataReader
        Public MustOverride Function News_Attach_Add(ByVal newsAttach As NewsAttachInfo) As Integer
        Public MustOverride Sub News_Attach_Update(ByVal newsAttach As NewsAttachInfo)
        Public MustOverride Sub News_Attach_UpdateFileName(ByVal AttachId As Integer, ByVal FileName As String)
        Public MustOverride Sub News_Attach_Delete(ByVal AttachId As Integer)
        Public MustOverride Sub News_Attach_SwapSort(ByVal FirstId As Integer, ByVal SecondId As Integer)
#End Region
#Region "News_Note"
        Public MustOverride Function News_Note_GetbyNewIdTop1(ByVal NewId As Integer) As IDataReader
        Public MustOverride Function News_Note_GetbyNewId(ByVal NewId As Integer) As IDataReader
        Public MustOverride Sub News_Note_Insert(ByVal objInfo As NewsNoteInfo)

#End Region

#End Region

    End Class
End Namespace
