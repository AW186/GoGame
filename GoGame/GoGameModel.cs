
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
namespace GoGame
{
	public class GoGameModel
	{
		private bool scoreMode = false;
		public enum Turn
		{
			white = -1,
			black = 1
		}
		private Point[] directions = { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
		private Turn turns = Turn.black;
		private Point deadPoint = new Point(-1, -1);
		public Turn Turns
		{
			get
			{
				return turns;
			}
		}
		private int[,] board = new int[19,19];
		public int[,] Board
		{
			get
			{
				return board;
			}
		}
		public GoGameModel()
		{
		}
		private void countChess(Queue<Point> whiteChess, Queue<Point> blackChess)
		{
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    if (board[x, y] == 1)
                    {
                        blackChess.Enqueue(new Point(x, y));
                    }
                    else if (board[x, y] == -1)
                    {
                        whiteChess.Enqueue(new Point(x, y));
                    }
                }
            }
        }
		public void bfsEnqueuePoint(Queue<Point> q, Point node, int mark)
		{
            foreach (Point dir in directions)
            {
                int newX = node.X + dir.X;
                int newY = node.Y + dir.Y;
                if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19 && board[newX, newY] == 0)
                {
                    q.Enqueue(new Point(newX, newY));
					board[newX, newY] = mark;
                }
            }
        }
		public bool SwitchScoreMode()
		{
			if (scoreMode)
			{
				removeScore();
            } else
			{
				showScore();
			}
			scoreMode = !scoreMode;
			return scoreMode;
		}
		private void removeScore()
		{
			for (int x = 0; x < 19; x++)
			{
                for (int y = 0; y < 19; y++)
                {
					if (board[x, y] > 1 || board[x, y] < -1)
						board[x, y] = 0;
                }
            }
		}
        private void showScore()
		{
			Queue<Point> whiteChess = new Queue<Point>();
            Queue<Point> blackChess = new Queue<Point>();
			countChess(whiteChess, blackChess);

            while (whiteChess.Count > 0 || blackChess.Count > 0)
			{
				int whiteCount = whiteChess.Count;
				int blackCount = blackChess.Count;
                for (int i = 0; i < blackCount; i++)
                {
					bfsEnqueuePoint(blackChess, blackChess.Dequeue(), 2);
                }
                for (int i = 0; i < whiteCount; i++)
                {
                    bfsEnqueuePoint(whiteChess, whiteChess.Dequeue(), -2);
                }
			}
        }
		public int GetScore(Turn side)
		{
			int count = 0;
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
					count += (board[x, y] * (int)side > 0) ? 1 : 0;
                }
            }
            return count;
		}
		public void skip()
		{
            turns = (Turn)(-(int)turns);
        }
		public void PlaceChessOnBoard(Point point)
		{
			PlaceChessOnBoard(point.X, point.Y);
		}
		public void PlaceChessOnBoard(int x, int y)
		{
            if (x < 0 || x >= 19 || y < 0 || y >= 19)
			{
				return;
			}

            if (x == deadPoint.X && y == deadPoint.Y || board[x,y] != 0)
			{
				return;
            }
            board[x, y] = (int)Turns;
            int killed = kill(x, y, (Turn)(-(int)turns));
			Point recordDead = deadPoint;
			deadPoint = new Point(-1, -1);
            if (killed == 0)
            {
                bool[,] visited = new bool[19, 19];
                Queue<Point> group = getGroup(new Point(x,y), visited, Turns);
				if (isDead(group))
				{
					board[x, y] = 0;
					return;
				}
            } else if (killed == 1)
            {
                bool[,] visited = new bool[19, 19];
                Queue<Point> group = getGroup(new Point(x, y), visited, Turns);
				if (group.Count == 1)
				{
					deadPoint = recordDead;
				}
            }
			turns = (Turn)(-(int)turns);
		}
		private int kill(int x, int y, Turn side)
		{
			int killed = 0;
			Console.Out.Write("Kill method {0:D}\n", (int)side);
            foreach (Point dir in directions)
            {
                int newX = x + dir.X;
                int newY = y + dir.Y;

                bool[,] visited = new bool[19, 19];
                if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19
					&& !visited[newX, newY] && board[newX, newY] == (int)side)
                {
                    Queue<Point> group = getGroup(new Point(newX, newY), visited, side);
                    if (isDead(group))
                    {
						deadPoint = new Point(newX, newY);
                        killed += group.Count();
						while(group.Count() > 0)
						{
							Point remove = group.Dequeue();
							board[remove.X, remove.Y] = 0;
                        }
					}
                }
            }
			return killed;
		}
		private Queue<Point> getGroup(Point start, bool[,] visited, Turn side)
		{
			Queue<Point> q = new Queue<Point>();
			Queue<Point> group = new Queue<Point>();
			group.Enqueue(start);
			q.Enqueue(start);
			visited[start.X,start.Y] = true;
			while (q.Count > 0)
			{
				Point node = q.Dequeue();
				foreach (Point dir in directions)
				{
					int newX = node.X + dir.X;
                    int newY = node.Y + dir.Y;
					if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19
						&& !visited[newX,newY] && board[newX, newY] == (int)side)
					{
                        visited[newX, newY] = true;
                        q.Enqueue(new Point(newX, newY));
                        group.Enqueue(new Point(newX, newY));
                    }
                }
			}
			return group;
		}
		bool isDead(Queue<Point> group) { 
			foreach (Point p in group)
			{
                foreach (Point dir in directions)
                {
                    int newX = p.X + dir.X;
                    int newY = p.Y + dir.Y;
                    if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19 && board[newX, newY] == 0)
                    {
						Console.Out.Write("Alive by {0:D}, {1:D}\n", newX, newY);
						return false;
                    }
                }
            }
            return true;
        }
	}
}

