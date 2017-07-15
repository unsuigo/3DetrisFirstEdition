﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour {

	public static int zoneWidth = 5;
	public static int zoneHeight = 12;

	public int cleanX, cleanY, cleanZ;

	Material newMat;
	public static Transform[,] wallF = new Transform[zoneWidth, zoneHeight];
	public static Transform[,] wallB = new Transform[zoneWidth, zoneHeight];
	public static Transform[,] wallL = new Transform[zoneWidth, zoneHeight];
	public static Transform[,] wallR = new Transform[zoneWidth, zoneHeight];



	// Use this for initialization
	void Start () {

		newMat = Resources.Load ("Materials/Wall Material", typeof(Material)) as Material;
		GameObject wallFront = GameObject.Find("TheWallFront");

		foreach (Transform item in wallFront.transform){
			Vector3 pos = FindObjectOfType<GameLimitsZone> ().Round (item.position);

				wallF [(int)pos.x, (int)pos.y] = item;
			
				Renderer renderer = item.GetComponent<MeshRenderer>();
				
				renderer.material = newMat;
		}

		GameObject wallBack = GameObject.Find("TheWallBack");

		foreach (Transform item in wallBack.transform){
			Vector3 pos = FindObjectOfType<GameLimitsZone> ().Round (item.position);

			wallB [(int)pos.x, (int)pos.y] = item;

			Renderer renderer = item.GetComponent<MeshRenderer>();
//			newMat = Resources.Load ("Materials/Wall Material", typeof(Material)) as Material;
			renderer.material = newMat;
		}

		GameObject wallLeft = GameObject.Find("TheWallLeft");

		foreach (Transform item in wallLeft.transform){
			Vector3 pos = FindObjectOfType<GameLimitsZone> ().Round (item.position);

			wallL [(int)pos.z, (int)pos.y] = item;

			Renderer renderer = item.GetComponent<MeshRenderer>();
//			newMat = Resources.Load ("Materials/Wall Material", typeof(Material)) as Material;
			renderer.material = newMat;
		}


		GameObject wallRight = GameObject.Find("TheWallRight");

		foreach (Transform item in wallRight.transform){
			Vector3 pos = FindObjectOfType<GameLimitsZone> ().Round (item.position);

			wallR [(int)pos.z, (int)pos.y] = item;

			Renderer renderer = item.GetComponent<MeshRenderer>();

//			newMat = Resources.Load ("Materials/Wall Material", typeof(Material)) as Material;
			renderer.material = newMat;
		}
	}
	
	public void Shadow(int x, int y, int z){

		cleanX = x; 
		cleanY = y;
		cleanZ = z;

		Renderer rendererF = wallF [x, y].GetComponent<MeshRenderer>();
		Renderer rendererB = wallB [x, y].GetComponent<MeshRenderer>();
		Renderer rendererR = wallR [z, y].GetComponent<MeshRenderer>();
		Renderer rendererL = wallL [z, y].GetComponent<MeshRenderer>();

			newMat = Resources.Load ("Materials/Wall Material Shadow", typeof(Material)) as Material;
			rendererF.material = newMat;
			rendererB.material = newMat;
			rendererR.material = newMat;
			rendererL.material = newMat;

	}

	public void CleanShadow(int x, int y, int z){

		
		Renderer rendererF = wallF [x, y].GetComponent<MeshRenderer>();
		Renderer rendererB = wallB [x, y].GetComponent<MeshRenderer>();
		Renderer rendererR = wallR [z, y].GetComponent<MeshRenderer>();
		Renderer rendererL = wallL [z, y].GetComponent<MeshRenderer>();

		newMat = Resources.Load ("Materials/Wall Material", typeof(Material)) as Material;
		rendererF.material = newMat;
		rendererB.material = newMat;
		rendererR.material = newMat;
		rendererL.material = newMat;

	}

}
