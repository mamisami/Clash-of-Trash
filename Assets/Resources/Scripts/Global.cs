using System;
using UnityEngine;

public static class Global
{
	public static bool isSinglePlayer = true;
	public static bool isStart = false;
	public static int level = 1;

	public const float GAME_TIME = 90.0f;

	public const float SPAWN_TIME_MIN = 0.0f;
	public const float SPAWN_TIME_MAX = 15.0f;

	public const float TRUCK_BAR_TIME_TO_FACTORY = 10f; // In seconds

	public const int PTS_PET_WASTE = 10;

	public static string WASTES_PATH = "Prefabs/Wastes";

	public static Vector3[] draggablesCoordinates = new [] { 
		new Vector3(-2.107f,18.20f), new Vector3(-1.007f,18.20f), new Vector3(0.177f,18.20f), new Vector3(1.123f,18.20f), new Vector3(2.133f,18.20f),
		new Vector3(-2.107f,19.70f), new Vector3(-1.007f,19.70f), new Vector3(0.177f,19.70f), new Vector3(1.123f,19.70f), new Vector3(2.133f,19.70f),
		new Vector3(-2.107f,21.20f), new Vector3(-1.007f,21.20f), new Vector3(0.177f,21.20f), new Vector3(1.123f,21.20f), new Vector3(2.133f,21.20f),
		new Vector3(-2.107f,22.70f), new Vector3(-1.007f,22.70f), new Vector3(0.177f,22.70f), new Vector3(1.123f,22.70f), new Vector3(2.133f,22.70f) };

	/*
	public static Vector3[] draggablesCoordinates = new [] { 
		new Vector3(-2.607f,18.20f), new Vector3(-1.507f,18.20f), new Vector3(-0.477f,18.20f), new Vector3(0.623f,18.20f), new Vector3(1.633f,18.20f), new Vector3(2.633f,18.20f),
		new Vector3(-2.607f,19.70f), new Vector3(-1.507f,19.70f), new Vector3(-0.477f,19.70f), new Vector3(0.623f,19.70f), new Vector3(1.633f,19.70f), new Vector3(2.633f,19.70f),
		new Vector3(-2.607f,21.20f), new Vector3(-1.507f,21.20f), new Vector3(-0.477f,21.20f), new Vector3(0.623f,21.20f), new Vector3(1.633f,21.20f), new Vector3(2.633f,21.20f),
		new Vector3(-2.607f,22.70f), new Vector3(-1.507f,22.70f), new Vector3(-0.477f,22.70f), new Vector3(0.623f,22.70f), new Vector3(1.633f,22.70f), new Vector3(2.633f,22.70f),
		new Vector3(-2.607f,24.20f), new Vector3(-1.507f,24.20f), new Vector3(-0.477f,24.20f), new Vector3(0.623f,24.20f), new Vector3(1.633f,24.20f), new Vector3(2.633f,24.20f) };
	*/
}