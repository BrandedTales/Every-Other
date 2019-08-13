using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] float maxEnergy = 1000f;
        [SerializeField] float currentEnergy;
        [SerializeField] float offset = 0f;

        [SerializeField] Slider fatigueSlider;

        // Start is called before the first frame update
        void Start()
        {
            currentEnergy = maxEnergy;
            InitializeSlider();
        }

        // Update is called once per frame
        void Update()
        {
            MoveSlider();
        }

        private void InitializeSlider()
        {
            fatigueSlider.maxValue = maxEnergy;
        }

        private void MoveSlider()
        {
            fatigueSlider.value = currentEnergy;
        }


        public void ResetEnergy()
        {
            //Reset the fatigue score on a new day, but be prepared to offset as a result of events from the day.
            currentEnergy = maxEnergy - offset;
        }

        public void ReduceEnergy(float reduceAmount)
        {
            if (!FindObjectOfType<Pause>().GamePaused()) currentEnergy -= reduceAmount;

            if (currentEnergy <= 0)
            {
                Collapse();
            }
        }

        private void Collapse()
        {
            Debug.Log("Oof!  Collapsed!");

            ResetEnergy();

            //Update the phase
            FindObjectOfType<PhaseController>().NewDay();
        }
    }
}
