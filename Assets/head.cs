using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class head : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;

    private Vector3 currentPosition;
    private Vector3 nextPosition;
    public Vector3 direction = Vector3.right; // Initial direction
    
    public float time;
    public float timeSinceStart;
    
    [SerializeField]  GameObject food;
    [SerializeField]  GameObject body;
    [SerializeField]  Text scoreText;
    [SerializeField]  GameObject replayButton;
    
    public int score = 0;
    
    void Start()
    {
        currentPosition = transform.position;
        nextPosition = currentPosition + 0.5f*direction;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeSinceStart += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector3.right;
        }
        
        //update currentPosition and nextPosition when pass 1s
        if (time> 0.5f)
        {
            time -= 0.5f;
            currentPosition = nextPosition;
            nextPosition = currentPosition + speed*direction;
        }
        
        // update position
        transform.position = Vector3.Lerp(currentPosition, nextPosition, time);
        // debug for moving not fluent
        // Debug.Log(timeSinceStart);
        // Debug.Log(time);
        // Debug.Log(transform.position);
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("hit something");
        //debug for case when touching
        if (other.gameObject.tag == "food")
        {
            Debug.Log("eat food");
            Destroy(other.gameObject);
            GameObject newFood = Instantiate(food);
            int x = Random.Range(0, 12);
            int y = Random.Range(0, 12);
            newFood.transform.position = new Vector3(0.5f * x - 3, 0.5f * y - 3, 0);
            score += 1; 
            scoreText.text = "Score: " + score.ToString();
        }
        else if (other.gameObject.tag == "obstacle")
        {
            Debug.Log("hit obstacle");
            Time.timeScale = 0f;
            replayButton.SetActive(true);
        }
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
    
}
