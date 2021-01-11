using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour
{
    public GameObject basicCloud;
    public Transform cloudsParent;
    public int cloudsNumber;
    public float areaWidth;

    public float cloudsScaleMin;
    public float cloudsScaleMax;
    public float cloudsHeightMin;
    public float cloudsHeightMax;
    public float cloudsSpeed;

    [HideInInspector] public Transform cameraTransform;

    public void Awake()
    {
        cameraTransform = Camera.main.transform;

        for (int i = 0; i < cloudsNumber; i++)
        {
            GenerateCloud();
        }
    }

    public void GenerateCloud()
    {
        float x = Random.Range(-areaWidth / 2, areaWidth / 2), z = Random.Range(-areaWidth / 2, areaWidth / 2);
        while (Mathf.PerlinNoise(x, z) > 0.5)
        {
            x = Random.Range(-areaWidth / 2, areaWidth / 2);
            z = Random.Range(-areaWidth / 2, areaWidth / 2);
        }
        Vector3 position = new Vector3(x + cameraTransform.position.x, Random.Range(cloudsHeightMin, cloudsHeightMax), z + cameraTransform.position.z);
        GameObject cloudClone = Instantiate(basicCloud, position, Quaternion.identity, cloudsParent);
        cloudClone.GetComponent<Cloud>().cloudManager = this;
        float scale = Random.Range(cloudsScaleMin, cloudsScaleMax);
        cloudClone.transform.localScale += new Vector3(scale, scale, scale);
        cloudClone.transform.localEulerAngles = new Vector3(Random.Range(0f, 20f), Random.Range(0f, 360f), Random.Range(0f, 20f));
    }
}
