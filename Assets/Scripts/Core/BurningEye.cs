using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RPG.Core
{
    public class BurningEye : MonoBehaviour
    {
        [SerializeField] Slider beSlider;

        [SerializeField] int threatLevel = 0; //Shown for editing purposes.
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
            var sliderPOS = Mathf.Ceil(threatLevel / 200);
            if (curPOS != sliderPOS)
            {
                beSlider.value = sliderPOS;
            }
        }
    }
}