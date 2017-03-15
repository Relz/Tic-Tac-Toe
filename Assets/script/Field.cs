using UnityEngine;
using UnityEngine.UI;
using System;

enum FieldItem
{
	Empty,
	Cross,
	Round
}

public class Field : MonoBehaviour 
{
	public GameObject cell0, cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8;
	public GameObject line;

	private static Image[,] cellsImage = new Image[3, 3];
	private static FieldItem[,] cells = new FieldItem[3, 3];

	private static Sprite crossSprite;
	private static Sprite roundSprite;

	private static RectTransform lineRectTransform;
	private static Image lineImage;

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
		lineRectTransform = line.GetComponent<RectTransform>();
		lineImage = line.GetComponent<Image>();
	}

	public static void Init()
	{
		Clear();
		if (lineImage)
		{
			DrawLine(FieldItem.Empty, 0, 507, 0, -507, 0);
		}
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
		string[] cellPosArr = cellPosStr.Split(':');
		Vector3 cellPos = new Vector3();
		cellPos.x = Convert.ToUInt32(cellPosArr[0], 10);
		cellPos.y = Convert.ToUInt32(cellPosArr[1], 10);
		cellsImage[(uint)cellPos.y, (uint)cellPos.x].color = new Color(255, 255, 255, 255);
		if (GameController.GetCurrentPlayer().GetRole() == PlayerRole.Cross)
		{
			cellsImage[(uint)cellPos.y, (uint)cellPos.x].sprite = crossSprite;
			cells[(uint)cellPos.y, (uint)cellPos.x] = FieldItem.Cross;
		}
		else
		{
			cellsImage[(uint)cellPos.y, (uint)cellPos.x].sprite = roundSprite;
			cells[(uint)cellPos.y, (uint)cellPos.x] = FieldItem.Round;
		}
		if (TryToWin((uint)cellPos.x, (uint)cellPos.y))
		{
			GameController.GetCurrentPlayer().IncreaseScore();
		}
		GameController.NextTurn();
	}

	private bool TryToWin(uint x, uint y)
	{
		Vector3 startCellPos = new Vector3(x, y);
		Vector3 endCellPos = new Vector3(x, y);
		if (
			TryToWinCheckVertical(x, y, ref startCellPos, ref endCellPos) ||
			TryToWinCheckHorizontal(x, y, ref startCellPos, ref endCellPos) ||
			TryToWinCheckLeftDiagonal(x, y, ref startCellPos, ref endCellPos) ||
			TryToWinCheckRightDiagonal(x, y, ref startCellPos, ref endCellPos)
		)
		{
			Image cellImage = cellsImage[(uint)startCellPos.y, (uint)startCellPos.x];
			FieldItem fieldItem = cells[(uint)startCellPos.y, (uint)startCellPos.x];
			if (startCellPos.x == endCellPos.x)
			{
				// Вертикаль
				if (startCellPos.x == 0)
				{
					// Левая
					DrawLine(fieldItem, 0, 114.25f, 0, 20.65f, 0);
				}
				else if (startCellPos.x == 1)
				{
					// Средняя
					DrawLine(fieldItem, 0, 67.0f, 0, 67.7f, 0);
				}
				else if (startCellPos.x == 2)
				{
					// Правая
					DrawLine(fieldItem, 0, 20.65f, 0, 114.25f, 0);
				}
			}
			else if (startCellPos.y == endCellPos.y)
			{
				// Горизонталь
				if (startCellPos.y == 0)
				{
					// Верхняя
					DrawLine(fieldItem, -47.1f, 67.0f, 47.1f, 67.7f, 90.0f);
				}
				else if (startCellPos.y == 1)
				{
					// Средняя
					DrawLine(fieldItem, 0, 67.0f, 0, 67.7f, 90.0f);
				}
				else if (startCellPos.y == 2)
				{
					// Нижняя
					DrawLine(fieldItem, 47.0f, 67.0f, -47.0f, 67.7f, 90.0f);
				}
			}
			else
			{
				// Диагональ
				if (startCellPos.x < endCellPos.x && startCellPos.x == 0 && startCellPos.y == 0)
				{
					// Левая
					DrawLine(fieldItem, -24.65f, 66.0f, -24.65f, 68.0f, 45.0f);
				}
				else
				{
					// Правая
					DrawLine(fieldItem, -24.65f, 66.0f, -24.65f, 68.0f, 135.0f);
				}
			}
			return true;
		}
		return false;
	}

	private bool TryToWinCheckVertical(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
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

	private bool TryToWinCheckHorizontal(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
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

	private bool TryToWinCheckLeftDiagonal(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
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

	private bool TryToWinCheckRightDiagonal(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
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

	private static void DrawLine(FieldItem fieldItem, float top, float right, float bottom, float left, float zRotation)
	{
		if (fieldItem == FieldItem.Cross)
		{
			lineImage.color = new Color(0.33f, 0.33f, 0.33f, 1);
		}
		else if (fieldItem == FieldItem.Round)
		{
			lineImage.color = new Color(0.95f, 0.92f, 0.827f, 1);
		}
		else
		{
			lineImage.color = new Color(0, 0, 0, 0);
		}
		lineRectTransform.offsetMin = new Vector2(left, bottom);
		lineRectTransform.offsetMax = new Vector2(-right, -top);
		lineRectTransform.rotation = Quaternion.Euler(0, 0, zRotation);
	}
}
