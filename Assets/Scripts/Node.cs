using UnityEngine;

public class Node : MonoBehaviour {
    
	[SerializeField] private int nodeID;

	public Pole Pole;
	public Disk CurrentDisk { get; private set; }

	private void Start() {
		Pole = GetComponentInParent<Pole>();
		ClearNode();
	}

	public void AttachDisk(Disk selectedDisk) {
		selectedDisk.transform.position = transform.position;
		SetNodeDisk(selectedDisk);
		selectedDisk.CurrentNode = this;
	}

	public void SetNodeDisk(Disk newDisk) {
		CurrentDisk = newDisk;
	}
	
	public void ClearNode() {
		CurrentDisk = null;
	}
}
