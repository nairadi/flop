using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
    public GameObject panel;
    public CoreGame coreGame;

    public TextMeshProUGUI levelText;

    public void TogglePanel() {
        levelText.text = "Level " + (coreGame.currentLevel + 1);
        panel.SetActive(!panel.activeInHierarchy);
        coreGame.launcher.canLaunch = !panel.activeInHierarchy; // no shooting while panel is open
    }

    public void OnLeftLevelButton() {
        coreGame.GoBackLevel();
        levelText.text = "Level " + (coreGame.currentLevel + 1);
    }

    public void OnRightLevelButton() {
        coreGame.AdvanceLevel();
        levelText.text = "Level " + (coreGame.currentLevel + 1);
    }
}
