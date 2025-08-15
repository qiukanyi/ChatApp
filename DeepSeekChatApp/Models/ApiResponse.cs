using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeepSeekChatApp.Models
{


    #region API响应模型
    /// <summary>
    /// API 响应模型
    /// </summary>
    /// 直接在kimi的api文档中可以看到，但是不是一模一样的
    class ApiResponse
    {
        /// API 响应的唯一标识符
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// API 响应的对象类型
        [JsonPropertyName("object")]
        public string ObjectType { get; set; } = string.Empty;

        /// API 响应的创建时间戳
        [JsonPropertyName("created")]
        public long Created { get; set; }

        //API响应的模型
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        //选项列表
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = new List<Choice>();

        //使用的信息
        [JsonPropertyName("usage")]
        public UsageInfo Usage { get; set; } = new UsageInfo();

        /// <summary>
        /// 
        /// </summary>
        public class Choice
        {
            [JsonPropertyName("index")]
            public int Index { get; set; }

            [JsonPropertyName("message")]
            public ChatMessage Message { get; set; } = new ChatMessage();

            [JsonPropertyName("finish_reason")]
            public string FinishReason { get; set; } = string.Empty;
        }

        public class UsageInfo
        {
            [JsonPropertyName("prompt_tokens")]
            public int PromptTokens { get; set; }

            [JsonPropertyName("completion_tokens")]
            public int CompletionTokens { get; set; }

            [JsonPropertyName("total_tokens")]
            public int TotalTokens { get; set; }
        }
    }
    #endregion
}
