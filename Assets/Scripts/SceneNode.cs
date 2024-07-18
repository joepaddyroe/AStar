using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNode : MonoBehaviour
{

    [SerializeField] private int _nodeID;
    [SerializeField] private SceneNodeStatus _sceneNodeStatus;

    private MeshRenderer _meshRenderer;

    public SceneNodeStatus SceneNodeStatus => _sceneNodeStatus;
    public int NodeID => _nodeID;
    
    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetID(int id)
    {
        _nodeID = id;
    }
    
    public void SetAsStartNode()
    {
        _sceneNodeStatus = SceneNodeStatus.Start;
        _meshRenderer.material.color = Color.green;
    }
    
    public void SetAsEndNode()
    {
        _sceneNodeStatus = SceneNodeStatus.End;
        _meshRenderer.material.color = Color.red;
    }
    
    public void SetAsPathNode()
    {
        if(_sceneNodeStatus == SceneNodeStatus.Start || _sceneNodeStatus == SceneNodeStatus.End)
            return;
        _sceneNodeStatus = SceneNodeStatus.Path;
        _meshRenderer.material.color = Color.blue;
    }
    
    public bool ToggleNodeWalkable()
    {
        if (_sceneNodeStatus == SceneNodeStatus.Walkable)
        {
            _sceneNodeStatus = SceneNodeStatus.Blocked;
            _meshRenderer.material.color = Color.black;
            return true;
        } else if (_sceneNodeStatus == SceneNodeStatus.Blocked)
        {
            _sceneNodeStatus = SceneNodeStatus.Walkable;
            _meshRenderer.material.color = Color.white;
            return false;
        }

        return false;
    }
    
    public void ResetNode()
    {
        if (_sceneNodeStatus == SceneNodeStatus.Blocked)
            return;
        
        _sceneNodeStatus = SceneNodeStatus.Walkable;
        _meshRenderer.material.color = Color.white;
    }
    
}

public enum SceneNodeStatus
{
    Walkable,
    Blocked,
    Start,
    End,
    Path
}
