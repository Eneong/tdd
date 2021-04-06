namespace BowlingGame
{
    public class Frame
    {
        public int? FirstRoll { get; private set; }
        public int? SecondRoll { get; private set; }
        public int Bonus { get; private set; }

        public void AddRoll(int pins)
        {
            if (FirstRoll == null)
            {
                FirstRoll = pins;
                return;
            }
            SecondRoll = pins;
        }

        public void AddBonus(int bonus)
        {
            Bonus += bonus;
        }

        public bool IsOver =>
             SecondRoll != null || IsStrike;

        public bool IsStrike =>
            FirstRoll == 10;

        public bool IsSpare =>
            !IsStrike &&
            (FirstRoll + SecondRoll == 10);

        public int Score =>
            (FirstRoll ?? 0) +
            (SecondRoll ?? 0) +
            Bonus;
    }
}
