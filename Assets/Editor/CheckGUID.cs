using UnityEngine;
using UnityEditor;

public class CheckGUID : EditorWindow
{
	[MenuItem("Window/Check missing asset by GUID")]
	static void Run()
	{
		CheckGUID window = GetWindow<CheckGUID>();
		window.minSize = new Vector2(400, 550);
		window.Show();
	}

	static string guid;
	private void OnGUI()
	{
		EditorGUILayout.LabelField("Enter GUIDs on separate lines");

		guid = EditorGUILayout.TextArea(guid, GUILayout.Height(500));

		if (GUILayout.Button("Search"))
		{
			SearchLibraryForGUID(guid);
		}
	}

	private void SearchLibraryForGUID(string guid)
	{
		if (string.IsNullOrEmpty(guid))
			return;

		string[] guids = guid.Split('\n');

		var sb = new System.Text.StringBuilder();

		for (int i = 0; i < guids.Length; i++)
		{
			guids[i] = guids[i].Trim();
			sb.AppendFormat(
				"[{0}] GUID: '{1}'\nAssetPath: '{2}'\n\n", i, guids[i],
				AssetDatabase.GUIDToAssetPath(guids[i]));
		}

		Debug.Log(sb.ToString());
	}
}