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
		if (!GameController.IsGameStarted() || GameController.GetCurrentPlayer().GetRole() != GetRole())
		{
			return;
		}
		FieldItem[,] cells = Field.GetFieldItems();
		Vector2 optimalCellPosToTurn = GetOptimalCellToTurn(cells);
		Field.CellClickImpl((int)optimalCellPosToTurn.x, (int)optimalCellPosToTurn.y);
	}

// Код метода нуждается в наведении красоты
	public Vector2 GetOptimalCellToTurn(FieldItem[,] cells)
	{
		Vector2 result = new Vector2(-1, -1);
		result = GetWinTurn(cells);
		if (result.x == -1 || result.y == -1)
		{
			result = GameController.GetAnotherPlayer().GetWinTurn(cells);
		}
		if (result.x == -1 || result.y == -1)
		{
			do
			{
				result.x = Random.Range(0, (int)Constant.FIELD_SIZE);
				result.y = Random.Range(0, (int)Constant.FIELD_SIZE);
			}
			while (!Field.IsCellFree((int)result.x, (int)result.y));
		}
		return result;
	}

	private Vector2 GetWinTurn(FieldItem[,] cells)
	{
		Vector2 result = new Vector2(-1, -1);
		bool winTurnFound = false;
		for (uint i = 0; i < Constant.FIELD_SIZE - 1 && !winTurnFound; ++i)
		{
			for (uint j = 0; j < Constant.FIELD_SIZE - 1 && !winTurnFound; ++j)
			{
				if (
						j < Constant.FIELD_SIZE - 1 &&
						(
							(
								GetRole() == PlayerRole.Cross &&
								cells[i, j] == FieldItem.Cross && 
								cells[i, j + 1] == FieldItem.Cross
							) || 
							(
								GetRole() == PlayerRole.Round &&
								cells[i, j] == FieldItem.Round && 
								cells[i, j + 1] == FieldItem.Round
							)
						) &&
						(
							(
								j == 0 && 
								cells[i, j + 2] == FieldItem.Empty
							) ||
							(
								j != 0 && 
								cells[i, j - 1] == FieldItem.Empty
							)
						)
					)
				{
					winTurnFound = true;
					if (j == 0)
					{
						result = new Vector2(j + 2, i);
					}
					else
					{
						result = new Vector2(j - 1, i);
					}
				}
				else if (
						i < Constant.FIELD_SIZE - 1 &&
						(
							(
								GetRole() == PlayerRole.Cross && 
								cells[i, j] == FieldItem.Cross && 
								cells[i + 1, j] == FieldItem.Cross
							) || 
							(
								GetRole() == PlayerRole.Round && 
								cells[i, j] == FieldItem.Round && 
								cells[i + 1, j] == FieldItem.Round
							)
						) &&
						(
							(
								i == 0 && 
								cells[i + 2, j] == FieldItem.Empty
							) ||
							(
								i != 0 && 
								cells[i - 1, j] == FieldItem.Empty
							)
						)
					)
				{
					winTurnFound = true;
					if (i == 0)
					{
						result = new Vector2(j, i + 2);
					}
					else
					{
						result = new Vector2(j, i - 1);
					}
				}
				else if (
						i < Constant.FIELD_SIZE - 1 &&
						j < Constant.FIELD_SIZE - 1 &&
						(
							(
								GetRole() == PlayerRole.Cross && 
								cells[i, j] == FieldItem.Cross && 
								cells[i + 1, j + 1] == FieldItem.Cross
							) || 
							(
								GetRole() == PlayerRole.Round && 
								cells[i, j] == FieldItem.Round && 
								cells[i + 1, j + 1] == FieldItem.Round
							)
						) &&
						(
							(
								i == 0 && 
								j == 0 && 
								cells[i + 2, j + 2] == FieldItem.Empty
							) ||
							(
								i != 0 && 
								j != 0 && 
								cells[i - 1, j - 1] == FieldItem.Empty
							)
						)
					)
				{
					winTurnFound = true;
					if (i == 0 && j == 0)
					{
						result = new Vector2(j + 2, i + 2);
					}
					else
					{
						result = new Vector2(j - 1, i - 1);
					}
				}
				else if (
						i < Constant.FIELD_SIZE - 1 &&
						j > 0 &&
						(
							(
								GetRole() == PlayerRole.Cross && 
								cells[i, j] == FieldItem.Cross && 
								cells[i + 1, j - 1] == FieldItem.Cross
							) || 
							(
								GetRole() == PlayerRole.Round && 
								cells[i, j] == FieldItem.Round && 
								cells[i + 1, j - 1] == FieldItem.Round
							)
						) &&
						(
							(
								i == Constant.FIELD_SIZE - 1 && 
								j == 0 && 
								cells[i - 2, j + 2] == FieldItem.Empty
							) ||
							(
								j != Constant.FIELD_SIZE - 1 && 
								j != 0 && 
								cells[i + 1, j - 1] == FieldItem.Empty
							)
						)
					)
				{
					winTurnFound = true;
					if (j == Constant.FIELD_SIZE - 1 && i == 0)
					{
						result = new Vector2(j - 2, i + 2);
					}
					else
					{
						result = new Vector2(j + 1, i - 1);
					}
				}
			}
		}
		return result;
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
