using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour {

	public Disk CurrentDisk;
	
	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Input.mousePosition;
			Vector3 screenPoint = Camera.main.ScreenToWorldPoint(mousePos);
			
			RaycastHit2D hit = Physics2D.Raycast(screenPoint, Vector2.zero);
			
			if (hit.collider.gameObject && hit.collider.gameObject.GetComponent<Disk>()) {
				Disk newDisk = hit.collider.gameObject.GetComponent<Disk>();
				if(newDisk.IsInteractable) CurrentDisk = newDisk;
			}
		}

		if (CurrentDisk && CurrentDisk.IsInteractable) {
			if(Input.GetMouseButton(0))
				CurrentDisk.transform.position = Camera.main.ScreenToWorldPoint(GetMousePosition());
			if (Input.GetMouseButtonUp(0)) {
				// Drop disk
				OnDropDisk();
			}
		}

	}

	private void OnDropDisk() {
		// LOOK FOR POLE OBJ ONLY
		if(!CurrentDisk) return;
		List<GameObject> objs = GetObjectsOnMousePosition(CurrentDisk.transform.position);

		Pole newPole = new();
		foreach (var o in objs) {
			if (o.GetComponent<Pole>()) {
				newPole = o.GetComponent<Pole>();
				break;
			}
		}
		
		// IF POLE 
		if (newPole && newPole != CurrentDisk.CurrentNode.Pole && newPole.CanAttachDisk(CurrentDisk)) {
			CurrentDisk.Evt_OnDiskMoved.Invoke(CurrentDisk);
			newPole.AttachDiskToPole(CurrentDisk);
		}
		else { // RETURN DISK TO PREVIOUS NODE
			CurrentDisk.CurrentNode.AttachDisk(CurrentDisk);
		}
		
		CurrentDisk = null;
	}
	
	private List<GameObject> GetObjectsOnMousePosition(Vector2 objectPos) {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(objectPos, 1f);

		List<GameObject> objs = new List<GameObject>();
		foreach (var h in colliders) {
			objs.Add(h.gameObject);
		}
        
		return objs;
	}
	
	private Vector3 GetMousePosition() {
		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = 1;
		return screenPoint;
	}
}
