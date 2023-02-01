using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[Header("Menu Panel")]
	[SerializeField] private GameObject menuPanel;
	
	[Header("Disk Selection Panel")]
	private int movesInputValue = 3;
	private static int minInputValue = 3;
	private static int maxInputValue = 8;
	[SerializeField] private GameObject diskSelectionPanel;
	[SerializeField] private TextMeshProUGUI movesInputText;
	[SerializeField] private Button leftArrowButton;
	[SerializeField] private Button rightArrowButton;
	[SerializeField] private Button playButton;

	[Header("HUD")]
	[SerializeField] private GameObject HUD;
	[SerializeField] private TextMeshProUGUI playerMovesText;
	[SerializeField] private TextMeshProUGUI playerDiskCounterText;

	[Header("Game Over Panel")]
	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private TextMeshProUGUI totalMovesText;
	
	private GameManager gameManager;
	
	private void Start() {
		gameManager = SingletonManager.Get<GameManager>();
		gameManager.Evt_OnModifyMove.AddListener(OnModifyMoves);
		gameManager.Evt_GameOver.AddListener(OnGameOver);
		
		menuPanel.SetActive(true);
		HUD.gameObject.SetActive(false);
	}

	public void SetDiskValue(int value) {
		movesInputValue = value;
		movesInputText.text = value.ToString();
		
		SetupArrowButtons();
	}
	
	public void OnClickLeftArrowButton() {
		if (movesInputValue <= minInputValue) return;
		
		movesInputValue--;
		SetDiskValue(movesInputValue);
		SetupArrowButtons();
	}
	
	public void OnClickRightArrowButton() {
		if (movesInputValue >= maxInputValue) return;
		
		movesInputValue++;
		SetDiskValue(movesInputValue);
		SetupArrowButtons();
	}
	
	private void SetupArrowButtons() {
		if(!diskSelectionPanel.activeSelf) return;
		
    	if (movesInputValue >= minInputValue && movesInputValue <= maxInputValue) {
    		leftArrowButton.interactable = true;
    		rightArrowButton.interactable = true;
    	}
    	if (movesInputValue <= minInputValue) leftArrowButton.interactable = false;
    	if (movesInputValue >= maxInputValue) rightArrowButton.interactable = false;
	}
	
	public void OnClickPlayButton() {
		gameManager.InitializeGame(movesInputValue);
		SetupHUD();
	}
	
	private void OnModifyMoves(int value) {
		playerMovesText.text = "Moves: " + value;
	}

	private void SetupHUD() {
		playerDiskCounterText.text = "Disks: " + movesInputValue;
		OnModifyMoves(0);
	}
	private void OnGameOver(int value) {
		HUD.SetActive(false);
        gameOverPanel.SetActive(true);
        totalMovesText.text = value.ToString();
	}

	public void OnClickExitGame() {
		Application.Quit();
	}
}
