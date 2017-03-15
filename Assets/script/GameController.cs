using UnityEngine;

public class GameController : MonoBehaviour 
{
	private static bool m_isGameStarted = false;
	private static Player m_player0 = new Player();
	private static Player m_player1 = new Player();
	private static Player m_currentPlayer;

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
		m_player0.ClearScore();
		m_player1.ClearScore();
	}

	public static void StartGame()
	{
		m_isGameStarted = true;
		StatusText.SetText(Constant.STATUS_TEXT.CROSS_TURN);
		m_player0.SetRole(PlayerChooseRole.GetChoice());
		m_player1.SetRole(PlayerChooseRole.GetAnotherChoice());
		m_currentPlayer = (m_player0.GetRole() == PlayerRole.Cross) ? m_player0 : m_player1;
		PlayerChooseRole.SetEnableChooseRoleButtons(false);
	}

	public static bool IsGameStarted()
	{
		return m_isGameStarted;
	}

	public static Player GetCurrentPlayer()
	{
		return m_currentPlayer;
	}

	public static void NextTurn()
	{
		m_currentPlayer = (m_currentPlayer == m_player0) ? m_player1 : m_player0;
		if (m_currentPlayer.GetRole() == PlayerRole.Cross)
		{
			StatusText.SetText(Constant.STATUS_TEXT.CROSS_TURN);
		}
		else
		{
			StatusText.SetText(Constant.STATUS_TEXT.ROUND_TURN);
		}
	}
}
