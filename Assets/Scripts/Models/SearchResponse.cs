using Newtonsoft.Json;

namespace NG.TwitterApi
{
	public delegate void SearchResponseDelegate(SearchResponse response);
	public class SearchResponse
	{
		[JsonProperty("statuses")]
		private SimpleTweet[] statusesIn;
		[JsonIgnore]
		public SimpleTweet[] statuses
		{
			get
			{
				if (statusesIn == null || statusesIn.Length < 1)
				{
					return new SimpleTweet[0];
				}
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
				if (errorsIn == null || errorsIn.Length < 1)
					return new Error[0];

				Error[] immutable = new Error[errorsIn.Length];
				errorsIn.CopyTo(immutable, 0);
				return errorsIn;
			}
		}

		public override string ToString()
		{
			string statusesOut = "";

			if (statuses == null)
				statusesOut = "\tNULL";
			else if (statuses.Length <= 0)
				statusesOut = "\tno results";
			else
			{
				for (int i = 0; i < statuses.Length; i++)
				{
					statusesOut += "\t" + statuses[i].ToString();
					if (i < statuses.Length - 1)
						statusesOut += "\n";
				}
			}

			return string.Format("statuses:\n{0}\nerrors:{1}", statusesOut, errors.ToStringExt());
		}
	}
}