using UnityEngine;
using System.Collections;

public class Snitch_AI : MonoBehaviour
{



    private Snitch_Move _snitchMove;

    public void Start()
    {
        TryGetComponent<Snitch_Move>(out _snitchMove);
    }

}
