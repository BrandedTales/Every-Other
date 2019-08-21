using UnityEngine;
using UB.Simple2dWeatherEffects.Standard;
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

            float newDensity = mistLevel / 4.00f;  //4 = max levels of mist.  Encapsulate this later.

            var mistLayers = FindObjectsOfType<D2FogsPE>();
            foreach (D2FogsPE layer in mistLayers)
            {
                layer.Density = newDensity;
            }

        }
        private void UpdatePhases()
        {
            myPhaseController.AdjustVisibility(mistLevel);  
        }

        public void AdjustMist(int mistTweak)
        {
            mistLevel += mistTweak;
            mistLevel = Mathf.Clamp(mistLevel, 0, 4);   //Numbers for min and max mist.  TODO->Encapsulate into variables
        }

        public int GetMist() { return mistLevel; }
    }
}
