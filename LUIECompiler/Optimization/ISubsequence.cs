namespace LUIECompiler.Optimization
{
    public interface ISubsequence<T>
    {
        public int StartIndex { get; }
        public T Parent { get; }

        /// <summary>
        /// Returns a sequence of type <typeparamref name="T"/> where the subsequence in the <see cref="Parent"/> is replace with the given <paramref name="replacement"/>.
        /// </summary>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public T Replace(T replacement);
    }
}