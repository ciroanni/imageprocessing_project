using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit;
using Photon.Pun;
public class Spawner : MonoBehaviourPun
{
    [SerializeField] GameObject building;
    [SerializeField] GameObject road;
    [SerializeField] GameObject tree;
    [SerializeField] private Transform objectLocation = default;

    private List<GameObject> objects = new List<GameObject>();
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
        var sphere_inst = PhotonNetwork.Instantiate("road",positionOnTopOfSurface, objectLocation.rotation);
        objects.Add(sphere_inst); 
    }

    public void SpawnCylinder()
    {
        var position = objectLocation.position;
        var positionOnTopOfSurface = new Vector3(position.x, position.y + objectLocation.localScale.y,
            position.z);
        var cylinder_inst = PhotonNetwork.Instantiate("street", positionOnTopOfSurface,objectLocation.rotation);
        objects.Add(cylinder_inst); 
    }

    public void Clear()
    {
        int id;
        for (int i = 0; i < objects.Count; i++)
        {
            id = objects[i].GetPhotonView().ViewID;
            PhotonView.Destroy(objects[i]);
            photonView.RPC("ClearRPC", RpcTarget.Others, id);
        }
        objects.Clear();
        
    }

    [PunRPC]
    public void ClearRPC(int id)
{
        GameObject _object = PhotonView.Find(id).gameObject;
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
