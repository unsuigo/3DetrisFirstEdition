using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLimitsZone : MonoBehaviour {

	private static int zoneWidth = 5;
	private static int zoneDeep = 5;
	private static int zoneHeight = 12;

	public static Transform[,,] zone = new Transform[zoneWidth, zoneHeight, zoneDeep];

	Material newMat;


	//Score
	public  int scoreOneLayer = 50;
	public  int scoreTwoLayers = 150;
	public  int scoreThreeLayers = 400;
	private int numberOfRowsInTurn = 0;
	public static int score = 0;
	public static int cubes = 0;
	public static int layers = 0;

	public static int scoreGameOver;
	public static int cubesGameOver;
	public static int layersGameOver;
//	public static int bestScore;

	public static float fall_speed = 6f;



	public Text score_text;
//	public Text layersyourScoreIs;


	//Audio
	private AudioSource audioSource;
	private bool IsMusicMuted = false;

	public AudioClip oneLayerDoneSound;
	public AudioClip twoLayersDoneSound;
	public AudioClip threeLayersDoneSound;
//	public AudioClip music;

	public AudioMixerGroup Music;


	//for future menu sliders
//	public Slider MainVolume;
//	public Slider MusicVolume;




	void Start () {
		SpawnNextItem ();
		audioSource = GetComponent<AudioSource> ();
	}

	void Update (){
		UpdateScore ();
		UpdateUI ();
	}



	void UpdateUI (){
		if(score_text.text != score.ToString ()){
		score_text.text = score.ToString ();
		Canvas.ForceUpdateCanvases(); 
		}
	}


	 
	public void UpdateScore (){
		if(numberOfRowsInTurn > 0){
			if (numberOfRowsInTurn == 1) {
				GotOneLayer ();
			} else if (numberOfRowsInTurn == 2) {
				GotTwoLayers ();
			} else if (numberOfRowsInTurn == 3) {
				GotThreeLayers ();
			}

			numberOfRowsInTurn = 0;
		}
	}

	public void GotOneLayer (){
		score += scoreOneLayer;
		PlayLayerDoneSound ();
	}

	public void GotTwoLayers (){
		score += scoreTwoLayers;
		PlayTwoLayersDoneSound ();
	}

	public void GotThreeLayers (){
		score += scoreThreeLayers;
		PlayThreeLayersDoneSound ();
	}


	public bool CheckIsAboveZoneItems(TetrisBehaviour form){

				foreach (Transform item in form.transform){
					Vector3 pos = Round (item.position);

					if (pos.y < zoneHeight - 1 ) {
						return true;
					}
				}

		return false;
	}


	public void DeleteLayerItems (int y){
		for (int x = 0; x < zoneWidth; x++) {
			for (int z = 0; z < zoneDeep; z++) {
				Destroy (zone[x,y,z].gameObject);
				zone [x, y, z] = null;
			}
		}
	}

	public void DeleteLayer (){
		for (int y = 0; y < zoneHeight; y++) {
			if (IsFullLayer(y)) {
				DeleteLayerItems (y);
				layers++;
				MoveAllLayersDown (y+1);
				--y;
			}
		}
	}

	public bool IsFullLayer (int y){
		for (int x = 0; x < zoneWidth; x++) {
			for (int z = 0; z < zoneDeep; z++) {
				if (zone[x,y,z] == null){
					
					return false;
				}
			}
		}

		numberOfRowsInTurn++;
		return true;
	}


	public void MoveLayerDown (int y){
//		Material newMat;

		for (int x = 0; x < zoneWidth; x++) {
			for (int z = 0; z < zoneDeep; z++) {
				
				if (zone[x, y, z] != null){
					
					Renderer renderer =zone [x, y, z]. GetComponent<MeshRenderer>();
					newMat = FindObjectOfType<Grafics> ().SetMaterialDown (y-1);
					renderer.material = newMat;

//					Debug.Log ("MoveLayerDown  " +x+y+z);
					zone[x, y-1, z]= zone[x,y,z];
					zone [x, y, z] = null;
					zone [x, y - 1, z].position += new Vector3 (0, -1, 0);

//					Vector3 pos = zone [x, y, z];
				}
			}
		}
	}


	public void MoveAllLayersDown (int y){
		for (int i = y; i < zoneHeight; i++) {
			MoveLayerDown (i);
		}
	}



	public void UpdateZone (TetrisBehaviour form){
		
		for (int y = 0; y < zoneHeight; ++y) {
			for (int x = 0; x < zoneWidth; ++x) {
				for (int z = 0; z < zoneDeep; ++z) {

					if (zone [x, y, z] != null) {
						
						if (zone [x, y, z].parent == form.transform) {
							FindObjectOfType<WallBehaviour> ().CleanShadow ((int)x, (int)y, (int)z);
							zone [x, y, z] = null;
						}
					}
				}
			}
		}


		foreach (Transform item in form.transform){
			Vector3 pos = Round (item.position);
			if (pos.y < zoneHeight ) {
				zone [(int)pos.x, (int)pos.y, (int)pos.z] = item;

				FindObjectOfType<WallBehaviour> ().Shadow ((int)pos.x, (int)pos.y, (int)pos.z);

//				Renderer renderer = item.GetComponent<MeshRenderer>();
//				newMat = FindObjectOfType<Grafics> ().SetMaterial ((int)pos.y);
//				renderer.material = newMat;
			}
		}
	}


	public Transform GetTransformZonePosition (Vector3 pos) {
		if (pos.y > zoneHeight ) {
			return null;
		} else {
//									Debug.Log ("GetTransformZonePosition   " +pos);
			return zone [(int)pos.x, (int)pos.y, (int)pos.z];

		}
	}

	// create a new form
	public void SpawnNextItem () {
		GameObject nextItem = (GameObject)Instantiate (
			                    Resources.Load (GetRandomForm (), 
								typeof(GameObject)), 
								new Vector3 (1.0f, 11.0f, 1.0f), 
								Quaternion.identity);
	}


	public bool CheckIsInsideZone (Vector3 pos) {

		return ((int)pos.x >= 0 && (int)pos.x < zoneWidth 
			 && (int)pos.z >= 0 && (int)pos.z < zoneDeep 
			 && (int)pos.y >= 0);
	}

	public Vector3 Round(Vector3 pos) {
		return new Vector3 (Mathf.Round(pos.x), 
							Mathf.Round(pos.y), 
							Mathf.Round(pos.z));
	}

	string GetRandomForm() {
		int randomItem = Random.Range (1, 9);

		string randomName = "Prefabs/Form_1";

		switch (randomItem) {

		case 1: 
			randomName = "Prefabs/Form_1";
			break;
		case 2: 
			randomName = "Prefabs/Form_2";
			break;
		case 3: 
			randomName = "Prefabs/Form_3_I";
			break;
		case 4: 
			randomName = "Prefabs/Form_3_L";
			break;
		case 5: 
			randomName = "Prefabs/Form_4_L";
			break;
		case 6: 
			randomName = "Prefabs/Form_4_H";
			break;
		case 7: 
			randomName = "Prefabs/Form_4_S";
			break;
		case 8: 
			randomName = "Prefabs/Form_4_Z";
			break;
		case 9: 
			randomName = "Prefabs/Form_4_Z";
			break;
		}
		return randomName;

	}

	// delete form parent when landed
	public void RemoveParent(){
		transform.DetachChildren();
		Destroy(gameObject);
	}

	
	public void GameOver (){
		scoreGameOver = score;
		cubesGameOver = cubes;
		layersGameOver = layers;

		score = 0;
		cubes = 0;
		layers = 0;
		SceneManager.LoadScene("GameOver");
	}

	//AUDIO

	public void VolumeMainChange()
	{
//		audMain.audioMixer.SetFloat("MyExposedParamMixer1", 0f);
	}
	public void VolumeMusicChange()
	{
		if(!IsMusicMuted){
			
			Music.audioMixer.SetFloat("MusicExposedParam", -80f);
			IsMusicMuted = true;
		}else {
			Music.audioMixer.SetFloat("MusicExposedParam", -12f);
			IsMusicMuted = false;
		}
	}
	//Layers Done Sound
	void PlayLayerDoneSound () {
		audioSource.PlayOneShot (oneLayerDoneSound);
	}

	void PlayTwoLayersDoneSound () {
		audioSource.PlayOneShot (twoLayersDoneSound);
	}

	void PlayThreeLayersDoneSound () {
		audioSource.PlayOneShot (threeLayersDoneSound);
	}

	public void MusicSwitch (){
				VolumeMusicChange ();

	}

}    




