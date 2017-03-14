using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour 
{
	public Text statusTextObj;
	private static Text statusText;
	
	void Start () 
	{
		statusText = statusTextObj.GetComponent<Text>();
	}

	public static void SetText(string text)
	{
		statusText.text = text;
	}
}
