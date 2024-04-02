using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class SnakeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;

    
    [SerializeField] public Vector2 food;
    public List<Vector2> snake;
    
    public Vector3 headPosition;
    public Vector3 foodPosition;
    public List<GameObject> snakeDisplay;
    
    
    
    public Vector2 direction = Vector2.right;
    public Vector2 tempDirection = Vector2.right;
    
    
    public float time;

    [SerializeField]  GameObject head;
    [SerializeField]  GameObject foodObject;
    [SerializeField]  GameObject bodyPrefab;
    [SerializeField]  Text scoreText;
    [SerializeField]  GameObject replayButton;
    
    public int score = 0;
    
    void Start()
    {
        time = 0f;
        
        food = new Vector2(2, 0);
        snake = new List<Vector2>();
        snake.Add(new Vector2(0, 0));
        snakeDisplay = new List<GameObject>();
        snakeDisplay.Add(head);
        
        foodPosition = 0.5f*new Vector3(food.x, food.y, 0);
        foodObject.transform.position = foodPosition;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i<snake.Count;i++)
        {
            snakeDisplay[i].gameObject.transform.position = 0.5f * new Vector3(snake[i].x, snake[i].y, 0);
        }
        
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tempDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tempDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempDirection = Vector2.right;
        }
        foodPosition = 0.5f*new Vector3(food.x, food.y, 0);
        foodObject.transform.position = foodPosition;
        
        if (time*speed> 1)
        {
            time -= 1f/speed;
            direction = tempDirection;
            snake.Insert(0, snake[0] + direction);
            snake.RemoveAt(snake.Count-1);
            
           
            
        }
        //check for eat food
        if (snake[0] + direction == food)
        {
            // //create a new body
            snake.Add(food);
            snakeDisplay.Add(Instantiate(bodyPrefab));
            
            food = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            
            
        }
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("hit something");
        //debug for case when touching
        if (other.gameObject.tag == "food")
        {
            // Debug.Log("eat food");
            // GameObject newBody = Instantiate(body);
            // newBody.transform.position = food.transform.position;
            //
            // int x = Random.Range(0, 12);
            // int y = Random.Range(0, 12);
            // food.transform.position = new Vector3(0.5f * x - 3, 0.5f * y - 3, 0);
            // score += 1; 
            // scoreText.text = "Score: " + score.ToString();
            // //add a body to the previous food location
            
            
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
