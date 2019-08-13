using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RPG.Core
{
    public class PhaseController : MonoBehaviour
    {

        [SerializeField] int maxDayCount = 4;

        Queue<Phase> phases = new Queue<Phase>();
        string[] phaseTypes = new string[3];

        [SerializeField] Image[] days = new Image[4];

        [SerializeField] Sprite imgUnknown;
        [SerializeField] Sprite imgOffensive;
        [SerializeField] Sprite imgDefensive;
        [SerializeField] Sprite imgSocial;

        // Use this for initialization
        void Start()
        {
            InitalizePhases();
        }



        // Update is called once per frame
        void Update()
        {
            UpdateImages();
        }

        private void InitalizePhases()
        {
            PopulateDictionary();

            for (int i = 0; i < maxDayCount; i++)
            {
                AddPhase(true);
            }

        }

        private void PopulateDictionary()
        {
            //Would have loved to use a dictionary or any number of other options.  Oh well.  
            //<<TODO:  Now that phases are simplified, this can be redone as a dictionary>>
            phaseTypes[0] = "Offensive";
            phaseTypes[1] = "Defensive";
            phaseTypes[2] = "Social";

        }

        private void AddPhase(bool visible)
        {
            int randomIndex = Random.Range(0, phaseTypes.Length);
            Debug.Log("Adding phase: " + phaseTypes[randomIndex]);
            phases.Enqueue(new Phase(phaseTypes[randomIndex], visible));
        }
        private void AddPhase(Phase newPhase)
        {
            phases.Enqueue(newPhase);
        }

        private void UpdateImages()
        {
            Debug.Log("updating the phase images...");
            int index = 0;
            foreach (var i in phases.ToArray())
            {
                if (!i.GetVisibility())
                {
                    days[index].sprite = imgUnknown;
                }
                else
                {
                    switch(i.phaseName)
                    {
                        case "Offensive": days[index].sprite = imgOffensive;
                            break;
                        case "Defensive": days[index].sprite = imgDefensive;
                            break;
                        case "Social": days[index].sprite = imgSocial;
                            break;
                        default: days[index].sprite = imgUnknown;
                            break;
                    }
                }
                index++;
            }
        }

        public void NewDay()
        {
            phases.Dequeue();
            AddPhase(true);
        }

        private void UpdatePhases(Queue<Phase> newPhases)
        {
            phases = newPhases;
        }

        public void AdjustVisibility(int invisibleCount)
        {
            int visibleRange = maxDayCount - invisibleCount;
            Queue<Phase> newQueue = new Queue<Phase>();

            Debug.Log(visibleRange + " : " + phases.Count);


            for (int i=0;i<maxDayCount;i++)
            {
                Phase temp = phases.Dequeue();
                Debug.Log(temp.phaseName + ": " + i);
                if (i < visibleRange)
                    temp.SetVisible(true);
                else
                    temp.SetVisible(false);

                newQueue.Enqueue(temp);
            }

            UpdatePhases(newQueue);

        }


    }
}
