using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NekoLegends
{
    public class DemoCameraSceneTargetsWithFocus : DemoScenes
    {

        [SerializeField] private DemoCameraController _cameraController;
        [Space]
        [SerializeField] private Button ChangeCameraBtn, NextVisibleBtn;
        [SerializeField] private Boolean DisableCameraControl;
        [SerializeField] private Boolean FocusOnNewTargets;
        [SerializeField] private List<Transform> FocusTargets;
        [SerializeField] private List<GameObject> Items;

        private int currentTargetIndex, currentVisibleTargetIndex;


        protected override void Start()
        {
            base.Start();
            if(!DisableCameraControl)
                _cameraController.target = FocusTargets[currentTargetIndex];
            InitTargets();

        }

        protected override void OnEnable()
        {
            if (ChangeCameraBtn)
                ChangeCameraBtn.onClick.AddListener(ChangeTarget); // Register the new button action
            if (NextVisibleBtn)
                NextVisibleBtn.onClick.AddListener(ChangeVisibleTarget); 

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            if (ChangeCameraBtn)
                ChangeCameraBtn.onClick.RemoveListener(ChangeTarget); // Remember to remove the listener to prevent memory leaks
            if (NextVisibleBtn)
                NextVisibleBtn.onClick.RemoveListener(ChangeVisibleTarget);
            base.OnDisable();
        }

        private void InitTargets()
        {
            currentVisibleTargetIndex = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                int targetIndex = (i) % Items.Count;
                GameObject targetTransform = Items[targetIndex];
                targetTransform.SetActive(i == 0);
            }
            SetDescriptionText(Items[0].name);
        }

        private void ChangeTarget()
        {
            currentTargetIndex = (currentTargetIndex + 1) % FocusTargets.Count; // Increment the index and wrap around if necessary
            if (!DisableCameraControl)
            {
                _cameraController.target = FocusTargets[currentTargetIndex]; // Set the new target
                _cameraController.AutoDOFTarget = FocusTargets[currentTargetIndex];
            }
        }

        private void ChangeVisibleTarget()
        {
            // Cycle through the list of targets
            currentVisibleTargetIndex = (currentVisibleTargetIndex + 1) % Items.Count;

            for (int i = 0; i < Items.Count; i++)
            {
                int targetIndex = (i + currentVisibleTargetIndex) % Items.Count;
                GameObject targetTransform = Items[targetIndex];
                targetTransform.SetActive(i == 0);
            }
            SetDescriptionText(Items[currentVisibleTargetIndex].name);

            if (FocusOnNewTargets)
                SetCameraToVisibleTarget();

        }

        private void SetCameraToVisibleTarget()
        {
            _cameraController.AutoDOFTarget = FocusTargets[currentVisibleTargetIndex];
        }

    }
}
