using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField] private int numberOfDisks; // Selected ON START
	[SerializeField] private List<Disk> DiskPrefabs; // Highest disk = index 0
	
	public List<Pole> Poles;

	[Header("Player Data")]
	public int NumberOfMoves;

	private void Start() {
		SpawnDisks();
		foreach (var p in Poles) {
			p.Evt_PoleModified.AddListener(AddMove);
		}
	}

	public void SpawnDisks() {
		for (int i = 0; i < numberOfDisks; i++) {
			Disk newDisk = Instantiate(DiskPrefabs[i]);
			Poles[0].OnAddDisk(newDisk);
		}
	}

	private void AddMove() {
		NumberOfMoves++;
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
	
}
