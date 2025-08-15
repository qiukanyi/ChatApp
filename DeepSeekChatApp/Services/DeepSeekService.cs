using DeepSeekChatApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;





namespace DeepSeekChatApp.Services
{
    /// <summary>
    /// DeepSeek API 服务类
    /// </summary>
    /// 相当于在这里处理 DeepSeek API 的请求和响应逻辑。
    /// 相当于水管连接自来水厂
    public class DeepSeekService : IDeepSeekService, IDisposable
    {
        
        private readonly HttpClient _httpClient;// HttpClient 实例，用于发送 HTTP 请求
        private readonly List<ChatMessage> _conversationHistory = new List<ChatMessage>();// 对话历史记录，存储用户和助手的消息
        private ApiRequest _lastRequest;// 最后一次 API 请求的记录，用于调试和查看请求内容




        #region 初始化HttpClient 并且设置请求头
        /// <summary>
        /// 构造函数，初始化 HttpClient 并设置请求头
        /// </summary>
        public DeepSeekService()
        {
            _httpClient = new HttpClient();// 创建 HttpClient 实例
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppConfig.ApiKey}");// 添加授权头，使用配置中的 API 密钥
            _conversationHistory.Add(new ChatMessage("system", "你是一个乐于助人的助手。请用中文回答用户的问题。"));// 初始化对话历史记录，添加系统消息
        }

        /// <summary>
        /// 发送聊天消息到 DeepSeek API，并获取助手的回复
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> SendChatMessageAsync(string userMessage)
        {
            // 添加用户消息到历史记录

            _conversationHistory.Add(new ChatMessage("user", userMessage));

            // 创建符合 API 要求的请求对象
            var apiRequest = new
            {
                model = AppConfig.ModelName,
                messages = _conversationHistory.Select(m => new
                {
                    role = m.Role,
                    content = m.Content
                }),
                max_tokens = 2000,
                temperature = 0.7
            };

            // 保存最后一次请求
            _lastRequest = new ApiRequest
            {
                Model = AppConfig.ModelName,
                Messages = _conversationHistory,
                MaxTokens = 2000,
                Temperature = 0.7
            };

            // 发送请求
            var response = await SendApiRequestAsync(apiRequest);

            // 处理响应
            if (response.Choices == null || response.Choices.Count == 0)
                throw new Exception("API 没有返回任何结果");

            // 修正：使用大写开头的 Content
            var assistantMessage = response.Choices[0].Message.Content;

            // 添加助手消息到历史记录
            _conversationHistory.Add(new ChatMessage("assistant", assistantMessage));

            return assistantMessage;
        }

        /// <summary>
        /// 获取最后一次请求的 JSON 字符串
        /// </summary>
        /// <returns></returns>
        public string GetLastRequestJson()
        {
            if (_lastRequest == null)
                return "无历史请求";

            try
            {
                // 使用可读性更好的格式化输出
                return JsonSerializer.Serialize(_lastRequest, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }
            catch (Exception ex)
            {
                return $"序列化失败: {ex.Message}";
            }
        }


        /// <summary>
        /// 清除对话历史记录
        /// </summary>
        public void ClearConversationHistory()
        {
            _conversationHistory.Clear();
            _conversationHistory.Add(new ChatMessage("system", "你是一个乐于助人的助手。请用中文回答用户的问题。"));
        }


        /// <summary>
        /// 发送 API 请求到 DeepSeek，并返回响应
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        private async Task<ApiResponse> SendApiRequestAsync(object requestData)
        {
            try
            {
                // 序列化请求数据
                var jsonContent = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // 日志记录
                Debug.WriteLine($"[API 请求] URL: {AppConfig.ApiUrl}");
                Debug.WriteLine($"[API 请求] 内容: {jsonContent}");

                // 发送请求
                var response = await _httpClient.PostAsync(AppConfig.ApiUrl, content);

                // 获取响应
                var statusCode = (int)response.StatusCode;
                var responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[API 响应] 状态码: {statusCode}");
                Debug.WriteLine($"[API 响应] 内容: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    // 尝试解析错误信息
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent);
                        throw new Exception($"API 错误: {errorResponse?.Error?.Message ?? responseContent}");
                    }
                    catch
                    {
                        throw new Exception($"API 请求失败: {response.StatusCode}\n{responseContent}");
                    }
                }

                // 解析响应 - 使用正确的设置
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                if (apiResponse == null)
                {
                    throw new Exception("API 响应解析为 null");
                }

                if (apiResponse.Choices == null || apiResponse.Choices.Count == 0)
                {
                    Debug.WriteLine("[警告] API 返回了空结果集");
                }

                return apiResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API 异常] {ex}");
                throw;
            }
        }

        public void Dispose() => _httpClient.Dispose();
    }

    // 添加错误响应模型
    public class ApiErrorResponse
    {
        [JsonPropertyName("error")]
        public ErrorInfo Error { get; set; }

        public class ErrorInfo
        {
            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("param")]
            public string Param { get; set; }

            [JsonPropertyName("code")]
            public string Code { get; set; }
        }

    }
    #endregion
}

