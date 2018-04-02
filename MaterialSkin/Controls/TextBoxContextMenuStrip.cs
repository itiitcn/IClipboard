/***
    * ===========================================================
	* 上海沃中宝软件技术有限公司
    * 创建人：强成
    * 创建时间：2018/01/15 14:11:02
    * 说明：
    * ==========================================================
    * */
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Controls
{
    public class TextBoxContextMenuStrip : MaterialContextMenuStrip
    {
        public readonly ToolStripItem Undo = new MaterialToolStripMenuItem
        {
            Text = "撤销"
        };
        public readonly ToolStripItem Redo = new MaterialToolStripMenuItem
        {
            Text = "重做"
        };
        public readonly ToolStripItem Seperator1 = new ToolStripSeparator();

        public readonly ToolStripItem Cut = new MaterialToolStripMenuItem
        {
            Text = "剪切"
        };

        public readonly ToolStripItem Copy = new MaterialToolStripMenuItem
        {
            Text = "复制"
        };

        public readonly ToolStripItem Paste = new MaterialToolStripMenuItem
        {
            Text = "粘贴"
        };

        public readonly ToolStripItem Delete = new MaterialToolStripMenuItem
        {
            Text = "删除"
        };

        public readonly ToolStripItem Seperator2 = new ToolStripSeparator();

        public readonly ToolStripItem SelectAll = new MaterialToolStripMenuItem
        {
            Text = "全选"
        };
        TextBox textBox;
        public TextBoxContextMenuStrip()
        {
            this.Items.AddRange(new ToolStripItem[9]
            {
                    this.Undo,
                    this.Redo,
                    this.Seperator1,
                    this.Cut,
                    this.Copy,
                    this.Paste,
                    this.Delete,
                    this.Seperator2,
                    this.SelectAll
            });
            textBox = (TextBox)this.SourceControl;
            this.Opening += this.ContextMenuStripOnOpening;
            this.ItemClicked += TextBoxContextMenuStrip_ItemClicked;
        }
        private string _RevokeText = "";
        private void TextBoxContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TextBoxContextMenuStrip textBoxContextMenuStrip = this;
            //TextBox textBox = (TextBox)textBoxContextMenuStrip.SourceControl;
            string text = e.ClickedItem.Text;
            switch (text)
            {
                default:
                    if (text == "全选")
                    {
                        textBox.SelectAll();
                    }
                    break;
                case "撤销":
                    _RevokeText = textBox.Text;
                    textBox.Undo();
                    break;
                case "重做":
                    textBox.Text = _RevokeText;
                    textBox.SelectionStart = _RevokeText.Length;
                    _RevokeText = "";
                    break;
                case "剪切":
                    textBox.Cut();
                    break;
                case "复制":
                    textBox.Copy();
                    break;
                case "粘贴":
                    textBox.Paste();
                    break;
                case "删除":
                    textBox.SelectedText = string.Empty;
                    break;
            }
        }
        
        private void ContextMenuStripOnOpening(object sender, CancelEventArgs e)
        {
            TextBoxContextMenuStrip textBoxContextMenuStrip = sender as TextBoxContextMenuStrip;
            textBox = (TextBox)textBoxContextMenuStrip.SourceControl;
            if (textBoxContextMenuStrip != null)
            {
                textBoxContextMenuStrip.Redo.Enabled = (_RevokeText != "");
                textBoxContextMenuStrip.Undo.Enabled = textBox.CanUndo;
                textBoxContextMenuStrip.Cut.Enabled = !string.IsNullOrEmpty(textBox.SelectedText);
                textBoxContextMenuStrip.Copy.Enabled = !string.IsNullOrEmpty(textBox.SelectedText);
                textBoxContextMenuStrip.Paste.Enabled = Clipboard.ContainsText();
                textBoxContextMenuStrip.Delete.Enabled = !string.IsNullOrEmpty(textBox.SelectedText);
                textBoxContextMenuStrip.SelectAll.Enabled = !string.IsNullOrEmpty(textBox.Text);
            }
        }
    }
}