using UnityEngine;

public enum PlayerRole
{
	None,
	Cross,
	Circle
}

public class Manager : MonoBehaviour 
{
	public static PlayerRole firstPlayerRole = PlayerRole.None;
}
