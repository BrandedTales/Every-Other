using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Core
{
    public class Phase
    {
        public string phaseName;

        bool visible = true;

        // Use this for initialization


        public Phase(string setName, bool isVisible)
        {
            phaseName = setName;
            visible = isVisible;
        }


        public void SetVisible(bool newVisible)
        {
            visible = newVisible;
        }

        public bool GetVisibility()
        {
            return visible;
        }


    }
}