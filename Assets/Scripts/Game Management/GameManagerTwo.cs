using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class GameManagerTwo : MonoBehaviour
{
    private LevelBuilder maze;
    HashSet<Vector2> usedSpots = new HashSet<Vector2>();
    private TextMeshProUGUI targetCount;
    
    [SerializeField] private int numToSpawn = 3;
    private int numTargetsLeftToDestroy = 0;
    private int numTargetsToDestroy;
    
    float blockwidth, blockheight;
    float xmin, xmax, zmin, zmax;

    public void SetNumToSpawn(int x)
    {
        numToSpawn = x;
    }

    private void Start()
    {
        
        
        maze = GameObject.Find("Ground").GetComponent<LevelBuilder>();
        UpdateMazeBounds();
        SetTargetCount();
        SpawnTargets();
        SpawnPlayer();
    }

    public void UpdateMazeBounds()
    {
        Bounds mazeBounds = maze.GetBounds();
        xmin = mazeBounds.min.x + 0.25f;
        xmax = mazeBounds.max.x - 0.25f;
        zmin = mazeBounds.min.z + 0.25f;
        zmax = mazeBounds.max.z - 0.25f;
        blockwidth = (xmax - xmin) / maze.xsquares;
        blockheight = (zmax - zmin) / maze.zsquares;
    }
    private void SpawnTargets()
    {
        int x = Random.Range(1, maze.xsquares + 1);
        int z = Random.Range(1, maze.xsquares + 1);
        usedSpots.Add(new Vector2(x, z));
       
        for (int i = 0; i < numToSpawn; i++)
        {
            x = Random.Range(1, maze.xsquares + 1);
            z = Random.Range(1, maze.xsquares + 1);
            if (usedSpots.Count < maze.xsquares * maze.zsquares)
            {
                while(usedSpots.Contains(new Vector2(x, z)))
                {
                    x = Random.Range(1, maze.xsquares + 1);
                    z = Random.Range(1, maze.xsquares + 1);
                }
                usedSpots.Add(new Vector2(x, z));
            }
            GameObject target = Instantiate(Resources.Load<GameObject>("Prefabs/Target"));
            target.transform.position = new Vector3(blockwidth * x + xmin - blockwidth/2, 1.5f,
                blockheight * z + zmin - blockheight/2);
        }
    }

    private void SpawnPlayer()
    {

        int x = Random.Range(1, maze.xsquares + 1);
        int z = Random.Range(1, maze.xsquares + 1);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (usedSpots.Count < maze.xsquares * maze.zsquares)
        {
            while (usedSpots.Contains(new Vector2(x, z)))
            {
                x = Random.Range(1, maze.xsquares + 1);
                z = Random.Range(1, maze.xsquares + 1);
            }
        }
        usedSpots.Clear();
        if (player == null)
        {
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        }
        CharacterController characterController = player.GetComponent<CharacterController>();
        characterController.enabled = false;
        FirstPersonMovement movementScript = player.GetComponent<FirstPersonMovement>();
        if (movementScript != null)
            movementScript.ResetMovement();
        player.transform.position = new Vector3(blockwidth * x + xmin - blockheight / 2, 1,
            blockheight * z + zmin - blockheight / 2);
        characterController.enabled = true;
    }
    public void UpdateTargetCount()
    {
        if(numTargetsLeftToDestroy > 0)
        {
            numTargetsLeftToDestroy--;
            targetCount.text = "Targets Remaining: " + numTargetsLeftToDestroy + "/" + numTargetsToDestroy;
        }

        if (numTargetsLeftToDestroy <= 0)
        {
            GameObject exit = Instantiate(Resources.Load<GameObject>("Prefabs/Exit"));
            exit.transform.position = new Vector3(Random.Range(1, maze.xsquares + 1) * blockwidth + xmin - blockwidth/2, 7, 
                Random.Range(1, maze.zsquares + 1) * blockheight + zmin - blockheight/2);
        }
    }

    private void SetTargetCount()
    {
        numTargetsToDestroy = numToSpawn;
        numTargetsLeftToDestroy = numToSpawn;
        targetCount = GameObject.Find("Target Remaining txt").GetComponent<TextMeshProUGUI>();
        targetCount.text = "Targets Remaining: " + numTargetsLeftToDestroy + "/" + numTargetsToDestroy;
    }

    public void ExitMaze()
    {
        maze.xsquares = maze.xsquares + 5;
        maze.zsquares = maze.zsquares + 5;
        maze.RebuildMaze();

        SetNumToSpawn(numToSpawn + (int)Random.Range(4, 12) * 2/3);
        SetTargetCount();
        UpdateMazeBounds();
        SpawnTargets();
        SpawnPlayer();
    }

}
