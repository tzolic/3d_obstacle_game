using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour{

    public static bool gameOver;
    public GameObject gameOverPanel;
    public static bool isGameStarted;
    public GameObject startingText;
    public static int numberOfCoins;
    [SerializeField] private TMP_Text coinsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update(){
        if(gameOver){
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        coinsText.text = "Coins: " + numberOfCoins;

        if(SwipeManager.tap){
            isGameStarted = true;
            Destroy(startingText);
        }
    }
}
