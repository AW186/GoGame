using System;
using System.Collections;

namespace AWFrameWork
{
	public interface Scene
	{
		SpriteBatch Batch { get; set; }
        public void Load();
		public void Update(GameTime time);
		public void Draw(GameTime time);
		public void AddSprite(Sprite s);
		public void Remove(Sprite s);
		public void RemoveAllSprite();
	}
}

