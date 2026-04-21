/***************************************************************************
 *
 * $Author: Turley
 * 
 * "啤酒许可证"
 * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
 * 如果我们某天相遇，并且你认为这个东西有价值，
 * 你可以请我喝杯啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;
using UoFiddler.Classes;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Plugin;

namespace UoFiddler.Forms
{
    public partial class ManagePluginsForm : Form
    {
        public ManagePluginsForm()
        {
            InitializeComponent();
            Icon = Options.GetFiddlerIcon();

            foreach (AvailablePlugin plugin in GlobalPlugins.Plugins.AvailablePlugins)
            {
                bool loaded = true;
                if (plugin.Instance == null)
                {
                    FiddlerOptions.Logger.Information("管理插件 - 创建插件实例: {Plugin} 路径: {AssemblyPath}", plugin.Type, plugin.AssemblyPath);
                    plugin.CreateInstance();
                    loaded = false;
                }
                checkedListBox1.Items.Add(plugin.Instance.Name, loaded);
            }
        }

        private void OnSelect(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            if (checkedListBox1.SelectedItem == null)
            {
                return;
            }

            AvailablePlugin selPlugin = GlobalPlugins.Plugins.AvailablePlugins.Find(checkedListBox1.SelectedItem.ToString());
            if (selPlugin == null)
            {
                return;
            }

            Font font = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size, FontStyle.Bold);
            richTextBox1.AppendText($"名称: {selPlugin.Instance.Name}\n");
            richTextBox1.Select(0, 5);
            richTextBox1.SelectionFont = font;
            richTextBox1.AppendText($"版本: {selPlugin.Instance.Version}\n");
            richTextBox1.Select(richTextBox1.Text.IndexOf("版本: ", StringComparison.Ordinal), 9);
            richTextBox1.SelectionFont = font;
            richTextBox1.AppendText($"作者: {selPlugin.Instance.Author}\n");
            richTextBox1.Select(richTextBox1.Text.IndexOf("作者: ", StringComparison.Ordinal), 8);
            richTextBox1.SelectionFont = font;
            richTextBox1.AppendText($"描述:\n{selPlugin.Instance.Description}\n");
            richTextBox1.Select(richTextBox1.Text.IndexOf("描述:", StringComparison.Ordinal), 12);
            richTextBox1.SelectionFont = font;
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            foreach (AvailablePlugin plug in GlobalPlugins.Plugins.AvailablePlugins)
            {
                if (Options.PluginsToLoad?.Contains(plug.Type.ToString()) == false)
                {
                    if (checkedListBox1.CheckedItems.Contains(plug.Instance.Name))
                    {
                        FiddlerOptions.Logger.Information("管理插件 - 向配置文件添加插件: {Plugin}", plug.Type.ToString());
                        Options.PluginsToLoad.Add(plug.Type.ToString());
                    }

                    plug.Instance = null;
                }
                else
                {
                    if (!checkedListBox1.CheckedItems.Contains(plug.Instance.Name))
                    {
                        FiddlerOptions.Logger.Information("管理插件 - 从配置文件中移除插件: {Plugin}", plug.Type.ToString());
                        Options.PluginsToLoad.Remove(plug.Type.ToString());
                    }
                }
            }
        }
    }
}