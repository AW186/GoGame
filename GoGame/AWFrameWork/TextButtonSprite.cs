using System;
namespace AWFrameWork
{
	public class TextButtonSprite: TextSprite
	{
		private Action action;

		public TextButtonSprite(String text, SpriteFont font, Action action) : base(text, font)
        {
			this.action = action;
		}
        public override void click(MouseState state)
        {
			action();
            base.click(state);
        }
    }
}

