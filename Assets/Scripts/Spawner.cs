using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float interval = 1.5f;
    public float lowInterval = 0.1f;
    public float range = 3.0f;
    public float speed = -4.5f;
    public float addSpeed = -0.25f;
    float term;
    int lastRound;
    public GameObject[] wallPrefabs;
    Player player;

    private void Awake()
    {

    }

    void Start()
    {
        term = interval;
        lastRound = 0;
        player = GameObject.Find(name: "Player").GetComponent<Player>();
    }

    void Update()
    {
        if (interval < 0.1f)
        {
            interval = 0.1f;
        }
        else
        {
            if (lastRound < player.round)
            {
                lastRound = player.round;
                interval -= lowInterval;
                // Debug.Log("Interval : " + interval);
            }
        }

        term += Time.deltaTime;
        if(term > interval)
        {
            Vector3 pos = transform.position;
            pos.y += Random.Range(-range, range);
            Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], pos, transform.rotation);
            term -= interval;
        }
    }
}
