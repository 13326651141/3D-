using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Controller;

public class FirstController : MonoBehaviour, SceneController, UserAction {
    /*动态常量 readonly*/
	readonly Vector3 water_pos = new Vector3(0,-2.0F,0);
    readonly Vector3 background_pos = new Vector3(0, 0, 20);


	UserGUI userGUI;
    public BackgroundController Background;
	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	private MyCharacterController[] characters;

	void Awake() {
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new MyCharacterController[6];
        loadBackground();
		loadResources ();
	}

    public void loadBackground()
    {
        /*创造背景*/
        Background = new BackgroundController();
        Background.setPosition(background_pos);
    }

	public void loadResources() {
        /*在Resources文件夹中加载Water，只有GameObject类型的才被加载，加载到water_pos的位置，无旋转，完全对齐于世界*/
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

        /*创造两个对岸*/
		fromCoast = new CoastController ("from");
		toCoast = new CoastController ("to");

        /*创造船*/
		boat = new BoatController ();

        /*创造牧师与魔鬼*/
		loadCharacter ();
	}

	private void loadCharacter() {
		for (int i = 0; i < 3; i++) {
			MyCharacterController Character = new MyCharacterController ("priest");
            Character.setName("priest" + i);
            Character.setPosition (fromCoast.getEmptyPosition ());
            Character.getOnCoast (fromCoast);
			fromCoast.getOnCoast (Character);

			characters [i] = Character;
		}

		for (int i = 0; i < 3; i++) {
			MyCharacterController Character = new MyCharacterController ("devil");
            Character.setName("devil" + i);
            Character.setPosition (fromCoast.getEmptyPosition ());
            Character.getOnCoast (fromCoast);
			fromCoast.getOnCoast (Character);

			characters [i+3] = Character;
		}
	}


	public void moveBoat() {
		if (boat.isEmpty ()) return;
		boat.Move ();
		userGUI.status = check_game_over ();
	}

	public void characterIsClicked(MyCharacterController characterCtrl) {
		if (characterCtrl.isOnBoat ()) {
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { // to->-1; from->1
				whichCoast = toCoast;
			} else {
				whichCoast = fromCoast;
			}

			boat.GetOffBoat (characterCtrl.getName());
			characterCtrl.moveToPosition (whichCoast.getEmptyPosition ());
			characterCtrl.getOnCoast (whichCoast);
			whichCoast.getOnCoast (characterCtrl);

		} else {
			CoastController whichCoast = characterCtrl.getCoastController ();

			if (boat.getEmptyIndex () == -1) {
				return;
			}

			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())	
                // boat is not on the side of character
				return;

			whichCoast.getOffCoast(characterCtrl.getName());
			characterCtrl.moveToPosition (boat.getEmptyPosition());
			characterCtrl.getOnBoat (boat);
			boat.GetOnBoat (characterCtrl);
		}
		userGUI.status = check_game_over ();
	}

    public bool isGameOver()
    {
        return check_game_over() != 0;
    }

	int check_game_over() {
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = fromCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)
			return 2;

		int[] boatCount = boat.getCharacterNum ();
		if (boat.get_to_or_from () == -1)
        {	
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		}
        else
        {	
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0)
        {
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0)
        {
			return 1;
		}
		return 0;
	}

	public void restart() {
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].reset ();
		}
	}
}
