using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NVCMS.API.Analytics
{
    /// <summary>
    /// Dựng yêu cầu GA4 Data API (runReport / batchRunReports / runRealtimeReport) và đọc kết quả thành bảng.
    /// Hai khoảng ngày được đặt tên "cur" / "prev": GA4 tự thêm dimension "dateRange" (đọc theo tên header, không theo vị trí).
    /// Yêu cầu là Dictionary để không gửi trường null.
    /// </summary>
    internal sealed class Ga4Query
    {
        private readonly Ga4Client _p;
        private string _token;

        public Ga4Query(AnalyticsSettings s)
        {
            _p = new Ga4Client(s);
            Conv = s.KeyEvents.Count > 0 ? s.KeyEvents.Select(e => "keyEvents:" + e).ToArray() : new[] { "keyEvents" };
        }

        /// <summary>Metric chuyển đổi: keyEvents (tổng) hoặc keyEvents:{tên} của từng sự kiện cấu hình.</summary>
        public string[] Conv { get; }

        /// <summary>Bộ metric chuẩn cho bảng phiên (MkMetrics).</summary>
        public string[] Std => new[] { "activeUsers", "newUsers", "sessions", "engagedSessions", "screenPageViews", "userEngagementDuration" }.Concat(Conv).ToArray();

        /// <summary>Bộ metric cho trang (không có sessions / newUsers).</summary>
        public string[] PageMets => new[] { "screenPageViews", "activeUsers", "userEngagementDuration" }.Concat(Conv).ToArray();

        private string Token() => _token ?? (_token = _p.Token());

        // ------------------------------------------------------------------ dựng yêu cầu
        public static Dictionary<string, object> Range(DateTime f, DateTime t, string name) => new Dictionary<string, object>
        {
            ["startDate"] = f.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["endDate"] = t.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["name"] = name
        };

        /// <summary>
        /// r: khoảng; prev = true thêm kỳ trước. order: tên metric sắp giảm dần ("" = theo dimension đầu tiên tăng dần, null = mặc định).
        /// </summary>
        public static Dictionary<string, object> Req(MkRange r, bool prev, string[] dims, string[] mets, object filter = null, string order = null, int limit = 0)
        {
            var ranges = new List<object> { Range(r.From, r.To, "cur") };
            if (prev) ranges.Add(Range(r.PrevFrom, r.PrevTo, "prev"));
            var q = new Dictionary<string, object>
            {
                ["dateRanges"] = ranges,
                ["metrics"] = mets.Select(m => new Dictionary<string, object> { ["name"] = m }).ToList()
            };
            if (dims != null && dims.Length > 0) q["dimensions"] = dims.Select(d => new Dictionary<string, object> { ["name"] = d }).ToList();
            if (filter != null) q["dimensionFilter"] = filter;
            if (order == "" && dims != null && dims.Length > 0)
                q["orderBys"] = new[] { new Dictionary<string, object> { ["dimension"] = new Dictionary<string, object> { ["dimensionName"] = dims[0] } } };
            else if (!string.IsNullOrEmpty(order))
                q["orderBys"] = new[] { new Dictionary<string, object> { ["metric"] = new Dictionary<string, object> { ["metricName"] = order }, ["desc"] = true } };
            if (limit > 0) q["limit"] = limit;
            return q;
        }

        // ------------------------------------------------------------------ bộ lọc
        public static object Exact(string field, string value) => Leaf(field, new Dictionary<string, object> { ["stringFilter"] = new Dictionary<string, object> { ["matchType"] = "EXACT", ["value"] = value ?? "", ["caseSensitive"] = true } });

        public static object Contains(string field, string value) => Leaf(field, new Dictionary<string, object> { ["stringFilter"] = new Dictionary<string, object> { ["matchType"] = "CONTAINS", ["value"] = value ?? "", ["caseSensitive"] = false } });

        public static object InList(string field, IEnumerable<string> values) => Leaf(field, new Dictionary<string, object> { ["inListFilter"] = new Dictionary<string, object> { ["values"] = values.ToList(), ["caseSensitive"] = true } });

        public static object Or(params object[] exprs) => new Dictionary<string, object> { ["orGroup"] = new Dictionary<string, object> { ["expressions"] = exprs.Where(x => x != null).ToList() } };

        public static object And(params object[] exprs) => new Dictionary<string, object> { ["andGroup"] = new Dictionary<string, object> { ["expressions"] = exprs.Where(x => x != null).ToList() } };

        public static object Not(object expr) => new Dictionary<string, object> { ["notExpression"] = expr };

        private static object Leaf(string field, Dictionary<string, object> f)
        {
            f["fieldName"] = field;
            return new Dictionary<string, object> { ["filter"] = f };
        }

        // ------------------------------------------------------------------ gọi API
        /// <summary>Tối đa 5 yêu cầu / lần (giới hạn của batchRunReports).</summary>
        public List<GaTable> Batch(params object[] requests)
        {
            var result = new List<GaTable>();
            for (int i = 0; i < requests.Length; i += 5)
            {
                JObject res = _p.Post(Token(), "batchRunReports", new Dictionary<string, object> { ["requests"] = requests.Skip(i).Take(5).ToList() });
                JArray reports = res["reports"] as JArray ?? new JArray();
                for (int k = 0; k < Math.Min(5, requests.Length - i); k++)
                    result.Add(k < reports.Count ? GaTable.Parse(reports[k] as JObject) : new GaTable());
            }
            return result;
        }

        public GaTable Run(object request) => GaTable.Parse(_p.Post(Token(), "runReport", request));

        public GaTable Realtime(object request) => GaTable.Parse(_p.Post(Token(), "runRealtimeReport", request));
    }

    /// <summary>Kết quả một báo cáo: dòng = giá trị dimension (theo tên) + metric (theo tên).</summary>
    internal sealed class GaTable
    {
        public List<GaRow> Rows { get; } = new List<GaRow>();
        public long RowCount { get; private set; }

        public static GaTable Parse(JObject report)
        {
            var t = new GaTable();
            if (report == null) return t;
            List<string> dims = (report["dimensionHeaders"] as JArray ?? new JArray()).Select(h => (string)h["name"]).ToList();
            List<string> mets = (report["metricHeaders"] as JArray ?? new JArray()).Select(h => (string)h["name"]).ToList();
            t.RowCount = report["rowCount"] != null && long.TryParse((string)report["rowCount"], out long rc) ? rc : 0;
            if (!(report["rows"] is JArray rows)) return t;
            foreach (JToken row in rows)
            {
                var r = new GaRow();
                JArray dv = row["dimensionValues"] as JArray ?? new JArray(), mv = row["metricValues"] as JArray ?? new JArray();
                for (int i = 0; i < dims.Count && i < dv.Count; i++) r.Dims[dims[i]] = (string)dv[i]["value"] ?? "";
                for (int i = 0; i < mets.Count && i < mv.Count; i++)
                    r.Mets[mets[i]] = double.TryParse((string)mv[i]["value"], NumberStyles.Float, CultureInfo.InvariantCulture, out double d) ? d : 0;
                t.Rows.Add(r);
            }
            return t;
        }

        public IEnumerable<GaRow> Cur => Rows.Where(r => r.Range != "prev");
        public IEnumerable<GaRow> Prev => Rows.Where(r => r.Range == "prev");
    }

    internal sealed class GaRow
    {
        public Dictionary<string, string> Dims { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
        public Dictionary<string, double> Mets { get; } = new Dictionary<string, double>(StringComparer.Ordinal);

        public string D(string name) => Dims.TryGetValue(name, out string v) ? v : "";
        public double M(string name) => Mets.TryGetValue(name, out double v) ? v : 0;
        public long L(string name) => (long)Math.Round(M(name));
        public string Range => Dims.TryGetValue("dateRange", out string v) ? v : "cur";

        /// <summary>Tổng các metric chuyển đổi.</summary>
        public double Conv(string[] conv) => conv.Sum(M);

        public MkMetrics Metrics(string[] conv) => Fill(new MkMetrics(), conv);

        public T Fill<T>(T m, string[] conv) where T : MkMetrics
        {
            m.Users = L("activeUsers");
            m.NewUsers = L("newUsers");
            m.Sessions = L("sessions");
            m.EngagedSessions = L("engagedSessions");
            m.Views = L("screenPageViews");
            m.EngagementSec = Math.Round(M("userEngagementDuration"), 1);
            m.Conversions = Conv(conv);
            return m;
        }
    }
}
