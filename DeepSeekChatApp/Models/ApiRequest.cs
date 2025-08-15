using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSeekChatApp.Models
{
    /// <summary>
    /// API 请求模型
    /// </summary>
    class ApiRequest
    {
        public string Model { get; set; } = AppConfig.ModelName;// 模型名称，默认为配置中的模型名称:
                                                                // 这里本来是引用不了了的，因为不小心把APPconfig中的常量改成了redonly,就会报一个CS0120:对象对于引用非静态类的字段，方法或属性“”是必须的
                                                                //解决方法：1.改成const,2,改成静态类
        
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();// 消息列表，包含用户和助手的对话内容
        public int MaxTokens { get; set; } = 2000;// 最大令牌数，默认为2000
       /* 首先：
            1.令牌：是模型在处理文本的时候使用的基本单位，可以是单词，符号。。也可以是一个词的一部分
            2.最大令牌数：就是一段文本在输入和输出的总令牌数不能超过这个数，不然可能会截断*/

        

        public double Temperature { get; set; } = 0.7;// 温度参数，控制生成文本的随机性，默认为0.7

      /*  对于温度的解释：
            1.温度就是控制文本输出随机性的参数，它决定了模型在生成每个令牌时选择概率分布的：平滑度
            2.平滑度是指模型在分配可能输出的令牌概率
            3.总的来说就是，温度越高，平滑度越高，创造性越强*/
           


    }
}
