using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NekoLegends
{
    public class DemoCameraScene : DemoScenes
    {

        [SerializeField] private DemoCameraController _cameraController;
        [Space]
        [SerializeField] private Button ChangeCameraBtn, NextVisibleBtn, PrevVisibleBtn;
        [SerializeField] private Boolean DisableCameraControl;
        [SerializeField] private Boolean FocusOnNewTargets;
        [SerializeField] private List<Transform> Targets;

        private int currentTargetIndex, currentVisibleTargetIndex;


        protected override void Start()
        {
            base.Start();
            if(!DisableCameraControl)
                _cameraController.target = Targets[currentTargetIndex];
            InitTargets();

        }

        protected override void OnEnable()
        {
            if (ChangeCameraBtn)
                ChangeCameraBtn.onClick.AddListener(ChangeTarget); // Register the new button action
            if (NextVisibleBtn)
                NextVisibleBtn.onClick.AddListener(ChangeVisibleTarget); 
            if (PrevVisibleBtn)
                PrevVisibleBtn.onClick.AddListener(ChangeVisibleTargetBackward);

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            if (ChangeCameraBtn)
                ChangeCameraBtn.onClick.RemoveListener(ChangeTarget); // Remember to remove the listener to prevent memory leaks
            if (NextVisibleBtn)
                NextVisibleBtn.onClick.RemoveListener(ChangeVisibleTarget);
            if (PrevVisibleBtn)
                PrevVisibleBtn.onClick.RemoveListener(ChangeVisibleTargetBackward);
            base.OnDisable();
        }

        private void InitTargets()
        {
            currentVisibleTargetIndex = 0;

            for (int i = 0; i < Targets.Count; i++)
            {
                int targetIndex = (i) % Targets.Count;
                Transform targetTransform = Targets[targetIndex];
                targetTransform.gameObject.SetActive(i == 0);
            }
            SetDescriptionText(Targets[0].name);
        }
        private void ChangeTarget()
        {
            currentTargetIndex = (currentTargetIndex + 1) % Targets.Count; // Increment the index and wrap around if necessary
            if (!DisableCameraControl)
            {
                _cameraController.target = Targets[currentTargetIndex]; // Set the new target
                _cameraController.AutoDOFTarget = Targets[currentTargetIndex];
            }
        }

        private void ChangeVisibleTarget()
        {
            // Cycle through the list of targets
            currentVisibleTargetIndex = (currentVisibleTargetIndex + 1) % Targets.Count;

            for (int i = 0; i < Targets.Count; i++)
            {
                int targetIndex = (i + currentVisibleTargetIndex) % Targets.Count;
                Transform targetTransform = Targets[targetIndex];
                targetTransform.gameObject.SetActive(i == 0);
            }
            SetDescriptionText(Targets[currentVisibleTargetIndex].name);

            if (FocusOnNewTargets)
                SetCameraToVisibleTarget();

        }
        private void ChangeVisibleTargetBackward()
        {
            // Decrement the index and wrap around if necessary
            if (currentVisibleTargetIndex == 0)
                currentVisibleTargetIndex = Targets.Count - 1;
            else
                currentVisibleTargetIndex--;

            for (int i = 0; i < Targets.Count; i++)
            {
                int targetIndex = (i + currentVisibleTargetIndex) % Targets.Count;
                Transform targetTransform = Targets[targetIndex];
                targetTransform.gameObject.SetActive(i == 0);
            }
            SetDescriptionText(Targets[currentVisibleTargetIndex].name);


            if (FocusOnNewTargets)
                SetCameraToVisibleTarget();
        }
        private void SetCameraToVisibleTarget()
        {
            _cameraController.AutoDOFTarget = Targets[currentVisibleTargetIndex];
        }

    }
}
