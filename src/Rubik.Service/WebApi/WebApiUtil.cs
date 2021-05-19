using System;
using System.Net.Http;
using System.Threading.Tasks;

using WebApiClientCore;
using WebApiClientCore.Exceptions;

using Rubik.Service.Log;

namespace Rubik.Service.Github.WebApi
{
    /// <summary>
    /// WebApi 帮助类
    /// </summary>
    public static class WebApiUtil
    {
        /// <summary>
        /// 用于捕获异步方法 func 的异常
        /// </summary>
        public static async Task<string> Catch(Func<Task> func)
        {
            try
            {
                await func.Invoke();
                return null;
            }
            catch (HttpRequestException ex) when (ex.InnerException is TaskCanceledException taskCanceledException)
            {
                Logger.Instance.WebApi.Error("[ WebApiUtil.Catch ] Request timed out");
                return "Request timed out, please try again!";
            }
            catch (Exception ex)
            {
                Logger.Instance.WebApi.Error($"[ WebApiUtil.Catch ] {ex.Message}");
                return $"Request failed, please try again!\n(Error: {ex.Message})";
            }
        }

        /// <summary>
        /// 用于捕获异步方法 func 的异常
        /// </summary>
        public static async Task<(T Output, string Error)> Catch<T>(Func<Task<T>> func)
        {
            try
            {
                var output = await func.Invoke();
                return (output, null);
            }
            catch (HttpRequestException ex) when (ex.InnerException is TaskCanceledException taskCanceledException)
            {
                Logger.Instance.WebApi.Error("[ WebApiUtil.Catch ] Request timed out");
                return (default(T), "Request timed out, please try again!");
            }
            catch (Exception ex)
            {
                Logger.Instance.WebApi.Error($"[ WebApiUtil.Catch ] {ex.Message}");
                return (default(T), $"Request failed, please try again!\n(Error: {ex.Message})");
            }
        }

        /// <summary>
        /// 用于捕获异步方法 func 的异常，可重试
        /// </summary>
        /// <typeparam name="T">接口返回数据类型</typeparam>
        /// <param name="func">接口</param>
        /// <param name="func">重试条件</param>
        /// <param name="retryMaxCount">重试次数</param>
        /// <param name="retryDelay">重试延时(s)</param>
        /// <returns></returns>
        public static async Task<(T Output, string Error)> CatchWithRetry<T>(Func<ITask<T>> func, Func<T,bool> retryCondition, int retryMaxCount = 3, int retryDelay = 1)
        {
            try
            {
                var output = await func.Invoke()
                    .Retry(retryMaxCount, TimeSpan.FromSeconds(retryDelay))
                    .WhenResult(r => retryCondition(r));

                return (output, null);
            }
            catch (HttpRequestException ex) when (ex.InnerException is TaskCanceledException taskCanceledException)
            {
                Logger.Instance.WebApi.Error("[ WebApiUtil.CatchWithRetry ] Request timed out");
                return (default(T), "Request timed out, please try again!");
            }
            catch (ApiRetryException ex)
            {
                Logger.Instance.WebApi.Error($"[ WebApiUtil.CatchWithRetry ] {ex.Message}");
                return (default(T), $"Retried too many times, retryTimes = {retryMaxCount}!");
            }
            catch (Exception ex)
            {
                Logger.Instance.WebApi.Error($"[ WebApiUtil.CatchWithRetry ] {ex.Message}");
                return (default(T), $"Request failed, please try again!\n(Error: {ex.Message})");
            }
        }
    }
}