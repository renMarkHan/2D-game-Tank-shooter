using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject bullet;
    public TextMeshProUGUI compTankText;
    public TextMeshProUGUI playerTankText;

    public GameObject loseObject;
    public GameObject winObject;
    

    private bool gameOver;

    private int computerCount,playerCount;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        computerCount = 0;
        playerCount = 0;
        winObject.SetActive(false);
        loseObject.SetActive(false);
        compTankText.text = " ";
        playerTankText.text = " ";
        

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    public bool youWin()
    {
        bool youWin = false;
        compTankText.text = compTankText.text+" X ";
        computerCount++;
        if (computerCount >= 3)
        {
            youWin = true;
            winObject.SetActive(true);
            gameOver = true;
        }
        return youWin;
    }

    public bool youLose()
    {
        bool youLose = false;
        playerTankText.text = playerTankText.text + " X ";
        playerCount++;
        if (playerCount >= 3)
        {
            youLose = true;
            loseObject.SetActive(true);
            gameOver = true;
            
        }
        return youLose;
    }

    public bool gameover()
    {
        return gameOver;
    }

}
