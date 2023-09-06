using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public static int playerHP;
    public TextMeshProUGUI playerHPText;
    public static bool win;
    public static bool lose;
    public static int score;

    void Start()
    {
        win = false;
        lose = false;
        playerHP = 100;
        score = 0;
    }

    void Update()
    {
        playerHPText.text = $"+{playerHP}";

        if (FindObjectsOfType<Enemy>().All(enemy => enemy.enemyHP <= 0))
            win = true;

        if (win || lose)
            SceneManager.LoadScene("Menu");
    }

    public static void TakeDamage(int damageAmount)
    {
        playerHP -= damageAmount;
        if (playerHP <= 0)
            lose = true;

    }
}
