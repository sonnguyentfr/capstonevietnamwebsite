using System.Collections.Generic;

namespace NVCMS.API.Model.Marketing
{
    public class ZaloPhoneValidateResult
    {
        public int ValidCount { get; set; }
        public int InvalidCount { get; set; }
        public int DupCount { get; set; }
        public List<string> ValidList { get; set; }
        public List<string> InvalidList { get; set; }
    }
}
