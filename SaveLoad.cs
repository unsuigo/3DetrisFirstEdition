using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour {
	static int bestScore;

	void Save () {
		PlayerPrefs.SetInt ("score", GameLimitsZone.score);
	}
	
	void Load () {
		bestScore = PlayerPrefs.GetInt("score");
	}
}
