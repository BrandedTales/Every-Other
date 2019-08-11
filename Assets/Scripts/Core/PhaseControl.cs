using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Core
{
    public class PhaseControl : MonoBehaviour
    {
        public struct phase
        {
            int id;
            string phaseName;
            string spriteName;

            public phase(int i, string pName, string sName)
            {
                id = i;
                phaseName = pName;
                spriteName = sName;
            }
        }

        private phase[] phases;


        // Use this for initialization
        void Start()
        {
            phases[0] = new phase(0, "Social", "Diplomat.png");
            phases[1] = new phase(1, "Offense", "Offense.png");
            phases[2] = new phase(2, "Defense", "Defense.png");


            InitalizePhases();

        }

        private void InitalizePhases()
        {
            //Build a day for 4 days.
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void AddPhase()
        {
            int newPhase = Random.Range(0, PHASE_DIPLOMAT);

        }

        private void UpdateImages()
        {

        }

        private void NewDay()
        {

        }
    }
}
