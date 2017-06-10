using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmerScript : MonoBehaviour {

    private Vector2 gridPos;
    private Vector3 nextGridPos;
    private Vector3 targetPos;
    private PlantManager plantM;
    private Vector2[] adjacentPath;
    private Transform farmerTransform;
    private Vector3 direction3;
    private Vector3[] movementPath;

    public float speed;

    public float maxDelay;
    private float waitTime = 0;

    private bool atGoal;

	// Use this for initialization
	void Start () {
        plantM = PlantManager.singleton;
        farmerTransform = this.transform;
	}

    // Update is called once per frame
    void Update() {

        if (atGoal)
        {
            if (Time.time > waitTime)
            {
                //choose Location
                calculatePath();
            }
        }else
        {
            if (distanceToPoint(nextGridPos) < 0.2f)
            {
                nextGridPos = chooseNextGrid();
            }else
            {
                
                farmerTransform.position += direction3 * speed * Time.deltaTime;
            }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos">Position to be calculated from</param>
    /// <returns>Distance between the farmer and the given position</returns>
    private float distanceToPoint(Vector3 pos)
    {
        return (farmerTransform.position - pos).sqrMagnitude;
    }

    private Vector3 chooseNextGrid()
    {
        return Vector3.up;
    }

    private void calculatePath()
    {
        List<WorldTile> path = new List<WorldTile>();
        LinkedList<WorldTile> openSet = new LinkedList<WorldTile>();

        List<WorldTile> visited = new List<WorldTile>();

        openSet.AddFirst(plantM.GetTile(Mathf.RoundToInt(gridPos.x), Mathf.RoundToInt(gridPos.y)));

        while(openSet.Count > 0)
        {
            WorldTile next = FindNearest(openSet);

            if (next != plantM.GetTile(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)))
            {
                WorldTile checkNode = openSet.First.Value;
                checkAdjacent(new Vector2(checkNode.x, checkNode.y));
                for (int i = 0; i < adjacentPath.Length; i++)
                {
                    if (adjacentPath[i] != null)
                    {

                    }
                    else
                    {
                        plantM.GetTile(Mathf.RoundToInt(adjacentPath[i].x), Mathf.RoundToInt(adjacentPath[i].y)).previous = checkNode;
                        openSet.AddFirst(plantM.GetTile(Mathf.RoundToInt(adjacentPath[i].x), Mathf.RoundToInt(adjacentPath[i].y)));
                        openSet.Remove(checkNode);
                        visited.Add(checkNode);
                    }
                }
            }else
            {
                

            }

        }

    }

    private WorldTile FindNearest(LinkedList<WorldTile> tileSet)
    {
        WorldTile near = null;
        float distance = 1000;
        for(int i = 0; i < tileSet.Count; i++)
        {
            WorldTile next = tileSet.First.Value;
            float newDisX = next.x - targetPos.x;
            float newDisY = next.y - targetPos.y;
            float newDist = newDisX * newDisX + newDisY * newDisY;
            if(newDist < distance)
            {
                distance = newDist;
                near = tileSet.First.Value;
            }
            tileSet.RemoveFirst();
        }

        return near;
    }

    private void checkAdjacent(Vector2 pos)
    {

        List<Vector2> adjacency = new List<Vector2>();

        PlantBase[] neighbours = PlantManager.GetNeighboursForPosition(pos);
        for(int i = 0; i < neighbours.Length; i++)
        {
            if(neighbours[i] != null)
            {
                break;
            }else
            {
                switch (i)
                {
                    case 0:
                        adjacency.Add(Vector2.up);
                        break;
                    case 1:
                        adjacency.Add(Vector2.down);
                        break;
                    case 2:
                        adjacency.Add(Vector2.left);
                        break;
                    case 3:
                        adjacency.Add(Vector2.right);
                        break;
                }

            }

            adjacentPath = adjacency.ToArray();
        }
    }


    
}
