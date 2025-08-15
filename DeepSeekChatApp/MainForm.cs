using DeepSeekChatApp.Models;
using DeepSeekChatApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSeekChatApp
{
    public partial class MainForm: Form
    {
        
        private readonly IDeepSeekService _deepSeekService;// 深度对话服务实例
        private readonly List<ChatMessage> _displayedMessages = new List<ChatMessage>();// 用于存储显示的消息列表
        public MainForm()
        {
            InitializeComponent();
            _deepSeekService = new DeepSeekService();// 初始化深度对话服务实例
            SetupUi();
        }
        #region 窗体布局
        /// <summary>
        /// 布局
        /// </summary>
        private void SetupUi()
        {
            // 窗体设置
            this.Text = "DeepSeek 聊天助手";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 40);

            // 使用TableLayoutPanel进行布局
            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10),
                BackColor = Color.Transparent
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));


            // 聊天历史面板
            var historyPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(45, 45, 55)
            };
            mainTable.Controls.Add(historyPanel, 0, 0);


            // 输入面板
            var inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));


            // 输入框
            var inputBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(60, 60, 70),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            inputBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter && !e.Shift)
                {
                    e.SuppressKeyPress = true;
                    SendMessage(inputBox.Text);
                    inputBox.Clear();
                }
            };
            inputPanel.Controls.Add(inputBox, 0, 0);

            // 发送按钮
            var sendButton = new Button
            {
                Text = "发送",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Click += (sender, e) =>
            {
                SendMessage(inputBox.Text);
                inputBox.Clear();
            };
            inputPanel.Controls.Add(sendButton, 1, 0);
            // 清除按钮
            var clearButton = new Button
            {
                Text = "清除",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(70, 70, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            clearButton.FlatAppearance.BorderSize = 0;
            clearButton.Click += (sender, e) =>
            {
                _displayedMessages.Clear();
                historyPanel.Controls.Clear();
                _deepSeekService.ClearConversationHistory();
                AddMessageToHistory(new ChatMessage("assistant", "对话已清除。请问有什么可以帮您？"), historyPanel);
            };
            inputPanel.Controls.Add(clearButton, 2, 0);

            mainTable.Controls.Add(inputPanel, 0, 1);
            // 状态栏
            var statusBar = new StatusStrip
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 55),
                RenderMode = ToolStripRenderMode.Professional
            };
            var statusLabel = new ToolStripStatusLabel
            {
                Text = "就绪 | DeepSeek API 聊天助手",
                ForeColor = Color.White
            };
            statusBar.Items.Add(statusLabel);
            mainTable.Controls.Add(statusBar, 0, 2);

            // 添加主布局
            this.Controls.Add(mainTable);

            // 添加欢迎消息
            AddMessageToHistory(new ChatMessage("assistant", "你好！我是DeepSeek AI助手。有什么我可以帮助你的吗？"), historyPanel);
            var debugButton = new Button
            {
                Text = "调试",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(90, 90, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            debugButton.FlatAppearance.BorderSize = 0;
            debugButton.Click += (sender, e) => ShowDebugInfo();
            inputPanel.Controls.Add(debugButton, 3, 0); // 添加到第四列

            // 调整列比例
            inputPanel.ColumnStyles.Clear();
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F)); // 输入框
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F)); // 发送
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F)); // 清除
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F)); // 调试

        }
        #endregion

        #region 调试信息显示方法
        // 🔧 添加调试信息显示方法
        private void ShowDebugInfo()
        {
            try
            {
                string requestJson = _deepSeekService.GetLastRequestJson();

                // 创建调试窗口
                var debugForm = new Form
                {
                    Text = "API 请求调试信息",
                    Size = new Size(600, 400),
                    StartPosition = FormStartPosition.CenterParent
                };

                var textBox = new TextBox
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.Both,
                    Font = new Font("Consolas", 10),
                    Text = requestJson
                };

                debugForm.Controls.Add(textBox);
                debugForm.ShowDialog(this); // 显示为模态对话框
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取调试信息失败: {ex.Message}", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 发送信息
        private async void SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var historyPanel = ((TableLayoutPanel)this.Controls[0]).Controls[0] as Panel;
            AddMessageToHistory(new ChatMessage("user", message), historyPanel);

            try
            {
                SetControlsEnabled(false);

                // 显示加载指示器
                var loadingMessage = new ChatMessage("assistant", "思考中...");
                AddMessageToHistory(loadingMessage, historyPanel);

                string response = await _deepSeekService.SendChatMessageAsync(message);
                //检查是否为空响应
                if (string.IsNullOrWhiteSpace(response))
                {
                    response = "[错误] API 返回了空结果";
                }
                RemoveLoadingMessage(historyPanel);
                AddMessageToHistory(new ChatMessage("assistant", response), historyPanel);
            }
            catch (Exception ex)
            {
                RemoveLoadingMessage(historyPanel);
                //显示更加详细的错误信息
                string errorMsg = $"API 错误: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n内部错误: {ex.InnerException.Message}";
                }
                AddMessageToHistory(new ChatMessage("system", $"错误: {ex.Message}"), historyPanel);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }
        #endregion

        #region  添加历史聊天面板
        /// <summary>
        /// 将消息添加到聊天历史面板中。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="historyPanel"></param>
        private void AddMessageToHistory(ChatMessage message, Panel historyPanel)
        {
            _displayedMessages.Add(message);

            var messagePanel = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 5, 10, 5),
                BackColor = message.Role == "user" ? Color.FromArgb(50, 50, 65) :
                           message.Role == "assistant" ? Color.FromArgb(40, 40, 55) :
                           Color.FromArgb(70, 40, 40)
            };

            var roleLabel = new Label
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Padding = new Padding(0, 0, 5, 0)
            };

            switch (message.Role)
            {
                case "user":
                    roleLabel.Text = "你:";
                    roleLabel.ForeColor = Color.LightSkyBlue;
                    break;
                case "assistant":
                    roleLabel.Text = "助手:";
                    roleLabel.ForeColor = Color.LightGreen;
                    break;
                default:
                    roleLabel.Text = "系统:";
                    roleLabel.ForeColor = Color.OrangeRed;
                    break;
            }

            var contentLabel = new Label
            {
                Text = message.Content,
                Dock = DockStyle.Fill,
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.WhiteSmoke
            };

            messagePanel.Controls.Add(contentLabel);
            messagePanel.Controls.Add(roleLabel);

            historyPanel.Controls.Add(messagePanel);
            messagePanel.BringToFront();

            historyPanel.ScrollControlIntoView(messagePanel);
        }
        #endregion

        #region  移除加载消息
        /// <summary>
        /// 移除加载消息。
        /// </summary>
        /// <param name="historyPanel"></param>
        private void RemoveLoadingMessage(Panel historyPanel)
        {
            if (historyPanel.Controls.Count > 0 &&
                historyPanel.Controls[0] is Panel panel &&
                panel.Controls[1] is Label label &&
                label.Text == "思考中...")
            {
                historyPanel.Controls.RemoveAt(0);
                _displayedMessages.RemoveAt(_displayedMessages.Count - 1);
            }
        }
        #endregion


        #region 设置控件的启用状态
        /// <summary>
        /// 设置控件的启用状态。
        /// </summary>
        /// <param name="enabled"></param>
        private void SetControlsEnabled(bool enabled)
        {
            var mainTable = (TableLayoutPanel)this.Controls[0];
            var inputPanel = (TableLayoutPanel)mainTable.Controls[1];

            foreach (Control ctrl in inputPanel.Controls)
            {
                ctrl.Enabled = enabled;
            }

            if (enabled)
            {
                inputPanel.Controls[0].Focus();
            }
        }
        #endregion


        #region
        /// <summary>
        /// 处理窗体关闭事件，释放 DeepSeekService 资源。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _deepSeekService.Dispose();
        }
        #endregion
    }
}