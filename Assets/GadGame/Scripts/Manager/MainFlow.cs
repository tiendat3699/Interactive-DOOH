using GadGame.SO;
using GadGame.State;

namespace GadGame.Manager
{
    public class MainFlow : StateRunner<MainFlow>
    {
        public SceneFlowConfig SceneFlowConfig;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetState(typeof(IdleState));
        }
    }
}