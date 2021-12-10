using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class CoreGame : MonoBehaviour {
    public int LEVEL_CAP;
    public int currentLevel;
    private readonly string SAVE_CURRENT_LEVEL = "saveCurrentLevel";

    [SerializeField] private GameObject fishPrefab;
    [SerializeField] public Launcher launcher;
    [SerializeField] private SmoothFollow smoothFollow;
	[SerializeField] private Button advanceStageButton;
	public GameObject restartButton;
    public GameObject levelSelectButton;
    public GameObject levelSelectPanel;

    public Text levelCompleteText;

	private bool advanceButtonClicked;
    private Level level;
    public Fish fish;
	public static bool restartOnGroundTouch;

    private Collider2D goalCollider;
    private Collider2D goalTrigger;
	private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        currentLevel = PlayerPrefs.GetInt(SAVE_CURRENT_LEVEL, 0);
        CreateLevel(currentLevel);
    }

    private void OnDestroy() {
        PlayerPrefs.SetInt(SAVE_CURRENT_LEVEL, currentLevel);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) || restartOnGroundTouch) {
			restartOnGroundTouch = false;
            SpawnFish();
        }

        // Check if goal reached
        CheckGoalReached();
    }

    public void CreateLevel(int levelID) {
        // Delete previous level
        if (level != null) {
            Destroy(level.gameObject);
        }

        // Load level
        GameObject levelPrefab = Resources.Load("Prefabs/Levels/Level" + levelID) as GameObject;
        GameObject levelGO = Instantiate(levelPrefab);
        level = levelGO.GetComponent<Level>();
        goalCollider = level.goalCollider;
        smoothFollow.bounds = level.cameraBounds.bounds;
        if (!levelSelectPanel.activeInHierarchy) {
            launcher.canLaunch = true;
        }

        // Spawn fish and link to camera
        SpawnFish();
    }

    public void SpawnFish(bool deleteOldFish = true) {
        if (fish != null && deleteOldFish) {
            Destroy(fish.gameObject);
        }

        GameObject fishGO = Instantiate(fishPrefab, level.spawnPosition.position, Quaternion.identity, level.transform);
        fish = fishGO.GetComponent<Fish>();

        // Hook up fish
        launcher.fishRigidbody = fish.mainRigidbody2D;
        smoothFollow.target = fish.mainRigidbody2D.transform;
        goalTrigger = fish.goalTrigger;
    }

	private void WinScreen()
	{
		//play victory audio, zoom camera on fish, show the advance stage button
		audioSource.Play();
		smoothFollow.CenterCamera();
		advanceStageButton.gameObject.SetActive(true);
		restartButton.SetActive(false);
        levelSelectButton.SetActive(false);
        levelSelectPanel.SetActive(false);
        fish.Smile();
        launcher.canLaunch = false;
        levelCompleteText.text = "Level " + (currentLevel + 1) + " Complete!";
    }

	public void AdvanceLevel()
	{
		// Clean up current level
		Destroy(level.gameObject);

		// Increment current level
		currentLevel++;

		// Load next level (TODO: handle no more levels left)
		if (currentLevel > LEVEL_CAP) {
			currentLevel = 0;
		}
		CreateLevel(currentLevel);
		 
		//go back to normal camera and hide advance stage button
		advanceStageButton.gameObject.SetActive(false);
		restartButton.SetActive(true);
        levelSelectButton.SetActive(true);
        smoothFollow.ResetCamera();
	}

    public void GoBackLevel() {
        // Clean up current level
        Destroy(level.gameObject);

        // Increment current level
        currentLevel--;

        // Load next level (TODO: handle no more levels left)
        if (currentLevel < 0) {
            currentLevel = LEVEL_CAP;
        }
        CreateLevel(currentLevel);

        //go back to normal camera and hide advance stage button
        advanceStageButton.gameObject.SetActive(false);
        restartButton.SetActive(true);
        levelSelectButton.SetActive(true);
        smoothFollow.ResetCamera();
    }

    private void CheckGoalReached() {
		if (goalTrigger != null && goalCollider != null && goalTrigger.IsTouching(goalCollider) && !advanceStageButton.gameObject.activeInHierarchy) {
			WinScreen();
        }
	}
}
