namespace PixelCrew.Creatures.UsableItems
{
    public class BoostSpeedAction : AbstractUseAction
    {
        public BoostSpeedAction(Hero.Hero hero) : base(hero)
        {
        }

        public override void Use(float value)
        {
            Hero.BoostSpeed(value);
        }
    }
}