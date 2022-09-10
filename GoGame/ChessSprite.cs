using System;
using AWFrameWork;
namespace GoGame
{
	public class ChessSprite: AWSprite
	{

		public ChessSprite(GoGameModel.Turn type, Rectangle rect)
		{
			base.Frame = rect;
			if (type == GoGameModel.Turn.black)
			{
				inputFrame = new Rectangle(0, 0, 340, 340);
			} else
			{
				inputFrame = new Rectangle(340, 0, 340, 340);
			}
		}

        public override void Load()
        {
			graphics = Game1.Content.Load<Texture2D>("Chess");
        }
    }
}

