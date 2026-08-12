namespace BaseKit.Guards
{
    /// <summary>نقطه‌ی ورود برای guard clauseها: <c>Guard.Against.Null(x, nameof(x))</c>.</summary>
    public static class Guard
    {
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
