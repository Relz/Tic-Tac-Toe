using UnityEngine;

public class GameController : MonoBehaviour 
{
	public PlayerRole firstPlayerRole = PlayerRole.None;
	private static bool isGameStarted = false;

	void Start()
	{
		Init();
	}

	public void Init()
	{
		isGameStarted = false;
		firstPlayerRole = PlayerRole.None;
		StatusText.SetText(Constant.STATUS_TEXT.waitForChoice);
		PlayerChooseRole.Init();
	}

	public void StartGame()
	{
		isGameStarted = true;
		StatusText.SetText(Constant.STATUS_TEXT.CircleTurn);
	}
}
