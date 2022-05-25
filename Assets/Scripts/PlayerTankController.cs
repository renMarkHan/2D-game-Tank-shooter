using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
    private Vector3 velocity;
    private SpriteRenderer rend;
    public float speed = 30.0f;
    public GameObject bullet;
    public GameController gameController;
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
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        //get the width of the object
        float width = rend.bounds.size.x;
        float height = rend.bounds.size.y;
        gameOver = gameController.gameover();

        if (gameOver)
        {
            velocity = new Vector3(0f, 0f, 0f);
        }
        else
        {
            //shooting the bullet from the player
            if (Input.GetButtonDown("Fire1") && canFire)
            {
                //the offset 
                Vector3 offset = new Vector3(0f, 2f, 0f);

                //create a bullet pointing in its natural direction 
                GameObject b = Instantiate(bullet, new Vector3(0f, -4f, 0f), Quaternion.identity);

                b.GetComponent<BulletController>().InitPosition(transform.position + offset, new Vector3(0f, 2f, 0f));
                canFire = false;

                //this starts a coroutine... a non-blocking function
                StartCoroutine(PlayerCanFireAgain());
            }


            velocity = new Vector3(0f, -Input.GetAxis("Horizontal") * speed, 0f);


            //make sure the obect is inside the borders... if edge is hit reverse direction
            if ((transform.position.x <= leftBorder + width / 2.0) && velocity.y > 0f)
            {
                velocity = new Vector3(0f, 0f, 0f);
            }
            if ((transform.position.x >= rightBorder - width / 2.0) && velocity.y < 0f)
            {
                velocity = new Vector3(0f, 0f, 0f);
            }
        }
        
        transform.Translate(velocity*Time.deltaTime);
    }

    //will wait 3 seconds and then will reset the flag
    IEnumerator PlayerCanFireAgain()
    {
        //this will pause the execution of this method for 3 seconds without blocking
        yield return new WaitForSecondsRealtime(3);
        canFire = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameController.youLose())
        {
            Destroy(gameObject);
        }
       
    }
}
