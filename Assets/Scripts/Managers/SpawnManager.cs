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

    private GameObject _snitchObj;

    public void Start()
    {
        _snitchObj = GameObject.FindWithTag("Snitch");
        spawnTeam(numGryffinToSpawn, true,_snitchObj);
    }

    public void spawnTeam(int numToSpawn, bool isGryffindor, GameObject snitchObj)
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
            GameObject newObj = Instantiate<GameObject>(newPlayer, new Vector3(spawnTrans.position.x + i,
               0, spawnTrans.position.z), Quaternion.identity);

            Player_Base newBase = null;
            newBase.setSnitchObj(snitchObj);
            newObj.TryGetComponent<Player_Base>(out newBase);

        }

    }

}
