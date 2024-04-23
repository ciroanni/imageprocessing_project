using Unity.XR.CoreUtils;
using UnityEngine;

    public class SharingModuleScript : MonoBehaviour
    {
        private AnchorModuleScript anchorModuleScript;

        private void Start()
        {
            anchorModuleScript = GetComponent<AnchorModuleScript>();
        }

        public void ShareAzureAnchor()
        {
            Debug.Log("\nSharingModuleScript.ShareAzureAnchor()");

            GenericNetworkManager.Instance.azureAnchorId = anchorModuleScript.currentAzureAnchorID;
            Debug.Log("GenericNetworkManager.Instance.azureAnchorId: " + GenericNetworkManager.Instance.azureAnchorId);

            var pvLocalUser = GenericNetworkManager.Instance.localUser.gameObject;
            var pu = pvLocalUser.gameObject.GetComponent<PhotonUser>();
            pu.ShareAzureAnchorId();
            //find a children of given name

        }

        public void GetAzureAnchor()
        {
            Debug.Log("\nSharingModuleScript.GetAzureAnchor()");
            Debug.Log("GenericNetworkManager.Instance.azureAnchorId: " + GenericNetworkManager.Instance.azureAnchorId);

            anchorModuleScript.FindAzureAnchor(GenericNetworkManager.Instance.azureAnchorId);
            var user1 = gameObject.transform.Find("User1");
            Debug.Log("User1: " + user1.transform.position);
            var user2 = gameObject.transform.Find("User2");
            Debug.Log("User2: " + user2.transform.position);
            Debug.Log("Distance: " + (user1.transform.position - user2.transform.position));
        }
    }
