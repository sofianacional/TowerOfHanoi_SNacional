using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

struct MoveOrderData {
	public Disk DiskToMove;
	public Vector2 MovePosition;
}

public class GameManager : MonoBehaviour {

	[SerializeField] private int numberOfDisks; // Selected ON START
	[SerializeField] private List<Disk> DiskPrefabs; // Highest disk = index 0
	[SerializeField] private List<Disk> spawnedDisks;

	public List<Pole> Poles;

	[Header("Player Data")]
	[SerializeField] private int numberOfMoves;

	[Header("Auto Mode")]
	[SerializeField] private float moveTime;
	private List<MoveOrderData> moveOrderDataList;

	private Coroutine autoPlaceCoroutine;

	public UnityEvent<int> Evt_OnAddMove;

	private void Start() {
		SingletonManager.Register(this);
		InitializeGame();
	}

	public void InitializeGame() {
		SpawnDisks();
		foreach (var p in Poles) {
			p.Evt_PoleModified.AddListener(AddMove);
		}
		
		//StartAutoSolve();
	}
	
	public void SpawnDisks() {
		for (int i = 0; i < numberOfDisks; i++) {
			Disk newDisk = Instantiate(DiskPrefabs[i]);
			Poles[0].AttachDiskToPole(newDisk);
			spawnedDisks.Add(newDisk);
		}
	}

	private void AddMove() {
		numberOfMoves++;
		Evt_OnAddMove.Invoke(numberOfMoves);
		CheckPoleContents();
	}

	private void CheckPoleContents() {
		int count = 0;
		foreach (var n in Poles[2].Nodes) {
			if (!n.CurrentDisk) continue;
			count++;
			if (count >= numberOfDisks) {
				// game over
				print("game over");
			}
		}
	}

	public void ResetGame() {
		foreach (var p in Poles) {
			foreach (var n in p.Nodes) {
				n.ClearNode();
			}
		}

		foreach (var d in spawnedDisks) {
			Destroy(d);
		}
		
		spawnedDisks.Clear();
	}

	private void DisableDisks() {
		foreach (var d in spawnedDisks) {
			d.IsInteractable = false;
		}
	}
	
	public void StartAutoSolve() {
		DisableDisks();
		autoPlaceCoroutine = StartCoroutine(MoveDisk(numberOfDisks, Poles[0], Poles[1], Poles[2]));
	}

	private IEnumerator MoveDisk(int n, Pole start, Pole middle, Pole end) {
		if (n <= 0) yield break;
		
		yield return StartCoroutine(MoveDisk(n-1, start, end, middle));
		
		// move disk from start to end code here
		Disk disk = start.GetTopMostFilledNode().CurrentDisk;
		Node endPoleNode = end.GetFirstEmptyNode();

		yield return StartCoroutine(DiskMovement(disk, endPoleNode.transform.position));
		
		disk.Evt_OnDiskMoved.Invoke(disk);
		end.OnAddDisk(disk);
		
		yield return StartCoroutine(MoveDisk(n-1, middle, start, end));
	}
	
	private IEnumerator DiskMovement(Disk disk, Vector3 endPos) {
		print("start move coroutine");
		float elapsedTime = 0;

		while (elapsedTime < moveTime) {
			disk.transform.position = Vector2.Lerp(disk.transform.position, endPos, elapsedTime / moveTime);
			elapsedTime += Time.deltaTime;

			yield return null;
		}
	}
	
}
