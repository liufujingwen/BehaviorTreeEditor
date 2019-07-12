using System;
using System.Collections.Generic;

namespace Serializable
{
	public class RefTable<T>
	{
		public RefTable ()
		{
			init ();
		}

		private List<T> values;
		private int index;

		/**
		 * 初始化对象
		 */
		public void init ()
		{
			values = new List<T> ();
			index = 0;
		}

		/**
		 * 销毁对象
		 */
		public void destroy ()
		{
			values = null;
			index = 0;
		}

		public int incrementAndGet ()
		{
			index++;
			return index;
		}

		public int getKey (T value)
		{
			int idx = values.IndexOf(value);
			return idx > -1 ? idx + 1 : -1;
		}

		public T getValue (int key)
		{
			if (key > values.Count) {
				return default(T);
			}
			return values[key - 1];
		}

		public void put (int key, T value)
		{
			int idx = key - 1;
			if (idx < values.Count) {
				values[idx] = value;
			}
			if (idx > index) {
				// 插入空元素
				for (int i = index - 1; i < idx; i++) {
					values.Add(default(T));
				}
			}
			values.Add(value); 
		}

	}
}

