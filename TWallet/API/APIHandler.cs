using System.Net.Http;
using System.Threading.Tasks;

namespace TWallet.API
{
	public class APIHandler
	{
		public static Task<HttpResponseMessage> Get(string url, HttpClientHandler handler)
		{
			using (var client = new HttpClient(handler, false))
			{
				return client.GetAsync(url);
			}
		}
	}
}