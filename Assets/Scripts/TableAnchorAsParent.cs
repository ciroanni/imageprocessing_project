using UnityEngine;

    public class TableAnchorAsParent : MonoBehaviour
    {
        private void Start()
        {
            if (TableAnchor.Instance != null) transform.parent = TableAnchor.Instance.transform;
        }
    }

