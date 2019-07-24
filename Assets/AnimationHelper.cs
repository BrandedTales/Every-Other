using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

public class AnimationHelper : MonoBehaviour
{
    public void Hit()
    {
        transform.root.GetComponent<Fighter>().Hit();
    }
}
