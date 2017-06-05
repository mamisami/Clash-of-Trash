using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Trash Manager which manage the number of waste per trash
/// </summary>
public class TrashManager : NetworkBehaviour {


	[SyncVar]
	int trashWaste = 0;

	[SyncVar]
	int trashAlu = 0;

	[SyncVar]
	int trashCompost = 0;

	[SyncVar]
	int trashGlass = 0;

	[SyncVar]
	int trashPaper = 0;

	[SyncVar]
	int trashPet = 0;

	/// <summary>
	/// Add a waste to a trash
	/// </summary>
	/// <param name="type">Type of the trash</param>
	public void addWaste(ClassificationType type) {
		switch (type) {
			case ClassificationType.Alu:
				trashAlu++;
				break;
			case ClassificationType.Compost:
				trashCompost++;
				break;
			case ClassificationType.Glass:
				trashGlass++;
				break;
			case ClassificationType.Paper:
				trashPaper++;
				break;
			case ClassificationType.Pet:
				trashPet++;
				break;
			case ClassificationType.Waste:
				trashWaste++;
				break;
		}
	}

	/// <summary>
	/// Get points made with a trash
	/// </summary>
	/// <param name="wasteType">Type of the trash</param>
	/// <returns>Number of points</returns>
	public int getPoints(ClassificationType wasteType) {
		int nbWastes = 0;

		switch (wasteType) {
			case ClassificationType.Alu:
				nbWastes = trashAlu;
				break;
			case ClassificationType.Compost:
				nbWastes = trashCompost;
				break;
			case ClassificationType.Glass:
				nbWastes = trashGlass;
				break;
			case ClassificationType.Paper:
				nbWastes = trashPaper;
				break;
			case ClassificationType.Pet:
				nbWastes = trashPet;
				break;
			case ClassificationType.Waste:
				nbWastes = trashWaste;
				break;
		}

		return nbWastes * Global.PTS_PET_WASTE;
	}

	/// <summary>
	/// Rebase to 0 the number of waste in a trash
	/// </summary>
	/// <param name="type">Type of the trash</param>
	public void reinitWaste(ClassificationType type) {
		switch (type) {
			case ClassificationType.Alu:
				trashAlu = 0;
				break;
			case ClassificationType.Compost:
				trashCompost = 0;
				break;
			case ClassificationType.Glass:
				trashGlass = 0;
				break;
			case ClassificationType.Paper:
				trashPaper = 0;
				break;
			case ClassificationType.Pet:
				trashPet = 0;
				break;
			case ClassificationType.Waste:
				trashWaste = 0;
				break;
		}
	}
}
