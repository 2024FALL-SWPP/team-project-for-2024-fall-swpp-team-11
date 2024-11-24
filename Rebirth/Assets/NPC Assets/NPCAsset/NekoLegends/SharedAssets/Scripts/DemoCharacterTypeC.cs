using System;
using System.Collections;
using UnityEngine;


namespace NekoLegends
{
    public class DemoCharacterTypeC : MonoBehaviour
    {
        [SerializeField] protected AnimationClip[] animations;

        [SerializeField] protected SkinnedMeshRenderer skinnedMeshRendererBody, skinnedMeshRendererHair, skinnedMeshRendererHeadFace, skinnedOutfitRenderer, skinnedMeshRenderBlush;

        [SerializeField] protected Material[] bodyMaterialList, hairMaterialList, outfitMaterialList;

        [SerializeField] protected GameObject LeftHandItem, RightHandItem;

        public Animator animator { get; protected set; }
      
        protected int _currentEmotionIndex, _hairColorIndex, _outfitStyleIndex, _currentAnimIndex = -1;

        protected int _emotionIndexMax = 42;
        protected float _transitionDuration = 0.25f; 

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            ResetBlendShapes();

        }

        public void NextEmotion()
        {
            _currentEmotionIndex++;
            if (_currentEmotionIndex > _emotionIndexMax)
                _currentEmotionIndex = 0;

            SetEmotion(_currentEmotionIndex);
        }

