using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Disk : MonoBehaviour {
    
    public int DiskID;
    public bool IsInteractable;

    public Node CurrentNode;

    public UnityEvent<Disk> Evt_OnDiskMoved = new();
    
}
