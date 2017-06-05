using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Trash object
/// </summary>
public class Trash : MonoBehaviour {
	TruckBar truckbar;

	/// <summary>
	/// Type of the trash
	/// </summary>
	public ClassificationType trashType = ClassificationType.Waste;

	/// <summary>
	/// Draggable possibility
	/// </summary>
	public bool draggable = true;

	Sprite spriteTrashClose;
	Sprite spriteTrashOpen;
	GameObject particleEffect;
	GameObject bonusText;
	GameObject trashEmpty;

	SpriteRenderer spriteRenderer;

	Vector3 screenPointDrag;
	Vector3 offsetDrag;

	float startScale = 1f;
	bool moving = false;
	GameObject trashEmptyClone;

	bool isInTruckBar = false;

	PlayerController player;

    void Start () {
		//Define the draggable state in function of the game world
		draggable = Global.level == 2;
    }

	void Awake(){
		//Load ressources
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteTrashClose = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_close");
		spriteTrashOpen = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_open");
		particleEffect = Resources.Load<GameObject>
			("Trash/ParticleSystemTrash");
		bonusText = Resources.Load<GameObject>
			("Trash/PopText3D");
		trashEmpty = Resources.Load<GameObject>
			("Trash/TrashEmpty");
		startScale = transform.localScale.x;

		//Create empty trash clone
		trashEmptyClone = Instantiate (trashEmpty, transform.position, transform.rotation) as GameObject;
		trashEmptyClone.GetComponent<SpriteRenderer>().sortingOrder = -2;
		trashEmptyClone.transform.localScale = transform.localScale;

		iTween.FadeTo(trashEmptyClone, iTween.Hash("alpha", 0f, "time",0.2f,"easetype", iTween.EaseType.linear));
		iTween.ScaleTo (trashEmptyClone, iTween.Hash ("scale", new Vector3(startScale*0.5f, startScale*0.5f, startScale*0.5f), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));
	}

	void Update()
	{
		GetComponent<Renderer>().enabled = Global.isStart;
	}

	/// <summary>
	/// Show the open trash
	/// </summary>
	public void Open(){
		if(!moving)
			spriteRenderer.sprite = spriteTrashOpen;
	}

	/// <summary>
	/// Show the closed trash
	/// </summary>
	public void Close(){
		spriteRenderer.sprite = spriteTrashClose;
	}

	/// <summary>
	/// Show the effect when we drop a waste in the trash
	/// </summary>
	public void MakeParticleEffect(){
		if (this.particleEffect) {
			Vector3 posParticle = transform.position;
			posParticle.y += 1.5f;
			posParticle.z += 1;
			GameObject particleClone = Instantiate (particleEffect, posParticle, Quaternion.Euler(new Vector3(-90, 0, 0))) as GameObject;
			Destroy (particleClone, 2);
		}

		// Make rotation effect
		Quaternion v = gameObject.transform.rotation;
		v.z = -0.1f;
		transform.rotation = v;
		iTween.RotateTo(gameObject, iTween.Hash("z",0f,"time",1f,"easetype", iTween.EaseType.easeOutElastic));
	}

	/// <summary>
	/// Show the text of the obteined points
	/// </summary>
	/// <param name="text">Text to show</param>
	/// <param name="color">Color of the text</param>
	public void MakePopText(string text, Color color){
		Vector3 posText = transform.position;
		GameObject bonusClone = Instantiate (bonusText, posText, Quaternion.identity) as GameObject;
		Destroy (bonusClone, 3f); // Destoy 3 seconds after
		TextMesh txtMesh = bonusClone.GetComponentInChildren<TextMesh> ();
		txtMesh.alignment = TextAlignment.Center;
		txtMesh.text = text;
		txtMesh.color = color;
		posText.y += 1f;
		posText.z -= 1; // Place text in front of all
		bonusClone.transform.position = posText;
	}

	/// <summary>
	/// Show a positive score (green) and make a sound
	/// </summary>
	/// <param name="score">Score to show</param>
	public void MakePopScoreGood(int score){
		MakePopText("+"+score, Global.TRASH_GOOD_SCORE_COLOR);
	
		//play sound and destroy audio source
		MakeSound ("Music/WasteNoise", 1.0f, 0.2f);
		MakeSound("Music/GoodPoint", 1.0f);
	}

	/// <summary>
	/// Show a negative score (red) and make a sound
	/// </summary>
	/// <param name="score">Score to show</param>
	public void MakePopScoreBad(int score){
		string prefix = "-";
		if (score < 0)
			prefix = "";
		MakePopText(prefix+score, Global.TRASH_BAD_SCORE_COLOR);

		//play sound and destroy audio source
		MakeSound ("Music/WasteNoise", 1.0f, 0.2f);
		MakeSound("Music/BadPoint", 0.3f);
	}

