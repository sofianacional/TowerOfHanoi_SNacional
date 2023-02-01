using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[Header("Menu Panel")]
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private GameObject diskSelectionPanel;
	
	[SerializeField] private Button playButton;
	
	private int movesInputValue = 3;
	[SerializeField] private int movesInputText;
	[SerializeField] private Button leftArrowButton;
	[SerializeField] private Button rightArrowButton;
	
	[Header("HUD")]
	[SerializeField] private TextMeshProUGUI playerMovesText;

	private GameManager gameManager;
	
	private void Start() {
		gameManager = SingletonManager.Get<GameManager>();
		
		gameManager.Evt_OnAddMove.AddListener(OnAddMove);
		
		//startButton.onClick.AddListener(gameManager.ini);
	}
	
	public void OnClickLeftArrowButton() {
		
	}
	
	public void OnClickRightArrowButton() {
		
	}
	
	private void OnAddMove(int value) {
		playerMovesText.text = "Moves: " + value;
	}
}
