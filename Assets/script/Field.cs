using UnityEngine;
using UnityEngine.UI;
using System;

public enum FieldItem
{
	Empty,
	Cross,
	Round
}

public class Field : MonoBehaviour 
{
	public GameObject cell0, cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8;
	public GameObject line;

	private static Image[,] m_cellsImage = new Image[3, 3];
	private static Image[,] m_cellsBackground = new Image[3, 3];
	private static FieldItem[,] m_cells = new FieldItem[3, 3];

	private static Sprite m_crossSprite;
	private static Sprite m_roundSprite;

	private static RectTransform m_lineRectTransform;
	private static Image m_lineImage;
	private static Vector2 m_lastTurn;

	void Start () 
	{
		m_cellsImage[0, 0] = cell0.GetComponentInChildren<Image>();
		m_cellsImage[0, 1] = cell1.GetComponentInChildren<Image>();
		m_cellsImage[0, 2] = cell2.GetComponentInChildren<Image>();
		m_cellsImage[1, 0] = cell3.GetComponentInChildren<Image>();
		m_cellsImage[1, 1] = cell4.GetComponentInChildren<Image>();
		m_cellsImage[1, 2] = cell5.GetComponentInChildren<Image>();
		m_cellsImage[2, 0] = cell6.GetComponentInChildren<Image>();
		m_cellsImage[2, 1] = cell7.GetComponentInChildren<Image>();
		m_cellsImage[2, 2] = cell8.GetComponentInChildren<Image>();
		m_cellsBackground[0, 0] = cell0.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[0, 1] = cell1.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[0, 2] = cell2.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[1, 0] = cell3.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[1, 1] = cell4.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[1, 2] = cell5.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[2, 0] = cell6.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[2, 1] = cell7.transform.GetChild(1).GetComponent<Image>();
		m_cellsBackground[2, 2] = cell8.transform.GetChild(1).GetComponent<Image>();
		m_crossSprite = Resources.Load<Sprite>("cross");
		m_roundSprite = Resources.Load<Sprite>("round");
		m_lineRectTransform = line.GetComponent<RectTransform>();
		m_lineImage = line.GetComponent<Image>();
	}

	public static void Init()
	{
		m_lastTurn = new Vector2(-1, -1);
		Clear();
		if (m_lineImage)
		{
			DrawLine(FieldItem.Empty, 0, 507, 0, -507, 0);
		}
	}

	private static void Clear()
	{
		for (uint i = 0; i < Constant.FIELD_SIZE; ++i)
		{
			for (uint j = 0; j < Constant.FIELD_SIZE; ++j)
			{
				if (m_cellsImage[i, j])
				{
					m_cellsImage[i, j].color = new Color(1, 1, 1, 0);
					m_cells[i, j] = FieldItem.Empty;
				}
			}
		}
	}
	
	public static void CellClickImpl(int x, int y)
	{
		if (!IsCellFree(x, y))
		{
			return;
		}
		Vector3 cellPos = new Vector2();
		cellPos.x = x;
		cellPos.y = y;
		m_cellsImage[(uint)cellPos.y, (uint)cellPos.x].color = new Color(1, 1, 1, 1);
		ClearMarks();
		if (GameController.GetCurrentPlayer().GetRole() == PlayerRole.Cross)
		{
			m_cellsImage[(uint)cellPos.y, (uint)cellPos.x].sprite = m_crossSprite;
			m_cells[(uint)cellPos.y, (uint)cellPos.x] = FieldItem.Cross;
		}
		else
		{
			m_cellsImage[(uint)cellPos.y, (uint)cellPos.x].sprite = m_roundSprite;
			m_cells[(uint)cellPos.y, (uint)cellPos.x] = FieldItem.Round;
		}
		m_lastTurn.x = cellPos.x;
		m_lastTurn.y = cellPos.y;
		if (TryToWin((uint)cellPos.x, (uint)cellPos.y))
		{
			GameController.Victory();
		}
		else if (IsFieldFull())
		{
			GameController.Draft();
		}
		else
		{
			GameController.NextTurn();
		}
	}

	private static void ClearMarks()
	{
		for (uint i = 0; i < Constant.FIELD_SIZE; ++i)
		{
			for (uint j = 0; j < Constant.FIELD_SIZE; ++j)
			{
				m_cellsBackground[i, j].color = new Color(1, 1, 1, 0);
			}
		}
	}

	public void OnCellClick(string cellPosStr)
	{
		if (GameController.IsGameOver())
		{
			return;
		}
		if (!GameController.IsGameStarted())
		{
			GameController.StartGame();
		}
		if (GameController.GetCurrentPlayer().IsAI())
		{
			return;
		}
		string[] cellPosArr = cellPosStr.Split(':');
		int x = Convert.ToInt32(cellPosArr[0], 10);
		int y = Convert.ToInt32(cellPosArr[1], 10);
		CellClickImpl(x, y);
	}

