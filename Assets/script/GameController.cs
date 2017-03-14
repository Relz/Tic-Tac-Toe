using UnityEngine;

public class GameController : MonoBehaviour 
{
	private static bool m_isGameStarted = false;
	public static Player m_player0 = new Player();
	public static Player m_player1 = new Player();

	void Start()
	{
		Init();
	}

	public void Init()
	{
		m_isGameStarted = false;
		StatusText.SetText(Constant.STATUS_TEXT.WAIT_FOR_CHOICE);
		PlayerChooseRole.Init();
		Field.Init();
	}

	public static void StartGame()
	{
		m_isGameStarted = true;
		StatusText.SetText(Constant.STATUS_TEXT.ROUND_TURN);
		m_player0.SetRole(PlayerChooseRole.GetChoice());
		m_player1.SetRole(PlayerChooseRole.GetAnotherChoice());
		PlayerChooseRole.SetEnableChooseRoleButtons(false);
	}

	public static bool IsGameStarted()
	{
		return m_isGameStarted;
	}
}
