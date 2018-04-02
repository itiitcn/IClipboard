using IClipboard.Properties;
using MaterialSkin.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IClipboard.Base
{

    public enum MessageFormIcon
    {
        /// <summary>
        /// 无图标
        /// </summary>
        None = 0,
        /// <summary>
        /// 显示一张 带有 OK 字样的图片
        /// </summary>
        OK = 1,
        /// <summary>
        /// 显示一张 带有 X 字样的图片
        /// </summary>
        Error = 2,
        /// <summary>
        /// 显示一张 带有 ！ 字样的图片
        /// </summary>
        Exclamation = 3,
        /// <summary>
        /// 显示一张 带有 ？ 字样的图片
        /// </summary>
        Doubt = 4
    }
    public enum MessageFormButtons
    {
        YesNo = 0,
        Ok = 1
    }
    /* ========================================================================
    * 【本类功能概述】
    * 
    * 作者：袁建廷 时间：2016/06/06 17:12:46
    * 文件名：SkinForm
    * 版本：V1.0.0
    *
    * 修改者： 时间： 
    * 修改说明：
    * ========================================================================
    */
    public partial class DialogForm : MaterialForm
    {
        private DialogForm()
        {
            InitializeComponent();
        }

        public static DialogResult ShowMsg(string message, string title="提示", MessageFormIcon icon = MessageFormIcon.Doubt, MessageFormButtons button=MessageFormButtons.YesNo) 
        {
            DialogForm dialg = new DialogForm();
            dialg.Text = title;
            dialg.SetIcon(icon);
            dialg.SetText(message);
            dialg.SetButton(button);
            return dialg.ShowDialog();
        }


        public void SetIcon(MessageFormIcon icon) 
        {
            switch (icon)
            {
                case MessageFormIcon.None:
                    pIcon.Visible = false;
                    break;
                case MessageFormIcon.OK:
                    pIcon.Image = Resources.ok; 
                    break;
                case MessageFormIcon.Doubt:
                    pIcon.Image = Resources.Doubt;
                    break;

                case MessageFormIcon.Error:
                    pIcon.Image = Resources.Error;
                    break;
                case MessageFormIcon.Exclamation:
                    pIcon.Image = Resources.Exclamation;
                    break;
                default:
                    break;
            }
        }

        public void SetText(string message) 
        {
            if (message.Length > 50) 
            {
                //换行
                string newMessage= BreakLongString(message,50);
                lblMessage.Text = newMessage;
            }else
                lblMessage.Text = message;
            int Width = lblMessage.Width;
            int Height=lblMessage.Height;
            if (Width > (this.Width - lblMessage.Location.X-15)) 
            {
                this.Width = lblMessage.Location.X + Width + 30;
            }
            if (Height > this.Height - lblMessage.Location.Y) 
            {
                this.Height = this.Height + Height;
            }


        }


        public void SetButton(MessageFormButtons button) 
        {
            switch (button)
            {
                case MessageFormButtons.YesNo:
                     this.btnOk.Visible = true;
                     this.btnNo.Visible = true;
                    break;
                case MessageFormButtons.Ok:
                    this.btnOk.Visible = true;
                    this.btnNo.Visible = false;
                    break;
                default:
                    break;
            }
        }

        public static string BreakLongString(string SubjectString, int lineLength)
        {
            StringBuilder sb = new StringBuilder(SubjectString);
            int offset = 0;
            ArrayList indexList = buildInsertIndexList(SubjectString, lineLength);
            for (int i = 0; i < indexList.Count; i++)
            {
                sb.Insert((int)indexList[i] + offset, '\n');
                offset++;
            }
            return sb.ToString();
        }

        public static bool IsChinese(char c)
        {
            return (int)c >= 0x4E00 && (int)c <= 0x9FA5;
        }

        private static ArrayList buildInsertIndexList(string str, int maxLen)
        {
            int nowLen = 0;
            ArrayList list = new ArrayList();
            for (int i = 1; i < str.Length; i++)
            {
                if (IsChinese(str[i]))
                {
                    nowLen += 2;
                }
                else
                {
                    nowLen++;
                }
                if (nowLen > maxLen)
                {
                    nowLen = 0;
                    list.Add(i);
                }
            }
            return list;
        }  

    }

}
