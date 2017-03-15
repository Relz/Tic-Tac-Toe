using UnityEngine;

public class Player
{
	private PlayerRole m_role;
	private bool m_isAI;
	private uint m_score = 0;

	public Player() 
	{
		m_role = PlayerRole.None;
		m_isAI = false;
	}

	public void SetRole(PlayerRole value)
	{
		m_role = value;
	}

	public PlayerRole GetRole()
	{
		return m_role;
	}

	public void SetAI(bool value)
	{
		m_isAI = value;
	}

	public bool IsAI()
	{
		return m_isAI;
	}

	public void DoTurn()
	{
		Debug.Log("I can't do that yet");
	}

	public void IncreaseScore()
	{
		++m_score;
		PlayerChooseRole.SetScore(GetRole(), GetScore());
	}

	public uint GetScore()
	{
		return m_score;
	}

	public void ClearScore()
	{
		m_score = 0;
	}
}
