using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [Header("Map Dimensions")]
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    
    [Header("Visual Nodes")]
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private LayerMask _sceneNodeLayerMask;
    [SerializeField] private Transform _nodesParent;

    private AStarMap _astarMap;
    private AStarPathfinder _astarPathfinder;
    
    private List<SceneNode> _sceneNodes = new List<SceneNode>();

    private SceneNode _currentStartNode;
    private SceneNode _currentEndNode;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _astarMap = new AStarMap();
        _astarMap.BuildMap(_columnCount,_rowCount);

        _astarPathfinder = new AStarPathfinder();
        _astarPathfinder.SetMap(_astarMap);
        
        BuildInteractiveNodes();
    }

    // Update is called once per frame
    void Update()
    {
        // left click to assign start node
        // right click to assign end node
        // middle click to toggle Walkable/Blocked

        if (Input.GetMouseButtonDown(0))
            AssignStartNode();
        if (Input.GetMouseButtonDown(1))
            AssignEndNode();
        if (Input.GetMouseButtonDown(2))
            ToggleNodeWalkable();
        
        
        
        // do random pressure test
        // DoRandomisedPressureTest();
    }

    private void DoRandomisedPressureTest()
    {
        _astarMap.ResetNodes();
        ResetSceneNodes();
        
        _astarPathfinder.SetStartNode(Random.Range(0, _sceneNodes.Count));
        _astarPathfinder.SetEndNode(Random.Range(0, _sceneNodes.Count));

        float startTime = Time.realtimeSinceStartup;
        //Debug.Log("Start Time: " + (startTime * 1000) + "ms");
        
        List<AStarNode> path = _astarPathfinder.GetPath();
        
        Debug.Log("Time Taken: " + ((Time.realtimeSinceStartup - startTime) * 1000) + "ms");
    }

    private void ResetSceneNodes()
    {
        foreach (SceneNode sceneNode in _sceneNodes)
        {
            sceneNode.ResetNode();
        }
    }
    
    private void AssignStartNode()
    {
        _astarMap.ResetNodes();
        ResetSceneNodes();
        
        SceneNode selectedNode = RayCheck();
        
        if(!selectedNode)
            return;
        
        if (_currentStartNode != selectedNode)
        {
            if (_currentStartNode)
                _currentStartNode.ResetNode();
            
            _currentStartNode = selectedNode;
            selectedNode.SetAsStartNode();
            
            _astarPathfinder.SetStartNode(selectedNode.NodeID);
        }
    }

    private void AssignEndNode()
    {
        _astarMap.ResetNodes();
        ResetSceneNodes();
        
        SceneNode selectedNode = RayCheck();
        
        if(!selectedNode)
            return;
        
        if (_currentEndNode != selectedNode)
        {
            if(_currentEndNode)
                _currentEndNode.ResetNode();
            
            _currentEndNode = selectedNode;
            selectedNode.SetAsEndNode();
            
            _astarPathfinder.SetEndNode(selectedNode.NodeID);
        }

        if (_currentStartNode != null & _currentEndNode != null)
        {
            List<AStarNode> path = _astarPathfinder.GetPath();
            string pathIndexes = "";
            foreach (AStarNode node in path)
            {
                pathIndexes += node.ID + ",";
                _sceneNodes[node.ID].SetAsPathNode();
            }
            
            Debug.Log("Returned Path: " + pathIndexes);
        }
    }

    private void ToggleNodeWalkable()
    {
        SceneNode selectedNode = RayCheck();
        
        if(!selectedNode)
            return;
        
        bool isBlocked = selectedNode.ToggleNodeWalkable();
        _astarMap.SetNodeBlocked(selectedNode.NodeID, isBlocked);
    }

    private SceneNode RayCheck()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, _sceneNodeLayerMask))
        {
            return hit.collider.gameObject.GetComponent<SceneNode>();
        }
    
        return null;
    }
    

    private void BuildInteractiveNodes()
    {
        float horizontalOffset = ((float)_columnCount / 2) + (0.5f); // offset plus node scale halved
        float verticalOffset = ((float)_rowCount / 2) + (0.5f); // offset plus node scale halved
        
        for (int row = 0; row < _rowCount; row++)
        {
            for (int column = 0; column < _columnCount; column++)
            {
                int currentNodeIndex = (row * _columnCount) + column;
                SceneNode node = GameObject.Instantiate(_nodePrefab, _nodesParent).GetComponent<SceneNode>();
                node.transform.position = new Vector3(-horizontalOffset + (column + 1), verticalOffset - (row + 1), 0);
                node.SetID(currentNodeIndex);
                node.name = "node " + currentNodeIndex + " : " + column + "," + row;
                _sceneNodes.Add(node);
            }
        }
    }
}
