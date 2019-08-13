using UnityEngine;
using System.Collections;
using System;

namespace RPG.Core
{

    public class MistController : MonoBehaviour
    {

        [SerializeField] int mistLevel = 0;
        PhaseController myPhaseController;

        // Use this for initialization
        void Start()
        {
            myPhaseController = GetComponent<PhaseController>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateMist();
        }

        private void UpdateMist()
        {
            UpdatePhases();
        }

        private void UpdatePhases()
        {
            myPhaseController.AdjustVisibility(mistLevel);
        }
    }
}
