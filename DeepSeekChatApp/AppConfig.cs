using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSeekChatApp
{
    /// <summary>
    /// 配置好钥匙
    /// </summary>
   public static  class AppConfig
    {
        /*// 修改成这两行
        public const string ApiUrl = "http://localhost:11434/v1/chat/completions";
        public const string ModelName = "llama3"; // 和你下载的模型名一致
*/
        //这是kimi的
        public const string ApiKey = "sk-HMPFHWZiNn6CoR8yWZzAKcqQVTBcH2dJEiOC0WHzhOIBFVFj";
        public const string ApiUrl = "https://api.moonshot.cn/v1/chat/completions";
        public const string ModelName = "moonshot-v1-8k";//  模型名称
    }
}
