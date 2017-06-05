using System;
using UnityEngine;

public static class Global
{
	//Global variables
	public static bool isSinglePlayer = true;
	public static bool isStart = false;
	public static bool reloadGame = false;
	public static int level = 1;

	//Global constants (most of the tunning is to be done here)
	public const float GAME_TIME = 90.0f;

	public const float SPAWN_TIME_MIN = 0.0f;
	public const float SPAWN_TIME_MAX = 15.0f;

	public const float TRUCK_BAR_TIME_TO_FACTORY = 10.0f; // In seconds

	/// <summary>
	/// Point per waste in a trash when the truck come to clear it
	/// </summary>
	public const int PTS_PET_WASTE = 20;

	public static string WASTES_PATH = "Prefabs/Wastes";
	
	public static Color TIMER_BLINK_COLOR_1 = Color.red;
	public static Color TIMER_BLINK_COLOR_2 = Color.yellow;

	public static Color TRASH_GOOD_SCORE_COLOR = Color.green;
	public static Color TRASH_BAD_SCORE_COLOR = Color.red;

	public static Vector3[] draggablesCoordinates = new [] { 
		new Vector3(-2.107f,18.20f), new Vector3(-1.007f,18.20f), new Vector3(0.177f,18.20f), new Vector3(1.123f,18.20f), new Vector3(2.133f,18.20f),
		new Vector3(-2.107f,19.70f), new Vector3(-1.007f,19.70f), new Vector3(0.177f,19.70f), new Vector3(1.123f,19.70f), new Vector3(2.133f,19.70f),
		new Vector3(-2.107f,21.20f), new Vector3(-1.007f,21.20f), new Vector3(0.177f,21.20f), new Vector3(1.123f,21.20f), new Vector3(2.133f,21.20f),
		new Vector3(-2.107f,22.70f), new Vector3(-1.007f,22.70f), new Vector3(0.177f,22.70f), new Vector3(1.123f,22.70f), new Vector3(2.133f,22.70f) };

	/* Old positions before tunning
	public static Vector3[] draggablesCoordinates = new [] { 
		new Vector3(-2.607f,18.20f), new Vector3(-1.507f,18.20f), new Vector3(-0.477f,18.20f), new Vector3(0.623f,18.20f), new Vector3(1.633f,18.20f), new Vector3(2.633f,18.20f),
		new Vector3(-2.607f,19.70f), new Vector3(-1.507f,19.70f), new Vector3(-0.477f,19.70f), new Vector3(0.623f,19.70f), new Vector3(1.633f,19.70f), new Vector3(2.633f,19.70f),
		new Vector3(-2.607f,21.20f), new Vector3(-1.507f,21.20f), new Vector3(-0.477f,21.20f), new Vector3(0.623f,21.20f), new Vector3(1.633f,21.20f), new Vector3(2.633f,21.20f),
		new Vector3(-2.607f,22.70f), new Vector3(-1.507f,22.70f), new Vector3(-0.477f,22.70f), new Vector3(0.623f,22.70f), new Vector3(1.633f,22.70f), new Vector3(2.633f,22.70f),
		new Vector3(-2.607f,24.20f), new Vector3(-1.507f,24.20f), new Vector3(-0.477f,24.20f), new Vector3(0.623f,24.20f), new Vector3(1.633f,24.20f), new Vector3(2.633f,24.20f) };
	*/
}