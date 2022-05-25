using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTankController : MonoBehaviour
{
    private Vector3 velocity;
    private SpriteRenderer rend;
    public float speed = 5.0f;
    public GameObject bullet;
    public GameController gameController;
    private int nextUpdate = 1;
    private bool canFire;
    private bool gameOver;


    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        velocity = new Vector3(0f, 0f, 0f);
        rend = GetComponent<SpriteRenderer>();
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;


        //get the width of the object
        float width = rend.bounds.size.x;

        gameOver = gameController.gameover();

        if (gameOver)
        {
            velocity = new Vector3(0f, 0f, 0f);
        }
        else
        {
            //shooting the bullet from the player
            if (canFire)
            {
                //the offset 
                Vector3 offset = new Vector3(0f, -2f, 0f);

                //creates a bullet that is pointing in the opposite direction... pick the one you need  
                GameObject b = Instantiate(bullet, new Vector3(0f, 3f, 0f), Quaternion.AngleAxis(180, Vector2.left));

                b.GetComponent<BulletController>().InitPosition(transform.position + offset, new Vector3(0f, 2f, 0f));
                canFire = false;

                //this starts a coroutine... a non-blocking function
                StartCoroutine(PlayerCanFireAgain());
            }

            //2% of the time, switch the direction: 
            int change = Random.Range(0, 100);
            if (change == 0 || change == 1)
            {

                velocity = new Vector3(0, -velocity.y, 0);

            }

            else if (change > 1 && change < 50)
            {
                if (Time.time >= nextUpdate)
                {
                    velocity = new Vector3(0f, -3f, 0f);
                    nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                }

            }

            else if (change >= 50 && change <= 100)
            {
                if (Time.time >= nextUpdate)
                {
                    velocity = new Vector3(0f, 3f, 0f);
                    nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                }

            }



            //make sure the obect is inside the borders... if edge is hit reverse direction
            if ((transform.position.x <= leftBorder + width / 2.0) && velocity.y > 0f)
            {
                velocity = new Vector3(0f, -velocity.y, 0f);
            }
            if ((transform.position.x >= rightBorder - width / 2.0) && velocity.y < 0f)
            {
                velocity = new Vector3(0f, -velocity.y, 0f);
            }
        }
        


        transform.Translate((velocity * Time.deltaTime));

    }


    IEnumerator PlayerCanFireAgain()
    {
        //this will pause the execution of this method for 3 seconds without blocking
        yield return new WaitForSecondsRealtime(3);
        canFire = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameController.youWin())
        {
            Destroy(gameObject);
        }

    }
}
