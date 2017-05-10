using System;

public static class Global
{
	public static bool isSinglePlayer = true;
	public static int level = 1;

	public const int MAX_DRAGGABLES = 100;

	public const int GOOD_CLASSIFICATION_SCORE = 10;
	public const int FALSE_CLASSIFICATION_SCORE = -5;

	public const float TRUCK_BAR_TIME_TO_FACTORY = 10f; // In seconds
}