	/// <summary>
	/// Play a sound
	/// </summary>
	/// <param name="resource">Ressource of the song</param>
	/// <param name="volume">Volume</param>
	/// <param name="time">Period of play</param>
	public void MakeSound(string resource, float volume, float time=0.0f){
		//play sound and destroy audio source
		AudioClip myClip = Resources.Load<AudioClip>(resource);
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = myClip;
		audioSource.time = time;
		audioSource.volume = volume;
		audioSource.Play();
		Destroy(audioSource, myClip.length);
	}

	void OnMouseDown(){
		if (draggable) {
			screenPointDrag = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			offsetDrag = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPointDrag.z));

			// Anim to grow up
			GrowUp();

			moving = true;

			//Show the trash shadow
			ShowEmptyTrash();
		}

	}

	void OnMouseDrag(){
		if (draggable) {
			Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPointDrag.z);
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorPoint) + offsetDrag;

			transform.position = cursorPosition;
		}
	}

	void OnMouseUp(){
		if (draggable) {
			// Anim to grow down
			GrowDown();

			moving = false;

			//If in the truckbar, place the trash in
			if (truckbar) {
				isInTruckBar = true;
				truckbar.PlaceTrash (this.gameObject, this);
				truckbar.Close ();
			} else{
				//If not, replace the trash to his place
				ReplaceTrash ();
			}
		}
	}

	public void OnMouseEnter() {
		Debug.Log ("OnMouseEnter");

		this.Open ();
		GetPlayer ().trashToDrag = this;
	}

	public void OnMouseExit() {
		Debug.Log ("OnMouseExit");
		GetPlayer ().trashToDrag = null;
		this.Close ();
	}

	void OnTriggerEnter (Collider col)
	{
		//Set the entered truckbar
		TruckBar truckbar = col.gameObject.GetComponent<TruckBar> ();
		if (truckbar && !this.truckbar) {
			this.truckbar = truckbar;
			this.truckbar.Open ();
		}
	}

	void OnTriggerExit (Collider col)
	{
		//Set the exit of the truckbar
		TruckBar truckbar = col.gameObject.GetComponent<TruckBar> ();
		if (truckbar && this.truckbar) {
			this.truckbar.Close ();
			this.truckbar = null;
		}
	}

	/// <summary>
	/// Replace the trash to his place
	/// </summary>
	public void ReplaceTrash(){
		isInTruckBar = false;
		GrowDown();
		iTween.MoveTo (gameObject, iTween.Hash ("position", trashEmptyClone.transform.position, "time", 0.3f, "easetype", iTween.EaseType.easeOutBack));

		// Hide the shadow
		HideEmptyTrash();
	}

	/// <summary>
	/// Grow down the trash
	/// </summary>
	public void GrowDown(){
		// Anim to grow down
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (startScale, startScale, startScale), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));
	}

	/// <summary>
	/// Grow up the trash
	/// </summary>
	public void GrowUp(){
		// Anim to grow up
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (startScale + 0.2f, startScale + 0.2f, startScale + 0.2f), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));
	}

	/// <summary>
	/// Show the shadow of the trash
	/// </summary>
	public void ShowEmptyTrash(){
		iTween.FadeTo(trashEmptyClone, iTween.Hash("alpha", 1, "time",0.3f,"easetype", iTween.EaseType.linear));
		iTween.ScaleTo (trashEmptyClone, iTween.Hash ("scale", new Vector3(startScale, startScale, startScale), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));
	}

	/// <summary>
	/// Hide the shadow of the trash
	/// </summary>
	void HideEmptyTrash(){
		iTween.FadeTo(trashEmptyClone, iTween.Hash("alpha", 0f, "time",0.3f,"easetype", iTween.EaseType.linear));
		iTween.ScaleTo (trashEmptyClone, iTween.Hash ("scale", new Vector3(0.1f, 0.1f, 0.1f), "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
	}	

	/// <summary>
	/// Say if the trash is in the truckbar
	/// </summary>
	/// <returns>true if is it, false otherwise</returns>
	public bool IsInTruckBar(){
		return isInTruckBar;
	}

	/// <summary>
	/// Get the local player game object
	/// </summary>
	/// <returns></returns>
	PlayerController GetPlayer() {
		if (player == null) {
			GameObject localPlayerObject = GameObject.FindGameObjectWithTag ("LocalPlayer");
			if (localPlayerObject != null)
				player = localPlayerObject.GetComponent<PlayerController> ();
		}
		return player;
	}

}
