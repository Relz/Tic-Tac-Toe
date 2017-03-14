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

enum FieldItems
{
	Empty,
	Cross,
	Round
}

public class Field : MonoBehaviour 
{
	public GameObject cell0, cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8;

	private static Image[,] cellsImage = new Image[3, 3];
	private static FieldItems[,] cells = new FieldItems[3, 3];

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
				cellsImage[i, j].color = new Color(0, 0, 0, 0);
				cells[i, j] = FieldItems.Empty;
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
		cellsImage[cellPos.y, cellPos.x].color = new Color(0, 0, 0, 255);
		if (GameController.m_player0.GetRole() == PlayerRole.Cross)
		{
			cellsImage[cellPos.y, cellPos.x].sprite = crossSprite;
			cells[cellPos.y, cellPos.x] = FieldItems.Cross;
		}
		else
		{
			cellsImage[cellPos.y, cellPos.x].sprite = roundSprite;
			cells[cellPos.y, cellPos.x] = FieldItems.Round;
		}
	}
}
