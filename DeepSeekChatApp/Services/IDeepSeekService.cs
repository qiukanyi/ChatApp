using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSeekChatApp.Services
{
    /// <summary>
    /// 接口定义了与DeepSeek聊天服务交互的方法。
    /// </summary>
    /// 类比：
    public interface IDeepSeekService
    {
        Task<string> SendChatMessageAsync(string userMessage);// 发送用户消息并获取回复
        void ClearConversationHistory();// 清除对话历史记录
        void Dispose();// 释放资源
        //获取最后一次请求的json
        string GetLastRequestJson();
    }
}
