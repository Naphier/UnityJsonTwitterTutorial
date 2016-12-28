using System;

namespace NG.TwitterApi
{
	public delegate void OAuthResponseDelegate(OAuthResponse response);

	public class OAuthResponse
	{
		public string token_type;
		public string access_token;
		public Error[] errors;

		private DateTime time = DateTime.Now;
		private const int AUTHORIZATION_LIFETIME_SECONDS = 30 * 60; // 30 minutes

		public bool isValid
		{
			get
			{
				if ((DateTime.Now - time).TotalSeconds > AUTHORIZATION_LIFETIME_SECONDS)
					return false;

				return !string.IsNullOrEmpty(token_type) && !string.IsNullOrEmpty(access_token);
			}
		}

		public override string ToString()
		{
			return string.Format(
				"isValid: {0}\ntoken_type: {1}\naccess_token: {2}\nerror(s):{3}",
				isValid,
				(string.IsNullOrEmpty(token_type) ? "NULL" : token_type),
				(string.IsNullOrEmpty(access_token) ? "NULL" : access_token),
				((errors == null || errors.Length == 0) ? "NONE" : "\n" + errors.ToStringExt())
				);
		}
	}
}