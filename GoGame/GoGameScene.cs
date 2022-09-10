using System;
using AWFrameWork;
namespace GoGame
{
	public class GoGameScene: AWScene
	{
		private int chessSize = 18;
		private GoGameModel model = new GoGameModel();
		private Sprite board = new GameBoardSprite();
		private List<Sprite> chesses = new List<Sprite>();
		private TextButtonSprite score;
        private TextButtonSprite skip;
        private TextButtonSprite newGame;
        private TextSprite scoreText;
        private TextSprite turnText;
        private bool showScore = false;
        private int boardSize
		{
			get
			{
				return board.Frame.Width * (2000 - 120) / 2000;
			}
		}
        private int boardPadding
        {
            get
            {
                return board.Frame.Width * 60 / 2000;
            }
        }
		public GoGameScene()
        {
            
        }
		public override void Load()
		{
            turnText = new TextSprite("black turn", Game1.Content.Load<SpriteFont>("Arial"));
            scoreText = new TextSprite("", Game1.Content.Load<SpriteFont>("Arial"));
            score = new TextButtonSprite("Score", Game1.Content.Load<SpriteFont>("Arial"), () => {
                showScore = model.SwitchScoreMode();
                
            });
            skip = new TextButtonSprite("Skip", Game1.Content.Load<SpriteFont>("Arial"), () => {
                model.skip();
            });
            newGame = new TextButtonSprite("New Game", Game1.Content.Load<SpriteFont>("Arial"), () => {
                model = new GoGameModel();
            });
            score.Frame = new Rectangle(500, 100, 200, 100);
            skip.Frame = new Rectangle(500, 200, 200, 100);
            scoreText.Frame = new Rectangle(500, 120, 200, 100);
            turnText.Frame = new Rectangle(500, 220, 200, 100);
            newGame.Frame = new Rectangle(500, 250, 200, 100);
            this.AddSprite(turnText);
            this.AddSprite(score);
            this.AddSprite(board);
            this.AddSprite(skip);
            this.AddSprite(newGame);
        }
        private void updateScoreBoard()
        {
            if (showScore) {
                scoreText.Text =
                    "black: " + model.GetScore(GoGameModel.Turn.black) + "\n"
                    + "white: " + model.GetScore(GoGameModel.Turn.white);
                this.AddSprite(scoreText);
            } else
            {
                scoreText.RemoveFromScene();
            }
        }
        private void updateChessBoard(MouseState mstate)
        {
            foreach (Sprite chess in chesses)
            {
                chess.RemoveFromScene();
            }
            chesses = new List<Sprite>();
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    if (model.Board[x, y] == (int)GoGameModel.Turn.black)
                    {
                        chesses.Add(new ChessSprite(GoGameModel.Turn.black,
                            centerToFrame(new Point(x, y), chessSize)));
                    }
                    else if (model.Board[x, y] == (int)GoGameModel.Turn.white)
                    {
                        chesses.Add(new ChessSprite(GoGameModel.Turn.white,
                            centerToFrame(new Point(x, y), chessSize)));
                    }
                    if (model.Board[x, y] == (int)GoGameModel.Turn.black * 2)
                    {
                        chesses.Add(new ChessSprite(GoGameModel.Turn.black,
                            centerToFrame(new Point(x, y), chessSize / 2)));
                    }
                    else if (model.Board[x, y] == (int)GoGameModel.Turn.white * 2)
                    {
                        chesses.Add(new ChessSprite(GoGameModel.Turn.white,
                            centerToFrame(new Point(x, y), chessSize / 2)));
                    }
                }
            }
            foreach (Sprite chess in chesses)
            {
                this.AddSprite(chess);
            }
        }
        private void updateTurnText()
        {
            turnText.Text = model.Turns == GoGameModel.Turn.black ? "balck turn" : "white turn";
        }
        public override void Update(GameTime time, KeyboardState kstate, MouseState mstate)
        {
            base.Update(time, kstate, mstate);
            updateChessBoard(mstate);
            updateScoreBoard();
            updateTurnText();
        }
        public override void click(MouseState state)
		{
			Point point = state.Position;
			model.PlaceChessOnBoard(pointOnBoard(point));
			base.click(state);
		}
		private Rectangle centerToFrame(Point point, int chessSize)
		{
			return new Rectangle((point.X * boardSize / 18 + board.Frame.X + boardPadding) - chessSize / 2,
				(point.Y * boardSize / 18 + board.Frame.Y + boardPadding) - chessSize / 2,
				chessSize, chessSize);
		}
        private Point pointOnBoard(Point input)
		{
			return new Point((input.X - board.Frame.X) * 19 / board.Frame.Width,
                (input.Y - board.Frame.Y) * 19 / board.Frame.Height);
		}
    }
}

