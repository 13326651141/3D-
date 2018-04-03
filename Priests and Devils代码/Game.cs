using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using Game;

namespace Game {
	/*单例模式*/
	public class Director : System.Object {
		private static Director _instance;
		public SceneController currentSceneController { get; set; }

		public static Director getInstance() {
			if (_instance == null) {
				_instance = new Director ();
			}
			return _instance;
		}
	}

    /*场景控制器，生成对象，背景等等*/
	public interface SceneController {
		void loadResources ();
        void loadBackground();
	}

    /*用户行为，即用户能够执行的操作*/
	public interface UserAction {
		void moveBoat();
		void characterIsClicked(MyCharacterController characterCtrl);
		void restart();
        bool isGameOver();
	}

	/*The Function of move, 能够使物体运动的类*/
	public class Moveable: MonoBehaviour {
		
		readonly float move_speed = 20;

		int moving_status;	
        // 0->not moving, 1->moving to middle, 2->moving to dest
		Vector3 middle;
        Vector3 dest;

        void Update() {
			if (moving_status == 1) {
				transform.position = Vector3.MoveTowards (transform.position, middle, move_speed * Time.deltaTime);
				if (transform.position == middle) {
					moving_status = 2;
				}
			} else if (moving_status == 2) {
				transform.position = Vector3.MoveTowards (transform.position, dest, move_speed * Time.deltaTime);
				if (transform.position == dest) {
					moving_status = 0;
				}
			}
		}

        /*通过设置目的地可以达到物体移动的效果*/
		public void setDestination(Vector3 _dest) {
			dest = _dest;
			middle = _dest;
			if (_dest.y == transform.position.y) {	
                // boat moving
				moving_status = 2;
			}
			else if (_dest.y < transform.position.y) {	
                // character from coast to boat
				middle.y = transform.position.y;
			} else {								
                // character from boat to coast
				middle.x = transform.position.x;
			}
			moving_status = 1;
		}

		public void reset() {
			moving_status = 0;
		}
	}
}