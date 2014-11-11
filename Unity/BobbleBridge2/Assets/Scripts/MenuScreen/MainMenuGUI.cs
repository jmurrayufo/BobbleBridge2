using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {
   
   public Texture btnTexture;
   public Texture alienTexture;

   // \brief 
   public float mainGUIAreaWidth;
   public float mainGUIAreaHeight;

   void Start() {
      if (!btnTexture) {
         Debug.LogError("Please assign a texture on the inspector");
         return;
      }
      if (!alienTexture) {
         Debug.LogError("Please assign a texture on the inspector");
         return;
      }
      
   }


   void OnGUI() {
      GUILayout.BeginArea(new Rect((Screen.width / 2) - (mainGUIAreaWidth / 2), (Screen.height / 2) - (mainGUIAreaHeight / 2), mainGUIAreaWidth, mainGUIAreaHeight));

      if (GUILayout.Button("Start Game"))
      {
         Debug.Log("Clicked the button with an image");
         // Load the level named "HighScore".
         Application.LoadLevel ("PilotScreen");
      }
         
      if (GUILayout.Button(btnTexture, GUILayout.Width(30), GUILayout.Height (30)))
         Debug.Log("Clicked the button with an image");

      if (GUILayout.Button("Quit")) {
         Debug.Log("Clicked the button with text");
         Application.Quit();
      }
      GUILayout.EndArea ();
   }
   
}