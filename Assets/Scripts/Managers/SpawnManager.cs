using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

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
    
    private GameObject snitchObj;

    public void Start()
    {
        snitchObj = GameObject.FindWithTag("Snitch");
        SpawnTeam(numGryffinToSpawn, true,snitchObj);
        SpawnTeam(numSlytherinToSpawn,false,snitchObj);
    }

    public void SpawnTeam(int numToSpawn, bool isGryffindor, GameObject snitchObj)
    {
        GameObject newPlayer = null;
        if (isGryffindor)
            newPlayer = gryffindorPrefab;
        else
            newPlayer = slytherinPrefab;
        
            

        Transform spawnTrans = null;
        if (isGryffindor)
            spawnTrans = gryffindorSpawn;
        else
            spawnTrans = slytherinSpawn;

        for (int i = 0; i < numToSpawn; i++)
        {

            GameObject newObj = Instantiate<GameObject>(newPlayer, new Vector3(spawnTrans.position.x + i,
               0, 0), Quaternion.identity);

            PlayerBase newBase = null;
            newObj.TryGetComponent<PlayerBase>(out newBase); 
            newBase.SetSnitchObj(snitchObj);
            newBase.setSpawnPoint(spawnTrans);


        }

    }
    
    //TODO: spawn gryffindor and slytherin player functionality
    

}
