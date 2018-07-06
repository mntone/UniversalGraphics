using System;
using System.Collections.Generic;

namespace UniversalGraphics.Test.Infrastructures
{
	public static  class DisposableExtensions
    {
		public static T AddTo<T>(this IDisposable disposable, T collection)
			where T : ICollection<IDisposable>
		{
			collection.Add(disposable);
			return collection;
		}
	}
}
