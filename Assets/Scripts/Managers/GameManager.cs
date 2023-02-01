using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	[SerializeField] private int numberOfDisks; // Selected ON START
	[SerializeField] private List<Disk> DiskPrefabs; // Highest disk = index 0
	[SerializeField] private List<Disk> spawnedDisks;
	
	[Header("Player")]
	[SerializeField] private PlayerInput playerInput;
	[SerializeField] private int numberOfMoves;

	[Header("Auto Mode")]
	[SerializeField] private float moveTime;
	[SerializeField] private bool isAutoSolving = false;

	private Coroutine autoPlaceCoroutine;
	private Coroutine stackingIntervalCoroutine;
	private Coroutine diskMovementCoroutine;

	public List<Pole> Poles;
	
	public UnityEvent<int> Evt_OnModifyMove = new();
	public UnityEvent<int> Evt_GameOver = new();

	private void Start() {
		SingletonManager.Register(this);
	}

	public void InitializeGame(int value) {
		playerInput.CanInteract = true;
		numberOfDisks = value;
		
		if (spawnedDisks.Count > 0) ResetGame();
		else SpawnDisks();
	}

	private void SpawnDisks() {
		for (int i = 0; i < numberOfDisks; i++) {
			Disk newDisk = Instantiate(DiskPrefabs[i]);
			Poles[0].AttachDiskToPole(newDisk);
			spawnedDisks.Add(newDisk);
		}
		numberOfMoves = 0;
		ModifyMove(numberOfMoves);
		
		foreach (var p in Poles) {
			p.Evt_PoleModified.AddListener(AddMove);
		}
	}

	private void AddMove() {
		numberOfMoves++;
		ModifyMove(numberOfMoves);
		CheckPoleContents();
	}

	private void ModifyMove(int value) {
		Evt_OnModifyMove.Invoke(value);
	}
	
	private void CheckPoleContents() {
		int count = 0;
		foreach (var n in Poles[2].Nodes) {
			if (!n.CurrentDisk) continue;
			count++;
			
			if (count >= numberOfDisks) {
				StartCoroutine(GameOverDelay());
			}
		}
	}
	
	private void StopGame() {
		if (isAutoSolving) StopAutoSolve();
		
		foreach (var p in Poles) {
			foreach (var n in p.Nodes) {
				n.ClearNode();
			}
			p.InitializePole();
			p.Evt_PoleModified.RemoveAllListeners();
		}
		
		foreach (var d in spawnedDisks) {
			Destroy(d.gameObject);
		}
		spawnedDisks.Clear();
	}
	
	public void ResetGame() {
		StopGame();
		SpawnDisks();
	}

	private IEnumerator GameOverDelay() {
		yield return new WaitForSeconds(1f);
		StopGame();
		Evt_GameOver.Invoke(numberOfMoves);
	}
	
	#region Auto Solve Feature

		public void StartAutoSolve() {
    		if(isAutoSolving) return;
    		playerInput.CanInteract = false;
    		autoPlaceCoroutine = StartCoroutine(StartAutoPlace());
    		isAutoSolving = true;
    	}
    
    	private IEnumerator StartAutoPlace() {
    		stackingIntervalCoroutine = StartCoroutine(StackingInterval(numberOfDisks, Poles[0], Poles[1], Poles[2]));
    		
    		yield return stackingIntervalCoroutine;
    		
    		ClearCoroutines();
    	}
    	
    	private IEnumerator StackingInterval(int n, Pole start, Pole middle, Pole end) {
    		if (n <= 0) yield break;
    
    		yield return stackingIntervalCoroutine = StartCoroutine(StackingInterval(n-1, start, end, middle));
    		
    		// Moving of Disk
    		Disk disk = start.GetTopMostFilledNode().CurrentDisk;
    		Node endPoleNode = end.GetFirstEmptyNode();
    		
    		disk.Evt_OnDiskMoved.Invoke(disk);
    		end.OnAddDisk(disk);
    		print(numberOfMoves);
    		diskMovementCoroutine = StartCoroutine(DiskMovement(disk, endPoleNode.transform.position));
    		yield return diskMovementCoroutine;
    		
    		yield return stackingIntervalCoroutine = StartCoroutine(StackingInterval(n-1, middle, start, end));
    	}
    	
    	private IEnumerator DiskMovement(Disk disk, Vector3 endPos) {
    		float elapsedTime = 0;
    
    		while (elapsedTime < moveTime) {
    			disk.transform.position = Vector2.Lerp(disk.transform.position, endPos, elapsedTime / moveTime);
    			elapsedTime += Time.deltaTime;
    
    			yield return null;
    		}
    	}
    
    	private void StopAutoSolve() {
    		if (autoPlaceCoroutine != null) StopCoroutine(autoPlaceCoroutine);
    		if (stackingIntervalCoroutine != null) StopCoroutine(stackingIntervalCoroutine);
    		if (diskMovementCoroutine != null) StopCoroutine(diskMovementCoroutine);
    
    		ClearCoroutines();
    	}
    	
    	private void ClearCoroutines() {
    		playerInput.CanInteract = true;
    		isAutoSolving = false;
    		autoPlaceCoroutine = null;
    		diskMovementCoroutine = null;
    		stackingIntervalCoroutine = null;
    	}

	#endregion
	
}
