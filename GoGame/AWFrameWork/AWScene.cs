using System;

namespace AWFrameWork
{
	public class AWScene : Scene
	{
        private List<Sprite> sprites = new List<Sprite>();

        public AWScene()
		{

		}
        private SpriteBatch batch;
        public SpriteBatch Batch {
            get {
                return batch;
            }
            set
            {
                batch = value;
            }
        }

        public void AddSprite(Sprite sprite)
        {
            sprites.Add(sprite);
            sprite.Load();
        }

        public void Draw(GameTime time)
        {
            foreach (Sprite sprite in sprites)
            {
                Batch.Draw(sprite.Graphics, sprite.Frame, sprite.InputFrame, sprite.Tint);
            }
        }

        public void Load()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Load();
            }
        }

        public void Remove(Sprite sprite)
        {
            sprites.Remove(sprite);
        }

        public void RemoveAllSprite()
        {
            while (sprites.Count > 0)
            {
                sprites.RemoveAt(0);
            } 
        }

        public void Update(GameTime time)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Update(time);
            }
        }
    }
}

