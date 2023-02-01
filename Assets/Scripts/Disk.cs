using UnityEngine;
using UnityEngine.Events;

public class Disk : MonoBehaviour {
    
    public int DiskID;
    public bool IsInteractable;

    public Node CurrentNode;

    public UnityEvent<Disk> Evt_OnDiskMoved = new();
    
}
