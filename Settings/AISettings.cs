public class AISettings
{
	public const int maxBuilders = 3;
	
	private static int[] depositsLimit = {0, 3, 5};
	
	public static int GetCountOfBuilders(int countOfDeposits)
	{
		int countOfBuilders = 0;
		
		foreach (int currentDepositLimit in depositsLimit)
		{
			if (countOfDeposits > currentDepositLimit)
				countOfBuilders++;
		}
		
		return countOfBuilders;
	}
}
