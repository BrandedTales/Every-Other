﻿using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System;

namespace RPG.Core
{
    public class BurningEye : MonoBehaviour
    {
        [SerializeField] Slider beSlider;

        [SerializeField] float threatLevel = 0f; //Shown for editing purposes.
        float minThreat = 0;
        float maxThreat = 1000;
        int curPOS = 1;



        // Use this for initialization
        void Start()
        { 

        }

        // Update is called once per frame
        void Update()
        {
            CheckSliderPosition();

        }



        private void CheckSliderPosition()
        {
            var sliderPOS = Mathf.Ceil(threatLevel / 200.0f);
            if (curPOS != sliderPOS)
            {
                beSlider.value = sliderPOS;
            }
        }

        public void AddThreat(float threat)
        {
            threatLevel += threat;
            threatLevel = Mathf.Clamp(threatLevel, minThreat, maxThreat);

        }

        public float GetThreat() { return threatLevel; }
    }
}