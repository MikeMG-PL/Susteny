using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
   [HideInInspector] public CloudsManager cloudManager;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + cloudManager.cloudsSpeed / transform.localScale.x, transform.position.y, transform.position.z);
        if (cloudManager.areaWidth < Vector3.Distance(transform.position,
            new Vector3(cloudManager.cameraTransform.position.x, transform.position.y, cloudManager.cameraTransform.position.z)))
        {
            cloudManager.GenerateCloud();
            Destroy(gameObject);
        }
    }
}
