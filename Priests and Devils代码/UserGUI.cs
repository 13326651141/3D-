using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using Game;

public class UserGUI : MonoBehaviour {
	private UserAction Player;
	public int status = 0;
	GUIStyle style;
	GUIStyle Button;

	void Start() {
		Player = Director.getInstance ().currentSceneController as UserAction;

		style = new GUIStyle();
		style.fontSize = 40;
		style.alignment = TextAnchor.MiddleCenter;

        Button = new GUIStyle("button");
        Button.fontSize = 30;
	}
	void OnGUI() {
		if (status == 1) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "Gameover!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "Restart", Button)) {
				status = 0;
				Player.restart ();
			}
		} else if(status == 2) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "You win!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "Restart", Button)) {
				status = 0;
				Player.restart ();
			}
		}

        if (GUI.Button(new Rect(0, 0, 150, 50), "Reset"))
        {
            Player.restart();
        }
	}
}
