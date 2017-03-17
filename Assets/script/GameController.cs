using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum GameMode
{
	PlayerVsAI,
	PlayerVsPlayer,
	AIVsAI
}

public class GameController : MonoBehaviour 
{
	static public GameController instance;
    private static IEnumerator m_coroutine;
	public GameObject buttonNextRound;
	public GameObject dropdownGameMode;
	public static GameObject dropdownGameModeStatic;
	public GameObject roundResult;
	private static GameObject roundResultStatic;
	private static Text roundResultStaticText;
	private static GameObject buttonNextRoundStatic;
	public GameObject buttonUndoOnce;
	public static GameObject buttonUndoOnceStatic;
	private static bool m_isGameStarted = false;
	private static bool m_isGameOver = false;
	private static Player m_player0 = new Player();
	private static Player m_player1 = new Player();
	private static Player m_currentPlayer;
	private static Player m_anotherPlayer;
	private static GameMode m_gameMode = 0;

	void Start()
	{
		m_coroutine = CurrentPlayerDoTurnAfterSeconds(Constant.AI_THINKING_SECONDS);
		buttonNextRoundStatic = buttonNextRound.gameObject;
		roundResultStatic = roundResult.gameObject;
		roundResultStaticText = roundResult.GetComponent<Text>();
		dropdownGameModeStatic = dropdownGameMode.gameObject;
		buttonUndoOnceStatic = buttonUndoOnce.gameObject;
		Init();
	}

	 void Awake()
	{
		instance = this;
	}

	public void Init()
	{
		m_isGameStarted = false;
		StatusText.SetText(Constant.STATUS_TEXT.WAIT_FOR_CHOICE);
		PlayerChooseRole.Init();
		Field.Init();
		m_player0.ClearScore();
		m_player1.ClearScore();
		buttonNextRound.SetActive(false);
		roundResult.SetActive(false);
		buttonUndoOnce.SetActive(true);
		dropdownGameMode.GetComponent<Dropdown>().interactable = true;
	}

	public static void StartGame()
	{
		m_isGameStarted = true;
		dropdownGameModeStatic.GetComponent<Dropdown>().interactable = false;
		StatusText.SetText(Constant.STATUS_TEXT.CROSS_TURN);
		m_player0.SetRole(PlayerChooseRole.GetChoice());
		m_player1.SetRole(PlayerChooseRole.GetAnotherChoice());
		if (m_gameMode == GameMode.PlayerVsAI)
		{
			m_player0.SetAI(false);
			m_player1.SetAI(true);
		}
		else if (m_gameMode == GameMode.PlayerVsPlayer)
		{
			m_player0.SetAI(false);
			m_player1.SetAI(false);
		}
		else if (m_gameMode == GameMode.AIVsAI)
		{
			m_player0.SetAI(true);
			m_player1.SetAI(true);
		}
		if (m_player0.GetRole() == PlayerRole.Cross)
		{
			m_currentPlayer = m_player0;
			m_anotherPlayer = m_player1;
		}
		else
		{
			m_currentPlayer = m_player1;
			m_anotherPlayer = m_player0;
		}
		PlayerChooseRole.SetEnableChooseRoleButtons(false);
		if (m_currentPlayer.IsAI())
		{
			instance.StartCoroutine(CurrentPlayerDoTurnAfterSeconds(Constant.AI_THINKING_SECONDS));
		}
	}
	
	private static IEnumerator CurrentPlayerDoTurnAfterSeconds(uint seconds) 
	{
		yield return new WaitForSeconds(seconds);
		if (m_currentPlayer.IsAI())
		{
			m_currentPlayer.DoTurn();
		}
	}

	public static bool IsGameStarted()
	{
		return m_isGameStarted;
	}

	public static Player GetCurrentPlayer()
	{
		return m_currentPlayer;
	}

	public static Player GetAnotherPlayer()
	{
		return m_anotherPlayer;
	}

	public static void NextTurn()
	{
		if (!IsGameStarted())
		{
			return;
		}
		if (m_currentPlayer == m_player0)
		{
			m_currentPlayer = m_player1;
			m_anotherPlayer = m_player0;
		}
		else
		{
			m_currentPlayer = m_player0;
			m_anotherPlayer = m_player1;
		}
		if (m_currentPlayer.GetRole() == PlayerRole.Cross)
		{
			StatusText.SetText(Constant.STATUS_TEXT.CROSS_TURN);
		}
		else
		{
			StatusText.SetText(Constant.STATUS_TEXT.ROUND_TURN);
		}
		if (m_currentPlayer.IsAI())
		{
			instance.StartCoroutine(CurrentPlayerDoTurnAfterSeconds(Constant.AI_THINKING_SECONDS));
		}
	}

	public static void Victory()
	{
		m_isGameOver = true;
		GameController.GetCurrentPlayer().IncreaseScore();
		buttonNextRoundStatic.SetActive(true);
		roundResultStatic.SetActive(true);
		buttonUndoOnceStatic.SetActive(false);
		if (GameController.GetCurrentPlayer().GetRole() == PlayerRole.Cross)
		{
			roundResultStaticText.text = Constant.ROUND_RESULT.VICTORY.CROSS;
		}
		else
		{
			roundResultStaticText.text = Constant.ROUND_RESULT.VICTORY.ROUND;
		}
	}

	public static void Draft()
	{
		m_isGameOver = true;
		buttonNextRoundStatic.SetActive(true);
		roundResultStatic.SetActive(true);
		buttonUndoOnceStatic.SetActive(false);
		roundResultStaticText.text = Constant.ROUND_RESULT.DRAFT;
	}

	public void OnNextRoundButtonClick()
	{
		Field.Init();
		m_isGameOver = false;
		buttonNextRound.SetActive(false);
		roundResult.SetActive(false);
		buttonUndoOnceStatic.SetActive(true);
		if (m_player0.GetRole() == PlayerRole.Cross)
		{
			m_currentPlayer = m_player0;
			m_anotherPlayer = m_player1;
		}
		else
		{
			m_currentPlayer = m_player0;
			m_anotherPlayer = m_player1;
		}
		StatusText.SetText(Constant.STATUS_TEXT.CROSS_TURN);
		if (m_currentPlayer.IsAI())
		{
			instance.StartCoroutine(m_coroutine);
		}
	}

	public static bool IsGameOver()
	{
		return m_isGameOver;
	}

	public void OnSelectGameMode(int gameMode)
	{
		m_gameMode = (GameMode)gameMode;
		if (m_gameMode == GameMode.AIVsAI)
		{
			StartGame();
		}
	}

	public void Tip()
	{
		if (!IsGameStarted() || IsGameOver() || m_currentPlayer.IsAI())
		{
			return;
		}
		Vector2 optimalCellToTurn = m_currentPlayer.GetOptimalCellToTurn(Field.GetFieldItems());
		Field.MarkCell((uint)optimalCellToTurn.x, (uint)optimalCellToTurn.y);
	}
}
