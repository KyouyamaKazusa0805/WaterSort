namespace System.Linq;

/// <summary>
/// Provides LINQ-based extension methods on <see cref="ReadOnlySpan{T}"/>.
/// </summary>
/// <seealso cref="ReadOnlySpan{T}"/>
public static class SpanEnumerable
{
	/// <summary>
	/// Returns a new <see cref="ReadOnlySpan{T}"/> instance that contains each element with its corresponding index.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The object to be iterated.</param>
	/// <returns>A new <see cref="ReadOnlySpan{T}"/> instance.</returns>
	public static ReadOnlySpan<(int Index, T Value)> Index<T>(this ReadOnlySpan<T> @this)
	{
		var result = new (int, T)[@this.Length];
		for (var i = 0; i < @this.Length; i++)
		{
			result[i] = (i, @this[i]);
		}
		return result;
	}

	/// <summary>
	/// Finds the first element satisfying the specified condition, and return its corresponding index.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The sequence.</param>
	/// <param name="predicate">The condition.</param>
	/// <returns>
	/// An <see cref="int"/> indicating the found element. -1 returns if the sequence has no element satisfying the condition.
	/// </returns>
	public static int FirstIndex<T>(this ReadOnlySpan<T> @this, Func<T, bool> predicate)
	{
		for (var i = 0; i < @this.Length; i++)
		{
			if (predicate(@this[i]))
			{
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// Finds the last element satisfying the specified condition, and return its corresponding index.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The sequence.</param>
	/// <param name="predicate">The condition.</param>
	/// <returns>
	/// An <see cref="int"/> indicating the found element. -1 returns if the sequence has no element satisfying the condition.
	/// </returns>
	public static int LastIndex<T>(this ReadOnlySpan<T> @this, Func<T, bool> predicate)
	{
		for (var i = @this.Length - 1; i >= 0; i--)
		{
			if (predicate(@this[i]))
			{
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// Try to get the minimal value appeared in the collection.
	/// </summary>
	/// <typeparam name="TNumber">The type of each element.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <returns>The minimal value.</returns>
	public static TNumber Min<TNumber>(this ReadOnlySpan<TNumber> @this) where TNumber : INumber<TNumber>, IMinMaxValue<TNumber>
	{
		var result = TNumber.MaxValue;
		foreach (ref readonly var element in @this)
		{
			if (element <= result)
			{
				result = element;
			}
		}
		return result;
	}

	/// <inheritdoc cref="MinBy{TSource, TKey}(ReadOnlySpan{TSource}, Func{TSource, TKey})"/>
	public static TKey Min<TSource, TKey>(this ReadOnlySpan<TSource> @this, Func<TSource, TKey> keySelector)
		where TKey : IMinMaxValue<TKey>?, IComparisonOperators<TKey, TKey, bool>?
	{
		var resultKey = TKey.MaxValue;
		foreach (var element in @this)
		{
			var key = keySelector(element);
			if (key <= resultKey)
			{
				resultKey = key;
			}
		}
		return resultKey;
	}

	/// <summary>
	/// Returns the minimum value in a generic sequence according to a specified key selector function.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <param name="keySelector">A function to extract the key for each element.</param>
	/// <returns>The value with the minimum key in the sequence.</returns>
	public static TSource? MinBy<TSource, TKey>(this ReadOnlySpan<TSource> @this, Func<TSource, TKey> keySelector)
		where TKey : IMinMaxValue<TKey>?, IComparisonOperators<TKey, TKey, bool>?
	{
		var (resultKey, result) = (TKey.MaxValue, default(TSource));
		foreach (var element in @this)
		{
			if (keySelector(element) <= resultKey)
			{
				result = element;
			}
		}
		return result;
	}

	/// <summary>
	/// Try to get the minimal value appeared in the collection.
	/// </summary>
	/// <typeparam name="TNumber">The type of each element.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <returns>The minimal value.</returns>
	public static TNumber Max<TNumber>(this ReadOnlySpan<TNumber> @this) where TNumber : INumber<TNumber>, IMinMaxValue<TNumber>
	{
		var result = TNumber.MinValue;
		foreach (ref readonly var element in @this)
		{
			if (element >= result)
			{
				result = element;
			}
		}
		return result;
	}

	/// <inheritdoc cref="MaxBy{TSource, TKey}(ReadOnlySpan{TSource}, Func{TSource, TKey})"/>
	public static TKey? Max<TSource, TKey>(this ReadOnlySpan<TSource> @this, Func<TSource, TKey> keySelector)
		where TKey : IMinMaxValue<TKey>?, IComparisonOperators<TKey, TKey, bool>?
	{
		var resultKey = TKey.MinValue;
		foreach (var element in @this)
		{
			var key = keySelector(element);
			if (key >= resultKey)
			{
				resultKey = key;
			}
		}
		return resultKey;
	}

	/// <summary>
	/// Returns the maximum value in a generic sequence according to a specified key selector function.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <param name="keySelector">A function to extract the key for each element.</param>
	/// <returns>The value with the maximum key in the sequence.</returns>
	public static TSource? MaxBy<TSource, TKey>(this ReadOnlySpan<TSource> @this, Func<TSource, TKey> keySelector)
		where TKey : IMinMaxValue<TKey>?, IComparisonOperators<TKey, TKey, bool>?
	{
		var (resultKey, result) = (TKey.MinValue, default(TSource));
		foreach (var element in @this)
		{
			if (keySelector(element) >= resultKey)
			{
				result = element;
			}
		}
		return result;
	}

	/// <summary>
	/// Totals up all elements, and return the result of the sum by the specified property calculated from each element.
	/// </summary>
	/// <typeparam name="T">The type of the elements of source.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <returns>The value with the sum key in the sequence.</returns>
	public static T Sum<T>(this ReadOnlySpan<T> @this) where T : IAdditiveIdentity<T, T>?, IAdditionOperators<T, T, T>?
	{
		var result = T.AdditiveIdentity;
		foreach (ref readonly var element in @this)
		{
			result += element;
		}
		return result;
	}

	/// <inheritdoc cref="Any{T}(ReadOnlySpan{T}, Func{T, bool})"/>
	public static bool Any<T>(this ReadOnlySpan<T> @this, Func<T, bool> match)
	{
		foreach (var element in @this)
		{
			if (match(element))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Checks whether all elements are satisfied the specified condition.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <param name="match">The <see cref="Func{T, TResult}"/> that defines the conditions of the elements to search for.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	public static bool All<T>(this ReadOnlySpan<T> @this, Func<T, bool> match)
	{
		foreach (var element in @this)
		{
			if (!match(element))
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Determines whether all elements are of type <typeparamref name="TDerived"/>.
	/// </summary>
	/// <typeparam name="TBase">The type of each element.</typeparam>
	/// <typeparam name="TDerived">The derived type to be checked.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	public static bool AllAre<TBase, TDerived>(this ReadOnlySpan<TBase> @this) where TDerived : class?, TBase?
	{
		foreach (ref readonly var element in @this)
		{
			if (element is not TDerived)
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Filters instances of type <typeparamref name="TBase"/>, only reserving elements of type <typeparamref name="TDerived"/>.
	/// </summary>
	/// <typeparam name="TBase">The type of base elements.</typeparam>
	/// <typeparam name="TDerived">The type derived from <typeparamref name="TBase"/> to be checked.</typeparam>
	/// <param name="this">The source elements.</param>
	/// <returns>A new <see cref="ReadOnlySpan{T}"/> instance of elements of type <typeparamref name="TDerived"/>.</returns>
	public static ReadOnlySpan<TDerived> OfType<TBase, TDerived>(this ReadOnlySpan<TBase> @this)
		where TBase : class?
		where TDerived : class?, TBase?
	{
		var result = new TDerived[@this.Length];
		var i = 0;
		foreach (ref readonly var element in @this)
		{
			if (element is TDerived derived)
			{
				result[i++] = derived;
			}
		}
		return result;
	}

	/// <summary>
	/// Casts each element from type <typeparamref name="TBase"/> to <typeparamref name="TDerived"/>, without element type checking;
	/// throws if casting operation is invalid.
	/// </summary>
	/// <typeparam name="TBase">The type of each element.</typeparam>
	/// <typeparam name="TDerived">The type of target elements.</typeparam>
	/// <param name="this">The source elements.</param>
	/// <returns>A new <see cref="ReadOnlySpan{T}"/> instance of elements of type <typeparamref name="TDerived"/>.</returns>
	public static ReadOnlySpan<TDerived> Cast<TBase, TDerived>(this ReadOnlySpan<TBase> @this)
		where TBase : class?
		where TDerived : class?, TBase?
	{
		var result = new TDerived[@this.Length];
		var i = 0;
		foreach (ref readonly var element in @this)
		{
			result[i++] = (TDerived)element;
		}
		return result;
	}

	/// <summary>
	/// Skips the specified number of elements, make a new <see cref="ReadOnlySpan{T}"/> instance points to it.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <param name="count">The number of elements to skip.</param>
	/// <returns>
	/// The new instance that points to the first element that has already skipped the specified number of elements.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<T> Skip<T>(this ReadOnlySpan<T> @this, int count) => @this[count..];

	/// <summary>
	/// Retrieves all the elements that match the conditions defined by the specified predicate.
	/// </summary>
	/// <typeparam name="T">The type of the elements of the span.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <param name="match">The <see cref="Func{T, TResult}"/> that defines the conditions of the elements to search for.</param>
	/// <returns>
	/// A <see cref="ReadOnlySpan{T}"/> containing all the elements that match the conditions defined
	/// by the specified predicate, if found; otherwise, an empty <see cref="ReadOnlySpan{T}"/>.
	/// </returns>
	public static ReadOnlySpan<T> FindAll<T>(this ReadOnlySpan<T> @this, Func<T, bool> match)
	{
		var result = new List<T>(@this.Length);
		foreach (ref readonly var element in @this)
		{
			if (match(element))
			{
				result.AddRef(in element);
			}
		}
		return result.AsSpan();
	}

	/// <summary>
	/// Projects each element in the current instance into the target-typed span of element type <typeparamref name="TResult"/>,
	/// using the specified function to convert.
	/// </summary>
	/// <typeparam name="T">The type of each elements in the span.</typeparam>
	/// <typeparam name="TResult">The type of target value.</typeparam>
	/// <param name="this">The collection to be used and checked.</param>
	/// <param name="selector">The selector.</param>
	/// <returns>An array of <typeparamref name="TResult"/> elements.</returns>
	public static ReadOnlySpan<TResult> Select<T, TResult>(this ReadOnlySpan<T> @this, Func<T, TResult> selector)
	{
		var result = new TResult[@this.Length];
		var i = 0;
		foreach (var element in @this)
		{
			result[i++] = selector(element);
		}
		return result;
	}

	/// <inheritdoc cref="Select{T, TResult}(ReadOnlySpan{T}, Func{T, TResult})"/>
	public static ReadOnlySpan<TResult> Select<T, TResult>(this ReadOnlySpan<T> @this, Func<T, int, TResult> selector)
	{
		var result = new TResult[@this.Length];
		var i = 0;
		foreach (var element in @this)
		{
			result[i++] = selector(element, i);
		}
		return result;
	}

	/// <inheritdoc cref="Enumerable.Where{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
	public static ReadOnlySpan<T> Where<T>(this ReadOnlySpan<T> @this, Func<T, bool> predicate)
	{
		var result = new T[@this.Length];
		var i = 0;
		foreach (var element in @this)
		{
			if (predicate(element))
			{
				result[i++] = element;
			}
		}
		return result.AsSpan()[..i];
	}

	/// <inheritdoc cref="Enumerable.First{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
	public static T First<T>(this ReadOnlySpan<T> @this, Func<T, bool> predicate)
	{
		foreach (var element in @this)
		{
			if (predicate(element))
			{
				return element;
			}
		}
		throw new InvalidOperationException();
	}

	/// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
	public static T? FirstOrDefault<T>(this ReadOnlySpan<T> @this, Func<T, bool> predicate)
	{
		foreach (var element in @this)
		{
			if (predicate(element))
			{
				return element;
			}
		}
		return default;
	}

	/// <inheritdoc cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<TSource> Take<TSource>(this ReadOnlySpan<TSource> @this, int count)
	{
		var result = new List<TSource>(count);
		result.AddRangeRef(@this[..Math.Min(count, @this.Length)]);
		return result.AsSpan();
	}

	/// <inheritdoc cref="Enumerable.Take{TSource}(IEnumerable{TSource}, Range)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<TSource> Take<TSource>(this ReadOnlySpan<TSource> @this, Range range)
	{
		var minIndex = range.Start.GetOffset(@this.Length);
		var maxIndex = range.End.GetOffset(@this.Length);
		if (maxIndex <= minIndex)
		{
			throw new InvalidOperationException();
		}

		var result = new List<TSource>(maxIndex - minIndex);
		result.AddRangeRef(@this[Math.Min(minIndex, @this.Length)..Math.Min(maxIndex, @this.Length)]);
		return result.AsSpan();
	}

	/// <inheritdoc cref="Enumerable.Aggregate{TSource}(IEnumerable{TSource}, Func{TSource, TSource, TSource})"/>
	public static TSource Aggregate<TSource>(this ReadOnlySpan<TSource> @this, Func<TSource, TSource, TSource> func)
	{
		var result = default(TSource)!;
		foreach (var element in @this)
		{
			result = func(result, element);
		}
		return result;
	}

	/// <inheritdoc cref="Enumerable.Aggregate{TSource, TAccumulate}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/>
	public static TAccumulate Aggregate<TSource, TAccumulate>(this ReadOnlySpan<TSource> @this, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		where TAccumulate : allows ref struct
	{
		var result = seed;
		foreach (var element in @this)
		{
			result = func(result, element);
		}
		return result;
	}
}
