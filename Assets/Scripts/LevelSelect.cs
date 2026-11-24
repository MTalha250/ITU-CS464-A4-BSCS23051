using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The main menu's level buttons, built from SaveData: locked until unlocked, with the best time on them.
public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button[] levelButtons;      // Level 1 first; level n is build index n
    [SerializeField] LevelLoader loader;
    [SerializeField] Color lockedLabel = Color.gray;   // a dim label on the dark locked button

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i + 1;
            Button button = levelButtons[i];
            bool open = level <= SaveData.Unlocked;
            button.interactable = open;
            TMP_Text label = button.GetComponentInChildren<TMP_Text>();
            if (!open)
            {
                label.color = lockedLabel;
                label.SetText("Level {0}\n<size=70%>LOCKED</size>", level);
            }
            else if (SaveData.HasBest(level))
                label.SetText("Level {0}\n<size=70%>Best {1:1} s</size>", level, SaveData.BestTime(level));
            else
                label.SetText("Level {0}\n<size=70%>Not played</size>", level);
            button.onClick.AddListener(() => loader.Load(level));
        }
    }
}
