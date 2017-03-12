using UnityEngine;
using UnityEngine.UI; 

public class PlayerChooseRole : MonoBehaviour 
{
	public Button btnChooseCross;
	public Button btnChooseCircle;
	// Use this for initialization
	void Start () 
	{
		ChooseCross();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void OnBtnChooseCrossClick () 
	{
		DisableChooseRoleButtons();
		ChooseCross();
	}
	
	public void OnBtnChooseCircleClick () 
	{
		DisableChooseRoleButtons();
		ChooseCircle();
	}

	public void ChooseCross () 
	{
		Manager.firstPlayerRole = PlayerRole.Cross;
		Debug.Log("Cross");
	}
	
	public void ChooseCircle () 
	{
		Manager.firstPlayerRole = PlayerRole.Circle;
		Debug.Log("Circle");
	}

	private void DisableChooseRoleButtons()
	{
		btnChooseCross.interactable = false;
		btnChooseCross.enabled = false;
		btnChooseCircle.interactable = false;
		btnChooseCircle.enabled = false;
	}
}
