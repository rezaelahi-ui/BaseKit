namespace BaseKit.Attributes
{
    /// <summary>نوع رابطه‌ی مقایسه‌ای بین دو فیلد، برای <see cref="CompareToAttribute"/>.</summary>
    public enum CompareType
    {
        /// <summary>برابر.</summary>
        Equal,

        /// <summary>نابرابر.</summary>
        NotEqual,

        /// <summary>کمتر.</summary>
        LessThan,

        /// <summary>کمتر یا مساوی.</summary>
        LessThanOrEqual,

        /// <summary>بیشتر.</summary>
        GreaterThan,

        /// <summary>بیشتر یا مساوی.</summary>
        GreaterThanOrEqual,
    }
}
