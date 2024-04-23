using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.Experimental.GlobalIllumination;
using Microsoft.Azure.SpatialAnchors;
using Unity.XR.CoreUtils;

public class DrawerScript : MonoBehaviourPun, IMixedRealityPointerHandler
{
    private LineRenderer lineRenderer;
    public GameObject drawingPrefab;
    public Material drawingMaterial;
    public Color32 drawingColor;
    public MeshRenderer resultColorMesh;
    public float startWidth = 0.01f;
    public float endWidth = 0.01f;
    public GameObject anchor = default;

    public static DrawerScript instance;


    private List<GameObject> drawings = new List<GameObject>();
    private bool isDrawing;
    private int currentDrawingID;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        isDrawing = false;
        Material lineMaterial = Instantiate(drawingMaterial);
        lineMaterial.color = drawingColor;
        resultColorMesh.material = lineMaterial;
    }

    public void enableDrawing()
    {
        isDrawing = true;
    }

    public void disableDrawing()
    {
        isDrawing = false;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        if (isDrawing)
            FreeDraw(eventData);
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

        if (isDrawing)
            AddDrawing();
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }

    public void Undo()
    {
        for (int i = drawings.Count - 1; i >= 0; i--)
        {
            if (drawings[i].activeSelf)
            {
                drawings[i].SetActive(false);
                break; //1 object at a time
            }
        }
        photonView.RPC("UndoRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void UndoRPC()
    {
        int lastobj = 0;
        List<GameObject> objects = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Drawing"))
        {
            if (!obj.GetPhotonView().IsMine)
            {
                objects[lastobj] = obj;
                lastobj++;
            }

        }

        for (int i = lastobj - 1; i > -1; i--)
        {
            if (objects[i].activeSelf)
            {
                objects[i].SetActive(false);
                break;
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < drawings.Count; i++)
        {
            PhotonView.Destroy(drawings[i]);
        }
        drawings.Clear();
        photonView.RPC("ClearRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void ClearRPC()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Drawing"))
        {
            if (!obj.GetPhotonView().IsMine)
                PhotonView.Destroy(obj);
        }
    }

    void AddDrawing()
    {
        GameObject drawing = PhotonNetwork.Instantiate("Line", anchor.transform.position, anchor.transform.rotation);
        lineRenderer = drawing.GetComponent<LineRenderer>();
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        drawings.Add(drawing);
        photonView.RPC("AddDrawingRPC", RpcTarget.All, drawing.GetPhotonView().ViewID, startWidth, endWidth);
        Debug.Log("RPC called");
    }

    [PunRPC]
    public void AddDrawingRPC(int id, float startWidth, float endWidth)
    {
        GameObject drawing = PhotonView.Find(id).gameObject;
        lineRenderer = drawing.GetComponent<LineRenderer>();
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
    }
    void FreeDraw(MixedRealityPointerEventData eventData)
    {
        Material lineMaterial = Instantiate(drawingMaterial);
        lineMaterial.color = drawingColor;
        lineRenderer.material = lineMaterial;
        var handPos = eventData.Pointer.Position;
        Vector3 mousePos = new Vector3(handPos.x, handPos.y, handPos.z);

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
        GameObject user = anchor.GetComponentInChildren<PhotonUser>().gameObject;

        if (!user.GetPhotonView().IsMine)
        {
            if (user.name.Equals("User1"))
            {
                user = GameObject.Find("User2");
            }
            else
            {
                user = GameObject.Find("User1");
            }
        }

        Vector3 mousePosRel = mousePos - user.transform.position;
        photonView.RPC("FreeDrawRPC", RpcTarget.Others, mousePosRel, lineMaterial.color.r, lineMaterial.color.g, lineMaterial.color.b, lineMaterial.color.a);

    }


    [PunRPC]
    public void FreeDrawRPC(Vector3 mousePosRel, float red, float green, float blue, float alpha)
    {
        Material lineMaterial = Instantiate(drawingMaterial);
        //lineMaterial.color = drawingColor;
        Color lineColor = new Color(red, green, blue, alpha);
        lineMaterial.color = lineColor;
        lineRenderer.material = lineMaterial;
        GameObject user = anchor.GetComponentInChildren<PhotonUser>().gameObject;

        if (user.GetPhotonView().IsMine)
        {
            if (user.name.Equals("User1"))
            {
                user = GameObject.Find("User2");
            }
            else
            {
                user = GameObject.Find("User1");
            }
        }

        Vector3 mousePos = mousePosRel + user.transform.position;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
    }
}