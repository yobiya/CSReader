namespace CSReader.Analyze
{
    /// <summary>
    /// 重複しないIDを生成する
    /// </summary>
    public class UniqueIdGenerator
    {
        private int _uniqueId;

        /// <summary>
        /// 1以上のユニークなIDを生成する
        /// </summary>
        /// <returns>ユニークなID</returns>
        public int Generate()
        {
            _uniqueId++;

            return _uniqueId;
        }
    }
}
