using UnityEngine;
using UnityEngine.UI;

namespace NekoLegends
{
    public class DemoSceneCombat : DemoScenes
    {

        [SerializeField] protected DemoNekoCombatCharacter _nekoCharacter;
        [Space]
        [SerializeField] protected Button IdleBtn;
        [SerializeField] protected Button WalkBtn;
        [SerializeField] protected Button RunBtn;
        [SerializeField] protected Button ImpairedBtn;
        [SerializeField] protected Button AlertBtn;
        [SerializeField] protected Button AttackABtn;
        [SerializeField] protected Button AttackBBtn;
        [SerializeField] protected Button SpecialABtn;
        [SerializeField] protected Button SpecialBBtn;
        [SerializeField] protected Button UltimateBtn;

        #region Singleton
        public static new DemoSceneCombat Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType(typeof(DemoSceneCombat)) as DemoSceneCombat;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static DemoSceneCombat _instance;
        #endregion



        protected override void Start()
        {
            base.Start();
            
        }

        protected override void OnEnable()
        {
            if(IdleBtn)
                RegisterButtonAction(IdleBtn, () => _nekoCharacter.PlayAnimation("Idle"));
            if (AlertBtn)
                RegisterButtonAction(AlertBtn, () => _nekoCharacter.PlayAnimation("Alert"));
            if (WalkBtn)
                RegisterButtonAction(WalkBtn, () => _nekoCharacter.PlayAnimation("Walk"));
            if (RunBtn)
                RegisterButtonAction(RunBtn, () => _nekoCharacter.PlayAnimation("Run"));
            if (ImpairedBtn)
                RegisterButtonAction(ImpairedBtn, () => _nekoCharacter.PlayAnimation("Impaired"));
            if (AttackABtn)
                RegisterButtonAction(AttackABtn, () => _nekoCharacter.PlayAnimation("AttackA"));
            if (AttackBBtn)
                RegisterButtonAction(AttackBBtn, () => _nekoCharacter.PlayAnimation("AttackB"));
            if (SpecialABtn)
                RegisterButtonAction(SpecialABtn, () => _nekoCharacter.PlayAnimation("SpecialA"));
            if (SpecialBBtn)
                RegisterButtonAction(SpecialBBtn, () => _nekoCharacter.PlayAnimation("SpecialB"));
            if (UltimateBtn)
                RegisterButtonAction(UltimateBtn, () => _nekoCharacter.PlayAnimation("Ultimate"));


            base.OnEnable();
        }
    }
}
