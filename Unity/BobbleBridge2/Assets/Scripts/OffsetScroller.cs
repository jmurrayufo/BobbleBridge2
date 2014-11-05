using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

   public float scrollSpeed;
   private Vector2 savedOffset;

	// Use this for initialization
	void Start () {
      savedOffset = renderer.sharedMaterial.GetTextureOffset ("_MainTex");
	}
	
	// Update is called once per frame
	void Update () {
      GameObject player = GameObject.FindWithTag("Player");
      Vector3 playerPostion = player.transform.position;
      playerPostion.z = 0;
      // Set Position
      // Adjust the z position so that the offset we apply doesn't override the player!
      playerPostion.z = 0;
      transform.position = playerPostion;

      // Set Offset
      float y = Mathf.Repeat (playerPostion.y * scrollSpeed, 1);
      float x = Mathf.Repeat (playerPostion.x * scrollSpeed, 1);
      Vector2 offset = new Vector2 (x, y);
      renderer.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}

   void OnDisable(){
      renderer.sharedMaterial.SetTextureOffset ("_MainTex", savedOffset);
   }
}
