using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

	// Use this for initialization
	void Start () {
	}

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
