using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int currentScore = 0;

        public void Roll(int pins)
        {
            currentScore += pins;
        }

        public int GetScore()
        {
            return currentScore;
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
    }
}
