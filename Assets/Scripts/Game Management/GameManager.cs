using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int pickupCount = 0;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI winText;
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private void Awake() //Awake runs before start, this makes sure that the game properly loads the game manager before anything tries to mess with it
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddPickup()
    {
        pickupCount++;
        scoreText.text = "Score: " + pickupCount;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pickupCount >= 10)
        {
            winText = GameObject.Find("Win Text").GetComponent<TextMeshProUGUI>();
            winText.text = "YOU WIN!";
        }

    }
}
