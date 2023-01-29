using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    
	[SerializeField] private int nodeID;

	public Pole Pole;
	public Disk CurrentDisk;

	private void Start() {
		Pole = GetComponentInParent<Pole>();
		ClearNode();
	}

	public void AttachDisk(Disk selectedDisk) {
		selectedDisk.transform.position = transform.position;
		CurrentDisk = selectedDisk;
		selectedDisk.CurrentNode = this;
	}

	public void ClearNode() {
		CurrentDisk = null;
	}
}
