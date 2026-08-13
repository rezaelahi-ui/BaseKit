namespace BaseKit.Guards
{
    /// <summary>نقطه‌ی ورود برای guard clauseها: <c>Guard.Against.Null(x, nameof(x))</c>.</summary>
    public static class Guard
    {
        /// <summary>نقطه‌ی شروع فراخوانی guard clauseها، مثل <c>Guard.Against.Null(...)</c>.</summary>
        public static IGuardClause Against { get; } = new GuardClause();

        private sealed class GuardClause : IGuardClause
        {
        }
    }

    /// <summary>مارکر اینترفیس برای امکان extension-based بودن guard clauseها.</summary>
    public interface IGuardClause
    {
    }
}
