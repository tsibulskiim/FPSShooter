using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static int winCount;
    public static int loseCount;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        scoreText.text = "";

        LoadGame();

        if (PlayerManager.win)
        {
            scoreText.text = $"You win!!!\nScore: {PlayerManager.score}";
            winCount++;
        }
        else if (PlayerManager.lose)
        {
            scoreText.text = $"You lose\nScore: {PlayerManager.score}";
            loseCount++;
        }

        scoreText.text += $"\nWins: {winCount}\nDefeats: {loseCount}";
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();

        using (FileStream file = File.Create(Application.persistentDataPath + "/settings.dat"))
        {
            var settins = new Dictionary<string, string>
            {
                { "winCount", winCount.ToString() },
                { "loseCount", loseCount.ToString() }
            };
            bf.Serialize(file, settins);
        }

        Debug.Log("Game data saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.dat"))
        {
            BinaryFormatter bf = new();
            Dictionary<string, string> settings;

            using (FileStream file = File.Open(Application.persistentDataPath + "/settings.dat", FileMode.Open))
            {
                settings = bf.Deserialize(file) as Dictionary<string, string>;
            }

            int.TryParse(settings["winCount"], out int winCount);
            int.TryParse(settings["loseCount"], out int loseCount);

            Menu.winCount = winCount;
            Menu.loseCount = loseCount;


            Debug.Log("Game data loaded!");
        }
        else
            Debug.Log("There is no save data!");
    }

}
