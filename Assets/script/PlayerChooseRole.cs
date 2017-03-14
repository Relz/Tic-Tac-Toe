using UnityEngine;
using UnityEngine.UI; 

public enum PlayerRole
{
	None,
	Cross,
	Round
}

public class PlayerChooseRole : MonoBehaviour 
{
	public Button btnChooseCrossObj;
	public Button btnChooseRoundObj;
	private static Button btnChooseCross;
	private static Button btnChooseRound;

	private static PlayerRole m_playerChoice = PlayerRole.None;
	
	void Start () 
	{
		btnChooseCross = btnChooseCrossObj.GetComponent<Button>();
		btnChooseRound = btnChooseRoundObj.GetComponent<Button>();
	}

	public static void Init()
	{
		ChooseCross();
		SetEnableChooseRoleButtons(true);
	}
	
	public void OnBtnChooseRoundClick () 
	{
		ChooseRound();
		GameController.StartGame();
	}

	public static void ChooseCross () 
	{
		m_playerChoice = PlayerRole.Cross;
		Shadow btnChooseCrossShadow = btnChooseCross.GetComponent<Shadow>();
		btnChooseCrossShadow.enabled = true;
		Shadow btnChooseRoundShadow = btnChooseRound.GetComponent<Shadow>();
		btnChooseRoundShadow.enabled = false;
	}
	
	public static void ChooseRound () 
	{
		m_playerChoice = PlayerRole.Round;
		Shadow btnChooseRoundShadow = btnChooseRound.GetComponent<Shadow>();
		btnChooseRoundShadow.enabled = true;
		Shadow btnChooseCrossShadow = btnChooseCross.GetComponent<Shadow>();
		btnChooseCrossShadow.enabled = false;
	}

	public static void SetEnableChooseRoleButtons(bool value)
	{
		btnChooseCross.interactable = value;
		btnChooseCross.enabled = value;
		btnChooseRound.interactable = value;
		btnChooseRound.enabled = value;
	}

	public static PlayerRole GetChoice()
	{
		return m_playerChoice;
	}

	public static PlayerRole GetAnotherChoice()
	{
		return (m_playerChoice == PlayerRole.Cross) ? PlayerRole.Round : PlayerRole.Cross;
	}
}
