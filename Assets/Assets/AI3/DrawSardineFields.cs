public class DrawSardineFields : IDrawEnemyFields
{
    private SardineController controller;

    private void Awake()
    {
        controller = GetComponent<SardineController>();
    }

    void Start() => controller.stateMachine.StateMachineNewStateEvent += UpdateDetectionSphere;
    private void OnDestroy() => controller.stateMachine.StateMachineNewStateEvent -= UpdateDetectionSphere;

}
