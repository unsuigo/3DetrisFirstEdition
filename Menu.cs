﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement; 

public class Menu : MonoBehaviour {
	
	public void Play (){
		SceneManager.LoadScene("Game");
	}

	public void QuitGame (){
//		game_end();
		Application.Quit();
	}
}
