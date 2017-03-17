public class Constant 
{
	public class STATUS_TEXT
	{
		public static string WAIT_FOR_CHOICE = "Начните игру или выберите игрока";
		public static string ROUND_TURN = "ходит O";
		public static string CROSS_TURN = "ходит X";
	}
	public static uint FIELD_SIZE = 3;
	public static uint LINE_LENGTH_TO_WIN = 3;
	// Изменение LINE_LENGTH_TO_WIN приведет к неправильной работе поиска оптимального хода ИИ
	public static uint AI_THINKING_SECONDS = 2;
	public class ROUND_RESULT
	{
		public static string DRAFT = "Ничья";
		public class VICTORY
		{
			public static string CROSS = "Крестик победил";
			public static string ROUND = "Нолик победил";
		}
	}
}
