using Newtonsoft.Json;

namespace NG.TwitterApi
{
	public class SimpleTweet
	{
		[JsonProperty("created_at")]
		public string createdAt { get; private set; }
		[JsonProperty]
		public string id_str { get; private set; }
		[JsonProperty]
		public string text { get; private set; }

		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(text);
			}
		}


		public override string ToString()
		{
			return string.Format(
				"created_at: {0}\nid_str: {1}\ntext: {2}",
				createdAt, id_str, text);
		}
	}
}