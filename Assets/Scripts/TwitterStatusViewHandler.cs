using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NG.TwitterApi;

public class TwitterStatusViewHandler : MonoBehaviour
{
	[SerializeField]
	private Text text;

	[SerializeField]
	private float delay = 2;

	private IEnumerator displayCoroutine;

	void Awake()
	{
		if (text == null)
			Debug.LogError("Text is null!");
	}


	public void DisplayStatuses(SimpleTweet[] statuses)
	{
		if (statuses == null || statuses.Length < 1)
		{
			Debug.LogError("No statuses to display");
			return;
		}

		if (displayCoroutine != null)
			StopCoroutine(displayCoroutine);

		displayCoroutine = DisplayCoroutine(statuses, delay);
		StartCoroutine(displayCoroutine);
	}

	IEnumerator DisplayCoroutine(SimpleTweet[] statuses, float delay)
	{
		WaitForSeconds wait = new WaitForSeconds(delay);

		int statusIndex = 0;

		while (true)
		{
			text.text = statuses[statusIndex].text;
			statusIndex++;
			if (statusIndex >= statuses.Length)
				statusIndex = 0;

			yield return wait;
		}
	}
}
