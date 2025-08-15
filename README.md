# ChatApp

因为好奇，就调用了其实是调用了kimi的api做ChatApp
主要是一个基于Windows Forms的桌面应用程序，它利用 API提供智能聊天功能。这个应用程序允许用户与AI助手进行文本对话，并提供了清晰的用户界面和实用的功能。

## 功能特点

- **直观的用户界面**：简洁美观的Windows Forms界面，易于操作
- **实时聊天功能**：通过DeepSeek API与AI助手进行对话
- **聊天历史记录**：自动保存并显示对话历史
- **清除对话**：一键清除当前对话历史
- **调试信息**：查看API请求的详细信息，便于开发和调试
- **自定义配置**：支持通过配置文件设置API密钥和模型参数

## 技术栈

- **编程语言**：C#
- **框架**：.NET Framework
- **UI技术**：Windows Forms
- **网络通信**：HttpClient
- **JSON处理**：System.Text.Json
- **API服务**：DeepSeek API

## 项目结构

```
DeepSeekChatApp/
├── App.config            # 应用程序配置文件
├── AppConfig.cs          # 配置读取类
├── DeepSeekChatApp.csproj # 项目文件
├── MainForm.Designer.cs  # 主窗体设计器代码
├── MainForm.cs           # 主窗体逻辑代码
├── Models/
│   ├── ApiRequest.cs     # API请求模型
│   ├── ApiResponse.cs    # API响应模型
│   └── ChatMessage.cs    # 聊天消息模型
├── Program.cs            # 程序入口
├── Properties/           # 属性文件夹
└── Services/
    ├── DeepSeekService.cs # DeepSeek API服务实现
    └── IDeepSeekService.cs # DeepSeek API服务接口
```

## 安装说明

1. **前提条件**
   - 安装Visual Studio 2019或更高版本
   - .NET Framework 4.7.2或更高版本
   - 有效的DeepSeek API密钥

2. **克隆项目**
   ```bash
   git clone https://github.com/yourusername/DeepSeekChatApp.git
   ```

3. **配置API密钥**
   - 打开App.config文件
   - 在appSettings部分中添加或修改以下内容：
     ```xml
     <add key="ApiKey" value="你的DeepSeek API密钥" />
     <add key="ApiUrl" value="https://api.deepseek.com/v1/chat/completions" />
     <add key="ModelName" value="deepseek-chat" />
     ```

4. **构建项目**
   - 在Visual Studio中打开解决方案
   - 选择生成 -> 生成解决方案

## 使用方法

1. **启动应用程序**
   - 直接运行生成的可执行文件或在Visual Studio中按F5运行

2. **进行聊天**
   - 在输入框中输入消息
   - 点击"发送"按钮或按Enter键发送消息
   - 等待AI助手回复

3. **清除对话**
   - 点击"清除"按钮清除当前对话历史

4. **查看调试信息**
   - 点击"调试"按钮查看最后一次API请求的详细信息

## 贡献指南

欢迎对本项目进行贡献！如果你有任何建议或改进，请提交Pull Request或Issue。

## 许可证

本项目采用MIT许可证。详情请见LICENSE文件。
