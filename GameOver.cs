﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	static int scoreIs;
	static int cubesGot;
	static int layersDone;

	static int bestScoreIs = 0;
	static int bestGotCubes = 0;
	static int bestLayersDone = 0;

	public Text scoreIs_text;
	public Text gotCubes_text;
	public Text layersDone_text;

	public Text bestScoreIs_text;
	public Text bestGotCubes_text;
	public Text bestLayersDone_text;


//	private Text yourScoreIs;
//	public Text bestScore_text;

	private AudioSource audioSource;
	public AudioClip gameOverSound;

	// Use this for initialization
	void Start () {
		GameOverScore ();
		if (scoreIs > bestScoreIs) {
			Save ();
		}
		audioSource = GetComponent<AudioSource> ();
		PlayGameOverSound ();
//		
	}
	

	public  void GameOverScore(){
		scoreIs = GameLimitsZone.scoreGameOver;
		cubesGot = GameLimitsZone.cubesGameOver;
		layersDone = GameLimitsZone.layersGameOver;

		Load ();
//		bestScoreIs = PlayerPrefs.GetInt("bestScore");
//		Debug.Log ("bestScoreIs   " + bestScoreIs);
//		Debug.Log ("layers   " + layersDone);

		scoreIs_text.text = scoreIs.ToString ();
		gotCubes_text.text = cubesGot.ToString ();
		layersDone_text.text = layersDone.ToString ();

		bestScoreIs_text.text = bestScoreIs.ToString ();
		bestGotCubes_text.text = bestGotCubes.ToString ();
		bestLayersDone_text.text = bestLayersDone.ToString ();


	}

	void Save () {
		PlayerPrefs.SetInt ("bestScore", scoreIs);
		PlayerPrefs.SetInt ("bestGotCubes", cubesGot);
		PlayerPrefs.SetInt ("bestLayersDone", layersDone);
	}

	void Load () {
		bestScoreIs = PlayerPrefs.GetInt("bestScore");
		bestGotCubes = PlayerPrefs.GetInt ("bestGotCubes");
		bestLayersDone = PlayerPrefs.GetInt ("bestLayersDone");
	}

	void PlayGameOverSound () {
		audioSource.PlayOneShot (gameOverSound);
	}
}
