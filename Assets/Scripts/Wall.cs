using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    float height = 6.0f;
    float speed = 0.5f;
    float rotSpeed = 45.0f;
    bool findRound;
    int rand;
    Player player;
    Spawner spawner;
    Vector3 pos;

    private void Awake()
    {

    }

    void Start()
    {
        rand = UnityEngine.Random.Range(0, 2);
        player = GameObject.Find(name: "Player").GetComponent<Player>();
        spawner = GameObject.Find(name: "Spawner").GetComponent<Spawner>();
        pos = transform.position;
        // Debug.Log("Random Num : " + rand);
    }

    void Update()
    {
        if ((gameObject.CompareTag("Reverse") && player.round < 9))
        {
            Destroy(gameObject);
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && player.fastSkip > 0)
        {
            Destroy(gameObject);
            player.fastSkip -= 1;
            player.AddScoreboard(player.score, player.round, player.defense, player.fastSkip);
        }
        pos.x += spawner.speed * Time.deltaTime;
        if (new List<int> { 2, 3, 4 }.Contains(player.round) || player.round > 7)
        {
            if (rand == 0)
            {
                if (pos.y < height || pos.y < -height)
                {
                    pos.y += Time.deltaTime * speed * height;
                }
                else if (pos.y > -height || pos.y > height)
                {
                    pos.y -= Time.deltaTime * speed * height;
                }
            }
            else if (rand == 1)
            {
                if (pos.y > -height || pos.y > height)
                {
                    pos.y -= Time.deltaTime * speed * height;
                }
                else if (pos.y < height || pos.y < -height)
                {
                    pos.y += Time.deltaTime * speed * height;
                }
            }
        }
        if (new List<int> { 5, 6, 7 }.Contains(player.round) || player.round > 7)
        {
            if (rand == 0)
            {
                transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
            }
            else if (rand == 1)
            {
                transform.Rotate(new Vector3(0, -rotSpeed * Time.deltaTime, 0));
            }
        }

        if (pos.x < -6.0f)
        {
            player.score += 1;
            Destroy(gameObject);
            findRound = Array.Exists(player.countRound, i => i == player.score);
            if (findRound)
            {
                if (player.round > 2)
                {
                    spawner.speed += spawner.addSpeed;
                    speed += -spawner.addSpeed * 0.33f;
                    player.speed += player.addSpeed;
                }
                // Debug.Log("Spawner Speed : " + spawner.speed + " / " + speed + ", " + "Player Speed : " + player.speed);
                player.round += 1;
                player.defense += 1;
            }
            player.AddScoreboard(player.score, player.round, player.defense, player.fastSkip);
        }
        transform.position = pos;
    }
}
