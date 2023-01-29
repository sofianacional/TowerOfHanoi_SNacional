using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Disk : MonoBehaviour {
    
    private bool isPickedUp;
    
    public int DiskID;
    public bool IsInteractable;

    public Node CurrentNode;

    public UnityEvent<Disk> Evt_OnDiskMoved = new();

    #region Input

        private void OnMouseDown() {
            print("pick up");
            if(!IsInteractable) return;
            isPickedUp = true;
        }

        public void OnMouseDrag() {
            // Follow cursor
            if(!IsInteractable) return;
            transform.position = Camera.main.ScreenToWorldPoint(GetMousePosition());
        }
        
        public void OnMouseUp() {
            // Check for Pole component
            if(!isPickedUp) return;
            
            List<GameObject> objs = GetObjectsOnMousePosition();

            Pole newPole = new();
            foreach (var o in objs) { // Search for POLE
                if (o.GetComponent<Pole>()) {
                    newPole = o.GetComponent<Pole>();
                    break;
                }
            }
            
            // CHECKING IF VALID
            if (newPole && newPole != CurrentNode.Pole && newPole.CanAttachDisk(this)) {
                Evt_OnDiskMoved.Invoke(this);
                newPole.OnAddDisk(this);
            }
            else { // RETURN DISK TO PREVIOUS NODE
                CurrentNode.AttachDisk(this);
            }

            isPickedUp = false;
        }

    #endregion

    private List<GameObject> GetObjectsOnMousePosition() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

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
