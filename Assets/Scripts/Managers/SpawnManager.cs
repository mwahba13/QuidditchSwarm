using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{


    public GameObject gryffindorPrefab;
    public GameObject slytherinPrefab;

    public Transform gryffindorSpawn;
    public Transform slytherinSpawn;

    [SerializeField]
    private int numGryffinToSpawn;
    [SerializeField]
    private int numSlytherinToSpawn;

    public void spawnTeam(int numToSpawn, bool isGryffindor)
    {
        GameObject newPlayer = null;
        if (isGryffindor)
            newPlayer = gryffindorPrefab;
        else { }
            //new slytherin

        Transform spawnTrans = null;
        if (isGryffindor)
            spawnTrans = gryffindorSpawn;
        else { }

        for (int i = 0; i < numToSpawn; i++)
        {
            
           GameObject newObj = Instantiate<GameObject>(newPlayer, new Vector3(spawnTrans.position.x,
               spawnTrans.position.y + i, 0), Quaternion.identity);

            Player_Base newBase = null;
            newObj.TryGetComponent<Player_Base>(out newBase);
            
        
        }

    }

}