        public virtual void SetEmotion(int emoIndex)
        {
            ResetBlendShapes();
            switch (emoIndex)
            {
                case 0:
                    ResetBlendShapes();
                    DemoScenes.Instance.SetDescriptionText("Emotion: None");
                    break;
                case 1:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Happy");
                    SetBlendShapeWeightFace(6, 10f);
                    SetBlendShapeWeightFace(32, 50f);
                    SetBlendShapeWeightFace(47, 100f);
                    break;
                case 2:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Celebratory");
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(32, 50f);
                    SetBlendShapeWeightFace(33, 100f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(47, 100f);
                    break;
                case 3:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Sleepy");
                    SetBlendShapeWeightFace(5, 80f);
                    SetBlendShapeWeightFace(37, 100f);
                    break;
                case 4:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Asleep");
                    SetBlendShapeWeightFace(1, 100f);
                    SetBlendShapeWeightFace(2, 100f);
                    SetBlendShapeWeightFace(33, 50f);
                    SetBlendShapeWeightFace(37, 100f);
                    break;
                case 5:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Concerned");
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(31, 50f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(37, 50f);
                    break;
                case 6:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Emotional");
                    SetBlendShapeWeightFace(3, 25f);
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(29, 50f);
                    SetBlendShapeWeightFace(33, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(37, 75f);
                    SetBlendShapeWeightFace(42, 50);
                    SetBlendShapeWeightFace(44, 50);
                    SetBlendShapeWeightFace(66, 100);
                    break;
                case 7:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Nervous");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(13, 50f);
                    SetBlendShapeWeightFace(17, 50f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(42, 100f);
                    SetBlendShapeWeightFace(46, 100f);
                    break;
                case 8:
                    DemoScenes.Instance.SetDescriptionText("Emotion: ...");
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(33, 50);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(37, 100f);
                    break;
                case 9:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Doubtful");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(11, 100f);
                    SetBlendShapeWeightFace(13, 100f);
                    SetBlendShapeWeightFace(16, 50);
                    SetBlendShapeWeightFace(33, 50f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(54, 35f);
                    break;
                case 10:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Serious");
                    SetBlendShapeWeightFace(35, 80f);
                    SetBlendShapeWeightFace(37, 100f);
                    break;
                case 11:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Malicious");
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(6, 50f);
                    SetBlendShapeWeightFace(28, 100f);
                    SetBlendShapeWeightFace(44, 100f);
                    break;
                case 12:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Sigh");
                    SetBlendShapeWeightFace(2, 100f);
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(20, 35f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(56, 100f);
                    break;
                case 13:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Stern");
                    SetBlendShapeWeightFace(6, 50f);
                    SetBlendShapeWeightFace(31, 25f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(46, 100f);
                    SetBlendShapeWeightFace(48, 35f);
                    break;
                case 14:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Cry");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(5, 45f);
                    SetBlendShapeWeightFace(13, 25f);
                    SetBlendShapeWeightFace(16, 50f);
                    SetBlendShapeWeightFace(20, 25f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(32, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(39, 15f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(46, 100f);
                    SetBlendShapeWeightFace(48, 35f);
                    SetBlendShapeWeightFace(58, 100f);
                    SetBlendShapeWeightFace(66, 100f);

                    break;
                case 15:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Blush");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(6, 50f);
                    SetBlendShapeWeightFace(7, 50f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(32, 100f);
                    SetBlendShapeWeightFace(34, 50f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(45, 100);
                    break;
                case 16:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Troubled");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(9, 35f);
                    SetBlendShapeWeightFace(13, 50f);
                    SetBlendShapeWeightFace(17, 30f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(33, 50f);
                    SetBlendShapeWeightFace(34, 50f);
                    SetBlendShapeWeightFace(35, 60f);
                    SetBlendShapeWeightFace(37, 60f);
                    SetBlendShapeWeightFace(43, 35f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(46, 100f);
                    SetBlendShapeWeightFace(48, 15f);
                    break;
                case 17:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Dumbstruck");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(8, 100f);
                    SetBlendShapeWeightFace(13, 50f);
                    SetBlendShapeWeightFace(17, 25f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(32, 100f);
                    SetBlendShapeWeightFace(41, 30f);
                    SetBlendShapeWeightFace(54, 50f);
                    break;
                case 18:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Angry");
                    SetBlendShapeWeightFace(6, 50f);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(28, 100f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(36, 15f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(39, 100f);
                    SetBlendShapeWeightFace(48, 100f);
                    SetBlendShapeWeightFace(55, 100f);
                    SetBlendShapeWeightFace(57, 35f);
                    break;
                case 19:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Sorry");
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(4, 50f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(33, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(42, 100f);
                    SetBlendShapeWeightFace(44, 50f);
                    break;
                case 20:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Hey!");
                    SetBlendShapeWeightFace(6, 50f);
                    SetBlendShapeWeightFace(10, 70f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(35, 70f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(46, 100f);
                    SetBlendShapeWeightFace(48, 35f);
                    SetBlendShapeWeightFace(55, 40f);
                    SetBlendShapeWeightFace(57, 75f);
                    break;
                case 21:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Sneaky");
                    SetBlendShapeWeightFace(5, 75f);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(12, 100f);
                    SetBlendShapeWeightFace(16, 50f);
                    SetBlendShapeWeightFace(28, 100f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(44, 100f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(48, 50f);
                    break;
                case 22:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Gloomy");
                    SetBlendShapeWeightFace(5, 75f);
                    SetBlendShapeWeightFace(8, 100f);
                    SetBlendShapeWeightFace(28, 100f);
                    SetBlendShapeWeightFace(31, 50f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(46, 100f);
                    SetBlendShapeWeightFace(49, 100f);
                    break;
                case 23:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Dissapointed");
                    SetBlendShapeWeightFace(5, 35f);
                    SetBlendShapeWeightFace(10, 100f);
                    SetBlendShapeWeightFace(20, 50f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(34, 75f);
                    SetBlendShapeWeightFace(35, 35f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(46, 100f);
                    break;
                case 24:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Defiant");
                    SetBlendShapeWeightFace(1, 100f);
                    SetBlendShapeWeightFace(2, 100f);
                    SetBlendShapeWeightFace(20, 35f);
                    SetBlendShapeWeightFace(29, 50f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(44, 50f);
                    SetBlendShapeWeightFace(48, 50f);
                    SetBlendShapeWeightFace(57, 50f);
                    break;
                case 25:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Apprehensive");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(31, 50f);
                    SetBlendShapeWeightFace(33, 50f);
                    SetBlendShapeWeightFace(34, 75f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(37, 50f);
                    SetBlendShapeWeightFace(46, 50f);
                    SetBlendShapeWeightFace(48, 50f);
                    SetBlendShapeWeightFace(51, 50f);
                    SetBlendShapeWeightFace(55, 100f);
                    break;
                case 26:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Coy");
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(11, 100f);
                    SetBlendShapeWeightFace(16, 50f);
                    SetBlendShapeWeightFace(28, 100f);
                    SetBlendShapeWeightFace(31, 50f);
                    SetBlendShapeWeightFace(44, 100f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(48, 50f);
                    SetBlendShapeWeightFace(58, 50f);
                    break;
                case 27:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Satisfied");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(32, 100f);
                    SetBlendShapeWeightFace(33, 100f);
                    SetBlendShapeWeightFace(34, 50f);
                    SetBlendShapeWeightFace(35, 75f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(44, 100f);
                    SetBlendShapeWeightFace(48, 50f);
                    SetBlendShapeWeightFace(51, 75f);
                    break;
                case 28:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Sad");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(12, 100f);
                    SetBlendShapeWeightFace(13, 50f);
                    SetBlendShapeWeightFace(16, 50f);
                    SetBlendShapeWeightFace(20, 100f);
                    SetBlendShapeWeightFace(29, 50f);
                    SetBlendShapeWeightFace(32, 100f);
                    SetBlendShapeWeightFace(34, 50f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(46, 100f);
                    SetBlendShapeWeightFace(48, 35f);
                    SetBlendShapeWeightFace(58, 100f);
                    SetBlendShapeWeightFace(66, 100f);
                    break;
                case 29:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Bewildered");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(10, 75f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(54, 50f);
                    SetBlendShapeWeightFace(55, 75f);
                    SetBlendShapeWeightFace(56, 50f);
                    break;
                case 30:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Frightened");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(17, 90f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(34, 75f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(37, 35f);
                    SetBlendShapeWeightFace(55, 75f);
                    SetBlendShapeWeightFace(57, 75f);
                    break;
                case 31:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Are you ok?");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(10, 100f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(54, 100f);
                    SetBlendShapeWeightFace(58, 100f);
                    break;
                case 32:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Bleh!");
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(6, 50f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(64, 100f);
                    break;
                case 33:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Really?");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(10, 75f);
                    SetBlendShapeWeightFace(17, 35f);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(54, 35f);
                    SetBlendShapeWeightFace(55, 35f);
                    SetBlendShapeWeightFace(58, 100f);
                    break;
                case 34:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Can't Stop Laughing");
                    SetBlendShapeWeightFace(3, 100f);
                    SetBlendShapeWeightFace(4, 40f);
                    SetBlendShapeWeightFace(6, 10f);
                    SetBlendShapeWeightFace(8, 100f);
                    SetBlendShapeWeightFace(32, 100f);
                    SetBlendShapeWeightFace(33, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(44, 100f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(47, 100f);
                    SetBlendShapeWeightFace(57, 50f);
                    SetBlendShapeWeightFace(66, 100f);
                    break;
                case 35:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Grumpy");
                    SetBlendShapeWeightFace(10, 100f);
                    SetBlendShapeWeightFace(11, 100f);
                    SetBlendShapeWeightFace(20, 100f);
                    SetBlendShapeWeightFace(31, 100f);
                    SetBlendShapeWeightFace(34, 50f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(46, 100f);
                    SetBlendShapeWeightFace(49, 50f);
                    SetBlendShapeWeightFace(50, 100f);
                    SetBlendShapeWeightFace(57, 50f);
                    break;
                case 36:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Energetic");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(31, 60f);
                    SetBlendShapeWeightFace(35, 75f);
                    SetBlendShapeWeightFace(37, 60f);
                    SetBlendShapeWeightFace(41, 35f);
                    SetBlendShapeWeightFace(44, 50f);
                    SetBlendShapeWeightFace(47, 100f);
                    SetBlendShapeWeightFace(58, 100f);
                    break;
                case 37:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Star Eyes");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(21, 100f);
                    SetBlendShapeWeightFace(31, 60f);
                    SetBlendShapeWeightFace(35, 75f);
                    SetBlendShapeWeightFace(37, 60f);
                    SetBlendShapeWeightFace(44, 100f);
                    SetBlendShapeWeightFace(45, 100f);
                    break;
                case 38:
                    DemoScenes.Instance.SetDescriptionText("Emotion: >_<");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(22, 100f,0);
                    SetBlendShapeWeightFace(31, 60f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 75f);
                    SetBlendShapeWeightFace(37, 60f);
                    SetBlendShapeWeightFace(39, 50f);
                    SetBlendShapeWeightFace(50, 100f);
                    break;
                case 39:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Hypnotized");
                    SetBlendShapeWeightFace(16, 100f);
                    SetBlendShapeWeightFace(23, 100f, 0);
                    SetBlendShapeWeightFace(29, 100f);
                    SetBlendShapeWeightFace(32, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 50f);
                    SetBlendShapeWeightFace(39, 50f);
                    SetBlendShapeWeightFace(51, 100f);
                    break;
                case 40:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Neko Eyes And Mouth");
                    SetBlendShapeWeightFace(15, 100f);
                    SetBlendShapeWeightFace(33, 100f);
                    SetBlendShapeWeightFace(52, 100f);
                    SetBlendShapeWeightFace(53, 100f);
                    break;
                case 41:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Neko Tooth");
                    skinnedMeshRenderBlush.SetBlendShapeWeight(0, 100);
                    SetBlendShapeWeightFace(5, 45);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(13, 45f);
                    SetBlendShapeWeightFace(17, 50f);
                    SetBlendShapeWeightFace(37, 100f);
                    SetBlendShapeWeightFace(52, 100f);
                    SetBlendShapeWeightFace(69, 100f);
                    break;
                case 42:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Dead");
                    SetBlendShapeWeightFace(24, 100f, 0);
                    SetBlendShapeWeightFace(46, 100f);
                    break;

            }
        }

        private void ResetBlendShapes()
        {
            for (int i = 0; i < skinnedMeshRendererHeadFace.sharedMesh.blendShapeCount; i++)
            {
                skinnedMeshRendererHeadFace.SetBlendShapeWeight(i, 0f);
            }

            skinnedMeshRenderBlush.SetBlendShapeWeight(0, 0);
        }


        public virtual void ToggleHairColor()
        {
            _hairColorIndex++;
            if (_hairColorIndex >= hairMaterialList.Length)
                _hairColorIndex = 0;

            Material[] hairMaterials = skinnedMeshRendererHair.materials;

            hairMaterials[0] = hairMaterialList[_hairColorIndex];

            skinnedMeshRendererHair.materials = hairMaterials;

            DemoScenes.Instance.DescriptionText.SetText(hairMaterialList[_hairColorIndex].name);
        }

        public virtual void NextAnim()
        {
            _currentAnimIndex++;
            if (_currentAnimIndex >= animations.Length)
            {
                _currentAnimIndex = 0;
            }
            AnimationClip nextAnim = animations[_currentAnimIndex];
            this.animator.CrossFade(nextAnim.name, _transitionDuration);
            DemoScenes.Instance.DescriptionText.SetText("Animation: " + nextAnim.name);
        }


        public void AddMaterialToRenderer(SkinnedMeshRenderer renderer, Material materialToAdd)
        {
            Material[] currentMaterials = renderer.materials;
            Material[] newMaterials = new Material[currentMaterials.Length + 1];
            currentMaterials.CopyTo(newMaterials, 0);
            newMaterials[currentMaterials.Length] = materialToAdd;
            renderer.materials = newMaterials;
        }

        public void RemoveLastMaterialFromRenderer(SkinnedMeshRenderer renderer)
        {
            if (renderer.materials.Length > 1)
            {
                Material[] materials = renderer.materials;
                Array.Resize(ref materials, materials.Length - 1);
                renderer.materials = materials;
            }
        }


        protected void SetBlendShapeWeightFace(int blendShapeIndex, float targetWeight, float duration=.25f)
        {
            StartCoroutine(AnimateBlendShape(blendShapeIndex, targetWeight, duration));

        }

        private IEnumerator AnimateBlendShape(int blendShapeIndex, float targetWeight, float duration)
        {
            float startTime = Time.time;
            float startWeight = skinnedMeshRendererHeadFace.GetBlendShapeWeight(blendShapeIndex);

            while (Time.time < startTime + duration)
            {
                float elapsed = Time.time - startTime;
                float normalizedTime = elapsed / duration;

                float weight = Mathf.Lerp(startWeight, targetWeight, normalizedTime);
                skinnedMeshRendererHeadFace.SetBlendShapeWeight(blendShapeIndex, weight);

                yield return null;
            }

            skinnedMeshRendererHeadFace.SetBlendShapeWeight(blendShapeIndex, targetWeight);
        }


        public virtual void NextClothing() { }

        public virtual void ToggleHeadGear() { }
        public virtual void ToggleFaceGear() { }

        public virtual void ToggleEquip(){}

    }
}
