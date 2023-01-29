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
	}

	public void SpawnDisks() {
		for (int i = 0; i < numberOfDisks; i++) {
			Disk newDisk = Instantiate(DiskPrefabs[i]);
			Poles[0].OnAddDisk(newDisk);
		}
	}
}
