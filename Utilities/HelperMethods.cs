using System;
using System.Threading.Tasks;
using JetBrains.Annotations;


namespace ErikTheCoder.Utilities
{
    [UsedImplicitly]
    public static class AsyncHelper
    {
        // ReSharper disable UnusedMember.Global
        public static Task MaterializeTask(Func<Task> Lambda) => Lambda();
        public static Task<TResult> MaterializeTask<TResult>(Func<Task<TResult>> Lambda) => Lambda();
        public static Task<TResult> MaterializeTask<T, TResult>(T Arg, Func<T, Task<TResult>> Lambda) => Lambda(Arg);
        public static Task<TResult> MaterializeTask<T1, T2, TResult>(T1 Arg1, T2 Arg2, Func<T1, T2, Task<TResult>> Lambda) => Lambda(Arg1, Arg2);
        public static Task<TResult> MaterializeTask<T1, T2, T3, TResult>(T1 Arg1, T2 Arg2, T3 Arg3, Func<T1, T2, T3, Task<TResult>> Lambda) => Lambda(Arg1, Arg2, Arg3);
        public static Task<TResult> MaterializeTask<T1, T2, T3, T4, TResult>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, Func<T1, T2, T3, T4, Task<TResult>> Lambda) => Lambda(Arg1, Arg2, Arg3, Arg4);
        public static Task<TResult> MaterializeTask<T1, T2, T3, T4, T5, TResult>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, Func<T1, T2, T3, T4, T5, Task<TResult>> Lambda) => Lambda(Arg1, Arg2, Arg3, Arg4, Arg5);
        public static Task<TResult> MaterializeTask<T1, T2, T3, T4, T5, T6, TResult>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, Func<T1, T2, T3, T4, T5, T6, Task<TResult>> Lambda) => Lambda(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        public static Task<TResult> MaterializeTask<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> Lambda) => Lambda(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        public static Task<TResult> MaterializeTask<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> Lambda) => Lambda(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        // ReSharper restore UnusedMember.Global
    }
}
