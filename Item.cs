using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    float speed = 1.0f;
    float rotSpeed = 45.0f;
    float height = 2.0f;
    bool posCheck;
    Vector3 pos;

    private void Awake()
    {

    }

    void Start()
    {
        posCheck = false;
        pos = transform.position;
        pos.x += (float)Random.Range(-3, 3);
        pos.y += (float)Random.Range(-10, 10);
        pos.z += (float)Random.Range(-1.5f, 1.5f);
    }

    
    void Update()
    {
        if (posCheck)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * speed) * height, transform.position.z);
            transform.Rotate(new Vector3(transform.rotation.x, rotSpeed * Time.deltaTime, transform.rotation.z));
        }
        else
        {
            transform.position = pos;
            posCheck = true;
        }
    }
}
