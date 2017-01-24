using System.Collections.Generic;

namespace Syntax
{
	class Context<T, U>
	{
		#region Constructors

		public Context()
		{

		}

		#endregion

		#region Private Fields

		private List<Dictionary<T, U>> backing = new List<Dictionary<T, U>>();

		#endregion

		#region Public Methods

		public Context<T, U> PushFrame()
		{
			backing.Add(new Dictionary<T, U>());
			return this;
		}

		public void PopFrame()
		{
			backing.RemoveAt(backing.Count - 1);
		}

		public void AddToCurrent(T key, U value)
		{
			backing[backing.Count - 1].Add(key, value);
		}

		public Dictionary<T, U> GetEffective()
		{
			Dictionary<T, U> result = new Dictionary<T, U>();

			foreach (Dictionary<T, U> list in backing)
			{
				foreach (KeyValuePair<T, U> kv in list)
					result.Add(kv.Key, kv.Value);
			}

			return result;
		}

		#endregion
	}
}
