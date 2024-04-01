using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class head : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 direction = Vector2.right; // Initial direction
    public Vector2 nextDirection;
    public float speed = 1.0f;

    private Vector2 currentPosition;
    private bool directionQueued = false;
    
    [SerializeField]  GameObject food;
    [SerializeField]  GameObject body;
    [SerializeField]  Text scoreText;
    public int score = 0;
    
    
    
    void Start()
    {
        currentPosition = transform.position; // Assuming the snake starts at an integer position
        nextDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        
        HandleInput();
        Move();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextDirection = Vector2.up;
            directionQueued = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextDirection = Vector2.down;
            directionQueued = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextDirection = Vector2.left;
            directionQueued = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextDirection = Vector2.right;
            directionQueued = true;
        }
        
    }

    void Move()
    {
        currentPosition += speed * direction  * Time.deltaTime;
        transform.position = currentPosition;

        if (directionQueued && IsAtCorrectPosition(currentPosition))
        {
            direction = nextDirection;
            directionQueued = false;
        }
    }

    bool IsAtCorrectPosition(Vector2 position)
    {
        float epsilon = 0.01f; // Adjust as needed for precision
        float modX = Mathf.Abs(position.x % 0.5f);
        float modY = Mathf.Abs(position.y % 0.5f);
        bool isXAligned = modX < epsilon || (0.5f - modX) < epsilon;
        bool isYAligned = modY < epsilon || (0.5f - modY) < epsilon;
        return isXAligned && isYAligned;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("hit something");
        if (other.gameObject.tag == "food")
        {
            Debug.Log("eat food");
            Destroy(other.gameObject);
            GameObject newFood = Instantiate(food);
            int x = Random.Range(0, 16);
            int y = Random.Range(0, 16);
            newFood.transform.position = new Vector3(0.5f * x - 4, 0.5f * y - 4, 0);
            score += 1;
            scoreText.text = "Score: " + score.ToString();
        }
        else if (other.gameObject.tag == "obstacle")
        {
            Debug.Log("hit obstacle");
        }
    }
    
}