	private static bool TryToWin(uint x, uint y)
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
			FieldItem fieldItem = m_cells[(uint)startCellPos.y, (uint)startCellPos.x];
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

	private static bool TryToWinCheckVertical(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = m_cells[y, x];
		if (y > 0 && m_cells[y - 1, x] == fieldItem)
		{
			++lineLength;
			startPos.y = y - 1;
			if (y > 1 && m_cells[y - 2, x] == fieldItem)
			{
				++lineLength;
				startPos.y = y - 2;
			}
		}
		if (y < 2 && m_cells[y + 1, x] == fieldItem)
		{
			++lineLength;
			endPos.y = y + 1;
			if (y < 1 && m_cells[y + 2, x] == fieldItem)
			{
				++lineLength;
				endPos.y = y + 2;
			}
		}
		return lineLength >= Constant.LINE_LENGTH_TO_WIN;
	}

	private static bool TryToWinCheckHorizontal(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = m_cells[y, x];
		if (x > 0 && m_cells[y, x - 1] == fieldItem)
		{
			++lineLength;
			startPos.x = x - 1;
			if (x > 1 && m_cells[y, x - 2] == fieldItem)
			{
				++lineLength;
				startPos.x = x - 2;
			}
		}
		if (x < 2 && m_cells[y, x + 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x + 1;
			if (x < 1 && m_cells[y, x + 2] == fieldItem)
			{
				++lineLength;
				endPos.x = x + 2;
			}
		}
		return lineLength >= Constant.LINE_LENGTH_TO_WIN;
	}

	private static bool TryToWinCheckLeftDiagonal(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = m_cells[y, x];
		if (x > 0 && y > 0 && m_cells[y - 1, x - 1] == fieldItem)
		{
			++lineLength;
			startPos.x = x - 1;
			startPos.y = y - 1;
			if (x > 1 && y > 1 && m_cells[y - 2, x - 2] == fieldItem)
			{
				++lineLength;
				startPos.x = x - 2;
				startPos.y = y - 2;
			}
		}
		if (x < 2 && y < 2 && m_cells[y + 1, x + 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x + 1;
			endPos.y = y + 1;
			if (x < 1 && y < 1 && m_cells[y + 2, x + 2] == fieldItem)
			{
				++lineLength;
				endPos.x = x + 2;
				endPos.y = y + 2;
			}
		}
		return lineLength >= Constant.LINE_LENGTH_TO_WIN;
	}

	private static bool TryToWinCheckRightDiagonal(uint x, uint y, ref Vector3 startPos, ref Vector3 endPos)
	{
		uint lineLength = 1;
		FieldItem fieldItem = m_cells[y, x];
		if (x < 2 && y > 0 && m_cells[y - 1, x + 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x + 1;
			endPos.y = y - 1;
			if (x < 1 && y > 1 && m_cells[y - 2, x + 2] == fieldItem)
			{
				++lineLength;
				endPos.x = x + 2;
				endPos.y = y - 2;
			}
		}
		if (x > 0 && y < 2 && m_cells[y + 1, x - 1] == fieldItem)
		{
			++lineLength;
			endPos.x = x - 1;
			endPos.y = y + 1;
			if (x > 1 && y < 1 && m_cells[y + 2, x - 2] == fieldItem)
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
			m_lineImage.color = new Color(0.33f, 0.33f, 0.33f, 1);
		}
		else if (fieldItem == FieldItem.Round)
		{
			m_lineImage.color = new Color(0.95f, 0.92f, 0.827f, 1);
		}
		else
		{
			m_lineImage.color = new Color(0, 0, 0, 0);
		}
		m_lineRectTransform.offsetMin = new Vector2(left, bottom);
		m_lineRectTransform.offsetMax = new Vector2(-right, -top);
		m_lineRectTransform.rotation = Quaternion.Euler(0, 0, zRotation);
	}

	public static bool IsCellFree(int x, int y)
	{
		return m_cells[y, x] == FieldItem.Empty;
	}

	public static bool IsFieldFull()
	{
		foreach (FieldItem cell in m_cells)
		{
			if (cell == FieldItem.Empty)
			{
				return false;
			}
		}
		return true;
	}

	public static FieldItem[,] GetFieldItems()
	{
		return m_cells;
	}

	public void UndoOnce()
	{
		if (m_lastTurn.x != -1 && m_lastTurn.y != -1)
		{
			m_cells[(uint)m_lastTurn.y, (uint)m_lastTurn.x] = FieldItem.Empty;
			m_cellsImage[(uint)m_lastTurn.y, (uint)m_lastTurn.x].color = new Color(1, 1, 1, 0);
			m_lastTurn = new Vector2(-1, -1);
			GameController.NextTurn();
		}
	}

	public static void MarkCell(uint x, uint y)
	{
		ClearMarks();
		m_cellsBackground[y, x].color = new Color(0.77f, 0.59f, 0.59f, 0.51f);
	}
}
