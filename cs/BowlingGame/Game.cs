using System.Linq;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace BowlingGame
{
    public class Game
    {
        private class Frame
        {
            public int? FirstRoll { get; private set; }
            public int? SecondRoll { get; private set; }
            public int? Bonus { get; private set; }

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
                Bonus = bonus;
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
                (Bonus ?? 0);
        }

        private List<(int BonusRepeats, Frame Frame)> superFrames = new List<(int, Frame)>(); //Todo нормальное название надо)

        private List<Frame> frames = new List<Frame>(10);

        public void Roll(int pins)
        {
            AddBonuses(pins);

            var frame = GetCurrentFrame();

            frame.AddRoll(pins);

            if (frame.IsSpare)
                superFrames.Add((1, frame));
            if (frame.IsStrike)
                superFrames.Add((2, frame));
        }

        public int GetScore()
        {
            return frames
                .Select(frame => frame.Score)
                .Sum();
        }

        private Frame GetCurrentFrame()
        {
            if (frames.Count == 0)
                frames.Add(new Frame());

            var lastFrame = frames[frames.Count - 1];
            if (!lastFrame.IsOver)
                return lastFrame;

            var newFrame = new Frame();
            frames.Add(newFrame);
            return newFrame;
        }

        private void AddBonuses(int pins)
        {
            if (superFrames.Count == 0)
                return;

            for (int i = superFrames.Count - 1; i >= 0; i--)
            {
                superFrames[i].Frame.AddBonus(pins);
                if (superFrames[i].BonusRepeats == 1)
                {
                    superFrames.RemoveAt(i);
                    continue;
                }
                superFrames[i] = (1, superFrames[i].Frame);
            }
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void AddScore_AfterRoll()
        {
            var game = new Game();

            game.Roll(5);

            game.GetScore()
                .Should().Be(5);
        }

        [Test]
        public void DoubleNextScore_AfterSpare()
        {
            var game = new Game();

            game.Roll(5);
            game.Roll(5);
            game.Roll(5);

            game.GetScore()
                .Should().Be(20);
        }

        [Test]
        public void DoubleNextScores_AfterStrike()
        {
            var game = new Game();

            game.Roll(10);
            game.Roll(2);
            game.Roll(2);

            game.GetScore()
                .Should().Be(18);
        }
    }
}
