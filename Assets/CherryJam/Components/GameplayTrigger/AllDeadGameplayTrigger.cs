
namespace CherryJam.Components.GameplayTrigger
{
    public class AllDeadGameplayTrigger : GameplayTriggerComponent
    {
        private void Update()
        {
            if (_activated) return;

            if (transform.childCount > 0) return;

            Activate();
        }
    }
}