public class DrawTunaFields : IDrawEnemyFields
{
    private TunaController controller;

    private void Awake()
    {
        controller = GetComponent<TunaController>();
    }

    void Start() => controller.stateMachine.StateMachineNewStateEvent += UpdateDetectionSphere;
    private void OnDestroy() => controller.stateMachine.StateMachineNewStateEvent -= UpdateDetectionSphere;

}
