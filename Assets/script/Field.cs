using UnityEngine;
using UnityEngine.UI;
using System;

struct Position
{
	public uint x;
	public uint y;

	public Position(uint x, uint y)
	{
		this.x = x;
		this.y = y;
	}
	public Position(string[] posStr)
	{
		this.x = Convert.ToUInt32(posStr[0], 10);
		this.y = Convert.ToUInt32(posStr[1], 10);
	}
}

enum FieldItem
{
	Empty,
	Cross,
	Round
}

public class Field : MonoBehaviour 
{
	public GameObject cell0, cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8;

	private static Image[,] cellsImage = new Image[3, 3];
	private static FieldItem[,] cells = new FieldItem[3, 3];

	private static Sprite crossSprite;
	private static Sprite roundSprite;

	void Start () 
	{
		cellsImage[0, 0] = cell0.GetComponentInChildren<Image>();
		cellsImage[0, 1] = cell1.GetComponentInChildren<Image>();
		cellsImage[0, 2] = cell2.GetComponentInChildren<Image>();
		cellsImage[1, 0] = cell3.GetComponentInChildren<Image>();
		cellsImage[1, 1] = cell4.GetComponentInChildren<Image>();
		cellsImage[1, 2] = cell5.GetComponentInChildren<Image>();
		cellsImage[2, 0] = cell6.GetComponentInChildren<Image>();
		cellsImage[2, 1] = cell7.GetComponentInChildren<Image>();
		cellsImage[2, 2] = cell8.GetComponentInChildren<Image>();
		crossSprite = Resources.Load<Sprite>("cross");
		roundSprite = Resources.Load<Sprite>("round");
	}

	public static void Init()
	{
		Clear();
	}

	private static void Clear()
	{
		for (uint i = 0; i < 3; ++i)
		{
			for (uint j = 0; j < 3; ++j)
			{
				if (cellsImage[i, j])
				{
					cellsImage[i, j].color = new Color(255, 255, 255, 0);
					cells[i, j] = FieldItem.Empty;
				}
			}
		}
	}
	
	public void OnCellClick(string cellPosStr)
	{
		if (!GameController.IsGameStarted())
		{
			GameController.StartGame();
		}
		Position cellPos = new Position(cellPosStr.Split(':'));
		cellsImage[cellPos.y, cellPos.x].color = new Color(255, 255, 255, 255);
		if (GameController.GetCurrentPlayer().GetRole() == PlayerRole.Cross)
		{
			cellsImage[cellPos.y, cellPos.x].sprite = crossSprite;
			cells[cellPos.y, cellPos.x] = FieldItem.Cross;
		}
		else
		{
			cellsImage[cellPos.y, cellPos.x].sprite = roundSprite;
			cells[cellPos.y, cellPos.x] = FieldItem.Round;
		}
		TryToWin(cellPos.x, cellPos.y);
		GameController.NextTurn();
	}

	private void TryToWin(uint x, uint y)
	{
		Position startPos = new Position(x, y);
		Position endPos = new Position(x, y);
		if (
			TryToWinCheckVertical(x, y, ref startPos, ref endPos) ||
			TryToWinCheckHorizontal(x, y, ref startPos, ref endPos) ||
			TryToWinCheckLeftDiagonal(x, y, ref startPos, ref endPos) ||
			TryToWinCheckRightDiagonal(x, y, ref startPos, ref endPos)
		)
		{
			
		}
	}

	private bool TryToWinCheckVertical(uint x, uint y, ref Position startPos, ref Position endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = cells[y, x];
		if (y > 0 && cells[y - 1, x] == fieldItem)
		{
			++lineLength;
			startPos.y = y - 1;
			if (y > 1 && cells[y - 2, x] == fieldItem)
			{
				++lineLength;
				startPos.y = y - 2;
			}
		}
		if (y < 2 && cells[y + 1, x] == fieldItem)
		{
			++lineLength;
			endPos.y = y + 1;
			if (y < 1 && cells[y + 2, x] == fieldItem)
			{
				++lineLength;
				endPos.y = y + 2;
			}
		}
		return lineLength >= Constant.LINE_LENGTH_TO_WIN;
	}

	private bool TryToWinCheckHorizontal(uint x, uint y, ref Position startPos, ref Position endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = cells[y, x];
		if (x > 0 && cells[y, x - 1] == fieldItem)
		{
			++lineLength;
			startPos.x = x - 1;
			if (x > 1 && cells[y, x - 2] == fieldItem)
			{
				++lineLength;
				startPos.x = x - 2;
			}
		}
		if (x < 2 && cells[y, x + 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x + 1;
			if (x < 1 && cells[y, x + 2] == fieldItem)
			{
				++lineLength;
				endPos.x = x + 2;
			}
		}
		return lineLength >= Constant.LINE_LENGTH_TO_WIN;
	}

	private bool TryToWinCheckLeftDiagonal(uint x, uint y, ref Position startPos, ref Position endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = cells[y, x];
		if (x > 0 && y > 0 && cells[y - 1, x - 1] == fieldItem)
		{
			++lineLength;
			startPos.x = x - 1;
			startPos.y = y - 1;
			if (x > 1 && y > 1 && cells[y - 2, x - 2] == fieldItem)
			{
				++lineLength;
				startPos.x = x - 2;
				startPos.y = y - 2;
			}
		}
		if (x < 2 && y < 2 && cells[y + 1, x + 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x + 1;
			endPos.y = y + 1;
			if (x < 1 && y < 1 && cells[y + 2, x + 2] == fieldItem)
			{
				++lineLength;
				endPos.x = x + 2;
				endPos.y = y + 2;
			}
		}
		return lineLength >= Constant.LINE_LENGTH_TO_WIN;
	}

	private bool TryToWinCheckRightDiagonal(uint x, uint y, ref Position startPos, ref Position endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = cells[y, x];
		if (x < 2 && y > 0 && cells[y - 1, x + 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x + 1;
			endPos.y = y - 1;
			if (x < 1 && y > 1 && cells[y - 2, x + 2] == fieldItem)
			{
				++lineLength;
				endPos.x = x + 2;
				endPos.y = y - 2;
			}
		}
		if (x > 0 && y < 2 && cells[y + 1, x - 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x - 1;
			endPos.y = y + 1;
			if (x > 1 && y < 1 && cells[y + 2, x - 2] == fieldItem)
			{
				++lineLength;
				endPos.x = x - 2;
				endPos.y = y + 2;
			}
		}
		return lineLength >= Constant.LINE_LENGTH_TO_WIN;
	}
}
