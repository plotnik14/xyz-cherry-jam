namespace CherryJam.Creatures.UsableItems
{
    public class HealingAction : AbstractUseAction
    {
        public HealingAction(Hero.Hero hero) : base(hero)
        {
        }

        public override void Use(float value)
        {
            Hero.Heal((int) value);
        }
    }
}