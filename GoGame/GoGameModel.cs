
using Microsoft.Xna.Framework.Graphics;
namespace GoGame
{
	public class GoGameModel
	{
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
		int[,] board = new int[19,19];
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
                int newY = node.X + dir.Y;
                if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19 && board[newX, newY] == 0)
                {
                    q.Enqueue(new Point(newX, newY));
					board[newX, newY] = mark;
                }
            }
        }
        public void showScore()
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
		public int getScore(Turn side)
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
		public void PlaceChessOnBoard(int x, int y)
		{
			if (x == deadPoint.X && y == deadPoint.Y || board[x,y] != 0)
			{
				return;
			}
			int killed = kill(x, y, (Turn)(-(int)turns));
            board[x, y] = (int)Turns;
            if (killed == 0)
            {
                bool[,] visited = new bool[19, 19];
                Queue<Point> group = getGroup(new Point(x,y), visited, Turns);
				if (isDead(group))
				{
					board[x, y] = 0;
					return;
				}
			}
			turns = (Turn)(-(int)turns);
		}
		private int kill(int x, int y, Turn side)
		{
			int killed = 0;
            foreach (Point dir in directions)
            {
                int newX = x + dir.X;
                int newY = x + dir.Y;

                bool[,] visited = new bool[19, 19];
                if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19
					&& !visited[newX, newY] && board[newX, newY] == (int)side)
                {
                    Queue<Point> group = getGroup(new Point(newX, newY), visited, side);
					if (isDead(group))
					{
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
                    int newY = node.X + dir.Y;
					if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19
						&& !visited[newX,newY] && board[newX, newY] == (int)side)
					{
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
                    int newY = p.X + dir.Y;
                    if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19 && board[newX, newY] == 0)
                    {
						return false;
                    }
                }
            }
            return true;
        }
	}
}

