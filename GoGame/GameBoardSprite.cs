using System;
using AWFrameWork;
namespace GoGame
{
	public class GameBoardSprite: AWSprite
	{
		public GameBoardSprite()
		{

		}

        public override void Load()
        {
            graphics = Game1.Content.Load<Texture2D>("Blank_Go_board");
            inputFrame = new Rectangle(0, 0, 2000, 2000);
            Frame = new Rectangle(20, 20, 440, 440);
            Console.Out.Write("print board");
        }
    }
}

