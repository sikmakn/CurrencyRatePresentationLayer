using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ODataClientConsoleApplication
{
    public class HistoryRateHandler
    {
        private readonly Reader _reader;
        private readonly JavaScriptSerializer _javaScriptSerializer;

        public HistoryRateHandler(Reader reader)
        {
            _reader = reader;
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        public async Task<IEnumerable<RateShortNbrb>> GetRateHistory(int currencyId, DateTime startDate, DateTime endDate)
        {
            var uri = $"http://localhost:64862/api/RateNbrb?&currencyId={currencyId}" +
                      $"&startDate={startDate.ToString("u").Split(' ')[0]}" +
                      $"&endDate={endDate.ToString("u").Split(' ')[0]}";
            var resultJson = await _reader.HttpClientRead(uri);
            var result = _javaScriptSerializer.Deserialize<IEnumerable<RateShortNbrb>>(resultJson);
            return result;
        }
    }
}
