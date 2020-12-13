using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public Car carPrefab;
    float timer;
    float nextCarIn;
    public int destinationMIN, destinationMAX;
    public float nextCarMIN = 4, nextCarMAX = 12;

    private void Awake()
    {
        Rand();
    }

    void Rand()
    {
        var c = Instantiate(carPrefab, transform.position, Quaternion.identity);
        c.destinationType = Random.Range(destinationMIN, destinationMAX + 1);
        nextCarIn = Random.Range(nextCarMIN, nextCarMAX);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextCarIn)
        {
            Rand();
            timer = 0;
        }
    }
}
