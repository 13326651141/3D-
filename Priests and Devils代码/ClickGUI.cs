using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using Game;

public class ClickGUI : MonoBehaviour {
	UserAction Player;
	MyCharacterController characterController;

	public void setController(MyCharacterController characterCtrl) {
		characterController = characterCtrl;
	}

	void Start() {
        Player = Director.getInstance ().currentSceneController as UserAction;
	}

    void OnMouseDown() {
        if(!Player.isGameOver())
        {
            if (gameObject.name == "boat")
            {
                Player.moveBoat();
            }
            else
            {
                Player.characterIsClicked(characterController);
            }
        }

	}
}