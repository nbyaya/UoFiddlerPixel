// /***************************************************************************
//  *
//  * $Author: Nikodemus
//  * 
//  * "葡萄酒许可证"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯葡萄酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    public partial class CalendarForm : Form
    {
        private ToolTip toolTip;

        public CalendarForm()
        {
            InitializeComponent();

            // 应用程序启动时更新 lbDaysTo、lbWeekendDays 和 lbWorkingDays 标签
            UpdateDaysTo();
            UpdateWeekendDays();
            UpdateWorkingDays();

            // 应用程序启动时显示当前日历周
            lbCalendarWeek.Text = "日历周: " + GetCalendarWeek(DateTime.Now).ToString();

            // 应用程序启动时显示当前日期
            lbDate.Text = "当前日期: " + DateTime.Now.ToShortDateString();

            timer1 = new Timer();
            timer1.Interval = 1000; // 设置间隔为1秒
            timer1.Tick += new EventHandler(this.timer1_Tick); // 添加事件处理程序
            timer1.Start(); // 启动计时器

            // 设置高亮日期
            monthCalendar1.BoldedDates = GetNotedDates().ToArray();

            // 创建一个新的 ToolTip 控件
            toolTip = new ToolTip();
            toolTip.OwnerDraw = true;
            toolTip.Draw += new DrawToolTipEventHandler(toolTip_Draw);
            toolTip.Popup += new PopupEventHandler(toolTip_Popup);

            // 创建一个新的计时器
            Timer timer = new Timer();
            timer.Interval = 1000; // 设置间隔为1秒
            timer.Tick += (sender, e) =>
            {
                // 获取选中的日期
                string date = monthCalendar1.SelectionStart.ToShortDateString();

                // 获取选中日期的备注
                string note = GetNoteForDate(date);

                // 设置工具提示文本
                toolTip.SetToolTip(monthCalendar1, note);

                // 停止计时器
                timer.Stop();
            };

            // 添加 MouseHover 和 MouseLeave 事件处理程序
            monthCalendar1.MouseHover += (sender, e) => timer.Start();
            monthCalendar1.MouseLeave += (sender, e) => timer.Stop();
        }

        #region 日历周
        // 计算日历周的函数
        public static int GetCalendarWeek(DateTime date)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            Calendar calendar = currentCulture.Calendar;
            int calendarWeek = calendar.GetWeekOfYear(date, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
            return calendarWeek;
        }
        #endregion

        #region monthCalendar_DateChange
        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            // 在标签中显示选中的日期
            lbDate.Text = "选中日期: " + monthCalendar1.SelectionStart.ToShortDateString();

            // 计算选中日期的日历周并在标签中显示
            int calendarWeek = GetCalendarWeek(monthCalendar1.SelectionStart);
            lbCalendarWeek.Text = "日历周: " + calendarWeek.ToString();

            // 更新 lbDaysTo 标签
            UpdateDaysTo();

            // 更新 lbWeekendDays 标签
            UpdateWeekendDays();

            // 更新 lbWorkingDays 标签
            UpdateWorkingDays();
        }
        #endregion

        #region 计时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            // 每秒更新 lbTime 标签为当前时间
            lbTime.Text = "时间: " + DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion

        #region monthCalendarForm_DateSelected
        private void monthCalendarForm_DateSelected(object sender, DateRangeEventArgs e)
        {
            // 创建一个新的窗体
            Form noteForm = new Form();
            noteForm.Text = "备注 - " + monthCalendar1.SelectionStart.ToShortDateString();

            // 始终将窗体置于最前
            noteForm.TopMost = true;

            // 创建一个新的 RichTextBox
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Dock = DockStyle.Fill;
            noteForm.Controls.Add(richTextBox);

            // 获取可执行文件的路径
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // 获取可执行文件所在的目录
            string exeDirectory = Path.GetDirectoryName(exePath);

            // 定义目录路径
            string directoryPath = Path.Combine(exeDirectory, "Data", "Calendar");
            Directory.CreateDirectory(directoryPath); // 如果目录不存在则创建

            // 定义文件路径
            string filePath = Path.Combine(directoryPath, "CalendarDateNotes.xml");

            // 定义日期
            string date = monthCalendar1.SelectionStart.ToShortDateString();

            XDocument doc;
            if (File.Exists(filePath))
            {
                doc = XDocument.Load(filePath);
            }
            else
            {
                doc = new XDocument(new XElement("notes"));
            }

            XElement note = doc.Root.Descendants("note").FirstOrDefault(n => n.Element("date").Value == date);
            if (note != null)
            {
                richTextBox.Text = note.Element("text").Value;
            }

            // 创建一个新的保存按钮
            Button saveButton = new Button();
            saveButton.Text = "保存";
            saveButton.Dock = DockStyle.Bottom;
            saveButton.Click += (sender, e) =>
            {
                // 将 RichTextBox 中的文本保存到 XML 文件
                if (note == null)
                {
                    doc.Root.Add(new XElement("note", new XElement("date", date), new XElement("text", richTextBox.Text)));
                }
                else
                {
                    note.Element("text").Value = richTextBox.Text;
                }
                try
                {
                    doc.Save(filePath); // 保存到指定的文件路径
                }
                catch (Exception ex)
                {
                    // 处理异常
                    MessageBox.Show("保存文件时出错: " + ex.Message);
                }

                // 更新 MonthCalendar 控件中的高亮日期
                monthCalendar1.BoldedDates = GetNotedDates().ToArray();

                // 关闭窗体
                noteForm.Close();
            };
            noteForm.Controls.Add(saveButton);

            // 创建一个新的删除按钮
            Button deleteButton = new Button();
            deleteButton.Text = "删除";
            deleteButton.Dock = DockStyle.Bottom;
            deleteButton.Click += (sender, e) =>
            {
                // 如果存在则从 XML 文件中删除条目
                if (note != null)
                {
                    note.Remove();
                    doc.Save(filePath); // 保存到指定的文件路径

                    // 更新 MonthCalendar 控件中的高亮日期
                    monthCalendar1.BoldedDates = GetNotedDates().ToArray();
                }

                // 关闭窗体
                noteForm.Close();
            };
            noteForm.Controls.Add(deleteButton);

            // 显示窗体
            noteForm.Show();
        }
        #endregion

        #region 高亮日期
        private List<DateTime> GetNotedDates()
        {
            List<DateTime> notedDates = new List<DateTime>();

            // 获取可执行文件的路径
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // 获取可执行文件所在的目录
            string exeDirectory = Path.GetDirectoryName(exePath);

            // 定义目录路径
            string directoryPath = Path.Combine(exeDirectory, "Data", "Calendar");

            // 定义文件路径
            string filePath = Path.Combine(directoryPath, "CalendarDateNotes.xml");

            try
            {
                if (File.Exists(filePath))
                {
                    XDocument doc = XDocument.Load(filePath);
                    foreach (XElement note in doc.Root.Descendants("note"))
                    {
                        try
                        {
                            DateTime date = DateTime.Parse(note.Element("date").Value);
                            notedDates.Add(date);
                        }
                        catch (FormatException ex)
                        {
                            // 处理异常
                            MessageBox.Show("解析日期时出错: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                MessageBox.Show("加载 XML 文件时出错: " + ex.Message);
            }

            return notedDates;
        }
        #endregion

        #region monthCalendar1_MouseHover
        private void monthCalendar1_MouseHover(object sender, EventArgs e)
        {
            // 获取选中的日期
            string date = monthCalendar1.SelectionStart.ToShortDateString();

            // 获取选中日期的备注
            string note = GetNoteForDate(date);

            // 设置工具提示文本
            toolTip.SetToolTip(monthCalendar1, note);
        }
        #endregion

        #region 工具提示
        private string GetNoteForDate(string date)
        {
            // 获取可执行文件的路径
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // 获取可执行文件所在的目录
            string exeDirectory = Path.GetDirectoryName(exePath);

            // 定义目录路径
            string directoryPath = Path.Combine(exeDirectory, "Data", "Calendar");

            // 定义文件路径
            string filePath = Path.Combine(directoryPath, "CalendarDateNotes.xml");

            XDocument doc;
            if (File.Exists(filePath))
            {
                doc = XDocument.Load(filePath);
                XElement note = doc.Root.Descendants("note").FirstOrDefault(n => n.Element("date").Value == date);
                if (note != null)
                {
                    return note.Element("text").Value;
                }
            }

            return null;
        }
        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            string toolTipText = toolTip.GetToolTip(e.AssociatedControl);
            e.ToolTipSize = TextRenderer.MeasureText(toolTipText, new Font("Arial", 16));
        }
        #endregion

        #region lbDaysTo
        private void UpdateDaysTo()
        {
            // 获取选中的日期
            DateTime selectedDate = monthCalendar1.SelectionStart;

            // 计算当前日期与选中日期的差值
            TimeSpan difference = selectedDate - DateTime.Now;

            // 更新 lbDaysTo 标签
            lbDaysTo.Text = "距离选中日期还有: " + difference.Days.ToString() + " 天";
        }
        #endregion

        #region lbWeekendDays
        private void UpdateWeekendDays()
        {
            // 获取当前日期
            DateTime currentDate = DateTime.Now;

            // 获取年底
            DateTime endOfYear = new DateTime(currentDate.Year, 12, 31);

            // 初始化周末天数计数器
            int weekendDays = 0;

            // 遍历到年底的所有天数
            for (DateTime date = currentDate; date <= endOfYear; date = date.AddDays(1))
            {
                // 检查是否为周末
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekendDays++;
                }
            }

            // 更新 lbWeekendDays 标签
            lbWeekendDays.Text = "剩余周末天数: " + weekendDays.ToString();
        }
        #endregion

        #region lbWorkingDays
        private void UpdateWorkingDays()
        {
            // 获取当前日期
            DateTime currentDate = DateTime.Now;

            // 获取年底
            DateTime endOfYear = new DateTime(currentDate.Year, 12, 31);

            // 初始化工作日天数计数器
            int workingDays = 0;

            // 遍历到年底的所有天数
            for (DateTime date = currentDate; date <= endOfYear; date = date.AddDays(1))
            {
                // 检查是否为工作日
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            // 更新 lbWorkingDays 标签
            lbWorkingDays.Text = "剩余工作日天数: " + workingDays.ToString();
        }
        #endregion
    }
}