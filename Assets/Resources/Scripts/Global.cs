using System;
using UnityEngine;

public static class Global
{
	public static bool isSinglePlayer = false;
	public static int level = 2;

	public const int GOOD_CLASSIFICATION_SCORE = 10;
	public const int FALSE_CLASSIFICATION_SCORE = -5;

	public const float TRUCK_BAR_TIME_TO_FACTORY = 3f; // In seconds

	public static string[] WASTES = new string[] { "Prefabs/Wastes/CerealBox", "Prefabs/Wastes/CerealBox2" };

	public static Vector3[] draggablesCoordinates = new [] { 
		new Vector3(-2.607f,17.70f), new Vector3(-1.507f,17.70f), new Vector3(-0.477f,17.70f), new Vector3(0.623f,17.70f), new Vector3(1.633f,17.70f), new Vector3(2.633f,17.70f),
		new Vector3(-2.607f,19.20f), new Vector3(-1.507f,19.20f), new Vector3(-0.477f,19.20f), new Vector3(0.623f,19.20f), new Vector3(1.633f,19.20f), new Vector3(2.633f,19.20f),
		new Vector3(-2.607f,20.70f), new Vector3(-1.507f,20.70f), new Vector3(-0.477f,20.70f), new Vector3(0.623f,20.70f), new Vector3(1.633f,20.70f), new Vector3(2.633f,20.70f),
		new Vector3(-2.607f,22.20f), new Vector3(-1.507f,22.20f), new Vector3(-0.477f,22.20f), new Vector3(0.623f,22.20f), new Vector3(1.633f,22.20f), new Vector3(2.633f,22.20f),
		new Vector3(-2.607f,23.70f), new Vector3(-1.507f,23.70f), new Vector3(-0.477f,23.70f), new Vector3(0.623f,23.70f), new Vector3(1.633f,23.70f), new Vector3(2.633f,23.70f) };
}