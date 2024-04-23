using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PositionSynchronizer : MonoBehaviourPun, IPunObservable
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastScale;
    //private Player lastPlayer;
    public void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastScale = transform.localScale;
    }

    public void Update()
    {
        if (!photonView.IsMine && (transform.position != lastPosition || transform.rotation != lastRotation || transform.localScale != lastScale))
        {
            Debug.Log("Is mine");
            //lastPlayer = gameObject.GetPhotonView().Owner;
            gameObject.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (transform.position != lastPosition || transform.rotation != lastRotation || transform.localScale != lastScale)
        {
            Debug.Log("Is Writing");
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            lastScale = transform.localScale; 
            photonView.RPC("UpdatePosition", RpcTarget.Others, transform.position, transform.rotation, transform.localScale);
            //gameObject.GetPhotonView().TransferOwnership(lastPlayer);
        }
    }

    [PunRPC]
    public void UpdatePosition(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastScale = transform.localScale;
    }

}
