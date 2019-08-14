using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Parallax : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            lastpos = camera.position;
        }

        [SerializeField] Transform camera;
        [SerializeField] float speedCoefficient;
        Vector3 lastpos;

        void Update()
        {
            transform.position -= ((lastpos - camera.position) * speedCoefficient);
            lastpos = camera.position;
        }
    }
}