using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    public float addSpeed = 0.25f;
    public float width = 1.5f;
    public float height = 8.5f;
    public int score = 0;
    public int round = 0;
    public int defense = 1;
    public int fastSkip = 1;
    public int maxRound = 10;
    public int scoreRound = 5;
    public int[] countRound;
    public bool shield;
    public float interval = 3.0f;
    float term;
    bool pause;
    bool reverse;
    new Camera camera;
    new Rigidbody rigidbody;
    Material material;
    TextMesh scoreOutput;

    private void Awake()
    {

    }

    void Start()
    {
        shield = false;
        countRound = new int[maxRound];
        GetComponent<MeshRenderer>().materials[0].color = Color.white;
        for (int i = 0; i < maxRound; ++i)
        {
            countRound[i] = i * scoreRound;
            //Debug.Log("CountRound[" + i + "] :" + countRound[i]);
        }
        term = 0;
        pause = false;
        reverse = false;
        camera = GetComponentInChildren<Camera>();
        rigidbody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        scoreOutput = GameObject.Find(name: "Score").GetComponent<TextMesh>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (pause)
            {
                Time.timeScale = 1f;
                pause = false;
                return;
            }
            if(!pause)
            {
                Time.timeScale = 0f;
                pause = true;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || (Input.GetKeyDown(KeyCode.RightControl)) && defense > 0 && !shield)
        {
            if (shield)
            {
                return;
            }
            shield = true;
            defense -= 1;
            camera.transform.position = new Vector3(camera.transform.position.x - 1.75f, camera.transform.position.y + 0.5f, camera.transform.position.z);
            material.color = Color.black;
            AddScoreboard(score, round, defense, fastSkip);
            //Debug.Log("Shield : On");
        }
        else
        {
            if (shield)
            {
                term += Time.deltaTime;
                if (term > interval)
                {
                    shield = false;
                    camera.transform.position = new Vector3(camera.transform.position.x + 1.75f, camera.transform.position.y - 0.5f, camera.transform.position.z);
                    material.color = Color.white;
                    term -= interval;
                    //Debug.Log("Shield : Off");
                }
            }
        }
        if (transform.position.y > height || transform.position.z > width || transform.position.z < -width)
        {
            if (transform.position.y > height)
            {
                //Debug.Log("Height");
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
            }
            if (transform.position.z > width)
            {
                //Debug.Log("width");
                transform.position = new Vector3(transform.position.x, transform.position.y, width);
            }
            if (transform.position.z < -width)
            {
                //Debug.Log("-Width");
                transform.position = new Vector3(transform.position.x, transform.position.y, -width);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rigidbody.velocity = new Vector3(0, speed, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rigidbody.velocity = new Vector3(0, 0, speed);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rigidbody.velocity = new Vector3(0, 0, -speed);
        }

        //Debug.Log(transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Enter");
        if (shield && material.color != Color.red)
        {
            material.color = Color.red;
            Destroy(collision.gameObject);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Trigger Enter");
        if (collider.gameObject.CompareTag("Shield"))
        {
            //Debug.Log("Get Shield");
            Destroy(collider.gameObject);
            defense += 1;
            AddScoreboard(score, round, defense, fastSkip);
        }
        if (collider.gameObject.CompareTag("Skip"))
        {
            //Debug.Log("Get Skip");
            Destroy(collider.gameObject);
            fastSkip += 1;
            AddScoreboard(score, round, defense, fastSkip);
        }
        if (collider.gameObject.CompareTag("Reverse"))
        {
            //Debug.Log("Get Reverse");
            if (!reverse)
            {
                Destroy(collider.gameObject);
                camera.transform.Rotate(transform.position.x, transform.position.y, 180.0f);
                scoreOutput.transform.Rotate(transform.position.x, transform.position.y, 180.0f);
                Physics.gravity = new Vector3(0, 9.81f, 0);
                reverse = true;
            }
            if (reverse)
            {
                Destroy(collider.gameObject);
                camera.transform.Rotate(transform.position.x, transform.position.y, 0);
                scoreOutput.transform.Rotate(transform.position.x, transform.position.y, 0);
                Physics.gravity = new Vector3(0, -9.81f, 0);
                reverse = false;
            }
            AddScoreboard(score, round, defense, fastSkip);
        }
        if (collider.gameObject.CompareTag("Bomb"))
        {
            //Debug.Log("Get Skip");
            Destroy(collider.gameObject);
            score -= 2;
            AddScoreboard(score, round, defense, fastSkip);
        }
    }

    public void AddScoreboard(int s, int r, int d, int f)
    {
        int score = s;
        int round = r;
        int defense = d;
        int fastSkip = f;

        scoreOutput.text = "Score : " + score + ", " + "Round : " + round + ", " + "Shield : " + defense + ", " + "Skip : " + fastSkip;
    }
}
