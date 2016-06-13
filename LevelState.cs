using UnityEngine;
using System.Collections;

public class LevelState : MonoBehaviour {

    public enum State
    {
        BEGINNER,
        INTERMEDIATE,
        ADVANCED
    }
    public State difficulty;
}
