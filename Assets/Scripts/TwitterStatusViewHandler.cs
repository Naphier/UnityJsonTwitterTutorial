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
	private int statusIndex = 0;


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

		statusIndex = 0;
		displayCoroutine = DisplayCoroutine(statuses, delay);
		StartCoroutine(displayCoroutine);
	}

	IEnumerator DisplayCoroutine(SimpleTweet[] statuses, float delay)
	{
		WaitForSeconds wait = new WaitForSeconds(delay);

		while(true)
		{
			text.text = statuses[statusIndex].text;
			statusIndex++;
			if (statusIndex >= statuses.Length)
				statusIndex = 0;

			yield return wait;
		}
	}
}
