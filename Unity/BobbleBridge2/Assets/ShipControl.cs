using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   if( Input.GetKey(KeyCode.W) )
         gameObject.rigidbody2D.AddRelativeForce( new Vector2(0f,1f) );
      if( Input.GetKey(KeyCode.S) )
         gameObject.rigidbody2D.AddRelativeForce( new Vector2(0f,-1f) );
      if (Input.GetKey (KeyCode.A))
         gameObject.rigidbody2D.AddTorque ( 0.2f);
      if (Input.GetKey (KeyCode.D))
         gameObject.rigidbody2D.AddTorque (-0.2f);
	}
}
