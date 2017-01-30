using Newtonsoft.Json;
using NG.TwitterApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public enum ResultType { mixed, recent, popular };


public class TwitterSearchHandler : MonoBehaviour
{
	private string apiKey = "";
	private string secret = "";
	private string query = "";
	private OAuthResponse authorization;

	[Serializable]
	public class HandleStatusesUnityEvent : UnityEngine.Events.UnityEvent<SimpleTweet[]> { }
	public HandleStatusesUnityEvent OnStatusesRecieved;


	private void Start ()
	{
		if (string.IsNullOrEmpty(apiKey) && string.IsNullOrEmpty(secret))
		{
			string[] keySecret = LoadApiKeyAndSecret();
			apiKey = keySecret[0];
			secret = keySecret[1];
		}
	}

	public void MakeSearchRequest(string query)
	{
		this.query = query;

		if (!string.IsNullOrEmpty(apiKey))
		{
			StartCoroutine(MakeOAuthRequest(apiKey, secret, MakeSearchRequest));
		}
	}

	// This is used to load a key file from the application's folder.
	// I used this because I wanted to have my keys stored in a file and not
	// publicly accessible on the repository.
	// Note it's usage in the Start() method.
	// You can use this method or simply fill in the apiKey and secret values.
	// If you use this method, make sure to put the twitter.key file in the parent directory
	// of your project's Assets folder. Make sure to have the apiKey on the first line of the file
	// and the secret on the second line.
	private string[] LoadApiKeyAndSecret()
	{
		DirectoryInfo appDataParent = Directory.GetParent(Application.dataPath);
		string path = Path.Combine(appDataParent.FullName, "twitter.key");

		if (!Directory.Exists(appDataParent.FullName))
		{
			Debug.LogErrorFormat("Directory not found: '{0}'", appDataParent.FullName);
			return null;
		}

		if (!File.Exists(path))
		{
			Debug.LogErrorFormat("Twitter key file not found at: '{0}'", path);
			return null;
		}
		string raw = File.ReadAllText(path);
		string[] keySecret = raw.Split('\n');
		if (keySecret.Length < 2)
		{
			Debug.LogErrorFormat("Failed to parse file: '{0}'  contents: '{1}'", path, raw);
			return null;
		}

		for (int i = 0; i < keySecret.Length; i++)
		{
			keySecret[i] = keySecret[i].Trim();
		}
		
		Debug.LogFormat("apiKey: '{0}'  secret: '{1}' loaded from file {2}.",
			keySecret[0],
			keySecret[1],
			((string.IsNullOrEmpty(keySecret[0]) || string.IsNullOrEmpty(keySecret[1]))
				? "UNSUCCESFULLY" : "successfully"));

		return keySecret;
	}
	

	private IEnumerator MakeOAuthRequest(
		string apiKey, 
		string apiSecret, 
		OAuthResponseDelegate OnResponse)
	{
		// Skip future authorization requests.
		if (authorization != null && authorization.isValid && OnResponse != null)
		{
			OnResponse.Invoke(authorization);
			Debug.Log("Do not reauth");
			yield break;
		}
		

		string oAuthUrl = "https://api.twitter.com/oauth2/token";

		byte[] body = Encoding.UTF8.GetBytes("grant_type=client_credentials");

		Dictionary<string, string> headers = new Dictionary<string, string>();

		string base64KeyAndSecret = 
			Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + apiSecret));

		headers.Add("Authorization", "Basic " + base64KeyAndSecret);

		WWW request = new WWW(oAuthUrl, body, headers);

		yield return request;

		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogErrorFormat("UnityEngine.WWW Error: {0}", request.error);
		}

		OAuthResponse response = null;

		if (!string.IsNullOrEmpty(request.text))
		{
			response = JsonConvert.DeserializeObject<OAuthResponse>(request.text);
			Debug.LogFormat("OAuthResponse:\n{0}", response.ToString());
		}

		if (OnResponse != null)
			OnResponse.Invoke(response);
	}

	private void MakeSearchRequest(OAuthResponse response)
	{
		if (response == null || !response.isValid)
		{
			Debug.LogError("response is null or invalid");
			return;
		}

		authorization = response;

		StartCoroutine(
			MakeSearchRequestCoroutine(
				response.access_token, 
				query, 
				HandleSearchResponse,
				count: 10));
	}

	IEnumerator MakeSearchRequestCoroutine(
		string accessToken, string query, SearchResponseDelegate OnResponse,
		ResultType resultType = ResultType.mixed, int count = 100)
	{
		string url = "https://api.twitter.com/1.1/search/tweets.json?q=" + WWW.EscapeURL(query);

		if (resultType != ResultType.mixed)
			url += "&result_type=" + resultType.ToString();

		if (count < 100)
			url += "&count=" + count;


		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "Bearer " + accessToken);

		WWW request = new WWW(url, null, headers);

		yield return request;

		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogErrorFormat("UnityEnigine.WWW Error: {0}", request.error);
		}

		SearchResponse searchResponse = null;

		if(!string.IsNullOrEmpty(request.text))
		{
			searchResponse = JsonConvert.DeserializeObject<SearchResponse>(request.text);
			Debug.Log(request.text);
			//Debug.Log(searchResponse.ToString());
		}


		if (OnResponse != null)
			OnResponse.Invoke(searchResponse);
	}

	private void HandleSearchResponse(SearchResponse response)
	{
		if (response == null || response.statuses == null)
		{
			Debug.LogError("Invalid response: NULL");
			return;
		}

		if (response.statuses.Length == 0)
		{
			Debug.LogError("No results");
			// A good point to ask the user if they'd like to try another query
			return;
		}

		if (OnStatusesRecieved != null)
			OnStatusesRecieved.Invoke(response.statuses);
	}
}















