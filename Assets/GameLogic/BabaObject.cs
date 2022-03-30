using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaObject : MonoBehaviour
{
    public ObjectType type;


}

public enum ObjectType
{
    Player,
    Obstacle,
    Killer,
}
