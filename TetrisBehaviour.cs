using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class TetrisBehaviour : MonoBehaviour {

	float fall = 0.1f;

	//temp var for BtnDone
	public float fallSpeed;

	public int itemScore = 10;
	private float itemScoreTime;

	private int qtyItemsOfForm = 0;
	private bool isPauseOn = false;

	public AudioClip moveSound;
	public AudioClip rotateSound;
	public AudioClip landSound;
//	public AudioClip oneLayerDoneSound;
//	public AudioClip twoLayersDoneSound;
//	public AudioClip threeLayersDoneSound;
	public AudioClip gameOverSound;

	private AudioSource audioSource;
//	private bool IsAudioSourcePlaying = true;





	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		CheckUserInput ();
		UpdateItemScore ();
	}

	//Start from 10 and reduse 1 every second
	void UpdateItemScore(){
		
		if (itemScoreTime < 1) {
			itemScoreTime += Time.deltaTime;
		} else {
			itemScoreTime = 0;
			itemScore--;
			itemScore = Mathf.Max (itemScore, 0);
		}
	}

	void CheckUserInput () {
		 
			if (
//				Input.GetKeyDown (KeyCode.RightArrow)
				CnInputManager.GetButtonDown("BtnRight")
			) {
			
			transform.position += new Vector3 (1, 0, 0);

			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayMoveSound ();
			}else{
				transform.position += new Vector3 (-1, 0, 0);
				}

		} else if (
//			Input.GetKeyDown (KeyCode.LeftArrow)
			CnInputManager.GetButtonDown("BtnLeft")
		) {

			transform.position += new Vector3 (-1, 0, 0);
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayMoveSound ();
			} else {
				transform.position += new Vector3 (1, 0, 0);
			}

		} else if (
//				Input.GetKeyDown (KeyCode.UpArrow)
				CnInputManager.GetButtonDown("BtnForward")
			) {
			transform.position += new Vector3 (0, 0, 1);
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayMoveSound ();
			} else {
				transform.position += new Vector3 (0, 0, -1);
			}

		} else if (
//			Input.GetKeyDown (KeyCode.DownArrow)
			CnInputManager.GetButtonDown("BtnBack")
		) {
			transform.position += new Vector3 (0, 0, -1);
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayMoveSound ();
			} else {
				transform.position += new Vector3 (0, 0, 1);
			}


		}else if (
//			Input.GetKeyDown (KeyCode.W)
			CnInputManager.GetButtonDown("BtnXup")
		) {
			transform.rotation = Quaternion.AngleAxis( 90, Vector3.right) * transform.rotation;
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayRotateSound ();
			} else {
				transform.rotation = Quaternion.AngleAxis( 90, Vector3.left) * transform.rotation;
			}

		}else if (
//			Input.GetKeyDown (KeyCode.S)
			CnInputManager.GetButtonDown("BtnXdown")
		) {
			transform.rotation = Quaternion.AngleAxis( 90, Vector3.left) * transform.rotation;
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayRotateSound ();
			} else {
				transform.rotation = Quaternion.AngleAxis( 90, Vector3.right) * transform.rotation;
			}

		}else if (
//			Input.GetKeyDown (KeyCode.E)
			CnInputManager.GetButtonDown("TurnRight")
		) {
			transform.rotation = Quaternion.AngleAxis( 90, Vector3.up) * transform.rotation;
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayRotateSound ();
			} else {
				transform.rotation = Quaternion.AngleAxis( 90, Vector3.down) * transform.rotation;
			}

		}else if (
//			Input.GetKeyDown (KeyCode.Q)
			CnInputManager.GetButtonDown("TurnLeft")
		) {
			transform.rotation = Quaternion.AngleAxis( 90, Vector3.down) * transform.rotation;
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayRotateSound ();
			} else {
				transform.rotation = Quaternion.AngleAxis( 90, Vector3.up) * transform.rotation;
			}

		}else if (
//			Input.GetKeyDown (KeyCode.A)
			CnInputManager.GetButtonDown("BtnZleft")
		) {
			transform.rotation = Quaternion.AngleAxis( 90, Vector3.forward) * transform.rotation;
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayRotateSound ();
			} else {
				transform.rotation = Quaternion.AngleAxis( 90, Vector3.back) * transform.rotation;
			}

		}else if (
//			Input.GetKeyDown (KeyCode.D)
			CnInputManager.GetButtonDown("BtnZright")
		) {
			transform.rotation = Quaternion.AngleAxis( 90, Vector3.back) * transform.rotation;
			if (CheckIsValidPosition ()) {
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
				PlayRotateSound ();
			} else {
				transform.rotation = Quaternion.AngleAxis( 90, Vector3.forward) * transform.rotation;
			}


			// speed fall and landing
		}else if ( Time.time - fall >= GameLimitsZone.fall_speed) {
			transform.position += new Vector3(0,-1,0);
			if (CheckIsValidPosition ()) {
				
				FindObjectOfType<GameLimitsZone> ().UpdateZone (this);
//										Debug.Log ("Space UpdateZoneShort done");

			} else {
				transform.position += new Vector3(0,1,0);

				PlayLandSound ();
				GameLimitsZone.fall_speed = fallSpeed;

				enabled = false;

				foreach (Transform item in transform){
					   Vector3 pos = FindObjectOfType<GameLimitsZone> ().Round (item.position);

					// set up material of layer to the child when it landed
					    Material newMat;
						Renderer renderer = item.GetComponent<MeshRenderer>();
						newMat = FindObjectOfType<Grafics> ().SetMaterialDown ((int)pos.y);
						renderer.material = newMat;

					// need for scores
						qtyItemsOfForm++;

				}

//				FindObjectOfType<GameLimitsZone> ().RemoveParent ();
				FindObjectOfType<GameLimitsZone> ().DeleteLayer ();

				if (!FindObjectOfType<GameLimitsZone> ().CheckIsAboveZoneItems(this)){ 
					
//					PlayGameOverSound ();
					FindObjectOfType<GameLimitsZone> ().GameOver ();

				}


//				SwitсhOffAaudioSource ();
				FindObjectOfType<GameLimitsZone> ().SpawnNextItem ();

//layer score plus speed score * qty of cubes
				GameLimitsZone.score += itemScore * qtyItemsOfForm ;
				GameLimitsZone.cubes += qtyItemsOfForm;
				qtyItemsOfForm = 0;
				itemScore = 10;

			}
			fall = Time.time;
			GameLimitsZone.fall_speed -= 0.0001f;
		}
		else if (Input.GetKeyDown (KeyCode.KeypadPlus)) 
		{
//			GameLimitsZone.fall_speed -= 1f;
		}

		else if (Input.GetKeyDown (KeyCode.KeypadMinus)) 
		{
//			GameLimitsZone.fall_speed += 10f;
		}

		else if (
//			Input.GetKeyDown (KeyCode.Space)
			CnInputManager.GetButtonDown("BtnDone")
		) 
		{
			if (!isPauseOn) {
				fallSpeed = GameLimitsZone.fall_speed;
				GameLimitsZone.fall_speed = 0.01f;
			}

		}

		else if (CnInputManager.GetButtonDown("BtnPause")){
			if (isPauseOn) {
				Time.timeScale = 1;
//				Debug.Log ("timer 1 ");
				isPauseOn = false;
			}else if (!isPauseOn) {
				Time.timeScale = 0;
				isPauseOn = true;
//				Debug.Log ("timer 0 ");
			}
		}

		else if (CnInputManager.GetButtonDown("BtnExit")){
			Application.Quit();
		}

		else if (CnInputManager.GetButtonDown("BtnMusic")){
			
//			if (IsAudioSourcePlaying) {
//				audioSource.Stop ();
//				IsAudioSourcePlaying = false;
////				Debug.Log ("IsAudioSourcePlaying false   ");
//			}else if (!IsAudioSourcePlaying){
//				audioSource.Play ();
//				IsAudioSourcePlaying = true;
////				Debug.Log ("IsAudioSourcePlaying true   ");
//			}

			FindObjectOfType<GameLimitsZone> ().MusicSwitch ();


		}


	}


	bool CheckIsValidPosition() {
		
		foreach(Transform item in transform){
			
			Vector3 pos = FindObjectOfType<GameLimitsZone> ().Round(item.transform.position);
			
//									Debug.Log("CheckIsValidPosition   " +pos);


			if (FindObjectOfType<GameLimitsZone> ().CheckIsInsideZone (pos) == false ) {
//									Debug.Log("CheckIsInsideZone   false " );
				return false;
			}

			if (FindObjectOfType<GameLimitsZone> ().GetTransformZonePosition (pos) != null
			    && FindObjectOfType<GameLimitsZone> ().GetTransformZonePosition (pos).parent != transform) 
			{
				return false;
			}
		}

		return true;
	}



// Audio

	void PlayMoveSound () {
		audioSource.PlayOneShot (moveSound);
	}

	void PlayRotateSound () {
		audioSource.PlayOneShot (rotateSound);
	}

	void PlayLandSound () {
		audioSource.PlayOneShot (landSound);
	}

//	void PlayLayerDoneSound () {
//		audioSource.PlayOneShot (oneLayerDoneSound);
//	}
//
//	void PlayTwoLayersDoneSound () {
//		audioSource.PlayOneShot (twoLayersDoneSound);
//	}
//
//	void PlayThreeLayersDoneSound () {
//		audioSource.PlayOneShot (threeLayersDoneSound);
//	}

//	void PlayGameOverSound () {
//		audioSource.PlayOneShot (gameOverSound);
//	}

	// Swith Off Aaudio Source when form is landed
	public void SwitсhOffAaudioSource (){
		audioSource.enabled = false;
	}

	
}
