using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using Game;


/*游戏通过Controller来创造相应的对象*/
namespace Controller {
    /*BackgroundController*/
    public class BackgroundController
    {
        readonly GameObject background;
        public BackgroundController()
        {
            background = Object.Instantiate(Resources.Load("Perfabs/Background", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        }
        public void setName(string _name)
        {
            background.name = _name;
        }
        public void setPosition(Vector3 background_position)
        {
            background.transform.position = background_position;
        }
        public string getName()
        {
            return background.name;
        }
    }


    /*MyCharacterController*/
    public class MyCharacterController
    {
        readonly GameObject character;
        readonly Moveable moveableScript;
        readonly ClickGUI clickGUI;
        readonly int characterType; 
        // 0->priest, 1->devil

        bool _isOnBoat;
        CoastController coastController;


        public MyCharacterController(string which_character)
        {

            if (which_character == "priest")
            {
                character = Object.Instantiate(Resources.Load("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                characterType = 0;
            }
            else
            {
                character = Object.Instantiate(Resources.Load("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                characterType = 1;
            }
            moveableScript = character.AddComponent(typeof(Moveable)) as Moveable;

            clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
            clickGUI.setController(this);
        }

        public void setName(string name)
        {
            character.name = name;
        }

        public void setPosition(Vector3 pos)
        {
            character.transform.position = pos;
        }

        public void moveToPosition(Vector3 destination)
        {
            moveableScript.setDestination(destination);
        }

        public int getType()
        { 
            return characterType;
        }

        public string getName()
        {
            return character.name;
        }

        public void getOnBoat(BoatController boatCtrl)
        {
            coastController = null;
            character.transform.parent = boatCtrl.getGameobj().transform;
            _isOnBoat = true;
        }

        public void getOnCoast(CoastController coastCtrl)
        {
            coastController = coastCtrl;
            character.transform.parent = null;
            _isOnBoat = false;
        }

        public bool isOnBoat()
        {
            return _isOnBoat;
        }

        public CoastController getCoastController()
        {
            return coastController;
        }

        public void reset()
        {
            moveableScript.reset();
            coastController = (Director.getInstance().currentSceneController as FirstController).fromCoast;
            getOnCoast(coastController);
            setPosition(coastController.getEmptyPosition());
            coastController.getOnCoast(this);
        }
    }

    /*CoastController*/
    public class CoastController
    {
        readonly GameObject coast;
        readonly Vector3 from_pos = new Vector3(9, -2, 0);
        readonly Vector3 to_pos = new Vector3(-9, -2, 0);
        readonly Vector3[] positions;
        readonly int to_or_from;    // to->-1, from->1

        // change frequently
        MyCharacterController[] passengerPlaner;

        public CoastController(string _to_or_from)
        {
            positions = new Vector3[] {
                new Vector3(5.4F,2.4F,0),
                new Vector3(6.6F,2.4F,0),
                new Vector3(7.8F,2.4F,0),
                new Vector3(9.0F,2.4F,0),
                new Vector3(10.2F,2.4F,0),
                new Vector3(11.4F,2.4F,0)};

            passengerPlaner = new MyCharacterController[6];

            if (_to_or_from == "from")
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
                coast.name = "from";
                to_or_from = 1;
            }
            else
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
                coast.name = "to";
                to_or_from = -1;
            }
        }

        public int getEmptyIndex()
        {
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 getEmptyPosition()
        {
            Vector3 pos = positions[getEmptyIndex()];
            pos.x *= to_or_from;
            return pos;
        }

        public void getOnCoast(MyCharacterController characterCtrl)
        {
            int index = getEmptyIndex();
            passengerPlaner[index] = characterCtrl;
        }

        public MyCharacterController getOffCoast(string passenger_name)
        {   // 0->priest, 1->devil
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passenger_name)
                {
                    MyCharacterController charactorCtrl = passengerPlaner[i];
                    passengerPlaner[i] = null;
                    return charactorCtrl;
                }
            }
            return null;
        }

        public int get_to_or_from()
        {
            return to_or_from;
        }

        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] == null)
                    continue;
                if (passengerPlaner[i].getType() == 0)
                {   // 0->priest, 1->devil
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            passengerPlaner = new MyCharacterController[6];
        }
    }

    /*BoatController*/
    public class BoatController
    {
        readonly GameObject boat;
        readonly Moveable moveableScript;
        readonly Vector3 fromPosition = new Vector3(3.0f, 0.8f, 0);
        readonly Vector3 toPosition = new Vector3(-3.0f, 0.8f, 0);
        readonly Vector3[] from_positions;
        readonly Vector3[] to_positions;

        int to_or_from; 
        // to->-1; from->1
        MyCharacterController[] passenger = new MyCharacterController[2];

        public BoatController()
        {
            to_or_from = 1;

            from_positions = new Vector3[] { new Vector3(2.5F, 2.0F, 0), new Vector3(3.5F, 2.0F, 0) };
            to_positions = new Vector3[] { new Vector3(-2.5F, 2.0F, 0), new Vector3(-3.5F, 2.0F, 0) };

            boat = Object.Instantiate(Resources.Load("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
            boat.name = "boat";

            moveableScript = boat.AddComponent(typeof(Moveable)) as Moveable;
            boat.AddComponent(typeof(ClickGUI));
        }


        public void Move()
        {
            if (to_or_from == -1)
            {
                moveableScript.setDestination(fromPosition);
                to_or_from = 1;
            }
            else
            {
                moveableScript.setDestination(toPosition);
                to_or_from = -1;
            }
        }

        public int getEmptyIndex()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool isEmpty()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public Vector3 getEmptyPosition()
        {
            Vector3 pos;
            int emptyIndex = getEmptyIndex();
            if (to_or_from == -1)
            {
                pos = to_positions[emptyIndex];
            }
            else
            {
                pos = from_positions[emptyIndex];
            }
            return pos;
        }

        public void GetOnBoat(MyCharacterController characterCtrl)
        {
            int index = getEmptyIndex();
            passenger[index] = characterCtrl;
        }

        public MyCharacterController GetOffBoat(string passenger_name)
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null && passenger[i].getName() == passenger_name)
                {
                    MyCharacterController charactorCtrl = passenger[i];
                    passenger[i] = null;
                    return charactorCtrl;
                }
            }
            return null;
        }

        public GameObject getGameobj()
        {
            return boat;
        }

        public int get_to_or_from()
        {
            return to_or_from;
        }

        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                    continue;
                if (passenger[i].getType() == 0)
                { 
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            moveableScript.reset();
            if (to_or_from == -1)
            {
                Move();
            }
            passenger = new MyCharacterController[2];
        }
    }
}
