using Newtonsoft.Json;

namespace NG.TwitterApi
{
	public delegate void SearchResponseDelegatte(SearchResponse response);
	public class SearchResponse
	{
		[JsonProperty("statuses")]
		private SimpleTweet[] statusesIn;
		[JsonIgnore]
		public SimpleTweet[] statuses
		{
			get
			{
				SimpleTweet[] immutable = new SimpleTweet[statusesIn.Length];
				statusesIn.CopyTo(immutable, 0);
				return immutable;
			}
		}

		[JsonProperty("errors")]
		private Error[] errorsIn;
		[JsonIgnore]
		public Error[] errors
		{
			get
			{
				Error[] immutable = new Error[errorsIn.Length];
				errorsIn.CopyTo(immutable, 0);
				return errorsIn;
			}
		}
	}
}