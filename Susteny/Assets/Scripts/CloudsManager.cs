using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour
{
    public GameObject basicCloud;
    public int cloudsNumber;
    public float areaWidth;

    public float cloudsScaleMin;
    public float cloudsScaleMax;
    public float cloudsHeightMin;
    public float cloudsHeightMax;
    public float cloudsSpeed;

    public void Start()
    {
        for (int i = 0; i < cloudsNumber; i++)
        {
            generateCloud();
        }
    }

    public void Update()
    {
        foreach(GameObject cloud in GameObject.FindGameObjectsWithTag("Cloud"))
        {
            cloud.transform.position = new Vector3(cloud.transform.position.x + cloudsSpeed / cloud.transform.localScale.x, cloud.transform.position.y, cloud.transform.position.z);
            if (areaWidth < Vector3.Distance(cloud.transform.position,
                new Vector3(Camera.main.transform.position.x, cloud.transform.position.y, Camera.main.transform.position.z)))
            {
                cloud.SetActive(false);
                generateCloud();
            }
        }
    }

    private void generateCloud()
    {
        float x = Random.Range(-areaWidth / 2, areaWidth / 2), z = Random.Range(-areaWidth / 2, areaWidth / 2);
        while (Mathf.PerlinNoise(x, z) > 0.5)
        {
            x = Random.Range(-areaWidth / 2, areaWidth / 2);
            z = Random.Range(-areaWidth / 2, areaWidth / 2);
        }
        Vector3 position = new Vector3(x + Camera.main.transform.position.x, Random.Range(cloudsHeightMin, cloudsHeightMax), z + Camera.main.transform.position.z);
        GameObject cloudClone = Instantiate(basicCloud, position, new Quaternion());
        float scale = Random.Range(cloudsScaleMin, cloudsScaleMax);
        cloudClone.transform.localScale += new Vector3(scale, scale, scale);
        cloudClone.transform.parent = transform;
    }
}
