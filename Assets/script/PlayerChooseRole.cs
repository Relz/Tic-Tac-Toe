using UnityEngine;
using UnityEngine.UI; 

public enum PlayerRole
{
	None,
	Cross,
	Circle
}

public class PlayerChooseRole : MonoBehaviour 
{
	public Button btnChooseCrossObj;
	public Button btnChooseCircleObj;
	private static Button btnChooseCross;
	private static Button btnChooseCircle;

	private static PlayerRole m_playerChoice = PlayerRole.None;
	
	void Start () 
	{
		btnChooseCross = btnChooseCrossObj.GetComponent<Button>();
		btnChooseCircle = btnChooseCircleObj.GetComponent<Button>();
	}

	public static void Init()
	{
		SetEnableChooseRoleButtons(true);
		ChooseCross();
	}
	
	public void OnBtnChooseCircleClick () 
	{
		SetEnableChooseRoleButtons(false);
		ChooseCircle();
	}

	public static void ChooseCross () 
	{
		m_playerChoice = PlayerRole.Cross;
		Shadow btnChooseCrossShadow = btnChooseCross.GetComponent<Shadow>();
		btnChooseCrossShadow.enabled = true;
		Shadow btnChooseCircleShadow = btnChooseCircle.GetComponent<Shadow>();
		btnChooseCircleShadow.enabled = false;
	}
	
	public static void ChooseCircle () 
	{
		m_playerChoice = PlayerRole.Circle;
		Shadow btnChooseCircleShadow = btnChooseCircle.GetComponent<Shadow>();
		btnChooseCircleShadow.enabled = true;
		Shadow btnChooseCrossShadow = btnChooseCross.GetComponent<Shadow>();
		btnChooseCrossShadow.enabled = false;
	}

	private static void SetEnableChooseRoleButtons(bool value)
	{
		btnChooseCross.interactable = value;
		btnChooseCross.enabled = value;
		btnChooseCircle.interactable = value;
		btnChooseCircle.enabled = value;
	}

	public static PlayerRole GetChoice()
	{
		return m_playerChoice;
	}
}
