namespace flxkbr.unknownasofyet.state
{
    public class Condition
    {
        public bool Satisfied
        {
            get 
            {
                return GameState.IsSatisfied(this);
            }
        }

        public readonly string Flag;
        public readonly bool Predicate;

        public Condition(string condition)
        {
            if (condition.StartsWith('!'))
            {
                Flag = condition.Substring(1);
                Predicate = false;
            }
            else
            {
                Flag = condition;
                Predicate = true;
            }
        }

        public override string ToString()
        {
            return Predicate ? Flag : $"!{Flag}";
        }
    }
}