namespace NG.TwitterApi
{
	public class Error
	{
		public int code;
		public string message;
		public string label;

		public override string ToString()
		{
			return string.Format(
				"code: {0}\nmessage: {1}\nlabel: {2}",
				code, message, label);
		}
	}


	public static class ErrorExt
	{
		public static string ToStringExt(this Error[] errors, string separator = "\n")
		{
			string s = "";

			for (int i = 0; i < errors.Length; i++)
			{
				s += errors[i].ToString();
				if (i < errors.Length - 1)
					s += separator;
			}

			if (string.IsNullOrEmpty(s))
				s = "NONE";

			return s;
		}
	}
}