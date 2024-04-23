using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
    public class GenericNetSync : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private bool isUser = default;
        [SerializeField] private bool isText = default;

        private Camera mainCamera;

        private Vector3 networkLocalPosition;
        private Quaternion networkLocalRotation;
        private Vector3 networkLocalScale;
    private string networkLocalText;
    private Vector3 startingLocalPosition;
        private Quaternion startingLocalRotation;
        private Vector3 startingLocalScale;

      

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
                stream.SendNext(transform.localScale);
                if (isText)
                {
                    stream.SendNext(GetComponent<Text>().text);
                }
            }
            else
            {
                networkLocalPosition = (Vector3) stream.ReceiveNext();
                networkLocalRotation = (Quaternion) stream.ReceiveNext();
                networkLocalScale = (Vector3) stream.ReceiveNext();
                if (isText)
                {
                    networkLocalText = (string) stream.ReceiveNext();
                }
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;

            if (isUser)
            {
                if (TableAnchor.Instance != null) transform.parent = FindObjectOfType<TableAnchor>().transform;

                if (photonView.IsMine) GenericNetworkManager.Instance.localUser = photonView;
            }

            var trans = transform;
            startingLocalPosition = trans.localPosition;
            startingLocalRotation = trans.localRotation;
            startingLocalScale = trans.localScale;

            networkLocalPosition = startingLocalPosition;
            networkLocalRotation = startingLocalRotation;
            networkLocalScale = startingLocalScale;
        }

        // private void FixedUpdate()
        private void Update()
        {
            if (!photonView.IsMine)
            {
                var trans = transform;
                trans.localPosition = networkLocalPosition;
                trans.localRotation = networkLocalRotation;
                trans.localScale = networkLocalScale;
                if (isText)
                {
                    gameObject.GetComponent<Text>().text = networkLocalText;
                }
            }

            if (photonView.IsMine && isUser)
            {
                var trans = transform;
                var mainCameraTransform = mainCamera.transform;
                trans.position = mainCameraTransform.position;
                trans.rotation = mainCameraTransform.rotation;
                trans.localScale = mainCameraTransform.localScale;
            }
        }
    }

