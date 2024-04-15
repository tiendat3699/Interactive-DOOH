using GadGame.SO;
using GadGame.State;
using GadGame.State.MainFlowState;

namespace GadGame
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
            SetState<IdleState>();
        }
    }
}