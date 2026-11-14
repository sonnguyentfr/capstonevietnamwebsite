using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface ICrmDataLadingRepository : IRepository<student_from_ladipage>
    {
        Task<IEnumerable<student_from_ladipage>> GetByEmailOrPhoneAsync(string emails, string phones);

        /// <summary>
        /// Insert cả lô; nếu lô lỗi thì insert lại từng dòng, dòng nào lỗi thì bỏ qua.
        /// Trả về các dòng KHÔNG insert được kèm exception.
        /// </summary>
        Task<List<(student_from_ladipage Record, Exception Error)>> AddRangeSkipFailuresAsync(
            IReadOnlyList<student_from_ladipage> records);
    }
}