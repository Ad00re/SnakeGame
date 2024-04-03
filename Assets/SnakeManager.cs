using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SnakeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;
    
    public Vector2Int food;
    public List<Vector2Int> snake;
    
    public List<Vector2Int> obstacles;
    
    
    public Vector3 foodPosition;
    public List<GameObject> snakeDisplay;

    public Vector2Int direction = new Vector2Int(1, 0);
    public Vector2Int tempDirection = new Vector2Int(1,0);
    
    
    
    public float time;
    

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject foodObject;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject replayButton;
    
    public int score;
    
    void Start()
    {
        time = 0f;
        score = 0;
        food = new Vector2Int(4, 0);
        snake = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(0, 0)
        };
        snakeDisplay = new List<GameObject>
        {
            head,
            Instantiate(bodyPrefab)
        };
        //create obstacles
        obstacles = new List<Vector2Int>();
        int[] values = { -1, 1 };
        foreach (int i in values)
        {
            foreach (int j in values)
            {
                obstacles.Add(new Vector2Int(7 * i, 7 * j));
                obstacles.Add(new Vector2Int(7 * i, 6 * j));
                obstacles.Add(new Vector2Int(6 * i, 7 * j));
            }
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tempDirection = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tempDirection = new Vector2Int(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempDirection = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempDirection = new Vector2Int(1, 0);
        }
        
        if (time*speed> 1f)
        {
            time -= 1f/speed;
            Vector2Int newHead = snake[0] + direction;
            snake.RemoveAt(0);
            snake.Insert(0, newHead);
            snake.Insert(0, newHead);
            snake.RemoveAt(snake.Count-1);
            direction = tempDirection;
        }
        //view
        foodPosition = 0.5f*new Vector3(food.x, food.y, 0);
        foodObject.transform.position = foodPosition;
        //head move depend on direction 
        Vector2 location =0.5f * Vector2.Lerp(snake[0], snake[0]+direction, time * speed);
        snakeDisplay[0].gameObject.transform.position = new Vector3(location.x, location.y, 0);
        
        if (snake.Count > 2)
        {
            // snake tail 
            location =0.5f * Vector2.Lerp(snake[^1], snake[^2], time * speed);
            snakeDisplay[^1].gameObject.transform.position = new Vector3(location.x, location.y, 0);
            // body after head
            location = 0.5f * Vector2.Lerp(snake[1], snake[0], time * speed);
            snakeDisplay[1].gameObject.transform.position = new Vector3(location.x, location.y, 0);
            // all other body move only at time 0
            for(int i = 2;i<snake.Count-1;i++)
            {
                location = 0.5f * Vector2.Lerp(snake[i], snake[i - 1], 0);
                snakeDisplay[i].gameObject.transform.position = new Vector3(location.x, location.y, 0);
            }
        }
        else
        {
            snakeDisplay[1].gameObject.transform.position = snakeDisplay[0].gameObject.transform.position;
        }
        
        
        //check for eat food
        if (snake[0] + direction == food)
        {
            //create a new body
            snake.Insert(0,snake[0]);
            snakeDisplay.Add(Instantiate(bodyPrefab));
            score += 1; 
            scoreText.text = "Score: " + score;
            do
            { 
                food = new Vector2Int(Random.Range(-8, 8), Random.Range(-8, 8));
            } while (obstacles.Contains(food) || snake.Contains(food));
            
            
        }
        // check for collide in to obstacle
        foreach(Vector2 obstacle in obstacles)
        {
            if (snake[0] + direction == obstacle)
            {
                Die();
            }
        }
        // check for collide to wall
        if (Math.Abs((snake[0] + direction).x)==9 || Math.Abs((snake[0] + direction).y)==9)
        {
            Die();
        }
        // check for collide to it self
        for(int i = 1;i<snake.Count;i++)
        {
            if (snake[0] + direction == snake[i])
            {
                Die();
            }
        }
    }


    void Die()
    {
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
        Start();
    }
    
}
