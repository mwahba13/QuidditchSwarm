using UnityEngine;
using System.Collections;


public abstract class Player_Base : MonoBehaviour
{

    public enum PlayerState : uint
    {

        Unconscious, Conscious

    }

    //Player traits, all between 0.0 and 1.0
    [SerializeField]
    protected double _aggro;
    [SerializeField]
    protected double _maxExhaust;
    [SerializeField]
    protected double _currentExhaust;
    [SerializeField]
    protected double _maxVelocity;
    [SerializeField]
    protected double _weight;

    private PlayerState _state;

    public void Start()
    {

        generateTraitValues();
        setState(PlayerState.Conscious);
    }


    public abstract void generateTraitValues();
    public void setState(PlayerState newState) { _state = newState; }
    public PlayerState getState() { return _state; }

}
