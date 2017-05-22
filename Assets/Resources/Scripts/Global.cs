using System;
using UnityEngine;

public static class Global
{
	public static bool isSinglePlayer = true;
	public static int level = 1;

	public const int GOOD_CLASSIFICATION_SCORE = 10;
	public const int FALSE_CLASSIFICATION_SCORE = -5;

	public const float TRUCK_BAR_TIME_TO_FACTORY = 10f; // In seconds

	public const int PTS_PET_WASTE = 10;

	public static string WASTES_PATH = "Prefabs/Wastes";

	public static Vector3[] draggablesCoordinates = new [] { 
		new Vector3(-2.607f,18.20f), new Vector3(-1.507f,18.20f), new Vector3(-0.477f,18.20f), new Vector3(0.623f,18.20f), new Vector3(1.633f,18.20f), new Vector3(2.633f,18.20f),
		new Vector3(-2.607f,19.70f), new Vector3(-1.507f,19.70f), new Vector3(-0.477f,19.70f), new Vector3(0.623f,19.70f), new Vector3(1.633f,19.70f), new Vector3(2.633f,19.70f),
		new Vector3(-2.607f,21.20f), new Vector3(-1.507f,21.20f), new Vector3(-0.477f,21.20f), new Vector3(0.623f,21.20f), new Vector3(1.633f,21.20f), new Vector3(2.633f,21.20f),
		new Vector3(-2.607f,22.70f), new Vector3(-1.507f,22.70f), new Vector3(-0.477f,22.70f), new Vector3(0.623f,22.70f), new Vector3(1.633f,22.70f), new Vector3(2.633f,22.70f),
		new Vector3(-2.607f,24.20f), new Vector3(-1.507f,24.20f), new Vector3(-0.477f,24.20f), new Vector3(0.623f,24.20f), new Vector3(1.633f,24.20f), new Vector3(2.633f,24.20f) };
}