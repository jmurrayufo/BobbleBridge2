﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
   void FixedUpdate () {
      GameObject player = GameObject.FindWithTag("Player");
      Vector3 playerPostion = player.transform.position;

      // Set Position
      playerPostion.z = -10;

      transform.position = playerPostion;
	
	}
}