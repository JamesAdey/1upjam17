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
            }
        }else
        {
            if (distanceToPoint(nextGridPos) < 0.2f)
            {
                //choose next grid
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


    private void checkAdjacent()
    {

        List<Vector2> adjacency = new List<Vector2>();

        PlantBase[] neighbours = PlantManager.GetNeighboursForPosition(gridPos);
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
