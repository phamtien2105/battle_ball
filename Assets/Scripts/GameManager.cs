using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public Text CountText;
    private float timer;
    public float TimeLimit;
    private int MaxMatches = 5;
    private int countMatch;
    private bool isPLayerAttack;
    public GameObject Ball;
    public GameObject Player;
    public GameObject Enermy;

    public static List<GameObject> listPlayer;
    public static List<GameObject> listEnermy;

    public GameObject PlayerManager;
    public GameObject AttackerManager;

    private bool isBallCreate;
    private bool isKeyPressed;
    // Start is called before the first frame update

    public  static int playerScore;

    public  static int enermyScore;

    public static bool isBallHItGate;

    public static bool NeedResetGame;
    
    void Start()
    {
        isPLayerAttack = true;
        playerScore = enermyScore = 0;
        CountText.text = "0s";
        timer = 0;
        listPlayer = new List<GameObject>();
        listEnermy = new List<GameObject>();
        resetGame();
        TimeLimit = 140;
    }

    // Update is called once per frame
    void Update()
    {

        // update timer text
        timer += Time.deltaTime;
        CountText.text = (int) timer + "s";

        //
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            int layer_mask = LayerMask.GetMask("playerLayer");
            int enermy_mask = LayerMask.GetMask("enermyLayer");

            if (Physics.Raycast(ray, out hit, 1000f, enermy_mask))
            {

                float attackerCost = Enermy.GetComponent<StateManager>().CostEnergy;

                if (AttackerManager.GetComponent<EnergyController>().getCurrentEngery() >= attackerCost)
                {
                AttackerManager.GetComponent<EnergyController>().subEnergy(attackerCost);
                GameObject attacker =
                    Instantiate(Enermy, new Vector3(hit.point.x, 2f, hit.point.z), Quaternion.identity);
                listEnermy.Add(attacker);
                }

            }
            else if (Physics.Raycast(ray, out hit, 1000f, layer_mask))
            {
                float playerCost = Player.GetComponent<StateManager>().CostEnergy;

                if (PlayerManager.GetComponent<EnergyController>().getCurrentEngery() >= playerCost)
                {
                PlayerManager.GetComponent<EnergyController>().subEnergy(playerCost);
                GameObject player = Instantiate(Player, new Vector3(hit.point.x, 2f, hit.point.z), Quaternion.identity);
                listPlayer.Add(player);
                }
            }

        }

        if (countMatch > 6)
        {
            if (GameManager.playerScore < GameManager.enermyScore)
            {
                //game over
                Debug.Log("game over");
                Time.timeScale = 0;
            }
            else if (GameManager.playerScore > GameManager.enermyScore)
            {
                //player win
                Debug.Log("player win");
                Time.timeScale = 0;
            }
            else
            {
                //player win
                Debug.Log("Penalty game");
                Time.timeScale = 0;
            }
        }

        // time out
        if (timer > TimeLimit)
        {
            if (!isBallHItGate)
            {
                //game draw
                Debug.Log("game draw");
                Time.timeScale = 0;
            }
        }

        if (NeedResetGame)
        {
            resetGame();
            NeedResetGame = false;
        }
    }


    private void genertateBall()
    {

        GameObject ball;
        if (isPLayerAttack)
        {

            ball = Instantiate(Ball, new Vector3(0, 2f, 0), Quaternion.identity);

        }
        else
        {
            ball = Instantiate(Ball, new Vector3(0, 2f, 0), Quaternion.identity);
        }

        isBallCreate = true;

    }


    private void resetGame()
    {
        foreach (var item in listPlayer)
        {
            item.gameObject.SetActive(false);
        }
        
        foreach (var item in listEnermy)
        {
            item.gameObject.SetActive(false);
        }
        GameManager.listPlayer.Clear();
        GameManager.listEnermy.Clear();
        //clean energy
        PlayerManager.GetComponent<EnergyController>().Reset();
        PlayerManager.GetComponent<EnergyController>().Reset();
        Destroy(Ball); 
        genertateBall();
        countMatch++;
        if (countMatch == 1)
        {
            isPLayerAttack = true;
        }
        else
        {
            isPLayerAttack = !isPLayerAttack;
        }

    }

}
