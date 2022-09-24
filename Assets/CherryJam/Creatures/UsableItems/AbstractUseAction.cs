
namespace CherryJam.Creatures.UsableItems
{
    public abstract class AbstractUseAction
    {
        protected Hero.Hero Hero;

        public AbstractUseAction(Hero.Hero hero)
        {
            Hero = hero;
        }

        public abstract void Use(float value);
    }
}