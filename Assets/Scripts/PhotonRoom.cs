using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
        public static PhotonRoom Room;
    UnityEngine.TouchScreenKeyboard keyboard;

    [SerializeField] private Transform objectLocation = default;
        [SerializeField] private GameObject photonUserPrefab = default;
        private List<GameObject> objects = new List<GameObject>();
        // private PhotonView pv;
        private Player[] photonPlayers;
        private int playersInRoom;
        private int myNumberInRoom;
    public DrawerScript drawerScript;

        // private GameObject module;
        // private Vector3 moduleLocation = Vector3.zero;

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom++;
        }

        private void Awake()
        {
            if (Room == null)
            {
                Room = this;
            }
            else
            {
                if (Room != this)
                {
                    Destroy(Room.gameObject);
                    Room = this;
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
             //pv = GetComponent<PhotonView>();

            // Allow prefabs not in a Resources folder
            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if (photonUserPrefab != null) pool.ResourceCache.Add(photonUserPrefab.name, photonUserPrefab);

            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;
            PhotonNetwork.NickName = myNumberInRoom.ToString();

            StartGame();
        }

        private void StartGame()
        {
            CreatPlayer();

            if (!PhotonNetwork.IsMasterClient) return;
            
        }

        private void CreatPlayer()
        {
            var player = PhotonNetwork.Instantiate(photonUserPrefab.name, Vector3.zero, Quaternion.identity);
        }


    public int getPlayersInRoom() { return playersInRoom; }
    public int getMyNumberInRoom() { return myNumberInRoom; }
    public void SpawnCube()
    {
        var position = objectLocation.position;
        var positionOnTopOfSurface = new Vector3(position.x, position.y + objectLocation.localScale.y,
            position.z);

        var cube_inst = PhotonNetwork.Instantiate("building", positionOnTopOfSurface, objectLocation.rotation);
        objects.Add(cube_inst);
    }
    public void SpawnSphere()
    {
        var position = objectLocation.position;
        var positionOnTopOfSurface = new Vector3(position.x, position.y + objectLocation.localScale.y,
            position.z);
        var sphere_inst = PhotonNetwork.Instantiate("road", positionOnTopOfSurface, objectLocation.rotation);
        objects.Add(sphere_inst);
    }

    public void SpawnCylinder()
    {
        var position = objectLocation.position;
        var positionOnTopOfSurface = new Vector3(position.x, position.y + objectLocation.localScale.y,
            position.z);
        var cylinder_inst = PhotonNetwork.Instantiate("tree", positionOnTopOfSurface, objectLocation.rotation);
        objects.Add(cylinder_inst);
    }

    public void SpawnNote()
    {
        var position = objectLocation.position;
        var positionOnTopOfSurface = new Vector3(position.x, position.y + objectLocation.localScale.y,
            position.z);
        var note_inst = PhotonNetwork.Instantiate("note", positionOnTopOfSurface, objectLocation.rotation);
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        objects.Add(note_inst);
    }

    public void Clear()
    {
        int id;
        for (int i = 0; i < objects.Count; i++)
        {
            id = objects[i].GetPhotonView().ViewID;
            Debug.Log("Object to delete is " + id);
            PhotonView.Destroy(objects[i]);
            photonView.RPC("ClearRPC", RpcTarget.Others, id);
        }
        objects.Clear();

    }

    [PunRPC]
    public void ClearRPC(int id)
    {
        Debug.Log("ClearRPC");
        GameObject _object = PhotonView.Find(id).gameObject;
        Debug.Log("Object to delete in RPC is " + _object);
        PhotonView.Destroy(_object);
    }

    public void Undo()
    {
        int id;
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i].activeSelf)
            {
                objects[i].SetActive(false);
                id = objects[i].GetPhotonView().ViewID;
                photonView.RPC("UndoRPC", RpcTarget.Others, id);
                break; //1 object at a time
            }
        }
    }

    [PunRPC]
    public void UndoRPC(int id)
    {
        GameObject _object = PhotonView.Find(id).gameObject;
        _object.SetActive(false);
    }
}
