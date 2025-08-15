using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSeekChatApp.Models
{
    #region 聊天响应模型
    /// <summary>
    /// 聊天消息模型：定义了聊天消息的数据结构，包括角色、内容等信息。
    /// </summary>
    class ChatMessage
    {
        public string Role { get; set; } = "user";// 默认角色为用户
        public string Content { get; set; } = string.Empty;// 消息内容
        //public DateTime Timestamp { get; set; } = DateTime.Now;// 消息时间戳(api不支持）

        public ChatMessage() { }// 默认构造函数


        /// <summary>
        /// 构造函数，初始化角色和内容
        /// </summary>
        /// <param name="role"></param>
        /// <param name="content"></param>
        public ChatMessage(string role, string content)
        {
            Role = role;
            Content = content;
        }
    }
    #endregion 
}
