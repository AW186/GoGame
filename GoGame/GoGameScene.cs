using System;
using AWFrameWork;
namespace GoGame
{
	public class GoGameScene: AWScene
	{
		private GoGameModel model = new GoGameModel();
		private Sprite board = new GameBoardSprite();
		public GoGameScene()
		{
			this.AddSprite(board);
		}

	}
}

