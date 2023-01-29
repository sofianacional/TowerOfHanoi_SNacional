using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

// MANAGES NODES, SETS TOP MOST DISK AS INTERACTABLE

public class Pole : MonoBehaviour {
    public enum PoleType {
        A,
        B,
        C
    }
    
    public PoleType PoleID;
    public List<Node> Nodes;
    
    private Node topEmptyNode;
    private int topEmptyNodeIndex = 0;

    private void Start() {
        topEmptyNode = GetTopMostNode();
    }

    private Node GetTopMostNode() { // TOP MOST EMPTY NODE
        int index = 0;
        foreach (var n in Nodes) {
            if (n.CurrentDisk) {
                index++;
                n.CurrentDisk.IsInteractable = false;
            }
            else {
                topEmptyNode = n;
                break;
            }
        }
        topEmptyNodeIndex = index;
        return topEmptyNode;
    }
    
    private void SetInteractableDisk() {
        if (topEmptyNodeIndex < 0) return;
        
        Node node = Nodes[topEmptyNodeIndex - 1];
        if (node) node.CurrentDisk.IsInteractable = true;
    }
    
    public bool CanAttachDisk(Disk newDisk) {
        // Check thorough all nodes and their contents
        if (topEmptyNodeIndex >= Nodes.Count) return false; // Tower is full
        if (topEmptyNodeIndex <= 0) return true; // Tower is empty
        if (newDisk.DiskID < Nodes[topEmptyNodeIndex - 1].CurrentDisk.DiskID) return true;

        return false;
    }

    public void OnAddDisk(Disk disk) {
        Nodes[topEmptyNodeIndex].AttachDisk(disk); // attach to node above last node
        topEmptyNode = GetTopMostNode();
        
        // Add listener to disk Evt_OnDiskMoved -> OnRemoveDisk
        disk.Evt_OnDiskMoved.AddListener(OnRemoveDisk);
        
        SetInteractableDisk();
    }
    
    private void OnRemoveDisk(Disk disk) {
        // Search for Node with DISK
        foreach (var n in Nodes) {
            if (n.CurrentDisk == disk) {
                disk.Evt_OnDiskMoved.RemoveListener(OnRemoveDisk);
                n.ClearNode();
                break;
            }
        }
        topEmptyNode = GetTopMostNode();
        
        if(topEmptyNodeIndex > 0) SetInteractableDisk();
    }
}
