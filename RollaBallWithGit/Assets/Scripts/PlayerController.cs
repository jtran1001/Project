using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody rb;
    private float movementX;
    private float movementZ;
    //This is for the pick up count
    private int count;
    //count text
    public TextMeshProUGUI countText;
    //win text
    public GameObject winTextObject;
    //countDown text
    public TextMeshProUGUI countDown;
    private int CountDown;
    //lose text
    public GameObject loseTextObject;
    //welcome Text
    public GameObject welcomeText;

    private int timeSinceLoad;

    private bool gameStart = false;
    // Start is called before the first frame update
    void Start()
    {
        //accept value passed in from the Inspector of RigidBody Component
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.gameObject.SetActive(false);
        loseTextObject.gameObject.SetActive(false);
        welcomeText.gameObject.SetActive(true);
    }

    void OnMove(InputValue movementValue)
    {
        if (gameStart)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();

            movementX = movementVector.x;
            movementZ = movementVector.y;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    void SetWinText()
    {
        winTextObject.gameObject.SetActive(true);
        gameStart = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameStart = true;
            welcomeText.gameObject.SetActive(false);
            //This line determines when the game start counting down
            //need to find a better place to fit it
            //if placed here, time resets everytime hit S button
            timeSinceLoad = (int)Time.timeSinceLevelLoad;
        }
        if (gameStart)
        {
            SetCountDownText();
        }
        
    }

    void SetCountDownText()
    {
        CountDown = 30 - ((int)Time.timeSinceLevelLoad - timeSinceLoad);
        if (CountDown <= 0)
        {
            CountDown = 0;
        }
        if (CountDown <= 0 && count < 6)
        {
            SetLoseText();
            gameStart = false;
        }
        countDown.text = "Time Left: " + CountDown;
 
    }

    void SetLoseText()
    {
        loseTextObject.gameObject.SetActive(true);
    }


    //physic that needs to be updated per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementZ);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick-Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            if(count >= 6)
            {
                SetWinText();
                gameStart = false;
            }
        }
    }
}
